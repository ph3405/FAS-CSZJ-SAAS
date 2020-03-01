layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    $.views.converters("type", function (val) {
       
        if (val == 1) {
            return "平台角色";
        }
        else if (val == 2) {
            return "代帐企业角色";
        }
        else if (val == 3) {
            return "雇主企业角色";
        }
    });


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Data = { Name: $('#txtName').val() };

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/sso/roleListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-roleList");

                    var dataHtml = template.render(res.Data);

                    $('#roleDt').html(dataHtml);

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
    var roleDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/sso/roleDel", request,
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
    $(".search_btn").click(function () {
        query(1);
    })

    //添加角色
    $("#btnRoleAdd").click(function () {
        $.open("添加角色", "roleAdd.aspx");
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('角色编辑', "/view/ssoRole/roleEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此角色？', function () {


            roleDel(_this.attr("data-id"));

        });
    })

 
    $("body").on("click", ".tks-rowPermission", function () {  //分配权限
        var id = $(this).attr('data-id');
        $.open('权限分配', "/view/ssoFunc/funcTreeChecked.aspx?id=" + id);

    })

})