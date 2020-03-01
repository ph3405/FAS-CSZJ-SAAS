<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pzEditor.aspx.cs" Inherits="view_fas_pz_pzEditor" %>

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
    <link href="css/pzEditor.css" rel="stylesheet" />
    <link href="../../../icon/iconfont.css" rel="stylesheet" />

    <style type="text/css">
   
    </style>
</head>
<body class="childrenBody" style="text-align: center; height: 100%; background-color: #f2f2f2">
    <form id="editForm" class="layui-form" style="width: 1000px; margin-left: auto; margin-right: auto">
        <div style="background-color: white; padding: 5px; width: 1010px">
            <div class="layui-form-item">
                <div style="float: left">
                    <a id="btnAdd" class="layui-btn ">新增</a>
                    <a id="btnSave" class="layui-btn ">保存</a>
                    <a id="btnPrint" class="layui-btn ">打印</a>
                    <a id="btnTPL" class="layui-btn ">选择模板</a>
                    <a id="addSub" class="layui-btn ">新增科目</a>
                    <a id="calHelper" class="layui-btn ">辅助核算</a>
                </div>
                <div style="float: right; height: 38px; line-height: 38px;">
                    <span id="kj" style="margin-right:20px;">快捷键</span>
                    <a id="btnPrev" class="layui-btn layui-btn-mini"><i class="layui-icon">&#xe603;</i></a>
                    <a id="btnNext" class="layui-btn layui-btn-mini"><i class="layui-icon">&#xe602;</i></a>
                </div>
            </div>
            

            <div class="layui-form-item">
                <div class="layui-input-inline" style="width: 70px;">
                    <select id="PZZ" name="PZZ" lay-verify="">
                             <option value=""></option>
                    </select>
                </div>

                <div class="layui-input-inline" style="width: 70px;">
                    <input id="PZZNO" type="text" class="layui-input " value="1"  name="" lay-verify="required" placeholder=""/>
                </div>
                <div class="layui-form-mid layui-word-aux">号</div>
                <div class="layui-form-mid layui-word-aux" style="margin-left: 100px;">日期：</div>
                <div class="layui-input-inline">
                    <input type="text" id="txtPZDate" class="  laydate-icon" name="" lay-verify="required" style="height: 36px; width: 100px" />

                </div>

                <div style="float: right;">
                    <div class="layui-form-mid layui-word-aux">附单据</div>
                    <div class="layui-input-inline" style="width: 70px;">
                        <input id="txtAppendNo" type="text" class="layui-input " name="" placeholder=""/>
                    </div>
                    <div class="layui-form-mid layui-word-aux" style="margin-right: 50px">张</div>
                     <a id="btnFJ" style="margin-right:30px;" ><i style="font-size:30px;cursor:pointer;" class=" iconfont icon-fujian"></i></a>

                </div>

            </div>

            <div id="print">
            <table id="ptTable" class="layui-table">
                <colgroup>
                    <col width="220">
                    <col width="340">
                    <col width="220">
                    <col width="220">
                </colgroup>
                <thead>
                    <tr>
                        <th>摘要</th>
                        <th>科目</th>
                        <th>
                            <div>
                            借方金额
                            </div>
                            <div class="moneyUint">
                                <span>亿</span> <span>千</span> <span>百</span> <span>十</span> <span>万</span> <span>千</span>
                                <span>百</span> <span>十</span> <span>元</span> <span>角</span> <span class="last">分</span>
                            </div>

                        </th>
                        <th>
                             <div>
                            贷方金额
                            </div>
                            <div class="moneyUint">
                                <span>亿</span> <span>千</span> <span>百</span> <span>十</span> <span>万</span> <span>千</span>
                                <span>百</span> <span>十</span> <span>元</span> <span>角</span> <span class="last">分</span>
                            </div>
                        </th>
                    </tr>
                </thead>
                <tbody id="editor">
                    <%--     <tr>
                        <td>

                            <textarea class="zyInput"></textarea>
                        </td>
                        <td>
                            <div class="kmContainer">


                                <div class="DisplayTextKemu">
                                    <div style="text-align: left">1001 库存现金</div>
                                    <div style="font-size: 12px; text-align: right">数量：100 单价：200</div>
                                    <div style="font-size: 12px; text-align: right">RMB 汇率：1 原币：0</div>
                                    <div style="text-align: left">余额：9090</div>
                                </div>
                                <textarea class="kmInput"></textarea>
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">

                                <input type="text" class=" DebitMoney" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">


                                <input type="text" class="CreditMoney" maxlength="12" lay-verify="number" />
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <textarea class="zyInput"></textarea>
                        </td>
                        <td>
                            <div class="kmContainer">
                                <div class="DisplayTextKemu">
                                    <div style="text-align: left">1001 库存现金</div>
                                    <div style="font-size: 12px; text-align: right">数量：100 单价：200</div>
                                    <div style="font-size: 12px; text-align: right">RMB 汇率：1 原币：0</div>
                                    <div style="text-align: left">余额：9090</div>
                                </div>
                                <textarea class="kmInput"></textarea>
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">

                                <input type="text" class=" DebitMoney" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">


                                <input type="text" class="CreditMoney" maxlength="12" lay-verify="number" />
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <textarea class="zyInput"></textarea>
                        </td>
                        <td>
                            <div class="kmContainer">
                                <div class="DisplayTextKemu">
                                    <div style="text-align: left">1001 库存现金</div>
                                    <div style="font-size: 12px; text-align: right">数量：100 单价：200</div>
                                    <div style="font-size: 12px; text-align: right">RMB 汇率：1 原币：0</div>
                                    <div style="text-align: left">余额：9090</div>
                                </div>
                                <textarea class="kmInput"></textarea>
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">

                                <input type="text" class=" DebitMoney" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">


                                <input type="text" class="CreditMoney" maxlength="12" lay-verify="number" />
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <textarea class="zyInput"></textarea>
                        </td>
                        <td>
                            <div class="kmContainer">
                                <div class="DisplayTextKemu">
                                    <div style="text-align: left">1001 库存现金</div>
                                    <div style="font-size: 12px; text-align: right">数量：100 单价：200</div>
                                    <div style="font-size: 12px; text-align: right">RMB 汇率：1 原币：0</div>
                                    <div style="text-align: left">余额：9090</div>
                                </div>
                                <textarea class="kmInput"></textarea>
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">

                                <input type="text" class=" DebitMoney" maxlength="12" lay-verify="number" />
                            </div>
                        </td>
                        <td>
                            <div class="DisplayMoney">
                                <input type="text" class="CreditMoney" maxlength="12" lay-verify="number" />
                            </div>

                        </td>
                    </tr>
                    --%>
                </tbody>
                <tfoot>
                    <tr>
                        <td style="border-right:none;overflow:visible;font-weight:bolder">合计:<span id="debitAllUper"></span>
                        </td>
                        <td style="border-left:none"></td>
                        <td>
                             <div class="DisplayMoney-total">
                                   <div class="DebitMoney-total" style="display: block;">
                                            <span id="txtDebitAll" style="margin-top:25px" class="DebitMoney-total-jiefang" ></span>
                                </div>
                               </div>
                        </td>
                        <td>
                             <div class="DisplayMoney-total">
                               <div class="CreditMoney-total" style="display: block;">
                                <span id="txtCreditAll" style="margin-top:25px" class="CreditMoney-total-daifang" ></span>
                                </div>
                               
                            </div>
                        </td>
                    </tr>
                </tfoot>
            </table>
            </div>
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
                     <div class="s-subject" style="text-align: left;word-break:break-all;white-space:normal;line-height:20px"
           data-isQuantity="{{:IsQuantity}}" data-isCurrency="{{:IsCurrency}}" data-isCalHelper="{{:IsCalHelper}}"
            data-subjectCode="{{:SubjectCode}}" data-quantity="{{:Quantity}}" data-price="{{:Price}}"  data-currencyCode="{{:CurrencyCode}}"
            data-rate="{{:Rate}}" data-YB="{{:YB}}" data-cal="{{:CalValue1}}" data-unit="{{:Unit}}" >{{:SubjectDescription}}</div>
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
                      <%--  <div style="text-align: left">
                            {{if SubjectCode!=undefined }}
                            余额：
                            {{/if}}
                            {{:Balance}}</div>--%>
                    </div>
                    <textarea class="kmInput" data-code="{{:SubjectCode}}"></textarea>
                  
                </div>
            </td>
            <td>
                <div class="DisplayMoney">

                   <div class="DisplayMoneyVal-d" style="display: block;">
                    <span class="DisplayJieFang" >{{trans2Show:Money_Debit}}</span>
                   </div>
                   <input type="text" class="DebitMoney" data-val="{{noZero:Money_Debit}}"  value="" maxlength="12" lay-verify="number" />
                </div>
                
            </td>
            <td>
                <div class="DisplayMoney">
                   <div class="DisplayMoneyVal-c" style="display: block;">
                    <span class="DisplayDaiFang" >{{trans2Show:Money_Credit}}</span>
                   </div>
                    <input type="text" class="CreditMoney" data-val="{{noZero:Money_Credit}}"  value="" maxlength="12" lay-verify="number" />
                </div>

            </td>
               
        </tr>
    
    </script>

    <script id="tpl-kmdes" type="text/x-jsrender">

        <div class="s-subject" style="text-align: left"
            data-isQuantity="{{:IsQuantity}}" data-isCurrency="{{:IsCurrency}}" data-isCalHelper="{{:IsCalHelper}}"
            data-subjectCode="{{:SubjectCode}}" data-quantity="{{:Quantity}}" data-price="{{:Price}}"  data-currencyCode="{{:CurrencyCode}}"
            data-rate="{{:Rate}}" data-YB="{{:YB}}" data-cal="{{:CalValue1}}" data-unit="{{:Unit}}">{{:SubjectDescription}}</div>
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
                       <%-- <div style="text-align: left">余额：{{:Balance}}</div>--%>
    </script>

    <script id="tpl-pzz" type="text/x-jsrender">
          <option value="{{:Id}}" data-title="{{:ShowTitle}}"   {{if IsDefault==1 }} selected {{/if}}>{{:PZZ}}</option>
    </script>


    <script id="tpl-pzPrint" type="text/x-jsrender">
        
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

        <table style="width:660px;">
            <tr style="height:30px;">
                <td style="width:33.3333%"></td>
                <td style="font-size:20px;text-align:center;font-weight:400">{{:PZName}}</td>
                <td style="font-size:12px;width:33.3333%;text-align:right">附单据数：{{:AttachmentNum}}</td>
            </tr>
            <tr style="height:30px;">
                <td style="font-size:12px;width:33.3333%">核算单位：{{:HSCompany}}</td>
                <td style="font-size:12px;text-align:center;">日期：{{:PZDate}}</td>
                <td style="font-size:12px;width:33.3333%;text-align:right">凭证号：{{:PZZ}}</td>
            </tr>
        </table>
        <table id="tb" style="border: 1.5px solid #000000" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td style="text-align:center;width:160px">摘要</td>
                    <td style="text-align :center;width:240px">科目</td>
                    <td style="text-align:center;width:120px;">借方金额</td>
                    <td style="text-align:center;width:120px;border-right:none">贷方金额</td>
                </tr>
            </thead>
            <tbody>

                {{for Docs}}
                <tr>
                    <td style="text-align:left">{{:Summary}}</td>
                    <td style="text-align :left">{{:SubjectDes}}</td>
                    <td style="text-align:right;padding-right:10px;">{{thousand:Debit}}</td>
                    <td style="text-align:right;padding-right:10px;border-right:none">{{thousand:Credit}}</td>
                </tr>
               {{/for}}

            </tbody>
            <tfoot>
                <tr>
                    <td style="text-align:left;border-bottom:none" colspan="2">合计:{{:Total}}</td>

                    <td style="text-align:right;padding-right:10px;border-bottom:none">{{thousand:DebitAll}}</td>
                    <td style="text-align:right;padding-right:10px;border:none">{{thousand:CreditAll}}</td>
                </tr>
            </tfoot>

        </table>

        <table style="width:660px;font-size:12px;height:40px;">
            <tr>
                <td style="width:20%">主管：</td>
                <td style="width:20%">记账：</td>
                <td style="width:20%">审核：</td>
                <td style="width:20%">出纳：</td>
                <td style="width:20%">制单：{{:TrueName}}</td>
            </tr>
        </table>
    </script>

    <script>
        var token = '<%=Token%>';
        var trueName = '<%=TrueName%>';
        var lastPZDate;
    </script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script src="../../../js/easyui/jquery.min.js"></script>
    <script src="../../../js/numeral.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <link href="../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../js/jqueryUI/jquery-ui.js"></script>
   
    <script type="text/javascript" src="pzEditor.js?v=20190101"></script>
</body>
</html>
