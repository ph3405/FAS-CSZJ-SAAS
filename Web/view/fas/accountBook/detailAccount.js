layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage,
        $ = layui.jquery;
    //var periodId = $.getQueryString('periodId');
    var periodId_S = $.getQueryString('periodId_S');
    var periodId_E = $.getQueryString('periodId_E');
    var subjectCode = $.getQueryString('subjectCode');
    var selYearS = $.getQueryString('selYearS');
    var selYearE = $.getQueryString('selYearE');
    var selMonthS = $.getQueryString('selMonthS');
    var selMonthE = $.getQueryString('selMonthE');
    var printData//打印的数据
        , num
        , allPZ
        , account
        , pageIndex = 1
        //, queryType = 'normal'
        , queryType = 'more'
        , _data
        , periodS
        , periodE;
    function GetAllYear(list) {
        var arr = [];
        for (var i = 0; i < list.length; i++) {
            if (i == 0) arr.push({ Year: list[i].Year, IsActive: 0 });
            b = false;
            if (arr.length > 0 && i > 0) {
                for (var j = 0; j < arr.length; j++) {
                    if (arr[j].Year == list[i].Year) {
                        b = true;
                        //break;
                    }
                }
                if (!b) {
                    arr.push({ Year: list[i].Year, IsActive: 0 });
                }
            }
        }
        for (var i = 0; i < arr.length; i++) {
            for (var j = 0; j < list.length; j++) {
                if (list[j].IsActive == 1 && arr[i].Year == list[j].Year) {
                    arr[i].IsActive = list[j].IsActive;
                }
            }
        }
        return arr;
    }
    var ActiveYear;//激活的年份
    var allPeriod = [];//所有期间集合
    function GetActiveYear(list) {
        for (var i = 0; i < list.length; i++) {
            if (list[i].IsActive == 1)
                ActiveYear = list[i].Year;

        }
    }
    function GetMonth(list) {
        var arr = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].Year == ActiveYear)
                arr.push({ Month: list[i].Month, IsActive: 0, Id: list[i].Id });
        }
        for (var i = 0; i < arr.length; i++) {
            for (var j = 0; j < list.length; j++) {
                if (list[j].IsActive == 1 && arr[i].Month == list[j].Month) {
                    arr[i].IsActive = list[j].IsActive;
                }
            }
        }
        return arr;
    }
    form.on('checkbox(num)', function (data) {
        num = data.elem.checked;

    });
    form.on('checkbox(allPZ)', function (data) {
        allPZ = data.elem.checked;

    });


    var query = function () {
        queryType = "normal";
        var index = $.loading('计算中');
        var request = {};
        request.Type = queryType;
        request.PeriodId = periodId;
        request.SubjectCode = subjectCode;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        request.allPZ = "";
        if (allPZ == true) {
            request.allPZ = "allPZ";
        }
        send(request, index);

    };

    var queryMore = function () {
        queryType = "more";
        var index = $.loading('查询中');
        request = {};
        request.More = {};

        if (periodId_S != undefined && periodId_E != undefined) {
            
            request.More.Period_S = periodId_S;
            request.More.Period_E = periodId_E;
        }
        else {
            request.More.Period_S = _data.field.selMonthS;
            request.More.Period_E = _data.field.selMonthE;
        }
        //request.More.Code = _data.field.code;


        if (subjectCode != undefined) {
            request.More.Code_S = subjectCode;
            request.More.Code_E = subjectCode;
        }
        else {
            request.More.Code_S = _data.field.codeS;
            request.More.Code_E = _data.field.codeE;
        }
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        request.allPZ = "";
        if (allPZ == true) {
            request.allPZ = "allPZ";
        }
        send(request, index);
    }

    var send = function (request, index) {
        $.Post("/fas/accountBook/DetailListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var tplHead = '';
                    var tplList = '';
                    if (num == true) {
                        tplHead = '#tpl-head01';
                        tplList = '#tpl-list01';
                    }
                    else if (num != true) {
                        tplHead = "#tpl-head02";
                        tplList = '#tpl-list02';
                    }


                    var templateHead = $.templates(tplHead);

                    var headHtml = templateHead.render({});

                    $('#tbHead').html(headHtml);


                    var template = $.templates(tplList);

                    var html1 = template.render(res.Data);
                    printData = res.Data;

                    $('#dt1').html(html1);
                    $('.layui-search-mored').hide();
                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / request.PageSize),
                        jump: function (obj, first) {
                            if (!first) {
                                pageIndex = obj.curr;
                                if (queryType == "normal") {
                                    query();
                                }
                                else {
                                    queryMore();
                                }
                            }
                        }
                    });

                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    }

    var Init = function () {

        var index = $.loading('初始化');
        var request = {};

        request.Token = token;

        $.Post("/fas/period/PeriodPaidGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-select");

                    var dataHtml = template.render(res.Data);
                    account = res.Account.QY_Name;
                    $('#selPeriod').append(dataHtml);
                   
                    var template_Year = $.templates("#tpl-Year");
                    var template_Month = $.templates("#tpl-Month");
                    allPeriod = res.Data;
                    var lstYear = GetAllYear(res.Data);
                    var dataHtml_Year = template_Year.render(lstYear);

                    $('#selYearS').html(dataHtml_Year);
                    $('#selYearE').html(dataHtml_Year);
                    GetActiveYear(res.Data);//获取当前激活的年份

                    var lstMonth = GetMonth(res.Data);

                    var dataHtml_Month = template_Month.render(lstMonth);

                    $('#selMonthS').html(dataHtml_Month);
                    $('#selMonthE').html(dataHtml_Month);
                    //if (dataHtml != '') {
                    //    $('#selPeriodS').html(dataHtml);
                    //    $('#selPeriodE').html(dataHtml);
                    //}

                    //加载期间内的科目，废弃，需求不要
                    //var subEle = $($('#selPeriodS').find('option:selected')[0]);
                    //if (subEle != undefined) {
                    //    var id = $(subEle).val();
                    //    periodS = id;
                    //    periodE = id;
                    //    if (periodS != undefined && periodE != undefined)
                    //        InitSubjectMore();
                    //}

                    //if (periodId != '' && periodId != undefined) {
                    //    $('#selPeriod').val(periodId);
                    //    InitSubject(periodId);
                    //}
                    if (subjectCode != '' && subjectCode != undefined) {
                        //$('#selSubject').val(subjectCode);
                        //query(1);
                        $('#selYearS option').each(function () {
                            if ($(this).text() == selYearS) {
                                $(this).prop("selected", true);
                            }
                        })
                        $('#selYearE option').each(function () {
                            if ($(this).text() == selYearE) {
                                $(this).prop("selected", true);
                            }
                        })
                        $('#selMonthS option').each(function () {
                            if ($(this).text() == selMonthS) {
                                $(this).prop("selected", true);
                            }
                        })
                        $('#selMonthE option').each(function () {
                            if ($(this).text() == selMonthE) {
                                $(this).prop("selected", true);
                            }
                        })
                        queryMore(1);
                    }
                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
        $.Post("/fas/set/GetSubject", request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    bindAutocomplete($("[name='codeS']"), $("[name='codeE']"), res.Data);
                    $.topTip(res.Message);
                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });

    };

    var InitSubject = function (periodId) {
        var index = $.loading('初始化');
        var request = {};
        request.PeriodId = periodId;
        request.Token = token;

        $.Post("/fas/accountBook/DocSubjectCodeGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-subject");

                    var dataHtml = template.render(res.Data);

                    $('#selSubject').append(dataHtml);
                    if (subjectCode != '' && subjectCode != undefined) {
                        $('#selSubject').val(subjectCode);
                        query(1);
                    }

                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    }

    var bindAutocomplete = function (codeS, codeE, data) {
        $(codeS).autocomplete({
            source: data,
            select: function (event, ui) {
                $(this).val(ui.item.Id);
            }
        })

        $(codeE).autocomplete({
            source: data,
            select: function (event, ui) {
                $(this).val(ui.item.Id);
            }
        })
    };

    //初始化更多的科目条件
    var InitSubjectMore = function () {

        var request = {};
        request.PeriodS = periodS;
        request.PeriodE = periodE;
        request.Token = token;

        $.Post("/fas/accountBook/CodeGetInPeriod", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-subject");

                    var dataHtml = template.render(res.Data);

                    $('#selSubjectMore').html(dataHtml);

                    form.render();
                } else {
                    $.warning(res.Message);
                }

            }, function (err) {
                $.warning(err.Message);

            });
    }

    Init();//初始化期间
    form.on('select(selYearS)', function (data) {
        var year = $('#selYearS').val();
        var arr = [];
        for (var i = 0; i < allPeriod.length; i++) {
            if (allPeriod[i].Year == year)
                arr.push({ Month: allPeriod[i].Month, IsActive: 0, Id: allPeriod[i].Id });
        }
        if (ActiveYear == year) {
            for (var i = 0; i < arr.length; i++) {
                for (var j = 0; j < allPeriod.length; j++) {
                    if (allPeriod[j].IsActive == 1 && arr[i].Month == allPeriod[j].Month) {
                        arr[i].IsActive = allPeriod[j].IsActive;
                    }
                }
            }
        }
        var template_Month = $.templates("#tpl-Month");
        var dataHtml_Month = template_Month.render(arr);
        if (dataHtml_Month == "") {
            dataHtml_Month = "<option value=''></option>";
        }
        $('#selMonthS').html(dataHtml_Month);
        form.render('select');
    });
    form.on('select(selYearE)', function (data) {
        var year = $('#selYearE').val();
        var arr = [];
        for (var i = 0; i < allPeriod.length; i++) {
            if (allPeriod[i].Year == year)
                arr.push({ Month: allPeriod[i].Month, IsActive: 0, Id: allPeriod[i].Id });
        }
        if (ActiveYear == year) {
            for (var i = 0; i < arr.length; i++) {
                for (var j = 0; j < allPeriod.length; j++) {
                    if (allPeriod[j].IsActive == 1 && arr[i].Month == allPeriod[j].Month) {
                        arr[i].IsActive = allPeriod[j].IsActive;
                    }
                }
            }
        }
        var template_Month = $.templates("#tpl-Month");
        var dataHtml_Month = template_Month.render(arr);
        if (dataHtml_Month == "") {
            dataHtml_Month = "<option value=''></option>";
        }
        $('#selMonthE').html(dataHtml_Month);
        form.render('select');
    });
    //一般查询
    $("#btnSearch").click(function () {
        periodId = $('#selPeriod').val();
        subjectCode = $('#selSubject').val();
        if (periodId != '' && subjectCode != '') {
            pageIndex = 1;
            query();
        }

    })

    //更多查询
    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore();
        return false;
    });
    form.on('select(period)', function (data) {
        periodId = data.value;
        InitSubject(data.value);
    });
    form.on('select(subject)', function (data) {

        subjectCode = data.value;

    });

    form.on('select(p1)', function (data) {
        periodS = data.value;
        if (periodS != undefined && periodE != undefined)
            InitSubjectMore();
    });
    form.on('select(p2)', function (data) {
        periodE = data.value;
        if (periodS != undefined && periodE != undefined)
            InitSubjectMore();
    });

    function CurentTime() {
        var now = new Date();

        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日

        var clock = year + "-";

        if (month < 10)
            clock += "0";

        clock += month + "-";
        if (day < 10)
            clock += "0";

        clock += day;
        return (clock);
    }

    function CreateOneFormPage() {
        LODOP = getLodop();
        LODOP.PRINT_INIT("明细账打印");

        var template = $.templates("#tpl-print");
        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:85%");

        var periodEle = $($('#selPeriod').find('option:selected')[0]);
        var periodTitle = $(periodEle).attr('data-title');
        var subEle = $($('#selSubject').find('option:selected')[0]);


        var title = $(subEle).attr('data-title');
        var subject = subjectCode + " " + title;
        var period = periodTitle;
        var unit = '元';
        var company = account;
        var printDate = CurentTime();
        var items = printData;

        LODOP.NewPage();

        var data = {
            Title: title,
            Subject: subject,
            Period: period,
            Unit: unit,
            Company: company,
            Detail: items,
            PrintDate: printDate

        };
        dataHtml = template.render(data);

        LODOP.ADD_PRINT_HTM(10, 10, '100%', '100%', dataHtml);

    };
    var LODOP;
    var html = "";//打印生成的HTML
    var nowPeriod = "";//期别
    $('#btnPrint').click(function () {
        //if (printData == undefined || printData == null) {
        //    $.warning('请先查出您要打印的数据');
        //    return;
        //}
        //CreateOneFormPage();
        //LODOP.PREVIEW();
        if (selYearS != undefined) {
            if (selYearS == selYearE && selMonthS == selMonthE) {
                nowPeriod = selYearS + "年" + selMonthS + "期";
            }
            else {
                nowPeriod = selYearS + "年" + selMonthS + "期" + "-" + selYearE + "年" + selMonthE + "期";
            }
        }
        else {
            if ($('#selMonthS').val() == $("#selMonthE").val()) {
                nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期";
            }
            else {
                nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期" + "-" + $('#selYearE').val() + "年" + $('#selMonthE option:selected').text() + "期";
            }
        }
        var request = {};
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 9999;
        request.More = {};
        if (periodId_S != undefined && periodId_E != undefined) {
            request.More.Period_S = periodId_S;
            request.More.Period_E = periodId_E;
        }
        else {
            request.More.Period_S = $('#selMonthS').val();
            request.More.Period_E = $("#selMonthE").val();
        }


        if (subjectCode != undefined) {
            request.More.Code_S = subjectCode;
            request.More.Code_E = subjectCode;
        }
        else {
            request.More.Code_S = $("[name='codeS']").val();
            request.More.Code_E = $("[name='codeE']").val();
        }



        request.Type = "more";
        LODOP = getLodop();

        LODOP.PRINT_INIT("明细账打印");
        //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
        LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
        var index = $.loading('打印获取数据');
        $.Post("/fas/accountBook/DetailListSearch", request,
            function (data) {

                var res = data;
                if (res.IsSuccess) {

                    GetMXHtml(data);
                    LODOP.PREVIEW();

                } else {

                }
                layer.close(index);
            }, function (err) {
                layer.close(index);
            });
    });
    //获取明细账打印页面
    function GetMXHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle_MX =
                "<style>body {text-align: center;} .tbMX td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbMX tr {" +
                "height: 25px;" +
                "}" +

                ".tbMX thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbMX tbody, .tbMX tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            //var perid = saveData.PrintData[0].Year + "年" + saveData.PrintData[0].Month + "期";
            var htmlHead_MX = "";
            //var htmlHead_MX = "<table style='width: 95%;margin: 0 auto;'>" +
            //    "<tr>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400'> 明细账</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr style='height: 30px; '>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + account + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;'>" + nowPeriod + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
            //    "</tr > " +
            //    "</table>";

            var htmlTbl_MX =
                "<table class='tbMX' style='width:90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='8' style='font-size:20px;text-align:center;font-weight:400;border:0px'>明细账</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td colspan='3' style='font-size: 12px;border:0px' >编制单位：" + account + "</td >" +
                "<td style='font-size: 12px;border:0px'>" + nowPeriod + "</td>" +
                "<td  colspan='4'  style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td style='text-align: center; width: 10%'>日期</td>" +
                "<td style='text-align: center; width: 7%'>凭证字</td>" +
                "<td style='text-align: center; width: 23%'>科目</td>" +
                "<td style='text-align: center; width: 15%'>摘要</td>" +
                "<td style='text-align: center; width: 10%;'>期初</td>" +
                "<td style='text-align:center;width:10%;'>借方</td>" +
                "<td style='text-align:center;width:10%;'>贷方</td>" +
                "<td style='text-align: center; width: 5%'>方向</td>" +
                "<td style='text-align: center; width: 10%;'>金额</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail_MX = "";
            for (var i = 0; i < saveData.Data.length; i++) {
                var Money1 = "";
                var Money2 = "";
                var Show_Credit_Debit = "平";
                var StartBAL = "";
                var Show_Money = "";
                var DetailDate = "";
                var PZZ = "";
                var Name = "";
                var Summary = "";
                try {
                    Money1 = saveData.Data[i].Money1 == 0 ? "" : saveData.Data[i].Money1;
                    Money2 = saveData.Data[i].Money2 == 0 ? "" : saveData.Data[i].Money2;
                    //Show_Credit_Debit = saveData.Data[i].Show_Credit_Debit == 0 ? "借" : "贷";
                    if (saveData.Data[i].Show_Credit_Debit == 0) {
                        Show_Credit_Debit = "借";
                    }
                    else if (saveData.Data[i].Show_Credit_Debit == 1) {
                        Show_Credit_Debit = "贷";
                    }
                    StartBAL = saveData.Data[i].StartBAL == 0 ? "" : saveData.Data[i].StartBAL;
                    Show_Money = saveData.Data[i].Show_Money == 0 ? "" : saveData.Data[i].Show_Money;
                    DetailDate = YearMonthDay(saveData.Data[i].DetailDate);
                    PZZ = saveData.Data[i].PZZ;
                    Name = saveData.Data[i].Name;
                    Summary = saveData.Data[i].Summary;
                } catch (e) {

                    continue;
                }

                htmlDetail_MX += "<tr>" +
                    "<td style='text-align: center'>" + DetailDate + "</td> " +
                    "<td style='text-align :center'>" + PZZ + "</td>" +
                    "<td style='text-align: left'>" + Name + "</td>" +
                    "<td style='text-align: left'>" + Summary + "</td>" +
                    "<td style='text-align: right;'>" + StartBAL + "</td>" +
                    "<td style='text-align: right'>" + Money1 + "</td>" +
                    "<td style='text-align: right'>" + Money2 + "</td>" +
                    "<td style='text-align: center'>" + Show_Credit_Debit + "</td>" +
                    "<td style='text-align: right;'>" + Show_Money + "</td>" +
                    "</tr>";
                count++;

            }
            htmlDetail_MX += "</tbody>" +
                "<tfoot>" +
                "<tr>" +
                "<td  colspan='4' style='border: 0px solid #000000;'></td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>【章小算财税】</td>" +
                "</tr>" +
                "</tfoot>" +
                "</table>";


            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle_MX +htmlHead_MX);
            LODOP.ADD_PRINT_TABLE(10, 0, '100%', '100%', htmlStyle_MX + htmlTbl_MX + htmlDetail_MX);
            LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);

        }
    }

    function YearMonthDay(time) {
        if (time == null || time == undefined) return "";
        var t = time.split('T');
        if (t.length < 2) return time;
        var result = t[0];
        return result;
    }
    //更多筛选
    $('#btnMore').click(function () {
        var offset = $("#btnMore").offset();
        $(".layui-search-mored").css("position", "abosolute").css("left", offset.left + "px").css("top", offset.top + 38 + "px");
        $('.layui-search-mored').toggle();
    });
    $('#btnCancel').click(function () {
        $('.layui-search-mored').hide();
    });
    window.query = queryMore;
})