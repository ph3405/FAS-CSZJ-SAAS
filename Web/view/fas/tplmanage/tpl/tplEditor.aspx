<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tplEditor.aspx.cs" Inherits="view_fas_pz_tplEditor" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>凭证编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />

    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link href="css/tplEditor.css" rel="stylesheet" />
    <style type="text/css">
   
    </style>
</head>
<body class="childrenBody" style="text-align: center; height: 100%; background-color: #f2f2f2">
    <form id="editForm" class="layui-form" style="width: 1000px; margin-left: auto; margin-right: auto">
        <div style="background-color: white; padding: 5px; width: 1010px">
            <div class="layui-form-item" style="float: left">
                <a id="btnAdd" class="layui-btn  ">新增</a>
                <a id="btnSave" class="layui-btn  ">保存</a>

            </div>
            <hr />

            <div class="layui-form-item">
                <div class="layui-input-inline">
                    <input id="txtTitle" type="text" class="layui-input " value="" lay-verify="required" placeholder="模板名称" />
                </div>
                <div class="layui-input-inline">
                    <select id="selTPLTarget" lay-verify="">
                         <option value="1">快速凭证</option>
                        <option value="2">发票上传</option>
                        <option value="3">固定资产</option>
                        <option value="4">期末检查</option>
                        <option value="5">期间费用结转</option>
                        <option value="6">利润结转</option>
                         <option value="7">固定资产变动类型</option>
                    </select>
                </div>
                <div class="layui-form-mid layui-word-aux">模板最后一列，默认发生额比例，仅发票上传支持其它类型，请慎重操作</div>
            </div>


            <table class="layui-table">
                <colgroup>
                    <col width="220">
                    <col width="340">
                    <col width="70">
                    <col width="70">
                </colgroup>
                <thead>
                    <tr>
                        <th>摘要</th>
                        <th>科目</th>
                        <th>借方</th>
                        <th>贷方</th>
                        <th>发生额比例</th>
                    </tr>
                </thead>
                <tbody id="editor">
                </tbody>
              <%--  <tfoot>
                    <tr>
                        <td style="border-right: none; overflow: visible; font-weight: bolder">合计:<span id="debitAllUper"></span>
                        </td>
                        <td style="border-left: none"></td>
                        <td>
                            <div class="DisplayMoney-total">

                                <input type="text" id="txtDebitAll" readonly="true" class="DebitMoney-total" value="" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney-total">
                                <input type="text" id="txtCreditAll" readonly="true" class="CreditMoney-total" value="" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                    </tr>
                </tfoot>--%>
            </table>

        </div>
    </form>

    <script id="tpl-row" type="text/x-jsrender">
        <tr data-id='{{:Id}}'>
            <td>

                <textarea class="zyInput">{{:Summary}}</textarea>
            </td>
            <td>
                <div class="kmContainer">
                    <div class="DisplayTextKemu">
                        <div class="s-subject" style="text-align: left"
                            data-isquantity="{{:IsQuantity}}" data-iscurrency="{{:IsCurrency}}" data-iscalhelper="{{:IsCalHelper}}"
                            data-subjectcode="{{:SubjectCode}}" data-quantity="{{:Quantity}}" data-price="{{:Price}}" data-currencycode="{{:CurrencyCode}}"
                            data-rate="{{:Rate}}" data-yb="{{:YB}}" data-cal="{{:CalValue1}}" data-unit="{{:Unit}}">
                            {{:SubjectDescription}}
                        </div>
                        {{if IsQuantity==1 }}
                        <div style="font-size: 12px; text-align: right">
                            数量：<span class="s-Quantity"> {{:NUM}}</span> 单价：<span class="s-Price">{{:Price}}</span>
                        </div>
                        {{/if}}
        {{if IsCurrency==1}}
                        <div style="font-size: 12px; text-align: right">
                            <span class="s-CurrencyCode">{{:CurrencyCode}}</span> 汇率： 
                            <span class="s-Rate">{{:Rate}}</span>  原币:
                              <span class="s-YB">{{:YB}}</span>
                        </div>
                        {{/if}}
                      
                    </div>
                    <textarea class="kmInput" data-code="{{:SubjectCode}}"></textarea>
                </div>
            </td>
            <td>
                <div class="debitFlag">
                    {{if Credit_Debit==0}}
                         <input type="checkbox" checked />
                    {{else}}
                         <input type="checkbox" />
                    {{/if}}
                    
                </div>
            </td>
            <td>
                <div class="creditFlag">
                    {{if Credit_Debit==1}}
                         <input type="checkbox" checked />
                    {{else}}
                        <input type="checkbox" />
                    {{/if}}
                </div>

            </td>
            <td style="overflow:visible">
                
                <div class="layui-input-inline" style="width:120px">
                     <select class="SourceType" >
                    <option value="0" {{if SourceType==0}} selected {{/if}}>发生额比例</option>
                     <option value="1" {{if SourceType==1}} selected {{/if}}>含税金额</option>
                     <option value="2" {{if SourceType==2}} selected {{/if}}>税金</option>
                     <option value="3" {{if SourceType==3}} selected {{/if}}>未税金额</option>
                </select>
                </div>
                  <div class="layui-input-inline" style="width:50px">
                    <input   type="text" class="layui-input IPercent" value="{{:IPercent}}" lay-verify="required"  />
                  </div>
            </td>

        </tr>

    </script>

    <script id="tpl-kmdes" type="text/x-jsrender">

        <div class="s-subject" style="text-align: left"
            data-isquantity="{{:IsQuantity}}" data-iscurrency="{{:IsCurrency}}" data-iscalhelper="{{:IsCalHelper}}"
            data-subjectcode="{{:SubjectCode}}" data-quantity="{{:Quantity}}" data-price="{{:Price}}" data-currencycode="{{:CurrencyCode}}"
            data-rate="{{:Rate}}" data-yb="{{:YB}}" data-cal="{{:CalValue1}}" data-unit="{{:Unit}}">
            {{:SubjectDescription}}
        </div>
        {{if IsQuantity==1 }}
                        <div style="font-size: 12px; text-align: right">
                            数量：<span class="s-Quantity"> {{:Quantity}}</span> 单价：<span class="s-Price">{{:Price}}</span>
                        </div>
        {{/if}}
        {{if IsCurrency==1}}
                        <div style="font-size: 12px; text-align: right">
                            <span class="s-CurrencyCode">{{:CurrencyCode}}</span> 汇率： 
                            <span class="s-Rate">{{:Rate}}</span>  原币:
                              <span class="s-YB">{{:YB}}</span>
                        </div>
        {{/if}}
                      
    </script>


    <script>
        var token = '<%=Token%>';
    </script>
    <script src="../../../../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <link href="../../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../../js/jqueryUI/jquery-ui.min.js"></script>
    <script type="text/javascript" src="tplEditor.js?_=20180923"></script>
</body>
</html>
