
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;

    var itemId = $.getQueryString('itemId');
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({});

    $('#editForm').html(dataHtml);


    form.render();
    var container = $('#colContainer');//容器
    var initUI = function () {
        var req = {};
        req.Token = token;
        req.Head = {
            Id: itemId
        };
        var index = $.loading('初始化中');
        $.Post("/fas/set/addUIInit", req, function (data) {
            var res = data;
            if (res.IsSuccess) {


                var template = $.templates("#tpl-col");

                var dataHtml = template.render(res.Data);

                $(container).html(dataHtml);


                form.render();
            } else {
                $.warning(res.Message);
            }
            layer.close(index);


        }, function (err) {
            $.warning(err.Message);
            layer.close(index);
        });
    };

    initUI();
    
   

    $('#colContainer .layui-btn').click(function () {
        var code = $(this).attr('data-id');

        $('.' + code).remove();
    });

    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/set/customDataUpdate";
        }
        else {
            url = "/fas/set/customDataAdd";
        }


        var request = {};
        request.Id = id;
        request.Head = {
            Id:itemId
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


        $.Post(url, request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                //id = res.Id;
               
                var dataHtml = template.render({});

                $('#editForm').html(dataHtml);
                initUI();
                form.render();
                $.topTip(res.Message);
                //$.info(res.Message + ",点击确定,返回列表页", function () {
                //    if (parent.query)
                //        parent.query(1);
                //    layer.closeAll('dialog'); //关闭信息框
                //    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                //    parent.layer.close(index); //再执行关闭  
                //})

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });




        return false;
    })

})
