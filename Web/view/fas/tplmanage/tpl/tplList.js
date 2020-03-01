layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    form.render();
    $.views.converters("tplTarget", function (val) {
        if (val == 1) {
            return "快速凭证";
        }
        else if (val == 2) {
            return "发票上传";
        }
        else if (val == 3) {
            return "固定资产";
        }
        else if (val == 4) {
            return "期末检查";
        }
        else if (val == 5) {
            return "期间费用结转";
        }
        else if (val == 6) {
            return "利润结转";
        }
        else if (val == 7) {
            return '固定资产变动类型';
        }
    });
    $.views.converters("isCarry", function (val) {
        if (val == 0) {
            return "否";
        }
        else if (val == 1) {
            return "是";
        }
        
    });

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Title = $('#txtTitle').val();
        request.TPLTarget = $('#selTPLTarget').val();
        request.IsCarry = $('#selIsCarry').val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;

        $.Post("/fas/tplmanage/TPLMListSearch", request,
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

        $.Post("/fas/tplmanage/tplMDel", request,
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

    $("#btnAdd").click(function () {
        parent.layer.open({
            type: 2,
            title: '新建模板',
            shade: 0.1,
            area: ['1200px', '600px'],
            content: 'view/fas/tplmanage/tpl/tplEditor.aspx',
            cancel: function (index, layero) {

            }
        });
    });
 
    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
 
        parent.layer.open({
            type: 2,
            title: '模板编辑',
            shade: 0.1,
            area: ['1200px', '600px'],
            content: 'view/fas/tplmanage/tpl/tplEditor.aspx?id=' + id,
            cancel: function (index, layero) {
             
            }
        });

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此模板？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

 

})