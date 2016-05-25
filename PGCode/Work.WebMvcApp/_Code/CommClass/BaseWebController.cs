using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

using ProcCore;
using ProcCore.Business.Base;
using ProcCore.Business.Logic;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.DatabaseCore.DatabaseName;
using ProcCore.NetExtension;
using ProcCore.ReturnAjaxResult;
using ProcCore.WebCore;

namespace DotWeb
{
    #region 基底控制器
    public abstract class SourceController : System.Web.Mvc.Controller
    {
        protected virtual String GetRecMessage(String MsgId)
        {
            return Resources.Res.ResourceManager.GetString(MsgId);
        }
        protected CommConnection getSQLConnection()
        {
            BaseConnection BConn = new BaseConnection();
            String DataConnectionCode = System.Configuration.ConfigurationManager.AppSettings["DB00"];
            String[] DataConnectionInfo = DataConnectionCode.Split(',');
            BConn.Server = String.IsNullOrWhiteSpace(DataConnectionInfo[0]) ? Server.MapPath("~") : DataConnectionInfo[0];
            BConn.Account = DataConnectionInfo[1];
            BConn.Password = DataConnectionInfo[2];

            if ( !(BConn.Server.EndsWith("/") || BConn.Server.EndsWith("\\")) )
                BConn.Server += "/";

            return BConn.GetConnection();
        }
        protected CommConnection getSQLConnection(DataBases DBName)
        {
            //預設的資料庫
            String DataBasesCodeName = DBName.ToString();
            String DataConnectionCode = System.Configuration.ConfigurationManager.AppSettings[DataBasesCodeName.Substring(0, 4)];
            String[] DataConnectionInfo = DataConnectionCode.Split(',');
            //直接採用預設的資料庫
            BaseConnection BConn = new BaseConnection();
            //SQL Server & MySql 採用
            BConn.Server = String.IsNullOrWhiteSpace(DataConnectionInfo[0]) ? Server.MapPath("~") : DataConnectionInfo[0];
            BConn.Account = DataConnectionInfo[1];
            BConn.Password = DataConnectionInfo[2];

            if (!(BConn.Server.EndsWith("/") || BConn.Server.EndsWith("\\")))
                BConn.Server += "/";

            return BConn.GetConnection(DBName);
        }
        protected CommConnection getSQLConnection(DataBases DBName, String ServerIP, String Account, String Password)
        {
            try
            {
                BaseConnection BConn = new BaseConnection();
                if (ServerIP != null)
                {
                    BConn.Server = ServerIP;
                    BConn.Account = Account;
                    BConn.Password = Password;
                }
                return BConn.GetConnection(DBName);
            }
            catch (Exception ex)
            {
                Log.Write(new Log.LogPlamInfo() { }, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return null;
            }
        }
    }

    public abstract class SourceFiles : SourceController
    {
        protected Int32 VisitCount = 0;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            a__WebVisitData ac_WebVisit = new a__WebVisitData() { Connection = getSQLConnection() };
            m__WebVisitData md_WebVisit = new m__WebVisitData();

            md_WebVisit.id = ac_WebVisit.GetIDX();
            md_WebVisit.ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            md_WebVisit.browser = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            md_WebVisit.setdate = DateTime.Now;
            if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                md_WebVisit.source = System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

            md_WebVisit.page = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            ac_WebVisit.InsertMaster(md_WebVisit, 0);

            a__WebCount ac_WebCount = new a__WebCount() { Connection = getSQLConnection() };
            if (System.Web.HttpContext.Current.Session["Visited"] == null)
            {
                System.Web.HttpContext.Current.Session["Visited"] = true;
                ac_WebCount.UpdateMaster(0);
            }
            VisitCount = ac_WebCount.SearchMaster(0).SearchData.Cnt;
            ViewBag.VisitCount = VisitCount;
        }

        public SourceFiles()
        {
            //var c = System.Diagnostics.Process.Start("dir C:\\ >> C.txt");
        }

        public FileResult DownLoadFile(Int32 Id, String GetArea, String GetController, String FileName, String FilesKind)
        {
            if (FilesKind == null)
                FilesKind = "DocFiles";

            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl + "\\" + FileName, GetArea, GetController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
            {
                fi = new FileInfo(DownFilePath);
            }
            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }

        public String GetSYSImage(Int32 Id, String GetArea, String GetController)
        {
            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, "DefaultKind", "OriginFile");
            String GetFolderPath = Server.MapPath(SearchPath);

            if (System.IO.Directory.Exists(GetFolderPath))
            {
                String fs = Directory.GetFiles(GetFolderPath).FirstOrDefault();
                FileInfo f = new FileInfo(fs);
                return SearchPath + "/" + f.Name;
            }
            else
            {
                return null;
            }
        }
    }

    [CommAttibute]
    public abstract class BaseController : SourceController
    {
        protected String AppPath;

        protected int LoginUserId;
        protected int LoginUnitId;

        protected int DefPageSize = 0;
        protected int DefGridHight = 0;
        protected int DefGridWidth = 0;

        protected String GetController = string.Empty;
        protected String GetArea = string.Empty;
        protected String GetAction = string.Empty;

        protected PowerHave powerHave;
        protected OperationMode operationMode;

        protected String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
        protected String SystemDelSysId = "~/_Code/SysUpFiles/{0}.{1}/{2}";
        protected String[] IMGExtDef = new String[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        protected Log.LogPlamInfo plamInfo = new Log.LogPlamInfo() { AllowWrite = true };
        protected String VarCookie = "PiKen";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Id"] == null)
            {
                filterContext.Result = RedirectToRoute("Manager");
            }
            base.OnActionExecuting(filterContext);
        }

        public BaseController()
            : base()
        {
            AppPath = System.Web.HttpContext.Current.Request.ApplicationPath == "/" ? System.Web.HttpContext.Current.Request.ApplicationPath : System.Web.HttpContext.Current.Request.ApplicationPath + "/";

            DefPageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
            DefGridHight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridHeight"]);
            DefGridWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["GridWidth"]);

            ViewBag.css_Edit_Master_CaptionCss = System.Configuration.ConfigurationManager.AppSettings["Edit_Master_CaptionCss"];
            ViewBag.css_Edit_Subtitle_CaptionCss = System.Configuration.ConfigurationManager.AppSettings["Edit_Subtitle_CaptionCss"];
            ViewBag.css_EditFormFieldsNameCss = System.Configuration.ConfigurationManager.AppSettings["EditFormFieldsNameCss"];
            ViewBag.css_EditFormFieldsDataCss = System.Configuration.ConfigurationManager.AppSettings["EditFormFieldsDataCss"];

            ViewBag.css_EditFormNoteCss = System.Configuration.ConfigurationManager.AppSettings["EditFormNoteCss"];
            ViewBag.css_EditFormNavigationFunctionCss = System.Configuration.ConfigurationManager.AppSettings["EditFormNavigationFunctionCss"];

            ViewBag.DialogTitle = "";
            ViewBag.DialogMessage = "";

            ViewData["WebAppPath"] = AppPath;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath("~\\_Code\\Log\\");
            Log.Enabled = true;

            plamInfo.BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            plamInfo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            Log.Write(plamInfo, System.Web.HttpContext.Current.Request.UserLanguages.JoinArray(","), System.Web.HttpContext.Current.Request.UserAgent, "");

            if (Session["Id"] != null)
            {
                this.LoginUserId = (int)Session["Id"];
                this.LoginUnitId = (int)Session["UnitId"];

                plamInfo.UserId = LoginUserId;
                plamInfo.UnitId = LoginUnitId;

                this.GetController = ControllerContext.RouteData.Values["controller"].ToString();
                this.GetArea = ControllerContext.RouteData.DataTokens["area"].ToString();
                this.GetAction = ControllerContext.RouteData.Values["action"].ToString();

                ViewBag.area = this.GetArea;
                ViewBag.controller = this.GetController;
            }
            else
                Response.Redirect("~/");
        }

        public int GetNewId()
        {
            LogicBase LB = new LogicBase();
            LB.Connection = getSQLConnection();
            return LB.GetIDX();
        }

        protected m_ProgData GetSystemInfo()
        {
            a_WebInfo ac = new a_WebInfo();
            m_ProgData md = new m_ProgData();
            ac.Connection = getSQLConnection();

            md = ac.GetSystemInfo(this.GetArea, this.GetController, "");

            //設定權限
            //先取得User Info 代入單位
            m_Users md_User;
            a_Users ac_User = new a_Users();
            ac_User.Connection = ac.Connection;
            md_User = ac_User.GetDataMaster(LoginUserId, LoginUserId).SearchData;

            powerHave = new PowerHave();
            powerHave.Connection = ac.Connection;
            powerHave.SetPower(LoginUserId, md.id);

            return md;
        }

        protected String HandleSysErr(Exception ex)
        {
            return ex.Message + ":" + ex.StackTrace;
        }

        protected void HandleResultCheck(RunEnd h)
        {
            if (!h.Result)
            {
                if (h.ErrType == BusinessErrType.Logic)
                {
                    ViewData["DialogTitle"] = "系統提醒";
                }

                if (h.ErrType == BusinessErrType.System)
                {
                    ViewData["DialogTitle"] = "系統發生錯誤";
                }
                ViewData["DialogMessage"] = h.Message.ScriptString();
            }
        }

        protected ReturnAjaxFiles HandleResultAjaxFiles(RunEnd h, String ReturnTrueMessage)
        {
            ReturnAjaxFiles r = new ReturnAjaxFiles();

            if (!h.Result)
            {
                if (h.ErrType == BusinessErrType.Logic)
                {
                    r.result = false;
                    r.message = GetRecMessage(h.Message); ;
                    r.title = Resources.Res.Log_Err_Title;
                    r.errtype = ReturnErrType.Logic;
                }

                if (h.ErrType == BusinessErrType.System)
                {
                    r.result = false;
                    r.message = h.Message;
                    r.title = Resources.Res.Sys_Err_Title;
                    r.errtype = ReturnErrType.System;
                }
            }
            else
            {
                r.result = true;
                r.title = Resources.Res.Info_WorkResult;
                r.message = ReturnTrueMessage;
            }
            return r;
        }
        protected ReturnAjaxData HandleResultAjaxData<m>(RunOneDataEnd<m> h, String ReturnTrueMessage) where m : ModuleBase
        {
            ReturnAjaxData r = new ReturnAjaxData();

            if (!h.Result)
            {
                if (h.ErrType == BusinessErrType.Logic)
                {
                    r.result = false;
                    r.message = GetRecMessage(h.Message); ;
                    r.title = Resources.Res.Log_Err_Title;
                    r.errtype = ReturnErrType.Logic;
                }

                if (h.ErrType == BusinessErrType.System)
                {
                    r.result = false;
                    r.message = h.Message;
                    r.title = Resources.Res.Sys_Err_Title;
                    r.errtype = ReturnErrType.System;
                }
            }
            else
            {
                r.result = true;
                r.data = h.SearchData;
                if (h.Message == "")
                    r.message = ReturnTrueMessage;

                else
                {
                    r.title = Resources.Res.Info_SystemAlert;
                    r.message = GetRecMessage(h.Message); ;
                }
            }
            return r;
        }
        protected List<SelectListItem> MakeCollectDataToOptions(Dictionary<String, String> OptionData, Boolean FirstIsBlank)
        {

            List<SelectListItem> r = new List<SelectListItem>();
            if (FirstIsBlank)
            {
                SelectListItem sItem = new SelectListItem();
                sItem.Value = "";
                sItem.Text = "";
                r.Add(sItem);
            }

            foreach (var a in OptionData)
            {
                SelectListItem s = new SelectListItem();
                s.Value = a.Key;
                s.Text = a.Value;
                r.Add(s);
            }
            return r;
        }
        /// <summary>
        /// 查詢 jqGrid的page參數
        /// </summary>
        /// <returns></returns>
        protected String QueryGridPage()
        {
            return Request.QueryString["page"] == null ? "1" : Request.QueryString["page"];
        }

        protected void HandFineSave(String FileName, int Id, FilesUpScope fp, String FilesKind, Boolean pdfConvertImage)
        {
            Stream upFileStream = Request.InputStream;
            BinaryReader BinRead = new BinaryReader(upFileStream);
            String FileExt = System.IO.Path.GetExtension(FileName);

            #region IE file stream handle


            String[] IEOlderVer = new string[] { "6.0", "7.0", "8.0", "9.0" };
            System.Web.HttpPostedFile GetPostFile = null;
            if (Request.Browser.Browser == "IE" && IEOlderVer.Any(x => x == Request.Browser.Version))
            {
                System.Web.HttpFileCollection collectFiles = System.Web.HttpContext.Current.Request.Files;
                GetPostFile = collectFiles[0];
                if (!GetPostFile.FileName.Equals(""))
                {
                    //GetFileName = System.IO.Path.GetFileName(GetPostFile.FileName);
                    BinRead = new BinaryReader(GetPostFile.InputStream);
                }
            }

            Byte[] fileContents = { };
            const int bufferSize = 1024 * 1024; //set 1MB buffer

            while (BinRead.BaseStream.Position < BinRead.BaseStream.Length - 1)
            {
                Byte[] buffer = new Byte[bufferSize];
                int ReadLen = BinRead.Read(buffer, 0, buffer.Length);
                Byte[] dummy = fileContents.Concat(buffer).ToArray();
                fileContents = dummy;
                dummy = null;
            }
            #endregion

            String tpl_Org_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, FilesKind, "OriginFile");
            String Org_Path = Server.MapPath(tpl_Org_FolderPath);

            #region 檔案上傳前檢查
            if (fp.LimitSize > 0)
                //if (GetPostFile.InputStream.Length > fp.LimitSize)
                if (BinRead.BaseStream.Length > fp.LimitSize)
                    throw new LogicError("Log_Err_FileSizeOver");

            if (fp.LimitCount > 0 && Directory.Exists(Org_Path))
            {
                String[] Files = Directory.GetFiles(Org_Path);
                if (Files.Count() >= fp.LimitCount) //還沒存檔，因此Selet到等於的數量，再加上現在要存的檔案即算超過
                    throw new LogicError("Log_Err_FileCountOver");
            }

            if (fp.AllowExtType != null)
                if (!fp.AllowExtType.Contains(FileExt.ToLower()))
                    throw new LogicError("Log_Err_AllowFileType");

            if (fp.LimitExtType != null)
                if (fp.LimitExtType.Contains(FileExt))
                    throw new LogicError("Log_Err_LimitedFileType");
            #endregion

            #region 存檔區

            if (!System.IO.Directory.Exists(Org_Path)) { System.IO.Directory.CreateDirectory(Org_Path); }

            //LogWrite.Write("Save File Start"+ Org_Path + "\\" + FileName);

            FileStream writeStream = new FileStream(Org_Path + "\\" + FileName, FileMode.Create);
            BinaryWriter BinWrite = new BinaryWriter(writeStream);
            BinWrite.Write(fileContents);
            //GetPostFile.SaveAs(Org_Path + "\\" + FileName);

            upFileStream.Close();
            upFileStream.Dispose();
            writeStream.Close();
            BinWrite.Close();
            writeStream.Dispose();
            BinWrite.Dispose();

            //LogWrite.Write("Save File End"+ Org_Path + "\\" + FileName);
            #endregion

            #region PDF轉圖檔
            if (pdfConvertImage)
            {
                FileInfo fi = new FileInfo(Org_Path + "\\" + FileName);
                if (fi.Extension == ".pdf")
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.FileName = @"C:\Program Files\Boxoft PDF to JPG (freeware)\pdftojpg.exe";
                    proc.StartInfo.Arguments = Org_Path + "\\" + FileName + " " + Org_Path;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();
                    proc.WaitForExit();
                    proc.Close();
                    proc.Dispose();
                }
            }
            #endregion
        }
        protected void HandImageSave(String FileName, int Id, ImageUpScope fp, String FilesKind)
        {
            Stream upFileStream = Request.InputStream;
            BinaryReader BinRead = new BinaryReader(upFileStream);
            String FileExt = System.IO.Path.GetExtension(FileName);

            #region IE file stream handle

            String[] IEOlderVer = new string[] { "6.0", "7.0", "8.0", "9.0" };
            System.Web.HttpPostedFile GetPostFile = null;
            if (Request.Browser.Browser == "IE" && IEOlderVer.Any(x => x == Request.Browser.Version))
            {
                System.Web.HttpFileCollection collectFiles = System.Web.HttpContext.Current.Request.Files;
                GetPostFile = collectFiles[0];
                if (!GetPostFile.FileName.Equals(""))
                {
                    //GetFileName = System.IO.Path.GetFileName(GetPostFile.FileName);
                    BinRead = new BinaryReader(GetPostFile.InputStream);
                }
            }

            Byte[] fileContents = { };
            const int bufferSize = 1024 * 1024; //set 1MB buffer

            while (BinRead.BaseStream.Position < BinRead.BaseStream.Length - 1)
            {
                Byte[] buffer = new Byte[bufferSize];
                int ReadLen = BinRead.Read(buffer, 0, buffer.Length);
                Byte[] dummy = fileContents.Concat(buffer).ToArray();
                fileContents = dummy;
                dummy = null;
            }
            #endregion

            String tpl_Org_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, FilesKind, "OriginFile");
            String Org_Path = Server.MapPath(tpl_Org_FolderPath);

            #region 檔案上傳前檢查
            if (fp.LimitSize > 0)
                //if (GetPostFile.InputStream.Length > fp.LimitSize)
                if (BinRead.BaseStream.Length > fp.LimitSize)
                    throw new LogicError("Log_Err_FileSizeOver");

            if (fp.LimitCount > 0 && Directory.Exists(Org_Path))
            {
                String[] Files = Directory.GetFiles(Org_Path);
                if (Files.Count() >= fp.LimitCount) //還沒存檔，因此Selet到等於的數量，再加上現在要存的檔案即算超過
                    throw new LogicError("Log_Err_FileCountOver");
            }

            if (fp.AllowExtType != null)
                if (!fp.AllowExtType.Contains(FileExt.ToLower()))
                    throw new LogicError("Log_Err_AllowFileType");

            if (fp.LimitExtType != null)
                if (fp.LimitExtType.Contains(FileExt))
                    throw new LogicError("Log_Err_LimitedFileType");
            #endregion

            #region 存檔區

            if (fp.KeepOriginImage)
            {
                //原始檔
                //tpl_Org_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, FilesKind, "OriginFile");
                Org_Path = Server.MapPath(tpl_Org_FolderPath);
                if (!System.IO.Directory.Exists(Org_Path)) { System.IO.Directory.CreateDirectory(Org_Path); }

                FileStream writeStream = new FileStream(Org_Path + "\\" + FileName, FileMode.Create);
                BinaryWriter BinWrite = new BinaryWriter(writeStream);
                BinWrite.Write(fileContents);

                upFileStream.Close();
                upFileStream.Dispose();
                writeStream.Close();
                BinWrite.Close();
                writeStream.Dispose();
                BinWrite.Dispose();
                //FileName.SaveAs(Org_Path + "\\" + FileName.FileName.GetFileName());
            }

            //後台管理的代表小圖
            String tpl_Rep_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, FilesKind, "RepresentICON");
            String Rep_Path = Server.MapPath(tpl_Rep_FolderPath);
            if (!System.IO.Directory.Exists(Rep_Path)) { System.IO.Directory.CreateDirectory(Rep_Path); }
            MemoryStream smr = UpFileReSizeImage(fileContents, 0, 90);
            System.IO.File.WriteAllBytes(Rep_Path + "\\" + FileName.GetFileName(), smr.ToArray());
            smr.Dispose();

            if (fp.Parm.Count() > 0)
            {
                foreach (ImageSizeParm imSize in fp.Parm)
                {
                    tpl_Rep_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, FilesKind, "s_" + imSize.SizeFolder);
                    Rep_Path = Server.MapPath(tpl_Rep_FolderPath);
                    if (!System.IO.Directory.Exists(Rep_Path)) { System.IO.Directory.CreateDirectory(Rep_Path); }
                    MemoryStream sm = UpFileReSizeImage(fileContents, imSize.width, imSize.heigh);
                    System.IO.File.WriteAllBytes(Rep_Path + "\\" + FileName.GetFileName(), sm.ToArray());
                    sm.Dispose();
                }
            }
            #endregion
        }

        protected MemoryStream UpFileReSizeImage(Byte[] s, int newWidth, int newHight)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap im = (Bitmap)tc.ConvertFrom(s);

                if (newHight == 0)
                    newHight = (im.Height * newWidth) / im.Width;

                if (newWidth == 0)
                    newWidth = (im.Width * newHight) / im.Height;

                if (im.Width < newWidth)
                    newWidth = im.Width;

                if (im.Height < newHight)
                    newHight = im.Height;

                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                EncoderParameters myEncoderParameter = new EncoderParameters(1);
                myEncoderParameter.Param[0] = qualityParam;

                ImageCodecInfo myImageCodecInfo = GetEncoder(im.RawFormat);

                Bitmap ImgOutput = new Bitmap(im, newWidth, newHight);

                //ImgOutput.Save();
                MemoryStream ss = new MemoryStream();

                ImgOutput.Save(ss, myImageCodecInfo, myEncoderParameter);
                im.Dispose();
                return ss;
            }
            catch (Exception ex)
            {
                Log.Write("Image Handle Error:" + ex.Message);
                return null;
            }
            //ImgOutput.Dispose(); 
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        protected MemoryStream UpFileCropCenterImage(Byte[] s, int width, int heigh)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));

                Bitmap ImgSource = (Bitmap)tc.ConvertFrom(s);
                Bitmap ImgOutput = new Bitmap(width, heigh);

                int x = (ImgSource.Width - width) / 2;
                int y = (ImgSource.Height - heigh) / 2;
                Rectangle cropRect = new Rectangle(x, y, width, heigh);

                using (Graphics g = Graphics.FromImage(ImgOutput))
                {
                    g.DrawImage(ImgSource, new Rectangle() { Height = heigh, Width = width, X = 0, Y = 0 }, cropRect, GraphicsUnit.Pixel);
                }

                MemoryStream ss = new MemoryStream();
                ImgOutput.Save(ss, ImgSource.RawFormat);
                ImgSource.Dispose();
                return ss;
            }
            catch (Exception ex)
            {
                Log.Write("Image Handle Error:" + ex.Message);
                return null;
            }
            //ImgOutput.Dispose(); 
        }

        /// <summary>
        /// 系統檔案列表
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AccessFilesKind">如果此參數為null代表存取的是一般文件圖，而不是圖檔。</param>
        /// <returns></returns>
        protected FilesObject[] ListSysFiles(int Id, String FilesKind)
        {
            return ListSysFiles(Id, FilesKind, true);
        }
        protected FilesObject[] ListSysFiles(int Id, String FilesKind, Boolean IsImageList)
        {
            String tpl_FolderPath = String.Empty;
            String Path = String.Empty;

            String AccessFilesKind = FilesKind == "" ? "DocFiles" : FilesKind;
            tpl_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, AccessFilesKind, "OriginFile");
            Path = Server.MapPath(tpl_FolderPath);
            IsImageList = false;

            if (Directory.Exists(Path))
            {
                String[] CheckFiles = Directory.GetFiles(Path);

                if (CheckFiles.Count() > 0)
                {
                    String FileListTypeCheck = CheckFiles.FirstOrDefault();
                    FileInfo GetFirstFileToCheck = new FileInfo(FileListTypeCheck);

                    if (GetFirstFileToCheck.Extension.ToLower().Contains("jpg") || GetFirstFileToCheck.Extension.ToLower().Contains("jpeg") ||
                    GetFirstFileToCheck.Extension.ToLower().Contains("png") || GetFirstFileToCheck.Extension.ToLower().Contains("gif") ||
                    GetFirstFileToCheck.Extension.ToLower().Contains("bmp"))
                    {
                        tpl_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, AccessFilesKind, "RepresentICON");
                        Path = Server.MapPath(tpl_FolderPath);
                        IsImageList = true;
                    }
                }
            }

            //LogWrite.Write("File List"+ Path);

            List<FilesObject> ls_Files = new List<FilesObject>();

            if (Directory.Exists(Path))
            {
                foreach (String fileString in Directory.GetFiles(Path))
                {
                    FileInfo fi = new FileInfo(fileString);
                    FilesObject fo = new FilesObject() { FileName = fi.Name, FilesKind = FilesKind, RepresentFilePath = Url.Content(tpl_FolderPath + "/" + fi.Name) };

                    if (fi.Extension.ToLower().Contains("jpg") || fi.Extension.ToLower().Contains("jpeg") ||
                        fi.Extension.ToLower().Contains("png") || fi.Extension.ToLower().Contains("gif") ||
                        fi.Extension.ToLower().Contains("bmp"))
                        fo.IsImage = true;

                    if (IsImageList)
                    {
                        String org_tpl_FolderPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, AccessFilesKind, "OriginFile");
                        Path = Server.MapPath(org_tpl_FolderPath);
                        fi = new FileInfo(Path + "/" + fi.Name);
                        fo.OriginFilePath = Url.Content(org_tpl_FolderPath + "/" + fi.Name);
                        fo.Size = fi.Length;
                    }
                    else
                    {
                        fo.OriginFilePath = Url.Content(tpl_FolderPath + "/" + fi.Name);
                        fo.Size = fi.Length;
                    }

                    ls_Files.Add(fo);
                }
            }
            return ls_Files.ToArray();
        }

        protected void DeleteSysFile(int Id, String FilesKind, String FileName, ImageUpScope im)
        {
            String SystemDelSysIdKind = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}";
            String tpl_FolderPath = Server.MapPath(String.Format(SystemDelSysIdKind, GetArea, GetController, Id, FilesKind));
            #region Delete Run
            if (Directory.Exists(tpl_FolderPath))
            {
                var folders = Directory.GetDirectories(tpl_FolderPath);
                foreach (var folder in folders)
                {
                    String herefile = folder + "\\" + FileName;
                    if (System.IO.File.Exists(herefile))
                        System.IO.File.Delete(herefile);
                }
            }
            #endregion
        }

        protected void DeleteIdFiles(int Id)
        {
            String tpl_FolderPath = String.Empty;
            tpl_FolderPath = String.Format(SystemDelSysId, GetArea, GetController, Id);
            String Path = Server.MapPath(tpl_FolderPath);
            if (Directory.Exists(Path))
                Directory.Delete(Path, true);
        }

        public FileResult DownLoadFile(Int32 Id, String FilesKind, String FileName)
        {
            String SearchPath = String.Format(SystemUpFilePathTpl + "\\" + FileName, GetArea, GetController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
                fi = new FileInfo(DownFilePath);

            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }
    }

    public abstract class WebFrontController : SourceController
    {
        protected Int32 VisitCount = 0;
        protected Log.LogPlamInfo plamInfo = new Log.LogPlamInfo() { AllowWrite = true };
        protected readonly String SessionShoppingString = "CarPurchase.shipping";
        protected readonly String SessionMemberLoginString = "CarPurchase.loginMail";
        private readonly String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
        protected String VarCookie = "PiKen";

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            plamInfo.BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            plamInfo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            plamInfo.UserId = 0;
            plamInfo.UnitId = 0;

            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath("~\\_Code\\Log\\");
            Log.Enabled = true;

            try
            {
                a__WebVisitData ac_WebVisit = new a__WebVisitData() { Connection = getSQLConnection() };
                m__WebVisitData md_WebVisit = new m__WebVisitData();
                md_WebVisit.id = ac_WebVisit.GetIDX();
                md_WebVisit.ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                md_WebVisit.browser = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
                md_WebVisit.setdate = DateTime.Now;
                if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
                    md_WebVisit.source = System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri;

                md_WebVisit.page = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                ac_WebVisit.InsertMaster(md_WebVisit, 0);

                a__WebCount ac_WebCount = new a__WebCount() { Connection = getSQLConnection() };
                if (System.Web.HttpContext.Current.Session["Visited"] == null)
                {
                    System.Web.HttpContext.Current.Session["Visited"] = true;
                    ac_WebCount.UpdateMaster(0);
                }
                VisitCount = ac_WebCount.SearchMaster(0).SearchData.Cnt;
                ViewBag.VisitCount = VisitCount;
                ViewBag.IsFirstPage = false; //是否為首頁，請在首頁的Action此值設為True

                ViewData["MemberIsLogin"] = false;
                if (System.Web.HttpContext.Current.Session[SessionMemberLoginString] != null)
                {
                    ViewData["MemberIsLogin"] = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
        public WebFrontController()
        {
            //HttpCookie WebLang = System.Web.HttpContext.Current.Request.Cookies["ClientLang"];

            //if (WebLang == null)
            //    WebLang = new HttpCookie("ClientLang", "en-US");
            //else
            //    WebLang.Value = "en-US";

            //System.Web.HttpContext.Current.Response.Cookies.Add(WebLang);

            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
        }

        public int GetNewId()
        {
            LogicBase LB = new LogicBase();
            LB.Connection = getSQLConnection();
            return LB.GetIDX();
        }

        public FileResult DownLoadFile(Int32 Id, String GetArea, String GetController, String FileName, String FilesKind)
        {
            if (FilesKind == null)
                FilesKind = "DocFiles";

            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl + "\\" + FileName, GetArea, GetController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
            {
                fi = new FileInfo(DownFilePath);
            }
            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }

        public String ImgSrc(String AreaName, String ContorllerName, Int32 Id, String FilesKind, Int32 ImageSizeTRype)
        {
            String ImgSizeString = "s_" + ImageSizeTRype;
            String SearchPath = String.Format(SystemUpFilePathTpl, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return Url.Content(SearchPath) + "/" + f.Name;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public FileResult AudioFile(String FilePath)
        {
            String S = Url.Content(FilePath);
            String DownFilePath = Server.MapPath(S);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
                fi = new FileInfo(DownFilePath);

            return File(DownFilePath, "audio/mp3", Url.Encode(fi.Name));
        }
        public String GetSYSImage(Int32 Id, String GetArea, String GetController)
        {
            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, "DefaultKind", "OriginFile");
            String GetFolderPath = Server.MapPath(SearchPath);

            if (System.IO.Directory.Exists(GetFolderPath))
            {
                String fs = Directory.GetFiles(GetFolderPath).FirstOrDefault();
                FileInfo f = new FileInfo(fs);
                return SearchPath + "/" + f.Name;
            }
            else
            {
                return null;
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            Log.WriteToFile();
        }
        public RedirectResult SetLanguage(String L, String A)
        {
            HttpCookie WebLang = new HttpCookie(VarCookie + ".Lang", L);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            Response.Cookies.Add(WebLang);
            return Redirect(Url.Action(A));
        }

        protected override string GetRecMessage(string MsgId)
        {
            return Resources.ResWeb.ResourceManager.GetString(MsgId);
        }
    }
    #endregion

    #region 泛型控制器擴充
    public abstract class BaseAction<M, A, Q, T> : BaseController
        where M : ModuleBase
        where A : LogicBase<M, Q, T>
        where Q : QueryBase
    {
        protected A ac;
        protected M md;
        protected T Tab;

        protected abstract void HandleCollectDataToOptions();
        public abstract ActionResult ListGrid(Q sh);
        public abstract ActionResult EditMasterNewData();
        public abstract ActionResult EditMasterDataByID(Int32 id);
        public abstract String ajax_MasterDeleteData(String ids);
        public abstract String ajax_MasterGridData(Q sh);
    }

    public abstract class BaseActionSub<M, A, Q, T, Ms, As, Qs, Ts> :
        BaseAction<M, A, Q, T>

        where M : ModuleBase
        where A : LogicBase<M, Q, T>
        where Q : QueryBase

        where Ms : ModuleBase
        where As : LogicBase<Ms, Qs, Ts>
        where Qs : QueryBase
    {
        protected Ms mdd;
        protected As acd;
        protected Ts Tabd;

        public abstract String ajax_DetailUpdata(Ms md);
        public abstract String ajax_SubDataUpdate(Ms md);

        public abstract String ajax_DetailGridData(Qs ssh);
        public abstract String ajax_MasterSubGridData(Qs ssh);

        public abstract String ajax_SubDataDelete(String ids);
    }
    #endregion

    public class CommAttibute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log.WriteToFile();
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }
    }

}