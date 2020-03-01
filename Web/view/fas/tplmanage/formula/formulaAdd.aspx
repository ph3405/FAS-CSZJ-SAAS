<%@ Page Language="C#" AutoEventWireup="true" CodeFile="formulaAdd.aspx.cs" Inherits="view_user_formulaAdd" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>公式新增</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
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

        .ui-autocomplete {
            text-align: left;
            height: 200px;
            overflow-y: auto;
            overflow-x: hidden;
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
            <div class="layui-form-item">
                <label class="layui-form-label">科目</label>
                <div class="layui-input-block">
                    <input type="text" id="txtSubjectCode" class="layui-input layui-hide "  value="{{:SubjectCode}}" name="SubjectCode" lay-verify="required" placeholder="" />

                    <input type="text" id="txtSubjectName" class="layui-input " value="{{:SubjectName}}" name="SubjectName" lay-verify="required" placeholder="" />

                </div>

            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">运算符</label>
                <div class="layui-input-inline">
                    <select name="OperatorCharacter" lay-filter="OperatorCharacter">
                        <option value="+" {{if OperatorCharacter=='+' }} selected {{/if}}>+</option>
                        <option value="-" {{if OperatorCharacter=='-' }} selected {{/if}}>-</option>
                    </select>
                </div>
               
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">取数规则</label>
                <div class="layui-input-block">
                      <select name="ValueRule" lay-filter="ValueRule">
                        <option value="0" {{if ValueRule==0 }} selected {{/if}}>发生额</option>
                        <option value="1" {{if ValueRule==1 }} selected {{/if}}>余额</option>
                    </select>
                    

                </div>

            </div>
        
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="">保存</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script src="../../../../js/easyui/jquery.min.js"></script>
   
    <link href="../../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../../js/jqueryUI/jquery-ui.min.js"></script>
    <script type="text/javascript" src="formulaAdd.js?_=20171111"></script>
</body>
</html>
