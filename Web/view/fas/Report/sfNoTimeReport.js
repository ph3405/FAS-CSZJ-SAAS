layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("thousand", function (val) {

        if (val == 0) {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
        }
    });

    $("#StartSFDate").click(function () {
        laydate({ elem: '#StartSFDate', format: 'YYYY-MM-DD' });
    });
    $("#EndSFDate").click(function () {
        laydate({ elem: '#EndSFDate', format: 'YYYY-MM-DD' });
    });

    var account = "";

    var query = function (pageIndex) {

        var index = $.loading('计算中');
        var request = {};
        //request.InvoiceNo = $('#InvoiceNo').val();
        //request.BasicDataName = $('#BasicDataName').val();
        //request.StartSFDate = $('#StartSFDate').val();
        //request.EndSFDate = $('#EndSFDate').val();
        request.AccountId = account;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/fp/SFNoTimeListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var html1 = template.render(res.Data);

                    $('#dt1').html(html1);

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
    
    window.query = query;

    var Init = function () {

        var index = $.loading('初始化');
        var request = {};
     
        request.Token = token;

        $.Post("/fas/period/PeriodPaidGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-select");
                    account = res.Account.Id;

                    query(1);
                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    }
    
    Init();//初始化期间

    $("#btnSearch").click(function () {
        query(1);
        
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        // $.open('凭证编辑', "pzEditor.aspx?id=" + id);

        parent.layer.open({
            type: 2,
            title: '发票编辑',
            shade: 0.1,
            area: ['1100px', '600px'],
            content: 'view/fas/fp/fpEditor.aspx?id=' + id,
            cancel: function (index, layero) {
                query(1);
            }
        });

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此发票？', function () {


            rowDel(_this.attr("data-id"));
            query(1);
        });
    })

    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Id = id;
        request.Token = token;

        $.Post("/fas/fp/WX_SFInvoiceDelete", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1);
                    });

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };



    function CurentTime() {
        var now = new Date();

        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日

        var clock = year + "-";

        if (month < 10)
            clock += "0";

        clock += month + "-";
        if (day < 10)
            clock += "0";

        clock += day;
        return (clock);
    }

})