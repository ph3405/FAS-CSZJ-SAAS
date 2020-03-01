<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sjReport.aspx.cs" Inherits="view_fas_Report_sjReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>税金表</title>
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
            <div id="txtPeriod" class="layui-form  component " style="margin-left: 10px; padding-top: 5px;">
            </div>
        </div>
    </blockquote>
    <div>

        <table class="layui-table"  style="width:50%">

            <thead>
                <tr>
                    <th width="100">项目</th>
                    <%--<th width="30">方向</th>--%>


                    <th width="100">金额</th>
                </tr>
            </thead>
            <tbody id="dt1"></tbody>
        </table>



    </div>
    <div id="page"></div>
    <script id="tpl-select" type="text/x-jsrender">
        <option data-title="{{:Year}}年{{:Month}}期"  {{if IsActive==1}} selected {{/if}} value="{{:Id}}">{{:Year}}-{{:Month}}</option>
    </script>
    <script id="tpl-Year" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Year}}">{{:Year}}</option>
    </script>
     <script id="tpl-Month" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Id}}" >{{:Month}}</option>
    </script>
    <script id="tpl-list" type="text/x-jsrender">
        {{if XXTax!=null }}
        <tr>
            {{for XXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if UnCal_XXTax!=null }}
        <tr>
            {{for UnCal_XXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
        {{/if}}
        {{if XXTax_TOTAL!=null }}
        <tr>
            {{for XXTax_TOTAL}}
            <td style="text-align: right">{{:Subject}}:</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
        {{/if}}
        {{if JXTax!=null }}
        <tr>
            {{for JXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
        {{/if}}
         {{if UnCal_JXTax!=null }}
        <tr>
            {{for UnCal_JXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
        {{/if}}
        {{if JXTax_TOTAL!=null }}
        <tr>
            {{for JXTax_TOTAL}}
            <td style="text-align: right">{{:Subject}}:</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if Pre_LiuDi!=null }}
        <tr>
            {{for Pre_LiuDi}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if Pre_ZZTax!=null }}
        <tr style="font-weight:bold;">
            {{for Pre_ZZTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if LocalSJ!=null }}
        <tr style="font-weight:bold;">
            {{for LocalSJ}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if Cal_TotalSDTax!=null }}
        <tr>
            {{for Cal_TotalSDTax}}
            {{if Subject=="本月企业所得税" }}
            <td style="font-weight:bold;">{{:Subject}}</td>
            
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right;font-weight:bold;">{{thousand:Money}}   </td>
             {{else}}
            <td >{{:Subject}}</td>

            <td style="text-align: right;">{{thousand:Money}}   </td>
            {{/if}}
            {{/for}}
        </tr>
        {{/if}}
        {{if TotalTax!=null }}
        <tr>
            {{for TotalTax}}
            <td style="text-align: right">{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if Quarter_VaTax!=null }}
        <tr>
            {{for Quarter_VaTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if Quarter_LocalSJ!=null }}
        <tr>
            {{for Quarter_LocalSJ}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
         {{if Quarter_TotalSDTax!=null }}
        <tr>
            {{for Quarter_TotalSDTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if TotalZZTax!=null }}
        <tr>
            {{for TotalZZTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if TotalLocalSJ!=null }}
        <tr>
            {{for TotalLocalSJ}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if Total_Deliver_SDTax!=null }}
        <tr>
            {{for Total_Deliver_SDTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{/if}}
        {{if TotalYearTax!=null }}
        <tr>
            {{for TotalYearTax}}
            <td style="text-align: right">{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if Pre_SDTax!=null }}
          <tr style="font-weight:bold;">
              {{for Pre_SDTax}}
            <td>{{:Subject}}</td>
              <%--<td style="">{{:Credit_Debit}}  </td>--%>
              <td style="text-align: right">{{thousand:Money}}   </td>
              {{/for}}
            
          </tr>
        {{/if}}
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

        <table style="width: 550px;">
            <tr style="height: 30px;">
                <td style="width: 33.3333%"></td>
                <td style="font-size: 20px; text-align: center; font-weight: 400">税金表</td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right"></td>
            </tr>
            <tr style="height: 30px;">
                <td style="font-size: 12px; width: 33.3333%">编制单位：{{:Company}}</td>
                <td style="font-size: 12px; text-align: center;"></td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right"></td>
            </tr>
        </table>
        <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td style="width: 250px; text-align: center">项目</td>
                    <%--<td style="width: 100px; text-align: center">方向</td>--%>
                    <td style="width: 200px; text-align: center">金额</td>

                </tr>
            </thead>
            <tbody>
                {{if Subject!=null }}
        <tr>
            {{for XXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: center">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
                {{/if}}
        {{if UnCal_XXTax!=null }}
        <tr>
            {{for UnCal_XXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
                {{/if}}
        {{if XXTax_TOTAL!=null }}
        <tr>
            {{for XXTax_TOTAL}}
            <td style="text-align: right">{{:Subject}}:</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
                {{/if}}
        {{if JXTax!=null }}
        <tr>
            {{for JXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
                {{/if}}
         {{if UnCal_JXTax!=null }}
        <tr>
            {{for UnCal_JXTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
             
        </tr>
                {{/if}}
        {{if JXTax_TOTAL!=null }}
        <tr>
            {{for JXTax_TOTAL}}
            <td>{{:Subject}}:</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: center">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
                {{/if}}
        {{if Pre_LiuDi!=null }}
        <tr>
            {{for Pre_LiuDi}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
                {{/if}}
        {{if Pre_ZZTax!=null }}
        <tr style="font-weight:bold;">
            {{for Pre_ZZTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
                {{/if}}
        {{if LocalSJ!=null }}
        <tr style="font-weight:bold;">
            {{for LocalSJ}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
                {{/if}}
        {{if Cal_TotalSDTax!=null }}
        <tr>
            {{for Cal_TotalSDTax}}
            {{if Subject=="本月企业所得税" }}
            <td style="font-weight:bold;">{{:Subject}}</td>
            
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right;font-weight:bold;">{{thousand:Money}}   </td>
             {{else}}
            <td >{{:Subject}}</td>

            <td style="text-align: right;">{{thousand:Money}}   </td>
            {{/if}}
            {{/for}}
        </tr>
                {{/if}}
        {{if TotalTax!=null }}
        <tr>
            {{for TotalTax}}
            <td style="text-align: right">{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if TotalZZTax!=null }}
        <tr>
            {{for TotalZZTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: center">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
        {{if TotalLocalSJ!=null }}
        <tr>
            {{for TotalLocalSJ}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: center">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}

        {{if Total_Deliver_SDTax!=null }}
        <tr>
            {{for Total_Deliver_SDTax}}
            <td>{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: center">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
        {{if TotalYearTax!=null }}
        <tr>
            {{for TotalYearTax}}
            <td  style="text-align: right">{{:Subject}}</td>
            <%--<td style="">{{:Credit_Debit}}  </td>--%>
            <td style="text-align: right">{{thousand:Money}}   </td>
            {{/for}}
        </tr>
         {{/if}}
                {{/if}}
        {{if Pre_SDTax!=null }}
          <tr>
              {{for Pre_SDTax}}
            <td>{{:Subject}}</td>
              <%--<td style="">{{:Credit_Debit}}  </td>--%>
              <td style="text-align: center">{{thousand:Money}}   </td>
              {{/for}}
            
          </tr>
                {{/if}}
            </tbody>

        </table>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script src="../../../js/numeral.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="sjReport.js?v=20180919"></script>
</body>
</html>
