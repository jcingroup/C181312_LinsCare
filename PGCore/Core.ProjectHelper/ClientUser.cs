using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Data.SqlClient;

using SkyCore.DataBaseConnection;

/// <summary>
/// ClientUser 的摘要描述
/// </summary>
public class ClientUser
{
    CommConnection conn = null;
    
    int s_UserID = 0;
    Boolean s_IsAdmin =false;
    int s_UnitID = 0;
    String _acoount;

    public ClientUser(CommConnection conn)
	{
        this.conn = conn;
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public Boolean Verify(string account, string password)
    {
        try
        {
            this._acoount = account;
            //String SQL = "Select id, account, password, name, unit, state, isadmin From UsersData Where account=@account and password=@password;";
            String SQL = "Select id, account, password, name, unit, state, isadmin From UsersData Where account='{0}' and password='{1}';";
            SQL = String.Format(SQL, account.Replace("'", "''"), password.Replace("'", "''"));
            //SqlDataAdapter adpLogin = new SqlDataAdapter(SQL, conn);
            //adpLogin.SelectCommand.Parameters.Add("@account", SqlDbType.VarChar).Value = account;
            //adpLogin.SelectCommand.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            //conn.BuildDataAdapterCommand();
            DataTable dt = conn.ExecuteData(SQL);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            if (dt.Rows[0]["state"].ToString() != "Normal")
            {
                return false;
            }
            else
            {
                //int isadm = int.Parse(dt.Rows[0]["isadmin"].ToString());
                s_UserID = (int)dt.Rows[0]["id"];
                s_IsAdmin = (Boolean)dt.Rows[0]["isadmin"];
                //s_IsAdmin = isadm == 0 ? false : true;
                s_UnitID = (int)dt.Rows[0]["unit"];
                return true;
            }
        }
        catch
        {
            //WriteLog.Write("登錄帳號錯誤:", ex.Message + ":" + ex.StackTrace);
            return false;
        } 
    }

    public int UserID{
        get {return s_UserID;}    
    }

    public Boolean IsAdmin {
        get {
            if (s_IsAdmin == true)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }

    public int UnitID {
        get { return s_UnitID; }
    }

    public String Account { get { return this._acoount; } }
}
