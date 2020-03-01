
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;



    var init = function (success) {
        var request = {};

        request.Token = token;

        $.Post("/fas/qm/QMSYStatusGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {


                  $('#container').html(res.Message);


                  form.render();
                  if (success) {
                      success();
                  }
              }
              else {
                  $.warning(res.Message);

              }

          }, function (err) {
              $.warning(err.Message);

          });
    };
    init();
    window.init = init;

    $('#btnGenPZ').click(function () {
        var request = {};

        request.Token = token;
        var index = $.loading('结转损益中');
        $.Post("/fas/qm/QMSYGen", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {


                  $('#container').html(res.Message);


                  form.render();

              }
              else {
                  $.warning(res.Message);
              }
              layer.close(index);
          }, function (err) {
              $.warning(err.Message);
              layer.close(index);
          });
    });

    $('#btnNext').click(function () {
        init(function () {
            window.location.href = "qm3.aspx";
        });
    });

    $('#btnPre').click(function () {
        window.location.href = "qm1.aspx";
    });

})
