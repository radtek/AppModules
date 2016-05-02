using System;
using System.Data.Common;

namespace Core.SDK.Db
{
    public interface IDbConnection : IDisposable
    {
        void OpenConnection(string username, string password, string database);

        DbConnection Connection { get; }

        void CloseConnection();

        void ReConnect();

        bool CheckConnection();

        void TestConnection();  // Генерируется исключение

        bool IsOpenConnection { get; }

        string UIConnectionString { get; }

        string User { get; }
    }
}
