<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bbDetailtplAdd.aspx.cs" Inherits="view_user_bbDetailAdd" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>报表模板新增</title>
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
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 80%;">
        <script id="tpl-Edit" type="text/x-jsrender">
           
            <div class="layui-form-item">
                <label class="layui-form-label">列名</label>
                <div class="layui-input-block">

                    <input type="text" id="txtColumn" class="layui-input " value="{{:ColumnName}}" name="ColumnName" lay-verify="required" placeholder="" />

                </div>

            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">值来源类型</label>
                <div class="layui-input-inline">
                    <select name="SourceType" lay-filter="SourceType">
                        <option value="0" {{if SourceType==0 }} selected {{/if}}>公式</option>
                        <option value="1" {{if SourceType==1 }} selected {{/if}}>求和</option>
                    </select>
                </div>
                <i id="btnFormula" class="layui-icon layui-btn">&#xe60a;</i> 
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">值</label>
                <div class="layui-input-block">

                    <input type="text" class="layui-input " value="{{:SourceValue}}" name="SourceValue"   placeholder="" />

                </div>

            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">分类</label>
                <div class="layui-input-block">
                    <select name="Category">
                        <option value="10" {{if Category==10 }} selected {{/if}}>流动资产</option>
                        <option value="11" {{if Category==11 }} selected {{/if}}>流动负债</option>
                        <option value="12" {{if Category==12 }} selected {{/if}}>非流动资产</option>
                         <option value="13" {{if Category==13 }} selected {{/if}}>非流动负债</option>
                         <option value="14" {{if Category==14 }} selected {{/if}}>负债合计</option>
                         <option value="15" {{if Category==15 }} selected {{/if}}>资产合计</option>
                         <option value="16" {{if Category==16 }} selected {{/if}}>所有者权益（或股东权益）</option>
                         <option value="17" {{if Category==17 }} selected {{/if}}>负债和所有者权益（或股东权益）合计</option>
                           <option value="20" {{if Category==20 }} selected {{/if}}>营业收入</option>
                           <option value="21" {{if Category==21 }} selected {{/if}}>营业利润</option>
                           <option value="22" {{if Category==22 }} selected {{/if}}>利润总额</option>
                           <option value="23" {{if Category==23 }} selected {{/if}}>净利润</option>

                    </select>
                </div>
            </div>
               <div class="layui-form-item">
                <label class="layui-form-label">运算符</label>
                <div class="layui-input-block">
                    <select name="OperatorCharacter">
                        <option value="#" {{if OperatorCharacter=='#' }} selected {{/if}}>不参与运算</option>
                        <option value="+" {{if OperatorCharacter=='+' }} selected {{/if}}>+</option>
                        <option value="-" {{if OperatorCharacter=='-' }} selected {{/if}}>-</option>
                       
                    </select>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">序号</label>
                <div class="layui-input-block">

                    <input type="text"  class="layui-input " value="{{:Seq}}" name="Seq" lay-verify="required" placeholder="" />

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
    <script type="text/javascript" src="bbDetailtplAdd.js"></script>
</body>
</html>
