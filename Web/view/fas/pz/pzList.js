layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
        $ = layui.jquery;
    var saveData; //保存查询的数据
    //千分位
    $.views.converters("trans", function (val) {

        if (val == 0) {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
        }
    });
    //转换大写
    $.views.converters("trans2DX", function (val) {
       
        if (val == 0 || val == undefined) {
            return "";
        } else {
            return digitUppercase(val);
        }


    });
    var request
    , type = "normal"
        , pageIndex = 1
        , _data;
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
    var digitUppercase = function (n) {
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

    var Init = function () {

        var index = $.loading('初始化');
        var request = {};

        request.Token = token;

        $.Post("/fas/period/PeriodGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-select");

                    var dataHtml = template.render(res.Data);
                    account = res.Account.QY_Name;
                    var qb = template.render({ Id: '##', Year: '全', Month: '部' });
                    //$('#selPeriod').append(qb);
                    //$('#selPeriod').append(dataHtml);

                    //$('#selPeriodS').append(dataHtml);
                    //$('#selPeriodE').append(dataHtml);
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
    };

    Init();//初始化期间

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
                         $('#selPZZMore').append("<option value=''  selected>全</option>");
                         $('#selPZZMore').append(dataHtml);
                         form.render();

                         //query();
                         queryMore();
                     }

                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });

    };

    
    var Type = "";
    var PZZ = "";
    var PZZ_S = "";
    var PZZ_E = "";
    var Period = "";
    var Token = "";
    var PageIndex = "";
    var PageSize = "";
    var Period_S = "";
    var Period_E = "";
    var MorePZZ = "";
    var MorePZZ_S = "";
    var MorePZZ_E = "";
    var query = function () {
        type = "normal";
        var index = $.loading('查询中');
        request = {};
        request.Type = 'normal';//普通查询
        request.PZZ = $('#PZZ').val();
        request.PZZ_S = $('#txtPZZ_S').val();
        request.PZZ_E = $('#txtPZZ_E').val();
        request.Period = $('#selPeriod').val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        Send(request,index);
    };
    var queryMore = function () {
        type = "more";
        var index = $.loading('查询中');
        request = {};
        request.More = {};
        //request.More.Period_S = _data.field.periodS;
        //request.More.Period_E = _data.field.periodE;
        //request.More.PZZ = _data.field.pzzMore;
        //request.More.PZZ_S = _data.field.pzzS;
        //request.More.PZZ_E = _data.field.pzzE;
        //request.More.Period_S = $('#selPeriodS').val();
        //request.More.Period_E = $('#selPeriodE').val();
        request.More.Period_S = $('#selMonthS').val();
        request.More.Period_E = $('#selMonthE').val();
        request.More.PZZ = $('#PZZ').val();
        request.More.PZZ_S = $('#txtPZZ_S').val();
        request.More.PZZ_E = $('#txtPZZ_E').val();
        request.Type = 'more';//更多筛选
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        Send(request, index);
  
    }
    function Send(request,index) {
        $.Post("/fas/doc/DocListSearch", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
             
                  saveData = data;
                  var template = $.templates("#tpl-list");

                  var dataHtml = template.render(res.Data);

                  $('#dt').html(dataHtml);
                  $('.layui-search-mored').hide();

                  laypage({
                      curr: pageIndex,
                      cont: "page",
                      pages: Math.ceil(res.Total / 10),
                      jump: function (obj, first) {
                          if (!first) {
                              pageIndex = obj.curr;
                              if (type == "normal") {
                                 
                                  query();
                              }
                              else {
                                  //$('#btnMoreQuery').click();
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

    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/doc/docDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        //query();
                        queryMore();
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

    var rowAudit = function (id) {
        var index = $.loading('正在审核');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/doc/docAudit", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        //query();
                        queryMore();
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
    var rowUnAudit = function (id) {
        var index = $.loading('正在取消审核');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/doc/docUnAudit", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        //query();
                        queryMore();
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
    form.on("submit(search)", function (data) {
        pageIndex = 1;
        _data = data;
        queryMore();
        return false;
    });

    //查询
    $(".search_btn").click(function () {
        pageIndex = 1;
        //query();
        queryMore();
    })

    //更多筛选
    $('#btnMore').click( function () {
        var offset = $("#btnMore").offset();
        //$(".layui-search-mored").css("position", "abosolute").css("left", offset.left - 357 + "px").css("top", offset.top + 38 + "px");
        $(".layui-search-mored").css("position", "abosolute").css("left", offset.left + "px").css("top", offset.top + 38 + "px");
        $('.layui-search-mored').toggle();
    });
    $('#btnCancel').click(function () {
        $('.layui-search-mored').hide();
    });
    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        // $.open('凭证编辑', "pzEditor.aspx?id=" + id);

        parent.layer.open({
            type: 2,
            title: '凭证编辑',
            shade: 0.1,
            area: ['1100px', '600px'],
            content: 'view/fas/pz/pzEditor.aspx?id=' + id,
            cancel: function (index, layero) {
                //query();
                queryMore();
            }
        });

    })

    $("body").on("click", ".tks-rowCopy", function () {  //复制
        var id = $(this).attr('data-id');
        // $.open('凭证编辑', "pzEditor.aspx?id=" + id);

        parent.layer.open({
            type: 2,
            title: '凭证新增',
            shade: 0.1,
            area: ['1100px', '600px'],
            content: 'view/fas/pz/pzEditor.aspx?type=FZ&id=' + id,
            cancel: function (index, layero) {
                //query();
                queryMore();
            }
        });

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此凭证？', function () {


            rowDel(_this.attr("data-id"));
            //query();
            queryMore();
        });
    })

    $("body").on("click", ".tks-audit", function () {  //审核
        var _this = $(this);
        $.confirm('确定要审核吗？', function () {


            rowAudit(_this.attr("data-id"));

        });
    })

    $("body").on("click", ".tks-unAudit", function () {  //取消审核
        var _this = $(this);
        $.confirm('确定要取消审核吗？', function () {


            rowUnAudit(_this.attr("data-id"));

        });
    })

    $("body").on("click", ".tks-fj", function () {
        var id = $(this).attr('data-id');
        parent.layer.open({
            type: 2,
            title: '附件编辑',
            shade: 0.1,
            area: ['1100px', '600px'],
            content: '/view/fas/fj/fjList.aspx?id=' + id,
            cancel: function (index, layero) {
                pageIndex = 1;
                //query();
                queryMore();
            }
        });
        //$.open("附件编辑", "/view/fas/fj/fjList.aspx?id=" + id);
    })
 
    //window.query = query; 
    window.query = queryMore;
    var LODOP;
    var AreaIndex = 0;
    var html = "";//打印生成的HTML
    $('#btnPrint').click(function () {
        

        type = "more";
        var index = $.loading('查询中');
        request = {};
        request.More = {};
        //request.More.Period_S = _data.field.periodS;
        //request.More.Period_E = _data.field.periodE;
        //request.More.PZZ = _data.field.pzzMore;
        //request.More.PZZ_S = _data.field.pzzS;
        //request.More.PZZ_E = _data.field.pzzE;
        //request.More.Period_S = $('#selPeriodS').val();
        //request.More.Period_E = $('#selPeriodE').val();
        request.More.Period_S = $('#selMonthS').val();
        request.More.Period_E = $('#selMonthE').val();
        request.More.PZZ = $('#PZZ').val();
        request.More.PZZ_S = $('#txtPZZ_S').val();
        request.More.PZZ_E = $('#txtPZZ_E').val();
        request.Type = 'more';//更多筛选
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 99999;
        $.Post("/fas/doc/DocListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    html = "";
                    //saveData = data;
                    LODOP = getLodop();
                    LODOP.PRINT_INIT("凭证打印");
                    LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
                    LODOP.SET_PRINT_PAGESIZE(1, 0, 0, "A4");
                    GetPZHtml(res);
                    if (html != "") {
                        LODOP.ADD_PRINT_HTM('10mm', 0, '100%', '100%', html);
              
                    }
                   
                    LODOP.PREVIEW();
                    //window.open("HtmlPrint.html?Type=" + Type + "&PZZ=" + PZZ + "&PZZ_S=" + PZZ_S + "&PZZ_E=" + PZZ_E + "&Period=" + Period + "&Token=" + Token + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&Period_S=" + Period_S + "&Period_E=" + Period_E + "&MorePZZ=" + MorePZZ + "&MorePZZ_S=" + MorePZZ_S + "&MorePZZ_E=" + MorePZZ_E);
                    //Print(saveData);
                    //window.print();
                } else {
                 
                }
                layer.close(index);
            }, function (err) {
                layer.close(index);
            });
        
        
         
    });


    //获取凭证打印页面
    function GetPZHtml(saveData) {
        //var row_index = 17;
        if (saveData != undefined && saveData.Data.length > 0) {
            AreaIndex = 0;
            var htmlStyle =
                "<style>body {text-align: center;} .tbPZ td {" +
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
                "font-size: 12px; font-family:'黑体'" +
                "}</style>";
            var count = 0;
            for (var i = 0; i < saveData.Data.length; i++) {
                //AreaIndex = 0;
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
                    "<td style='text-align:left;border-bottom:none' colspan='2'>合计: " + AMT_DBT + "</td>" +

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

                var index = 0;


                var Area = Math.ceil(saveData.Data[i].Detail.length / 5);
                for (var a = 0; a < Area; a++) {

                    if (Area > 1) {
                        var page = a + 1 + "/" + Area;
                        htmlHead = "<table style=' width:600px;margin: 0 auto;'>" +
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
                    //if (a==0) {
                    //    html += htmlStyle + htmlHead + htmlDetail + htmlSum + htmlFoot;
                    //}
                    //else {
                    //    html +=  htmlHead + htmlDetail + htmlSum + htmlFoot;
                    //}
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
})