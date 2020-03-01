layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;

    $("#StartDate").click(function () {
        laydate({
            elem: '#StartDate', format: 'YYYY-MM-DD'
        });
    });
    $("#EndDate").click(function () {
        laydate({
            elem: '#EndDate', format: 'YYYY-MM-DD'
        });
    });

    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.StartDate = $('#StartDate').val();
        request.EndDate = $('#EndDate').val();
        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/set/BackUpSearch", request,
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

        $.Post("/fas/set/BackUpDelete", request,
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
    var rowHuiFu = function (id) {

        var index = $.loading('正在恢复');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/BackUpHuiFu", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.topTip(res.Message);

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    var rowActive = function (id) {
        var index = $.loading('切换中');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/accountActive", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    parent.layer.alert(res.Message + "，将重新加载页面", {

                        cancel: function (index, layer) {
                            parent.opAccountListGet();
                            return false;
                        }
                    }, function () {
                        parent.opAccountListGet();
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

    var unOutSource = function (id) {
        var request = {};
        

        request.Data = {Id:id}
        request.Token = token;
       

        $.Post("/fas/set/AccountUnOut", request,
        function (data) {
            var res = data;
          
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message, function () {
                    query(1);
                });


            }


        }, function (err) {

          
            $.warning(err.Message);
        });

    }

    var acceptOut = function (id) {
        var request = {};


        request.Data = { Id: id }
        request.Token = token;


        $.Post("/fas/set/AccountAcceptOut", request,
        function (data) {
            var res = data;

            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                $.info(res.Message, function () {
                    query(1);
                });


            }


        }, function (err) {


            $.warning(err.Message);
        });
    }

    var invitationCode = function (id) {
        var request = {};
        request.Data = { Id: id }
        request.Token = token;
        $.Post("/fas/set/AccountAcceptOut", request,
        function (data) {
            var res = data;

            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
               
            }
        }, function (err) {
            $.warning(err.Message);
        });
    }

    var checkPass = function (pass,func) {
        var index = $.loading('处理中');
        var request = {};
        request.UserName = userName;
        request.Password = pass;
        request.Token = token;

        $.Post("/sso/userCheck", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    func();

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

    //备份
    $("#StartBackup").click(function () {
        var index = $.loading('备份中');
        var request = {};
        request.Token = token;
        $.Post("/fas/set/BackUp", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.topTip(res.Message);
                    query(1);
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
        
    })

    //上传
    $("#Import").click(function () {
     
        $.dialog('上传', 'attachment.aspx', undefined, function () {
            query(1);
        });

    })

    //操作
    $("body").on("click", ".tks-rowEdit", function () {  //编辑
        var id = $(this).attr('data-id');
        var isMe = $(this).attr('data-me');
        $.open('账套编辑', "accountInfoEditor.aspx?id=" + id+"&me="+isMe);

    })
    $("body").on("click", ".tks-rowOut", function () {  //外包
        var id = $(this).attr('data-id');
        $.open('账套外包', "accountOutSource.aspx?id=" + id);

    })


    $("body").on("click", ".tks-rowUnOut", function () {  //撤销外包
        var id = $(this).attr('data-id');

        layer.prompt({
            title: '请输入密码后撤销',
            formType: 1

        },
            function (val, index) {


                checkPass(val, function () {
                    unOutSource(id);
                    layer.close(index);
                });


            }
        );

    })

    $("body").on("click", ".tks-rowHuiFu", function () {  
        var _this = $(this);
        layer.prompt({
            title: '请输入密码后恢复',
            formType: 1

        },
            function (val, index) {


                checkPass(val, function () {

                    //var id = $(this).attr('data-id');
                    rowHuiFu(_this.attr("data-id"));
                    layer.close(index);
                    //var index = $.loading('正在恢复');
                    //var request = {};
                    //request.Data = { Id: id };
                    //request.Token = token;

                    //$.Post("/fas/set/BackUpHuiFu", request,
                    //    function (data) {
                    //        var res = data;
                    //        if (res.IsSuccess) {
                    //            $.topTip(res.Message);

                    //        } else {
                    //            $.warning(res.Message);
                    //        }
                    //        layer.close(index);
                    //    }, function (err) {
                    //        $.warning(err.Message);
                    //        layer.close(index);
                    //    });
                });


            }
        );

    })


   



    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        layer.prompt({
            title: '请输入密码后删除',
            formType: 1
           
            },
            function(val, index) {
                
              
                checkPass(val, function () {
               
                    rowDel(_this.attr("data-id"));
                    layer.close(index);
                    //parent.opAccountListGet();
                  
                });

              
            }
        );


    })


    $("body").on("click", ".tks-rowDownload", function () {  //下载
        var id = $(this).attr('data-id');
        var index = $.loading('正在下载');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/set/BackUpDownload", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    window.open('TPLDownload.aspx?id=' + id);

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
        


    })

})