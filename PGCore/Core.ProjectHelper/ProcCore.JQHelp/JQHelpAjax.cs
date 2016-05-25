using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcCore.JqueryHelp.DeferredHelp;

namespace ProcCore.JqueryHelp.AjaxHelp
{
    public class ajaxBaseObject : BaseJqueryScriptHelp, ijQueryUIScript
    {
        protected funcMethodModule _beforeSend;
        protected DataModule _data;

        public ajaxBaseObject()
        {
            type = "post";
            dataType = "json";
            global = false;
            contentType = "application/x-www-form-urlencoded; charset=utf-8";
        }
        public Boolean? async { get; set; }
        public String url { get; set; }
        public String type { get; set; }
        public String dataType { get; set; }
        public String contentType { get; set; }
        /// <summary>
        /// Default: true
        /// Whether to trigger global Ajax event handlers for this request. The default is true. Set to false to prevent the global handlers like ajaxStart or ajaxStop from being triggered. This can be used to control various
        /// </summary>
        public Boolean? global { get; set; }
        public DataModule data
        {
            get
            {
                if (_data == null)
                {
                    _data = new DataModule();
                }
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public funcMethodModule data_Func { get; set; }
        public funcMethodModule xhr { get; set; }

        public Boolean? cache { get; set; }
        public Boolean? ifModified { get; set; }
        /// <summary>
        /// Default: true
        /// By default, data passed in to the data option as an object (technically, anything other than a string) will be processed and transformed into a query string, fitting to the default content-type "application/x-www-form-urlencoded". If you want to send a DOMDocument, or other non-processed data, set this option to false.
        /// </summary>
        public Boolean? processData { get; set; }
        /// <summary>
        /// Only for requests with "jsonp" or "script" dataType and "GET" type. Forces the request to be interpreted as a certain charset. Only needed for charset differences between the remote and local content.
        /// </summary>
        public String scriptCharset { get; set; }
        public String password { get; set; }
        public String username { get; set; }
        public int? timeout { get; set; }
        /// <summary>
        /// Set this to true if you wish to use the traditional style of param serialization.
        /// </summary>
        public Boolean? traditional { get; set; }

        /// <summary>
        /// jqXHR,settings
        /// </summary>
        public funcMethodModule beforeSend
        {
            get
            {
                return _beforeSend;
            }
            set
            {
                _beforeSend = value;
                if (_beforeSend != null)
                {
                    _beforeSend.funcParam.AddRange(new String[] { "jqXHR", "settings" });
                }
            }
        }
        /// <summary>
        /// 未包括ax = $.ajax({{ {0} }});ajaxRequest.push(ax);
        /// </summary>
        /// <returns></returns>
        public virtual String ToScriptString()
        {
            MakeJqScript c = new MakeJqScript() { GetObject = this, needBrace = false };
            c.ItemRename.Add("data_Func", "data");
            c.ItemNotMake.Add("deferred");
            String s = c.MakeScript();
            return s;
        }
        /// <summary>
        /// 有包括var ax = $.ajax({{ {0} }});ajaxRequest.push(ax);
        /// </summary>
        /// <returns></returns>
        public String ToSelfScriptString()
        {
            String GetToScriptString = this.ToScriptString();
            String tpl = "var ax = $.ajax({{ {0} }});ajaxRequest.push(ax);";
            String script = String.Format(tpl, GetToScriptString);

            return script;
        }
    }
    public class ajaxObject : ajaxBaseObject, ijQueryUIScript
    {
        protected funcMethodModule _error;
        protected funcMethodModule _success;
        protected funcMethodModule _complete;
        public ajaxObject()
            : base()
        {
        }
        public funcMethodModule complete
        {
            get
            {
                return _complete;
            }
            set
            {
                _complete = value;
                if (_complete != null)
                {
                    _complete.funcParam.AddRange(new String[] { "jqXHR", "textStatus" });
                }
            }
        }
        public funcMethodModule error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                if (_error != null)
                {
                    //_error = new funcMethodModule();
                    _error.funcParam.AddRange(new String[] { "jqXHR", "textStatus", "errorThrown" });
                }
            }
        }
        /// <summary>
        /// 參數：data,textStatus,jqXHR
        /// </summary>
        public funcMethodModule success
        {
            get
            {
                return _success;
            }
            set
            {
                _success = value;
                if (_success != null)
                {
                    _success.funcParam.AddRange(new String[] { "data", "textStatus", "jqXHR" });
                }
            }
        }
        public override String ToScriptString()
        {
            return base.ToScriptString();
        }
    }
    public class ajaxDeferred : ajaxBaseObject, ijQueryUIScript
    {
        public ajaxDeferred()
            : base()
        {
            deferred = new Deferred();
        }
        public Deferred deferred { get; set; }
        public override String ToScriptString()
        {
            String t = "var ax = $.ajax({{ {0} }}){1};ajaxRequest.push(ax);";
            
            String s = String.Format(t, base.ToScriptString(), this.deferred.ToScriptString());
            return s;
        }
    }
    public class ajaxEventAjaxHandle : ijQueryUIScript
    {
        public ajaxEventAjaxHandle()
            : base()
        {
            deferred = new Deferred();
        }

        public funcBase UseStandardDone()
        {
            DeferredLive dfObj = new DeferredLive();
            dfObj.deferredFunc.parmsRange = new String[] { "data", "textStatus", "jqXHR" };
            this.deferred.DeferredCollect.Add(dfObj);
            return dfObj.deferredFunc;
        }
        public String paramString { get; set; }
        public String Url { get; set; }
        public Deferred deferred { get; set; }
        public String ToScriptString()
        {
            return "$.EventAjaxHandle({" + this.paramString + "},'" + this.Url + "')" + this.deferred.ToScriptString();
        }
    }
}
