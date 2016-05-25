using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.Mobile
{
    public class MobileMethod
    {
        protected String prefix = "$.mobile.";
        public class ChangePage : MobileMethod
        {
            public String to { get; set; }
            public Boolean? allowSamePageTransition { get; set; }
            public Boolean? changeHash { get; set; }
            public DataModule data { get; set; }
            public String dataUrl { get; set; }
            public String pageContainer { get; set; }
            public Boolean? reverse { get; set; }
            public String role { get; set; }
            public Boolean? showLoadMsg { get; set; }
            /// <summary>
            /// default: $.mobile.defaultPageTransition The transition to use when showing the page.
            /// </summary>
            public String transition { get; set; }
            public String type { get; set; }

            public String ToScriptString()
            {
                MakeJqScript CS = new MakeJqScript() { GetObject = this };
                CS.ItemNotMake.Add("to");
                return prefix + "changePage(" + to + "," + CS + ")";
            }
        }
        public class LoadPage : MobileMethod
        {

            public Boolean? allowSamePageTransition { get; set; }
            public Boolean? changeHash { get; set; }
            public DataModule data { get; set; }
            public int? loadMsgDelay { get; set; }
            public String pageContainer { get; set; }
            public Boolean? reloadPage { get; set; }
            public String role { get; set; }
            public Boolean? showLoadMsg { get; set; }
            /// <summary>
            /// default: $.mobile.defaultPageTransition The transition to use when showing the page.
            /// </summary>
            public String transition { get; set; }
            public String type { get; set; }

            public String ToScriptString()
            {
                MakeJqScript CS = new MakeJqScript() { GetObject = this };
                return prefix + "loadPage(" + CS + ")";
            }
        }
        public class Navigate : MobileMethod
        {
            public String url { get; set; }
            public String data { get; set; }

            public String ToScriptString()
            {
                return prefix + "navigate(" + url + "," + data + ")";
            }
        }
    }
    public abstract class Attrbute
    {
        public String ToAttribute()
        {
            List<String> itm = new List<String>();
            PropertyInfo[] _zs = this.GetType().GetProperties();
            foreach (PropertyInfo _z in _zs)
            {
                String an = _z.Name;
                if (an.StartsWith("_"))
                    an = an.Substring(1);

                String _a = String.Empty;
                if (an != "class")
                    _a = "data-" + an.Replace('_', '-');
                else
                    _a = an.Replace('_', '-');

                Object _t = _z.GetValue(this, null);
                if (_t != null)
                {
                    if (_t.GetType() == typeof(Boolean))
                        itm.Add(_a + "=\"" + _t.ToString().ToLower() + "\"");

                    if (_t.GetType() == typeof(String) || _t.GetType().IsEnum)
                    {
                        if (_t.GetType().IsEnum)
                        {
                            if (_t.ToString() != "no")
                                itm.Add(_a + "=\"" + _t.ToString().Replace('_', '-') + "\"");
                        }
                        else
                            itm.Add(_a + "=\"" + _t.ToString().Replace('_', '-') + "\"");
                    }
                }
            }

            return itm.ToArray().JoinArray(" ");
        }
    }

    public class m_Page : Attrbute
    {
        public m_Page()
        {
            role = "page";
        }
        public String role { get; set; }
        public Themes theme { get; set; }
        public Boolean? add_back_btn { get; set; }
        public String title { get; set; }
        public String url { get; set; }
        public String close_btn_text { get; set; }
        public Themes back_btn_themet { get; set; }
        public Themes overlay_theme { get; set; }
        public Boolean? fullscreen { get; set; }
        public Boolean? dom_cache { get; set; }
    }
    public class m_Header : Attrbute
    {
        public m_Header()
        {
            role = "header";
        }
        public String role { get; set; }
        public Themes theme { get; set; }
        public Position position { get; set; }
        public String id { get; set; }
        public Boolean? fullscreen { get; set; }
    }
    public class m_Footer : m_Header
    {
        public m_Footer()
        {
            role = "footer";
        }
    }
    public class m_Content : Attrbute
    {
        public String role { get; set; }
        public m_Content()
        {
            role = "content";
        }
        public Themes theme { get; set; }
    }
    public class m_Button : Attrbute
    {
        public m_Button()
        {
            role = "button";
        }
        public String role { get; set; }
        public Icon icon { get; set; }
        public IconPos iconpos { get; set; }
        public Boolean? iconshadow { get; set; }
        public Boolean? inline { get; set; }
        public Boolean? mini { get; set; }
        public Boolean? shadow { get; set; }
        public Themes theme { get; set; }

        public Rel rel { get; set; }
        public Transition transition { get; set; }

        public classset _class { get; set; }
        public enum classset
        {
            no, ui_btn_active
        }
    }
    public class m_Checkbox : Attrbute
    {
        public Boolean? mini { get; set; }
        public Themes theme { get; set; }
    }
    /// <summary>
    ///  &lt;div data-role=&quot;collapsible&quot;&gt;
    ///  &lt;h3&gt;標題&lt;/h3&gt;
    ///  &lt;p&gt;內容&lt;/p&gt;
    ///  &lt;/div&gt;
    /// </summary>
    public class m_Collapsible : Attrbute
    {
        public m_Collapsible()
        {
            role = "collapsible";
        }

        public String role { get; set; }
        public Boolean? collapsed { get; set; }
        public Icon collapsed_icon { get; set; }
        public Icon expanded_icon { get; set; }
        public IconPos iconpos { get; set; }
        public Boolean? iconshadow { get; set; }
        public Boolean? inset { get; set; }
        public Boolean? mini { get; set; }
        public Themes theme { get; set; }
        public Themes content_theme { get; set; }

    }
    /// <summary>
    /// &lt;div data-role=&quot;collapsible-set&quot;&gt;
    ///&lt;div data-role=&quot;collapsible&quot; data-collapsed=&quot;false&quot;&gt;
    ///&lt;h3&gt;Section 1&lt;/h3&gt;
    ///&lt;p&gt;I'm the collapsible set content for section B.&lt;/p&gt;
    ///&lt;/div&gt;
    ///&lt;div data-role=&quot;collapsible&quot;&gt;
    ///&lt;h3&gt;Section 2&lt;/h3&gt;
    ///&lt;p&gt;I'm the collapsible set content for section B.&lt;/p&gt;
    ///&lt;/div&gt;
    ///&lt;/div&gt;
    /// </summary>
    public class m_CollapsibleSet : Attrbute
    {
        public m_CollapsibleSet()
        {
            role = "collapsible-set";
        }

        public String role { get; set; }

        public Icon collapsed_icon { get; set; }
        public Icon expanded_icon { get; set; }
        public IconPos iconpos { get; set; }
        public Boolean? inset { get; set; }
        public Boolean? mini { get; set; }
        public Themes theme { get; set; }
        public Themes content_theme { get; set; }

    }
    public class m_Field : Attrbute
    {
        public m_Field()
        {
            role = "fieldcontain";
        }
        public String role { get; set; }
        public classset _class { get; set; }
        public enum classset
        {
            no, ui_hide_label
        }
    }

    /// <summary>
    /// &lt;div data-role=&quot;footer&quot;&gt;		
    /// &lt;div data-role=&quot;navbar&quot;&gt;
    ///	&lt;ul&gt;
    ///		&lt;li&gt;&lt;a href=&quot;#&quot;&gt;One&lt;/a&gt;&lt;/li&gt;
    ///		&lt;li&gt;&lt;a href=&quot;#&quot;&gt;Two&lt;/a&gt;&lt;/li&gt;
    ///		&lt;li&gt;&lt;a href=&quot;#&quot;&gt;Three&lt;/a&gt;&lt;/li&gt;
    ///	&lt;/ul&gt;
    /// &lt;/div&gt;
    /// &lt;/div&gt;
    /// </summary>
    public class m_navbar : Attrbute
    {
        public m_navbar()
        {
            role = "navbar";
        }
        public String role { get; set; }
    }

    public enum Themes
    {
        no, a, b, c, d, e
    }
    public enum Position
    {
        no, _fixed
    }
    public enum Icon
    {
        no, home, delete, plus, arrow_u, arrow_d, check, gear, grid, star, custom, arrow_r, arrow_l, minus, refresh, forward, back, alert, info, search
    }
    public enum IconPos
    {
        no, left, right, top, bottom, notext
    }
    public enum Rel
    {
        no, external, back, dialog
    }
    public enum Transition
    {
        no, pop, slide, slideup, fade, slidedown, flip
    }

    public enum data_attr
    {
        role, icon, theme, type, iconpos, inline, min, divider_theme, native_menu, overlay_theme, placeholder, rel, transition, tolerance,
        position_to, dismissable
    }



}
