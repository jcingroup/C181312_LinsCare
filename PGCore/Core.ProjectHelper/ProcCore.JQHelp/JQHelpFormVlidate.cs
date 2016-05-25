using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;
using ProcCore.NetExtension;
using System.Linq.Expressions;

using ProcCore.JqueryHelp.AddValidator;

//給Jquery Form Submit 時做驗證
namespace ProcCore.JqueryHelp.FormvValidate
{
    public class FormValidateSetup : BaseJqueryScriptHelp, ijQueryUIScript
    {
        List<FormValidateModels> _f = new List<FormValidateModels>();
        List<AjaxValidatorMethod> _g = new List<AjaxValidatorMethod>();
        List<CustomValidatoeMethod> _h = new List<CustomValidatoeMethod>();

        public FormValidateSetup(jqSelector Id)
        {
            this.Id = Id;
            Option = new Validate();
        }
        public jqSelector Id { get; set; }
        public Validate Option { get; set; }
        /// <summary>
        /// 增加欄位編輯使用規則
        /// </summary>
        /// <param name="f"></param>
        public void Add(FormValidateModels f)
        {
            _f.Add(f);
        }
        public String ToScriptString()
        {
            List<String> r = new List<String>();
            List<String> m = new List<String>();

            foreach (FormValidateModels f in _f)
            {
                String extra = f.ExtraRule.ToKeyValueList().ToArray().JoinArray(",");

                extra = extra == "" ? "" : "," + extra;
                r.Add(f.fieldName + ":{" + f.rules.ToScriptString() + extra + "}");
                m.Add(f.fieldName + ":{" + f.messages.ToScriptString() + "}");
            }
            return  this.Id.IDPack() + "validate({" + Option.ToScriptString() + ",rules:{" + r.ToArray().JoinArray(",") + "},messages:{" +  m.ToArray().JoinArray(",") + "}})";
        }

        /// <summary>
        /// 驗證事項設定
        /// </summary>
        public class Validate : ijQueryUIScript
        {
            private funcMethodModule _invalidHandler;
            private funcMethodModule _showErrors;
            private funcMethodModule _errorPlacement;
            private funcMethodModule _success;
            private funcMethodModule _highlight;
            private funcMethodModule _unhighlight;
            private Dictionary<String, String> _groups;

            public funcMethodModule invalidHandler
            {
                get
                {
                    return _invalidHandler;
                }
                set
                {
                    _invalidHandler = value;
                    if (_invalidHandler != null)
                    {
                        // _invalidHandler = new funcMethodModule();
                        _invalidHandler.funcParam.AddRange(new String[] { "form", "validator" });
                    }
                }
            }
            public funcMethodModule showErrors
            {
                get
                {

                    return _showErrors;
                }
                set
                {
                    _showErrors = value;
                    if (_showErrors != null)
                    {
                        //_showErrors = new funcMethodModule();
                        _showErrors.funcParam.AddRange(new String[] { "errorMap", "errorList" });
                    }
                }
            }
            public funcMethodModule errorPlacement
            {
                get
                {

                    return _errorPlacement;
                }
                set
                {
                    _errorPlacement = value;
                    if (_errorPlacement != null)
                    {
                        //_errorPlacement = new funcMethodModule();
                        _errorPlacement.funcParam.AddRange(new String[] { "error", "element" });
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
                        _success.funcParam.AddRange(new String[] { });
                    }
                }
            }
            public funcMethodModule highlight
            {
                get
                {

                    return _highlight;
                }
                set
                {
                    _highlight = value;
                    if (_highlight != null)
                    {
                        //_highlight = new funcMethodModule();
                        _highlight.funcParam.AddRange(new String[] { "element", "errorClass", "validClass" });
                    }
                }
            }
            public funcMethodModule unhighlight
            {
                get
                {
                    return _unhighlight;
                }
                set
                {
                    _unhighlight = value;
                    if (_unhighlight != null)
                    {
                        //_unhighlight = new funcMethodModule();
                        _unhighlight.funcParam.AddRange(new String[] { "element", "errorClass", "validClass" });
                    }
                }
            }

            public Validate()
            {
                errorClass = "error";
                validClass = "valid";
                errorElement = "label";
            }

            public Boolean? debug { get; set; }
            public Boolean? onsubmit { get; set; }
            public Boolean? onfocusout { get; set; }
            public Boolean? onkeyup { get; set; }
            public Boolean? onclick { get; set; }
            public Boolean? focusInvalid { get; set; }
            public Boolean? focusCleanup { get; set; }
            public String meta { get; set; }
            public String errorClass { get; set; }
            public String validClass { get; set; }

            /// <summary>
            /// 錯誤訊息所要用的元素 預設是用 label
            /// </summary>
            public String errorElement { get; set; }
            public String wrapper { get; set; }
            /// <summary>
            /// EX:   errorLabelContainer: "#messageBox"
            /// </summary>
            public String errorLabelContainer { get; set; }

            /// <summary>
            /// EX: 如果不表單通過 的錯誤訊息顯示的地方
            ///    errorContainer: "#messageBox1, #messageBox2",
            ///    errorLabelContainer: "#messageBox1 ul",
            ///    wrapper: "li", debug:true
            ///Ex: <div id="messageBox2"><h3>有錯誤發生請檢查</h3></div>
            /// </summary>
            public String errorContainer { get; set; }
            public Boolean? ignoreTitle { get; set; }
            public Dictionary<String, String> groups
            {
                get
                {
                    if (_groups == null)
                    {
                        _groups = new Dictionary<String, String>();
                    }
                    return _groups;
                }
                set
                {
                    _groups = value;
                }
            }
            public String ToScriptString()
            {
                return (new MakeJqScript() { GetObject = this, needBrace = false }).MakeScript();
            }
        }
    }

    /// <summary>
    /// 表單規則及錯誤訊息設定模組
    /// </summary>
    public class FormValidateModels
    {
        /// <summary>
        /// 表單規則及錯誤訊息設定
        /// </summary>
        public FormValidateModels()
        {
            rules = new FieldRule();
            messages = new FieldMessage();
            ExtraRule = new Dictionary<String, String>();
        }
        public String fieldName { get; set; }
        public FieldRule rules { get; set; }
        public FieldMessage messages { get; set; }
        public Dictionary<String, String> ExtraRule { get; set; }
    }

    public class FieldRule : ijQueryUIScript
    {
        public Boolean? required { get; set; }
        public Boolean? email { get; set; }
        public Boolean? digits { get; set; }
        public Boolean? url { get; set; }
        public Boolean? date { get; set; }
        public Boolean? creditcard { get; set; }
        public Boolean? number { get; set; }
        public int? maxlength { get; set; }
        public int? minlength { get; set; }
        public int? max { get; set; }
        public int? min { get; set; }
        public int[] rangelength { get; set; }
        public int[] range { get; set; }

        public String ToScriptString()
        {
            return (new MakeJqScript() { GetObject = this, needBrace = false }).MakeScript();
        }
    }
    public class FieldMessage : ijQueryUIScript
    {
        public String required { get; set; }
        public String email { get; set; }
        public String digits { get; set; }
        public String url { get; set; }
        public String date { get; set; }
        public String number { get; set; }
        public String creditcard { get; set; }
        public String maxlength { get; set; }
        public String minlength { get; set; }
        public String rangelength { get; set; }
        public String max { get; set; }
        public String min { get; set; }
        public String ToScriptString()
        {
            return (new MakeJqScript() { GetObject = this, needBrace = false }).MakeScript();
        }
    }
}