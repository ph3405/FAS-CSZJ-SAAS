<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fptplList.aspx.cs" Inherits="view_fptplList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>发票对应模板</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />

    <%--<link rel="stylesheet" href="../../css/user.css" media="all" />--%>
</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote news_search">
        <div class="layui-inline">

            <a class="layui-btn search_btn">查询</a>
        </div>
        <div class="layui-inline">
            <a class="layui-btn layui-btn-normal usersAdd_btn">新增</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form users-list">
        <table class="layui-table">

            <thead>
                <tr>
                    <th width="200">模板</th>
                    <th width="100">发票类型</th>
                    <th width="100">是否增票</th>
                    <th width="100">收付状态</th>
                    <th width="100">支付方式</th>

                    <th>操作</th>
                </tr>
            </thead>
            <tbody id="dt"  ></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Title}}</td>
            <td>{{type:Type}}</td>

            <td>{{if IsVAT==1}} 是 {{else}} 否 {{/if}} </td>
            <td>{{if RPStatus==0}} 未收付 {{else}} 已收付 {{/if}}</td>
            <td>{{if PayMode==0}} 现金 
                {{else PayMode==1}} 银行转账 
                {{else PayMode==2}} 承兑汇票 
                {{else }} 
                {{/if}}</td>
           
 
            <td>
              
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>

                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fptplList.js?v=1"></script>
</body>
</html>
