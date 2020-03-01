<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qm2.aspx.cs" Inherits="view_fas_qm2" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>结转损益</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
  
</head>
<body class="childrenBody" style="margin-left:50px;">
    <form id="editForm" class="layui-form">
        <fieldset class="layui-elem-field layui-field-title">
            <legend>第三步：结转损益</legend>
        </fieldset>

        <div id="container" style="height:200px">
            如果需要结转损益，请<a id="btnGenPZ" style="color:blue;font-weight:bold;cursor:pointer">结转损益</a>
        </div>
        <div class="layui-clear"></div>
       <a id="btnPre" class="layui-btn ">上一步</a>
            <a id="btnNext" class="layui-btn ">下一步</a>
      
    </form>

   

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script type="text/javascript" src="qm2.js?_=20190108"></script>
</body>
</html>
