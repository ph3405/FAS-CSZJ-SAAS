layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;


    var docId = $.getQueryString('id');

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
      
        request.Token = token;
        request.DocId = docId;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/doc/fjListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);
                    $('.total').text('附件总数 '+res.Total);
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

        $.Post("/fas/doc/fjDel", request,
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

    $('#btnAdd').click(function () {
        $.dialog('上传', 'attachment.aspx?docId=' + docId, undefined, function () {
            query(1);
        });
       
    });
 
    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //查看
        var url = $(this).attr('data-url');
      

        var picUrl = $.baseUrl + url;
        window.open(picUrl);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
  
        $.confirm('确定删除此附件？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

 

})