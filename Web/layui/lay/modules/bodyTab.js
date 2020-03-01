
var tabFilter, menu = [], liIndex, curNav, delMenu;
layui.define(["element", "jquery"], function (exports) {
    var element = layui.element(),
		$ = layui.jquery,
		layId,
		Tab = function () {
		    this.tabConfig = {
		        closed: true,
		        openTabNum: 10,
		        tabFilter: "bodyTab"
		    }
		};

    //显示左侧菜单


    Tab.prototype.init = function (data) {
        $(".navBar").html(navBar(data)).height($(window).height() - 230);
        element.init();  //初始化页面元素
        $(window).resize(function () {
            $(".navBar").height($(window).height() - 230);
        })
    };
    //参数设置
    Tab.prototype.set = function (option) {
        var _this = this;
        $.extend(true, _this.tabConfig, option);
        return _this;
    };

    //通过title获取lay-id
    Tab.prototype.getLayId = function (title) {
        $(".layui-tab-title.top_tab li").each(function () {
            if ($(this).find("cite").text() == title) {
                layId = $(this).attr("lay-id");
            }
        })
        return layId;
    }
    //通过title判断tab是否存在
    Tab.prototype.hasTab = function (title) {
        var tabIndex = -1;
        $(".layui-tab-title.top_tab li").each(function () {
            if ($(this).find("cite").text() == title) {
                tabIndex = 1;
            }
        })
        return tabIndex;
    }

    //右侧内容tab操作
    var tabIdIndex = 0;
    Tab.prototype.tabAdd = function (_this) {
        
        if (_this.attr("data-url") == undefined)
            return;
        var that = this;
        var closed = that.tabConfig.closed,
			openTabNum = that.tabConfig.openTabNum;
        tabFilter = that.tabConfig.tabFilter;
        // $(".layui-nav .layui-nav-item a").on("click",function(){

        var title = '';
        if (that.hasTab(_this.find("cite").text()) == -1 && _this.siblings("dl.layui-nav-child").length == 0) {
            if ($(".layui-tab-title.top_tab li").length == openTabNum) {
                layer.msg('只能同时打开' + openTabNum + '个选项卡哦。不然系统会卡的！');
                return;
            }
            tabIdIndex++;
            if (_this.find("i.iconfont").attr("data-icon") != undefined) {
                title += '<i class="iconfont ' + _this.find("i.iconfont").attr("data-icon") + '"></i>';
            } else if (_this.find("i.layui-icon").attr("data-icon") != undefined) {
                title += '<i class="layui-icon">' + _this.find("i.layui-icon").attr("data-icon") + '</i>';
            }
            else {
                title += '<i class="layui-icon">&#xe638;</i>';
            }
            title += '<cite>' + _this.find("cite").text() + '</cite>';
            title += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + tabIdIndex + '">&#x1006;</i>';
            element.tabAdd(tabFilter, {
                title: title,
                content: "<iframe src='" + _this.attr("data-url") + "' data-id='" + tabIdIndex + "'></frame>",
                id: new Date().getTime()
            })

        
            element.tabChange(tabFilter, that.getLayId(_this.find("cite").text()));
        } else {

            element.tabChange(tabFilter, that.getLayId(_this.find("cite").text()));
        }

        // })
    }
    $("body").on("click", ".top_tab li", function () {
       

        element.tabChange(tabFilter, $(this).attr("lay-id")).init();
    })

    //删除tab
    $("body").on("click", ".top_tab li i.layui-tab-close", function () {
        
        element.tabDelete("bodyTab", $(this).parent("li").attr("lay-id")).init();
    })

    var bodyTab = new Tab();
    exports("bodyTab", function (option) {
        return bodyTab.set(option);
    });
})