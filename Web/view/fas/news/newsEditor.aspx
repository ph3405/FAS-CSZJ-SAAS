<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newsEditor.aspx.cs" Inherits="view_fas_newsEditor" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>资讯编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <script type="text/javascript" charset="utf-8" src="../../../ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="../../../ueditor/lang/zh-cn/zh-cn.js"></script>


    <style type="text/css">
        .status-select {
            height: 38px;
            line-height: 38px;
            line-height: 36px;
            border: 1px solid #e6e6e6;
            background-color: #fff;
        }
    </style>
</head>
<body class="childrenBody">
    <form runat="server" class="layui-form" style="width: 100%;">
        <asp:HiddenField  runat="server" ID="hidId"/>
        <div class="layui-form-item">
            <label class="layui-form-label">标题</label>
            <div class="layui-input-block">
                <input id="txtTitle" runat="server" type="text" lay-verify="required" placeholder="请输入标题" class="layui-input">
            </div>
        </div>
        <div class="layui-form-item">

            <label class="layui-form-label">状态</label>
            <div class="layui-input-inline">
                <select runat="server" id="selStatus" cssclass="status-select">
                    <option value="0">草稿</option>
                    <option value="1">发布</option>
                </select>
            </div>

        </div>
        <div class="layui-form-item layui-form-text">
            <label class="layui-form-label">概述</label>
            <div class="layui-input-block">
                <textarea runat="server" id="txtSummary" placeholder="请输入内容" class="layui-textarea"></textarea>
            </div>
        </div>
        <div class="layui-form-item layui-form-text">
            <label class="layui-form-label">内容</label>
            <div class="layui-input-block">
                <textarea id="txtBody" name="txtBody" runat="server" style="height: 300px">
             
                          </textarea>

                <script type="text/javascript" charset="utf-8">
                  
                </script>
            </div>
        </div>

        <div class="layui-form-item">
            <label class="layui-form-label"></label>
            <div class="layui-input-block">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>

        <div class="layui-form-item">
            <div class="layui-input-block">
                <asp:Button runat="server" ID="btnNext" CssClass="layui-btn" OnClick="btnNext_Click" Text="保存" />
            </div>
        </div>

    </form>



    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script src="../../../../js/easyui/jquery.min.js"></script>

    <link href="../../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../../js/jqueryUI/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>

    <script>
        layui.config({
            base: "js/"
        }).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
            var form = layui.form(),
                layer = layui.layer,
                laypage = layui.laypage;
            var $ = layui.jquery;
            form.render();
            var editor_textarea = new UE.ui.Editor();
            editor_textarea.render("txtBody");
        });
    </script>

</body>
</html>
