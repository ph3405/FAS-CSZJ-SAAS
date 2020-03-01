layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    $.views.converters("noZero", function (val) {

        if (val == 0) {
            return "";
        }
        else {
            return val;
        }
    });
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
    var account
    , num
        , year
    , periodId
    , calItem //核算项
    , pageIndex = 1
    , queryType = 'normal';


    var query = function () {
        queryType = 'normal';
        var index = $.loading('计算中');
        var request = {};
        request.Type = queryType;
        request.PeriodId = periodId;
        request.CalculateItem = calItem;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 1000;
        send(request, index);
    };
    var queryMore = function () {
        queryType = "more";
        var index = $.loading('查询中');
        request = {};
        request.More = {};
        request.More.Period_S = _data.field.selMonthS;
        request.More.Period_E = _data.field.selMonthE;
        request.CalculateItem = calItem;
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 1000;
        send(request, index);
    }
    var send = function (request, index) {

        $.Post("/fas/accountBook/CalBalListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    var tplHead = '';
                    var tplList = '';
                    if (num == true && year == true) {
                        tplHead = '#tpl-head1';
                        tplList = '#tpl-list1';
                    }
                    else if (num != true && year == true) {
                        tplHead = "#tpl-head2";
                        tplList = '#tpl-list2';
                    }
                    else if (num == true && year != true) {
                        tplHead = "#tpl-head3";
                        tplList = '#tpl-list3';
                    }
                    else {
                        tplHead = "#tpl-head4";
                        tplList = '#tpl-list4';
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

                    account = res.Account.QY_Name;

                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    var initCalItem = function () {
        var index = $.loading('初始化');
        var request = {};

        request.Token = token;

        $.Post("/fas/accountBook/CalculateItemGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-calItem");

                    var dataHtml = template.render(res.Data);

                    $('#selCalItem').append(dataHtml);


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

    Init();//初始化期间
    initCalItem();//核算项
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
    form.on('checkbox(num)', function (data) {
        num = data.elem.checked;

    });

    form.on('checkbox(year)', function (data) {
        year = data.elem.checked;

    });

    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore();
        return false;
    });

    //核算项
    form.on('select(calItem)', function (data) {
        calItem = data.value;
    });

    $("#btnSearch").click(function () {
        periodId = $('#selPeriod').val();
        if (periodId === '') {
            $.warning('请选择期间');
            return;
        }
        if (calItem === undefined) {
            $.warning('请选择核算项');
            return;
        }

        pageIndex = 1;
        query();


    })

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

    var printData;
    function CreateOneFormPage() {
        LODOP = getLodop();
        LODOP.PRINT_INIT("科目余额打印");

        var template = $.templates("#tpl-print");
        LODOP.SET_PREVIEW_WINDOW(2, 2, 0, -1, -1
        , '预览查看.开始打印');

        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:70%");

        var periodEle = $($('#selPeriod').find('option:selected')[0]);
        var periodTitle = $(periodEle).attr('data-title');

        var calItem = $($('#selCalItem').find('option:selected')[0]);
        var calItemVal = $(calItem).attr('data-title');


        var period = periodTitle;
        var unit = '元';
        var company = account;
        var printDate = CurentTime();
        var items = printData;

        var data = {
            CalItem: calItemVal,
            Company: company,
            Period: period,
            Unit: unit,
            PrintDate: printDate,
            Items: items
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
        if ($('#selMonthS').val() == $("#selMonthE").val()) {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期";
        }
        else {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期" + "-" + $('#selYearE').val() + "年" + $('#selMonthE option:selected').text() + "期"
        }
        queryType = "more";
        var index = $.loading('查询中');
        request = {};
        request.More = {};
        request.More.Period_S = _data.field.selMonthS;
        request.More.Period_E = _data.field.selMonthE;
        request.CalculateItem = calItem;
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 99999;
        $.Post("/fas/accountBook/CalBalListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("核算项目余额表打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
                    LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1);//横向时的正向显示
                    GetCalBalListHtml(data);
                    LODOP.PREVIEW();

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    });
    //获取核算项目余额表打印页面
    function GetCalBalListHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle =
                "<style> body {text-align: center;} .tbCalBalList td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbCalBalList tr {" +
                "height: 25px;" +
                "}" +

                ".tbCalBalList thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbCalBalList tbody, .tbCalBalList tfoot {" +
                "font-size: 13px;;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            var htmlHead = "";

            var htmlTbl = "<table class='tbCalBalList' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='8' style='font-size:20px;text-align:center;font-weight:400;border:0px'>核算项目余额表</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td colspan='8' style='font-size: 12px;border:0px;text-align:center;' ><div style='float:left;width:30%;text-align:left;'>编制单位：" + account + "</div>" +
                nowPeriod +
                "<div style='float:right;width:30%;text-align:right;'>单位：元</div></td >" +
            
                "</tr > " +
                "<tr>" +
                "<td style='width: 10%; text-align: center'>科目编码</td>" +
                "<td style='width: 10%; text-align: center'>科目名称</td >" +
                "<td style='width: 10%; text-align: center'>期初借方金额</td>" +
                "<td style='width: 10%; text-align: center'>期初贷方金额</td>" +
                "<td style='width: 10%; text-align: center'>本期借方金额</td>" +
                "<td style='width: 10%; text-align: center'>本期贷方金额</td>" +
                "<td style='width: 10%; text-align: center'>期末借方金额 </td>" +
                "<td style='width: 10%; text-align: center'>期末贷方金额 </td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail = "";
            for (var i = 0; i < saveData.Data.length; i++) {

                //var perid = saveData.PrintData[i].Year + "年" + saveData.PrintData[i].Month + "期";

                htmlDetail += "<tr>" +
                    "<td style='text-align: left'>" + saveData.Data[i].Code + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].Name + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWBStart_J) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWBStart_D) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWB_CJ) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWB_CD) + "</td>" +
             
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWBEnd_J) + "</td>" +
                    "<td style='text-align: right;'>" + noZero(saveData.Data[i].BWBEnd_D) + "</td>" +
                    "</tr>";
                count++;



            }
            htmlDetail += "</tbody>" +
                "<tfoot >" +
                "<tr>" +
                "<td  colspan='4' style='border: 0px solid #000000;'></td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>【章小算财税】</td>" +
                "</tr>" +
                "</tfoot>" +
                "</table>";


            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);
            LODOP.ADD_PRINT_TABLE(10, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
            LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);

        }
    }
    //去0
    function noZero(val) {
        if (val == 0) {
            return "";
        }
        else {
            //return val.toString().replace('-', '');
            return val.toString();
        }
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
    window.query = query;
})