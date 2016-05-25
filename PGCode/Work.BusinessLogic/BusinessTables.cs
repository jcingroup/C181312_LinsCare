using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;

using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.TableFieldModule;

namespace ProcCore.Business.Logic.TablesDescription
{
    #region DataBase Module

    #region Area
    public class _AddressCity : TableMap<_AddressCity>
    {
        public _AddressCity() { N = "_AddressCity"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }}; }
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
    }
    public class _AddressCounty : TableMap<_AddressCounty>
    {
        public _AddressCounty() { N = "_AddressCounty"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }, { this.county.N, this.county }}; }
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule county = new FieldModule() { M = "county", N = "county", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { M = "zip", N = "zip", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule code = new FieldModule() { M = "code", N = "code", T = SQLValueType.String };
    }
    public class _BooleanSheet : TableMap<_BooleanSheet>
    {
        public _BooleanSheet() { N = "_BooleanSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Boolean.N, this.Boolean }}; }
        public FieldModule Boolean = new FieldModule() { M = "Boolean", N = "Boolean", T = SQLValueType.Boolean };
        public FieldModule sex = new FieldModule() { M = "sex", N = "sex", T = SQLValueType.String };
        public FieldModule yn = new FieldModule() { M = "yn", N = "yn", T = SQLValueType.String };
        public FieldModule ynv = new FieldModule() { M = "ynv", N = "ynv", T = SQLValueType.String };
        public FieldModule ynvx = new FieldModule() { M = "ynvx", N = "ynvx", T = SQLValueType.String };
    }
    public class _CodeHead : TableMap<_CodeHead>
    {
        public _CodeHead() { N = "_CodeHead"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule IsEdit = new FieldModule() { M = "IsEdit", N = "IsEdit", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _CodeSheet : TableMap<_CodeSheet>
    {
        public _CodeSheet() { N = "_CodeSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.CodeHeadId.N, this.CodeHeadId }, { this.Code.N, this.Code }, { this._語系.N, this._語系 }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule CodeHeadId = new FieldModule() { M = "CodeHeadId", N = "CodeHeadId", T = SQLValueType.Int };
        public FieldModule Code = new FieldModule() { M = "Code", N = "Code", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.String };
        public FieldModule Sort = new FieldModule() { M = "Sort", N = "Sort", T = SQLValueType.Int };
        public FieldModule IsUse = new FieldModule() { M = "IsUse", N = "IsUse", T = SQLValueType.Boolean };
        public FieldModule IsEdit = new FieldModule() { M = "IsEdit", N = "IsEdit", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _Currency : TableMap<_Currency>
    {
        public _Currency() { N = "_Currency"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name_currency = new FieldModule() { M = "name_currency", N = "name_currency", T = SQLValueType.String };
        public FieldModule english_currency = new FieldModule() { M = "english_currency", N = "english_currency", T = SQLValueType.String };
        public FieldModule sign = new FieldModule() { M = "sign", N = "sign", T = SQLValueType.String };
        public FieldModule code = new FieldModule() { M = "code", N = "code", T = SQLValueType.String };
    }
    public class _IDX : TableMap<_IDX>
    {
        public _IDX() { N = "_IDX"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.IDX.N, this.IDX }}; }
        public FieldModule IDX = new FieldModule() { M = "IDX", N = "IDX", T = SQLValueType.Int };
    }
    public class _Lang : TableMap<_Lang>
    {
        public _Lang() { N = "_Lang"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.lang.N, this.lang }}; }
        public FieldModule lang = new FieldModule() { M = "lang", N = "lang", T = SQLValueType.String };
        public FieldModule area = new FieldModule() { M = "area", N = "area", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { M = "memo", N = "memo", T = SQLValueType.String };
        public FieldModule isuse = new FieldModule() { M = "isuse", N = "isuse", T = SQLValueType.Boolean };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
    }
    public class _ParmBit : TableMap<_ParmBit>
    {
        public _ParmBit() { N = "_ParmBit"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmDateTime : TableMap<_ParmDateTime>
    {
        public _ParmDateTime() { N = "_ParmDateTime"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.DateTime };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmFloat : TableMap<_ParmFloat>
    {
        public _ParmFloat() { N = "_ParmFloat"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Int };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmInt : TableMap<_ParmInt>
    {
        public _ParmInt() { N = "_ParmInt"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Int };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmString : TableMap<_ParmString>
    {
        public _ParmString() { N = "_ParmString"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.String };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _PowerName : TableMap<_PowerName>
    {
        public _PowerName() { N = "_PowerName"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { M = "memo", N = "memo", T = SQLValueType.String };
    }
    public class _PowerUnit : TableMap<_PowerUnit>
    {
        public _PowerUnit() { N = "_PowerUnit"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ProgID.N, this.ProgID }, { this.UnitID.N, this.UnitID }, { this.PowerID.N, this.PowerID }}; }
        public FieldModule ProgID = new FieldModule() { M = "ProgID", N = "ProgID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { M = "UnitID", N = "UnitID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { M = "PowerID", N = "PowerID", T = SQLValueType.Int };
        public FieldModule AccessUnit = new FieldModule() { M = "AccessUnit", N = "AccessUnit", T = SQLValueType.Int };
    }
    public class _PowerUsers : TableMap<_PowerUsers>
    {
        public _PowerUsers() { N = "_PowerUsers"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ProgID.N, this.ProgID }, { this.UserID.N, this.UserID }, { this.PowerID.N, this.PowerID }}; }
        public FieldModule ProgID = new FieldModule() { M = "ProgID", N = "ProgID", T = SQLValueType.Int };
        public FieldModule UserID = new FieldModule() { M = "UserID", N = "UserID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { M = "PowerID", N = "PowerID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { M = "UnitID", N = "UnitID", T = SQLValueType.Int };
    }
    public class _WebCount : TableMap<_WebCount>
    {
        public _WebCount() { N = "_WebCount"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Cnt.N, this.Cnt }}; }
        public FieldModule Cnt = new FieldModule() { M = "Cnt", N = "Cnt", T = SQLValueType.Int };
    }
    public class _WebVisitData : TableMap<_WebVisitData>
    {
        public _WebVisitData() { N = "_WebVisitData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule ip = new FieldModule() { M = "ip", N = "ip", T = SQLValueType.String };
        public FieldModule setdate = new FieldModule() { M = "setdate", N = "setdate", T = SQLValueType.DateTime };
        public FieldModule browser = new FieldModule() { M = "browser", N = "browser", T = SQLValueType.String };
        public FieldModule source = new FieldModule() { M = "source", N = "source", T = SQLValueType.String };
        public FieldModule page = new FieldModule() { M = "page", N = "page", T = SQLValueType.String };
    }
    public class NewsData : TableMap<NewsData>
    {
        public NewsData() { N = "NewsData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule Title = new FieldModule() { M = "Title", N = "Title", T = SQLValueType.String };
        public FieldModule SetDate = new FieldModule() { M = "SetDate", N = "SetDate", T = SQLValueType.DateTime };
        public FieldModule IsOpen = new FieldModule() { M = "IsOpen", N = "IsOpen", T = SQLValueType.Boolean };
        public FieldModule NewsKind = new FieldModule() { M = "NewsKind", N = "NewsKind", T = SQLValueType.String };
        public FieldModule Context = new FieldModule() { M = "Context", N = "Context", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class ProgData : TableMap<ProgData>
    {
        public ProgData() { N = "ProgData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule area = new FieldModule() { M = "area", N = "area", T = SQLValueType.String };
        public FieldModule controller = new FieldModule() { M = "controller", N = "controller", T = SQLValueType.String };
        public FieldModule action = new FieldModule() { M = "action", N = "action", T = SQLValueType.String };
        public FieldModule path = new FieldModule() { M = "path", N = "path", T = SQLValueType.String };
        public FieldModule page = new FieldModule() { M = "page", N = "page", T = SQLValueType.String };
        public FieldModule prog_name = new FieldModule() { M = "prog_name", N = "prog_name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.String };
        public FieldModule isfolder = new FieldModule() { M = "isfolder", N = "isfolder", T = SQLValueType.Boolean };
        public FieldModule ishidden = new FieldModule() { M = "ishidden", N = "ishidden", T = SQLValueType.Boolean };
        public FieldModule isRoute = new FieldModule() { M = "isRoute", N = "isRoute", T = SQLValueType.Boolean };
        public FieldModule ismb = new FieldModule() { M = "ismb", N = "ismb", T = SQLValueType.Boolean };
        public FieldModule power_serial = new FieldModule() { M = "power_serial", N = "power_serial", T = SQLValueType.Int };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class TreeData : TableMap<TreeData>
    {
        public TreeData() { N = "TreeData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule deep = new FieldModule() { M = "deep", N = "deep", T = SQLValueType.Int };
        public FieldModule parentId = new FieldModule() { M = "parentId", N = "parentId", T = SQLValueType.Int };
        public FieldModule isfolder = new FieldModule() { M = "isfolder", N = "isfolder", T = SQLValueType.Boolean };
        public FieldModule link = new FieldModule() { M = "link", N = "link", T = SQLValueType.String };
        public FieldModule context = new FieldModule() { M = "context", N = "context", T = SQLValueType.String };
        public FieldModule tabs_1 = new FieldModule() { M = "tabs_1", N = "tabs_1", T = SQLValueType.String };
        public FieldModule tabs_2 = new FieldModule() { M = "tabs_2", N = "tabs_2", T = SQLValueType.String };
        public FieldModule tabs_3 = new FieldModule() { M = "tabs_3", N = "tabs_3", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class UnitData : TableMap<UnitData>
    {
        public UnitData() { N = "UnitData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class UsersData : TableMap<UsersData>
    {
        public UsersData() { N = "UsersData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule account = new FieldModule() { M = "account", N = "account", T = SQLValueType.String };
        public FieldModule password = new FieldModule() { M = "password", N = "password", T = SQLValueType.String };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule unit = new FieldModule() { M = "unit", N = "unit", T = SQLValueType.Int };
        public FieldModule state = new FieldModule() { M = "state", N = "state", T = SQLValueType.String };
        public FieldModule isadmin = new FieldModule() { M = "isadmin", N = "isadmin", T = SQLValueType.Boolean };
        public FieldModule type = new FieldModule() { M = "type", N = "type", T = SQLValueType.String };
        public FieldModule email = new FieldModule() { M = "email", N = "email", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { M = "zip", N = "zip", T = SQLValueType.String };
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule country = new FieldModule() { M = "country", N = "country", T = SQLValueType.String };
        public FieldModule address = new FieldModule() { M = "address", N = "address", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }

    ///<summary>
    ///消息資料檔
    ///</summary>
    public class Message : TableMap<Message>
    {
        public Message() { N = "消息"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule title = new FieldModule() { M = "title", N = "標題", T = SQLValueType.String };
        public FieldModule set_date = new FieldModule() { M = "set_date", N = "日期", T = SQLValueType.DateTime };
        public FieldModule context = new FieldModule() { M = "context", N = "內容", T = SQLValueType.String };
        public FieldModule isopen = new FieldModule() { M = "isopen", N = "開放", T = SQLValueType.Boolean };
        //public FieldModule kind = new FieldModule() { M = "kind", N = "分類", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class DocData : TableMap<DocData>
    {
        public DocData() { N = "DocData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule Title = new FieldModule() { M = "Title", N = "Title", T = SQLValueType.String };
        public FieldModule Context = new FieldModule() { M = "Context", N = "Context", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    #endregion

    #endregion
}
