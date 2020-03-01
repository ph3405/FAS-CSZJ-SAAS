layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer ,
		laypage = layui.laypage,
		$ = layui.jquery;

   
    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.UserName = $('#txtName').val();
        request.TrueName = $('#txtName').val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/set/userListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-userList");

                    var dataHtml = template.render(res.Data);

                    $('#userDt').html(dataHtml);
                    $('.users-list thead input[type="checkbox"]').prop("checked", false);


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
    var userDel = function (id) {
       
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/userDel", request,
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

    //添加会员
    $(".usersAdd_btn").click(function () {
        $.open("添加用户", "/view/fas/set/permission/userAdd.aspx");
    })

   

    //操作
    $("body").on("click", ".tks-rowRole", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('设置角色', "/view/fas/set/permission/roleChecked.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('用户编辑', "/view/fas/set/permission/userEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此用户？', function () {


            userDel(_this.attr("data-id"));

        });
    })



})