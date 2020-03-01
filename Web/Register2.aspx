<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register2.aspx.cs" Inherits="Register2" %>

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

                var provinceVal = $('#province').find("option:selected")[0].value;
                var cityVal = $('#city').find("option:selected")[0].value;

                var townVal = $('#town').find("option:selected")[0].value;

                $('#txtProvince').val(provinceVal);
                $('#txtCity').val(cityVal);
                $('#txtTown').val(townVal);

                $('#btnOK').click();
                return false;

            });
            form.on('select(nodeType)', function (data) {
                if (data.value == 0) {
                    $('#d1,#d2').hide();
                } else {
                    $('#d1,#d2').show();
                }

            });

            var province = $("#province"), city = $("#city"), town = $("#town");
            for (var i = 0; i < provinceList.length; i++) {
                addEle(province, provinceList[i].name);
            }
            function addEle(ele, value) {
                var optionStr = "";
                optionStr = "<option value=" + value + ">" + value + "</option>";
                ele.append(optionStr);
            }
            function removeEle(ele) {
                ele.find("option").remove();
                var optionStar = "<option value=''>" + "请选择" + "</option>";
                ele.append(optionStar);
            }
            var provinceText, cityText, cityItem;
            province.on("change", function () {
                provinceText = $(this).val();
                $.each(provinceList, function (i, item) {
                    if (provinceText == item.name) {
                        cityItem = i;
                        return cityItem
                    }
                });
                removeEle(city);
                removeEle(town);
                $.each(provinceList[cityItem].cityList, function (i, item) {
                    addEle(city, item.name)
                })
            });
            city.on("change", function () {
                cityText = $(this).val();
                removeEle(town);
                $.each(provinceList, function (i, item) {
                    if (provinceText == item.name) {
                        cityItem = i;
                        return cityItem
                    }
                });
                $.each(provinceList[cityItem].cityList, function (i, item) {
                    if (cityText == item.name) {
                        for (var n = 0; n < item.areaList.length; n++) {
                            addEle(town, item.areaList[n])
                        }
                    }
                });
            });

            form.render();

        });

    </script>
    <style>
        .layui-nav .layui-nav-item a {
            color: #fff;
            font-size: 20px;
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
        <%--<div class="fly-header   " style="background: #0095df; border-bottom: 1px solid #7f95d2">
            <div class="layui-container">
                <a class="fly-logo" href="/" style="position: absolute; left: 35px; color: #fff">
                    <img src="images/logo.png" alt="layui">
                </a>
                <ul class="layui-nav fly-nav  ">
                    <li class="layui-nav-item"><a href="index.aspx">首页</a> </li>
                    <li class="layui-nav-item"><a href="/">小算课堂</a> </li>
                    <li class="layui-nav-item"><a href="/">咨询中心</a> </li>
                    <li class="layui-nav-item"><a href="/">关于我们</a> </li>
                    <span class="layui-nav-bar" style="width: 0px; left: 0px; opacity: 0;"></span>
                </ul>

            </div>
        </div>--%>
        <div class="layui-container fly-marginTop" style="margin-left: 50px; margin-right: 50px;">


            <div class="fly-panel fly-panel-user" pad20="">
                <img style="margin-left: auto; margin-right: auto; margin-bottom: 10px; display: block; margin-top: 10px" src="images/step2.jpg" />

                <fieldset class="layui-elem-field layui-field-title">
                    <legend>第二步</legend>

                </fieldset>
                <div class="layui-form layui-form-pane">
                    <div class="layui-form-item" style="display:none">
                        <label for="txtUserName" class="layui-form-label">注册类型</label>
                        <div class="layui-input-inline">
                            <asp:DropDownList runat="server" ID="selNodeType" lay-filter="nodeType">
                                <asp:ListItem Value="1">我是代帐会计</asp:ListItem>
                                <%--<asp:ListItem Value="0">我是企业主</asp:ListItem>--%>

                            </asp:DropDownList>
                        </div>
                        <div class="layui-form-mid layui-word-aux">企业主：代帐的需求方，代帐会计：需求的提供方，注册后无法更改</div>
                    </div>
                    <div class="layui-form-item">
                        <label for="txtNodeName" class="layui-form-label">企业名称</label>
                        <div class="layui-input-inline">
                            <input type="text" id="txtNodeName" runat="server" maxlength="20" placeholder="代帐会计非公司形式的不用填写" name="NodeName" class="layui-input" />
                        </div>
                        <div class="layui-form-mid layui-word-aux"></div>
                    </div>
                    <div class="layui-form-item">
                        <label for="txtUCode" class="layui-form-label">社会统一化代码</label>
                        <div class="layui-input-inline">
                            <input type="text" id="txtUCode" runat="server" maxlength="20" placeholder="代帐会计非公司形式的不用填写" name="UCode" class="layui-input" />
                        </div>
                        <div class="layui-form-mid layui-word-aux"></div>
                    </div>
                    <div id="d1" class="layui-form-item">
                        <label class="layui-form-label">提供的服务</label>
                        <div class="layui-input-inline">
                            <asp:CheckBox runat="server" Text="代帐" Checked="true" Enabled="false" />

                            <asp:CheckBox ID="chkZC" runat="server" Text="工商注册" Checked="false" />

                        </div>
                        <div class="layui-form-mid layui-word-aux">代帐会计提供的服务</div>
                    </div>

                    <div id="d2" class="layui-form-item">
                        <label class="layui-form-label">兼职选项</label>
                        <div class="layui-input-inline">
                            <asp:DropDownList runat="server" ID="selOUT">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="layui-form-mid layui-word-aux">代帐从业者是否愿意兼职，接受其他代帐会计的委托</div>
                    </div>

                    <div class="layui-form-item">
                        <label class="layui-form-label">所在区域</label>
                        <div class="layui-input-inline" style="width: 500px;">
                            <input type="text" id="txtProvince" runat="server" style="display: none" />
                            <input type="text" id="txtCity" runat="server" style="display: none" />
                            <input type="text" id="txtTown" runat="server" style="display: none" />
                            <select runat="server" class="area-select " name="province" id="province" lay-ignore>
                                <option value="">请选择</option>
                            </select>
                            <select runat="server" class="area-select " name="city" id="city" lay-ignore>
                                <option value="">请选择</option>
                            </select>
                            <select runat="server" class="area-select " name="town" id="town" lay-ignore>
                                <option value="">请选择</option>
                            </select>
                        </div>

                    </div>

                    <div class="layui-form-item">
                        <label for="txtUserName" class="layui-form-label">用户名</label>
                        <div class="layui-input-inline">
                            <input type="text" id="txtUserName" runat="server" maxlength="20" name="userName" lay-verify="required" class="layui-input" />
                        </div>
                        <div class="layui-form-mid layui-word-aux">将会成为您唯一的登入名</div>
                    </div>
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
                        <label class="layui-form-label">性别</label>
                        <div class="layui-input-inline">
                            <asp:DropDownList runat="server" ID="selSex">
                                <asp:ListItem Value="1">男</asp:ListItem>
                                <asp:ListItem Value="0">女</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>


                    <div class="layui-form-item">
                        <button class="layui-btn" lay-filter="" lay-submit="">立即注册</button>
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
