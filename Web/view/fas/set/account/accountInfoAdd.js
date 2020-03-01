
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({ TaxRate: 25 });

    $('#editForm').html(dataHtml);


    form.render();

    $("#txtStartYearMonth").click(function () {
        laydate({ elem: '#txtStartYearMonth', format: 'YYYY-MM' });
    });


    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/set/accountUpdate";
        }
        else {
            url = "/fas/set/accountAdd";
        }


        var request = {};
        request.Data = data.field;
        request.Token = token;
        request.Data.Id = id;
        if (request.Data.AccountantId == '' || request.Data.AccountantId == undefined) {
            $.warning('请选择主板会计');
            return false;
        }

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
                    $('#btnInvitation').show();

                    $.info(res.Message + ",点击确定,返回列表页", function () {
                        if (parent.query)
                            parent.query(1);
                        parent.layer.closeAll();

                    });
                    //parent.parent.location.reload();
                    parent.parent.opAccountListGet();
                }



            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });




        return false;
    })


    var getInvitationCode = function (success) {
        if (id == '' || id == undefined) {
            $.warning('请先保存');
            return false;
        }
        var request = {};

        request.Token = token;
        request.Id = id;
        var index = $.loading('请稍后');


        $.Post('/fas/set/InvitationCodeGet', request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    success(res.InvitationCode);
                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });
    }

    var sendCode = function (phone, code) {
        if (id == '' || id == undefined) {
            $.warning('请先保存');
            return false;
        }
        var request = {};

        request.Token = token;
        request.MobilePhone = phone;
        request.Code = code;
        request.QYName = '';
        var index = $.loading('请稍后');


        $.ajax({
            type: "Post",
            url: "/fasservice.asmx/InvitationSend",
            data: JSON.stringify({ request: request }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var res = data.d;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    $.info("邀请成功", function () {

                        layer.closeAll();
                    })
                }
            },
            error: function (err) {
                layer.close(index);
                $.warning(err.Message);

            }
        });

    }

    var accountName = '';
    var invitationCode = '';
    $('#btnInvitation').click(function () {

        getInvitationCode(function (code) {

            var template = $.templates("#tpl-send");
            accountName = $('#iptQYName').val();
            invitationCode = code;
            var dataHtml = template.render({ AccountName: accountName, InvitationCode: code });

            layer.open({
                type: 1,
                title: '邀请企业主',
                shade: 0.1,
                area: ['500px', '350px'],
                content: dataHtml
            });
        });




    });


    $("body").on("click", "#btnSend", function () {  //发送
        var phone = $('#txtQYMobile').val();
        if (phone == '') {
            $.warning('请填写手机号码');
            return;
        }

        sendCode(phone, invitationCode);
    });

    $('#btnUserChoose').click(function () {
        $.dialog("用户选择", '/view/fas/set/account/userChoose.aspx');
    });

    window.setValue = function (code, name) {
        $('#txtAccountantId').val(code);
        $('#txtTrueName').val(name);
    }
})
