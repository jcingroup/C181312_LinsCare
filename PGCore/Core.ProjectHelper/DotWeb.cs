using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;

using ProcCore.CKEdit;
using ProcCore.JqueryHelp;
using ProcCore.JqueryHelp.AjaxFilesUpLoadHelp;
using ProcCore.JqueryHelp.AjaxFormObj;
using ProcCore.JqueryHelp.AjaxHelp;
using ProcCore.JqueryHelp.AutocompleteHelp;
using ProcCore.JqueryHelp.CustomButton;
using ProcCore.JqueryHelp.DateTimePickerHelp;
using ProcCore.JqueryHelp.FormvValidate;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.JqueryHelp.JSTreeHelp;
using ProcCore.JqueryHelp.DialogHelp;
using ProcCore.NetExtension;
using ProcCore.JqueryHelp.AddressHandle;
using ProcCore.DatabaseCore.TableFieldModule;


namespace DotWeb
{
    public class HandleRequest
    {
        Dictionary<String, String> _s = new Dictionary<String, String>();

        public Boolean encodeURIComponent { get; set; }

        public HandleRequest()
        {
            encodeURIComponent = false;
            AutoGet();
        }
        private void AutoGet()
        {
            foreach (var qs in HttpContext.Current.Request.QueryString)
            {
                if (qs != null)
                {
                    _s.Add(qs.ToString(), HttpContext.Current.Request.QueryString[qs.ToString()]);
                }
            }
        }

        public string GetQueryValue(string key)
        {

            if (_s.ContainsKey(key))
            {
                return _s[key];
            }
            else
            {
                return "";
            }
        }

        public void Remove(string key)
        {
            _s.Remove(key);
        }
        public string ToQueryString()
        {

            List<string> l_s = new List<string>();
            foreach (var s in _s)
            {
                if (encodeURIComponent)
                {
                    l_s.Add(s.Key + "=" + HttpContext.Current.Server.UrlEncode(s.Value));
                }
                else
                {
                    l_s.Add(s.Key + "=" + s.Value);
                }
            }
            return string.Join("&", l_s.ToArray());
        }
    }
    public class StringResult : ViewResult
    {
        public String ToHtmlString { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;

            if (this.View == null)
            {
                result = this.FindView(context);
                this.View = result.View;
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            ViewContext viewContext = new ViewContext(context, this.View, this.ViewData, this.TempData, writer);

            this.View.Render(viewContext, writer);

            writer.Flush();

            ToHtmlString = Encoding.UTF8.GetString(stream.ToArray());

            if (result != null)
                result.ViewEngine.ReleaseView(context, this.View);
        }
    }
    public enum OperationMode
    {
        EnterList,
        EditInsert,
        EditModify,
        Delete,
        Search,
        Updating
    }

    public class BaseRptInfo
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String MakeDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd");
            }
        }
        public String Title { get; set; }
    }
    public class CReportInfo
    {
        public CReportInfo()
        {
            SubReportDataSource = new List<SubReportData>();
        }
        public static String ReportCompany = "";
        public String ReportFile { get; set; }
        public DataTable ReportData { get; set; }
        public List<SubReportData> SubReportDataSource { get; set; }

        public DataSet ReportMDData { get; set; }
        public Dictionary<String, Object> ReportParm { get; set; }
    }
    public class SubReportData
    {
        public string SubReportName { get; set; }
        public DataTable DataSource { get; set; }
    }
}

namespace DotWeb.Helpers
{
    public static class HtmlWebObject
    {
        #region JQuery Ui DateTimePicker Section
        public static MvcHtmlString DateTimePickerPlugin(this HtmlHelper h, jqSelector ElemntID, DateTimePicker option)
        {
            DateTimePickerUI jqObj = new DateTimePickerUI(ElemntID);
            if (option != null)
                jqObj.Options = option;

            return MvcHtmlString.Create(jqObj.ToScriptString());
        }

        /// <summary>
        /// 日期元件
        /// </summary>
        /// <param name="e">強型 代入欄位</param>
        /// <param name="option">參數選項設定</param>
        /// <returns>Jquery Script字串</returns>
        public static MvcHtmlString DateTimePickerPlugin<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, DateTimePicker option)
        where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(e);
            return DateTimePickerPlugin(h, new jqSelector() { IdName = inputName }, option);
        }
        #endregion

        #region JQuery Ui Autocomplete Section
        public static MvcHtmlString AutocompletePlugin(this HtmlHelper h, String ElemntID, AutocompleteHandle.Autocomplete option)
        {
            AutocompleteHandle jqObj = new AutocompleteHandle(new jqSelector() { IdName = ElemntID });
            if (option != null)
            {
                jqObj.Options = option;
                jqObj.Options.delay = 500;
            }
            return MvcHtmlString.Create(jqObj.ToScriptString());
        }
        public static MvcHtmlString AutocompletePlugin<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, AutocompleteHandle.Autocomplete option)
        where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(e);
            return AutocompletePlugin(h, n, option);
        }
        public static MvcHtmlString AutocompleteSimple<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String[] data)
        where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(e);
            return AutocompletePlugin(h, n, new AutocompleteHandle.Autocomplete() { source = new MutileType() { attrType = MutileType.AttrType.StringArray, StringArray = data } });
        }

        /// <summary>
        /// 回傳給Key值固定變數為 val 例：action?val=value
        /// </summary>
        public static MvcHtmlString mhs_AutocompleteAjax<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String Url)
        where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(e);
            ajaxObject ajax = new ajaxObject();
            ajax.url = Url;
            ajax.type = "get";
            ajax.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext, funcString = "response(data)" };
            ajax.data = new DataModule();
            ajax.data.Add("val", "$('#" + n + "').val()");

            return AutocompletePlugin(h, n, new AutocompleteHandle.Autocomplete()
            {
                source = new MutileType()
                {
                    attrType = MutileType.AttrType.funcMethod,
                    funcMethod = new funcMethodModule()
                    {
                        MakeStyle = funcMethodModule.funcMakeStyle.funcConext,
                        parmsRange = new String[] { "request", "response" },
                        funcString = ajax.ToSelfScriptString()
                    }
                }
            });
        }

        /// <summary>
        /// 回傳給Key值固定變數為 val 例：action?val=value
        /// </summary>
        public static AutocompleteHandle AutocompleteAjax<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String Url)
        where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(e);
            ajaxObject ajax = new ajaxObject();
            ajax.url = Url;
            ajax.type = "get";
            ajax.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext, funcString = "response(data)" };
            ajax.data = new DataModule();
            ajax.data.Add("val", "$('#" + n + "').val()");

            var option = new AutocompleteHandle.Autocomplete()
            {
                source = new MutileType()
                {
                    attrType = MutileType.AttrType.funcMethod,
                    funcMethod = new funcMethodModule()
                    {
                        MakeStyle = funcMethodModule.funcMakeStyle.funcConext,
                        parmsRange = new String[] { "request", "response" },
                        funcString = ajax.ToSelfScriptString()
                    }
                }
            };

            AutocompleteHandle jqObj = new AutocompleteHandle(new jqSelector() { IdName = n });

            jqObj.Options = option;
            jqObj.Options.delay = 500;
            return jqObj;
        }

        #endregion

        #region Tree Handle
        public static MvcHtmlString TreeHelp(this HtmlHelper helper, String Id)
        {
            JSTreeHandle jt = new JSTreeHandle(Id);
            jt.jsTree = new JSTree();
            jt.jsTree.plugins = new Plugins[] { Plugins.themes, Plugins.json_data, Plugins.ui };
            jt.jsTree.json_data = new Json_Data();
            jt.jsTree.json_data.ajax = new ProcCore.JqueryHelp.AjaxHelp.ajaxObject();
            jt.jsTree.json_data.ajax.url = "ajax_Tree";
            jt.jsTree.json_data.ajax.dataType = "json";
            jt.jsTree.json_data.ajax.contentType = null;
            jt.jsTree.json_data.ajax.type = "get";
            jt.jsTree.json_data.ajax.async = true;

            jt.jsTree.json_data.ajax.data_Func = new funcMethodModule();
            jt.jsTree.json_data.ajax.data_Func.parmsRange = new String[] { "n" };
            jt.jsTree.json_data.ajax.data_Func.funcString = "return { id : n.attr ? n.attr(\"id\") : 0 }; ";
            jt.jsTree.json_data.ajax.data_Func.MakeStyle = funcMethodModule.funcMakeStyle.funcConext;

            jt.jsTree.json_data.ajax.error = new funcMethodModule();
            jt.jsTree.json_data.ajax.error.MakeStyle = funcMethodModule.funcMakeStyle.funcConext;
            jt.jsTree.json_data.ajax.error.funcString = "alert(errorThrown)";

            jt.jsBind = new List<JSBind>() { new JSBind() { JsEvent = JSBindEvent.select_node, FunctionString = "var nodeId = data.rslt.obj.data(\"id\");$(\"#productkind\").val(nodeId);Call_Ajax_A(nodeId);" } };

            return MvcHtmlString.Create(jt.ToScriptString());
        }
        #endregion

        /// <summary>
        /// 資源檔圖檔協助器。
        /// </summary>
        public static MvcHtmlString GetImageFor(this HtmlHelper helper, Bitmap bitmap, string AltText = "")
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            String base64 = Convert.ToBase64String(stream.ToArray());

            return new MvcHtmlString(String.Format("<img src=\"data:image/gif;base64,{0}\" alt=\"{1}\" />", base64, AltText));
        }

        public static String SetCommCKEditor2(this HtmlHelper helper, String Id, Boolean UseCKFinder)
        {
            CKEditor ck = new CKEditor(new jqSelector() { IdName = Id });

            ck.Options.height = 320;
            ck.Options.toolbar = new CKEditor.Toolbar[]{
                new CKEditor.Toolbar(){name=EditBarNames.basicstyles,items=new EditFun[]{EditFun.FontSize, EditFun.Bold, EditFun.Italic,  EditFun.Dot_}},
                new CKEditor.Toolbar(){name=EditBarNames.paragraph,items=new EditFun[]{EditFun.NumberedList, EditFun.BulletedList, EditFun.Dot_, }},
                new CKEditor.Toolbar(){name=EditBarNames.tools,items=new EditFun[]{EditFun.Maximize, EditFun.Dot_, EditFun.Image,EditFun.Table}},
                new CKEditor.Toolbar(){name=EditBarNames.styles,items=new EditFun[]{EditFun.Styles, EditFun.Format}},
                new CKEditor.Toolbar(){name=EditBarNames.links,items=new EditFun[]{ EditFun.Link, EditFun.Unlink, EditFun.Anchor}},
                new CKEditor.Toolbar(){name=EditBarNames.colors,items=new EditFun[]{ EditFun.TextColor, EditFun.BGColor}},
                new CKEditor.Toolbar(){name=EditBarNames.editing,items=new EditFun[]{ }},
                new CKEditor.Toolbar(){name=EditBarNames.document,items=new EditFun[]{ EditFun.Source, EditFun.Dot_, EditFun.DocProps}},
                new CKEditor.Toolbar(){name=EditBarNames.clipboard,items=new EditFun[]{ EditFun.Cut, EditFun.Copy, EditFun.Paste, EditFun.PasteText, EditFun.PasteFromWord, EditFun.Undo, EditFun.Redo}}
            };

            if (UseCKFinder)
            {
                ck.Options.filebrowserBrowseUrl = "../../_Code/ckfinder_aspnet_2.3.1/ckfinder.html";
                ck.Options.filebrowserImageBrowseUrl = "../../_Code/ckfinder_aspnet_2.3.1/ckfinder.html?type=Images";
                ck.Options.filebrowserFlashBrowseUrl = "../../_Code/ckfinder_aspnet_2.3.1/ckfinder.html?type=Flash";
                ck.Options.filebrowserUploadUrl = "../../_Code/ckfinder_aspnet_2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files";
                ck.Options.filebrowserImageUploadUrl = "../../_Code/ckfinder_aspnet_2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
                ck.Options.filebrowserFlashUploadUrl = "../../_Code/ckfinder_aspnet_2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash";
                ck.Options.filebrowserWindowWidth = 1000;
                ck.Options.filebrowserWindowHeight = 700;
            }
            return ck.ToScriptString();
        }
        public static String AddressScript(this HtmlHelper helper, String ZipId, String CityId, String CountyId, String CityValue, String CountyValue)
        {
            AddressAjaxHandle A = new AddressAjaxHandle(CityId);
            A.options.countyElement = new jqSelector() { IdName = CountyId };
            A.options.zipElement = new jqSelector() { IdName = ZipId };
            A.options.cityValue = CityValue;
            A.options.countyValue = CountyValue;
            return A.ToScriptString();
        }

        /// <summary>
        ///  &lt;label for=""&gt;&lt;span class=""&gt;本文&lt;/span&gt;&lt;/label&gt;
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString LabelField<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String t)
        {
            return LabelField(h, ExpressionHelper.GetExpressionText(e), t);
        }
        public static MvcHtmlString LabelField(this HtmlHelper h, String n, String t)
        {
            return MvcHtmlString.Create(String.Format("<label for=\"{0}\">{1}</label>", n, t));
        }

        public static MvcHtmlString LabelHiddenField<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        {
            String n = ExpressionHelper.GetExpressionText(e);
            Object t = h.ViewData.Model.GetType().GetProperty(n).GetValue(h.ViewData.Model, null);

            if (t != null)
            {
                return LabelHiddenField(h, ExpressionHelper.GetExpressionText(e), t.ToString());
            }
            else {
                return LabelHiddenField(h, ExpressionHelper.GetExpressionText(e), "");
            }
        }
        public static MvcHtmlString LabelHiddenField<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String t)
        {
            return LabelHiddenField(h, ExpressionHelper.GetExpressionText(e), t);
        }
        public static MvcHtmlString LabelHiddenField(this HtmlHelper h, String n, String t)
        {
            return MvcHtmlString.Create(String.Format("<span>{1}</span><input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\">", n, t));
        }

        public static MvcHtmlString LabelDateField<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        {
            String n = ExpressionHelper.GetExpressionText(e);
            Object t = h.ViewData.Model.GetType().GetProperty(n).GetValue(h.ViewData.Model, null);
            if(t!=null){
             return    LabelHiddenField(h, ExpressionHelper.GetExpressionText(e), t.CDateTime().ToString("yyyy/MM/dd"));
            }else{
             return  LabelHiddenField(h, ExpressionHelper.GetExpressionText(e), "");
            }
        }

        /// <summary>
        /// 屬性及取值範例
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="h"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static MvcHtmlString SpanText<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        {
            String n = ExpressionHelper.GetExpressionText(e);
            var v = h.ViewData.Model.GetType().GetProperty(n).GetValue(h.ViewData.Model, null);
            return MvcHtmlString.Create(String.Format("<span id=\"{0}\">{1}</span>", n, v));
        }
        public static MvcHtmlString SpanText<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String v)
        {
            String n = ExpressionHelper.GetExpressionText(e);
            return MvcHtmlString.Create(String.Format("<span id=\"{0}\">{1}</span>", n, v));
        }

        public static MvcHtmlString TextHiddenField<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String v, String t)
        {
            return TextHiddenField(h, ExpressionHelper.GetExpressionText(e), v, t);
        }
        public static MvcHtmlString TextHiddenField(this HtmlHelper h, String n, String v, String t)
        {
            return MvcHtmlString.Create(String.Format("<input type=\"text\" id=\"ex_{0}\" name=\"ex_{0}\" value=\"{2}\"><input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\">", n, v, t));
        }

        public static FormValidateModels FieldsRuleSetup<TModel, TProperty>(this HtmlHelper<TModel> h,
        Expression<Func<TModel, TProperty>> e,
         FieldRule r, FieldMessage m, Dictionary<String, String> xr)
        {
            return FieldsRuleSetup(h, ExpressionHelper.GetExpressionText(e), r, m, xr);
        }

        public static FormValidateModels FieldsRuleSetup(this HtmlHelper h, String f, FieldRule r, FieldMessage m, Dictionary<String, String> xr)
        {
            FormValidateModels o = new FormValidateModels() { fieldName = f };
            if (r != null)
                o.rules = r;
            if (m != null)
                o.messages = m;
            if (xr != null)
                o.ExtraRule = xr;

            return o;
        }
        public static funcMethodModule CommSetFormOnSuccesss(this HtmlHelper h)
        {
            funcMethodModule func = new funcMethodModule() { funcName = "$.FormResultJson", MakeStyle = funcMethodModule.funcMakeStyle.jqfunc };
            func.funcParam.Add("response");
            func.funcString = @"
    var jsonobj = jQuery.parseJSON(response);
    if (jsonobj.result)
        $('#EditType').val('Modify');
    $.UiMessage(jsonobj);";

            return func;
        }
        public static MvcHtmlString OpenDialogPlugin(this HtmlHelper helper, String Title, String Message)
        {
            if (Message != "")
            {
                String s = "$('#dialog').html('" + Message + @"');"
                        + "$('#dialog').dialog({ title: '" + Title + "'})";

                TagBuilder script = new TagBuilder("script");
                script.MergeAttribute("type", "text/javascript");
                script.InnerHtml = (s + ";").ToJqueryDocumentReady();
                return MvcHtmlString.Create(script.ToString());
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }
    }
    public static class jqGridHelper
    {
    }
    public static class jqValue
    {
        public static String Prefix { get; set; }
        public static String jqJsonObjectString { get; set; }
        /// <summary>
        /// 處理指定板型為：$('#{1}').val({2}.{0})
        /// jsonobj.data
        /// </summary>
        public static MvcHtmlString valJ<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(expression);
            String[] s = inputName.Split('.');

            if (jqJsonObjectString == null || jqJsonObjectString == "")
                jqJsonObjectString = "jsonobj.data";

            String tpl = "$('#{1}').val({2}.{0});";
            if (s.Length > 1)
                return MvcHtmlString.Create(String.Format(tpl, s[s.Length - 1], Prefix + s[s.Length - 1], jqJsonObjectString));
            else
                return MvcHtmlString.Create(String.Format(tpl, s[0], Prefix + s[0], jqJsonObjectString));

        }
        /// <summary>
        /// 處理指定板型為：$('#{1}').val($.parseMsJsonDate({2}.{0}))
        /// jsonobj.data
        /// </summary>
        public static MvcHtmlString valJD<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(expression);
            String[] s = inputName.Split('.');

            if (jqJsonObjectString == null || jqJsonObjectString == "")
                jqJsonObjectString = "jsonobj.data";

            String tpl = "$('#{1}').val($.parseMsJsonDate({2}.{0}));";
            if (s.Length > 1)
                return MvcHtmlString.Create(String.Format(tpl, s[s.Length - 1], Prefix + s[s.Length - 1], jqJsonObjectString));
            else
                return MvcHtmlString.Create(String.Format(tpl, s[0], Prefix + s[0], jqJsonObjectString));
        }
        public static MvcHtmlString valTD<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(expression);
            String[] s = inputName.Split('.');

            if (jqJsonObjectString == null || jqJsonObjectString == "")
                jqJsonObjectString = "jsonobj.data";

            String tpl = "$('#{0}{1}').text($.parseMsJsonDate({2}.{1}));$('#hid_{0}{1}').val($.parseMsJsonDate({2}.{1}));";
            if (s.Length > 1)
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[s.Length - 1], jqJsonObjectString));
            else
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[0], jqJsonObjectString));

        }
        public static MvcHtmlString valT<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(expression);
            String[] s = inputName.Split('.');

            if (jqJsonObjectString == null || jqJsonObjectString == "")
                jqJsonObjectString = "jsonobj.data";

            String tpl = "$('#{0}{1}').text({2}.{1});$('#hid_{0}{1}').val({2}.{1});";
            if (s.Length > 1)
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[s.Length - 1], jqJsonObjectString));
            else
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[0], jqJsonObjectString));

        }
        public static MvcHtmlString fdlLabel<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, Object value) where TModel : class
        {
            String inputName = ExpressionHelper.GetExpressionText(expression);
            String[] s = inputName.Split('.');

            String tpl = "<label id=\"{0}{1}\">{2}</label><input type=\"hidden\" id=\"hid_{0}{1}\" name=\"{1}\" value=\"{2}\">";
            if (s.Length > 1)
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[s.Length - 1], value == null ? "" : value.ToString()));
            else
                return MvcHtmlString.Create(String.Format(tpl, Prefix, s[0], value == null ? "" : value.ToString()));
        }

        /// <summary>
        /// reutrn $('#s').val()
        /// </summary>
        public static MvcHtmlString GVal<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(expression);
            return MvcHtmlString.Create("$('#" + n + "').val()");
        }
    }
    public class MakeColumnModule
    {
        public MakeColumnModule()
        {
            CM = new jqGrid.colObject();
        }
        /// <summary>
        /// Grid欄位顯示的名稱
        /// </summary>
        public String CN { get; set; }
        /// <summary>
        /// Grid的Jquery ColumnModel
        /// </summary>
        public jqGrid.colObject CM { get; set; }
    }
}