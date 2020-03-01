<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kmAss.aspx.cs" Inherits="view_fas_pz_kmAss" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>科目辅助</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
   <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style>
        .layui-form-item .layui-input-inline {
            width: 70px;
        }
    </style>

</head>
<body class="childrenBody">
     <form id="editForm" class="layui-form" style="width: 80%;">
      <div id="container">

      </div>
             <div class="layui-form-item">
                <div class="layui-input-block">
                    <a id="btnSave" class="layui-btn"  lay-submit="" lay-filter="save" >保存</a>
                    <a id="btnCancel" class="layui-btn layui-btn-primary">取消</a>
                </div>
            </div>
    </form>

    <script id="tpl-quantity" type="text/x-jsrender">
         <div class="layui-form-item">
                 <label class="layui-form-label">数量</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input "   value="{{:Quantity}}" name="Quantity" lay-verify="required" placeholder=""/>
                </div>
                  <label class="layui-form-label">单价</label>
                <div class="layui-input-inline">
                    <input type="text" class="layui-input "   value="{{:Price}}" name="Price" lay-verify="required" placeholder=""/>
                </div>
         </div>
    </script>
    <script id="tpl-currency" type="text/x-jsrender">
          <div class="layui-form-item">
                <label class="layui-form-label">币别</label>
                <div class="layui-input-block">
                    <select name="CurrencyCode"  >
                       
                    {{for currency}}
                        <option value="{{:Code}}"  {{if select==1 }}selected{{/if}} >{{:Code}}</option>
                    {{/for}}
                       
                    </select>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">汇率</label>
                <div class="layui-input-inline">
                     <input type="text" class="layui-input "  readonly="true"  value="{{:Rate}}" name="Rate"  placeholder=""/>
                </div>
                  <label class="layui-form-label">原币</label>
                <div class="layui-input-inline">
                     <input type="text" class="layui-input "     value="{{:YB}}" name="YB"  placeholder=""/>
                </div>
            </div>
    </script>

     <script id="tpl-calHelper" type="text/x-jsrender">
          <div class="layui-form-item">
                <label class="layui-form-label">{{:Item.Value}}</label>
                <div class="layui-input-block">
                    <select name="cal-{{:Item.Code}}"  >
                       
                    {{for Source}}
                        <option value="{{:Code}} {{:Value}}"    >{{:Value}}</option>
                    {{/for}}
                       
                    </select>
                </div>
            </div>
          
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="kmAss.js"></script>
</body>
</html>
