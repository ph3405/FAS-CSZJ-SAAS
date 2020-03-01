<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kspz.aspx.cs" Inherits="view_fas_kspz" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>凭证字编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <style type="text/css">
        .itemBox {
            display: inline;
            float: left;
            position: relative;
            margin: 0 16px 22px 0;
            color: #545454;
            font-size: 14px;
            width: 228px;
            height: 150px;
        }

        .itemBox-create {
            background: url(./img/itembox.png) 0 0 no-repeat;
            background-position: 0 -20px;
        }

        .itemBox-check {
            background: url(./img/itembox.png) 0 0 no-repeat;
            background-position: 0 -180px;
        }

        .itemBox .titleIcon {
            display: block;
            float: left;
            height: 20px;
            width: 25px;
        }

        .titleIcon-edit {
            background: url(./img/itembox.png) 0 0 no-repeat;
            background-position-x: -240px;
            background-position-y: -40px;
        }

        .titleIcon-check {
            background: url(./img/itembox.png) 0 0 no-repeat;
            background-position-x: -240px;
            background-position-y: -20px;
        }

        .itemBox .itemBoxTitle {
            margin: 0;
            padding: 8px 10px;
            height: 13%;
            line-height: 20px;
        }

        .itemBox .type {
            display: block;
            float: left;
            width: 120px;
            padding-left: 5px;
            padding-bottom: 5px;
            text-align: left;
            font-size: 14px;
        }

        .itemBox .money {
            margin-top: 20px;
            text-align: center;
            font-size: 14px;
        }

        .hasCurrency {
            font-size: 18px;
            text-decoration: underline;
            cursor: pointer;
            color: #02b5b6;
        }

        .createVoucherBtn {
            position: absolute;
            right: 13px;
            bottom: 13px;
            width: 35.5%;
            height: 18%;
            text-align: center;
            background-color: #0cc;
            cursor: pointer;
            line-height: 27px;
            color: #fff;
            border-radius: 2px;
        }

        .greenBtn {
            position: absolute;
            right: 13px;
            bottom: 13px;
            width: 35.5%;
            height: 18%;
            text-align: center;
            cursor: pointer;
            line-height: 27px;
            color: #fff;
            background-color: #ffc314;
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form">

        <blockquote class="layui-elem-quote ">
        </blockquote>
        <div id="container">
        </div>
    </form>

    <script id="tpl-Edit" type="text/x-jsrender">
        {{if IsGenPZ==1}}
        <div class="itemBox itemBox-check" id="salesCost">
            {{else}}
             <div class="itemBox itemBox-create" id="salesCost">
                 {{/if}}
            <ul class="itemBoxTitle">
                <li>{{if IsGenPZ==0}}
                    <span class="titleIcon titleIcon-edit"></span>
                    {{else}}
                    <span class="titleIcon titleIcon-check"></span>
                    {{/if}}
                    <span class="type">{{:Title}}</span>

                </li>
            </ul>
                 <ul class="money">
                     {{if IsGenPZ==1}}
                <span class="currecy hasCurrency tooltip-f" title="">{{:Money}}</span>
                     {{else}}
                 <input type="text" class="layui-input input-money"
                     style="width: 100px; margin-left: auto; margin-right: auto" name="" placeholder="" />
                     {{/if}}
                 </ul>

                 {{if IsGenPZ==0}}
            <ul data-id="{{:Id}}" class="createVoucherBtn btnGenPZ">生成凭证</ul>
                 {{else}}
            <ul data-id="{{:PZId}}" class="greenBtn btnPZEdit">查看凭证</ul>
                 {{/if}}
             </div>
    </script>

    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>

    <script type="text/javascript" src="kspz.js"></script>
</body>
</html>
