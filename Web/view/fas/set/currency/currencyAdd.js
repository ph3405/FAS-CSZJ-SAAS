
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id;
    var template = $.templates("#tpl-Edit");

    var dataHtml = template.render({});

    $('#editForm').html(dataHtml);


    form.render();

 
 

    form.on("submit(save)", function (data) {
        var url = '';
        if (id != '' && id != undefined) {
            url = "/fas/set/currencyUpdate";
        }
        else {
            url = "/fas/set/currencyAdd";
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
                id = res.Id;
                $.info(res.Message + ",点击确定,返回列表页", function () {
                    if (parent.query)
                        parent.query(1);
                    parent.layer.closeAll();
                })

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });




        return false;
    })
    $(document).on("input", ".rule-num-input", function () {

        if (!/^\d*(\.\d*)?$/.test(this.value))
            this.value = this.value.substr(0, this.value.length - 1)
    });

    $(document).on("propertychange", ".rule-num-input", function () {

        if (!/^\d*(\.\d*)?$/.test(this.value))
            this.value = this.value.substr(0, this.value.length - 1)
    });
})
