
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var parentId = $.getQueryString('parentId');
    var category=$.getQueryString('category');
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({ Category :category});

    $('#editForm').html(dataHtml);


    form.render();



 
    var initUI = function () {

        var request = {};
        request.Token = token;
        request.ParentId = parentId;
        var index = $.loading('初始化中');
        $.Post("/fas/set/subjectUIInit", request, function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {

                var template = $.templates("#tpl-currency");

                var dataHtml = template.render(res.Currency);

                $("#currencyContainer").html(dataHtml);

                template = $.templates("#tpl-calItem");

                dataHtml = template.render(res.CalItem);

                $("#calItemContainer").html(dataHtml);

                $('#selCreditDebit').val(res.ParentSubject.Credit_Debit);

                form.render();
            }
 
        }, function (err) {
            layer.close(index);
            $.warning(err.Message);
        });
    }

    initUI();

    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/set/subjectUpdate";
        }
        else {
            url = "/fas/set/subjectAdd";
        }


        var request = {};
        request.Data = data.field;

        if (data.field.IsQuantityValid == 'on') {
            data.field.IsQuantityValid = 1;
        }
        else {
            data.field.IsQuantityValid = 0;
            request.Data.QuantityValue = '';
        }
        if (data.field.IsCalHelperValid == 'on') {
            data.field.IsCalHelperValid = 1;
            request.CalItem=  getCheckedVal('cal', data.field);
        }
        else {
            data.field.IsCalHelperValid = 0;
            request.CalItem = new Array();
        }
        if (data.field.IsCurrencyValid == 'on') {
            data.field.IsCurrencyValid = 1;
            request.Currency = getCheckedVal('currency', data.field);
        }
        else {
            data.field.IsCurrencyValid = 0;
            request.Currency = new Array();
        }
      
        request.Data.ParentId = parentId;
        request.Token = token;
        request.Data.Id = id;
        request.checkSub = "0";
        //弹出loading
        var index = $.loading('数据提交中，请稍候');
        $.Post("/fas/set/CheckSubject", request, function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                var index1 = $.loading('数据提交中，请稍候');
                if (res.Message!="") {
                    $.confirm(res.Message, function () {
                        request.checkSub ="1"
                        $.Post(url, request, function (data) {
                            var res = data;
                            layer.close(index1);
                            if (!res.IsSuccess) {
                                $.warning(res.Message);
                            }
                            else {
                                //id = res.Id;
                           
                                var dataHtml = template.render({ Category: category });
                                $('#editForm').html(dataHtml);
                                initUI();
                                form.render();
                                $.topTip("新增成功");
                                //$.info(res.Message + ",点击确定,返回列表页", function () {
                                //    if (parent.query)
                                //        parent.query(1, category);
                                //    parent.layer.closeAll();
                                //})

                            }


                        }, function (err) {

                            layer.close(index1);
                            $.warning(err.Message);
                        });
                    });
                }
                else {
                    $.Post(url, request, function (data) {
                        var res = data;
                        layer.close(index1);
                        if (!res.IsSuccess) {
                            $.warning(res.Message);
                        }
                        else {
                            //id = res.Id;

                            var dataHtml = template.render({ Category: category });

                            $('#editForm').html(dataHtml);
                            initUI();
                            form.render();
                            $.topTip("新增成功");
                            //$.info(res.Message + ",点击确定,返回列表页", function () {
                            //    if (parent.query)
                            //        parent.query(1, category);
                            //    parent.layer.closeAll();
                            //})

                        }


                    }, function (err) {

                        layer.close(index1);
                        $.warning(err.Message);
                    });
                }

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });
        


        




        return false;
    });


    var getCheckedVal = function (type,fields) {
        var arr = new Array();
        for(var p in fields){
            if (p.indexOf(type) > -1) {
                var val = p.split("-");
                if (val.length > 1) {
                    arr.push(val[1]);
                }
            }
        }
        return arr;
    };

})
