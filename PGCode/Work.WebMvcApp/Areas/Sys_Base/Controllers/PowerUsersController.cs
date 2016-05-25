using System;
using System.Collections.Generic;
using System.Web.Mvc;

using System.Data;
using System.Collections;
using System.Web.Script.Serialization;

using ProcCore.WebCore;
using ProcCore.JqueryHelp.JQGridScript;
using ProcCore.JqueryHelp;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class PowerUsersController : BaseController
    {
        ProgData Tab;
        a_PowerUser ac;

        public PowerUsersController()
        { 
                    Tab = new ProgData();
        }

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public ActionResult ListGrid(q_ProgData sh)
        {
            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.encodeURIComponent = true;
            //HRq.Remove("page");

            ac = new a_PowerUser();
            ac.Connection = getSQLConnection();
            CreatGridInitScript();
            HandleCollectDataToOptions();

            return View("EditData");
        }

        protected void CreatGridInitScript()
        {
            List<String> ls_ColNames = new List<string>() ;
            List<jqGrid.colObject> ls_ColModel = new List<jqGrid.colObject>();

            ls_ColNames.Add("程式名稱");
            ls_ColModel.Add(new jqGrid.colObject() { name = "程式名稱", width = "160" });

            PowerManagement pCollect = new PowerManagement(0);

            foreach (Power p in pCollect.Powers) {
                ls_ColNames.Add(p.name.ToString());
                ls_ColModel.Add(new jqGrid.colObject()
                {
                    name = p.name.ToString(), 
                    align = "center", 
                    resizable = false, 
                    width="60",
                    formatter = new funcMethodModule() { funcName = "$.fn.formatter_MakeCheckBoxID" } });
            }

            ViewData["array_ColNames"] = ls_ColNames.ToArray();
            ViewData["array_ColModel"] = ls_ColModel.ToArray();
        }

        public string getGridData(q_PowerUser sh)
        {
            ProgData TObj = new ProgData();

            a_PowerUser ac = new a_PowerUser();
            ac.Connection = this.getSQLConnection();
            RunQueryEnd HResult = ac.SearchMaster(sh,LoginUserId.ToString());

            HandleResultAjaxFiles(HResult,Resources.Res.NoMessage);

            int page = (sh.page == null ? 1 : sh.page.CInt());//int.Parse(getPage);

            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);

            JQGridDataObject dataObject = new JQGridDataObject();
            List<RowElement> setRowElement = new List<RowElement>();

            IEnumerable<DataRow> dr = HResult.SearchData.AsEnumerable();

            int Cols = HResult.SearchData.Columns.Count;

            foreach (DataRow v in dr)
            {
                RowElement re = new RowElement();

                re.id = v[Tab.id.N].ToString();
                re.cell = new string[Cols];

                for (int i = 1; i < Cols;i++ ) {
                    re.cell[i-1] = v[i].ToString();
                }
                setRowElement.Add(re);
            }

            dataObject.rows = setRowElement.ToArray();
            dataObject.total = PageCount.TotalPage;
            dataObject.page = PageCount.Page;
            dataObject.records = PageCount.RecordCount;

            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 1024 * 10;

            string rS = js.Serialize(dataObject);
            return rS;
        }

        protected void HandleCollectDataToOptions()
        {
            ViewBag.User_Option = MakeCollectDataToOptions(ac.CollectKeyValueData_Users(),true);
        }

        [HttpPost]
        public String ajaxGetUsersPowerList(m_PowerUser md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;
            string returnString = string.Empty;

            a_PowerUser ac = new a_PowerUser();
            ac.Connection = getSQLConnection();

            RunUpdateEnd HResult = ac.UpdateMaster(md, LoginUserId.ToString());
            rAjaxResult = HandleResultAjaxFiles(HResult, "Data_Update_Success");

            returnString = js.Serialize(rAjaxResult);
            return returnString;
        }

    }
}
