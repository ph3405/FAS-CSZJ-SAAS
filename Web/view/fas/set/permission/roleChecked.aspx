<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roleChecked.aspx.cs" Inherits="view_user_RoleChecked" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>设置角色</title>
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
    </form>
    <script id="tpl-Edit" type="text/x-jsrender">
     

        <blockquote class="layui-elem-quote ">
            企业角色
        </blockquote>
        <div class="layui-form-item">

            <div class="layui-input-block">
                {{for QYRoles}}
                    {{if IsChecked==true}}
                        <input type="radio" name="Role" value="{{:Id}}" title="{{:Name}}" checked="checked"/>
                    {{else}}
                        <input type="radio" name="Role" value="{{:Id}}" title="{{:Name}}"/>
                    {{/if}}
                {{/for}}
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">立即提交</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>

    </script>
      <script>
        var token='<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="roleChecked.js"></script>
</body>
</html>
