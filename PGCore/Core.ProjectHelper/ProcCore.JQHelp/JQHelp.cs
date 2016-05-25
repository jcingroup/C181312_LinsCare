using System;
using System.Collections.Generic;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp
{
    #region Form 基礎物件
    public enum formMethods
    {
        post, get
    }
    public enum formEnctype
    {
        application_x_www_form_urlencoded,
        multipart_form_data,
        text_plain
    }
    public class Form
    {
        public Form()
        {
            this.method = formMethods.post;
            this.enctype = formEnctype.application_x_www_form_urlencoded;
        }
        public String Id { get; set; }
        public String action { get; set; }
        public formMethods method { get; set; }
        public formEnctype enctype { get; set; }
        private String GetEnctype()
        {

            if (enctype == formEnctype.application_x_www_form_urlencoded)
                return "application/x-www-form-urlencoded";

            else if (enctype == formEnctype.multipart_form_data)
                return "multipart/form-data";

            else
                return "text/plain";
        }
        public String ExportFormContextTag(String formInnerHtml)
        {
            return ExportFormStartTag() + formInnerHtml + "</form>";
        }
        /// <summary>
        /// 提供form 啟始Tag內容
        /// </summary>
        public String ExportFormStartTag()
        {
            return String.Format("<form id=\"{0}\" name=\"{0}\" method=\"{1}\" action=\"{2}\" enctype=\"{3}\">",
                this.Id, this.method, this.action, this.GetEnctype());
        }

        public String SubmitScript(String script)
        {
            return "$('#" + this.Id + "').submit(function(){" + script + "});";
        }
    }
    #endregion

    /// <summary>
    /// 製作Jquery Help Script產生器 Options
    /// </summary>
    interface ijQueryUIOption
    {
        //String ToOptionString();
    }

    /// <summary>
    /// 製作Jquery Help Script產生器 必需引用的 interface
    /// </summary>
    interface ijQueryUIScript : ijQueryUIOption
    {
        /// <summary>
        /// 統一使用 ToScriptString() 做最後的script字串產生
        /// </summary>
        /// <returns></returns>
        String ToScriptString();
    }

    interface iHtmlToString
    {
        String ToHtmlString();
    }

    public class jqSelector : ijQueryUIScript
    {
        public enum IdMakeStyle
        {
            ElementId, ClassName, TagStringName, WindowObject, DocumentObject
        }
        public jqSelector()
        {
            IdStyle = IdMakeStyle.ElementId;
        }
        /// <summary>
        /// 原Id Name
        /// </summary>
        public String IdName { get; set; }
        public IdMakeStyle IdStyle { get; set; }
        /// <summary>
        /// 設定Id格式
        /// </summary>
        /// <returns>依設定 retur # Id or . class</returns>
        public String ToOptionString()
        {
            if (IdStyle == IdMakeStyle.ElementId)
                return "'#" + IdName + "'";
            else if (IdStyle == IdMakeStyle.ClassName)
                return "'." + IdName + "'";
            else if (IdStyle == IdMakeStyle.TagStringName)
                return "'" + IdName + "'";
            else if (IdStyle == IdMakeStyle.WindowObject)
                return "window";
            else if (IdStyle == IdMakeStyle.DocumentObject)
                return "document";
            else
                return "";
        }
        /// <summary>
        /// 包裝成 $('{0}').
        /// </summary>
        /// <returns></returns>
        public String IDPack()
        {
            return String.Format("$({0}).", ToOptionString());
        }
        public String ToScriptString()
        {
            return String.Format("$({0})", ToOptionString());
        }
    }

    public abstract class BaseJqueryScriptHelp
    {
        protected jqSelector jqId;
    }
    /// <summary>
    /// jQuery element event script maker
    /// </summary>
    //public class jqElementEvent : BaseJqueryScriptHelp, ijQueryUIScript
    //{
    //    private jqSelector _ElementId;
    //    public jqElementEvent(jqSelector Id)
    //    {
    //        this._ElementId = Id;
    //        this.jqRaiseEvent = jqEventStyle.normal;
    //        this.funcObject = new funcMethodModule();
    //        this.funcObject.MakeStyle = funcMethodModule.funcMakeStyle.funcConext;
    //    }
    //    public HtmlObjectEvent Event { get; set; }

    //    public funcMethodModule funcObject { get; set; }
    //    public jqEventStyle jqRaiseEvent { get; set; }
    //    public virtual String ToScriptString()
    //    {
    //        return ToOptionString();
    //    }
    //    public String ToOptionString()
    //    {
    //        #region MyRegion

    //        String EventName = Event.ToString();

    //        if (this._ElementId != null && this.funcObject.funcString != null)
    //        {
    //            if (this.jqRaiseEvent == jqEventStyle.normal)
    //                return _ElementId.IDPack() + EventName + "(" + this.funcObject.ToScriptString() + ");";
    //            if (this.jqRaiseEvent == jqEventStyle.on)
    //                return "$(document).on('" + EventName + "','" + _ElementId.IdName + "'," + this.funcObject.ToScriptString() + ");";
    //            if (this.jqRaiseEvent == jqEventStyle.bind)
    //                return _ElementId.IDPack() + "bind('" + EventName + "'," + this.funcObject.ToScriptString() + ");";
    //            else
    //                return "";
    //        }
    //        else
    //            return "";
    //        #endregion
    //    }
    //    public String Id
    //    {
    //        get
    //        {
    //            return _ElementId.IdName;
    //        }
    //    }
    //}

    public class jqElementEvent : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public String Id
        {
            get
            {
                return this.jqId.IdName;
            }
        }
        public jqElementEvent(jqSelector Object)
        {
            this.jqId = Object;
            this.events = new List<jqEvents>();
        }

        public jqEventStyle jqRaiseEvent { get; set; }
        public List<jqEvents> events { get; set; }

        public virtual String ToScriptString()
        {
            List<String> r = new List<String>();

            if (this.jqId != null)
                r.Add(this.jqId.ToScriptString());


            foreach (jqEvents e in events)
            {
                String EventName = e.Is_Mobile ? e.mobilePageEvent.ToString() : e.htmlElementEvent.ToString();

                if (jqRaiseEvent == jqEventStyle.normal)
                {
                    r.Add(EventName + "(" + e.funcObject.ToScriptString() + ")");
                }
                else if (jqRaiseEvent == jqEventStyle.on)
                {
                    r.Add("on('" + EventName + "'," + e.funcObject.ToScriptString() + ")");
                }
                else if (jqRaiseEvent == jqEventStyle.bind)
                {
                    r.Add("bind('" + EventName + "'," + e.funcObject.ToScriptString() + ")");
                }
                else if (jqRaiseEvent == jqEventStyle.functionName)
                {

                }
            }
            return r.ToArray().JoinArray(".");

        }
        public class jqEvents
        {
            public Boolean Is_Mobile { get; set; }
            private HtmlObjectEvent _he;
            private MobilePageEvent _me;

            public jqEvents()
            {
                funcObject = new funcMethodModule() { MakeStyle = funcMethodModule.funcMakeStyle.funcConext };
            }
            public funcMethodModule funcObject { get; set; }
            public String funcString
            {
                get
                {
                    return this.funcObject.funcString;
                }

                set
                {
                    this.funcObject.funcString = value;
                }
            }
            public HtmlObjectEvent htmlElementEvent
            {
                get { return _he; }
                set
                {
                    _he = value;
                    Is_Mobile = false;
                }
            }
            public MobilePageEvent mobilePageEvent
            {
                get { return _me; }
                set
                {
                    _me = value;
                    Is_Mobile = true;
                    funcObject.funcParam.Add("event");
                }
            }

        }
    }
    public class funcBase : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public funcBase()
        {
            this.funcParam = new List<String>();
        }

        public String funcString { get; set; }
        public List<String> funcParam { get; set; }
        public String[] parmsRange
        {
            set
            {
                funcParam.AddRange(value);
            }
        }
        public funcBase[] funcThen { get; set; }
        public virtual String ToScriptString()
        {
            return ToOptionString();
        }
        public String ToOptionString()
        {
            if (this.funcThen != null)
            {
                List<String> l = new List<String>();
                foreach (funcBase f in funcThen)
                    l.Add(f.ToScriptString());
                return String.Join(",", l.ToArray());
            }
            else
            {
                return "function (" + String.Join(",", funcParam.ToArray()) + "){" + funcString + "}";
            }
        }

    }
    public class funcMethodModule : funcBase, ijQueryUIScript
    {
        public enum funcMakeStyle
        {
            onlyName, complete, funcConext, jqfunc
        }
        public funcMethodModule()
            : base()
        {
            this.MakeStyle = funcMakeStyle.onlyName;
        }

        public funcMakeStyle MakeStyle { get; set; }
        public String funcName { get; set; }
        public String ToScriptString(funcMakeStyle _MakeStyle)
        {
            String script = String.Empty;

            if (_MakeStyle == funcMakeStyle.onlyName)
                return funcName;

            else if (_MakeStyle == funcMakeStyle.funcConext)
                return "function (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "}";

            else if (_MakeStyle == funcMakeStyle.complete)
                return "function " + this.funcName + " (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "}";

            else if (_MakeStyle == funcMakeStyle.jqfunc)
                return this.funcName + "= function (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "};";
            else
                return "";
        }
        public override String ToScriptString()
        {
            String script = String.Empty;

            if (this.MakeStyle == funcMakeStyle.onlyName)
                return funcName;

            else if (this.MakeStyle == funcMakeStyle.funcConext)
                return "function (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "}";

            else if (this.MakeStyle == funcMakeStyle.complete)
                return "function " + this.funcName + " (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "}";

            else if (this.MakeStyle == funcMakeStyle.jqfunc)
                return this.funcName + "= function (" + String.Join(",", funcParam.ToArray()) + ") { " + funcString + "};";
            else
                return "";
        }
    }

    public class DataModule
    {
        private SimpleDataJson _Data;

        public DataModule()
        {
            _Data = new SimpleDataJson();
        }

        public void Add(String key, String value)
        {
            _Data.Add(key, value);
        }

        public int Count
        {
            get
            {
                return _Data.Count;
            }
        }

        public String ToJsonString()
        {
            return _Data.ToJsonString();
        }

    }
    public class MutileType
    {
        public enum AttrType
        {
            String, StringArray, funcMethod, DataModule
        }
        public AttrType attrType { get; set; }
        public String String { get; set; }
        public String[] StringArray { get; set; }
        public funcMethodModule funcMethod { get; set; }
        public DataModule dataModule { get; set; }
    }
    /// <summary>
    /// 功能:簡單式 JSON 產生器 主要用在JQuery有一些pluging其參教為Data:{ A=1,B=2 } 這類簡式JSON
    /// 列如 $.ajax  中的 data 參數
    /// </summary>
    public class SimpleDataJson
    {
        Dictionary<String, String> ItemValue = new Dictionary<String, String>();

        public void Add(String key, String value)
        {
            this.ItemValue.Add(key, value);
        }

        public int Count
        {
            get
            {
                return this.ItemValue.Count;
            }
        }

        public String ToJsonString()
        {
            List<String> val = new List<String>();
            foreach (var KeyValue in this.ItemValue)
            {
                val.Add("'" + KeyValue.Key + "':" + KeyValue.Value);
            }
            //this.ItemValue.Clear();
            return "{" + String.Join(",", val.ToArray()) + "}";
        }
    }

    /// <summary>
    /// Jquery Ui 動畫特效
    /// </summary>
    public enum UiEffects
    {
        Blind,
        Clip,
        Drop,
        Expl,
        Fade,
        Fold,
        Puff,
        Slide,
        Scale,

        Bounce,
        Highlight,
        Pulsate,
        Shake,
        Size,
        Transfer,

        show,
        slideDown,
        fadeIn
    }
    public enum FrameworkIcons
    {
        none,
        ui_icon_carat_1_n,
        ui_icon_carat_1_ne,
        ui_icon_carat_1_e,
        ui_icon_carat_1_se,
        ui_icon_carat_1_s,
        ui_icon_carat_1_sw,
        ui_icon_carat_1_w,
        ui_icon_carat_1_nw,
        ui_icon_carat_2_n_s,
        ui_icon_carat_2_e_w,
        ui_icon_triangle_1_n,
        ui_icon_triangle_1_ne,
        ui_icon_triangle_1_e,
        ui_icon_triangle_1_se,
        ui_icon_triangle_1_s,
        ui_icon_triangle_1_sw,
        ui_icon_triangle_1_w,
        ui_icon_triangle_1_nw,
        ui_icon_triangle_2_n_s,
        ui_icon_triangle_2_e_w,
        ui_icon_arrow_1_n,
        ui_icon_arrow_1_ne,
        ui_icon_arrow_1_e,
        ui_icon_arrow_1_se,
        ui_icon_arrow_1_s,
        ui_icon_arrow_1_sw,
        ui_icon_arrow_1_w,
        ui_icon_arrow_1_nw,
        ui_icon_arrow_2_n_s,
        ui_icon_arrow_2_ne_sw,
        ui_icon_arrow_2_e_w,
        ui_icon_arrow_2_se_nw,
        ui_icon_arrowstop_1_n,
        ui_icon_arrowstop_1_e,
        ui_icon_arrowstop_1_s,
        ui_icon_arrowstop_1_w,
        ui_icon_arrowthick_1_n,
        ui_icon_arrowthick_1_ne,
        ui_icon_arrowthick_1_e,
        ui_icon_arrowthick_1_se,
        ui_icon_arrowthick_1_s,
        ui_icon_arrowthick_1_sw,
        ui_icon_arrowthick_1_w,
        ui_icon_arrowthick_1_nw,
        ui_icon_arrowthick_2_n_s,
        ui_icon_arrowthick_2_ne_sw,
        ui_icon_arrowthick_2_e_w,
        ui_icon_arrowthick_2_se_nw,
        ui_icon_arrowthickstop_1_n,
        ui_icon_arrowthickstop_1_e,
        ui_icon_arrowthickstop_1_s,
        ui_icon_arrowthickstop_1_w,
        ui_icon_arrowreturnthick_1_w,
        ui_icon_arrowreturnthick_1_n,
        ui_icon_arrowreturnthick_1_e,
        ui_icon_arrowreturnthick_1_s,
        ui_icon_arrowreturn_1_w,
        ui_icon_arrowreturn_1_n,
        ui_icon_arrowreturn_1_e,
        ui_icon_arrowreturn_1_s,
        ui_icon_arrowrefresh_1_w,
        ui_icon_arrowrefresh_1_n,
        ui_icon_arrowrefresh_1_e,
        ui_icon_arrowrefresh_1_s,
        ui_icon_arrow_4,
        ui_icon_arrow_4_diag,
        ui_icon_extlink,
        ui_icon_newwin,
        ui_icon_refresh,
        ui_icon_shuffle,
        ui_icon_transfer_e_w,
        ui_icon_transferthick_e_w,
        ui_icon_folder_collapsed,
        ui_icon_folder_open,
        ui_icon_document,
        ui_icon_document_b,
        ui_icon_note,
        ui_icon_mail_closed,
        ui_icon_mail_open,
        ui_icon_suitcase,
        ui_icon_comment,
        ui_icon_person,
        ui_icon_print,
        ui_icon_trash,
        ui_icon_locked,
        ui_icon_unlocked,
        ui_icon_bookmark,
        ui_icon_tag,
        ui_icon_home,
        ui_icon_flag,
        ui_icon_calculator,
        ui_icon_cart,
        ui_icon_pencil,
        ui_icon_clock,
        ui_icon_disk,
        ui_icon_calendar,
        ui_icon_zoomin,
        ui_icon_zoomout,
        ui_icon_search,
        ui_icon_wrench,
        ui_icon_gear,
        ui_icon_heart,
        ui_icon_star,
        ui_icon_link,
        ui_icon_cancel,
        ui_icon_plus,
        ui_icon_plusthick,
        ui_icon_minus,
        ui_icon_minusthick,
        ui_icon_close,
        ui_icon_closethick,
        ui_icon_key,
        ui_icon_lightbulb,
        ui_icon_scissors,
        ui_icon_clipboard,
        ui_icon_copy,
        ui_icon_contact,
        ui_icon_image,
        ui_icon_video,
        ui_icon_alert,
        ui_icon_info,
        ui_icon_notice,
        ui_icon_help,
        ui_icon_check,
        ui_icon_bullet,
        ui_icon_radio_off,
        ui_icon_radio_on,
        ui_icon_play,
        ui_icon_pause,
        ui_icon_seek_next,
        ui_icon_seek_prev,
        ui_icon_seek_end,
        ui_icon_seek_first,
        ui_icon_stop,
        ui_icon_eject,
        ui_icon_volume_off,
        ui_icon_volume_on,
        ui_icon_power,
        ui_icon_signal_diag,
        ui_icon_signal,
        ui_icon_battery_0,
        ui_icon_battery_1,
        ui_icon_battery_2,
        ui_icon_battery_3,
        ui_icon_circle_plus,
        ui_icon_circle_minus,
        ui_icon_circle_close,
        ui_icon_circle_triangle_e,
        ui_icon_circle_triangle_s,
        ui_icon_circle_triangle_w,
        ui_icon_circle_triangle_n,
        ui_icon_circle_arrow_e,
        ui_icon_circle_arrow_s,
        ui_icon_circle_arrow_w,
        ui_icon_circle_arrow_n,
        ui_icon_circle_zoomin,
        ui_icon_circle_zoomout,
        ui_icon_circle_check,
        ui_icon_circlesmall_plus,
        ui_icon_circlesmall_minus,
        ui_icon_circlesmall_close,
        ui_icon_squaresmall_plus,
        ui_icon_squaresmall_minus,
        ui_icon_squaresmall_close,
        ui_icon_grip_dotted_vertical,
        ui_icon_grip_dotted_horizontal,
        ui_icon_grip_solid_vertical,
        ui_icon_grip_solid_horizontal,
        ui_icon_gripsmall_diagonal_se,
        ui_icon_grip_diagonal_se

    }
    public enum HtmlObjectEvent
    {
        blur, change, click, dblclick, error, focus, focusin, focusout, keydown, keypress, keyup, load, mousedown, mouseenter, mousemove, mouseover, resize, scroll, select, submit, unload
    }
    public enum MobilePageEvent
    {
        pagebeforechange, pagebeforecreate, pagebeforehide, pagebeforeload, pagebeforeshow, pagechange, pagechangefailed, pagecreate, pagehide, pageinit, pageload, pageloadfailed, pageremove, pageshow, scrollstart, scrollstop, swipe, swipeleft, swiperight, tap, taphold, throttledresize, updatelayout
    }

    public enum jqEventStyle
    {
        normal, on, bind, functionName
    }
}
