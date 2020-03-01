<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pzzEditor.aspx.cs" Inherits="view_fas_set_pzzEditor" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>凭证字编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style type="text/css">
      
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="layui-form-item">
                <label class="layui-form-label">凭证字</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input " value="{{:PZZ}}" name="PZZ" lay-verify="required" placeholder="">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">显示标题</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input" value="{{:ShowTitle}}" name="ShowTitle" lay-verify="required" placeholder="">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">是否默认</label>
                <div class="layui-input-inline">
                       <input type="checkbox" name="IsDefault" lay-skin="switch" {{if IsDefault==1}} checked {{/if}} />
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
 
    <script type="text/javascript" src="pzzEditor.js"></script>
</body>
</html>
