<%@ Page Language="C#" AutoEventWireup="true" CodeFile="currencyEditor.aspx.cs" Inherits="view_fas_set_currencyEditor" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>币制编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style type="text/css">
      
    </style>
    <script>
        function toUpperCase(obj) {
            obj.value = obj.value.toUpperCase()
        }
    </script>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
               <div class="layui-form-item">
                <label class="layui-form-label">编码</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input " value="{{:Code}}" onkeyup="toUpperCase(this)" name="Code" lay-verify="required" placeholder="例如：USD">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">名称</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input" value="{{:Name}}" name="Name" lay-verify="required" placeholder="例如：美元">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">汇率</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input rule-num-input" value="{{:ExchangeRate}}" name="ExchangeRate" lay-verify="required" placeholder="例如：6.8">
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
 
    <script type="text/javascript" src="currencyEditor.js?_=20171119"></script>
</body>
</html>
