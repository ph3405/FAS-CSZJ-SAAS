layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $.views.converters("changeType", function (val) {
        if (val == 1) {
            return '新增';
        }
        else if (val == 2) {
            return '原值调整';
        }
        else if (val == 3) {
            return '累计折旧调整';
        }
        else if (val == 4) {
            return '使用年限调整';
        }
        else if (val == 5) {
            return '部门转移';
        }
        else if (val == 6) {
            return '折旧方法调整';
        }
        else if(val==7){
            return '状态修改';
        }
        else if (val == 8) {
            return "处置";
        }
    });


    var name = '';


    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Name = name;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/FixedAssets/FixedAssetsChangeListSearch", request,
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

    window.query = query;

    query(1);
    //查询
    $(".search_btn").click(function () {
        name = $('#txtName').val();
        query(1);
    })



    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //生成凭证
        var tplId = $(this).attr('data-tpl');
        var id = $(this).attr('data-id');
        var money = $(this).attr('data-money');
       
        if (tplId == '') {
            $.warning('未配置凭证模板');
            return;
        }

        parent.layer.open({
            type: 2,
            title: '凭证编辑',
            shade: 0.1,
            area: ['1200px', '600px'],
            content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=CHANGE&money=" + money+"&key="+id,
            cancel: function (index, layer) {

                query(1);

            }
        });

    })

    $("body").on("click", ".tks-rowDel", function () {  //取消凭证
        var id = $(this).attr('data-id');
        var DocId = $(this).attr('data-docid');
        var gId = $(this).attr('data-gid');
        $.confirm('确定取消此凭证？', function () {
      
            rowDel(DocId, id, gId);
            query(1);
        });

    })

    var rowDel = function (DocId, id, gId) {
        var index = $.loading('正在取消');
        var request = {};
        request.Data = { Id: DocId};//凭证ID
        request.Token = token;
        request.TKS_FAS_FixedAssetsChange_Id = id;
        request.TKS_FAS_FixedAssets_Id = gId;//固定资产ID

        $.Post("/fas/doc/docDel", request,
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

})