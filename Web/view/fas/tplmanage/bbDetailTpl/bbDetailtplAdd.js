
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer =  layui.layer  ,
		laypage = layui.laypage;
    var $ = layui.jquery;

   
    var id;
    var template = $.templates("#tpl-Edit");
    var parentId = $.getQueryString('parentId');
    var dataHtml = template.render({});
    $('#editForm').html(dataHtml);
    form.render();

    form.on("submit()", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/tplmanage/BBDetailtplUpdate";
        }
        else {
            url = "/fas/tplmanage/BBDetailtplAdd";
        }

       
        var request = {};
        request.Data = data.field;
        request.Token = token;
 
        request.Data.Id = id;
        request.Data.ParentId = parentId;
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
 

    form.on('select(SourceType)', function (data) {
        if(data.value==0)
        {
            $('#btnFormula').show();
        }
        else {
            $('#btnFormula').hide();
        }
    });

    $('#btnFormula').click(function () {
        if (id == ''||id==undefined) {
            $.warning('请先保存');
            return;
        }
        layer.open({
            type: 2,
            title: '公式管理',
            shade: 0.1,
            area: ['800px', '500px'],
            content: '/view/fas/tplmanage/formula/formulaList.aspx?id=' + id,
            cancel: function (index, layero) {
                
            }
        });

    });
})