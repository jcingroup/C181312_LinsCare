using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProcCore.JqueryHelp.AjaxHelp;
using ProcCore.JqueryHelp.ElementAjaxScriptHelp;
using ProcCore.JqueryHelp.DeferredHelp;

namespace ProcCore.JqueryHelp.AddValidator
{
    /// <summary>
    /// jQuery.validator.addMethod用法如下：
    /// jQuery.validator.addMethod('MethodName',function(value, element, param){})
    /// </summary>
    /// <param name="MethodName">自訂方法名稱</param>

    public class CustomValidatoeMethod : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public String Method { get; set; }
        public String ErrMessage { get; set; }
        protected funcMethodModule func { get; set; }
        protected String tplreturnfunc { get; set; }

        /// <summary>
        /// 自訂檢查的function必須return true或false
        /// </summary>
        /// <param name="MethodName">為自訂檢查的名稱：jQuery.validator.addMethod(MethodName, function (value, element, param) {},錯誤時的訊息)</param>
        public CustomValidatoeMethod(String MethodName)
        {
            this.Method = MethodName;
            func = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            func.funcParam.AddRange(new String[] { "value", "element", "param" });
            tplreturnfunc = "jQuery.validator.addMethod('{0}',\r\n{1},\r\n'{2}');\r\n";
        }

        public virtual String ToScriptString()
        {
            return ToOptionString();
        }

        public String ToOptionString() {
            return String.Format(tplreturnfunc, this.Method, func.ToScriptString(), this.ErrMessage);
        }
    }

    /// <summary>
    /// 搭配jQuery.validator.addMethod所做的Ajax即時檢查
    /// 
    /// </summary>
    public class AjaxValidatorMethod : CustomValidatoeMethod, ijQueryUIScript
    {
        /// <summary>
        /// jQuery.validator.addMethod用法如下：
        /// jQuery.validator.addMethod('MethodName',function(value, element, param){})
        /// </summary>
        /// <param name="MethodName">自訂方法名稱，由系統產javascript function name，此method name要通知驗證功能。</param>
        /// 
        public AjaxValidatorMethod(String MethodName)
            : base(MethodName)
        {
            this.Method = MethodName;
            this.funcBefore = "var jsonobj";

            ajax = new ajaxObject();

            ajax.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajax.success.funcString = "jsonobj = data;";
            ajax.error = new funcMethodModule(){ MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajax.error.funcString = "alert('Ajax Eror：' + errorThrown);";

            ajax.async = false;
            ajax.type = "post";
            this.funcAfter = "return jsonobj.result";
        }

        public ajaxObject ajax { get; set; }

        /// <summary>
        /// 在做Ajax檢查動作前要做的scriptn內容，預設為var jsonobj。
        /// </summary>
        public String funcBefore { get; set; }

        /// <summary>
        /// 在做Ajax檢查動作後要做的scriptn內容，預設為return jsonobj.result。
        /// </summary>
        public String funcAfter { get; set; }

        /// <summary>
        /// 產生script程式
        /// </summary>
        /// <returns></returns>
        public override String ToScriptString()
        {
            //樣版
            String t = "{0};{1};{2};";
            this.func.funcString = String.Format(t, this.funcBefore, ajax.ToSelfScriptString(), funcAfter);
            String r = base.ToScriptString();
            return r;
        }
    }

    public class CollectCustomValidatorMethod : ijQueryUIScript
    {
        private List<CustomValidatoeMethod> CollectCustomValidator = new List<CustomValidatoeMethod>();
        public void Add(CustomValidatoeMethod c)
        {
            CollectCustomValidator.Add(c);
        }
        public String ToScriptString()
        {
            return ToOptionString();
        }
        public String ToOptionString() {
            String s = String.Empty;
            foreach (var c in this.CollectCustomValidator)
                s += c.ToScriptString();

            return s;
        }
        public Dictionary<String, String> ToSetRuleForFormValidatorOption()
        {
            Dictionary<String, String> extraRule = new Dictionary<string, string>();
            foreach (var c in this.CollectCustomValidator)
            {
                extraRule.Add(c.Method, "true");
            }
            return extraRule;
        }
    }
}
