<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindPass1.aspx.cs" Inherits="FindPass1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="layui/css/layui.css" media="all" />
    <link href="css/global.css" rel="stylesheet" />
    <script src="js/easyui/jquery.min.js"></script>
    <script src="layui-v2.3.0/layui/layui.all.js"></script>
    <title></title>
    <script>
        $.Post = function (url, data, successCall, errorCall) {
            $.ajax({
                type: "Post",
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) { successCall(data); },
                error: function (err) { errorCall(err); }
            });
        }
        //验证码按钮重发时间
        var countdown = 60;
        //验证码按钮倒计时
        function settime(obj) {
            if (countdown == 0) {
                obj.removeAttribute("disabled");
                obj.value = "获取验证码";
                countdown = 60;
                return;
            } else {
                obj.setAttribute("disabled", true);
                obj.value = "重新发送(" + countdown + ")";
                countdown--;
            }
            setTimeout(function () {
                settime(obj)
            }
                , 1000)
        }
        //发送验证码程序调用
        function YZM(obj) {
            if (!CheckMobile()) {

                setError('请输入合理的手机号码');
                return;
            }
            var phoneNo = $("#txtMobile").val();
            settime(obj);
            var request = { MobilePhone: phoneNo };
            $.Post("/fasservice.asmx/FindPassMessage", { request: request }, function (data) {
                var response = data.d;
                if (response.IsSuccess) {
                  
                    setError('');
                }
                else {
                    
                    setError(response.Message);
                    if (response.Message=="手机号不存在") {
                        layui.layer.alert("该手机号未注册，是否注册", { icon: 2 },
                            function () {
                                top.location.href = 'register1.aspx';
                                layer.close(layer.index);
                            }
                        );
                    }
 
                }
            }, function (err) { });
        }

        function CheckMobile() {
            var re = /^(((13[0-9]{1})|(15[0-9]{1})|(14[0-9]{1})|(16[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;    //  /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;邮箱校验

            var email = $("#txtMobile").val();
            if (!re.test(email))  //test字符串一部分符合要求，true
            {

                return false;
            }

            return true;
        }
        var setError = function (error) {
            $("#lblError").text(error);
        };

    
        window.YZM = YZM;
    </script>
    <style>
        .layui-nav .layui-nav-item a{
            color:#fff;
             font-size:20px;
        }

       
    </style>
</head>
<body>
    <form runat="server" class="layui-form">
        <div class="fly-header   " style="background:#0095df;border-bottom:1px solid #7f95d2">
            <div class="layui-container">
                <a class="fly-logo" href="/" style="position: absolute; left: 35px;  color:#fff">
                    <img src="img/logo.png" alt="layui">
                   
                </a>
                <ul class="layui-nav fly-nav  ">
                    <li class="layui-nav-item"><a href="index.aspx">  首页</a> </li>
                      <li class="layui-nav-item"><a href="/">  财税讲堂</a> </li>
                      <li class="layui-nav-item"><a href="/">  咨询中心</a> </li>
                      <li class="layui-nav-item"><a href="/">  关于我们</a> </li>
                    <span class="layui-nav-bar" style="width: 0px; left: 0px; opacity: 0;"></span>
                </ul>

            </div>
        </div>
        <div class="layui-container fly-marginTop" style="margin-left: 50px; margin-right: 50px;">


            <div class="fly-panel fly-panel-user" pad20="">
                <img style="margin-left:auto;margin-right:auto;margin-bottom:10px;display:block;margin-top:10px;" src="images/step1.jpg" />

                <fieldset class="layui-elem-field layui-field-title">
                    <legend>找回密码 第一步</legend>
                </fieldset>
                <div class="layui-form layui-form-pane">

                    <div class="layui-form-item">
                        <label for="L_email" class="layui-form-label">手机</label>
                        <div class="layui-input-inline">
                            <input type="text" id="txtMobile" name="mobile"  runat="server" placeholder="请输入手机号"   class="layui-input"/>
                        </div>
                        <div class="layui-form-mid layui-word-aux"></div>
                    </div>
                   
                    <div class="layui-form-item">
                        <label for="L_vercode" class="layui-form-label">验证</label>
                        <div class="layui-input-inline">
                            <input type="text" id="txtVerCode" name="vercode" runat="server"   placeholder="请输入验证码"  class="layui-input" />
                        </div>
                        <div class="layui-input-inline">

                             <input type="button" class="layui-btn layui-btn-primary" onclick="YZM(this)" value="发送验证码"/>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <asp:Button runat="server" ID="btnNext" CssClass="layui-btn" OnClick="btnNext_Click" Text="下一步" />
                       
                    </div>
                    <div class="layui-form-item fly-form-app"><label runat="server" id="lblError" style="color:red"></label> </div>

                </div>



            </div>
        </div>
    </form>
</body>
</html>
