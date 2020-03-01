<%@ Page Language="C#" AutoEventWireup="true" CodeFile="roleAdd.aspx.cs" Inherits="view_user_RoleAdd" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>角色新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
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
                <label class="layui-form-label">角色名</label>
                <div class="layui-input-block">
                    <input type="text" class="layui-input    " value="{{:Name}}" name="Name" lay-verify="required" placeholder="请输入角色名">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">类型</label>
                <div class="layui-input-block">
                    <select name="Type">
                         <option value="1" {{if Type==1}} selected {{/if}}>平台角色</option>
                         <option value="2" {{if Type==2}} selected {{/if}}>代帐企业角色</option>
                         <option value="3" {{if Type==3}} selected {{/if}}>雇主企业角色</option>
                           
                    </select>
                </div>
            </div>
             <div class="layui-form-item">
                <label class="layui-form-label">权限</label>
                <div class="layui-input-block">
                    <input type="checkbox" name="平台管理员" title="平台管理员">
                    <input type="checkbox" name="组织机构管理员" title="组织机构管理员">
                    <input type="checkbox" name="组织机构会计" title="组织机构会计">

                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">描述</label>
                <div class="layui-input-block">
                    <textarea class="layui-textarea " name="Memo">{{:Memo}}</textarea>
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">立即提交</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script type="text/javascript" src="roleAdd.js"></script>
</body>
</html>
