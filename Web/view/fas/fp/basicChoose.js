layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
        $ = layui.jquery;
    var DataType = $.getQueryString('DataType');
    $.views.converters("type", function (val) {
       
        if (val == 0) {
            return '雇主企业';
        }
        else if(val==1)
        {
            return "代账企业";
        }
        else
        {
            return "未设置";
        }
       
    });


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Name = $('#txtName').val(); 
        request.UserId = userId;
        request.DataType = DataType;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/BasicData/BasicDataSearch", request,
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
    })

    $("#btnAdd").click(function () {
        var url = '/view/fas/basic/customerAdd.aspx?isAdd=1';
        if (DataType=='Vendor') {
            url = '/view/fas/basic/vendorAdd.aspx?isAdd=1';
        }
        parent.layer.open({
            type: 2,
            title: '新增',
   
            area: ['400px', '300px'],
            content: url,
            cancel: function (index, layer) {

                query(1);

            },
            end: function () {
                query(1);
            }
        });
    })
})