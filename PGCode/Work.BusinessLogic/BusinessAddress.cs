using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace SkyCore.BusinessBase
{
    public class LogicAdress : LogicBase
    {
        public Dictionary<String, String> GetCity()
        {
            string sql = string.Empty;
            sql = string.Format("Select city as id,city From _AddressCity order by sort");
            DataTable dt = ExecuteData(sql);

            Dictionary<string, string> Key_Value = new Dictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                Key_Value.Add(dr[0].ToString(), dr[1].ToString());
            }
            return Key_Value;
        }
        public Dictionary<String, String> GetCountry(String city)
        {
            string sql = string.Empty;
            sql = string.Format("Select country as id,country From _AddressCountry where city='{0}' order by sort", city);
            DataTable dt = ExecuteData(sql);
            Dictionary<string, string> Key_Value = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                Key_Value.Add(dr[0].ToString(), dr[1].ToString());
            }
            return Key_Value;
        }
        public string GetZip(String city, String country)
        {
            string sql = string.Empty;
            sql = string.Format("Select zip From _AddressCountry where city='{0}' and country='{1}'", city, country);
            DataTable dt = ExecuteData(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["zip"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
