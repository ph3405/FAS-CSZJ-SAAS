<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tplChoose.aspx.cs" Inherits="view_TplChoose" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模板选择</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />


</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote ">
        <div class="layui-inline">
            <div class="layui-input-inline">
                <input type="text" id="txtTitle" value="" placeholder="请输入关键字" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
       
        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux">双击行选择</div>
        </div>
    </blockquote>
    <div class="layui-form ">
        <table class="layui-table">
           
            <thead>
                <tr>
                  
                  <th style="width: 150px">标题</th>
                  <th style="width: 100px">类型</th>
 
                </tr>
            </thead>
            <tbody id="dt"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>
            <td class="tks-code" style="display:none">{{:Id}}</td>
        
             <td  class="tks-name">{{:Title}}</td>
            <td>{{tplTarget:TPLTarget}}</td>
 
        </tr>
    </script>
    <script>
        var token='<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="tplChoose.js"></script>
</body>
</html>
