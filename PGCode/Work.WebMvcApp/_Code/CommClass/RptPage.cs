using System;
using System.Web.UI;

namespace DotWeb.Report
{
    /// <summary>
    /// BasePage 的摘要描述
    /// </summary>
    /// 
    #region Excel Handle
    public class RptPage : WebFormBasePage
    {
        public RptPage()
        {
        }

        public void OpenCRReport()
        {
            String sScript = "window.open('../../ReportView/CRView.aspx', 'CRReport', 'height=800, width=1024, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=n o, status=no') ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CRReport", sScript, true);
        }
    }
    #endregion
}