using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ProcCore.NetExtension;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.DataBaseConnection;

namespace ProcCore.DatabaseCore.TableFieldModule
{
    public abstract class TableMap<TabObjSource> : IDisposable
    {
        public String N { get; set; }
        public String NameAs
        {
            get
            {
                if (Alias == null)
                    return N;

                else
                    return N + " AS " + Alias;
            }
        }
        public String NameDot
        {
            get
            {
                if (Alias == null)
                    return N + ".";

                else
                    return Alias + ".";
            }
        }
        public String NameBlank
        {
            get
            {
                if (Alias == null)
                    return "";

                else
                    return Alias + ".";
            }
        }
        public String SQLSelect { get; set; }
        public String SQLWhere { get; set; }
        public String SQLOrderBy { get; set; }
        public String SQLGroupBy { get; set; }
        public String SQLHaving { get; set; }

        public String Alias { get; set; }

        public TabObjSource GetTabObj { get; set; }

        /// <summary>
        /// 收集Table的Key Value對應表，主要提供給Grid的代碼類欄位做轉換，可減沙在SQL做Table Join 。
        /// </summary>
        /// <param name="idTabFields">Id欄位</param>
        /// <param name="nameTabFields">Text欄位</param>
        /// <param name="conn">Connection連線</param>
        /// <returns> Dictionary int String </returns>
        public Dictionary<int, String> CollectIdNameFields(
            Expression<Func<TabObjSource, FieldModule>> idTabFields,
            Expression<Func<TabObjSource, FieldModule>> nameTabFields,
            CommConnection conn
            )
        {
            Func<TabObjSource, FieldModule> id = idTabFields.Compile();
            Func<TabObjSource, FieldModule> name = nameTabFields.Compile();

            FieldModule fieldId = id.Invoke(this.GetTabObj);
            FieldModule fieldName = name.Invoke(this.GetTabObj);

            String sql = String.Format("Select {0},{1} From {2}", fieldId.N, fieldName.N, this.N);
            DataTable dt = conn.ExecuteData(sql);

            Dictionary<int, String> data = new Dictionary<int, String>();

            foreach (DataRow dr in dt.Rows)
            {
                data.Add(dr[fieldId.N].CInt(), dr[fieldName.N].ToString());
            }
            return data;
        }

        /// <summary>
        /// 取得Table的FieldModule的陣列集合
        /// </summary>
        /// <returns></returns>
        public FieldModule[] GetFieldModules()
        {
            var F = this.GetType().GetFields();
            List<FieldModule> L = new List<FieldModule>();
            foreach (FieldInfo f in F)
            {
                Object O = f.GetValue(this);

                if (O.GetType() == typeof(FieldModule))
                {
                    L.Add((FieldModule)O);
                }
            }
            return L.ToArray();
        }
        public Dictionary<String, FieldModule> KeyFieldModules { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }
    }
    public class FieldModule
    {
        /// <summary>
        /// 屬性名稱
        /// </summary>
        public String M { get; set; }
        /// <summary>
        /// 對應資料庫欄位實際名稱
        /// </summary>
        public String N { get; set; }
        /// <summary>
        /// 資料庫欄位大略型態 Int Boolean DateTime String
        /// </summary>
        public SQLValueType T { get; set; }
        /// <summary>
        /// 可代入值 在primary key才比較會用到，其他請用標準module代值 
        /// </summary>
        public Object V { get; set; }
        public String Alias { get; set; }
        public String NameAs
        {
            get
            {
                if (Alias == null)
                    return N;

                else
                    return N + " AS " + Alias;
            }
        }
    }

    //以下尚未進行完成 先做訂義
    public enum CheckType
    {
        none, email, digits, url, date, customer
    }
    public class FieldsRules
    {
        public Boolean required { get; set; }
        public Boolean rangecheck { get; set; }
        public CheckType checktype { get; set; }
        public String requiredErrMessage { get; set; }
        public String checktypeErrMessage { get; set; }

        public int? min { get; set; }
        public int? max { get; set; }

        public DateTime? minDate { get; set; }
        public DateTime? maxDate { get; set; }
    }
    public static class ExtensionData
    {
        /// <summary>
        /// 此版本為可參照實際資料庫欄位名稱。
        /// </summary>
        /// <typeparam name="T"> TableMap</typeparam>
        /// <param name="dr"></param>
        /// <param name="md">m_module模型。</param>
        /// <param name="tb">table description module.</param>
        public static void LoadDataToModule<T>(this DataRow dr, Object md, T tb) where T : TableMap<T>
        {
            DataColumnCollection Cs = dr.Table.Columns;

            PropertyInfo[] f1 = md.GetType().GetProperties();
            FieldInfo[] f2 = tb.GetType().GetFields();

            foreach (var f in f1)
            {
                var q2 = f2.AsEnumerable().Where(x => x.Name == f.Name);
                if (q2.Count() > 0)
                {
                    FieldInfo qs = q2.FirstOrDefault();
                    FieldModule fm = (FieldModule)qs.GetValue(tb);

                    if (Cs.Contains(fm.N))
                        if (dr[fm.N] != DBNull.Value)
                            f.SetValue(md, dr[fm.N], null);
                }
                else
                {
                    if (Cs.Contains(f.Name))
                        if (dr[f.Name] != DBNull.Value)
                            f.SetValue(md, dr[f.Name], null);
                }
            }
        }


        public static void LiteDataToModule(this DataRow dr, Object md)
        {
            PropertyInfo[] f1 = md.GetType().GetProperties();

            foreach (var f in f1)
            {
                var q2 = from c in dr.Table.Columns.Cast<DataColumn>() where c.ColumnName == f.Name select c.ColumnName;
                if (q2.Count() > 0)
                {
                    var qs = q2.FirstOrDefault();
                    if (dr[qs] != DBNull.Value)
                        f.SetValue(md, dr[qs], null);
                }
            }
        }

    }
}
