
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

        $.Post("/fas/tplmanage/FPtplGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-editor");

                  var dataHtml = template.render(res.Data);

                  $('#Form').html(dataHtml);

                  $('#btnTplChoose').click(function () {
                      $.dialog("模板选择", '/view/fas/tplmanage/fptpl/tplChoose.aspx');

                  });
                  var RPStatus = $('#RPStatus').val();
                  if (RPStatus == "0") {
                      $('#ZFType').hide();
                  }
                  else {
                      $('#ZFType').show();
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

    form.on('select(RPStatus)', function (data) {
        var RPStatus = $('#RPStatus').val();
        if (RPStatus == "0") {
            $('#ZFType').hide();
        }
        else {
            $('#ZFType').show();
        }

        form.render('select');
    }); 
    form.on("submit(update)", function (data) {

        console.log(data.field);

        var request = {};
        request.Data = data.field;
        request.Data.Id = id;
        request.Token = token;
        var RPStatus = $('#RPStatus').val();
        if (RPStatus == "0") {
            $('#ZFType').hide();
            request.Data.PayMode = "-1"
        }
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/tplmanage/FPtplUpdate", request,
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

  
    window.setValue = function (code, name) {
        $('#txtTPLId').val(code);
        $('#txtTPLName').val(name);
    }
})
