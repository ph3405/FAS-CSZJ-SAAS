
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
        $.Post("/fas/BasicData/WX_BasicDataGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var tpl = "#tpl-Edit";
                  var template = $.templates(tpl);

                  var dataHtml = template.render(res.Data);

                  $('#editForm').html(dataHtml);
                

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
        request.Name = data.field.Name;
        request.UserId = userId;
        request.Token = token;
        request.DataType = 'Vendor';
        request.Id = id;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/BasicData/WX_BasicDataUpdate", request,
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
    });

})
