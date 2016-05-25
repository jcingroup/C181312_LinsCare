using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcCore.JqueryHelp.AjaxHelp;

namespace ProcCore.JqueryHelp.ElementAjaxScriptHelp
{
    public class ElementAjaxScriptCreate : BaseJqueryScriptHelp,ijQueryUIScript
    {
        private String _id = String.Empty;
        private String _class = String.Empty;

        public ElementAjaxScriptCreate()
        {
            AjaxOptionScript = new ajaxObject();
            AjaxOptionScript.success = new funcMethodModule() { MakeStyle= funcMethodModule.funcMakeStyle.funcConext};
            AjaxOptionScript.success.funcString = "";

            AjaxOptionScript.error = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            AjaxOptionScript.error.funcString = "alert('Ajax Request Error State:' + textStatus + ':' + errorThrown);\r\n";
        }
        public jqEventStyle CreateStyle { get; set; }

        public String Id { 
            get { 
                return this._id; 
            }

            set { 
                this._id = value; 
                Event = HtmlObjectEvent.click; 
            } 
        }
        public String ClassName
        {
            get
            {
                return this._class;
            }

            set
            {
                this._class = value;
                Event = HtmlObjectEvent.click;
            }
        }

        public HtmlObjectEvent Event { get; set; }
        //===================================
        public String BeforeAjaxScript { get; set; }
        public ajaxObject AjaxOptionScript { get; set; }
        public String AfterAjaxScript { get; set; }
        //================================
        public String ToScriptString()
        {
            String scriptContext = String.Empty;
            String script = String.Empty;
            String _TagName = String.Empty;

            _TagName = _id != null && _id != "" ? "#" + _id : "." + _class;

            if (CreateStyle == jqEventStyle.normal)
            {
                scriptContext = @"$('{3}').{4}(function () {{ {0} {1} {2} }})";

                script = String.Format(scriptContext,
                                BeforeAjaxScript,
                                AjaxOptionScript.ToSelfScriptString(),
                                AfterAjaxScript,
                                _TagName,
                                Event.ToString());
            }
            else if (CreateStyle == jqEventStyle.on)
            {
                scriptContext = "$('{3}').live('{4}',function () {{ {0} {1} {2} }})";

                script = String.Format(scriptContext,
                                BeforeAjaxScript,
                                AjaxOptionScript.ToSelfScriptString(),
                                 AfterAjaxScript,
                                _TagName,
                                Event.ToString());
            }
            else if (CreateStyle == jqEventStyle.functionName)
            {
                scriptContext = @"
                function () {{
                {0}
                {1}
                {2}
                }}
                ";

                script = String.Format(scriptContext,
                                String.Join(";\r\n", BeforeAjaxScript.ToArray()),
                                AjaxOptionScript.ToScriptString(),
                                String.Join(";\r\n", AfterAjaxScript.ToArray())
                                );
            }
            return script;
        }

    }

    public class ElementAjaxHandle : jqElementEvent, ijQueryUIScript
    {
        private ajaxDeferred _ajaxOption;
        private jqEvents jqEVT;

        public ElementAjaxHandle(jqSelector Id):base(Id) {
            _ajaxOption = new ajaxDeferred();
            jqEVT = new jqEvents();
        }

        public ajaxDeferred ajax { 
            get { 
                return _ajaxOption; 
            } 
        }

        public HtmlObjectEvent Event {
            get {
                return jqEVT.htmlElementEvent;
            }
            set {
                jqEVT.htmlElementEvent = value;
            }
        }

        public String funcString
        {
            get
            {
                return jqEVT.funcString;
            }

            set {
                jqEVT.funcString = value;
            }
        }

        public override String ToScriptString()
        {
            this.jqEVT.funcString += this.ajax.ToScriptString();
            this.events.Add(this.jqEVT);
            return base.ToScriptString();
        }
    }
}
