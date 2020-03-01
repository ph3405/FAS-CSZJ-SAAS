<%@ Page Language="C#" AutoEventWireup="true" CodeFile="calHelperList.aspx.cs" Inherits="view_fas_set_account_calHelperList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>辅助核算类别</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style>
        .tks-rowOpen:hover{
            text-decoration:underline;
            color:darkcyan;
        }
    </style>

</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote news_search">
       
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal "><i class="layui-icon">&#xe654;</i>新增自定义辅助核算</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table" style="width:50%">

            <thead>
                <tr>

                    <th width="500">名称</th>
                   
                    <th width="100">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td><a style="cursor:pointer" data-url="/view/fas/set/caldata/customDataList.aspx?id={{:Id}}" class=" tks-rowOpen"   data-id="{{:Id}}"><cite>{{:Title}}</cite></a></td>
          
            <td style="text-align:left">
               {{if IsCustom==1}}
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>

                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
               {{/if}}
            </td>
        </tr>
    </script>
    <script>
        var token='<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="calHelperList.js"></script>
</body>
</html>

