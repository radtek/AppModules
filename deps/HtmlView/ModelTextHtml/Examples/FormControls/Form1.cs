using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using System.Windows.Forms;

using System.IO;
using System.Reflection;

using ModelText.ModelEditControl;

namespace FormControls
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
        void initialize()
        {
            //log any unhandled exceptions
            ModelText.ModelException.LogUnhandledException.enable(true);

            //load the data after the form as finished loading
            this.Load += new EventHandler(Form1_Load);
        }

        /// <summary>
        /// This event handler is called after the form has finished loading.
        /// Note that although the form is loaded, the HTML document isn't loaded yet
        /// (when the document is loaded then the afterNewDocumentLoaded is called).
        /// </summary>
        void Form1_Load(object sender, EventArgs e)
        {
            //load the simple form
            loadSimpleForm();
        }

        void loadSimpleForm()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FormControls.SimpleForm.html"))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    load(streamReader);
                }
            }
        }

        void load(TextReader textReader)
        {
            modelEdit.openDocument(textReader);
            ModelText.ModelDom.Events.DomEventListeners.onFormSubmitted(modelEdit.domDocument, onFormSubmitted);
        }

        IModelEdit modelEdit
        {
            get { return scrollableControl1.modelEdit; }
        }

        /// <summary>
        /// This method copied from the help associated with the
        /// IModelEdit.formDataset property.
        /// </summary>
        void onFormSubmitted(string formAction)
        {
            //get the "form data set" from the form
            string formDataset = modelEdit.formDataset;
            //call some other method to submit the "form data set" together with the form action
            MessageBox.Show("Contents of the form are as follows:\r\n\r\n" + formDataset, "On submit");
        }

        private void simpleFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadSimpleForm();
        }
    }
}
