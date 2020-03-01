<%@ Page Language="C#" AutoEventWireup="true" CodeFile="balAss.aspx.cs" Inherits="view_fas_bal_kmAss" %>

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
    <link href="../../../../css/formSelects-v4.css" rel="stylesheet" />
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

    
   

     <script id="tpl-calHelper" type="text/x-jsrender">
          <div class="layui-form-item">
                <label class="layui-form-label">{{:Item.Value}}</label>
                <div class="layui-input-block">
                    <select name="cal-{{:Item.Code}}" xm-select="selectId"  xm-select-search >
                       
                    {{for Source}}
                        <option value="{{:Code}} {{:Value}}">{{:Value}}</option>
                    {{/for}}
                       
                    </select>
                </div>
            </div>
          
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script type="text/javascript" src="balAss.js?v=20181023"></script>

</body>
</html>
