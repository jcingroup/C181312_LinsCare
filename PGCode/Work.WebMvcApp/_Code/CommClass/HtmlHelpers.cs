using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Resources;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.WebPages;

using ProcCore.JqueryHelp;
using ProcCore.JqueryHelp.AjaxFilesUpLoadHelp;
using ProcCore.JqueryHelp.AjaxFormObj;
using ProcCore.JqueryHelp.CustomButton;
using ProcCore.JqueryHelp.FormvValidate;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.NetExtension;
using ProcCore.DatabaseCore.TableFieldModule;

namespace DotWeb.Helpers
{
    public static class HtmlWebObject
    {
        #region JQuery Ui framework ICON section
        private static String ConvertIcornString(FrameworkIcons icons)
        {
            return icons.ToString().Replace("_", "-");
        }

        public static MvcHtmlString ButtonScript(this HtmlHelper helper, jqButton button)
        {
            return MvcHtmlString.Create(button.ToScriptString());
        }
        public static MvcHtmlString FrameworkButton(this HtmlHelper helper, String Id, String text, FrameworkIcons icons)
        {
            jqButton button = new jqButton(new jqSelector() { IdName = Id });
            button.options.icons.primary = icons;
            return MvcHtmlString.Create("<button id=\"" + Id + "\" type=\"button\">" + text + "</button><script type=\"text/javascript\">" + button.ToScriptString() + "</script>");
        }

        public static MvcHtmlString GetFrameworkICON(this HtmlHelper helper, String Id, FrameworkIcons icons, String title)
        {
            String html = @"<span id=""{0}"" class=""ui-icon {1}"" title=""{2}"" ></span>";
            return MvcHtmlString.Create(String.Format(html, Id, icons.ToString().Replace("_", "-"), title));
        }
        public static MvcHtmlString FrameworkICON(this HtmlHelper helper, FrameworkIcons icons, String title)
        {
            String html = "<span class=\"ui-icon {0}\" title=\"{1}\" ></span>";
            return MvcHtmlString.Create(String.Format(html, icons.ToString().Replace("_", "-"), title));
        }

        public static MvcHtmlString GetNote(this HtmlHelper helper)
        {
            //String html = @"<span class=""note"" title=""{0}"" >＊</span>";
            String html = "<span class=\"ui-state-highlight ui-icon " + FrameworkIcons.ui_icon_star.ToString().Replace("_", "-") + "\" title=\"{0}\" ></span>";
            return MvcHtmlString.Create(String.Format(html, Resources.Res.Info_StarMustEdit));
        }
        public static MvcHtmlString SetMemo(this HtmlHelper helper, String Memo)
        {
            //String html = @"<span class=""note"" title=""{0}"" >＊</span>";
            String html = "<span class=\"ui-state-highlight ui-icon " + FrameworkIcons.ui_icon_help.ToString().Replace("_", "-") + "\" title=\"{0}\" ></span>";
            return MvcHtmlString.Create(String.Format(html, Memo));
        }
        #endregion

        #region AjaxForm and Rules Setup section
        public static MvcHtmlString AjaxFormSearch(this HtmlHelper helper, String FormId, String GridId, String SubmitEleName, Boolean UseDefault)
        {
            AjaxFormObject ajaxForm = new AjaxFormObject(FormId);
            if (UseDefault)
            {
                ajaxForm.SubmitElementId = SubmitEleName;
                ajaxForm.SubmitEvent = HtmlObjectEvent.click;
                ajaxForm.options = new FormAjaxOption();

                ajaxForm.options.beforeSubmit = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                ajaxForm.options.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };

                ajaxForm.options.dataType = "json";
                ajaxForm.options.beforeSubmit.funcString = "var queryString = $.param(formData);return true;";

                if (GridId != null)
                {
                    ajaxForm.options.success.funcString = @"
                        var GetGridMaster = jQuery('#" + GridId + @"')[0];
                        GetGridMaster.addJSONData(jsonobj);
                        //jQuery('#" + GridId + @"').editGridRow( 24 );
                        if(jsonobj.records==0){jsonobj.message = '" + Resources.Res.Search_No_Data + "';$.UiMessage(jsonobj);GetGridMaster=null;jsonobj=null}";

                }
            }

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("type", "text/javascript");
            script.InnerHtml = ajaxForm.ToScriptString();
            return MvcHtmlString.Create(script.ToString());
        }
        public static MvcHtmlString AjaxFormPrint(this HtmlHelper helper, String FormId, String GridId, String SubmitEleName, Boolean UseDefault)
        {
            AjaxFormObject ajaxForm = new AjaxFormObject(FormId);
            if (UseDefault)
            {
                ajaxForm.SubmitElementId = SubmitEleName;
                ajaxForm.SubmitEvent = HtmlObjectEvent.click;
                ajaxForm.options = new FormAjaxOption();

                ajaxForm.options.beforeSubmit = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                ajaxForm.options.success = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };

                ajaxForm.options.dataType = "json";
                ajaxForm.options.beforeSubmit.funcString = "var queryString = $.param(formData);return true;";

                if (GridId != null)
                {
                    ajaxForm.options.success.funcString = @"
                        var GetGridMaster = jQuery('#" + GridId + @"')[0];
                        GetGridMaster.addJSONData(jsonobj);
                        //jQuery('#" + GridId + @"').editGridRow( 24 );
                        if(jsonobj.records==0){jsonobj.message = '" + Resources.Res.Search_No_Data + "';$.UiMessage(jsonobj);GetGridMaster=null;jsonobj=null}";

                }
            }

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("type", "text/javascript");
            script.InnerHtml = ajaxForm.ToScriptString();
            return MvcHtmlString.Create(script.ToString());
        }
        public static String FM_BFR_Hdl_CKEdit(this HtmlHelper h)
        {
            return "for ( instance in CKEDITOR.instances ) CKEDITOR.instances[instance].updateElement();";
        }
        
        #endregion
    }
    public static class jqGridHelper
    {
        #region UI jqGrid Section
        private static String ConvertIcornString(FrameworkIcons icons)
        {
            return icons.ToString().Replace("_", "-");
        }
        #region Grid Help 2.0

        public static jqGrid jqGridMobile_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            return jqGridMobile_Standard(helper, Id, Caption, Page, AddQuery, DataUrl, DelUrl, AddUrl, Height, 10, subGrid, GridColumnSetup);
        }

        public static jqGrid jqGridMobile_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, int rowNum, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            //新增按鍵 路徑
            String f = @"var getQuery = $.CollectQuery();document.location.href = '{0}?' + $.pageQuery('" + Id + @"') + (getQuery == '' ? '' : '&' + getQuery);";
            f = String.Format(f, AddUrl);

            List<String> colNames = new List<String>();
            List<jqGrid.colObject> colModel = new List<jqGrid.colObject>();
            //處理Column設定
            foreach (var c in GridColumnSetup)
            {
                if (c.CM.AssignFormatter != null)
                {
                    if (c.CM.formatter == null) c.CM.formatter = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
                    c.CM.formatter.funcName = c.CM.AssignFormatter.func.funcName;
                }
                colNames.Add(c.CN);
                colModel.Add(c.CM);
            }

            jqGrid jd = new jqGrid()
            {
                Id = Id,
                GridModule = new jqGrid.gridMasterObject()
                {
                    colModel = colModel.ToArray(),
                    colNames = colNames.ToArray(),
                    caption = Caption,
                    pager = "pager",
                    url = DataUrl + (AddQuery == "" ? "" : "?") + AddQuery,
                    page = Page == null ? 1 : int.Parse(Page),
                    autowidth = true,
                    multiboxonly = false,
                    multiselect = false,
                    height = Height,
                    scroll = false,
                    search = true,
                    gridview = true,
                    viewrecords = true,
                    hoverrows = false,
                    rowNum = rowNum,
                    loadError = new funcMethodModule()
                    {
                        funcString = @"
						try {
							jQuery.jgrid.info_dialog(jQuery.jgrid.errors.errcap,'<div class=""ui-state-error"">'+ xhr.responseText +'</div>', jQuery.jgrid.edit.bClose,
                            {buttonalign:'right'});	} catch(e) {alert(xhr.responseText);}",
                        MakeStyle = funcMethodModule.funcMakeStyle.funcConext
                    },
                    gridComplete = new funcMethodModule()
                    {
                        funcString = "$('.divGridFunctionButton').trigger('create');",
                        MakeStyle = funcMethodModule.funcMakeStyle.funcConext
                    }
                },
                NavGridModule = new jqGrid.navGridObject()
                {
                    navOption = new jqGrid.navGridObject.navGridOption()
                    {
                        alerttext = Resources.Res.Select_Delete_Data,
                        del = true,
                        deltitle = Resources.Res.Info_Delete_Data,
                        deltext = Resources.Res.Button_Delete,
                        refresh = true,
                        add = false,
                        edit = false,
                        search = true
                    },
                    Del = new jqGrid.navGridObject.editGridRow() { url = DelUrl, msg = Resources.Res.IsSureDelete },
                    Add = new jqGrid.navGridObject.editGridRow(),
                    Edit = new jqGrid.navGridObject.editGridRow()
                }
                ,
                navCustomButtons =
                    new jqGrid.navButtonAddModule[] { 
                    new jqGrid.navButtonAddModule {    caption = Resources.Res.Button_Insert, 
                                                                    buttonicon = "plus",
                                                                    onClickButton=new funcMethodModule{funcString=f, MakeStyle= funcMethodModule.funcMakeStyle.funcConext},
                                                                    position="last" ,
                                                                }
                }
            };

            if (subGrid != null)
            {
                jd.GridModule.subGrid = true;
                jd.GridModule.ExpandColClick = true;
                jd.GridModule.subGridRowExpanded = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                jd.GridModule.subGridRowExpanded.funcString = "var subgrid_table_id;var subgrid_pager_id;subgrid_table_id = pID+ '_t';subgrid_pager_id = pID + '_p';"
  + "jQuery(\"#\"+pID).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table><div id='\" + subgrid_pager_id + \"'></div>\");";
                subGrid.ToScriptHandle();
                jd.GridModule.subGridRowExpanded.funcString += subGrid.jqGridScript;
            }
            return jd;
        }
        /// <summary>
        /// 標準式Grid，導入客制編輯畫面，新增及修改Button為特殊設定
        /// </summary>
        public static jqGrid jqGrid_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            return jqGrid_Standard(helper, Id, Caption, Page, AddQuery, DataUrl, DelUrl, AddUrl, Height, 10, subGrid, GridColumnSetup);
        }

        public static jqGrid jqGrid_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, int rowNum, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            //新增按鍵 路徑
            String f = @"var getQuery = $.CollectQuery();document.location.href = '{0}?' + $.pageQuery('" + Id + @"') + (getQuery == '' ? '' : '&' + getQuery);";
            f = String.Format(f, AddUrl);

            List<String> colNames = new List<String>();
            List<jqGrid.colObject> colModel = new List<jqGrid.colObject>();
            //處理Column設定
            foreach (var c in GridColumnSetup)
            {
                if (c.CM.AssignFormatter != null)
                {
                    if (c.CM.formatter == null) c.CM.formatter = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
                    c.CM.formatter.funcName = c.CM.AssignFormatter.func.funcName;
                }
                colNames.Add(c.CN);
                colModel.Add(c.CM);
            }

            jqGrid jd = new jqGrid()
            {
                Id = Id,
                GridModule = new jqGrid.gridMasterObject()
                {
                    colModel = colModel.ToArray(),
                    colNames = colNames.ToArray(),
                    caption = Caption,
                    pager = "pager",
                    url = DataUrl + (AddQuery == "" ? "" : "?") + AddQuery,
                    page = Page == null ? 1 : int.Parse(Page),
                    autowidth = true,
                    multiboxonly = true,
                    height = Height,
                    rowNum = rowNum,
                    viewrecords = true
                },
                NavGridModule = new jqGrid.navGridObject()
                {
                    navOption = new jqGrid.navGridObject.navGridOption()
                    {
                        alerttext = Resources.Res.Select_Delete_Data,
                        del = true,
                        deltitle = Resources.Res.Info_Delete_Data,
                        deltext = Resources.Res.Button_Delete,
                        refresh = true,
                        add = false,
                        edit = false,
                        search = false
                    },
                    Del = new jqGrid.navGridObject.editGridRow() { url = DelUrl, msg = Resources.Res.IsSureDelete },
                    Add = new jqGrid.navGridObject.editGridRow(),
                    Edit = new jqGrid.navGridObject.editGridRow()
                },
                navCustomButtons =
                    new jqGrid.navButtonAddModule[] { 
                    new jqGrid.navButtonAddModule {    caption = Resources.Res.Button_Insert, 
                                                                    buttonicon = "ui-icon " + ConvertIcornString(FrameworkIcons.ui_icon_plusthick),
                                                                    onClickButton=new funcMethodModule{funcString=f, MakeStyle= funcMethodModule.funcMakeStyle.funcConext},
                                                                    position="last" ,
                                                                }
                }
            };

            if (subGrid != null)
            {
                jd.GridModule.subGrid = true;
                jd.GridModule.ExpandColClick = true;
                jd.GridModule.subGridRowExpanded = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                jd.GridModule.subGridRowExpanded.funcString = "var subgrid_table_id;var subgrid_pager_id;subgrid_table_id = pID+ '_t';subgrid_pager_id = pID + '_p';"
  + "jQuery(\"#\"+pID).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table><div id='\" + subgrid_pager_id + \"'></div>\");";
                subGrid.ToScriptHandle();
                jd.GridModule.subGridRowExpanded.funcString += subGrid.jqGridScript;
            }
            return jd;
        }


        public static MvcHtmlString mhs_jqGrid_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            return MvcHtmlString.Create(jqGrid_Standard(helper, Id, Caption, Page, AddQuery, DataUrl, DelUrl, AddUrl, Height, 10, subGrid, GridColumnSetup).ToScriptString());
        }

        public static MvcHtmlString mhs_jqGrid_Standard(this HtmlHelper helper, String Id, String Caption, String Page,
    String AddQuery, String DataUrl, String DelUrl, String AddUrl, int Height, int rowNum, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            return MvcHtmlString.Create(jqGrid_Standard(helper, Id, Caption, Page, AddQuery, DataUrl, DelUrl, AddUrl, Height, rowNum, subGrid, GridColumnSetup).ToScriptString());
        }


        /// <summary>
        /// 採用jqGrid編輯功能，不需導入Edit頁面。
        /// </summary>
        public static jqGrid jqGrid_Edit(this HtmlHelper helper, String Id, String Caption, String Page,
     String DataUrl, String DelUrl, String UpdateUrl, int Height, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {

            String f = @"   var getQuery = $.CollectQuery();document.location.href = '{0}?' + $.pageQuery('" + Id + @"') + (getQuery == '' ? '' : '&' + getQuery);";

            List<String> colNames = new List<String>();
            List<jqGrid.colObject> colModel = new List<jqGrid.colObject>();
            //處理Column設定
            foreach (var c in GridColumnSetup)
            {
                if (c.CM.AssignFormatter != null)
                {
                    if (c.CM.formatter == null) c.CM.formatter = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
                    c.CM.formatter.funcName = c.CM.AssignFormatter.func.funcName;
                }
                colNames.Add(c.CN);
                colModel.Add(c.CM);
            }

            jqGrid jd = new jqGrid()
            {
                Id = Id,
                GridModule = new jqGrid.gridMasterObject()
                {
                    caption = Caption,
                    pager = "pager_" + Id,
                    url = DataUrl,
                    page = Page == null ? 1 : int.Parse(Page),
                    autowidth = true,
                    height = Height > 0 ? Height : 320,
                    multiselect = true,
                    multiboxonly = true,
                    colModel = colModel.ToArray(),
                    colNames = colNames.ToArray()
                },
                NavGridModule = new jqGrid.navGridObject()
                {
                    navOption = new jqGrid.navGridObject.navGridOption()
                    {
                        del = true,
                        deltitle = Resources.Res.Info_Delete_Data,
                        deltext = Resources.Res.Button_Delete,
                        refresh = true,
                        add = true,
                        addtext = Resources.Res.Button_Insert,
                        edit = true,
                        edittext = Resources.Res.Button_Modify,
                        search = false,
                    },
                    Add = new jqGrid.navGridObject.editGridRow() { url = UpdateUrl, closeAfterAdd = true },
                    Edit = new jqGrid.navGridObject.editGridRow() { url = UpdateUrl },
                    Del = new jqGrid.navGridObject.editGridRow() { url = DelUrl, msg = Resources.Res.IsSureDelete }
                }
            };

            if (subGrid != null)
            {
                jd.GridModule.subGrid = true;
                jd.GridModule.ExpandColClick = true;
                jd.GridModule.subGridRowExpanded = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                jd.GridModule.subGridRowExpanded.funcString = "var subgrid_table_id;var subgrid_pager_id;subgrid_table_id = pID+ '_t';subgrid_pager_id = pID + '_p';"
  + "jQuery(\"#\"+pID).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table><div id='\" + subgrid_pager_id + \"'></div>\");";
                subGrid.ToScriptHandle();
                jd.GridModule.subGridRowExpanded.funcString += subGrid.jqGridScript;
            }

            return jd;
        }

        /// <summary>
        /// 採用jqGrid編輯功能，不需導入Edit頁面。
        /// </summary>
        public static MvcHtmlString mhs_jqGrid_Edit(this HtmlHelper helper, String Id, String Caption, String Page,
     String DataUrl, String DelUrl, String UpdateUrl, int Height, jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {
            return MvcHtmlString.Create(jqGrid_Edit(helper, Id, Caption, Page, DataUrl, DelUrl, UpdateUrl, Height, subGrid, GridColumnSetup).ToScriptString());
        }

        /// <summary>
        /// SubGrid的Id 由主Grid提供
        /// </summary>
        public static jqGrid jqSubGrid(this HtmlHelper helper, String DataUrl, String DeleteActionUrl, String AddNewEditUrl, int? Height,
            jqGrid subGrid, params MakeColumnModule[] GridColumnSetup)
        {

            List<String> colNames = new List<String>();
            List<jqGrid.colObject> colModel = new List<jqGrid.colObject>();

            //處理Column設定
            foreach (var col in GridColumnSetup)
            {
                if (col.CM.AssignFormatter != null)
                {
                    if (col.CM.formatter == null) col.CM.formatter = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.onlyName };
                    col.CM.formatter.funcName = col.CM.AssignFormatter.func.funcName;
                }
                colNames.Add(col.CN);
                colModel.Add(col.CM);
            }

            jqGrid jd = new jqGrid()
            {
                GridIdWorkTosubGridRowExpanded = true,
                GridModule = new jqGrid.gridMasterObject()
                {
                    url = DataUrl,
                    multiboxonly = true,
                    postData = new DataModule(),
                    colModel = colModel.ToArray(),
                    colNames = colNames.ToArray(),
                    height = Height
                },
                NavGridModule = new jqGrid.navGridObject()
                {
                    navOption = new jqGrid.navGridObject.navGridOption() { del = true, edit = false, add = false, search = false, refresh = false, deltitle = Resources.Res.Info_Delete_Data, deltext = Resources.Res.Button_Delete },
                    Del = new jqGrid.navGridObject.editGridRow() { url = DeleteActionUrl, msg = Resources.Res.IsSureDelete },
                    Add = new jqGrid.navGridObject.editGridRow() { url = AddNewEditUrl },
                    Edit = new jqGrid.navGridObject.editGridRow() { url = AddNewEditUrl }
                }
            };

            if (subGrid != null)
            {
                jd.GridModule.subGrid = true;
                jd.GridModule.ExpandColClick = true;
                jd.GridModule.subGridRowExpanded = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
                jd.GridModule.subGridRowExpanded.funcString = "var subgrid_table_id;var subgrid_pager_id;subgrid_table_id = pID+ '_t';subgrid_pager_id = pID + '_p';"
  + "jQuery(\"#\"+pID).html(\"<table id='\"+subgrid_table_id+\"' class='scroll'></table><div id='\" + subgrid_pager_id + \"'></div>\");";
                subGrid.ToScriptHandle();
                jd.GridModule.subGridRowExpanded.funcString += subGrid.jqGridScript;
            }

            return jd;
        }
        #endregion

        /// <summary>
        /// formatter要使用自訂function所要加的參數 "cellValue", "options", "rowObject"
        /// </summary>
        public static String GridIDColumnCommScriptContext(this HtmlHelper helper, String GridId, String LinkActionName)
        {
            //Script 的參數 "cellValue", "options", "rowObject"
            String 修改樣版 = "var cellHtml = '<a href=\"" + LinkActionName + "?Id=' + jQuery.trim(options.rowId) + '&' + $.pageQuery('" + GridId + "') + '&' + getQuery + '\">{0}</a>';";
            修改樣版 = String.Format(修改樣版, "<div class=\"ui-state-default ui-corner-all\" style=\"height:24px;width:24px;margin:1px\"><span class=\"ui-icon " + ConvertIcornString(FrameworkIcons.ui_icon_pencil) + "\" style=\"margin:3px\"></span></div>");
            String s = "var getQuery = $.CollectQuery();" + 修改樣版 + "return cellHtml;";
            return s;
        }

        /// <summary>
        /// formatter要使用自訂function所要加的參數 "cellValue", "options", "rowObject"
        /// </summary>
        public static String MobileGridIDColumnCommScript(this HtmlHelper helper, String GridId, String LinkActionName)
        {
            //Script 的參數 "cellValue", "options", "rowObject"
            //String 修改樣版 = "var cellHtml = '<a data-role=\"button\" href=\"" + LinkActionName + "?Id=' + jQuery.trim(options.rowId) + '&' + $.pageQuery('" + GridId + "') + '&' + getQuery + '\">{0}</a>';";
            String 修改樣版 = "var cellHtml = '<div class=\"divGridFunctionButton\"><button type=\"button\" class=\"RedireModify\" action=\"" + LinkActionName + "\" sn=\"' + options.rowId + '\" page=\"' + $.pageQuery('" + GridId + "') + '\" query=\"' + getQuery + '\">修改</button></div>';";
            //修改樣版 = String.Format(修改樣版, "<span class=\"ui-icon " + ConvertIcornString(FrameworkIcons.ui_icon_pencil) + "\"></span>");
            修改樣版 = String.Format(修改樣版, "修改");
            String s = "var getQuery = $.CollectQuery();" + 修改樣版 + "return cellHtml;";
            return s;
        }

        /// <summary>
        /// 格式：setGridParam({postData: {s_title:$('#s_title').val()}});
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="h"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static MvcHtmlString setGridParam_postData<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        {
            String n = ExpressionHelper.GetExpressionText(e);
            return MvcHtmlString.Create("setGridParam({postData: {" + n + ":$('#" + n + "').val()}})");
        }

        public static String ls_To_Options(this HtmlHelper helper, List<SelectListItem> l)
        {
            List<String> n = new List<string>();
            foreach (SelectListItem s in l)
            {
                n.Add(s.Value + ":" + s.Text);
            }
            return n.ToArray().JoinArray(";");
        }

        #endregion
    }
    public static class SysFilesHelp
    {
        private static String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
        private static String SystemUpImagePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";

        #region File List Helper
        public static String[] FilesAppendInfo(this HtmlHelper h, String Area, String Controller, Int32 Id)
        {
            String SearchPath = String.Format(SystemUpFilePathTpl, Area, Controller, Id, "DocFiles", "OriginFile");
            String FolderPth = h.ViewContext.HttpContext.Server.MapPath(SearchPath);

            List<String> fiInfo = new List<String>();

            if (Directory.Exists(FolderPth))
            {
                String[] fs = Directory.GetFiles(FolderPth);

                foreach (String f in fs)
                {
                    FileInfo fi = new FileInfo(f);
                    fiInfo.Add(fi.Name);
                }
            }
            return fiInfo.ToArray();
        }
        public static String[] FilesAppendInfo(this HtmlHelper h, String Area, String Controller, Int32 Id, String FilesKind)
        {
            String SearchPath = String.Format(SystemUpFilePathTpl, Area, Controller, Id, FilesKind, "OriginFile");
            String FolderPth = h.ViewContext.HttpContext.Server.MapPath(SearchPath);

            List<String> fiInfo = new List<String>();

            if (Directory.Exists(FolderPth))
            {
                String[] fs = Directory.GetFiles(FolderPth);

                foreach (String f in fs)
                {
                    FileInfo fi = new FileInfo(f);
                    fiInfo.Add(fi.Name);
                }
            }
            return fiInfo.ToArray();
        }

        public static String[] ImagesAppendInfo(this HtmlHelper h, String Area, String Controller, Int32 Id, String FilesKind)
        {
            return ImagesAppendInfo(h, Area, Controller, Id, FilesKind, "OriginFile");
        }
        public static String[] ImagesAppendInfo(this HtmlHelper h, String Area, String Controller, Int32 Id, String FilesKind, int SizeParm)
        {
            return ImagesAppendInfo(h, Area, Controller, Id, FilesKind, "s_" + SizeParm);
        }
        public static String[] ImagesAppendInfo(this HtmlHelper h, String Area, String Controller, Int32 Id, String FilesKind, String SizeKind)
        {
            String S = String.Format(SystemUpImagePathTpl, Area, Controller, Id, FilesKind, SizeKind);
            String F = h.ViewContext.HttpContext.Server.MapPath(S);
            List<String> I = new List<String>();
            if (Directory.Exists(F))
            {
                String[] fs = Directory.GetFiles(F);

                foreach (String f in fs)
                {
                    FileInfo fi = new FileInfo(f);
                    String P = UrlHelper.GenerateContentUrl(S + "/" + fi.Name, h.ViewContext.HttpContext);
                    I.Add(P);
                }
            }
            return I.ToArray();
        }

        public static String ImgSrc(this HtmlHelper h, String AreaName, String ContorllerName, Int32 Id, String FilesKind, Int32 ImageSizeTRype)
        {
            String ImgSizeString = "s_" + ImageSizeTRype;
            String SearchPath = String.Format(SystemUpFilePathTpl, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = h.ViewContext.HttpContext.Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return UrlHelper.GenerateContentUrl(SearchPath, h.ViewContext.HttpContext) + "/" + f.Name;
                }
                else
                    return null;
            }
            else
                return null;
        }
        public static String ImgSrc(this HtmlHelper h, String AreaName, String ContorllerName, Int32 Id, String FilesKind, Int32 ImageSizeTRype, String NoPicName)
        {
            String ImgSizeString = "s_" + ImageSizeTRype;
            String SearchPath = String.Format(SystemUpFilePathTpl, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = h.ViewContext.HttpContext.Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                //String[] SFiles = Directory.GetFiles(FolderPth);
                String[] SFiles = Directory.GetFiles(FolderPth).Where(x => !x.Contains(".db")).ToArray();

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return UrlHelper.GenerateContentUrl(SearchPath, h.ViewContext.HttpContext) + "/" + f.Name;
                }
                else
                {
                    return UrlHelper.GenerateContentUrl("~/Content/images", h.ViewContext.HttpContext) + "/" + NoPicName;
                }
            }
            else
            {
                return UrlHelper.GenerateContentUrl("~/Content/images", h.ViewContext.HttpContext) + "/" + NoPicName;
            }
        }

        #endregion

        #region File Upload UI Helper

        public static MvcHtmlString FileFineUpLoad(this HtmlHelper helper,
            String divId, String title,
            String OpenDilaogElementId,
            String ajax_url_UpLoadFiles,
            String ajax_url_ListFiles,
            String ajax_url_DeleteFiles,
            int SystemId, String queryFileKind, Int32 height, Int32 Width)
        {
            AjaxFineUpLoaderUI ajaxUI = new AjaxFineUpLoaderUI(divId, "fine_uploader_" + divId, OpenDilaogElementId, ajax_url_UpLoadFiles, ajax_url_ListFiles, ajax_url_DeleteFiles);

            ajaxUI.Options.element = "$('#fine_uploader_" + divId + "')";
            ajaxUI.Options.request.inputName = "hd_FileUp_EL";
            ajaxUI.Options.request.endpoint = ajax_url_UpLoadFiles;
            ajaxUI.Options.request._params = new DataModule();
            ajaxUI.Options.request._params.Add("id", SystemId.ToString());
            ajaxUI.Options.request._params.Add("FilesKind", "'" + queryFileKind + "'");
            ajaxUI.Options.callbacks = new AjaxFineUpLoaderUI.callbackOptons();
            //$(this).fineUploader('clearStoredFiles');
            ajaxUI.Options.callbacks.complete = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Options.callbacks.complete.funcString = String.Format("$.FilesListHandle('{0}_filesList','{1}','{2}',{3},'{4}');", divId, ajax_url_ListFiles, ajax_url_DeleteFiles, SystemId, queryFileKind);
            ajaxUI.Options.callbacks.complete.funcString += "if($(this).fineUploader('getInProgress')==0)";
            ajaxUI.Options.callbacks.complete.funcString += "{$(this).fineUploader('clearStoredFiles');}";

            ajaxUI.Options.callbacks.error = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Options.callbacks.error.funcString = "alert('file uploader error:' + errorReason);";
            ajaxUI.Options.text = new AjaxFineUpLoaderUI.textOptions() { uploadButton = "<i class='icon-plus icon-white'></i>" + Resources.Res.Button_Choice_Files };
            ajaxUI.Options.autoUpload = false;
            ajaxUI.Options.debug = true;
            ajaxUI.Options.failedUploadTextDisplay = new AjaxFineUpLoaderUI.failedUploadTextDisplayOptions() { mode = "custom", enableTooltip = true, responseProperty = "error", maxChars = 100 };

            ajaxUI.SubmitButton = new jqElementEvent(new jqSelector() { IdName = divId + "_SubmitFileButton" }) ;
            ajaxUI.SubmitButton.events.Add(new jqElementEvent.jqEvents() { htmlElementEvent= HtmlObjectEvent.click,
                                                                           funcString = "$('#fine_uploader_" + divId + "').fineUploader('uploadStoredFiles');"
            });

            jqButton btn_upd_fmwk = new jqButton(new jqSelector() { IdName = ajaxUI.SubmitButton.Id });
            btn_upd_fmwk.options.icons.primary = FrameworkIcons.ui_icon_arrowreturnthick_1_n;

            ajaxUI.Dialog.options.height = height;
            ajaxUI.Dialog.options.width = Width;

            ajaxUI.Dialog.options.open = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Dialog.options.open.funcString = String.Format("$.FilesListHandle('{0}_filesList','{1}','{2}',{3},'{4}');{5};", divId, ajax_url_ListFiles, ajax_url_DeleteFiles, SystemId, queryFileKind, btn_upd_fmwk.ToScriptString());
            ajaxUI.Dialog.options.zIndex = 2;

            StringBuilder sb_Html = new StringBuilder();
            sb_Html.AppendLine(String.Format("<div id=\"{0}\" title=\"{1}\">", divId, title));
            sb_Html.AppendLine("<div id=\"fine_uploader_" + divId + "\"></div>");
            sb_Html.AppendLine(String.Format("<button id=\"{0}\" type=\"button\">{1}</button>", ajaxUI.SubmitButton.Id, Resources.Res.Button_Start_FilesUp));
            sb_Html.AppendLine(String.Format("<fieldset><legend class=\"ui-state-active edit-subtitle-caption\" style=\"width:100%;font-size: 0.95em;padding:5px 5px 5px 5px;font-weight:normal\">{1}</legend><div id=\"{0}_filesList\"></div></fieldset>", divId, Resources.Res.Info_Files_List));
            sb_Html.AppendLine("</div>");

            return MvcHtmlString.Create(sb_Html.ToString() + ajaxUI.ToScriptString().ToJqueryDocumentReady().ToScriptTag());
        }

        public static MvcHtmlString ImageFineUpLoad(this HtmlHelper helper,
            String divId, String title,
            String OpenDilaogElementId,
            String ajax_url_UpLoadFiles,
            String ajax_url_ListFiles,
            String ajax_url_DeleteFiles,
            int SystemId, String queryFileKind, Int32 height, Int32 Width)
        {
            AjaxFineUpLoaderUI ajaxUI = new AjaxFineUpLoaderUI(divId, "fine_uploader_" + divId, OpenDilaogElementId, ajax_url_UpLoadFiles, ajax_url_ListFiles, ajax_url_DeleteFiles);

            ajaxUI.Options.element = "$('#fine_uploader_" + divId + "')";
            ajaxUI.Options.request.inputName = "hd_FileUp_EL";
            ajaxUI.Options.request.endpoint = ajax_url_UpLoadFiles;
            ajaxUI.Options.request._params = new DataModule();
            ajaxUI.Options.request._params.Add("id", SystemId.ToString());
            ajaxUI.Options.request._params.Add("FilesKind", "'" + queryFileKind + "'");
            ajaxUI.Options.callbacks = new AjaxFineUpLoaderUI.callbackOptons();
            //$(this).fineUploader('clearStoredFiles');
            ajaxUI.Options.callbacks.complete = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Options.callbacks.complete.funcString = String.Format("$.FilesListHandle('{0}_filesList','{1}','{2}',{3},'{4}');", divId, ajax_url_ListFiles, ajax_url_DeleteFiles, SystemId, queryFileKind);
            ajaxUI.Options.callbacks.complete.funcString += "if($(this).fineUploader('getInProgress')==0)";
            ajaxUI.Options.callbacks.complete.funcString += "{$(this).fineUploader('clearStoredFiles');}";

            ajaxUI.Options.callbacks.error = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Options.callbacks.error.funcString = "alert('file uploader error:' + errorReason);";
            ajaxUI.Options.text = new AjaxFineUpLoaderUI.textOptions() { uploadButton = "<i class='icon-plus icon-white'></i>" + Resources.Res.Button_Choice_Images };
            ajaxUI.Options.autoUpload = false;
            ajaxUI.Options.debug = true;
            ajaxUI.Options.failedUploadTextDisplay = new AjaxFineUpLoaderUI.failedUploadTextDisplayOptions() { mode = "custom", enableTooltip = true, responseProperty = "error", maxChars = 100 };

            ajaxUI.Options.validation = new AjaxFineUpLoaderUI.validationOptions();
            ajaxUI.Options.validation.allowedExtensions = new String[] { "jpg", "jpeg", "gif", "png" };

            ajaxUI.SubmitButton = new jqElementEvent(new jqSelector() { IdName = divId + "_SubmitFileButton" });
            ajaxUI.SubmitButton.events.Add(new jqElementEvent.jqEvents() { htmlElementEvent= HtmlObjectEvent.click
            , funcString = "$('#fine_uploader_" + divId + "').fineUploader('uploadStoredFiles');"
            });

            jqButton btn_upd_fmwk = new jqButton(new jqSelector() { IdName = ajaxUI.SubmitButton.Id });
            btn_upd_fmwk.options.icons.primary = FrameworkIcons.ui_icon_arrowreturnthick_1_n;

            ajaxUI.Dialog.options.open = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            ajaxUI.Dialog.options.open.funcString = String.Format("$.FilesListHandle('{0}_filesList','{1}','{2}',{3},'{4}');{5};", divId, ajax_url_ListFiles, ajax_url_DeleteFiles, SystemId, queryFileKind, btn_upd_fmwk.ToScriptString());
            ajaxUI.Dialog.options.zIndex = 2;

            ajaxUI.Dialog.options.height = height;
            ajaxUI.Dialog.options.width = Width;

            StringBuilder sb_Html = new StringBuilder();
            sb_Html.AppendLine(String.Format("<div id=\"{0}\" title=\"{1}\">", divId, title));
            sb_Html.AppendLine("<div id=\"fine_uploader_" + divId + "\"></div>"); //此div交由FineUpload控制
            sb_Html.AppendLine(String.Format("<button id=\"{0}\" type=\"button\">{1}</button>", ajaxUI.SubmitButton.Id, Resources.Res.Button_Start_ImagesUp));
            sb_Html.AppendLine(String.Format("<fieldset><legend class=\"ui-state-active edit-subtitle-caption\" style=\"width:100%;font-size: 0.95em;padding:5px 5px 5px 5px;font-weight:normal\">{1}</legend><div id=\"{0}_filesList\"></div></fieldset>", divId, Resources.Res.Info_Images_List));
            sb_Html.AppendLine("</div>");

            return MvcHtmlString.Create(sb_Html.ToString() + ajaxUI.ToScriptString().ToJqueryDocumentReady().ToScriptTag());
        }

        #endregion

        public static String PFile(this HtmlHelper h, String f)
        {
            var pagePath = ((WebViewPage)(h.ViewDataContainer)).VirtualPath;
            var filePath = h.ViewContext.HttpContext.Server.MapPath( pagePath.Substring(0, pagePath.LastIndexOf('/') + 1) + "//" + f);

            String r = String.Empty;
            if (System.IO.File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                r = sr.ReadToEnd();
                sr.Close();
            }

            return r;
        }
    }
    public static class LocalizationHelpe
    {
        public static String Lang(this HtmlHelper htmlHelper, String key)
        {
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, key);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            var inputName = ExpressionHelper.GetExpressionText(expression);
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, inputName);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, String prefx)
        where TModel : class
        {
            var inputName = prefx + ExpressionHelper.GetExpressionText(expression);
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, inputName);
        }

        /// <summary>
        /// Grid欄位專用，為必免命名衝突以g_為開頭在加欄位名稱
        /// </summary>
        public static String GLang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            return Lang<TModel, TProperty>(htmlHelper, expression, "g_");
        }
        public static String GLang(this HtmlHelper htmlHelper, String key)
        {
            String s = "g_" + key;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }
        public static String GLang(this HtmlHelper htmlHelper, FieldModule fm)
        {
            String s = "g_" + fm.M;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }

        /// <summary>
        /// 編輯表欄位專用以，為必免命名衝突以f_為開頭在加欄位名稱
        /// </summary>
        public static String FieldLang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            return Lang<TModel, TProperty>(htmlHelper, expression, "f_");
        }
        public static String FLang(this HtmlHelper htmlHelper, String key)
        {
            String s = "f_" + key;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }
        public static String FLang(this HtmlHelper htmlHelper, FieldModule fm)
        {
            String s = "f_" + fm.M;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }

        /// <summary>
        /// 編輯表欄提示訊息專用，為必免命名衝突以t_為開頭在加欄位名稱
        /// </summary>
        public static String TipLang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            return Lang<TModel, TProperty>(htmlHelper, expression, "t_");
        }
        public static String TLang(this HtmlHelper htmlHelper, String key)
        {
            string s = "t_" + key;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }

        private static IEnumerable<DictionaryEntry> GetResx(String LocalResourcePath)
        {
            ObjectCache cache = MemoryCache.Default;
            IEnumerable<DictionaryEntry> resxs = null;

            if (cache.Contains(LocalResourcePath))
                resxs = cache.GetCacheItem(LocalResourcePath).Value as IEnumerable<DictionaryEntry>;
            else
            {
                if (File.Exists(LocalResourcePath))
                {
                    resxs = new ResXResourceReader(LocalResourcePath).Cast<DictionaryEntry>();
                    cache.Add(LocalResourcePath, resxs, new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable });
                }
            }

            return resxs;
        }

        public static String Lang(this WebPageBase page, String key)
        {
            var pagePath = page.VirtualPath;
            var pageName = pagePath.Substring(pagePath.LastIndexOf('/'), pagePath.Length - pagePath.LastIndexOf('/')).TrimStart('/');
            var filePath = page.Server.MapPath(pagePath.Substring(0, pagePath.LastIndexOf('/') + 1)) + "App_LocalResources";

            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            String resxKey = String.Empty;

            resxKey =
                lang == "zh-TW" || String.IsNullOrWhiteSpace(lang) ?
                String.Format(@"{0}\{1}.resx", filePath, pageName) :
                String.Format(@"{0}\{1}.{2}.resx", filePath, pageName, lang);

            IEnumerable<DictionaryEntry> resxs = GetResx(resxKey);

            if (resxs != null)
                return (String)resxs.FirstOrDefault<DictionaryEntry>(x => x.Key.ToString() == key).Value;
            else
                return "";
        }
    }

    #region Form Help
    public class MVCFormReport : IDisposable
    {
        private Boolean disposed;
        private ViewContext _vc;

        public MVCFormReport(ViewContext VCText, String AjaxFormId, String ActionUrl, Boolean UseReturn, String SubmitId)
        {
            HandleInit(VCText, AjaxFormId, ActionUrl, SubmitId);
        }
        public MVCFormReport(ViewContext VCText, String AjaxFormId, String ActionUrl)
        {
            HandleInit(VCText, AjaxFormId, ActionUrl, "btn_submit");
        }
        public MVCFormReport(ViewContext VCText, String AjaxFormId)
        {
            HandleInit(VCText, AjaxFormId, "ajax_MasterUpdata", "btn_submit");
        }

        private void HandleInit(ViewContext VCText, String AjaxFormId, String ActionUrl, String SubmitId)
        {
            _vc = VCText;
            FormId = AjaxFormId;
            action = ActionUrl;

            //                "/" + _vc.RouteData.DataTokens["area"].ToString() + "/" + _vc.RouteData.Values["controller"].ToString() + "/" +

            String tpl = String.Format("<form id=\"{0}\" name=\"{0}\" method=\"{1}\" action=\"{2}\" enctype=\"{3}\">\r\n", FormId, method, action, GetEnctype());
            _vc.Writer.Write(tpl);

//            SubmitElement = new jqElementEvent(new jqSelector() { IdName = SubmitId }) { Event = HtmlObjectEvent.click };
//            SubmitElement.funcObject.funcString = @"
//                ajaxHasDone=$.when.apply( $, ajaxRequest ); 
//                ajaxHasDone.done(function() {  
//                    if($('#" + FormId + @"').valid()) 
//                            $('#" + FormId + @"').submit();
//                });";

            ajaxOptions = new FormAjaxOption()
            {
                beforeSubmit = new funcMethodModule() { funcName = "AjaxFormBeforeSubmit", MakeStyle = funcMethodModule.funcMakeStyle.onlyName },
                success = new funcMethodModule() { funcName = "AjaxFormSuccess", MakeStyle = funcMethodModule.funcMakeStyle.onlyName },
                dataType = "json",
                contentType = "application/x-www-form-urlencoded; charset=utf-8"
            };

            ajaxOptions.beforeSubmit.funcString = "var queryString = $.param(formData);return true;";
            ajaxOptions.success.funcString = "if (jsonobj.result)";
            ajaxOptions.success.funcString += "{ $('#ifm_exceldownload').attr('src','ExportExcelFile?FileName=' + jsonobj.filesObject[0].FileName + '&WebPath=' + jsonobj.filesObject[0].OriginFilePath);}";
            ajaxOptions.success.funcString += "if(jsonobj.message!='') $.UiMessage(jsonobj);";

            formValidate = new FormValidateSetup(new jqSelector() { IdName = this.FormId });
        }

        public String FormId { get; set; }
        public formMethods method { get; set; }
        public String action { get; set; }
        public formEnctype enctype { get; set; }
        public String GetEnctype()
        {

            if (enctype == formEnctype.application_x_www_form_urlencoded)
                return "application/x-www-form-urlencoded";

            else if (enctype == formEnctype.multipart_form_data)
                return "multipart/form-data";

            else
                return "text/plain";
        }

        public jqElementEvent SubmitElement { get; set; }
        public FormAjaxOption ajaxOptions { get; set; }
        public FormValidateSetup formValidate { get; set; }
        public String LastAppenScript { get; set; }
        private void ToScriptSting()
        {
            _vc.Writer.Write("</form>\r\n");

            String script = String.Empty;

            if (SubmitElement != null)
                script += SubmitElement.ToScriptString();

            if (ajaxOptions != null)
            {
                String tpl = "\tvar options={0};\t\r\n{1};\t\r\n{2};\t\r\n{3};";
                MakeJqScript createJsonStrig = new MakeJqScript() { GetObject = this.ajaxOptions };
                //createJsonStrig.AddPropertyOption(new PropertyObjectJsonOption() { PropertyName = "ajaxOptions" });
                script += String.Format(tpl,
                    createJsonStrig.MakeScript(),
                    ajaxOptions.beforeSubmit.ToScriptString(funcMethodModule.funcMakeStyle.complete),
                    ajaxOptions.success.ToScriptString(funcMethodModule.funcMakeStyle.complete),
                    formValidate.ToScriptString()
                    );
                tpl = null; createJsonStrig = null;
            }

            //jqElementEvent jqFormSubmit = new jqElementEvent(new jqSelector() { IdName = this.FormId }) { Event = HtmlObjectEvent.submit };
            //jqFormSubmit.funcObject.funcString = "$(this).ajaxSubmit(options);return false;";
            //script += jqFormSubmit.ToScriptString();
            //jqFormSubmit = null;
            script += LastAppenScript;

            _vc.Writer.Write(script.ToJqueryDocumentReady().ToScriptTag());
            script = null;
        }

        public void FormEnd()
        {
            ToScriptSting();
            Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
    }
    public class TagInstanct : IDisposable
    {
        private ViewContext _vwContext;
        private String _tagName;

        public TagInstanct(ViewContext VCText, String TagName)
            : base()
        {
            this._vwContext = VCText;
            this._tagName = TagName;
            _vwContext.Writer.Write("<" + TagName + ">");
        }

        public TagInstanct(ViewContext VCText, String TagName, String className)
            : base()
        {
            this._vwContext = VCText;
            this._tagName = TagName;
            TagBuilder t = new TagBuilder(TagName);
            if (className != null)
                t.AddCssClass(className);
            _vwContext.Writer.Write(t.ToString((TagRenderMode.StartTag)));
        }

        public void Dispose()
        {
            _vwContext.Writer.Write("</" + this._tagName + ">");
            _vwContext = null;
            GC.SuppressFinalize(this);
        }

    }
    #endregion
}