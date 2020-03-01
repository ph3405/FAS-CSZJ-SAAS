
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
        request.Data = {
            Id: id
        };
        request.Token = token;

        $.Post("/sso/userGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-userEditor");

                  var dataHtml = template.render(res.Data);

                  $('#userForm').html(dataHtml);


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


    form.on("submit(updateUser)", function (data) {
 
        var request = {};
        request.Data = data.field;
        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/sso/userUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message + ",点击确定,返回列表页", function () {
                    if (parent.query  )
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
