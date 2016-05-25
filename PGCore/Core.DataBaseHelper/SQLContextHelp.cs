using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.SqlServerCe;
using System.Linq;
using MySql.Data.MySqlClient;

using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.NetExtension;
using ProcCore.DatabaseCore.TableFieldModule;

namespace ProcCore.DatabaseCore.SQLContextHelp
{
    static class SQLText
    {
        public static String MakeParm(String FieldName, WhereCompareType CompareType)
        {
            String tpl_SQL = String.Empty;

            switch (CompareType)
            {
                case WhereCompareType.Like:
                    tpl_SQL = "{0} like '%'+{1}+'%'"; break;
                case WhereCompareType.LikeRight:
                    tpl_SQL = "{0} like {1}+'%'"; break;
                case WhereCompareType.LikeLeft:
                    tpl_SQL = "{0} like '%'+{1}"; break;
                case WhereCompareType.Equel:
                    tpl_SQL = "{0}={1}"; break;
                case WhereCompareType.Than:
                    tpl_SQL = "{0}>{1}"; break;
                case WhereCompareType.Less:
                    tpl_SQL = "{0}<{1}"; break;
                case WhereCompareType.ThanEquel:
                    tpl_SQL = "{0}>={1}"; break;
                case WhereCompareType.LessEquel:
                    tpl_SQL = "{0}<={1}"; break;
                case WhereCompareType.UnEquel:
                    tpl_SQL = "{0}!={1}"; break;
                case WhereCompareType.Between:
                    tpl_SQL = "({0} Between {1} and {2})"; break;
                case WhereCompareType.NotBetween:
                    tpl_SQL = "(Not {0} Between {1} and {2})"; break;

            }

            if (CompareType == WhereCompareType.Between || CompareType == WhereCompareType.NotBetween)
                return String.Format(tpl_SQL, FieldName, "@" + FieldName + "1", "@" + FieldName + "2");
            else
                return String.Format(tpl_SQL, FieldName, "@" + FieldName);
        }
        public static String MakeParm(String FieldName, String FieldVar, WhereCompareType CompareType)
        {
            String tpl_SQL = String.Empty;
            switch (CompareType)
            {
                case WhereCompareType.Like:
                    tpl_SQL = "{0} like '%'+{1}+'%'"; break;
                case WhereCompareType.LikeRight:
                    tpl_SQL = "{0} like {1}+'%'"; break;
                case WhereCompareType.LikeLeft:
                    tpl_SQL = "{0} like '%'+{1}"; break;
                case WhereCompareType.Equel:
                    tpl_SQL = "{0}={1}"; break;
                case WhereCompareType.Than:
                    tpl_SQL = "{0}>{1}"; break;
                case WhereCompareType.Less:
                    tpl_SQL = "{0}<{1}"; break;
                case WhereCompareType.ThanEquel:
                    tpl_SQL = "{0}>={1}"; break;
                case WhereCompareType.LessEquel:
                    tpl_SQL = "{0}<={1}"; break;
                case WhereCompareType.UnEquel:
                    tpl_SQL = "{0}!={1}"; break;
                case WhereCompareType.Between:
                    tpl_SQL = "({0} Between {1} and {2})"; break;
                case WhereCompareType.NotBetween:
                    tpl_SQL = "(Not {0} Between {1} and {2})"; break;
            }

            if (CompareType == WhereCompareType.Between || CompareType == WhereCompareType.NotBetween)
                return String.Format(tpl_SQL, FieldName, "@" + FieldVar + "1", "@" + FieldVar + "2");
            else
                return String.Format(tpl_SQL, FieldName, "@" + FieldVar);
        }
        public static String MakeParmIn(String FieldName, int ValuesCount)
        {
            String tpl_SQL = String.Empty;
            String HandleParmName = String.Empty;

            List<String> ls_p = new List<String>();

            for (int i = 1; i <= ValuesCount; i++)
            {
                ls_p.Add(String.Format("@{0}{1}", FieldName, i));
            }

            tpl_SQL = String.Format("{0} IN ({1})", FieldName, ls_p.ToArray().JoinArray(","));

            HandleParmName = "@" + FieldName;

            return String.Format(tpl_SQL, FieldName, HandleParmName, "");
        }
        public static String MakeParmIn(String FieldName, String ldot,String rldot,int ValuesCount)
        {
            String tpl_SQL = String.Empty;
            String HandleParmName = String.Empty;

            List<String> ls_p = new List<String>();

            for (int i = 1; i <= ValuesCount; i++)
            {
                ls_p.Add(String.Format("@{0}{1}", FieldName, i));
            }

            tpl_SQL = String.Format("{2}{0}{3} IN ({1})", FieldName, ls_p.ToArray().JoinArray(","),ldot,rldot);

            HandleParmName = "@" + FieldName;

            return String.Format(tpl_SQL, FieldName, HandleParmName, "");
        }
    }

    public class TableLite {

        private CommConnection _Connection;
        private ConnectionType _ConnType;

        private SqlDataAdapter _SqlDataAdapter = null;
        private SqlCeDataAdapter _CEDataAdapter = null;
        private OleDbDataAdapter _OleDbDataAdapter;
        private MySqlDataAdapter _MySqlDataAdapter;


        public TableLite(CommConnection conn,String sql)
        {
            SQL = sql;
            _Connection = conn;
            _ConnType = _Connection.ConnectionType;

        }
        public Object DataAdapter
        {
            set
            {
                if (_ConnType == ConnectionType.SqlClient)
                {
                    _SqlDataAdapter = (SqlDataAdapter)value;
                }
                if (_ConnType == ConnectionType.OleDb)
                {
                    _OleDbDataAdapter = (OleDbDataAdapter)value;
                }
                if (_ConnType == ConnectionType.MySqlClient)
                {
                    _MySqlDataAdapter = (MySqlDataAdapter)value;
                }
                if (_ConnType == ConnectionType.SqlCE)
                {
                    _CEDataAdapter = (SqlCeDataAdapter)value;
                }
            }
        }
        public String SQL { get; set; }
        public m_Module[] DataByAdapter<m_Module>() where m_Module : new()
        {
            List<m_Module> L = new List<m_Module>();
            DataTable D=new DataTable();

            if (_ConnType == ConnectionType.SqlClient)
            {
                if (_SqlDataAdapter == null) 
                    _SqlDataAdapter = new SqlDataAdapter();

                _SqlDataAdapter.SelectCommand = new SqlCommand(SQL, (SqlConnection)this._Connection.Connection,(SqlTransaction)_Connection.Transaction);
                _SqlDataAdapter.Fill(D);
            }
            if (_ConnType == ConnectionType.OleDb)
            {
                if (_OleDbDataAdapter == null)
                    _OleDbDataAdapter = new OleDbDataAdapter();

                _OleDbDataAdapter.SelectCommand = new OleDbCommand(SQL, (OleDbConnection)this._Connection.Connection, (OleDbTransaction)_Connection.Transaction);
                _OleDbDataAdapter.Fill(D);
            }
            if (_ConnType == ConnectionType.MySqlClient)
            {
                if (_MySqlDataAdapter == null)
                    _MySqlDataAdapter = new MySqlDataAdapter();

                _MySqlDataAdapter.SelectCommand = new MySqlCommand(SQL, (MySqlConnection)this._Connection.Connection,(MySqlTransaction)_Connection.Transaction);
                _MySqlDataAdapter.Fill(D);
            }
            if (_ConnType == ConnectionType.SqlCE)
            {
                if (_CEDataAdapter == null)
                    _CEDataAdapter = new SqlCeDataAdapter();

                _CEDataAdapter.SelectCommand = new SqlCeCommand(SQL, (SqlCeConnection)this._Connection.Connection, (SqlCeTransaction)_Connection.Transaction);
                _CEDataAdapter.Fill(D);
            }   


            foreach (DataRow R in D.Rows)
            {
                m_Module md = new m_Module();
                R.LiteDataToModule(md);
                L.Add(md);
            }
            return L.ToArray();
        }
    }
    public class TablePack<TM> where TM : TableMap<TM>, IDisposable, new()
    {
        public TablePack(CommConnection conn)
        {
            _Connection = conn;
            _ConnType = _Connection.ConnectionType;
            _T = new TM();
        }

        private List<String> jForm = new List<String>();
        private List<String> jSelect = new List<String>();
        private List<String> jWhere = new List<String>();

        public void TableJoin<TJ>(Expression<Func<TM, FieldModule>> fa, Expression<Func<TJ, FieldModule>> fb, JoinType jt, TJ t)
            where TJ : TableMap<TJ>, IDisposable
        {
            Func<TM, FieldModule> C1 = fa.Compile();
            FieldModule f1 = C1.Invoke(this._T);

            Func<TJ, FieldModule> C2 = fb.Compile();
            FieldModule f2 = C2.Invoke(t);

            if (t.SQLSelect != null)
                jSelect.Add(t.SQLSelect);

            jForm.Add(jt + " Join " + t.NameAs + " On " + _T.NameDot + f1.N + " = " + t.NameDot + f2.N);

            if (t.SQLWhere != null)
                jWhere.Add(t.SQLWhere);
        }

        #region variable define

        private TM _T;
        private String _UserID = String.Empty;

        private DataTable _DataTable;
        private DataRow _DataRow;

        private CommConnection _Connection;
        private ConnectionType _ConnType;

        private SqlDataAdapter _SqlDataAdapter = null;
        private SqlCommandBuilder _SqlCommandBuilder;

        private OleDbDataAdapter _OleDbDataAdapter;
        private OleDbCommandBuilder _OleDbCommandBuilder;

        private MySqlDataAdapter _MySqlDataAdapter;
        private MySqlCommandBuilder _MySqlCommandBuilder;

        private SqlCeDataAdapter _CEDataAdapter;
        private SqlCeCommandBuilder _CECommandBuilder;

        private List<String> _CollectSelectFields;
        private List<WhereFieldsObject> _CollectWhereFields;
        private List<String> _CollectOrderFields;
        private List<GroupFieldObject> _CollectGroupFields;

        #endregion
        #region function Area
        #region General Area
        public TM TableModule
        {
            set { _T = value; }
            get { return _T; }
        }

        public String TableAlias
        {
            get { return _T.Alias; }
            set { _T.Alias = value; }
        }
        public int InsertAutoFieldsID { get; set; }
        public int AffetCount { get; set; }
        public int LoginUserID { get; set; }
        public int LoginUnitID { get; set; }

        private void RaisDataAdapter()
        {
            if (_ConnType == ConnectionType.SqlClient)
            {
                if (_SqlDataAdapter == null) _SqlDataAdapter = new SqlDataAdapter("Select * From " + _T.N, (SqlConnection)_Connection.Connection);
                if (_SqlDataAdapter.SelectCommand == null) _SqlDataAdapter.SelectCommand = new SqlCommand();
                if ((SqlTransaction)_Connection.Transaction != null) _SqlDataAdapter.SelectCommand.Transaction = (SqlTransaction)_Connection.Transaction;
            }

            if (_ConnType == ConnectionType.SqlCE)
            {
                if (_CEDataAdapter == null) _CEDataAdapter = new SqlCeDataAdapter("Select * From " + _T.N, (SqlCeConnection)_Connection.Connection);
                if (_CEDataAdapter.SelectCommand == null) _CEDataAdapter.SelectCommand = new SqlCeCommand();
                if ((SqlCeTransaction)_Connection.Transaction != null) _CEDataAdapter.SelectCommand.Transaction = (SqlCeTransaction)_Connection.Transaction;
            }

            if (_ConnType == ConnectionType.OleDb)
            {
                if (_OleDbDataAdapter == null) _OleDbDataAdapter = new OleDbDataAdapter("Select * From " + _T.N, (OleDbConnection)_Connection.Connection);
                if (_OleDbDataAdapter.SelectCommand == null) _OleDbDataAdapter.SelectCommand = new OleDbCommand();
                if ((OleDbTransaction)_Connection.Transaction != null) _OleDbDataAdapter.SelectCommand.Transaction = (OleDbTransaction)_Connection.Transaction;
            }

            if (_ConnType == ConnectionType.MySqlClient)
            {
                if (_MySqlDataAdapter == null) _MySqlDataAdapter = new MySqlDataAdapter("Select * From " + _T.N, (MySqlConnection)_Connection.Connection);
                if (_MySqlDataAdapter.SelectCommand == null) _MySqlDataAdapter.SelectCommand = new MySqlCommand();
                if ((MySqlTransaction)_Connection.Transaction != null) _MySqlDataAdapter.SelectCommand.Transaction = (MySqlTransaction)_Connection.Transaction;
            }

        }
        private void RaisDataTable()
        {
            if (_DataTable == null) _DataTable = new DataTable();

            if (_ConnType == ConnectionType.SqlClient)
                _SqlDataAdapter.FillSchema(_DataTable, SchemaType.Source);

            if (_ConnType == ConnectionType.SqlCE)
                _CEDataAdapter.FillSchema(_DataTable, SchemaType.Source);

            if (_ConnType == ConnectionType.OleDb)
                _OleDbDataAdapter.FillSchema(_DataTable, SchemaType.Source);

            if (_ConnType == ConnectionType.MySqlClient)
                _MySqlDataAdapter.FillSchema(_DataTable,SchemaType.Source);
        }

        public DataTable DataTable
        {
            get
            {
                return _DataTable;
            }
            set
            {
                _DataTable = value;
            }
        }

        public Object DataAdapter
        {
            set
            {
                if (_ConnType == ConnectionType.SqlClient)
                {
                    _SqlDataAdapter = (SqlDataAdapter)value;
                }
                if (_ConnType == ConnectionType.OleDb)
                {
                    _OleDbDataAdapter = (OleDbDataAdapter)value;
                }
                if (_ConnType == ConnectionType.MySqlClient)
                {
                    _MySqlDataAdapter = (MySqlDataAdapter)value;
                }
                if (_ConnType == ConnectionType.SqlCE)
                {
                    _CEDataAdapter = (SqlCeDataAdapter)value;
                }
            }
        }

        public void NewRow()
        {
            RaisDataAdapter();
            RaisDataTable();

            _DataRow = _DataTable.NewRow();
        }

        public void EditFirstRow()
        {
            _DataRow = _DataTable.Rows[0];
        }
        public void GoToRow(int i)
        {
            _DataRow = _DataTable.Rows[i];
        }
        public void GoToRow(DataRow r)
        {
            foreach (DataRow dr in _DataTable.Rows)
                if (dr.Equals(r))
                    _DataRow = dr;
        }

        public int RowCount()
        {
            return _DataTable.Rows.Count;
        }

        public void AddRow()
        {
            _DataTable.Rows.Add(_DataRow);
        }
        public void DeleteAll()
        {
            foreach (DataRow dr in _DataTable.Rows)
            {
                dr.Delete();
            }
        }
        public void DeleteNowRow()
        {
            _DataRow.Delete();
        }
        /// <summary>
        /// 使用此功能，Module的主key欄位一定要有值，否則會引發錯誤。
        /// </summary>
        /// <typeparam name="m_Module"></typeparam>
        /// <param name="md"></param>
        public void ModifyNowRow<m_Module>(m_Module md)
        {
            PropertyInfo[] GetPropertyInfos = md.GetType().GetProperties();
            foreach (var GetPropInfo in GetPropertyInfos)
                if (_DataTable.Columns.Contains(GetPropInfo.Name))
                    _DataRow[GetPropInfo.Name] = GetPropInfo.GetValue(md, null);
        }

        public Boolean GetDataRowByKeyHasData()
        {
            GetDataRowByKeyFields();
            return _DataRow == null ? false : true;
        }
        private void GetDataRowByKeyFields()
        {
            List<Object> l = new List<object>();
            List<DataColumn> c = new List<DataColumn>();

            foreach (var FModule in _T.KeyFieldModules)
            {
                l.Add(FModule.Value.V);
                c.Add(_DataTable.Columns[FModule.Value.N]);
            }

            _DataTable.PrimaryKey = c.ToArray();
            _DataRow = this._DataTable.Rows.Find(l.ToArray());
        }

        /// <summary>
        /// 設定資料集中某欄位的值都為value
        /// </summary>
        /// <param name="FieldName">要異動的欄位</param>
        /// <param name="value">設定值</param>
        public void SetAllRowValue(String FieldName, object value)
        {
            foreach (DataRow dr in _DataTable.Rows)
            {
                _DataRow = dr;
                _DataRow[FieldName] = value;
            }
        }

        /// <summary>
        /// 設定系統欄位=>異動欄位為true
        /// </summary>
        public void SetTransFieldToTrue()
        {
            SetAllRowValue("_異動", true);
        }
        public void SetTransFieldToFalse()
        {
            SetAllRowValue("_異動", false);
        }
        public void SetTransFieldToDelete()
        {
            foreach (DataRow dr in _DataTable.Rows)
            {
                if ((Boolean)dr["_異動"] == true)
                    dr.Delete();
            }
        }

        public void Reset()
        {
            this.TopLimit = 0;
            if (_CollectSelectFields != null) _CollectSelectFields.Clear();
            if (_CollectWhereFields != null) _CollectWhereFields.Clear();
            if (_CollectOrderFields != null) _CollectOrderFields.Clear();
            if (_CollectGroupFields != null) _CollectGroupFields.Clear();

            _DataTable.Dispose();
            _DataTable = null;

            #region 連線類型設定
            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.Dispose();
                _SqlDataAdapter = null;
                if (_SqlCommandBuilder != null) _SqlCommandBuilder.Dispose();
                _SqlCommandBuilder = null;
            }

            if (_ConnType == ConnectionType.OleDb)
            {
                _OleDbDataAdapter.Dispose();
                _OleDbDataAdapter = null;

                if (_OleDbCommandBuilder != null) _OleDbCommandBuilder.Dispose();
                _OleDbCommandBuilder = null;
            }

            if (_ConnType == ConnectionType.MySqlClient)
            {
                _MySqlDataAdapter.Dispose();
                _MySqlDataAdapter = null;

                if (_MySqlCommandBuilder != null) _MySqlCommandBuilder.Dispose();
                _MySqlCommandBuilder = null;
            }
            #endregion
        }
        /// <summary>
        /// 使用Select功能
        /// </summary>

        public void SetDataRowValue(Func<TM, FieldModule> FieldObj, Object FieldValue)
        {
            var MakeFieldObj = FieldObj.Invoke(this._T);
            _DataRow[MakeFieldObj.N] = FieldValue == null ? DBNull.Value : FieldValue;
        }
        public void UpdateFieldsInfo(UpdateFieldsInfoType t)
        {
            if (t == UpdateFieldsInfoType.Insert)
            {
                _DataRow["_新增人員"] = LoginUserID;
                _DataRow["_新增單位"] = LoginUnitID;
                _DataRow["_新增日期"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Update)
            {
                _DataRow["_修改人員"] = LoginUserID;
                _DataRow["_修改單位"] = LoginUnitID;
                _DataRow["_修改日期"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Both)
            {
                _DataRow["_新增人員"] = LoginUserID;
                _DataRow["_新增單位"] = LoginUnitID;
                _DataRow["_新增日期"] = DateTime.Now;
                _DataRow["_修改人員"] = LoginUserID;
                _DataRow["_修改單位"] = LoginUnitID;
                _DataRow["_修改日期"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Lock)
            {
                _DataRow["_LockUserID"] = LoginUserID;
                _DataRow["_LockDateTime"] = DateTime.Now;
                _DataRow["_LockState"] = true;
            }

            if (t == UpdateFieldsInfoType.UnLock)
            {
                _DataRow["_LockUserID"] = DBNull.Value;
                _DataRow["_LockDateTime"] = DBNull.Value;
                _DataRow["_LockState"] = false;
            }
            if (t == UpdateFieldsInfoType.Lang)
                _DataRow["_語系"] = System.Globalization.CultureInfo.CurrentCulture.Name;

            if (t == UpdateFieldsInfoType.TranFalse)
                _DataRow["_異動"] = false;

            if (t == UpdateFieldsInfoType.TranTrue)
                _DataRow["_異動"] = true;
        }

        public void WhereFilterDataLock()
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = "_LockState";
            w.WhereCompareStyle = WhereCompareType.Equel;
            w.SQLDataType = SQLValueType.Boolean;
            w.ValueA = 0;
            this._CollectWhereFields.Add(w);
        }

        /// <summary>
        /// 需依語系讀取
        /// </summary>
        public void WhereLang()
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFields("_語系", System.Globalization.CultureInfo.CurrentCulture.Name, WhereCompareType.Equel);
        }

        private void BindAdapterSQLEvent()
        {
            RaisDataAdapter();

            if (_ConnType == ConnectionType.SqlClient)
            {
                #region MyRegion
                _SqlCommandBuilder = new SqlCommandBuilder(_SqlDataAdapter);
                _SqlDataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(SQLServerOnRowUpdated);

                SqlParameter SqlParm = new SqlParameter("@id", SqlDbType.Int);
                SqlParm.Direction = ParameterDirection.Output;

                SqlCommand cmd = _SqlCommandBuilder.GetInsertCommand();
                cmd.Connection = (SqlConnection)_Connection.Connection;
                cmd.CommandText += " ;SET @id = SCOPE_IDENTITY()";
                cmd.Parameters.Add(SqlParm);

                _SqlDataAdapter.InsertCommand = cmd;
                _SqlDataAdapter.UpdateCommand = _SqlCommandBuilder.GetUpdateCommand();
                _SqlDataAdapter.DeleteCommand = _SqlCommandBuilder.GetDeleteCommand();

                if (_Connection.Transaction != null)
                {
                    _SqlDataAdapter.InsertCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                    _SqlDataAdapter.UpdateCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                    _SqlDataAdapter.DeleteCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                }

                //_SqlCommandBuilder.Dispose();
                #endregion
            }

            if (_ConnType == ConnectionType.SqlCE)
            {
                #region MyRegion
                _CECommandBuilder = new SqlCeCommandBuilder(_CEDataAdapter);
                _CEDataAdapter.RowUpdated += new SqlCeRowUpdatedEventHandler(CEServerOnRowUpdated);

                //SqlCeParameter SqlParm = new SqlCeParameter("@id", SqlDbType.Int);
                //SqlParm.Direction = ParameterDirection.Output;

                SqlCeCommand cmd = _CECommandBuilder.GetInsertCommand();
                //cmd.Connection = (SqlCeConnection)_Connection.Connection;
                //cmd.CommandText += " ;SET @id = SCOPE_IDENTITY()";
                //cmd.Parameters.Add(SqlParm);

                _CEDataAdapter.InsertCommand = cmd;
                _CEDataAdapter.UpdateCommand = _CECommandBuilder.GetUpdateCommand();
                _CEDataAdapter.DeleteCommand = _CECommandBuilder.GetDeleteCommand();

                if (_Connection.Transaction != null)
                {
                    _CEDataAdapter.InsertCommand.Transaction = (SqlCeTransaction)_Connection.Transaction;
                    _CEDataAdapter.UpdateCommand.Transaction = (SqlCeTransaction)_Connection.Transaction;
                    _CEDataAdapter.DeleteCommand.Transaction = (SqlCeTransaction)_Connection.Transaction;
                }
                #endregion
            }

            if (_ConnType == ConnectionType.OleDb)
            {
                #region MyRegion
                
                _OleDbCommandBuilder = new OleDbCommandBuilder(_OleDbDataAdapter);
                _OleDbCommandBuilder.QuotePrefix = "[";
                _OleDbCommandBuilder.QuoteSuffix = "]";

                //OleDbParameter SqlParm = new OleDbParameter("@id", OleDbType.Integer);
                //SqlParm.Direction = ParameterDirection.Output;
                //OleDbCommand.Prepare 方法需要所有的參數都明確地設定型別。
                //var cmd = _OleDbCommandBuilder.GetInsertCommand();
                //cmd.Connection = (OleDbConnection)_Connection.Connection;
                //cmd.CommandText += " ;SET @id = SCOPE_IDENTITY()";
                //cmd.Parameters.Add(SqlParm);

                _OleDbDataAdapter.InsertCommand = _OleDbCommandBuilder.GetInsertCommand();
                _OleDbDataAdapter.UpdateCommand = _OleDbCommandBuilder.GetUpdateCommand();
                _OleDbDataAdapter.DeleteCommand = _OleDbCommandBuilder.GetDeleteCommand();

                if (_Connection.Transaction != null)
                {
                    _OleDbDataAdapter.InsertCommand.Transaction = (OleDbTransaction)_Connection.Transaction;
                    _OleDbDataAdapter.UpdateCommand.Transaction = (OleDbTransaction)_Connection.Transaction;
                    _OleDbDataAdapter.DeleteCommand.Transaction = (OleDbTransaction)_Connection.Transaction;
                }

                //_OleDbCommandBuilder.Dispose();
                #endregion
            }

            if (_ConnType == ConnectionType.MySqlClient)
            {
                #region MyRegion
                _MySqlCommandBuilder = new MySqlCommandBuilder(_MySqlDataAdapter);
                _MySqlDataAdapter.RowUpdated += new MySqlRowUpdatedEventHandler(MySQLOnRowUpdated);
                //MySqlParameter SqlParm = new MySqlParameter("@id",MySqlDbType.Int32);
                //SqlParm.Direction = ParameterDirection.Output;

                MySqlCommand cmd = _MySqlCommandBuilder.GetInsertCommand();
                cmd.Connection = (MySqlConnection)_Connection.Connection;
                //cmd.CommandText += " ;SET @id = SCOPE_IDENTITY()";
                //cmd.Parameters.Add(SqlParm);

                _MySqlDataAdapter.InsertCommand = cmd;
                _MySqlDataAdapter.UpdateCommand = _MySqlCommandBuilder.GetUpdateCommand();
                _MySqlDataAdapter.DeleteCommand = _MySqlCommandBuilder.GetDeleteCommand();

                if (_Connection.Transaction != null)
                {
                    _MySqlDataAdapter.InsertCommand.Transaction = (MySqlTransaction)_Connection.Transaction;
                    _MySqlDataAdapter.UpdateCommand.Transaction = (MySqlTransaction)_Connection.Transaction;
                    _MySqlDataAdapter.DeleteCommand.Transaction = (MySqlTransaction)_Connection.Transaction;
                }

                //_MySqlCommandBuilder.Dispose();
                #endregion
            }
        }
        public void UpdateDataAdapter()
        {
            BindAdapterSQLEvent();

            if (_ConnType == ConnectionType.SqlClient)
                this.AffetCount = _SqlDataAdapter.Update(_DataTable);

            if (_ConnType == ConnectionType.SqlCE)
                this.AffetCount = _CEDataAdapter.Update(_DataTable);

            if (_ConnType == ConnectionType.OleDb)
                this.AffetCount = _OleDbDataAdapter.Update(_DataTable);

            if (_ConnType == ConnectionType.MySqlClient)
                this.AffetCount = _MySqlDataAdapter.Update(_DataTable);

            _DataTable.AcceptChanges();
        }
        public void UpdateDataAdapter(int BathSize)
        {
            BindAdapterSQLEvent();

            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.UpdateBatchSize = BathSize;
                this.AffetCount = _SqlDataAdapter.Update(_DataTable);
            }

            if (_ConnType == ConnectionType.SqlCE)
            {

                _CEDataAdapter.UpdateBatchSize = BathSize;
                this.AffetCount = _CEDataAdapter.Update(_DataTable);
            }

            if (_ConnType == ConnectionType.OleDb)
            {
                _OleDbDataAdapter.UpdateBatchSize = BathSize;
                this.AffetCount = _OleDbDataAdapter.Update(_DataTable);
            }

            if (_ConnType == ConnectionType.MySqlClient)
            {
                _MySqlDataAdapter.UpdateBatchSize = BathSize;
                this.AffetCount = _MySqlDataAdapter.Update(_DataTable);
            }

            _DataTable.AcceptChanges();
        }
        protected void SQLServerOnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {

            //檢查是否有自動編號欄位
            if (args.Status == UpdateStatus.Continue)
            {
                if (args.StatementType == StatementType.Insert)
                {
                    //Boolean CheckHasAutoIdentify = false; ;

                    String LastName = String.Empty;
                    foreach (DataColumn dc in args.Row.Table.Columns)
                    {
                        if (dc.AutoIncrement == true)
                        {
                            //CheckHasAutoIdentify = true;
                            LastName = dc.ColumnName;
                        }
                    }
                    //if (CheckHasAutoIdentify)
                    //    InsertAutoFieldsID = (int)_SqlDataAdapter.InsertCommand.Parameters["@" + LastName + ""].Value;
                }
            }
        }
        protected void CEServerOnRowUpdated(object sender, SqlCeRowUpdatedEventArgs args)
        {

            //檢查是否有自動編號欄位
            if (args.Status == UpdateStatus.Continue)
            {
                if (args.StatementType == StatementType.Insert)
                {
                    //Boolean CheckHasAutoIdentify = false; ;

                    String LastName = String.Empty;
                    foreach (DataColumn dc in args.Row.Table.Columns)
                    {
                        if (dc.AutoIncrement == true)
                        {
                            //CheckHasAutoIdentify = true;
                            LastName = dc.ColumnName;
                        }
                    }
                    //if (CheckHasAutoIdentify)
                    //    InsertAutoFieldsID = (int)_SqlDataAdapter.InsertCommand.Parameters["@" + LastName + ""].Value;
                }
            }
        }
        protected void MySQLOnRowUpdated(object sender, MySqlRowUpdatedEventArgs args) { 
        }

        #endregion
        #region Select Area

        public int TopLimit { get; set; }

        public void SelectFields(Expression<Func<TM, FieldModule>> F)
        {
            Func<TM, FieldModule> C = F.Compile();
            FieldModule M = C.Invoke(this._T);

            if (this._CollectSelectFields == null)
                this._CollectSelectFields = new List<String>();

            this._CollectSelectFields.Add(M.NameAs);
        }
        public void SelectFields<TResult>(Expression<Func<TM, TResult>> F)
        {
            Func<TM, TResult> CompileFieldObj = F.Compile();
            TResult MakeFieldObj = CompileFieldObj.Invoke(this._T);

            if (this._CollectSelectFields == null)
                this._CollectSelectFields = new List<String>();

            if (F.Body.NodeType == ExpressionType.New)
            {
                PropertyInfo[] TypePropertys = MakeFieldObj.GetType().GetProperties();
                foreach (PropertyInfo Property in TypePropertys)
                {
                    FieldModule FObj = (FieldModule)Property.GetValue(MakeFieldObj, null);
                    this._CollectSelectFields.Add(FObj.NameAs);
                }
            }

            if (F.Body.NodeType == ExpressionType.MemberAccess)
            {
                PropertyInfo TypeProperty = MakeFieldObj.GetType().GetProperty("N");
                String GetName = TypeProperty.GetValue(MakeFieldObj, null).ToString();
                this._CollectSelectFields.Add(GetName);
            }
        }

        /// <summary>
        /// 聚合Function
        /// </summary>
        /// <param name="FieldObj"></param>
        /// <param name="aggregate"></param>
        public void AggregateFields(Func<TM, FieldModule> FieldObj, String asName, AggregateType aggregate)
        {
            //var CompileFieldObj = FieldObj.Compile();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            GroupFieldObject g = new GroupFieldObject() { AggregateType = aggregate, AsName = asName, FieldName = MakeFieldObj.N };

            if (this._CollectGroupFields == null)
            {
                this._CollectGroupFields = new List<GroupFieldObject>();
            }
            this._CollectGroupFields.Add(g);
        }

        public void WhereFields(String FieldObj, Object FieldValue, WhereCompareType WhereType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = FieldObj;
            w.FieldVar = FieldObj;
            w.WhereCompareStyle = WhereType;
            w.ValueA = FieldValue;
            w.whereLogic = WhereLogicType.and;
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(String FieldObj, Object FieldValue, String FieldVar, WhereCompareType WhereType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = FieldObj;
            w.FieldVar = FieldVar;
            w.WhereCompareStyle = WhereType;
            w.ValueA = FieldValue;
            w.whereLogic = WhereLogicType.and;
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValue)
        {
            WhereFields(FieldObj, FieldValue, WhereCompareType.Equel);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType)
        {
            WhereFields(FieldObj, FieldValue, WhereType, null);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType, WhereLogicType logType)
        {
            WhereFields(FieldObj, FieldValue, WhereType, null, logType);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType, String FieldVar)
        {
            WhereFields(FieldObj, FieldValue, WhereType, FieldVar, WhereLogicType.and);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType, String FieldVar, WhereLogicType logType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.FieldVar = FieldVar.NullValue(MakeFieldObj.N);

            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereType;
            w.ValueA = FieldValue;
            w.whereLogic = logType;
            this._CollectWhereFields.Add(w);
        }

        /// <summary>
        /// only between use
        /// </summary>
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValueA, Object FieldValueB, WhereCompareType WhereType, WhereLogicType logType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereType;
            w.FieldVar = MakeFieldObj.N;
            w.ValueA = FieldValueA;
            w.ValueB = FieldValueB;
            w.whereLogic = logType;
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object FieldValueA, Object FieldValueB, WhereCompareType WhereType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereType;
            w.FieldVar = MakeFieldObj.N;
            w.ValueA = FieldValueA;
            w.ValueB = FieldValueB;
            w.whereLogic = WhereLogicType.and;
            this._CollectWhereFields.Add(w);
        }

        /// <summary>
        /// Sql In Use
        /// </summary>
        public void WhereFields(Func<TM, FieldModule> FieldObj, Object[] FieldValues)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereCompareType.In;
            w.whereLogic = WhereLogicType.and;

            List<Object> ls_Value = new List<Object>();
            foreach (Object FieldValue in FieldValues)
                ls_Value.Add(FieldValue);

            w.Values = ls_Value.ToArray();
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(Func<TM, FieldModule> FieldObj, Int32[] FieldValues)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            //var CompileFieldObj = FieldObj.Compile();
            var MakeFieldObj = FieldObj.Invoke(this._T);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereCompareType.In;
            w.whereLogic = WhereLogicType.and;

            List<Object> ls_Value = new List<Object>();
            foreach (Int32 FieldValue in FieldValues)
                ls_Value.Add(FieldValue);

            w.Values = ls_Value.ToArray();
            this._CollectWhereFields.Add(w);
        }

        public void OrderByFields(Func<TM, FieldModule> FieldObj, OrderByType OrderType)
        {
            if (this._CollectOrderFields == null)
                this._CollectOrderFields = new List<String>();

            var get_Obj = FieldObj.Invoke(this._T);
            this._CollectOrderFields.Add(get_Obj.N + " " + OrderType);
        }
        public void OrderByFields(Func<TM, FieldModule> FieldObj)
        {
            OrderByFields(FieldObj, OrderByType.ASC);
        }

        #endregion
        #region Sql make and Data make
        public String TranSQLString()
        {
            String Sql = String.Empty;

            #region Handle Select
            if (this._CollectSelectFields == null)
                _CollectSelectFields = new List<String>();

            if (this._CollectSelectFields.Count == 0 && _CollectGroupFields == null)
                _CollectSelectFields.Add("*");

            List<String> ls_CombinSelect = new List<String>();
            if (_CollectSelectFields.Count > 0)
                ls_CombinSelect.Add(_CollectSelectFields.ToArray().JoinArray(",", _T.NameBlank, ""));

            foreach (var q in jSelect)
                ls_CombinSelect.Add(q);

            List<String> ls_AggregateFields = new List<String>();
            if (_CollectGroupFields != null)
            {
                foreach (GroupFieldObject GFields in _CollectGroupFields)
                    ls_AggregateFields.Add(GFields.AggregateType + "(" + GFields.FieldName + ") as " + GFields.AsName);

                ls_CombinSelect.Add(ls_AggregateFields.ToArray().JoinArray(",", _T.NameBlank));
            }
            _T.SQLSelect = ls_CombinSelect.ToArray().JoinArray(",");

            #endregion

            #region Handle Where

            List<String> ls_WhereSQL = new List<String>();

            if (_CollectWhereFields != null)
            {
                String rDot = String.Empty, lDot = String.Empty;
                if (_ConnType == ConnectionType.OleDb)
                {
                    lDot = "["; rDot = "]";
                }
                # region Handle Begin
                foreach (WhereFieldsObject obj in _CollectWhereFields)
                {
                    FieldModule WhereFieldMD = _T.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();
                    if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists：" + obj.FieldName);

                    if (obj.WhereCompareStyle == WhereCompareType.In) //In
                        ls_WhereSQL.Add(SQLText.MakeParmIn(obj.FieldName, lDot, rDot, obj.Values.Length));

                    else if (obj.WhereCompareStyle == WhereCompareType.Between || obj.WhereCompareStyle == WhereCompareType.NotBetween)
                        if (obj.whereLogic != WhereLogicType.none && !_CollectWhereFields.Last().Equals(obj))
                            ls_WhereSQL.Add(SQLText.MakeParm((_T.NameDot) + lDot + obj.FieldName + rDot, obj.FieldVar, obj.WhereCompareStyle) + " " + obj.whereLogic);
                        else
                            ls_WhereSQL.Add(SQLText.MakeParm((_T.NameDot) + lDot + obj.FieldName + rDot, obj.FieldVar, obj.WhereCompareStyle));

                    else  //一般比較式
                        if (obj.whereLogic != WhereLogicType.none && !_CollectWhereFields.Last().Equals(obj))
                            ls_WhereSQL.Add(SQLText.MakeParm((_T.NameDot) + lDot + obj.FieldName + rDot, obj.FieldVar, obj.WhereCompareStyle) + " " + obj.whereLogic);
                        else
                            ls_WhereSQL.Add(SQLText.MakeParm((_T.NameDot) + lDot + obj.FieldName + rDot, obj.FieldVar, obj.WhereCompareStyle));
                }
                #endregion
            }

            if (ls_WhereSQL.Count > 0)
                _T.SQLWhere = ls_WhereSQL.ToArray().JoinArray(" ", " ");

            #endregion

            #region Handle GroupBy
            if (_CollectGroupFields != null)
            {
                if (_CollectGroupFields.Count > 0)
                {
                    var result = from CollectHaveGroupFields in _CollectSelectFields
                                 where !_CollectGroupFields.Select(x => x.FieldName).Contains(CollectHaveGroupFields)
                                 select CollectHaveGroupFields;

                    if (result.Count() > 0)
                        _T.SQLGroupBy = result.ToArray().JoinArray(",");
                }
            }

            #endregion

            #region Handle OrderBy
            if (_CollectOrderFields != null)
                if (_CollectOrderFields.Count > 0)
                    _T.SQLOrderBy = _CollectOrderFields.ToArray().JoinArray(",", _T.NameBlank, "");

            #endregion

            #region SQL String

            if (_ConnType == ConnectionType.SqlClient || _ConnType == ConnectionType.OleDb || _ConnType == ConnectionType.SqlCE)
                Sql = "Select " + (TopLimit > 0 ? "Top " + TopLimit + " " : "") + _T.SQLSelect + " From " + _T.NameAs + " " + jForm.ToArray().JoinArray(" ") + " ";

            if (_ConnType == ConnectionType.MySqlClient)
                Sql = "Select " + _T.SQLSelect + " From " + _T.NameAs + " " + jForm.ToArray().JoinArray(" ") + " ";

            //+ (TopLimit > 0 ? "Top " + TopLimit + " " : "") +

            if (_T.SQLWhere != null && _T.SQLWhere != "")
                Sql += "Where " + _T.SQLWhere + " ";

            if (_T.SQLGroupBy != null)
                Sql += "Group By " + _T.SQLGroupBy + " ";

            if (_T.SQLHaving != null)
                Sql += "Having " + _T.SQLHaving + " ";

            if (_T.SQLOrderBy != null)
                Sql += "Order By " + _T.SQLOrderBy;
            #endregion

            return Sql;
        }

        public DataTable DataByAdapter(SqlDataAdapter parmSqlAdp)
        {
            String Sql = TranSQLString();

            #region SQL Server Where Handle
            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter = _SqlDataAdapter == null ? new SqlDataAdapter() : _SqlDataAdapter;
                _SqlDataAdapter.SelectCommand = new SqlCommand();
                _SqlDataAdapter.SelectCommand.Connection = (SqlConnection)_Connection.Connection;
                _SqlDataAdapter.SelectCommand.Transaction = (SqlTransaction)_Connection.Transaction;

                if (_CollectWhereFields != null)
                {
                    # region Handle Begin
                    foreach (WhereFieldsObject obj in _CollectWhereFields)
                    {
                        #region 型態配置
                        SqlDbType t = SqlDbType.Int;

                        FieldModule WhereFieldMD = _T.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();

                        if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists");

                        if (WhereFieldMD.T == SQLValueType.Int) t = SqlDbType.Int;
                        if (WhereFieldMD.T == SQLValueType.String) t = SqlDbType.NVarChar;
                        if (WhereFieldMD.T == SQLValueType.DateTime) t = SqlDbType.DateTime;
                        if (WhereFieldMD.T == SQLValueType.Boolean) t = SqlDbType.Bit;
                        #endregion

                        if (obj.WhereCompareStyle == WhereCompareType.In)
                        {
                            #region 處理In
                            for (int i = 1; i <= obj.Values.Length; i++)
                            {
                                String ParamName = "@" + obj.FieldName.Trim() + i.ToString().Trim();
                                SqlParameter Parm = new SqlParameter(ParamName, t);
                                Parm.Value = obj.Values[i - 1];
                                _SqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                            }
                            #endregion
                        }
                        else if (obj.WhereCompareStyle == WhereCompareType.Between || obj.WhereCompareStyle == WhereCompareType.NotBetween)
                        {
                            #region 處理 Between
                            SqlParameter ParmA = new SqlParameter("@" + obj.FieldVar + "1", t);
                            ParmA.Value = obj.ValueA;
                            _SqlDataAdapter.SelectCommand.Parameters.Add(ParmA);

                            SqlParameter ParmB = new SqlParameter("@" + obj.FieldVar + "2", t);
                            ParmB.Value = obj.ValueB;
                            _SqlDataAdapter.SelectCommand.Parameters.Add(ParmB);
                            #endregion
                        }
                        else
                        {
                            #region 一般比較式
                            SqlParameter Parm = new SqlParameter("@" + obj.FieldVar, t);
                            Parm.Value = obj.ValueA;
                            _SqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region SQL Server CE Where Handle
            if (_ConnType == ConnectionType.SqlCE)
            {
                _CEDataAdapter = _CEDataAdapter == null ? new SqlCeDataAdapter() : _CEDataAdapter;
                _CEDataAdapter.SelectCommand = new SqlCeCommand();
                _CEDataAdapter.SelectCommand.Connection = (SqlCeConnection)_Connection.Connection;
                _CEDataAdapter.SelectCommand.Transaction = (SqlCeTransaction)_Connection.Transaction;

                if (_CollectWhereFields != null)
                {
                    # region Handle Begin
                    foreach (WhereFieldsObject obj in _CollectWhereFields)
                    {
                        #region 型態配置
                        SqlDbType t = SqlDbType.Int;

                        FieldModule WhereFieldMD = _T.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();

                        if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists");

                        if (WhereFieldMD.T == SQLValueType.Int) t = SqlDbType.Int;
                        if (WhereFieldMD.T == SQLValueType.String) t = SqlDbType.NVarChar;
                        if (WhereFieldMD.T == SQLValueType.DateTime) t = SqlDbType.DateTime;
                        if (WhereFieldMD.T == SQLValueType.Boolean) t = SqlDbType.Bit;
                        #endregion

                        if (obj.WhereCompareStyle == WhereCompareType.In)
                        {
                            #region 處理In
                            for (int i = 1; i <= obj.Values.Length; i++)
                            {
                                String ParamName = "@" + obj.FieldName.Trim() + i.ToString().Trim();
                                SqlCeParameter Parm = new SqlCeParameter(ParamName, t);
                                Parm.Value = obj.Values[i - 1];
                                _CEDataAdapter.SelectCommand.Parameters.Add(Parm);
                            }
                            #endregion
                        }
                        else if (obj.WhereCompareStyle == WhereCompareType.Between || obj.WhereCompareStyle == WhereCompareType.NotBetween)
                        {
                            #region 處理 Between
                            SqlCeParameter ParmA = new SqlCeParameter("@" + obj.FieldVar + "1", t);
                            ParmA.Value = obj.ValueA;
                            _CEDataAdapter.SelectCommand.Parameters.Add(ParmA);

                            SqlCeParameter ParmB = new SqlCeParameter("@" + obj.FieldVar + "2", t);
                            ParmB.Value = obj.ValueB;
                            _CEDataAdapter.SelectCommand.Parameters.Add(ParmB);
                            #endregion
                        }
                        else
                        {
                            #region 一般比較式
                            SqlCeParameter Parm = new SqlCeParameter("@" + obj.FieldVar, t);
                            Parm.Value = obj.ValueA;
                            _CEDataAdapter.SelectCommand.Parameters.Add(Parm);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region OleDB Where Handle
            if (_ConnType == ConnectionType.OleDb)
            {
                _OleDbDataAdapter = _OleDbDataAdapter == null ? new OleDbDataAdapter() : _OleDbDataAdapter;
                _OleDbDataAdapter.SelectCommand = new OleDbCommand();
                _OleDbDataAdapter.SelectCommand.Connection = (OleDbConnection)_Connection.Connection;
                _OleDbDataAdapter.SelectCommand.Transaction = (OleDbTransaction)_Connection.Transaction;

                if (_CollectWhereFields != null)
                {
                    # region Handle Begin
                    foreach (WhereFieldsObject obj in _CollectWhereFields)
                    {
                        #region 型態配置
                        OleDbType t = OleDbType.Integer;

                        FieldModule WhereFieldMD = _T.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();

                        if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists");

                        if (WhereFieldMD.T == SQLValueType.Int) t = OleDbType.Integer;
                        if (WhereFieldMD.T == SQLValueType.String) t = OleDbType.VarChar;
                        if (WhereFieldMD.T == SQLValueType.DateTime) t = OleDbType.Date;
                        if (WhereFieldMD.T == SQLValueType.Boolean) t = OleDbType.Boolean;
                        #endregion

                        if (obj.WhereCompareStyle == WhereCompareType.In)
                        {
                            #region 處理In
                            for (int i = 1; i <= obj.Values.Length; i++)
                            {
                                String ParamName = "@" + obj.FieldName.Trim() + i.ToString().Trim();
                                OleDbParameter Parm = new OleDbParameter(ParamName,t);
                                Parm.Value = obj.Values[i - 1];
                                _OleDbDataAdapter.SelectCommand.Parameters.Add(Parm);
                            }
                            #endregion
                        }
                        else if (obj.WhereCompareStyle == WhereCompareType.Between || obj.WhereCompareStyle == WhereCompareType.NotBetween)
                        {
                            #region 處理 Between
                            OleDbParameter ParmA = new OleDbParameter("@" + obj.FieldVar + "1", t);
                            ParmA.Value = obj.ValueA;
                            _OleDbDataAdapter.SelectCommand.Parameters.Add(ParmA);

                            OleDbParameter ParmB = new OleDbParameter("@" + obj.FieldVar + "2", t);
                            ParmB.Value = obj.ValueB;
                            _OleDbDataAdapter.SelectCommand.Parameters.Add(ParmB);
                            #endregion
                        }
                        else
                        {
                            #region 一般比較式
                            OleDbParameter Parm = new OleDbParameter("@" + obj.FieldVar, t);
                            Parm.Value = obj.ValueA;
                            _OleDbDataAdapter.SelectCommand.Parameters.Add(Parm);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region MySQL Where Handle
            if (_ConnType == ConnectionType.MySqlClient)
            {
                _MySqlDataAdapter = _MySqlDataAdapter == null ? new MySqlDataAdapter() : _MySqlDataAdapter;
                _MySqlDataAdapter.SelectCommand = new MySqlCommand();
                _MySqlDataAdapter.SelectCommand.Connection = (MySqlConnection)_Connection.Connection;
                _MySqlDataAdapter.SelectCommand.Transaction = (MySqlTransaction)_Connection.Transaction;

                if (_CollectWhereFields != null)
                {
                    # region Handle Begin
                    foreach (WhereFieldsObject obj in _CollectWhereFields)
                    {
                        #region 型態配置
                        MySqlDbType t = MySqlDbType.Int32;

                        FieldModule WhereFieldMD = _T.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();

                        if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists");

                        if (WhereFieldMD.T == SQLValueType.Int) t = MySqlDbType.Int32;
                        if (WhereFieldMD.T == SQLValueType.String) t = MySqlDbType.VarChar;
                        if (WhereFieldMD.T == SQLValueType.DateTime) t = MySqlDbType.DateTime;
                        if (WhereFieldMD.T == SQLValueType.Boolean) t = MySqlDbType.Bit;
                        #endregion

                        if (obj.WhereCompareStyle == WhereCompareType.In)
                        {
                            #region 處理In
                            for (int i = 1; i <= obj.Values.Length; i++)
                            {
                                String ParamName = "@" + obj.FieldName.Trim() + i.ToString().Trim();
                                MySqlParameter Parm = new MySqlParameter(ParamName,t );
                                Parm.Value = obj.Values[i - 1];
                                _MySqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                            }
                            #endregion
                        }
                        else if (obj.WhereCompareStyle == WhereCompareType.Between || obj.WhereCompareStyle == WhereCompareType.NotBetween)
                        {
                            #region 處理 Between
                            MySqlParameter ParmA = new MySqlParameter("@" + obj.FieldVar + "1", t);
                            ParmA.Value = obj.ValueA;
                            _MySqlDataAdapter.SelectCommand.Parameters.Add(ParmA);

                            MySqlParameter ParmB = new MySqlParameter("@" + obj.FieldVar + "2", t);
                            ParmB.Value = obj.ValueB;
                            _MySqlDataAdapter.SelectCommand.Parameters.Add(ParmB);
                            #endregion
                        }
                        else
                        {
                            #region 一般比較式
                            MySqlParameter Parm = new MySqlParameter("@" + obj.FieldVar, t);
                            Parm.Value = obj.ValueA;
                            _MySqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region Release Object
            if (_CollectGroupFields != null)
            {
                _CollectGroupFields.Clear();
                _CollectGroupFields = null;
            }

            if (_CollectSelectFields != null)
            {
                _CollectSelectFields.Clear();
                _CollectSelectFields = null;
            }

            if (_CollectOrderFields != null)
            {
                _CollectOrderFields.Clear();
                _CollectOrderFields = null;
            }
            #endregion

            #region 依連線類型做Fill DataTable
            if (_DataTable == null) _DataTable = new DataTable(_T.N);

            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.SelectCommand.CommandText = Sql;
                _SqlDataAdapter.Fill(_DataTable);
            }

            if (_ConnType == ConnectionType.SqlCE)
            {
                _CEDataAdapter.SelectCommand.CommandText = Sql;
                _CEDataAdapter.Fill(_DataTable);
            }

            if (_ConnType == ConnectionType.OleDb)
            {
                _OleDbDataAdapter.SelectCommand.CommandText = Sql;
                _OleDbDataAdapter.Fill(_DataTable);
            }
            if (_ConnType == ConnectionType.MySqlClient)
            {
                if (TopLimit > 0) Sql += " Limit " + TopLimit;

                _MySqlDataAdapter.SelectCommand.CommandText = Sql;
                _MySqlDataAdapter.Fill(_DataTable);
            }

            #endregion

            Sql = String.Empty; //清空Sql

            return _DataTable;
        }

        public DataTable DataByAdapter()
        {
            return DataByAdapter(null);
        }

        public m_Module[] DataByAdapter<m_Module>() where m_Module : new()
        {
            List<m_Module> L = new List<m_Module>();

            DataTable D = DataByAdapter();

            foreach (DataRow R in D.Rows)
            {
                m_Module md = new m_Module();
                R.LoadDataToModule<TM>(md, this._T);
                L.Add(md);
            }
            return L.ToArray();
        }

        /// <summary>
        /// 使用此項功能要先給定 DataTableWorking.Table.KeyFieldModules[key].V 值
        /// </summary>
        public DataTable GetDataByKey()
        {
            String[] cols = _T.KeyFieldModules.Select(x => x.Value).Select(x => x.N).ToArray();
            foreach (String col in cols)
                WhereFields(col, _T.KeyFieldModules[col].V, WhereCompareType.Equel);

            return DataByAdapter(null);
        }
        public m_Module GetDataByKey<m_Module>()
            where m_Module : new()
        {
            String[] cols = _T.KeyFieldModules.Select(x => x.Value).Select(x => x.N).ToArray();
            foreach (String col in cols)
                WhereFields(col, _T.KeyFieldModules[col].V, WhereCompareType.Equel);

            return DataByAdapter<m_Module>().Single();
        }
        #endregion
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Reset();
            }
        }
        #endregion

        #endregion
    }
    public enum AggregateType
    {
        AVG,
        CHECKSUM_AGG,
        COUNT,
        COUNT_BIG,
        GROUPING,
        GROUPING_ID,
        MAX,
        MIN,
        STDEV,
        STDEVP,
        SUM,
        VAR,
        VARP
    }
    public enum JoinType
    {
        None, Left, Right, Inner, Cross
    }
    public class FieldsObject
    {
        public String FieldName { get; set; }
        public String TableAliasdName { get; set; }
        public String AsName { get; set; }
    }
    public class JoinObject
    {

        public String SQLSelect { get; set; }
        public String SQLWhere { get; set; }
        public String SQLOrderBy { get; set; }
        public String SQLGroupBy { get; set; }
        public String SQLHaving { get; set; }
    }
    public class WhereFieldsObject : FieldsObject
    {
        /// <summary>
        /// SQL @變數 預設是跟欄位名稱一樣
        /// </summary>
        public String FieldVar { get; set; }
        public Object ValueA { get; set; }
        /// <summary>
        /// SQL Between 會用到 第二變數 預設是 欄位名 + 1,欄位名 + 2
        /// </summary>
        public String FieldVarB { get; set; }
        public Object ValueB { get; set; }
        /// <summary>
        /// 主要For In在使用
        /// </summary>
        public Object[] Values { get; set; }
        public WhereCompareType WhereCompareStyle { get; set; }
        public SQLValueType SQLDataType { get; set; }

        /// <summary>
        /// 接在[後面]要用的羅輯運算
        /// </summary>
        public WhereLogicType whereLogic { get; set; }
    }
    public class GroupFieldObject : FieldsObject
    {
        public AggregateType AggregateType { get; set; }
    }
}