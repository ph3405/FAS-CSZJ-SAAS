<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customDataAdd.aspx.cs" Inherits="view_fas_set_customDataAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>自定义数据新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style>
        .fas-del {
            float: left;
            margin: 5px 15px 5px 0px;
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="layui-form-item" style="display:none">
                <label class="layui-form-label">编码</label>
                <div class=" layui-input-inline">
                    <input type="text" class="layui-input " name="Code"  placeholder="">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">名称</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input " name="Name" lay-verify="required" placeholder="">
                </div>
            </div>

            <div id="colContainer" >
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">备注</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input " name="Memo" placeholder="">
                </div>
            </div>
               <div class="layui-form-item">
                <label class="layui-form-label">是否启用</label>
                <div class="layui-input-inline">
                    <input type="checkbox" name="IsValid" lay-skin="switch"  checked >
                   
                </div>
             
            </div>


            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>

                </div>
            </div>

        </script>

    </form>
    <script id="tpl-col" type="text/x-jsrender">
        <div class="layui-form-item">
            <label class="layui-form-label">{{:ColumnName}}</label>
            <div class="layui-input-inline ">
                <input type="text" name="Custom{{:ColumnCode}}" class="layui-input" />
            </div>
        </div>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="customDataAdd.js"></script>
</body>
</html>
