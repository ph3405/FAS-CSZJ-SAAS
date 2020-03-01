
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var isAdd = $.getQueryString('isAdd');
    var template = $.templates("#tpl-Edit");
    var dataHtml = template.render({
    });

    $('#editForm').html(dataHtml);
    

    form.render();




   


    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/BasicData/WX_BasicDataUpdate";
        }
        else {
            url = "/fas/BasicData/WX_BasicDataAdd";
        }


        var request = {};
        request.Name = data.field.Name;
        request.UserId = userId;
        request.Token = token;
        request.DataType = 'Vendor';
        request.Id = id;
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
                //$.topTip(res.Message);
                //window.location.href = "customerAdd.aspx";
                if (isAdd == "1") {
                    layer.closeAll('dialog'); //关闭信息框
                    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    parent.layer.close(index); //再执行关闭  
                }
                else {
                    $.topTip(res.Message);
                    window.location.href = "vendorAdd.aspx";
                }

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });




        return false;
    })



    var dealSubject = function (codeEle,nameEle, d) {
        $(codeEle).val(d.item.Id);//code
        $(nameEle).val(d.item.Value);//name
    };


    var bindAutocomplete = function (objEle,valEle, data) {
        $(objEle).autocomplete({
            source: data,

            select: function (event, ui) {

                dealSubject(valEle, objEle, ui);
            }
        });
    };
    var UIInit = function () {
        var request = {};
        request.Token = token;
        var index = $.loading('初始化中');
        $.Post("/fas/set/subjectTotalGet", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         subjectData = res.Data;
                         bindAutocomplete($("#txtDCostSubjectName"),$('#txtDCostSubjectCode'), res.Data);

                         bindAutocomplete($("#txtGDName"), $('#txtGDCode'), res.Data);

                         bindAutocomplete($("#ADSubjectName"), $('#ADSubjectCode'), res.Data);
                         //$.topTip(res.Message);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });

    };

    UIInit();
})
