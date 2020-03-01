layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    function YearMonthDay(time) {
        if (time == null || time == undefined) return "";
        var t = time.split('T');
        if (t.length < 2) return time;
        var result = t[0];
        return result;
    }
    var num //启用数量
        , periodId //一般查询当前的期间
        ,allPZ//显示所有凭证数据
    , account
    , pageIndex = 1
    ,queryType='normal';
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
    var query = function () {
        queryType = 'normal';
        var index = $.loading('计算中');
        var request = {};
        request.PeriodId = periodId;
        request.Type = queryType;
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
        request.More.Period_S = _data.field.selMonthS;
        request.More.Period_E = _data.field.selMonthE;

        request.More.Code_S = _data.field.codeS;
        request.More.Code_E = _data.field.codeE;
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        send(request, index);
    }
    var send = function (request, index) {
        $.Post("/fas/accountBook/DocDetailListSearch", request,
        function (data) {
            var res = data;
            if (res.IsSuccess) {

                var type = 0;
                var tplList = '#tpl-list';
6
                var template = $.templates(tplList);

                var html1 = template.render(res.Data);

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
    form.on('checkbox(num)', function (data) {
        num = data.elem.checked;
        
    });
    form.on('checkbox(allPZ)', function (data) {
        allPZ = data.elem.checked;

    });

    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore();
        return false;
    });

    $("#btnSearch").click(function () {
        periodId = $('#selPeriod').val();
    
        if (periodId != '')
        {
            pageIndex = 1;
            query();
        }
        
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
        LODOP.PRINT_INIT("科目汇总打印");

        var template = $.templates("#tpl-print");
        LODOP.SET_PREVIEW_WINDOW(2, 2, 0, -1, -1
        , '预览查看.开始打印');

        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");

        var periodEle = $($('#selPeriod').find('option:selected')[0]);
        var periodTitle = $(periodEle).attr('data-title');




        var period = periodTitle;
       
        var company = account;
        var printDate = CurentTime();
        var items = printData;

        var data = {
            Company: company,
            Period: period,
         
            PrintDate: printDate,
            Items: items
        };


        dataHtml = template.render(data);

        LODOP.ADD_PRINT_HTM(10, 10, '100%', '100%', dataHtml);


    };
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
    //操作
    $("body").on("click", ".row-link", function () {  // 
        var subjectCode = $(this).attr('data-code');
        var periodId_S = $('#selMonthS').val();
        var periodId_E = $('#selMonthE').val();
        var selYearS = $('#selYearS').val();
        var selYearE = $('#selYearE').val();
        var selMonthS = $('#selMonthS option:selected').text();
        var selMonthE = $('#selMonthE option:selected').text();
        //$(this).attr('data-url', '/view/fas/accountbook/detailAccount.aspx?subjectCode=' + subjectCode + "&periodId=" + periodId);
        $(this).attr('data-url', '/view/fas/accountbook/detailAccount.aspx?subjectCode=' + subjectCode + "&periodId_S=" + periodId_S + "&periodId_E=" + periodId_E + "&selYearS=" + selYearS + "&selYearE=" + selYearE + "&selMonthS=" + selMonthS + "&selMonthE=" + selMonthE);
        window.parent.addTab($(this));

    })
    var LODOP;
    var html = "";//打印生成的HTML
    var nowPeriod = "";//期别
    $('#btnPrint').click(function () {
        if ($('#selMonthS').val() == $("#selMonthE").val()) {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期";
        }
        else {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期" + "-" + $('#selYearE').val() + "年" + $('#selMonthE option:selected').text() + "期"
        }
        queryType = "more";
        LODOP = getLodop();
        LODOP.PRINT_INIT("科目汇总打印");
        var index = $.loading('查询中');
        request = {};
        request.More = {};
        request.More.Period_S = _data.field.selMonthS;
        request.More.Period_E = _data.field.selMonthE;

        request.More.Code_S = _data.field.codeS;
        request.More.Code_E = _data.field.codeE;
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 99999;
        request.allPZ = "";

        //var index = $.loading('查询中');
        //var request = {};
        //request.PeriodId = periodId;

        //request.Token = token;
        //request.PageIndex = 1;
        //request.PageSize = 99999;
        $.Post("/fas/accountBook/DocDetailListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    //LODOP = getLodop();
                    //LODOP.PRINT_INIT("科目汇总打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
                    GetSumKMHtml(data);
                    LODOP.PREVIEW();

                    //printData = res.Data;
                    //CreateOneFormPage();
                    //LODOP.PREVIEW();

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });


    });

    //获取科目汇总打印页面
    function GetSumKMHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle =
                "<style> body {text-align: center;} .tbDocDetailAccount td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbDocDetailAccount tr {" +
                "height: 25px;" +
                "}" +

                ".tbDocDetailAccount thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbDocDetailAccount tbody, .tbDocDetailAccount tfoot {" +
                "font-size: 13px;;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            var htmlHead = "";

            var htmlTbl = "<table class='tbDocDetailAccount' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='7' style='font-size:20px;text-align:center;font-weight:400;border:0px'>序时账</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td colspan='7' style='font-size: 12px;border:0px;text-align:center;' ><div style='float:left;width:30%;text-align:left;'>编制单位：" + account + "</div>" +
                nowPeriod +
                "<div style='float:right;width:30%;text-align:right;'>单位：元</div></td >" +
                "</tr > " +

                "<tr>" +
                "<td style='width: 10%; text-align: center'>日期</td>" +
                "<td style='width: 10%; text-align: center'>凭证号</td>" +
                "<td style='width: 15%; text-align: center'>科目编码</td>" +
                "<td style='width: 25%; text-align: center'>科目名称</td>" +
                "<td style='width: 25%; text-align: center'>摘要</td >" +
                "<td style='width: 5%; text-align: center'>方向</td>" +
                "<td style='width: 10%; text-align: center'>金额</td>" +

                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail = "";
            for (var i = 0; i < saveData.Data.length; i++) {

                
                htmlDetail += "<tr>" +
                    "<td style='text-align: center'>" + YearMonthDay(saveData.Data[i].PZDate)  + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].PZZ + "-" + saveData.Data[i].PZZNO + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].SubjectCode + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].Name + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].Summary + "</td>" +
                    "<td style='text-align: center'>" + saveData.Data[i].Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Data[i].amt + "</td>" +
                    "</tr>";
                count++;



            }
            htmlDetail += "</tbody>" +
                "<tfoot >" +
                "<tr>" +
                "<td  colspan='4' style='border: 0px solid #000000;'></td>" +
                "<td style='text-align: right;border: 0px solid #000000;' >打印日期：" + CurentTime() + "</td>" +
                "<td  colspan='2'  style='text-align: right;border: 0px solid #000000;'>【章小算财税】</td>" +
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