<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lrReport.aspx.cs" Inherits="view_lrReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>利润表</title>
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

        <div class="layui-inline"  style="width:100px;display:none">
            <select id="selPeriod" lay-filter="">
                <option value="">请选择</option>

            </select>
        </div>
        <div class="layui-inline"  style="width:80px;">
            <select id="selYear" lay-filter="selYear">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">年</div>
         <div class="layui-inline"  style="width:70px;">
            <select id="selMonth" lay-filter="">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">期</div>

        <div class="layui-inline">

            <a id="btnSearch" class="layui-btn ">查询</a>
              <a id="btnPrint" class="layui-btn  ">打印</a>
        </div>


        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div>

        <table class="layui-table" style="width:50%">
            <thead>
                <tr>
                    <th width="100">项目</th>
                    <th width="30">行次</th>

                    <th width="100">本年累计金额</th>
                    <th width="100">本期金额</th>
                </tr>
            </thead>
            <tbody id="dt1"></tbody>
        </table>

    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>
            <td style="text-align: left;">{{if Seq==21||Seq==30||Seq==32 }}<b> {{:ColumnName}} </b>{{else}}&nbsp;&nbsp; {{:ColumnName}} {{/if}}
                {{if SourceType==0 }} 
                   <a data-id="{{:Id}}" class="row-edit" style="cursor: pointer"><i class="layui-icon">&#xe63c;</i></a>
                {{/if}}
            </td>
            <td>{{if Seq!=0 }} {{:Seq}} {{/if}}</td>
            <td style="text-align:right">{{if Money_Year!=0 }} {{thousand:Money_Year}} {{/if}}</td>
            <td style="text-align:right">{{if Money_Month!=0 }} {{thousand:Money_Month}} {{/if}} </td>
        </tr>
    </script>

    <script id="tpl-print" type="text/x-jsrender">
          <style>
        #tb td {
            border-right: 1px solid #808080;
            border-bottom: 1px solid #808080;
            padding: 2px;
            letter-spacing: 1px;
        }

        #tb tr {
            height: 40px;
        }

        #tb thead {
            font-size: 14px;
        }

        #tb tbody, #tb tfoot {
            font-size: 12px;
        }
    </style>

    <table style="width:770px;">
        <tr style="height:30px;">
            <td style="width:33.3333%"></td>
            <td style="font-size:20px;text-align:center;font-weight:400">利润表</td>
            <td style="font-size:12px;width:33.3333%;text-align:right"></td>
        </tr>
        <tr style="height:30px;">
            <td style="font-size:12px;width:33.3333%">编制单位：{{:Company}}</td>
            <td style="font-size:12px;text-align:center;">{{:Period}}</td>
            <td style="font-size:12px;width:33.3333%;text-align:right">单位：{{:Unit}}</td>
        </tr>
    </table>
    <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
        <thead>
            <tr>
                <td style="width:250px;text-align:center">项目</td>
                <td style="width:100px;text-align:center">行次</td>
                <td style="width:200px;text-align:center">本年累计金额</td>
                <td style="width:200px;text-align:center">本期金额</td>
            </tr>
        </thead>
        <tbody>
            {{for Items}}
            <tr>
                <td style="text-align:left">{{if Seq==21||Seq==30||Seq==32 }}<b> {{:ColumnName}} </b>{{else}}&nbsp;&nbsp; {{:ColumnName}} {{/if}}
              </td>
                <td style="text-align :center">{{if Seq!=0 }} {{:Seq}} {{/if}}</td>
                <td style="text-align :right">{{if Money_Year!=0 }} {{thousand:Money_Year}} {{/if}}</td>
                <td style="text-align :right">{{if Money_Month!=0 }} {{thousand:Money_Month}} {{/if}} </td>
 
            </tr>
           {{/for}}
        </tbody>
       
    </table>
    </script>
    <script id="tpl-select" type="text/x-jsrender">
        <option data-title="{{:Year}}年{{:Month}}期" {{if IsActive==1}} selected {{/if}} value="{{:Id}}">{{:Year}}-{{:Month}}</option>
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
    <script src="../../../js/numeral.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="lrReport.js?v=20180919"></script>
</body>
</html>
