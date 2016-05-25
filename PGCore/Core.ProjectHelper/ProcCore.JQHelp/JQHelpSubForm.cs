using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProcCore.NetExtension;


namespace ProcCore.JqueryHelp.SubFormHelp
{
    public class HiddenDivHandle : BaseJqueryScriptHelp, ijQueryUIScript,iHtmlToString
    {
        public HiddenDivHandle(String DivId)
        {
            this.DivId = DivId;
            this.ButtonCloseId = "Button_Click_Close_" + DivId;
            this.ButtonOpenId = "Button_Click_Opene_" + DivId;
            this.OpenEvent = HtmlObjectEvent.click;
            this.DivHtmlTpl = "<div id=\"{0}\" style=\"display:none; position:absolute; padding:5px;\" class=\"ui-widget-header edit-master-caption\">{1}</div>";
        }

        public String DivId { get; set; }
        public String ButtonOpenId { get; set; }
        public String ButtonCloseId { get; set; }
        public String AppendHtml { get; set; }

        public HtmlObjectEvent OpenEvent { get; set; }

        public String AfterOpenScript { get; set; }
        public String AfterCloseScript { get; set; }
        public String AppendScript { get; set; }

        public String DivHtmlTpl { get; set; }

        public virtual String ToScriptString()
        {
            String tplScript = "$('#" + DivId + @"').center();";
            tplScript += "$('#" + ButtonOpenId + @"')." + this.OpenEvent + "(function(){$('#" + DivId + @"').show();" + AfterOpenScript + "});";
            tplScript += "$('#" + ButtonCloseId + @"').click(function(){$('#" + DivId + @"').hide();" + AfterCloseScript + "});";
            tplScript += this.AppendScript;

            return tplScript;
        }

        public virtual String ToHtmlString()
        {
            return String.Format(this.DivHtmlTpl, DivId, this.AppendHtml);
        }

    }
}
