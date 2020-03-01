<%@ Page Language="C#" AutoEventWireup="true" CodeFile="accountInfoList.aspx.cs" Inherits="view_fas_set_account_accountInfoList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>账套管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />
  

</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote ">
        <div class="layui-inline">
            <div class="layui-input-inline">
                <input type="text" id="txtName" value="" placeholder="请输入关键字" class="layui-input search_input">
            </div>
            <a class="layui-btn search_btn">查询</a>
        </div>
        <div class="layui-inline">
            <a id="btnAdd" class="layui-btn layui-btn-normal ">添加新账套</a>
            <%--<a id="btnAddMyAccount" class="layui-btn layui-btn-normal "><i class="layui-icon">&#xe654;</i>关联账套</a>--%>
        </div>

        <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th width="150">单位名称</th>
                    <th width="100">启用年月</th>
                    <th width="100">增值税种类</th>
                    <th width="100">会计准则</th>
                    <th width="100">主办会计</th>
                    <th width="100">账套状态</th>
                    <th width="300">操作</th> 
                </tr>
            </thead>
            <tbody id="dt" class="users_content"></tbody>
        </table>
    </div>
    <div id="page"></div>

    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:QY_Name}}</td>
            <td>{{:~YearMonth(StartYearMonth)}}</td>
            <td>{{if AddedValueTaxType==1}}小规模纳税人{{else}}一般纳税人{{/if}}</td>
            <td>{{if AccountantRule==1}} 小企业会计准则 {{else}} 企业会计准则 {{/if}} </td>
            <td>{{:TrueName}}</td>
            <td>
                {{if CreateByMe=='0'}}

                        {{if WB_Status==2}}
                        外包邀请
                        {{else WB_Status==4}}
                        接受外包
                        {{/if}}
                {{else CreateByMe=='1'}}
                          {{if WB_Status==1}}
                        自有
                        {{else WB_Status==2}}
                        发起外包
                         {{else WB_Status==3}}
                        外包被拒绝
                        {{else WB_Status==4}}
                        外包被接受
                          {{else WB_Status==5}}
                        撤销外包
                        {{/if}}
                {{/if}}
            </td>
            
            <td style="text-align:left">
                {{if Active!=1}}
             <%--   <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowActive" data-id="{{:Id}}"><i class="layui-icon">&#xe612;</i>激活</a>--%>
                {{/if}}
                {{if Active!=2}}
                  {{if CreateByMe=='0'}}

                        {{if WB_Status==2}}
                           <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowAccept" data-id="{{:Id}}"><i class="layui-icon">&#xe612;</i>接受外包</a>

                        {{else WB_Status==4}}
                             <a class="layui-btn layui-btn-mini tks-rowEdit" data-me="{{:CreateByMe}}" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>
                        {{/if}}
                {{else CreateByMe=='1'}}
                          {{if WB_Status==1||WB_Status==3||WB_Status==5}}
                         <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowOut" data-id="{{:Id}}"><i class="layui-icon">&#xe612;</i>外包</a>
                         <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
      
                        {{else WB_Status==2||WB_Status==4}}
                          <a class="layui-btn layui-btn-mini layui-btn-warm tks-rowUnOut" data-id="{{:Id}}"><i class="layui-icon">&#xe612;</i>撤销外包</a>
 
                   
 
                        {{/if}}
                
                    <a class="layui-btn layui-btn-mini tks-rowEdit" data-id="{{:Id}}"><i class="layui-icon">&#xe642;</i>编辑</a>
               <a class="layui-btn layui-btn-mini tks-rowInvitation" data-id="{{:Id}}"  data-name="{{:QY_Name}}"><i class="layui-icon">&#xe613;</i>邀请</a>
                {{/if}}
                {{if InvitationQYName!=''&&CreateByMe!='2'}}
              <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDelMyAccount" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i>取消关联企业</a>
                {{/if}}
                {{/if}}
                   </td>
            
        </tr>
    </script>
    <script id="tpl-send" type="text/x-jsrender">
        <div style="width:80%">
        <div class="layui-form-item">
                <label class="layui-form-label">账套：</label>
                <div class=" layui-form-text">
                   <label style="font-weight: 400; text-align: left;  padding: 9px 15px;  line-height: 20px;display:block"> {{:AccountName}}</label>
                </div>
              </div>
             <div class="layui-form-item">
                <label class="layui-form-label">邀请码：</label>
                <div class=" layui-form-text">
                     <label  style="font-weight: 400; text-align: left;  padding: 9px 15px;  line-height: 20px;display:block">{{:InvitationCode}}</label> 
                </div>
              </div>
 
           <div class="layui-form-item">
                <label class="layui-form-label">手机号码：</label>
                <div class="layui-input-block">
                  <input id="txtQYMobile" type="text" name="QYMobile"     class="layui-input"/>
                </div>
           </div>
          <div class="layui-form-item">
                <div class="layui-input-block">
                    <button id="btnSend" class="layui-btn" >发送</button>
                  
                </div>
            </div>
            </div>
    </script>
    <script>
        var token = '<%=Token%>';
        var userName = '<%=UserName%>';
    </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="accountInfoList.js"></script>
</body>
</html>

