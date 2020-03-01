<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printList.aspx.cs" Inherits="view_fas_print_printList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>打印工具</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    
   <%-- <link href="css/public.css" rel="stylesheet" />--%>
    <%-- <link href="../../../layui/css/layui.css" rel="stylesheet"  media="all" />--%>
    <%-- <link href="layui.css" rel="stylesheet" media="all" />--%>
</head>
<body class="childrenBody">
    <div class="layui-form">
        <blockquote class="layui-elem-quote ">
            <div class="layui-inline">

                <div class="layui-inline" style="width: 80px;">
                    <select id="selYearS_henxiang" lay-filter="selYearS_henxiang">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">年</div>
                <div class="layui-inline" style="width: 70px;">
                    <select id="selMonthS_henxiang" lay-filter="">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">期</div>
                <div class="layui-inline"> -- </div>
                <div class="layui-inline" style="width: 80px;">
                    <select id="selYearE_henxiang" lay-filter="selYearE_henxiang">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">年</div>
                <div class="layui-inline" style="width: 70px;">
                    <select id="selMonthE_henxiang" lay-filter="">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">期</div>
<%--                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodS" name="periodS">
                        <option value="">请选择期间</option>
                    </select>
                </div>
                -
                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodE" name="periodE">
                        <option value="">请选择期间</option>
                    </select>
                </div>--%>
                
            </div>
            <div class="layui-inline">
                <a id="btnPrint_henxiang" class="layui-btn">打印</a>
            </div>
            <%--<div class="layui-inline">
                <b style="color:red;font-size:12px">不建议使用win10自带的edge浏览器打印</b>
            </div>--%>
            <%--<div class="layui-row" style="width: 350px">
                <div class=" layui-input-inline" style="width: 80%">
                    <input type="text" id="txtAccountId" class="layui-input  layui-hide" value="" name="AccountId" lay-verify="required" placeholder="">
                    <input type="text" id="txtAccountDes" class="layui-input   layui-disabled" disabled value="" name="AccountDes" lay-verify="required" placeholder="">
                </div>
                <i id="SearchAccountList" class="layui-icon layui-btn">&#xe615;</i>
            </div>--%>
        </blockquote>
        <div class="layui-form-item">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 100px;">
                    <input type="checkbox" name="KM" class="ckb_henxiang" title="科目余额" />
                </div>
                <div id="SearchKM" class="layui-inline" style="display: none;">
                    <label class="layui-form-label" style="width: 70px; text-align: left;">科目代码：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="KM_codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />
                    </div>
                    <div class="layui-form-mid">-</div>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="KM_codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
                    </div>
                </div>
            </div>
        </div>

        <blockquote class="layui-elem-quote ">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 300px;display:none">
                    <select  id="selAccount"  lay-filter="currentAccount"  lay-search="">
                         <option value="">请选择</option>
                        
                    </select>
                </div>
                <div class="layui-inline" style="width: 80px;">
                    <select id="selYearS" lay-filter="selYearS">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">年</div>
                <div class="layui-inline" style="width: 70px;">
                    <select id="selMonthS" lay-filter="">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">期</div>
                <div class="layui-inline"> -- </div>
                <div class="layui-inline" style="width: 80px;">
                    <select id="selYearE" lay-filter="selYearE">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">年</div>
                <div class="layui-inline" style="width: 70px;">
                    <select id="selMonthE" lay-filter="">
                        <option value=""></option>

                    </select>
                </div>
                <div class="layui-inline">期</div>
<%--                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodS" name="periodS">
                        <option value="">请选择期间</option>
                    </select>
                </div>
                -
                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodE" name="periodE">
                        <option value="">请选择期间</option>
                    </select>
                </div>--%>
                
            </div>
            <div class="layui-inline">
                <a id="btnPrint_1" class="layui-btn">打印</a>
            </div>
            <%--<div class="layui-inline">
                <b style="color:red;font-size:12px">不建议使用win10自带的edge浏览器打印</b>
            </div>--%>
            <%--<div class="layui-row" style="width: 350px">
                <div class=" layui-input-inline" style="width: 80%">
                    <input type="text" id="txtAccountId" class="layui-input  layui-hide" value="" name="AccountId" lay-verify="required" placeholder="">
                    <input type="text" id="txtAccountDes" class="layui-input   layui-disabled" disabled value="" name="AccountDes" lay-verify="required" placeholder="">
                </div>
                <i id="SearchAccountList" class="layui-icon layui-btn">&#xe615;</i>
            </div>--%>
        </blockquote>

        <div class="layui-form-item">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 100px;">
                    <input type="checkbox" name="PZ" class="ckb_1" title="凭证" />
                </div>
                <div id="SearchPZ" class="layui-inline" style="display: none;">
                    <label class="layui-form-label" style="width: 60px; text-align: left;">凭证字：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <select id="PZZ" name="PZZ">
                            <option value=''  selected>全</option>
                        </select>
                    </div>
                    <label class="layui-form-label" style="width: 60px; text-align: left;">凭证号：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="pzzS" placeholder="" autocomplete="off" class="layui-input" />
                    </div>
                    <div class="layui-form-mid">-</div>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="pzzE" placeholder="" autocomplete="off" class="layui-input" />
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 100px;">
                    <input type="checkbox" name="PZFJ" class="ckb_1" title="凭证附件" />
                </div>
                <div id="SearchPZFJ" class="layui-inline" style="display: none;">
                    
                </div>
            </div>
        </div>
        <%--<div class="layui-form-item">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 100px;">
                    <input type="checkbox" name="KM" class="ckb_1" title="科目余额" />
                </div>
                <div id="SearchKM" class="layui-inline" style="display: none;">
                    <label class="layui-form-label" style="width: 70px; text-align: left;">科目代码：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="KM_codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />
                    </div>
                    <div class="layui-form-mid">-</div>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="KM_codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="layui-form-item">
            <div class="layui-inline">
                <div class="layui-input-inline" style="width: 100px;">
                    <input type="checkbox" name="MX" class="ckb_1" title="明细账" />
                </div>
                <div id="SearchMX" class="layui-inline" style="display: none;">
                    <label class="layui-form-label" style="width: 70px; text-align: left;">科目代码：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />
                    </div>
                    <div class="layui-form-mid">-</div>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-inline" style="width: 100px;">
                <input type="checkbox" name="ZZ" class="ckb_1" title="总账" />
            </div>
            <div id="SearchZZ" class="layui-inline" style="display: none;">
                    <label class="layui-form-label" style="width: 70px; text-align: left;">科目代码：</label>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="ZZ_codeS" placeholder="起始编码" autocomplete="off" class="layui-input" />
                    </div>
                    <div class="layui-form-mid">-</div>
                    <div class="layui-input-inline" style="width: 100px;">
                        <input type="text" name="ZZ_codeE" placeholder="结束编码" autocomplete="off" class="layui-input" />
                    </div>
                </div>
        </div>
        <blockquote class="layui-elem-quote ">
        <div class="layui-inline"  style="width:80px;">
            <select id="selYear" lay-filter="selYear">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">年</div>
         <div class="layui-inline"  style="width:70px;">
            <select id="selMonth" lay-filter="">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">期</div>
            <div class="layui-inline">
                <a id="btnPrint_2" class="layui-btn">打印</a>
            </div>
           <%-- <div class="layui-inline">
                <b style="color:red;font-size:12px">不建议使用win10自带的edge浏览器打印</b>
            </div>--%>

        </blockquote>
        <div class="layui-form-item">
            <div class="layui-input-inline" style="width: 100px;">
                <input type="checkbox" name="ZCFZ" class="ckb_2" title="资产负债表" />
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-inline" style="width: 100px;">
                <input type="checkbox" name="LR" class="ckb_2" title="利润表" />
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-inline" style="width: 100px;">
                <input type="checkbox" name="JY" class="ckb_2" title="经营报表" />
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-inline" style="width: 100px;">
                <input type="checkbox" name="SJ" class="ckb_2" title="税金报表 " />
            </div>
        </div>
        <div class="layui-form-item">
        </div>
    </div>



    <script>
        var token = '<%=Token%>';
    </script>
    <script id="tpl-pzz" type="text/x-jsrender">
          <option value="{{:Id}}"  >{{:PZZ}}</option>
    </script>
    <script id="tpl-select" type="text/x-jsrender">
        <option data-title="{{:Year}}年{{:Month}}期"  {{if IsActive==1}} selected {{/if}} value="{{:Id}}">{{:Year}}-{{:Month}}</option>
    </script>
         <script id="tpl-Year" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Year}}">{{:Year}}</option>
    </script>
     <script id="tpl-Month" type="text/x-jsrender">
        <option   {{if IsActive==1}} selected {{/if}} value="{{:Id}}" >{{:Month}}</option>
    </script>
    <%--<script id="tpl-selectAccount" type="text/x-jsrender" >
        <option value="{{:Id}}"   {{if Active==1}}selected{{/if}}>{{:QY_Name}}</option>
    </script>--%>
    <%--<script type="text/javascript" src="../../../layui/layui.js"></script>--%>
  <script src="../../../js/LodopFuncs.js"></script>
    <script src="../../../layui/layui.js"></script>
   <%-- <script src="layui-mz-min.js" charset="utf-8"></script>--%>
 
    <script type="text/javascript" src="printList.js?v=20180921"></script>
   
    <script type="text/javascript">

</script>
   
</body>
</html>

