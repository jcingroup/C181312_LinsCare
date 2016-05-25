using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

using ProcCore.NetExtension;
using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    /// AjaxDownLoadFile 的摘要描述
    /// </summary>
    /// 
    public class AjaxDownLoadFile : BaseIHttpHandler //已停止不用 改由 FileResult做下載
    {
        protected String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/OriginFile/{4}";

        public override void ProcessRequest(HttpContext context)
        {
            vmJsonResult r_json_data = null;
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;

            String Area = context.Request.QueryString["area"];
            String Controller = context.Request.QueryString["controller"];
            int Id = context.Request.QueryString["id"].CInt();
            String FileKind = context.Request.QueryString["FileKind"];
            String FileName = context.Request.QueryString["FileName"];

            if (FileKind == null || FileKind == "") {
                FileKind = "DocFiles";
            }
            String FileWebPath = String.Format(SystemUpFilePathTpl, Area, Controller, Id, FileKind, FileName);
            String FilePath = context.Request.MapPath(FileWebPath);
            


            if (File.Exists(FilePath))
            {
                FileInfo fi = new FileInfo(FilePath);
                try
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/" + fi.Extension.Replace(".","");
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" +  FilePath.GetFileName());
                    context.Response.WriteFile(FilePath);
                    context.Response.End();
                }
                catch (Exception ex)
                {
                    r_json_data = new vmJsonResult() { result = false, message = ex.Message + ":" + ex.StackTrace };
                    context.Response.Write(js.Serialize(r_json_data));
                } 
            }
        }

    }
}