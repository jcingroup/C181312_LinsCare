using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcCore.JqueryHelp.ztreeHelp
{
    public class ztree : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public _async async { get; set; }
        public _callback callback { get; set; }
        public _check check { get; set; }
        public _data data { get; set; }
        public _edit edit { get; set; }
        public _view view { get; set; }

        public class _async : ijQueryUIScript
        {
            public _async()
            {
                this.enable = true;
                this.type = "post";
                this.dataType = "json";
                this.contentType = "application/x-www-form-urlencoded; charset=utf-8";
            }
            public String[] autoParam { get; set; }
            public String contentType { get; set; }
            public String dataType { get; set; }
            public Boolean? enable { get; set; }
            public String[] otherParam { get; set; }
            public String type { get; set; }
            public String url { get; set; }
            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }
        }
        public class _callback : ijQueryUIScript
        {

            public _callback()
            {

            }

            private funcMethodModule _beforeAsync;
            public funcMethodModule beforeAsync
            {
                get
                {
                    return _beforeAsync;
                }
                set
                {
                    _beforeAsync = value;
                    if (_beforeAsync != null)
                    {
                        _beforeAsync.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeCheck;
            public funcMethodModule beforeCheck
            {
                get
                {
                    return _beforeCheck;
                }
                set
                {
                    _beforeCheck = value;
                    if (_beforeCheck != null)
                    {
                        _beforeCheck.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeClick;
            public funcMethodModule beforeClick
            {
                get
                {
                    return _beforeClick;
                }
                set
                {
                    _beforeClick = value;
                    if (_beforeClick != null)
                    {
                        _beforeClick.funcParam.AddRange(new String[] { "treeId", "treeNode", "clickFlag" });
                    }
                }
            }

            private funcMethodModule _beforeCollapse;
            public funcMethodModule beforeCollapse
            {
                get
                {
                    return _beforeCollapse;
                }
                set
                {
                    _beforeCollapse = value;
                    if (_beforeCollapse != null)
                    {
                        _beforeCollapse.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeDblClick;
            public funcMethodModule beforeDblClick
            {
                get
                {
                    return _beforeDblClick;
                }
                set
                {
                    _beforeDblClick = value;
                    if (_beforeDblClick != null)
                    {
                        _beforeDblClick.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeDrag;
            public funcMethodModule beforeDrag
            {
                get
                {
                    return _beforeDrag;
                }
                set
                {
                    _beforeDrag = value;
                    if (_beforeDrag != null)
                    {
                        _beforeDrag.funcParam.AddRange(new String[] { "treeId", "treeNodes" });
                    }
                }
            }

            private funcMethodModule _beforeDragOpen;
            public funcMethodModule beforeDragOpen
            {
                get
                {
                    return _beforeDragOpen;
                }
                set
                {
                    _beforeDragOpen = value;
                    if (_beforeDragOpen != null)
                    {
                        _beforeDragOpen.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeDrop;
            public funcMethodModule beforeDrop
            {
                get
                {
                    return _beforeDrop;
                }
                set
                {
                    _beforeDrop = value;
                    if (_beforeDrop != null)
                    {
                        _beforeDrop.funcParam.AddRange(new String[] { "treeId", "treeNodes", "targetNode", "moveType" });
                    }
                }
            }

            private funcMethodModule _beforeEditName;
            public funcMethodModule beforeEditName
            {
                get
                {
                    return _beforeEditName;
                }
                set
                {
                    _beforeEditName = value;
                    if (_beforeEditName != null)
                    {
                        _beforeEditName.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeExpand;
            public funcMethodModule beforeExpand
            {
                get
                {
                    return _beforeExpand;
                }
                set
                {
                    _beforeExpand = value;
                    if (_beforeExpand != null)
                    {
                        _beforeExpand.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeMouseDown;
            public funcMethodModule beforeMouseDown
            {
                get
                {
                    return _beforeMouseDown;
                }
                set
                {
                    _beforeMouseDown = value;
                    if (_beforeMouseDown != null)
                    {
                        _beforeMouseDown.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeMouseUp;
            public funcMethodModule beforeMouseUp
            {
                get
                {
                    return _beforeMouseUp;
                }
                set
                {
                    _beforeMouseUp = value;
                    if (_beforeMouseUp != null)
                    {
                        _beforeMouseUp.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeRemove;
            public funcMethodModule beforeRemove
            {
                get
                {
                    return _beforeRemove;
                }
                set
                {
                    _beforeRemove = value;
                    if (_beforeRemove != null)
                    {
                        _beforeRemove.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _beforeRename;
            public funcMethodModule beforeRename
            {
                get
                {
                    return _beforeRename;
                }
                set
                {
                    _beforeRename = value;
                    if (_beforeRename != null)
                    {
                        _beforeRename.funcParam.AddRange(new String[] { "treeId", "treeNode", "newName" });
                    }
                }
            }

            private funcMethodModule _beforeRightClick;
            public funcMethodModule beforeRightClick
            {
                get
                {
                    return _beforeRightClick;
                }
                set
                {
                    _beforeRightClick = value;
                    if (_beforeRightClick != null)
                    {
                        _beforeRightClick.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                    }
                }
            }


            private funcMethodModule _onAsyncError;
            public funcMethodModule onAsyncError
            {
                get
                {
                    return _onAsyncError;
                }
                set
                {
                    _onAsyncError = value;
                    if (_onAsyncError != null)
                    {
                        _onAsyncError.funcParam.AddRange(new String[] { "event", "treeId", "treeNode", "XMLHttpRequest", "textStatus", "errorThrown" });
                    }
                }
            }

            private funcMethodModule _onAsyncSuccess;
            public funcMethodModule onAsyncSuccess
            {
                get
                {
                    return _onAsyncSuccess;
                }
                set
                {
                    _onAsyncSuccess = value;
                    if (_onAsyncSuccess != null)
                    {
                        _onAsyncSuccess.funcParam.AddRange(new String[] { "event", "treeId", "treeNode", "msg" });
                    }
                }
            }

            private funcMethodModule _onCheck;
            public funcMethodModule onCheck
            {
                get
                {
                    return _onCheck;
                }
                set
                {
                    _onCheck = value;
                    if (_onCheck != null)
                    {
                        _onCheck.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onClick;

            /// <summary>
            /// event, treeId, treeNode, clickFlag
            /// </summary>
            public funcMethodModule onClick
            {
                get
                {
                    return _onClick;
                }
                set
                {
                    _onClick = value;
                    if (_onClick != null)
                    {
                        _onClick.funcParam.AddRange(new String[] { "event", "treeId", "treeNode", "clickFlag" });
                    }
                }
            }

            private funcMethodModule _onCollapse;
            public funcMethodModule onCollapse
            {
                get
                {
                    return _onCollapse;
                }
                set
                {
                    _onCollapse = value;
                    if (_onCollapse != null)
                    {
                        _onCollapse.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onDblClick;
            public funcMethodModule onDblClick
            {
                get
                {
                    return _onDblClick;
                }
                set
                {
                    _onDblClick = value;
                    if (_onDblClick != null)
                    {
                        _onDblClick.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onDrag;
            public funcMethodModule onDrag
            {
                get
                {
                    return _onDrag;
                }
                set
                {
                    _onDrag = value;
                    if (_onDrag != null)
                    {
                        _onDrag.funcParam.AddRange(new String[] { "event", "treeId", "treeNodes" });
                    }
                }
            }

            private funcMethodModule _onDrop;
            public funcMethodModule onDrop
            {
                get
                {
                    return _onDrop;
                }
                set
                {
                    _onDrop = value;
                    if (_onDrop != null)
                    {
                        _onDrop.funcParam.AddRange(new String[] { "event", "treeId", "treeNodes", "targetNode", "moveType", "isCopy" });
                    }
                }
            }

            private funcMethodModule _onExpand;
            public funcMethodModule onExpand
            {
                get
                {
                    return _onExpand;
                }
                set
                {
                    _onExpand = value;
                    if (_onExpand != null)
                    {
                        _onExpand.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onMouseDown;
            public funcMethodModule onMouseDown
            {
                get
                {
                    return _onMouseDown;
                }
                set
                {
                    _onMouseDown = value;
                    if (_onMouseDown != null)
                    {
                        _onMouseDown.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onMouseUp;
            public funcMethodModule onMouseUp
            {
                get
                {
                    return _onMouseUp;
                }
                set
                {
                    _onMouseUp = value;
                    if (_onMouseUp != null)
                    {
                        _onMouseUp.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onNodeCreated;
            public funcMethodModule onNodeCreated
            {
                get
                {
                    return _onNodeCreated;
                }
                set
                {
                    _onNodeCreated = value;
                    if (_onNodeCreated != null)
                    {
                        _onNodeCreated.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onRemove;
            public funcMethodModule onRemove
            {
                get
                {
                    return _onRemove;
                }
                set
                {
                    _onRemove = value;
                    if (_onRemove != null)
                    {
                        _onRemove.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onRename;
            public funcMethodModule onRename
            {
                get
                {
                    return _onRename;
                }
                set
                {
                    _onRename = value;
                    if (_onRename != null)
                    {
                        _onRename.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            private funcMethodModule _onRightClick;
            public funcMethodModule onRightClick
            {
                get
                {
                    return _onRightClick;
                }
                set
                {
                    _onRightClick = value;
                    if (_onRightClick != null)
                    {
                        _onRightClick.funcParam.AddRange(new String[] { "event", "treeId", "treeNode" });
                    }
                }
            }

            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }
        }
        public class _check : ijQueryUIScript
        {
            public Boolean? autoCheckTrigger { get; set; }
            public String[] chkboxType { get; set; }
            /// <summary>
            /// checkbox radio
            /// </summary>
            public String chkStyle { get; set; }
            public Boolean? enable { get; set; }
            public Boolean? nocheckInherit { get; set; }
            /// <summary>
            /// radio 的分组范围。[setting.check.enable = true 且 setting.check.chkStyle = "radio" 时生效]
            /// 默认值："level"
            /// radioType = "level" 时，在每一级节点范围内当做一个分组。 
            /// radioType = "all" 时，在整棵树范围内当做一个分组。
            /// 请注意大小写，不要改变
            /// </summary>
            public String radioType { get; set; }

            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }
        }
        public class _data : ijQueryUIScript
        {
            public _keep keep { get; set; }
            public _key key { get; set; }
            public _simpleData simpleData { get; set; }
            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }

            public class _keep
            {
                public Boolean? leaf { get; set; }
                public Boolean? parent { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                    return createJsonString.MakeScript();
                }
            }
            public class _key
            {
                public String _checked { get; set; }
                public String children { get; set; }
                public String name { get; set; }
                public String title { get; set; }
                public String url { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                    return createJsonString.MakeScript();
                }
            }
            public class _simpleData : ijQueryUIScript
            {
                public _simpleData()
                {
                    enable = true;
                    this.idKey = "id";
                    this.pIdKey = "pId";
                    this.rootPId = 0;
                }
                public Boolean? enable { get; set; }
                public String idKey { get; set; }
                public String pIdKey { get; set; }
                public int? rootPId { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                    return createJsonString.MakeScript();
                }
            }
        }
        public class _edit : ijQueryUIScript
        {
            public _drag drag { get; set; }
            public Boolean? editNameSelectAll { get; set; }
            public Boolean? enable { get; set; }
            public String removeTitle { get; set; }
            public String renameTitle { get; set; }
            public Boolean? showRemoveBtn { get; set; }
            public Boolean? showRenameBtn { get; set; }



            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }

            public class _drag
            {
                public Boolean? autoExpandTrigger { get; set; }
                public Boolean? isCopy { get; set; }
                public Boolean? isMove { get; set; }
                public Boolean? prev { get; set; }
                public Boolean? next { get; set; }
                public Boolean? inner { get; set; }

                public int? borderMax { get; set; }
                public int? borderMin { get; set; }
                public int? minMoveSize { get; set; }
                public int? maxShowNodeNum { get; set; }
                public int? autoOpenTime { get; set; }
                public String ToScriptString()
                {
                    MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                    return createJsonString.MakeScript();
                }
            }
        }
        public class _view
        {
            private funcMethodModule _addDiyDom;
            private funcMethodModule _addHoverDom;
            private funcMethodModule _removeHoverDom;

            public Boolean? autoCancelSelected { get; set; }
            public Boolean? dblClickExpand { get; set; }
            public String expandSpeed { get; set; }
            public Boolean? nameIsHTML { get; set; }
            public Boolean? selectedMulti { get; set; }
            public Boolean? showIcon { get; set; }
            public Boolean? showLine { get; set; }
            public Boolean? showTitle { get; set; }
            public DataModule fontCss { get; set; }
            /// <summary>
            /// 用于在节点上固定显示用户自定义控件
            ///1. 大数据量的节点加载请注意：在 addDiyDom 中针对每个节点 查找 DOM 对象并且添加新 DOM 控件，肯定会影响初始化性能；如果不是必须使用，建议不使用此功能
            ///2. 属于高级应用，使用时请确保对 zTree 比较了解。
            ///默认值：nul
            /// </summary>
            public funcMethodModule addDiyDom {
                get
                {
                    return _addDiyDom;
                }
                set
                {
                    _addDiyDom = value;
                    if (_addDiyDom != null)
                        _addDiyDom.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                }
            }
            /// <summary>
            /// 用于当鼠标移动到节点上时，显示用户自定义控件，显示隐藏状态同 zTree 内部的编辑、删除按钮
            /// 请务必与 setting.view.removeHoverDom 同时使用；属于高级应用，使用时请确保对 zTree 比较了解。
            /// Function 参数说明
            /// treeId String 对应 zTree 的 treeId，便于用户操控
            /// treeNode JSON 需要显示自定义控件的节点 JSON 数据对象
            /// </summary>
            public funcMethodModule addHoverDom {
                get
                {
                    return _addHoverDom;
                }
                set
                {
                    _addHoverDom = value;
                    if (_addHoverDom != null)
                        _addHoverDom.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                }
            }
            public funcMethodModule removeHoverDom {
                get
                {
                    return _removeHoverDom;
                }
                set
                {
                    _removeHoverDom = value;
                    if (_removeHoverDom != null)
                        _removeHoverDom.funcParam.AddRange(new String[] { "treeId", "treeNode" });
                }
            }
            public String ToScriptString()
            {
                MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = false };
                return createJsonString.MakeScript();
            }
        }
        public String ToScriptString()
        {
            MakeJqScript createJsonString = new MakeJqScript() { GetObject = this, needBrace = true };
            createJsonString.ItemNoDot = new List<String>();
            createJsonString.ItemNoDot.Add("async");
            createJsonString.ItemNoDot.Add("callback");
            createJsonString.ItemNoDot.Add("check");
            createJsonString.ItemNoDot.Add("data");
            createJsonString.ItemNoDot.Add("edit");
            createJsonString.ItemNoDot.Add("view");

            String r = createJsonString.MakeScript();
            return r;
        }
    }

    public class treeNode
    {
        public Int32 id { get; set; }
        public Int32 pId { get; set; }

        public Boolean? _checked { get; set; }
        public Boolean? chkDisabled { get; set; }
        public Boolean? halfCheck { get; set; }
        public String icon { get; set; }
        public String iconClose { get; set; }
        public String iconOpen { get; set; }
        public String iconSkin { get; set; }
        public Boolean? isHidden { get; set; }
        /// <summary>
        /// 记录 treeNode 节点是否为父节点。
        /// </summary>
        public Boolean? isParent { get; set; }
        public String name { get; set; }
        public Boolean? nocheck { get; set; }
        public Boolean? open { get; set; }
        public String target { get; set; }
        public String url { get; set; }
        public Int32? level { get; set; }

        public Int32? check_Child_State { get; set; }
        public Boolean? checkedOld { get; set; }

        public Boolean? isAjaxing { get; set; }
        public Boolean? isFirstNode { get; set; }
        public Boolean? isLastNode { get; set; }

        public String parentTId { get; set; }
        public String tId { get; set; }

        public treeNode children { get; set; }
    }
}
