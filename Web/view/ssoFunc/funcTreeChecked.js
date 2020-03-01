layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;


    var entityId = $.getQueryString('id');

    //$('#funcTree').tree({
    //    checkbox: true,
 
    //});
    var init = function () {

        var index = $.loading('加载中...');
        var request = {};
        request.Data = {};
        request.EntityId = entityId;
        request.Token = token;

        $.Post("/sso/funcTreeCheckedGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    $('#funcTree').tree({
                        checkbox: true,
                        //cascadeCheck:false,
                        data: res.Data
                        
                    });


                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    init();



    form.on("submit(save)", function (data) {

        var request = {};
        var nodes = $('#funcTree').tree('getChecked', ['checked','indeterminate']);;
        request.RoleId = entityId;
        request.Data = nodes;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');

        $.Post('/sso/funcTreeCheckedUpdate', request,
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