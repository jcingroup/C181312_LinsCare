using System;

namespace ProcCore
{
    public enum EditModeType
    {
        Insert,
        Modify
    }
}
namespace ProcCore.WebCore
{
    public class FilesUpScope
    {
        public FilesUpScope()
        {
            this.LimitExtType = new String[] { ".asp", ".aspx", ".exe", ".php", ".bat" };
            this.AllowExtType = new String[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".doc", ".xls", "ppt", ".docx", ".xlsx", "pptx",".pdf",".txt",".rar",".zip" };
        }

        public int LimitSize { get; set; }
        /// <summary>
        /// 以副檔名為設定，此為禁止上傳類型，要加. 例.exe。
        /// </summary>
        public String[] LimitExtType { get; set; }
        public String[] AllowExtType { get; set; }
        public int LimitCount { get; set; }
    }
    public class ImageUpScope : FilesUpScope
    {
        public String KindName { get; set; }
        public Boolean KeepOriginImage { get; set; }
        public ImageSizeParm[] Parm { get; set; }
    }
    public class ImageSizeParm
    {
        public int SizeFolder { get; set; }
        public int heigh { get; set; }
        public int width { get; set; }

    }

    /// <summary>
    /// 提供JqGrid頁數分頁所需資訊
    /// </summary>
    public static class PageCount
    {
        public static int PageInfo(int page, int pagesize, int recordCount)
        {
            RecordCount = recordCount;
            Page = page;
            Decimal c = Convert.ToDecimal(RecordCount) / pagesize;
            TotalPage = (RecordCount > 0 && pagesize > 0 && pagesize < RecordCount) ? Convert.ToInt32(Math.Ceiling(c)) : 1;
            Page = (Page > TotalPage) ? TotalPage : Page;
            return (Page - 1) * pagesize;
        }

        public static int TotalPage { get; set; }
        public static int RecordCount { get; set; }
        public static int Page { get; set; }
    }
    public class NavPageMap
    {
        public String LinkUrl { get; set; }
        public String Title { get; set; }
        public String Context { get; set; }
    }
    public class UserAgentUtility
    {
        private static string[] mobiles = new[]
    {
        "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource",
        "240x320", "opwv", "chtml","pda", "windows ce", "mmp/",
        "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi",
        "phone", "cdm", "up.b", "audio", "sie-", "sec-", "samsung", "htc",
        "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
        "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch",
        "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi",
        "bird", "compal", "kg", "voda","sany", "kdd", "dbt", "sendo",
        "sgh", "gradi", "jb", "dddi", "moto", "iphone", "android",
        "iPod", "incognito", "webmate", "dream", "cupcake", "webos",
        "s8000", "bada", "googlebot-mobile"
    };

        /// <summary>
        /// 判斷是否為行動版瀏覽器
        /// </summary>
        /// <param name="UserAnget"></param>
        /// <returns></returns>
        public static bool isMobile(string UserAnget)
        {
            if (string.IsNullOrEmpty(UserAnget))
                return false;

            foreach (var item in mobiles)
            {
                if (UserAnget.ToLower().IndexOf(item) != -1)
                    return true;
            }
            return false;
        }
    }
}
namespace ProcCore.ReturnAjaxResult
{
    /// <summary>
    /// 回傳Ajax Result JSON格式
    /// </summary>
    public class ReturnAjaxInfo
    {
        public ReturnAjaxInfo()
        {
            result = true;
            message = "";
            sessionout = false;
        }
        public Boolean result { get; set; }
        public String message { get; set; }
        public ReturnErrType errtype { get; set; }
        public String title { get; set; }
        public Int32 id { get; set; }
        public Boolean sessionout { get; set; }

    }
    /// <summary>
    /// 回傳Ajax Result JSON格式含Module Object
    /// </summary>
    public class ReturnAjaxData : ReturnAjaxInfo
    {
        public Object Module { get; set; }
        public Object data { get; set; }
    }
    /// <summary>
    /// 回傳Ajax Result JSON格式含Files資訊
    /// </summary>
    public class ReturnAjaxFiles : ReturnAjaxInfo
    {
        public FilesObject[] filesObject { get; set; }
        public String FileName { get; set; }
        /// <summary>
        /// 搭配Fine Uploader
        /// </summary>
        public Boolean success { get; set; }
        /// <summary>
        /// 搭配Fine Uploader
        /// </summary>
        public String error { get; set; }
    }
    public class FilesObject
    {
        public String RepresentFilePath { get; set; }
        public String OriginFilePath { get; set; }
        public String FileName { get; set; }
        public String FilesKind { get; set; }
        public Boolean IsImage { get; set; }
        public long Size { get; set; }
    }
    public enum ReturnErrType
    {
        Logic,
        System
    }

}