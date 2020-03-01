layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage,
		$ = layui.jquery;



    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};
        request.Data = { QY_Name: $('#txtName').val() };

        request.Token = token;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/set/accountListSearch", request,
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

        $.Post("/fas/set/accountDel", request,
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
                    parent.opAccountListGet();
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
    var sendCode = function (phone, code) {
        //if (id == '' || id == undefined) {
        //    $.warning('请先保存');
        //    return false;
        //}
        var request = {};

        request.Token = token;
        request.MobilePhone = phone;
        request.Code = code;
        request.QYName = '';
        var index = $.loading('请稍后');


        $.ajax({
            type: "Post",
            url: "/fasservice.asmx/InvitationSend",
            data: JSON.stringify({ request: request }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var res = data.d;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    $.info("邀请成功", function () {

                        layer.closeAll();
                    })
                }
            },
            error: function (err) {
                layer.close(index);
                $.warning(err.Message);

            }
        });

    }

    //查询
    $(".search_btn").click(function () {
        query(1);
    })

    //添加
    $("#btnAdd").click(function () {
        $.open("添加新账套", "accountInfoAdd.aspx");
        
    })
    //添加关联账套
    $("#btnAddMyAccount").click(function () {
        $.open("关联账套", "/view/fas/myAccount/myAccountAdd.aspx");
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
                    parent.opAccountListGet();
                });


            }
        );

    })

    $("body").on("click", ".tks-rowAccept", function () {  //接受外包
        var id = $(this).attr('data-id');

        $.confirm('确定接受外包吗？', function () {


            acceptOut(id);

        });
    })
    var Code = '';
    $("body").on("click", ".tks-rowInvitation", function () {  //邀请
        var id = $(this).attr('data-id');
        var isMe = $(this).attr('data-me'); 
        var accountName = $(this).attr('data-name');
        var request = {};

        request.Token = token;
        request.Id = id;
        var index = $.loading('请稍后');

        $.Post('/fas/set/InvitationCodeGet', request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    var template = $.templates("#tpl-send");
                    
                    Code = res.InvitationCode;
                    var dataHtml = template.render({ AccountName: accountName, InvitationCode: Code });

                    layer.open({
                        type: 1,
                        title: '邀请企业主',
                        shade: 0.1,
                        area: ['500px', '350px'],
                        content: dataHtml
                    });
                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });


    })

    $("body").on("click", "#btnSend", function () {  //发送
        var phone = $('#txtQYMobile').val();
        if (phone == '') {
            $.warning('请填写手机号码');
            return;
        }

        sendCode(phone, Code);
    });



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
                    parent.opAccountListGet();
                    //parent.location.reload();
                });

              
            }
        );


    })
    $("body").on("click", ".tks-rowDelMyAccount", function () {  //取消关联企业
        var _this = $(this);
        $.confirm('确定取消吗？', function () {
            var index = $.loading('正在取消');
            var request = {};
            request.Data = { Id: _this.attr("data-id") };
            request.Token = token;

            $.Post("/fas/set/DelMyAccount", request,
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


        });
    })

    $("body").on("click", ".tks-rowActive", function () {  //激活账套
        var id = $(this).attr('data-id');
        rowActive(id);

    })

})