using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;

using ProcCore.NetExtension;
using ProcCore.JqueryHelp.AjaxHelp;


//給Jquery Form Submit 時做驗證
namespace ProcCore.JqueryHelp.JSTreeHelp
{
    public class JSTreeHandle : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public JSTreeHandle(String Id)
        {
            this.Id = Id;
            jsTree = new JSTree();
        }

        public String Id { get; set; }

        public JSTree jsTree { get; set; }

        public List<JSBind> jsBind { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = jsTree, FalseNotCreate = true };
            String s = String.Empty;

            foreach (JSBind jb in this.jsBind)
            {
                s += "." + jb.ToScriptString();
            }

            String tpl = "jQuery(\"#{0}\").jstree(\r\n\t{{\r\n\t\t{1}\r\n\t}}\r\n){2};";
            return String.Format(tpl, Id, CreateJson.MakeScript(), s);
        }
    }

    public enum JSBindEvent{
        select_node, load_node, loaded, open_node, before
    }
    public class JSBind : BaseJqueryScriptHelp, ijQueryUIScript {

        public JSBindEvent JsEvent { get; set; }

        /// <summary>
        /// 參數及格式function (event, data){}
        /// </summary>
        public String FunctionString {get;set;}
        public String ToScriptString()
        {
            String tpl = String.Format("bind(\"{0}.jstree\", function (event, data) {{ {1} }})",JsEvent,FunctionString);
            return tpl;
        }
    }

    public class JSTree : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public Json_Data json_data { get; set; }
        public String html_data { get; set; }
        public Core core { get; set; }
        public Themes themes { get; set; }
        public Plugins[] plugins { get; set; }
        public UI ui { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }
    public class Json_Data : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public DataStruct[] data { get; set; }
        public ajaxObject ajax { get; set; }
        public Boolean progressive_render { get; set; }
        public String icon { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }
    public class Core : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public Boolean? html_titles { get; set; }
        public int animation { get; set; }
        public String[] initially_open { get; set; }
        public String[] initially_load { get; set; }
        public Boolean? open_parents { get; set; }
        public Boolean? notify_plugins { get; set; }
        public Boolean? rtl { get; set; }
        public Dictionary<String, String> strings { get; set; }
        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }

    public class jsTreeAjaxData : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public DataStructAjax[] data { get; set; }
        public String ToScriptString()
        {
            String s = String.Empty;
            List<String> ls_RecordLVL = new List<String>();
            foreach (DataStructAjax DataLVL1 in data)
            {
                ls_RecordLVL.Add(DataLVL1.ToScriptString());
            }
            s =  ls_RecordLVL.ToArray().JoinArray(",") ;
            return s;
        }
    }
    public class DataStructAjax : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public Dictionary<String, String> attr { get; set; }
        public State state { get; set; }
        public DataStructAjax[] children { get; set; }

        public SubItemDataAjax data { get; set; }
        public Dictionary<String, String> metadata { get; set; }
        public String language { get; set; }

        public String ToScriptString()
        {
            List<String> ls_ExportString = new List<String>();

            if (data != null)
            {
                ls_ExportString.Add("\"data\":" + data.ToScriptString());
            }

            //
            if (attr != null)
            {
                List<String> ls_ItemHandle = new List<String>();
                foreach (var q in attr)
                {
                    ls_ItemHandle.Add("\"" + q.Key + "\":\"" + q.Value + "\"");
                }
                ls_ExportString.Add("\"attr\":{" + ls_ItemHandle.ToArray().JoinArray(",") + "}");
                ls_ItemHandle.Clear();
                ls_ItemHandle = null;
            }

            //
            ls_ExportString.Add("\"state\":\"" + state.ToString() + "\"");

            //
            if (children != null)
            {
                List<String> ls_ItemHandle = new List<String>();
                foreach (DataStructAjax dsa in children)
                {
                    ls_ItemHandle.Add(dsa.ToScriptString());
                }
                if (ls_ItemHandle.Count > 0)
                {
                    ls_ExportString.Add("\"children\":[" + ls_ItemHandle.ToArray().JoinArray(",") + "]");
                }
                ls_ItemHandle.Clear();
                ls_ItemHandle = null;
            }

            //
            if (metadata != null)
            {
                List<String> ls_ItemHandle = new List<String>();
                foreach (var q in metadata)
                {
                    ls_ItemHandle.Add("\"" + q.Key + "\":\"" + q.Value + "\"");
                }
                ls_ExportString.Add("\"metadata\":{" + ls_ItemHandle.ToArray().JoinArray(",") + "}");
                ls_ItemHandle.Clear();
                ls_ItemHandle = null;
            }

            if (language != null)
            {
                ls_ExportString.Add("\"language\":\"" + language + "\"");
            }

            return "{\r\n" + ls_ExportString.ToArray().JoinArray(",\r\n") + "\r\n}";
        }
    }
    public class SubItemDataAjax : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public String title { get; set; }
        public Dictionary<String, String> attr { get; set; }
        public String icon { get; set; }

        public String ToScriptString()
        {
            if (this.attr == null)
            {
                return "\"" + this.title + "\"";
            }
            else
            {
                List<String> ls_ExportString = new List<String>();

                ls_ExportString.Add("\"title\":\"" + title + "\"");

                //
                if (attr != null)
                {
                    List<String> ls_ItemHandle = new List<String>();
                    foreach (var q in attr)
                    {
                        ls_ItemHandle.Add("\"" + q.Key + "\":\"" + q.Value + "\"");
                    }
                    ls_ExportString.Add("\"attr\":{" + ls_ItemHandle.ToArray().JoinArray(",") + "}");
                    ls_ItemHandle.Clear();
                    ls_ItemHandle = null;
                }

                if (icon != null)
                {
                    ls_ExportString.Add("\"icon\":\"" + icon + "\"");
                }

                return "{" + ls_ExportString.ToArray().JoinArray(",\r\n") + "}";
            }
        }
    }
    public class DataStruct : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public Dictionary<String, String> attr { get; set; }
        public State state { get; set; }
        public DataStruct[] children { get; set; }

        public SubItemData data { get; set; }
        public Dictionary<String, String> metadata { get; set; }
        public String language { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }
    public class SubItemData : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public String title { get; set; }
        public Dictionary<String, String> attr { get; set; }
        public String icon { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }
    public class Themes : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public String theme { get; set; }
        public Boolean dots { get; set; }
        public Boolean icons { get; set; }
        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }
    public class UI : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public int select_limit { get; set; }
        public String select_multiple_modifier { get; set; }
        public String selected_parent_close { get; set; }
        public String select_range_modifier { get; set; }
        public Boolean selected_parent_open { get; set; }
        public Boolean select_prev_on_delete { get; set; }
        public Boolean disable_selecting_children { get; set; }
        public String[] initially_select { get; set; }

        public String ToScriptString()
        {
            MakeJqScript CreateJson = new MakeJqScript() { needBrace = false, GetObject = this, FalseNotCreate = true };
            return CreateJson.MakeScript();
        }
    }

    public enum ThemesStyle
    {
        apple
    }
    public enum State
    {
        closed, open
    }
    public enum Plugins
    {
        themes, html_data, json_data, xml_data, ui, sort, checkbox
    }
}