 layui.config({
    base: "../../js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    form = layui.form();
    var form = layui.form(),
          layer = layui.layer,
          laypage = layui.laypage;
    var $ = layui.jquery;

   
 
    //添加验证规则
    form.verify({
        oldPwd: function (value, item) {
            if (value != "123456") {
                return "密码错误，请重新输入！";
            }
        },
        newPwd: function (value, item) {
            if (value.length < 6) {
                return "密码长度不能小于6位";
            }
        },
        confirmPwd: function (value, item) {
            if (!new RegExp($("#oldPwd").val()).test(value)) {
                return "两次输入密码不一致，请重新输入！";
            }
        }
    })

    var init = function () {
        var request = {};
      
        request.Data = {
            Id: id
        };
        request.Token = token;

        $.Post("/sso/userGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-edit");

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
    init();

    //提交个人资料
    form.on("submit(changeUser)", function (data) {

        var request = {};
        request.Data = data.field;
        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('提交中');


        $.Post("/sso/userUpdate2", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                layer.alert(res.Message);

            }
       

        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });

        return false;
    })



   

})

