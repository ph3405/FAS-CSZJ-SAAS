<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SFDetailEditor.aspx.cs" Inherits="view_fas_fp_SFDetailEditor" %>

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
        input::-webkit-outer-spin-button, input::-webkit-inner-spin-button {
            -webkit-appearance: none !important;
        }
        input[type="number"] {
            -moz-appearance: textfield; /* firefox */
        }
    </style>

</head>
<body class="childrenBody">
     <form id="editForm" class="layui-form" style="width: 80%;">
      <div id="container">
          <div class="layui-form-item">
                <label class="layui-form-label">收付日期</label>
                <div class="layui-input-block">
                      <input type="text" id="SFDate" readonly name="SFDate" class="layui-input laydate-icon "  style="height: 38px" value=""  lay-verify="required" placeholder=""/>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">收付状态</label>
                <div class="layui-input-block">
                      <select id="SFStatus" name="SFStatus" lay-filter="fp" lay-verify="required">
                        <option value="">请选择</option>
                    </select>
                </div>
            </div>
           <div class="layui-form-item">
                <label class="layui-form-label">收付金额</label>
                <div class="layui-input-block">
                       <input type="number" autocomplete="off"   id="SFMoney" name="SFMoney" class="layui-input " value=""  lay-verify="required"/>
                </div>
            </div>
          <div class="layui-form-item">
                <label class="layui-form-label">细项备注</label>
                <div class="layui-input-block">
                        <textarea id="SFRemark" name="SFRemark"  class="layui-textarea"></textarea>
                </div>
            </div>
      </div>
             <div class="layui-form-item">
                <div class="layui-input-block">
                    <a id="btnSave" class="layui-btn"  lay-submit="" lay-filter="save" >保存</a>
                    <a id="btnCancel" class="layui-btn layui-btn-primary">取消</a>
                </div>
            </div>
    </form>
       <script id="tpl-status" type="text/x-jsrender">
            {{for Source}}
                  <option value="{{:Code}}">{{:Name}}</option>
             {{/for}}
        
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="SFDetailEditor.js"></script>
</body>
</html>
