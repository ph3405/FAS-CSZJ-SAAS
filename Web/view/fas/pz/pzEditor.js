/// <reference path="../../../layui/lay/modules/jquery.js" />

layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
        layer = layui.layer,
        laypage = layui.laypage;
    var $ = layui.jquery;
    var LODOP;

    var savedData;       //保存的数据
    var accountName = '';//账套名称

    //千分位
    $.views.converters("thousand", function (val) {

        if (val == 0 || val == null || val == undefined) {
            return "";
        }
        else {
            return numeral(val).format('0,0.00');
        }
    });

    //去0
    $.views.converters("noZero", function (val) {

        if (val == 0 || val == undefined) {
            return "";
        }
        else {
            return val;
        }

    });

    //大写转换
    $.views.converters("trans2Show", function (val) {

        if (val == 0 || val == undefined) {
            return "";
        } else {
            return editor.trans2Show(val);
        }


    });

    function CreateOneFormPage() {
        LODOP = getLodop();
        LODOP.PRINT_INIT("凭证打印");

        var template = $.templates("#tpl-pzPrint");
        //Auto-Width 整宽高度会按比例缩放Full-Width
        LODOP.SET_PRINT_MODE("PRINT_PAGE_PERCENT", "Width:100%");


        var pzEle = $($('#PZZ').find('option:selected')[0]);
        var pzTitle = $(pzEle).attr('data-title');
        var pzz = $(pzEle).text();
        var arrDoc = new Array();
        var debitAll = 0;
        var items = new Array();

        for (var i = 0, j = 1, k = 1; i < savedData.Data.length; i++ , j++) {
            arrDoc.push({
                Summary: $.trim(savedData.Data[i].Summary),
                SubjectDes: $.trim(savedData.Data[i].SubjectDescription),
                Debit: savedData.Data[i].Money_Debit,
                Credit: savedData.Data[i].Money_Credit
            });
            if (j / 5 == k) {
                k++;
                var arr = arrDoc;
                items.push(arr);
                arrDoc = new Array();
            }

        }
        var len = arrDoc.length;
        if (arrDoc.length != 0) {
            for (var i = 0; i < 5 - len; i++) {
                arrDoc.push({});
            }
            items.push(arrDoc);
        }


        LODOP.NewPage();
        var printHtml = new Array();
        var dataHtml = '';
        for (var i = 0, j = 1, k = 1; i < items.length; i++ , j++) {


            var pageNo = '';
            if (items.length > 1)
                pageNo = '(' + (i + 1) + '/' + items.length + ')';

            var data = {
                PZName: pzTitle,
                AttachmentNum: savedData.Head.AppendNum,
                HSCompany: accountName,
                PZDate: savedData.Head.PZDate,
                PZZ: pzz + '-' + $('#PZZNO').val() + pageNo,
                Docs: items[i],
                Total: $('#debitAllUper').text(),
                DebitAll: savedData.Head.AMT_DBT,
                CreditAll: savedData.Head.AMT_DBT,
                TrueName: trueName
            };
            dataHtml += template.render(data);

            if (j / 2 == k) {
                k++;
                printHtml.push(dataHtml);
                dataHtml = '';
            }

        }
        if (dataHtml != '')
            printHtml.push(dataHtml);

        for (var i = 0; i < printHtml.length; i++) {
            LODOP.NewPage();
            LODOP.ADD_PRINT_HTM(10, 10, '100%', '100%', printHtml[i]);
        }




    };


    var id = $.getQueryString('Id');//凭证ID

    var tplId = $.getQueryString('tplid');//模板ID

    var type = $.getQueryString('type');//来源  QM  生成凭证，会携带一个金额

    var money = $.getQueryString('money');//模板生成凭证，所填写的金额

    var tax = $.getQueryString('tax');//发票上传，特有的税金

    var key = $.getQueryString('key');//通用的KEY，用来存储对应的单证的ID

    var editor = new PZEditor('editor');

    $("#txtPZDate").click(function () {
        laydate({
            elem: '#txtPZDate', format: 'YYYY-MM-DD',
            choose: function (value) {
                var request = {};
                request.Token = token;
                request.DocDate = value;
                $.Post('/fas/doc/GetNextDocNO', request,
                    function (data) {
                        var res = data;
                        if (!res.IsSuccess) {
                            $.warning(res.Message);
                            $("#txtPZDate").val(lastPZDate);
                        }
                        else {
                            $("#PZZNO").val(res.NO);
                            lastPZDate = request.PZDate;
                        }
                    }, function (err) {
                    });
            }
        });
    });


    $("#btnFJ").click(function () {
        if (id == '' || id == null) {
            $.warning('请先保存，再上传附件');
            return;
        }
        $.open("附件编辑", "/view/fas/fj/fjList.aspx?id=" + id);
    });

    var setBtnDisabledAddPrint = function () {
        $('#btnAdd').attr('disabled', 'disabled');
        $('#btnPrint').attr('disabled', 'disabled');
        $('#btnAdd').addClass('layui-btn-disabled');
        $('#btnPrint').addClass('layui-btn-disabled');
        $('#btnAdd').off('click');
        $('#btnPrint').off('click');
    }

    var setBtnEnabledAddPrint = function () {
        $('#btnAdd').removeAttr('disabled');
        $('#btnPrint').removeAttr('disabled');
        $('#btnAdd').removeClass('layui-btn-disabled');
        $('#btnPrint').removeClass('layui-btn-disabled');
        $('#btnAdd').off('click');
        $('#btnPrint').off('click');
        $('#btnAdd').click(function () {
            window.location.href = "pzEditor.aspx";
        });
        $('#btnPrint').click(function () {
            CreateOneFormPage();
            LODOP.PREVIEW();
        });
    }

    var setBtnDisabledSave = function () {
        $('#btnSave').attr('disabled', 'disabled');
        $('#btnSave').addClass('layui-btn-disabled');
        $('#btnSave').off('click');
    };

    var setBtnEnabledSave = function () {
        $('#btnSave').removeAttr('disabled');
        $('#btnSave').removeClass('layui-btn-disabled');
        //保存
        $('#btnSave').off('click');
        $('#btnSave').click(function () {
            save('');
        });
    }

    var setBtnEnabledPrev = function () {
        $('#btnPrev').removeAttr('disabled');
        $('#btnPrev').removeClass('layui-btn-disabled');
        //上一页
        $('#btnPrev').off('click');
        $('#btnPrev').click(function () {
            Prev();
        });
    }
    var setBtnDisabledPrev = function () {
        $('#btnPrev').attr('disabled', 'disabled');
        $('#btnPrev').addClass('layui-btn-disabled');
        $('#btnPrev').off('click');
    };
    var setBtnEnabledNext = function () {
        $('#btnNext').removeAttr('disabled');
        $('#btnNext').removeClass('layui-btn-disabled');
        //下一页
        $('#btnNext').off('click');
        $('#btnNext').click(function () {
            Next();
        });
    }
    var setBtnDisabledNext = function () {
        $('#btnNext').attr('disabled', 'disabled');
        $('#btnNext').addClass('layui-btn-disabled');
        $('#btnNext').off('click');
    };
    if (id == '' || id == null || type == "FZ") {
        setBtnDisabledAddPrint();
        setBtnEnabledSave();
    } else {
        setBtnEnabledAddPrint();
        setBtnDisabledSave();
    }

    var dataInit = function (PZZNO, PZDate) {
        //setBtnEnabledSave();
        var request = {};
        request.Token = token;
        request.Data = { Id: id };
        var index = $.loading('获取数据');
        $.Post('/fas/doc/docGet', request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    
                    var date = res.Head.PZDate.split('T');
                    $('#txtPZDate').val(date[0]);
                    $('#PZZ').val(res.Head.PZZ);
                    $('#PZZNO').val(res.Head.PZZNO);
                    if (type == "FZ") {
                        $('#PZZNO').val(PZZNO);
                        $('#txtPZDate').val(PZDate);
                    }
                    $('#txtAppendNo').val(res.Head.AppendNum);
                    var m = digitUppercase(res.Head.AMT_DBT);
                    $('#debitAllUper').html(m);

                    var total = editor.trans2Show(res.Head.AMT_DBT);

                    $('#txtDebitAll').text(total);
                    $('#txtCreditAll').text(total);
                    editor.init(res.Detail,"");
                    savedData = editor.getData();
                    if (savedData == null) {
                        return false;
                    }
                    form.render();

                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });
    }
    var getAllPZ = function () {
        var index = $.loading('获取凭证');
        var request = {};
        request.Token = token;
        $.Post("/fas/doc/GetDocByAccount", request,
            function (data) {
                
                var res = data;
                if (res.IsSuccess) {

                    if (res.lstAll.length > 0) {
                        setBtnEnabledPrev();
                        var doc = res.lstAll;
                        for (var i = 0; i < doc.length; i++) {
                            allPZ.push(doc[i].Id);
                        }
                    }
                    else {
                        setBtnDisabledPrev();
                    }
                } else {
                    $.warning(res.Message);
                }
                layer.close(index);
            }, function (err) {
                $.warning(err.Message);
                layer.close(index);
            });
    }
    var allPZ = [];//所有凭证
    var PZ_index = 0;
    var pzzInit = function () {
        setBtnDisabledNext();
        var request = {};
        request.Token = token;
        request.Type = type;
        var index = $.loading('初始化中');
        $.Post("/fas/set/PZZTotalGet", request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    accountName = res.AccountName;

                    var template = $.templates("#tpl-pzz");

                    var dataHtml = template.render(res.Data);

                    $('#PZZ').append(dataHtml);

                    form.render();
                  
                    getAllPZ();
                    if (id != '' && id != null && type != 'FZ') {
                        dataInit('', '');//加载已生成的凭证
                    }
                    else if (type == 'QM' || type == 'KS' || type == 'FP' || type == 'CHANGE' || type == 'GD') {
                        //模板加载
                      
                        TPLInit();
                        $('#PZZNO').val(res.No);
                        $('#txtPZDate').val(res.DefaultDate);
                    }
                    else {
                        //加载复制的凭证
                        if (type == 'FZ') {
                            dataInit(res.No, res.DefaultDate);//加载已生成的凭证

                        }
                        else {
                            editor.init([],"");
                            $('#PZZNO').val(res.No);
                            $('#txtPZDate').val(res.DefaultDate);
                        }

                    }
                    lastPZDate = $('#txtPZDate').val();
                    //var tds = $('table tr:first td');
                    $("tr:eq(1) td:eq(0)").find(".zyInput").focus();
                    //$("#editor").parents("tr").find(":eq(0)").find(".zyInput").focus();
                }

            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });

    };
    pzzInit();

    function TPLInit() {
        setTPL2(tplId);
    }


    

    $('#btnTPL').click(function () {
        $.dialog("模板选择", 'tplChoose.aspx');
    });
    //$('#addSub').click(function () {
    //    AddSubject();
    //});
    function Prev() {
        setBtnEnabledNext();
        setBtnEnabledAddPrint();
        if (PZ_index == 0) {
            setBtnDisabledNext();
            id = allPZ[allPZ.length - 1];
            PZ_index = allPZ.length - 1;
            
            if (PZ_index == 0 || allPZ.length == 1) {
                setBtnDisabledPrev();
            }
            dataInit('', '');
        }
        else {
            id = allPZ[PZ_index-1];
            PZ_index--;
            if (PZ_index == 0) {
                setBtnDisabledPrev();
            }
            dataInit('', '');
        }
    }

    function Next() {
        setBtnEnabledPrev();
        if (PZ_index < allPZ.length-1) {
            id = allPZ[PZ_index + 1];
            PZ_index++;
            if (PZ_index == allPZ.length - 1) {
                setBtnDisabledNext();
            }
            dataInit('', '');
        }
       
    }

    function save(isAdd) {
        var data = editor.getData();
        if (data == null) {
            return false;
        }
        if (data.Data.length>0) {
            for (var i = 0; i < data.Data.length; i++) {
                if (data.Data[i].SubjectCode != "") {
                    if (data.Data[i].Money_Debit == "" && data.Data[i].Money_Credit == "") {
                        $.warning('亲，请填写明细金额');
                        return false;
                    }
                }
                if (data.Data[i].Money_Debit != "" || data.Data[i].Money_Credit != "") {
                    if (data.Data[i].SubjectCode == "" || data.Data[i].SubjectCode==undefined) {
                        $.warning('第' + Number(i + 1) + '行，请填写正确的科目');
                        return false;
                    }
                }
            }
        }
        else {
            $.warning('亲，请填写明细');
            return false;
        }
        savedData = data;//打印
        if (type == "FZ") {
            id = "";
        }
        var url = '';
        if (id == "" || id == undefined) {
            url = '/fas/doc/docAdd';
        }
        else {
            url = '/fas/doc/DocUpdate';
        }
        var request = {};
        request.Token = token;
        request.Head = data.Head;
        request.Head.Id = id;
        request.Detail = data.Data;
        request.Type = type;
        request.TPLId = tplId;
        request.Key = key;
        var index = $.loading('数据提交中');
        $.Post(url, request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    id = res.Id;
                    setBtnEnabledAddPrint();
                    setBtnDisabledSave();
                    $.topTip(res.Message);
                    if (isAdd=='123') {
                        window.location.href = "pzEditor.aspx";
                    }
                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });
    }
    function AddSubject() {
        parent.layer.open({
            type: 2,
            title: '新增科目',
            shade: 0.1,
            area: ['1200px', '600px'],
            //content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=CHANGE&money=" + money + "&key=" + id,
            content: '/view/fas/set/subject/subjectAddForPz.aspx',
            cancel: function (index, layer) {

                //query(1);

            },
            end: function () {
                window.clearActiveValue = editor.clearActiveValue;
                window.setActiveValue = editor.setActiveValue;
                window.setTPL = setTPL;
                //bindEvents();
                var request = {};
                request.Token = token;
                var index = $.loading('初始化中');
                $.Post("/fas/set/subjectTotalGet", request,
                    function (data) {
                        var res = data;
                        layer.close(index);
                        if (!res.IsSuccess) {
                            $.warning(res.Message);
                        }
                        else {
                            subjectData = res.Data;
                            bindAutocomplete($(".kmInput"), res.Data);
                            $.topTip(res.Message);
                        }


                    }, function (err) {

                        layer.close(index);
                        $.warning(err.Message);
                    });
            }
        });
    }
    //由界面选择模板加载
    function setTPL(id) {
        var request = {};
        request.Token = token;

        request.Data = { Id: id };

        var index = $.loading('加载中');
        $.Post('/fas/set/tplGet', request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    
                    
                    var m = digitUppercase(res.Head.AMT_DBT);
                    $('#debitAllUper').html(m);
                    $('#txtDebitAll').text(res.Head.AMT_DBT);
                    $('#txtCreditAll').text(res.Head.AMT_DBT);

                    

                    //editor.initTPL(res.Detail, res.Message);
                    //update by Hero.Zhang 判断模板明细中科目是否有子科目
                    if (res.CheckParent == "1") {
                        editor.initTPL(res.Detail, res.Message);

                    }
                    else {
                        editor.initTPL(res.Detail, "");
                    }
                    savedData = editor.getData();
                    if (savedData == null) {
                        return false;
                    }
                    form.render();

                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });
    }
    function selectSubject(d) {
        //dealSubject($(currentTr).find('.DisplayTextKemu'), d);
        var elem = $(currentTr).find('.kmInput');
        active(elem);//激活当前容器

        if (d.IsCalHelperValid == 1 ||
            d.IsCurrencyValid == 1 ||
            d.IsQuantityValid == 1) {
            layer.open({
                type: 2,
                title: '辅助核算编辑',
                shade: 0.1,
                area: ['500px', '400px'],
                content: 'kmAss.aspx?id=' + d.Id,
                cancel: function (index, layero) {
                    $.warning('请保存辅助核算项');
                    return false;
                }
            });
            $(elem).blur();
            return false;
        }
        else {
            var data = {};
            data.SubjectCode = d.Id;
            data.SubjectDescription = d.value;
            data.IsCalHelper = 0;
            data.IsCurrency = 0;
            data.IsQuantity = 0;
            data.Balance = 0;
            var html = getKMDes(data);
            $(elem).parent().find('.DisplayTextKemu').html(html);
            $(elem).html(data.SubjectDescription);
        }

    };
    //初始化根据模板ID加载模板，并填充金额
    function setTPL2(id) {
        var request = {};
        request.Token = token;
        request.Money = money;
        request.Tax = tax;
        request.Data = { Id: id };
        request.Key = key;
        var index = $.loading('加载中');
        $.Post('/fas/tplManage/TPLMGet', request,
            function (data) {
                var res = data;
                layer.close(index);
                if (!res.IsSuccess) {
                    $.warning(res.Message);
                }
                else {
                    
                    var m = digitUppercase(res.Head.AMT_DBT);
                    $('#debitAllUper').html(m);

                    var total = editor.trans2Show(res.Head.AMT_DBT);
                    $('#txtDebitAll').text(total);
                    $('#txtCreditAll').text(total);
                    //editor.init(res.Detail);
                    //update by Hero.Zhang 判断模板明细中科目是否有子科目
                    if (res.CheckParent == "1") {
                        editor.init(res.Detail, res.Message);

                    }
                    else {
                        editor.init(res.Detail, "");
                    }
                    savedData = editor.getData();
                    if (savedData == null) {
                        return false;
                    }
                    form.render();

                }


            }, function (err) {

                layer.close(index);
                $.warning(err.Message);
            });
    }
    var currentTr = {};
    var activeSubjectElem;
    //激活容器
    var active = function (elem) {
        activeSubjectElem = elem;
    }
    //取消激活
    var unActive = function () {
        activeSubjectElem = null;
    };
    //获取科目描述模板
    var getKMDes = function (data) {
        var template = $.templates("#tpl-kmdes");

        var dataHtml = template.render(data);


        return dataHtml;
    }
    function PZEditor(id) {

        var ed = $('#' + id);
        var rowAdd = $(' <a class="layui-btn layui-btn-mini rowAdd"><i class="layui-icon">&#xe654;</i></a>').appendTo('body');
        var rowRemove = $(' <a class="layui-btn layui-btn-mini rowRemove"><i class="layui-icon">&#xe640;</i></a>').appendTo('form');
        var rowAddSub = $(' <a class="layui-btn layui-btn-mini rowAddSub"><i class="layui-icon">&#xe615;</i></a>').appendTo('body');
        //var currentTr = {};
        //var activeSubjectElem;
        var subjectData;
        var saveData = new Array();
        var firstVal = '';
        var secondVal = '';
        var zy = '';//摘要

        //初始化
        this.init = function (data, msg) {
            var html = '';
            for (var j = 0; j < data.length; j++) {
                //没有金额的明细行，不显示
                if (Number(data[j].Money_Credit) + Number(data[j].Money_Debit) != 0) {
                    html += genRow(data[j]);
                }

            }


            for (var i = 0; i < 4 - data.length; i++) {
                html += genRow({});
            }
            $(ed).html(html);
            bindEvents(msg);

        };

        this.initTPL = function (data,msg) {
            var html = '';
            for (var j = 0; j < data.length; j++) {
                html += genRow(data[j]);
            }

            for (var i = 0; i < 4 - data.length; i++) {
                html += genRow({});
            }
            $(ed).html(html);
            
            bindEvents(msg);
            //if (msg != "") {
            //    $.warning(msg);
            //    return false;
            //}
        };

        this.getData = function () {

            var flag = checkJDPH();
            if (!flag) {
                $.warning('亲，借贷要平衡');
                return null;
            }
            var flag2 = checkJieDai();
            if (!flag2) {
                $.warning('亲，借贷不能同时有值');
                return null;
            }
            var res = {};
            saveData = new Array();
            $(ed).find('tr').each(function (index) {
                
                try {
                    var subEl = $(this).find('.s-subject')[0];
                    var data = new TKS_FAS_DocDetail();
                    data.Summary = $(this).find('.zyInput')[0].value;
                    data.SubjectCode = $(subEl).attr('data-subjectCode');
                    data.SubjectDescription = $(subEl).html();
                    data.IsCalHelper = $(subEl).attr('data-isCalHelper');
                    data.IsCurrency = $(subEl).attr('data-isCurrency');
                    data.IsQuantity = $(subEl).attr('data-isQuantity');
                    if (data.IsQuantity == 1) {
                        data.Quantity = $(subEl).attr('data-quantity');
                        data.Price = $(subEl).attr('data-price');
                        data.Unit = $(subEl).attr('data-unit');
                    }
                    if (data.IsCurrency == 1) {
                        data.CurrencyCode = $(subEl).attr('data-currencyCode');
                        data.Rate = $(subEl).attr('data-rate');
                        data.YB = $(subEl).attr('data-YB');
                    }

                    if (data.IsCalHelper == 1) {
                        data.CalValue1 = $(subEl).attr('data-cal');
                    }

                    data.Money_Debit = $($(this).find('.DebitMoney')[0]).attr('data-val');
                    data.Money_Credit = $($(this).find('.CreditMoney')[0]).attr('data-val');
                    data.Seq = index;
                    checkRowData(data);
                    saveData.push(data);
                } catch (e) {

                }

            });
            res.Data = saveData;
            var head = new TKS_FAS_Doc();
            head.PZDate = $('#txtPZDate').val();
            if (head.PZDate == '') {
                $.warning('请选择凭证日期');
            }
            head.PZZ = $('#PZZ').val();
            head.PZZNO = $('#PZZNO').val();
            head.AppendNum = $('#txtAppendNo').val();
            head.AMT_DBT = getCreditAll();
            res.Head = head;
            return res;

        };

        this.trans2Show = trans2Show;

        var appendRowTo = function (obj) {
            var html = genRow({});
            $(html).insertAfter(obj);
            var tr = $(obj).next('tr');

            bindHover(tr);
            bindZY(tr.find('.zyInput'));
            bindKM(tr.find('.DisplayTextKemu'));
            bindAutocomplete(tr.find(".kmInput"), subjectData);
            bindDebit(tr.find('.DebitMoney'));
            bindDebitDisplay(tr.find('.DisplayMoneyVal-d'));
            bindCredit(tr.find('.CreditMoney'));
            bindCreditDisplay(tr.find('.DisplayMoneyVal-c'));
        };

        var removeRow = function (obj) {
            var count = $("tbody tr").length;
            if (count <= 2)
                return;
            $(obj).remove();
            count = $("tbody tr").length;
            if (count == 3) {
                var last = $("tbody tr:last");
                appendRowTo(last);
            }
            //重计算金额
            var debit = getDebitAll();
            var credit = getCreditAll();
            var debitUper = digitUppercase(debit);
            $('#debitAllUper').html(debitUper);
            var val = trans2Show(debit);
            $('#txtDebitAll').text(val);
            val = trans2Show(credit);
            $('#txtCreditAll').text(val);


        };
    
        var SelSubject = function (obj) {
            
            //var a = currentTr;
          
            $.dialog("科目选择", 'kmChoose.aspx');


        };
        $('#calHelper').click(function () {
            parent.layer.open({
                type: 2,
                title: '辅助核算',
                shade: 0.1,
                area: ['1200px', '600px'],
                //content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=CHANGE&money=" + money + "&key=" + id,
                content: '/view/fas/set/calHelper/calHelperList.aspx',
                cancel: function (index, layer) {

                    //query(1);

                },
                end: function () {

                }
            });
        });
        $('#addSub').click(function () {
            AddSubject();
        });
        var AddSubject = function () {
            parent.layer.open({
                type: 2,
                title: '新增科目',
                shade: 0.1,
                area: ['1200px', '600px'],
                //content: '/view/fas/pz/pzEditor.aspx?tplid=' + tplId + "&type=CHANGE&money=" + money + "&key=" + id,
                content: '/view/fas/set/subject/subjectAddForPz.aspx',
                cancel: function (index, layer) {

                    //query(1);

                },
                end: function () {
                    window.clearActiveValue = editor.clearActiveValue;
                    window.setActiveValue = editor.setActiveValue;
                    window.setTPL = setTPL;
                    //bindEvents();
                    var request = {};
                    request.Token = token;
                    var index = $.loading('初始化中');
                    $.Post("/fas/set/subjectTotalGet", request,
                        function (data) {
                            var res = data;
                            layer.close(index);
                            if (!res.IsSuccess) {
                                $.warning(res.Message);
                            }
                            else {
                                subjectData = res.Data;
                                bindAutocomplete($(".kmInput"), res.Data);
                                $.topTip(res.Message);
                            }


                        }, function (err) {

                            layer.close(index);
                            $.warning(err.Message);
                        });
                }
            });
        }
        //借贷平衡
        var checkJDPH = function () {
            
            var debit = 0;
            $(ed).find('.DebitMoney').each(function () {
                var money = $($(this)[0]).attr('data-val');
                if ($.trim(money) != '') {
                    debit += parseFloat(money)
                }
            });
            var credit = 0;
            $(ed).find('.CreditMoney').each(function () {
                var money = $($(this)[0]).attr('data-val');
                if ($.trim(money) != '') {
                    credit += parseFloat(money)
                }
            });

            if (fomatFloat(credit, 2) != fomatFloat(debit, 2)) {
                return false;
            }
            else {
                return true;
            }
        };
        var checkJieDai = function () {
            var f=0;
            $(ed).each(function () {
                var DebitMoney = $(this).find('.DebitMoney').attr('data-val');
                var CreditMoney = $(this).find('.CreditMoney').attr('data-val');
                var money = $($(this)[0]).attr('data-val');
                if (DebitMoney != '' && CreditMoney != '') {
                    f= 1;
                }
            });

            if (f == 1) {
                return false;
            }
            else {
                return true;
            }
        };
        var checkRowData = function (data) {
            if (data.Money_Credit != '' || data.Money_Debit != '') {
                if (data.Summary == '') {
                    $.warning('亲，请填写摘要');
                    throw '';
                }
                if (data.SubjectCode == '') {
                    $.warning('亲，请选择科目');
                    throw '';
                }
            }
        };

        var getDebitAll = function () {
            var debit = 0;
            $(ed).find('.DebitMoney').each(function () {
                var money = $($(this)[0]).attr('data-val');
                if ($.trim(money) != '') {
                    debit += parseFloat(money);
                }
            });
            return fomatFloat(debit, 2);
        };
        var getCreditAll = function () {
            var credit = 0;
            $(ed).find('.CreditMoney').each(function () {
                var money = $($(this)[0]).attr('data-val');
                if ($.trim(money) != '') {
                    credit += parseFloat(money);
                }
            });
            return fomatFloat(credit, 2);
        };
        function fomatFloat(src, pos) {
            return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
        }
        //输入的值是否有效
        var isValueValid = function () {

            if (firstVal === secondVal) return true;

            if (activeSubjectElem != null || activeSubjectElem != undefined) {
                return true;
            }
            else {
                return false;
            }
        };
        //清除激活容器的值
        this.clearActiveValue = function () {
            $(activeSubjectElem).val('');


            $(activeSubjectElem).parent().find('.DisplayTextKemu').html('');
        };



        //设置激活容器的值
        this.setActiveValue = function (data) {
            
            var html = getKMDes(data);
          
            $(activeSubjectElem).parent().find('.DisplayTextKemu').html(html);
            $(activeSubjectElem).html(data.SubjectDescription);
            unActive();
        };
        var clearValue = function (elem) {
            $(elem).val('');

            $(elem).parent().find('.DisplayTextKemu').html('');
        };
        //取消激活
        //var unActive = function () {
        //    activeSubjectElem = null;
        //};
        //激活容器
        //var active = function (elem) {
        //    activeSubjectElem = elem;
        //}
        //生成row
        var genRow = function (row) {
            var template = $.templates("#tpl-row");

            var dataHtml = template.render(row);


            return dataHtml;
        }
        function trans2Show(val) {
            var result = '';
            val = val + '';
            if ($.trim(val) == '')
                return '';
            var arr = val.split('.');
            if (arr.length == 2) {
                if (arr[1].length == 1) {
                    result = arr[0] + '' + arr[1] + '0';
                }
                else {
                    result = arr[0] + '' + arr[1];
                }
            }
            else if (arr.length == 1) {
                if (arr[0] != '0')
                    result = arr[0] + "00";
            }


            return result;
        }



        var bindHover = function (obj) {
            $(obj).hover(
                function () {
                    rowAdd.css({
                        display: 'block',
                        left: $(this).offset().left - 30,
                        top: $(this).offset().top + $(this).outerHeight() - 25
                    });
                    rowRemove.css({
                        display: 'block',
                        left: $(this).offset().left - 30,
                        top: $(this).offset().top + $(this).outerHeight() - 60
                    });
                    rowAddSub.css({
                        display: 'block',
                        left: $(this).offset().left + 485+42,
                        top: $(this).offset().top + $(this).outerHeight() - 25
                    });
                    currentTr = this;

                },
                function () {
                    //rowAdd.hide();
                }
            );
        };

        var bindZY = function (obj) {
            //新增凭证里如果上方借贷平衡，则下一项摘要无内容，如不平，则摘要重复上一项内容 update by Hero.Zhang 20180319
            //$(obj).click(function () {
            //    var val = $(this).val();
            //    if ($.trim(val) == '') {
            //        $(this).val("");
            //        var flag = checkJDPH();
            //        if (!flag) {
            //            $(this).val(zy);
            //        }
            //        //$(this).val(zy);
            //    }

            //});
            $(obj).focus(function () {
                var flag = checkJDPH();
                if (!flag) {
                    var val = $(this).val();
                    if ($.trim(val) == '') {
                        $(this).val(zy);
                    }
                }
                //else {
                //    $(this).val('');
                //}
            });
            $(obj).blur(function () {
                zy = $(this).val().replace(/^\s+|\s+$/g, '');
            });
        }
        var bindKM = function (obj) {
            $(obj).click(function () {
                $(this).hide();
                $(this).parent().find('.kmInput').focus();

                $(this).parent().find('.kmInput').autocomplete("search", "");
            });

        };

        var bindAutocomplete = function (obj, data) {

            $(obj).autocomplete({

                source: data,

                select: function (event, ui) {

                    dealSubject($(this), ui);
                }
            }).keyup(function (e) {
                if (e.keyCode == 13) {
                    
                    var subCode = $(this).val().replace(/[\r\n]/g, "");
                    var html = "";
                    if (subCode.split(' ').length > 1) {
                        
                    }
                    else {
                        $(this).html("");
                        $(this).parent().find('.DisplayTextKemu').html("");
                    }
                    
                    if (subjectData != undefined && subjectData.length > 0) {
                        
                        for (var i = 0; i < subjectData.length; i++) {
                            if (subCode == subjectData[i].Id) {
                                
                                active($(this));//激活当前容器
                                if (subjectData[i].IsCalHelperValid == 1 ||
                                    subjectData[i].IsCurrencyValid == 1 ||
                                    subjectData[i].IsQuantityValid == 1) {
                                    layer.open({
                                        type: 2,
                                        title: '辅助核算编辑',
                                        shade: 0.1,
                                        area: ['500px', '400px'],
                                        content: 'kmAss.aspx?id=' + subjectData[i].Id,
                                        cancel: function (index, layero) {
                                            $.warning('请保存辅助核算项');
                                            return false;
                                        }
                                    });
                                    $(this).blur();
                                   
                                    return false;
                                }
                                else {
                                    var data = {};
                                    data.SubjectCode = subjectData[i].Id;
                                    data.SubjectDescription = subjectData[i].value;
                                    data.IsCalHelper = 0;
                                    data.IsCurrency = 0;
                                    data.IsQuantity = 0;
                                    data.Balance = 0;
                                    html = getKMDes(data);
                                    $(this).html(data.SubjectDescription);
                                    $(this).parent().find('.DisplayTextKemu').html(html);
                                  
                                }

                            }
                        }
                       
                    }

                }
            }).blur(function () {
              

                secondVal = $(this).val();
                
                if (!isValueValid()) {

                    clearValue($(this));
                    unActive();

                }
                $(this).parent().find('.DisplayTextKemu').show();
            }).focus(function () {
                firstVal = $(this).val();
            })
        };

        var bindDebitDisplay = function (obj) {
            $(obj).click(function () {
                $(this).hide();
                var input = $(this).parent().find('.DebitMoney')[0];
                var val = $(input).attr('data-val');


                $(input).val(val);
                $(this).parent().find('.DebitMoney').focus();

            });
        };
        var bindDebit = function (obj) {
            $(obj).focus(function () {
               
                
                $(this).addClass("selectInput");
                $(this).parent().find('.DisplayMoneyVal-d').hide();
                $(this).val($(this).attr('data-val'));
                $(this).select();
            });
            $(obj).keyup(function () {
           
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
                $(this).attr('data-val', $amountInput.val());
                //清空贷方金额

                $(this).closest("tr").find(".DisplayDaiFang").html("");
                $(this).closest("tr").find(".CreditMoney").attr('data-val','');
            })
            $(obj).blur(function () {
                var $amountInput = $(this);
               
                var display = $(this).parent().find('.DisplayJieFang')[0];
                //var value = $(this).val() == "" ? $(this).attr('data-val') : $(this).val().replace(/\b(0+)/gi, "");
                var value = $(this).val() == "" ? $(this).attr('data-val')  :Number($(this).val());
                $(this).val('');
                $(this).attr('data-val', value);


                var debit = getDebitAll();//借方求和
                var debitUper = digitUppercase(debit);
                $('#debitAllUper').html(debitUper);
                var val = trans2Show(debit);
                $('#txtDebitAll').text(val);
                //最后一位是小数点的话，移除
                $amountInput.val(($amountInput.val().replace(/\.$/g, "")));
                val = trans2Show(value);

                $(display).html(val);
                $(this).parent().find('.DisplayMoneyVal-d').show();
                //$(this).val(varNum);
                $(this).removeClass("selectInput");


                var credit = getCreditAll();//贷方求和
                var val_credit = trans2Show(credit)
                $('#txtCreditAll').text(val_credit);
            });
        };

        var bindCreditDisplay = function (obj) {
            $(obj).click(function () {
                $(this).hide();
                var input = $(this).parent().find('.CreditMoney')[0];
                var val = $(input).attr('data-val');


                $(input).val(val);
                $(this).parent().find('.CreditMoney').focus();

            });
        };
        var bindCredit = function (obj) {
            $(obj).focus(function () {

               
                $(this).addClass("selectInput");
                $(this).parent().find('.DisplayMoneyVal-c').hide();
                $(this).val($(this).attr('data-val'));
                $(this).select();
            });
            $(obj).keyup(function () {

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
                $(this).attr('data-val', $amountInput.val());
                //清空借方金额
                
                if($amountInput.val() != "") {
                    $(this).closest("tr").find(".DisplayJieFang").html("");
                    $(this).closest("tr").find(".DebitMoney").attr('data-val', '');
                }
                
            })
            $(obj).blur(function () {
                var $amountInput = $(this);
                var display = $(this).parent().find('.DisplayDaiFang')[0];
                //var value = $(this).val() == "" ? $(this).attr('data-val') : $(this).val().replace(/\b(0+)/gi, "");
                var value = $(this).val() == "" ? $(this).attr('data-val') : Number($(this).val());
                $(this).val('');
                $(this).attr('data-val', value);
                var credit = getCreditAll();//贷方求和
                var val = trans2Show(credit)
                $('#txtCreditAll').text(val);
                //最后一位是小数点的话，移除
                $amountInput.val(($amountInput.val().replace(/\.$/g, "")));
                val = trans2Show(value);
                $(display).html(val);
                $(this).parent().find('.DisplayMoneyVal-c').show();
                $(this).removeClass("selectInput");

                var debit = getDebitAll();//借方求和
                var debitUper = digitUppercase(debit);
                $('#debitAllUper').html(debitUper);
                var val_debit = trans2Show(debit);
                $('#txtDebitAll').text(val_debit);
            });
        }
        //绑定事件
        var bindEvents = function (msg) {

            bindZY($('.zyInput'))
            bindKM($('.DisplayTextKemu'));
            bindHover($("tbody tr"));
            rowAdd.click(function () {

                appendRowTo(currentTr);

            });
            rowRemove.click(function () {

                removeRow(currentTr);

            });
            rowAddSub.click(function () {

                //AddSubject(currentTr);
                SelSubject(currentTr);

            });

            var availableTags = [
                { label: "tx提现", value: "提现" },
                { label: "lxsr利息收入", value: "利息收入" },
                { label: "zfyhsxf支付银行手续费", value: "支付银行手续费" },

            ];
            $(".zyInput").autocomplete({
                source: availableTags,

                select: function (event, ui) {
                    console.log(ui);
                }
            });

            var request = {};
            request.Token = token;
            var index = $.loading('初始化中');
            $.Post("/fas/set/subjectTotalGet", request,
                function (data) {
                    
                    var res = data;
                    layer.close(index);
                    if (!res.IsSuccess) {
                        $.warning(res.Message);
                    }
                    else {
                        subjectData = res.Data;
                        
                        bindAutocomplete($(".kmInput"), res.Data);
                        if (msg != "") {
                            $.warning(msg);                         
                        }
                        else {
                            $.topTip(res.Message);
                        }
                       
                    }


                }, function (err) {

                    layer.close(index);
                    $.warning(err.Message);
                });

            bindDebit($('.DebitMoney'));
            bindDebitDisplay($('.DisplayMoneyVal-d'));
            bindCredit($('.CreditMoney'));
            bindCreditDisplay($('.DisplayMoneyVal-c'));

            //$("input,textarea").change(function () {

            //    setBtnEnabledSave();
            //});
            $("input,textarea").focus(function () {

                setBtnEnabledSave();
            });
        };

        //select 选中科目后处理
        var dealSubject = function (elem, d) {
            
            active(elem);//激活当前容器
            if (d.item.IsCalHelperValid == 1 ||
                d.item.IsCurrencyValid == 1 ||
                d.item.IsQuantityValid == 1) {
                layer.open({
                    type: 2,
                    title: '辅助核算编辑',
                    shade: 0.1,
                    area: ['500px', '400px'],
                    content: 'kmAss.aspx?id=' + d.item.Id,
                    cancel: function (index, layero) {
                        $.warning('请保存辅助核算项');
                        return false;
                    }
                });

            }
            else {
                var data = {};
                data.SubjectCode = d.item.Id;
                data.SubjectDescription = d.item.value;
                data.IsCalHelper = 0;
                data.IsCurrency = 0;
                data.IsQuantity = 0;
                data.Balance = 0;
                var html = getKMDes(data);
                $(elem).parent().find('.DisplayTextKemu').html(html);
                $(elem).html(data.SubjectDescription);
            }
        };
        //获取科目描述模板
        var getKMDes = function (data) {
            var template = $.templates("#tpl-kmdes");

            var dataHtml = template.render(data);


            return dataHtml;
        }

        var index = 0;
        $(document).keyup(function (e) {
            if (e.keyCode == 13) {

                e.preventDefault();
                if ($(e.target).hasClass("zyInput")) {
                    $(e.target).parents("tr").find('.DisplayTextKemu').click();

                }
                if ($(e.target).hasClass("kmInput")) {
                    
                    $(e.target).parents("tr").find(".DebitMoney").focus();
                }
                if ($(e.target).hasClass("DebitMoney")) {
                    $(e.target).parents("tr").find(".CreditMoney").focus();
                }
                if ($(e.target).hasClass("CreditMoney")) {
                    var count = $("tbody tr").length;
                    index = (e.target).parentNode.parentNode.parentNode.rowIndex;
                    if (index >= count) {
                        //新增一行
                        var last = $("tbody tr:last");
                        appendRowTo(last);
                    }
                    $(e.target).parents("tr").next(":eq(0)").find(".zyInput").focus();
                }
                //var index1 = (e.target).parentNode.parentNode.rowIndex;
                //alert((e.target).parentNode.parentNode.rowIndex);
            }
            
            if (e.keyCode == 187) {
                e.preventDefault();
                if ($(e.target).hasClass("DebitMoney")) {
                   
                    if ($(e.target).parents("tr").find(".DebitMoney").attr("data-val") == "" && $(e.target).parents("tr").find(".CreditMoney").attr("data-val") == "") {
                        var debit = getDebitAll();//借方求和
                        var credit = getCreditAll();
                        if (debit != credit) {
                            if (debit > credit) {
                                $(e.target).parents("tr").find(".DebitMoney").val(-FloatSub(debit, credit));

                                //$(e.target).parents("tr").find(".CreditMoney").focus();
                                //$(e.target).parents("tr").find(".CreditMoney").val(FloatSub(debit, credit));
                            }
                            else {
                                $(e.target).parents("tr").find(".DebitMoney").val(FloatSub(credit, debit));
                            }
                           // $(e.target).parents("tr").find(".CreditMoney").focus();
                        }
                        
                    }
                   
                    
                }
                if ($(e.target).hasClass("CreditMoney")) {

                    if ($(e.target).parents("tr").find(".DebitMoney").attr("data-val") == "" && $(e.target).parents("tr").find(".CreditMoney").attr("data-val") == "") {
                        var debit = getDebitAll();//借方求和
                        var credit = getCreditAll();
                      
                        if (debit != credit) {
                            if (debit > credit) {
                                $(e.target).parents("tr").find(".CreditMoney").focus(); 
                                $(e.target).parents("tr").find(".CreditMoney").val(FloatSub(debit, credit));
                            }
                            else {
                                $(e.target).parents("tr").find(".CreditMoney").val(-FloatSub(credit, debit));

                                //$(e.target).parents("tr").find(".DebitMoney").focus();
                                //$(e.target).parents("tr").find(".DebitMoney").val(FloatSub(credit, debit));
                      
                            }
                           
                        }

                    }
                    
                }
            }
            if (e.keyCode == 123) {
                //alert("111");
                //return false
            }
        });

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
    window.onkeydown = window.onkeyup = window.onkeypress = function (event) {
        // 判断是否按下F12，F12键码为123
        if (event.keyCode == 123) {
            //F12 保存并新增
            save('123');
            event.preventDefault(); // 阻止默认事件行为
            window.event.returnValue = false;
            return false;
        }
        if (event.keyCode == 83 && event.ctrlKey) {
            //ctrl+S 保存
            save('');
            event.preventDefault(); // 阻止默认事件行为
            window.event.returnValue = false;
            return false;
        }
    }
    var digitUppercase = function (n) {
        var fraction = ['角', '分'];
        var digit = [
            '零', '壹', '贰', '叁', '肆',
            '伍', '陆', '柒', '捌', '玖'
        ];
        var unit = [
            ['元', '万', '亿'],
            ['', '拾', '佰', '仟']
        ];
        var head = n < 0 ? '欠' : '';
        n = Math.abs(n);
        var s = '';
        for (var i = 0; i < fraction.length; i++) {
            s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
        }
        s = s || '整';
        n = Math.floor(n);
        for (var i = 0; i < unit[0].length && n > 0; i++) {
            var p = '';
            for (var j = 0; j < unit[1].length && n > 0; j++) {
                p = digit[n % 10] + unit[1][j] + p;
                n = Math.floor(n / 10);
            }
            s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
        }
        return head + s.replace(/(零.)*零元/, '元')
            .replace(/(零.)+/g, '零')
            .replace(/^整$/, '零元整');
    };

    function TKS_FAS_DocDetail() {
        this.Id = '',
            this.ParentId = '',
            this.LineNo = '',
            this.AccountId = '',
            this.Year = '',
            this.SubjectCode = '',
            this.SubjectDescription = '',
            this.Summary = '',
            this.Credit_Debit = '',
            this.PartnerCode = '',
            this.Unit = '',
            this.CurrencyCode = '',
            this.CalValue1 = '',
            this.Rate = '',
            this.Quantity = '',
            this.Price = '',
            this.IsCurrency = '',
            this.IsQuantity = '',
            this.IsCalHelper = '',
            this.Balance = '',
            this.Money_Debit = '',
            this.Money_Credit = ''

    }

    function TKS_FAS_Doc() {
        this.Id = '',
            this.PZZ = '',
            this.PZZNO = '',
            this.PeriodId = '',
            this.AccountId = '',
            this.Year = '',
            this.PZDate = '',
            this.AppendNum = '',
            this.CheckMan = '',
            this.CheckStatus = '',
            this.AMT_DBT = ''


    }


    window.clearActiveValue = editor.clearActiveValue;
    window.setActiveValue = editor.setActiveValue;
    window.setTPL = setTPL;
    window.selectSubject = selectSubject;
    var index = 0;
    $("#kj").hover(function () {
        openMsg();
    }, function () {
        layer.close(subtips);
    });
    function openMsg() {
        subtips = layer.tips('保存并新增：F12<br/>保存：Ctrl+S<br/>自动平衡借贷金额：=', '#kj', { tips: [1, '#009688']});
    }

    //$(document).keyup(function (e) {
    //    if (e.keyCode == 13) {

    //        e.preventDefault();
    //        if ($(e.target).hasClass("zyInput")) {
    //            index = (e.target).parentNode.parentNode.rowIndex;
    //            $(e.target).parents("tr").find('.DisplayTextKemu').click();
    //        }
    //        if ($(e.target).hasClass("kmInput")) {
    //            $(e.target).parents("tr").find(".DebitMoney").focus();
    //        }
    //        if ($(e.target).hasClass("DebitMoney")) {
    //            $(e.target).parents("tr").find(".CreditMoney").focus();
    //        }
    //        if ($(e.target).hasClass("CreditMoney")) {
    //            var count = $("tbody tr").length;
    //            if (index >= count) {
    //                var editor = new PZEditor('editor');
    //                //editor.init([]);
    //                //新增一行
    //                //var obj = $("tbody tr:last");
    //                //var html = genRow({});
    //                //$(html).insertAfter(obj);
    //                //var tr = $(obj).next('tr');

    //                //bindHover(tr);
    //                //bindZY(tr.find('.zyInput'));
    //                //bindKM(tr.find('.DisplayTextKemu'));
    //                //bindAutocomplete(tr.find(".kmInput"), subjectData);
    //                //bindDebit(tr.find('.DebitMoney'));
    //                //bindDebitDisplay(tr.find('.DisplayMoneyVal-d'));
    //                //bindCredit(tr.find('.CreditMoney'));
    //                //bindCreditDisplay(tr.find('.DisplayMoneyVal-c'));
    //            }
    //            $(e.target).parents("tr").next(":eq(0)").find(".zyInput").focus();
    //        }
    //        //var index1 = (e.target).parentNode.parentNode.rowIndex;
    //        //alert((e.target).parentNode.parentNode.rowIndex);
    //    }
    //});
})
