using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Db;
using Core.SDK.Log;
using Core.SDK.Dom;
using Core.OracleModule;
using System.Data;
using ChipAndDale.SDK.Nsi;
using System.Collections.Generic;
using System;

namespace ChipAndDale.Main.Db
{
    internal class NsiDbAccessor : INsiDbAccessor
    {
        internal NsiDbAccessor()
        {
            _dbMgr = ServiceMgr.Current.GetInstance<IDbMgr>();
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("Main.CommonDbAccessor");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: IDBMgr = {0}.", _dbMgr.ToStateString());
        }


        #region Nsi Application

        public BindingCollection<AppEntity> GetApps()
        {
            _logger.Debug("GetApp.");
            BindingCollection<AppEntity> result = new BindingCollection<AppEntity>();
            OraCommand command = new OraCommand("MAIN_PKG.SEL_COMP");
            command.CommandType = CommandType.StoredProcedure;
            OraParamRefCursor refCur = new OraParamRefCursor("p_comp_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);

            Execute(command);
            if (refCur.ParamValue != null)
                foreach (DataRow userRow in refCur.ParamValue.Rows)
                {
                    result.Add(new AppEntity()
                    {
                        Id = userRow["ID"].ToString(),
                        Name = userRow["COMP_NAME"].ToString()
                    });
                }
            return result;
        }

        public void AddApp(AppEntity app)
        {
            if (app == null) throw new NullReferenceException("App param can not be null.");
            _logger.Debug("AddApp. Name = {0}", app.Name);

            OraCommand command = new OraCommand("MAIN_PKG.INS_COMP");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamString("p_name", ParameterDirection.Input, app.Name));

            Execute(command);
        }

        public void RemoveApp(AppEntity app)
        {
            if (app == null) throw new NullReferenceException("App param can not be null.");
            _logger.Debug("RemoveApp. Id = {0}", app.Id);

            OraCommand command = new OraCommand("MAIN_PKG.DEL_COMP");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(app.Id)));

            Execute(command);
        }

        public void UpdateApp(AppEntity app)
        {
            if (app == null) throw new NullReferenceException("App param can not be null.");
            _logger.Debug("UpdateApp. Id = {0}; Name = {1}", app.Id, app.Name);

            OraCommand command = new OraCommand("MAIN_PKG.UPD_COMP");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(app.Id)));
            command.AddDBParam(new OraParamString("p_newName", ParameterDirection.Input, app.Name));

            Execute(command);
        }

        public void ProccessAppChanges(IEnumerable<AppEntity> forAddList, IEnumerable<AppEntity> forUpdList, IEnumerable<AppEntity> forDelList)
        {
            if (forAddList == null) throw new NullReferenceException("Add param can not be null.");
            if (forUpdList == null) throw new NullReferenceException("Upd param can not be null.");
            if (forDelList == null) throw new NullReferenceException("Del param can not be null.");

            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                foreach (AppEntity app in forAddList)
                    AddApp(app);
                foreach (AppEntity app in forUpdList)
                    UpdateApp(app);
                foreach (AppEntity app in forDelList)
                    RemoveApp(app);

                transaction.Success = true;
            }
        }

        #endregion Nsi Application



        #region Nsi Organization

        public BindingCollection<OrgEntity> GetOrgs()
        {
            _logger.Debug("GetOrgs.");

            BindingCollection<OrgEntity> result = new BindingCollection<OrgEntity>();
            OraCommand command = new OraCommand("MAIN_PKG.SEL_ORG");
            command.CommandType = CommandType.StoredProcedure;
            OraParamRefCursor refCur = new OraParamRefCursor("p_org_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);

            Execute(command);
            if (refCur.ParamValue != null)
                foreach (DataRow userRow in refCur.ParamValue.Rows)
                {
                    result.Add(new OrgEntity()
                    {
                        Id = userRow["ID"].ToString(),
                        Name = userRow["ORG_NAME"].ToString()
                    });
                }
            return result;
        }

        public void AddOrg(OrgEntity org)
        {
            if (org == null) throw new NullReferenceException("Org param can not be null.");
            _logger.Debug("AddOrg. Name = {0}", org.Name);

            OraCommand command = new OraCommand("MAIN_PKG.INS_ORG");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamString("p_name", ParameterDirection.Input, org.Name));

            Execute(command);
        }

        public void RemoveOrg(OrgEntity org)
        {
            if (org == null) throw new NullReferenceException("Org param can not be null.");
            _logger.Debug("RemoveUser. Id = {0}", org.Id);

            OraCommand command = new OraCommand("MAIN_PKG.DEL_ORG");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(org.Id)));

            Execute(command);
        }

        public void UpdateOrg(OrgEntity org)
        {
            if (org == null) throw new NullReferenceException("Org param can not be null.");
            _logger.Debug("UpdateOrg. Id = {0}; Name = {1}", org.Id, org.Name);

            OraCommand command = new OraCommand("MAIN_PKG.UPD_ORG");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(org.Id)));
            command.AddDBParam(new OraParamString("p_newName", ParameterDirection.Input, org.Name));

            Execute(command);
        }

        public void ProccessOrgChanges(IEnumerable<OrgEntity> forAddList, IEnumerable<OrgEntity> forUpdList, IEnumerable<OrgEntity> forDelList)
        {
            if (forAddList == null) throw new NullReferenceException("Add param can not be null.");
            if (forUpdList == null) throw new NullReferenceException("Upd param can not be null.");
            if (forDelList == null) throw new NullReferenceException("Del param can not be null.");

            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                foreach (OrgEntity app in forAddList)
                    AddOrg(app);
                foreach (OrgEntity app in forUpdList)
                    UpdateOrg(app);
                foreach (OrgEntity app in forDelList)
                    RemoveOrg(app);

                transaction.Success = true;
            }
        }

        #endregion Nsi Organization



        #region Nsi Tag

        public BindingCollection<TagEntity> GetTags()
        {
            _logger.Debug("GetTags.");
            BindingCollection<TagEntity> result = new BindingCollection<TagEntity>();
            OraCommand command = new OraCommand("MAIN_PKG.SEL_TAG");
            command.CommandType = CommandType.StoredProcedure;
            OraParamRefCursor refCur = new OraParamRefCursor("p_tag_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);

            Execute(command);
            if (refCur.ParamValue != null)
                foreach (DataRow userRow in refCur.ParamValue.Rows)
                {
                    result.Add(new TagEntity()
                    {
                        Id = userRow["ID"].ToString(),
                        Name = userRow["TAG_NAME"].ToString()
                    });
                }
            return result;
        }

        public void AddTag(TagEntity tag)
        {
            if (tag == null) throw new NullReferenceException("Tag param can not be null.");
            _logger.Debug("AddTag. Name = {0}", tag.Name);

            OraCommand command = new OraCommand("MAIN_PKG.INS_TAG");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamString("p_name", ParameterDirection.Input, tag.Name));

            Execute(command);
        }

        public void RemoveTag(TagEntity tag)
        {
            if (tag == null) throw new NullReferenceException("Tag param can not be null.");
            _logger.Debug("RemoveTag. Id = {0}", tag.Id);

            OraCommand command = new OraCommand("MAIN_PKG.DEL_TAG");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(tag.Id)));

            Execute(command);
        }

        public void UpdateTag(TagEntity tag)
        {
            if (tag == null) throw new NullReferenceException("Tag param can not be null.");
            _logger.Debug("UpdateTag. Id = {0}; Name = {1}", tag.Id, tag.Name);

            OraCommand command = new OraCommand("MAIN_PKG.UPD_TAG");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(tag.Id)));
            command.AddDBParam(new OraParamString("p_newName", ParameterDirection.Input, tag.Name));

            Execute(command);
        }

        public void ProccessTagChanges(IEnumerable<TagEntity> forAddList, IEnumerable<TagEntity> forUpdList, IEnumerable<TagEntity> forDelList)
        {
            if (forAddList == null) throw new NullReferenceException("Add param can not be null.");
            if (forUpdList == null) throw new NullReferenceException("Upd param can not be null.");
            if (forDelList == null) throw new NullReferenceException("Del param can not be null.");

            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                foreach (TagEntity app in forAddList)
                    AddTag(app);
                foreach (TagEntity app in forUpdList)
                    UpdateTag(app);
                foreach (TagEntity app in forDelList)
                    RemoveTag(app);

                transaction.Success = true;
            }
        }

        #endregion Nsi Tag



        #region Nsi Users

        public BindingCollection<UserEntity> GetUsers()
        {
            _logger.Debug("GetUsers.");

            BindingCollection<UserEntity> result = new BindingCollection<UserEntity>();
            OraCommand command = new OraCommand("MAIN_PKG.SEL_USER");
            command.CommandType = CommandType.StoredProcedure;
            OraParamRefCursor refCur = new OraParamRefCursor("p_user_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);

            Execute(command);
            if (refCur.ParamValue != null)
                foreach (DataRow userRow in refCur.ParamValue.Rows)
                {
                    result.Add(new UserEntity()
                    {
                        Id = userRow["ID"].ToString(),
                        Login = userRow["USER_NAME"].ToString(),
                        Name = userRow["FULL_NAME"].ToString()
                    });
                }
            return result;
        }

        public void AddUser(UserEntity user)
        {
            if (user == null) throw new NullReferenceException("user param can not be null.");

            _logger.Debug("AddUser. Login = {0}; Name = {1}", user.Login, user.Name);

            OraCommand command = new OraCommand("MAIN_PKG.INS_USER");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamString("p_name", ParameterDirection.Input, user.Login));
            command.AddDBParam(new OraParamString("p_fullname", ParameterDirection.Input, user.Name));

            Execute(command);
        }

        public void RemoveUser(UserEntity user)
        {
            if (user == null) throw new NullReferenceException("user param can not be null.");
            _logger.Debug("RemoveUser. Id = {0}", user.Id);

            OraCommand command = new OraCommand("MAIN_PKG.DEL_USER");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(user.Id)));

            Execute(command);
        }

        public void UpdateUser(UserEntity user)
        {
            if (user == null) throw new NullReferenceException("user param can not be null.");
            _logger.Debug("UpdateUser. Id = {0};l Login = {1}; Name = {2}", user.Id, user.Login, user.Name);

            OraCommand command = new OraCommand("MAIN_PKG.UPD_USER");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(user.Id)));
            command.AddDBParam(new OraParamString("p_newName", ParameterDirection.Input, user.Login));
            command.AddDBParam(new OraParamString("p_newFullName", ParameterDirection.Input, user.Name));
            command.AddDBParam(new OraParamString("p_newReceiveMail", ParameterDirection.Input, null));
            command.AddDBParam(new OraParamString("p_newReceiveJabber", ParameterDirection.Input, null));

            Execute(command);
        }

        public void ProccessUserChanges(IEnumerable<UserEntity> forAddList, IEnumerable<UserEntity> forUpdList, IEnumerable<UserEntity> forDelList)
        {
            if (forAddList == null) throw new NullReferenceException("Add param can not be null.");
            if (forUpdList == null) throw new NullReferenceException("Upd param can not be null.");
            if (forDelList == null) throw new NullReferenceException("Del param can not be null.");

            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                foreach (UserEntity user in forAddList)
                    AddUser(user);
                foreach (UserEntity user in forUpdList)
                    UpdateUser(user);
                foreach (UserEntity user in forDelList)
                    RemoveUser(user);

                transaction.Success = true;
            }
        }

        #endregion Nsi Users



        #region private

        IDbMgr _dbMgr;
        ILogMgr _logMgr;
        ILogger _logger;

        void Execute(OraCommand command)
        {
            _dbMgr.Execute(command);
        }

        void ExecuteWithTransaction(OraCommand command)
        {
            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                _dbMgr.Execute(command);
                transaction.Success = true;
            }
        }

        #endregion private
    }
}
