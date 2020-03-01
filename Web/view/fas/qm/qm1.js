
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;


    //结转生产成本，放在第二步做
    form.on("submit(next)", function (data) {
       
        var request = {};
        request.SCCBCode = data.field.code;//结转生产成本科目对应的科目
        request.Token = token;
        var index = $.loading('生成结转生产成本中');
        $.Post("/fas/qm/QMSCCBGen", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {

                  window.location.href = "qm2.aspx";
              }
              else {
                  $.warning(res.Message);
              }
              layer.close(index);
          }, function (err) {
              $.warning(err.Message);
              layer.close(index);
          });

      
        return false;
    });


    $('#btnPre').click(function () {
        window.location.href = "qmEditor.aspx";
    });

})
