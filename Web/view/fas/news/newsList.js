layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("toStatus", function (val) {
        if (val == '0') {
            return '草稿';
        }
        else if (val == '1') {
            return '发布';
        }

    });



    var title = '';


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Title = title;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        request.Type = "news";
        $.Post("/fas/set/NewsSearch", request,
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
        request.Id = id ;

        request.Token = token;

        $.Post("/fas/set/NewsDel", request,
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

    var rowPublish = function (id) {
        var index = $.loading('正在发布');
        var request = {};
        request.Id =  id ;
        request.Token = token;

        $.Post("/fas/set/NewsPublish", request,
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

    var rowUnPublish = function (id) {
        var index = $.loading('取消发布');
        var request = {};
        request.Id = id;
        request.Token = token;

        $.Post("/fas/set/NewsUnPublish", request,
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
        title = $('#txtTitle').val();
        query(1);
    })

    //添加
    $("#btnAdd").click(function () {
        $.open("新增资讯", "newsEditor.aspx");
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑资讯', "newsEditor.aspx?id=" + id);

    });

    $("body").on("click", ".tks-publish", function () {  //发布
        var id = $(this).attr('data-id');
        rowPublish(id);

    });

    $("body").on("click", ".tks-unpublish", function () {  //取消发布
        var id = $(this).attr('data-id');
        rowUnPublish(id);
    });

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此资讯？', function () {


            rowDel(_this.attr("data-id"));

        });
    })




})