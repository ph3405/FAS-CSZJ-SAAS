layui.config({
    base: "js/"
}).use([ 'form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
 

    var itemId = $.getQueryString('id');

    var initUI = function () {
        var req = {};
        req.Token = token;
        req.Data = {
            Id:itemId
        };
        var index = $.loading('初始化中');
        $.Post("/fas/set/searchUIInit", req, function (data) {
            var res = data;
            if (res.IsSuccess) {


                var template = $.templates("#tpl-thead");

                var dataHtml = template.render(res);

                $('#dt-thead').html(dataHtml);
 

                form.render();

                query(1);
            } else {
                $.warning(res.Message);
            }
            layer.close(index);


        }, function (err) {
            $.warning(err.Message);
            layer.close(index);
        });
    }
    initUI();

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Data = {
            Code: $('#txtName').val(),
            Name: $('#txtName').val()
        };
        request.Head = {
            Id:itemId
        };

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/set/customDataListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / 10),
                        jump: function (obj, first) {
                            if (!first) {
                                query(obj.curr);
                            }
                        }
                    });

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
   
    window.query = query;
    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/customDataDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1);
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

   
    //查询
    $(".search_btn").click(function () {
        query(1);
    })

    //添加
    $("#btnAdd").click(function () {

        $.open("新增", "customDataAdd.aspx?itemId=" + itemId, undefined, function (a, b) {
           
            query(1);
            
        });
    })
    $('#btnImport').click(function () {
        $.dialog('导入', 'attachment.aspx?itemId=' + itemId, undefined, function () {
            query(1);
        });

    });

    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑', "customDataEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除吗？', function () {


            rowDel(_this.attr("data-id"));

        });
    })


   
})