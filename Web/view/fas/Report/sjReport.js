layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage,
        $ = layui.jquery;

    $.views.converters("thousand", function (val) {

        if (val == undefined || val == null) {
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
        $.Post("/fas/report/SJGet", request,
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
        //var request = {};
        //request.Token = token;
        //var index = $.loading('账套获取中');
        //$.Post('/fas/set/opAccountListGet', request, function (data) {
        //    var res = data;
        //    layer.close(index);
        //    if (!res.IsSuccess) {
        //        $.warning(res.Message);
        //    }
        //    else {
        //        if (res.IsSelected) {
        //            $('#txtPeriod').html(res.Year + '年第' + res.Month + "期");
        //        }

        //        form.render();
        //    }


        //}, function (err) {
        //    $.warning(err.Message);
        //    layer.close(index);

        //});
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
        LODOP.PRINT_INIT("资产负债表打印");

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

        $.Post("/fas/report/SJGet", request,
            function (data) {

                var res = data;
                if (res.IsSuccess) {
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("税金报表打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
                    GetSJHtml(data);
                    LODOP.PREVIEW();
                } else {

                }
                layer.close(index);
            }, function (err) {
                layer.close(index);
            });

    });

    function GetSJHtml(saveData) {
        if (saveData != undefined) {
            var htmlStyle =
                "<style>body {text-align: center;}.tbSJ td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbSJ tr {" +
                "height: 35px;" +
                "}" +

                ".tbSJ thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbSJ tbody, .tbSJ tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "}</style>";
            var count = 0;
            //for (var i = 0; i < saveData.length; i++) {
            var htmlHead = "";
            //var htmlHead = "<table style='width: 70%;margin: 0 auto;'>" +
            //    "<tr style='height: 30px;'>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400'>税金表</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr  style='height: 30px;'>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + saveData.Account + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;'>" + nowPeriod + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
            //    "</tr > " +
            //    "</table >";
            var htmlTbl =
                "<table class='tbSJ' style='width: 70%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='3' style='font-size:20px;text-align:center;font-weight:400;border:0px'>税金表</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td ' style='font-size: 12px;border:0px' >编制单位：" + account + "</td >" +
                "<td  style='font-size: 12px;text-align:center;border:0px'>" + nowPeriod + "</td>" +
                "<td    style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td style='width: 50%; text-align: center'>项目</td>" +
                "<td style='width: 15%; text-align: center'>方向</td>" +
                "<td style='width: 35%; text-align: center'>金额</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail = "";

            if (saveData.XXTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.XXTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.XXTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.XXTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.UnCal_XXTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.UnCal_XXTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.UnCal_XXTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.UnCal_XXTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.XXTax_TOTAL != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.XXTax_TOTAL.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.XXTax_TOTAL.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.XXTax_TOTAL.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.JXTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.JXTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.JXTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.JXTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.UnCal_JXTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.UnCal_JXTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.UnCal_JXTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.UnCal_JXTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.JXTax_TOTAL != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.JXTax_TOTAL.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.JXTax_TOTAL.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.JXTax_TOTAL.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Pre_LiuDi != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Pre_LiuDi.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Pre_LiuDi.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Pre_LiuDi.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Pre_ZZTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Pre_ZZTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Pre_ZZTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Pre_ZZTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.LocalSJ != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.LocalSJ.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.LocalSJ.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.LocalSJ.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Cal_TotalSDTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Cal_TotalSDTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Cal_TotalSDTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Cal_TotalSDTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.TotalTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.TotalTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.TotalTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.TotalTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Quarter_VaTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Quarter_VaTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Quarter_VaTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Quarter_VaTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Quarter_LocalSJ != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Quarter_LocalSJ.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Quarter_LocalSJ.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Quarter_LocalSJ.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Quarter_TotalSDTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Quarter_TotalSDTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Quarter_TotalSDTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Quarter_TotalSDTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.TotalZZTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.TotalZZTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.TotalZZTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.TotalZZTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.TotalLocalSJ != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.TotalLocalSJ.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.TotalLocalSJ.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.TotalLocalSJ.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Total_Deliver_SDTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Total_Deliver_SDTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Total_Deliver_SDTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Total_Deliver_SDTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.TotalYearTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.TotalYearTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.TotalYearTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.TotalYearTax.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.Pre_SDTax != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.Pre_SDTax.Subject + "</td>" +
                    "<td style='text-align: center'>" + saveData.Pre_SDTax.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.Pre_SDTax.Money + "</td>" +
                    "</tr>";
            }



            htmlDetail += "</tbody>" +
                "<tfoot>" +
                "<tr>" +
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
            //}


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