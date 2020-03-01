
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer =  layui.layer  ,
		laypage = layui.laypage;
    var $ = layui.jquery;

   
    var id;
    var template = $.templates("#tpl-Edit");
    var parentId = $.getQueryString('parentId');
    var dataHtml = template.render({});
    $('#editForm').html(dataHtml);
    form.render();

    form.on("submit()", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/tplmanage/FormulaUpdate";
        }
        else {
            url = "/fas/tplmanage/FormulaAdd";
        }

       
        var request = {};
        request.Data = data.field;
        request.Token = token;
 
        request.Data.Id = id;
        request.Data.ReportDetailTPLId = parentId;
        request.Token = token;
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
 

   
    

    var dealSubject = function (elem, d) {
        
      
        $('#txtSubjectCode').val( d.item.Id);//code
        $('#txtSubjectName').val(d.item.Value);//name
    };
    var bindAutocomplete = function (obj, data) {
        $(obj).autocomplete({
            source: data,

            select: function (event, ui) {

                dealSubject($(this), ui);
            }
        });
    };
    var UIInit = function () {
        var request = {};
        request.Token = token;
        var index = $.loading('初始化中');
        $.Post("/fas/set/subjectFormulaGet", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         subjectData = res.Data;
                         bindAutocomplete($("#txtSubjectName"), res.Data);
                         $.topTip(res.Message);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });

    };

    UIInit();
})
