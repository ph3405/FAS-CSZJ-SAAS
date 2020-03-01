<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpEditor.aspx.cs" Inherits="view_fas_set_fpEditor" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>发票编辑</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link href="../fpfj/css/animate.css" rel="stylesheet" />
    <link href="../fpfj/css/simple.slide.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../../css/grid.css" />
    <style type="text/css">
        input::-webkit-outer-spin-button, input::-webkit-inner-spin-button {
            -webkit-appearance: none !important;
        }
        input[type="number"] {
            -moz-appearance: textfield; /* firefox */
        }
    </style>
</head>
<body class="childrenBody">
    <form id="editForm" class="layui-form" style="width: 100%;">
    </form>
<script id="tpl-Edit" type="text/x-jsrender">

        <blockquote class="layui-elem-quote ">
            <div class="layui-inline">
                <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                <%--<button type="reset" class="layui-btn layui-btn-primary">重置</button>--%>
                <button id="btnBack" class="layui-btn layui-btn-warm">返回</button>
            </div>

        </blockquote>

        <div class="tks-tbcolumn33">
            <div class="layui-form-item">
                <label class="layui-form-label">发票日期</label>
                <div class="layui-input-block">
                    <input type="text" id="InvoiceDate" class="layui-input laydate-icon "  style="height: 38px" value="{{:InvoiceDate}}" name="InvoiceDate" lay-verify="required" placeholder="">
                </div>
            </div>


        </div>
        <div class="tks-tbcolumn33">
            <div class="layui-form-item">
                <label class="layui-form-label">发票号码</label>
                <div class="layui-input-block">
                    <input type="text" class="layui-input " value="{{:InvoiceNo}}" name="InvoiceNo" lay-verify="required" placeholder="">
                </div>
            </div>

        </div>
         
        <div class="layui-clear"></div>
        <div class="layui-form-item">
            <label class="layui-form-label">备注</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input " value="{{:Memo}}" name="Memo" placeholder="">
            </div>
        </div>
        <div class="layui-form-item">
                <label class="layui-form-label" style="width:155px;">启用税金预知</label>
                <div class="layui-input-inline">
                   
                    <input type="checkbox"  name="IsTaxYZ" lay-text="ON|OFF" lay-skin="switch" {{if IsTaxYZ==1}} checked {{/if}} lay-filter="yz"  />
                   
                </div>        
         </div>
        <div id="divYZ">
        <div class="tks-tbcolumn33">
            <div class="layui-form-item">
                <label class="layui-form-label">业务类型</label>
                <div class="layui-input-block">
                    <select id="selFPType" name="Type" lay-filter="fp" lay-verify="required">
                        <option value="">请选择</option>
                    </select>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">支付方式</label>
                <div class="layui-input-block">
                    <select id="selPay" name="PayMode" lay-filter="pm" lay-verify="required">
                        <option value="">请选择</option>
                    </select>
                </div>
            </div>
        </div>

        <div class="tks-tbcolumn33">
            <div class="layui-form-item">
                <label class="layui-form-label">增值税专用发票</label>
                <div class="layui-input-block">
                    <select id="selVAT" name="IsVAT" lay-filter="vat" lay-verify="required">
                        <option value="">请选择</option>
                    </select>
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">含税金额</label>
                <div class="layui-input-block">
                    <input id="Money" type="number" autocomplete="off" class="layui-input " value="{{:Money}}" name="Money" placeholder="">
                </div>
            </div>
          
        </div>
        <div class="tks-tbcolumn33">
            
            <div class="layui-form-item">
                <label class="layui-form-label">收付状态</label>
                <div class="layui-input-block">
                    <select id="selRP" name="RPStatus" lay-filter="rp" lay-verify="required">
                        <option value="">请选择</option>
                    </select>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">税金金额</label>
                <div class="layui-input-block">
                    <input id="txtTAX" type="number" autocomplete="off" class="layui-input "  value="{{:TaxMoney}}" name="TaxMoney" lay-verify="required" placeholder="">
                </div>
            </div>

           
        
        </div>
        </div>
        
        <div class="layui-clear"></div>
         <div class="layui-form-item">
                <label class="layui-form-label" style="width:155px;">启用应收应付与到期提醒</label>
                <div class="layui-input-inline">
                   
                    <input type="checkbox"  name="IsUse" lay-text="ON|OFF" lay-skin="switch" {{if IsUse==1}} checked {{/if}} lay-filter="use"  />
                   
                </div>        
         </div>
        <div id="divUSE">
        <div class="tks-tbcolumn33">
            <div class="layui-form-item">
                <label class="layui-form-label">含税金额</label>
                <div class="layui-input-block">
                    <input id="SF_Money" type="number" autocomplete="off" class="layui-input "  value="{{:Money}}" name="SF_Money" placeholder="">
                </div>
            </div>
        </div>
        <div class="tks-tbcolumn33">
             <div class="layui-form-item">
                <label class="layui-form-label">坏账金额</label>
                <div class="layui-input-block">
                    <input id="BadMoney" type="text" readonly class="layui-input " value="{{:BadMoney}}" name="BadMoney" lay-verify="required" placeholder="">
                </div>
            </div>

        </div>
        <div class="tks-tbcolumn33">
             <div class="layui-form-item">
                <label class="layui-form-label">收付类型</label>
                <div class="layui-input-block">
                    <select id="selSFType" name="SFType" lay-filter="SFType" lay-verify="required">
                        <option value="">请选择</option>
                        <option value="应收">应收</option>
                        <option value="应付">应付</option>
                    </select>
                </div>
            </div>

        </div>
        <div class="tks-tbcolumn40" >
            <div class="layui-form-item">
                    <label class="layui-form-label">客户/供应商</label>
                    <div class="layui-input-inline">
                        <input type="text" id="txtBasicDataId" class="layui-input   layui-hide " value="{{:BasicDataId}}" name="BasicDataId">

                        <input type="text" id="txtBasicDataName" class="layui-input  "  readonly  value="{{:BasicDataName}}" name="BasicDataName" lay-verify="required" placeholder="请选择客户/供应商">
                    </div>
                     <i id="btnBasicSearch" class="layui-icon layui-btn">&#xe615;</i> 
                
                </div>

        </div>
            <div class="tks-tbcolumn20">
                <i id="btnAddDetail" class="layui-icon layui-btn">添加收付明细</i>
            </div>
            <div class="tks-tbcolumn33">
                <span style="height:38px;line-height:38px;color:red;">请务必点击左上角【保存】,然后关闭</span>
            </div>
        <div style="background-color: white; width: 900px">
            <table class="layui-table">
                <colgroup>
                    <col width="60">
                    <col width="60">
                    <col width="60">
                    <col width="150">
                    <col width="50">
                </colgroup>
                <thead>
                    <tr>
                        <th>收付日期</th>
                        <th>细项收付状态</th>
                        <th>收付金额</th>
                        <th>细项备注</th>
                         <th>操作</th>
                    </tr>
                </thead>
                <tbody  id="SFDetail">

                </tbody>
            </table>
        </div>
        </div>

        <div class="layui-form-item">
            <label class="layui-form-label"></label>
            <div class="layui-inline">
                <a id="btnAdd" class="layui-btn  layui-btn-normal">添加附件</a>
            </div>
            <%--<div class="layui-inline">
                <a class="layui-btn search_btn">查询</a>
            </div>--%>
            <div class="layui-inline">
                <div class="layui-form-mid layui-word-aux total"></div>
            </div>
        </div>


        <div id="dt" style="height: 250px;">
        </div>



    </script>
    <script id="tpl-img" type="text/x-jsrender">

        <div class="tks-tbcolumn20" style="text-align: center; width: 160px">
            <a i="{{:Path}}" class="product-item Slide One">
                <img src="{{:Path}}" style="width: 150px; height: 150px" />
            </a>

            <a style="margin-top: 5px;" class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>

        </div>

    </script>
    <script>
        var token = '<%=Token%>';
    </script>
    <script id="tpl-opt" type="text/x-jsrender">
        <option value="{{:Code}}">{{:Name}}</option>
    </script>
    <script src="../fpfj/js/jquery.min.js"></script>
    <script src="../fpfj/js/simple.slide.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="../../../../layui/laydate/laydate.js"></script>
    <script type="text/javascript" src="fpEditor.js?v=21"></script>
</body>
</html>
