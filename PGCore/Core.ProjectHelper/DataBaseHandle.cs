using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

public static class DataBaseHandle
{
    public static string Server { get; set; }
    public static string Account { get; set; }
    public static string Password { get; set; }

    public static SqlConnection conn;

    public static SqlConnection GetConnection()
    {
        return GetConnection(DataBaseName.AspNetWeb);
    }

    public static SqlConnection GetConnection(DataBaseName DBName)
    {
        String connectionstring = String.Empty;
        String GetDBName = String.Empty;

        if (DBName == DataBaseName.AspNetWeb)
        {
            GetDBName = "AspNetWeb";
        }
        else if (DBName == DataBaseName.DB2)
        {
            GetDBName = "DB2";
        }
        else if (DBName == DataBaseName.DB3)
        {
            GetDBName = "DB3";
        }
        else { 
        //DefDB
            GetDBName = "AspNetWeb";
        }

        connectionstring = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True", Server, GetDBName, Account, Password);

        if (conn == null) {
            conn = new SqlConnection(connectionstring);
            conn.Open();
        }

        if (conn.State != System.Data.ConnectionState.Open){
                //conn.ConnectionString = connectionstring;
                //conn.Open();
            conn.Dispose();
            conn = null;
            conn = new SqlConnection(connectionstring);
            conn.Open();
        }

        return conn;
    }

    public static void CloseDB()
    {
        if (conn.State == System.Data.ConnectionState.Open)
        {
            conn.Close();
        }
    }

    public enum DataBaseName { 
        AspNetWeb,
        DB2,
        DB3
    }

}
