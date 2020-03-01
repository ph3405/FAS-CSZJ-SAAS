<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocDetailAccount.aspx.cs" Inherits="view_fas_accountBook_DocDetailAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>序时账</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="../../../../css/grid.css" media="all" />
    <link rel="stylesheet" href="../../../css/searchmore.css" />
    <style>
        .layui-table td, .layui-table th {
            padding: 2px 3px;
            font-size: 12px;
            height: 40px;
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
       
        <div class="layui-inline">
            <a class="layui-btn layui-btn-primary"  id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>
            <a id="btnPrint" class="layui-btn  ">打印</a>
        </div>
          <%--更多--%>
         <div class="layui-search-mored layui-anim layui-anim-upbit">
            <div class="layui-search-body">
                <div class="layui-form-item">
                    <div class="layui-inline">
                        <label class="layui-form-label">开始期间</label>
                        <div class="layui-inline" style="width: 80px;">
                            <select id="selYearS" lay-filter="selYearS">
                                <option value=""></option>

                            </select>
                        </div>
                        <div class="layui-inline">年</div>
                        <div class="layui-inline" style="width: 70px;">
                            <select id="selMonthS" name="selMonthS" lay-filter="">
                                <option value=""></option>

                            </select>
                        </div>
                        <div class="layui-inline">期</div>
                    </div>
                </div>
                <div class="layui-form-item">
                    <div class="layui-inline">
                        <label class="layui-form-label">结束期间</label>
                        <div class="layui-inline" style="width: 80px;">
                            <select id="selYearE" lay-filter="selYearE">
                                <option value=""></option>

                            </select>
                        </div>
                        <div class="layui-inline">年</div>
                        <div class="layui-inline" style="width: 70px;">
                            <select id="selMonthE" name="selMonthE" lay-filter="">
                                <option value=""></option>

                            </select>
                        </div>
                        <div class="layui-inline">期</div>
                    </div>
                </div>
                <div class="layui-form-item">
                    <div class="layui-inline">
                        <label class="layui-form-label">科目代码</label>
                        <div class="layui-input-inline" style="width: 100px;">
                            <input type="text" name="codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />

                        </div>
                        <%--<div class="layui-input-inline">-</div>--%>
                        <div class="layui-input-inline" style="width: 100px;">
                            <input type="text" name="codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                </div>
               
            </div>
             <div class="layui-search-footer">
                <button id="btnCancel" class="layui-btn layui-btn-sm layui-btn-primary layui-btn-more">取消</button>
                <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">查询</button>
            </div>
          </div>
      
    </blockquote>
    <div>

        <table class="layui-table">

            <thead>
                <tr id="tbHead">
                    <th width="50">日期</th>
                     <th width="50">凭证号</th>
                     <th width="100">科目编码</th>
                     <th width="200">科目名称</th>
                     <th width="150">摘要</th>
                     <th width="50">方向</th>
                     <th width="100">金额</th>
                </tr>
            </thead>
            <tbody id="dt1"></tbody>
        </table>



    </div>
    <div id="page"></div>


    <script id="tpl-list" type="text/x-jsrender">
        <tr>
                     
              <td style="text-align:center"> {{>~YearMonthDay(PZDate)}}</td>
            <td style="text-align:left">
                {{>PZZ}}-{{:PZZNO}}

            </td>
       
            <td style="text-align:left">{{:SubjectCode}} </td>
        
            <td style="text-align:left">{{:Name}}  </td>
            <td style="text-align:left">{{:Summary}} </td>
         
            <td>{{:Credit_Debit}} </td>
         <td>{{:amt}} </td>
        </tr>
    </script>


    <script id="tpl-Year" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Year}}">{{:Year}}</option>
    </script>
     <script id="tpl-Month" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Id}}" >{{:Month}}</option>
    </script>
    <script id="tpl-select" type="text/x-jsrender">
        <option data-title="{{:Year}}年{{:Month}}期" value="{{:Id}}">{{:Year}}-{{:Month}}</option>
    </script>
 
    
    <script>
        var token = '<%=Token%>';
    </script>
      <script src="../../../js/LodopFuncs.js"></script>

    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script type="text/javascript" src="DocDetailAccount.js?v=14"></script>
</body>
</html>
