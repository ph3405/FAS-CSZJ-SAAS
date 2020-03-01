<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newsList.aspx.cs" Inherits="view_fas_newsList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>资讯管理</title>
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
                <input type="text" id="txtTitle" value="" placeholder="请输入名称" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal ">增加资讯</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="150">编号</th>
                    <th width="60">状态</th>
                    <th width="70">作者</th>
                    <th width="100">标题</th>
                    <th width="100">概述</th>
                    <th width="150">创建时间</th>
                    <th width="150">发布时间</th>



                    <th width="150">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:id}}</td>
            <td>{{toStatus:status}}</td>
            <td>{{:author_name}}</td>
            <td>{{:title}} </td>
            <td>{{:summary}}</td>
            <td>{{:~TimeFormatter(created_at)}}</td>
            <td>{{:~TimeFormatter(published_at)}} </td>
           
            <td>
               
                {{if status==0}}
                 <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:id}}">编辑</a>

                    <a class="layui-btn layui-btn-normal layui-btn-mini tks-publish" data-id="{{:id}}">发布</a>
                    <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:id}}">删除</a>
                {{else}}
                    <a class="layui-btn layui-btn-normal layui-btn-mini tks-unpublish" data-id="{{:id}}">取消发布</a>
               
                {{/if}}
            </td>
        </tr>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="newsList.js?_=20181013"></script>
</body>
</html>

