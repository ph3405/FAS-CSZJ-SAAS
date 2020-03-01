<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fptplEditor.aspx.cs" Inherits="view_user_UserEditor" %>


<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>用户编辑</title>
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
    <form id="Form" class="layui-form" style="width: 80%;">
        <script id="tpl-editor" type="text/x-jsrender">
             <div class="layui-form-item">
            <label class="layui-form-label">模板</label>
            <div class="layui-input-inline">
                <input type="text" id="txtTPLId" class="layui-input  layui-hide" value="{{:TPLID}}" name="TPLID" lay-verify="required" placeholder=""/>
                <input type="text" id="txtTPLName" class="layui-input   layui-disabled" disabled value="{{:Title}}" name="Title" lay-verify="required" placeholder=""/>

            </div>
            <i id="btnTplChoose" class="layui-icon layui-btn">&#xe615;</i> 
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">业务类型</label>
            <div class="layui-input-block">
                <select name="Type">
                    <option value="0" {{if Type==0 }} selected {{/if}}>销售</option>
                    <option value="1" {{if Type==1 }} selected {{/if}}>原料辅料采购</option>
                    <option value="2" {{if Type==2 }} selected {{/if}}>设备采购</option>
                    <option value="3" {{if Type==3 }} selected {{/if}}>房租</option>
                    <option value="4" {{if Type==4 }} selected {{/if}}>物流运输费</option>
                    <option value="5" {{if Type==5 }} selected {{/if}}>维修费</option>
                    <option value="6" {{if Type==6 }} selected {{/if}}>差旅费</option>
                    <option value="7" {{if Type==7 }} selected {{/if}}>油费</option>
                    <option value="8" {{if Type==8 }} selected {{/if}}>交通费</option>
                    <option value="9" {{if Type==9 }} selected {{/if}}>办公用品</option>
                    <option value="10" {{if Type==10 }} selected {{/if}}>业务招待</option>
                    <option value="11" {{if Type==11 }} selected {{/if}}>员工福利</option>
                    <option value="12" {{if Type==12 }} selected {{/if}}>礼品</option>
                    <option value="13" {{if Type==13 }} selected {{/if}}>水电费</option>
                    <option value="14" {{if Type==14 }} selected {{/if}}>电话费</option>
                    <option value="15" {{if Type==15 }} selected {{/if}}>广告费</option>
                    <option value="16" {{if Type==16 }} selected {{/if}}>咨询费</option>
                    <option value="17" {{if Type==17 }} selected {{/if}}>其他</option>
                </select>
                <%--<select name="Type">

                   <option value="0" {{if Type==0 }} selected {{/if}}> 办公劳保费</option>
                     <option value="1" {{if Type==1 }} selected {{/if}}>餐饮费-非客户</option>
                     <option value="2" {{if Type==2 }} selected {{/if}}>餐饮费-客户</option>
                      <option value="3" {{if Type==3 }} selected {{/if}}>电话费</option>
                   <option value="4" {{if Type==4 }} selected {{/if}}>汽车过路费</option>
                   <option value="5" {{if Type==5 }} selected {{/if}}>汽车加油费</option>
                    <option value="6" {{if Type==6 }} selected {{/if}}>汽车维修保养费费</option>
                    <option value="7" {{if Type==7 }} selected {{/if}}>电脑等办公电子产品</option>
                     <option value="8" {{if Type==8 }} selected {{/if}}>礼品费</option>
                                        <option value="9" {{if Type==9 }} selected {{/if}}>银行手续费</option>
                                        <option value="10" {{if Type==10 }} selected {{/if}}>维修及工具</option>
                                        <option value="11" {{if Type==11 }} selected {{/if}}>广告费</option>
                                        <option value="12" {{if Type==12 }} selected {{/if}}>酒店住宿费</option>
                                       <option value="13" {{if Type==13 }} selected {{/if}}>停车费</option>
                                       <option value="14" {{if Type==14 }} selected {{/if}}>生产耗材费用</option>
                                       <option value="15" {{if Type==15 }} selected {{/if}}>生产设备</option>
                                     <option value="16" {{if Type==16 }} selected {{/if}}>建筑及办公室维修费</option>
                                      <option value="17" {{if Type==17 }} selected {{/if}}>水电天然气费</option>
                                      <option value="18" {{if Type==18 }} selected {{/if}}>政府税费</option>
                                       <option value="19" {{if Type==19 }} selected {{/if}}>高铁飞机大巴等交通费</option>
                                      <option value="20" {{if Type==20 }} selected {{/if}}>运输费</option>
                                       <option value="21" {{if Type==21 }} selected {{/if}}>保险费</option>
                                      <option value="22" {{if Type==22 }} selected {{/if}}>包装费等耗材</option>
                                      <option value="23" {{if Type==23 }} selected {{/if}}>咨询费</option>
                                      <option value="24" {{if Type==24 }} selected {{/if}}>材料及产品采购</option>
                                       <option value="25" {{if Type==25 }} selected {{/if}}>销售发票</option>
                                      <option value="26" {{if Type==26 }} selected {{/if}}>其他</option>

                </select>--%>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">是否增票</label>
            <div class="layui-input-block">
                <select name="IsVAT">

                    <option value="0" {{if IsVAT==0 }} selected {{/if}}>否</option>
                    <option value="1" {{if IsVAT==1 }} selected {{/if}}>是</option>

                </select>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">收付状态</label>
            <div class="layui-input-block">
                <select name="RPStatus"  id="RPStatus" lay-filter="RPStatus">

                    <option value="0" {{if RPStatus==0 }} selected {{/if}}>未收付</option>
                    <option value="1" {{if RPStatus==1 }} selected {{/if}}>已收付</option>

                </select>
            </div>
        </div>
         <div class="layui-form-item"  id="ZFType">
            <label class="layui-form-label">支付方式</label>
            <div class="layui-input-block">
               <%-- <select name="PayMode">

                    <option value="0" {{if PayMode==0 }} selected {{/if}}>现金</option>
                    <option value="1" {{if PayMode==1 }} selected {{/if}}>转账</option>
                    <option value="2" {{if PayMode==2 }} selected {{/if}}>银行存款</option>

                </select>--%>
                <select name="PayMode">
            
                    <option value="0" {{if PayMode==0 }} selected {{/if}}>现金</option>
                    <option value="1" {{if PayMode==1 }} selected {{/if}}>银行转账</option>
                     <option value="2" {{if PayMode==2 }} selected {{/if}}>承兑汇票</option>
                </select>
            </div>
        </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <button class="layui-btn" lay-submit="" lay-filter="update">立即提交</button>
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>

        </script>

    </form>
      <script>
          var token = '<%=Token%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fptplEditor.js?v=2"></script>
</body>
</html>
