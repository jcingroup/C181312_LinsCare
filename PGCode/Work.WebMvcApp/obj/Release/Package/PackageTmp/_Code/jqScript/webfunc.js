var ajaxRequest = [];

(function ($) {

    $.EventAjaxHandle = function (jsonData, handleUrl) {

        var ax = $.ajax
		    (
                handleUrl,
			    {
			        type: "POST",
			        datatype: "json",
			        async: false,
			        contentType: "application/x-www-form-urlencoded; charset=utf-8",
			        data: jsonData,
			        cache: false,
			        global: true,
			        complete: function (jqXHR, textStatus) { jqXHR = null; $('#progressbar').hide(); },
			        xhr: function () {
			            var xhrObject = $.ajaxSettings.xhr();
			            if (xhrObject.upload) {
			                xhrObject.upload.addEventListener('progress',
                                function (event) {
                                    var percentComplete = (event.loaded / event.total) * 100;
                                }, false);
			            } else {

			            }
			            return xhrObject;
			        }
			    }
		    )
            .fail(function (jqXHR, textStatus, errorThrown) {
                alert('Ajax發生錯誤[Url:' + handleUrl + ']:[' + errorThrown + "]");
            });

        ajaxRequest.push(ax);
        return ax;
    }

    $.ajax_GetNewId = function () {
        var NewId = 0;
        $.EventAjaxHandle({}, 'GetNewId').done(function (data, textStatus, jqXHR) {
            NewId = jQuery.parseJSON(data);
        })
        return NewId;
    }

    $.parseMsJsonDate = function (value) {
        var dateRegExp = /^\/Date\((.*?)\)\/$/;
        var dateInfo = dateRegExp.exec(value);
        var dateObject = new Date(parseInt(dateInfo[1]));
        return dateObject.getFullYear() + '/' + (dateObject.getMonth()+1) + '/' + dateObject.getDate();
    }

    $.fn.extend({
        center: function () {
            this.css("position", "absolute");
            this.css("top", ($(document).height() - this.height()) / 2 + $(document).scrollTop() + "px");
            this.css("left", ($(document).width() - this.width()) / 2 + $(document).scrollLeft() + "px");
            return this;
        },
        check: function () {
            return this.each(function () { this.checked = true; });
        },
        uncheck: function () {
            return this.each(function () { this.checked = false; });
        },
        addressajax: function (options) {
            //地址處理
            options = $.extend({
                zipElement: $('#zip'),
                countryElement: $('#country'),
                cityValue: '桃園縣',
                countryValue: '中壢市'
            }, options || {});

            var cityElement = $(this);

            if (options.cityValue != '')
                $(this).val(options.cityValue);

            $(this).change(function () {
                options.countryElement.empty();
                $.EventAjaxHandle({ 'city': $(this).val() }, gb_approot + '/_Code/Ashx/AjaxGetCountry.ashx?uid=' + uniqid())
                .done(function (data, textStatus, jqXHR) {
                    var jsonobj = jQuery.parseJSON(data);

                    if (jsonobj.result) {
                        for (property in jsonobj.data) {
                            var option_html;
                            if (options.countryValue == property) {
                                option_html = '<option value="' + property + '" selected>' + jsonobj.data[property] + '</option>';
                            } else {
                                option_html = '<option value="' + property + '">' + jsonobj.data[property] + '</option>'
                            }
                            options.countryElement.append(option_html);
                        }
                        options.countryElement.trigger('change');
                    }

                    if (jsonobj.message != '')
                        alert(jsonobj.message);
                });
            }).trigger('change');

            options.countryElement.change(function () {
                $.EventAjaxHandle({ 'city': cityElement.val(), 'country': $(this).val() }, gb_approot + '/_Code/Ashx/AjaxGetZip.ashx?uid=' + uniqid())
                .done(function (data, textStatus, jqXHR) {

                    var jsonobj = jQuery.parseJSON(data);
                    if (jsonobj.result)
                        options.zipElement.val(jsonobj.data);

                    if (jsonobj.message != '')
                        alert(jsonobj.message);
                });
            }).trigger('change');
            return this;
        },
        selectajax: function (options) {
            options = $.extend({
                relation_element: $('#relation'),
                master_value: 0,
                relation_value: 0,
                data_url: ''
            }, options || {});

            var master_element = $(this);

            if (options.master_value > 0)
                $(this).val(options.master_value);

            $(this).change(function () {
                options.relation_element.empty();
                $.EventAjaxHandle({ 'master_value': $(this).val() }, gb_approot + options.data_url + '?uid=' + uniqid())
                .done(function (data, textStatus, jqXHR) {
                    var jsonobj = jQuery.parseJSON(data);

                    if (jsonobj.result) {
                        for (property in jsonobj.data) {
                            var option_html;
                            if (options.relation_value == property)
                                option_html = '<option value="' + property + '" selected>' + jsonobj.data[property] + '</option>';
                            else
                                option_html = '<option value="' + property + '">' + jsonobj.data[property] + '</option>';

                            options.relation_element.append(option_html);
                        }
                    }

                    if (jsonobj.message != '')
                        alert(jsonobj.message);
                });
            }).trigger('change');

            return this;
        }
    })
})(jQuery);

function uniqid() { var newDate = new Date; return newDate.getTime(); }
