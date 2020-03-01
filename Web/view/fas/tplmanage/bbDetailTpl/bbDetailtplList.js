layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("category", function (val) {

        if (val == 10) {
            return "流动资产";
        }
        else if (val == 11) {
            return "流动负债";
        }
        else if (val == 12) {
            return "非流动资产";
        }
        else if (val == 13) {
            return "非流动负债";
        }
        else if (val == 14) {
            return "负债合计";
        }
        else if (val == 15) {
            return "资产合计";
        }
        else if (val == 16) {
            return "所有者权益（或股东权益）";
        }
        else if (val == 17) {
            return "负债和所有者权益（或股东权益）合计";
        }
        else if (val == 20) {
            return "营业收入";
        }
        else if (val == 21) {
            return "营业利润";
        }
        else if (val == 22) {
            return "利润总额";
        }
        else if (val == 23) {
            return "净利润";
        }
    });

    
    $.views.converters("sourceType", function (val) {
        if (val == 0) {
            return "公式";
        }
        else {
            return "求和";
        }
    });
    var id = $.getQueryString('id');//模板表头ID

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Id = id;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/tplmanage/BBDetailtplListSearch", request,
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

        $.Post("/fas/tplmanage/BBDetailtplDel", request,
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
        $.open("新增列", "/view/fas/tplmanage/bbDetailtpl/bbDetailtplAdd.aspx?parentId=" + id);
    })




    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑列', "/view/fas/tplmanage/bbDetailtpl/bbDetailtplEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此关系吗？', function () {


            rowDel(_this.attr("data-id"));

        });
    })


})