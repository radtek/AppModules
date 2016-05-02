using System;
using System.Collections.Generic;
using ChipAndDale.Request.ViewModel;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Request;
using Core.UtilsModule;
using Core.XtraCompositeModule.Utils;
using System.IO;
using DevExpress.XtraEditors;

namespace ChipAndDale.Request.UI
{   
    internal partial class RequestListControl : DevExpress.XtraEditors.XtraUserControl
    {
        internal RequestListControl(RequestListViewModel viewModel, bool readOnly)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _readOnly = readOnly;
            if (readOnly)
            {
                MainToolBar.Visible = false;
                RequestGridView.OptionsSelection.MultiSelect = true;
            }
            _viewModel.RequestList.SetBindingSource(RequestBndingSource);

            //RestoreLayout();

            RequestGridView.Columns["State"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            RequestGridView.Columns["State"].DisplayFormat.Format = new EnumFormatter<RequestState>();

            RequestGridView.Columns["InfoSourceType"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            RequestGridView.Columns["InfoSourceType"].DisplayFormat.Format = new EnumFormatter<InfoSourceType>();
        }        


        public List<RequestEntity> SelectedRows
        {
            get
            {
                List<RequestEntity> list = new List<RequestEntity>();
                for (int i = 0; i < RequestGridView.SelectedRowsCount; i++)
                {
                    int row = (RequestGridView.GetSelectedRows()[i]);
                    RequestEntity request = RequestGridView.GetRow(row) as RequestEntity;
                    if (request != null)
                    {
                        list.Add(request);
                    }
                }
                return list;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!_isLoaded)
            {
                RestoreLayout();

                RequestListViewModelBindingSource.DataSource = _viewModel;
                repositoryItemLookUpEdit1.DataSource = FiltersBindingSource;
                this.DataBindings.Add("FilterListEditValueProperty", RequestListViewModelBindingSource, "Filter");
                _isLoaded = true;
            }
        }

        byte[] _gridLayoutBytes;
        public void SetGridLayoutData(byte[] bytes)
        {
            _gridLayoutBytes = bytes;
        }

        public byte[] GetGridLayoutData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                RequestGridControl.MainView.SaveLayoutToStream(stream);
                return stream.ToArray();
            }
        }

        public object FilterListEditValueProperty
        {
            get { return FilterListBarEditItem.EditValue; }
            set { FilterListBarEditItem.EditValue = value; }
        }

        #region Private 

        RequestListViewModel _viewModel;
        bool _readOnly;
        bool _isLoaded = false;

        private void RestoreLayout()
        {
            if (_gridLayoutBytes != null)
            {
                using (MemoryStream stream = new MemoryStream(_gridLayoutBytes))
                {
                    RequestGridControl.MainView.RestoreLayoutFromStream(stream);
                }
            }
        }

        private void RefreshButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnRefreshRequestList();
        }

        private void AddBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnAddRequest();
        }

        private void EditBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnEditRequest();
        }

        private void CloneBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnCloneRequest();
        }

        private void RemoveBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnRemoveRequest();
        }

        private void FilterEditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnEditFilter();
        }

        private void AddFilterButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnAddFilter();
        }

        private void DelFilterBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _viewModel.OnDeleteFilter();
        }

        private void FilterListRepositoryLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = sender as LookUpEdit;
            if (editor == null) return;

            RequestListFilterEntity filter = editor.EditValue as RequestListFilterEntity;

            if (filter != null) _viewModel.Filter = filter;
        }

        private void RequestGridView_DoubleClick(object sender, EventArgs e)
        {
            _viewModel.OnEditRequest();
        }

        private void FilterListBarEditItem_EditValueChanged(object sender, EventArgs e)
        {
            _viewModel.OnRefreshRequestList();
        }

        #endregion Private                                       
    }   
}
