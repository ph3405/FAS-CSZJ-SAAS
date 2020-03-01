
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender', 'jqExt', 'element', 'util'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage
        , element = layui.element()
        , util = layui.util;
    var $ = layui.jquery;

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
    /******初始化打印查询参数******/
    //凭证
    //var Type = "";
    //var PZZ = "";
    //var PZZ_S = "";
    //var PZZ_E = "";
    //var Period = "";
    //var Token = "";
    //var PageIndex = "";
    //var PageSize = "";
    //var Period_S = "";
    //var Period_E = "";
    //var MorePZZ = "";
    //var MorePZZ_S = "";
    //var MorePZZ_E = "";


    //初始化凭证字
    var pzzInit = function () {
        var request = {};
        request.Token = token;
        var index = $.loading('初始化中');
        $.Post("/fas/set/PZZTotalGet", request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    var template = $.templates("#tpl-pzz");

                    var dataHtml = template.render(res.Data);
                    $('#PZZ').append("<option value=''  selected>全</option>");
                    $('#PZZ').append(dataHtml);
                    form.render();


                }

            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });

    };

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
                    $('#selPeriod').html(dataHtml);

                    var template_Year = $.templates("#tpl-Year");
                    var template_Month = $.templates("#tpl-Month");
                    allPeriod = res.Data;
                    var lstYear = GetAllYear(res.Data);
                    var dataHtml_Year = template_Year.render(lstYear);
                    $('#selYear').html(dataHtml_Year);
                    $('#selYearS').html(dataHtml_Year);
                    $('#selYearE').html(dataHtml_Year);

                    $('#selYearS_henxiang').html(dataHtml_Year);
                    $('#selYearE_henxiang').html(dataHtml_Year);
                    GetActiveYear(res.Data);//获取当前激活的年份

                    var lstMonth = GetMonth(res.Data);

                    var dataHtml_Month = template_Month.render(lstMonth);
                    $('#selMonth').html(dataHtml_Month);
                    $('#selMonthS').html(dataHtml_Month);
                    $('#selMonthE').html(dataHtml_Month);

                    $('#selMonthS_henxiang').html(dataHtml_Month);
                    $('#selMonthE_henxiang').html(dataHtml_Month);

                    $('#selPeriodS').append(dataHtml);
                    $('#selPeriodE').append(dataHtml);



                    pzzInit();
                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
        $.Post('/fas/set/opAccountListGet', request, function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                var template = $.templates("#tpl-selectAccount");

                var dataHtml = template.render(res.Data);
                selAccount = res.SelectAccount.QY_Name;
                $('#selAccount').append(dataHtml);
                //if (res.IsSelected) {
                //    $('#txtPeriod').html(res.Year + '年第' + res.Month + "期");
                //}

                form.render();
            }


        }, function (err) {
            $.warning(err.Message);
            layer.close(index);

        });
    };

    form.render();
    Init();//初始化
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

    form.on('select(selYearS_henxiang)', function (data) {
        var year = $('#selYearS_henxiang').val();
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
        $('#selMonthS_henxiang').html(dataHtml_Month);
        form.render('select');
    });
    form.on('select(selYearE_henxiang)', function (data) {
        var year = $('#selYearE_henxiang').val();
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
        $('#selMonthE_henxiang').html(dataHtml_Month);
        form.render('select');
    });
    $('#SearchAccountList').click(function () {
        $.dialog("账套选择", '/view/fas/print/nodeChoose.aspx');

    });
    window.setValue = function (code, name) {
        $('#txtAccountId').val(code);
        $('#txtAccountDes').val(name);
        //alert(code);
    }
    form.on('checkbox', function (data) {
        //console.log(data.elem); //得到checkbox原始DOM对象
        //console.log(data.elem.checked); //是否被选中，true或者false
        //console.log(data.value); //复选框value值，也可以通过data.elem.value得到
        //console.log(data.othis); //得到美化后的DOM对象
        var name = data.elem.name;
        if (name == "PZ") {
            if (data.elem.checked) {
                $("#SearchPZ").show();

            }
            else {
                $("#SearchPZ").hide();
            }
        }
        if (name == "MX") {
            if (data.elem.checked) {
                $("#SearchMX").show();

            }
            else {
                $("#SearchMX").hide();
            }
        }
        if (name == "KM") {
            if (data.elem.checked) {
                $("#SearchKM").show();

            }
            else {
                $("#SearchKM").hide();
            }
        }

    });


    var Token = token;
    var PageIndex = 1;
    var PageSize = 99999;
    var arrType = new Array();
    var type_param = "";//勾选的打印项
    var Param = "";//打印时，查询数据的参数
    var htmlStyle = "";//打印生成的HTML样式
    var html = "";//打印生成的HTML
    var selAccount = "";//编制公司
    var nowPeriod = "";//期别

    var AreaIndex = 0;
    var request;
    var LODOP;
    //var Period_S = $('#selPeriodS').val();//开始期间
    //var Period_E = $('#selPeriodE').val();//结束期间
    var Period_S = $('#selMonthS').val();//开始期间
    var Period_E = $('#selMonthE').val();//结束期间
    $('#btnPrint_1').click(function () {
        //var index = $.loading('填充打印数据中');
        arrType = new Array();
        type_param = "";
        Param = "";
        LODOP = getLodop();
        LODOP.PRINT_INIT("批量打印");
        //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
        LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
        LODOP.SET_PRINT_STYLE("FontName", "黑体");
        $(".ckb_1").each(function () {
            if ($(this).is(":checked")) {

                var printType = $(this).attr("name");
                arrType.push(printType)
                GetPrintParam(printType,1);//获取各项打印查询参数

            }
        });
        LODOP.PREVIEW();
        //layer.close(index);
        if (arrType.length > 0) {
            for (var i = 0; i < arrType.length; i++) {
                if (i == 0) {
                    type_param += arrType[i];
                }
                else {
                    type_param += "," + arrType[i];
                }

            }
            //var Period_S = $('#selPeriodS').val();//开始期间
            //var Period_E = $('#selPeriodE').val();//结束期间
            var url = "HtmlPrint.html?arrCheckType=" + type_param + "&Token=" + Token + "&AccountList=" + $('#selAccount').val() + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&Period_S=" + Period_S + "&Period_E=" + Period_E + Param;
            //window.open(url);

        }

    });
    $('#btnPrint_2').click(function () {
        //var index = $.loading('填充打印数据中');
        arrType = new Array();
        type_param = "";
        Param = "";
        LODOP = getLodop();
        LODOP.PRINT_INIT("批量打印");
        LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
        LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
        $(".ckb_2").each(function () {
            if ($(this).is(":checked")) {

                var printType = $(this).attr("name");
                arrType.push(printType)
                GetPrintParam(printType,2);//获取各项打印查询参数

            }
        });
        LODOP.PREVIEW();
        //layer.close(index);
        if (arrType.length > 0) {
            for (var i = 0; i < arrType.length; i++) {
                if (i == 0) {
                    type_param += arrType[i];
                }
                else {
                    type_param += "," + arrType[i];
                }

            }
            //var Period_S = $('#selPeriodS').val();//开始期间
            //var Period_E = $('#selPeriodE').val();//结束期间
            var url = "HtmlPrint.html?arrCheckType=" + type_param + "&Token=" + Token + "&AccountList=" + $('#selAccount').val() + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&Period_S=" + Period_S + "&Period_E=" + Period_E + Param;
            //window.open(url);

        }

    });

    $('#btnPrint_henxiang').click(function () {
        //var index = $.loading('填充打印数据中');
        arrType = new Array();
        type_param = "";
        Param = "";
        LODOP = getLodop();
        LODOP.PRINT_INIT("批量打印");
        //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
        LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
        LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1);//横向时的正向显示
        LODOP.SET_PRINT_STYLE("FontName", "黑体");
        $(".ckb_henxiang").each(function () {
            if ($(this).is(":checked")) {

                var printType = $(this).attr("name");
                arrType.push(printType)
                GetPrintParam(printType, 1);//获取各项打印查询参数

            }
        });
        LODOP.PREVIEW();
        //layer.close(index);
        if (arrType.length > 0) {
            for (var i = 0; i < arrType.length; i++) {
                if (i == 0) {
                    type_param += arrType[i];
                }
                else {
                    type_param += "," + arrType[i];
                }

            }
            //var Period_S = $('#selPeriodS').val();//开始期间
            //var Period_E = $('#selPeriodE').val();//结束期间
            var url = "HtmlPrint.html?arrCheckType=" + type_param + "&Token=" + Token + "&AccountList=" + $('#selAccount').val() + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&Period_S=" + Period_S + "&Period_E=" + Period_E + Param;
            //window.open(url);

        }

    });
    //打印
    function GetPrintParam(PType,PeriodType) {
        var Period_S = $('#selMonthS').val();//开始期间
        var Period_E = $('#selMonthE').val();//结束期间

        var Period_S_henxiang = $('#selMonthS_henxiang').val();//开始期间
        var Period_E_henxiang = $('#selMonthE_henxiang').val();//结束期间

        var selPeriod = $('#selMonth').val();//单个期间
        if (PeriodType == 1) {


            if (Period_S == Period_E) {
                nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期";
            }
            else {
                nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期" + "-" + $('#selYearE').val() + "年" + $('#selMonthE option:selected').text() + "期"
            }
        }
        else {
            nowPeriod = $('#selYear').val() + "年" + $('#selMonth option:selected').text() + "期";
        }
        if (PType == "PZ") {
            //打印凭证
            var data = new Get_PZ_Select();

            request = {};
            request.Type = "more";
            request.Token = Token;
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            request.More = {};
            request.More.Period_S = Period_S;
            request.More.Period_E = Period_E;
            request.More.PZZ = data.MorePZZ;
            request.More.PZZ_S = data.MorePZZ_S;
            request.More.PZZ_E = data.MorePZZ_E;
            //request.AccountList = AccountList;
            $.PostTonBu("/fas/doc/DocListSearch", request,
                function (data) {
                    var res = data;
                    if (res.IsSuccess) {
                        //saveData = data;
                        LODOP.NEWPAGEA();
                        //LODOP.NewPage();
                        GetPZHtml(data);

                       
                        LODOP.ADD_PRINT_HTM('10mm', 0, '100%', '100%', html);
                    } else {

                    }
                }, function (err) {

                });
            Param += "&MorePZZ=" + data.MorePZZ + "&MorePZZ_S=" + data.MorePZZ_S + "&MorePZZ_E=" + data.MorePZZ_E;
        }
        if (PType == "PZFJ") {
            //打印凭证附件
            request = {};
            request.Token = Token;
            request.More = {};
            request.More.Period_S = Period_S;
            request.More.Period_E = Period_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/doc/DocAttachment", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {


                        if (data != undefined && data.ImgUrl.length > 0) {
                            var count = 0;
                            for (var i = 0; i < data.ImgUrl.length; i++) {
                                LODOP.NEWPAGE();
                                //LODOP.NEWPAGEA();
                                //LODOP.NewPage();
                                count++;
                                var pageHtml = "<p style='page-break-after: always;'></p>";
                                var url = "../../.." + data.ImgUrl[i];
                                //html += "<div><img src='" + url + "'/></div>";
                                LODOP.ADD_PRINT_IMAGE(0, 0, "100%", "100%", "<img src='" + url + "'/>");
                                if (count == 2) {
                                    //html += pageHtml;
                                    count = 0;
                                }
                            }


                            //window.close();

                        }
                        //html += "<p style='page-break-after: always;'></p>";
                    } else {

                    }
                }, function (err) {

                });

        }
        if (PType == "KM") {
            //打印科目余额(横向)
            var data = new Get_KM_Select();
            //Param += "&KM_MoreCode_S=" + data.MoreCode_S + "&KM_MoreCode_E=" + data.MoreCode_E;
            request = {};
            request.Type = "more";
            request.Token = Token;
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            request.More = {};
            request.More.Period_S = Period_S_henxiang;
            request.More.Period_E = Period_E_henxiang;
            request.More.Code_S = data.MoreCode_S;
            request.More.Code_E = data.MoreCode_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/accountBook/BalAccountListSearch", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {
                      
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetKMHtml(data);
                       
                       
                    } else {

                    }
                }, function (err) {

                });
            //LODOP.ADD_PRINT_TEXT(0, 0, 100, 20, "科目余额");
        }
        if (PType == "MX") {
            //打印明细账
            var data = new Get_MX_Select();
            //Param += "&MoreCode_S=" + data.MoreCode_S + "&MoreCode_E=" + data.MoreCode_E;

            request = {};
            request.Type = "more";
            request.Token = Token;
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            request.More = {};
            request.More.Period_S = Period_S;
            request.More.Period_E = Period_E;
            request.More.Code_S = data.MoreCode_S;
            request.More.Code_E = data.MoreCode_E;
            //request.AccountList = AccountList;
            $.PostTonBu("/fas/accountBook/DetailListSearch", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {
                       
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetMXHtml(data);
                       
                        
                    } else {

                    }
                }, function (err) {

                });
        }
        if (PType == "ZZ") {
            //打印总账
            var data = new Get_ZZ_Select();
            //Param += "&ZZ_MoreCode_S=" + data.MoreCode_S + "&ZZ_MoreCode_E=" + data.MoreCode_E;
            request = {};
            request.Type = "more";
            request.Token = Token;
            request.PageIndex = PageIndex;
            request.PageSize = PageSize;
            request.More = {};
            request.More.Period_S = Period_S;
            request.More.Period_E = Period_E;
            request.More.Code_S = data.MoreCode_S;
            request.More.Code_E = data.MoreCode_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/accountBook/GenAccountListSearch", request,
                function (data) {
                    var res = data;
                    if (res.IsSuccess) {
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetZZHtml(data);
                   
                    } else {

                    }
                }, function (err) {

                });
        }
        if (PType == "ZCFZ") {
            //打印资产负债表
            request = {};
            request.Token = Token;
            request.Period_S = selPeriod;
            //request.Period_E = Period_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/report/PrintZCFZ", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {
                       LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetZCFZHtml(data);
                
                    } else {

                    }
                }, function (err) {

                });
        }
        if (PType == "LR") {
            //打印利润表
            request = {};
            request.Token = Token;
            request.PeriodId = selPeriod;
            //request.Period_E = Period_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/report/LRGet", request,
                function (data) {
                    var res = data;
                    if (res.IsSuccess) {
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetLRHtml(data);
                        //html += "<p style='page-break-after: always;'></p>";
                    } else {

                    }
                }, function (err) {

                });
        }
        if (PType == "JY") {
            //打印经营报表
            request = {};
            request.Token = Token;
            request.PeriodId = selPeriod;
            //request.Period_E = Period_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/report/JYGet", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetJYHtml(data);
                      
                    } else {

                    }
                }, function (err) {

                });
        }
        if (PType == "SJ") {
            //打印税金报表
            request = {};
            request.Token = Token;
            request.PeriodId = selPeriod;
            //request.Period_E = Period_E;
            //request.AccountList = arrAccount[0];
            $.PostTonBu("/fas/report/SJGet", request,
                function (data) {

                    var res = data;
                    if (res.IsSuccess) {
                        LODOP.NEWPAGEA();
                        //LODOP.NEWPAGE();
                        GetSJHtml(data);
                    } else {

                    }
                }, function (err) {

                });
        }

    }
    //获取【凭证】打印时，查询参数
    var Get_PZ_Select = function () {
        this.MorePZZ = $('#PZZ').val();//凭证字
        this.MorePZZ_S = $("[name='pzzS']").val();//凭证开始号
        this.MorePZZ_E = $("[name='pzzE']").val();//凭证结束号
    }
    //获取【明细账】打印时，查询参数
    var Get_MX_Select = function () {
        this.MoreCode_S = $("[name='codeS']").val();//科目起始编码
        this.MoreCode_E = $("[name='codeE']").val();//科目结束编码
    }
    //获取【科目余额】打印时，查询参数
    var Get_KM_Select = function () {
        this.MoreCode_S = $("[name='KM_codeS']").val();//科目起始编码
        this.MoreCode_E = $("[name='KM_codeE']").val();//科目结束编码
    }
    //获取【总账】打印时，查询参数
    var Get_ZZ_Select = function () {
        this.MoreCode_S = $("[name='ZZ_codeS']").val();//科目起始编码
        this.MoreCode_E = $("[name='ZZ_codeE']").val();//科目结束编码
    }
    function AllHide() { }


    //获取凭证打印页面
    function GetPZHtml(saveData) {
        //var row_index = 17;
        if (saveData != undefined && saveData.Data.length > 0) {
            var htmlStyle =
                "<style> body {text-align: center;}  .tbPZ td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "padding: 2px;" +
                "letter-spacing: 1px;" +
                "}" +

                ".tbPZ tr {" +
                "height: 55px;" +
                "}" +

                ".tbPZ thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbPZ tbody, .tbPZ tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "}" +
                ".A4 {   " +
                " page -break-before: auto; " +
                "page -break-after: always; " +
                "} </style>";
            var count = 0;
            for (var i = 0; i < saveData.Data.length; i++) {

                var AMT_DBT = digitUppercase(saveData.Data[i].Head["AMT_DBT"]);
                //var AMT_DBT = saveData.Data[i].Head["AMT_DBT"];
                var htmlHead = "<table style='width:600px;margin: 0 auto;'>" +
                    "<tr>" +
                    "<td style='width:33.3333%'></td>" +
                    "<td style='font-size:20px;text-align:center;font-weight:400'> " + saveData.Data[i].Head["PZZName"] + "账凭证</td>" +
                    "<td style='font-size:12px;width:33.3333%;text-align:right'>附单据数：" + saveData.Data[i].Head["AttachmentCount"] + "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td style='font-size:12px;width:33.3333%'>核算单位：" + saveData.AccountName + "</td>" +
                    "<td style='font-size:12px;text-align:center;'>日期：" + saveData.Data[i].Head["PZDate"].split('T')[0] + "</td>" +
                    "<td style='font-size:12px;width:33.3333%;text-align:right'>凭证号：" + saveData.Data[i].Head["PZZName"] + " - " + saveData.Data[i].Head["PZZNO"] + "</td>" +
                    "</tr>" +
                    "</table >" +
                    "<table class='tbPZ' style='border: 1px solid #000000;width:600px;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                    "<thead style='display: table-header-group'>" +
                    "<tr>" +
                    "<td style='text-align:center;width:30%'>摘要</td>" +
                    "<td style='text-align :center;width:30%'>科目</td>" +
                    "<td style='text-align:center;width:20%;'>借方金额</td>" +
                    "<td style='text-align:center;width:20%;border-right:none'>贷方金额</td>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
                var htmlDetail = "";
                var htmlSum = "</tbody>" +
                    "<tfoot  style='display: table-footer-group'>" +
                    "<tr>" +
                    "<td style='text-align:left;border-bottom:none' colspan='2'>合计:" + AMT_DBT + "</td>" +

                    "<td style='text-align:right;padding-right:10px;border-bottom:none'> " + saveData.Data[i].Head["AMT_DBT"] + " </td>" +
                    "<td style='text-align:right;padding-right:10px;border:none'> " + saveData.Data[i].Head["AMT_DBT"] + " </td>" +
                    "</tr>" +
                    "</tfoot>" +

                    "</table>";

                var htmlFoot = "<table style='width:600px;margin: 0 auto;font-size:12px;height:40px;'>" +
                    "<tr>" +
                    "<td style='width:20%'>主管：</td>" +
                    "<td style='width:20%'>记账：</td>" +
                    "<td style='width:20%'>审核：</td>" +
                    "<td style='width:20%'>出纳：</td>" +
                    "<td style='width:20%'>制单： " + saveData.Data[i].Head.CreateUser + "</td>" +
                    "</tr>" +
                    "</table>" +
                    "<div style='height:1px'></div>";
                //row_index = row_index - 3;
                //var flag = 0;
                //var row = 0;
                var index = 0;

                //var a = 1 % 2;//1
                //var b = 5 % 2;//1
                //var c = 6 % 2;//0
                //var aa = Math.ceil(1 / 2);//1
                //var bb = Math.ceil(5 / 2);//3
                //var cc = Math.ceil(13 / 2);//7
                var Area = Math.ceil(saveData.Data[i].Detail.length / 5);
                for (var a = 0; a < Area; a++) {

                    if (Area > 1) {
                        var page = a + 1 + "/" + Area;
                        htmlHead = "<table style='width:600px;margin: 0 auto;'>" +
                            "<tr>" +
                            "<td style='width:33.3333%'></td>" +
                            "<td style='font-size:20px;text-align:center;font-weight:400'> " + saveData.Data[i].Head["PZZName"] + "账凭证</td>" +
                            "<td style='font-size:12px;width:33.3333%;text-align:right'>附单据数：" + saveData.Data[i].Head["AttachmentCount"] + "</td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td style='font-size:12px;width:33.3333%'>核算单位：" + saveData.AccountName + "</td>" +
                            "<td style='font-size:12px;text-align:center;'>日期：" + saveData.Data[i].Head["PZDate"].split('T')[0] + "</td>" +
                            "<td style='font-size:12px;width:33.3333%;text-align:right'>凭证号：" + saveData.Data[i].Head["PZZName"] + " - " + saveData.Data[i].Head["PZZNO"] + "(" + page + ")" + "</td>" +
                            "</tr>" +
                            "</table >" +
                            "<table class='tbPZ' style=' border: 1px solid #000000;width:600px;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                            "<thead style='display: table-header-group'>" +
                            "<tr>" +
                            "<td style='text-align:center;width:30%'>摘要</td>" +
                            "<td style='text-align :center;width:30%'>科目</td>" +
                            "<td style='text-align:center;width:20%;'>借方金额</td>" +
                            "<td style='text-align:center;width:20%;border-right:none'>贷方金额</td>" +
                            "</tr>" +
                            "</thead>" +
                            "<tbody>";
                    }
                    AreaIndex++;
                    htmlDetail = "";
                    for (var j = 0; j < 5; j++) {
                        var Summary = "";
                        var SubjectDescription = "";
                        var Money_Debit = "";
                        var Money_Credit = "";
                        try {
                            Summary = saveData.Data[i].Detail[j + index].Summary;
                            SubjectDescription = saveData.Data[i].Detail[j + index].SubjectDescription;
                            Money_Debit = saveData.Data[i].Detail[j + index].Money_Debit;
                            Money_Credit = saveData.Data[i].Detail[j + index].Money_Credit;
                        } catch (e) {

                        }
                        htmlDetail += "<tr>" +
                            "<td style='text-align:left'>" + Summary + "</td > " +
                            "<td style='text-align :left'>" + SubjectDescription + "</td>" +
                            "<td style='text-align:right;padding-right:10px;'>" + Money_Debit + " </td>" +
                            "<td style='text-align:right;padding-right:10px;border-right:none'>" + Money_Credit + "</td>" +
                            "</tr>";


                    }
                    html += htmlStyle + htmlHead + htmlDetail + htmlSum + htmlFoot;

                    if (AreaIndex == 2) {
                        AreaIndex = 0;
                        //if (Area > 1 && a <= Area - 1) {
                        //    html += "<p style='page-break-after: always;'></p>";
                        //}
                        //else if (Area==1) {
                        //    html += "<p style='page-break-after: always;'></p>";
                        //}
                        //html += "<p style='page-break-after: always;'></p>";
                        //LODOP.NewPage();
                        
                        LODOP.ADD_PRINT_HTM('10mm', 0, '100%', '100%', html);
                        LODOP.NEWPAGEA();
                        html = "";
                    }
                    else {
                        html += "<p style='height: 20px;'></p>";
                    }
                    index = index + 5;

                }


            }

        }
    }

    //获取科目余额打印页面
    function GetKMHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle =
                "<style> body {text-align: center;} .tbKM td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbKM tr {" +
                "height: 25px;" +
                "}" +

                ".tbKM thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbKM tbody, .tbKM tfoot {" +
                "font-size: 13px;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            var htmlHead = "";
            //var htmlHead = "<table style='width: 95%;margin: 0 auto;'>" +
            //    "<tr>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400'>科目余额表</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr style='height: 30px; '>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + selAccount + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;'>" + nowPeriod + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
            //    "</tr> " +
            //    "</table>";

            var htmlTbl = "<table class='tbKM' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='10' style='font-size:20px;text-align:center;font-weight:400;border:0px'>科目余额表</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td colspan='2' style='font-size: 12px;border:0px' >编制单位：" + selAccount + "</td >" +
                "<td colspan='4' style='font-size: 12px;text-align:center;border:0px'>" + nowPeriod + "</td>" +
                "<td  colspan='4'  style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td rowspan='2' style='text-align: center; width: 10%' >科目编码</td>" +
                "<td rowspan='2' style='text-align: center; width: 26%' >科目名称</td > " +
                "<td colspan='2' style='text-align: center; width: 16%' >期初余额</td > " +
                "<td colspan='2' style='text-align: center; width: 16%' >本期发生额</td > " +
                "<td colspan='2' style='text-align: center; width: 16%' >本年累计发生额</td > " +
                "<td colspan='2' style='text-align: center; width: 16%' >期末余额</td > " +
                "</tr > " +
                "<tr>" +
                "<td style='width: 8%; text-align: center'>借方</td>" +
                "<td style='width: 8%; text-align: center'>贷方</td >" +
                "<td style='width: 8%; text-align: center'>借方</td>" +
                "<td style='width: 8%; text-align: center'>贷方</td>" +
                "<td style='width: 8%; text-align: center'>借方</td>" +
                "<td style='width: 8%; text-align: center'>贷方</td>" +
                "<td style='width: 8%; text-align: center'>借方</td>" +
                "<td style='width: 8%; text-align: center'>贷方</td>" +
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
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWB_YJ) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWB_YD) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.Data[i].BWBEnd_J) + "</td>" +
                    "<td style='text-align: right;'>" + noZero(saveData.Data[i].BWBEnd_D) + "</td>" +
                    "</tr>";
                count++;



            }
            htmlDetail += "</tbody>" +
                "<tfoot >" +
                "<tr>" +
                "<td  colspan='6' style='border: 0px solid #000000;'></td>" +
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

    //获取明细账打印页面
    function GetMXHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            //html = "";
            var htmlStyle_MX =
                "<style> body {text-align: center;} .tbMX td {" +
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
                "font-size: 12px; font-family:'黑体'" +
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
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + selAccount + "</td >" +
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
                "<td colspan='3' style='font-size: 12px;border:0px' >编制单位：" + selAccount + "</td >" +
                "<td style='font-size: 12px;border:0px'>" + nowPeriod + "</td>" +
                "<td  colspan='4'  style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td style='text-align: center; width: 10%'>日期</td>" +
                "<td style='text-align: center; width: 10%'>凭证字</td>" +
                "<td style='text-align: center; width: 25%'>科目</td>" +
                "<td style='text-align: center; width: 20%'>摘要</td>" +
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
                var Show_Credit_Debit = "";
                var Show_Money = "";
                var DetailDate = "";
                var PZZ = "";
                var Name = "";
                var Summary = "";
                try {
                    Money1 = saveData.Data[i].Money1 == 0 ? "" : saveData.Data[i].Money1;
                    Money2 = saveData.Data[i].Money2 == 0 ? "" : saveData.Data[i].Money2;
                    Show_Credit_Debit = saveData.Data[i].Show_Credit_Debit == 0 ? "借" : "贷";
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

    //获取总账打印页面
    function GetZZHtml(saveData) {
        if (saveData != undefined && saveData.Data.length > 0) {
            var htmlStyle =
                "<style> body {text-align: center;} .tbZZ td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbZZ tr {" +
                "height: 30px;" +
                "}" +

                ".tbZZ thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbZZ tbody, .tbZZ tfoot {" +
                "font-size: 12px; font-family:'黑体'" +
                "}</style>";
            var count = 0;
            for (var i = 0; i < saveData.Data.length; i++) {
                var htmlHead = "";
                //var htmlHead = "<table style='width: 95%;margin: 0 auto;'>" +
                //    "<tr style='height: 30px;'>" +
                //    "<td  colspan='3' style='font-size:20px;text-align:center;font-weight:400'> " + saveData.Data[i].Name + "总账</td>" +
                //    "</tr>" +
                //    "<tr style='height: 30px;'>" +
                //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + selAccount + "</td >" +
                //    "<td style='font-size:12px;text-align:center;'>" + nowPeriod + "</td>" +
                //    "<td style='font-size:12px;width:33.3333%;text-align:right'>单位：元</td>" +
                //    "</tr>" +
                //    "</table >";
                var htmlTbl =
                    "<table class='tbZZ' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                    "<thead>" +
                    "<tr style='height: 30px;border:0px'>" +
                    "<td colspan='8' style='font-size:20px;text-align:center;font-weight:400;border:0px'>" + saveData.Data[i].Name + "总账</td>" +
                    "</tr>" +
                    "<tr style='height: 30px; '>" +
                    "<td colspan='2' style='font-size: 12px;border:0px' >编制单位：" + selAccount + "</td >" +
                    "<td colspan='2' style='font-size:text-align:center; 12px;border:0px'>" + nowPeriod + "</td>" +
                    "<td  colspan='4'  style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                    "</tr > " +
                    "<tr>" +
                    "<td style='text-align: center; width: 10%'>科目编码</td>" +
                    "<td style='text-align: center; width: 29%'> 科目名称</td >" +
                    "<td style='text-align: center; width: 11%'> 期间</td > " +
                    "<td style='text-align: center; width: 15%'> 摘要</td > " +
                    "<td style='text-align: center; width: 10%'> 借方金额</td > " +
                    "<td style='text-align: center; width: 10%'> 贷方金额</td > " +
                    "<td style='text-align: center; width: 5%'> 方向</td > " +
                    "<td style='text-align: center; width: 10%;'>余额</td>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
                var htmlDetail = "";
                for (var j = 0; j < saveData.Data[i].Data.length; j++) {
                    var Money1 = saveData.Data[i].Data[j].Money1 == 0 ? "" : saveData.Data[i].Data[j].Money1;
                    var Money2 = saveData.Data[i].Data[j].Money2 == 0 ? "" : saveData.Data[i].Data[j].Money2;
                    var Show_Credit_Debit = saveData.Data[i].Data[j].Show_Credit_Debit == 0 ? "借" : "贷";
                    var Show_Money = saveData.Data[i].Data[j].Show_Money == 0 ? "" : saveData.Data[i].Data[j].Show_Money;
                    htmlDetail += "<tr>" +
                        "<td style='text-align: center'>" + saveData.Data[i].Data[j].SubjectCode + "</td>" +
                        "<td style='text-align: center'>" + saveData.Data[i].Data[j].Name + "</td >" +
                        "<td style='text-align: center'>" + saveData.Data[i].Data[j].Period + "</td>" +
                        "<td style='text-align:left'>" + saveData.Data[i].Data[j].Summary + "</td > " +
                        "<td style='text-align :right'>" + Money1 + "</td>" +
                        "<td style='text-align:right;'>" + Money2 + " </td>" +
                        "<td style='text-align: center'>" + Show_Credit_Debit + "</td>" +
                        "<td style='text-align: right;'>" + Show_Money + "</td>" +
                        "</tr>";
                }
                htmlDetail += "</tbody>" +
                    "<tfoot>" +
                    
                    "<tr>" +
                    "<td  colspan='4' style='border: 0px solid #000000;'></td>" +
                    "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                    "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>【章小算财税】</td>" +

                    "</tfoot></table>";
                count++;
                //var pageHtml = "<p style='page-break-after: always;'></p>";
                //html += htmlHead + htmlDetail + htmlSum + htmlFoot;
                //LODOP.NEWPAGEA();
                //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);

               
                LODOP.ADD_PRINT_TABLE(10, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);

                LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
                LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
                LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
                if (saveData.Data.length > 1 && i < saveData.Data.length - 1) {
                    //html += "<p style='page-break-after: always;'></p>";
                    LODOP.NEWPAGE();
                }
            }
        }
    }

    //获取资产负债表打印页面
    function GetZCFZHtml(saveData) {
        if (saveData != undefined && saveData.PrintData.length > 0) {
            var htmlStyle =
                "<style> body {text-align: center;}.tbzcfz td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbzcfz tr {" +
                "height: 25px;" +
                "}" +

                ".tbzcfz thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbzcfz tbody, .tbzcfz tfoot {" +
                "font-size: 12px; font-family:'黑体'" +
                "}</style>";
            var count = 0;
            var printData1 = new Array();
            var printData2 = new Array();
            for (var i = 0; i < saveData.PrintData.length; i++) {
                var printData1 = new Array();
                var printData2 = new Array();
                printData1 = printData1.concat(saveData.PrintData[i].Data.LDZC);
                printData1 = printData1.concat(saveData.PrintData[i].Data.FLDZC);
                printData1 = printData1.concat(saveData.PrintData[i].Data.ZCHJ);
                printData2 = printData2.concat(saveData.PrintData[i].Data.LDFZ);
                printData2 = printData2.concat(saveData.PrintData[i].Data.FLDFZ);
                printData2 = printData2.concat(saveData.PrintData[i].Data.FZHJ);
                printData2 = printData2.concat(saveData.PrintData[i].Data.SYZQY);
                var len2 = saveData.PrintData[i].Data.LDFZ.length + saveData.PrintData[i].Data.FLDFZ.length + saveData.PrintData[i].Data.FZHJ.length + saveData.PrintData[i].Data.SYZQY.length;
                if (printData1.length > len2) {
                    for (var l = 0; l < printData1.length - len2 - saveData.PrintData[i].Data.SYZQYHJ.length; l++) {
                        printData2 = printData2.concat({
                            AccountId: null
                            , Category: 0
                            , ColumnName: ""
                            , EndBalance: 0
                            , Id: null
                            , OperatorCharacter: null
                            , ParentId: null
                            , PeriodId: null
                            , Seq: 0
                            , SourceType: 3
                            , SourceValue: null
                            , YearStartBalance: 0
                        });
                    }
                }
                printData2 = printData2.concat(saveData.PrintData[i].Data.SYZQYHJ);
                var perid = saveData.PrintData[i].Year + "年" + saveData.PrintData[i].Month + "期";
                var htmlHead = "";
                //var htmlHead = "<table style='width: 85%;margin: 0 auto;'>" +
                //    "<tr style='height: 25px;'>" +
                //    "<td style='width:33.3333%'></td>" +
                //    "<td style='font-size:20px;text-align:center;font-weight:400'>资产负债表</td>" +
                //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
                //    "</tr>" +
                //    "<tr style='height: 15px;'>" +
                //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + selAccount + "</td >" +
                //    "<td style='font-size:12px;text-align:center;'>" + perid + "</td>" +
                //    "<td style='font-size:12px;width:33.3333%;text-align:right'>单位：元</td>" +
                //    "</tr>" +
                //    "</table >";
                var htmlTbl =
                    "<table class='tbzcfz' style='width: 90%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                    "<thead>" +
                    "<tr style='height: 30px;border:0px'>" +
                    "<td colspan='8' style='font-size:20px;text-align:center;font-weight:400;border:0px'>资产负债表</td>" +
                    "</tr>" +
                    "<tr style='height: 30px; '>" +
                    "<td colspan='3' style='font-size: 12px;border:0px' >编制单位：" + selAccount + "</td >" +
                    "<td colspan='2' style='font-size: 12px;text-align:center;border:0px'>" + perid + "</td>" +
                    "<td  colspan='3'  style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                    "</tr > " +
                    "<tr>" +
                    "<td style='text-align: center; width: 20%'>资产</td>" +
                    "<td style='text-align: center; width: 8%'>行次</td >" +
                    "<td style='text-align: center; width: 10%'>期末余额</td > " +
                    "<td style='text-align: center; width: 10%'>年初余额</td > " +
                    "<td style='text-align: center; width: 24%'>负债和所有者权益</td > " +
                    "<td style='text-align: center; width: 8%'>行次</td > " +
                    "<td style='text-align: center; width: 10%'>期末余额</td > " +
                    "<td style='text-align: center; width: 10%;'>年初余额</td>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
                var htmlDetail = "";

                for (var j = 0; j < printData1.length; j++) {
                    var weight1 = "<b>" + printData1[j].ColumnName + "</b>";
                    var small1 = "&nbsp;&nbsp;" + printData1[j].ColumnName;
                    var head1 = printData1[j].Seq == 0 ? weight1 : small1;
                    var Seq1 = printData1[j].Seq != 0 ? printData1[j].Seq : "";
                    var EndBalance1 = printData1[j].EndBalance != 0 ? printData1[j].EndBalance : "";
                    var YearStartBalance1 = printData1[j].YearStartBalance != 0 ? printData1[j].YearStartBalance : "";
                    var html1 = "<td style='text-align: left;'>" + head1 + "</td >" +
                        "<td style='text-align: center'>" + Seq1 + "</td>" +
                        "<td style='text-align: right'>" + EndBalance1 + "</td>" +
                        "<td style='text-align: right;'>" + YearStartBalance1 + "</td>";

                    var weight2 = "<b>" + printData2[j].ColumnName + "</b>";
                    var small2 = "&nbsp;&nbsp;" + printData2[j].ColumnName;
                    var head2 = printData2[j].Seq == 0 ? weight2 : small2;
                    var Seq2 = printData2[j].Seq != 0 ? printData2[j].Seq : "";
                    var EndBalance2 = printData2[j].EndBalance != 0 ? printData2[j].EndBalance : "";
                    var YearStartBalance2 = printData2[j].YearStartBalance != 0 ? printData2[j].YearStartBalance : "";
                    var html2 = "<td style='text-align: left;'>" + head2 + "</td >" +
                        "<td style='text-align: center'>" + Seq2 + "</td>" +
                        "<td style='text-align: right'>" + EndBalance2 + "</td>" +
                        "<td style='text-align: right;'>" + YearStartBalance2 + "</td>";
                    htmlDetail += "<tr>" + html1 + html2 + "</tr>";
                }
                htmlDetail += "</tbody>" +
                    "<tfoot>" +
                    "<tr>" +
                    "<td  colspan='4' style='border: 0px solid #000000;'></td>" +
                    "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>打印日期：" + CurentTime() + "</td>" +
                    "<td style='text-align: right;border: 0px solid #000000;' colspan='2'>【章小算财税】</td>" +
                    "</tr>" +
                    "</tfoot></table>";
                count++;

                //html += htmlHead + htmlDetail + htmlSum + htmlFoot;
                //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);
                LODOP.ADD_PRINT_TABLE(20, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
                LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
                LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
                LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
            }
        }
    }

    //获取利润表打印页面
    function GetLRHtml(saveData) {
        if (saveData != undefined && saveData.PrintData.length > 0) {
            var htmlStyle =
                "<style> body {text-align: center;}.tbLR td {" +
                "border-right: 1px solid #808080;" +
                "border-bottom: 1px solid #808080;" +
                "border-top: 1px solid #808080;" +
                "border-left: 1px solid #808080;" +
                "}" +

                ".tbLR tr {" +
                "height: 40px;" +
                "}" +

                ".tbLR thead {" +
                " font-size: 14px;font-family:'黑体'" +
                "}" +

                ".tbLR tbody, .tbLR tfoot {" +
                "font-size: 12px;font-family:'黑体' " +
                "}</style>";
            var count = 0;
            for (var i = 0; i < saveData.PrintData.length; i++) {
                var perid = saveData.PrintData[i].Year + "年" + saveData.PrintData[i].Month + "期";
                var htmlHead = "";
                //var htmlHead = "<table style='width: 70%;margin: 0 auto;'>" +
                //    "<tr style='height: 30px;'>" +
                //    "<td style='width:33.3333%'></td>" +
                //    "<td style='font-size:20px;text-align:center;font-weight:400'>利润表</td>" +
                //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
                //    "</tr>" +
                //    "<tr  style='height: 30px;'>" +
                //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + selAccount + "</td >" +
                //    "<td style='font-size: 12px; text-align: center;'>" + perid + "</td>" +
                //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
                //    "</tr > " +
                //    "</table >";
                var htmlTbl =
                    "<table class='tbLR' style='width:70%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                    "<thead>" +
                    "<tr style='height: 30px;border:0px'>" +
                    "<td colspan='4' style='font-size:20px;text-align:center;font-weight:400;border:0px'>利润表</td>" +
                    "</tr>" +
                    "<tr style='height: 30px; '>" +
                    "<td  style='font-size: 12px;border:0px' >编制单位：" + selAccount + "</td >" +
                    "<td colspan='2' style='font-size: 12px;text-align:center;border:0px'>" + perid + "</td>" +
                    "<td    style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                    "</tr > " +
                    "<tr>" +
                    "<td style='width: 30%; text-align: center'>项目</td>" +
                    "<td style='width: 10%; text-align: center'>行次</td >" +
                    "<td style='width: 30%; text-align: center'>本年累计金额</td>" +
                    "<td style='width: 30%; text-align: center'>本期金额</td>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
                var htmlDetail = "";
                for (var j = 0; j < saveData.PrintData[i].Data.length; j++) {
                    var Seq = saveData.PrintData[i].Data[j].Seq != 0 ? saveData.PrintData[i].Data[j].Seq : "";
                    var weight = "<b>" + saveData.PrintData[i].Data[j].ColumnName + "</b>";
                    var small = "&nbsp;&nbsp;" + saveData.PrintData[i].Data[j].ColumnName;
                    var head = small;
                    if (Seq == 21 || Seq == 30 || Seq == 32) {
                        head = weight;
                    }
                    var Money_Year = saveData.PrintData[i].Data[j].Money_Year != 0 ? saveData.PrintData[i].Data[j].Money_Year : "";
                    var Money_Month = saveData.PrintData[i].Data[j].Money_Month != 0 ? saveData.PrintData[i].Data[j].Money_Month : "";
                    htmlDetail += "<tr>" +
                        "<td style='text-align: left'>" + head + "</td>" +
                        "<td style='text-align: center'>" + Seq + "</td>" +
                        "<td style='text-align: right'>" + Money_Year + "</td>" +
                        "<td style='text-align: right'>" + Money_Month + "</td>" +
                        "</tr>";
                    count++;
                }
                htmlDetail += "</tbody>" +
                    "<tfoot>" +
                    "<tr>" +
                    "<td  colspan='2' style='border: 0px solid #000000;'></td>" +
                    "<td style='text-align: right;border: 0px solid #000000;'>打印日期：" + CurentTime() + "</td>" +
                    "<td style='text-align: right;border: 0px solid #000000;'>【章小算财税】</td>" +
                    "</tr>" +
                    "</tfoot></table>";
                //html += htmlHead + htmlDetail + htmlSum + htmlFoot;
                //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);
                LODOP.ADD_PRINT_TABLE(20, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
                LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
                LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
                LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);
            }


        }
    }

    //获取经营表打印页面
    function GetJYHtml(saveData) {
        if (saveData != undefined) {
            var htmlStyle =
                "<style> body {text-align: center;}.tbJY td {" +
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
            //    "<td style='font-size:20px;text-align:center;font-weight:400'>经营报表</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr  style='height: 30px;'>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + saveData.Account + "</td >" +
            //    "<td style='font-size: 12px; text-align: center;'>" + nowPeriod + "</td>" +
            //    "<td style='font-size: 12px; width: 33.3333%; text-align: right'>单位：元</td>" +
            //    "</tr > " +
            //    "</table >";
            var htmlTbl =
                "<table class='tbJY' style='width: 70%;margin: 0 auto;' cellspacing='0' cellpadding='0'>" +
                "<thead>" +
                "<tr style='height: 30px;border:0px'>" +
                "<td colspan='4' style='font-size:20px;text-align:center;font-weight:400;border:0px'>经营报表</td>" +
                "</tr>" +
                "<tr style='height: 30px; '>" +
                "<td ' style='font-size: 12px;border:0px' >编制单位：" + saveData.Account + "</td >" +
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
                    "<td>" + saveData.KPMoney.Credit_Debit + "</td>" +
                    "<td style='text-align: right'>" + saveData.KPMoney.Money + "</td>" +
                    "</tr>";
            }
            if (saveData.KPMoneyYear != undefined) {
                htmlDetail += "<tr>" +
                    "<td>" + saveData.KPMoneyYear.Subject + "</td>" +
                    "<td></td>" +
                    "<td>" + saveData.KPMoneyYear.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
                        "<td>" + obj.Credit_Debit + "</td>" +
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
            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);
            LODOP.ADD_PRINT_TABLE(20, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
            LODOP.ADD_PRINT_TEXT(15, '85%', 300, 100, "第#页/共&页");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "FontSize", 9);

        }
    }

    //获取税金表打印页面
    function GetSJHtml(saveData) {
        if (saveData != undefined) {
            var htmlStyle =
                "<style> body {text-align: center;}.tbSJ td {" +
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
            var htmlHead = "";
            //for (var i = 0; i < saveData.length; i++) {
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
                "<td ' style='font-size: 12px;border:0px' >编制单位：" + saveData.Account  + "</td >" +
                "<td  style='font-size: 12px;text-align:center;border:0px'>" + nowPeriod + "</td>" +
                "<td    style='font-size: 12px;  text-align: right;border:0px'>单位：元</td>" +
                "</tr > " +
                "<tr>" +
                "<td style='width: 50%; text-align: center'>项目</td>" +
                "<td style='width: 10%; text-align: center'>方向</td>" +
                "<td style='width: 40%; text-align: center'>金额</td>" +
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
            //LODOP.ADD_PRINT_HTML('5mm', 0, '100%', '100%', htmlStyle +htmlHead);
            LODOP.ADD_PRINT_TABLE(20, 0, '100%', '100%', htmlStyle + htmlTbl + htmlDetail);
            //}


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

})
