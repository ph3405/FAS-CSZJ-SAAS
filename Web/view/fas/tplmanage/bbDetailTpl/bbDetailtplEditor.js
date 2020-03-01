
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');

    var init = function (id) {
        var request = {};
        request.Data = {
            Id: id
        };
        request.Token = token;

        $.Post("/fas/tplmanage/BBDetailtplGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-editor");
                
                  var dataHtml = template.render(res.Data);

                  $('#Form').html(dataHtml);

                  if (res.Data.SourceType == 1) {
                      $('#btnFormula').hide();
                  }

                  form.render();
                  layer.msg(res.Message);
              }
              else {
                  $.warning(res.Message);
              }

          }, function (err) {
              $.warning(err.Message);

          });
    };
    init(id);


    form.on("submit()", function (data) {

        console.log(data.field);
        var request = {};
        request.Data = data.field;
        request.Data.Id = id;
        request.Token = token;
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/tplmanage/bbDetailtplUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message + ",点击确定,返回列表页", function () {
                    if (parent.query  )
                        parent.query(1);
                    parent.layer.closeAll();
                })

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });






        return false;
    })

    form.on('select(SourceType)', function (data) {
        if (data.value == 0) {
            $('#btnFormula').show();
        }
        else {
            $('#btnFormula').hide();
        }
    });

    $("body").on("click", "#btnFormula", function () {
        if (id == '' || id == undefined) {
            $.warning('请先保存');
            return;
        }

        layer.open({
            type: 2,
            title: '公式管理',
            shade: 0.1,
            area: ['800px', '500px'],
            content: '/view/fas/tplmanage/formula/formulaList.aspx?id=' + id,
            cancel: function (index, layero) {
                
            }
        });
    });
    
})
