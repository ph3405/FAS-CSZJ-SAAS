<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kmChoose.aspx.cs" Inherits="view_fas_set_kmChoose" %>


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
        <div class="layui-form-item">
            <div class="layui-inline">
                <label class="layui-form-label" style="width: 30px;">类型</label>

                <div class="layui-input-inline" style="width: 80px;">
                    <select id="Category" lay-filter="category">
                        <option value="1" selected>资产</option>
                        <option value="2">负债</option>
                        <option value="3">权益</option>
                        <option value="4">成本</option>
                        <option value="5">损益</option>
                    </select>

                </div>
                <div class="layui-inline">编号</div>
                <div class="layui-inline" style="width: 120px;">
                    <input type="text" id="Code" value="" placeholder="" class="layui-input search_input" />
                </div>


            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label" style="width: 30px;">描述</label>

            <div class="layui-inline" style="width: 250px;">
                <input type="text" id="ShowTitle" value="" placeholder="" class="layui-input search_input" />
            </div>
            <div class="layui-inline">
                <a class="layui-btn search_btn">查询</a>
            </div>
        </div>

    </blockquote>
    <div >
        <table class="layui-table">

            <thead>
                <tr>

                    <th style="width: 150px">科目</th>
                 
                  
                
                </tr>
            </thead>
            <tbody id="dt"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr data-id="{{:Id}}" 
            data-CreditDebit="{{:Credit_Debit}}" 
            data-IsCalHelperValid="{{:IsCalHelperValid}}" 
            data-IsCurrencyValid="{{:IsCurrencyValid}}" 
            data-IsQuantityValid="{{:IsQuantityValid}}" 
            data-label="{{:label}}" 
            data-value="{{:value}}">

            <td>{{:value}}</td>
     
         
         
        </tr>

    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="kmChoose.js?v=4"></script>
</body>
</html>

