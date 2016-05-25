using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using ProcCore;
using ProcCore.NetExtension;
using ProcCore.WebCore;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class ProgController : BaseAction<m_ProgData, a_ProgData, q_ProgData, ProgData>
    {
        #region action section
        public ProgController()
        {
            //this.Tab = new ProgData();
        }
        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }
        public override ActionResult ListGrid(q_ProgData sh)
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
            md = new m_ProgData();

            //設定預設值

            ac = new a_ProgData();
            ac.Connection = getSQLConnection();

            md.EditType = EditModeType.Insert;
            md.PowerItem = new PowerManagement(0);
            md.id = ac.GetIDX();

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
            ac = new a_ProgData();
            ac.Connection = getSQLConnection();

            RunOneDataEnd<m_ProgData> HResult = ac.GetDataMaster(id, LoginUserId);
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
        } 
        #endregion

        #region Ajax Call Section

        [HttpGet]
        public override String ajax_MasterGridData(q_ProgData queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_ProgData();
            ac.Connection = this.getSQLConnection();
            RunQueryPackage<m_ProgData> HResult = ac.SearchMasterLVL1(LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            //int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            //int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 組成資料
            List<RowElement> setRowElement = new List<RowElement>();
            m_ProgData[] drDataCollect = HResult.SearchData;

            foreach (m_ProgData dr in drDataCollect)
            {
                List<String> setFields = new List<String>();

                setFields.Add(dr.id.ToString());
                setFields.Add(dr.prog_name);
                setFields.Add(dr.area);
                setFields.Add(dr.controller);
                setFields.Add(dr.sort);
                setFields.Add(dr.isfolder.BooleanValue(BooleanSheet.ynvx));
                setRowElement.Add(new RowElement() { id = dr.id.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料
            JQGridDataObject dataObj = new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = 1,
                page = 1,
                records = PageCount.RecordCount
            };

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(dataObj);
            #endregion
        }

        [HttpGet]
        public String ajax_MasterSubGridData(n_ProgData queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_ProgData();
            ac.Connection = this.getSQLConnection();
            RunQueryPackage<m_ProgData> HResult = ac.SearchMasterLVL2(queryObj,LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            //int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            //int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 組成資料
            List<RowElement> setRowElement = new List<RowElement>();
            m_ProgData[] drDataCollect = HResult.SearchData;

            foreach (m_ProgData dr in drDataCollect)
            {
                List<String> setFields = new List<String>();

                setFields.Add(dr.id.ToString());
                setFields.Add(dr.prog_name);
                setFields.Add(dr.area);
                setFields.Add(dr.controller);
                setFields.Add(dr.sort);
                setRowElement.Add(new RowElement() { id = dr.id.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料
            JQGridDataObject dataObj = new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = 1,
                page = 1,
                records = PageCount.RecordCount
            };

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(dataObj);
            #endregion
        }

        [HttpPost]
        public String ajax_MasterUpdata(m_ProgData md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;
            string returnString = string.Empty;

            ac = new a_ProgData();
            ac.Connection = getSQLConnection();

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunUpdateEnd HResult = ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }

            returnString = js.Serialize(rAjaxResult);
            return returnString;
        }

        [HttpPost]
        public override String ajax_MasterDeleteData(String id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            String returnString = string.Empty;
            js.MaxJsonLength = 4096;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a_ProgData();
            ac.Connection = getSQLConnection();

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(),LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            returnString = js.Serialize(rAjaxResult);
            return returnString;
        }
        #endregion
    }
}
