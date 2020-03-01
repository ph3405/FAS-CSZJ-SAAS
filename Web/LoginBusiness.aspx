<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginBusiness.aspx.cs" Inherits="LoginBusiness" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
     <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />
    <link href="css/login.css" rel="stylesheet" />
    <link href="layui/css/layui.css" rel="stylesheet" media="all"/>
    <title></title>
     <script language="JavaScript">
            
        function loadTopWindow() {
            if (window.top != null && window.top.document.URL != document.URL) {``
                window.top.location = document.URL;
            }
        }
    </script>
</head>
<body onload="loadTopWindow()">
    <form id="form1" runat="server">
        <div class="main">
            <div class="login">
                <div class="login_1">章小算登陆平台</div>
                <div class="login_2">
                    <input class="input1" type="text" value="" placeholder="手机号" id="txtMobile" runat="server" />

                </div>
                <div class="login_2">
                    <div style="float:left;width:120px;">
                         <input class="input1" style="width:120px;" type="text" runat="server" id="txtVerCode" placeholder="验证码" value="" />
                    </div>
                   <div style="float:right;height:45px;line-height:45px;">
                    <button class="" id="btnVerCode">获取验证码</button>
                       </div>
                </div>

                <div class="login_2s">

                    <asp:Button runat="server" ID="btnLogin" class="login_btn" Text="登陆" OnClick="btnLogin_Click"></asp:Button>

                </div>
                <div class="login_2s">
                     <asp:Label runat="server" ForeColor="Red" ID="lbError"></asp:Label>
                </div>
            </div>
        </div>
    </form>
    <script src="layui/layui.js"></script>
    <script type="text/javascript">
        layui.config({
            base: "js/"
        }).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
            var form = layui.form(),
                layer = layui.layer,
                laypage = layui.laypage;
            var $ = layui.jquery;
            var wait = 60;
            $('#btnVerCode').click(function () {
                var phone = $('#txtMobile').val();
                if (phone=="") {
                     $.warning('请填写手机号');
                    return false;
                }
                if (!(/^1[34578]\d{9}$/.test(phone))) {
                    $.warning('手机号格式有误');
                    return false;
                }
                time(this);
                var request = {};
                request.MobilePhone = phone;

                $.Post("/sso/SendMessage", request,
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
            });
            function time(o) {
                if (wait == 0) {
                    o.removeAttribute("disabled");
                    o.innerHTML = "免费获取验证码";
                    wait = 60;
                } else {
                    o.setAttribute("disabled", true);
                    o.innerHTML = wait + "秒后可以重新发送";
                    wait--;
                    setTimeout(function () {
                        time(o)
                    }, 1000)
                }
        }

        });
         

        
    </script>
</body>
</html>
