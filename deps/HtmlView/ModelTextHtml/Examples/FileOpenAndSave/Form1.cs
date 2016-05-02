using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Assembly = System.Reflection.Assembly;

using System.IO;
using ModelText.ModelEditControl;
using ModelText.ModelDom.Range;
using ModelText.ModelDom.Nodes;
using ModelText.ModelEditToolCommands;

namespace FileOpenAndSave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            initialize();
        }

        /// <summary>
        /// This method is invoked from the constructor.
        /// It contains hand-written code to initialize the form,
        /// which is not created by the Vistual Studio Designer
        /// in the InitializeComponent() method.
        /// </summary>
        private void initialize()
        {
            //log any unhandled exceptions
            ModelText.ModelException.LogUnhandledException.enable(true);

            //get this program's persisted settings
            m_settings = new Settings();

            //instantiate a ModelText HTML control
            m_modelTextHtmlControl = new TooledControl();

            //create the control's toolbar
            m_modelTextHtmlControl.addTools();

            //add the control to the form
            //by superimposing it on the panel1 element
            //which I added to the form using the Visual Studio Designer to act as a
            //placeholder for the location of the ModelText HTML control within the form.
            m_modelTextHtmlControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(m_modelTextHtmlControl);

            //load the data after the form as finished loading
            this.Load += new EventHandler(Form1_Load);

            //install an event handler, to help process some of the buttons on the edit toolbar
            modelEdit.toolContainer.onToolCommand = onToolCommand;

            //disable menu items until a document is loaded
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            showTextToolStripMenuItem.Enabled = false;

            //instantiate (but don't show) the tooltip instance
            this.components = new System.ComponentModel.Container();
            m_toolTip = new ToolTip(this.components);

            //show initial/default CSS
            defaultbuiltinToolStripMenuItem.Checked = true;
        }

        /// <summary>
        /// This event handler is called after the form has finished loading.
        /// Note that although the form is loaded, the HTML document isn't loaded yet
        /// (when the document is loaded then the afterNewDocumentLoaded is called).
        /// </summary>
        void Form1_Load(object sender, EventArgs e)
        {
            //cssCustomToolStripMenuItem_Click(null, null);
            //cssDefaultToolStripMenuItem_Click(null, null);
            //try to reload whatever file was loaded last time
            string filename = m_settings.recentFilename;
            if (!string.IsNullOrEmpty(filename) && load(filename, true))
            {
                //success
                return;
            }

            //else try to load the readme.html
            load(filenameOfReadme, false);
        }

        /// <summary>
        /// This method is invoked when any command on the toolbar is pressed
        /// </summary>
        /// <param name="command"></param>
        void onToolCommand(Command command)
        {
            //see what type of command it is
            //the command-handler for most of the commands are implemented within the control
            //but two of the commands (i.e. Save and InsertHyperlink) need help from the client application
            if (object.ReferenceEquals(command, CommandInstance.commandInsertHyperlink))
            {
                //invoke a method to handle the Insert Hyperlink command being pressed
                onInsertHyperlink();
            }
            else if (object.ReferenceEquals(command, CommandInstance.commandSave))
            {
                //invoke the same method as the one that's invoked via the form's File/Save menu item
                saveOrSaveAs();
            }
        }

        /// <summary>
        /// This method attempts to load the contents of the specified file into the control.
        /// </summary>
        bool load(string filename, bool canEditThisFile)
        {
            if (filename == null)
                return false;
            try
            {
                using (StreamReader textReader = new StreamReader(filename))
                {
                    modelEdit.openDocument(textReader);
                }

                //successfully loaded this file
                afterNewDocumentLoaded((canEditThisFile) ? filename : null);

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        bool saveOrSaveAs()
        {
            if (m_settings.recentFilename != null)
            {
                return onSave(m_settings.recentFilename);
            }
            else
            {
                return onSaveAs();
            }
        }

        /// <summary>
        /// Gets the IModelEdit interface which is implemented by the ModelText HTML Control.
        /// </summary>
        IModelEdit modelEdit
        {
            get { return m_modelTextHtmlControl.modelEdit; }
        }

        #region Event handlers for the menu items

        /// <summary>
        /// This is the event handler for the File/New menu item.
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onNew();
        }

        /// <summary>
        /// This is the event handler for the File/Open menu item.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onOpen();
        }

        /// <summary>
        /// This is the event handler for the File/Save menu item.
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onSave(m_settings.recentFilename);
        }

        /// <summary>
        /// This is the event handler for the File/Save As menu item.
        /// </summary>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onSaveAs();
        }

        /// <summary>
        /// This is the event handler for the File/Show Text menu item.
        /// </summary>
        private void showTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onShowText();
        }

        private void cssNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCss("", noneresetToolStripMenuItem);
        }

        private void cssDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCss(null, defaultbuiltinToolStripMenuItem);
        }

        private void cssCustomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //read the custom CSS from embedded resource
            string css;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FileOpenAndSave.custom.css"))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    css = streamReader.ReadToEnd();
                }
            }
            setCss(css, customuserdefinedToolStripMenuItem);
        }

        private void mergeParagraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParagraphsToMerge paragraphsToMerge = ParagraphsToMerge.create(modelEdit.windowSelection);
            if (paragraphsToMerge == null)
                return;
            paragraphsToMerge.run(modelEdit);
        }

        #endregion


        void setCss(string css, ToolStripMenuItem menuItem)
        {
            if (menuItem.Checked)
            {
                //this CSS is already selected
                return;
            }

            //see whether the current document needs saving
            CommandState saveCommandState = m_modelTextHtmlControl.modelEdit.toolContainer.getCommandState(CommandInstance.commandSave);
            if (saveCommandState.enabled)
            {
                //the save button is enabled which means that there are unsaved changes
                switch (MessageBox.Show(
                    "Changing the CSS requires reloading the current document. The current document has unsaved changes. Choose 'Yes' to save the changes, or 'No' to discard them.",
                    "Save changes?",
                     MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        //don't change the CSS
                        return;
                    case DialogResult.No:
                        //don't save the changes
                        break;
                    case DialogResult.Yes:
                        //save the changes
                        if (!saveOrSaveAs())
                        {
                            //failed to save
                            //so return without changing the CSS
                            return;
                        };
                        break;
                }
            }

            //change the CSS
            m_modelTextHtmlControl.modelEdit.css = css;

            //reload the current file
            if (m_settings.recentFilename != null)
            {
                load(m_settings.recentFilename, true);
            }
            else
            {
                //else try to load the readme.html
                load(filenameOfReadme, false);
            }

            //change the checkbox
            noneresetToolStripMenuItem.Checked = object.ReferenceEquals(noneresetToolStripMenuItem, menuItem);
            defaultbuiltinToolStripMenuItem.Checked = object.ReferenceEquals(defaultbuiltinToolStripMenuItem, menuItem);
            customuserdefinedToolStripMenuItem.Checked = object.ReferenceEquals(customuserdefinedToolStripMenuItem, menuItem);
        }

        void onNew()
        {
            modelEdit.newDocument();
            //successfully loaded this file
            afterNewDocumentLoaded(null);
        }

        void onOpen()
        {
            //repeat indefinitely until success or until the user quits
            for (; ; )
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "XHTML Files|*.html";
                openFileDialog1.Title = "Select a XHTML File";
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    //user cancelled
                    return;
                }

                //get the filename from the dialog box
                string filename = openFileDialog1.FileName;
                //try to load the file into the control
                if (load(filename, true))
                    return;

                //failed to load the specified file
                DialogResult dialogResult = MessageBox.Show(
                    "Failed to load the specified file; maybe it isn't HTML. Try again with another file?",
                    "Cannot Open This File",
                    MessageBoxButtons.OKCancel);

                if (dialogResult != DialogResult.OK)
                    return;

                //else loop to try again
            }
        }

        bool onSave(string filename)
        {
            try
            {
                string tempFilename = Path.GetTempFileName();
                using (StreamWriter textWriter = new StreamWriter(tempFilename))
                {
                    modelEdit.save(textWriter, XmlHeaderType.Xhtml);
                }
                File.Copy(tempFilename, filename, true);
                setCurrentFilename(filename);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
        }

        bool onSaveAs()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XHTML Files|*.html";
            saveFileDialog1.Title = "Save XHTML File";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                //user cancelled
                return false;
            }

            //get the filename from the dialog box
            string filename = saveFileDialog1.FileName;
            //try to save the file from the control
            return onSave(filename);
        }

        void onInsertHyperlink()
        {
            //get the document fragment which the user has currently selected using mouse and/or cursor
            IWindowSelection windowSelection = modelEdit.windowSelection;
            //verify that there's only one selection
            if (windowSelection.rangeCount != 1)
            {
                //this can happen when the user has selected several cell in a table,
                //in which case each cell is a separate selection/range
                MessageBox.Show("Can't insert a hyperlink when more than one range in the document is selected");
                return;
            }
            using (IDomRange domRange = windowSelection.getRangeAt(0))
            {
                //verify that only one node is selected
                if (!domRange.startContainer.isSameNode(domRange.endContainer))
                {
                    //this can happen for example when the selection spans multiple paragraphs
                    MessageBox.Show("Can't insert a hyperlink when more than one node in the document is selected");
                    return;
                }
                IDomNode container = domRange.startContainer; //already just checked that this is the same as domRange.endContainer
                //read existing values from the current selection
                string url;
                string visibleText;
                IDomElement existingHyperlink;
                switch (container.nodeType)
                {
                    case DomNodeType.Text:
                        //selection is a text fragment
                        visibleText = container.nodeValue.Substring(domRange.startOffset, domRange.endOffset - domRange.startOffset);
                        IDomNode parentNode = container.parentNode;
                        if ((parentNode.nodeType == DomNodeType.Element) && (parentNode.nodeName == "a"))
                        {
                            //parent of this text node is a <a> element
                            existingHyperlink = parentNode as IDomElement;
                            url = existingHyperlink.getAttribute("href");
                            visibleText = container.nodeValue;
                            if ((existingHyperlink.childNodes.count != 1) || (existingHyperlink.childNodes.itemAt(0).nodeType != DomNodeType.Text))
                            {
                                //this can happen when an anchor tag wraps more than just a single, simple text node
                                //for example when it contains inline elements like <strong>
                                MessageBox.Show("Can't edit a complex hyperlink");
                                return;
                            }
                        }
                        else
                        {
                            existingHyperlink = null;
                            url = null;
                        }
                        break;

                    default:
                        //unexpected
                        MessageBox.Show("Can't insert a hyperlink when more than one node in the document is selected");
                        return;
                }
                //display the modal dialog box
                using (FormInsertHyperlink formInsertHyperlink = new FormInsertHyperlink())
                {
                    formInsertHyperlink.url = url;
                    formInsertHyperlink.visibleText = visibleText;
                    DialogResult dialogResult = formInsertHyperlink.ShowDialog();
                    if (dialogResult != DialogResult.OK)
                    {
                        //user cancelled
                        return;
                    }
                    //get new values from the dialog box
                    //the FormInsertHyperlink.onEditTextChanged method assures that both strings are non-empty
                    url = formInsertHyperlink.url;
                    visibleText = formInsertHyperlink.visibleText;
                }
                //need to change href, change text, and possibly delete existing text;
                //do this within the scope of a single IEditorTransaction instance so
                //that if the user does 'undo' then it will undo all these operations at once, instead of one at a time
                using (IEditorTransaction editorTransaction = modelEdit.createEditorTransaction())
                {
                    if (existingHyperlink != null)
                    {
                        //changing an existing hyperlink ...
                        //... change the href attribute value
                        existingHyperlink.setAttribute("href", url);
                        //... change the text, by removing the old text node and inserting a new text node
                        IDomText newDomText = modelEdit.domDocument.createTextNode(visibleText);
                        IDomNode oldDomText = existingHyperlink.childNodes.itemAt(0);
                        existingHyperlink.removeChild(oldDomText);
                        existingHyperlink.insertBefore(newDomText, null);
                    }
                    else
                    {
                        //creating a new hyperlink
                        IDomElement newHyperlink = modelEdit.domDocument.createElement("a");
                        IDomText newDomText = modelEdit.domDocument.createTextNode(visibleText);
                        newHyperlink.insertBefore(newDomText, null);
                        newHyperlink.setAttribute("href", url);
                        //remove whatever was previously selected, if anything
                        if (!domRange.collapsed)
                        {
                            domRange.deleteContents();
                        }
                        //insert the new hyperlink
                        domRange.insertNode(newHyperlink);
                    }
                }
            }
        }

        void onShowText()
        {
            //get the document content, using the same algorithm as the save(string filename) method above
            string document;
            using (StringWriter textWriter = new StringWriter())
            {
                modelEdit.save(textWriter, XmlHeaderType.Xhtml);
                document = textWriter.ToString();
            }
            //create the form
            using (FormShowText formShowText = new FormShowText())
            {
                //write the document value into the form
                formShowText.text = document;
                //show the form as a modal dialog
                formShowText.ShowDialog();
            }
        }

        /// <summary>
        /// This method is called from the onNew and load methods.
        /// </summary>
        void afterNewDocumentLoaded(string filename)
        {
            //enable menu items
            saveAsToolStripMenuItem.Enabled = true;
            showTextToolStripMenuItem.Enabled = true;
            //remember the current filename
            setCurrentFilename(filename);
            //hide any tooltip possibly left over fom a previous document
            hyperlinkHref = null;

            //install event handlers to display anchors' href values in a tooltip on hover
            ModelText.ModelDom.Events.DomEventListeners.onAnchor(
                modelEdit.domDocument,
                ModelText.ModelDom.Events.DomEventType.Mousemove,
                delegate(string hrefValue) { this.hyperlinkHref = hrefValue; });
            ModelText.ModelDom.Events.DomEventListeners.onAnchor(
                modelEdit.domDocument,
                ModelText.ModelDom.Events.DomEventType.Mouseout,
                delegate(string hrefValue) { this.hyperlinkHref = null; });

            //install an event handler, which can do something when the user clicks on an anchor
            modelEdit.domDocument.addDefaultEventListener(ModelText.ModelDom.Events.DomEventType.Click, onClick);
        }

        void onClick(ModelText.ModelDom.Events.IDomEvent domEvent)
        {
            //see whether the clicked node is an anchor element
            //(or an ancestor of the clicked node, because the clicked node
            //is actually the text node which is a child of the anchor node)
            for (IDomNode target = domEvent.target; target != null; target = target.parentNode)
            {
                if ((target.nodeType == DomNodeType.Element) && (target.nodeName == "a"))
                {
                    //invoke the method which would be invoked if the user
                    //pressed the "Hyperlink" button on the toolbar
                    onInsertHyperlink();
                    return;
                }
            }
        }

        void setCurrentFilename(string filename)
        {
            saveToolStripMenuItem.Enabled = (filename != null);
            //reopen this file if the program is next restarted
            m_settings.recentFilename = filename;
        }

        string hyperlinkHref
        {
            set
            {
                if (value == null)
                {
                    //disable the tooltip
                    m_toolTip.Hide(m_modelTextHtmlControl);
                    m_toolTipText = null;
                }
                else
                {
                    //compare with cached m_toolTipText to avoid flickering
                    if (value == m_toolTipText)
                    {
                        //this tooltip value is already displayed
                        return;
                    }
                    m_toolTipText = value;
                    //show the tooltip, wherever the mouse currently is
                    Point point = Cursor.Position; //screen coordinates
                    point = m_modelTextHtmlControl.PointToClient(point); //client coordinates
                    point.Offset(0, -50); //higher, to avoid obscuring the text
                    m_toolTip.Show(value, m_modelTextHtmlControl, point);
                }
            }
        }

        #region instance data that's not created by the designer

        Settings m_settings;
        TooledControl m_modelTextHtmlControl;
        const string filenameOfReadme = "../../../readme.html";
        ToolTip m_toolTip;
        string m_toolTipText;

        #endregion
    }
}
