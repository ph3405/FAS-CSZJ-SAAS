<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
     <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />
    <link href="css/login.css" rel="stylesheet" />
    <title></title>
     <script language="JavaScript">
            
        function loadTopWindow() {
            if (window.top != null && window.top.document.URL != document.URL) {
                window.top.location = document.URL;
            }
        }
    </script>
</head>
<body onload="loadTopWindow()">
    <form id="form1" runat="server">
        <div class="main">
            <div class="login">
                <div class="login_1">智能财务登陆平台</div>
                <div class="login_2">
                    <input class="input1" type="text" value="" placeholder="用户名" id="txtUserName" runat="server" />

                </div>
                <div class="login_2">
                    <input class="input1" type="password" runat="server" id="txtPass" placeholder="密码" value="" />

                </div>
                <div class="login_2s">
                    <div class="login_2a">
                     <%--   <input class="input2" name="" type="checkbox" value="">
                        记住密码--%>
                    </div>
                    <div class="login_2b"><a href="register1.aspx" class="sig">注册</a>
                        <a href="findPass1.aspx" class="fpw">忘记密码</a></div>
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
</body>
</html>
