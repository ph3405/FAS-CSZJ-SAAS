layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    //$.views.converters("thousand", function (val) {

    //    if (val == 0 || val == undefined || val == null) {
    //        return "";
    //    }
    //    else {
    //        return numeral(val).format('0,0.00');
    //    }
    //});

    var account;

    var query = function (PeriodId) {

        //var index = $.loading('计算中');
        var request = {};
        request.Token = token;
        request.PeriodId = PeriodId
        $.Post("/fas/report/SJGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var html1 = template.render(res);
                    printData1 = res;
                    account = res.Account;
                    $('#dt2').html(html1);


                    form.render();
                } else {
                    $.warning(res.Message);
                }
                //layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                //layer.close(index);
            });
    };

    window.query = query;

   
    var Init = function () {

        var request = {};
        request.Token = token;
        //var index = $.loading('账套获取中');
        $.Post('/fas/set/opAccountListGet', request, function (data) {
            var res = data;
            //layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                if (res.IsSelected) {
                    $("#mainSJ_Period").html(res.Year + '年' + res.Month + "月应交税金");
                    $("#mainJY_Period").html(res.Year + '年' + res.Month + "月经营数据");
                    var request = {};
                    request.Token = token;
                    request.PeriodId = res.PeriodId;
                    $.Post("/fas/report/JYGet", request,
                        function (data) {
                            var res = data;
                            if (res.IsSuccess) {

                                
                                var template = $.templates("#tpl-list1");
                           
                                var html1 = template.render(res);
                                $('#dt1').html(html1);


                                form.render();
                            } else {
                                $.warning(res.Message);
                            }
                            //layer.close(index);
                        }, function (err) {
                            $.warning(err.Message);
                            //layer.close(index);
                        });
                    
                    query(res.PeriodId);
                }
                
                form.render();
            }


        }, function (err) {
            $.warning(err.Message);
            layer.close(index);

        });
    }

    Init();//初始化期间

    $("#btnSearch").click(function () {


        query();

    })

 
 
})