<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main_html.aspx.cs" Inherits="main_html" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <title>首页--layui后台管理模板</title>
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="format-detection" content="telephone=no">
    <link rel="stylesheet" href="../layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="//at.alicdn.com/t/font_eolqem241z66flxr.css" media="all" />
    <!--<link rel="stylesheet" href="../css/main.css" media="all" />-->
    <link href="../layui-v2.3.0/layui/css/layui.css" rel="stylesheet" />
    <style>
        .top {
            height: 80px;
            line-height: 80px;
            text-align: center;
        }

        body {
            background-color: #E6E6E6;
        }

        .layui-table td {
            border: 1px solid #FFFFFF;
        }



        .top {
            background: #fff;
            border-radius: 5px;
            border: 1px solid #dadada;
            box-sizing: border-box;
            color: #333;
        }

            .top.first {
                border-bottom: 5px solid #44b449;
                box-sizing: border-box;
            }

                .top.first:hover {
                    background: #44b449;
                    color: #fff;
                    border: 0;
                }

            .top.second {
                border-bottom: 5px solid #FF5722;
                box-sizing: border-box;
            }

                .top.second:hover {
                    background: #FF5722;
                    color: #fff;
                    border: 0;
                }

            .top.third {
                border-bottom: 5px solid #9d78e4;
                box-sizing: border-box;
            }

                .top.third:hover {
                    background: #9d78e4;
                    color: #fff;
                    border: 0;
                }

            .top.fourth {
                border-bottom: 5px solid #01AAED;
                box-sizing: border-box;
            }

                .top.fourth:hover {
                    background: #01AAED;
                    color: #fff;
                    border: 0;
                }



        .add_box {
            display: block;
            width: 60px;
            height: 60px;
            margin: 0 auto;
            padding: 15px;
            border: 1px solid #44b449;
            border-radius: 5px;
        }

            .add_box img {
                width: 100%;
                height: 100%;
                display: block;
            }

        .add_tips {
            font-size: 12px;
            text-align: center;
            margin: 10px auto;
        }
        .add_tips cite {
            font-style: normal;
        }

        .layui-row {
            margin-top: 30px;
        }

        .welcome {
            position: absolute;
            left: 50%;
            transform: translateX(-50%);
            bottom: 0;
            font-size: 12px;
        }

        .data_unit {
            position: absolute;
            right: 0;
            top: 10px;
            color: #999;
            font-size: 12px;
        }

        .data_list {
            margin: 30px 0;
        }

            .data_list li {
                list-style: none;
                display: inline-block;
                width: 19%;
                text-align: center;
                font-size: 12px;
            }
    </style>
</head>
<body class="childrenBody">

    <div class="layui-row layui-col-space20 panel" style="margin-top: 10px;">
        <div class="layui-col-md6">
            <div class="layui-card">

                <div class="layui-card-body">
                    <div  style="display: block; padding: 20px 0 100px 0; width: 100%; height: 280px; background-color: white; position: relative;">
                        <a href="javascript:;" data-url="view/fas/pz/pzEditor.aspx">
                            <div class="add_box">
                                <img src="/images/add_data.png" />
                            </div>
                            <div class="add_tips"><cite>新增凭证</cite></div>
                        </a>
                        <div class="layui-row layui-col-space10 panel">

                            <a href="javascript:;" class="layui-col-md3 " data-url="view/fas/pz/pzList.aspx">
                                <div style="" class="top first">
                                    <span></span>
                                    <cite>查看凭证</cite>
                                </div>
                            </a>

                            <a href="javascript:;" class="layui-col-md3" data-url="view/fas/accountBook/balAccount.aspx">
                                <div style="" class="top second">
                                    <span></span>
                                    <cite>科目余额表</cite>
                                </div>
                            </a>
                            <a href="javascript:;" class="layui-col-md3 " data-url="view/fas/report/zcfzReport.aspx">
                                <div style="" class="top third">
                                    <span></span>
                                    <cite>资产负债表</cite>
                                </div>
                            </a>
                            <a href="javascript:;" class="layui-col-md3 " data-url="view/fas/report/lrReport.aspx">
                                <div style="" class="top fourth">
                                    <span></span>
                                    <cite>利润表</cite>
                                </div>
                            </a>
                        </div>

                        <div class="welcome">欢迎来到这里体验我们的产品！</div>
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-col-md6">
            <div class="layui-card">

                <div class="layui-card-body">
                    <div style="width: 100%; height: 200px; background-color: white; position: relative;">
                        <div class="layui-card-header" id="mainSJ_Period"></div>
                        <span class="data_unit">单位：（元）</span>
                        <ul class="data_list" id="dt2">
                        </ul>
                    </div>
                    <div style="width: 100%; height: 200px; background-color: white; position: relative;">
                        <div class="layui-card-header" id="mainJY_Period"></div>
                        <span class="data_unit">单位：（元）</span>
                        <ul class="data_list" id="dt1">
                        </ul>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../layui/layui.js"></script>
    <script type="text/javascript" src="../js/main.js"></script>
    <script type="text/javascript" src="main_html.js?v=20180806"></script>
    <script id="tpl-list" type="text/x-jsrender">
        {{for Pre_ZZTax}}
        <li>

            <div class="data_number">{{:Money}}</div>
            <div class="data_caption">应交增值税</div>
        </li>
        {{/for}}
         {{for LocalSJ}}
                            <li>

                                <div class="data_number">{{:Money}}</div>


                                <div class="data_caption">应交地方税金</div>
                            </li>
        {{/for}}
        {{for Pre_SDTax}}
                            <li>
                                <div class="data_number">{{:Money}}</div>
                                <div class="data_caption">应交所得税</div>
                            </li>
        {{/for}}


    </script>

    <script id="tpl-list1" type="text/x-jsrender">
        {{for KPMoney}}
        <li>

            <div class="data_number">{{:Money}}</div>
            <div class="data_caption">销售收入</div>
        </li>
        {{/for}}
         {{for BankDepositBal_Main}}
                            <li>

                                <div class="data_number">{{:Money}}</div>


                                <div class="data_caption">银行存款</div>
                            </li>
        {{/for}}
        {{for ARAccountBal_Main}}
                            <li>
                                <div class="data_number">{{:Money}}</div>
                                <div class="data_caption">应收账款</div>
                            </li>
        {{/for}}
        {{for APAccountBal_Main}}
                            <li>
                                <div class="data_number">{{:Money}}</div>
                                <div class="data_caption">应付账款</div>
                            </li>
        {{/for}}

    </script>
</body>

</html>
