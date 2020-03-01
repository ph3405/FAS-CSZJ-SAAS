
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');

    var itemId = $.getQueryString('itemId');

  
  
    var init = function () {
        var req = {};
        req.Data = { Id: id };
        var index = $.loading('加载中');
        $.Post("/fas/set/customDataGet", req,
         function (data) {
             var res = data;
             layer.close(index);
             if (!res.IsSuccess) {
                 $.warning(res.Message);
             }
             else {
                 var template = $.templates("#tpl-Edit");

                 var dataHtml = template.render(res.Data);

                 $('#editForm').html(dataHtml);

                 template = $.templates("#tpl-col");

                 dataHtml = template.render(res.CustomDes);
                 var container = $('#colContainer');//容器
                 $(container).html(dataHtml);


                 form.render();


             }


         }, function (err) {

             layer.close(index);
             $.warning(err.Message);
         });

    };
    init();


    form.on("submit(save)", function (data) {
        
 
        var request = {};
        request.Id = id;
        request.Head = {
            Id: itemId
        };
        request.Token = token;

        request.Data = new Array();

        var d = data.field;
        for (var p in d) {
            var item = {};
            item.Code = p;
            item.Value = d[p];
            request.Data.push(item);
        }


        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/set/customDataUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                id = res.Id;
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
    })

})
