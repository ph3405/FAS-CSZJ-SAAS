<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="view_personal_UserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
	<link rel="stylesheet" href="../../css/grid.css" media="all" />
</head>
<body>
   	<form id="editForm" class="layui-form">
	
	</form>
    <script id="tpl-edit" type="text/x-jsrender">
        <div class="tks-tbcolumn40" style="margin:20px 0 0 5%;">
        <div class="layui-form-item">
            <label class="layui-form-label">用户名</label>
            <div class="layui-input-block">
                <input type="text" value="{{:UserName}}"  disabled class="layui-input layui-disabled">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">机构名称</label>
            <div class="layui-input-block">
                <input type="text" value="{{:NodeName}}" disabled class="layui-input layui-disabled">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">机构类型</label>
            <div class="layui-input-block">
                <input type="text" value="{{if NodeType==0}} 雇主企业 {{else NodeType==1}} 代帐企业 {{/if}} " disabled class="layui-input layui-disabled">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">真实姓名</label>
            <div class="layui-input-block">
                  <input type="text" class="layui-input   " value="{{:TrueName}}" name="TrueName" lay-verify="required" placeholder="请输入真实姓名">
       
            </div>
        </div>
        <div class="layui-form-item"  >
            <label class="layui-form-label">性别</label>
            <div class="layui-input-block">
                <input type="radio" name="Sex" value="1" title="男" {{if Sex==1}} checked="" {{/if}}>
                <input type="radio" name="Sex" value="0" title="女" {{if Sex==0}} checked="" {{/if}}>
	     		 
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">手机号码</label>
            <div class="layui-input-block">
                <input type="tel" value="{{:Mobile}}" disabled placeholder="请输入手机号码"   class="layui-input layui-disabled">
            </div>
        </div>
		  <div class="layui-form-item">
                    <label class="layui-form-label">会员状态</label>
                    <div class="layui-input-block layui-disabled">
                        <select name="Status"  disabled >
                            {{if Status==0}}
                             <option value="1">启用</option>
                            <option value="0" selected>停用</option>
                            {{else}}
                             <option value="1" selected>启用</option>
                            <option value="0">停用</option>
                            {{/if}}
                           
                        </select>
                    </div>
                </div>
			 
    </div>
	 
    <div class="layui-form-item" style="margin-left: 5%;">
        <div class="layui-input-block">
            <button class="layui-btn" lay-submit="" lay-filter="changeUser">修改</button>
			 
        </div>
    </div>
    </script>
       <script>
           var token = '<%=Token%>';
           var id = '<%=Id%>';
    </script>
	<script type="text/javascript" src="../../layui/layui.js"></script>
	<script type="text/javascript" src="address.js"></script>
	<script type="text/javascript" src="userInfo.js"></script>
</body>
</html>
