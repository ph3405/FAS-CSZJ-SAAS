
layui.define(['jquery', 'JsRender', 'layer'], function (exports) {
    jQuery = layui.jquery;
    var layer = layui.layer;
    //var _url='http://api.isuanpan.cn'
    var _url = 'http://localhost:17222';//API
    var reqUrl = 'http://localhost:59176';//web


    jQuery.baseUrl = _url;
   
    jQuery.Post = function (url, data, successCall, errorCall) {
        var __url = _url + url;
        var request = { request: data };
        jQuery.ajax({
            type: "Post",
            url: __url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                successCall(data);
            },
            error: function (err) {
                if (err.status == 0) {
                    console.warn(err);
                    err.responseJSON = {};
                    err.responseJSON.Message = '请求被拒绝，请联系管理员';
                }
                errorCall(err.responseJSON);
            }
        });
    };

    jQuery.PostTonBu = function (url, data, successCall, errorCall) {
        var __url = _url + url;
        var request = { request: data };
        jQuery.ajax({
            type: "Post",
            url: __url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false, 
            success: function (data) {
                successCall(data);
            },
            error: function (err) {
                if (err.status == 0) {
                    console.warn(err);
                    err.responseJSON = {};
                    err.responseJSON.Message = '请求被拒绝，请联系管理员';
                }
                errorCall(err.responseJSON);
            }
        });
    };

    jQuery.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2];
        return null;
    };

    jQuery.warning = function (msg) {
        layer.alert(msg, { icon: 2 },
            function () {
                if (msg =="登陆超时，请重新登陆") {
                    top.location.href = 'Login.aspx';
                }
                layer.close(layer.index);
            }
        );
    };

    jQuery.topTip = function (msg) {
        layer.msg(msg, { icon: 1, offset: '0px' });
    }

    jQuery.loading = function (msg) {
        var index = layer.msg(msg, { icon: 16, time: false, shade: 0.1 });
        return index;
    };

    jQuery.confirm = function (msg, callBack) {
        layer.confirm(msg, { icon: 3, title: '提示信息' }, function (index) {

            layer.close(index);
            if (callBack != undefined)
                callBack();
        });
    }

    jQuery.info = function (msg, callBack) {
        layer.alert(msg, function () {
            if (callBack != undefined) {
                callBack()
            }
        });
    }

    jQuery.open = function (title, content, isShow, cancelCallback) {


        var index = layui.layer.open({
            title: title,
            type: 2,
            content: content,
            area: ['95%', '95%'],
            success: function (layer, index) {
                if (isShow != false)

                    setTimeout(function () {
                        layui.layer.tips('点击此处返回', '.layui-layer-close', {
                            tips: 3
                        });
                    }, 1000);

            },
            cancel: function () {
                if (cancelCallback != undefined)
                    cancelCallback();
            }
        })
        //改变窗口大小时，重置弹窗的高度，防止超出可视区域（如F12调出debug的操作）
        //$(window).resize(function () {
        //    layui.layer.full(index);
        //})
        //layui.layer.full(index);
    }

    jQuery.dialog = function (title, content, area, cancelCallback) {
        layer.open({
            type: 2,
            title: title,
            shadeClose: true,
            shade: 0,
            area: area != undefined ? area : ['500px', '400px'],
            content: content,
            cancel: function () {
                if (cancelCallback != undefined)
                    cancelCallback();
            }
        });
    }

    jQuery.views.helpers({
        TimeFormatter: function (time) {
            if (time == null || time == undefined) return "";
            var t = time.split('T');
            if (t.length < 2) return time;
            var result = t[0].substr(0, 10) + ' ' + t[1].substr(0, 8);
            return result;
        },
        YearMonthDay: function (time) {
            if (time == null || time == undefined) return "";
            var t = time.split('T');
            if (t.length < 2) return time;
            var result = t[0];
            return result;
        },
        ShortTimeFormatter: function (time) {
            if (time == null || time == undefined) return "";
            var t = time.split('T');
            if (t.length < 2) return time;
            var result = t[0].substr(2, 8) + ' ' + t[1].substr(0, 5);
            return result;
        },
        YearMonth: function (time) {
            if (time == null || time == undefined) return "";
            var t = time.split('T');
            if (t.length < 2) return time;
            var result = t[0].substr(0, 7);
            return result;
        },
        LastTimeFormatter: function (time) {
            return $.getDateDiff(time);
        }
    });
    exports('jqExt', jQuery);
});