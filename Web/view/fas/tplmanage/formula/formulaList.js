layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

   

    
    $.views.converters("valueRule", function (val) {
        if (val == 0) {
            return "发生额";
        }
        else {
            return "余额";
        }
    });
    var id = $.getQueryString('id');//模板列ID
    
    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.ReportDetailTPLId = id;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/tplmanage/formulaListSearch", request,
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

        $.Post("/fas/tplmanage/formulaDel", request,
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
        $.open("新增公式", "/view/fas/tplmanage/formula/formulaAdd.aspx?parentId=" + id);
    })




    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑列', "/view/fas/tplmanage/formula/formulaEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除吗？', function () {


            rowDel(_this.attr("data-id"));

        });
    })


})