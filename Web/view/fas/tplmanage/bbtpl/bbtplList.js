layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer ,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("type", function (val) {

        if (val == 0) {
            return "类型1";
        }
        else if (val == 1) {
            return "类型2";
        }
    });

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
     
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/tplmanage/BBtplListSearch", request,
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
    query(1);
    window.query = query;
    var rowDel = function (id) {
       
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/tplmanage/BBtplDel", request,
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
    }
    //查询
    $("#btnSearch").click(function () {
        query(1);
    })

    //新增报表模板
    $("#btnAdd").click(function () {
        $.open("新增报表模板", "/view/fas/tplmanage/bbtpl/bbtplAdd.aspx");
    })

   

    
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑报表模板', "/view/fas/tplmanage/bbtpl/bbtplEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此关系吗？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

    $("body").on("click", ".tks-rowColumn", function () {  //编辑列明细
       
        window.parent.addTab($(this));
    })

})