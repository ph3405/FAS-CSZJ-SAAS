<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fpfjList.aspx.cs" Inherits="view_fas_set_fpfjList" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>附件管理</title>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="format-detection" content="telephone=no" />
    <link href="css/animate.css" rel="stylesheet" />
    <link href="css/simple.slide.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../../../layui/css/layui.css" media="all" />


</head>
<body class="childrenBody">
    <blockquote class="layui-elem-quote ">
       
         <div class="layui-inline">
       <%--     <a id="btnAdd" class="layui-btn  layui-btn-normal">添加附件</a>--%>
        </div>
        <div class="layui-inline">
            <a class="layui-btn search_btn">查询</a>
        </div>
             <div class="layui-inline">
            <div class="layui-form-mid layui-word-aux total"></div>
        </div>
    </blockquote>
    <div class="layui-form  ">
        <table class="layui-table">

            <thead>
                <tr>

                    <th style="width: 70px">标题</th>
                 
                 
                    <th style="width: 70px">附件</th>
                    <th style="width: 70px">创建日期</th>
                    <th style="width: 100px">操作</th>
                </tr>
            </thead>
            <tbody id="dt" ></tbody>
        </table>
    </div>
    <div id="page"></div>
   
    <script id="tpl-list" type="text/x-jsrender">
        <tr>

            <td>{{:Title}}</td>
 
            <td> <a i="{{:Path}}" class="product-item Slide One">
                <img src="{{:Path}}"   style="width:200px;height:200px" />
                </a>
                </td>
            <td>{{:~TimeFormatter(CreateDate)}} </td>
            <td>
                
               

                <a class="layui-btn layui-btn-danger layui-btn-mini tks-rowDel" data-id="{{:Id}}"><i class="layui-icon">&#xe640;</i> 删除</a>
            </td>
        </tr>
    </script>
    <script>
        var token = '<%=Token%>';
    </script>
      <script src="js/jquery.min.js"></script>
    <script src="js/simple.slide.min.js"></script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <script type="text/javascript" src="fpfjList.js?_=20180121"></script>
</body>
</html>

