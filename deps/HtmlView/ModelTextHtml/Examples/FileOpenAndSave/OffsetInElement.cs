using System;
using System.Collections.Generic;
using System.Text;

using ModelText.ModelDom.Nodes;

namespace FileOpenAndSave
{
    /// <summary>
    /// This class can be used to store and restore the selected range
    /// when the DOM is edited
    /// </summary>
    class OffsetInElement
    {
        IDomNode m_ancestor;
        int m_offset;
        bool m_endOfNode;

        enum When
        {
            BeforeNode,
            AfterNode
        }

        internal delegate void SetOffset(IDomNode parent, int offset);

        internal OffsetInElement(IDomNode ancestor, IDomNode parent, int offset)
        {
            m_ancestor = ancestor;
            m_offset = 0;

            When when;
            IDomNode lastNode;
            switch (parent.nodeType)
            {
                case DomNodeType.Text:
                    //remember whether we're at the very end of this text node
                    m_endOfNode = (offset == parent.nodeValue.Length);

                    //add offset within the parent text node
                    m_offset = offset;
                    //now stop when (before) we've processed parent
                    lastNode = parent;
                    when = When.BeforeNode;
                    break;
                case DomNodeType.Element:
                    //remember whether we're at the very end of this element
                    m_endOfNode = (offset == parent.childNodes.count);

                    //want to stop before we've processed child of parent
                    if (offset == 0)
                    {
                        //want to stop before we've processed parent
                        when = When.BeforeNode;
                        lastNode = parent;
                    }
                    else if (offset == parent.childNodes.count)
                    {
                        //want to stop after we've processed last descendent of parent
                        when = When.AfterNode;
                        //so find the last descendent
                        lastNode = parent;
                        while ((lastNode.nodeType == DomNodeType.Element) && (lastNode.childNodes.count > 0))
                        {
                            lastNode = lastNode.childNodes.itemAt(lastNode.childNodes.count - 1);
                        }
                    }
                    else
                    {
                        //want to stop before we've processed child
                        when = When.BeforeNode;
                        lastNode = parent.childNodes.itemAt(offset);
                    }
                    break;
                default:
                    throw new ApplicationException("unexpected");
            }
            foreach (IDomNode descendent in getChildNodes(ancestor))
            {
                if ((when == When.BeforeNode) && ReferenceEquals(lastNode, descendent))
                {
                    return;
                }

                if (descendent.nodeType == DomNodeType.Text)
                {
                    m_offset += descendent.nodeValue.Length;
                }

                if ((when == When.AfterNode) && ReferenceEquals(lastNode, descendent))
                {
                    return;
                }
            }
            //didn't find the lastNode that we were looking for
            throw new ApplicationException("unexpected");
        }

        internal void increment()
        {
            ++m_offset;
        }

        internal void restore(SetOffset setOffset)
        {
            int offset = m_offset;
            foreach (IDomNode descendent in getChildNodes(m_ancestor))
            {
                if (descendent.nodeType == DomNodeType.Text)
                {
                    int textNodeLength = descendent.nodeValue.Length;
                    if (offset > textNodeLength)
                    {
                        offset -= textNodeLength;
                        continue;
                    }
                    else if ((offset == textNodeLength) && (offset != 0))
                    {
                        //are we positioned at the end of this node, or at the start of the next one?
                        if (m_endOfNode)
                        {
                            setOffset(descendent, offset);
                            return;
                        }
                        else
                        {
                            offset = 0;
                            continue;
                        }
                    }
                    else
                    {
                        setOffset(descendent, offset);
                        return;
                    }
                }
            }
        }

        static IEnumerable<IDomNode> getChildNodes(IDomNode ancestor)
        {
            foreach (IDomNode child in ancestor.childNodes.elements)
            {
                yield return child;
                //recurse
                if (child.childNodes != null)
                {
                    foreach (IDomNode descendent in getChildNodes(child))
                        yield return descendent;
                }
            }
        }
    }
}
