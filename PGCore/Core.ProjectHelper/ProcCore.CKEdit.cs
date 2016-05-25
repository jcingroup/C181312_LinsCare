using System;
using System.Collections.Generic;

using ProcCore.JqueryHelp;
using ProcCore.NetExtension;

namespace ProcCore.CKEdit
{
    public enum EditBarNames
    {
        document, clipboard, editing, forms, basicstyles, paragraph, links, insert, styles, colors, tools
    }
    public enum EditFun
    {
        Dot_, Source, Save, NewPage, DocProps, Preview, Print, Templates,
        Cut, Copy, Paste, PasteText, PasteFromWord, Undo, Redo,
        Find, Replace, SelectAll, SpellChecker, Scayt,
        Form, Checkbox, Radio, TextField, Textarea, Select, Button, ImageButton, HiddenField,
        Bold, Italic, Underline, Strike, Subscript, Superscript, RemoveFormat,
        NumberedList, BulletedList, Outdent, Indent, Blockquote, CreateDiv, JustifyLeft, JustifyCenter, JustifyRight, JustifyBlock, BidiLtr, BidiRtl,
        Link, Unlink, Anchor,
        Image, Flash, Table, HorizontalRule, Smiley, SpecialChar, PageBreak, Iframe,
        Styles, Format, Font, FontSize,
        TextColor, BGColor,
        Maximize, ShowBlocks, About
    }
    public enum EditSkin { moono,office2003, v2, kama }

    public class CKEditor : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public CKEditor(jqSelector ElementId)
        {
            this.jqId = ElementId;
            Options = new CKEditorOpton();
        }
        public String Id
        {
            get
            {
                return this.jqId.IdName;
            }
        }

        public class Toolbar: ijQueryUIScript
        {
            public EditBarNames name { get; set; }
            public EditFun[] items { get; set; }
            public String ToScriptString()
            {
                return ToOptionString();
            }
            public String ToOptionString() {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }
        }
        public class CKEditorOpton
        {
            public Toolbar[] toolbar { get; set; }
            public EditSkin skin { get; set; }
            public int height { get; set; }
            public String enterMode { get; set; }
            public String shiftEnterMode { get; set; }

            public String filebrowserBrowseUrl { get; set; }
            public String filebrowserImageBrowseUrl { get; set; }
            public String filebrowserFlashBrowseUrl { get; set; }
            public String filebrowserUploadUrl { get; set; }
            public String filebrowserImageUploadUrl { get; set; }
            public String filebrowserFlashUploadUrl { get; set; }
            public Int32? filebrowserWindowWidth { get; set; }
            public Int32? filebrowserWindowHeight { get; set; }
        }

        public CKEditorOpton Options { get; set; }
        public String ToScriptString()
        {
           return "CKEDITOR.replace( '" + this.jqId.IdName + "'," + ToOptionString() + ");"; 
        }
        public String ToOptionString() {
            MakeJqScript createJsonString = new MakeJqScript() { GetObject = this.Options };
            return createJsonString.MakeScript().Replace("Dot_", "-");        
        }
    }
}
