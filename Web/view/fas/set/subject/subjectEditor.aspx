<%@ Page Language="C#" AutoEventWireup="true" CodeFile="subjectEditor.aspx.cs" Inherits="view_fas_set_account_subjectEditor" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>账套科目</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="../../../../css/grid.css"/>
    <style type="text/css">
        /*.layui-form-item .layui-inline {
            width: 33.333%;
            float: left;
            margin-right: 0;
        }*/
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form "   style="width: 100%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="tks-tbcolumn50">
                    <div class="layui-form-item">
                <label class="layui-form-label">科目编码</label>
                <div class="layui-input-block">
                    <input type="text" class="layui-input  layui-disabled" disabled value="{{:Code}}" name="Code" placeholder="自动生成">
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">科目名称</label>
                <div class="layui-input-block">
                    {{if IsCustom==1&&IsUse==0}}
                      <input type="text" class="layui-input  "   value="{{:Name}}" name="Name" lay-verify="required" placeholder="">
       
                    {{else}}
                      <input type="text" class="layui-input layui-disabled" disabled value="{{:Name}}" name="Name" lay-verify="required" placeholder="">
       
                    {{/if}}
              </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">科目类别</label>
                <div class="layui-input-block">
                    <select class="layui-disabled" name="Category" disabled>
                        <option value="1"  {{if Category==1}}selected{{/if}}>资产</option>
                        <option value="2"  {{if Category==2}}selected{{/if}}>负债</option>
                        <option value="3"  {{if Category==3}}selected{{/if}}>权益</option>
                        <option value="4"  {{if Category==4}}selected{{/if}}>成本</option>
                        <option value="5"  {{if Category==5}}selected{{/if}}>损益</option>
                      
                    </select>
                </div>
            </div>
            </div>
            <div class="tks-tbcolumn50">
                
            <div class="layui-form-item">
                <label class="layui-form-label">余额方向</label>
                <div class="layui-input-block">
                    <select class="layui-disabled" name="Credit_Debit" disabled>
                        <option value="0" {{if Credit_Debit==0}}  selected {{/if}}>借</option>
                        <option value="1" {{if Credit_Debit==1}}  selected {{/if}}>贷</option>

                    </select>
                </div>
            </div>
                <div class="layui-form-item">
                <label class="layui-form-label">科目状态</label>
                <div class="layui-input-block">
                    <select name="IsValid">
                        <option value="1" {{if IsValid==1}} selected {{/if}}>正常</option>
                        <option value="0" {{if IsValid==0}} selected {{/if}}>不可用</option>

                    </select>
                </div>
            </div>
            </div>
            <div class="layui-clear"></div>

        
            <div class="layui-form-item">
                <label class="layui-form-label">数量核算</label>
                <div class="layui-input-inline">

                    <input type="checkbox" name="IsQuantityValid" lay-skin="switch" {{if IsQuantityValid==1}} checked{{/if}}>
                   
                </div>
                 <label class="layui-form-label">计量单位</label>
                 <div class="layui-input-inline">
                   <input type="text" class="layui-input" value="{{:QuantityValue}}" name="QuantityValue" lay-verify=" " placeholder="">
                 </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">外币核算</label>
                <div class="layui-input-inline">
                    <input type="checkbox" name="IsCurrencyValid" lay-skin="switch" {{if IsCurrencyValid==1}} checked{{/if}}>
                   
                </div>
            
               
            </div>
                  <div class="layui-form-item">
                <label class="layui-form-label"></label>
                <div id="currencyContainer" name="Currency" class="layui-input-block">
                  
                </div>
            </div>
            
            <div class="layui-form-item">
                <label class="layui-form-label">辅助核算</label>
                <div class="layui-input-inline">
                    <input type="checkbox" name="IsCalHelperValid" lay-skin="switch" {{if IsCalHelperValid==1}} checked{{/if}}>
                   
                </div>
            
                
            </div>
               <div class="layui-form-item">
                  <label class="layui-form-label"></label>
                  <div id="calItemContainer"  class="layui-input-block">
                    
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

      <script id="tpl-currency" type="text/x-jsrender">
          <input type="checkbox" name="currency-{{:Id}}" title="{{:Name}}" {{if IsBaseCurrency==1}} checked disabled  {{else  IsChecked==1}} checked  {{/if}}  />
    </script>
    <script id="tpl-calItem" type="text/x-jsrender">
          <input type="checkbox" name="cal-{{:Id}}" title="{{:Title}}" {{if IsChecked==1}} checked {{/if}}/>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="subjectEditor.js"></script>
</body>
</html>
