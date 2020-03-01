layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer ,
		laypage = layui.laypage,
		$ = layui.jquery;


    $.views.converters("type", function (val) {

        if (val == 0) {
            return "办公劳保费";
        }
        else if (val == 1) {
            return "餐饮费-非客户";
        }
        else if (val == 2) {
            return "餐饮费-客户";
        }
        else if (val == 3) {
            return "电话费";
        }
        else if (val == 4) {
            return "汽车过路费";
        }
        else if (val == 5) {
            return "汽车加油费";
        }
        else if (val == 6) {
            return "汽车维修保养费费";
        }
        else if (val == 7) {
            return "电脑等办公电子产品";
        }
        else if (val == 8) {
            return "礼品费";
        }
        else if (val == 9) {
            return "银行手续费";
        }
        else if (val == 10) {
            return "维修及工具";
        }
        else if (val == 11) {
            return "广告费";
        }
        else if (val == 12) {
            return "酒店住宿费";
        } else if (val == 13) {
            return "停车费";
        } else if (val == 14) {
            return "生产耗材费用";
        } else if (val == 15) {
            return "生产设备";
        } else if (val == 16) {
            return "建筑及办公室维修费";
        } else if (val == 17) {
            return "水电天然气费";
        } else if (val == 18) {
            return "政府税费";
        } else if (val == 19) {
            return "高铁飞机大巴等交通费";
        }
        else if (val == 20) {
            return "运输费";
        }
        else if (val == 21) {
            return "保险费";
        }
        else if (val == 22) {
            return "包装费等耗材";
        }
        else if (val == 23) {
            return "咨询费";
        }
        else if (val == 24) {
            return "材料及产品采购";
        }
        else if (val == 25) {
            return "销售发票";
        }
        else if (val == 26) {
            return "其他";
        }


    });

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
     
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/set/FPTPLSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


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
    query(1);
    window.query = query;
    var rowDel = function (id) {
       
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/tplmanage/FPtplDel", request,
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
    }
    //查询
    $(".search_btn").click(function () {
        query(1);
    })
 
    
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');

        parent.layer.open({
            type: 2,
            title: '模板编辑',
            shade: 0.1,
            area: ['1200px', '600px'],
            content: 'view/fas/tplmanage/tpl/tplEditor.aspx?type=custom&id=' + id,
            cancel: function (index, layero) {

            }
        });

      

    })

 
})