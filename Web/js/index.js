var $, tab, form, layer, element;
layui.config({
    base: "/layui/lay/modules/"
}).use(['bodyTab', 'form', 'element', 'layer', 'jquery', 'jqExt'], function () {
    form = layui.form(),
        layer = layui.layer,
        element = layui.element();
    $ = layui.jquery;

    tab = layui.bodyTab();

    var init = function () {
        
        var request = {};
        request.Token = token;
        request.FuncId = funcId;
        var index = $.loading('初始化');
        $.Post('/sso/userMenuGet', request, function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                tab.init(res.Data);

                // 添加新窗口
                $(".layui-nav .layui-nav-item a").on("click", function () {
                    addTab($(this));
                    $(this).parent("li").siblings().removeClass("layui-nav-itemed");
                })
                if (res.FuncId =="112323") {
                    opAccountListGet();
                }
                
            }


        }, function (err) {
            $.warning(err.Message);
            layer.close(index);

        });
    }
    init();


    var accountActive = function (id) {
        
        var index = $.loading('切换中');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/accountActive", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    parent.location.reload();
                   // parent.opAccountListGet();

                    //parent.layer.alert(res.Message + "，将重新加载页面", {

                    //    cancel: function (index, layer) {
                    //        parent.location.reload();
                    //        return false;
                    //    }
                    //}, function () {
                    //    parent.location.reload();
                    //});

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    form.on('select(currentAccount)', function (data) {
        accountActive(data.value);
    });

    //add by Hero.Zhang 关闭网页时，提示
    window.onbeforeunload = function (event) {
        var event = event || window.event;
        // 兼容IE8和Firefox 4之前的版本
        if (event) {
            event.returnValue = "确定要关闭窗口吗？";
        }
        // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
        return '确定要关闭窗口吗>现代浏览器？';
    }

})

function opAccountListGet() {
   
    var request = {};
    request.Token = token;
    var index = $.loading('账套获取中');
    $.Post('/fas/set/opAccountListGet', request, function (data) {
        var res = data;
        layer.close(index);
        if (!res.IsSuccess) {
            $.warning(res.Message);
        }
        else {
            var template = $.templates("#tpl-select");

            var dataHtml = template.render(res.Data);
            var SelectAccount = res.SelectAccount;
            $('#selAccount').html(dataHtml);
            if (res.IsSelected) {
                $('#txtPeriod').html(res.Year + '年第' + res.Month + "期");
            }
            else {
                if (res.Data.length > 0 && res.Data[0].Id != undefined) {
                    //add by Hero.Zhang 每次重新获取账套都重新激活
                    Active(res.Data[0].Id);
                }
            }
           
            if (SelectAccount != null && SelectAccount.IsOutSource == 1 && res.UserCreditCode != SelectAccount.WB_CreditCode) {
               
                CheckWB(SelectAccount.Id);
                //add by Hero.Zhang 每次重新获取账套都重新激活
                Active(SelectAccount.Id);
                

            }
            else {
                var request = {};
                request.Token = token;
                request.FuncId = funcId;
                var index = $.loading('初始化');
                $.Post('/sso/userMenuGet', request, function (data) {
                    var res = data;
                    layer.close(index);
                    if (!res.IsSuccess) {
                        $.warning(res.Message);
                    }
                    else {
                        tab.init(res.Data);

                        // 添加新窗口
                        $(".layui-nav .layui-nav-item a").on("click", function () {
                            addTab($(this));
                            $(this).parent("li").siblings().removeClass("layui-nav-itemed");
                        })
                    }


                }, function (err) {
                    $.warning(err.Message);
                    layer.close(index);

                });
            } 
            
            form.render();
        }


    }, function (err) {
        $.warning(err.Message);
        layer.close(index);

    });
}

function Active(id) {

    var index = $.loading('切换中');
    var request = {};
    request.Data = { Id: id };
    request.Token = token;

    $.Post("/fas/set/accountActive", request,
        function (data) {
            var res = data;
            if (res.IsSuccess) {

            } else {
                $.warning(res.Message);
            }
            layer.close(index);
        }, function (err) {
            $.warning(err.Message);
            layer.close(index);
        });
};
//打开新窗口
function addTab(_this) {
    tab.tabAdd(_this);
}

function CheckWB(id) {
    var request = {};
    request.Token = token;
    request.AccountId = id;    
    //request.FuncId = funcId;
    var index = $.loading('菜单获取中');
    $.Post('/sso/GetMenu', request, function (data) {
        var res = data;
        layer.close(index);
        if (!res.IsSuccess) {
            $.warning(res.Message);
        }
        else {
            tab.init(res.Data);

            // 添加新窗口
            $(".layui-nav .layui-nav-item a").on("click", function () {
                addTab($(this));
                $(this).parent("li").siblings().removeClass("layui-nav-itemed");
            })

            //opAccountListGet();
        }


    }, function (err) {
        $.warning(err.Message);
        layer.close(index);

    });
}
