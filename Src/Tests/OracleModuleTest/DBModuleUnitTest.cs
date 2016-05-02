using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.OracleModule;
using System.Diagnostics;
using System.Threading;
using Core.OracleModule.Utils;

using Core.SDK.Db;

namespace DBWraperTest
{
    [TestClass]
    public class DBModuleUnitTest
    {
        // Возвращаемый параметр всегда нужно задаваться первым в списке параметров
        // Порядок следования параметров должен совпадать с порядком следования в хранимой процедуре
        // В возвращаемом (или OUT) параметре типа DBParamString нужно указывать размер строки (параметр size в конструкторе)
        // Есть два метода для выполнения команды - Execute и ExecuteSelect:
        // ExecuteSelect предназначен только для выполнения Select-ов (результат выполнения - неименованная выборка, например 'select sysdate from dual' или
        // 'select * from all_object'). ExecuteSelect возвращает экземпляр DataTable, содержащий результаты запроса.
        // Execute - для все остальных случаев, когда выполняемый SQL ничего не возвращает, 
        // или возвращает через параметры команды (ReturnValue, Output, InputOutput параметры) 
        // Многопоточность: 
        // - можно использовать один экземпляр DBManager (а соответственно и DBConnection и DBTransaction) для многих потоков, при этом запросы буду все равно выполняться последовательно
        //   следующий начнется, когда закончится предыдущий. Транзакция может быть только одна на все потоки. Можно использовать для фоновых потоков для которых некритичны ожидания
        //   окончания предыдущих запросов
        // - можно в каждом потоке создать свой экземпляр DBManager и DBConnection.
        // - пул соединений на данный момент не поддерживается

        #region sql script for testing
        /*
        create table test_tab (stmp timestamp default systimestamp, int32 number, double number, string varchar2(2000), datetime date, clob_ CLOB, blob_ BLOB);
        create table test_blob_clob_tab (id number, clob_ CLOB, blob_ BLOB);
        
        CREATE OR REPLACE package CHIPANDDALE.test_pkg as
            function FuncInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob) return clob;
            procedure ProcInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob);
    
            function FuncOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return blob;
            function FuncNullOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return varchar2;
            procedure ProcOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob);
    
            function FuncRefCur(p_int32 in number, p_clob in out clob, p_cur out sys_refcursor) return date;
            procedure ProcRefCur (p_date out date, p_cur in out sys_refcursor);
    
            function FuncAll(p_int32 in out number, p_double in out number, p_date in out date, p_string in out varchar2, p_clob in out clob, p_blob in out blob, p_cur out sys_refcursor, p_cur2 in out sys_refcursor) return varchar2;    
        end;
        /

        CREATE OR REPLACE package body CHIPANDDALE.test_pkg as   
            function FuncInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob) return clob as
                l_clob CLOB;
            begin
                insert into test_tab (int32, double, string, datetime, clob_, blob_) values (p_int32, p_double, p_string, p_date, p_clob, p_blob);
                l_clob :='test, тест, йцу йцу!"№;';
                return l_clob;
            end;
    
            procedure ProcInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob) as
            begin
                insert into test_tab (int32, double, string, datetime, clob_, blob_) values (p_int32, p_double, p_string, p_date, p_clob, p_blob);       
            end;
    
            function FuncOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return blob as
                 l_blob BLOB;
            begin
                p_int32 := -12;
                p_double := 234.234;
                p_date := sysdate;
                p_string := 'test тест !!!';
                p_clob := 'test CLOB тест !!!!';
                select blob_ into p_blob from test_blob_clob_tab where id = 1; 
                l_blob := p_blob;       
                return l_blob;       
            end;
    
             function FuncNullOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return varchar2 as
                 l_blob BLOB;
            begin
                p_int32 := null;
                p_double := null;
                p_date := null;
                p_string :=null;
                p_clob :=null;
                p_blob := null; 
                l_blob := null;       
                return null;
            end;
    
            procedure ProcOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) as
             begin
                p_int32 := -12;
                p_double := 234.234;
                p_date := sysdate;
                p_string := 'test тест !!!2';
                p_clob := 'test CLOB тест !!!!2';
                select blob_ into p_blob from test_blob_clob_tab where id = 1;                     
            end;
    
            function FuncRefCur(p_int32 in number, p_clob in out clob, p_cur out sys_refcursor) return date   as
            begin
                p_clob := '12';
                open p_cur for select 1, 1.2, 'str', sysdate from dual;
                return sysdate+1;
            end;
            procedure ProcRefCur (p_date out date, p_cur in out sys_refcursor) as
             begin
                open p_cur for select id, clob_, blob_ from test_blob_clob_tab;
                p_date := sysdate+2;
            end;
    
            function FuncAll(p_int32 in out number, p_double in out number, p_date in out date, p_string in out varchar2, p_clob in out clob, p_blob in out blob, p_cur out sys_refcursor, p_cur2 in out sys_refcursor) return varchar2 as
            begin
                insert into test_tab (int32, double, string, datetime, clob_, blob_) values (p_int32, p_double,p_string,  p_date, p_clob, p_blob);
                p_int32 := 1;
                p_double := p_double + 1.11;
                p_date := p_date + 11;
                p_string := p_string || '$$$$$';
                p_clob :='!!!!!!!!!!!!!!!!' || p_clob;
                select blob_ into p_blob from test_blob_clob_tab where id = 1;    
        
                 open p_cur for select 1, 1.2, 'str', sysdate from dual;
                 open p_cur2 for select id, clob_, blob_ from test_blob_clob_tab;
                     
                return 'ok';
            end;
    
        end;
        /
        */
        #endregion

        Core.SDK.Log.ILogMgr _LogMgr;
        Core.SDK.Log.ILogger _Logger;
        public DBModuleUnitTest()
        {
            Environment.SetEnvironmentVariable("TNS_ADMIN", @"J:\Other_project\MyUtils\Deps\ODAC11\", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.CL8MSWIN1251", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("Path", @"J:\Other_project\MyUtils\Deps\ODAC11\;" + Environment.GetEnvironmentVariable("Path"), EnvironmentVariableTarget.Process);
            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("DBModuleUnitTest");
            _Logger.Info("Start OracleModuleTest.");           
        }

        #region Test connection
        [TestMethod]
        public void TestConnection()
        {
            using (IDbConnection conn = new OraConnection())
            {
                Debug.Assert(!conn.IsOpenConnection, "IsConnect1");
                Debug.Assert(!conn.CheckConnection(), "CheckConnection1");

                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                Debug.Assert(conn.CheckConnection(), "CheckConnection2");
                Debug.Assert(conn.IsOpenConnection, "IsConnect2");

                conn.TestConnection();
                Debug.Assert(conn.CheckConnection(), "CheckConnection3");
                Debug.Assert(conn.IsOpenConnection, "IsConnect3");

                conn.CloseConnection();
                Debug.Assert(!conn.IsOpenConnection, "IsConnect4");
                Debug.Assert(!conn.CheckConnection(), "CheckConnection4");

                conn.ReConnect();
                Debug.Assert(conn.IsOpenConnection, "IsConnect5");
                Debug.Assert(conn.CheckConnection(), "CheckConnection5");

                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                Debug.Assert(conn.CheckConnection(), "CheckConnection6");
                Debug.Assert(conn.IsOpenConnection, "IsConnect6");

                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                conn.ReConnect();
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                Debug.Assert(conn.CheckConnection(), "CheckConnection7");
                Debug.Assert(conn.IsOpenConnection, "IsConnect7");


                conn.ReConnect();
                conn.ReConnect();
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                conn.CloseConnection();
                conn.CloseConnection();
                conn.ReConnect();
                Debug.Assert(conn.CheckConnection(), "CheckConnection8");
                Debug.Assert(conn.IsOpenConnection, "IsConnect8");
            }
        }
        #endregion Test connection

        #region Test call oracle function and procedure
        [TestMethod]
        public void TestFuncInParams()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // function FuncInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob) return clob;
                IDbCommand command = new OraCommand("test_pkg.FuncInParams");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                OraParamCLOB returnParam = new OraParamCLOB("return", System.Data.ParameterDirection.ReturnValue, null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Input, 1234));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.Input, 1234.23d));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Input, DateTime.Now));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.Input, "STRING qwe фывфыв"));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.Input, "CLOB 123123 qweqweqweq йцуйцуйу"));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.Input, new byte[] { 1, 2, 3, 4, 5, 6 }));

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                string retStr = returnParam.ParamValue;
                Debug.Assert(!string.IsNullOrEmpty(retStr), "FuncInParams");
            }
        }

        [TestMethod]
        public void TestProcInParams()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // procedure ProcInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob);
                IDbCommand command = new OraCommand("test_pkg.ProcInParams");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Input, 1234));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.Input, 1234.23d));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Input, DateTime.Now));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.Input, "STRING qwe фывфыв"));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.Input, "CLOB 123123 qweqweqweq йцуйцуйу"));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.Input, new byte[] { 1, 2, 3, 4, 5, 6 }));

                dbManager.StartTransaction();
                dbManager.Execute(command);
                dbManager.RollbackTransaction();
            }
        }

        [TestMethod]
        public void TestProcNullInParams()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // procedure ProcInParams (p_int32 in number, p_double in number, p_date in date, p_string in varchar2, p_clob in clob, p_blob in blob);
                IDbCommand command = new OraCommand("test_pkg.ProcInParams");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.Input, (byte[])null));

                dbManager.StartTransaction();
                dbManager.Execute(command);
                dbManager.RollbackTransaction();
            }
        }

        [TestMethod]
        public void TestFuncOutParams()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // function FuncOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return blob;            
                IDbCommand command = new OraCommand("test_pkg.FuncOutParams");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                OraParamBLOB returnParam = new OraParamBLOB("return", System.Data.ParameterDirection.ReturnValue, (byte[])null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.Output, null, 2000));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.Output, (byte[])null));

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                byte[] blob = returnParam.ParamValue;
                Debug.Assert(blob != null, "returnParam");
                Debug.Assert(command.Params["p_int32"].GetValue() != null, "p_int32");
                Debug.Assert(command.Params["p_double"].GetValue() != null, "p_double");
                Debug.Assert(command.Params["p_date"].GetValue() != null, "p_date");
                Debug.Assert(command.Params["p_string"].GetValue() != null, "p_string");
                Debug.Assert(command.Params["p_clob"].GetValue() != null, "p_clob");
                Debug.Assert(command.Params["p_blob"].GetValue() != null, "p_blob");
            }
        }

        [TestMethod]
        public void TestFuncNullOutParams()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // function FuncNullOutParams (p_int32 out number, p_double out number, p_date out date, p_string out varchar2, p_clob out clob, p_blob out blob) return blob;            
                IDbCommand command = new OraCommand("test_pkg.FuncNullOutParams");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                OraParamString returnParam = new OraParamString("return", System.Data.ParameterDirection.ReturnValue, null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.Output, null, 2000));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.Output, null));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.Output, (byte[])null));

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                Debug.Assert(returnParam.ParamValue == null, "returnParam");
                Debug.Assert(command.Params["p_int32"].GetValue() == null, "p_int32");
                Debug.Assert(command.Params["p_double"].GetValue() == null, "p_double");
                Debug.Assert(command.Params["p_date"].GetValue() == null, "p_date");
                Debug.Assert(command.Params["p_string"].GetValue() == null, "p_string");
                Debug.Assert(command.Params["p_clob"].GetValue() == null, "p_clob");
                Debug.Assert(command.Params["p_blob"].GetValue() == null, "p_blob");
            }
        }

        [TestMethod]
        public void TestFuncRefCur()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // function FuncRefCur(p_int32 in number, p_clob in out number, p_cur out sys_refcursor) return date;
                IDbCommand command = new OraCommand("test_pkg.FuncRefCur");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                OraParamDateTime returnParam = new OraParamDateTime("return", System.Data.ParameterDirection.ReturnValue, null);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.Input, null));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.InputOutput, "132"));
                OraParamRefCursor refCur = new OraParamRefCursor("p_cur", System.Data.ParameterDirection.Output);
                command.AddDBParam(refCur);

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                Debug.Assert(refCur.ParamValue.Rows.Count != 0, "p_cur");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[0] != null, "p_cur[0][0]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[1] != null, "p_cur[0][1]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[2] != null, "p_cur[0][2]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[3] != null, "p_cur[0][3]");
                Debug.Assert(command.Params["p_clob"].GetValue() != null, "p_clob");
                Debug.Assert(command.Params["return"].GetValue() != null, "return");
            }
        }

        [TestMethod]
        public void TestProcRefCur()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // procedure ProcRefCur (p_date out date, p_cur in out sys_refcursor) as
                IDbCommand command = new OraCommand("test_pkg.ProcRefCur");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.Output, null));
                OraParamRefCursor refCur = new OraParamRefCursor("p_cur", System.Data.ParameterDirection.InputOutput);
                command.AddDBParam(refCur);

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                Debug.Assert(refCur.ParamValue.Rows.Count != 0, "p_cur");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[0] != null, "p_cur[0][0]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[1] != null, "p_cur[0][1]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[2] != null, "p_cur[0][2]");
                Debug.Assert(command.Params["p_date"].GetValue() != null, "p_date");

                object o = refCur.ParamValue.Rows[0]["blob_"];
                byte[] ba = (byte[])o;
            }
        }

        [TestMethod]
        public void TestFuncAll()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                // function FuncAll(p_int32 in out number, p_double in out number, p_date in out date, p_string in out varchar2, p_clob in out clob, p_blob in out blob, p_cur out sys_refcursor, p_cur2 in out sys_refcursor) return varchar2;
                IDbCommand command = new OraCommand("test_pkg.FuncAll");
                command.CommandType = System.Data.CommandType.StoredProcedure;

                OraParamString returnParam = new OraParamString("return", System.Data.ParameterDirection.ReturnValue, null, 2000);
                command.AddDBParam(returnParam);
                command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.InputOutput, null));
                command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.InputOutput, -1.234));
                command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.InputOutput, DateTime.Now));
                command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.InputOutput, "INPUT ", 2000));
                command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.InputOutput, null));
                command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.InputOutput, (byte[])null));
                OraParamRefCursor refCur = new OraParamRefCursor("p_cur", System.Data.ParameterDirection.Output);
                command.AddDBParam(refCur);
                OraParamRefCursor refCur2 = new OraParamRefCursor("p_cur2", System.Data.ParameterDirection.InputOutput);
                command.AddDBParam(refCur2);

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }

                Debug.Assert(returnParam.ParamValue != null, "returnParam");
                Debug.Assert(command.Params["p_int32"].GetValue() != null, "p_int32");
                Debug.Assert(command.Params["p_double"].GetValue() != null, "p_double");
                Debug.Assert(command.Params["p_date"].GetValue() != null, "p_date");
                Debug.Assert(command.Params["p_string"].GetValue() != null, "p_string");
                Debug.Assert(command.Params["p_clob"].GetValue() != null, "p_clob");
                Debug.Assert(command.Params["p_blob"].GetValue() != null, "p_blob");

                Debug.Assert(refCur.ParamValue.Rows.Count != 0, "p_cur");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[0] != null, "p_cur[0][0]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[1] != null, "p_cur[0][1]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[2] != null, "p_cur[0][2]");
                Debug.Assert(refCur.ParamValue.Rows[0].ItemArray[3] != null, "p_cur[0][3]");

                Debug.Assert(refCur2.ParamValue.Rows.Count != 0, "p_cur2");
                Debug.Assert(refCur2.ParamValue.Rows[0].ItemArray[0] != null, "p_cur2[0][0]");
                Debug.Assert(refCur2.ParamValue.Rows[0].ItemArray[1] != null, "p_cur2[0][1]");
                Debug.Assert(refCur2.ParamValue.Rows[0].ItemArray[2] != null, "p_cur2[0][2]");
            }
        }
        #endregion Test call oracle function and procedure

        #region Test SQL query and anonim PS/SQL blocks
        [TestMethod]
        public void TestSQLQuery()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                IDbCommand command = new OraCommand("select sysdate from dual");
                command.CommandType = System.Data.CommandType.Text;

                System.Data.DataTable tab = dbManager.ExecuteSelect(command);

                Debug.Assert(tab.Rows.Count != 0, "DataTable");
                DateTime date = (DateTime)tab.Rows[0]["sysdate"];
            }
        }

        [TestMethod]
        public void TestSQLQuery2()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                IDbCommand command = new OraCommand("select to_date('01.01.1111 01:01:01', 'dd.mm.yyyy hh24:mi:ss') col_date, '1+2' col_str, 1+2 col_num from dual");
                command.CommandType = System.Data.CommandType.Text;

                System.Data.DataTable tab = dbManager.ExecuteSelect(command);

                Debug.Assert(tab.Rows.Count != 0, "DataTable");
                DateTime date = (DateTime)tab.Rows[0]["col_date"];
                Debug.Assert(date == new DateTime(1111, 1, 1, 1, 1, 1), "DateEqual");

                string str = tab.Rows[0]["col_str"].ToString();
                Debug.Assert(string.Equals(str, "1+2"), "StrEqual");

                Int32 n = Convert.ToInt32(tab.Rows[0]["col_num"]);
                Debug.Assert(n == 1 + 2, "NumEqual");
            }
        }

        [TestMethod]
        public void TestSQLQuery3()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                IDbCommand command = new OraCommand("insert into  test_tab (int32, double, string, datetime) values (1, 1.2, '123', sysdate)");
                command.CommandType = System.Data.CommandType.Text;

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    dbManager.Execute(command);
                    dbManager.Execute(command);
                    transaction.Success = true;
                }
            }
        }


        [TestMethod]
        public void TestSQLQuery4()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                IDbCommand command = new OraCommand("update test_tab set int32 = :param1 where string = :param2 returning count(*) into :param3");
                command.CommandType = System.Data.CommandType.Text;

                command.AddDBParam(new OraParamInt32(":param1", System.Data.ParameterDirection.Input, 222));
                command.AddDBParam(new OraParamString(":param2", System.Data.ParameterDirection.Input, "123", 100));
                command.AddDBParam(new OraParamInt32(":param3", System.Data.ParameterDirection.Output, null));

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }
            }
        }

        [TestMethod]
        public void TestSQLQuery5()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                IDbCommand command = new OraCommand("begin execute immediate 'create or replace view test_view as select 1 col_name from dual'; end;");
                command.CommandType = System.Data.CommandType.Text;

                using (DbTransaction transaction = new DbTransaction(dbManager))
                {
                    dbManager.Execute(command);
                    transaction.Success = true;
                }
            }
        }
        #endregion Test SQL query and anonim PS/SQL blocks

        #region Test using multithread environment
        [TestMethod]
        public void TestMultiTread1()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                Thread thread1 = new Thread(() =>
                {
                    IDbCommand command = new OraCommand("select * from all_objects");
                    command.CommandType = System.Data.CommandType.Text;

                    System.Data.DataTable tab = null;
                    for (int i = 0; i < 50; i++)
                    {
                        tab = dbManager.ExecuteSelect(command);
                    }
                });

                Thread thread2 = new Thread(() =>
                {
                    IDbCommand command = new OraCommand("insert into  test_tab (int32, double, string, datetime) values (1, 1.2, '123', sysdate)");
                    command.CommandType = System.Data.CommandType.Text;

                    using (DbTransaction transaction = new DbTransaction(dbManager))
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            dbManager.Execute(command);
                        }
                        transaction.Success = true;
                    }
                });

                Thread thread3 = new Thread(() =>
                {
                    IDbCommand command = new OraCommand("begin execute immediate 'create or replace view test_view as select 1 col_name from dual'; end;");
                    command.CommandType = System.Data.CommandType.Text;

                    for (int i = 0; i < 100; i++)
                    {
                        dbManager.Execute(command);
                    }
                });

                Thread thread4 = new Thread(() =>
                {
                    IDbCommand command = new OraCommand("test_pkg.FuncAll");
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Транзакцию не начинаем, т.к. начата в первом потоке, параметры пересоздаем, т.к. InputOutput
                    for (int i = 0; i < 100; i++)
                    {
                        command.Params.Clear();
                        OraParamString returnParam = new OraParamString("return", System.Data.ParameterDirection.ReturnValue, null, 2000);
                        command.AddDBParam(returnParam);
                        command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.InputOutput, null));
                        command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.InputOutput, -1.234));
                        command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.InputOutput, DateTime.Now));
                        command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.InputOutput, "INPUT ", 2000));
                        command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.InputOutput, null));
                        command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.InputOutput, (byte[])null));
                        OraParamRefCursor refCur = new OraParamRefCursor("p_cur", System.Data.ParameterDirection.Output);
                        command.AddDBParam(refCur);
                        OraParamRefCursor refCur2 = new OraParamRefCursor("p_cur2", System.Data.ParameterDirection.InputOutput);
                        command.AddDBParam(refCur2);
                        dbManager.Execute(command);
                    }
                });

                thread1.Start();
                thread2.Start();
                thread3.Start();
                thread4.Start();

                thread1.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();
            }
        }

        [TestMethod]
        public void TestMultiTread2()
        {
            Thread thread1 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("select * from all_objects");
                    command.CommandType = System.Data.CommandType.Text;

                    System.Data.DataTable tab = null;
                    for (int i = 0; i < 50; i++)
                    {
                        tab = dbManager.ExecuteSelect(command);
                    }
                }
            });

            Thread thread2 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("insert into  test_tab (int32, double, string, datetime) values (1, 1.2, '123', sysdate)");
                    command.CommandType = System.Data.CommandType.Text;

                    using (DbTransaction transaction = new DbTransaction(dbManager))
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            dbManager.Execute(command);
                        }
                        transaction.Success = true;
                    }
                }
            });

            Thread thread3 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("begin execute immediate 'create or replace view test_view as select 1 col_name from dual'; end;");
                    command.CommandType = System.Data.CommandType.Text;

                    for (int i = 0; i < 100; i++)
                    {
                        dbManager.Execute(command);
                    }
                }
            });

            Thread thread4 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("test_pkg.FuncAll");
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    for (int i = 0; i < 100; i++)
                    {
                        command.Params.Clear();
                        OraParamString returnParam = new OraParamString("return", System.Data.ParameterDirection.ReturnValue, null, 2000);
                        command.AddDBParam(returnParam);
                        command.AddDBParam(new OraParamInt32("p_int32", System.Data.ParameterDirection.InputOutput, null));
                        command.AddDBParam(new OraParamDouble("p_double", System.Data.ParameterDirection.InputOutput, -1.234));
                        command.AddDBParam(new OraParamDateTime("p_date", System.Data.ParameterDirection.InputOutput, DateTime.Now));
                        command.AddDBParam(new OraParamString("p_string", System.Data.ParameterDirection.InputOutput, "INPUT ", 2000));
                        command.AddDBParam(new OraParamCLOB("p_clob", System.Data.ParameterDirection.InputOutput, null));
                        command.AddDBParam(new OraParamBLOB("p_blob", System.Data.ParameterDirection.InputOutput, (byte[])null));
                        OraParamRefCursor refCur = new OraParamRefCursor("p_cur", System.Data.ParameterDirection.Output);
                        command.AddDBParam(refCur);
                        OraParamRefCursor refCur2 = new OraParamRefCursor("p_cur2", System.Data.ParameterDirection.InputOutput);
                        command.AddDBParam(refCur2);
                        using (DbTransaction transaction = new DbTransaction(dbManager))
                        {
                            dbManager.Execute(command);
                            dbManager.Execute(command);
                            transaction.Success = true;
                        }
                    }
                }
            });

            Thread thread5 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("insert into  test_tab (int32, double, string, datetime) values (1, 1.2, '123', sysdate)");
                    command.CommandType = System.Data.CommandType.Text;

                    using (DbTransaction transaction = new DbTransaction(dbManager))
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            dbManager.Execute(command);
                        }
                        transaction.Success = true;
                    }
                }
            });

            Thread thread6 = new Thread(() =>
            {
                using (IDbConnection conn = new OraConnection())
                {
                    conn.OpenConnection("chipanddale", "chipanddale", "xe");
                    IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);
                    IDbCommand command = new OraCommand("insert into  test_tab (int32, double, string, datetime) values (1, 1.2, '123', sysdate)");
                    command.CommandType = System.Data.CommandType.Text;

                    for (int i = 0; i < 1000; i++)
                    {
                        using (DbTransaction transaction = new DbTransaction(dbManager))
                        {
                            dbManager.Execute(command);
                            transaction.Success = true;
                        }
                    }

                }
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
            thread6.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();
            thread5.Join();
            thread6.Join();
        }
        #endregion Test using multithread environment

        #region Test DBSettingReadWriter
        [TestMethod]
        public void TestDBSettingReadWriter()
        {
            using (IDbConnection conn = new OraConnection())
            {
                conn.OpenConnection("chipanddale", "chipanddale", "xe");
                IDbMgr dbManager = new OraDBMgr(conn, _LogMgr);

                byte[] etalon_blob = new byte[] {1,2,3,4,5,4,3,2,1,23,3,4};
                OraDBSettingReadWriter settingRW = new OraDBSettingReadWriter(dbManager, _LogMgr);
                settingRW.WriteValue("Setting1", "Simple string1");
                settingRW.WriteValue("Setting2", "Simple string2");
                settingRW.WriteValue("Setting3", "Long string - CLOB. ".PadRight(10000, '#'));
                settingRW.WriteValue("Setting4", string.Empty);
                settingRW.WriteValue("Setting5", (string)null);
                settingRW.WriteValue("SettingB1", etalon_blob);
                settingRW.WriteValue("SettingB2", new byte[] { });
                settingRW.WriteValue("SettingB3", (byte[])null);
                settingRW.Save("User1", "Section1", "Subsection1");

                string result = null;
                byte[] result_blob = null;                
                settingRW.Load("User1", "Section1", "Subsection1");

                Debug.Assert(!settingRW.ReadValue("Setting_?", out result), "Find non-exist setting");
                settingRW.ReadValue("Setting1", out result);
                Debug.Assert(result == "Simple string1", "Setting1 non-equal");
                settingRW.ReadValue("Setting2", out result);
                Debug.Assert(result == "Simple string2", "Setting2 non-equal");
                settingRW.ReadValue("Setting3", out result);
                Debug.Assert(result == "Long string - CLOB. ".PadRight(10000, '#'), "Setting3 CLOB non-equal");
                settingRW.ReadValue("Setting4", out result);
                Debug.Assert(string.IsNullOrEmpty(result), "Setting4 non-empty");
                settingRW.ReadValue("Setting5", out result);
                Debug.Assert(string.IsNullOrEmpty(result), "Setting5 non-empty");

                settingRW.ReadValue("SettingB1", out result_blob);
                for (int i = 0; i<result_blob.Length; i++)
                { 
                    if (result_blob[i]!=etalon_blob[i])
                    {
                        Debug.Assert(false, "SettingB1. Blob byte["+i.ToString()+"] non-equal");
                        return;
                    }
                }
                settingRW.ReadValue("SettingB2", out result_blob);
                Debug.Assert(result_blob == null, "SettingB2 non-null");

                settingRW.ReadValue("SettingB3", out result_blob);
                Debug.Assert(result_blob == null, "SettingB3 non-null");

                settingRW.Load("User__2", "Section1", "Subsection1");
                Debug.Assert(!settingRW.ReadValue("Setting1", out result), "Find setting of non-exist user");

                settingRW.Load("User1", "Section__2", "Subsection1");
                Debug.Assert(!settingRW.ReadValue("Setting1", out result), "Find setting of non-exist section");

                settingRW.Load("User1", "Section1", "Subsection__2");
                Debug.Assert(!settingRW.ReadValue("Setting1", out result), "Find setting of non-exist subsection");   
            }
        }
        #endregion Test DBSettingReadWriter
    
    }
}
