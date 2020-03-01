<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainBusiness.aspx.cs" Inherits="MainBusiness" %>

<!DOCTYPE html>
 

<html>
<head>
    <meta charset="utf-8">
    <title>财税之家-AI智能财税系统</title>
   
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
     <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />
    <meta http-equiv="Access-Control-Allow-Origin" content="*">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="format-detection" content="telephone=no">
    <link rel="icon" href="favicon.ico">

    <link rel="stylesheet" href="layui/css/layui.css" media="all" />

    <link rel="stylesheet" href="css/main.css" media="all" />
</head>
<body class="main_body">
    <div class="layui-layout layui-layout-admin">
        <!-- 顶部 -->
        <div class="layui-header header ">
            <div class="layui-main  ">
                <a href="#" class="logo">财税之家</a>
                <!-- 搜索 -->
                <div class="layui-form  component">

                    <select  id="selAccount"  lay-filter="currentAccount"  lay-search="">
                         <option value="">请选择</option>
                        
                    </select>
                    
                
                </div>
                    <div id="txtPeriod"  class="layui-form  component " style="margin-left: 10px;padding-top: 5px;width:30px">
                        <i class="layui-icon" id="btnAddAccount" style="font-size:36px; color: white;cursor:pointer">&#xe61f;</i> 
                    </div>

                <!-- 顶部右侧菜单 -->
                <ul class="layui-nav top_menu">
                    <li class="layui-nav-item  " pc>
                        <a>为提高效率，请将浏览器设置为极速模式</a>
                    </li>
                    <li class="layui-nav-item  " pc>
                        <a href="http://saas.caishuizhijia.com/" target="_blank"><cite>门户首页</cite></a>
                    </li>

                    <li style="position: relative; display: inline-block; vertical-align: middle; line-height: 60px;">
                        <%=NodeName %>
                    </li>
                    <li class="layui-nav-item" pc>
                        <a href="javascript:;">
                            <img src="<%=PhotoPath%>" class="layui-circle" width="35" height="35">
                            <cite><%=TrueName%></cite>
                        </a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:;" data-url="view/personal/userInfo.aspx"><i class="iconfont icon-zhanghu" data-icon="icon-zhanghu"></i><cite>个人资料</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/personal/modifyPassword.aspx"><i class="iconfont icon-zhanghu" data-icon="icon-zhanghu"></i><cite>修改密码</cite></a></dd>

                            <dd>
                                <form runat="server">
                                    <asp:LinkButton ID="logout" runat="server" OnClick="logout_Click"><i class="iconfont icon-loginout"></i><cite>退出</cite></asp:LinkButton>
                                </form>
                            </dd>
                        </dl>
                    </li>
                </ul>
            </div>
        </div>
        <!-- 左侧导航 -->
        <div class="layui-side layui-bg-black">
            <div class="user-photo">
                <a class="img" title="我的头像">
                    <img src="<%=PhotoPath%>"></a>
                <p>你好！<span class="userName"><%=TrueName%></span>, 欢迎登录</p>
                
            </div>
            <div class="navBar layui-side-scroll">
                <ul class="layui-nav layui-nav-tree">
                    <li class="layui-nav-item">
                        <a href="javascript:;">
                            <cite>发票盒子</cite>
                            <span class="layui-nav-more"></span></a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:;" data-url="view/fas/fp/fpupList.aspx"><i class="layui-icon" data-icon=""></i><cite>发票上传</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/fas/Report/sfReport.aspx"><i class="layui-icon" data-icon=""></i><cite>应收应付管理</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/fas/Report/sfWarnReport.aspx"><i class="layui-icon" data-icon=""></i><cite>应收应付提醒</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/fas/Report/sfNoTimeReport.aspx"><i class="layui-icon" data-icon=""></i><cite>实时未收付</cite></a></dd>
                        </dl>
                    </li>
                    <li class="layui-nav-item">
                        <a href="javascript:;">
                            <cite>报表</cite>
                            <span class="layui-nav-more"></span></a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:;" data-url="view/fas/report/jyReport.aspx"><i class="layui-icon" data-icon=""></i><cite>经营报表</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/fas/report/sjReport.aspx"><i class="layui-icon" data-icon=""></i><cite>税金报表</cite></a></dd>
                        </dl>
                    </li>
                    <li class="layui-nav-item">
                        <a href="javascript:;">
                            <cite>基础数据维护</cite>
                            <span class="layui-nav-more"></span></a>
                        <dl class="layui-nav-child">
                            <dd><a href="javascript:;" data-url="view/fas/basic/customerList.aspx"><i class="layui-icon" data-icon=""></i><cite>客户维护</cite></a></dd>
                            <dd><a href="javascript:;" data-url="view/fas/basic/vendorList.aspx"><i class="layui-icon" data-icon=""></i><cite>供应商维护</cite></a></dd>
                        </dl>
                    </li>
                    <%--<span class="layui-nav-bar" style="top: 247.5px; height: 0px; opacity: 0;"></span>--%>
                </ul>
            </div>
        </div>
        <!-- 右侧内容 -->
        <div class="layui-body layui-form">
            <div class="layui-tab marg0" lay-filter="bodyTab">
                <ul class="layui-tab-title top_tab">
                    <li class="layui-this" lay-id=""><i class="iconfont icon-computer"></i> <cite>首页</cite></li>
                </ul>
                <div class="layui-tab-content clildFrame">
                    
                    <div class="layui-tab-item layui-show">
						<%--<iframe src="view/main_html.aspx"></iframe>--%>
					</div>
                </div>
            </div>
        </div>
        <!-- 底部 -->
     <%--   <div class="layui-footer footer">
            <p>copyright @2017 TKS　</p>
        </div>--%>
    </div>

    <script id="tpl-select" type="text/x-jsrender" >
        <option value="{{:Id}}"   {{if Active==1}}selected{{/if}}>{{:QY_Name}}</option>
    </script>

    <script>
        var token = '<%=Token%>';
        var funcId = '<%=FuncId%>';
    </script>
    <script type="text/javascript" src="layui/layui.js"></script>
    <script type="text/javascript" src="js/leftNav.js"></script>
    <script type="text/javascript" >
        var $, tab, form, layer, element;
        layui.config({
            base: "/layui/lay/modules/"
        }).use(['bodyTab', 'form', 'element', 'layer', 'jquery', 'jqExt'], function () {
            form = layui.form(),
                layer = layui.layer,
                element = layui.element();
            $ = layui.jquery;

            tab = layui.bodyTab();

            var init = function () {
                // 添加新窗口
                $(".layui-nav .layui-nav-item a").on("click", function () {
                    addTab($(this));
                    $(this).parent("li").siblings().removeClass("layui-nav-itemed");
                });
               
                opAccountListGet();
            }
            init();

            function opAccountListGet() {
                var request = {};
                request.Token = token;
                var index = $.loading('账套获取中');
                $.Post('/sso/WX_GetAccountList', request, function (data) {
                    var res = data;
                    layer.close(index);
                    if (!res.IsSuccess) {
                        $.warning(res.Message);
                    }
                    else {
                        var template = $.templates("#tpl-select");

                        var dataHtml = template.render(res.Data);
                        var SelectAccount = res.SelectAccount;
                        $('#selAccount').html(dataHtml);
                        if (res.IsSelected) {
                            //$('#txtPeriod').html(res.Year + '年第' + res.Month + "期");
                        }
                        else {
                            if (res.Data.length > 0 && res.Data[0].Id != undefined) {
                                //add by Hero.Zhang 每次重新获取账套都重新激活
                                Active(res.Data[0].Id);

                            }
                        }

                        

   

                        form.render();
                    }


                }, function (err) {
                    $.warning(err.Message);
                    layer.close(index);

                });
            }
            
            var accountActive = function (id) {

                var index = $.loading('切换中');
                var request = {};
                request.Data = { Id: id };
                request.Token = token;

                $.Post("/fas/set/accountActive", request,
                    function (data) {
                        var res = data;
                        if (res.IsSuccess) {
                            parent.location.reload();

                        } else {
                            $.warning(res.Message);
                        }
                        layer.close(index);
                    }, function (err) {
                        $.warning(err.Message);
                        layer.close(index);
                    });
            };
            form.on('select(currentAccount)', function (data) {
                accountActive(data.value);
            });
            $('#btnAddAccount').click(function () {
                layer.open({
                    title: '邀请码',
                    content: '<input type="text" class="layui-input "  placeholder="请输入邀请码"/>',
                    btn: ['确认', '取消'],
                    yes: function (index, layero) {
                        var YQCode=layero.find('input').val();
                        if (YQCode != "") {
                            var request = {};
                            request.InvitationCode = YQCode;
                            request.Token = token;
                            var index1 = $.loading('验证中');
                            $.Post("/sso/WX_CheckInvitation", request,
                                function (data) {
                                    var res = data;
                                    if (res.IsSuccess) {
                                        parent.location.reload();

                                    } else {
                                        $.warning(res.Message);
                                    }
                                    layer.close(index1);
                                }, function (err) {
                                    $.warning(err.Message);
                                    layer.close(index1);
                                });
                        }
                    },
                    cancel: function (index, layer) {

                        //query(1);

                    },
                    end: function () {

                    }
                });
  
            })
            //add by Hero.Zhang 关闭网页时，提示
            window.onbeforeunload = function (event) {
                var event = event || window.event;
                // 兼容IE8和Firefox 4之前的版本
                if (event) {
                    event.returnValue = "确定要关闭窗口吗？";
                }
                // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
                return '确定要关闭窗口吗>现代浏览器？';
            }

        });
        //打开新窗口
        function addTab(_this) {
            tab.tabAdd(_this);
        }
        function Active(id) {

            var index = $.loading('切换中');
            var request = {};
            request.Data = { Id: id };
            request.Token = token;

            $.Post("/fas/set/accountActive", request,
                function (data) {
                    var res = data;
                    if (res.IsSuccess) {

                    } else {
                        $.warning(res.Message);
                    }
                    layer.close(index);
                }, function (err) {
                    $.warning(err.Message);
                    layer.close(index);
                });
        };
    </script>


</body>
</html>
