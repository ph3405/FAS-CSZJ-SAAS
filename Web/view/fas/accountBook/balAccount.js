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
    var num,//显示数量
        year,//显示本年累计
        allPZ,//显示所有凭证数据
        IsFuZhu,//显示辅助核算科目
       periodId,
      account,
      queryType = "normal"//普通查询
     , pageIndex = 1//页码
    ,_data;
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
                    //}
                  
                    form.render();

                    //$('#selPeriodS').children('option:first').attr("selected",'true');
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
    init();

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
    
    var query = function () {
        queryType = 'normal';
        var index = $.loading('查询中');
        var request = {};
        request.PeriodId = periodId;
        request.Type = queryType;//一般筛选
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        request.allPZ = "";
        if (allPZ == true) {
            request.allPZ = "allPZ";
        }
        Send(request, index);
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
        request.allPZ = "";
        if (allPZ == true) {
            request.allPZ = "allPZ";
        }
        Send(request, index);
    }
    function Send(request, index) {
        request.IsFuZhu = "";
        if (IsFuZhu == true) {
            request.IsFuZhu = "IsFuZhu";
        }
        $.Post("/fas/accountBook/BalAccountListSearch", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  printData = res.Data;
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

                  $('#dt').html(html1);
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
  

    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore( );
        return false;
    });
    form.on('checkbox(num)', function (data) {
        num = data.elem.checked;

    });

    form.on('checkbox(year)', function (data) {
        year = data.elem.checked;

    });
    form.on('checkbox(allPZ)', function (data) {
        allPZ = data.elem.checked;

    });
    form.on('checkbox(IsFuZhu)', function (data) {
        IsFuZhu = data.elem.checked;

    });
    $("#btnSearch").click(function () {
        periodId = $('#selPeriod').val();

        if (periodId != '') {
            pageIndex = 1;
            query();
        }

    })
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




        var period = periodTitle;
        var unit = '元';
        var company = account;
        var printDate = CurentTime();
        var items = printData;

        var data = {
            Company: company,
            Period: period,
            Unit: unit,
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
    var LODOP;
    var html = "";//打印生成的HTML
    var nowPeriod = "";//期别
    $('#btnPrint').click(function () {
        //CreateOneFormPage();
        //LODOP.PREVIEW();
        if ($('#selMonthS').val() == $("#selMonthE").val()) {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期";
        }
        else {
            nowPeriod = $('#selYearS').val() + "年" + $('#selMonthS option:selected').text() + "期" + "-" + $('#selYearE').val() + "年" + $('#selMonthE option:selected').text() + "期"
        }
        var request = {};
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 9999;
        request.More = {};

        request.More.Period_S = $('#selMonthS').val(); 
        request.More.Period_E = $("#selMonthE").val();

        request.More.Code_S = $("[name='codeS']").val();
        request.More.Code_E = $("[name='codeE']").val();
        request.Type = "more";
        var index = $.loading('打印获取数据');
        $.Post("/fas/accountBook/BalAccountListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("科目余额打印");
                    //LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");
                    LODOP.SET_PRINT_PAGESIZE(2, 0, 0, "A4");
                    LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1);//横向时的正向显示
                    GetKMHtml(data);
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
    //获取科目余额打印页面
    function GetKMHtml(saveData) {
        if (saveData != undefined && saveData.PrintData.length > 0) {
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
                "font-size: 13px;;font-family:'黑体' " +
                "} </style>";
            var count = 0;
            var htmlHead = "";
            //var htmlHead = "<table style='width: 75%;margin: 0 auto;'>" +
            //    "<tr>" +
            //    "<td style='width:33.3333%'></td>" +
            //    "<td style='font-size:20px;text-align:center;font-weight:400'>科目余额表</td>" +
            //    "<td style='font-size:12px;width:33.3333%;text-align:right'></td>" +
            //    "</tr>" +
            //    "<tr style='height: 30px; '>" +
            //    "<td style='font-size: 12px; width: 33.3333%' >编制单位：" + account + "</td >" +
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
                "<td colspan='2' style='font-size: 12px;border:0px' >编制单位：" + account + "</td >" +
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
            for (var i = 0; i < saveData.PrintData.length; i++) {

                //var perid = saveData.PrintData[i].Year + "年" + saveData.PrintData[i].Month + "期";

                htmlDetail += "<tr>" +
                    "<td style='text-align: left'>" + saveData.PrintData[i].Code + "</td>" +
                    "<td style='text-align: left'>" + saveData.PrintData[i].Name + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWBStart_J) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWBStart_D) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWB_CJ) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWB_CD) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWB_YJ) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWB_YD) + "</td>" +
                    "<td style='text-align: right'>" + noZero(saveData.PrintData[i].BWBEnd_J) + "</td>" +
                    "<td style='text-align: right;'>" + noZero(saveData.PrintData[i].BWBEnd_D) + "</td>" +
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