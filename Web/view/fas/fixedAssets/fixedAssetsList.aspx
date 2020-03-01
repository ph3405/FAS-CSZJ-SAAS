<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fixedAssetsList.aspx.cs" Inherits="view_fas_fixedAssetsList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>固定资产管理</title>
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

        <div class="layui-inline">
            <label class="layui-form-label" style="width: 30px;">期间</label>
            <div class="layui-inline" style="width: 80px;">
                <select id="selYear" lay-filter="selYear">
                    <option value=""></option>

                </select>
            </div>
            <div class="layui-inline">年</div>
            <div class="layui-inline" style="width: 70px;">
                <select id="selMonth" lay-filter="">
                    <option value=""></option>

                </select>
            </div>
            <div class="layui-inline">期</div>
        </div>
        <div class="layui-inline">
            <div class="layui-input-inline">
                <input type="text" id="txtName" value="" placeholder="请输入名称" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
           
        </div>
        <div class="layui-inline">
             <a class="layui-btn" id="print">打印</a>
        </div>
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal ">增加资产</a>
             <a id="btnImport" class="layui-btn layui-btn-normal ">导入</a>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  " style="width:99%">
        <table class="layui-table">

            <thead>
                <tr>
                    <th width="80">资产编号</th>
                    <th width="150">资产类别</th>
                    <th width="100">资产名称</th>
                    <th width="100">本期折旧额</th>
                    <th width="100">开始使用日期</th>
                    <th width="100">资产原值</th>
                  
                    <th width="80">资产净值</th>
                       <th width="50">状态</th>
                    <th width="100">操作</th>
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:DocNo}}</td>
            <td>{{assetsClass:AssetsClass}}</td>
            <td>{{:Name}}</td>
            <td>{{noZero:Amount}} </td>
            <td>{{:~YearMonthDay(StartUseDate)}}</td>
            <td>{{:InitialAssetValue}} </td>
            <td>{{:ZCJZ}} </td>
             <td>{{status:Status}} </td>
            <td>
              

                {{if IsGenPZ==1&&Status==0}}
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}">查看</a>
                  <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDeal" data-id="{{:Id}}">处置</a>


                {{else IsGenPZ==0}}
                  <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}">编辑</a>
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}">删除</a>
                {{else }}
                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}">查看</a>
                {{/if}}
            </td>
        </tr>
    </script>
    <script id="tpl-Year" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Year}}">{{:Year}}</option>
    </script>
     <script id="tpl-Month" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Id}}" >{{:Month}}</option>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fixedAssetsList.js?_=20181206"></script>
</body>
</html>

