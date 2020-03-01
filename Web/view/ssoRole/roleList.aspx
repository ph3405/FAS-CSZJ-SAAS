<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roleList.aspx.cs" Inherits="view_user_roleList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>角色管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
   
   
</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote ">
        <div class="layui-inline">
            <div class="layui-input-inline">
                <input type="text" id="txtName" value="" placeholder="请输入关键字" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
        <div class="layui-inline">
            <a class="layui-btn layui-btn-normal "  id="btnRoleAdd">添加新角色</a>
        </div>
       
        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux">谨慎操作角色</div>
        </div>
    </blockquote>
    <div class="layui-form role-list">
        <table class="layui-table">

            <thead>
                <tr>
                   
                    <th width="100">角色名称</th>
                    <th width="100">描述</th>
                    <th width="80">类型</th>
                    <th width="100">创建人</th>
                    <th width="100">创建日期</th>
                    <th width="150">操作</th>
                </tr>
            </thead>
            <tbody id="roleDt" ></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-roleList" type="text/x-jsrender">
        <tr>
             
            <td>{{:Name}}</td>
             <td>{{:Memo}}</td>
             <td>{{type:Type}}</td>
             <td>{{:CreateUser}}</td>
             <td>{{:~TimeFormatter(CreateDate)}}</td>
            <td>
                 <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowPermission" data-id="{{:Id}}"><i class="layui-icon">&#xe614;</i>分配权限</a>
			
                <%-- <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowUser" data-id="{{:Id}}"><i class="layui-icon">&#xe613;</i>分配用户</a>--%>
			
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>
					
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
		    </td>
        </tr>
    </script>
    <script>
        var token='<%=Token%>';
    </script>
    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script type="text/javascript" src="roleList.js?_=20180107"></script>
</body>
</html>
