using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

using ProcCore.NetExtension;
using ProcCore.JqueryHelp.SubFormHelp;
using ProcCore.JqueryHelp.AjaxHelp;
using ProcCore.JqueryHelp.DialogHelp;

namespace ProcCore.JqueryHelp.AjaxFilesUpLoadHelp
{
    public class AjaxFineUpLoaderUI : ijQueryUIScript
    {
        private DialogButton _dialog;
        private FineUploader _options;
        private jqElementEvent _submitUpFileButton;

        public AjaxFineUpLoaderUI(
            String Dialog_divId, String FineUploader_divId,
            String OpenDilaogElementId,
            String ajax_url_UpLoadFiles,
            String ajax_url_ListFiles,
            String ajax_url_DeleteFiles)
        {
            this.ajax_url_UpLoadFiles = ajax_url_UpLoadFiles;
            this.ajax_url_ListFiles = ajax_url_ListFiles;
            this.ajax_url_DeleteFiles = ajax_url_DeleteFiles;

            _dialog = new DialogButton(Dialog_divId, OpenDilaogElementId);
            _dialog.options.autoOpen = false;
            _dialog.options.width = 520;
            _dialog.options.height = 480;

            _options = new FineUploader(new jqSelector() { IdName = FineUploader_divId });
            _options.request = new requestOptions() { endpoint = ajax_url_UpLoadFiles };
        }

        public String ajax_url_UpLoadFiles { get; set; }
        public String ajax_url_ListFiles { get; set; }
        public String ajax_url_DeleteFiles { get; set; }
        public jqElementEvent SubmitButton
        {
            set
            {
                _submitUpFileButton = value;
            }
            get
            {
                return _submitUpFileButton;
            }
        }
        public DialogButton Dialog
        {
            get
            {
                return _dialog;
            }
        }
        public FineUploader Options
        {
            get
            {
                return _options;
            }
        }
        public String ToScriptString()
        {
            #region script section
            return _dialog.ToScriptString() + ";" +  _submitUpFileButton.ToScriptString() + ";" + Options.ToScriptString();
            #endregion
        }
        public class FineUploaderBasic : BaseJqueryScriptHelp
        {
            public Boolean? debug { get; set; }
            public String button { get; set; }
            public Boolean? multiple { get; set; }
            public Int32? maxConnections { get; set; }
            public Boolean? disableCancelForFormUploads { get; set; }
            public Boolean? autoUpload { get; set; }
        }
        public class FineUploader : FineUploaderBasic, ijQueryUIScript
        {
            private funcMethodModule _showMessage;

            public FineUploader(jqSelector ElementId)
            {
                this.jqId = ElementId;
                //request = new requestOptions() { inputName = "hd_FileUp_EL", endpoint = "ajax_UploadFiles" };
            }

            public String element { get; set; }
            public String listElement { get; set; }
            public funcMethodModule showMessage
            {
                get
                {
                    return _showMessage;
                }
                set
                {
                    _showMessage = value;
                    if (_showMessage != null)
                        _showMessage.funcParam.AddRange(new String[] { "message" });
                }
            }

            public requestOptions request { get; set; }
            public validationOptions validation { get; set; }
            public textOptions text { get; set; }
            public dragAndDropOptions dragAndDrop { get; set; }
            public failedUploadTextDisplayOptions failedUploadTextDisplay { get; set; }
            public callbackOptons callbacks { get; set; }

            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this };
                makeScript.ItemNotMake.Add("callbacks"); //Fine Upload的事處理格式另訂如下
                makeScript.ItemNoDot.Add("element");

                StringBuilder ls_CallbacksScript = null;

                if (this.callbacks != null)
                {
                    ls_CallbacksScript = new StringBuilder();
                    PropertyInfo[] propertyInfos = this.callbacks.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        Object tempObjValue = propertyInfo.GetValue(this.callbacks, null);
                        if (tempObjValue != null)
                        {
                            if (tempObjValue.GetType() == typeof(funcMethodModule))
                            {
                                funcMethodModule fM = (funcMethodModule)tempObjValue;
                                ls_CallbacksScript.AppendLine(".on('" + propertyInfo.Name + "'," + fM.ToScriptString() + ")");
                            }
                        }
                    }
                }

                if (ls_CallbacksScript != null)
                {
                    return "$(" + this.jqId.ToOptionString() + ").fineUploader(" + makeScript.MakeScript() + ")" + ls_CallbacksScript.ToString() + ";";
                }
                else
                {
                    return "$(" + this.jqId.ToOptionString() + ").fineUploader(" + makeScript.MakeScript() + ");";
                }
            }
        }
        public class requestOptions : ijQueryUIScript
        {
            public String endpoint { get; set; }
            public DataModule _params { get; set; }
            public DataModule customHeaders { get; set; }
            public Boolean? forceMultipart { get; set; }
            public String inputName { get; set; }
            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this, needBrace = false };
                return makeScript.MakeScript();
            }
        }
        public class validationOptions : ijQueryUIScript
        {
            public String[] allowedExtensions { get; set; }
            public String acceptFiles { get; set; }
            public Int32? sizeLimit { get; set; }
            public Int32? minSizeLimit { get; set; }
            public Boolean? autoUplstopOnFirstInvalidFileoad { get; set; }
            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this, needBrace = false };
                return makeScript.MakeScript();
            }

        }
        public class dragAndDropOptions : ijQueryUIScript
        {
            public String[] extraDropzones { get; set; }
            public Boolean? disableDefaultDropzone { get; set; }
            public Boolean? hideDropzones { get; set; }
            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this, needBrace = false };
                return makeScript.MakeScript();
            }
        }
        public class textOptions : ijQueryUIScript
        {
            public String uploadButton { get; set; }
            public String cancelButton { get; set; }
            public String failUpload { get; set; }
            public String dragZone { get; set; }
            public String formatProgress { get; set; }

            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this, needBrace = false };
                return makeScript.MakeScript();
            }
        }
        public class failedUploadTextDisplayOptions : ijQueryUIScript
        {
            public String mode { get; set; }
            public Int32 maxChars { get; set; }
            public String responseProperty { get; set; }
            public Boolean? enableTooltip { get; set; }

            public String ToScriptString()
            {
                MakeJqScript makeScript = new MakeJqScript() { GetObject = this, needBrace = false };
                return makeScript.MakeScript();
            }
        }
        public class callbackOptons
        {
            funcMethodModule _onSubmit;
            funcMethodModule _onUpload;
            funcMethodModule _onProgress;
            funcMethodModule _onComplete;
            funcMethodModule _onCancel;
            funcMethodModule _onError;
            funcMethodModule _onAutoRetry;
            funcMethodModule _onManualRetry;
            funcMethodModule _onValidate;

            public funcMethodModule submit
            {
                get
                {
                    return _onSubmit;
                }
                set
                {
                    _onSubmit = value;
                    if (_onSubmit != null)
                        _onSubmit.funcParam.AddRange(new String[] { "id", "fileName" });
                }
            }
            public funcMethodModule onUpload
            {
                get
                {
                    return _onUpload;
                }
                set
                {
                    _onUpload = value;
                    if (_onUpload != null)
                        _onUpload.funcParam.AddRange(new String[] { "id", "fileName" });
                }
            }
            public funcMethodModule progress
            {
                get
                {
                    return _onProgress;
                }
                set
                {
                    _onProgress = value;
                    if (_onProgress != null)
                        _onProgress.funcParam.AddRange(new String[] { "id", "fileName", "uploadedBytes", "totalBytes" });
                }
            }
            public funcMethodModule complete
            {
                get
                {
                    return _onComplete;
                }
                set
                {
                    _onComplete = value;
                    if (_onComplete != null)
                        _onComplete.funcParam.AddRange(new String[] { "id", "fileName", "responseJSON" });
                }
            }
            public funcMethodModule cancel
            {
                get
                {
                    return _onCancel;
                }
                set
                {
                    _onCancel = value;
                    if (_onCancel != null)
                        _onCancel.funcParam.AddRange(new String[] { "id", "fileName" });
                }
            }
            public funcMethodModule error
            {
                get
                {
                    return _onError;
                }
                set
                {
                    _onError = value;
                    if (_onError != null)
                        _onError.funcParam.AddRange(new String[] { "id", "fileName", "errorReason" });
                }
            }
            public funcMethodModule autoRetry
            {
                get
                {
                    return _onAutoRetry;
                }
                set
                {
                    _onAutoRetry = value;
                    if (_onAutoRetry != null)
                        _onAutoRetry.funcParam.AddRange(new String[] { "id", "fileName", "attemptNumber" });
                }
            }
            public funcMethodModule manualRetry
            {
                get
                {
                    return _onManualRetry;
                }
                set
                {
                    _onManualRetry = value;
                    if (_onManualRetry != null)
                        _onManualRetry.funcParam.AddRange(new String[] { "id", "fileName" });
                }
            }
            public funcMethodModule validate
            {
                get
                {
                    return _onValidate;
                }
                set
                {
                    _onValidate = value;
                    if (_onValidate != null)
                        _onValidate.funcParam.AddRange(new String[] { "fileData" });
                }
            }
        }
    }
}
