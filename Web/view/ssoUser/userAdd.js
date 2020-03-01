
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer =  layui.layer  ,
		laypage = layui.laypage;
    var $ = layui.jquery;

   
    var id;
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({});
    $('#editForm').html(dataHtml);
    form.render();

    form.on("submit()", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/sso/userUpdate";
        }
        else {
            url = "/sso/userAdd";
        }

       
        var request = {};
        request.Data = data.field;
        request.Token = token;

        if (data.field.Password1 != data.field.Password2) {
            $.warning('两次密码输入不一致');
            return;
        }
        request.Data.Password = data.field.Password1;
        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post(url, request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                id = res.Id;
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

    $('#btnNodeChoose').click(function () {
        $.dialog("机构选择", '/view/ssoNode/nodeChoose.aspx');

    });

    window.setValue = function (code,name) {
        $('#txtNodeId').val(code);
        $('#txtNodeName').val(name);
    }
})
