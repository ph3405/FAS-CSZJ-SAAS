
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer =  layui.layer  ,
		laypage = layui.laypage;
    var $ = layui.jquery;

   
    var id;
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({});
    $('#editForm').html(dataHtml);
    form.render();
    var init = function () {
        var RPStatus = $('#RPStatus').val();
        if (RPStatus == "0") {
            $('#ZFType').hide();
        }
        else {
            $('#ZFType').show();
        }
    }

    init();
    form.on("submit()", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/tplmanage/FPtplUpdate";
        }
        else {
            url = "/fas/tplmanage/FPtplAdd";
        }

       
        var request = {};
        request.Data = data.field;
        request.Token = token;
 
        request.Data.Id = id;
        request.Token = token;
        var RPStatus = $('#RPStatus').val();
        if (RPStatus == "0") {
            $('#ZFType').hide();
            request.Data.PayMode="-1"
        }

        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post(url, request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                id = res.Id;
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
    form.on('select(RPStatus)', function (data) {
        var RPStatus = $('#RPStatus').val();
        if (RPStatus=="0") {
            $('#ZFType').hide();
        }
        else {
            $('#ZFType').show();
        }

        form.render('select');
    }); 
    $('#btnTplChoose').click(function () {
        $.dialog("模板选择", '/view/fas/tplmanage/fptpl/tplChoose.aspx');

    });

    window.setValue = function (code,name) {
        $('#txtTPLId').val(code);
        $('#txtTPLName').val(name);
    }
})
