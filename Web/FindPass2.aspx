<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindPass2.aspx.cs" Inherits="FindPass2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="layui/css/layui.css" media="all" />
    <link href="css/global.css" rel="stylesheet" />
    <script src="js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="layui/layui.js"></script>
    <script src="js/area.js"></script>

    <title></title>
    <style>
        .layui-form-pane .layui-form-label {
            width: 130px;
        }

        .area-select {
            height: 38px;
            line-height: 38px;
            line-height: 36px;
            border: 1px solid #e6e6e6;
            background-color: #fff;
        }

        .layui-form-item .layui-input-inline {
            width: 220px;
        }
    </style>
    <script>


        layui.config({
            base: "js/"
        }).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
            var form = layui.form(),
                layer = layui.layer,
                laypage = layui.laypage;
            var $ = layui.jquery;

            form.on("submit()", function (data) {

 
                $('#btnOK').click();
                return false;

            });
        

         
            form.render();

        });

    </script>
    <style>
        .layui-nav .layui-nav-item a {
            color: #fff;
            font-size: 20px;
        }
    </style>
</head>
<body>
    <form runat="server" class="layui-form">
        <div class="fly-header   " style="background: #0095df; border-bottom: 1px solid #7f95d2">
            <div class="layui-container">
                <a class="fly-logo" href="/" style="position: absolute; left: 35px; color: #fff">
                    <img src="img/logo.png" alt="layui">
                </a>
                <ul class="layui-nav fly-nav  ">
                    <li class="layui-nav-item"><a href="index.aspx">首页</a> </li>
                    <li class="layui-nav-item"><a href="/">财税课堂</a> </li>
                    <li class="layui-nav-item"><a href="/">咨询中心</a> </li>
                    <li class="layui-nav-item"><a href="/">关于我们</a> </li>
                    <span class="layui-nav-bar" style="width: 0px; left: 0px; opacity: 0;"></span>
                </ul>

            </div>
        </div>
        <div class="layui-container fly-marginTop" style="margin-left: 50px; margin-right: 50px;">


            <div class="fly-panel fly-panel-user" pad20="">
                <img style="margin-left: auto; margin-right: auto; margin-bottom: 10px; display: block; margin-top: 10px" src="images/step2.jpg" />

                <fieldset class="layui-elem-field layui-field-title">
                    <legend>找回密码 第二步</legend>

                </fieldset>
                <div class="layui-form layui-form-pane">
                    
 
 
                    <div class="layui-form-item">
                        <label for="txtPassword1" class="layui-form-label">密码</label>
                        <div class="layui-input-inline">
                            <input type="password" id="txtPassword1" maxlength="16" runat="server" name="pass1" lay-verify="required" class="layui-input" />
                        </div>
                        <div class="layui-form-mid layui-word-aux">密码必须包含数字、小写或大写字母、6到16个字符</div>
                    </div>
                    <div class="layui-form-item">
                        <label for="txtPassword2" class="layui-form-label">确认密码</label>
                        <div class="layui-input-inline">
                            <input type="password" id="txtPassword2" maxlength="16" runat="server" name="pass2" lay-verify="required" class="layui-input" />
                        </div>
                        <div class="layui-form-mid layui-word-aux">请确认您的密码</div>
                    </div>
                 

                    <div class="layui-form-item">
                        <button class="layui-btn" lay-filter="" lay-submit="">提交</button>
                        <asp:Button runat="server" ID="btnOK" Style="display: none" OnClick="btnOK_Click" />
                    </div>
                    <div class="layui-form-item fly-form-app">
                        <label runat="server" id="lblError" style="color: red"></label>
                    </div>

                </div>



            </div>
        </div>
    </form>
</body>
</html>