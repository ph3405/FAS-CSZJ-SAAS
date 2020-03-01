layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage,
        $ = layui.jquery;

    //全选  
    form.on('checkbox(allChoose)', function (data) {
        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
        child.each(function (index, item) {
            item.checked = data.elem.checked;
        });
        form.render('checkbox');
    });

    $.views.converters("type", function (val) {

        if (val == 0) {
            return '雇主企业';
        }
        else if (val == 1) {
            return "代账企业";
        }
        else {
            return "未设置";
        }

    });


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        request.Name = $('#txtName').val().trim();
        $.Post('/fas/set/opAccountListGet', request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {


                    var template = $.templates("#tpl-list");

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);
                    $("#dt > tr").dblclick(function () {
                        var row = $(this);
                        var code = $.trim(row.find('.tks-code').text());
                        var name = $.trim(row.find('.tks-name').text());
                        parent.setValue(code, name);
                        parent.layer.closeAll();
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


    //查询
    $(".search_btn").click(function () {
        query(1);
    });

 
    //确认
    $(".OK").click(function () {
        var arr_code = new Array();
        var arr_name = new Array();
        $("#dt > tr").each(function () {
            var row = $(this);
            if (row.find("input[type=checkbox]:checked").val() != undefined) {
                var valCode = $.trim(row.find('.tks-code').text());
                var valName = $.trim(row.find('.tks-name').text());
                arr_code.push(valCode);
                arr_name.push(valName);
            }
            
        });
        if (arr_code.length>0) {
            parent.setValue(arr_code, arr_name);
            parent.layer.closeAll();
        }

    })


})