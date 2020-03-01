
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'JsRender', 'jqExt'], function () {
    var form = layui.form(),
		layer = layui.layer,
		laypage = layui.laypage;
    var $ = layui.jquery;

    var id = $.getQueryString('Id');
    var type = $.getQueryString('type');
    var editor = new PZEditor('editor');
    window.clearActiveValue = editor.clearActiveValue;
    window.setActiveValue = editor.setActiveValue;
 
 

    var dataInit = function () {
        var request = {};
        request.Token = token;
       
        request.Data = { Id: id };
        var index = $.loading('获取数据');
        $.Post('/fas/tplmanage/tplMGet2', request,
                function (data) {
                    var res = data;
                    layer.close(index);
                    if (!res.IsSuccess) {
                        $.warning(res.Message);
                    }
                    else {
                      
                        $('#txtTitle').val(res.Head.Title);
                        $('#selTPLTarget').val(res.Head.TPLTarget);
                       // $('#selIsCarry').val(res.Head.IsCarry);
                        var m = digitUppercase(res.Head.AMT_DBT);
                        $('#debitAllUper').html(m);
                        $('#txtDebitAll').val(res.Head.AMT_DBT);
                        $('#txtDebitAll').val(res.Head.AMT_DBT);
                        editor.init(res.Detail);

                        if (type == 'custom') {
                            $('#txtTitle').attr('disabled', 'disabled');
                            $('#selTPLTarget').attr('disabled', 'disabled');
                            $('#btnAdd').hide();
                        }

                        form.render();
                        
                    }


                }, function (err) {

                    layer.close(index);
                    $.warning(err.Message);
                });
    }

    if (id != '' && id != null) {
        dataInit();
    }
    else {
        editor.init([]);
        form.render();

    }

    $('#btnAdd').click(function () {
        window.location.href = "tplEditor.aspx";
    });

    $('#btnSave').click(function () {
        var data = editor.getData();
        if (data == null)
        {
            return;
        }
        console.log(data);
        var url = '';
        if (id == "" || id == undefined) {
            url = '/fas/tplmanage/tplMAdd2';
        }
        else {
            url = '/fas/tplmanage/TPLMUpdate2';
        }
        var request = {};
        request.Token = token;
        request.Head = data.Head;
        request.Head.Id = id;
        request.Type = type;//自定义
        request.Detail = data.Data;
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
                         $.topTip(res.Message);
                     }


                 }, function (err) {

                     layer.close(index);
                     $.warning(err.Message);
                 });
    });

    function PZEditor(id) {

        var ed = $('#' + id);
        var rowAdd = $(' <a class="layui-btn layui-btn-mini rowAdd"><i class="layui-icon">&#xe654;</i></a>').appendTo('body');
        var rowRemove = $(' <a class="layui-btn layui-btn-mini rowRemove"><i class="layui-icon">&#xe640;</i></a>').appendTo('form');
        var currentTr = {};
        var activeSubjectElem;
        var subjectData;
        var saveData = new Array();
        var firstVal = '';
        var secondVal = '';
        //初始化
        this.init = function (data) {
            var html = '';
            for (var j = 0; j < data.length; j++) {
                html += genRow(data[j]);
            }

            for (var i = 0; i <4-data.length; i++) {
                html += genRow({});
            }
            $(ed).html(html);
            bindEvents();
        };


        this.getData = function () {

         
            var res = {};
            saveData = new Array();
            var flag = true;
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

                    var debitEl = $(this).find('.debitFlag')[0];
                    var creditEl = $(this).find('.creditFlag')[0];
                    if ($(debitEl).find('.layui-form-checked').length > 0&&
                        $(creditEl).find('.layui-form-checked').length > 0) {
                        $.warning('亲，借贷只能勾选一个');
                        flag = false;
                        return false;
                    }

                    if ($(debitEl).find('.layui-form-checked').length > 0)
                    {
                        data.Credit_Debit = 0;
                    }
                    else
                    {
                        data.Credit_Debit = 1;
                    }

                    //发生额比例
                    data.IPercent = $(this).find('.IPercent')[0].value;
                    if (data.IPercent == '') {
                        data.IPercent = 1;//默认为1
                    }
                    //值来源类型
                    data.SourceType = $(this).find('.SourceType')[0].value
                    if (data.SourceType == '') {
                        data.IPercent = 0;//默认为 发生额比例
                    }
                   
                    saveData.push(data);
                } catch (e) {

                }

            });

            if (!flag) return null;

            res.Data = saveData;
            var head = new TKS_FAS_Doc();
            head.Title = $('#txtTitle').val();
 
            head.TPLTarget = $('#selTPLTarget').val();
           // head.IsCarry = $('#selIsCarry').val();
            res.Head = head;
            return res;
        };



        var appendRowTo = function (obj) {
            var html = genRow({});
            $(html).insertAfter(obj);
            var tr = $(obj).next('tr');
            bindHover(tr);
            bindKM(tr.find('.DisplayTextKemu'));
            bindAutocomplete(tr.find(".kmInput"), subjectData);
            form.render();
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
        };
        //借贷平衡
        var checkJDPH = function () {
            var debit = 0;
            $(ed).find('.DebitMoney').each(function () {
                var money = $(this)[0].value;
                if ($.trim(money) != '') {
                    debit += parseFloat(money)
                }
            });
            var credit = 0;
            $(ed).find('.CreditMoney').each(function () {
                var money = $(this)[0].value;
                if ($.trim(money) != '') {
                    credit += parseFloat(money)
                }
            });
            if (credit != debit) {
                return false;
            }
            else {
                return true;
            }
        };

        

        var getDebitAll = function () {
            var debit = 0;
            $(ed).find('.DebitMoney').each(function () {
                var money = $(this)[0].value;
                if ($.trim(money) != '') {
                    debit += parseFloat(money)
                }
            });
            return debit;
        };
        var getCreditAll = function () {
            var credit = 0;
            $(ed).find('.CreditMoney').each(function () {
                var money = $(this)[0].value;
                if ($.trim(money) != '') {
                    credit += parseFloat(money)
                }
            });
            return credit;
        };
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
            unActive();
        };
        var clearValue = function (elem) {
            $(elem).val('');

            $(elem).parent().find('.DisplayTextKemu').html('');
        };
        //取消激活
        var unActive = function () {
            activeSubjectElem = null;
        };
        //激活容器
        var active = function (elem) {
            activeSubjectElem = elem;
        }
        //生成row
        var genRow = function (row) {
            var template = $.templates("#tpl-row");

            var dataHtml = template.render(row);


            return dataHtml;
        }
        var bindHover = function (obj) {
            $(obj).hover(
            function () {
                rowAdd.css({
                    display: 'block',
                    left: $(this).offset().left - 30,
                    top: $(this).offset().top + $(this).outerHeight() - 5
                });
                rowRemove.css({
                    display: 'block',
                    left: $(this).offset().left - 30,
                    top: $(this).offset().top + $(this).outerHeight() - 60
                });
                currentTr = this;

            },
            function () {
                //rowAdd.hide();
            }
          );
        };

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
            }).blur(function () {
                secondVal = $(this).val();
                if (!isValueValid()) {

                    clearValue($(this));
                    unActive();

                }
                $(this).parent().find('.DisplayTextKemu').show();

            }).focus(function () {
                firstVal = $(this).val();
            });
        };
        var bindDebit = function (obj) {
            $(obj).blur(function () {
                var debit = getDebitAll();
                var debitUper = digitUppercase(debit);
                $('#debitAllUper').html(debitUper);
                $('#txtDebitAll').val(debit);
            });
        };
        var bindCredit = function (obj) {
            $(obj).blur(function () {
                var credit = getCreditAll();
                $('#txtCreditAll').val(credit);
            });
        }
        //绑定事件
        var bindEvents = function () {

            bindKM($('.DisplayTextKemu'));
            bindHover($("tbody tr"));
            rowAdd.click(function () {

                appendRowTo(currentTr);

            });
            rowRemove.click(function () {

                removeRow(currentTr);

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
            $.Post("/fas/set/tplsubjectTotalGet", request,
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

            bindDebit($('.DebitMoney'));

            bindCredit($('.CreditMoney'));
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

            }
        };
        //获取科目描述模板
        var getKMDes = function (data) {
            var template = $.templates("#tpl-kmdes");

            var dataHtml = template.render(data);


            return dataHtml;
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
        this.Money_Credit = '',
        this.IPercent = 1;
        this.SourceType = 0;

    }

    function TKS_FAS_Doc() {
        this.Id = '',
		this.Title = '',
		this.Type = ''
	 


    }
})
