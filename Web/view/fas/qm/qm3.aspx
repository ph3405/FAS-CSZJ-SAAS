<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qm3.aspx.cs" Inherits="view_fas_qm3" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>结账</title>
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
<body class="childrenBody" style="margin-left:50px;">
    <form id="editForm" class="layui-form">
        <fieldset class="layui-elem-field layui-field-title">
            <legend>第四步：结账</legend>
        </fieldset>

        <div id="container" style="height: 200px">
            <div class="layui-form-item">
                <label class="layui-form-label">期末检查已完成</label>
            </div>
            
        </div>

        <div class="layui-clear"></div>
        <a id="btnPre" class="layui-btn ">上一步</a>
        <a id="btnSave" class="layui-btn " lay-submit="" lay-filter="jz">结账</a>

    </form>



    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script type="text/javascript" src="qm3.js?_=201807"></script>
</body>
</html>
