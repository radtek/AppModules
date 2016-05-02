using System;
using Core.SDK.Common;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Event.EventMessage;
using Core.SDK.Composite.Service;
using Core.SDK.Db;
using Core.SDK.Log;
using Oracle.DataAccess.Client;

namespace Core.OracleModule
{
    public class OraDBMgr: IDbMgr
    {
        public OraDBMgr(IDbConnection connection, ILogMgr logMgr)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (logMgr == null) throw new ArgumentNullException("logMgr");

            _dbConnection = connection;
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("OraDBMgr");
            _logger.Info("Create.");

            InitAdditionalService();
        }             

        public void Execute(IDbCommand command)
        {
            LogCommand(command);
            try
            {
                CheckConnection();
                ExecuteInternal(command);
                FireComandExecutedEvent(command.CommandText, true);                
            }
            catch (OracleException oraEx)
            {
                WrapOraException(command, oraEx);
            }
            catch (Exception ex)
            {
                throw new UnexpectedDbException(command.CommandText, ex);
            }
        }        

        public System.Data.DataTable ExecuteSelect(IDbCommand command)
        {
            LogCommand(command);
            try
            {
                CheckConnection();
                System.Data.DataTable result = ExecuteSelectInternal(command);
                FireComandExecutedEvent(command.CommandText, true);
                return result;
            }
            catch (OracleException oraEx)
            {
                WrapOraException(command, oraEx);
                return null;
            }
            catch (Exception ex)
            {
                throw new UnexpectedDbException(command.CommandText, ex);
            }
        }        

        public void StartTransaction()
        {
            lock (_dbConnection)
            {
                if (_transaction != null)
                    throw new TransactionDbException("Transaction already created.");

                _transaction = (OracleTransaction)_dbConnection.Connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            lock (_dbConnection)
            {
                _transaction.Commit();
                _transaction = null;
                FireTransactionFinishEvent(TransactionResult.Commit);
            }
        }

        public void RollbackTransaction()
        {
            lock (_dbConnection)
            {
                _transaction.Rollback();
                _transaction = null;
                FireTransactionFinishEvent(TransactionResult.Rollback);
            }
        }

        public CheckConnectionMode CheckConnectionMode 
        { 
            get; set; 
        }



        #region private

        IDbConnection _dbConnection;
        System.Data.IDbTransaction _transaction;
        ILogMgr _logMgr;
        ILogger _logger;
        IEventMgr _eventMgr;

        private void InitAdditionalService()
        {
            if (ServiceMgr.Current != null)
            {
                _eventMgr = ServiceMgr.Current.GetInstanceNoEx<IEventMgr>();
                _logger.Debug("AdditionalServices: IEventMgr = {0}.", _eventMgr.ToStateString());
            }
        }   

        private void LogCommand(IDbCommand command)
        {
            if ((_logger == null) || (!_logger.IsDebugEnabled) || (command == null)) return;

            _logger.Debug(command.ToString());            
        }

        private void CheckConnection()
        {
            if (!_dbConnection.IsOpenConnection) throw new NotLogedOnDbException();

            if (CheckConnectionMode == CheckConnectionMode.CheckAndReconnect)                 
            {
                if (!_dbConnection.CheckConnection())
                {
                    _logger.Warn("CheckConnection failed. Try to recennect.");
                    _dbConnection.ReConnect();
                }
            }
        }  

        private void WrapOraException(IDbCommand command, OracleException oraEx)
        {
            if (oraEx.Number == 1403)
                throw new NoDataFoundDbException(oraEx.Procedure, oraEx);
            else if (oraEx.Number == 1422)
                throw new TooManyRowsDbException(oraEx.Procedure, oraEx);
            else if (oraEx.Number == 1012)
                throw new NotLogedOnDbException(oraEx.Procedure, oraEx);
            else if (oraEx.Number == 1)
                throw new UniqueConstraintDbException(oraEx.Procedure, oraEx);
            else throw new UnexpectedDbException(command.CommandText, oraEx);
        }

        private void ExecuteInternal(IDbCommand command)
        {
            OracleConnection connection = _dbConnection.Connection as OracleConnection;
            using (OracleCommand oraCommand = new OracleCommand())
            {
                oraCommand.Connection = connection;
                oraCommand.CommandText = command.CommandText;
                oraCommand.CommandType = command.CommandType;
                foreach (IDbParam param in command.Params.Values)
                {
                    oraCommand.Parameters.Add(param.ToDBParam());
                }

                lock (_dbConnection)
                {
                    oraCommand.ExecuteNonQuery();
                    foreach (OracleParameter oraParam in oraCommand.Parameters)
                    {
                        if (oraParam.Direction != System.Data.ParameterDirection.Input)
                            command.Params[oraParam.ParameterName].FromDBParam(oraParam);
                    }
                }
            }
        }

        private System.Data.DataTable ExecuteSelectInternal(IDbCommand command)
        {
            OracleConnection connection = _dbConnection.Connection as OracleConnection;
            using (OracleCommand oraCommand = new OracleCommand())
            {
                oraCommand.Connection = connection;
                oraCommand.CommandText = command.CommandText;
                oraCommand.CommandType = command.CommandType;
                foreach (IDbParam param in command.Params.Values)
                {
                    oraCommand.Parameters.Add(param.ToDBParam());
                }

                System.Data.DataTable table = new System.Data.DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(oraCommand);
                lock (_dbConnection)
                {
                    adapter.Fill(table);
                    foreach (OracleParameter oraParam in oraCommand.Parameters)
                    {
                        if (oraParam.Direction != System.Data.ParameterDirection.Input)
                            command.Params[oraParam.ParameterName].FromDBParam(oraParam);
                    }
                }
                return table;
            }
        }

        private void FireComandExecutedEvent(string commandText, bool result)
        {
            if (_eventMgr == null) return;
            DbCommandExecutedMessage message = new DbCommandExecutedMessage(commandText, result);
            _eventMgr.GetEvent<DbCommandExecutedEvent>().Publish(message);
        }        

        private void FireTransactionFinishEvent(TransactionResult result)
        {
            if (_eventMgr == null) return;
            DbTransactionFinishMessage message = new DbTransactionFinishMessage(result);
            _eventMgr.GetEvent<DbTransactionFinishEvent>().Publish(message);
        }

        #endregion private
    }
}
