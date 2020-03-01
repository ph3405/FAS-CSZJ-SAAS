<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fixedAssetsChangeList.aspx.cs" Inherits="view_fas_fixedAssetsChangeList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>变更记录</title>
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
                <input type="text" id="txtName" value="" placeholder="请输入名称" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>


        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="100">变动单类别</th>
                    <th width="100">资产编号</th>
                    <th width="100">资产名称</th>
                    <th width="100">变动前内容</th>
                    <th width="100">变动后内容</th>
                    <th width="100">变动期间</th>

                    <th width="100">关联凭证</th>
                    <th width="80">操作</th>

                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{changeType:ChangeType}}</td>
            <td>{{:DocNo}}</td>
            <td>{{:AssetsName}}</td>
            <td>{{:PreContent}} </td>
            <td>{{:CurContent}}</td>
            <td>{{:Period}} </td>
            <td>{{:DocPZZ}} </td>

            <td>{{if DocId==''||DocId==undefined}}

                    {{if (ChangeType==1||ChangeType==2||ChangeType==3||ChangeType==8)  }}
                    <a class="layui-btn layui-btn-mini tks-rowEdit" data-money="{{:CurContent}}" data-tpl="{{:TPLId}}" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>生成凭证</a>
                {{/if}}
                
                {{/if}}
                {{if DocId!=''&&DocId!=undefined  }}
                    {{if ChangeType!=8&&ISCHUZHI!="Y"  }}
                    <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-docid="{{:DocId}}" data-id="{{:Id}}" data-gid="{{:ParentId}}"><i class="layui-icon">&#xe642;</i>取消凭证</a>
                {{/if}}
                    {{/if}}
            </td>
        </tr>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fixedAssetsChangeList.js"></script>
</body>
</html>

