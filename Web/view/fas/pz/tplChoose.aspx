<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tplChoose.aspx.cs" Inherits="view_fas_set_tplChoose" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模板管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />


</head>
<body class="childrenBody layui-form">
    <blockquote class="layui-elem-quote ">

        <div class="layui-inline">
          
            <div class="layui-input-inline">
                <select id="txtType" name="Type" lay-verify="">
                    <option value="-1" selected>全部</option>
                    <option value="0">日常支出</option>
                    <option value="1">采购</option>
                    <option value="2">销售</option>
                    <option value="3">工资</option>
                    <option value="4">税金</option>
                    <option value="5">折旧和摊销</option>
                </select>
            </div>
        </div>


        <div class="layui-inline">
            <a class="layui-btn search_btn">查询</a>
        </div>
       

    </blockquote>
    <div >
        <table class="layui-table">

            <thead>
                <tr>

                    <th style="width: 150px">模板标题</th>
                 
                  
                
                </tr>
            </thead>
            <tbody id="dt"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr data-id="{{:Id}}">

            <td>{{:Title}}</td>
          <%--  <td>{{type:Type}}</td>--%>
         
         
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="tplChoose.js"></script>
</body>
</html>

