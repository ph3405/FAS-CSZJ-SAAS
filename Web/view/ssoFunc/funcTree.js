layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    var nodeId = '', type = 'A';


    $('#funcTree').tree({
        onContextMenu: function (e, node) {
            e.preventDefault();
            $(this).tree('select', node.target);
            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onClick: function (node) {
            nodeId = node.id;
            type = 'E';
            loadData(node.id);
        }
    });
    var init = function () {

        var index = $.loading('加载中...');
        var request = {};
        request.Data = {};

        request.Token = token;

        $.Post("/sso/funcTreeGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    $('#funcTree').tree({
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

    var loadData = function (id) {
        var index = $.loading('加载中...');
        var request = {};
        request.Data = { Id: id };

        request.Token = token;

        $.Post("/sso/funcGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-Edit");

                    var dataHtml = template.render(res.Data);

                    $('#editForm').html(dataHtml);


                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    }

    form.on("submit(save)", function (data) {
        var url = '';
        var request = {};
        request.Data = data.field;
        request.Token = token;
        if (type === 'E') {
            url = "/sso/funcUpdate";
            request.Data.Id = nodeId;
        }
        else {
            url = "/sso/funcAdd";
            request.Data.ParentId = nodeId;
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

                   init();
               }


           }, function (err) {

               layer.close(index);
               $.warning(err.Message);
           });
        return false;
    })

    var append = function () {
        var node = $('#funcTree').tree('getSelected');
        nodeId = node.id;
        type = 'A';
        var template = $.templates("#tpl-Add");

        var dataHtml = template.render({});

        $('#editForm').html(dataHtml);
    }

    var funcDel = function () {
        var node = $('#funcTree').tree('getSelected');
        nodeId = node.id;
        if (nodeId == "-")
        {
            $.warning('根目录不允许删除');
            return;
        }
        $.confirm('确定删除此菜单？', function () {
            //弹出loading
            var index = $.loading('正在删除');
            var request = {};
            request.Data = { Id: nodeId };
            request.Token = token;
            $.Post('/sso/funcDel', request,
               function (data) {
                   var res = data;
                   layer.close(index);
                   if (!res.IsSuccess) {
                       $.warning(res.Message);
                   }
                   else {
                    
                       init();
                   }


               }, function (err) {

                   layer.close(index);
                   $.warning(err.Message);
               });
 
        });
    }
    var collapse = function () {
        var node = $('#funcTree').tree('getSelected');
        $('#funcTree').tree('collapse', node.target);
    }
    var expand = function () {
        var node = $('#funcTree').tree('getSelected');
        $('#funcTree').tree('expand', node.target);
    }

    window.init = init;
    window.append = append;
    window.collapse = collapse;
    window.expand = expand;
    window.removeit = funcDel;
   

})