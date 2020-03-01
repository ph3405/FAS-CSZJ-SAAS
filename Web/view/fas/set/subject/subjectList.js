layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    
    var category=1;
    form.on('select(category)', function (data) {
        category=data.value;
        query(1,data.value);

    });

    var query = function (pageIndex,category) {

        var index = $.loading('查询中');
        var request = {};
        request.Data = { Category:category};

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 9999;
        $.Post("/fas/set/subjectListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.lst);

                    $('#dt').html(dataHtml);
                    $("#dt").find("tr").each(function () {
    
                        if ($(this).attr("name") != "-") {
                            $(this).hide();
                        }

                    });
                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / 9999),
                        jump: function (obj, first) {
                            if (!first) {
                                query(obj.curr,category);
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
    query(1,1);
    window.query = query;
    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/subjectDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1,category);
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

  
    
 
    $("body").on("click", ".tks-rowAdd", function () {  //新增
        var id = $(this).attr('data-id');

        $.open("新增科目", "subjectAdd.aspx?parentId=" + id + "&category=" + category, true, function (a, b) {

            query(1, category);

        });
    })

    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('科目编辑', "subjectEditor.aspx?id=" + id + "&category=" + category);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此科目？', function () {


            rowDel(_this.attr("data-id"));

        });
    })
    $("body").on("click", ".tks-rowTree", function () {  //删除
        var cls = $(this).attr("data-id");
        var check = $(this).attr("data-check");
        if (check=="0") {
            $("[name='" + cls + "']").hide();
            $(this).attr("data-check", "1");
            $(this).html("<i class='layui-icon layui-icon-triangle-d'>&#xe623;</i>");
        }
        else {
            $("[name='" + cls + "']").show();
            $(this).attr("data-check", "0");
            $(this).html("<i class='layui-icon layui-icon-triangle-d'>&#xe625;</i>");
        }
        
    })
 
})