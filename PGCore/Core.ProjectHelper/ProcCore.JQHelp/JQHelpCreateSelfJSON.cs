using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp
{
    public class PropertyObjectJsonOption
    {
        public PropertyObjectJsonOption()
        {
            this.SetPropertyNameState = PropertyNameState.HaveItemName;
            this.NeedBrace = true;
        }
        public String PropertyName { get; set; }
        public PropertyNameState SetPropertyNameState { get; set; }
        public Boolean NeedBrace { get; set; }

        public enum PropertyNameState
        {
            NoItemName,
            HaveItemName,
            HaveItemNameAndArray,
            HaveItemObjectArray,
            HaveItemEnumArray
        }
    }

    /// <summary>
    /// 此版為自動遞廻版 會偵測是否有ToScriptString()此方法。
    /// </summary>
    public class MakeJqScript
    {
        /// <summary>
        /// 此enum來設定其屬性的陣列性質為何種 一般、enum or class
        /// </summary>
        enum SpecArrayType
        {
            GeneralType, EnumType, ClassType
        }

        public MakeJqScript()
        {
            ItemNotMake = new List<String>();
            ItemNoDot = new List<String>();
            ItemNameStop = new List<String>();
            ItemRename = new Dictionary<String, String>();
            needBrace = true;
        }

        public List<String> ItemNotMake { get; set; }
        public List<String> ItemNoDot { get; set; }
        public List<String> ItemNameStop { get; set; }
        public Dictionary<String, String> ItemRename { get; set; }
        /// <summary>
        /// Is Need { String }
        /// 自行下探物件不用設定 此屬性設成False
        /// </summary>
        public Boolean needBrace { get; set; }
        public Boolean FalseNotCreate { get; set; }

        public Object GetObject { get; set; }

        public String MakeScript()
        {
            return MK(GetObject,true);
        }

        public String MakeScript(Boolean needNewLine)
        {
            return MK(GetObject, needNewLine);
        }

        private String MK(Object JsonObject,Boolean needNewLine)
        {
            List<String> itm = new List<String>();

            //最後結果字串
            String _y = String.Empty;

            //取得物件全部屬性集合
            PropertyInfo[] _zs = JsonObject.GetType().GetProperties();

            foreach (PropertyInfo _z in _zs)
            {
                #region Loop Area

                _y = String.Empty;

                //項目名稱
                String _p = _z.Name;
                if (ItemNameStop.Contains(_p))
                    _p = "";

                if (ItemRename.Any(x => x.Key == _p))
                    _p = ItemRename[_p];

                if (!ItemNotMake.Contains(_p))
                {
                    #region 主區
                    if (_p.StartsWith("_"))
                        _p = _p.Substring(1);
                    //暫存物件值
                    Object _t = _z.GetValue(JsonObject, null);
                    //正式決定出的物件值
                    Object _V;

                    if (_t != null)
                    {
                        MethodInfo[] CollectMInfo = _t.GetType().GetMethods();
                        if (typeof(funcMethodModule) != _t.GetType() && CollectMInfo.Any(x => x.Name == "ToScriptString"))
                        {
                            var p = CollectMInfo.Where(x => x.Name == "ToScriptString" && x.GetParameters().Count() == 0).FirstOrDefault();

                            if (typeof(jqSelector) == _t.GetType())
                                _y = _p + ":" + (String)p.Invoke(_t, p.GetParameters());
                            else
                                _y = _p + ":{" + (String)p.Invoke(_t, p.GetParameters()) + "}";
                        }
                        else
                        {
                            #region 多重類型設定
                            if (_t.GetType() == typeof(MutileType))
                            {
                                MutileType MutileObj = (MutileType)_t;

                                if (MutileObj.attrType == MutileType.AttrType.DataModule)
                                {
                                    _V = MutileObj.dataModule;
                                }
                                else if (MutileObj.attrType == MutileType.AttrType.StringArray)
                                {
                                    _V = MutileObj.StringArray;
                                }
                                else if (MutileObj.attrType == MutileType.AttrType.funcMethod)
                                {
                                    _V = MutileObj.funcMethod;
                                }
                                else
                                {
                                    _V = MutileObj.String;
                                }
                            }
                            else
                            {
                                _V = _t;
                            }
                            #endregion

                            #region Handle Start
                            if (_V.GetType().IsArray)
                            {
                                #region 陣列集合性質
                                if (_V.GetType() == typeof(String[]))
                                {
                                    String[] S = (String[])_V;
                                    _y = _p + ":[" + S.ToArray().JoinArray(",", "'") + "]";
                                }
                                else if (_V.GetType() == typeof(int[]))
                                {
                                    int[] S = (int[])_V;
                                    _y = _p + ":[" + S.ToStringArray().JoinArray(",") + "]";
                                }
                                else
                                {
                                    #region 需Loop處理
                                    SpecArrayType aType = SpecArrayType.GeneralType;
                                    List<String> TrnStr = new List<String>();

                                    foreach (var _v in (Array)_V)
                                    {
                                        //限定Enum 陣列
                                        if (_v.GetType().IsEnum)
                                        {
                                            TrnStr.Add(_v.ToString());
                                            aType = SpecArrayType.EnumType;
                                        }

                                        //如果是Sub物件，另funcMethod沒看過有陣列方式實作
                                        MethodInfo GetSubToSctiptString = _v.GetType().GetMethod("ToScriptString");
                                        if (GetSubToSctiptString != null)
                                        {
                                            String SubArrayClassString = (String)GetSubToSctiptString.Invoke(_v, GetSubToSctiptString.GetParameters());
                                            TrnStr.Add(SubArrayClassString);
                                            aType = SpecArrayType.ClassType;
                                        }
                                    }

                                    if (aType == SpecArrayType.EnumType)
                                    {
                                        _y = _p + ":[" + TrnStr.ToArray().JoinArray(",", "\"") + "]";
                                    }

                                    if (aType == SpecArrayType.ClassType)
                                    {
                                        _y = _p + ":[" + TrnStr.ToArray().JoinArray(",", "{", "}") + "]";
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region 非陣判集合
                                if (_V.GetType() == typeof(FrameworkIcons))
                                {
                                    FrameworkIcons tObj = (FrameworkIcons)_V;
                                    if (tObj == FrameworkIcons.none)
                                        _y = "";
                                    else
                                        _y = _p + ":'" + tObj.ToString().Replace("_", "-") + "'";
                                }
                                else if (_V.GetType() == typeof(String) || _V.GetType().IsEnum)
                                {
                                    if (this.ItemNoDot.Exists(x => x.Contains(_z.Name)))
                                        _y = _p + ":" + _V.ToString();
                                    else
                                        _y = _p + ":\"" + _V.ToString() + "\"";
                                }
                                else if (_V.GetType() == typeof(Boolean))
                                {
                                    Boolean tObj = (Boolean)_V;
                                    if (tObj || (FalseNotCreate == false && tObj == false))
                                        _y = _p + ":" + _V.ToString().ToLower();
                                }
                                else if (_V.GetType() == typeof(int))
                                    _y = _p + ":" + _V.ToString();
                                else if (_V.GetType() == typeof(funcMethodModule))
                                {
                                    funcMethodModule tObj = ((funcMethodModule)_V);
                                    if (tObj.funcName != null || tObj.funcString != null)
                                        _y = _p + ":" + tObj.ToScriptString();
                                }
                                else if (_V.GetType() == typeof(DataModule))
                                {
                                    DataModule tObj = (DataModule)_V;
                                    if (tObj.Count > 0)
                                        _y = _p + ":" + tObj.ToJsonString();
                                }
                                else if (_V.GetType() == typeof(Dictionary<String, String>))
                                {
                                    Dictionary<String, String> dic = (Dictionary<String, String>)_V;
                                    List<String> hStr = new List<string>();
                                    foreach (var q in dic)
                                        hStr.Add(q.Key + ":\"" + q.Value + "\"");

                                    _y = _p + ":{" + hStr.ToArray().JoinArray(",") + "}";
                                }
                                #endregion
                            }
                            #endregion
                        }

                        if (_y.Trim() != "")
                        {
                            if (_p == "")
                                _y = _y.Substring(1);
                            itm.Add(_y);
                        }
                    }

                    #endregion
                }
                #endregion
            }

            #region Last Export
            if (needBrace)
            {
                if (needNewLine)
                {
                    return "{" + itm.ToArray().JoinArray(",\r\n") + "}";
                }
                else {
                    return "{" + itm.ToArray().JoinArray(",") + "}";
                }
            }
            else
            {
                if (needNewLine)
                {
                    return itm.ToArray().JoinArray(",\r\n");
                }
                else {
                    return itm.ToArray().JoinArray(",");
                }
            }
            #endregion
        }
    }
}
