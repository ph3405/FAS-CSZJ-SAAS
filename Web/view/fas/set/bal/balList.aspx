<%@ Page Language="C#" AutoEventWireup="true" CodeFile="balList.aspx.cs" Inherits="view_fas_set_balList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>期初</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
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

        .layui-table input {
            padding: 1px 2px;
            text-align: right;
        }

        .layui-form-label {
            width: 50px;
        }

        .layui-input-block {
            margin-left: 80px;
        }
    </style>

</head>
<body class="childrenBody layui-form">
    <blockquote class="layui-elem-quote ">
        <div class="layui-inline">
            <div class="layui-input-inline">
                <label class="layui-form-label">类别</label>
                <div class="layui-input-block">
                    <select lay-filter="category">

                        <option value="1" selected>资产</option>
                        <option value="2">负债</option>
                        <option value="3">权益</option>
                        <option value="4">成本</option>
                        <option value="5">损益</option>
                    </select>
                </div>
            </div>

        </div>
          <div class="layui-inline">
            <div class="layui-input-inline">
                <label class="layui-form-label">币别</label>
                <div class="layui-input-block">
                    <select id="selCurrency" lay-filter="currency">
                          <option value=""></option>
                    
                    </select>
                </div>
            </div>

        </div>
        <div class="layui-hide">
        <input id="chkNum" type="checkbox"  name="" title="显示数量" lay-skin="primary" lay-filter=""  />
       </div>
        <div class="layui-inline">
            <a id="btnCal" class="layui-btn layui-btn-normal ">试算平衡</a>

             <a id="btnImport" class="layui-btn layui-btn-normal ">导入</a>

        </div>


        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>

    <table class="layui-table">

        <thead>
            <tr id="tbHead">

             

            </tr>
        </thead>
        <tbody id="dt"></tbody>
    </table>

    <div id="page"></div>

    <div id="memo">
       
    </div>

    <script id="tpl-memo" type="text/x-jsrender">
         <table  class="layui-table">
            <tr>
                <td>
                    期初余额合计[{{:CreditDebit}}]
                </td>
                <td>{{:startBWB}}</td>
                <td>借方累计合计</td>
                <td>{{:deljBWB}}</td>
                  <td>贷方累计合计</td>
                <td>{{:crljBWB}}</td>
            </tr>
        </table>
    </script>

    <script id="tpl-head"  type="text/x-jsrender">
        <th width="80">科目编码</th>
             <th width="80">科目名称</th>
             <th width="30">方向</th>
             {{if type==1||type==4}}
             <th width="50">期初余额<br />
                 数量</th>
             {{/if}}
             {{if type==1||type==2}}
             <th width="50">期初余额<br />
                 原币</th>
             {{/if}}
             <th width="50">期初余额<br />
                 本位币</th>
             {{if type==1||type==4}}
             <th width="50">借方累计<br />
                 数量</th>
             {{/if}}
             {{if type==1||type==2}}
             <th width="50">借方累计<br />
                 原币</th>
             {{/if}}
             <th width="50">借方累计<br />
                 本位币</th>
             {{if type==1||type==4}}
             <th width="50">贷方累计<br />
                 数量</th>
             {{/if}}
             {{if type==1||type==2}}
             <th width="50">贷方累计<br />
                 原币</th>
             {{/if}}

             <th width="50">贷方累计<br />
                 本位币</th>
              {{if type==1||type==4}}
             <th width="50">年初余额<br />
                 数量</th>
             {{/if}}
             {{if type==1||type==2}}
             <th width="50">年初余额<br />
                 原币</th>
            {{/if}}
             <th width="50">年初余额<br />
                 本位币</th>
    </script>

    <%--1.数量+原币+本位币--%>
    <script id="tpl-list" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
            {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}
                {{if IsLeaf==1&&IsCalHelperValid==1}}
                    <a data-id="{{:Id}}" data-subjectcode="{{:SubjectCode}}" class="row-add" style="cursor:pointer"> <i class="layui-icon">&#xe654;</i></a>
                {{else Type==2}}
                      <a   data-id="{{:Id}}" class="row-del" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" style="cursor:pointer"> <i class="layui-icon">&#x1006;</i></a>
                {{/if}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMStartBAL}}" />
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}

                <input data-id="{{:Id}}" data-type="YBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBStartBAL}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBStartBAL}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMDebitTotal_Y}}" />
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                <input data-id="{{:Id}}" data-type="YBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBDebitTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBDebitTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMCreditTotal_Y}}" />
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                <input data-id="{{:Id}}" data-type="YBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBCreditTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBCreditTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                {{noZero:YearStartNumBAL}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
               {{noZero:YearStartYBBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
               {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

    <%--2.原币+本位币--%>
    <script id="tpl-list2" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}
                {{if IsLeaf==1&&IsCalHelperValid==1}}
                    <a data-id="{{:Id}}" data-subjectcode="{{:SubjectCode}}" class="row-add" style="cursor:pointer"> <i class="layui-icon">&#xe654;</i></a>
                {{else Type==2}}
                      <a   data-id="{{:Id}}" class="row-del" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" style="cursor:pointer"> <i class="layui-icon">&#x1006;</i></a>
                {{/if}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
       

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}

                <input data-id="{{:Id}}" data-type="YBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBStartBAL}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBStartBAL}}" />
                {{/if}}
            </td>
         

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                <input data-id="{{:Id}}" data-type="YBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBDebitTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBDebitTotal_Y}}" />
                {{/if}}
            </td>
         

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                <input data-id="{{:Id}}" data-type="YBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:YBCreditTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBCreditTotal_Y}}" />
                {{/if}}
            </td>
       

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
               {{noZero:YearStartYBBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
               {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

       <%--3.本位币--%>
    <script id="tpl-list3" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}
                <input style="display:none" data-code="{{:SubjectCode}}" value="{{:CalItem1}}-{{:CalValue1}}" />
            </td>
            <td style="text-align: left">{{:Name}}
                {{if IsLeaf==1&&IsCalHelperValid==1}}
                    <a data-id="{{:Id}}" data-subjectcode="{{:SubjectCode}}" class="row-add" style="cursor:pointer"> <i class="layui-icon">&#xe654;</i></a>
                {{else Type==2}}
                      <a   data-id="{{:Id}}" class="row-del" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" style="cursor:pointer"> <i class="layui-icon">&#x1006;</i></a>
                {{/if}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
       

       
            <td>
               <%-- {{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBStartBAL}}" />
                {{/if}}--%>
                {{if IsLeaf==1&&IsCalHelperValid==0}}
               
                 <input data-id="{{:Id}}" data-type="BWBStartBAL"  data-Subject="{{:SubjectCode}}" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBStartBAL}}" />
                {{else}}
                {{noZero:BWBStartBAL}}
                {{/if}}
            </td>
         

     
            <td>
                <%--{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBDebitTotal_Y}}" />
                {{/if}}--%>
                {{if IsLeaf==1&&IsCalHelperValid==0}}
                 <input data-id="{{:Id}}" data-type="BWBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBDebitTotal_Y}}" />
                {{else}}
                 {{noZero:BWBDebitTotal_Y}}
                {{/if}}
            </td>
         

        
            <td>
               <%-- {{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBCreditTotal_Y}}" />
                {{/if}}--%>
                {{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBCreditTotal_Y}}" />
                {{else}}
                 {{noZero:BWBCreditTotal_Y}}
                {{/if}}
            </td>
       

          
            <td><%--{{if IsLeaf==1&&IsCalHelperValid==0}}
               {{noZero:YearStartBWBBAL}} 
                {{/if}}--%>
                {{noZero:YearStartBWBBAL}} 
            </td>
        </tr>
    </script>

      <%--4.数量+本位币--%>
    <script id="tpl-list4" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}
                {{if IsLeaf==1&&IsCalHelperValid==1}}
                    <a data-id="{{:Id}}" data-subjectcode="{{:SubjectCode}}" class="row-add" style="cursor:pointer"> <i class="layui-icon">&#xe654;</i></a>
                {{else Type==2}}
                      <a   data-id="{{:Id}}" class="row-del" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" style="cursor:pointer"> <i class="layui-icon">&#x1006;</i></a>
                {{/if}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMStartBAL}}" />
                {{/if}}
            </td>

        
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBStartBAL" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBStartBAL}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMDebitTotal_Y}}" />
                {{/if}}
            </td>

         
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBDebitTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBDebitTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                <input data-id="{{:Id}}" data-type="NUMCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:NUMCreditTotal_Y}}" />
                {{/if}}
            </td>

         
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                <input data-id="{{:Id}}" data-type="BWBCreditTotal_Y" class="layui-input row-input" data-subjectname="{{:Name}}" data-subjectcode="{{:SubjectCode}}" value="{{noZero:BWBCreditTotal_Y}}" />
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                {{noZero:YearStartNumBAL}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
               {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

      <script id="tpl-currency" type="text/x-jsrender" >
        <option value="{{:Code}}"   {{if IsBaseCurrency==1}}selected{{/if}}>{{:Name}} - 汇率：{{:ExchangeRate}}</option>
    </script>
     <%--1.数量+原币+本位币--%>
     <script id="tpl-readonlylist" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}
               

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMStartBAL}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}

                    {{noZero:YBStartBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBStartBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMDebitTotal_Y}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YBDebitTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBDebitTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMCreditTotal_Y}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YBCreditTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBCreditTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:YearStartNumBAL}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YearStartYBBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

      <%--2.原币+本位币--%>
     <script id="tpl-readonlylist2" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
          
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}

                    {{noZero:YBStartBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBStartBAL}} 
                {{/if}}
            </td>
         
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YBDebitTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBDebitTotal_Y}} 
                {{/if}}
            </td>
          

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YBCreditTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBCreditTotal_Y}} 
                {{/if}}
            </td>
         

            <td>{{if IsLeaf==1&&IsCalHelperValid==0&& IsDefaultCurrency==0}}
                    {{noZero:YearStartYBBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

    <%--3.本位币--%>
      <script id="tpl-readonlylist3" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">
                {{if SLevel==2}}
                &nbsp;&nbsp;
                {{else SLevel==3}}
                  &nbsp;&nbsp;&nbsp;&nbsp;
                    {{else SLevel==4}}
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                {{/if}}
                {{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
          
       
            <td>
              <%--  {{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBStartBAL}} 
                {{/if}}--%>
                {{noZero:BWBStartBAL}} 
            </td>
         
        
            <td>
                <%--{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBDebitTotal_Y}} 
                {{/if}}--%>
                {{noZero:BWBDebitTotal_Y}} 
            </td>
          
 
            <td>
               <%-- {{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBCreditTotal_Y}} 
                {{/if}}--%>
                 {{noZero:BWBCreditTotal_Y}} 
            </td>
         

            <td>
                <%--{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:YearStartBWBBAL}} 
                {{/if}}--%>
                 {{noZero:YearStartBWBBAL}} 
            </td>
        </tr>
    </script>
          <%--4.数量+本位币--%>
      <script id="tpl-readonlylist4" type="text/x-jsrender">
        <tr  data-table="{{:Type}}">

            <td style="text-align: left">{{:SubjectCode}}</td>
            <td style="text-align: left">{{:Name}}
               

            </td>
            <td>{{if SCredit_Debit==0}}借{{else}}贷{{/if}}</td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMStartBAL}} 
                {{/if}}
            </td>

      
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBStartBAL}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMDebitTotal_Y}} 
                {{/if}}
            </td>

           
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBDebitTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:NUMCreditTotal_Y}} 
                {{/if}}
            </td>

        
            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:BWBCreditTotal_Y}} 
                {{/if}}
            </td>
            <td>{{if IsLeaf==1&&IsCalHelperValid==0&&IsQuantityValid==1}}
                    {{noZero:YearStartNumBAL}} 
                {{/if}}
            </td>

            <td>{{if IsLeaf==1&&IsCalHelperValid==0}}
                    {{noZero:YearStartBWBBAL}} 
                {{/if}}
            </td>
        </tr>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="balList.js"></script>
</body>
</html>

