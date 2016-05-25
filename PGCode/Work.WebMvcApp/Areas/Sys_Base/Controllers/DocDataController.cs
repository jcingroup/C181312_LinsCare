using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProcCore;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using ProcCore.JqueryHelp.JQGridScript;
using DotWeb.CommSetup;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class DocDataController : BaseAction<m_DocData, a_DocData, q_DocData, DocData>
    {
        #region action and function section
        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public override ActionResult ListGrid(q_DocData sh)
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
            md = new m_DocData();

            //設定預設值
            //如有在模組做Log記錄，請加logPlamInfo = plamInfo
            ac = new a_DocData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            md.id = ac.GetIDX();
            md.EditType = EditModeType.Insert;

            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("Id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;

            //ViewData["ImageUpScope"] = ImageFileUpParm.DocBasic;

            return View("EditData", md);
        }
        public override ActionResult EditMasterDataByID(int id)
        {
            operationMode = OperationMode.EditModify;
            ac = new a_DocData() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunOneDataEnd<m_DocData> HResult = ac.GetDataMaster(id, LoginUserId);
            md = HResult.SearchData;
            md.EditType = EditModeType.Modify;
            HandleResultCheck(HResult);
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;
            //ViewData["ImageUpScope"] = ImageFileUpParm.DocBasic;
            return View("EditData", md);
        }

        /// <summary>
        /// 介面製作，例如Option Radio CheckBox 多項目
        /// </summary>
        protected override void HandleCollectDataToOptions()
        {
            ViewBag.DocKind_Option = MakeCollectDataToOptions(CodeSheet.消息分類.ToDictionary(), false);
        }
        #endregion

        #region ajax call section
        /// <summary>
        /// 新增修改提供給Ajax Form Call
        /// </summary>
        /// <returns>JSON format for jquery</returns>
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_MasterUpdata(m_DocData md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            ac = new a_DocData() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = this.ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = this.ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// Grid Ajax 刪除資料 Section
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public override String ajax_MasterDeleteData(String id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            string returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a_DocData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// 由ajax從這裡下載資料 並搭配queryObj作為Search條件搜尋
        /// </summary>
        /// <param name="queryObj"></param>
        /// <returns></returns>
        [HttpGet]
        public override String ajax_MasterGridData(q_DocData queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_DocData() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_DocData> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            foreach (m_DocData md in Modules)
            {
                List<String> setFields = new List<String>(5);

                setFields.Add(md.id.ToString());
                setFields.Add(md.Title);
                setRowElement.Add(new RowElement() { id = md.id.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount
            });
            #endregion
        }
        #endregion

        #region ajax file upload handle
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_UploadFine(int Id, String FilesKind, String hd_FileUp_EL)
        {
            //hd_FileUp_EL
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            #region
            String tpl_File = String.Empty;
            try
            {
                //判斷是否為圖片檔
                if (!IMGExtDef.Any(x => x == hd_FileUp_EL.GetFileExt()))
                {
                    HandFineSave(hd_FileUp_EL, Id, new FilesUpScope(), FilesKind, false);
                    rAjaxResult.result = true;
                    rAjaxResult.success = true;
                    rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                }
                else
                {
                    HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.NewsBasic, FilesKind);
                    rAjaxResult.result = true;
                    rAjaxResult.success = true;
                    rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                }
            }
            catch (LogicError ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = GetRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = ex.Message;
            }
            #endregion
            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_ListFiles(int Id, String FileKind)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.filesObject = ListSysFiles(Id, FileKind);
            rAjaxResult.result = true;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_DeleteFiles(int Id, String FileKind, String FileName)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            DeleteSysFile(Id, FileKind, FileName, ImageFileUpParm.NewsBasic);
            rAjaxResult.result = true;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }
        #endregion

        public String ajax_gddata()
        {
            return "{\"records\":\"15\",\"page\":1,\"total\":1,\"rows\":[{\"OrderID\":\"10248\",\"OrderDate\":\"1996-07-04 00:00:00\",\"ShipName\":\"Vins et alcools Chevalier\"},{\"OrderID\":\"10249\",\"OrderDate\":\"1996-07-05 00:00:00\",\"ShipName\":\"Toms Spezialit\u00e4ten\"},{\"OrderID\":\"10250\",\"OrderDate\":\"1996-07-08 00:00:00\",\"ShipName\":\"Hanari Carnes\"},{\"OrderID\":\"10251\",\"OrderDate\":\"1996-07-08 00:00:00\",\"ShipName\":\"Victuailles en stock\"},{\"OrderID\":\"10252\",\"OrderDate\":\"1970-01-01 00:00:00\",\"ShipName\":\"Supr\u00eames d\u00e9lices\"},{\"OrderID\":\"10253\",\"OrderDate\":\"1996-07-10 00:00:00\",\"ShipName\":\"Hanari Carnes\"},{\"OrderID\":\"10254\",\"OrderDate\":\"1996-07-11 00:00:00\",\"ShipName\":\"Chop-suey Chinese\"},{\"OrderID\":\"10255\",\"OrderDate\":\"1996-07-12 00:00:00\",\"ShipName\":\"Richter Supermarkt\"},{\"OrderID\":\"10256\",\"OrderDate\":\"1996-07-15 00:00:00\",\"ShipName\":\"Wellington Importadora\"},{\"OrderID\":\"10257\",\"OrderDate\":\"1996-07-16 00:00:00\",\"ShipName\":\"HILARI\u00d3N-Abastos\"},{\"OrderID\":\"10258\",\"OrderDate\":\"1996-07-17 00:00:00\",\"ShipName\":\"Ernst Handel\"},{\"OrderID\":\"10259\",\"OrderDate\":\"1996-07-18 00:00:00\",\"ShipName\":\"Centro comercial Moctezuma\"},{\"OrderID\":\"10260\",\"OrderDate\":\"1996-07-19 00:00:00\",\"ShipName\":\"Ottilies K\u00e4seladen\"},{\"OrderID\":\"10261\",\"OrderDate\":\"1996-07-19 00:00:00\",\"ShipName\":\"Que Del\u00edcia\"},{\"OrderID\":\"10262\",\"OrderDate\":\"1996-07-22 00:00:00\",\"ShipName\":\"Rattlesnake Canyon Grocery\"}]}";
        }

        public String ajax_acdata()
        {
            //return "[\"Maine\",\"Maryland\",\"Massachusetts\"]";
            return "[{\"value\":\"22\",\"label\":\"Maine\"}]";
        }
    }
}
