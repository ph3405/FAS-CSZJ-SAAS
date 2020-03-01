<%@ Page Language="C#" AutoEventWireup="true" CodeFile="attachment.aspx.cs" Inherits="view_fas_fj_attachment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--   <link href="css/uploadify.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/uploadifive.css" rel="stylesheet" />
    <link href="../../../../layui/css/layui.css" rel="stylesheet" />

    <script src="js/jquery.min.js" type="text/javascript"> </script>
    <script type="text/javascript" src="../../../../layui/layui.js"></script>
    <%-- <script src="js/jquery.uploadify.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.uploadifive.min.js"></script>


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
            <div class="layui-inline">
<%--                <a id="btnDownload" href="../../../excelTemp/固定资产导入模板.xls" class="layui-btn  layui-btn-primary" onclick="window.open('TPLDownload.aspx')">下载模板</a>--%>
                <a id="btnDownload" href="../../../excelTemp/固定资产导入模板.xls" class="layui-btn  layui-btn-primary">下载模板</a>
            </div>

            <div class="layui-form-item">
                <div class=" layui-form-mid layui-word-aux">
                    模板中栏位顺序不可变更
                </div>
            </div>
        </blockquote>


        <div id="fileQueue" style="border: solid 0px #000000">
        </div>
    </form>

    <script>
        layui.config({
            base: "js/"
        }).use(['form', 'layer', 'jquery', 'laypage', 'jqExt', 'JsRender'], function () {
            var form = layui.form(),
                layer = layui.layer,
                laypage = layui.laypage,
                $ = layui.jquery;
            var index;
            $("#uploadify").uploadifive({

                // 'swf': 'js/uploadify.swf?var=' + (new Date()).getTime(),  //指定flash文件路径
                // 'uploader': 'FileHandler.ashx?var=' + (new Date()).getTime(),//指定处理页面 
                'uploadScript': 'FileHandler.ashx?var=' + (new Date()).getTime(),//指定处理页面 
                'queueID': 'fileQueue',//唯一ID标识与显示的DIVID一致
                //'fileTypeDesc': '只允许上传图片',
                'fileTypeExts': '*.xlsx; *.xls;',
                //'fileType': 'xlsx',
                'auto': false, //如果是true,那么上传的文件不需要点击按钮直接上传
                'multi': true, //是否允许多文件同时上传
                'method ': 'post',//传递方式
                'width': 100,
                'height': 25,
                'queueSizeLimit': 1, //限制上传文件数
                'fileSizeLimit': '3MB',
                'buttonText': '选择附件',
                'onUpload': function (filesToUpload) {
                   
                    if (filesToUpload>0) {
                        index = $.loading('导入中');
                    }
                    
                },

                'onError': function (errorType) {
                    $.warning('异常: ' + errorType);
                },
                'onSelect': function (queue) {
                    //console.log(queue.queued + ' files were added to the queue.');
                },
                'onUploadComplete': function (file, data) {
    
                    if (data != "1") {
                        layer.alert(data, { icon: 2 },
                            function () {
                          
                                var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                                parent.layer.close(index); //再执行关闭   
                                layer.closeAll();
                            }
                        );
                        //$.warning('异常: ' + data);
                    }
                    else {
                        layer.alert("导入成功", function () {
                           window.parent.location.reload();
                        });
            
                    }
                    //layer.close(index);
                    //layer.closeAll('dialog');
                }

            });
        });
    </script>
</body>


</html>
