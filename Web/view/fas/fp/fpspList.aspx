<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpspList.aspx.cs" Inherits="view_fas_set_fpspList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>财务发票管理</title>
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
            <a id="btnQuery" class="layui-btn  ">查询</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="70">发票编号</th>
                    <th width="100">发票类型</th>
                    <th width="50">增票</th>
                    <th width="70">收付状态</th>
                    <th width="70">支付方式</th>
                    <th width="100">发票状态</th>
                    <th width="50">发票</th>
                     <th width="70">凭证字</th>
                    <th width="200">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:InvoiceNo}}</td>
            <td>{{type:Type}}</td>

            <td>{{if IsTaxYZ==1}}{{if IsVAT==1}} 是 {{else}} 否 {{/if}}{{/if}} </td>
            <td>{{if IsTaxYZ==1}}{{if RPStatus==0}} 未收付 {{else}} 已收付 {{/if}}{{/if}}</td>
             <td>{{if IsTaxYZ==1}}{{if PayMode==0}} 现金 
                {{else PayMode==1}} 银行转账 
                {{else PayMode==2}} 承兑汇票 
                {{else }} 
                {{/if}} {{/if}}</td>
            <td>{{if Status==0}} 草稿 {{else Status==1 }} 递交财务 {{else}} 财务立账 {{/if}}</td>
            <td>{{:InvoiceNum}}</td>
            <td>{{:PZZ}}</td>
            <td style="text-align:left">{{if Status==1}}
                <a class="layui-btn layui-btn-mini tks-rowLZ" data-id="{{:Id}}">生成凭证</a>


            

                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}">编辑</a>

                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"> 删除</a>
                {{/if}}

                {{if Status==2}}
                  <a class="layui-btn layui-btn-mini tks-rowView" data-id="{{:PZId}}">查看凭证</a>
                
                {{/if}}
                    <a class="layui-btn layui-btn-mini tks-rowFP" data-id="{{:Id}}">附件</a>

            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fpspList.js?_=20180108"></script>
</body>
</html>

