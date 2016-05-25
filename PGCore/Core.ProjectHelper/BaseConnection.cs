using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.Data;
using SkyCore.ConnectionDefault;

namespace SkyCore.DataBaseConnection
{
    public static class BaseConnection
    {
        public static String Server { get; set; }
        public static String Account { get; set; }
        public static String Password { get; set; }

        public static String Path { get; set; }

        public static CommConnection conn;

        public static CommConnection GetConnection()
        {
            return GetConnection(DataBaseName.DB_3M_MVC);
        }

        public static CommConnection GetConnection(DataBaseName DBName)
        {
            return GetConnection(DBName, ConnectionDBType.SqlServer);
        }

        public static CommConnection GetConnection(DataBaseName DBName, ConnectionDBType type)
        {
            String connectionstring = String.Empty;
            String GetDBName = String.Empty;

            #region Get need used database name

            if (DBName.ToString().StartsWith("Default"))
            {
                GetDBName = DBName.ToString().Replace("Default_", "");
            }
            else
            {
                GetDBName = DBName.ToString().Replace("DB_", "");
            }
            #endregion

            if (type == ConnectionDBType.SqlServer)
            {
                //SQL Server
                connectionstring = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True", Server, GetDBName, Account, Password);
            }

            if (type == ConnectionDBType.OleDB)
            {
                //Access
                String FilePath = Path + "\\_Code\\Database\\MainDB.mdb";
                connectionstring = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", FilePath);
            }

            if (type == ConnectionDBType.MySql)
            {
                //SQL Server
                connectionstring = String.Format("Network Address={0};Initial Catalog='{1}';Persist Security Info=no;User Name='{2}';Password='{3}'", Server, GetDBName, Account, Password);
            }

            if (conn == null)
            {
                conn = new CommConnection(connectionstring, type);
                conn.Open();
            }

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Dispose();
                conn = null;
                conn = new CommConnection(connectionstring, type);
                conn.Open();
            }
            return conn;
        }

        public static void CloseDB()
        {
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }

    public class CommConnection
    {
        private OleDbConnection OleConn;
        private MySqlConnection MySqlConn;
        private SqlConnection SqlConn;

        public ConnectionDBType connType { get; set; }

        private SqlTransaction sTrans;
        //private OleDbTransaction oTrans;
        private MySqlTransaction mTrans;

        private SqlDataAdapter sadp;
        private OleDbDataAdapter oadp;
        //private MySqlDataAdapter madp;

        private SqlCommandBuilder ssb;
        private OleDbCommandBuilder osb;

        public int InsertAutoFieldsID { get; set; }
        public int AffetCount { get; set; }

        public CommConnection(String ConnectionString)
        {
            SqlConn = new SqlConnection(ConnectionString);
            connType = ConnectionDBType.SqlServer;
            AcceptChanges = true;
        }

        public CommConnection(String ConnectionString, ConnectionDBType type)
        {
            connType = type;
            AcceptChanges = true;

            if (type == ConnectionDBType.SqlServer)
            {
                SqlConn = new SqlConnection(ConnectionString);
            }
            else if (type == ConnectionDBType.OleDB)
            {
                OleConn = new OleDbConnection(ConnectionString);
            }
            else if (type == ConnectionDBType.MySql)
            {
                MySqlConn = new MySqlConnection(ConnectionString);
            }
        }

        public System.Data.ConnectionState State
        {
            get
            {

                if (connType == ConnectionDBType.SqlServer)
                {
                    return SqlConn.State;
                }
                else if (connType == ConnectionDBType.OleDB)
                {
                    return OleConn.State;
                }
                //else if (connType == DatabaseType.MySql)
                //{
                // return MySqlConn.State;
                //}
                else
                {
                    return SqlConn.State;
                }
            }
        }

        public void Open()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                SqlConn.Open();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                OleConn.Open();
            }
            //else if (connType == DatabaseType.MySql)
            //{
            //    MySqlConn.Open();
            //}
        }

        public void BeginTransaction()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                sTrans = SqlConn.BeginTransaction();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                //oTrans = OleConn.BeginTransaction();
            }
            else if (connType == ConnectionDBType.MySql)
            {
                mTrans = MySqlConn.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                sTrans.Commit();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                //oTrans.Commit();
            }
            //else if (connType == DatabaseType.MySql)
            // {
            //    mTrans.Commit();
            //}
        }

        public void Rollback()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                sTrans.Rollback();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                //oTrans.Rollback();
            }
            //else if (connType == DatabaseType.MySql)
            //{
            //    mTrans.Rollback();
            //}
        }

        public void Close()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                SqlConn.Close();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                OleConn.Close();
            }
            //else if (connType == DatabaseType.MySql)
            //{
            //    MySqlConn.Close();
            //}
        }

        public void Dispose()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                SqlConn.Dispose();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                OleConn.Dispose();
            }
            //else if (connType == DatabaseType.MySql)
            //{
            //    MySqlConn.Dispose();
            //}
        }

        public void ExecuteNonQuery(String SQL)
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                SqlCommand Cmd = new SqlCommand(SQL, SqlConn);
                if (sTrans != null)
                {
                    Cmd.Transaction = sTrans;
                }
                Cmd.ExecuteNonQuery();
                Cmd.Dispose();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                OleDbCommand Cmd = new OleDbCommand(SQL, OleConn);
                Cmd.ExecuteNonQuery();
                Cmd.Dispose();
            }
            else if (connType == ConnectionDBType.MySql)
            {
                // MySqlCommand Cmd = new MySqlCommand(SQL, MySqlConn);
                // if (mTrans != null)
                // {
                //     Cmd.Transaction = mTrans;
                // }
                // Cmd.ExecuteNonQuery();
                // Cmd.Dispose();
            }
        }

        public DataTable ExecuteData(String SQL)
        {
            DataTable dt = new DataTable();

            if (connType == ConnectionDBType.SqlServer)
            {
                if (sadp == null)
                {
                    sadp = new SqlDataAdapter();
                    sadp.SelectCommand = new SqlCommand(SQL, SqlConn);
                }
                sadp.SelectCommand.CommandText = SQL;

                if (sTrans != null) sadp.SelectCommand.Transaction = sTrans;
                sadp.Fill(dt);
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                oadp = new OleDbDataAdapter(SQL, OleConn);
                oadp.Fill(dt);
            }
            else if (connType == ConnectionDBType.MySql)
            {

            }
            return dt;
        }

        public Object ExecuteScalar(String SQL)
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                SqlCommand Cmd = new SqlCommand(SQL, SqlConn);
                if (sTrans != null)
                {
                    Cmd.Transaction = sTrans;
                }
                return Cmd.ExecuteScalar();
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                OleDbCommand Cmd = new OleDbCommand(SQL, OleConn);
                return Cmd.ExecuteScalar();
            }
            else if (connType == ConnectionDBType.MySql)
            {
                MySqlCommand Cmd = new MySqlCommand(SQL, MySqlConn);
                if (mTrans != null)
                {
                    Cmd.Transaction = mTrans;
                }
                return Cmd.ExecuteScalar();
            }
            else
            {

                return null;
            }
        }

        public DataTable GetTableOBJ(String TableName)
        {

            String Sql = "Select Top 1 * From " + TableName;
            return ExecuteData(Sql);
        }

        public DataTable GetTableOBJByID(String TableName, int id)
        {
            return GetTableOBJByID(TableName, "id", id);
        }

        public DataTable GetTableOBJByID(String TableName, String IdFieldName, int id)
        {
            String Sql = "Select * From " + TableName + " Where " + IdFieldName + "=" + id;
            return ExecuteData(Sql);
        }

        public void BuildDataAdapterCommand()
        {
            if (connType == ConnectionDBType.SqlServer)
            {
                if (sTrans != null)
                {
                    sadp.SelectCommand.Transaction = sTrans;
                }

                sadp.RowUpdated += new SqlRowUpdatedEventHandler(SQLServerOnRowUpdated);
                ssb = new SqlCommandBuilder(sadp);
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                oadp.RowUpdated += new OleDbRowUpdatedEventHandler(OleDBOnRowUpdated);
                osb = new OleDbCommandBuilder(oadp);
            }
            else if (connType == ConnectionDBType.MySql)
            {
                //madp = new MySqlDataAdapter(SQL, MySqlonn);
                //MySQLCommandBuilder sb = new MySQLDbCommandBuilder(oadp);
                //if (mTrans != null)
                //{
                //    madp.SelectCommand.Transaction = mTrans;
                //}
            }
        }

        public Boolean AcceptChanges { get; set; }

        public void UpdateDataAdapter(DataTable dt)
        {
            BuildDataAdapterCommand();

            if (connType == ConnectionDBType.SqlServer)
            {
                AffetCount = sadp.Update(dt);
                if (AcceptChanges)
                {
                    dt.AcceptChanges();
                    if (dt.Columns.Contains("id"))
                    {
                        InsertAutoFieldsID = (int)dt.Rows[0]["id"];
                    }
                }
            }
            else if (connType == ConnectionDBType.OleDB)
            {
                AffetCount = oadp.Update(dt);
                if (AcceptChanges) dt.AcceptChanges();
            }
            else if (connType == ConnectionDBType.MySql)
            {
                //madp.Update(dt);
                //dt.AcceptChanges();
            }
        }

        protected void OleDBOnRowUpdated(object sender, OleDbRowUpdatedEventArgs args)
        {
            OleDbCommand idCMD = new OleDbCommand("SELECT @@IDENTITY", OleConn);

            if (args.StatementType == StatementType.Insert)
            {
                InsertAutoFieldsID = (int)idCMD.ExecuteScalar();
            }
        }

        protected void SQLServerOnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            /*
            if (args.Status == UpdateStatus.Continue)
            {
                SqlCommand idCMD = new SqlCommand("SELECT SCOPE_IDENTITY()", SqlConn);
                if (sTrans != null) idCMD.Transaction = sTrans;
                if (args.StatementType == StatementType.Insert)
                {
                    Object o = idCMD.ExecuteScalar();
                    //InsertAutoFieldsID = (int)idCMD.ExecuteScalar();
                }
            }
            */
        }
    }
}
