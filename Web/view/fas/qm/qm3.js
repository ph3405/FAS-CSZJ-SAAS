
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    

    form.on("submit(jz)", function (data) {

        var request = {};
    
        request.Token = token;
        var index = $.loading('结账中');
        $.Post("/fas/qm/QMCarryOver", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  $.info(res.Message, function () {
                      if (parent.query)
                          parent.query(1);
                      parent.layer.closeAll();
                  });
                  parent.parent.opAccountListGet();
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
        window.location.href = "qm2.aspx";
    });

})
