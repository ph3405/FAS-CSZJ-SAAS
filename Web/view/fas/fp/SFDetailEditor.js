layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    var SFType = $.getQueryString('SFType');//收付类型
    var seq = $.getQueryString('seq');
    var SFDate = $.getQueryString('SFDate');
    var SFStatus = $.getQueryString('SFStatus');
    var SFMoney = $.getQueryString('SFMoney');
    var SFRemark = $.getQueryString('SFRemark');

    $("#SFDate").click(function () {
        laydate({ elem: '#SFDate', format: 'YYYY-MM-DD' });
    });
    var Unit = '';
    var editor = $('#container');
    var title = '';
    var init = function () {
        debugger;

        var data = {};
        data.Source = [];
        if (SFType == "ys") {        
            data.Source.push(
                {
                    Code: 'ys',
                    Name: '已收'
                });
            data.Source.push(
                {
                    Code: 'ws',
                    Name: '未收'
                });
            var template = $.templates("#tpl-status");

            var dataHtml = template.render(data);

            $('#SFStatus').append(dataHtml);        
        }
        else if (SFType == "yf") {
            data.Source.push(
                {
                    Code: 'yf',
                    Name: '已付'
                });
            data.Source.push(
                {
                    Code: 'wf',
                    Name: '未付'
                });
            var template = $.templates("#tpl-status");

            var dataHtml = template.render(data);

            $('#SFStatus').append(dataHtml);
        }
        if (seq != null) {
            $("#SFDate").val(SFDate);
            $("#SFStatus").val(SFStatus);
            $("#SFMoney").val(SFMoney);
            $("#SFRemark").text(decodeURI(SFRemark));
            
        }
        else {

        }
        form.render();
    };
    init();


    form.on("submit(save)", function (data) {
        var d = {};
        d.seq = seq;
        d.SFDate = data.field.SFDate;
        d.SFStatusCode = data.field.SFStatus;
        if (data.field.SFStatus =="ys") {
            d.SFStatus = "已收";
        }
        else if (data.field.SFStatus == "ws") {
            d.SFStatus = "未收";
        }
        else if (data.field.SFStatus == "yf") {
            d.SFStatus = "已付";
        }
        else if (data.field.SFStatus == "wf") {
            d.SFStatus = "未付";
        }

        d.SFMoney = data.field.SFMoney;
        d.SFRemark = data.field.SFRemark;


        parent.setSFdetail(d);
        parent.layer.closeAll();
        return false;
    })


    $('#btnCancel').click(function () {
        parent.layer.closeAll();
        //parent.clearActiveValue();
    });
    $("body").on("keyup", "[name='SFMoney']", function () {

        var $amountInput = $(this);
        var tmptxt = $(this).val();
        //$(this).val(tmptxt.subString(0,1) + '.' + tmptxt.subString(2));
        var FirstChar = tmptxt.substr(0, 1);
        //使用字符分离获取输入的第一位
        var SecondChar = tmptxt.substr(1, 2);
        // 使用字符分离获取输入的第二位
        if (FirstChar == "0") {
            SecondChar.replace(/[0,1,2,3,45,6,7,8,9]/, "0.");
        }
        //如果第一位是0，将第一位替换成0.
        // $(this).val(tmptxt.replace(/\D|^0/g,''));
        event = window.event || event;
        if (event.keyCode == 37 | event.keyCode == 39) {
            return;
        }

        //先把非数字的都替换掉，除了数字和. 
        $amountInput.val($amountInput.val().replace(/[^\d.]/g, "").
            //只允许一个小数点              
            replace(/^\./g, "").replace(/\.{2,}/g, ".").
            //只能输入小数点后两位
            replace(".", "$#$").replace(/\./g, "").replace("$#$", ".").replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'));
        //如果第一位是负号，则允许添加
        if (FirstChar == '-') {
            $amountInput.val('-' + $amountInput.val());

        }


    });
})