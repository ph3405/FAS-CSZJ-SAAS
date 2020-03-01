<%@ Page Language="C#" AutoEventWireup="true" CodeFile="calHelperAdd.aspx.cs" Inherits="view_fas_set_account_calHelperAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>自定义辅助核算</title>
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
            <div class="layui-form-item">
                <label class="layui-form-label">辅助核算类别</label>
                <div class=" layui-input-inline">
                    <input type="text" class="layui-input " value="{{:Title}}" name="Title" lay-verify="required" placeholder="">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">默认列</label>
                <div class="  layui-input-inline">
                    <input type="text" class="layui-input layui-disabled" disabled value="编码">
                </div>
                <div class="  layui-input-inline">
                    <input type="text" class="layui-input layui-disabled" disabled value="名称">
                </div>
                <div class="  layui-input-inline">
                    <input type="text" class="layui-input layui-disabled" disabled value="备注">
                </div>
            </div>



            <div class="layui-form-item">
                <label class="layui-form-label">自定义列</label>
                <div class="layui-input-block">
                    <a id="btnAdd" class="layui-btn   ">添加列</a>
                </div>
            </div>
            <div id="colContainer" class="layui-form-item" style="padding-left: 110px">
             
            </div>

           
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                 
                </div>
            </div>

        </script>

    </form>
    <script id="tpl-col" type="text/x-jsrender">
        <div class="layui-input-inline {{:code}}">
            <input type="text" class="layui-input" lay-verify="required" value="{{:field}}" />
        </div>
        <div class="fas-del {{:code}}">
            <a data-id="{{:code}}" class="layui-btn  layui-btn-small"><i class="layui-icon">&#xe640;</i></a>
        </div>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="calHelperAdd.js"></script>
</body>
</html>
