<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vendorAdd.aspx.cs" Inherits="view_fas_vendorAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>客户新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link href="../../../css/grid.css" rel="stylesheet" />

    <style type="text/css">
        .ui-autocomplete {
            text-align: left;
            height: 200px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .layui-form-item .layui-input-inline {
            width: 33.333%;
            float: left;
            margin-right: 0;
        }

        .layui-form-label {
            width: 120px;
        }

        .layui-input-block {
            margin-left: 150px;
        }

        @media(max-width:1240px) {
            .layui-form-item .layui-input-inline {
                width: 33.333%;
                float: left;
            }
        }
    </style>
</head>
<body class="childrenBody" style="font-size: 80%">
    <form id="editForm" class="layui-form" style="width: 100%;">
    </form>

    <script id="tpl-Edit" type="text/x-jsrender">
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>基本信息</legend>
        </fieldset>
        <div>
            <%--<div class="tks-tbcolumn33">--%>
                <div class="layui-form-item">
                    <label class="layui-form-label" style="width:70px">供应商名称</label>
                    <div class="layui-input-block" style="width:250px;margin-left: 100px;">
                        <input type="text" class="layui-input" value="{{:Name}}" name="Name" lay-verify="required" placeholder="">
                    </div>
                </div>
                
           <%-- </div>--%>
        </div>

        <div class="layui-clear"></div>
        <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>

    </script>
    <script>
        var token = '<%=Token%>';
        var userId='<%=Id%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script src="../../../../js/easyui/jquery.min.js"></script>

    <link href="../../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../../js/jqueryUI/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="vendorAdd.js?_=20181115"></script>

</body>
</html>
