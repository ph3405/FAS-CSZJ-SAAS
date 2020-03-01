<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sfWarnReport.aspx.cs" Inherits="view_sfWarnReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应收应付管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="../../../../css/grid.css" media="all" />
    <style>
        .layui-table td, .layui-table th {
            padding: 2px 3px;
            font-size: 12px;
            height: 25px;
        }

        .layui-table td {
            text-overflow: initial;
            white-space: normal;
            text-wrap: normal;
        }
    </style>

</head>
<body class="childrenBody layui-form">
    <blockquote class="layui-elem-quote ">
       <%-- <div class="layui-inline">收付日期</div>
        <div class="layui-inline"  >
         
            <input type="text" id="StartSFDate" name="StartSFDate" class="layui-input laydate-icon " style="height: 36px; width: 120px" value="" />
        </div>
        <div class="layui-inline">至</div>
        <div class="layui-inline"  style="width:120px;">
           <input type="text" id="EndSFDate" name="EndSFDate" class="layui-input laydate-icon " style="height: 36px;" value="" />
        </div>
        <div class="layui-inline">
            发票号码
        </div>
         <div class="layui-inline" >
        
            <input type="text" id="InvoiceNo" value="" class="layui-input search_input" />
        </div>
        <div class="layui-inline">
            客户/供应商
        </div>
         <div class="layui-inline" >
        
            <input type="text" id="BasicDataName" value="" class="layui-input search_input" />
        </div>

        <div class="layui-inline">

            <a id="btnSearch" class="layui-btn ">查询</a>

        </div>--%>

        <div class="layui-inline">
            您有如下应收应付即将到期
        </div>
        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div>

        <table class="layui-table"  style="width:70%">
            <thead>
                <tr>
                    <th width="50">收付日期</th>
                    <th width="50">收付金额</th>

                    <th width="50">收付状态</th>
                    <th width="150">细项备注</th>
                </tr>
            </thead>
            <tbody id="dt1"></tbody>
        </table>

    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        {{for Head}}
         <tr style="background-color:#daf9d9">
            <td colspan="3" style="text-align:left;font-weight: bold; color: gray; border-right: none">
                操作日期：{{>~ShortTimeFormatter(CreateDate)}}
                &nbsp;&nbsp;  发票日期：{{>~YearMonthDay(InvoiceDate)}}     
            </td>
           <td  style="border-left:none">

<%--                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:    Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>--%>
        
           </td>
        </tr>
        <tr style="background-color:#daf9d9">
            <td colspan="3" style="text-align:left;font-weight: bold; color: gray; border-right: none">
                发票号码：{{:InvoiceNo}}
            </td>
            <td  style="border-left:none;text-align:left;color: gray;font-weight: bold;">
           发票金额：{{:Money}}
        &nbsp;&nbsp; 坏账：{{:BadMoney}}
           </td>
        </tr>
        <tr style="background-color:#daf9d9">
            <td colspan="3" style="text-align:left;font-weight: bold; color: gray; border-right: none">
                客户/供应商：{{:Name}}
            </td>
                 <td  style="border-left:none;text-align:left;color: gray;font-weight: bold;">
           收付类型：{{:SFType}}

           </td>
        </tr>
        {{/for}}
        {{for Detail}}
        <tr>
            <td style="text-align:left">{{:SFDate}}</td>
            <td style="text-align:right">{{:SFMoney}}</td>
            <td style="text-align:left">{{:SFStatus}}</td>
            <td style="text-align:left">{{:SFRemark}} </td>
        </tr>
        {{/for}}
        {{for Head}}
        <tr style="height:1px;">
            <td colspan="4"></td>

        </tr>
        {{/for}}
    </script>


    <script>
        var token = '<%=Token%>';
    </script>
       <script src="../../../js/LodopFuncs.js"></script>
    <script src="../../../js/numeral.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="sfWarnReport.js?v=20180919"></script>
</body>
</html>
