﻿(function ($) {
    var tlwDraw = {
        dataUrl: "http://localhost:5000/api/lottery",
        //是否在动画中
        animation: false,
        //抽奖界面ID
        drawId: 0,
        //人员名单
        personnelData: null,
        //奖品数据
        awardsData: null,
        //当前抽奖信息
        awardStyle: null,
        //剩余奖品
        residualAwards: null,
        //抽奖历史数据
        drawHistory: null,
        //设置中奖历史记录
        setDrawHistory: function () {
            if (!this.drawId) return;
            $.ajaxSettings.async = false;
            $.getJSON(this.dataUrl + "/" + this.drawId, function (json) {
                if (!json || !json.winners) return;
                tlwDraw.drawHistory = json.winners;
                tlwDraw.setResidualAwards();
            });
            $.ajaxSettings.async = true;
        },
        //中奖历史展示
        drawHistoryShow: function () {
            var $historyDiv = $("#draw-historyDiv"), awardsData = tlwDraw.awardsData, drawHistory = this.drawHistory;
            if (!awardsData || !$historyDiv.length || $.isEmptyObject(drawHistory)) return;
            $historyDiv.empty();
            $.each(awardsData, function (i, item) {
                var $div = $("<div class='historyDiv-item " + item.id + "'></div>");
                if ($.isEmptyObject(drawHistory[item.name])) return;
                $div.append("<span class='title'>" + item.name + "：</span>").append("<div class='items'>"+tlwDraw.getUserInfo(drawHistory[item.name])+"</div>");
                $historyDiv.append($div);
                return;
            });
        },
        //设置剩余奖品
        setResidualAwards: function () {
            var awardsData = tlwDraw.awardsData, drawHistory = this.drawHistory, residualAwards=new Array();
            if (!awardsData) return;
            $.each(awardsData, function (i, item) {
                residualAwards.push({ id: item.id, name: item.name, number: item.number });
                if ($.isEmptyObject(drawHistory[item.name])) return;
                residualAwards[i].number -= drawHistory[item.name].length;
                return;
            });
            this.residualAwards = (residualAwards.length>0?residualAwards:null);
        },
        //更新奖项剩余奖品数
        updateResidualAwards: function () {
            var residualAwards = this.residualAwards;
            if (!residualAwards) return;
            $.each(residualAwards, function (i, item) {
                $("#awards-" + item.id + ",.big-span." + item.id).find(".s-num").html(item.number);
            });
        },
        //奖品内容显示
        awardsShow: function (state) {
            var i = 0, bool = true, num = 1;
            this.awardStyle = "";
            switch (state) {
                case "one":
                    //特等奖
                    i = 0;
                    num = 1;
                    break;
                case "two":
                    //一等奖
                    i = 1;
                    num = 1;
                    break;
                case "three":
                    //二等奖
                    i = 2;
                    num = 5;
                    break;
                case "four":
                    //三等奖
                    i = 3;
                    bool = false;
                    num = 10;
                    break;
                case "five":
                    //四等奖
                    i = 4;
                    num = 25;
                    bool = false;
                    break;
                default:
                    return;
            }
            var awardsData = this.awardsData;
            if (!awardsData || !awardsData.length) return;
            var $result = $("#draw-result"), $awards = $("#draw-awards"), css = "big";
            $result.empty();
            if (!$result.length || !$awards.length) return;
            function setAwards(data) {
                $awards.html("&lt; " + data.name + "奖品<span>" + data.meed + "</span> &gt;");
            }
            setAwards(awardsData[i]);
            this.awardStyle = state + "/" + num;
            if (bool)
                $result.addClass(css);
            else
                $result.removeClass(css);
            $("#big-span").attr("class", "big-span " + state).find(".s-num").html(this.residualAwards[i].number)
        },
        //人员名单显示HTML结构
        getUserInfo: function (data) {
            var result = "";
            $.each(data, function (i, item) {
                //" + (item.isAbsent ? " absent" : "") + "
                result += "<span class=\"item\" title='" + item.name + "/" + item.department + "'><span class=\"name\">" + item.name + "</span><span class=\"dept\">/" + item.department + "</span></span>";
            });
            return result;
        },
        //全部人员名单显示
        personnelShow: function () {
            var personnelData = this.personnelData;
            if (!personnelData || !personnelData.length) return;
            $("#draw-marquee").html(this.getUserInfo(this.personnelData));
        },
        //抽奖滚动开始及暂停
        drawOnOff: function (obj, state) {
            //开始 暂停
            var $info = $("#draw-info"), $btn = $(obj), css = "off", css2 = "select";
            if (this.animation) return;
            this.animation = true;
            $btn.siblings("input[type='button']").removeClass(css2);
            switch (state) {
                case "on":
                    $info.removeClass(css);
                    drawMarquee.start();
                    setTimeout(function () { tlwDraw.animation = false; }, 500);
                    break;
                case "off":
                    var $result = $("#draw-result"), drawId = this.drawId, awardStyle = this.awardStyle;
                    if (!drawMarquee.st || !drawId || !awardStyle || !$result.length) {
                        tlwDraw.animation = false;
                        return;
                    }
                    drawMarquee.stop();
                    var url = this.dataUrl + "/" + drawId + "/", txtNum = $("#draw-add-text").val();
                    if ($("html").hasClass("draw-add")) {
                        if (txtNum > 5) {
                            $("#draw-result").removeClass("big");
                        } else {
                            $("#draw-result").addClass("big");
                        }
                        url += (awardStyle.split("/")[0] + "/" + (txtNum ? txtNum : 1) + "/true");
                    }
                    else
                        url += awardStyle;
                    $.getJSON(url, function (json) {
                        if (!json || !json.persons) return;
                        var data = json;
                        $result.html(tlwDraw.getUserInfo(data.persons));
                        tlwDraw.setDrawHistory();
                        tlwDraw.updateResidualAwards();
                        setTimeout(function () { $info.addClass(css); }, 100);
                        //$result.hasClass('big') ? 9000 : 5000
                        setTimeout(function () { tlwDraw.animation = false; }, 5000);
                    });

                    break;
            }
            $btn.addClass(css2);
        },
        //返回奖项选择
        returnIndex: function () {
            if (this.animation) return;
            $(".draw-start").removeClass("draw-start");
            $(".draw-add").removeClass("draw-add");
            $(".draw-history").removeClass("draw-history");
            $(".off").removeClass("off");
            $(".draw-btn.select").removeClass("select");
            drawMarquee.stop();
        },
        //创建奖项按钮
        createBtns: function () {
            var awardsData = this.awardsData, $btns = $("#draw-btns2"), $btn; 
            if (!awardsData || !awardsData.length || !$btns.length) return;
            $btns.empty();
            //奖项选择按钮
            function addBtn(data) {
                $btn = $("<span class='draw1 small-span " + data.id + "' id='awards-" + data.id + "'><span class='s-num'>" + data.number + "</span></span>");
                $btns.append($btn);
                $btn.click(function () {
                    if (parseInt($(this).find(".s-num").text())<=0) return;
                    tlwDraw.awardsShow(data.id);
                    $("html").addClass("draw-start");
                });
                return $btn;
            }
            $.each(awardsData, function (i, item) {
                addBtn(item);
            });
            //补抽界面显示按钮
            $("#draw-add-btn").click(function () {
                $("html").addClass("draw-add");
                if (!tlwDraw.awardStyle) return;
                $("#draw-add-dll").combobox("setValue", tlwDraw.awardStyle.split("/")[0]);
            });
            //补抽奖项的选择下拉框
            $("#draw-add-dll").combobox({
                data:tlwDraw.awardsData,
                valueField: 'id',
                textField: 'name',
                editable:false,
                onSelect: function (item) {
                    tlwDraw.awardsShow(item.id);
                }
            });
            $("#startBtn").click(function () { tlwDraw.drawOnOff(this, 'on'); });//开始按钮
            $("#stopBtn").click(function () { tlwDraw.drawOnOff(this, 'off'); });//暂停按钮
            $("#returnBtn").click(function () { tlwDraw.returnIndex(); });//返回首页按钮

            //中奖历史显示按钮
            var $drawHistoryBtn = $("<span class='draw-historyBtn'></span>");
            $(".draw-img").append($drawHistoryBtn);
            $drawHistoryBtn.click(function () {
                tlwDraw.drawHistoryShow();
                tlwDraw.returnIndex();
                $("html").addClass("draw-history");
            });
        },
        //设置DrawId Cookie值
        setDrawId: function (drawId) {
            var name = "lotteryId";
            document.cookie = name+"=" + escape(drawId);
        },
        //读取DrawId Cookie值
        getDrawId: function () {
            var arr, name = "lotteryId", reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        },
        //数据初始化
        init: function () {
            $.getJSON(this.dataUrl, function (json) {
                if (!json || !json.id) return;
                tlwDraw.drawId = tlwDraw.getDrawId();
                if (!tlwDraw.drawId) {
                    tlwDraw.setDrawId(json.id);
                    tlwDraw.drawId = json.id;
                }
                tlwDraw.personnelData = json.define.partners;
                tlwDraw.awardsData = json.define.awards;
                tlwDraw.personnelShow();
                tlwDraw.createBtns();
                tlwDraw.setDrawHistory();
                tlwDraw.updateResidualAwards();
                tlwDraw.awardsShow(tlwDraw.awardsData[0].id);
            });
        }
    };
    //滚动
    var drawMarquee = {
        st: null,
        start: function () {
            if (this.st) return;
            var $marquee = $("#draw-marquee"), $copy = $(".draw-marquee-copy"), speed = 10,
            len = 2, m_top, height = $marquee.height();
            if (!$copy.length) {
                $copy = $("<div class='draw-marquee-copy'></div>");
                $copy.html($marquee.html());
                $marquee.after($copy);
            }
            $marquee.addClass("on");
            drawMarquee.st = setInterval(function () {
                m_top = parseInt($marquee.css("margin-top")) - len;
                $marquee.css("margin-top", -m_top > height ? 0 : m_top);
            }, speed);
        },
        stop: function () {
            if (!this.st) return;
            clearInterval(this.st);
            this.st = null;
            $("#draw-marquee").removeClass("on").css("margin-top","");
        }
    }
    $(function () {
        tlwDraw.init();
    });
})(jQuery);