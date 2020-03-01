layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage,
        $ = layui.jquery;
    var category = 1;
    var currencyCode = 'RMB';
    var pageIndex = 1;
    var num = '';
    $.views.converters("noZero", function (val) {

        if (val == 0) {
            return "";
        }
        else {
            return val;
        }
    });

    var init = function () {

        var index = $.loading('初始化中');
        var request = {};
        request.Token = token;
        request.PageIndex = 1;
        request.PageSize = 999;
        $.Post("/fas/set/currencyListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-currency");

                    var dataHtml = template.render(res.Data);

                    $('#selCurrency').html(dataHtml);

                    form.render();
                    query();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };

    init();

    form.on('select(category)', function (data) {
        category = data.value;
        query();

    });

    form.on('select(currency)', function (data) {
        currencyCode = data.value;
        query();

    });

    form.on('checkbox()', function (data) {
        num = data.elem.checked;
        query();
    });

    function setMemo(res) {
        var data = {};
        if (category == 1) {
            data.CreditDebit = '借';
            data.startBWB = res.startBWB1;
            data.deljBWB = res.deLJBWB1;
            data.crljBWB = res.crLJBWB1;
        }
        else if (category == 2) {
            data.CreditDebit = '贷';
            data.startBWB = res.startBWB2;
            data.deljBWB = res.deLJBWB2;
            data.crljBWB = res.crLJBWB2;
        }
        else if (category == 3) {
            data.CreditDebit = '贷';
            data.startBWB = res.startBWB3;
            data.deljBWB = res.deLJBWB3;
            data.crljBWB = res.crLJBWB3;
        }
        else if (category == 4) {
            data.CreditDebit = '借';
            data.startBWB = res.startBWB4;
            data.deljBWB = res.deLJBWB4;
            data.crljBWB = res.crLJBWB4;
        }
        else if (category == 5) {
            data.CreditDebit = '借';
            data.startBWB = res.startBWB5;
            data.deljBWB = res.deLJBWB5;
            data.crljBWB = res.crLJBWB5;
        }
        var template = $.templates('#tpl-memo');

        var dataHtml = template.render(data);

        $('#memo').html(dataHtml);
    }
    var query = function () {

        // var index = $.loading('查询中');
        var request = {};
        request.Category = category;
        request.CurrencyCode = currencyCode;
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 9999;
        $.Post("/fas/set/balListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var tpl = "#tpl-list";
                    var type = 1;
                    setMemo(res);
                    if (res.IsFirstPeriodPaid) {

                        //根据类型选择模板
                        if (currencyCode == "RMB" && num == true) {
                            type = 4;
                            tpl = "#tpl-readonlylist4";
                        }
                        else if (currencyCode == "RMB" && num != true) {
                            type = 3;
                            tpl = "#tpl-readonlylist3";
                        }
                        else if (currencyCode != 'RMB' && num == true) {
                            type = 1;
                            tpl = "#tpl-readonlylist";
                        }
                        else if (currencyCode != 'RMB' && num != true) {
                            type = 2;
                            tpl = '#tpl-readonlylist2';
                        }
                    }
                    else {


                        //根据类型选择模板
                        if (currencyCode == "RMB" && num == true) {
                            type = 4;
                            tpl = "#tpl-list4";
                        }
                        else if (currencyCode == "RMB" && num != true) {
                            type = 3;
                            tpl = "#tpl-list3";
                        }
                        else if (currencyCode != 'RMB' && num == true) {
                            type = 1;
                            tpl = "#tpl-list";
                        }
                        else if (currencyCode != 'RMB' && num != true) {
                            type = 2;
                            tpl = '#tpl-list2';
                        }


                    }
                    var headTpl = $.templates("#tpl-head");
                    var headHtml = headTpl.render({ type: type });

                    $('#tbHead').html(headHtml);

                    var template = $.templates(tpl);

                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);



                    //laypage({
                    //    curr: pageIndex,
                    //    cont: "page",
                    //    pages: Math.ceil(res.Total / 9999),
                    //    jump: function (obj, first) {
                    //        if (!first) {
                    //            pageIndex = obj.curr;
                    //            query();
                    //        }
                    //    }
                    //});

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

    var rowDel = function (id, Code, Name) {
        var index = $.loading('正在删除');
        var request = {};
        request.Id = id;
        request.Token = token;
        request.SubjectCode = Code;
        request.SubjectName = Name;
        $.Post("/fas/set/balDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        layer.close(index);
                        query();
                        layer.closeAll();
                    });

                } else {
                    $.warning(res.Message);
                }

            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };


    $('#btnCal').click(function () {

        var request = {};

        request.Token = token;

        var index = $.loading('试算中');
        $.Post("/fas/set/balcal", request,
            function (data) {
                var res = data;
                setMemo(res);
                if (res.IsSuccess) {



                    layer.alert(res.Message);
                }
                else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    });

    $('#btnImport').click(function () {
        $.dialog('导入', 'attachment.aspx', undefined, function () {
            query();
        });

    });

    $("body").on("click", ".row-add", function () {  //新增
        var id = $(this).attr('data-subjectcode');
        var balId = $(this).attr('data-id');

        layer.open({
            type: 2,
            title: '新增辅助核算',
            shade: 0.1,
            area: ['500px', '400px'],
            content: "balAss.aspx?id=" + id + "&balId=" + balId,
            cancel: function (index, layero) {

            }
        });
    })

    //操作
    $("body").on("blur", ".row-input", function () {  //焦点失去的时候计算当前行
        var id = $(this).attr('data-id');
        var inputType = $(this).attr('data-type');//类型
        var tableType = $(this).parent().parent().attr('data-table');
        var SubjectCode = $(this).attr('data-subjectcode');
        var SubjectName = $(this).attr('data-subjectname');
        var request = {};
        request.Id = id;
        request.Token = token;
        request.Type = tableType;
        request.Money = $(this).val();
        request.InputType = inputType;
        request.SubjectCode = SubjectCode;
        request.SubjectName = SubjectName;
        //if (request.Money == '0' || request.Money == '')
        //    return;
        // var index = $.loading('提交中');
        $.Post("/fas/set/balupdate", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {

                    query();

                }
                else {
                    $.warning(res.Message);
                }
                //layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                //layer.close(index);
            });

    })

    //获取焦点，判断是否可编辑
    //$("body").on("focus", ".row-input", function () {
    //    debugger;
    //    var SubjectCode = $(this).attr('data-Subject');
    //    var check=""
    //    var request = {};

    //    request.Data = { Category: category, Code: SubjectCode};
    //    request.Token = token;
    //    $.PostTonBu("/fas/set/CheckSubjectIsReadonly", request,
    //        function (data) {
    //            var res = data;
    //            if (res.IsSuccess) {
    //                check = res.CheckReadonly;

    //            } else {
    //                $.warning(res.Message);
    //            }
    //        }, function (err) {
    //            $.warning(err.Message);
    //        });
    //    if (check == "readonly") {
    //        $(this).attr({ readonly: 'true' });  
    //    }
    //    else {
    //        $(this).removeAttr("readonly");

    //    }

    //})

    $("body").on("click", ".row-del", function () {  //删除
        var _this = $(this);
        var SubjectCode = $(this).attr('data-subjectcode');
        var SubjectName = $(this).attr('data-subjectname');
        $.confirm('确定删除此科目？', function () {


            rowDel(_this.attr("data-id"), SubjectCode, SubjectName);

        });
    })


})