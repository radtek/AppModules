using System;
using System.Data;
using ChipAndDale.SDK.Common;
using Core.OracleModule;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Db;
using Core.SDK.Log;

namespace ChipAndDale.Main.Db
{
    /// <summary>
    /// Комит транзакций необходимо выполнять в вызывающем коде
    /// </summary>        
    internal class CommonDbAccessor : ICommonDbAccessor
    {
        internal CommonDbAccessor()
        {
            _dbMgr = ServiceMgr.Current.GetInstance<IDbMgr>();
            _logMgr =  ServiceMgr.Current.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("Main.CommonDbAccessor");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: IDBMgr = {0}.", _dbMgr.ToStateString());
        }


        #region ICommonDbAccessor

        public void AuthorizateUser(string name)
        {
            try
            {
                _logger.Debug("AuthorizateUser.");
                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name param can not be null.");
                _logger.Debug("Params: name = {0};", name);

                OraCommand command = new OraCommand("MAIN_PKG.CHECK_USER");
                command.CommandType = CommandType.StoredProcedure;

                OraParamInt32 returnParam = new OraParamInt32("return", ParameterDirection.ReturnValue, null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamString("p_userName", ParameterDirection.Input, name));

                Execute(command);
            }
            catch (Exception ex)
            {
                throw new AuthorizateExceptions(ex);
            }
        }

        public void AuthorizateUser(string name, string password)
        {
            try
            {
                _logger.Debug("AuthorizateUser.");
                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name param can not be null.");
                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("Password param can not be null.");
                _logger.Debug("Params: name = {0};", name);

                OraCommand command = new OraCommand("MAIN_PKG.CONNECT_TO_DB");
                command.CommandType = CommandType.StoredProcedure;

                OraParamInt32 returnParam = new OraParamInt32("return", ParameterDirection.ReturnValue, null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamString("p_userName", ParameterDirection.Input, name));
                command.AddDBParam(new OraParamString("p_password", ParameterDirection.Input, password));

                Execute(command);
            }
            catch (Exception ex)
            {
                throw new AuthorizateExceptions(ex);
            }
        }

        public void SetParam(string name, string value)
        {
            _logger.Debug("SetParam.");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name param can not be null.");
            _logger.Debug("Params: name = {0};", name);               

            OraCommand command = new OraCommand("USER_DATA_PKG.SET_FILTER_VALUE");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamString("p_filterName", ParameterDirection.Input, name));
            command.AddDBParam(new OraParamString("p_filterValue", ParameterDirection.Input, value));

            Execute(command);
        }

        public void SendMessage(MessageEntity message)
        {
            _logger.Debug("SendMessage.");
            if (message == null) throw new ArgumentNullException("Message param can not be null.");
            _logger.Debug("Params: Message = {0};", message.ToInternalString());

            OraCommand command = new OraCommand("MAIN_PKG.INS_MESS");
            command.CommandType = CommandType.StoredProcedure;

            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(message.Key)));
            command.AddDBParam(new OraParamInt32("p_receiver", ParameterDirection.Input, message.Receiver.NumId));
            command.AddDBParam(new OraParamString("p_subject", ParameterDirection.Input, message.Subject));
            command.AddDBParam(new OraParamString("p_body", ParameterDirection.Input,  message.Body));
            command.AddDBParam(new OraParamInt32("p_type", ParameterDirection.Input, message.ChannelId));

            Execute(command);
        }

        #endregion ICommonDbAccessor




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
