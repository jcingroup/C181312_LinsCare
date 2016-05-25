using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcCore.NetExtension;
using ProcCore.JqueryHelp.AjaxHelp;

namespace ProcCore.JqueryHelp.JQGridScript
{
    #region Grid 主物件區

    public class jqGrid : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public jqGrid()
        {
            GridIdWorkTosubGridRowExpanded = false;
        }
        public String Id { get; set; }

        public gridMasterObject GridModule { get; set; }
        public navGridObject NavGridModule { get; set; }
        public navButtonAddModule[] navCustomButtons { get; set; }

        /// <summary>
        /// 此grid是否為sub grid，sub grid的id處理方式會有所不同。
        /// </summary>
        public Boolean? GridIdWorkTosubGridRowExpanded { get; set; }

        public String jqGridHtml { get; set; }
        public String jqGridScript { get; set; }
        public String jqFormatterScript { get; set; }

        public String jqmbGridScript { get; set; }
        public String jqmbCustomScript { get; set; }

        public void ToScriptHandle()
        {
            List<String> ls_dot_script = new List<String>();

            if (GridIdWorkTosubGridRowExpanded == false)
            {
                ls_dot_script.Add("jQuery(\"#" + Id + "\")");
            }
            else
            {
                ls_dot_script.Add("jQuery(\"#\" + subgrid_table_id)");
                GridModule.pager = "subgrid_pager_id";
            }

            jqGridHtml = "<table id=\"" + Id + "\"></table>";

            #region 獨立處理Grid Column Format to javascript code
            Dictionary<String, String> dicFormatterFunc = new Dictionary<String, String>();
            foreach (var funcFormatterObj in GridModule.colModel)
            {
                if (funcFormatterObj.AssignFormatter != null)
                {
                    if (!dicFormatterFunc.ContainsKey(funcFormatterObj.AssignFormatter.func.funcName))
                    {
                        dicFormatterFunc.Add(funcFormatterObj.AssignFormatter.func.funcName, funcFormatterObj.AssignFormatter.JqueryFunctionString());

                        //如果該欄位客制化是要使用Framework ICON 則要做下列動做。
                        if (funcFormatterObj.AssignFormatter.buttonRaise)
                        {
                            if (this.GridModule.loadComplete == null)
                                this.GridModule.loadComplete = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };

                            this.GridModule.loadComplete.funcString += "$('." + funcFormatterObj.AssignFormatter.buttonClass + "').button({icons: {primary: 'ui-icon-scissors'}});";
                        }
                    }
                }
            }
            #endregion

            #region GridModule Option Setup
            if (GridIdWorkTosubGridRowExpanded == true)
                GridModule.pager = "jQuery(\"#\" + subgrid_pager_id)";
            var json = GridModule.ToScriptString();

            if (GridIdWorkTosubGridRowExpanded == false)
                jqmbGridScript = "jQuery(\"#" + Id + "\").jqGrid({" + json + "})";
            else
                jqmbGridScript = "jQuery(\"#\" + subgrid_table_id).jqGrid({" + json + "})"; ;

            ls_dot_script.Add("jqGrid({" + json + "})");
            json = String.Empty;
            #endregion

            #region NavGridModule special module setup
            if (NavGridModule != null)
            {
                var NavJson = NavGridModule.ToScriptString();
                if (GridIdWorkTosubGridRowExpanded == false)
                {
                    jqGridHtml += "<div id=\"" + GridModule.pager + "\"></div>";
                    ls_dot_script.Add("navGrid('#" + GridModule.pager + "'," + NavJson + ")");
                    jqmbGridScript += ".navGrid('#" + GridModule.pager + "'," + NavJson + ")";
                }
                else
                {
                    ls_dot_script.Add("navGrid(\"#\" + subgrid_pager_id," + NavJson + ")");
                    jqmbGridScript += ".navGrid(\"#\" + subgrid_pager_id," + NavJson + ")";
                }
            }
            #endregion

            #region Custom Button Handle
            if (navCustomButtons != null)
            {
                List<String> ls_mbCustomButton = new List<String>();

                foreach (navButtonAddModule navCustomButton in navCustomButtons)
                {
                    if (GridIdWorkTosubGridRowExpanded == false)
                    {
                        ls_dot_script.Add("navButtonAdd('#" + GridModule.pager + "',{" + navCustomButton.ToScriptString() + "})");
                        ls_mbCustomButton.Add("navButtonAdd('#" + GridModule.pager + "',{" + navCustomButton.ToScriptString() + "})");
                    }
                    else
                    {
                        ls_dot_script.Add("navButtonAdd('#' + subgrid_pager_id,{" + navCustomButton.ToScriptString() + "})");
                    }
                }

                if (GridIdWorkTosubGridRowExpanded == false)
                    jqmbCustomScript = "jQuery(\'#" + Id + "\')." + ls_mbCustomButton.ToArray().JoinArray(".");
                else
                    jqmbCustomScript = "jQuery(\"#\" + subgrid_table_id)" + ls_mbCustomButton.ToArray().JoinArray(".");

            }
            #endregion

            ls_dot_script.Add("bindKeys()");


            jqGridScript = ls_dot_script.ToArray().JoinArray("."); ls_dot_script.Clear();
            jqFormatterScript = dicFormatterFunc.Values.ToArray().JoinArray(";");
        }
        public String ToScriptString()
        {
            ToScriptHandle();
            return jqGridHtml + (jqGridScript.ToJqueryDocumentReady() + ";(function ($){" + jqFormatterScript + "})(jQuery);").ToScriptTag();
        }

        /// <summary>
        /// grid 主物件
        /// </summary>
        public class gridMasterObject : BaseJqueryScriptHelp, ijQueryUIScript
        {
            #region 變數定義
            private funcMethodModule _afterInsertRow;
            private funcMethodModule _beforeProcessing;
            private funcMethodModule _beforeRequest;
            private funcMethodModule _beforeSelectRow;
            private funcMethodModule _gridComplete;
            private funcMethodModule _loadBeforeSend;
            private funcMethodModule _loadComplete;
            private funcMethodModule _loadError;
            private funcMethodModule _onCellSelect;
            private funcMethodModule _ondblClickRow;
            private funcMethodModule _onHeaderClick;
            private funcMethodModule _onPaging;
            private funcMethodModule _onRightClickRow;
            private funcMethodModule _onSelectAll;
            private funcMethodModule _onSelectRow;
            private funcMethodModule _onSortCol;
            private funcMethodModule _resizeStart;
            private funcMethodModule _resizeStop;
            private funcMethodModule _serializeGridData;

            private funcMethodModule _subGridBeforeExpand;
            private funcMethodModule _subGridRowExpanded;
            private funcMethodModule _subGridRowColapsed;
            private funcMethodModule _serializeSubGridData;

            #endregion

            public gridMasterObject()
            {
                datatype = "json";
                mtype = "get";
                height = 0;
                width = "100%";
                multiselect = true;
            }

            public ajaxObject ajaxGridOptions { get; set; }
            public ajaxObject ajaxSelectOptions { get; set; }

            public String altclass { get; set; }
            public Boolean? altRows { get; set; }
            public Boolean? autoencode { get; set; }
            public Boolean? autowidth { get; set; }
            public String caption { get; set; }
            public int? cellLayout { get; set; }
            public Boolean? cellEdit { get; set; }
            public String cellsubmit { get; set; }
            public String cellurl { get; set; }
            /// <summary>
            /// Not Use
            /// </summary>
            public String cmTemplate { get; set; }
            public colObject[] colModel { get; set; }
            public String[] colNames { get; set; }

            /// <summary>
            /// Not Use
            /// </summary>
            public DataModule data { get; set; }
            public String datastr { get; set; }
            public String datatype { get; set; }
            public Boolean? deepempty { get; set; }
            public Boolean? deselectAfterSort { get; set; }
            /// <summary>
            /// The default is “ltr” (Left To Right language). When set to “rtl” (Right To Left) the grid automatically do the needed
            /// </summary>
            public String direction { get; set; }
            /// <summary>
            /// Defines the url for inline and form editing. May be set to clientArray to manually post data to server, see Inline Editing. 
            /// </summary>
            public String editurl { get; set; }
            /// <summary>
            /// The string to display when the returned (or the current) number of records in the grid is zero. This option is valid only if viewrecords option is set to true.
            /// </summary>
            public String emptyrecords { get; set; }
            public Boolean? ExpandColClick { get; set; }
            public String ExpandColumn { get; set; }
            public Boolean? footerrow { get; set; }
            public Boolean? forceFit { get; set; }
            /// <summary>
            /// 'visible' or 'hidden'
            /// </summary>
            public String gridstate { get; set; }
            public Boolean? gridview { get; set; }
            public Boolean? grouping { get; set; }
            public Boolean? headertitles { get; set; }
            public int? height { get; set; }
            public Boolean? hidegrid { get; set; }
            public Boolean? hoverrows { get; set; }
            public String idPrefix { get; set; }
            public Boolean? ignoreCase { get; set; }

            /// <summary>
            /// Not Use
            /// </summary>
            public String inlineData { get; set; }

            public String[] jsonReader { get; set; }
            public int? lastpage { get; set; }
            public int? lastsort { get; set; }
            public Boolean? loadonce { get; set; }
            public String loadtext { get; set; }
            /// <summary>
            /// disable - disables the jqGrid progress indicator. This way you can use your own indicator.
            ///enable (default) - enables “Loading” message in the center of the grid. 
            ///block - enables the “Loading” message and blocks all actions in the grid until the ajax request is finished. Note that this disables paging, 
            /// </summary>
            public String loadui { get; set; }
            public String mtype { get; set; }
            public String multikey { get; set; }
            public Boolean? multiboxonly { get; set; }
            public Boolean? multiselect { get; set; }
            public int? multiselectWidth { get; set; }
            public int? page { get; set; }
            public String pager { get; set; }
            public String pagerpos { get; set; }
            public Boolean? pgbuttons { get; set; }
            public Boolean? pginput { get; set; }
            public String pgtext { get; set; }
            public DataModule postData { get; set; }

            public int? reccount { get; set; }
            public String recordpos { get; set; }
            public int? records { get; set; }
            public String recordtext { get; set; }

            public String resizeclass { get; set; }
            public int[] rowList { get; set; }
            public int? rowNum { get; set; }
            public int? rowTotal { get; set; }
            public int? rownumbers { get; set; }

            public int? scrollTimeout { get; set; }
            public Boolean? scrollrows { get; set; }
            public Boolean? shrinkToFit { get; set; }

            public Boolean? sortable { get; set; }
            public String sortname { get; set; }
            public String sortorder { get; set; }

            public Boolean? subGrid { get; set; }
            public SubGridOptions subGridOptions { get; set; }
            public SubGridModel subGridModel { get; set; }

            public String subGridType { get; set; }
            public String subGridUrl { get; set; }
            public int? subGridWidth { get; set; }

            public Boolean? toppager { get; set; }
            public int? totaltime { get; set; }
            public String treedatatype { get; set; }
            public Boolean? treeGrid { get; set; }
            public String treeGridModel { get; set; }
            public String[] treeIcons { get; set; }
            public String[] treeReader { get; set; }
            public int? tree_root_level { get; set; }

            public String url { get; set; }
            public String[] userData { get; set; }

            public Boolean? userDataOnFooter { get; set; }
            public Boolean? viewrecords { get; set; }
            public String[] viewsortcols { get; set; }

            public String width { get; set; }
            public String[] xmlReader { get; set; }
            //以下為mb版屬性
            public Boolean? search { get; set; }
            public Boolean? scroll { get; set; }
            public Int32? scrollOffset { get; set; }

            public funcMethodModule afterInsertRow
            {
                get
                {

                    return _afterInsertRow;
                }
                set
                {
                    _afterInsertRow = value;

                    if (_afterInsertRow == null)
                    {
                        //_afterInsertRow = new funcMethodModule();
                        _afterInsertRow.funcParam.AddRange(new String[] { "rowid", "rowdata", "rowelem" });
                    }
                }
            }
            public funcMethodModule beforeProcessing
            {
                get
                {

                    return _beforeProcessing;
                }
                set
                {
                    _beforeProcessing = value;
                    if (_beforeProcessing != null)
                    {
                        //_beforeProcessing = new funcMethodModule();
                        _beforeProcessing.funcParam.AddRange(new String[] { "data", "status", "xhr" });
                    }
                }
            }
            public funcMethodModule beforeRequest
            {
                get
                {
                    return _beforeRequest;
                }
                set
                {
                    _beforeRequest = value;
                    if (_beforeRequest != null)
                    {
                        //_beforeRequest = new funcMethodModule();
                    }
                }
            }
            public funcMethodModule beforeSelectRow
            {
                get
                {
                    return _beforeSelectRow;
                }
                set
                {
                    _beforeSelectRow = value;
                    if (_beforeSelectRow != null)
                    {
                        //_beforeSelectRow = new funcMethodModule();
                        _beforeSelectRow.funcParam.AddRange(new String[] { "rowid", "e" });
                    }
                }
            }
            public funcMethodModule gridComplete
            {
                get
                {
                    return _gridComplete;
                }
                set
                {
                    _gridComplete = value;
                    if (_gridComplete != null)
                    {
                        //_gridComplete = new funcMethodModule();
                    }
                }
            }
            public funcMethodModule loadBeforeSend
            {
                get
                {
                    return _loadBeforeSend;
                }
                set
                {
                    _loadBeforeSend = value;
                    if (_loadBeforeSend != null)
                    {
                        //_loadBeforeSend = new funcMethodModule();
                        _loadBeforeSend.funcParam.AddRange(new String[] { "xhr", "settings" });
                    }
                }
            }
            public funcMethodModule loadComplete
            {
                get
                {
                    return _loadComplete;
                }
                set
                {
                    _loadComplete = value;
                    if (_loadComplete != null)
                    {
                        //_loadComplete = new funcMethodModule();
                        _loadComplete.funcParam.AddRange(new String[] { "data" });
                    }
                }
            }
            public funcMethodModule loadError
            {
                get
                {

                    return _loadError;
                }
                set
                {
                    _loadError = value;
                    if (_loadError != null)
                    {
                        //_loadError = new funcMethodModule();
                        _loadError.funcParam.AddRange(new String[] { "xhr", "status", "error" });
                    }
                }
            }
            public funcMethodModule onCellSelect
            {
                get
                {
                    return _onCellSelect;
                }
                set
                {
                    _onCellSelect = value;
                    if (_onCellSelect != null)
                    {
                        //_onCellSelect = new funcMethodModule();
                        _onCellSelect.funcParam.AddRange(new String[] { "rowid", "iCol", "cellcontent", "e" });
                    }
                }
            }
            public funcMethodModule ondblClickRow
            {
                get
                {
                    return _ondblClickRow;
                }
                set
                {
                    _ondblClickRow = value;
                    if (_ondblClickRow != null)
                    {
                        //_ondblClickRow = new funcMethodModule();
                        _ondblClickRow.funcParam.AddRange(new String[] { "rowid", "iRow", "iCol", "e" });
                    }
                }
            }
            public funcMethodModule onHeaderClick
            {
                get
                {

                    return _onHeaderClick;
                }
                set
                {
                    _onHeaderClick = value;
                    if (_onHeaderClick != null)
                    {
                        //_onHeaderClick = new funcMethodModule();
                        _onHeaderClick.funcParam.AddRange(new String[] { "gridstate" });
                    }
                }
            }
            public funcMethodModule onPaging
            {
                get
                {

                    return _onPaging;
                }
                set
                {
                    _onPaging = value;
                    if (_onPaging != null)
                    {
                        //_onPaging = new funcMethodModule();
                        _onPaging.funcParam.AddRange(new String[] { "pgButton" });
                    }
                }
            }
            public funcMethodModule onRightClickRow
            {
                get
                {

                    return _onRightClickRow;
                }
                set
                {
                    _onRightClickRow = value;
                    if (_onRightClickRow != null)
                    {
                        //_onRightClickRow = new funcMethodModule();
                        _onRightClickRow.funcParam.AddRange(new String[] { "rowid", "iRow", "iCol", "e" });
                    }
                }
            }
            public funcMethodModule onSelectAll
            {
                get
                {

                    return _onSelectAll;
                }
                set
                {
                    _onSelectAll = value;
                    if (_onSelectAll != null)
                    {
                        //_onSelectAll = new funcMethodModule();
                        _onSelectAll.funcParam.AddRange(new String[] { "aRowids", "status" });
                    }
                }
            }
            public funcMethodModule onSelectRow
            {
                get
                {
                    if (_onSelectRow == null)
                    {
                        _onSelectRow = new funcMethodModule();
                        _onSelectRow.funcParam.AddRange(new String[] { "rowid", "status" });
                    }
                    return _onSelectRow;
                }
                set
                {
                    _onSelectRow = value;
                }
            }
            public funcMethodModule onSortCol
            {
                get
                {

                    return _onSortCol;
                }
                set
                {
                    _onSortCol = value;
                    if (_onSortCol != null)
                    {
                        //_onSortCol = new funcMethodModule();
                        _onSortCol.funcParam.AddRange(new String[] { "index", "iCol", "sortorder" });
                    }
                }
            }
            public funcMethodModule resizeStart
            {
                get
                {

                    return _resizeStart;
                }
                set
                {
                    _resizeStart = value;
                    if (_resizeStart != null)
                    {
                        //_resizeStart = new funcMethodModule();
                        _resizeStart.funcParam.AddRange(new String[] { "event", "indexm" });
                    }
                }
            }
            public funcMethodModule resizeStop
            {
                get
                {

                    return _resizeStop;
                }
                set
                {
                    _resizeStop = value;
                    if (_resizeStop != null)
                    {
                        //_resizeStop = new funcMethodModule();
                        _resizeStop.funcParam.AddRange(new String[] { "newwidth", "index" });
                    }
                }
            }
            public funcMethodModule serializeGridData
            {
                get
                {
                    return _serializeGridData;
                }
                set
                {
                    _serializeGridData = value;
                    if (_serializeGridData != null)
                    {
                        //_serializeGridData = new funcMethodModule();
                        _serializeGridData.funcParam.AddRange(new String[] { "postData" });
                    }
                }
            }
            public funcMethodModule subGridBeforeExpand
            {
                get
                {
                    return _subGridBeforeExpand;
                }
                set
                {
                    _subGridBeforeExpand = value;
                    if (_subGridBeforeExpand != null)
                    {
                        // _subGridBeforeExpand = new funcMethodModule();
                        _subGridBeforeExpand.funcParam.AddRange(new String[] { "pID", "id" });
                    }
                }
            }
            /// <summary>
            /// pID is the unique id of the div element where we can put contents when subgrid is enabled,
            /// id is the id of the row
            /// </summary>
            public funcMethodModule subGridRowExpanded
            {
                get
                {
                    return _subGridRowExpanded;
                }
                set
                {
                    _subGridRowExpanded = value;
                    if (_subGridRowExpanded != null)
                    {
                        _subGridRowExpanded.funcParam.AddRange(new String[] { "pID", "id" });
                    }
                }
            }
            public funcMethodModule subGridRowColapsed
            {
                get
                {

                    return _subGridRowColapsed;
                }
                set
                {
                    _subGridRowColapsed = value;
                    if (_subGridRowColapsed != null)
                    {
                        //_subGridRowColapsed = new funcMethodModule();
                        _subGridRowColapsed.funcParam.AddRange(new String[] { "pID", "id" });
                    }
                }
            }
            public funcMethodModule serializeSubGridData
            {
                get
                {

                    return _serializeSubGridData;
                }
                set
                {
                    _serializeSubGridData = value;
                    if (_serializeSubGridData != null)
                    {
                        //_serializeSubGridData = new funcMethodModule();
                        _serializeSubGridData.funcParam.AddRange(new String[] { "sPostData" });
                    }
                }
            }
            public String ToScriptString()
            {
                MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                jqOption.ItemNotMake.Add("AssignFormatter");
                jqOption.ItemNoDot.Add("pager");
                return jqOption.MakeScript();
            }

            public class SubGridOptions : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public String plusicon { get; set; }
                public String minusicon { get; set; }
                public String openicon { get; set; }
                public Boolean? expandOnLoad { get; set; }
                public Boolean? selectOnExpand { get; set; }
                public Boolean? reloadOnExpand { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
            public class SubGridModel : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public String[] name { get; set; }
                public int[] width { get; set; }
                public String[] align { get; set; }
                public String[] _params { get; set; }
                public String[] mapping { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
        }
        public class colObject : BaseJqueryScriptHelp, ijQueryUIScript
        {
            public colObject()
            {
            }

            public Boolean? key { get; set; }
            public String name { get; set; }
            public String index { get; set; }
            public String width { get; set; }
            public Boolean? sortable { get; set; }
            public Boolean? hidedlg { get; set; }
            public Boolean? frozen { get; set; }

            public String align { get; set; }
            public Boolean? editable { get; set; }
            public String sorttype { get; set; }

            public funcMethodModule formatter { get; set; }
            public String formatterstring { get; set; }

            public FormatterColumnScript AssignFormatter { get; set; }
            public FormatOptions formatoptions { get; set; }

            public Boolean? hidden { get; set; }
            public editRules editrules { get; set; }
            public String edittype { get; set; }
            public String label { get; set; }
            public Boolean? resizable { get; set; }
            public Boolean? search { get; set; }
            public String stype { get; set; }
            public String surl { get; set; }
            public Boolean? title { get; set; }
            public String xmlmap { get; set; }

            public Boolean? viewable { get; set; }

            public String classes { get; set; }
            public String datefmt { get; set; }
            public String defval { get; set; }
            public String firstsortorder { get; set; }
            //public Boolean fixed { get; set; }
            //===================================
            public formOptions formoptions { get; set; }
            public editOptions editoptions
            {
                get;
                set;
            }

            public String ToScriptString()
            {
                MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                jqOption.ItemRename.Add("formatterstring", "formatter");
                return jqOption.MakeScript();
            }

            public class editOptions : BaseJqueryScriptHelp, ijQueryUIScript
            {
                private funcMethodModule _dataInit;

                public String value { get; set; }
                public String dataUrl { get; set; }

                public funcMethodModule buildSelect { get; set; }
                public funcMethodModule dataInit
                {
                    get
                    {

                        return _dataInit;
                    }
                    set
                    {
                        _dataInit = value;
                        if (_dataInit != null)
                        {
                            //_dataInit = new funcMethodModule();
                            _dataInit.funcParam.AddRange(new String[] { "elem" });
                        }
                    }
                }

                public DataEvents[] dataEvents { get; set; }
                public String defaultValue { get; set; }
                public Boolean? NullIfEmpty { get; set; }

                //以下為擴充屬性 在官方網站就是 other options
                public String maxlength { get; set; }
                public String size { get; set; }
                public String id { get; set; }
                public String style { get; set; }
                public String src { get; set; }
                public String _class { get; set; }
                public String _readonly { get; set; }

                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };

                    return jqOption.MakeScript();
                }

                public class DataEvents : BaseJqueryScriptHelp, ijQueryUIScript
                {
                    public DataEvents()
                    {
                        this.data = new Dictionary<string, string>();
                        this.fn = new funcMethodModule();
                        fn.funcParam.Add("e");
                        fn.MakeStyle = funcMethodModule.funcMakeStyle.funcConext;
                    }
                    public HtmlObjectEvent type { get; set; }
                    public Dictionary<String, String> data { get; set; }
                    public funcMethodModule fn { get; set; }
                    public String ToScriptString()
                    {
                        MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                        return jqOption.MakeScript();
                    }
                }
            }
            public class editRules : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public Boolean? edithidden { get; set; }
                public Boolean? required { get; set; }
                public Boolean? number { get; set; }
                public Boolean? integer { get; set; }
                public int? minValue { get; set; }
                public int? maxValue { get; set; }
                public Boolean? email { get; set; }
                public Boolean? url { get; set; }
                public Boolean? date { get; set; }
                public Boolean? time { get; set; }
                public Boolean? custom { get; set; }
                public funcMethodModule custom_func { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
            public class formOptions : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public String elmprefix { get; set; }
                public String elmsuffix { get; set; }
                public String label { get; set; }
                public int rowpos { get; set; }
                public int colpos { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
            public class FormatOptions : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public Boolean disabled { get; set; }
                public String thousandsSeparator { get; set; }
                public String defaultValue { get; set; }
                public String decimalSeparator { get; set; }
                public int decimalPlaces { get; set; }
                public String prefix { get; set; }
                public String suffix { get; set; }
                public String newformat { get; set; }
                public String srcformat { get; set; }
                public String target { get; set; }
                public String baseLinkUrl { get; set; }
                public String showAction { get; set; }
                public String addParam { get; set; }
                public String idName { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
        }
        /// <summary>
        /// If the id of the button is not set we use the following rule: 
        ///For the add button we use “add_” + the id of the grid 
        ///For the edit button we use “edit_” + the id of the grid 
        ///For the delete button we use “del_” + the id of the grid 
        ///For the view button we use “view_” + the id of the grid 
        ///For the search button we use “search_” + the id of the grid 
        ///For the refresh button we use “refresh_” + the id of the grid 
        /// </summary>
        public class navGridObject : BaseJqueryScriptHelp, ijQueryUIScript
        {
            public navGridObject()
            {
                navOption = new navGridOption();
                Edit = new editGridRow();
                Add = new editGridRow();
                Del = new editGridRow();
            }
            public navGridOption navOption { get; set; }
            public editGridRow Edit { get; set; }
            public editGridRow Add { get; set; }
            public editGridRow Del { get; set; }
            public String ToScriptString()
            {
                MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                jqOption.ItemNameStop.Add("navOption");
                jqOption.ItemNameStop.Add("Add");
                jqOption.ItemNameStop.Add("Edit");
                jqOption.ItemNameStop.Add("Del");

                return jqOption.MakeScript();
            }

            public class navGridOption : BaseJqueryScriptHelp, ijQueryUIScript
            {
                public Boolean? add { get; set; }
                public String addicon { get; set; }
                public String addtext { get; set; }
                public String addtitle { get; set; }
                public String alertcap { get; set; }
                public String alerttext { get; set; }
                public Boolean? cloneToTop { get; set; }
                public Boolean? closeOnEscape { get; set; }
                public Boolean? del { get; set; }
                public String delicon { get; set; }
                public String deltext { get; set; }
                public String deltitle { get; set; }
                public Boolean? edit { get; set; }
                public String editicon { get; set; }
                public String edittext { get; set; }
                public String edittitle { get; set; }
                public String position { get; set; }
                public Boolean? refresh { get; set; }
                public String refreshicon { get; set; }
                public String refreshtext { get; set; }
                public String refreshtitle { get; set; }
                public String refreshstate { get; set; }
                public funcMethodModule afterRefresh { get; set; }
                public funcMethodModule beforeRefresh { get; set; }
                public Boolean? search { get; set; }
                public String searchicon { get; set; }
                public String searchtext { get; set; }
                public String searchtitle { get; set; }
                public Boolean? view { get; set; }
                public String viewicon { get; set; }
                public String viewtext { get; set; }
                public String viewtitle { get; set; }

                public funcMethodModule addfunc { get; set; }
                public funcMethodModule editfunc { get; set; }
                public funcMethodModule delfunc { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
            /// <summary>
            /// http://www.trirand.com/jqgridwiki/doku.php?id=wiki:form_editing
            /// http://www.trirand.com/jqgridwiki/doku.php?id=wiki:navigator
            /// </summary>
            public class editGridRow : BaseJqueryScriptHelp, ijQueryUIScript
            {
                funcMethodModule _afterclickPgButtons;
                funcMethodModule _afterComplete;
                funcMethodModule _afterShowForm;
                funcMethodModule _afterSubmit;
                funcMethodModule _beforeCheckValues;
                funcMethodModule _beforeInitData;
                funcMethodModule _beforeSubmit;
                funcMethodModule _beforeShowForm;
                funcMethodModule _onclickPgButtons;
                funcMethodModule _onclickSubmit;
                funcMethodModule _onInitializeForm;
                funcMethodModule _onClose;
                funcMethodModule _errorTextFormat;
                funcMethodModule _serializeEditData;

                public editGridRow()
                {

                }
                public funcMethodModule afterclickPgButtons
                {
                    get
                    {
                        if (_afterclickPgButtons == null)
                        {
                            _afterclickPgButtons = new funcMethodModule();
                            _afterclickPgButtons.funcParam.AddRange(new String[] { "whichbutton", "formid", "rowid" });
                        }
                        return _afterclickPgButtons;
                    }
                    set
                    {
                        _afterclickPgButtons = value;
                    }
                }
                public funcMethodModule afterComplete
                {
                    get
                    {
                        if (_afterComplete == null)
                        {
                            _afterComplete = new funcMethodModule();
                            _afterComplete.funcParam.AddRange(new String[] { "response", "postdata", "formid" });
                        }
                        return _afterComplete;
                    }
                    set
                    {
                        _afterComplete = value;
                    }
                }
                public funcMethodModule afterShowForm
                {
                    get
                    {

                        return _afterShowForm;
                    }
                    set
                    {
                        _afterShowForm = value;
                        if (_afterShowForm != null)
                        {
                            _afterShowForm.funcParam.AddRange(new String[] { "formid" });
                        }
                    }
                }
                /// <summary>
                /// return [success,message,new_id] 
                /// </summary>
                public funcMethodModule afterSubmit
                {
                    get
                    {
                        return _afterSubmit;
                    }
                    set
                    {
                        _afterSubmit = value;
                        if (_afterSubmit != null)
                        {
                            _afterSubmit.funcParam.AddRange(new String[] { "response", "postdata" });
                        }
                    }
                }
                public funcMethodModule beforeCheckValues
                {
                    get
                    {

                        return _beforeCheckValues;
                    }
                    set
                    {
                        _beforeCheckValues = value;
                        if (_beforeCheckValues != null)
                        {
                            //_beforeCheckValues = new funcMethodModule();
                            _beforeCheckValues.funcParam.AddRange(new String[] { "posdata", "formid", "mode" });
                        }
                    }
                }
                /// <summary>
                /// return[success,message]; 
                /// </summary>
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
                            _beforeSubmit.funcParam.AddRange(new String[] { "posdata", "formid" });
                        }
                    }
                }
                public funcMethodModule beforeInitData
                {
                    get
                    {

                        return _beforeInitData;
                    }
                    set
                    {
                        _beforeInitData = value;
                        if (_beforeInitData != null)
                        {
                            _beforeInitData.funcParam.AddRange(new String[] { "formid" });
                        }
                    }
                }
                public funcMethodModule beforeShowForm
                {
                    get
                    {

                        return _beforeShowForm;
                    }
                    set
                    {
                        _beforeShowForm = value;
                        if (_beforeShowForm != null)
                        {
                            //_beforeShowForm = new funcMethodModule();
                            _beforeShowForm.funcParam.AddRange(new String[] { "formid" });
                        }
                    }
                }
                public funcMethodModule onclickPgButtons
                {
                    get
                    {
                        if (_onclickPgButtons == null)
                        {
                            _onclickPgButtons = new funcMethodModule();
                            _onclickPgButtons.funcParam.AddRange(new String[] { "whichbutton", "formid", "rowid" });
                        }
                        return _onclickPgButtons;
                    }
                    set
                    {
                        _onclickPgButtons = value;
                    }
                }
                /// <summary>
                /// return {add_data} 
                /// </summary>
                public funcMethodModule onclickSubmit
                {
                    get
                    {

                        return _onclickSubmit;
                    }
                    set
                    {
                        _onclickSubmit = value;
                        if (_onclickSubmit != null)
                        {
                            //_onclickSubmit = new funcMethodModule();
                            _onclickSubmit.funcParam.AddRange(new String[] { "params", "posdata" });
                        }
                    }
                }
                public funcMethodModule onInitializeForm
                {
                    get
                    {

                        return _onInitializeForm;
                    }
                    set
                    {
                        _onInitializeForm = value;
                        if (_onInitializeForm != null)
                        {
                            //_onInitializeForm = new funcMethodModule();
                            _onInitializeForm.funcParam.AddRange(new String[] { "formid" });
                        }
                    }
                }
                public funcMethodModule onClose
                {
                    get
                    {

                        return _onClose;
                    }
                    set
                    {
                        _onClose = value;
                        if (_onClose != null)
                        {
                            //_onClose = new funcMethodModule();
                            _onClose.funcParam.AddRange(new String[] { });
                        }
                    }
                }
                public funcMethodModule errorTextFormat
                {
                    get
                    {

                        return _errorTextFormat;
                    }
                    set
                    {
                        _errorTextFormat = value;
                        if (_errorTextFormat != null)
                        {
                            //_errorTextFormat = new funcMethodModule();
                            _errorTextFormat.funcParam.AddRange(new String[] { });
                        }
                    }
                }
                public funcMethodModule serializeEditData
                {
                    get
                    {

                        return _serializeEditData;
                    }
                    set
                    {
                        _serializeEditData = value;
                        if (_serializeEditData != null)
                        {
                            //_serializeEditData = new funcMethodModule();
                            _serializeEditData.funcParam.AddRange(new String[] { });
                        }
                    }
                }

                public int? top { get; set; }
                public int? left { get; set; }
                public int? width { get; set; }
                public int? height { get; set; }
                public int? dataheight { get; set; }
                public Boolean? modal { get; set; }
                public Boolean? drag { get; set; }
                public Boolean? resize { get; set; }
                public String mtype { get; set; }
                public Dictionary<String, String> editData { get; set; }
                public Boolean? recreateForm { get; set; }
                public Boolean? jqModal { get; set; }

                /// <summary>
                /// first last
                /// </summary>
                public String addedrow { get; set; }
                public String topinfo { get; set; }
                public String bottominfo { get; set; }
                /// <summary>
                /// The default value is [true,”left”,”ui-icon-disk”]. 
                /// </summary>
                public String[] saveicon { get; set; }
                public String[] closeicon { get; set; }

                public String[] savekey { get; set; }
                public String[] navkeys { get; set; }

                public Boolean? checkOnSubmit { get; set; }
                public Boolean? checkOnUpdate { get; set; }
                public Boolean? closeAfterAdd { get; set; }
                public Boolean? clearAfterAdd { get; set; }
                public Boolean? closeAfterEdit { get; set; }
                public Boolean? reloadAfterSubmit { get; set; }
                public Boolean? closeOnEscape { get; set; }

                /// <summary>
                /// 請勿使用
                /// </summary>
                public String ajaxEditOptions { get; set; }
                public Boolean? viewPagerButtons { get; set; }

                public int? zIndex { get; set; }

                public String caption { get; set; }
                public String msg { get; set; }
                public String url { get; set; }

                public String ToScriptString()
                {
                    MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };
                    return jqOption.MakeScript();
                }
            }
        }
        public class navButtonAddModule : BaseJqueryScriptHelp, ijQueryUIScript
        {
            public navButtonAddModule()
            {
                onClickButton = new funcMethodModule();
            }
            public String caption { get; set; }
            public String buttonicon { get; set; }
            public funcMethodModule onClickButton { get; set; }
            public String position { get; set; }
            public String ToScriptString()
            {
                MakeJqScript jqOption = new MakeJqScript() { GetObject = this, needBrace = false };

                return jqOption.MakeScript();
            }
        }
    }

    public class JQGridDataEasy
    {
        public int total;
        public int page;
        public int records;
        public object rows;
    }

    public class JQGridDataObject
    {
        public int total;
        public int page;
        public int records;
        public RowElement[] rows;
    }

    public class RowElement
    {
        public String id;
        public String[] cell;
    }
    public class FormatterColumnScript
    {
        private funcMethodModule _f;
        public FormatterColumnScript()
        {
            _f = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.jqfunc };
            _f.funcParam.AddRange(new String[] { "cellValue", "options", "rowObject" });
        }
        /// <summary>
        /// 參數：cellValue, options, rowObject
        /// 取得Id： options.rowId
        /// </summary>
        public String FunctionName
        {
            get
            {
                return _f.funcName;
            }
            set
            {
                _f.funcName = value;
            }
        }
        public String FunctionString
        {
            get
            {
                return _f.funcString;
            }
            set
            {
                _f.funcString = value;
            }
        }
        public String JqueryFunctionString()
        {
            return _f.ToScriptString();
        }
        public funcMethodModule func
        {
            get { return _f; }
            set { _f = value; }
        }

        public void TakeButtonColumn(String Prefix, String text)
        {
            this.buttonRaise = true;
            _f.funcName = "$.f_" + Prefix;
            this.func.funcName = _f.funcName;
            _f.funcString = String.Format("var cellHtml = '<button type=\"button\" id=\"id_{0}_' + options.rowId + '\" class=\"class_{0}_button\" value=\"' + options.rowId + '\">{1}</button>';return cellHtml;", Prefix, text);
            this.buttonClass = "class_" + Prefix + "_button";
        }
        public Boolean buttonRaise { get; set; }
        public String buttonClass { get; set; }
    }
    #endregion
}