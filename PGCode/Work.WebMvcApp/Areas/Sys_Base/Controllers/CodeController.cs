using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProcCore;
using ProcCore.JqueryHelp;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using ProcCore.JqueryHelp.JQGridScript;
using DotWeb.CommSetup;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class CodeController : BaseActionSub<m__CodeHead, a__CodeHead, q__CodeHead, _CodeHead,m__CodeSheet,a__CodeSheet,q__CodeSheet,_CodeSheet>
    {
        #region Action and function section
        public CodeController()
        {
        }

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public override ActionResult ListGrid(q__CodeHead sh)
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
            md = new m__CodeHead();

            //設定預設值

            ac = new a__CodeHead() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;
            md.id = ac.GetIDX();
            md.IsEdit = false;
            md.EditType = EditModeType.Insert;

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
            ac = new a__CodeHead() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunOneDataEnd<m__CodeHead> HResult = ac.GetDataMaster(id, LoginUserId);
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

        /// <summary>
        /// 介面製作，例如Option Radio CheckBox 多項目
        /// </summary>
        protected override void HandleCollectDataToOptions()
        {
            ViewBag.NewsKind_Option = MakeCollectDataToOptions(CodeSheet.消息分類.ToDictionary(), false);
        }
        #endregion

        #region ajax call section
        
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_MasterUpdata(m__CodeHead md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            ac = new a__CodeHead() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            if (md.oper == "add")
            {   //新增
                RunInsertEnd HResult = this.ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {   //修改
                RunEnd HResult = this.ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }
        
        [HttpPost]
        public override String ajax_MasterDeleteData(String id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            string returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a__CodeHead() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return js.Serialize(rAjaxResult);
        }
        
        [HttpGet]
        public override String ajax_MasterGridData(q__CodeHead queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a__CodeHead() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunQueryPackage<m__CodeHead> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            foreach (m__CodeHead md in Modules)
            {
                List<String> setFields = new List<String>();
                setFields.Add(md.id.ToString());
                setFields.Add(md.name);
                setFields.Add(md.Memo);
                setFields.Add(md.IsEdit.BooleanValue(BooleanSheet.ynv));
                setRowElement.Add(new RowElement() { id = md.id.ToString(), cell = setFields.ToArray() });
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

        [HttpGet]
        public override String ajax_MasterSubGridData(q__CodeSheet queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            acd = new a__CodeSheet() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunQueryPackage<m__CodeSheet> HResult = acd.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            foreach (m__CodeSheet md in Modules)
            {
                List<String> setFields = new List<String>();
                setFields.Add(md.id.ToString());
                setFields.Add(md.Code);
                setFields.Add(md.Value);
                setFields.Add(md.Memo);
                setFields.Add(md.Sort.ToString());
                setFields.Add(md.IsUse.BooleanValue(BooleanSheet.ynv));
                setFields.Add(md.CodeHeadId.ToString());
                setRowElement.Add(new RowElement() { id = md.id.ToString(), cell = setFields.ToArray() });
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
        public override String ajax_SubDataDelete(String id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            String returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            acd = new a__CodeSheet() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;
            RunDeleteEnd HResult = acd.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        public override String ajax_SubDataUpdate(m__CodeSheet md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            a__CodeSheet acd = new a__CodeSheet();
            acd.Connection = getSQLConnection();

            if (md.oper == "add")
            {   //新增
                RunInsertEnd HResult = acd.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = acd.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }

        public override String ajax_DetailGridData(q__CodeSheet ssh)
        {
            throw new NotImplementedException();
        }
        public override string ajax_DetailUpdata(m__CodeSheet md)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [ValidateInput(false)]
        public String ajax_GetAddNewID()
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.result = true;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(this.GetNewId());
        }
        #endregion
    }
}