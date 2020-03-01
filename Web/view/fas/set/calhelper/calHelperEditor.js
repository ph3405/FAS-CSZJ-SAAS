
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');
  
    var bindEvent = function () {
        var container = $('#colContainer');//容器
        var i = 10;
        $('#btnAdd').click(function () {
            var total = $('#colContainer input').length;
            if (total === 8) {
                $.warning('最多添加8个自定义列');
                return;
            }
            i++;
            var tpl = $.templates('#tpl-col');
            var data = {
                code: 'custom' + i,
                field: '字段'
            };
            var dataHtml = tpl.render(data);

            $(container).append(dataHtml);

            $("." + data.code + ' .layui-btn').click(function () {
                var code = $(this).attr('data-id');

                $('.' + code).remove();
            });

        });
    };
  


    var init = function (id) {
        var request = {};
        request.Data = {
            Id: id
        };


        $.Post("/fas/set/CalHelperGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-Edit");

                  var dataHtml = template.render(res.Head);

                  $('#editForm').html(dataHtml);
                  var container = $('#colContainer');//容器
                  for (var j = 0; j < res.CustomDes.length; j++) {
                      var item = res.CustomDes[j];
                      var tpl = $.templates('#tpl-col');
                      var data = {
                          code: "custom"+item.ColumnCode,
                          field: item.ColumnName,
                          IsUse:res.IsUse
                      };
                      var colHtml = tpl.render(data);

                      $(container).append(colHtml);

                      $("." + data.code + ' .layui-btn').click(function () {
                          var code = $(this).attr('data-id');

                          $('.' + code).remove();
                      });
                  }
                  bindEvent();
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
        request.Head = data.field;
        request.Head.Id = id;
        request.Token = token;
        request.CustomDes = new Array();
        var cols = $('#colContainer input');
        for (var j = 0; j < cols.length; j++) {
            var item = {};
            item.ColumnCode = j + 1;
            item.ColumnName = $(cols[j]).val();
            request.CustomDes.push(item);
        }
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/set/CalHelperUpdate", request,
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

})
