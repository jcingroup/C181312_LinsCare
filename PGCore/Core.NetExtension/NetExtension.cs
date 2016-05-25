using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using System.Text;

namespace ProcCore.NetExtension
{
    public static class ExtensionDateTime
    {
        public static string ToStandardDate(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd");
        }
        public static string ToStandardTime(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }
        public static string ToStandardDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string ToStandardDateTime2(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string To民國年(this DateTime dt)
        {
            return (dt.Year - 1911).ToString() + dt.ToString("/MM/dd");
        }
    }

    public static class ExtensionInt
    {
        public static String[] ToStringArray(this int[] s)
        {

            List<String> r = new List<String>();
            foreach (int i in s)
            {
                r.Add(i.ToString());
            }
            return r.ToArray();
        }
    }

    public static class ExtensionString
    {
        public static String NullValue(this String s, String v)
        {
            return s == null ? v : s;
        }
        public static String RemovHtmlTag(this String s)
        {
            if (s != null)
                return Regex.Replace(s, "<.*?>", string.Empty);
            else
                return "";
        }
        public static String ScriptString(this String s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }
        public static String ToScriptTag(this String s)
        {
            if (s != null)
                return "<script type=\"text/javascript\">\r\n" + s + "\r\n</script>\r\n";
            else
                return s;
        }
        public static String ToJqueryDocumentReady(this String s)
        {
            return s != null ? "$(document).ready(function () {".NewLine(1) + s.NewLine(2) + "});".NewLine(1) : s;
        }
        public static String JoinArray(this String[] s, String JoinChar, String beforedot, String afterdot)
        {
            String r = String.Join(afterdot + JoinChar + beforedot, s);

            if (r != "")
            {
                return beforedot + r + afterdot;
            }
            else
            {
                return "";
            }
        }
        public static String JoinArray(this String[] s, String JoinChar, String dot)
        {
            String r = String.Join(dot + JoinChar + dot, s); // ","

            if (r != "")
            {
                return dot + r + dot; //整個字串的前後
            }
            else
            {
                return "";
            }
        }
        public static String JoinArray(this String[] s, String JoinChar)
        {
            String r = String.Join(JoinChar, s);
            return r;
        }
        public static String JoinArrayAppendString(this String[] s, String JoinChar, String AppendString)
        {
            List<String> l = new List<String>();
            foreach (String g in s)
            {
                l.Add(g + AppendString);
            }
            return l.ToArray().JoinArray(",");
        }
        public static String GetFileExt(this String s)
        {
            int c = s.LastIndexOf('.');
            String r = String.Empty;
            if (c > 0)
                r = s.Substring(c);
            else
                r = "";

            return r.ToLower();
        }
        public static String GetFileName(this String s)
        {
            int c = s.LastIndexOf('\\');
            string r = string.Empty;
            if (c > 0)
            {
                r = s.Substring(c);
            }
            else
            {
                c = s.LastIndexOf('/');
                if (c > 0)
                {
                    r = s.Substring(c);
                }
                else
                {
                    r = s;
                }
            }
            return r;
        }
        public static String OnlyMasterName(this String s)
        {
            int c = s.LastIndexOf('.');
            string r = string.Empty;

            if (c > 0)
                r = s.Substring(0, c);
            else
                r = s;

            return r;
        }

        public static String Right(this String s, int n)
        {
            return s.Substring(n > s.Length ? 0 : s.Length - n);
        }

        public static String Left(this String s, int n)
        {
            return s.Substring(0, n > s.Length ? s.Length : n);
        }

        public static String NewLine(this String s)
        {
            return s != null ? s + "\r\n" : s;
        }
        public static Int32[] CInt(this String[] s)
        {
            List<Int32> i = new List<Int32>();
            foreach (String S in s)
            {
                i.Add(S.CInt());
            }
            return i.ToArray();
        }

        /// <summary>
        ///  \t n個 + s + \r\n
        /// </summary>
        /// <param name="tabCount">\t n個</param>
        public static String NewLine(this String s, int tabCount)
        {
            return s != null ? (new String('\t', tabCount)) + s + "\r\n" : s;
        }

        #region 繁簡轉換
        private const int LocaleSystemDefault = 0x0800;
        private const int LcmapSimplifiedChinese = 0x02000000;
        private const int LcmapTraditionalChinese = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc,
                                              [Out] string lpDestStr, int cchDest);
        public static String ToSimplified(this String argSource)
        {
            var t = new String(' ', argSource.Length);
            LCMapString(LocaleSystemDefault, LcmapSimplifiedChinese, argSource, argSource.Length, t, argSource.Length);
            return t;
        }
        public static String ToTraditional(this String argSource)
        {
            var t = new String(' ', argSource.Length);
            LCMapString(LocaleSystemDefault, LcmapTraditionalChinese, argSource, argSource.Length, t, argSource.Length);
            return t;
        }
        #endregion
    }

    public static class ExtensionBoolean
    {
        public static String BooleanValue(this Boolean s, string TrueString, string FalseString)
        {
            if (s) return TrueString; else return FalseString;
        }
    }

    public static class ExtensionObject
    {
        public static int? ToInt(this object o)
        {
            if (o != null)
            { return (int)o; }
            else
            { return null; }
        }
        public static DateTime? ToDateTime(this object o)
        {
            if (o != null)
            { return (DateTime)o; }
            else
            { return null; }
        }
        public static Boolean? ToBoolean(this object o)
        {
            if (o != null)
            { return (Boolean)o; }
            else
            { return null; }
        }
        public static int CInt(this object o)
        {
            return int.Parse(o.ToString());
        }
        public static DateTime CDateTime(this object o)
        {
            return DateTime.Parse(o.ToString());
        }
        public static Boolean CBoolean(this object o)
        {
            return (Boolean)o;
        }
    }

    public static class ExtensionCollect
    {
        public static List<String> ToKeyValueList(this Dictionary<String, String> s)
        {
            List<String> r = new List<String>();
            foreach (KeyValuePair<String, String> kv in s)
            {
                r.Add(kv.Key + ":" + kv.Value);
            }
            return r;
        }
    }

    public static class ExtensionData
    {
        public static void LoadDataToModule(this DataRow drData, Object mdObject)
        {
            //http://www.codeproject.com/Articles/11914/Using-Reflection-to-convert-DataRows-to-objects-or
            DataColumnCollection GetTableColumnsObj = drData.Table.Columns;

            Type t = mdObject.GetType();
            PropertyInfo[] f = t.GetProperties();
            String GetColName = "";
            for (Int32 i = 0; i <= GetTableColumnsObj.Count - 1; i++)
            {
                try
                {
                    GetColName = GetTableColumnsObj[i].ColumnName;
                    var gf = f.AsEnumerable().Where(x => x.Name == GetColName);

                    if (gf.Count() > 0)
                    {
                        PropertyInfo mf = gf.FirstOrDefault();
                        if (drData[GetColName] != DBNull.Value)
                        {
                            mf.SetValue(mdObject, drData[GetColName], null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ":" + GetColName);
                }
            };
        }

        public static Dictionary<String, String> dicMakeKeyValue(this DataTable s, int KeyIndex, int ValueIndex)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            foreach (DataRow dr in s.Rows)
            {
                dic.Add(dr[KeyIndex].ToString().TrimEnd(), dr[ValueIndex].ToString().TrimEnd());
            }
            return dic;
        }
        public static String TableCodeValue(this int intValue, Dictionary<int, String> dic)
        {
            var query = dic.Where(x => x.Key == intValue);

            if (query.Count() > 0)
                return query.FirstOrDefault().Value;
            else
                return "";

        }
        public static String TableCodeValue(this String StringValue, Dictionary<String, String> dic)
        {
            var query = dic.Where(x => x.Key == StringValue);

            if (query.Count() > 0)
                return query.FirstOrDefault().Value;
            else
                return "";
        }
    }
}