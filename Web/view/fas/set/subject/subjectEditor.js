
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');
    var category=$.getQueryString('category');
    var init = function (id) {
        var request = {};
        request.Data = {
            Id: id
        };
        request.Token = token;

        $.Post("/fas/set/subjectGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-Edit");

                  var dataHtml = template.render(res.Data);

                  $('#editForm').html(dataHtml);


                  template = $.templates("#tpl-currency");

                  dataHtml = template.render(res.Currency);

                  $("#currencyContainer").html(dataHtml);

                  template = $.templates("#tpl-calItem");

                  dataHtml = template.render(res.CalItem);

                  $("#calItemContainer").html(dataHtml);

                  form.render();
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

        if (data.field.IsQuantityValid == 'on') {
            data.field.IsQuantityValid = 1;
        }
        else {
            data.field.IsQuantityValid = 0;
            request.Data.QuantityValue = '';
        }
        if (data.field.IsCalHelperValid == 'on') {
            data.field.IsCalHelperValid = 1;
            request.CalItem = getCheckedVal('cal', data.field);
        }
        else {
            data.field.IsCalHelperValid = 0;
            request.CalItem = new Array();
        }
        if (data.field.IsCurrencyValid == 'on') {
            data.field.IsCurrencyValid = 1;
            request.Currency = getCheckedVal('currency', data.field);
        }
        else {
            data.field.IsCurrencyValid = 0;
            request.Currency = new Array();
        }


        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/set/subjectUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message + ",点击确定,返回列表页", function () {
                    if (parent.query)
                        parent.query(1, category);
                    parent.layer.closeAll();
                })

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });


        return false;
    });

    var getCheckedVal = function (type, fields) {
        var arr = new Array();
        for (var p in fields) {
            if (p.indexOf(type) > -1) {
                var val = p.split("-");
                if (val.length > 1) {
                    arr.push(val[1]);
                }
            }
        }
        return arr;
    };
})
