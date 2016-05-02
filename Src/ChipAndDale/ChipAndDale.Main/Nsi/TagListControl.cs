using System;
using System.ComponentModel;
using System.Windows.Forms;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Composite.UI;
using Core.SDK.Dom;
using DevExpress.XtraGrid.Columns;

namespace ChipAndDale.Main.Nsi
{
    public partial class TagListControl : DevExpress.XtraEditors.XtraUserControl, IDialogResult
    {       
        public TagListControl()
        {
            InitializeComponent();         
        }

        public TagListControl(EntityList<TagEntity> entityList)
            : this()
        {
            _tagEntityList = entityList;
        }

        public void SetTagEntityList(EntityList<TagEntity> entityList)
        {
            _tagEntityList = entityList;
            _tagEntityList.SetBindingSource(NsiBindingSource);
            InitGridFilter();
        }
       

        bool _readOnly;
        public bool ReadOnly
        {
            get { return _readOnly; }
            set 
            {
                _readOnly = value;
                AddButtonItem.Enabled = !_readOnly;
                RemoveButtonItem.Enabled = !_readOnly;
                TagGridView.OptionsBehavior.ReadOnly = _readOnly;
            }
        }

        object IDialogResult.OkButton
        {
            get { return OkButton; }
        }

        object IDialogResult.CancelButton
        {
            get { return CancelButton; }
        }

        EntityList<TagEntity> _tagEntityList;
        public EntityList<TagEntity> TagEntityList
        {
            get { return _tagEntityList; }
        }


        #region override
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            _tagEntityList.SetBindingSource(null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Form parentForm = this.Parent as Form;
            parentForm.CancelButton = CancelButton;
            parentForm.AcceptButton = OkButton;
            TagGridControl.DataSource = NsiBindingSource;
            SetBindingSource();
        }
        #endregion override


        #region private         

        private void InitGridFilter()
        {
            foreach (GridColumn col in TagGridView.Columns)
                FieldBarListItem.Strings.Add(col.FieldName);
            
        }       

        private void NsiBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (NsiBindingSource == null) return;
            RecordCountStatusBar.Caption = "Кількість записів: " + NsiBindingSource.Count.ToString();
        }

        private void SetBindingSource()
        {
            _tagEntityList.SetBindingSource(NsiBindingSource);
            InitGridFilter();
        }

        private void AddButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NsiBindingSource.AddNew();
        }

        private void RemoveButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (NsiBindingSource.Current != null) NsiBindingSource.RemoveCurrent();
        }       

        /*
            if (!string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
                gridView1.ActiveFilterString = string.Format("[{0}] LIKE '{1}%'", comboBox1.SelectedItem.ToString(), textBox1.Text);
            else gridView1.ActiveFilterString = string.Empty;
         */

        #endregion private
    }
}
