
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage;
    var $ = layui.jquery;

    var init = function () {
        var request = {};

        request.Token = token;

        $.Post("/fas/qm/QMCheckTPLsGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-Edit");
                    
                    var dataHtml = template.render(res.Data);

                    $('#container').html(dataHtml);

                    BindEvent();
                    form.render();

                }
                else {
                    $.warning(res.Message);
                }

            }, function (err) {
                $.warning(err.Message);

            });
    };
    init();
    window.init = init;

    function BindEvent() {
        $('.btnGenPZ').click(function () {
            var tplId = $(this).attr('data-id');
            var ul = $(this).parent().find('.money')[0];
            var money = 0;
            var tplTarget = $(this).attr('data-tpl');
            var type = 'QM';//期末检查
            if (tplTarget != 3) {
                money = $(ul).find('.input-money')[0].value;
                if (!checkRate(money)) {
                    $.warning('请输入有效金额');
                    return;
                }
            }
            else {
                type = 'GD';//固定资产,计提折旧与结账
            }



            if (type == "GD") {
                var request = {};
                request.Token = token;

                $.Post("/fas/FixedAssets/IsGenPZ", request,
                    function (data) {
                        var res = data;
                        if (res.IsSuccess) {
                            parent.layer.open({
                                type: 2,
                                title: '凭证编辑',
                                shade: 0.1,
                                area: ['1200px', '600px'],
                                content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=" + type + "&money=" + money,
                                cancel: function (index, layer) {

                                    init();

                                }
                            });

                        }
                        else {
                            $.warning(res.Message);
                        }

                    }, function (err) {
                        $.warning(err.Message);

                    });
            }
            else {
                parent.layer.open({
                    type: 2,
                    title: '凭证编辑',
                    shade: 0.1,
                    area: ['1200px', '600px'],
                    content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=" + type + "&money=" + money,
                    cancel: function (index, layer) {

                        init();

                    }
                });
            }





        });

        $('.btnPZEdit').click(function () {
            var pzid = $(this).attr('data-id');

            parent.layer.open({
                type: 2,
                title: '凭证编辑',
                shade: 0.1,
                area: ['1200px', '600px'],
                content: '/view/fas/pz/pzEditor.aspx?id=' + pzid,
                cancel: function (index, layer) {
                    init();
                }
            });
        });

    }

    function checkRate(nubmer) {
        var re = /^\d+(\.([1-9]|\d[1-9]))?$/;  //判断数字以及小数

        if (!re.test(nubmer)) {

            return false;
        }
        else {
            return true;
        }
    }

    $('#btnNext').click(function () {
        var request = {};

        request.Token = token;

        $.Post("/fas/qm/QMValidate", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    window.location.href = "qm1.aspx";
                }
                else {
                    $.warning(res.Message);
                }

            }, function (err) {
                $.warning(err.Message);

            });

    });

})
