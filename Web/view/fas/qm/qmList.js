layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
 
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/qm/qmListSearch", request,
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
   
    parent.query = query;
    //查询
    $("#btnSearch").click(function () {
        query(1);
    })

    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //期末处理
        var id = $(this).attr('data-id');
       // $.open('期末处理', "qmEditor.aspx");

        parent.layer.open({
            type: 2,
            title: '期末处理',
            shade: 0.1,
            area: ['1200px', '500px'],
            content: '/view/fas/qm/qmEditor.aspx',
            cancel: function (index, layer) {

                query(1);

            }
        });

    })
 
    //操作
    $("body").on("click", ".tks-rowUnCarry", function () {  //期末处理
      
        
        var request = {};

        request.Token = token;
        var index = $.loading('反结转中');
        $.Post("/fas/qm/QMUnCarryOver", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  $.info(res.Message, function () {
                      location.reload();
                  });
                  parent.opAccountListGet();
              }
              else {
                  $.warning(res.Message);
              }
              layer.close(index);
          }, function (err) {
              $.warning(err.Message);
              layer.close(index);
          });

    })
 
})