<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vendorList.aspx.cs" Inherits="view_fas_vendorList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>固定资产管理</title>
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
                <input type="text" id="txtName" value="" placeholder="请输入名称" class="layui-input search_input"/>
            </div>
            <a class="layui-btn search_btn">查询</a>
           
        </div>
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal ">增加供应商</a>
            <%-- <a id="btnImport" class="layui-btn layui-btn-normal ">导入</a>--%>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  " style="width:99%">
        <table class="layui-table">

            <thead>
                <tr>
                    <th width="150">供应商名称</th>

                    <th width="100">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Name}}</td>
            <td>
               <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}">编辑</a>
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}">删除</a>
            </td>
        </tr>
    </script>

    <script>
        var token = '<%=Token%>';
        var userId='<%=Id%>';
    </script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="vendorList.js?_=20181015"></script>
</body>
</html>

