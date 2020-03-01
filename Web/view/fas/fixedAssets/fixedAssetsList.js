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
            //return val.toString().replace('-', '');
            return val.toString();
        }
    });
    $.views.converters("assetsClass", function (val) {
        if(val=='001'){
            return '001 房屋、建筑物';
        }
        else if(val=='002'){
            return '002 机器机械生产设备';
        }
        else if(val=='003'){
            return '003 器具、工具、家具';
        }
        else if(val=='004'){
            return '004 运输工具';
        }
        else if(val=='005'){
            return ' 005 电子设备';
        }
        else {
            return '006 其他固定资产';
        }
    });

    $.views.converters("status", function (val) {
        if (val ==0) {
            return '正常';
        }
        else if (val == 1) {
            return '处置';
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
    var ActiveMonth;//激活的年份
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
                    ActiveMonth = arr[i].Month;
                }
            }
        }
        return arr;
    }
    var NowDate=''
    var init = function () {
        var index = $.loading('初始化');
        var request = {};

        request.Token = token;

        $.Post("/fas/period/PeriodPaidGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    account = res.Account.QY_Name;
                    var template_Year = $.templates("#tpl-Year");
                    var template_Month = $.templates("#tpl-Month");
                    allPeriod = res.Data;
                    var lstYear = GetAllYear(res.Data);
                    var dataHtml_Year = template_Year.render(lstYear);
                    $('#selYear').html(dataHtml_Year);

                    GetActiveYear(res.Data);//获取当前激活的年份

                    var lstMonth = GetMonth(res.Data);
                    NowDate = ActiveYear + "-" + ActiveMonth;
                    console.log(NowDate);
                    var dataHtml_Month = template_Month.render(lstMonth);
                    $('#selMonth').html(dataHtml_Month);
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });

    }
    var name = '';
    var account;
    init();
    var searchData = 0;
    var query = function (pageIndex) {
        searchData = 0;
        var index = $.loading('查询中');
        var request = {};
        request.PeriodDate = $('#selYear').val() + "-" + $('#selMonth option:selected').text();
        request.PeriodId = $('#selMonth').val();
        request.Name = $("#txtName").val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/FixedAssets/FixedAssetsListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    searchData = res.Data.length;

                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / 10),
                        jump: function (obj, first) {
                            if (!first) {
                                query(obj.curr);
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
    };
    
    window.query = query;
    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/FixedAssets/FixedAssetsDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1);
                    });

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };

    var rowDeal = function (id) {
        var index = $.loading('正在处置');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/FixedAssets/FixedAssetsDeal", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1);
                    });

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };

    query(1);
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
    //查询
    $(".search_btn").click(function () {

    
            query(1);
    })

    //添加
    $("#btnAdd").click(function () {
        //$.open("增加资产", "fixedAssetsAdd.aspx");
        $.open("增加资产", "fixedAssetsAdd.aspx?NowDate=" + NowDate, undefined, function (a, b) {

            query(1);

        });
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑资产', "fixedAssetsEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此资产？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

    $("body").on("click", ".tks-rowDeal", function () {  //删除
        var _this = $(this);
        $.confirm('确定处置此资产？', function () {


            rowDeal(_this.attr("data-id"));

        });
    })
    var Token = token;
    var LODOP;
    var html = "";//打印生成的HTML
    var nowPeriod = "";//期别
    //打印
    $("#print").click(function () {
        //if (searchData >0) {
        //    var url = "HtmlPrint.html?Token=" + Token + "&Name=" + $("#txtName").val();
        //    window.open(url);
           
        //}
        nowPeriod = $('#selYear').val() + "年" + $('#selMonth option:selected').text() + "期";
        var request = {};
        request.PeriodDate = $('#selYear').val() + "-" + $('#selMonth option:selected').text();
        request.PeriodId = $('#selMonth').val();
        request.Name = $("#txtName").val();
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 99999;
        var index = $.loading('打印获取数据');
        $.Post("/fas/FixedAssets/FixedAssetsListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    if (res.Data.length > 0) {
                        LODOP = getLodop();
                        LODOP.PRINT_INIT("固定资产打印");
                        //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                        LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
                        LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1);//横向时的正向显示
                        GetPrintHtml(res.Data);
                        LODOP.PREVIEW();
                    }
                } else {

                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
        
    })

    //获取打印页面
    function GetPrintHtml(saveData) {

        if (saveData != undefined && saveData.length > 0) {
            var htmlStyle =
                "<style>body {text-align: center;} .tbZZ td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbZZ tr {" +
                "height: 25px;" +
                "}" +

                ".tbZZ thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbZZ tbody, .tbZZ tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "}</style>";
            var htmlHead = "";
            //var htmlHead = "<table style='width: 80%;margin: 0 auto;'>" +
            //    "<tr style='height: 30px;'>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400'>固定资产</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr style='height: 30px; '>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + account + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;'>" + "" + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
            //    "</tr> " +
            //    "</table >";
            var htmlTbl = "<table class='tbZZ' style=';width:90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='9' style='font-size:20px;text-align:center;font-weight:400;border:0px'>固定资产</td>" +
                "</tr>" +

                "<tr style='height: 30px; '>" +
                "<td colspan='9' style='font-size: 12px;border:0px;text-align:center;' ><div style='float:left;width:40%;text-align:left;'>编制单位：" + account + "</div>" +
                nowPeriod +
                "<div style='float:right;width:40%;text-align:right;'>单位：元</div></td >" +
                "</tr > " +
                "<tr style='border: 1px solid #000000;'>" +
                "<td style='text-align: center; width: 10%'>资产编号</td>" +
                "<td style='text-align: center; width: 19%'> 资产名称</td >" +
                "<td style='text-align: center; width: 12%'> 开始使用日期</td > " +
                "<td style='text-align: center; width: 10%'> 预计月份</td > " +
                "<td style='text-align: center; width: 10%'> 资产原值</td > " +
                "<td style='text-align: center; width: 10%'> 资产净值</td > " +
                "<td style='text-align: center; width: 10%'> 累计折旧</td > " +
                "<td style='text-align: center; width: 11%'> 本月折旧</td > " +
                "<td style='text-align: center; width: 7%;'>残值率</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var SumInitialAssetValue = 0;
            var SumZCJZ = 0;
            var SumAccumulativeDpre = 0;
            var SumDprePerMonth = 0;
            var htmlDetail = "";
            for (var i = 0; i < saveData.length; i++) {
                SumInitialAssetValue += saveData[i].InitialAssetValue;
                SumZCJZ += saveData[i].ZCJZ;
                SumAccumulativeDpre += saveData[i].AccumulativeDpre;
                SumDprePerMonth += saveData[i].DprePerMonth;
                var DocNo = "";
                var Name = "";
                var StartUseDate = "";
                var PreUseMonth = "";
                var InitialAssetValue = "";
                var ZCJZ = "";
                var AccumulativeDpre = "";
                var DprePerMonth = "";
                var ScrapValueRate = "";
                try {
                    DocNo = saveData[i].DocNo;
                    Name = saveData[i].Name;
                    StartUseDate = YearMonthDay(saveData[i].StartUseDate);
                    PreUseMonth = saveData[i].PreUseMonth;
                    InitialAssetValue = saveData[i].InitialAssetValue;
                    ZCJZ = saveData[i].ZCJZ;
                    AccumulativeDpre = saveData[i].AccumulativeDpre;
                    DprePerMonth = saveData[i].DprePerMonth;
                    ScrapValueRate = saveData[i].ScrapValueRate;
                } catch (e) {
                    continue;
                }
                htmlDetail += "<tr style='border: 1px solid #000000;'>" +
                    "<td style='text-align: center'>" + DocNo + "</td>" +
                    "<td style='text-align: center'>" + Name + "</td >" +
                    "<td style='text-align: center'>" + StartUseDate + "</td>" +
                    "<td style='text-align: right'>" + PreUseMonth + "</td>" +
                    "<td style='text-align:right'>" + InitialAssetValue + "</td > " +
                    "<td style='text-align :right'>" + ZCJZ + "</td>" +
                    "<td style='text-align:right;'>" + AccumulativeDpre + " </td>" +
                    "<td style='text-align: right'>" + DprePerMonth + "</td>" +
                    "<td style='text-align: right;'>" + ScrapValueRate + "%</td>" +
                    "</tr>";
            }
            htmlDetail += "</tbody>" +
                "<tfoot >" +
                "<tr>" +
                "<td style='text-align:center;' colspan='4'>合计:</td>" +

                "<td style='text-align:right;'> " + Number(SumInitialAssetValue).toFixed(2) + " </td>" +
                "<td style='text-align:right;'> " + Number(SumZCJZ).toFixed(2) + " </td>" +
                "<td style='text-align:right;'> " + Number(SumAccumulativeDpre).toFixed(2) + " </td>" +
                "<td style='text-align:right;'> " + Number(SumDprePerMonth).toFixed(2) + " </td>" +
                "<td></td>" +
                "</tr>" +
                "<tr>" +
                "<td  colspan='5' style='border: 0px solid #000000;'></td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>【章小算财税】</td>" +
                "</tr>" +
                "</tfoot>" +
                "</table>";

            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle + htmlHead);
            LODOP.ADD_PRINT_TABLE(10, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
            LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
        }
    }



    function digitUppercase(n) {
        var fraction = ['角', '分'];
        var digit = [
            '零', '壹', '贰', '叁', '肆',
            '伍', '陆', '柒', '捌', '玖'
        ];
        var unit = [
            ['元', '万', '亿'],
            ['', '拾', '佰', '仟']
        ];
        var head = n < 0 ? '欠' : '';
        n = Math.abs(n);
        var s = '';
        for (var i = 0; i < fraction.length; i++) {
            s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
        }
        s = s || '整';
        n = Math.floor(n);
        for (var i = 0; i < unit[0].length && n > 0; i++) {
            var p = '';
            for (var j = 0; j < unit[1].length && n > 0; j++) {
                p = digit[n % 10] + unit[1][j] + p;
                n = Math.floor(n / 10);
            }
            s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
        }
        return head + s.replace(/(零.)*零元/, '元')
            .replace(/(零.)+/g, '零')
            .replace(/^整$/, '零元整');
    };

    function YearMonthDay(time) {
        if (time == null || time == undefined) return "";
        var t = time.split('T');
        if (t.length < 2) return time;
        var result = t[0];
        return result;
    }

    //当前日期
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

    function thousand(val) {
        if (val == 0 || val == "") {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
        }
    }

    $('#btnImport').click(function () {
        $.dialog('导入', 'attachment.aspx', undefined, function () {
            query(1);
        });

    });

})