using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProcCore.JqueryHelp.FormvValidate;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.AjaxFormObj
{

    public class AjaxFormObject : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public AjaxFormObject(String FormId)
        {
            this.Id = FormId;

            this.SubmitElementId = "btn_submit";
            this.SubmitEvent = HtmlObjectEvent.click;
            this.SubmitFunctionString = @"
                ajaxHasDone=$.when.apply( $, ajaxRequest ); 
                ajaxHasDone.done(function() {  
                    if($('#" + this.Id + @"').valid()) {$('#" + this.Id + @"').submit();}
                });";

            this.options = new FormAjaxOption();

            this.formValidate = new FormValidateSetup(new jqSelector() { IdName = FormId });

            this.options.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
            this.options.beforeSubmit = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
        }

        public String Id { get; set; }
        public String SubmitElementId { get; set; }
        public HtmlObjectEvent SubmitEvent { get; set; }

        public String SubmitFunctionString { get; set; }
        public FormAjaxOption options { get; set; }

        /// <summary>
        /// 表單驗證模組，AjaxFormObject設定後formVlidate即有建立物件，可不用再重建。
        /// </summary>
        public FormValidateSetup formValidate { get; set; }

        public Boolean? SuccessUseMessage { get; set; }
        public Boolean? UseReturnButton { get; set; }
        public Boolean? UseFormVlidate { get; set; }

        public String ReturnElementId { get; set; }
        public String ReturnUrl { get; set; }

        public String AppendScriptBefore { get; set; }
        public String AppendScriptAfter { get; set; }

        public virtual String ToScriptString()
        {
            MakeJqScript createJsonStrig = new MakeJqScript() { GetObject = this.options , needBrace=true};
            
            String returnScript = String.Empty;
            jqElementEvent returnButton = new jqElementEvent(new jqSelector() { IdName = this.ReturnElementId }) {  jqRaiseEvent= jqEventStyle.normal};

            if (this.UseReturnButton == true)
                returnButton.events.Add(new jqElementEvent.jqEvents() {  htmlElementEvent = HtmlObjectEvent.click, funcString = "document.location.href = '" + this.ReturnUrl + @"?' + $('#returnQueryString').val();" });

            String tplStr = @"
{0};

{1};

$('#{7}').{8}(function () {{ {9} }});
$('#" + this.Id + @"').submit(function () {{$(this).ajaxSubmit(options);return false;}});

var options = {2};

{3};

{4};

{5};

{6};";

            String f = String.Format(tplStr, AppendScriptBefore, returnButton.ToScriptString(), createJsonStrig.MakeScript()
                , options.beforeSubmit.ToScriptString(funcMethodModule.funcMakeStyle.complete),
                options.success.ToScriptString(funcMethodModule.funcMakeStyle.complete),
                UseFormVlidate == true ? formValidate.ToScriptString() : "", AppendScriptAfter, SubmitElementId, SubmitEvent, SubmitFunctionString);

            return f.ToJqueryDocumentReady();
        }
    }

    public class FormAjaxOption:ijQueryUIScript
    {
        funcMethodModule _beforeSubmit;
        funcMethodModule _beforeSerialize;
        funcMethodModule _success;

        public FormAjaxOption() { }

        public funcMethodModule beforeSubmit
        {
            get
            {
                return _beforeSubmit;
            }
            set
            {
                _beforeSubmit = value;
                if (_beforeSubmit != null)
                {
                    //_beforeSubmit = new funcMethodModule();
                    _beforeSubmit.funcParam.AddRange(new String[] { "formData", "jqForm", "options" });
                    _beforeSubmit.funcName = "AjaxFormBeforeSubmit";
                }
            }
        }
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
                    //_success = new funcMethodModule();
                    _success.funcParam.AddRange(new String[] { "jsonobj", "statusText", "xhr", "$form" });
                    _success.funcName = "AjaxFormSuccess";
                }
            }
        }
        public funcMethodModule beforeSerialize
        {
            get
            {
                return _beforeSerialize;
            }
            set
            {
                _beforeSerialize = value;
                if (_beforeSerialize != null)
                {
                    //_beforeSerialize = new funcMethodModule();
                    _beforeSerialize.funcParam.AddRange(new String[] { "jqXHR", "textStatus" });
                }
            }
        }

        public String dataType { get; set; }
        public String contentType { get; set; }
        public String target { get; set; }
        public Boolean? replaceTarget { get; set; }
        public String iframeTarget { get; set; }
        public String data { get; set; }

        public Boolean? semantic { get; set; }
        public Boolean? resetForm { get; set; }
        public Boolean? clearForm { get; set; }
        public Boolean? iframe { get; set; }
        public Boolean? forceSync { get; set; }

        public String ToScriptString()
        {
            return (new MakeJqScript() { GetObject = this, needBrace = false }).MakeScript();
        }
    }
}
