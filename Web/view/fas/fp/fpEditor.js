
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('id');
    var SFDetail = [];//收付明细
    var DataType = "";//收付类型
    var _data;
    $('#divYZ').hide();
    $('#divUSE').hide();
    
    
    var init = function (id) {
        var request = {};
        request.Data = {
            Id: id
        };


        $.Post("/fas/fp/InvoiceGet", request,
          function (data) {
              var res = data;
              
              if (res.IsSuccess) {
                  var template = $.templates("#tpl-Edit");
                  debugger;
                  var dataHtml = template.render(res.Data);
                  _data = res.Data;
                  $('#editForm').html(dataHtml);
                  $('#selSFType').val(res.Data.SFType);
                  DataType = res.Data.DataType;
                  InitAllControl();
                  initSelect();
                  var lstSFDetail = res.lstSFDetail;
                  var newSFDetail = [];
                  for (var i = 0; i < lstSFDetail.length; i++) {
                      var d = lstSFDetail[i];
                      var SFStatusCode = "";
                      if (d.SFStatus =="已收") {
                          SFStatusCode = "ys";
                      }
                      if (d.SFStatus == "未收") {
                          SFStatusCode = "ws";
                      }
                      if (d.SFStatus == "已付") {
                          SFStatusCode = "yf";
                      }
                      if (d.SFStatus == "未付") {
                          SFStatusCode = "wf";
                      }
                      newSFDetail.push({
                          "seq": i + 1,
                          "SFDate": d.SFDate,
                          "SFMoney": d.SFMoney,
                          "SFRemark": d.SFRemark,
                          "SFStatus": d.SFStatus == null ? "请选择" : d.SFStatus,
                          "SFStatusCode": SFStatusCode
                      });
                  }
                  SFDetail = newSFDetail;
                  InitSFDetail(SFDetail);
                  
                  if (res.Data.IsTaxYZ == "1") {
                      $('#divYZ').show();
                  }
                  else {
                      $('#divYZ').hide();
                  }
                  if (res.Data.IsUse == "1") {
                      $('#divUSE').show();
                  }
                  else {
                      $('#divUSE').hide();
                  }
                  var RPStatus = res.Data.RPStatus;
                  if (RPStatus == 1) {
                      $('#selPay').attr("lay-verify", "required");
                      $('#selPay').removeAttr('disabled');
                      $('#selPay').removeClass('layui-disabled');
                  }
                  else if (RPStatus == 0) {
                      $('#selPay').removeAttr('lay-verify');
                      $('#selPay').attr('disabled', 'disabled');
                      $('#selPay').addClass('layui-disabled');
                  }
                  $("#InvoiceDate").click(function () {
                      laydate({ elem: '#InvoiceDate', format: 'YYYY-MM-DD' });
                  });
                  form.render();
                  layer.msg(res.Message);
              }
              else {
                  $.warning(res.Message);
              }

          }, function (err) {
              $.warning(err.Message);

          });
    };
    init(id);
    
    function InitAllControl() {
        $('#divYZ').hide();
        $('#divUSE').hide();
        InitTaxYZControl("0");
        InitUseControl("0");
    }
    function InitTaxYZControl(d) {
        if (d == "0") {
            //设置非必填
            $('#selFPType').removeAttr('lay-verify');


            $('#selPay').removeAttr('lay-verify');


            $('#selVAT').removeAttr('lay-verify');


            $('#Money').removeAttr('lay-verify');
            //$('#Money').attr('disabled', 'disabled');
            //$('#Money').addClass('layui-disabled');

            $('#selRP').removeAttr('lay-verify');


            $('#txtTAX').removeAttr('lay-verify');
            $('#txtTAX').attr('disabled', 'disabled');
            $('#txtTAX').addClass('layui-disabled');
        }
        else {
            //设置必填
            $('#selFPType').attr("lay-verify", "required");


            $('#selPay').attr("lay-verify", "required");


            $('#selVAT').attr("lay-verify", "required");


            $('#Money').attr("lay-verify", "required");
            //$('#Money').removeAttr('disabled');
            //$('#Money').removeClass('layui-disabled');

            $('#selRP').attr("lay-verify", "required");


            $('#txtTAX').attr("lay-verify", "required");
            $('#txtTAX').removeAttr('disabled');
            $('#txtTAX').removeClass('layui-disabled');
        }
    }
    function InitUseControl(d) {
        if (d == "0") {
            //设置非必填
            $('#SF_Money').removeAttr('lay-verify');
            //$('#SF_Money').attr('disabled', 'disabled');
            //$('#SF_Money').addClass('layui-disabled');

            $('#BadMoney').removeAttr('lay-verify');
            //$('#BadMoney').attr('disabled', 'disabled');
            //$('#BadMoney').addClass('layui-disabled');

            $('#selSFType').removeAttr('lay-verify');
            //$('#selSFType').attr('disabled', 'disabled');
            //$('#selSFType').addClass('layui-disabled');

            $('#txtBasicDataName').removeAttr('lay-verify');
            //$('#txtBasicDataName').attr('disabled', 'disabled');
            //$('#txtBasicDataName').addClass('layui-disabled');

        }
        else {
            //设置必填
            $('#SF_Money').attr("lay-verify", "required");
            //$('#SF_Money').removeAttr('disabled');
            //$('#SF_Money').removeClass('layui-disabled');

            $('#BadMoney').attr("lay-verify", "required");
            //$('#BadMoney').removeAttr('disabled');
            //$('#BadMoney').removeClass('layui-disabled');

            $('#selSFType').attr("lay-verify", "required");
            //$('#selSFType').removeAttr('disabled');
            //$('#selSFType').removeClass('layui-disabled');

            $('#txtBasicDataName').attr("lay-verify", "required");
            //$('#txtBasicDataName').removeAttr('disabled');
            //$('#txtBasicDataName').removeClass('layui-disabled');

        }
    }
    function initSelect() {
        var request = {};
        request.Token = token;
        request.Type = 1;
        $.Post("/fas/fp/InvoiceDataGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-opt");

                    var dataHtml = template.render(res.Data);

                    $('#selFPType').append(dataHtml);
                    if (_data) {
                        $('#selFPType').val(_data.Type);
                        sendFP();
                    }
 
                    form.render();
                }
                else {
                    $.warning(res.Message);
                }
            }, function (err) {
                $.warning(err.Message);
            });
    };


    //发票选择
    form.on("select(fp)", function (data) {
        _data = undefined;
        sendFP();
        return false;
    });

    //是否增票
    form.on("select(vat)", function (data) {

        var isVAT = data.value;
        if (isVAT == 1) {
            $('#txtTAX').attr("lay-verify", "required");
            $('#txtTAX').removeAttr('disabled');
            $('#txtTAX').removeClass('layui-disabled');
        }
        else if (isVAT == 0) {
            $('#txtTAX').removeAttr('lay-verify');
            $('#txtTAX').attr('disabled', 'disabled');
            $('#txtTAX').addClass('layui-disabled');
        }
        sendVAT();
        return false;
    });
    //收付状态
    form.on("select(rp)", function (data) {
        var RPStatus = data.value;
        if (RPStatus == 1) {
            $('#selPay').attr("lay-verify", "required");
            $('#selPay').removeAttr('disabled');
            $('#selPay').removeClass('layui-disabled');
        }
        else if (RPStatus == 0) {
            $('#selPay').removeAttr('lay-verify');
            $('#selPay').attr('disabled', 'disabled');
            $('#selPay').addClass('layui-disabled');
        }
        sendRP();
        return false;
    });

    function sendFP() {
        var request = {};
        request.Token = token;
        request.FPType = $('#selFPType').val();
        request.Type = 2;
        $.Post("/fas/fp/InvoiceDataGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-opt");

                    var dataHtml = template.render(res.Data);
                    $('#selVAT').html('');
                    var opt = template.render({ Code: '', Name: '请选择' });
                    $('#selVAT').append(opt);
                    $('#selVAT').append(dataHtml);

                    if (_data) {
                        if (_data.IsVAT == 0) {
                            $('#selVAT').val(_data.IsVAT);
                            $('#txtTAX').removeAttr('lay-verify');
                            $('#txtTAX').attr('disabled', 'disabled');
                            $('#txtTAX').addClass('layui-disabled');
                        }
                        else if (_data.IsVAT == 1) {
                            $('#selVAT').val(_data.IsVAT);
                            $('#txtTAX').attr("lay-verify", "required");
                            $('#txtTAX').removeAttr('disabled');
                            $('#txtTAX').removeClass('layui-disabled');
                        }
                        sendVAT();
                    }
                  

                    $('#selRP').html('');
                    var opt = template.render({ Code: '', Name: '请选择' });
                    $('#selRP').append(opt);

                    $('#selPay').html('');
                    var opt = template.render({ Code: '', Name: '请选择' });
                    $('#selPay').append(opt);

                
                    form.render();
                }
                else {
                    $.warning(res.Message);
                }
            }, function (err) {
                $.warning(err.Message);
            });
    }
    function sendVAT() {
        var request = {};
        request.Token = token;
        request.FPType = $('#selFPType').val();
        request.IsVAT = $('#selVAT').val();
        request.Type = 3;
        $.Post("/fas/fp/InvoiceDataGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-opt");

                    var dataHtml = template.render(res.Data);
                    $('#selRP').html('');
                    var opt = template.render({ Code: '', Name: '请选择' });
                    $('#selRP').append(opt);
                    $('#selRP').append(dataHtml);

                    if (_data) {
                        $('#selRP').val(_data.RPStatus);
                        sendRP();
                    }
                    form.render();
                }
                else {
                    $.warning(res.Message);
                }
            }, function (err) {
                $.warning(err.Message);
            });
    }
    function sendRP() {
        var request = {};
        request.Token = token;
        request.FPType = $('#selFPType').val();
        request.IsVAT = $('#selVAT').val();
        request.RPStatus = $('#selRP').val();
        request.Type = 4;
        $.Post("/fas/fp/InvoiceDataGet", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    var template = $.templates("#tpl-opt");

                    var dataHtml = template.render(res.Data);
                    $('#selPay').html('');
                    var opt = template.render({ Code: '', Name: '请选择' });
                    $('#selPay').append(opt);
                    $('#selPay').append(dataHtml);
                    if (_data) {
                        $('#selPay').val(_data.PayMode);
                    }

                    form.render();
                }
                else {
                    $.warning(res.Message);
                }
            }, function (err) {
                $.warning(err.Message);
            });
    }
 
   
    //启用税金预知
    form.on('switch(yz)', function (data) {
        debugger;
        if (data.elem.checked) {
            $('#divYZ').show();
            InitTaxYZControl("1");
            var RPStatus = $('#selRP').val();
            if (RPStatus == 1) {
                $('#selPay').attr("lay-verify", "required");
                $('#selPay').removeAttr('disabled');
                $('#selPay').removeClass('layui-disabled');
            }
            else if (RPStatus == 0) {
                $('#selPay').removeAttr('lay-verify');
                $('#selPay').attr('disabled', 'disabled');
                $('#selPay').addClass('layui-disabled');
            }
            var isVAT = $('#selVAT').val();
            if (isVAT == 1) {
                $('#txtTAX').attr("lay-verify", "required");
                $('#txtTAX').removeAttr('disabled');
                $('#txtTAX').removeClass('layui-disabled');
            }
            else if (isVAT == 0) {
                $('#txtTAX').removeAttr('lay-verify');
                $('#txtTAX').attr('disabled', 'disabled');
                $('#txtTAX').addClass('layui-disabled');
            }
        }
        else {
            $('#divYZ').hide();
            InitTaxYZControl("0");
        }


        return false;
    });
    //启用应收应付与到期提醒
    form.on('switch(use)', function (data) {
        debugger;
        if (data.elem.checked) {
            $('#divUSE').show();
            InitUseControl("1");
        }
        else {
            $('#divUSE').hide();
            InitUseControl("0");
        }


        return false;
    });
    //设置收付明细
     setSFdetail = function (data) {
        if (data.seq != null) {
            //update

            for (var i = 0; i < SFDetail.length; i++) {
                if (data.seq == SFDetail[i].seq) {
                    SFDetail[i].SFDate = data.SFDate;
                    SFDetail[i].SFMoney = data.SFMoney;
                    SFDetail[i].SFRemark = data.SFRemark;
                    SFDetail[i].SFStatus = data.SFStatus;
                    SFDetail[i].SFStatusCode = data.SFStatusCode;
                }

            }

        }
        else {
            //add
            var newDetails = [];
            if (SFDetail.length == 0) {
                newDetails.push({
                    "SFDate": data.SFDate,
                    "SFMoney": data.SFMoney,
                    "seq": newDetails.length + 1,
                    "SFRemark": data.SFRemark,
                    "SFStatus": data.SFStatus,
                    "SFStatusCode": data.SFStatusCode
                });
            }
            else {
                for (var i = 0; i < SFDetail.length; i++) {
                    SFDetail[i].seq = i + 1;
                    newDetails.push(SFDetail[i]);
                }
                newDetails.push({
                    "SFDate": data.SFDate,
                    "SFMoney": data.SFMoney,
                    "seq": SFDetail.length + 1,
                    "SFRemark": data.SFRemark,
                    "SFStatus": data.SFStatus,
                    "SFStatusCode": data.SFStatusCode
                });
            }
            SFDetail = newDetails;
        }
        InitSFDetail(SFDetail);

    };

    //加载收付明细
    function InitSFDetail(detail) {
        var html = '';
        if (detail.length > 0) {
            detail.sort(compare("SFDate"));
        }
        for (var i = 0; i < detail.length; i++) {
            html += '<tr>'
                + '<td>' + detail[i].SFDate + '</td>'
                + '<td>' + detail[i].SFStatus + '</td>'
                + '<td>' + detail[i].SFMoney + '</td>'
                + '<td>' + detail[i].SFRemark + '</td>'
                + '<td>'
                + '<a class="layui-btn layui-btn-mini tks-Edit" data-seq="' + detail[i].seq + '" data-SFDate="' + detail[i].SFDate + '" data-SFStatus="' + detail[i].SFStatusCode + '"  data-SFMoney="' + detail[i].SFMoney + '"  data-SFRemark="' + detail[i].SFRemark + '" > 编辑</a>'
                + '<a class="layui-btn layui-btn-danger layui-btn-mini tks-Del" data-seq="' + detail[i].seq + '"> 删除</a>'
                + '</td > '
                + '</tr>';

        }

        $('#SFDetail').html(html);
        GetBadMoney();
    }

    //计算坏账
    var GetBadMoney = function () {
        var Money = $("[name='Money']").val() == "" ? "0" : $("[name='Money']").val();
        var SumMoney = 0;
        for (var i = 0; i < SFDetail.length; i++) {
            SumMoney += Number(SFDetail[i].SFMoney);
        }
        $("[name='BadMoney']").val(FloatSub(Number(Money), Number(SumMoney)));
    }
    //浮点数减法运算
    function FloatSub(arg1, arg2) {
        var r1, r2, m, n;
        try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
        try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
        m = Math.pow(10, Math.max(r1, r2));
        //动态控制精度长度
        n = (r1 >= r2) ? r1 : r2;
        return ((arg1 * m - arg2 * m) / m).toFixed(n);
    }
    $("body").on("keyup", "[name='Money']", function () {

        var $amountInput = $(this);
        var tmptxt = $(this).val();
        //$(this).val(tmptxt.subString(0,1) + '.' + tmptxt.subString(2));
        var FirstChar = tmptxt.substr(0, 1);
        //使用字符分离获取输入的第一位
        var SecondChar = tmptxt.substr(1, 2);
        // 使用字符分离获取输入的第二位
        if (FirstChar == "0") {
            SecondChar.replace(/[0,1,2,3,45,6,7,8,9]/, "0.");
        }
        //如果第一位是0，将第一位替换成0.
        // $(this).val(tmptxt.replace(/\D|^0/g,''));
        event = window.event || event;
        if (event.keyCode == 37 | event.keyCode == 39) {
            return;
        }

        //先把非数字的都替换掉，除了数字和. 
        $amountInput.val($amountInput.val().replace(/[^\d.]/g, "").
            //只允许一个小数点              
            replace(/^\./g, "").replace(/\.{2,}/g, ".").
            //只能输入小数点后两位
            replace(".", "$#$").replace(/\./g, "").replace("$#$", ".").replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'));
        //如果第一位是负号，则允许添加
        if (FirstChar == '-') {
            $amountInput.val('-' + $amountInput.val());

        }
        $("[name='SF_Money']").val($amountInput.val());
        GetBadMoney();

    });
    $("body").on("keyup", "[name='SF_Money']", function () {

        var $amountInput = $(this);
        var tmptxt = $(this).val();
        //$(this).val(tmptxt.subString(0,1) + '.' + tmptxt.subString(2));
        var FirstChar = tmptxt.substr(0, 1);
        //使用字符分离获取输入的第一位
        var SecondChar = tmptxt.substr(1, 2);
        // 使用字符分离获取输入的第二位
        if (FirstChar == "0") {
            SecondChar.replace(/[0,1,2,3,45,6,7,8,9]/, "0.");
        }
        //如果第一位是0，将第一位替换成0.
        // $(this).val(tmptxt.replace(/\D|^0/g,''));
        event = window.event || event;
        if (event.keyCode == 37 | event.keyCode == 39) {
            return;
        }

        //先把非数字的都替换掉，除了数字和. 
        $amountInput.val($amountInput.val().replace(/[^\d.]/g, "").
            //只允许一个小数点              
            replace(/^\./g, "").replace(/\.{2,}/g, ".").
            //只能输入小数点后两位
            replace(".", "$#$").replace(/\./g, "").replace("$#$", ".").replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'));
        //如果第一位是负号，则允许添加
        if (FirstChar == '-') {
            $amountInput.val('-' + $amountInput.val());

        }
        $("[name='Money']").val($amountInput.val());
        GetBadMoney();

    });
    $("body").on("keyup", "[name='TaxMoney']", function () {

        var $amountInput = $(this);
        var tmptxt = $(this).val();
        //$(this).val(tmptxt.subString(0,1) + '.' + tmptxt.subString(2));
        var FirstChar = tmptxt.substr(0, 1);
        //使用字符分离获取输入的第一位
        var SecondChar = tmptxt.substr(1, 2);
        // 使用字符分离获取输入的第二位
        if (FirstChar == "0") {
            SecondChar.replace(/[0,1,2,3,45,6,7,8,9]/, "0.");
        }
        //如果第一位是0，将第一位替换成0.
        // $(this).val(tmptxt.replace(/\D|^0/g,''));
        event = window.event || event;
        if (event.keyCode == 37 | event.keyCode == 39) {
            return;
        }

        //先把非数字的都替换掉，除了数字和. 
        $amountInput.val($amountInput.val().replace(/[^\d.]/g, "").
            //只允许一个小数点              
            replace(/^\./g, "").replace(/\.{2,}/g, ".").
            //只能输入小数点后两位
            replace(".", "$#$").replace(/\./g, "").replace("$#$", ".").replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'));
        //如果第一位是负号，则允许添加
        if (FirstChar == '-') {
            $amountInput.val('-' + $amountInput.val());

        }


    });
    //操作收付明细
    $("body").on("click", ".tks-Edit", function () {  //编辑

        if ($('#selSFType').val() == "" || $('#selSFType').val() == undefined) {
            $.warning("请选择收付类型");
            return false;
        }
        var SFType = "ys";//应收
        if ($('#selSFType').val() == "应付") {
            SFType = "yf";//应付
        }
        var seq = $(this).attr('data-seq');
        var SFDate = $(this).attr('data-SFDate');
        var SFStatus = $(this).attr('data-SFStatus');
        var SFMoney = $(this).attr('data-SFMoney');
        var SFRemark = $(this).attr('data-SFRemark');
        layer.open({
            type: 2,
            title: '编辑收付明细',
            shade: 0.1,
            area: ['500px', '400px'],
            content: 'SFDetailEditor.aspx?SFType=' + SFType + '&seq=' + seq + '&SFDate=' + SFDate + '&SFStatus=' + SFStatus + '&SFMoney=' + SFMoney + '&SFRemark=' + SFRemark,
            cancel: function (index, layero) {
                //$.warning('请保存辅助核算项');
                //return false;
            }
        });

    });

    $("body").on("click", ".tks-Del", function () {  //删除

        var seq = $(this).attr('data-seq');
        $.confirm('确定删除吗？', function () {

            for (var i = 0; i < SFDetail.length; i++) {
                if (SFDetail[i].seq == seq) {
                    SFDetail.splice(i, 1);
                }
            }
            var newDetails = [];
            for (var i = 0; i < SFDetail.length; i++) {
                newDetails.push({
                    "SFDate": SFDetail[i].SFDate,
                    "SFMoney": SFDetail[i].SFMoney,
                    "seq": i + 1,
                    "SFRemark": SFDetail[i].SFRemark,
                    "SFStatus": SFDetail[i].SFStatus,
                    "SFStatusCode": SFDetail[i].SFStatusCode
                });
            }
            SFDetail = newDetails;
            InitSFDetail(SFDetail);
        });

    });
    var compare = function (prop) {
        return function (obj1, obj2) {
            var val1 = new Date(Date.parse(obj1[prop]));
            var val2 = new Date(Date.parse(obj2[prop]));
            if (val1 < val2) {
                return -1;
            } else if (val1 > val2) {
                return 1;
            } else {
                return 0;
            }
        }
    }
    form.on("submit(save)", function (data) {

        if (data.field.PayMode == "") {
            data.field.PayMode = "-1";
        }
        if (data.field.RPStatus == "") {
            data.field.RPStatus = "-1";
        }
        if (data.field.IsVAT == "") {
            data.field.IsVAT = "-1";
        }
        if (data.field.Type == "") {
            data.field.Type = "-1";
        }
        if (data.field.IsTaxYZ == 'on') {
            data.field.IsTaxYZ = 1;
            if (data.field.Type == "" || data.field.Type =="-1") {
                $.warning("请选择业务类型");
                return false;
            }
            if (data.field.IsVAT == "" || data.field.IsVAT == "-1") {
                $.warning("请选择增值税专用发票");
                return false;
            }
            if (data.field.RPStatus == "" || data.field.RPStatus == "-1") {
                $.warning("请选择收付状态");
                return false;
            }
            if (data.field.RPStatus == "1") {
                if (data.field.PayMode == "" || data.field.PayMode == "-1") {
                    $.warning("请选择支付方式");
                    return false;
                }
            }
            if (data.field.Money == "") {
                $.warning("请填写含税金额");
                return false;
            }
        }
        else {
            data.field.IsTaxYZ = 0;

        }
        if (data.field.IsUse == 'on') {
            data.field.IsUse = 1;
            if (SFDetail.length==0) {
                $.warning("请添加收付明细");
                return false;
            }
            if (data.field.Money == "") {
                $.warning("请填写含税金额");
                return false;
            }
            if (data.field.SFType == "" || data.field.SFType == "-1") {
                $.warning("请选择收付类型");
                return false;
            }
            if (data.field.BasicDataId == "") {
                $.warning("请选择客户/供应商");
                return false;
            }
        }
        else {
            data.field.IsUse = 0;

        }
        //收付明细
        var newSFDetail = [];
        for (var i = 0; i < SFDetail.length; i++) {
            var info = SFDetail[i];
            if (info.SFStatus == "") {
                $.warning("请选择细项收付状态");
                return false;
            }
            if (info.SFMoney == "") {
                $.warning("请填写收付金额");
                return false;
            }
            newSFDetail.push({
                "SFDate": info.SFDate,
                "SFMoney": info.SFMoney,
                "SFStatus": info.SFStatus,
                "SFRemark": info.SFRemark,
                "Seq": info.seq
            });
        }
        var jsonStrSFDetail = JSON.stringify(newSFDetail);
        var request = {};
        request.Data = data.field;

        request.Data.Id = id;
        request.Token = token;
        request.SFDetail = jsonStrSFDetail
        //弹出loading
        var index = $.loading('数据提交中，请稍候');


        $.Post("/fas/fp/InvoiceUpdate", request,
        function (data) {
            var res = data;
            layer.close(index);
            if (!res.IsSuccess) {
                $.warning(res.Message);
            }
            else {
                layer.alert(res.Message);

            }


        }, function (err) {

            layer.close(index);
            $.warning(err.Message);
        });


        return false;
    })





    var query = function (pageIndex) {

        var index = $.loading('查询中');
        var request = {};

        request.Token = token;
        request.InvoiceId = id;
        request.PageIndex = pageIndex;
        request.PageSize = 10;
        $.Post("/fas/fp/FPFJListSearch", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    debugger;

                    var template = $.templates("#tpl-img");

                    for (var i = 0; i < res.Data.length; i++) {
                        res.Data[i].Path = $.baseUrl + res.Data[i].Path;
                    }
                    var dataHtml = template.render(res.Data);

                    $('#dt').html(dataHtml);

                    $('.One').simpleSlide();
                    $('.total').text('附件总数 ' + res.Total);


                    form.render();
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };
    var rowDel = function (id) {
        var index = $.loading('正在删除');
        var request = {};
        request.Data = { Id: id };
        request.Token = token;

        $.Post("/fas/fp/FPFJDel", request,
            function (data) {
                var res = data;
                if (res.IsSuccess) {
                    $.info(res.Message, function () {
                        query(1);
                    });

                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    };


    //收付类型
   
    form.on("select(SFType)", function (data) {
        $('#txtBasicDataId').val("");
        $('#txtBasicDataName').val("");
        var SFType = data.value;
        if (SFType == "应收") {
            DataType = "Customer";
        }
        else if (SFType == "应付") {
            DataType = "Vendor";
        }
        else {
            DataType = "";
        }
        for (var i = 0; i < SFDetail.length; i++) {
            SFDetail[i].SFStatus = "";
            SFDetail[i].SFStatusCode = "";

        }
        InitSFDetail(SFDetail);
        return false;
    });
    $("body").on("click", "#btnBasicSearch", function () {
        if (DataType != "") {
            $.dialog("客户/供应商选择", '/view/fas/fp/basicChoose.aspx?DataType=' + DataType);
        }
        else {
            $.warning("请选择收付类型");
            return false;
        }
    })

    //添加明细
    $("body").on("click", "#btnAddDetail", function () {
        if ($('#selSFType').val() == "" || $('#selSFType').val() == undefined) {
            $.warning("请选择收付类型");
            return false;
        }
        var SFType = "ys";//应收
        if ($('#selSFType').val() == "应付") {
            SFType = "yf";//应付
        }
        layer.open({
            type: 2,
            title: '添加收付明细',
            shade: 0.1,
            area: ['500px', '400px'],
            content: 'SFDetailEditor.aspx?SFType=' + SFType,
            cancel: function (index, layero) {
                //$.warning('请保存辅助核算项');
                //return false;
            }
        });
    })

    window.setValue = function (code, name) {
        $('#txtBasicDataId').val(code);
        $('#txtBasicDataName').val(name);
    }
    $("body").on("click", ".tks-rowDel", function () {  //删除
        var _this = $(this);
        $.confirm('确定删除此附件？', function () {


            rowDel(_this.attr("data-id"));

        });
    })
    $("body").on("click", "#btnBack", function () {
        if (parent.query)
            parent.query(1);
        parent.layer.closeAll();
    })
    $("body").on("click", "#btnAdd", function () {
        if (id == '' || id == null) {
            $.warning('请先保存');
            return;
        }

        $.dialog('上传', '../fpfj/attachment.aspx?id=' + id + '&token=' + token, undefined, function () {
            query(1);
        });
    })

    if (id != '')
        query(1);

})
