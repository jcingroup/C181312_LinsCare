using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProcCore.JqueryHelp.FormvValidate;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.DialogHelp
{
    public class DialogUI : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public DialogUI(String DivId)
        {
            this.Id = DivId;
            this.options = new Options();
        }

        /// <summary>
        /// div的Id
        /// </summary>
        public String Id { get; set; }
        public Options options { get; set; }
        public virtual String ToScriptString()
        {
            return String.Format("$('#{0}').dialog({1});", this.Id, options.ToScriptString());
        }
        public class Options : ijQueryUIScript
        {

            #region 屬性區
            public Boolean? autoOpen { get; set; }
            public Boolean? closeOnEscape { get; set; }
            public String closeText { get; set; }
            public String dialogClass { get; set; }
            public Boolean? disabled { get; set; }
            public Boolean? draggable { get; set; }
            public Int32? height { get; set; }
            public Int32? maxHeight { get; set; }
            public Int32? maxWidth { get; set; }
            public Int32? minHeight { get; set; }
            public Int32? minWidth { get; set; }
            public Boolean? modal { get; set; }
            public String position { get; set; }
            public Boolean? resizable { get; set; }
            public Boolean? stack { get; set; }
            public String title { get; set; }
            public Int32? width { get; set; }
            public Int32? zIndex { get; set; }
            #endregion

            #region 事件區
            private funcMethodModule _beforeClose;
            private funcMethodModule _close;
            private funcMethodModule _create;
            private funcMethodModule _drag;
            private funcMethodModule _dragStart;
            private funcMethodModule _dragStop;
            private funcMethodModule _focus;
            private funcMethodModule _open;
            private funcMethodModule _resize;
            private funcMethodModule _resizeStart;
            private funcMethodModule _resizeStop;


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
                        _resizeStop.funcParam.AddRange(new String[] { "event", "ui" });
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
                        _resizeStart.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            public funcMethodModule resizepen
            {
                get
                {
                    return _resize;
                }
                set
                {
                    _resize = value;
                    if (_resize != null)
                        _resize.funcParam.AddRange(new String[] { "event", "ui" });
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
                        _open.funcParam.AddRange(new String[] { "event", "ui" });
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
                        _focus.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            public funcMethodModule dragStop
            {
                get
                {
                    return _dragStop;
                }
                set
                {
                    _dragStop = value;
                    if (_dragStop != null)
                        _dragStop.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            public funcMethodModule dragStart
            {
                get
                {
                    return _dragStart;
                }
                set
                {
                    _dragStart = value;
                    if (_dragStart != null)
                        _dragStart.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            public funcMethodModule drag
            {
                get
                {
                    return _drag;
                }
                set
                {
                    _drag = value;
                    if (_drag != null)
                        _drag.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            public funcMethodModule beforeClose
            {
                get
                {
                    return _beforeClose;
                }
                set
                {
                    _beforeClose = value;
                    if (_beforeClose != null)
                        _beforeClose.funcParam.AddRange(new String[] { "event", "ui" });
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
                        _close.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
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
                        _create.funcParam.AddRange(new String[] { "event", "ui" });
                }
            }
            #endregion
            public String ToScriptString()
            {
                return (new MakeJqScript() { GetObject = this, needBrace = true }).MakeScript();
            }
        }
    }
    /// <summary>
    /// Dialog包括Button 開啟Dialog的Script
    /// </summary>
    public class DialogButton : DialogUI, ijQueryUIScript
    {
        public DialogButton(String DivId, String ButtonId)
            : base(DivId)
        {
            this.jqOpenButtonElement = new jqElementEvent(new jqSelector() { IdName = ButtonId });
            this.jqOpenButtonElement.events.Add(new jqElementEvent.jqEvents()
            {
                htmlElementEvent = HtmlObjectEvent.click,
                funcString = String.Format("$('#{0}').dialog('open');", DivId)
            });
            this.Id = DivId;
        }
        public jqElementEvent jqOpenButtonElement { get; set; }
        public override String ToScriptString()
        {
            return base.ToScriptString() + jqOpenButtonElement.ToScriptString() + "";
        }
    }
}
