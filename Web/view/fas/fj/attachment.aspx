<%@ Page Language="C#" AutoEventWireup="true" CodeFile="attachment.aspx.cs" Inherits="view_fas_fj_attachment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
 <%--   <link href="css/uploadify.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/uploadifive.css" rel="stylesheet" />
    <link href="../../../layui/css/layui.css" rel="stylesheet" />
    <script src="js/jquery.min.js" type="text/javascript"> </script>

   <%-- <script src="js/jquery.uploadify.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.uploadifive.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var getQueryString = function (name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return r[2];
                return null;
            };

            var docId = getQueryString('docId');

            $("#uploadify").uploadifive({
                'formData': { 'docId': docId},
               // 'swf': 'js/uploadify.swf?var=' + (new Date()).getTime(),  //指定flash文件路径
               // 'uploader': 'FileHandler.ashx?var=' + (new Date()).getTime(),//指定处理页面 
                'uploadScript': 'FileHandler.ashx?var=' + (new Date()).getTime(),//指定处理页面 
                'queueID': 'fileQueue',//唯一ID标识与显示的DIVID一致
                //'fileTypeDesc': '只允许上传图片',
                //'fileTypeExts': '*.gif; *.jpg; *.png; *.jpeg',
                'fileType': 'image',
                'auto': false, //如果是true,那么上传的文件不需要点击按钮直接上传
                'multi': true, //是否允许多文件同时上传
                'method ': 'post',//传递方式
                'width': 100,
                'height': 38,
                'buttonClass': 'layui-btn',
                'queueSizeLimit': 10, //限制上传文件数
                'fileSizeLimit': '3000KB',
                'buttonText': '选择附件',
                //'onSelectError': function (file) {
                //    console.log(file);
                //},
                'onSelect' : function(queue) {
                   console.log(queue.queued + ' files were added to the queue.');
                },
                'onUploadComplete': function (file) {
                    console.log(file);

                }

            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <blockquote class="layui-elem-quote ">

            <div class="layui-inline">
                <input type="file" id="uploadify" />
            </div>
            <div class="layui-inline">
                <a id="SaveAndUpload" class="layui-btn" onclick=" javascript: $('#uploadify').uploadifive('upload'); ">上传</a>
            </div>
        </blockquote>

       
        <div id="fileQueue" style="border: solid 0px #000000">
        </div>
    </form>
</body>
</html>
