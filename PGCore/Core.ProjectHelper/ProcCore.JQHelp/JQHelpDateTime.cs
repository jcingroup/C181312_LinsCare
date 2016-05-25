using System;
using System.Collections.Generic;

using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.DateTimePickerHelp
{
    public class DateTimePickerUI : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public DateTimePickerUI(jqSelector ElementId)
        {
            this.jqId = new jqSelector() { IdName = ElementId.IdName };
            this.Options = new DateTimePicker();
        }

        public String Id
        {
            get
            {
                return this.jqId.IdName;
            }
        }
        public DateTimePicker Options { get; set; }

        public String ToScriptString()
        {
            MakeJqScript createJsonString = new MakeJqScript() {GetObject = this.Options };
            return this.jqId.IDPack() + "datepicker(" + createJsonString.MakeScript() + ");";
        }
        public String ToMethodSting(DateTimePickerMethod Method, List<String> ExtraParmater)
        {

            String P_Str = String.Empty;

            if (ExtraParmater != null)
            {
                P_Str = "," + ExtraParmater.ToArray().JoinArray(",");
            }
            return "$('#" + this.Id + "').datepicker(\"" + Method + "\"" + P_Str + ");";
        }

    }

    public class DateTimePicker
    {
        private funcMethodModule _beforeShow;
        private funcMethodModule _beforeShowDay;
        private funcMethodModule _onChangeMonthYear;
        private funcMethodModule _onClose;
        private funcMethodModule _onSelect;

        public funcMethodModule beforeShow
        {
            get
            {

                return _beforeShow;
            }
            set
            {
                _beforeShow = value;
                if (_beforeShow != null)
                {
                    //_beforeShow = new funcMethodModule();
                    _beforeShow.funcParam.AddRange(new String[] { "input", "inst" });
                }
            }
        }
        public funcMethodModule beforeShowDay
        {
            get
            {
                return _beforeShowDay;
            }
            set
            {
                _beforeShowDay = value;
                if (_beforeShowDay != null)
                {
                    //_beforeShowDay = new funcMethodModule();
                    _beforeShowDay.funcParam.AddRange(new String[] { "date" });
                }
            }
        }
        public funcMethodModule onChangeMonthYear
        {
            get
            {
                return _onChangeMonthYear;
            }
            set
            {
                _onChangeMonthYear = value;
                if (_onChangeMonthYear != null)
                {
                    //_onChangeMonthYear = new funcMethodModule();
                    _onChangeMonthYear.funcParam.AddRange(new String[] { "year", "month", "inst" });
                }
            }
        }
        public funcMethodModule onClose
        {
            get
            {
                return _onClose;
            }
            set
            {
                _onClose = value;
                if (_onClose != null)
                {
                    //_onClose = new funcMethodModule();
                    _onClose.funcParam.AddRange(new String[] { "dateText", "inst" });
                }
            }
        }
        public funcMethodModule onSelect
        {
            get
            {

                return _onSelect;
            }
            set
            {
                _onSelect = value;
                if (_onSelect != null)
                {
                    //_onSelect = new funcMethodModule();
                    _onSelect.funcParam.AddRange(new String[] { "dateText", "inst" });
                }
            }
        }

        public DateTimePicker()
        {
            this.constrainInput = true;
            this.duration = Duration.normal;
            this.showAnim = UiDateTimeEffects.show;
            this.stepMonths = 1;
        }

        public Boolean? disabled { get; set; }
        /// <summary>
        /// $( ".selector" ).datepicker({ altField: '#actualDate' });
        /// </summary>
        public String altField { get; set; }
        /// <summary>
        /// $( ".selector" ).datepicker({ altFormat: 'yy-mm-dd' });
        /// </summary>
        public String altFormat { get; set; }
        /// <summary>
        /// Initialize a datepicker with the appendText option specified. 
        /// $( ".selector" ).datepicker({ appendText: '(yyyy-mm-dd)' });
        /// </summary>
        public String appendText { get; set; }
        /// <summary>
        /// Set to true to automatically resize the input field to accommodate dates in the current dateFormat.
        /// </summary>
        public Boolean? autoSize { get; set; }
        /// <summary>
        /// The URL for the popup button image. If set, buttonText becomes the alt value and is not directly displayed.
        /// </summary>
        public String buttonImage { get; set; }
        /// <summary>
        /// Set to true to place an image after the field to use as the trigger without it appearing on a button.
        /// </summary>
        public Boolean? buttonImageOnly { get; set; }
        /// <summary>
        /// $( ".selector" ).datepicker({ buttonText: "Choose" });
        /// </summary>
        public String buttonText { get; set; }
        public funcMethodModule calculateWeek { get; set; }
        public Boolean? changeMonth { get; set; }
        public Boolean? changeYear { get; set; }

        public String closeText { get; set; }
        public Boolean? constrainInput { get; set; }
        public String currentText { get; set; }
        /// <summary>
        /// $( ".selector" ).datepicker({ dateFormat: "yy-mm-dd" });
        /// </summary>
        public String dateFormat { get; set; }
        /// <summary>
        /// Default:["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
        /// </summary>
        public String[] dayNames { get; set; }
        /// <summary>
        /// Default:["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        /// </summary>
        public String[] dayNamesMin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String[] dayNamesShort { get; set; }
        /// <summary>
        /// '+1m +7d'
        /// </summary>
        public String defaultDate { get; set; }

        public Duration? duration { get; set; }

        /// <summary>
        /// Sunday is 0, Monday is 1
        /// </summary>
        public int firstDay { get; set; }

        public Boolean? gotoCurrent { get; set; }
        public Boolean? hideIfNoPrevNext { get; set; }
        public Boolean? isRTL { get; set; }

        /// <summary>
        /// '+1m +1w'
        /// </summary>
        public String maxDate { get; set; }
        /// <summary>
        /// '-1y -1m'
        /// </summary>
        public String minDate { get; set; }

        /// <summary>
        /// Default:["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
        /// </summary>
        public String[] monthNames { get; set; }
        /// <summary>
        /// Default:["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
        /// </summary>
        public String[] monthNamesShort { get; set; }

        public Boolean? navigationAsDateFormat { get; set; }
        public String nextText { get; set; }
        /// <summary>
        /// Set how many months to show at once. The value can be a straight integer, or can be a two-element array to define the number of rows and columns to display.
        /// $( ".selector" ).datepicker({ numberOfMonths: [2, 3] });
        /// </summary>
        public int[] numberOfMonths { get; set; }
        public String prevText { get; set; }
        public Boolean? selectOtherMonths { get; set; }
        /// <summary>
        /// $( ".selector" ).datepicker({ shortYearCutoff: 50 });
        /// </summary>
        public int? shortYearCutoff { get; set; }
        public UiDateTimeEffects? showAnim { get; set; }
        public Boolean? showButtonPanel { get; set; }
        public int? showCurrentAtPos { get; set; }
        public Boolean? showMonthAfterYear { get; set; }
        public DateTimeShowOn? showOn { get; set; }
        public Boolean? showOtherMonths { get; set; }
        public Boolean? showWeek { get; set; }
        public int? stepMonths { get; set; }
        public String weekHeader { get; set; }
        /// <summary>
        /// Default:"c-10:c+10"
        /// $( ".selector" ).datepicker({ yearRange: "2000:2010" });
        /// </summary>
        public String yearRange { get; set; }
        /// <summary>
        /// Additional text to display after the year in the month headers. This attribute is one of the regionalisation attributes.
        /// $( ".selector" ).datepicker({ yearSuffix: "西元" });
        /// </summary>
        public String yearSuffix { get; set; }
    }

    public enum Duration { slow, normal, fast }
    public enum UiDateTimeEffects { show, slideDown, fadeIn }
    public enum DateTimeShowOn { focus, both, button }
    public enum DateTimePickerMethod
    {
        destroy, disable, enable, widget, isDisabled, hide, refresh, getDate, setDate, option
    }
}
