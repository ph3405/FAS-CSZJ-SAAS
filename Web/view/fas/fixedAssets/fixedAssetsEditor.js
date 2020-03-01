
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');

    var init = function (id) {
        var request = {};
        request.Id = id;
        request.Token = token;
        $.Post("/fas/FixedAssets/FixedAssetsGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var tpl = "#tpl-Edit";
                  if (res.Data.IsGenPZ == 1) {
                      tpl = "#tpl-read";
                  }
                  
                  if (res.Data.AddType == "期初" && res.Data.StartPeriod != res.NowPeriod) {
                      tpl = "#tpl-read";
                  }
                  //if (res.Data.Status == 1) {
                  //    处置后无法编辑
                  //    tpl = "#tpl-read";
                  //}
                  var template = $.templates(tpl);

                  var dataHtml = template.render(res.Data);

                  $('#editForm').html(dataHtml);

                  $('#selAssetsClass').val(res.Data.AssetsClass);
                  $('#selAddType').val(res.Data.AddType);
                  $('#selDepreciationMethod').val(res.Data.DepreciationMethod);
                  $('#selIsStartPeriodDepreciation').val(res.Data.IsStartPeriodDepreciation);
                

                  form.render();
                  $("#txtInitialAssetValue,#txtScrapValueRate,#txtDpreMonth,#txtPreUseMonth,#txtAccumulativeDpre,#txtAccumulativeDpre_Y").change(function () {
                      calculate();
                  });
                  $("#txtStartUseDate").click(function () {
                      laydate({ elem: '#txtStartUseDate', format: 'YYYY-MM-DD' });

                  });
                  UIInit();
                  layer.msg(res.Message);
              }
              else {
                  $.warning(res.Message);
              }

          }, function (err) {
              $.warning(err.Message);

          });
    };
    init(id);


    form.on("submit(save)", function (data) {

       
        var request = {};
        request.Data = data.field;
        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/FixedAssets/FixedAssetsUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message + ",点击确定,返回列表页", function () {
                    if (parent.query)
                        parent.query(1);
                    parent.layer.closeAll();
                })

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });

 
        return false;
    })

   
    $("#txtStartUseDate").click(function () {
        laydate({ elem: '#txtStartUseDate', format: 'YYYY-MM-DD' });
    });

    function calculate() {
        var val = $('#txtInitialAssetValue').val();
        var flag1 = $.isNumeric(val);

        var rate = $('#txtScrapValueRate').val();
        var flag2 = $.isNumeric(rate);
        var yjcz = 0;
        if (flag1 && flag2) {

            yjcz = (val * rate / 100).toFixed(2);
            $('#txtScrapValue').val(yjcz);//预计残值
        }

        //剩余使用月份

        var preMonth = $('#txtPreUseMonth').val();
        var useMonth = $('#txtDpreMonth').val();
        var flag4 = $.isNumeric(preMonth);
        var flag5 = $.isNumeric(useMonth);
        var month;
        if (flag4 && flag5) {
            month = preMonth - useMonth;
            $('#txtRemainderUseMonth').val(month);
        }

        //每月折旧额
        month = $('#txtRemainderUseMonth').val();
        var flag3 = $.isNumeric(month);
        if (flag1 && flag2 && flag3) {
            var dpre = ((val - yjcz) / month).toFixed(2);
            $('#txtDprePerMonth').val(dpre);
        }

        var ljzj = $('#txtAccumulativeDpre').val();//累计折旧
        var flag6 = $.isNumeric(ljzj);
        if (flag1 && flag2 && flag3 && flag6) {
            var dpre = ((val - yjcz - ljzj) / month).toFixed(2);
            $('#txtDprePerMonth').val(dpre);
        }

        var curLjzj = $('#txtAccumulativeDpre_Y').val();//本年累计折旧
        var flag7 = $.isNumeric(curLjzj);
        if (flag6 && flag7) {
            var preLjzj = ljzj - curLjzj;
            $('#txtPreviousAccumulativeDpre').val(preLjzj);
        }
    }

    var dealSubject = function (codeEle, nameEle, d) {
        $(codeEle).val(d.item.Id);//code
        $(nameEle).val(d.item.Value);//name
    };


    var bindAutocomplete = function (objEle, valEle, data) {
        $(objEle).autocomplete({
            source: data,

            select: function (event, ui) {

                dealSubject(valEle, objEle, ui);
            }
        });
    };
    var UIInit = function () {
        var request = {};
        request.Token = token;
        var index = $.loading('初始化中');
        $.Post("/fas/set/subjectTotalGet", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         subjectData = res.Data;
                         bindAutocomplete($("#txtDCostSubjectName"), $('#txtDCostSubjectCode'), res.Data);

                         bindAutocomplete($("#txtGDName"), $('#txtGDCode'), res.Data);
                         bindAutocomplete($("#ADSubjectName"), $('#ADSubjectCode'), res.Data);
                         $.topTip(res.Message);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });

    };

    //input输入框回车禁止提交表单
    document.getElementsByTagName('form')[0].onkeydown = function (e) {
        var e = e || event;
        var keyNum = e.which || e.keyCode;
        return keyNum == 13 ? false : true;
    };

})
