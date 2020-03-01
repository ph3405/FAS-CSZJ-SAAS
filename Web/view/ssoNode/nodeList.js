layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    $.views.converters("type", function (val) {
       
        if (val == 0) {
            return '雇主企业';
        }
        else if(val==1)
        {
            return "代账企业";
        }
        else if (val == 2) {
            return "平台管理";
        }
        else
        {
            return "未设置";
        }
       
    });


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Data = { Name: $('#txtName').val() };

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/sso/nodeListSearch", request,
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

        $.Post("/sso/nodeDel", request,
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

    //添加机构
    $("#btnRoleAdd").click(function () {
        $.open("添加机构", "/view/ssoNode/nodeAdd.aspx");
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('机构编辑', "/view/ssoNode/nodeEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此角色？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

  

})