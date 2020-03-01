layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("thousand", function (val) {

        if (val == 0 || val == undefined || val == null) {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
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
    var account;

    var query = function (periodId) {

        var index = $.loading('计算中');
        var request = {};
        request.Token = token;
        request.PeriodId = periodId;
        $.Post("/fas/report/JYGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var html1 = template.render(res);
                    printData1 = res;
                    account = res.Account;
                    $('#dt1').html(html1);


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

    window.query = query;
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
                    //$('#selPeriod').append("<option value='QC'  selected>期初</option>");
                    //$('#selPeriod').append(dataHtml);
                    $('#selPeriod').html(dataHtml);

                    var template_Year = $.templates("#tpl-Year");
                    var template_Month = $.templates("#tpl-Month");
                    allPeriod = res.Data;
                    var lstYear = GetAllYear(res.Data);
                    var dataHtml_Year = template_Year.render(lstYear);
                    $('#selYear').html(dataHtml_Year);

                    GetActiveYear(res.Data);//获取当前激活的年份

                    var lstMonth = GetMonth(res.Data);

                    var dataHtml_Month = template_Month.render(lstMonth);
                    $('#selMonth').html(dataHtml_Month);
                    query($('#selMonth').val());
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
    form.on('select(selYear)', function (data) {
        var year = $('#selYear').val();
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
        $('#selMonth').html(dataHtml_Month);
        form.render('select');
    }); 

    $("#btnSearch").click(function () {
        //var period = $('#selPeriod').val();
        var period = $('#selMonth').val();
        if (period != '') {
            query(period);
        }

    })


    var printData1;

    function CreateOneFormPage() {
        LODOP = getLodop();
        LODOP.PRINT_INIT("经营报表打印");

        var template = $.templates("#tpl-print");
        LODOP.SET_PREVIEW_WINDOW(2, 2, 0, -1, -1
        , '预览查看.开始打印');

        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "70%");

        var periodEle = $($('#selPeriod').find('option:selected')[0]);
        var periodTitle = $(periodEle).attr('data-title');


        var period = periodTitle;
        var unit = '元';
        var company = account;


        var data = {
            Company: company,
            Period: period,
            Unit: unit,
            Items: printData1

        };


        dataHtml = template.render(data);

        LODOP.ADD_PRINT_HTM(10, 10, '100%', '100%', dataHtml);


    };

    var LODOP;
    var html = "";//打印生成的HTML
    var nowPeriod = "";//期别
    $('#btnPrint').click(function () {

        //if (printData1 == undefined || printData1 == null) {
        //    $.warning('请先查出您要打印的数据');
        //    return;
        //}
        //CreateOneFormPage();
        //LODOP.PREVIEW();
        nowPeriod = $('#selYear').val() + "年" + $('#selMonth option:selected').text() + "期";
        var index = $.loading('打印获取数据');
        request = {};
        request.Token = token;
        request.PeriodId = $('#selMonth').val();

        $.Post("/fas/report/JYGet", request,
            function (data) {

                var res = data;
                if (res.IsSuccess) {
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("经营报表打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
                    GetJYHtml(data);
                    LODOP.PREVIEW();
                } else {

                }
                layer.close(index);
            }, function (err) {
                layer.close(index);
            });

    });

    //获取经营表打印页面
    function GetJYHtml(saveData) {
        if (saveData != undefined) {
            var htmlStyle =
                "<style>body {text-align: center;}.tbJY td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbJY tr {" +
                "height: 30px;" +
                "}" +

                ".tbJY thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbJY tbody, .tbJY tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "}</style>";
            var count = 0;
            //for (var i = 0; i < saveData.length; i++) {
            var htmlHead = "";
            //var htmlHead = "<table style='width: 70%;margin: 0 auto;'>" +
            //    "<tr style='height: 30px;'>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400;font-family:'黑体''>经营报表</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr  style='height: 30px;'>" +
            //    "<td style='font-size: 12px; width: 33.3333%;font-family:'黑体'' >编制单位：" + saveData.Account + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;font-family:'黑体''>" + nowPeriod + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right;font-family:'黑体''>单位：元</td>" +
            //    "</tr > " +
            //    "</table >";
            var htmlTbl =
                "<table class='tbJY' style='width: 70%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='4' style='font-size:20px;text-align:center;font-weight:400;border:0px'>经营报表</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td ' style='font-size: 12px;border:0px' >编制单位：" + saveData.Account  + "</td >" +
                "<td  style='font-size: 12px;text-align:center;border:0px'>" + nowPeriod + "</td>" +
                "<td colspan='2'   style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td style='width: 35%; text-align: center'>项目</td>" +
                "<td style='width: 35%; text-align: center'>科目</td >" +
                "<td style='width: 10%; text-align: center'>方向</td>" +
                "<td style='width: 20%; text-align: center'>金额</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail = "";

            if (saveData.KPMoney != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.KPMoney.Subject + "</td>" +
                    "<td></td>" +
                    "<td style='text-align: center'>" + saveData.KPMoney.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.KPMoney.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.KPMoneyYear != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.KPMoneyYear.Subject + "</td>" +
                    "<td></td>" +
                    "<td style='text-align: center'>" + saveData.KPMoneyYear.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.KPMoneyYear.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.BankDepositBal.length > 0) {
                $.each(saveData.BankDepositBal, function (index, obj) {
                    var head = "";
                    if (index == 0) {

                        head = obj.Item;
                    }

                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            if (saveData.ARAccountBal.length > 0) {
                $.each(saveData.ARAccountBal, function (index, obj) {
                    var head = "";
                    if (index == 0) {
                        head = obj.Item;
                    }
                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            if (saveData.KHMoney.length > 0) {
                $.each(saveData.KHMoney, function (index, obj) {
                    var head = "";
                    if (index == 0) {
                        head = obj.Item;
                    }
                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            if (saveData.APAccountBal.length > 0) {
                $.each(saveData.APAccountBal, function (index, obj) {
                    var head = "";
                    if (index == 0) {
                        head = obj.Item;
                    }
                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            if (saveData.PaySupplierMoney.length > 0) {
                $.each(saveData.PaySupplierMoney, function (index, obj) {
                    var head = "";
                    if (index == 0) {
                        head = obj.Item;
                    }
                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            if (saveData.CGSupplierMoney.length > 0) {
                $.each(saveData.CGSupplierMoney, function (index, obj) {
                    var head = "";
                    if (index == 0) {
                        head = obj.Item;
                    }
                    htmlDetail += "<tr>" +
                        "<td>" + head + "</td>" +
                        "<td>" + obj.Subject + "</td>" +
                        "<td style='text-align: center'>" + obj.Credit_Debit + "</td>" +
                        "<td style='text-align: right'>" + obj.Money + "</td>" +
                        "</tr>";
                });
            }
            htmlDetail += "</tbody>" +
                "<tfoot>" +
                "<tr>" +
                "<td   style='border: 0px solid #000000;'></td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                "<td style='text-align: right;border: 0px solid #000000;'>【章小算财税】</td>" +
                "</tr>" +
                "</tfoot></table>";
            //html += htmlHead + htmlDetail + htmlSum + htmlFoot;
            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle + htmlHead);
            LODOP.ADD_PRINT_TABLE(20, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
            LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);

        }
    }

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

})