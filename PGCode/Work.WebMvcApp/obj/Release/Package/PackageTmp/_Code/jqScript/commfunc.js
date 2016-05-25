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
                                    //$('#progressbar').center();
                                    //$('#progressbar').show();
                                    //$('#progressbar').progressbar({ value: percentComplete });
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

    $.UiMessage = function (jsonobj) {
        if (jsonobj.message != null)
            if (jsonobj.message != '')
                $('#dialog')
                    .dialog({ title: jsonobj.title })
                    .html(jsonobj.message.replace(/\r\n/g, '<br />'))
                    .dialog('open');
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
        return dateObject.getFullYear() + '/' + (dateObject.getMonth() + 1) + '/' + dateObject.getDate();
    }

    $.CollectQuery = function () {
        var getelement = $("#gridform table input");
        var getQueryStr = '';

        $.each(getelement,
            function (index, value) {
                if (value.value != '') {
                    getQueryStr += value.id + '=' + encodeURIComponent(value.value) + '&';
                }
            }
        )

        return getQueryStr;
    };

    $.pageQuery = function (GridID) {
        return 'page=' + jQuery("#" + GridID).getGridParam('page');
    };

    $.FilesCount = function (elementId, ajax_url_CountFiles, Id, FileKind) {

        $("#wait").html('檔案計算中...請稍侯!');
        $("#wait").show();
        var count_ct = 0;
        $.EventAjaxHandle({ 'id': Id, 'FileKind': FileKind }, ajax_url_CountFiles)
        .done(function (data, textStatus, jqXHR) {
            var jsonobj = jQuery.parseJSON(data);
            count_ct = jsonobj.filesObject.length;
        })
        .always(function () {
            $("#wait").hide();
        });
        return count_ct;
    }

    $.FilesHave = function (elementId, ajax_url_CountFiles, Id, FileKind) {

        var count_ct = $.FilesCount(elementId, ajax_url_CountFiles, Id, FileKin);
        return count_ct > 0;
    }

    $.FilesListHandle = function (elementId, ajax_url_ListFiles, ajax_url_DeleteFiles, Id, FileKind) {

        $("#" + elementId).html(''); //清空 filesLits
        $("#wait").html('檔案載入中...請稍侯!');
        $("#wait").show();

        $.EventAjaxHandle({ 'id': Id, 'FileKind': FileKind }, ajax_url_ListFiles)
        .done(function (data, textStatus, jqXHR) {

            var jsonobj = jQuery.parseJSON(data);
            //以下為FileList顯示介面
            for (var ij = 0; ij < jsonobj.filesObject.length; ij++) {

                if (jsonobj.filesObject[ij].IsImage) {
                    $("#" + elementId)
                    .append(
                        $('<div style="padding:3px;width:420px;height:120px">')
                        .append($('<div style="padding:3px;margin-right:5px;float:left;border:1px; border-style:solid">').html('<a class="fancybox" rel="group" href="' + jsonobj.filesObject[ij].OriginFilePath + '"><img src="' + jsonobj.filesObject[ij].RepresentFilePath + '"></a>'))
                        .append($('<div style="padding:5px">').html(jsonobj.filesObject[ij].FileName))
                        .append($('<div style="padding:5px">').html(Math.ceil((jsonobj.filesObject[ij].Size / 1024)) + ' KB'))
                        .append($('<div style="padding:5px">').html('<button type="button" class="DeleteFilesButton" listElementId="' + elementId + '" listaction="' + ajax_url_ListFiles + '" delaction="' + ajax_url_DeleteFiles + '" idx="' + Id + '" FileKind="' + FileKind + '" FileName="' + jsonobj.filesObject[ij].FileName + '">刪除</button>'))
                        .append($('<div style="padding:5px">').html(jsonobj.filesObject[ij].OriginFilePath))
                    );
                } else {
                    var setFileKind = 'DocFiles';
                    if (jsonobj.filesObject[ij].FilesKind != null) {
                        setFileKind = jsonobj.filesObject[ij].FilesKind;
                    }
                    $("#" + elementId)
                    .append(
                        $('<div style="padding:3px;width:420px;height:60px;">')
                        .append($('<div style="padding:3px;margin-right:5px;float:left;">').html('<button class="DownLoadFilesButton" type="button" area="' + gb_area + '" controller="' + gb_controller + '" Id="' + Id + '" FileKind="' + setFileKind + '" FileName="' + jsonobj.filesObject[ij].FileName + '" >下載</button>' + '<button type="button" class="DeleteFilesButton" listElementId="' + elementId + '" listaction="' + ajax_url_ListFiles + '" delaction="' + ajax_url_DeleteFiles + '" idx="' + Id + '" FileKind="' + setFileKind + '" filename="' + jsonobj.filesObject[ij].FileName + '">刪除</button>'))
                        .append($('<div style="padding:5px">').html(jsonobj.filesObject[ij].FileName))
                        .append($('<div style="padding:5px">').html(Math.ceil((jsonobj.filesObject[ij].Size / 1024)) + ' KB'))
                    );
                }
            }
            $(".DeleteFilesButton").button({ icons: { primary: 'ui-icon-closethick', secondary: '' } });
            $(".DownLoadFilesButton").button({ icons: { primary: 'ui-icon-arrowstop-1-s', secondary: '' } });
            $(".fancybox").fancybox();
        })
        .always(function () {
            $("#wait").hide();
        });
    }

    $.FileDownLoad = function (Id, FilesKind, FileName) {
        //利用Hidden iframe 下載檔案
        var path = gb_approot + gb_area + '/' + gb_controller + '/DownLoadFile?Id=' + Id + '&FileName=' + encodeURIComponent(FileName) + '&FilesKind=' + FilesKind;
        $('#ifm_filedownload').attr('src', path);
    }

    $.AjaxFormOnFailure = function (ajaxContext) {
        var response = ajaxContext.get_response();
        var statusCode = response.get_statusCode();
        alert("AJAX Request Failure： " + statusCode);
    }

    $(document).on('click', '.DeleteFilesButton', function () {

        var idx = $(this).attr('idx');
        var filekind = $(this).attr('filekind');
        var filename = $(this).attr('filename');

        var url_ListFiles = $(this).attr('listaction');
        var url_DeleteFiles = $(this).attr('delaction');
        var listElementId = $(this).attr('listElementId');

        $.EventAjaxHandle({ 'id': idx, 'FileKind': filekind, 'FileName': filename }, url_DeleteFiles)
        .done(function (data, textStatus, jqXHR) {
            var jsonobj = jQuery.parseJSON(data);
            $.FilesListHandle(listElementId, url_ListFiles, url_DeleteFiles, idx, filekind);
        })
    })
    $(document).on('click', '.DownLoadFilesButton', function () {
        var gId = $(this).attr('Id');
        var gFileKind = $(this).attr('FileKind');
        var gFileName = $(this).attr('FileName');

        $.FileDownLoad(gId, gFileKind, gFileName);
    })

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
                countyElement: $('#county'),
                cityValue: '桃園縣',
                countyValue: '中壢市'
            }, options || {});

            var cityElement = $(this);

            if (options.cityValue != '')
                $(this).val(options.cityValue);

            $(this).change(function () {
                options.countyElement.empty();
                $.EventAjaxHandle({ 'city': $(this).val() }, gb_approot + '_Code/Ashx/AjaxGetCounty.ashx?uid=' + uniqid())
                .done(function (data, textStatus, jqXHR) {
                    var jsonobj = jQuery.parseJSON(data);

                    if (jsonobj.result) {
                        for (property in jsonobj.data) {
                            var option_html;
                            if (options.countyValue == property) {
                                option_html = '<option value="' + property + '" selected>' + jsonobj.data[property] + '</option>';
                            } else {
                                option_html = '<option value="' + property + '">' + jsonobj.data[property] + '</option>'
                            }
                            options.countyElement.append(option_html);
                        }
                        options.countyElement.trigger('change');
                    }

                    if (jsonobj.message != '')
                        $.UiMessage(jsonobj);

                });
            }).trigger('change');

            options.countyElement.change(function () {
                $.EventAjaxHandle({ 'city': cityElement.val(), 'county': $(this).val() }, gb_approot + '_Code/Ashx/AjaxGetZip.ashx?uid=' + uniqid())
                .done(function (data, textStatus, jqXHR) {

                    var jsonobj = jQuery.parseJSON(data);
                    if (jsonobj.result)
                        options.zipElement.val(jsonobj.data);

                    if (jsonobj.message != '')
                        $.UiMessage(jsonobj);
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
                $.EventAjaxHandle({ 'master_value': $(this).val() }, options.data_url + '?uid=' + uniqid())
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
                        $.UiMessage(jsonobj);
                });
            }).trigger('change');

            return this;
        }
    })
})(jQuery);

jQuery.extend({
    handleError: function (s, xhr, status, e) {
        if (s.error) {
            s.error.call(s.context, xhr, status, e);
        }

        if (s.global) {
            jQuery.triggerGlobal(s, "ajaxError", [xhr, s, e]);
        }
    }
})

jQuery.validator.addMethod("TWIDCheck", function (value, element, param) {
    var a = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O');
    var b = new Array(1, 9, 8, 7, 6, 5, 4, 3, 2, 1);
    var c = new Array(2);
    var d;
    var e;
    var f;
    var g = 0;
    var h = /^[a-z](1|2)\d{8}$/i;
    if (value.search(h) == -1) {
        return false;
    }
    else {
        d = value.charAt(0).toUpperCase();
        f = value.charAt(9);
    }
    for (var i = 0; i < 26; i++) {
        if (d == a[i])//a==a
        {
            e = i + 10; //10
            c[0] = Math.floor(e / 10); //1
            c[1] = e - (c[0] * 10); //10-(1*10)
            break;
        }
    }
    for (i = 0; i < b.length; i++) {
        if (i < 2) {
            g += c[i] * b[i];
        }
        else {
            g += parseInt(value.charAt(i - 1)) * b[i];
        }
    }
    if ((g % 10) == f) {
        return true;
    }
    if ((10 - (g % 10)) != f) {
        return false;
    }
    return true;
}, "請輸入有效的身份證字號!");

function uniqid() { var newDate = new Date; return newDate.getTime(); }
