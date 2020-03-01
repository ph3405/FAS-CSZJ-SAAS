<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register3.aspx.cs" Inherits="Register3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="layui/css/layui.css" media="all" />
    <link href="css/global.css" rel="stylesheet" />
    <script src="js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="layui/layui.js"></script>
    <title></title>
    <script>


        layui.config({
            base: "js/"
        }).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
            var form = layui.form(),
                layer = layui.layer,
                laypage = layui.laypage;
            var $ = layui.jquery;
            form.render();
          
           
        });
        $(function () {



        })
    </script>
    <style>
        .layui-nav .layui-nav-item a {
            color: #fff;
                font-size:20px;
        }
               .top-bar {
            background: #00a1e9;
            height: 60px;
        }

        .zxs-logo {
            height: 50px;
            position: absolute;
            left: 30px;
            top: 10px;
            z-index: 22;
        }

            .zxs-logo img {
                height: 100%;
                display: block;
            }

        .nav-menu {
            display: flex;
            justify-content: center;
            align-items: center;
            position: absolute;
            right: 30px;
            top: 15px;
            z-index: 22;
        }

        .nav-item {
            display: block;
            color: #fff;
            font-size: 14px;
            margin-left: 30px;
        }

            .nav-item.active {
                padding: 6px 16px;
                background: #26afec;
                border-radius: 50px;
            }
    </style>
</head>
<body>
    <form runat="server" class="layui-form">
        <div class="fly-header   " style="background: #0095df; border-bottom: 1px solid #7f95d2">
            <div class="top-bar">
                <div class="zxs-logo">
                    <img src="img/Logo.png" />
                </div>

                <div class="nav-menu">
                    <a class="nav-item" href="#">首页</a>
                    <a class="nav-item " href="#">手册</a>
                    <a class="nav-item" href="#">我们</a>
                    <a class="nav-item" href="login.aspx">登录</a>
                    <a class="nav-item active" href="#">注册</a>
                </div>
            </div>
        </div>
<%--        <div class="fly-header   " style="background: #0095df; border-bottom: 1px solid #7f95d2">
            <div class="layui-container">
                <a class="fly-logo" href="/" style="position: absolute; left: 35px;  color: #fff">
                    <img src="images/logo.png" alt="layui">
                 
                </a>
                <ul class="layui-nav fly-nav  ">
                    <li class="layui-nav-item"><a href="index.aspx">首页</a> </li>
                    <li class="layui-nav-item"><a href="/">小算课堂</a> </li>
                    <li class="layui-nav-item"><a href="/">咨询中心</a> </li>
                    <li class="layui-nav-item"><a target="_blank" href="site/us.html">关于我们</a> </li>
                    <span class="layui-nav-bar" style="width: 0px; left: 0px; opacity: 0;"></span>
                </ul>

            </div>
        </div>--%>
        <div class="layui-container fly-marginTop" style="margin-left: 50px; margin-right: 50px;">


            <div class="fly-panel fly-panel-user" pad20="" style="height:500px">
                 <img style="margin-left:auto;margin-right:auto;margin-bottom:10px;margin-top:10px;display:block" src="images/step3.jpg" />

                <fieldset class="layui-elem-field layui-field-title">
                    <legend>完成</legend>

                </fieldset>
                <div class="layui-form layui-form-pane">
                 恭喜你，注册成功！<a href="Login.aspx" style="color:red">登录</a>后，使用章小算吧！
                </div>


                
            </div>
        </div>
    </form>
</body>
</html>
