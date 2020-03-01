<%@ Page Language="C#" AutoEventWireup="true" CodeFile="accountInfoAdd.aspx.cs" Inherits="view_fas_set_account_accountInfoAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>账套新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link href="../../../../css/grid.css" rel="stylesheet" />

    <style type="text/css">
        .layui-form-item .layui-inline {
            width: 33.333%;
            float: left;
            margin-right: 0;
        }

        @media(max-width:1240px) {
            .layui-form-item .layui-inline {
                width: 100%;
                float: none;
            }
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 100%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="tks-tbcolumn50">
                    <div class="layui-form-item">
                        <label class="layui-form-label">单位名称</label>
                        <div class="layui-input-block">
                            <input type="text" id="iptQYName" class="layui-input "   value="{{:QY_Name}}" name="QY_Name" lay-verify="required" placeholder="">
                        </div>
                    </div>
                     <div class="layui-form-item">
                        <label class="layui-form-label">单位所在地</label>
                        <div class="layui-input-block">
                            <input type="text" class="layui-input"   value="{{:QY_Address}}" name="QY_Address" lay-verify="" placeholder="">
                        </div>
                    </div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">主办会计</label>
                    <div class="layui-input-inline">
                        <input type="text" id="txtAccountantId" class="layui-input   layui-hide " value="{{:AccountantId}}" name="AccountantId">

                        <input type="text" id="txtTrueName" class="layui-input  "   value="{{:TrueName}}" name="TrueName" lay-verify="required" placeholder="请选择会计">
                    </div>
                     <i id="btnUserChoose" class="layui-icon layui-btn">&#xe615;</i> 
                
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">行业</label>
                    <div class="layui-input-block">
                        <select name="QY_Type"  >
                       
                            <option value="0"  {{if QY_Type==0}}selected{{/if}} >一般工商业</option>
                            <option value="1" {{if QY_Type==1}}selected{{/if}}>建筑安装业</option>
                            <option value="2" {{if QY_Type==2}}selected{{/if}}>餐饮业</option>
                              
                        </select>
                    </div>
                </div>
          
               <div class="layui-form-item">
                    <label class="layui-form-label">凭证是否需要审核</label>
                    <div class="layui-input-block">
                        <select name="IsNeedReviewed" >
                       
                            <option value="0"  {{if IsNeedReviewed==0}}selected{{/if}} >否</option>
                            <option value="1" {{if IsNeedReviewed==1}}selected{{/if}}>是</option>
                       
                        </select>
                    </div>
                </div>

                   <div class="layui-form-item">
                    <label class="layui-form-label">账套计费对象</label>
                    <div class="layui-input-block layui-disabled">
                        <select name="BillTarget"  disabled>
                       
                            <option value="DZ"  {{if BillTarget==0}}selected{{/if}} >代帐会计</option>
                            <option value="QY" {{if BillTarget==1}}selected{{/if}}>企业主</option>
                       
                        </select>
                    </div>
                </div>
                <div class="layui-form-item" style="display:none" >
                        <label class="layui-form-label">关联企业</label>
                        <div class="layui-input-inline">
                              <input type="text" class="layui-input layui-disabled"  disabled   value="{{:InvitationQYName}}" name="InvitationQYName" lay-verify="" placeholder="">
                 
                        </div>
                        <a id="btnInvitation" class="layui-btn" style="display:none" >邀请</a>
                    </div>

            </div>
            <div class="tks-tbcolumn50">
                   <div class="layui-form-item">
                        <label class="layui-form-label">账套启用年月</label>
                        <div class="layui-input-block">
                           <input  id="txtStartYearMonth" name="StartYearMonth" lay-verify="required"  value="{{:StartYearMonth}}" class="layui-input"  >
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">统一社会信用代码</label>
                        <div class="layui-input-block">
                            <input type="text" class="layui-input"   value="{{:QY_CreditCode}}" name="QY_CreditCode" lay-verify="required" placeholder="">
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">会计准则</label>
                        <div class="layui-input-block">
                            <select name="AccountantRule"  >
                       
                                <option value="1"  {{if AccountantRule==1}}selected{{/if}} >小企业会计准则</option>
                                <option value="2" {{if AccountantRule==2}}selected{{/if}}>企业会计准则</option>
                       
                            </select>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">增值税种类</label>
                        <div class="layui-input-block">
                            <select name="AddedValueTaxType"  >
                       
                                <option value="1"  {{if AddedValueTaxType==1}}selected{{/if}} >小规模纳税人</option>
                                <option value="2" {{if AddedValueTaxType==2}}selected{{/if}}>一般纳税人</option>
                       
                            </select>
                        </div>
                    </div>
                   <div class="layui-form-item">
                        <label class="layui-form-label">企业所得税</label>
                        <div  style="width:80px;float:left">
                               <input type="text" class="layui-input"   value="{{:TaxRate}}" name="TaxRate" lay-verify="required"  placeholder="">
                           <%-- <select name="TaxRate"  >
                       
                                <option value="25"  {{if TaxRate==25}}selected{{/if}} >25%</option>
                                <option value="15" {{if TaxRate==15}}selected{{/if}}>15%</option>
                                <option value="10" {{if TaxRate==10}}selected{{/if}}>10%</option>
                       
                            </select>--%>
                            
                        </div>
                       <div style="float:left;height:38px;line-height:38px;">%</div>
                        
                    </div>
                 <div class="layui-form-item">
                       <%-- <label class="layui-form-label" >地税(城建、教育、地方)税率</label>--%>
                     <label class="layui-form-label" >地税</label>
                        <div class="layui-input-block">
                            <select name="LandTax"  >
                       
                                <option value="12" {{if LandTax==12}}selected{{/if}}>12%</option>
                                <option value="10" {{if LandTax==10}}selected{{/if}}>10%</option>
                       
                            </select>
                        </div>
                    </div>
                  
            </div>
            <div class="layui-clear"></div>


        

         
            <div class="layui-form-item">
                <label class="layui-form-label">描述</label>
                <div class="layui-input-block">
                    <textarea class="layui-textarea " name="Memo">{{:Memo}}</textarea>
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
    <script id="tpl-send" type="text/x-jsrender">
        <div style="width:80%">
        <div class="layui-form-item">
                <label class="layui-form-label">账套：</label>
                <div class=" layui-form-text">
                   <label style="font-weight: 400; text-align: left;  padding: 9px 15px;  line-height: 20px;display:block"> {{:AccountName}}</label>
                </div>
              </div>
             <div class="layui-form-item">
                <label class="layui-form-label">邀请码：</label>
                <div class=" layui-form-text">
                     <label  style="font-weight: 400; text-align: left;  padding: 9px 15px;  line-height: 20px;display:block">{{:InvitationCode}}</label> 
                </div>
              </div>
              <div class="layui-form-item">
                <label class="layui-form-label">企业尊称</label>
                <div class="layui-input-block">
                  <input id="txtQYName" type="text" name="QYName"   class="layui-input"/>
                </div>
              </div>
           <div class="layui-form-item">
                <label class="layui-form-label">手机号码</label>
                <div class="layui-input-block">
                  <input id="txtQYMobile" type="text" name="QYMobile"     class="layui-input"/>
                </div>
           </div>
          <div class="layui-form-item">
                <div class="layui-input-block">
                    <button id="btnSend" class="layui-btn" >发送</button>
                  
                </div>
            </div>
            </div>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
     <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="accountInfoAdd.js?_=20180807"></script>
</body>
</html>
