using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.NetExtension;
using ProcCore.ReturnAjaxResult;
using ProcCore.WebCore;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class UnitController : BaseAction<m_Unit, a_Unit, q_Unit, UnitData>
    {
        public UnitController() {
        }

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public override ActionResult ListGrid(q_Unit sh)
        {
            operationMode = OperationMode.EnterList;
            HandleRequest HRq = new HandleRequest();
            //記錄QueryString
            HRq.encodeURIComponent = true;
            HRq.Remove("page");

            ViewBag.Page = QueryGridPage();
            ViewBag.Caption = GetSystemInfo().prog_name;
            ViewBag.AppendQuertString = HRq.ToQueryString();
            HRq = null;

            return View("ListData", sh);
        }

        public override ActionResult EditMasterNewData()
        {
            operationMode = OperationMode.EditInsert;
            md = new m_Unit();

            //設定預設值
            md.EditType = EditModeType.Insert;

            ac = new a_Unit();
            ac.Connection = getSQLConnection();
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("Id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;
            return View("EditData", md);
        }

        public override ActionResult EditMasterDataByID(int id)
        {
            operationMode = OperationMode.EditModify;
            ac = new a_Unit();
            ac.Connection = getSQLConnection();

            RunOneDataEnd<m_Unit> HResult = ac.GetDataMaster(id,  LoginUserId);
            md = HResult.SearchData;
            md.EditType = EditModeType.Modify;
            HandleResultCheck(HResult);
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;

            return View("EditData", md);
        }

        protected override void HandleCollectDataToOptions()
        {
            //ViewBag.NewsKind_Option = MakeDropDownOption(ac.GetGroupCodeValue("NewsKind"));
            a_Users ac_User = new a_Users() { Connection = getSQLConnection()};
            LogicAddress address = new LogicAddress() { Connection=this.getSQLConnection() };

            ViewBag.Unit_Option = MakeCollectDataToOptions(ac_User.MakeOption_Unit(),false);
            ViewBag.City_Option = MakeCollectDataToOptions(address.GetCity(), false);
            ViewBag.Country_Option = MakeCollectDataToOptions(address.GetCounty(""), false);
        }

        #region Ajax Call Section
        [HttpGet]
        public override string ajax_MasterGridData(q_Unit queryObj)
        {

            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_Unit();
            ac.Connection = this.getSQLConnection();
            RunQueryPackage<m_Unit> HResult = ac.SearchMaster(queryObj,LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 組成資料
            List<RowElement> setRowElement = new List<RowElement>();
            var drDataCollect = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            this.Tab = new UnitData();
            foreach (var dr in drDataCollect)
            {
                RowElement GridRow = new RowElement() { id = dr.id.ToString(), cell = new String[3] };

                GridRow.cell[0] = dr.id.ToString();
                GridRow.cell[1] = dr.name;
                GridRow.cell[2] = dr.sort.ToString();

                setRowElement.Add(GridRow);
            }
            #endregion
            #region 回傳JSON資料
            JQGridDataObject dataObj = new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount
            };

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(dataObj);
            #endregion
        }

        [HttpPost]
        public override String ajax_MasterDeleteData(string ids)
        {
            throw new NotImplementedException();
        }
        public String ajax_MasterUpdata(m_Unit md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;
            string returnString = string.Empty;

            a_Unit ac = new a_Unit();
            ac.Connection = getSQLConnection();

            if (!ModelState.IsValid)
            {//return RedirectToAction("Write", ModelObj);
            }

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }

            returnString = js.Serialize(rAjaxResult);
            return returnString;
        }

        [HttpPost]
        public String ajax_MasterDeleteData()
        {
            String[] deleteID = Request.Form["id"].Split(',');
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            string returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a_Unit();
            ac.Connection = getSQLConnection();

            RunDeleteEnd HResult = ac.DeleteMaster(deleteID, LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return js.Serialize(rAjaxResult); ;
        }
        #endregion
    }
}
