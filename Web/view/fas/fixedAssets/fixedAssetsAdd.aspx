<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fixedAssetsAdd.aspx.cs" Inherits="view_fas_fixedAssetsAdd" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>资产新增</title>
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
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">资产编号</label>
                    <div class="layui-input-block">
                        <input type="text" class="layui-input " value="{{:DocNo}}" name="DocNo" lay-verify="required" placeholder="">
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">资产名称</label>
                    <div class="layui-input-block">
                        <input type="text" class="layui-input" value="{{:Name}}" name="Name" lay-verify="required" placeholder="">
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">开始使用日期</label>
                    <div class="layui-input-inline">
                        <input type="text" id="txtStartUseDate" readonly="readonly" class="layui-input laydate-icon " style="height: 38px"
                            value="{{:StartUseDate}}" name="StartUseDate" lay-verify="required">
                    </div>

                </div>
            </div>
            <div class="tks-tbcolumn33">

                <div class="layui-form-item">
                    <label class="layui-form-label">资产类别</label>
                    <div class="layui-input-block">
                        <select name="AssetsClass">

                            <option value="001">001 房屋、建筑物</option>
                            <option value="002">002 机器机械生产设备</option>
                            <option value="003">003 器具、工具、家具</option>
                            <option value="004">004 运输工具</option>
                            <option value="005">005 电子设备</option>
                            <option value="006">006 其他固定资产</option>
                        </select>
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">使用部门</label>
                    <div class="layui-input-block">

                    <%--    <select name="UseDeptId">

                            <option value="001">总经办</option>
                            <option value="002">人力资源部</option>
                            <option value="003">行政部</option>
                            <option value="004">财务部</option>
                            <option value="005">采购部</option>
                            <option value="006">销售部</option>
                        </select>--%>
                          <input name="UseDeptName" value="{{:UseDeptName}}" class="layui-input" />
                    </div>
                </div>

                <div class="layui-form-item">
                    <label class="layui-form-label">增加方式</label>
                    <div class="layui-input-block">
                        <select name="AddType">

                            <option value="购入">购入</option>
                            <option value="在建工程转入">在建工程转入</option>
                            <option value="期初">期初</option>
                            <option value="其他">其他</option>

                        </select>
                    </div>
                </div>
            </div>
            <div class="tks-tbcolumn33">

                <div class="layui-form-item">
                    <label class="layui-form-label">规格型号</label>
                    <div class="layui-input-block">
                        <input name="SpecificationType" value="{{:SpecificationType}}" class="layui-input" />

                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">供应商</label>
                    <div class="layui-input-block">
                        <input name="Supplier" value="{{:Supplier}}" class="layui-input" />
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-clear"></div>
        <%--end   基本信息--%>
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>折旧方式</legend>
        </fieldset>
        <div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">折旧方法</label>
                    <div class="layui-input-block">
                        <select name="DepreciationMethod">

                            <option value="1">平均年限法</option>
                            <option value="2">不折旧</option>
                        </select>
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">累计折旧科目</label>
                    <div class="layui-input-block">
                        <input id="ADSubjectCode" type="text" class="layui-input   layui-hide " value="{{:ADSubjectCode}}" lay-verify="required" name="ADSubjectCode" />

                        <input id="ADSubjectName" type="text" class="layui-input" value="{{:ADSubjectName}}" name="ADSubjectName" lay-verify="required" />
                    </div>
<%--                    <div class="layui-input-block">
                       <input class="layui-hide" name="ADSubjectName" value="累计折旧" />
                        <select name="ADSubjectCode">

                            <option value="1502">累计折旧</option>

                        </select>
                    </div>--%>
                </div>
            </div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">录入当期是否折旧</label>
                    <div class="layui-input-block">
                        <select name="IsStartPeriodDepreciation">
                            <option value="0">否</option>
                            <option value="1">是</option>

                        </select>
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">折旧费用科目</label>
                    <div class="layui-input-block">
                        <input id="txtDCostSubjectCode" type="text" class="layui-input   layui-hide " value="{{:DCostSubjectCode}}" lay-verify="required"  name="DCostSubjectCode" />

                        <input id="txtDCostSubjectName" type="text" class="layui-input   " value="{{:DCostSubjectName}}" name="DCostSubjectName" lay-verify="required" />
                    </div>

                </div>
            </div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">资产清理科目</label>
                    <div class="layui-input-block">
                          <input class="layui-hide" name="AssetImpairmentSubjectName" value="固定资产清理" />
                        <select name="AssetImpairmentSubjectCode">

                            <option value="1606">固定资产清理</option>

                        </select>
                    </div>
                </div>
            </div>

        </div>
        <%--end   折旧方式--%>
        <div class="layui-clear"></div>
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>资产数据</legend>
        </fieldset>

        <div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">资产原值</label>
                    <div class="layui-input-block">
                        <input id="txtInitialAssetValue" name="InitialAssetValue" value="{{:InitialAssetValue}}" lay-verify="required" class="layui-input" />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">预计使用月份</label>
                    <div class="layui-input-block">
                        <input id="txtPreUseMonth" name="PreUseMonth" value="{{:PreUseMonth}}" lay-verify="required" class="layui-input" />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">累计折旧</label>
                    <div class="layui-input-block">
                        <input id="txtAccumulativeDpre" name="AccumulativeDpre" value="{{:AccumulativeDpre}}" lay-verify="required" class="layui-input" />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">每月折旧额</label>
                    <div class="layui-input-block">
                        <input id="txtDprePerMonth" name="DprePerMonth" value="{{:DprePerMonth}}" lay-verify="required" class="layui-input layui-radio-disbaled" disabled />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">备注</label>
                    <div class="layui-input-block">
                        <input name="Memo" value="{{:Memo}}" class="layui-input" />
                    </div>
                </div>
            </div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">残值率%</label>
                    <div class="layui-input-block">
                        <input id="txtScrapValueRate" name="ScrapValueRate" value="{{:ScrapValueRate}}" class="layui-input" />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">已折旧月份</label>
                    <div class="layui-input-block">
                        <input id="txtDpreMonth" name="DpreMonth" value="{{:DpreMonth}}" class="layui-input" />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">本年累计折旧</label>
                    <div class="layui-input-block">
                        <input id="txtAccumulativeDpre_Y" name="AccumulativeDpre_Y" value="{{:AccumulativeDpre_Y}}" class="layui-input" />
                    </div>
                </div>
                  <div class="layui-form-item">
                    <label class="layui-form-label">固定资产科目</label>
                    <div class="layui-input-block">
                        <input id="txtGDCode" type="text" class="layui-input   layui-hide " value="{{:GDCode}}" lay-verify="required"  name="GDCode" />

                        <input id="txtGDName" type="text" class="layui-input   " value="{{:GDName}}" name="GDName" lay-verify="required" />
                    </div>

                </div>
            </div>
            <div class="tks-tbcolumn33">
                <div class="layui-form-item">
                    <label class="layui-form-label">预计残值</label>
                    <div class="layui-input-block">
                        <input id="txtScrapValue" name="ScrapValue" value="{{:ScrapValue}}" class="layui-input layui-disabled" disabled />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">剩余使用月份</label>
                    <div class="layui-input-block">
                        <input id="txtRemainderUseMonth" name="RemainderUseMonth" value="{{:RemainderUseMonth}}" class="layui-input layui-disabled" disabled />
                    </div>
                </div>
                <div class="layui-form-item">
                    <label class="layui-form-label">以前年度累计折旧</label>
                    <div class="layui-input-block">
                        <input id="txtPreviousAccumulativeDpre" name="PreviousAccumulativeDpre" value="{{:PreviousAccumulativeDpre}}" class="layui-input layui-disabled" disabled />
                    </div>
                </div>
                     <div class="layui-form-item">
                    <label class="layui-form-label">进项税额</label>
                    <div class="layui-input-block">
                        <input id="txtInputVAT" name="InputVAT" value="{{:InputVAT}}" class="layui-input"   />
                    </div>
                </div>
            </div>
        </div>


        <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>

    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script src="../../../../js/easyui/jquery.min.js"></script>

    <link href="../../../../js/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <script src="../../../../js/jqueryUI/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="fixedAssetsAdd.js?_=20181207"></script>

</body>
</html>
