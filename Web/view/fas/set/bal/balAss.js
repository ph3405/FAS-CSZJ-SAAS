layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender','formSelects'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;
    var formSelects = layui.formSelects;
   
    var subjectCode = $.getQueryString('id');
    var balId = $.getQueryString('balId');
    var isCalHelper = 0;
    var Unit = '';
    var editor = $('#container');
    var title = '';
    var init = function () {

        var request = {};
        request.Token = token;
        request.Id = subjectCode;
        var index = $.loading('初始化中');
        $.Post("/fas/set/SubjectAssGet", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         isCalHelper = res.IsCalHelperValid;
                         isCurrency = res.IsCurrencyValid;
                         isQuantity = res.IsQuantityValid;

                         Unit = res.QuantityValue;
                         title = res.Code + " " + res.Name;
                         
                         dealCalHelper(res);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });


    };
    init();

    var dealCalHelper = function (res) {
        if (res.IsCalHelperValid == true) {
            var template = $.templates("#tpl-calHelper");
            
            var arr_cal = [];
            var tb = $("#dt", parent.document);
            tb.find("tr").each(function (i) {
   
                var CalItem = $(this).find('td:eq(0) input').val().split("-")[0];
                var CalValue = $(this).find('td:eq(0) input').val().split("-")[1];
                var SubjectCode = $(this).find('td:eq(0) input').attr('data-code');
                if (CalItem != "" && CalValue != "" && CalItem == res.CalHelper[0].Item.Code && SubjectCode == res.Code) {
                    arr_cal.push(CalValue);
                }

            })
 
            var arrSource = res.CalHelper[0].Source;
            var newSource = [];
            for (var i = 0; i < arrSource.length; i++) {
                var cal = arrSource[i].Code;
                if ($.inArray(cal, arr_cal) == -1) {
                    newSource.push(arrSource[i]);
                }
            }

            res.CalHelper[0].Source = newSource;
           
            var dataHtml = template.render(res.CalHelper);

            $(editor).append(dataHtml);
            form.render();
            formSelects.render('selectId');
        }
    };

    form.on("submit(save)", function (data) {
        var sel = formSelects.value('selectId', 'val');
        if (sel.length == 0) {
            $.warning("请选择项目");
            return false;
        }
        var calID = Object.keys(data.field)[0].split("-")[1];

        var arr_data = [];
        for (var i = 0; i < sel.length; i++) {
            var dd = new TKS_FAS_FGLBalance();
            dd.BALId = balId;
            dd.SubjectId = subjectCode;
            dd.CalValue1 = '';
            //var calID = data.field.split("-")[1];
       
            var code = sel[i].split(" ")[0];
            var name = sel[i].split(" ")[1];
            dd.CalValue1 += calID + "," + code + "," + name + "#";
            arr_data.push(dd);
        }
        var d = new TKS_FAS_FGLBalance();
        d.BALId = balId;
        d.SubjectId = subjectCode;
        d.CalValue1 = '';
      
       
        for (var item in data.field) {
            if (item.indexOf('cal') > -1) {
                d.SubjectDescription += '-' + data.field[item];
                var calID = item.split("-")[1];
                var code = data.field[item].split(" ")[0];
                var name = data.field[item].split(" ")[1];
                d.CalValue1 += calID + "," + code +","+name+ "#";

            }
        }
      
        var request = {};
        request.Token = token;
        request.Data = d;
        request.lst = arr_data;
        var index = $.loading('提交数据中');
        $.Post("/fas/set/balAdd", request,
                 function (data) {
                     var res = data;
                     layer.close(index);
                     if (!res.IsSuccess) {
                         $.warning(res.Message);
                     }
                     else {
                         parent.query(1, 1, "RMB");
                         parent.layer.closeAll();
                     }



                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });

     
        return false;
    })


    $('#btnCancel').click(function () {
        parent.layer.closeAll();

    });


    function TKS_FAS_FGLBalance() {
        this.Id = '',
		this.ParentId = '',
		this.AccountId = '',
		this.SubjectCode = '',
		this.Name = '',
		this.PeriodId = '',
		this.Year = '',
		this.CalItem1 = '',
		this.CalValue1 = '',
		this.CalItem2 = '',
		this.CalValue2 = '',
		this.CalItem3 = '',
		this.CalValue3 = '',
		this.CalItem4 = '',
		this.CalValue4 = '',
		this.CalItem5 = '',
		this.CalValue5 = '',
		this.CurrencyCode = '',
		this.SCredit_Debit = '',
		this.NUMStartBAL = '',
		this.BWBStartBAL = '',
		this.YBStartBAL = '',
		this.NUMDebitTotal = '',
		this.NUMDebitTotal_Y = '',
		this.BWBDebitTotal = '',
		this.BWBDebitTotal_Y = '',
		this.YBDebitTotal = '',
		this.YBDebitTotal_Y = '',
		this.NUMCreditTotal = '',
		this.NUMCreditTotal_Y = '',
		this.BWBCreditTotal = '',
		this.BWBCreditTotal_Y = '',
		this.YBCreditTotal = '',
		this.YBCreditTotal_Y = '',
		this.ECredit_Debit = '',
		this.NUMEndBAL = '',
		this.BWBEndBAL = '',
		this.YBEndBAL = '',
		this.Category = '',
		this.Unit = '',
		this.YearStartNumBAL = '',
		this.YearStartYBBAL = '',
		this.YearStartBWBBAL = '',
		this.CreateUser = '',
		this.CreateDate = '',
		this.IsDefaultCurrency = '',
		this.IsQuantityValid = '',

        this.SubjectId = '';

    }
})