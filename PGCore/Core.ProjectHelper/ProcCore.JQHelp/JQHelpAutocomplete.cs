using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.AutocompleteHelp
{
    /// <summary>
    /// 地址的Ajax Jquery Script 產生器
    /// </summary>
    public class AutocompleteHandle : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public AutocompleteHandle(jqSelector ElementID)
        {
            this.jqId = ElementID;
            this.Id = ElementID.IdName;
            this.Options = new Autocomplete();
        }

        public String Id { get; set; }
        public Autocomplete Options { get; set; }
        public String ToScriptString()
        {
            MakeJqScript c = new MakeJqScript() { GetObject = this.Options };
            return this.jqId.IDPack() + "autocomplete(" + c.MakeScript() + ");";
        }
        public String ToMethodSting(AutocompleteMethod Method, List<String> ExtraParmater)
        {
            String P_Str = String.Empty;

            if (ExtraParmater != null) {
                P_Str = "," + ExtraParmater.ToArray().JoinArray(",");
            }
            return "$('#" + this.Id + "').autocomplete(\"" + Method + "\"" + P_Str + ");";
        }

        public class Autocomplete
        {
            private funcMethodModule _create;
            private funcMethodModule _search;
            private funcMethodModule _open;
            private funcMethodModule _focus;
            private funcMethodModule _select;
            private funcMethodModule _close;
            private funcMethodModule _change;

            public funcMethodModule create
            {
                get
                {

                    return _create;
                }
                set
                {
                    _create = value;
                    if (_create != null)
                    {
                        _create.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            public funcMethodModule search
            {
                get
                {
                    return _search;
                }
                set
                {
                    _search = value;
                    if (_search != null)
                    {
                        _search.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            public funcMethodModule open
            {
                get
                {
                    return _open;
                }
                set
                {
                    _open = value;
                    if (_open != null)
                    {
                        _open.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            public funcMethodModule focus
            {
                get
                {
                    return _focus;
                }
                set
                {
                    _focus = value;
                    if (_focus != null)
                    {
                        _focus.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            /// <summary>
            /// parms:event,ui
            /// </summary>
            public funcMethodModule select
            {
                get
                {

                    return _select;
                }
                set
                {
                    _select = value;
                    if (_select != null)
                    {
                        _select.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            public funcMethodModule close
            {
                get
                {

                    return _close;
                }
                set
                {
                    _close = value;
                    if (_close != null)
                    {
                        _close.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }
            public funcMethodModule change
            {
                get
                {

                    return _change;
                }
                set
                {
                    _change = value;
                    if (_change != null)
                    {
                        _change.funcParam.AddRange(new String[] { "event", "ui" });
                    }
                }
            }

            public Autocomplete()
            {
            }

            public Boolean? disabled { get; set; }

            /// <summary>
            /// $( ".selector" ).autocomplete({ appendTo: "#someElem" });
            /// </summary>
            public String disappendTo { get; set; }

            public Boolean? autoFocus { get; set; }
            public int? delay { get; set; }
            public int? minLength { get; set; }

            public Position position { get; set; }
            public MutileType source { get; set; }

            public class Position
            {
                public String my { get; set; }
                public String at { get; set; }
                public String collision { get; set; }
            }

        }
        public enum AutocompleteMethod
        {
            destroy, disable, enable, widget, search, option, close
        }
        public class DataStruct {
            public String label { get; set; }
            public String value { get; set; }
        }
    }
}
