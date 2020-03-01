layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    var printData//打印的数据
    , account //账套
        , num  //启用数量
        , allPZ
    , pageIndex = 1 //页码
    , periodId  //一般查询 期间
    , calItemId //一般查询 核算项
    , calValue  //一般查询 核算项内容
    , queryType = 'normal' //查询类型
    , _data  //更多  查询提交的数据
    , periodS  //更多 期间开始
    ,periodE; //更多 期间结束
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
        queryType = 'normal';
        var index = $.loading('计算中');
        var request = {};
        request.Type = queryType;
        request.PeriodId = periodId;
        request.CalculateItem = calItemId;
        request.CalculateValue = calValue;
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
        request.CalculateItem = _data.field.calItemMore;
        request.CalculateValue = _data.field.calValMore;
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
        $.Post("/fas/accountBook/CalcuAccountDetailListSearch", request,
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

                 $('#dt1').html(html1);
                 $('.layui-search-mored').hide();
                 printData = res.Data;


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

    var init = function () {

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
                    //    periodS = $('#selPeriodS').val();
                    //    periodE = $('#selPeriodE').val();
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
    };
 
    init();//初始化期间
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
        $('#selCalValMore').html('<option value="">核算项内容</option>');
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
        $('#selCalValMore').html('<option value="">核算项内容</option>');
        form.render('select');
    });
    form.on('select(selMonthS)', function (data) {
        initCalItem();
        $('#selCalValMore').html('<option value="">核算项内容</option>');
        form.render('select');
    });
    form.on('select(selMonthE)', function (data) {
        initCalItem();
        $('#selCalValMore').html('<option value="">核算项内容</option>');
        form.render('select');
    });
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
                    $('#selCalItemMore').html('<option value="">辅助核算项</option>');
                    $('#selCalItemMore').append(dataHtml);

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

    var initCalValue = function (calItemId, periodId) {
        var index = $.loading('初始化');
        var request = {};
        request.PeriodId = periodId;
        request.CalculateItemId = calItemId;
        request.Token = token;

        $.Post("/fas/accountBook/CalculateValuesGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-calValue");

                    var dataHtml = template.render(res.Data);
                    $('#selCalValue').html('<option value="">请选择</option>');
                    $('#selCalValue').append(dataHtml);


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

    //获取期间区间内的核算项内容
    var initCalValueInPeriod = function (itemId,ps,pe) {
        var index = $.loading('初始化');
        var request = {};
        request.Period_S = ps;
        request.Period_E = pe;
        request.CalculateItemId = itemId;
        request.Token = token;

        $.Post("/fas/accountBook/CalculateValuesGetInPeriod", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    var template = $.templates("#tpl-calValue");

                    var dataHtml = template.render(res.Data);
                    $('#selCalValMore').html('');
                    $('#selCalValMore').html('<option value="">核算项内容</option>');
                    $('#selCalValMore').append(dataHtml);
                    

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

    initCalItem();

    $("#btnSearch").click(function () {

        if (periodId === undefined) {
            $.warning('请选择期间');
            return;
        }
        if (calItemId === undefined) {
            $.warning('请选择核算项');
            return;
        }
        if (calValue === undefined) {
            $.warning('请选择核算项内容');
            return;
        }
        pageIndex = 1;
        query();

    })

 

    //更多 核算项
    form.on('select(calItemMore)', function (data) {
        var _calItem = data.value;
        //if (periodS != undefined)
        //    initCalValueInPeriod(_calItem, periodS, periodE); 
        initCalValueInPeriod(_calItem, $("#selMonthS").val(), $("#selMonthE").val()); 
        
        
    });

    //更多  开始期间
    form.on('select(periodS)', function (data) {
        periodS = data.value;
    });

    //更多  结束期间
    form.on('select(periodE)', function (data) {
        periodE = data.value;
    });
    //更多 查询
    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore();
        return false;
    });

    form.on('select(period)', function (data) {
        periodId = data.value;
    });

    form.on('select(calItem)', function (data) {
        calItemId = data.value;
        if (periodId != undefined)
            initCalValue(data.value, periodId);
    });

  
    form.on('select(calValue)', function (data) {
        calValue = data.value;
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
        LODOP.PRINT_INIT("核算项目明细账打印");

        var template = $.templates("#tpl-print");
        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:85%");

        var periodEle = $($('#selPeriod').find('option:selected')[0]);
        var periodTitle = $(periodEle).attr('data-title');
        var calItem = $($('#selCalItem').find('option:selected')[0]);


        var title = $(calItem).attr('data-title');

        var calVal = $($('#selCalValue').find('option:selected')[0]);
        var val = $(calVal).attr('data-title');

        var calItem = title + ":" + val;
        var period = periodTitle;
        var unit = '元';
        var company = account;
        var printDate = CurentTime();
        var items = printData;




        LODOP.NewPage();


        var data = {
            CalItem: calItem,

            Period: period,
            Unit: unit,
            Company: company,
            Items: items,
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
        request.CalculateItem = _data.field.calItemMore;
        request.CalculateValue = _data.field.calValMore;
        request.Type = queryType;//更多筛选
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 99999;
        request.allPZ = "";
        $.Post("/fas/accountBook/CalcuAccountDetailListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("核算项明细账打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
                    GetFZDetailHtml(data);
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
    //获取核算项明细账打印页面
    function GetFZDetailHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle =
                "<style> body {text-align: center;} .tbFZDetail td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbFZDetail tr {" +
                "height: 25px;" +
                "}" +

                ".tbFZDetail thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +
                ".textCenter{" +
                " margin: 0 auto;" +
                "text-align: center; " +
                "display: inline-block; " +
                "width: 40%; " +
                "height: 30px; " +
                "line-height: 30px; " +
                "float: left; " +

                "}" +
                ".tbFZDetail tbody, .tbFZDetail tfoot {" +
                "font-size: 13px;;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            var htmlHead = ""; 
            var headTitle = $('#selCalItemMore option:selected').text() + "：" + $('#selCalValMore option:selected').text()
            var htmlTbl = "<table class='tbFZDetail' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='7' style='font-size:20px;text-align:center;font-weight:400;border:0px'>核算项明细账</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td colspan='7' style='font-size: 12px;border:0px;text-align:center;' ><span style='float:left;width:30%;text-align:left;height:  30px;line-height: 30px;display:  inline-block;'>" + headTitle + "</span>" +
                "<span class='textCenter'>" + nowPeriod +"</span>"+
                "<span style='float:right;width:30%;text-align:right;height:  30px;line-height: 30px;display:  inline-block;'>单位：元</span></td >" +
                "</tr > " +
                "<tr>" +
                "<td style='width: 12%; text-align: center'>日期</td>" +
                "<td style='width: 10%; text-align: center'>凭证字号</td >" +
                "<td style='width: 23%; text-align: center'>摘要</td>" +
                "<td style='width: 15%; text-align: center'>借方金额</td>" +
                "<td style='width: 15%; text-align: center'>贷方金额</td>" +
                "<td style='width: 10%; text-align: center'>余额方向</td>" +
                "<td style='width: 15%; text-align: center'>余额金额</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var htmlDetail = "";
            for (var i = 0; i < saveData.Data.length; i++) {

                //var perid = saveData.PrintData[i].Year + "年" + saveData.PrintData[i].Month + "期";
                var FX = saveData.Data[i].Show_Credit_Debit==0?"借":"贷"
                htmlDetail += "<tr>" +
                    "<td style='text-align: left'>" + YearMonthDay(saveData.Data[i].DetailDate) + "</td>" +
                    "<td style='text-align: center'>" + saveData.Data[i].PZZ + "</td>" +
                    "<td style='text-align: left'>" + saveData.Data[i].Summary + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].Money1) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].Money2) + "</td>" +
                    "<td style='text-align: center'>" + FX+ "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].Show_Money) + "</td>" +
        
                    "</tr>";
                count++;



            }
            htmlDetail += "</tbody>" +
                "<tfoot >" +
                "<tr>" +
                "<td  colspan='3' style='text-align: left;border: 0px solid #000000;'>编制单位：" + account + "</td>" +
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
    function YearMonthDay(time) {
        if (time == null || time == undefined) return "";
        var t = time.split('T');
        if (t.length < 2) return time;
        var result = t[0];
        return result;
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