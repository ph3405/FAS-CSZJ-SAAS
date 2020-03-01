
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({});

    $('#editForm').html(dataHtml);


    form.render();

    $("#txtStartYearMonth").click(function () {
        laydate({ elem: '#txtStartYearMonth', format: 'YYYY-MM' });
    });

    var container = $('#colContainer');//容器
    var i = 0;
    $('#btnAdd').click(function () {
        var total = $('#colContainer input').length;
        if (total === 8) {
            $.warning('最多添加8个自定义列');
            return;
        }
        i++;
        var tpl = $.templates('#tpl-col');
        var data = {
            code: 'custom' + i,
            field: '字段'
        };
        var dataHtml = tpl.render(data);

        $(container).append(dataHtml);

        $("."+data.code+' .layui-btn').click(function () {
            var code = $(this).attr('data-id');

            $('.' + code).remove();
        });

    });

    $('#colContainer .layui-btn').click(function () {
        var code = $(this).attr('data-id');

        $('.' + code).remove();
    });

    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/set/CalHelperUpdate";
        }
        else {
            url = "/fas/set/CalHelperAdd";
        }


        var request = {};
        request.Head = data.field;
        request.Token = token;
        request.Head.Id = id;
        request.CustomDes = new Array();
        var cols = $('#colContainer input');
        for (var j = 0; j < cols.length; j++) {
            var item = {};
            item.ColumnCode = j + 1;
            item.ColumnName = $(cols[j]).val();
            request.CustomDes.push(item);
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
