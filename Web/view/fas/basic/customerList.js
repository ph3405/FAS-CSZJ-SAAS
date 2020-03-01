layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
        $ = layui.jquery;

    var searchData = 0;
    var query = function (pageIndex) {
        searchData = 0;
        var index = $.loading('查询中');
        var request = {};
        request.DataType = 'Customer';
        request.UserId = userId;
        request.Name = $("#txtName").val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/BasicData/BasicDataSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    searchData = res.Data.length;

                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

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
    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Id = id;
        //request.Token = token;

        $.Post("/fas/BasicData/WX_BasicDataDelete", request,
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


    query(1);

    //查询
    $(".search_btn").click(function () {

    
            query(1);
    })

    //添加
    $("#btnAdd").click(function () {
        //$.open("增加资产", "fixedAssetsAdd.aspx");
        $.open("增加客户", "customerAdd.aspx", undefined, function (a, b) {

            query(1);

        });
    })


    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        $.open('编辑客户', "customerEditor.aspx?id=" + id);

    })

    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此客户？', function () {


            rowDel(_this.attr("data-id"));

        });
    })

 


    //当前日期
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
    //去0
    function noZero(val) {
        if (val == 0) {
            return "";
        }
        else {
            //return val.toString().replace('-', '');
            return val.toString();
        }
    }

    function thousand(val) {
        if (val == 0 || val == "") {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
        }
    }

    $('#btnImport').click(function () {
        $.dialog('导入', 'attachment.aspx', undefined, function () {
            query(1);
        });

    });

})