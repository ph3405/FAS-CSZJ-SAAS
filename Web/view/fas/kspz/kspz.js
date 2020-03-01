
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

   

    var init = function () {
        var request = {};

        request.Token = token;
 
        $.Post("/fas/KSPZ/KSPZTPLsGet", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-Edit");

                  var dataHtml = template.render(res.Data);

                  $('#container').html(dataHtml);

                  BindEvent();
                  form.render();
                
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

    function BindEvent() {
        $('.btnGenPZ').click(function () {
            var tplId = $(this).attr('data-id');
            var ul = $(this).parent().find('.money')[0];
            var money = $(ul).find('.input-money')[0].value;
            if (!checkRate(money))
            {
                $.warning('请输入有效金额');
                return;
            }

            parent.layer.open({
                type: 2,
                title: '凭证编辑',
                shade: 0.1,
                area: ['1200px', '600px'],
                content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId+"&type=KS&money="+money,
                cancel: function (index, layer) {
 
                }
            });
        });

        $('.btnPZEdit').click(function () {
            var pzid = $(this).attr('data-id');
 
            parent.layer.open({
                type: 2,
                title: '凭证编辑',
                shade: 0.1,
                area: ['1200px', '600px'],
                content: '/view/fas/pz/pzEditor.aspx?id=' + pzid ,
                cancel: function (index, layer) {
                     
                }
            });
        });

    }

    function checkRate(nubmer) {
        //var re = /^[1-9]+[0-9]*]*$/;  //判断正整数 /^[1-9]+[0-9]*]*$/ 
        var re = /^[0-9]+([.]{1}[0-9]{1,2})?$/
        if (!re.test(nubmer)) {
 
            return false;
        }
        else {
            return true;
        }
    }

    //期末结转
    $('#btnSave').click(function () {
        var request = {};

        request.Token = token;
 
        $.Post("/fas/qm/QMCarryOver", request,
          function (data) {
              var res = data;
              if (res.IsSuccess) {
                  $.info(res.Message, function () {
                      if (parent.query)
                          parent.query(1);
                      parent.layer.closeAll();
                  });

              }
              else {
                  $.warning(res.Message);
              }

          }, function (err) {
              $.warning(err.Message);

          });
    });

})
