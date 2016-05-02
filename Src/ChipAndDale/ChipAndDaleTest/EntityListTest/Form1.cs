using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using Core.SDK.Dom;

namespace EntityListTest
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            _entityList = new EntityList<Person>();
            _entityList.Entities.Add(new Person("1", "12","123"));
            _entityList.Entities.Add(new Person("2", "22","123"));
            _entityList.Entities.Add(new Person("3", "32", "123"));
        }
        EntityList<Person> _entityList; 

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _entityList.SetBindingSource(personBindingSource);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            _entityList.Current = _entityList.Entities[1];
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            _entityList.Entities.Add(new Person("4", "444", ""));
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            personBindingSource.Add(new Person("5", "555", "1"));
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            personBindingSource.RemoveAt(1);
        }        

    }
}