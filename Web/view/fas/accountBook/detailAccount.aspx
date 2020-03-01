﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailAccount.aspx.cs" Inherits="view_detailAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>明细账</title>
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

        <%--<div class="layui-inline">
            <select id="selPeriod" lay-filter="period">
                <option value="">选择期间</option>

            </select>
        </div>--%>
        <%--<div class="layui-inline" style="width: 80px;">
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
        <div class="layui-inline">-- </div>--%>
        <%--    <div class="layui-inline" style="width: 80px;">
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
        <div class="layui-inline">期</div>--%>
        <%--        <div class="layui-input-inline" style="width: 100px;">
                            <select id="selPeriodS" name="periodS" lay-filter="p1">
                                <option value="">选择期间</option>

                            </select>
                        </div>
                        <div class="layui-input-inline" >-</div>
                        <div class="layui-input-inline" style="width: 100px;">
                            <select id="selPeriodE" name="periodE" lay-filter="p2">
                                <option value="">选择期间</option>

                            </select>
                        </div>--%>
        <%--<div class="layui-inline">
            <select id="selSubject" lay-filter="subject">
                <option value="">选择科目</option>

            </select>
        </div>--%>
        <%--<div class="layui-inline">
            <label class="layui-form-label" style="width: 70px;">科目代码</label>
            <div class="layui-input-inline" style="width: 100px;">
                <input type="text" name="codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />

            </div>
            <div class="layui-input-inline">-</div>
            <div class="layui-input-inline" style="width: 100px;">
                <input type="text" name="codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
            </div>
        </div>--%>
        <input id="chkNum" type="checkbox" name="" title="显示数量" lay-skin="primary" lay-filter="num" />
        <%-- <input type="checkbox" name="" title="所有凭证" lay-skin="primary" lay-filter="allPZ" > --%>
        <div class="layui-inline">
            <a class="layui-btn layui-btn-primary" id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>
            <%-- <a id="btnSearch" class="layui-btn ">查询</a>--%>

            <a id="btnPrint" class="layui-btn  ">打印</a>
            <%--  <a class="layui-btn layui-btn-primary" id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>--%>
            <a class="layui-search-footer">
                <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">查询</button>
            </a>
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
        </div>
    </blockquote>
    <div>

        <table class="layui-table">

            <thead>
                <tr id="tbHead">
                </tr>
            </thead>
            <tbody id="dt1"></tbody>
        </table>



    </div>
    <div id="page"></div>

    <%--全部--%>
    <script id="tpl-head01" type="text/x-jsrender">
        <th width="100">日期</th>
        <th width="60">凭证字号</th>

        <th width="100">科目</th>
        <th width="100">摘要</th>
          <th width="100">期初</th>
        <th width="100">借方<br />
            数量</th>
        <th width="100">借方<br />
            单价</th>
        <th width="100">借方<br />
            金额</th>
        <th width="100">贷方<br />
            数量</th>
        <th width="100">贷方<br />
            单价</th>
        <th width="100">贷方<br />
            金额</th>
        <th width="100">余额<br />
            方向</th>
        <th width="100">余额<br />
            数量</th>
        <th width="100">余额<br />
            单价</th>
        <th width="100">余额<br />
            金额</th>
    </script>

    <%--不显示数量--%>
    <script id="tpl-head02" type="text/x-jsrender">
        <th width="80">日期</th>
        <th width="60">凭证字号</th>

        <th width="120">科目</th>
        <th width="100">摘要</th>
         <th width="100">期初</th>
        <th width="100">借方<br />
            金额</th>

        <th width="100">贷方<br />
            金额</th>
        <th width="100">余额<br />
            方向</th>

        <th width="100">余额<br />
            金额</th>
    </script>

    <%--全部--%>
    <script id="tpl-list01" type="text/x-jsrender">
        <tr>
            <td style="text-align: left;">{{:~YearMonthDay(DetailDate)}}
            </td>
            <td>{{:PZZ}} </td>
            <td>{{:Name}} </td>
            <td>{{:Summary}}  </td>
             <td>{{if StartBAL!=0}} {{:StartBAL}}  {{/if}}</td>
            <td>{{if NUM1!=0}} {{:NUM1}} {{/if}} </td>
            <td>{{if Unit_Price1!=0}} {{:Unit_Price1}}  {{/if}}</td>
            <td>{{if Money1!=0}} {{:Money1}} {{/if}} </td>
            <td>{{if NUM2!=0}} {{:NUM2}} {{/if}} </td>
            <td>{{if Unit_Price2!=0}} {{:Unit_Price2}}  {{/if}}</td>
            <td>{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
            <td>
                {{if Show_Credit_Debit==0}} 借 
                {{else Show_Credit_Debit==1}}
                贷 
                {{else}}平
                {{/if}} 

            </td>
            <td>{{if Show_Quantity!=0 }} {{:Show_Quantity}} {{/if}} </td>
            <td>{{if Show_Unit_Price!=0 }} {{:Show_Unit_Price}} {{/if}} </td>

            <td>{{if Show_Money!=0}} {{:Show_Money}}  {{/if}}</td>
        </tr>
    </script>
    <%--不显示数量--%>
    <script id="tpl-list02" type="text/x-jsrender">
        <tr>
            <td style="text-align: left;">{{:~YearMonthDay(DetailDate)}}
              <%--  &nbsp;&nbsp;{{:~YearMonthDay(CreateDate)}}&nbsp;&nbsp;{{:Seq}} --%>
            </td>
            <td>{{:PZZ}} </td>
            <td>{{:Name}} </td>
            <td>{{:Summary}}  </td>
             <td>{{if StartBAL!=0}} {{:StartBAL}}  {{/if}}</td>
            <td>{{if Money1!=0}} {{:Money1}} {{/if}} </td>

            <td>{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
            <td>
                {{if Show_Credit_Debit==0}} 借 
                {{else Show_Credit_Debit==1}}
                贷 
                {{else}}平
                {{/if}} 

            </td>

            <td>{{if Show_Money!=0}} {{:Show_Money}}  {{/if}}</td>
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

    <script id="tpl-subject" type="text/x-jsrender">
        <option data-title="{{:Value}}" value="{{:Code}}">{{:Value}}</option>
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
                height: 60px;
            }

            #tb thead {
                font-size: 14px;
            }

            #tb tbody, #tb tfoot {
                font-size: 12px;
            }
        </style>

        <table style="width: 840px;">
            <tr style="height: 30px;">
                <td style="width: 33.3333%"></td>
                <td style="font-size: 20px; text-align: center; font-weight: 400">{{:Title}} 明细账</td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right"></td>
            </tr>
            <tr style="height: 30px;">
                <td style="font-size: 12px; width: 33.3333%">科目：{{:Subject}}</td>
                <td style="font-size: 12px; text-align: center;">{{:Period}}</td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right">单位：{{:Unit}}</td>
            </tr>
        </table>
        <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td style="text-align: center; width: 100px">日期</td>
                    <td style="text-align: center; width: 50px">凭证字号</td>
                    <td style="text-align: center; width: 120px">科目</td>
                    <td style="text-align: center; width: 120px">摘要</td>
                    <td style="text-align: center; width: 120px">借方</td>
                    <td style="text-align: center; width: 120px">贷方</td>
                    <td style="text-align: center; width: 50px">方向</td>

                    <td style="text-align: center; width: 120px; border-right: none">金额</td>

                </tr>
            </thead>
            <tbody>
                {{for Detail}}
                <tr>
                    <td style="text-align: center">{{:~YearMonthDay(DetailDate)}}</td>
                    <td style="text-align: center">{{:PZZ}}</td>
                    <td style="text-align: left">{{:Name}}</td>
                    <td style="text-align: left">{{:Summary}}</td>
                    <td style="text-align: right">{{if Money1!=0}} {{:Money1}} {{/if}} </td>
                    <td style="text-align: right">{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
                    <td style="text-align: center">{{if Show_Credit_Debit==0}} 借 {{else}} 贷 {{/if}} </td>
                    <td style="text-align: right; border-right: none">{{if Show_Money!=0}} {{:Show_Money}}  {{/if}}</td>
                </tr>
                {{/for}}
          

            </tbody>


        </table>

        <table style="width: 840px; font-size: 12px; height: 40px;">
            <tr>
                <td style="width: 20%">编制单位：{{:Company}}</td>
                <td style="width: 20%"></td>
                <td style="width: 20%"></td>
                <td style="width: 20%; text-align: right">打印日期：{{:PrintDate}}</td>
                <td style="width: 20%; text-align: right">【章小算财税】</td>
            </tr>
        </table>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script src="../../../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <link href="../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../js/jqueryUI/jquery-ui.js"></script>
    <script type="text/javascript" src="detailAccount.js?v=2019021201"></script>
</body>
</html>