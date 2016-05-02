using System;
using System.Collections.Generic;
using System.Text;

using ModelText.ModelDom.Range;
using ModelText.ModelDom.Nodes;
using ModelText.ModelEditControl;

namespace FileOpenAndSave
{
    class ParagraphsToMerge
    {
        class Location
        {
            readonly IDomNode m_parent;
            IDomNode m_previousNode;
            internal Location(IDomNode parent, IDomNode previousNode)
            {
                m_parent = parent;
                m_previousNode = previousNode;
            }
            internal void insert(IDomNode newChild)
            {
                if (m_previousNode != null)
                {
                    //insert after previous node
                    int offset = m_parent.childNodes.indexOf(m_previousNode);
                    IDomNode refChild = (offset < (m_parent.childNodes.count - 1)) ? m_parent.childNodes.itemAt(offset + 1) : null;
                    m_parent.insertBefore(newChild, refChild);
                    //next time insert after the just-inserted node
                    m_previousNode = newChild;
                }
                else
                {
                    m_parent.insertBefore(newChild, null);
                }
            }
        }

        readonly Location m_previous;
        readonly List<List<IDomNode>> m_selected;
        readonly OffsetInElement m_start;
        readonly OffsetInElement m_end;

        internal static ParagraphsToMerge create(IWindowSelection windowSelection)
        {
            //multiple ranges when several (separate) cells are selected
            //could support this scenario later but it's uncessarily
            //complicated for a demo so for now simply disable the
            //'merge paragraphs' feature when the selection includes table cells

            if (windowSelection.rangeCount != 1)
                return null;

            try
            {
                //get the selected range
                using (IDomRange domRange = windowSelection.getRangeAt(0))
                {
                    return create(domRange);
                }
            }
            catch
            {
                return null;
            }
        }

        static ParagraphsToMerge create(IDomRange domRange)
        {
            //find the block-level element associated with the start of the range
            IDomNode startBlock = findBlock(domRange.startContainer, domRange.startOffset, true);
            //find the block-level element associated with the end of the range
            IDomNode endBlock = findBlock(domRange.endContainer, domRange.endOffset, false);

            //block may be null if it's positioned directly under a <td>
            //which (removing things from a table) isn't a scenario supported by this class
            if ((startBlock == null) || (endBlock == null))
                return null;

            //iterate the inline-level elements to be moved
            List<List<IDomNode>> selected = new List<List<IDomNode>>();

            IDomNode currentBlock = startBlock;
            for (; ; )
            {
                //get the inline elements inside this block
                List<IDomNode> children = null;
                bool childIsBlock = false;
                foreach (IDomNode child in currentBlock.childNodes.elements)
                {
                    if (!isBlock(child))
                    {
                        //found an inline child
                        if (children == null)
                        {
                            children = new List<IDomNode>();
                        }
                        children.Add(child);
                    }
                    else
                    {
                        //child of this block is itself a block
                        currentBlock = child;
                        childIsBlock = true;
                        break;
                    }
                }
                if (children != null)
                {
                    //found some inline children
                    selected.Add(children);
                }
                if (childIsBlock)
                {
                    //descend: iterate this child block
                    continue;
                }
                //else finished iterating children of currentBlock

                if (ReferenceEquals(currentBlock, endBlock))
                {
                    //currentBlock is the last block we wanted to iterate
                    break;
                }

                //find the next block to iterate
                for (; ; )
                {
                    IDomNode currentParent = currentBlock.parentNode;
                    int currentIndex = currentParent.childNodes.indexOf(currentBlock);
                    if (currentIndex < (currentParent.childNodes.count - 1))
                    {
                        //next we'll iterate the next sibling after currentBlock
                        currentBlock = currentParent.childNodes.itemAt(currentIndex + 1);
                        break;
                    }
                    else
                    {
                        //no next sibling after currentBlock
                        //so iterate next sibling after parent block
                        currentBlock = currentParent;
                        continue;
                    }
                }

                if (!isBlock(currentBlock))
                {
                    //this can happen if an 'anonymous block' is not the first block of a parent block
                    //which isn't supported by the current version of the renderer
                    throw new ApplicationException("unexpected");
                }
            }

            //find the block before the selected block
            //into which we'll merge the inline elements
            currentBlock = startBlock;
            Location previous;
            for (; ; )
            {
                //find the block before the first block
                IDomNode parent = currentBlock.parentNode;
                int index = parent.childNodes.indexOf(currentBlock);
                if (index > 0)
                {
                    //found a previous element
                    IDomNode previousNode = parent.childNodes.itemAt(index - 1);
                    if (isBlock(previousNode))
                    {
                        //previous element is a block
                        //but it might have block-level descendents
                        while ((previousNode.childNodes.count > 0) && isBlock(previousNode.childNodes.itemAt(previousNode.childNodes.count - 1)))
                        {
                            previousNode = previousNode.childNodes.itemAt(previousNode.childNodes.count - 1);
                        }
                        //want to insert at the end (after the last child) of the previousNode
                        previous = new Location(previousNode, null);
                        break;
                    }
                    else
                    {
                        //previous is not a block,
                        //so it's an anonymous block
                        //near the start of a list item or table cell
                        previous = new Location(parent, previousNode);
                        break;
                    }
                }
                if (parent.nodeName == "body")
                {
                    //startBlock is the first block in the body
                    //there is no previous block
                    //so don't merge
                    return null;
                }
                currentBlock = parent;
                continue;
            }

            //search up to find the <body> element
            IDomNode body = startBlock;
            while (body.nodeName != "body")
            {
                body = body.parentNode;
            }

            //remember whether the selection range starts and ends
            //in order to restore it after we edit the DOM
            OffsetInElement start = new OffsetInElement(body, domRange.startContainer, domRange.startOffset);
            OffsetInElement end = new OffsetInElement(body, domRange.endContainer, domRange.endOffset);

            return new ParagraphsToMerge(previous, selected, start, end);
        }

        /// <summary>
        /// Find the block-level element associated with the start or end of the range
        /// </summary>
        static IDomNode findBlock(IDomNode container, int offset, bool findStart)
        {
            IDomNode foundBlock;
            if (!isBlock(container))
            {
                //range starts or ends in/on an inline element so find the block-level ancestor
                for (foundBlock = container.parentNode;  !isBlock(foundBlock); foundBlock = foundBlock.parentNode)
                {
                    if (foundBlock.nodeName == "body")
                    {
                        //
                        return null;
                    }
                }
            }
            else
            {
                //range starts or ends in/on a block-level element so find any block-level descendent
                for (
                    foundBlock = container;
                    //if the child of this block is a block ...
                    (offset < foundBlock.childNodes.count) && isBlock(foundBlock.childNodes.itemAt(offset));
                    //... then descend to either the first or the last the child block
                    foundBlock = foundBlock.childNodes.itemAt(offset), offset = (findStart) ? 0 : Math.Max(0, foundBlock.childNodes.count - 1)
                    )
                    ;
            }
            return foundBlock;
        }

        /// <summary>
        /// Return true if this is a block-level element, or false if it's an inline-level element or a text node.
        /// </summary>
        static bool isBlock(IDomNode domNode)
        {
            //A more sophisticated implementation of this method would use the
            //the ModelText.ModelCssInterfaces.View.ICssViewCss.getComputedStyle method
            //to determine the CSS styling for each element at run-time.
            //Instead, this element assumes that there isn't user-defined CSS which
            //for example renders <p> elements as inline instead of as block.
            switch (domNode.nodeType)
            {
                case DomNodeType.Text:
                    //it's a text node
                    return false;
                //it's an element node
                case DomNodeType.Element:
                    break;
                default:
                    throw new ApplicationException("unexpected");
            }

            IDomElement domElement = (IDomElement)domNode;
            switch (domElement.tagName)
            {
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "div":
                case "p":
                case "li":
                case "ol":
                case "ul":

                    return true;

                default:

                    return false;
            }
        }

        ParagraphsToMerge(Location previous, List<List<IDomNode>> selected, OffsetInElement start, OffsetInElement end)
        {
            m_previous = previous;
            m_selected = selected;
            m_start = start;
            m_end = end;
        }

        internal void run(IModelEdit modelEdit)
        {
            //we're about to make several changes to the DOM
            //wrap them within one IEditorTransaction so that
            //they can undo as a single operation
            using (IEditorTransaction editorTransaction = modelEdit.createEditorTransaction())
            {
                //change the DOM
                editDom(modelEdit.domDocument);
                //restore the selected range into the newly-located DOM nodes
                //(the renderer removed the selected range from these nodes automatically
                //when the editDom method temporarily removed the nodes from the document)
                IWindowSelection windowSelection = modelEdit.windowSelection;
                m_start.restore(windowSelection.collapse);
                m_end.restore(windowSelection.extend);
            }
        }

        void editDom(IDomDocument domDocument)
        {
            //increment the serialized range location start only once
            //to account for extra whitespace we add before the first block
            m_start.increment();

            foreach (List<IDomNode> elements in m_selected)
            {
                //insert a whitespace, instead of the 'carriage return' which used to separate the blocks
                IDomText whitespace = domDocument.createTextNode(" ");
                m_previous.insert(whitespace);

                //increment the serialized range location end multiple times
                //to account for extra whitespace we just added
                m_end.increment();

                IDomNode parent = elements[0].parentNode;

                //insert each element
                foreach (IDomNode element in elements)
                {
                    //remove from current parent before inerting into new parent
                    parent.removeChild(element);
                    //insert into new parent
                    m_previous.insert(element);
                }

                if (parent.childNodes.count == 0)
                {
                    //this block is now empty (all its children removed)
                    //so remove it from the document
                    IDomNode grandparent = parent.parentNode;
                    while (grandparent.childNodes.count == 1)
                    {
                        parent = grandparent;
                        grandparent = grandparent.parentNode;
                    }
                    grandparent.removeChild(parent);
                }
            }
        }
    }
}
