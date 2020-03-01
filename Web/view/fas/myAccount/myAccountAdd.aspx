<%@ Page Language="C#" AutoEventWireup="true" CodeFile="myAccountAdd.aspx.cs" Inherits="view_fas_set_myAccountAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title> </title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="../../../../css/grid.css"/>
    <style type="text/css">
       
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 50%;">
        <script id="tpl-Edit" type="text/x-jsrender">
          
                <div class="layui-form-item">
                <label class="layui-form-label">邀请码</label>
                <div class=" layui-input-inline">
                    <input type="text" class="layui-input " name="InvitationCode" lay-verify="required" placeholder="">
                </div>
            </div>
         
          
            

            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                  
                </div>
            </div>

        </script>

    </form>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
 
    <script type="text/javascript" src="myAccountAdd.js"></script>
</body>
</html>
