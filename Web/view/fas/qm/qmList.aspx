<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qmList.aspx.cs" Inherits="view_fas_qmList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>期末管理</title>
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
           <a id="btnSearch" class="layui-btn ">查询</a>
        </div>
        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="100">年份</th>
                    <th width="100">期间</th>
                    <th width="100">是否结转</th>

                    <th width="100">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Year}}</td>
            <td>{{:Month}}</td>

            <td>{{if IsPay==1}} 是 {{else}} 否 {{/if}} </td>
            <td style="text-align:left">
                
                {{if IsPay==0 && IsActive==1}} 
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe602;</i>结转</a>
                {{/if}} 

                {{if CanUnCarryOver==1}}
                <a class="layui-btn layui-btn-mini layui-btn-danger tks-rowUnCarry" data-id="{{:Id}}">
                    <i class="layui-icon">&#xe603;</i>反结转</a>
           
                {{/if}}
 
            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="qmList.js?_=20190108"></script>
</body>
</html>

