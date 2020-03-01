<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qm1.aspx.cs" Inherits="view_fas_qm_qm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>结转生产成本</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
</head>
<body style="margin-left:50px;">
    <div class="layui-form">
        <fieldset class="layui-elem-field layui-field-title">
            <legend>第二步：结转生产成本</legend>
        </fieldset>

        <div id="container" style="height: 200px">

            <div class="layui-form-item">
                <label class="layui-form-label">结转生产成本</label>
                <div class="layui-input-block">
                    <input type="radio" name="code" value="1" title="5401 主营业务成本" checked>
                    <input type="radio" name="code" value="2" title="1243 库存商品">
                </div>
            </div>
        </div>

        <div class="layui-clear"></div>
        <a id="btnPre" class="layui-btn ">上一步</a>
        <a id="btnNext" class="layui-btn " lay-submit="" lay-filter="next">下一步</a>
    </div>
      <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script src="qm1.js?_=2019"></script>

</body>
</html>
