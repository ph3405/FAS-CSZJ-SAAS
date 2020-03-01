layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;


    var subjectId = $.getQueryString('id');
    var isCurrency = 0;
    var isQuantity = 0;
    var isCalHelper = 0;
    var Unit = '';
    var editor = $('#container');
    var title = '';
    var init = function () {

        var request = {};
        request.Token = token;
        request.Id = subjectId;
        var index = $.loading('初始化中');
        $.Post("/fas/set/SubjectAssGet", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         
                         $(editor).html("");
                         isCalHelper = res.IsCalHelperValid;
                         isCurrency = res.IsCurrencyValid;
                         isQuantity = res.IsQuantityValid;
                         Unit = res.QuantityValue;
                         title = res.Code + " " + res.Name;
                         dealQuantity(res);
                         dealCurrency(res);
                         dealCalHelper(res);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });


    };
    init();
    var dealQuantity = function (res) {
        if (res.IsQuantityValid == true) {
            var template = $.templates("#tpl-quantity");

            var dataHtml = template.render({ Quantity: 0, Price: 0 });

            $(editor).append(dataHtml);

        }
    };

    var dealCurrency = function (res) {
        if (res.IsCurrencyValid == true) {
            var template = $.templates("#tpl-currency");

            var data = {};
            data.currency = [];
            data.Rate = 1;
            data.YB = 0;
            for(var c in res.Currency){
                data.currency.push(
                    {
                        Code: res.Currency[c].Item.Code,
                        Value: res.Currency[c].Item.Value,
                        Rate: res.Currency[c].Rate,
                        select: res.Currency[c].Item.Select
                    });
            }
            

            var dataHtml = template.render(data);

            $(editor).append(dataHtml);
            form.render();
        }
    };

    var dealCalHelper = function (res) {
        if (res.IsCalHelperValid == true) {
            var template = $.templates("#tpl-calHelper");

            var dataHtml = template.render(res.CalHelper);

            $(editor).append(dataHtml);
            form.render();
            $(".rowAdd").click(function () {
                var id = $(this).attr('data-id');
                var value = $(this).attr('data-value');
                parent.layer.open({
                    type: 2,
                    title: '新增' + value,
                    shade: 0.1,
                    area: ['500px', '400px'],
                    content: '/view/fas/set/calData/customDataAdd.aspx?itemId=' + id,
                    cancel: function (index, layer) {

                    },
                    end: function () {
                        init();
                    }
                });
            })
        }

       
    };

    form.on("submit(save)", function (data) {
        var d = {};
        d.SubjectCode = subjectId;
        d.SubjectDescription = title;
        d.Quantity = data.field.Quantity;
        d.Price = data.field.Price;
        d.CurrencyCode = data.field.CurrencyCode;
        d.Rate = data.field.Rate;
        d.YB = data.field.YB;
        d.IsQuantity = isQuantity?1:0;
        d.IsCurrency = isCurrency ? 1 : 0;
        d.IsCalHelper = isCalHelper ? 1 : 0;
        d.CalValue1 = '';
        d.Unit = Unit;
        var p1=0;
        if (isQuantity) {
            p1 = d.Quantity * d.Price
        }
        var p2 = 0;
        if (isCurrency) {
            p2 = d.Rate * d.YB
        }


        d.Balance = p1 +p2;

        for (var item in data.field) {
            if (item.indexOf('cal') > -1) {
                d.SubjectDescription += '-' + data.field[item];
                var calID=item.split("-")[1];
                var code=data.field[item].split(" ")[0];
                d.CalValue1 += calID + "," + code+"#";

            }


        }

        parent.setActiveValue(d);
        parent.layer.closeAll();
        return false;
    })


    $('#btnCancel').click(function () {
        parent.layer.closeAll();
        parent.clearActiveValue();
    });
})