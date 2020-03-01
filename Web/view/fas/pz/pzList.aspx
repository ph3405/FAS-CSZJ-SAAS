<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pzList.aspx.cs" Inherits="view_fas_set_pzList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>凭证管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
    <link rel="stylesheet"  href="../../../css/searchmore.css" />

    <style>
        .layui-table td, .layui-table th {
            font-size: 12px;
        }
    </style>

</head>
<body class="childrenBody ">
    <div class="layui-form">

        <blockquote class="layui-elem-quote ">

            <%--       <div class="layui-inline" style="width:100px">
            <select id="selPeriod" lay-filter="period" >
                <option value="">请选择期间</option>

            </select>
           
        </div>--%>
       <%-- <div class="layui-inline" style="width: 80px;">
            <select id="selYearS" lay-filter="selYearS">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">年</div>
        <div class="layui-inline" style="width: 70px;">
            <select id="selMonthS" name="selMonthS" lay-filter="">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">期</div>
        <div class="layui-inline">-- </div>
        <div class="layui-inline" style="width: 80px;">
            <select id="selYearE" lay-filter="selYearE">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">年</div>
        <div class="layui-inline" style="width: 70px;">
            <select id="selMonthE" name="selMonthE" lay-filter="">
                <option value=""></option>

            </select>
        </div>
        <div class="layui-inline">期</div>--%>
            
<%--            <div class="layui-inline" style="clear: both;">
                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodS" name="periodS">
                        <option value="">请选择期间</option>

                    </select>
                </div>
                <div class="layui-input-inline" >-</div>
                <div class="layui-input-inline" style="width: 100px;">
                    <select id="selPeriodE" name="periodE">
                        <option value="">请选择期间</option>

                    </select>
                </div>
            </div>--%>
                
           <%-- <div class="layui-input-inline" style="width: 70px;">
                <select id="PZZ" name="PZZ" lay-verify="">
                    <option value='' selected="selected">全</option>
                </select>
            
            </div>--%>
            <%--<div class="layui-inline">
                <div class="layui-input-inline">
                    <input type="text" id="txtPZZ_S" value="" placeholder="凭证号起始" class="layui-input search_input" />
                </div>
                <div class="layui-input-inline">
                    <input type="text" id="txtPZZ_E" value="" placeholder="凭证号结束" class="layui-input search_input" />
                </div>

            </div>--%>


            <div class="layui-inline ">
                <a class="layui-btn layui-btn-primary"  id="btnMore">筛选条件<i class="layui-icon">&#xe61a;</i> </a>
                <a class="layui-btn search_btn" >查询</a>
                 
                <a id="btnPrint" class="layui-btn  ">批量打印</a>
            </div>
            <%--更多--%>
            <div class="layui-search-mored layui-anim layui-anim-upbit">
                <div class="layui-search-body">
                    <div class="layui-form-item">
                        <div class="layui-inline">
                            <label class="layui-form-label">开始期间</label>
                            <div class="layui-inline" style="width: 80px;">
                                <select id="selYearS" lay-filter="selYearS">
                                    <option value=""></option>

                                </select>
                            </div>
                            <div class="layui-inline">年</div>
                            <div class="layui-inline" style="width: 70px;">
                                <select id="selMonthS" name="selMonthS" lay-filter="">
                                    <option value=""></option>

                                </select>
                            </div>
                            <div class="layui-inline">期</div>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <div class="layui-inline">
                            <label class="layui-form-label">结束期间</label>
                            <div class="layui-inline" style="width: 80px;">
                                <select id="selYearE" lay-filter="selYearE">
                                    <option value=""></option>

                                </select>
                            </div>
                            <div class="layui-inline">年</div>
                            <div class="layui-inline" style="width: 70px;">
                                <select id="selMonthE" name="selMonthE" lay-filter="">
                                    <option value=""></option>

                                </select>
                            </div>
                            <div class="layui-inline">期</div>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">凭证字</label>
                        <div class="layui-input-inline" style="width: 70px;">
                            <select id="PZZ" name="PZZ" lay-verify="">
                                <option value='' selected="selected">全</option>
                            </select>

                        </div>
                    </div>

                    <div class="layui-form-item">
                        <label class="layui-form-label">凭证号</label>
                        <div class="layui-inline">
                            <div class="layui-input-inline"  style="width: 100px;">
                                <input type="text" id="txtPZZ_S" value="" placeholder="凭证号起始" class="layui-input search_input" />
                            </div>
                            <div class="layui-input-inline"  style="width: 100px;">
                                <input type="text" id="txtPZZ_E" value="" placeholder="凭证号结束" class="layui-input search_input" />
                            </div>

                        </div>
                    </div>

                </div>

         <%--        <div class="layui-search-footer">
                    <button id="btnCancel" class="layui-btn layui-btn-sm layui-btn-primary layui-btn-more">取消</button>
                    <a class="layui-btn search_btn">查询</a>
                    <button id="btnMoreQuery" lay-submit lay-filter="search" class="layui-btn layui-btn-sm layui-btn-more">确定</button>
                </div>   --%>
            </div>
        </blockquote>
    </div>
  
    <div >
        <table class="layui-table">

            <thead>
                <tr>
                    <th style="width: 250px">摘要</th>
                    <th style="width: 250px">科目</th>
                    <th style="width: 200px">借方金额</th>
                    <th style="width: 200px">贷方金额</th>
                </tr>
            </thead>
            <tbody id="dt" ></tbody>
        </table>
    </div>
    
    <div id="page"></div>
    <!--startprint-->
     <div id="divPrint">       
     </div>
    <!--endprint-->
     <script id="tpl-pzz" type="text/x-jsrender">
          <%--<option value="{{:Id}}"   {{if IsDefault==1 }} selected {{/if}}>{{:PZZ}}</option>--%>
         <option value="{{:Id}}">{{:PZZ}}</option>
    </script>

    <script id="tpl-list" type="text/x-jsrender">
        {{for Head}}
         <tr style="background-color:#daf9d9">
            <td colspan="3" style="text-align:left;font-weight: bold; color: gray; border-right: none">
                日期：{{>~YearMonthDay(PZDate)}}
                &nbsp;&nbsp;  凭证字号：{{>PZZName}}-{{:PZZNO}}
                &nbsp;&nbsp; <a style="text-decoration:underline;cursor:pointer" class="tks-fj" data-id="{{>Id}}"> 附件数量：{{:AttachmentCount}} </a>
            </td>
           <td  style="border-left:none">
                {{if CheckStatus==1}}
                   <a class="layui-btn layui-btn-mini tks-audit" data-id="{{>Id}}"> 审核</a>
                {{/if}}

                {{if CheckStatus==2}}
                   <a class="layui-btn layui-btn-mini tks-unAudit" data-id="{{>Id}}"> 取消审核</a>
                {{/if}}

                <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{>Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>
               <a class="layui-btn layui-btn-mini tks-rowCopy" data-id="{{>Id}}"><i class="layui-icon">&#xe642;</i>复制</a>
                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{>Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
        
           </td>
        </tr>
        {{/for}}
        {{for Detail}}
        <tr>
            <td>{{:Summary}}</td>
            <td style="text-align:left">{{:SubjectDescription}}</td>
            <td style="text-align:right">{{trans:Money_Debit}}</td>
            <td style="text-align:right">{{trans:Money_Credit}} </td>
        </tr>
        {{/for}}
        {{for Head}}
        <tr>
            <td>合计</td>
            <td style="text-align:left">{{trans2DX:AMT_DBT}}</td>
            <td style="text-align:right">{{trans:AMT_DBT}}</td>
            <td style="text-align:right">{{trans:AMT_DBT}} </td>
        </tr>
            {{/for}}
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
    <script>
        var token = '<%=Token%>';
    </script>
       <script src="../../../js/numeral.min.js"></script>
    <script src="../../../js/LodopFuncs.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="pzList.js?_=20181027"></script>
</body>
</html>

