<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

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
                    <div id="txtPeriod"  class="layui-form  component " style="color:white;margin-left: 10px;padding-top: 5px;">

                    </div>

                <!-- 顶部右侧菜单 -->
                <ul class="layui-nav top_menu">
                   <li class="layui-nav-item  "   pc>
                        <a>为提高效率，请将浏览器设置为极速模式</a>
                    </li> 
                  <li class="layui-nav-item  "   pc>
                        <a href="http://saas.caishuizhijia.com/" target="_blank"> <cite>门户首页</cite></a>
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
                                <asp:LinkButton ID="logout" runat="server"  OnClick="logout_Click"><i class="iconfont icon-loginout"></i><cite>退出</cite></asp:LinkButton>
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
            <div class="navBar layui-side-scroll"></div>
        </div>
        <!-- 右侧内容 -->
        <div class="layui-body layui-form">
            <div class="layui-tab marg0" lay-filter="bodyTab">
                <ul class="layui-tab-title top_tab">
                    <li class="layui-this" lay-id=""><i class="iconfont icon-computer"></i> <cite>首页</cite></li>
                </ul>
                <div class="layui-tab-content clildFrame">
                    
                    <div class="layui-tab-item layui-show">
						<iframe src="view/main_html.aspx"></iframe>
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
    <script type="text/javascript" src="js/index.js?v=20181009"></script>


</body>
</html>
