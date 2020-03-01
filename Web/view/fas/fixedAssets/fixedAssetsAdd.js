
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var template = $.templates("#tpl-Edit");
    var NowDate = $.getQueryString('NowDate');//当前期间
    var dataHtml = template.render({
        DCostSubjectCode: '5602007',
        DCostSubjectName: '5602007 管理费用-折旧费',
        GDCode: '1501',
        GDName: '1501 固定资产',
        InputVAT:0,
        ScrapValueRate: 10.00,
        PreUseMonth: 240,
        RemainderUseMonth: 240,
        AccumulativeDpre: 0,
        DpreMonth: 0,
        AccumulativeDpre_Y:0
    });

    $('#editForm').html(dataHtml);
    

    form.render();

    $("#txtStartUseDate").click(function () {
        laydate({ elem: '#txtStartUseDate', format: 'YYYY-MM-DD', max: NowDate, istoday: false });
        //laydate.render({
        //    elem: '#txtStartUseDate', //指定元素
        //    format: 'YYYY-MM-DD'
        //});
    });

    function calculate() {
        var val = $('#txtInitialAssetValue').val();//资产原值
        var flag1 = $.isNumeric(val);

        var rate = $('#txtScrapValueRate').val();//残值率
        var flag2 = $.isNumeric(rate);
        var yjcz=0;
        if (flag1 && flag2) {

            yjcz = (val * rate / 100).toFixed(2);
            $('#txtScrapValue').val(yjcz);//预计残值
        }

        //剩余使用月份

        var preMonth = $('#txtPreUseMonth').val();
        var useMonth = $('#txtDpreMonth').val();
        var flag4 = $.isNumeric(preMonth);
        var flag5 = $.isNumeric(useMonth);
        var month ;
        if (flag4 && flag5) {
            month = preMonth - useMonth;
            $('#txtRemainderUseMonth').val(month);
        }

        //每月折旧额
        month= $('#txtRemainderUseMonth').val();
        var flag3 = $.isNumeric(month);
        if (flag1 && flag2 && flag3) {
            var dpre = ((val - yjcz) / month).toFixed(2);
            $('#txtDprePerMonth').val(dpre);
        }

        var ljzj = $('#txtAccumulativeDpre').val();//累计折旧
        var flag6 = $.isNumeric(ljzj);
        if (flag1 && flag2 && flag3 && flag6) {
            var dpre = ((val - yjcz-ljzj) / month).toFixed(2);
            $('#txtDprePerMonth').val(dpre);
        }

        var curLjzj = $('#txtAccumulativeDpre_Y').val();//本年累计折旧
        var flag7 = $.isNumeric(curLjzj);
        if (flag6 && flag7) {
            var preLjzj = ljzj - curLjzj;
            $('#txtPreviousAccumulativeDpre').val(preLjzj);
        }
    }

    $("#txtInitialAssetValue,#txtScrapValueRate,#txtDpreMonth,#txtPreUseMonth,#txtAccumulativeDpre,#txtAccumulativeDpre_Y").change(function () {
        calculate();
    });


   


    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/FixedAssets/FixedAssetsUpdate";
        }
        else {
            url = "/fas/FixedAssets/FixedAssetsAdd";
        }


        var request = {};
        request.Data = data.field;
        request.Token = token;
        request.Data.Id = id;
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
                $.topTip(res.Message);
                window.location.href = "fixedAssetsAdd.aspx?NowDate=" + NowDate;
                //id = res.Id;
                //var dataHtml = template.render({});

                //$('#editForm').html(dataHtml);
                
                //form.render();
                //UIInit();
                //form.render();
                //$("#txtStartUseDate").click(function () {
                //    laydate({ elem: '#txtStartUseDate', format: 'YYYY-MM-DD' });

                //});
                //$.topTip(res.Message);
                //$.info(res.Message + ",点击确定,返回列表页", function () {
                //    if (parent.query)
                //        parent.query(1);
                //    parent.layer.closeAll();
                //})

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
    //input输入框回车禁止提交表单
    document.getElementsByTagName('form')[0].onkeydown = function (e) {
        var e = e || event;
        var keyNum = e.which || e.keyCode;
        return keyNum == 13 ? false : true;
    };
})
