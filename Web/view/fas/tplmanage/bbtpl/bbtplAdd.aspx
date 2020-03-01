<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bbtplAdd.aspx.cs" Inherits="view_user_UserEditor" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>报表模板新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style type="text/css">
        .layui-form-item .layui-inline {
            width: 33.333%;
            float: left;
            margin-right: 0;
        }

        @media(max-width:1240px) {
            .layui-form-item .layui-inline {
                width: 100%;
                float: none;
            }
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="layui-form-item">
                <label class="layui-form-label">ID</label>
                <div class="layui-input-block">

                    <input type="text" id="txtId" class="layui-input " value="{{:Id}}" name="Id" lay-verify="required" placeholder="" />

                </div>

            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">模板名称</label>
                <div class="layui-input-block">

                    <input type="text" id="txtTPLName" class="layui-input " value="{{:Title}}" name="Title" lay-verify="required" placeholder="" />

                </div>
               
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="">保存</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="bbtplAdd.js"></script>
</body>
</html>
