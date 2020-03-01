<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tplList.aspx.cs" Inherits="view_fas_set_tplList" %>


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
                <input type="text" id="txtTitle" value="" placeholder="标题" class="layui-input search_input">
            </div>
            <div class="layui-input-inline">
                <select id="txtType" name="Type" lay-verify="">
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
          <div class="layui-inline">
            <a id="btnAdd" class="layui-btn  layui-btn-normal">新建</a>
        </div>

    </blockquote>
    <div >
        <table class="layui-table">

            <thead>
                <tr>

                    <th style="width: 150px">标题</th>
                    <th style="width: 100px">类型</th>
                  
                    <th style="width: 100px">操作</th>
                </tr>
            </thead>
            <tbody id="dt"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Title}}</td>
            <td>{{type:Type}}</td>
         
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
    <script type="text/javascript" src="tplList.js"></script>
</body>
</html>

