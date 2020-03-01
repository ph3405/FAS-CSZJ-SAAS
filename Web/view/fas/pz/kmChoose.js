layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    form.render();
    $.views.converters("type", function (val) {
        if (val == 0) {
            return "日常支出";
        }
        else if (val == 1) {
            return "采购";
        }
        else if (val == 2) {
            return "销售";
        }
        else if (val == 3) {
            return "工资";
        }
        else if (val == 4) {
            return "税金";
        }
        else if (val == 5) {
            return "折旧和摊销";
        }
    });
   

    var query = function (pageIndex) {
     
        var index = $.loading('查询中');
        var request = {};
        request.Category = $('#Category').val();
        request.Token = token;
        request.Code = $('#Code').val();
        request.ShowTitle = $('#ShowTitle').val();

        $.Post("/fas/set/subjectTotalGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
            

                    var template = $.templates("#tpl-list");
                    console.log(res.Data);
                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

                    $("#dt > tr").dblclick(function () {
                        
                        var row = $(this);
                        var id = row.attr('data-id');
                        var value = row.attr('data-value');
                        //var Credit_Debit = row.attr('data-CreditDebit');
                        var IsCalHelperValid = row.attr('data-IsCalHelperValid');
                        var IsCurrencyValid = row.attr('data-IsCurrencyValid');
                        var IsQuantityValid = row.attr('data-IsQuantityValid');
                        //var label = row.attr('data-label');
                        var d = {};
                        d.IsCalHelperValid = IsCalHelperValid;
                        d.IsCurrencyValid = IsCurrencyValid;
                        d.IsQuantityValid = IsQuantityValid;
                        d.Id = id;
                        d.value = value;
                        //parent.selectSubject({ id: id, value: value, IsCalHelperValid: IsCalHelperValid, IsCurrencyValid: IsCurrencyValid, IsQuantityValid: IsQuantityValid});
                        parent.selectSubject(d);
                        if (d.IsCalHelperValid == 1 ||
                            d.IsCurrencyValid == 1 ||
                            d.IsQuantityValid == 1) {

                        }
                        else {
                            parent.layer.closeAll(); 
                        }
                       
                    });

                    laypage({
                        curr: pageIndex,
                        cont: "page",
                        pages: Math.ceil(res.Total / 10),
                        jump: function (obj, first) {
                            if (!first) {
                                query(obj.curr);
                            }
                        }
                    });

                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    query(1);
    window.query = query;
 

    
    //查询
    $(".search_btn").click(function () {
        query(1);
    });

 


 

})