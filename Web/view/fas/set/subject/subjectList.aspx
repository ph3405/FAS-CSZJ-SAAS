<%@ Page Language="C#" AutoEventWireup="true" CodeFile="subjectList.aspx.cs" Inherits="view_fas_set_subjectList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>科目管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style>
        .layui-form-label {
            width: 50px;
        }

        .layui-input-block {
            margin-left: 80px;
        }
    </style>

</head>
<body class="childrenBody layui-form">
    <blockquote class="layui-elem-quote ">
        <div class="layui-inline">

            <label class="layui-form-label">类别</label>
            <div class=" layui-input-block ">
                <select lay-filter="category">

                    <option value="1" selected>资产</option>
                    <option value="2">负债</option>
                    <option value="3">权益</option>
                    <option value="4">成本</option>
                    <option value="5">损益</option>
                </select>
            </div>


        </div>


        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>

    <table class="layui-table">

        <thead>
            <tr>

                <th width="100">科目编码</th>
                <th width="130">科目名称</th>
                <th width="30">方向</th>
                <th width="150">辅助核算</th>
                <th width="50">数量核算</th>
                <th width="100">外币核算</th>
                <th width="50">状态</th>
                <th width="100">操作</th>
            </tr>
        </thead>
        <tbody id="dt"></tbody>
    </table>

    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr name="{{:ParentId}}">
           
            <td style="text-align: left">{{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp; 
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;        
                {{/if}}
                {{if IsTree=="Y"}}
                 <a class="layui-btn layui-btn-mini  tks-rowTree" data-id="{{:Id}}" data-check="1"><i class="layui-icon">&#xe623;</i></a>
                {{else}}
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 {{/if}}
                {{:Code}}

            </td>
            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:Name}}</td>
            <td>{{if Credit_Debit==0}}借{{else}}贷{{/if}}</td>
            <td>{{:CalHelperValue}} </td>
            <td>{{:QuantityValue}} </td>
            <td>{{:CurrencyValue}} </td>
            <td>{{if IsValid==1}} 正常 {{else}} 不启用 {{/if}} </td>
            <td style="text-align: left">{{if IsUse==0&&SLevel!=4}}
                <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowAdd" data-id="{{:Id}}"><i class="layui-icon">&#xe654;</i></a>
                {{/if}}
                 {{if IsLeaf==1}}
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i></a>
                {{/if}}
                 {{if IsLeaf==1&&IsCustom==1}}
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> </a>
                {{/if}}
            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
   
    <script type="text/javascript" src="subjectList.js?v=20180830"></script>
</body>
</html>

