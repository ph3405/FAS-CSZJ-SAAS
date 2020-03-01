<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user2Role.aspx.cs" Inherits="view_user_user2Role" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>已分配用户</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />

    <%--<link rel="stylesheet" href="../../css/user.css" media="all" />--%>
</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote  ">
        <div class="layui-inline">
            <div class="layui-input-inline">
                <input type="text" id="txtName" value="" placeholder="请输入关键字" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
       

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="200">登录名</th>
                    <th width="200">真实姓名</th>
                    <th width="100">手机</th>
                    <th width="100">机构</th>

                    <th width="60">会员状态</th>

                    <th>操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:UserName}}</td>
            <td>{{:TrueName}}</td>
            <td>{{:Mobile}}</td>

            <td>{{:NodeName}}</td>
            <td>{{if Status==0}}  未启用   {{else}}   启用   {{/if}} 

            </td>

            <td>
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
            </td>
        </tr>
    </script>
    <script>
        var token='<%=Token%>';
    </script>
    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script type="text/javascript" src="user2Role.js"></script>
</body>
</html>
