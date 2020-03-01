<%@ Page Language="C#" AutoEventWireup="true" CodeFile="funcTreeChecked.aspx.cs" Inherits="view_func_TreeChecked" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>功能选择</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
    <link href="../../css/grid.css" rel="stylesheet" />

</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
       
        <div class="layui-form-item">
            <ul id="funcTree" class="easyui-tree"></ul>

        </div>
         <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">立即提交</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>
    </form>


   


    <script>
        var token='<%=Token%>';
    </script>

    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script src="../../js/easyui/jquery.min.js"></script>
    <!--easyui tree & datagrid-->
    <link href="../../js/easyui/themes/icon.css" rel="stylesheet" />
    <link href="../../js/easyui/themes/color.css" rel="stylesheet" />

    <link href="../../js/easyui/themes/metroBlue/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/easyui/jquery.easyui.min.js"></script>

    <script type="text/javascript" src="../../js/easyui/easyui-lang-zh_CN.js"></script>
    <!--easyui tree & datagrid-->
    <script type="text/javascript" src="funcTreeChecked.js?_=20171214"></script>
</body>
</html>
