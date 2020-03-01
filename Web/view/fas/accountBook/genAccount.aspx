<%@ Page Language="C#" AutoEventWireup="true" CodeFile="genAccount.aspx.cs" Inherits="view_genAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>总账</title>
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
<body class="childrenBody ">
    <blockquote class="layui-elem-quote layui-form">

        <%--<div class="layui-inline">
            <select id="selPeriod" lay-filter="period">
                <option value="">请选择期间</option>

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
            <label class="layui-form-label" style="width: 70px;">科目代码</label>
            <div class="layui-input-inline" style="width: 100px;">
                <input type="text" name="codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />
            </div>
            <div class="layui-input-inline">-</div>
            <div class="layui-input-inline" style="width: 100px;">
                <input type="text" name="codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
            </div>
        </div>--%>
        <input id="chkNum" type="checkbox" name="" title="显示数量" lay-skin="primary" lay-filter="" />

        <div class="layui-inline">
            <a class="layui-btn layui-btn-primary" id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>
            <%--<a id="btnSearch" class="layui-btn ">查询</a>--%>
            <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">查询</button>
            <a id="btnPrint" class="layui-btn  ">打印</a>
            <%--<a class="layui-btn layui-btn-primary" id="btnMore">更多筛选<i class="layui-icon">&#xe61a;</i> </a>--%>
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
<%--            <div class="layui-search-footer">
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

    <script id="tpl-head" type="text/x-jsrender">

        <th width="100">科目编码</th>
        <th width="100">科目</th>
        <th width="100">期间</th>
        <th width="100">摘要</th>
        {{if type==1}}
             <th width="100">借方<br />
                 数量</th>
        <th width="100">借方<br />
            单价</th>
        {{/if}}

      
             <th width="100">借方<br />
                 金额</th>

        {{if type==1}}
             <th width="100">贷方<br />
                 数量</th>
        <th width="100">贷方<br />
            单价</th>
        {{/if}}

         
             <th width="100">贷方<br />
                 金额</th>
        <th width="100">余额<br />
            方向</th>

        {{if type==1}}
             <th width="100">余额<br />
                 数量</th>
        <th width="100">余额<br />
            单价</th>
        {{/if}}
 
             <th width="100">余额<br />
                 金额</th>
    </script>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>
            <td style="font-weight: bold; color: #0072ff; border-right: none">
                <a style="cursor: pointer" data-code="{{:Code}}" class="row-link"><cite>{{:Name}}</cite></a>
            </td>
        </tr>
        {{for Data}}
        <tr>
            <td><a><cite>{{:SubjectCode}}</cite> </a></td>
            <td>{{:Name}} </td>
            <td>{{:Period}}</td>
            <td>{{:Summary}}  </td>
            <td>{{if NUM1!=0}} {{:NUM1}} {{/if}} </td>
            <td>{{if Unit_Price1!=0}} {{:Unit_Price1}}  {{/if}}</td>
            <td>{{if Money1!=0}} {{:Money1}} {{/if}} </td>
            <td>{{if NUM2!=0}} {{:NUM2}} {{/if}} </td>
            <td>{{if Unit_Price2!=0}} {{:Unit_Price2}}  {{/if}}</td>
            <td>{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
            <td>{{if Show_Credit_Debit==0}} 借 {{else}} 贷 {{/if}}  </td>
            <td>{{if Show_Quantity!=0 }} {{:Show_Quantity}} {{/if}} </td>
            <td>{{if Show_Unit_Price!=0 }} {{:Show_Unit_Price}} {{/if}} </td>

            <td>{{if Show_Money!=0}} {{:Show_Money}}  {{/if}}</td>
        </tr>
        {{/for}}
    </script>

    <%--不显示数量--%>
    <script id="tpl-list2" type="text/x-jsrender">
        <tr>
            <td style="font-weight: bold; color: #0072ff; border-right: none">
                <a style="cursor: pointer" data-code="{{:Code}}" class="row-link"><cite>{{:Name}}</cite></a>
            </td>
            <td style="border: none"></td>
            <td style="border: none"></td>
            <td style="border: none"></td>
            <td style="border: none"></td>
            <td style="border: none"></td>
            <td style="border-left: none"></td>
        </tr>
        {{for Data}}
        <tr>


            <td>{{:SubjectCode}} </td>
            <td>{{:Name}} </td>
              <td>{{:Period}}</td>
            <td>{{:Summary}}  </td>


            <td>{{if Money1!=0}} {{:Money1}} {{/if}} </td>


            <td>{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
            <td>{{if Show_Credit_Debit==0}} 借 {{else}} 贷 {{/if}}  </td>



            <td>{{if Show_Money!=0}} {{:Show_Money}}  {{/if}}</td>
        </tr>
        {{/for}}
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
                <td style="font-size: 20px; text-align: center; font-weight: 400">{{:Name}} 总账</td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right"></td>
            </tr>
            <tr style="height: 30px;">
                <td style="font-size: 12px; width: 33.3333%">科目：{{:Code}} {{:Name}}</td>
                <td style="font-size: 12px; text-align: center;">{{:Period}}</td>
                <td style="font-size: 12px; width: 33.3333%; text-align: right">单位：{{:Unit}}</td>
            </tr>
        </table>
        <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td style="text-align: center; width: 100px">科目编码</td>
                    <td style="text-align: center; width: 100px">科目名称</td>
                    <td style="text-align: center; width: 100px">期间</td>
                    <td style="text-align: center; width: 120px">摘要</td>
                    <td style="text-align: center; width: 120px">借方金额</td>
                    <td style="text-align: center; width: 120px">贷方金额</td>
                    <td style="text-align: center; width: 50px">方向</td>

                    <td style="text-align: center; width: 120px; border-right: none">余额</td>

                </tr>
            </thead>
            <tbody>
                {{for Data}}
            <tr>
                <td style="text-align: center">{{:SubjectCode}}</td>
                <td style="text-align: center">{{:Name}}</td>
                <td style="text-align: center">{{:Period}}</td>
                <td style="text-align: left">{{:Summary}}</td>
                <td style="text-align: right">{{if Money1!=0}} {{:Money1}} {{/if}}</td>
                <td style="text-align: right">{{if Money2!=0}} {{:Money2}}  {{/if}}</td>
                <td style="text-align: center">{{if Show_Credit_Debit==0}} 借 {{else}} 贷 {{/if}}</td>
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
    <script type="text/javascript" src="genAccount.js?v=20180927"></script>
</body>
</html>
