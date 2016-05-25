using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProcCore;
using ProcCore.JqueryHelp.FormvValidate;
using ProcCore.JqueryHelp.JQGridScript;

namespace ProcCore.JqueryHelp.AddressHandle
{
    /// <summary>
    /// 地址的Ajax Jquery Script 產生器
    /// </summary>
    public class AddressAjaxHandle : ijQueryUIScript
    {
        public AddressAjaxHandle(String ElementId)
        {
            this.Id = new jqSelector() { IdName = ElementId, IdStyle = jqSelector.IdMakeStyle.ElementId };
            this.options = new Options();
        }

        public jqSelector Id { get; set; }
        public Options options { get; set; }
        public String ToScriptString()
        {
            return this.Id.ToScriptString() + ".addressajax(" + ToOptionString() + ");";
        }
        public String ToOptionString() {
            return options.ToScriptString();
        }

        public class Options :ijQueryUIOption
        {
            public Options()
            {
                this.cityValue = "桃園縣";
                this.countyValue = "中壢市";

                zipElement = new jqSelector();
                countyElement = new jqSelector();
            }

            public jqSelector zipElement { get; set; }
            public jqSelector countyElement { get; set; }
            public String cityValue { get; set; }
            public String countyValue { get; set; }
            public String ToScriptString()
            {
                return ToOptionString();
            }
            public String ToOptionString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this };
                return createJsonString.MakeScript();
            }
        }
    }
}
