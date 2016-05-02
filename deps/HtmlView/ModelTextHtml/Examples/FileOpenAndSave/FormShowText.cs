using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileOpenAndSave
{
    public partial class FormShowText : Form
    {
        public FormShowText()
        {
            InitializeComponent();
        }

        internal string text
        {
            set
            {
                this.textBox.Text = value;
            }
        }
    }
}
