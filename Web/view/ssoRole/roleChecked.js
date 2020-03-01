
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
        request.UserId = id;
        request.Token = token;

        $.Post("/sso/userRoleCheckedGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-Edit");

                  var dataHtml = template.render(res);

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

        console.log(data.field);
        var request = {};
        request.Token = token;
        request.UserId = id;
        request.RoleIds = [];
        for (var p in data.field) {
            request.RoleIds.push(p);
        }
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/sso/userRoleCheckedUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.topTip(res.Message);
            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });






        return false;
    })

})
