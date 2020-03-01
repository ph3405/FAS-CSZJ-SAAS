<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyPassword.aspx.cs" Inherits="view_personal_ModifyPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
  
    <script src="../../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script src="../../js/area.js"></script>

    <title></title>
    <style>
       

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
    <form runat="server" class="layui-form" style="padding:20px 0 0 50px">

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
    </form>
</body>
</html>
