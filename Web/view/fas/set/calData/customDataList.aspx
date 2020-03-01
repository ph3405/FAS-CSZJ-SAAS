<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customDataList.aspx.cs" Inherits="view_fas_set_customDataList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>辅助核算数据列表</title>
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
                <input type="text" id="txtName" value="" placeholder="请输入编码或名称" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal ">新增</a>
            <a id="btnImport" class="layui-btn layui-btn-normal ">导入</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form">
        <table class="layui-table">
            <thead>
                <tr id="dt-thead">
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>
    <script id="tpl-thead" type="text/x-jsrender">
        <th width="50">编码</th>
        <th width="80">名称</th>
        {{for Data}}
            <th width="100">{{:ColumnName}}</th>
        {{/for}}
        <th width="100">备注</th>
        <th width="150">操作</th>
    </script>
    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Code}}</td>
            <td>{{:Name}}</td>
            {{if Custom1!='#nodata#'}} 
            <td>{{:Custom1}} </td>
            {{/if}}
               {{if Custom2!='#nodata#'}} 
            <td>{{:Custom2}} </td>
            {{/if}}
               {{if Custom3!='#nodata#'}} 
            <td>{{:Custom3}} </td>
            {{/if}}
               {{if Custom4!='#nodata#'}} 
            <td>{{:Custom4}} </td>
            {{/if}}
               {{if Custom5!='#nodata#'}} 
            <td>{{:Custom5}} </td>
            {{/if}}
               {{if Custom6!='#nodata#'}} 
            <td>{{:Custom6}} </td>
            {{/if}}
            {{if Custom7!='#nodata#'}} 
            <td>{{:Custom7}} </td>
            {{/if}}
               {{if Custom8!='#nodata#'}} 
            <td>{{:Custom8}} </td>
            {{/if}}

            <td>{{:Memo}}</td>
            >
            <td style="text-align:left">


                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>

                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="customDataList.js"></script>
</body>
</html>

