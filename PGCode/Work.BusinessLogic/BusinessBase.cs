using System;
using System.Data;
using ProcCore.DatabaseCore.DataBaseConnection;

namespace ProcCore.Business.Base
{
    #region LogicBase
    public class LogicBase : IDisposable
    {
        public CommConnection Connection { get; set; }
        public Log.LogPlamInfo logPlamInfo { get; set; }
        public String Lang = System.Globalization.CultureInfo.CurrentCulture.Name;
        public int GetIDX()
        {
            CommConnection conn = new CommConnection(this.Connection.ConnectionString, this.Connection.ConnectionType);

            conn.Tran();
            conn.ExecuteNonQuery("Update _IDX Set IDX = IDX + 1");
            DataTable dt = conn.ExecuteData("Select IDX From _IDX");
            int i = (int)dt.Rows[0][0];
            conn.Commit();
            conn.Dispose();
            return i;
        }
        protected void ExecuteSQL(String SQL)
        {
            Connection.ExecuteNonQuery(SQL);
        }
        protected DataTable ExecuteData(String SQL)
        {
            return Connection.ExecuteData(SQL);
        }
        protected void Params(String name,Object value) {

            Connection.AddParm(name,value);
        }
        protected Object ExecuteScalar(String SQL)
        {
            return Connection.ExecuteScalar(SQL);
        }
        protected String PackErrMessage(Exception ex)
        {
            return ex.Message + ":" + ex.StackTrace;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Connection != null) Connection.Close(); Connection.Dispose();
            }
        }
    }
    public abstract class LogicBase<m_Module, q_Module, TabModule> : IDisposable
    {
        public CommConnection Connection { get; set; }
        public Log.LogPlamInfo logPlamInfo { get; set; }
        public String Lang = System.Globalization.CultureInfo.CurrentCulture.Name;
        public int GetIDX()
        {
            CommConnection conn = new CommConnection(this.Connection.ConnectionString,this.Connection.ConnectionType);
            
            conn.Tran();
            conn.ExecuteNonQuery("Update _IDX Set IDX = IDX + 1");
            DataTable dt = conn.ExecuteData("Select IDX From _IDX");
            int i =(int)dt.Rows[0][0];
            conn.Commit();
            conn.Dispose();
            return i;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (Connection != null) Connection.Close(); Connection.Dispose();
            }
        }

        protected String PackErrMessage(Exception ex)
        {
            String s = "[Message:{0}][Track:{1}]";
            return String.Format(s, ex.Message, ex.StackTrace);
        }
        public abstract RunInsertEnd InsertMaster(m_Module md, Int32 accountId);
        public abstract RunUpdateEnd UpdateMaster(m_Module md, Int32 accountId);
        public abstract RunDeleteEnd DeleteMaster(Int32[] ids, Int32 accountId);
        public abstract RunQueryPackage<m_Module> SearchMaster(q_Module qr, Int32 accountId);
        public abstract RunOneDataEnd<m_Module> GetDataMaster(Int32 id, Int32 accountId);
        public abstract RunOneDataEnd<m_Module> GetDataMaster(Int32[] ids, Int32 accountId);
    }
    public class ClientInfo
    {
        public Int32 AccountId { get; set; }
        public String IP { get; set; }
        public String ApplcationInfo { get; set; }
    }

    #endregion

    #region 基礎類別區
    public abstract class JQOperation
    {
        public String oper { get; set; }
    }
    public abstract class ModuleBase : JQOperation
    {
        public EditModeType EditType { get; set; }
    }
    public abstract class QueryBase
    {
        public QueryBase()
        {
            FliterMinute = 10;
        }
        public Boolean FliterLock { get; set; }
        /// <summary>
        /// Lock時間定 在這時間範圍會被過慮
        /// </summary>
        public int FliterMinute { get; set; }

        public int? page { get; set; }
        public String sidx { get; set; }

        /// <summary>
        /// JQGrid欄位排序行態 (asc or desc)由JQGrid自動產生
        /// </summary>
        public String sord { get; set; }
        public int rows { get; set; }
        public Boolean _search { get; set; }
        /// <summary>
        /// 限制查詢筆數，0為不限制。
        /// </summary>
        public Int16 MaxRecord { get; set; }

        //以下屬性為jqGrid的搜尋功能傳入
        public String searchField { get; set; }
        public String searchString { get; set; }
        public String searchOper { get; set; }
    }
    public abstract class SubQueryBase
    {
        public String id { get; set; }
        public String nd_ { get; set; }
    }
    #endregion
}
