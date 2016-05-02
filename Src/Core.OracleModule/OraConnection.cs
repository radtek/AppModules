using Oracle.DataAccess.Client;
using Core.SDK.Db;
using Core.SDK.Composite.Event.EventMessage;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Service;

namespace Core.OracleModule
{
    public class OraConnection : Core.SDK.Db.IDbConnection
    {       
        public OraConnection()
        {
            _OracleConnection = new OracleConnection();

            if (ServiceMgr.Current != null)            
                _eventMgr = ServiceMgr.Current.GetInstanceNoEx<IEventMgr>();                                    
        }


        public void OpenConnection(string username, string password, string database)
        {
            _UserID = username;
            _Password = password;
            _DataSource = database;
            CloseConnection();
            _OracleConnection.ConnectionString = ConnectionString;
            try
            {
                System.Data.ConnectionState prevstate = _OracleConnection.State;
                _OracleConnection.Open();
                FireConnectionChangedEvent(prevstate, _OracleConnection.State);
            }
            catch (OracleException oraEx)
            {
                throw new ConnectDbException(oraEx.Procedure, oraEx);
            }
        }

        public System.Data.Common.DbConnection Connection
        {
            get { return _OracleConnection; }
        }

        public void CloseConnection()
        {
            if (_OracleConnection.State != System.Data.ConnectionState.Closed)
            {
                System.Data.ConnectionState prevstate = _OracleConnection.State;
                _OracleConnection.Close();
                FireConnectionChangedEvent(prevstate, _OracleConnection.State);
            }
        }

        public void ReConnect()
        {
            CloseConnection();
            OpenConnection(_UserID, _Password, _DataSource);
        }

        public bool CheckConnection()
        {
            try
            {
                TestConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void TestConnection()
        {
            OracleCommand command = new OracleCommand("select 1 from dual", _OracleConnection);
            command.ExecuteNonQuery();
        }

        public bool IsOpenConnection
        {
            get 
            {
                return ((_OracleConnection != null) && (_OracleConnection.State == System.Data.ConnectionState.Open));
            }
        }        

        public string UIConnectionString
        {
            get
            {
                return _UserID.ToUpper() + " " + _DataSource;
            }
        }

        public string User
        {
            get { return _UserID; }
        }

        public void Dispose()
        {
            _OracleConnection.Dispose();
        }

        #region private

        string _UserID;
        string _Password;
        string _DataSource;
        IEventMgr _eventMgr;

        OracleConnection _OracleConnection;

        private string ConnectionString
        {
            get
            {
                return string.Format("Data Source={0};User ID={1};Password={2};Pooling=false",
                                      _DataSource, _UserID, _Password);
            }
        }

        private void FireConnectionChangedEvent(System.Data.ConnectionState prevState, System.Data.ConnectionState newState)
        {
            if (_eventMgr == null) return;
            DbConnectionChangedMessage message = new DbConnectionChangedMessage(prevState, newState);
            _eventMgr.GetEvent<DbConnectionChangedEvent>().Publish(message);
        }

        #endregion private
    }
}
