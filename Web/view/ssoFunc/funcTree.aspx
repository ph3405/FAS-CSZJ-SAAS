<%@ Page Language="C#" AutoEventWireup="true" CodeFile="funcTree.aspx.cs" Inherits="view_func_Tree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>功能管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link rel="stylesheet" href="../../layui/css/layui.css" media="all" />
    <link href="../../css/grid.css" rel="stylesheet" />

</head>
<body class="childrenBody">
    <div class="tks-tbcolumn40" style="padding: 5px">
        <ul id="funcTree" class="easyui-tree" style="height:500px;overflow:auto"></ul>
    </div>
    <div class="tks-tbcolumn50" style="padding: 5px">
        <form id="editForm" class="layui-form" style="width: 80%;">
        </form>
    </div>
    <script id="tpl-Edit" type="text/x-jsrender">
        
         <div class="layui-form-item">
            <label class="layui-form-label">Id</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input  layui-disabled"  disabled name="Id" value="{{:Id}}" lay-verify="required" placeholder="请输入菜单名称">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">菜单名称</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input   "  name="Name" value="{{:Name}}" lay-verify="required" placeholder="请输入菜单名称">
            </div>
        </div>



        <div class="layui-form-item">
            <label class="layui-form-label">URL</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input   " name="URL" value="{{:URL}}" placeholder="请输入地址">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input    " name="Img" value="{{>Img}}">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">顺序</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input    " name="Seq" value="{{:Seq}}" lay-verify="number"  placeholder="请输入数字">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">描述</label>
            <div class="layui-input-block">
                <textarea class="layui-textarea " name="Memo">{{:Memo}}</textarea>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>

    </script>
     <script id="tpl-Add" type="text/x-jsrender">
        
       
        <div class="layui-form-item">
            <label class="layui-form-label">菜单名称</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input   "  name="Name" value="{{:Name}}" lay-verify="required" placeholder="请输入菜单名称">
            </div>
        </div>



        <div class="layui-form-item">
            <label class="layui-form-label">URL</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input   " name="URL" value="{{:URL}}" placeholder="请输入地址">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input    " name="Img" value="{{:Img}}">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">顺序</label>
            <div class="layui-input-block">
                <input type="text" class="layui-input    " name="Seq" value="{{:Seq}}" lay-verify="number"  placeholder="请输入数字">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">描述</label>
            <div class="layui-input-block">
                <textarea class="layui-textarea " name="Memo">{{:Memo}}</textarea>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-block">
                <button class="layui-btn" lay-submit="" lay-filter="save">保存</button>
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>

    </script>
    <div id="mm" class="easyui-menu" style="width: 120px;">
        <div onclick="append()" data-options="iconCls:'icon-add'">新增</div>

        <div class="menu-sep"></div>
        <div onclick="removeit()" data-options="iconCls:'icon-remove'">删除</div>
        <div class="menu-sep"></div>
        <div onclick="expand()">展开</div>
        <div onclick="collapse()">折叠</div>
    </div>


    <script>
        var token='<%=Token%>';
    </script>

    <script type="text/javascript" src="../../layui/layui.js"></script>
    <script src="../../js/easyui/jquery.min.js"></script>
    <!--easyui tree & datagrid-->
    <link href="../../js/easyui/themes/icon.css" rel="stylesheet" />
    <link href="../../js/easyui/themes/color.css" rel="stylesheet" />

    <link href="../../js/easyui/themes/metroBlue/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/easyui/jquery.easyui.min.js"></script>

    <script type="text/javascript" src="../../js/easyui/easyui-lang-zh_CN.js"></script>
    <!--easyui tree & datagrid-->
    <script type="text/javascript" src="funcTree.js"></script>
</body>
</html>
