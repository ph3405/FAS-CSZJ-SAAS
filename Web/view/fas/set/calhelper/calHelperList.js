layui.config({
    base: "/layui/lay/modules/"
}).use([ 'form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
    

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 999;
        $.Post("/fas/set/calHelperListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / 999),
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

        $.Post("/fas/set/calHelperDel", request,
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

   
    

    //添加
    $("#btnAdd").click(function () {
        $.open("新增自定义辅助核算", "calHelperAdd.aspx");
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('自定义辅助核算编辑', "calHelperEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此类别？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

    //操作
    $("body").on("click", ".tks-rowOpen", function () {  // 
       
        //window.parent.addTab($(this));
        var url = $(this).attr('data-url');
        $.open($(this).text(), url);

    })
 

})