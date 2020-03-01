<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalBalAccount.aspx.cs" Inherits="view_fas_accountBook_SummaryAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>核算项目余额表</title>
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

        <%--<div class="layui-inline" style="width: 100px;">
            <select id="selPeriod" lay-filter="period">
                <option value="">选择期间</option>

            </select>
        </div>--%>
   <%--     <div class="layui-inline" style="width: 80px;">
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
        <div class="layui-inline">-- </div>
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
        <div class="layui-inline">期</div>--%>
<%--        <div class="layui-input-inline" style="width: 100px;">
            <select id="selPeriodS" name="periodS">
                <option value="">选择期间</option>

            </select>
        </div>
        <div class="layui-input-inline">-</div>
        <div class="layui-input-inline" style="width: 100px;">
            <select id="selPeriodE" name="periodE">
                <option value="">选择期间</option>

            </select>
        </div>--%>
        <%--<div class="layui-inline">
            <select id="selCalItem" lay-filter="calItem">
                <option value="">选择核算项</option>

            </select>
        </div>--%>

        <input id="chkNum" type="checkbox" name="" title="显示数量" lay-skin="primary" lay-filter="num" />
        <input id="chkYear" type="checkbox" name="" title="显示本年累计" lay-skin="primary" lay-filter="year" />
        <div class="layui-inline">
            <a class="layui-btn layui-btn-primary"  id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>
            <%-- <a id="btnSearch" class="layui-btn ">查询</a>--%>
            <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">查询</button>
            <a id="btnPrint" class="layui-btn  ">打印</a>
            <%-- <a class="layui-btn layui-btn-primary"  id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>--%>
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
                        <label class="layui-form-label">选择核算项</label>
                        <div class="layui-inline">
            <select id="selCalItem" lay-filter="calItem">
                <option value="">选择核算项</option>

            </select>
        </div>

                    </div>
                </div>
             
               
            </div>
<%--             <div class="layui-search-footer">
                <button id="btnCancel" class="layui-btn layui-btn-sm layui-btn-primary layui-btn-more">取消</button>
                <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">查询</button>
            </div>--%>
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

    <%--head全部--%>
    <script id="tpl-head1" type="text/x-jsrender">

        <th width="100">编码</th>
        <th width="120">名称</th>


        <th width="100">期初借方<br />
            数量</th>

        <th width="100">期初借方<br />
            单价</th>

        <th width="100">期初借方<br />
            金额</th>


        <th width="100">期初贷方<br />
            数量</th>

        <th width="100">期初贷方<br />
            单价</th>

        <th width="100">期初贷方<br />
            金额</th>

        <th width="100">本期借方<br />
            数量</th>

        <th width="100">本期借方<br />
            单价</th>

        <th width="100">本期借方<br />
            金额</th>


        <th width="100">本期贷方<br />
            数量</th>

        <th width="100">本期贷方<br />
            单价</th>

        <th width="100">本期贷方<br />
            金额</th>

        <th width="100">本年累计借方<br />
            数量</th>

        <th width="100">本年累计借方<br />
            单价</th>

        <th width="100">本年累计借方<br />
            金额</th>


        <th width="100">本年累计贷方<br />
            数量</th>

        <th width="100">本年累计贷方<br />
            单价</th>

        <th width="100">本年累计贷方<br />
            金额</th>

        <th width="100">期末借方<br />
            数量</th>

        <th width="100">期末借方<br />
            单价</th>

        <th width="100">期末借方<br />
            金额</th>


        <th width="100">期末贷方<br />
            数量</th>

        <th width="100">期末贷方<br />
            单价</th>

        <th width="100">期末贷方<br />
            金额</th>

    </script>

    <%--head不显示数量--%>
    <script id="tpl-head2" type="text/x-jsrender">

        <th width="100">编码</th>
        <th width="120">名称</th>

        <th width="100">期初借方<br />
            金额</th>

        <th width="100">期初贷方<br />
            金额</th>

        <th width="100">本期借方<br />
            金额</th>

        <th width="100">本期贷方<br />
            金额</th>

        <th width="100">本年累计借方<br />
            金额</th>

        <th width="100">本年累计贷方<br />
            金额</th>

        <th width="100">期末借方<br />
            金额</th>


        <th width="100">期末贷方<br />
            金额</th>

    </script>

    <%--head不显示本年累计--%>
    <script id="tpl-head3" type="text/x-jsrender">

        <th width="100">编码</th>
        <th width="120">名称</th>


        <th width="100">期初借方<br />
            数量</th>

        <th width="100">期初借方<br />
            单价</th>

        <th width="100">期初借方<br />
            金额</th>


        <th width="100">期初贷方<br />
            数量</th>

        <th width="100">期初贷方<br />
            单价</th>

        <th width="100">期初贷方<br />
            金额</th>

        <th width="100">本期借方<br />
            数量</th>

        <th width="100">本期借方<br />
            单价</th>

        <th width="100">本期借方<br />
            金额</th>


        <th width="100">本期贷方<br />
            数量</th>

        <th width="100">本期贷方<br />
            单价</th>

        <th width="100">本期贷方<br />
            金额</th>


        <th width="100">期末借方<br />
            数量</th>

        <th width="100">期末借方<br />
            单价</th>

        <th width="100">期末借方<br />
            金额</th>


        <th width="100">期末贷方<br />
            数量</th>

        <th width="100">期末贷方<br />
            单价</th>

        <th width="100">期末贷方<br />
            金额</th>

    </script>

    <%--head不显示数量，本年累计--%>
    <script id="tpl-head4" type="text/x-jsrender">

        <th width="100">编码</th>
        <th width="120">名称</th>
        <th width="100">期初借方<br />
            金额</th>
        <th width="100">期初贷方<br />
            金额</th>
        <th width="100">本期借方<br />
            金额</th>
        <th width="100">本期贷方<br />
            金额</th>
        <th width="100">期末借方<br />
            金额</th>
        <th width="100">期末贷方<br />
            金额</th>

    </script>

    <%--list全部--%>
    <script id="tpl-list1" type="text/x-jsrender">
        <tr>


            <td style="text-align: left">{{:Code}}</td>
            <td style="text-align: left">{{:Name}} </td>

            <td>{{noZero:NumStart_J}} </td>
            <td>{{noZero:PriceStart_J}} </td>
            <td>{{noZero:BWBStart_J}} </td>

            <td>{{noZero:NumStart_D}} </td>
            <td>{{noZero:PriceStart_D}} </td>
            <td>{{noZero:BWBStart_D}} </td>

            <td>{{noZero:Num_CJ}} </td>
            <td>{{noZero:Price_CJ}} </td>
            <td>{{noZero:BWB_CJ}} </td>

            <td>{{noZero:NUM_CD}} </td>
            <td>{{noZero:Price_CD}} </td>
            <td>{{noZero:BWB_CD}} </td>

            <td>{{noZero:Num_YJ}} </td>
            <td>{{noZero:Price_YJ}} </td>
            <td>{{noZero:BWB_YJ}} </td>

            <td>{{noZero:Num_YD}} </td>
            <td>{{noZero:Price_YD}} </td>
            <td>{{noZero:BWB_YD}} </td>

            <td>{{noZero:NumEnd_J}} </td>
            <td>{{noZero:PriceEnd_J}} </td>
            <td>{{noZero:BWBEnd_J}} </td>

            <td>{{noZero:NumEnd_D}} </td>
            <td>{{noZero:PriceEnd_D}} </td>
            <td>{{noZero:BWBEnd_D}} </td>
        </tr>
    </script>

    <%--list不显示数量--%>
    <script id="tpl-list2" type="text/x-jsrender">
        <tr>


            <td style="text-align: left">{{:Code}} </td>
            <td style="text-align: left">{{:Name}} </td>


            <td>{{noZero:BWBStart_J}} </td>


            <td>{{noZero:BWBStart_D}} </td>


            <td>{{noZero:BWB_CJ}} </td>


            <td>{{noZero:BWB_CD}} </td>


            <td>{{noZero:BWB_YJ}} </td>


            <td>{{noZero:BWB_YD}} </td>


            <td>{{noZero:BWBEnd_J}} </td>

            <td>{{noZero:BWBEnd_D}} </td>


        </tr>
    </script>

    <%--list不显示本年累计--%>
    <script id="tpl-list3" type="text/x-jsrender">
        <tr>


            <td style="text-align: left">{{:Code}}</td>
            <td style="text-align: left">{{:Name}} </td>

            <td>{{noZero:NumStart_J}} </td>
            <td>{{noZero:PriceStart_J}} </td>
            <td>{{noZero:BWBStart_J}} </td>

            <td>{{noZero:NumStart_D}} </td>
            <td>{{noZero:PriceStart_D}} </td>
            <td>{{noZero:BWBStart_D}} </td>

            <td>{{noZero:Num_CJ}} </td>
            <td>{{noZero:Price_CJ}} </td>
            <td>{{noZero:BWB_CJ}} </td>

            <td>{{noZero:NUM_CD}} </td>
            <td>{{noZero:Price_CD}} </td>
            <td>{{noZero:BWB_CD}} </td>

       
            <td>{{noZero:NumEnd_J}} </td>
            <td>{{noZero:PriceEnd_J}} </td>
            <td>{{noZero:BWBEnd_J}} </td>

            <td>{{noZero:NumEnd_D}} </td>
            <td>{{noZero:PriceEnd_D}} </td>
            <td>{{noZero:BWBEnd_D}} </td>
        </tr>
    </script>

       <%--list不显示数量、本年累计--%>
    <script id="tpl-list4" type="text/x-jsrender">
        <tr>
            <td style="text-align: left">{{:Code}}</td>
            <td style="text-align: left">{{:Name}} </td>
            <td>{{noZero:BWBStart_J}} </td>
            <td>{{noZero:BWBStart_D}} </td>
            <td>{{noZero:BWB_CJ}} </td>
            <td>{{noZero:BWB_CD}} </td>
            <td>{{noZero:BWBEnd_J}} </td>
            <td>{{noZero:BWBEnd_D}} </td>
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
    <script id="tpl-calItem" type="text/x-jsrender">
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
            height: 40px;
        }

        #tb thead {
            font-size: 14px;
        }

        #tb tbody, #tb tfoot {
            font-size: 12px;
        }
    </style>

    <table style="width:1050px;">
        <tr style="height:30px;">
            <td style="width:33.3333%"></td>
            <td style="font-size:20px;text-align:center;font-weight:400">核算项目余额表</td>
            <td style="font-size:12px;width:33.3333%;text-align:right"></td>
        </tr>
        <tr style="height:30px;">
            <td style="font-size:12px;width:33.3333%">辅助核算类别：{{:CalItem}}</td>
            <td style="font-size:12px;text-align:center;">{{:Period}}</td>
            <td style="font-size:12px;width:33.3333%;text-align:right">单位：{{:Unit}}</td>
        </tr>
    </table>
    <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
        <thead>
            <tr>
                <td rowspan="2" style="text-align:center;width:100px">编码</td>
                <td rowspan="2" style="text-align :center;width:100px">名称</td>

                <td colspan="2" style="text-align:center;width:120px">期初余额</td>

                <td colspan="2" style="text-align:center;width:120px">本期发生额</td>
                <td colspan="2" style="text-align:center;width:120px">本年累计发生额</td>
                <td colspan="2" style="text-align:center;width:120px">期末余额</td>
            </tr>
            <tr>

                <td style="width:100px">借方</td>
                <td style="width:100px">贷方</td>
                <td style="width:100px">借方</td>
                <td style="width:100px">贷方</td>
                <td style="width:100px">借方</td>
                <td style="width:100px">贷方</td>
                <td style="width:100px">借方</td>
                <td style="width:100px">贷方</td>

            </tr>
        </thead>
        <tbody>
            {{for Items}}
            <tr>
                <td style="text-align:left">{{:Code}} </td>
                <td style="text-align :left">{{:Name}}</td>
                <td style="text-align :right">{{noZero:BWBStart_J}}</td>
                <td style="text-align :right">{{noZero:BWBStart_D}}</td>
                <td style="text-align :right">{{noZero:BWB_CJ}}</td>
                <td style="text-align:right">{{noZero:BWB_CD}}</td>
                <td style="text-align :right">{{noZero:BWB_YJ}}</td>
                <td style="text-align:right">{{noZero:BWB_YD}}</td>
                <td style="text-align :right">{{noZero:BWBEnd_J}}</td>
                <td style="text-align:right;border-right:none">{{noZero:BWBEnd_D}}</td>
            </tr>
            {{/for}}
        </tbody>
     
    </table>

    <table style="width:1050px;font-size:12px;height:40px;">
        <tr>
            <td style="width:20%">编制单位：{{:Company}}</td>
            <td style="width:20%"></td>
            <td style="width:20%"></td>
            <td style="width:20%;text-align:right">打印日期：{{:PrintDate}}</td>
            <td style="width:20%;text-align:right">【章小算财税】</td>
        </tr>
    </table>

    </script>
    <script>
        var token = '<%=Token%>';
    </script>
     <script src="../../../js/LodopFuncs.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="CalBalAccount.js?_=20180925"></script>
</body>
</html>
