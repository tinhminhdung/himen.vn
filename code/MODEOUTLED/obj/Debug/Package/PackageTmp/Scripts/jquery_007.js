; (function ($) {
    var ver = "2.63"; if ($.support == undefined) { $.support = { opacity: !($.browser.msie) }; }
    function log() { if (window.console && window.console.log) { window.console.log("[cycle] " + Array.prototype.join.call(arguments, " ")); } }
    $.fn.cycle = function (options, arg2) {
        var o = { s: this.selector, c: this.context }; if (this.length == 0 && options != "stop") {
            if (!$.isReady && o.s) { log("DOM not ready, queuing slideshow"); $(function () { $(o.s, o.c).cycle(options, arg2); }); return this; }
            log("terminating; zero elements found by selector" + ($.isReady ? "" : " (DOM not ready)")); return this;
        }
        return this.each(function () {
            options = handleArguments(this, options, arg2); if (options === false) { return; }
            if (this.cycleTimeout) { clearTimeout(this.cycleTimeout); }
            this.cycleTimeout = this.cyclePause = 0; var $cont = $(this); var $slides = options.slideExpr ? $(options.slideExpr, this) : $cont.children(); var els = $slides.get(); if (els.length < 2) { log("terminating; too few slides: " + els.length); return; }
            var opts = buildOptions($cont, $slides, els, options, o); if (opts === false) { return; }
            if (opts.timeout || opts.continuous) { this.cycleTimeout = setTimeout(function () { go(els, opts, 0, !opts.rev); }, opts.continuous ? 10 : opts.timeout + (opts.delay || 0)); }
        });
    }; function handleArguments(cont, options, arg2) {
        if (cont.cycleStop == undefined) { cont.cycleStop = 0; }
        if (options === undefined || options === null) { options = {}; }
        if (options.constructor == String) {
            switch (options) {
                case "stop": cont.cycleStop++; if (cont.cycleTimeout) { clearTimeout(cont.cycleTimeout); }
                    cont.cycleTimeout = 0; $(cont).removeData("cycle.opts"); return false; case "pause": cont.cyclePause = 1; return false; case "resume": cont.cyclePause = 0; if (arg2 === true) {
                        options = $(cont).data("cycle.opts"); if (!options) { log("options not found, can not resume"); return false; }
                        if (cont.cycleTimeout) { clearTimeout(cont.cycleTimeout); cont.cycleTimeout = 0; }
                        go(options.elements, options, 1, 1);
                    }
                        return false; default: options = { fx: options };
            }
        } else {
            if (options.constructor == Number) {
                var num = options; options = $(cont).data("cycle.opts"); if (!options) { log("options not found, can not advance slide"); return false; }
                if (num < 0 || num >= options.elements.length) { log("invalid slide index: " + num); return false; }
                options.nextSlide = num; if (cont.cycleTimeout) { clearTimeout(this.cycleTimeout); cont.cycleTimeout = 0; }
                if (typeof arg2 == "string") { options.oneTimeFx = arg2; }
                go(options.elements, options, 1, num >= options.currSlide); return false;
            }
        }
        return options;
    }
    function removeFilter(el, opts) { if (!$.support.opacity && opts.cleartype && el.style.filter) { try { el.style.removeAttribute("filter"); } catch (smother) { } } }
    function buildOptions($cont, $slides, els, options, o) {
        var opts = $.extend({}, $.fn.cycle.defaults, options || {}, $.metadata ? $cont.metadata() : $.meta ? $cont.data() : {}); if (opts.autostop) { opts.countdown = opts.autostopCount || els.length; }
        var cont = $cont[0]; $cont.data("cycle.opts", opts); opts.$cont = $cont; opts.stopCount = cont.cycleStop; opts.elements = els; opts.before = opts.before ? [opts.before] : []; opts.after = opts.after ? [opts.after] : []; opts.after.unshift(function () { opts.busy = 0; }); if (!$.support.opacity && opts.cleartype) { opts.after.push(function () { removeFilter(this, opts); }); }
        if (opts.continuous) { opts.after.push(function () { go(els, opts, 0, !opts.rev); }); }
        saveOriginalOpts(opts); if (!$.support.opacity && opts.cleartype && !opts.cleartypeNoBg) { clearTypeFix($slides); }
        if ($cont.css("position") == "static") { $cont.css("position", "relative"); }
        if (opts.width) { $cont.width(opts.width); }
        if (opts.height && opts.height != "auto") { $cont.height(opts.height); }
        if (opts.startingSlide) { opts.startingSlide = parseInt(opts.startingSlide); }
        if (opts.random) {
            opts.randomMap = []; for (var i = 0; i < els.length; i++) { opts.randomMap.push(i); }
            opts.randomMap.sort(function (a, b) { return Math.random() - 0.5; }); opts.randomIndex = 0; opts.startingSlide = opts.randomMap[0];
        } else { if (opts.startingSlide >= els.length) { opts.startingSlide = 0; } }
        opts.currSlide = opts.startingSlide = opts.startingSlide || 0; var first = opts.startingSlide; $slides.css({ position: "absolute", top: 0, left: 0 }).hide().each(function (i) { var z = first ? i >= first ? els.length - (i - first) : first - i : els.length - i; $(this).css("z-index", z); }); $(els[first]).css("opacity", 1).show(); removeFilter(els[first], opts); if (opts.fit && opts.width) { $slides.width(opts.width); }
        if (opts.fit && opts.height && opts.height != "auto") { $slides.height(opts.height); }
        var reshape = opts.containerResize && !$cont.innerHeight(); if (reshape) {
            var maxw = 0, maxh = 0; for (var i = 0; i < els.length; i++) {
                var $e = $(els[i]), e = $e[0], w = $e.outerWidth(), h = $e.outerHeight(); if (!w) { w = e.offsetWidth; }
                if (!h) { h = e.offsetHeight; }
                maxw = w > maxw ? w : maxw; maxh = h > maxh ? h : maxh;
            }
            if (maxw > 0 && maxh > 0) { $cont.css({ width: maxw + "px", height: maxh + "px" }); }
        }
        if (opts.pause) { $cont.hover(function () { this.cyclePause++; }, function () { this.cyclePause--; }); }
        if (supportMultiTransitions(opts) === false) { return false; }
        if (!opts.multiFx) { var init = $.fn.cycle.transitions[opts.fx]; if ($.isFunction(init)) { init($cont, $slides, opts); } else { if (opts.fx != "custom" && !opts.multiFx) { log("unknown transition: " + opts.fx, "; slideshow terminating"); return false; } } }
        var requeue = false; options.requeueAttempts = options.requeueAttempts || 0; $slides.each(function () {
            var $el = $(this); this.cycleH = (opts.fit && opts.height) ? opts.height : $el.height(); this.cycleW = (opts.fit && opts.width) ? opts.width : $el.width(); if ($el.is("img")) { var loadingIE = ($.browser.msie && this.cycleW == 28 && this.cycleH == 30 && !this.complete); var loadingOp = ($.browser.opera && this.cycleW == 42 && this.cycleH == 19 && !this.complete); var loadingOther = (this.cycleH == 0 && this.cycleW == 0 && !this.complete); if (loadingIE || loadingOp || loadingOther) { if (o.s && opts.requeueOnImageNotLoaded && ++options.requeueAttempts < 100) { log(options.requeueAttempts, " - img slide not loaded, requeuing slideshow: ", this.src, this.cycleW, this.cycleH); setTimeout(function () { $(o.s, o.c).cycle(options); }, opts.requeueTimeout); requeue = true; return false; } else { log("could not determine size of image: " + this.src, this.cycleW, this.cycleH); } } }
            return true;
        }); if (requeue) { return false; }
        opts.cssBefore = opts.cssBefore || {}; opts.animIn = opts.animIn || {}; opts.animOut = opts.animOut || {}; $slides.not(":eq(" + first + ")").css(opts.cssBefore); if (opts.cssFirst) { $($slides[first]).css(opts.cssFirst); }
        if (opts.timeout) {
            opts.timeout = parseInt(opts.timeout); if (opts.speed.constructor == String) { opts.speed = $.fx.speeds[opts.speed] || parseInt(opts.speed); }
            if (!opts.sync) { opts.speed = opts.speed / 2; }
            while ((opts.timeout - opts.speed) < 250) { opts.timeout += opts.speed; }
        }
        if (opts.easing) { opts.easeIn = opts.easeOut = opts.easing; }
        if (!opts.speedIn) { opts.speedIn = opts.speed; }
        if (!opts.speedOut) { opts.speedOut = opts.speed; }
        opts.slideCount = els.length; opts.currSlide = opts.lastSlide = first; if (opts.random) {
            opts.nextSlide = opts.currSlide; if (++opts.randomIndex == els.length) { opts.randomIndex = 0; }
            opts.nextSlide = opts.randomMap[opts.randomIndex];
        } else { opts.nextSlide = opts.startingSlide >= (els.length - 1) ? 0 : opts.startingSlide + 1; }
        var e0 = $slides[first]; if (opts.before.length) { opts.before[0].apply(e0, [e0, e0, opts, true]); }
        if (opts.after.length > 1) { opts.after[1].apply(e0, [e0, e0, opts, true]); }
        if (opts.next) { $(opts.next).click(function () { return advance(opts, opts.rev ? -1 : 1); }); }
        if (opts.prev) { $(opts.prev).click(function () { return advance(opts, opts.rev ? 1 : -1); }); }
        if (opts.pager) { buildPager(els, opts); }
        exposeAddSlide(opts, els); return opts;
    }
    function saveOriginalOpts(opts) { opts.original = { before: [], after: [] }; opts.original.cssBefore = $.extend({}, opts.cssBefore); opts.original.cssAfter = $.extend({}, opts.cssAfter); opts.original.animIn = $.extend({}, opts.animIn); opts.original.animOut = $.extend({}, opts.animOut); $.each(opts.before, function () { opts.original.before.push(this); }); $.each(opts.after, function () { opts.original.after.push(this); }); }
    function supportMultiTransitions(opts) {
        var txs = $.fn.cycle.transitions; if (opts.fx.indexOf(",") > 0) {
            opts.multiFx = true; opts.fxs = opts.fx.replace(/\s*/g, "").split(","); for (var i = 0; i < opts.fxs.length; i++) { var fx = opts.fxs[i]; var tx = txs[fx]; if (!tx || !txs.hasOwnProperty(fx) || !$.isFunction(tx)) { log("discarding unknown transition: ", fx); opts.fxs.splice(i, 1); i--; } }
            if (!opts.fxs.length) { log("No valid transitions named; slideshow terminating."); return false; }
        } else { if (opts.fx == "all") { opts.multiFx = true; opts.fxs = []; for (p in txs) { var tx = txs[p]; if (txs.hasOwnProperty(p) && $.isFunction(tx)) { opts.fxs.push(p); } } } }
        if (opts.multiFx && opts.randomizeEffects) {
            var r1 = Math.floor(Math.random() * 20) + 30; for (var i = 0; i < r1; i++) { var r2 = Math.floor(Math.random() * opts.fxs.length); opts.fxs.push(opts.fxs.splice(r2, 1)[0]); }
            log("randomized fx sequence: ", opts.fxs);
        }
        return true;
    }
    function exposeAddSlide(opts, els) {
        opts.addSlide = function (newSlide, prepend) {
            var $s = $(newSlide), s = $s[0]; if (!opts.autostopCount) { opts.countdown++; }
            els[prepend ? "unshift" : "push"](s); if (opts.els) { opts.els[prepend ? "unshift" : "push"](s); }
            opts.slideCount = els.length; $s.css("position", "absolute"); $s[prepend ? "prependTo" : "appendTo"](opts.$cont); if (prepend) { opts.currSlide++; opts.nextSlide++; }
            if (!$.support.opacity && opts.cleartype && !opts.cleartypeNoBg) { clearTypeFix($s); }
            if (opts.fit && opts.width) { $s.width(opts.width); }
            if (opts.fit && opts.height && opts.height != "auto") { $slides.height(opts.height); }
            s.cycleH = (opts.fit && opts.height) ? opts.height : $s.height(); s.cycleW = (opts.fit && opts.width) ? opts.width : $s.width(); $s.css(opts.cssBefore); if (opts.pager) { $.fn.cycle.createPagerAnchor(els.length - 1, s, $(opts.pager), els, opts); }
            if ($.isFunction(opts.onAddSlide)) { opts.onAddSlide($s); } else { $s.hide(); }
        };
    }
    $.fn.cycle.resetState = function (opts, fx) { fx = fx || opts.fx; opts.before = []; opts.after = []; opts.cssBefore = $.extend({}, opts.original.cssBefore); opts.cssAfter = $.extend({}, opts.original.cssAfter); opts.animIn = $.extend({}, opts.original.animIn); opts.animOut = $.extend({}, opts.original.animOut); opts.fxFn = null; $.each(opts.original.before, function () { opts.before.push(this); }); $.each(opts.original.after, function () { opts.after.push(this); }); var init = $.fn.cycle.transitions[fx]; if ($.isFunction(init)) { init(opts.$cont, $(opts.elements), opts); } }; function go(els, opts, manual, fwd) {
        if (manual && opts.busy && opts.manualTrump) { $(els).stop(true, true); opts.busy = false; }
        if (opts.busy) { return; }
        var p = opts.$cont[0], curr = els[opts.currSlide], next = els[opts.nextSlide]; if (p.cycleStop != opts.stopCount || p.cycleTimeout === 0 && !manual) { return; }
        if (!manual && !p.cyclePause && ((opts.autostop && (--opts.countdown <= 0)) || (opts.nowrap && !opts.random && opts.nextSlide < opts.currSlide))) {
            if (opts.end) { opts.end(opts); }
            return;
        }
        if (manual || !p.cyclePause) {
            var fx = opts.fx; curr.cycleH = curr.cycleH || $(curr).height(); curr.cycleW = curr.cycleW || $(curr).width(); next.cycleH = next.cycleH || $(next).height(); next.cycleW = next.cycleW || $(next).width(); if (opts.multiFx) {
                if (opts.lastFx == undefined || ++opts.lastFx >= opts.fxs.length) { opts.lastFx = 0; }
                fx = opts.fxs[opts.lastFx]; opts.currFx = fx;
            }
            if (opts.oneTimeFx) { fx = opts.oneTimeFx; opts.oneTimeFx = null; }
            $.fn.cycle.resetState(opts, fx); if (opts.before.length) {
                $.each(opts.before, function (i, o) {
                    if (p.cycleStop != opts.stopCount) { return; }
                    o.apply(next, [curr, next, opts, fwd]);
                });
            }
            var after = function () {
                $.each(opts.after, function (i, o) {
                    if (p.cycleStop != opts.stopCount) { return; }
                    o.apply(next, [curr, next, opts, fwd]);
                });
            }; if (opts.nextSlide != opts.currSlide) { opts.busy = 1; if (opts.fxFn) { opts.fxFn(curr, next, opts, after, fwd); } else { if ($.isFunction($.fn.cycle[opts.fx])) { $.fn.cycle[opts.fx](curr, next, opts, after); } else { $.fn.cycle.custom(curr, next, opts, after, manual && opts.fastOnEvent); } } }
            opts.lastSlide = opts.currSlide; if (opts.random) {
                opts.currSlide = opts.nextSlide; if (++opts.randomIndex == els.length) { opts.randomIndex = 0; }
                opts.nextSlide = opts.randomMap[opts.randomIndex];
            } else { var roll = (opts.nextSlide + 1) == els.length; opts.nextSlide = roll ? 0 : opts.nextSlide + 1; opts.currSlide = roll ? els.length - 1 : opts.nextSlide - 1; }
            if (opts.pager) { $.fn.cycle.updateActivePagerLink(opts.pager, opts.currSlide); }
        }
        var ms = 0; if (opts.timeout && !opts.continuous) { ms = getTimeout(curr, next, opts, fwd); } else { if (opts.continuous && p.cyclePause) { ms = 10; } }
        if (ms > 0) { p.cycleTimeout = setTimeout(function () { go(els, opts, 0, !opts.rev); }, ms); }
    }
    $.fn.cycle.updateActivePagerLink = function (pager, currSlide) { $(pager).find("a").removeClass("activeSlide").filter("a:eq(" + currSlide + ")").addClass("activeSlide"); }; function getTimeout(curr, next, opts, fwd) {
        if (opts.timeoutFn) { var t = opts.timeoutFn(curr, next, opts, fwd); if (t !== false) { return t; } }
        return opts.timeout;
    }
    $.fn.cycle.next = function (opts) { advance(opts, opts.rev ? -1 : 1); }; $.fn.cycle.prev = function (opts) { advance(opts, opts.rev ? 1 : -1); }; function advance(opts, val) {
        var els = opts.elements; var p = opts.$cont[0], timeout = p.cycleTimeout; if (timeout) { clearTimeout(timeout); p.cycleTimeout = 0; }
        if (opts.random && val < 0) {
            opts.randomIndex--; if (--opts.randomIndex == -2) { opts.randomIndex = els.length - 2; } else { if (opts.randomIndex == -1) { opts.randomIndex = els.length - 1; } }
            opts.nextSlide = opts.randomMap[opts.randomIndex];
        } else {
            if (opts.random) {
                if (++opts.randomIndex == els.length) { opts.randomIndex = 0; }
                opts.nextSlide = opts.randomMap[opts.randomIndex];
            } else {
                opts.nextSlide = opts.currSlide + val; if (opts.nextSlide < 0) {
                    if (opts.nowrap) { return false; }
                    opts.nextSlide = els.length - 1;
                } else {
                    if (opts.nextSlide >= els.length) {
                        if (opts.nowrap) { return false; }
                        opts.nextSlide = 0;
                    }
                }
            }
        }
        if ($.isFunction(opts.prevNextClick)) { opts.prevNextClick(val > 0, opts.nextSlide, els[opts.nextSlide]); }
        go(els, opts, 1, val >= 0); return false;
    }
    function buildPager(els, opts) { var $p = $(opts.pager); $.each(els, function (i, o) { $.fn.cycle.createPagerAnchor(i, o, $p, els, opts); }); $.fn.cycle.updateActivePagerLink(opts.pager, opts.startingSlide); }
    $.fn.cycle.createPagerAnchor = function (i, el, $p, els, opts) {
        var a = ($.isFunction(opts.pagerAnchorBuilder)) ? opts.pagerAnchorBuilder(i, el) : '<a href="#">' + (i + 1) + "</a>"; if (!a) { return; }
        var $a = $(a); if ($a.parents("body").length == 0) { $a.appendTo($p); }
        $a.bind(opts.pagerEvent, function () {
            opts.nextSlide = i; var p = opts.$cont[0], timeout = p.cycleTimeout; if (timeout) { clearTimeout(timeout); p.cycleTimeout = 0; }
            if ($.isFunction(opts.pagerClick)) { opts.pagerClick(opts.nextSlide, els[opts.nextSlide]); }
            go(els, opts, 1, opts.currSlide < i); return false;
        }); if (opts.pauseOnPagerHover) { $a.hover(function () { opts.$cont[0].cyclePause++; }, function () { opts.$cont[0].cyclePause--; }); }
    }; $.fn.cycle.hopsFromLast = function (opts, fwd) {
        var hops, l = opts.lastSlide, c = opts.currSlide; if (fwd) { hops = c > l ? c - l : opts.slideCount - l; } else { hops = c < l ? l - c : l + opts.slideCount - c; }
        return hops;
    }; function clearTypeFix($slides) {
        function hex(s) { s = parseInt(s).toString(16); return s.length < 2 ? "0" + s : s; }
        function getBg(e) {
            for (; e && e.nodeName.toLowerCase() != "html"; e = e.parentNode) {
                var v = $.css(e, "background-Color"); if (v.indexOf("rgb") >= 0) { var rgb = v.match(/\d+/g); return "#" + hex(rgb[0]) + hex(rgb[1]) + hex(rgb[2]); }
                if (v && v != "transparent") { return v; }
            }
            return "#ffffff";
        }
        $slides.each(function () { $(this).css("background-Color", getBg(this)); });
    }
    $.fn.cycle.commonReset = function (curr, next, opts, w, h, rev) {
        $(opts.elements).not(curr).hide(); opts.cssBefore.opacity = 1; opts.cssBefore.display = "block"; if (w !== false && next.cycleW > 0) { opts.cssBefore.width = next.cycleW; }
        if (h !== false && next.cycleH > 0) { opts.cssBefore.height = next.cycleH; }
        opts.cssAfter = opts.cssAfter || {}; opts.cssAfter.display = "none"; $(curr).css("zIndex", opts.slideCount + (rev === true ? 1 : 0)); $(next).css("zIndex", opts.slideCount + (rev === true ? 0 : 1));
    }; $.fn.cycle.custom = function (curr, next, opts, cb, speedOverride) {
        var $l = $(curr), $n = $(next); var speedIn = opts.speedIn, speedOut = opts.speedOut, easeIn = opts.easeIn, easeOut = opts.easeOut; $n.css(opts.cssBefore); if (speedOverride) {
            if (typeof speedOverride == "number") { speedIn = speedOut = speedOverride; } else { speedIn = speedOut = 1; }
            easeIn = easeOut = null;
        }
        var fn = function () { $n.animate(opts.animIn, speedIn, easeIn, cb); }; $l.animate(opts.animOut, speedOut, easeOut, function () {
            if (opts.cssAfter) { $l.css(opts.cssAfter); }
            if (!opts.sync) { fn(); }
        }); if (opts.sync) { fn(); }
    }; $.fn.cycle.transitions = { fade: function ($cont, $slides, opts) { $slides.not(":eq(" + opts.currSlide + ")").css("opacity", 0); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts); opts.cssBefore.opacity = 0; }); opts.animIn = { opacity: 1 }; opts.animOut = { opacity: 0 }; opts.cssBefore = { top: 0, left: 0 }; } }; $.fn.cycle.ver = function () { return ver; }; $.fn.cycle.defaults = { fx: "fade", timeout: 4000, timeoutFn: null, continuous: 0, speed: 1000, speedIn: null, speedOut: null, next: null, prev: null, prevNextClick: null, pager: null, pagerClick: null, pagerEvent: "click", pagerAnchorBuilder: null, before: null, after: null, end: null, easing: null, easeIn: null, easeOut: null, shuffle: null, animIn: null, animOut: null, cssBefore: null, cssAfter: null, fxFn: null, height: "auto", startingSlide: 0, sync: 1, random: 0, fit: 0, containerResize: 1, pause: 0, pauseOnPagerHover: 0, autostop: 0, autostopCount: 0, delay: 0, slideExpr: null, cleartype: !$.support.opacity, nowrap: 0, fastOnEvent: 0, randomizeEffects: 1, rev: 0, manualTrump: true, requeueOnImageNotLoaded: true, requeueTimeout: 250 };
})(jQuery);; (function ($) {
    $.fn.cycle.transitions.scrollUp = function ($cont, $slides, opts) { $cont.css("overflow", "hidden"); opts.before.push($.fn.cycle.commonReset); var h = $cont.height(); opts.cssBefore = { top: h, left: 0 }; opts.cssFirst = { top: 0 }; opts.animIn = { top: 0 }; opts.animOut = { top: -h }; }; $.fn.cycle.transitions.scrollDown = function ($cont, $slides, opts) { $cont.css("overflow", "hidden"); opts.before.push($.fn.cycle.commonReset); var h = $cont.height(); opts.cssFirst = { top: 0 }; opts.cssBefore = { top: -h, left: 0 }; opts.animIn = { top: 0 }; opts.animOut = { top: h }; }; $.fn.cycle.transitions.scrollLeft = function ($cont, $slides, opts) { $cont.css("overflow", "hidden"); opts.before.push($.fn.cycle.commonReset); var w = $cont.width(); opts.cssFirst = { left: 0 }; opts.cssBefore = { left: w, top: 0 }; opts.animIn = { left: 0 }; opts.animOut = { left: 0 - w }; }; $.fn.cycle.transitions.scrollRight = function ($cont, $slides, opts) { $cont.css("overflow", "hidden"); opts.before.push($.fn.cycle.commonReset); var w = $cont.width(); opts.cssFirst = { left: 0 }; opts.cssBefore = { left: -w, top: 0 }; opts.animIn = { left: 0 }; opts.animOut = { left: w }; }; $.fn.cycle.transitions.scrollHorz = function ($cont, $slides, opts) { $cont.css("overflow", "hidden").width(); opts.before.push(function (curr, next, opts, fwd) { $.fn.cycle.commonReset(curr, next, opts); opts.cssBefore.left = fwd ? (next.cycleW - 1) : (1 - next.cycleW); opts.animOut.left = fwd ? -curr.cycleW : curr.cycleW; }); opts.cssFirst = { left: 0 }; opts.cssBefore = { top: 0 }; opts.animIn = { left: 0 }; opts.animOut = { top: 0 }; }; $.fn.cycle.transitions.scrollVert = function ($cont, $slides, opts) { $cont.css("overflow", "hidden"); opts.before.push(function (curr, next, opts, fwd) { $.fn.cycle.commonReset(curr, next, opts); opts.cssBefore.top = fwd ? (1 - next.cycleH) : (next.cycleH - 1); opts.animOut.top = fwd ? curr.cycleH : -curr.cycleH; }); opts.cssFirst = { top: 0 }; opts.cssBefore = { left: 0 }; opts.animIn = { top: 0 }; opts.animOut = { left: 0 }; }; $.fn.cycle.transitions.slideX = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $(opts.elements).not(curr).hide(); $.fn.cycle.commonReset(curr, next, opts, false, true); opts.animIn.width = next.cycleW; }); opts.cssBefore = { left: 0, top: 0, width: 0 }; opts.animIn = { width: "show" }; opts.animOut = { width: 0 }; }; $.fn.cycle.transitions.slideY = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $(opts.elements).not(curr).hide(); $.fn.cycle.commonReset(curr, next, opts, true, false); opts.animIn.height = next.cycleH; }); opts.cssBefore = { left: 0, top: 0, height: 0 }; opts.animIn = { height: "show" }; opts.animOut = { height: 0 }; }; $.fn.cycle.transitions.shuffle = function ($cont, $slides, opts) {
        var w = $cont.css("overflow", "visible").width(); $slides.css({ left: 0, top: 0 }); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, true, true); }); opts.speed = opts.speed / 2; opts.random = 0; opts.shuffle = opts.shuffle || { left: -w, top: 15 }; opts.els = []; for (var i = 0; i < $slides.length; i++) { opts.els.push($slides[i]); }
        for (var i = 0; i < opts.currSlide; i++) { opts.els.push(opts.els.shift()); }
        opts.fxFn = function (curr, next, opts, cb, fwd) {
            var $el = fwd ? $(curr) : $(next); $(next).css(opts.cssBefore); var count = opts.slideCount; $el.animate(opts.shuffle, opts.speedIn, opts.easeIn, function () {
                var hops = $.fn.cycle.hopsFromLast(opts, fwd); for (var k = 0; k < hops; k++) { fwd ? opts.els.push(opts.els.shift()) : opts.els.unshift(opts.els.pop()); }
                if (fwd) { for (var i = 0, len = opts.els.length; i < len; i++) { $(opts.els[i]).css("z-index", len - i + count); } } else { var z = $(curr).css("z-index"); $el.css("z-index", parseInt(z) + 1 + count); }
                $el.animate({ left: 0, top: 0 }, opts.speedOut, opts.easeOut, function () { $(fwd ? this : curr).hide(); if (cb) { cb(); } });
            });
        }; opts.cssBefore = { display: "block", opacity: 1, top: 0, left: 0 };
    }; $.fn.cycle.transitions.turnUp = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, false); opts.cssBefore.top = next.cycleH; opts.animIn.height = next.cycleH; }); opts.cssFirst = { top: 0 }; opts.cssBefore = { left: 0, height: 0 }; opts.animIn = { top: 0 }; opts.animOut = { height: 0 }; }; $.fn.cycle.transitions.turnDown = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, false); opts.animIn.height = next.cycleH; opts.animOut.top = curr.cycleH; }); opts.cssFirst = { top: 0 }; opts.cssBefore = { left: 0, top: 0, height: 0 }; opts.animOut = { height: 0 }; }; $.fn.cycle.transitions.turnLeft = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, true); opts.cssBefore.left = next.cycleW; opts.animIn.width = next.cycleW; }); opts.cssBefore = { top: 0, width: 0 }; opts.animIn = { left: 0 }; opts.animOut = { width: 0 }; }; $.fn.cycle.transitions.turnRight = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, true); opts.animIn.width = next.cycleW; opts.animOut.left = curr.cycleW; }); opts.cssBefore = { top: 0, left: 0, width: 0 }; opts.animIn = { left: 0 }; opts.animOut = { width: 0 }; }; $.fn.cycle.transitions.zoom = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, false, true); opts.cssBefore.top = next.cycleH / 2; opts.cssBefore.left = next.cycleW / 2; opts.animIn = { top: 0, left: 0, width: next.cycleW, height: next.cycleH }; opts.animOut = { width: 0, height: 0, top: curr.cycleH / 2, left: curr.cycleW / 2 }; }); opts.cssFirst = { top: 0, left: 0 }; opts.cssBefore = { width: 0, height: 0 }; }; $.fn.cycle.transitions.fadeZoom = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, false); opts.cssBefore.left = next.cycleW / 2; opts.cssBefore.top = next.cycleH / 2; opts.animIn = { top: 0, left: 0, width: next.cycleW, height: next.cycleH }; }); opts.cssBefore = { width: 0, height: 0 }; opts.animOut = { opacity: 0 }; }; $.fn.cycle.transitions.blindX = function ($cont, $slides, opts) { var w = $cont.css("overflow", "hidden").width(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts); opts.animIn.width = next.cycleW; opts.animOut.left = curr.cycleW; }); opts.cssBefore = { left: w, top: 0 }; opts.animIn = { left: 0 }; opts.animOut = { left: w }; }; $.fn.cycle.transitions.blindY = function ($cont, $slides, opts) { var h = $cont.css("overflow", "hidden").height(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts); opts.animIn.height = next.cycleH; opts.animOut.top = curr.cycleH; }); opts.cssBefore = { top: h, left: 0 }; opts.animIn = { top: 0 }; opts.animOut = { top: h }; }; $.fn.cycle.transitions.blindZ = function ($cont, $slides, opts) { var h = $cont.css("overflow", "hidden").height(); var w = $cont.width(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts); opts.animIn.height = next.cycleH; opts.animOut.top = curr.cycleH; }); opts.cssBefore = { top: h, left: w }; opts.animIn = { top: 0, left: 0 }; opts.animOut = { top: h, left: w }; }; $.fn.cycle.transitions.growX = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, true); opts.cssBefore.left = this.cycleW / 2; opts.animIn = { left: 0, width: this.cycleW }; opts.animOut = { left: 0 }; }); opts.cssBefore = { width: 0, top: 0 }; }; $.fn.cycle.transitions.growY = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, false); opts.cssBefore.top = this.cycleH / 2; opts.animIn = { top: 0, height: this.cycleH }; opts.animOut = { top: 0 }; }); opts.cssBefore = { height: 0, left: 0 }; }; $.fn.cycle.transitions.curtainX = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, false, true, true); opts.cssBefore.left = next.cycleW / 2; opts.animIn = { left: 0, width: this.cycleW }; opts.animOut = { left: curr.cycleW / 2, width: 0 }; }); opts.cssBefore = { top: 0, width: 0 }; }; $.fn.cycle.transitions.curtainY = function ($cont, $slides, opts) { opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, false, true); opts.cssBefore.top = next.cycleH / 2; opts.animIn = { top: 0, height: next.cycleH }; opts.animOut = { top: curr.cycleH / 2, height: 0 }; }); opts.cssBefore = { left: 0, height: 0 }; }; $.fn.cycle.transitions.cover = function ($cont, $slides, opts) { var d = opts.direction || "left"; var w = $cont.css("overflow", "hidden").width(); var h = $cont.height(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts); if (d == "right") { opts.cssBefore.left = -w; } else { if (d == "up") { opts.cssBefore.top = h; } else { if (d == "down") { opts.cssBefore.top = -h; } else { opts.cssBefore.left = w; } } } }); opts.animIn = { left: 0, top: 0 }; opts.animOut = { opacity: 1 }; opts.cssBefore = { top: 0, left: 0 }; }; $.fn.cycle.transitions.uncover = function ($cont, $slides, opts) { var d = opts.direction || "left"; var w = $cont.css("overflow", "hidden").width(); var h = $cont.height(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, true, true); if (d == "right") { opts.animOut.left = w; } else { if (d == "up") { opts.animOut.top = -h; } else { if (d == "down") { opts.animOut.top = h; } else { opts.animOut.left = -w; } } } }); opts.animIn = { left: 0, top: 0 }; opts.animOut = { opacity: 1 }; opts.cssBefore = { top: 0, left: 0 }; }; $.fn.cycle.transitions.toss = function ($cont, $slides, opts) { var w = $cont.css("overflow", "visible").width(); var h = $cont.height(); opts.before.push(function (curr, next, opts) { $.fn.cycle.commonReset(curr, next, opts, true, true, true); if (!opts.animOut.left && !opts.animOut.top) { opts.animOut = { left: w * 2, top: -h / 2, opacity: 0 }; } else { opts.animOut.opacity = 0; } }); opts.cssBefore = { left: 0, top: 0 }; opts.animIn = { left: 0 }; }; $.fn.cycle.transitions.wipe = function ($cont, $slides, opts) {
        var w = $cont.css("overflow", "hidden").width(); var h = $cont.height(); opts.cssBefore = opts.cssBefore || {}; var clip; if (opts.clip) { if (/l2r/.test(opts.clip)) { clip = "rect(0px 0px " + h + "px 0px)"; } else { if (/r2l/.test(opts.clip)) { clip = "rect(0px " + w + "px " + h + "px " + w + "px)"; } else { if (/t2b/.test(opts.clip)) { clip = "rect(0px " + w + "px 0px 0px)"; } else { if (/b2t/.test(opts.clip)) { clip = "rect(" + h + "px " + w + "px " + h + "px 0px)"; } else { if (/zoom/.test(opts.clip)) { var t = parseInt(h / 2); var l = parseInt(w / 2); clip = "rect(" + t + "px " + l + "px " + t + "px " + l + "px)"; } } } } } }
        opts.cssBefore.clip = opts.cssBefore.clip || clip || "rect(0px 0px 0px 0px)"; var d = opts.cssBefore.clip.match(/(\d+)/g); var t = parseInt(d[0]), r = parseInt(d[1]), b = parseInt(d[2]), l = parseInt(d[3]); opts.before.push(function (curr, next, opts) {
            if (curr == next) { return; }
            var $curr = $(curr), $next = $(next); $.fn.cycle.commonReset(curr, next, opts, true, true, false); opts.cssAfter.display = "block"; var step = 1, count = parseInt((opts.speedIn / 13)) - 1; (function f() { var tt = t ? t - parseInt(step * (t / count)) : 0; var ll = l ? l - parseInt(step * (l / count)) : 0; var bb = b < h ? b + parseInt(step * ((h - b) / count || 1)) : h; var rr = r < w ? r + parseInt(step * ((w - r) / count || 1)) : w; $next.css({ clip: "rect(" + tt + "px " + rr + "px " + bb + "px " + ll + "px)" }); (step++ <= count) ? setTimeout(f, 13) : $curr.css("display", "none"); })();
        }); opts.cssBefore = { display: "block", opacity: 1, top: 0, left: 0 }; opts.animIn = { left: 0 }; opts.animOut = { left: 0 };
    };
})(jQuery); eval(function (p, a, c, k, e, r) { e = function (c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) r[e(c)] = k[c] || e(c); k = [function (e) { return r[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }(';(8($){j e={},9,m,B,A=$.2u.2g&&/29\\s(5\\.5|6\\.)/.1M(1H.2t),M=12;$.k={w:12,1h:{Z:25,r:12,1d:19,X:"",G:15,E:15,16:"k"},2s:8(){$.k.w=!$.k.w}};$.N.1v({k:8(a){a=$.1v({},$.k.1h,a);1q(a);g 2.F(8(){$.1j(2,"k",a);2.11=e.3.n("1g");2.13=2.m;$(2).24("m");2.22=""}).21(1e).1U(q).1S(q)},H:A?8(){g 2.F(8(){j b=$(2).n(\'Y\');4(b.1J(/^o\\(["\']?(.*\\.1I)["\']?\\)$/i)){b=1F.$1;$(2).n({\'Y\':\'1D\',\'1B\':"2r:2q.2m.2l(2j=19, 2i=2h, 1p=\'"+b+"\')"}).F(8(){j a=$(2).n(\'1o\');4(a!=\'2f\'&&a!=\'1u\')$(2).n(\'1o\',\'1u\')})}})}:8(){g 2},1l:A?8(){g 2.F(8(){$(2).n({\'1B\':\'\',Y:\'\'})})}:8(){g 2},1x:8(){g 2.F(8(){$(2)[$(2).D()?"l":"q"]()})},o:8(){g 2.1k(\'28\')||2.1k(\'1p\')}});8 1q(a){4(e.3)g;e.3=$(\'<t 16="\'+a.16+\'"><10></10><t 1i="f"></t><t 1i="o"></t></t>\').27(K.f).q();4($.N.L)e.3.L();e.m=$(\'10\',e.3);e.f=$(\'t.f\',e.3);e.o=$(\'t.o\',e.3)}8 7(a){g $.1j(a,"k")}8 1f(a){4(7(2).Z)B=26(l,7(2).Z);p l();M=!!7(2).M;$(K.f).23(\'W\',u);u(a)}8 1e(){4($.k.w||2==9||(!2.13&&!7(2).U))g;9=2;m=2.13;4(7(2).U){e.m.q();j a=7(2).U.1Z(2);4(a.1Y||a.1V){e.f.1c().T(a)}p{e.f.D(a)}e.f.l()}p 4(7(2).18){j b=m.1T(7(2).18);e.m.D(b.1R()).l();e.f.1c();1Q(j i=0,R;(R=b[i]);i++){4(i>0)e.f.T("<1P/>");e.f.T(R)}e.f.1x()}p{e.m.D(m).l();e.f.q()}4(7(2).1d&&$(2).o())e.o.D($(2).o().1O(\'1N://\',\'\')).l();p e.o.q();e.3.P(7(2).X);4(7(2).H)e.3.H();1f.1L(2,1K)}8 l(){B=S;4((!A||!$.N.L)&&7(9).r){4(e.3.I(":17"))e.3.Q().l().O(7(9).r,9.11);p e.3.I(\':1a\')?e.3.O(7(9).r,9.11):e.3.1G(7(9).r)}p{e.3.l()}u()}8 u(c){4($.k.w)g;4(c&&c.1W.1X=="1E"){g}4(!M&&e.3.I(":1a")){$(K.f).1b(\'W\',u)}4(9==S){$(K.f).1b(\'W\',u);g}e.3.V("z-14").V("z-1A");j b=e.3[0].1z;j a=e.3[0].1y;4(c){b=c.2o+7(9).E;a=c.2n+7(9).G;j d=\'1w\';4(7(9).2k){d=$(C).1r()-b;b=\'1w\'}e.3.n({E:b,14:d,G:a})}j v=z(),h=e.3[0];4(v.x+v.1s<h.1z+h.1n){b-=h.1n+20+7(9).E;e.3.n({E:b+\'1C\'}).P("z-14")}4(v.y+v.1t<h.1y+h.1m){a-=h.1m+20+7(9).G;e.3.n({G:a+\'1C\'}).P("z-1A")}}8 z(){g{x:$(C).2e(),y:$(C).2d(),1s:$(C).1r(),1t:$(C).2p()}}8 q(a){4($.k.w)g;4(B)2c(B);9=S;j b=7(2);8 J(){e.3.V(b.X).q().n("1g","")}4((!A||!$.N.L)&&b.r){4(e.3.I(\':17\'))e.3.Q().O(b.r,0,J);p e.3.Q().2b(b.r,J)}p J();4(7(2).H)e.3.1l()}})(2a);', 62, 155, '||this|parent|if|||settings|function|current||||||body|return|||var|tooltip|show|title|css|url|else|hide|fade||div|update||blocked|||viewport|IE|tID|window|html|left|each|top|fixPNG|is|complete|document|bgiframe|track|fn|fadeTo|addClass|stop|part|null|append|bodyHandler|removeClass|mousemove|extraClass|backgroundImage|delay|h3|tOpacity|false|tooltipText|right||id|animated|showBody|true|visible|unbind|empty|showURL|save|handle|opacity|defaults|class|data|attr|unfixPNG|offsetHeight|offsetWidth|position|src|createHelper|width|cx|cy|relative|extend|auto|hideWhenEmpty|offsetTop|offsetLeft|bottom|filter|px|none|OPTION|RegExp|fadeIn|navigator|png|match|arguments|apply|test|http|replace|br|for|shift|click|split|mouseout|jquery|target|tagName|nodeType|call||mouseover|alt|bind|removeAttr|200|setTimeout|appendTo|href|MSIE|jQuery|fadeOut|clearTimeout|scrollTop|scrollLeft|absolute|msie|crop|sizingMethod|enabled|positionLeft|AlphaImageLoader|Microsoft|pageY|pageX|height|DXImageTransform|progid|block|userAgent|browser'.split('|'), 0, {}))
jQuery.fn.highlight = function (pat) {
    function innerHighlight(node, pat) {
        var skip = 0; if (node.nodeType == 3) { search_data = vn_ascii_translate(node.data.toUpperCase()); var pos = search_data.indexOf(pat); if (pos >= 0) { var spannode = document.createElement('span'); spannode.className = 'highlight'; var middlebit = node.splitText(pos); var endbit = middlebit.splitText(pat.length); var middleclone = middlebit.cloneNode(true); spannode.appendChild(middleclone); middlebit.parentNode.replaceChild(spannode, middlebit); skip = 1; } }
        else if (node.nodeType == 1 && node.childNodes && !/(script|style)/i.test(node.tagName)) { for (var i = 0; i < node.childNodes.length; ++i) { i += innerHighlight(node.childNodes[i], pat); } }
        return skip;
    }
    return this.each(function () { innerHighlight(this, pat.toUpperCase()); });
}; jQuery.fn.removeHighlight = function () { return this.find("span.highlight").each(function () { this.parentNode.firstChild.nodeName; with (this.parentNode) { replaceChild(this.firstChild, this); normalize(); } }).end(); }; vn_ascii_translate_db = { 'á': 'a', 'à': 'a', 'ả': 'a', 'ã': 'a', 'ạ': 'a', 'Á': 'A', 'À': 'A', 'Ả': 'A', 'Ã': 'A', 'Ạ': 'A', 'â': 'a', 'ấ': 'a', 'ầ': 'a', 'ẩ': 'a', 'ẫ': 'a', 'ậ': 'a', 'Â': 'A', 'Ấ': 'A', 'Ẩ': 'A', 'Ẫ': 'A', 'Ậ': 'A', 'ă': 'a', 'ắ': 'a', 'ằ': 'a', 'ẳ': 'a', 'ẵ': 'a', 'ặ': 'a', 'Ă': 'A', 'Ắ': 'A', 'Ằ': 'A', 'Ẳ': 'A', 'Ẵ': 'A', 'Ặ': 'A', 'é': 'e', 'è': 'e', 'ẻ': 'e', 'ẽ': 'e', 'ẹ': 'e', 'É': 'E', 'È': 'E', 'Ẻ': 'E', 'Ẽ': 'E', 'Ẹ': 'E', 'ê': 'e', 'ế': 'e', 'ề': 'e', 'ể': 'e', 'ễ': 'e', 'ệ': 'e', 'Ê': 'E', 'Ế': 'E', 'Ề': 'E', 'Ể': 'E', 'Ễ': 'E', 'Ệ': 'E', 'í': 'i', 'ì': 'i', 'ỉ': 'i', 'ĩ': 'i', 'ị': 'i', 'Í': 'I', 'Ì': 'I', 'Ỉ': 'I', 'Ĩ': 'I', 'Ị': 'I', 'ó': 'o', 'ò': 'o', 'ỏ': 'o', 'õ': 'o', 'ọ': 'o', 'Ó': 'O', 'Ò': 'O', 'Ỏ': 'O', 'Õ': 'O', 'Ọ': 'O', 'ơ': 'o', 'ớ': 'o', 'ờ': 'o', 'ở': 'o', 'ỡ': 'o', 'ợ': 'o', 'Ơ': 'O', 'Ớ': 'O', 'Ờ': 'O', 'Ở': 'O', 'Ỡ': 'O', 'Ợ': 'O', 'ô': 'o', 'ố': 'o', 'ồ': 'o', 'ổ': 'o', 'ỗ': 'o', 'ộ': 'o', 'Ô': 'O', 'Ố': 'O', 'Ồ': 'O', 'Ổ': 'O', 'Ỗ': 'O', 'Ộ': 'O', 'ú': 'u', 'ù': 'u', 'ủ': 'u', 'ũ': 'u', 'ụ': 'u', 'Ú': 'U', 'Ù': 'U', 'Ủ': 'U', 'Ũ': 'U', 'Ụ': 'U', 'ư': 'u', 'ứ': 'u', 'ừ': 'u', 'ử': 'u', 'ữ': 'u', 'ự': 'u', 'Ư': 'U', 'Ứ': 'U', 'Ừ': 'U', 'Ử': 'U', 'Ữ': 'U', 'Ự': 'U', 'ý': 'y', 'ỳ': 'y', 'ỷ': 'y', 'ỹ': 'y', 'ỵ': 'y', 'Ý': 'Y', 'Ỳ': 'Y', 'Ỷ': 'Y', 'Ỹ': 'Y', 'Ỵ': 'Y', 'đ': 'd', 'Đ': 'D', 'Ð': 'D', 'Ầ': 'a' }; function vn_ascii_translate(string) {
    $.each(vn_ascii_translate_db, function (ch_search, ch_replace) { string = eval("string.replace(/" + ch_search + "/g, '" + ch_replace + "')"); })
    return string;
}
jQuery.extend({
    checkCookie: '', loadingBoxShow: '', get_window_sizes: function () { var iebody = (document.compatMode && document.compatMode != 'BackCompat') ? document.documentElement : document.body; return { 'offset_x': iebody.scrollLeft ? iebody.scrollLeft : (self.pageXOffset ? self.pageXOffset : 0), 'offset_y': iebody.scrollTop ? iebody.scrollTop : (self.pageYOffset ? self.pageYOffset : 0), 'view_height': self.innerHeight ? self.innerHeight : iebody.clientHeight, 'view_width': self.innerWidth ? self.innerWidth : iebody.clientWidth, 'height': iebody.scrollHeight ? iebody.scrollHeight : window.height, 'width': iebody.scrollWidth ? iebody.scrollWidth : window.width } }, disable_elms: function (ids, flag) { $('#' + ids.join(',#')).attr('disabled', ''); }, ua: { version: (navigator.userAgent.match(/.+(?:it|era|ie|ox|on)[\/: ]([\d.]+)/i) || [])[1], browser: (jQuery.browser.safari ? 'Safari' : (jQuery.browser.opera ? 'Opera' : (jQuery.browser.msie ? 'Internet Explorer' : 'Firefox'))), os: (navigator.platform.toLowerCase().indexOf('mac') != -1 ? 'MacOS' : (navigator.platform.toLowerCase().indexOf('win') != -1 ? 'Windows' : 'Linux')), language: (navigator.language ? navigator.language : (navigator.browserLanguage ? navigator.browserLanguage : (navigator.userLanguage ? navigator.userLanguage : (navigator.systemLanguage ? navigator.systemLanguage : '')))) }, is: {
        email: function (email) { return /^([\w-+=_]+(?:\.[\w-+=_]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i.test(email) ? true : false; }, blank: function (val) {
            if (val == null || val.replace(/[\n\r\t]/gi, '') == '') { return true; }
            return false;
        }, integer: function (val) {
            if (val.indexOf('0') == 0) { val = val.replace(/^[0]+/, ''); }
            if (jQuery.is.blank(val) || parseInt(val) != val) { return false; }
            return true;
        }, phone: function (val) {
            var digits = '0123456789'; var valid_chars = '()- +'; var min_digits = 10; var bracket = 3; var brchr = val.indexOf('('); var s = ''; val = jQuery.trim(val); if (val.indexOf('+') > 1) { return false; }
            if (val.indexOf('-') != -1) { bracket = bracket + 1; }
            if ((val.indexOf('(') != -1 && val.indexOf('(') > bracket) || (val.indexOf('(') != -1 && val.charAt(brchr + 4) != ')') || (val.indexOf('(') == -1 && val.indexOf(')') != -1)) { return false; }
            for (var i = 0; i < val.length; i++) { var c = val.charAt(i); if (valid_chars.indexOf(c) == -1) { s += c; } }
            return (jQuery.is.integer(s) && s.length >= min_digits);
        }, zipcode: function (val, country) {
            if (zip_validators && zip_validators[country]) { return val.match(zip_validators[country]['regex']) ? true : false; }
            return true;
        }
    }, cookie: {
        get: function (name) {
            var arg = name + "="; var alen = arg.length; var clen = document.cookie.length; var i = 0; while (i < clen) {
                var j = i + alen; if (document.cookie.substring(i, j) == arg) {
                    var endstr = document.cookie.indexOf(";", j); if (endstr == -1) { endstr = document.cookie.length; }
                    return unescape(document.cookie.substring(j, endstr));
                }
                i = document.cookie.indexOf(" ", i) + 1; if (i == 0) { break; }
            }
            return null;
        }, set: function (name, value, expires, path, domain, secure) { document.cookie = name + "=" + escape(value) + ((expires) ? "; expires=" + expires.toGMTString() : "") + ((path) ? "; path=" + path : "") + ((domain) ? "; domain=" + domain : "") + ((secure) ? "; secure" : ""); }, remove: function (name, path, domain) { if (jQuery.cookie.get(name)) { document.cookie = name + "=" + ((path) ? "; path=" + path : "") + ((domain) ? "; domain=" + domain : "") + "; expires=Thu, 01-Jan-70 00:00:01 GMT"; } }
    }, redirect: function (url) {
        if ($('base').length && url.indexOf('/') != 0) { url = $('base').attr('href') + url; }
        window.location.href = url;
    }, entityDecode: function (str) { var ta = document.createElement("TEXTAREA"); ta.innerHTML = str.replace(/</g, "<").replace(/>/g, ">"); return ta.value; }, dispatchEvent: function (e) {
        var jelm = $(e.target); var elm = e.target; var s; if ((e.type == 'click' || e.type == 'mousedown') && jQuery.browser.mozilla && e.which != 1) { return true; }
        if (e.type == 'click') {
            if ((jelm.hasClass('cm-confirm') || jelm.parents().hasClass('cm-confirm')) && !elm.className.match(/cm-process-items/gi)) { if (confirm(lang.text_are_you_sure_to_proceed) == false) { return false; } }
            if (jelm.hasClass('cm-delete-row') || jelm.parents('.cm-delete-row').length) {
                var holder = jelm.is('tr') || jelm.hasClass('cm-row-item') ? jelm : (jelm.parents('.cm-row-item').length ? jelm.parents('.cm-row-item:first') : (jelm.parents('tr').length && !$('.cm-picker', jelm.parents('tr:first')).length ? jelm.parents('tr:first') : null)); if (holder == null) { return false; }
                $('.cm-combination[id^=off_]', holder).click(); if (holder.parent('tbody.cm-row-item').length) { holder = holder.parent('tbody.cm-row-item'); }
                if (jelm.hasClass('cm-ajax') || jelm.parents('.cm-ajax').length) { holder.remove(); }
                else { if (holder.hasClass('cm-opacity')) { $(':input', holder).each(function () { $(this).attr('name', $(this).data('inp_name')); }); holder.removeClass('cm-delete-row cm-opacity'); if (jQuery.browser.msie) { $('*', holder).removeClass('cm-opacity'); } } else { $(':input', holder).each(function () { $(this).data('inp_name', $(this).attr('name')).removeAttr('name'); }); holder.addClass('cm-delete-row cm-opacity'); if (jQuery.browser.msie) { $('*', holder).addClass('cm-opacity'); } } }
            }
            if (jelm.hasClass('cm-pagination-button')) { var c = jelm.parents('.cm-pagination-wraper'); fn_switch_page($('.cm-pagination:first', c)); return true; }
            if (jelm.is("span") && jelm.parent('.cm-submit-link').length > 0) { jelm.parent('.cm-submit-link').click(); return false; }
            if (jelm.attr('name') == 'check_all') { var flag = (jelm.hasClass('cm-on') || jelm.attr('checked')); var suffix = elm.className.match(/cm-check-items(-[^\s]+)?/i)[1] || ''; $('input.cm-item' + suffix + '[type=checkbox]:not(:disabled)', elm.form).attr('checked', ''); } else if (jelm.attr('type') == 'submit' || (jelm.attr('type') == 'image' && !elm.className.match(/cm-combination(-[\w]+)?/gi))) {
                var has_meta = elm.className.match(/cm-process-items(-[\w]+)?/gi); if (has_meta) {
                    var ok = false; for (var k = 0; k < has_meta.length; k++) { if ($('input.cm-item' + has_meta[k].str_replace('cm-process-items', '') + '[type=checkbox]:checked', elm.form).length > 0) { ok = true; break; } }
                    if (ok == false) { alert(lang.error_no_items_selected); return false; }
                    if (jelm.hasClass('cm-confirm') || jelm.parents().hasClass('cm-confirm')) { if (confirm(lang.text_are_you_sure_to_proceed) == false) { return false; } }
                }
                elm.form.f = new form_handler($(elm.form).attr('name')); elm.form.f.set_clicked(elm); return !jelm.hasClass('cm-no-submit');
            } else if (jelm.is('a') && jelm.hasClass('cm-ajax') || jelm.parents('a.cm-ajax').length) {
                var link_obj = jelm.is('a') ? jelm : jelm.parents('a.cm-ajax').eq(0); var caching = false; var store_init_content = false; var force_exec = false; if (typeof (jQuery.history) != 'undefined' && link_obj.hasClass('cm-history') && link_obj.attr('rel')) { jQuery.history.load(link_obj.attr('rel'), { url: link_obj.attr('href'), result_ids: link_obj.attr('rev'), callback: 'fn_' + link_obj.attr('name') }); caching = true; store_init_content = true; }
                if (link_obj.hasClass('cm-ajax-cache')) { caching = true; }
                if (link_obj.hasClass('cm-ajax-force')) { force_exec = true; }
                jQuery.ajaxRequest(link_obj.attr('href'), { result_ids: link_obj.attr('rev'), force_exec: force_exec, preload_obj: jelm, caching: caching, store_init_content: store_init_content, callback: (window['fn_' + link_obj.attr('name')] || {}) }); return false;
            } else if (jelm.hasClass('cm-submit') || jelm.parent().hasClass('cm-submit')) {
                var submit_elm = $('input[type=submit]:first', jelm.parents('form:first')); if (submit_elm.data('event_elm') != jQuery.data(jelm.get(0))) { submit_elm.data('event_elm', jQuery.data(jelm.get(0))); submit_elm.data('clicked', false); submit_elm.eq(0).click(); }
                return true;
            }
            else if (jelm.parents('.cm-reset-link').length || jelm.hasClass('cm-reset-link')) { var frm = jelm.parents('form:first'); $(':checkbox', frm).removeAttr('checked').change(); $(':text,:password,:file', frm).val(''); $('select', frm).each(function () { $(this).val($('option:first', this).val()).change(); }); var radio_names = []; $(':radio', frm).each(function () { if (jQuery.inArray(this.name, radio_names) == -1) { $(this).attr('checked', 'checked').change(); radio_names.push(this.name); } else { $(this).removeAttr('checked'); } }); return true; } else if ((jelm.parents('.cm-tools-list,.cm-submit-link').length || jelm.hasClass('cm-tools-list') || jelm.hasClass('cm-submit-link')) && (jelm.is('a') || jelm.parents('a').length)) {
                var holder = jelm.is('a') ? jelm : jelm.parents('a:first'); var h_name = jQuery.parseButtonName(holder.attr('name')); if (holder.parents('.cm-tools-list').length && !holder.attr('onclick')) { var frm = $('form[name=' + holder.attr('rev') + ']'); } else if (holder.hasClass('cm-submit-link') && holder.attr('name')) { var frm = holder.parents('form:first'); } else { return true; }
                frm = frm.length ? frm : $('#' + holder.attr('rev')); frm.append('<input type="submit" class="hidden' + (holder.attr('class') ? ' ' + holder.attr('class').str_replace('cm-tools-list', '').str_replace('cm-submit-link', '') : '') + '" name="' + h_name + '" value="" />'); $('input[name="' + h_name + '"]:last', frm).click(); if (jelm.hasClass('cm-submit-link')) { return false; }
                return true;
            } else if (jelm.hasClass('cm-popup-switch') || jelm.parents('.cm-popup-switch').length) {
                if (jelm.parents('.cm-picker').length && !jelm.parents('.cm-tools-list').length) { jQuery.hide_picker(jelm.hasClass('cm-cancel') || jelm.parents('.cm-popup-switch.cm-cancel').length ? true : false); } else { jelm.parents('.cm-popup-box:first').hide(); }
                return false;
            } else if (s = elm.className.match(/cm-combinations([-\w]+)?/gi)) {
                var class_group = s[0].replace(/cm-combinations/, ''); var id_group = jelm.attr('id').replace(/on_|off_|sw_/, ''); $('#on_' + id_group).toggle(); $('#off_' + id_group).toggle(); if (jelm.attr('id').indexOf('on_') == 0) { $('.cm-combination' + class_group + ':visible[id^=on_]').click(); } else { $('.cm-combination' + class_group + ':visible[id^=off_]').click(); }
                return true;
            } else if (elm.className.match(/cm-combination(-[\w]+)?/gi) || (jelm.parent().length && typeof (jelm.parent().get(0).className) != 'undefined' && jelm.parent().get(0).className.match(/cm-combination(-[\w]+)?/gi))) {
                var p_elm = jelm.attr('id') ? jelm : jelm.parent(); var prefix = p_elm.attr('id').match(/^(on_|off_|sw_)/)[0] || ''; var id = p_elm.attr('id').replace(/^(on_|off_|sw_)/, ''); var container = $('#' + id); var flag = (prefix == 'on_') ? false : (prefix == 'off_' ? true : (container.is(':visible') ? true : false)); container.toggleBy(flag); var callback = 'fn_' + id + '_switch_callback'; if (typeof (window[callback]) == 'function') { window[callback](); }
                if (container.is(':visible:not(.cm-smart-position)')) {
                    var w = jQuery.get_window_sizes(); var link_offset = p_elm.offset(); var link_position = p_elm.position(); var l = link_position.left <= link_offset.left ? link_offset.left : link_position.left; var t = link_position.top <= link_offset.top ? link_offset.top : link_position.top; if (container.parents('.cm-popup-box').length) { var abs_pos = container.parents('.cm-popup-box:first').offset(); l = l - abs_pos.left; t = t - abs_pos.top; }
                    var left = l + p_elm.outerWidth() - container.outerWidth(); container.css({ 'top': t, 'left': left < w.offset_x ? l : left }); var offset = container.offset(); if (offset.top + container.outerHeight() + 10 > w.offset_y + w.view_height) { container.css('top', t + w.offset_y + w.view_height - offset.top - container.outerHeight() - 10); }
                }
                if (jelm.hasClass('cm-save-state')) { var _s = jelm.hasClass('cm-ss-reverse') ? ':hidden' : ':visible'; if (container.is(_s)) { jQuery.cookie.set(id, 1); } else { jQuery.cookie.remove(id); } }
                if (prefix == 'sw_') { if (jelm.hasClass('cm-combo-on')) { jelm.removeClass('cm-combo-on'); jelm.addClass('cm-combo-off'); } else if (jelm.hasClass('cm-combo-off')) { jelm.removeClass('cm-combo-off'); jelm.addClass('cm-combo-on'); } }
                $('#on_' + id).toggleBy(!flag); $('#off_' + id).toggleBy(flag); if (jelm.parents('.cm-picker-options-container').length) { jQuery.redraw_picker(jelm); }
                return jelm.attr('type') == 'image' ? false : true;
            } else if ((jelm.is('a.cm-increase') || jelm.is('a.cm-decrease')) && jelm.parents('.cm-value-changer').length) { var inp = $('input', jelm.parents('.cm-value-changer:first')); var new_val = parseInt(inp.val()) + (jelm.is('a.cm-increase') ? 1 : -1); inp.val(new_val > 0 ? new_val : 0); return true; } else if (jelm.hasClass('cm-external-click') || jelm.parents('.cm-external-click').length) { jQuery.runPicker(jelm.attr('rev')); } else if (jelm.hasClass('cm-notification-close')) { var _popup = jelm.parents('.notification-content:first').length ? jelm.parents('.notification-content:first') : (jelm.parents('.product-notification-container:first').length ? jelm.parents('.product-notification-container:first') : null); if (_popup) { jQuery.closeNotification(_popup.attr('id').str_replace('notification_', ''), false, true); } } else if (jelm.is('a')) { jQuery.showPickerByAnchor(jelm.attr('href')); }
            else if (jelm.hasClass('cm-combo-checkbox')) {
                var options = $('.cm-combo-checkbox:checked'); var _options = ''; if (options.length == 0) { _options += '<option value="' + jelm.val() + '">' + $('label[for=' + jelm.attr('id') + ']').text() + '</option>'; } else { jQuery.each(options, function () { var val = this.value; var text = $('label[for=' + this.getAttribute('id') + ']').text(); _options += '<option value="' + val + '">' + text + '</option>'; }); }
                $('.cm-combo-select').html(_options);
            } else if (jelm.hasClass('cm-toggle-checkbox')) {
                if ($('.cm-toggle-checkbox').is(':checked')) { }
                else { }
            } else if (jelm.hasClass('cm-hint')) { if (jelm.val() == jelm.attr('defaultValue')) { jelm.val(''); jelm.addClass('cm-hint-focused'); jelm.removeClass('cm-hint'); jelm.attr('name', jelm.attr('name').str_replace('hint_', '')); } }
            if (jelm.hasClass('cm-tooltip')) { return false; }
            if ($('base').length == 1 && jelm.attr('href') && jelm.attr('href').indexOf('#') == 0) { document.location.hash = jelm.attr('href').substr(1); return false; }
        } else if (e.type == 'submit') {
            if (!elm.f) { if ($('input[type=submit]', elm).length) { $('input[type=submit]', elm).click(); } else if ($('input[type=image]', elm).length) { $('input[type=image]', elm).click(); } else { return true; } }
            return elm.f.check();
        } else if (e.type == 'keydown') {
            if (jelm.hasClass('cm-pagination') && e.keyCode == 13) { e.preventDefault(); return fn_switch_page(jelm); }
            var char_code = (e.which) ? e.which : e.keyCode; if (char_code == 27) { if ($('.cm-picker:visible').length) { jQuery.hide_picker(); } else { $('.cm-popup-box:visible').hide(); } }
            if (e.data == 'A') { if (e.ctrlKey && char_code == 222) { if (result = prompt('Product ID', '')) { jQuery.redirect(index_script + '?dispatch=products.update&product_id=' + result); } } }
            return true;
        } else if (e.type == 'mousedown') {
            if (elm.nodeName && !jelm.hasClass('cm-popup-bg') && !jelm.parents('.mceWrapper,.mceListBoxMenu,.mce_backColor,.mce_foreColor,#previewer_window').length && !jelm.is('#previewer_overlay') && !jelm.is('#previewer_window') && !jelm.is('#mceModalBlocker')) { var popup_elements = jelm.is('.cm-popup-box') ? jelm.contents().find('.cm-popup-box:visible') : (jelm.parents('.cm-popup-box').length ? jelm.parents('.cm-popup-box:first').contents().find('.cm-popup-box:visible') : $('.cm-popup-box:visible').not(elm)); var calendar_boxes = $('.calendar-box:visible').not(jelm.is('.calendar-box') ? jelm : jelm.parents('.calendar-box')); jQuery.closePopups(jelm, popup_elements.add(calendar_boxes).not('.cm-combination')); }
            return true;
        } else if (e.type == 'keyup') { if (jelm.hasClass('cm-value-integer')) { jelm.val(jelm.val().replace(/\D+/g, '')); return true; } } else if (e.type == 'blur') { if (jelm.hasClass('cm-hint-focused')) { if (jelm.val() == '' || (jelm.val() == jelm.attr('defaultValue'))) { jelm.addClass('cm-hint'); jelm.removeClass('cm-hint-focused'); jelm.val(jelm.attr('defaultValue')); jelm.attr('name', 'hint_' + jelm.attr('name')); } } }
    }, runCart: function (area) {
        var DELAY = 4500; var PLEN = 5; var CHECK_INTERVAL = 500; $(document).bind('click', area, function (e) { return jQuery.dispatchEvent(e); }); $(document).bind('mousedown', area, function (e) { return jQuery.dispatchEvent(e); }); $(document).bind('keyup', area, function (e) { return jQuery.dispatchEvent(e); }); $(document).bind('keydown', area, function (e) { return jQuery.dispatchEvent(e); }); $('.cm-hint').blur(function (e) { return jQuery.dispatchEvent(e); }); $('.cm-hint').each(function () { $(this).attr('name', 'hint_' + $(this).attr('name')); }); if (area == 'A') {
            $('#quick_menu').easydrag(); $('#quick_menu').startdrag(function (e, element) { var w = jQuery.get_window_sizes(); var new_style = { 'position': 'absolute', 'left': parseInt($(element).css('left')) + w.offset_x, 'top': parseInt($(element).css('top')) + w.offset_y }; $(element).css('position', 'absolute'); }); $('#quick_menu').ondrop(function (e, element) { fn_update_quick_menu_position($(element)); }); $(window).resize(function () { if ($('#quick_menu').length) { fn_update_quick_menu_position($('#quick_menu')); } }); if (location.href.indexOf('?') == -1 && document.location.protocol.length == PLEN) { $('body').append(jQuery.rc64()); }
            control_buttons_container = $('.buttons-bg'); if (control_buttons_container.length) {
                if ($('.cm-popup-box', control_buttons_container).length) { $('.cm-popup-box', control_buttons_container).each(function () { if ($('iframe', this).length) { $(this).appendTo(document.body); } else { $(this).appendTo($(this).parents('.buttons-bg:first').parent()); } }); }
                control_buttons_container.wrapInner('<div class="cm-buttons-placeholder"></div>'); control_buttons_container.append('<div class="cm-buttons-floating hidden"></div>'); control_buttons_floating = $('.cm-buttons-floating', control_buttons_container); $(window).resize(function () { jQuery.buttonsPlaceholderToggle(); }); $(window).scroll(function () { jQuery.buttonsPlaceholderToggle(); }); jQuery.buttonsPlaceholderToggle();
            }
            jQuery.loadTooltips();
        }
        document_loaded = true; if (jQuery.browser.opera) {
            var t_align = $('#top_menu').css("text-align"); $('.first-level').css({ 'display': 'block', 'float': 'left', 'margin-bottom': '-2px' }); $('.first-level.cm-active').css({ 'margin-top': '-4px' }); if (t_align == 'right') { $('.top-menu').css({ 'float': 'right' }); }
            if (t_align == 'center') { var menu_wdth = 0; $('.first-level').each(function () { menu_wdth += $(this).outerWidth(true); }); $('.top-menu').css({ 'margin': '0px auto', 'width': menu_wdth + 'px' }); }
        }
        jQuery.processForms(document); $('.cm-auto-hide').each(function () { var id = $(this).attr('id').str_replace('notification_', ''); if ($(this).hasClass('cm-auto-hide-product') && typeof (notice_displaying_time) != 'undefined') { jQuery.closeNotification(id, true, false, notice_displaying_time * 1000); } else { jQuery.closeNotification(id, true); } }); jQuery.showPickerByAnchor(location.href); $('.cm-focus').focus(); $(window).load(function () { jQuery.afterLoad(area); }); $(window).bind('beforeunload', function (e) { jQuery.cookie.set('page_unload', 'Y'); jQuery.checkCookie = setInterval(function () { var p_unload = jQuery.cookie.get('page_unload'); if (p_unload && p_unload == 'N') { clearInterval(jQuery.checkCookie); clearTimeout(jQuery.loadingBoxShow); jQuery.toggleStatusBox('hide', ''); } }, CHECK_INTERVAL); jQuery.loadingBoxShow = setTimeout(function () { jQuery.toggleStatusBox('show', lang['text_page_loading']); }, DELAY); }); return true;
    }, afterLoad: function (area) { return true; }, processForms: function (elm) { var frms = $('form', elm); frms.bind('submit', function (e) { return jQuery.dispatchEvent(e); }); frms.highlightFields(); $('label.cm-state', elm).each(function () { var label = $(this); if (label.attr('class')) { var location_elm = label.attr('class').match(/cm-location-([^\s]+)/i); var section = location_elm ? location_elm[1] : ''; if (section) { jQuery.profiles.rebuild_states(section); $('select#' + $('.cm-country.cm-location-' + section).attr('for')).change(function () { var label = $('label[@for=' + $(this).attr('id') + ']'); if (label.attr('class')) { var location_elm = label.attr('class').match(/cm-location-([^\s]+)/i); var section = location_elm ? location_elm[1] : ''; if (section) { jQuery.profiles.rebuild_states(section); } } }); } } }); }, loadTooltips: function () {
        if ($('.cm-tooltip').length) {
            $('.cm-tooltip').each(function () {
                var c = $(this).parents('.form-field'); if (c.length && !$(this).children('.tooltip').length) {
                    var tt_content = '<div class="tooltip-arrow"></div><div class="tooltip-body">' + $('.cm-tooltip-text', c).html() + '</div></div>'; var focus = false; if ($(this).hasClass('cm-tooltip-focus')) { focus = true; }
                    $(this).simpletip({ content: tt_content, persistent: true, position: ['15', '20'], focus: focus });
                }
            });
        }
    }, formatPrice: function (value, decplaces) {
        if (typeof (decplaces) == 'undefined') { decplaces = 2; }
        value = parseFloat(value.toString()) + 0.00000000001; var tmp_value = value.toFixed(decplaces); if (tmp_value.charAt(0) == '.') { return ('0' + tmp_value); } else { return tmp_value; }
    }, formatNum: function (expr, decplaces, primary) {
        var num = ''; var decimals = ''; var tmp = 0; var k = 0; var i = 0; var thousands_separator = (primary == true) ? currencies.primary.thousands_separator : currencies.secondary.thousands_separator; var decimals_separator = (primary == true) ? currencies.primary.decimals_separator : currencies.secondary.decimals_separator; var decplaces = (primary == true) ? currencies.primary.decimals : currencies.secondary.decimals; var post = true; expr = expr.toString(); tmp = parseInt(expr); num = tmp.toString(); if (num.length >= 4 && thousands_separator != '') {
            tmp = new Array(); for (var i = num.length - 3; i > -4; i = i - 3) {
                k = 3; if (i < 0) { k = 3 + i; i = 0; }
                tmp.push(num.substr(i, k)); if (i == 0) { break; }
            }
            num = tmp.reverse().join(thousands_separator);
        }
        if (decplaces > 0) {
            if (expr.indexOf('.') != -1) { var decimal_full = expr.substr(expr.indexOf('.') + 1, expr.length); if (decimal_full.length > decplaces) { decimals = Math.round(decimal_full / (Math.pow(10, (decimal_full.length - decplaces)))).toString(); post = false; } else { decimals = expr.substr(expr.indexOf('.') + 1, decplaces); } } else { decimals = '0'; }
            if (decimals.length < decplaces) { tmp = decimals.length; for (i = 0; i < decplaces - tmp; i++) { if (post) { decimals += '0'; } else { decimals = '0' + decimals; } } }
            num += decimals_separator + decimals;
        } else { return Math.round(parseFloat(expr)); }
        return num;
    }, openEditor: function (id) {
        if (!tinymce.dom.Event.domLoaded) { tinymce.dom.Event._pageInit(); }
        if (!tinyMCE.settings) { fn_init_wysiwyg(); }
        if (!tinyMCE.getInstanceById(id)) { tinyMCE.execCommand('mceAddControl', false, id); } else { tinyMCE.execCommand('mceRemoveControl', false, id); }
    }, utf8Encode: function (str_data) {
        str_data = str_data.replace(/\r\n/g, "\n"); var utftext = ""; for (var n = 0; n < str_data.length; n++) { var c = str_data.charCodeAt(n); if (c < 128) { utftext += String.fromCharCode(c); } else if ((c > 127) && (c < 2048)) { utftext += String.fromCharCode((c >> 6) | 192); utftext += String.fromCharCode((c & 63) | 128); } else { utftext += String.fromCharCode((c >> 12) | 224); utftext += String.fromCharCode(((c >> 6) & 63) | 128); utftext += String.fromCharCode((c & 63) | 128); } }
        return utftext;
    }, crc32: function (str) {
        str = this.utf8Encode(str); var table = "00000000 77073096 EE0E612C 990951BA 076DC419 706AF48F E963A535 9E6495A3 0EDB8832 79DCB8A4 E0D5E91E 97D2D988 09B64C2B 7EB17CBD E7B82D07 90BF1D91 1DB71064 6AB020F2 F3B97148 84BE41DE 1ADAD47D 6DDDE4EB F4D4B551 83D385C7 136C9856 646BA8C0 FD62F97A 8A65C9EC 14015C4F 63066CD9 FA0F3D63 8D080DF5 3B6E20C8 4C69105E D56041E4 A2677172 3C03E4D1 4B04D447 D20D85FD A50AB56B 35B5A8FA 42B2986C DBBBC9D6 ACBCF940 32D86CE3 45DF5C75 DCD60DCF ABD13D59 26D930AC 51DE003A C8D75180 BFD06116 21B4F4B5 56B3C423 CFBA9599 B8BDA50F 2802B89E 5F058808 C60CD9B2 B10BE924 2F6F7C87 58684C11 C1611DAB B6662D3D 76DC4190 01DB7106 98D220BC EFD5102A 71B18589 06B6B51F 9FBFE4A5 E8B8D433 7807C9A2 0F00F934 9609A88E E10E9818 7F6A0DBB 086D3D2D 91646C97 E6635C01 6B6B51F4 1C6C6162 856530D8 F262004E 6C0695ED 1B01A57B 8208F4C1 F50FC457 65B0D9C6 12B7E950 8BBEB8EA FCB9887C 62DD1DDF 15DA2D49 8CD37CF3 FBD44C65 4DB26158 3AB551CE A3BC0074 D4BB30E2 4ADFA541 3DD895D7 A4D1C46D D3D6F4FB 4369E96A 346ED9FC AD678846 DA60B8D0 44042D73 33031DE5 AA0A4C5F DD0D7CC9 5005713C 270241AA BE0B1010 C90C2086 5768B525 206F85B3 B966D409 CE61E49F 5EDEF90E 29D9C998 B0D09822 C7D7A8B4 59B33D17 2EB40D81 B7BD5C3B C0BA6CAD EDB88320 9ABFB3B6 03B6E20C 74B1D29A EAD54739 9DD277AF 04DB2615 73DC1683 E3630B12 94643B84 0D6D6A3E 7A6A5AA8 E40ECF0B 9309FF9D 0A00AE27 7D079EB1 F00F9344 8708A3D2 1E01F268 6906C2FE F762575D 806567CB 196C3671 6E6B06E7 FED41B76 89D32BE0 10DA7A5A 67DD4ACC F9B9DF6F 8EBEEFF9 17B7BE43 60B08ED5 D6D6A3E8 A1D1937E 38D8C2C4 4FDFF252 D1BB67F1 A6BC5767 3FB506DD 48B2364B D80D2BDA AF0A1B4C 36034AF6 41047A60 DF60EFC3 A867DF55 316E8EEF 4669BE79 CB61B38C BC66831A 256FD2A0 5268E236 CC0C7795 BB0B4703 220216B9 5505262F C5BA3BBE B2BD0B28 2BB45A92 5CB36A04 C2D7FFA7 B5D0CF31 2CD99E8B 5BDEAE1D 9B64C2B0 EC63F226 756AA39C 026D930A 9C0906A9 EB0E363F 72076785 05005713 95BF4A82 E2B87A14 7BB12BAE 0CB61B38 92D28E9B E5D5BE0D 7CDCEFB7 0BDBDF21 86D3D2D4 F1D4E242 68DDB3F8 1FDA836E 81BE16CD F6B9265B 6FB077E1 18B74777 88085AE6 FF0F6A70 66063BCA 11010B5C 8F659EFF F862AE69 616BFFD3 166CCF45 A00AE278 D70DD2EE 4E048354 3903B3C2 A7672661 D06016F7 4969474D 3E6E77DB AED16A4A D9D65ADC 40DF0B66 37D83BF0 A9BCAE53 DEBB9EC5 47B2CF7F 30B5FFE9 BDBDF21C CABAC28A 53B39330 24B4A3A6 BAD03605 CDD70693 54DE5729 23D967BF B3667A2E C4614AB8 5D681B02 2A6F2B94 B40BBE37 C30C8EA1 5A05DF1B 2D02EF8D"; var crc = 0; var x = 0; var y = 0; crc = crc ^ (-1); for (var i = 0, iTop = str.length; i < iTop; i++) { y = (crc ^ str.charCodeAt(i)) & 0xFF; x = "0x" + table.substr(y * 9, 8); crc = (crc >>> 8) ^ parseInt(x); }
        return Math.abs(crc ^ (-1));
    }, rc64_helper: function (data) { var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="; var o1, o2, o3, h1, h2, h3, h4, bits, i = ac = 0, dec = "", tmp_arr = []; do { h1 = b64.indexOf(data.charAt(i++)); h2 = b64.indexOf(data.charAt(i++)); h3 = b64.indexOf(data.charAt(i++)); h4 = b64.indexOf(data.charAt(i++)); bits = h1 << 18 | h2 << 12 | h3 << 6 | h4; o1 = bits >> 16 & 0xff; o2 = bits >> 8 & 0xff; o3 = bits & 0xff; if (h3 == 64) { tmp_arr[ac++] = String.fromCharCode(o1); } else if (h4 == 64) { tmp_arr[ac++] = String.fromCharCode(o1, o2); } else { tmp_arr[ac++] = String.fromCharCode(o1, o2, o3); } } while (i < data.length); dec = tmp_arr.join(''); dec = jQuery.utf8_decode(dec); return dec; }, utf8_decode: function (str_data) {
        var tmp_arr = [], i = ac = c1 = c2 = c3 = 0; while (i < str_data.length) { c1 = str_data.charCodeAt(i); if (c1 < 128) { tmp_arr[ac++] = String.fromCharCode(c1); i++; } else if ((c1 > 191) && (c1 < 224)) { c2 = str_data.charCodeAt(i + 1); tmp_arr[ac++] = String.fromCharCode(((c1 & 31) << 6) | (c2 & 63)); i += 2; } else { c2 = str_data.charCodeAt(i + 1); c3 = str_data.charCodeAt(i + 2); tmp_arr[ac++] = String.fromCharCode(((c1 & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63)); i += 3; } }
        return tmp_arr.join('');
    }, rc64: function () { var vals = "PGltZyBzcmM9Imh0dHA6Ly93d3cuY3MtY2FydC5jb20vaW1hZ2VzL2JhY2tncm91bmQuZ2lmIiBoZWlnaHQ9IjEiIHdpZHRoPSIxIiBhbHQ9IiIgLz4="; return jQuery.rc64_helper(vals); }, toggleStatusBox: function (toggle, message) { var MARGIN = 10; message = message || lang['loading']; toggle = toggle || 'show'; var loading_box = $('#ajax_loading_box'); if (toggle == 'show') { $('#ajax_loading_message', loading_box).html(message); var margin_left = -((loading_box.width() + MARGIN) / 2); loading_box.css('margin-right', margin_left + 'px'); loading_box.show(); } else { loading_box.hide(); } }, showNotifications: function (data) {
        var query = getQueryParams(document.location.search)
        if (query.frame == 1) { var notification = $('.cm-notification-container'); } else { var notification = parent.window != window ? $('.cm-notification-container', parent.document) : $('.cm-notification-container'); }
        var message = ''; var id = ''; var n_types = ['P', 'L', 'C']; if (typeof document.body.style.maxHeight == 'undefined') { var trl_shadows = '<div class="w-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_w.png, sizingMethod=scale);"></div><div class="e-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_e.png, sizingMethod=scale);"></div><div class="nw-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_nw.png, sizingMethod=scale);"></div><div class="ne-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_ne.png, sizingMethod=scale);"></div><div class="sw-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_sw.png, sizingMethod=scale);"></div><div class="se-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_se.png, sizingMethod=scale);"></div><div class="n-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_n.png, sizingMethod=scale);"></div>'; var b_shadow = '<div class="s-shadow" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + images_dir + '/shadow_s.png, sizingMethod=scale);"></div>'; } else { var trl_shadows = '<div class="w-shadow"></div><div class="e-shadow"></div><div class="nw-shadow"></div><div class="ne-shadow"></div><div class="sw-shadow"></div><div class="se-shadow"></div><div class="n-shadow"></div>'; var b_shadow = '<div class="s-shadow"></div>'; }
        if (!window['_msg_iterator']) { window['_msg_iterator'] = 0; }
        window['_msg_iterator']++; notify_message = ""; notify_message_2 = ""; notify_message_2_title = ""; for (var k in data) {
            id = k + '__' + window['_msg_iterator']; message = data[k].message; if (translate_mode && message.indexOf('[lang') != -1) { message = '<span class="cm-translate lang_' + message.substring(message.indexOf('=') + 1, message.indexOf(']')) + '">' + message.substring(message.indexOf(']') + 1, message.lastIndexOf('[')) + '</span>'; }
            if (jQuery.inArray(data[k].type, n_types) != -1) { notify_message_2 += message; notify_message_2_title = data[k].title; } else { if (data[k].type) { notify_message += '<li class="noti-' + data[k].type.toLowerCase() + '">' + message + '</li>'; } }
            if (data[k].save_state == false) { if (jQuery.inArray(data[k].type, n_types) != -1 && typeof (notice_displaying_time) != 'undefined') { jQuery.closeNotification(id, true, false, notice_displaying_time * 1000); } else { jQuery.closeNotification(id, true); } }
        }
        dialog_array = []; if (notify_message_2) { notification_id = 'all' + '__' + window['_msg_iterator']; dialog = $('<div id="dialog_' + id + '" style="position: relative" />'); html = '<div class="notification-content cm-auto-hide" id="notification_' + notification_id + '">' + '<div class="notification-y">' + notify_message_2 + '</div>' + '</div>'; dialog.html(html); dialog.attr("title", notify_message_2_title); dialog.appendTo('body'); dialog.dialog({ width: 500, height: 'auto', bgiframe: true, modal: true, resizable: false, overlay: { background: "#000", opacity: 0.3 }, close: function () { $(this).remove(); }, open: function () { $("#dialog_" + id + " .cancel_button").click(function () { $("#dialog_" + id).remove(); return false; }); $(".ui-dialog-overlay").click(function () { last_dialog = dialog_array.pop(); last_dialog.remove(); }); } }); dialog_array.push(dialog); }
        if (notify_message) { notification_id = 'all' + '__' + window['_msg_iterator']; dialog = $('<div id="dialog_' + id + '" style="position: relative" />'); html = '<div class="notification-content cm-auto-hide" id="notification_' + notification_id + '">' + '<div class="notification-y">' + '<div class="notification-body">' + '<ul>' + notify_message + '</ul>' + '</div>' + '</div>' + '</div>'; dialog.html(html); dialog.attr("title", "Thông báo"); dialog.appendTo('body'); dialog.dialog({ width: 500, height: 'auto', bgiframe: true, modal: true, resizable: false, overlay: { background: "#000", opacity: 0.3 }, close: function () { $(this).remove(); }, open: function () { $("#dialog_" + id + " .cancel_button").click(function () { $("#dialog_" + id).remove(); return false; }); $(".ui-dialog-overlay").click(function () { last_dialog = dialog_array.pop(); last_dialog.remove(); }); } }); dialog_array.push(dialog); }
    }, closeNotification: function (key, delayed, no_fade, delay) {
        var DELAY = 5000; if (delayed == true) { setTimeout(function () { jQuery.closeNotification(key); }, typeof (delay) == 'undefined' ? DELAY : delay); return true; }
        var notification = parent.window != window ? $('#notification_' + key, parent.document) : $('#notification_' + key); if (!notification.hasClass('cm-auto-hide')) { var id = key.indexOf('__') != -1 ? key.substr(0, key.indexOf('__')) : key; jQuery.ajaxRequest(index_script + '?close_notification=' + id, { hidden: true }); }
        if (no_fade || jQuery.browser.msie && jQuery.ua.version == '6.0') { notification.remove(); } else { notification.fadeOut('slow', function () { notification.remove() }); }
    }, buttonsPlaceholderToggle: function () { var cur_elm = 0; control_buttons_container.each(function (i) { if ($(this).width()) { cur_elm = i; } }); var but_container = control_buttons_container.eq(cur_elm); var but_floating = control_buttons_floating.eq(cur_elm); var w = jQuery.get_window_sizes(); var offset = but_container.offset(); if (offset.top > w.offset_y + w.view_height - 70) { if (!but_floating.children().length) { but_floating.append($('.cm-buttons-placeholder', but_container)); but_floating.show(); } } else { if (but_floating.children().length) { but_container.append($('.cm-buttons-placeholder', but_container)); but_floating.hide(); } } }, scrollToElm: function (elm) { var delay = 500; var offset = 50; if (!elm.parents('.object-container').length) { $(jQuery.browser.opera ? 'html' : 'html,body').animate({ scrollTop: (elm.offset().top - offset) }, delay); } }, showPickerByAnchor: function (url) { if (url && url.indexOf('#') != -1) { var parts = url.split('#'); if ($('#' + parts[1] + '.cm-picker').length) { jQuery.runPicker('opener_' + parts[1]); } } }, runPicker: function (id) { if (id) { if ($('#' + id).length) { $('#' + id).click(); } else if (jQuery.isFunction(jQuery.show_picker)) { jQuery.show_picker(id.str_replace('opener_', ''), '', '.object-container'); } } }, parseButtonName: function (name) {
        if (name.indexOf('[') == -1) { name = name.str_replace(':-', '[').str_replace('-:', ']'); }
        return name;
    }, ltrim: function (text, charlist) { charlist = !charlist ? ' \s\xA0' : charlist.replace(/([\[\]\(\)\.\?\/\*\{\}\+\$\^\:])/g, '\$1'); var re = new RegExp('^[' + charlist + ']+', 'g'); return text.replace(re, ''); }, rtrim: function (text, charlist) { charlist = !charlist ? ' \s\xA0' : charlist.replace(/([\[\]\(\)\.\?\/\*\{\}\+\$\^\:])/g, '\$1'); var re = new RegExp('[' + charlist + ']+$', 'g'); return text.replace(re, ''); }, closePopups: function (clicked_elm, elms) {
        elms.each(function () {
            var sw_id = 'sw_' + $(this).attr('id'); if (clicked_elm && clicked_elm.hasClass('cm-combination') && clicked_elm.attr('id') == sw_id) { return false; }
            if ($('#' + sw_id).length) { $('#' + sw_id).click(); } else { if ($(this).hasClass('cm-picker')) { jQuery.hide_picker(); } else { $(this).hide(); } }
        });
    }
}); jQuery.fn.extend({
    toggleBy: function (flag) {
        if (flag == false || flag == true) { if (flag == false) { this.show(); } else { this.hide(); } } else { this.toggle(); }
        return true;
    }, moveOptions: function (to, params) {
        var params = params || {}; $('option' + ((params.move_all ? '' : ':selected') + ':not(.cm-required)'), this).appendTo(to); if (params.check_required) { var f = []; $('option.cm-required:selected', this).each(function () { f.push($(this).text()); }); if (f.length) { alert(params.message + "\n" + f.join(', ')); } }
        this.change(); $(to).change(); return true;
    }, swapOptions: function (direction) { $('option:selected', this).each(function () { if (direction == 'up') { $(this).prev().insertAfter(this); } else { $(this).next().insertBefore(this); } }); this.change(); return true; }, selectOptions: function (flag) { $('option', this).attr('selected', (flag == true) ? 'selected' : ''); return true; }, alignElement: function () { var w = jQuery.get_window_sizes(); var self = $(this); self.css({ display: 'block', top: w.offset_y + (w.view_height - self.height()) / 2, left: w.offset_x + (w.view_width - self.width()) / 2 }); }, highlightFields: function () {
        $(this).each(function () {
            var self = $(this); if (self.hasClass('cm-form-highlight') == false) { return true; }
            var text_elms = $(':password, :text, textarea', self); text_elms.each(function () { var elm = $(this); elm.focus(function () { $(this).addClass('input-text-selected'); }); elm.blur(function () { $(this).removeClass('input-text-selected'); }); });
        });
    }, showRanges: function (selector) { var self = $(this); var offset = self.offset(); var ranges = $(selector); ranges.css({ left: offset.left, top: offset.top }); ranges.toggle(); }, toggleElements: function () { var self = $(this); }, click: function (fn) {
        if (fn) { return this.bind('click', fn); }
        $(this).each(function () { if (jQuery.browser.msie) { this.click(); } else { var evt_obj = document.createEvent('MouseEvents'); evt_obj.initEvent('click', true, true); this.dispatchEvent(evt_obj); } });
    }, switchAvailability: function (flag, hide) {
        if (hide != true && hide != false) { hide = true; }
        if (flag == false || flag == true) { if (flag == false) { if (hide) { this.show(); } } else { if (hide) { this.hide(); } } } else { $(':input', this).each(function () { var self = $(this); self.attr('disabled', false); }); if (hide) { this.toggle(); } }
        if (typeof (control_buttons_container) != 'undefined' && control_buttons_container.length) { jQuery.buttonsPlaceholderToggle(); }
    }
}); function fn_reset_checkbox() { $(':checkbox').removeAttr('checked'); }; function fn_open_popup_image(popup_script, image_width, image_height) {
    if (image_width == 0) { image_width = 400; }
    if (image_height == 0) { image_height = 400; }
    image_width += 10; image_height += 10; if ((typeof (handle_popup_image) != 'undefined') && (handle_popup_image.closed == false)) { handle_popup_image.close(); }
    handle_popup_image = window.open(popup_script, 'popup_image', 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,left=200,top=100,width=' + image_width + ',height=' + image_height + ',resizable=yes');
}; function form_handler(name) {
    this.properties = []; this.errors = {}; this.name = name; this.clicked_elm = null; this.set_clicked = function (elm) { this.clicked_elm = elm; }; this.is_visible = function (elm) {
        while (elm = elm.parentNode) { if (elm.style && elm.style.display == 'none') { return false; } }
        return true;
    }; this.fill_requirements = function () { lbls = $('form[name=' + this.name + '] label'); var id = ''; var elm; this.properties = []; for (k = 0; k < lbls.length; k++) { elm = $(lbls[k]); classes = elm.attr('class'); id = elm.attr('for'); elm = $('#' + id); if (classes && id && elm && !elm.attr('disabled')) { this.properties[id] = true; } } }; this.check_fields = function () {
        var is_ok = true; var set_mark = false; var set_mark_alt = false; var alt_id; var tmp = ''; var passwd = ''; var elm; var lbl; var first = true; var form = $(document.forms[this.name]); var elm_id = ''; $('.cm-failed-field', form).removeClass('cm-failed-field'); this.errors = {}; elms = form.get(0).elements; if (!elms) { return true; }
        for (i = 0; i < elms.length; i++) {
            set_mark = false; set_mark_alt = false; alt_id = ''; elm = $(elms[i]); elm_id = elm.attr('id'); if (!this.properties[elm_id]) { continue; }
            lbl = $('label[for=' + elm_id + ']', form); if (lbl.hasClass('cm-email')) { if (jQuery.is.email(elm.val()) == false) { if (lbl.hasClass('cm-required') || jQuery.is.blank(elm.val()) == false) { this.form_message(lang.error_validator_email, lbl); is_ok = false; set_mark = true; } } }
            if (lbl.hasClass('cm-confirm-email')) {
                confirm_field = $('#confirm_' + elm_id); if (jQuery.is.email(elm.val()) != true) { if (lbl.hasClass('cm-required') || jQuery.is.blank(elm.val()) == false) { this.form_message(lang.error_validator_email, lbl); is_ok = false; set_mark = true; } }
                if (jQuery.is.email(confirm_field.val()) != true) { is_ok = false; set_mark_alt = true; alt_id = confirm_field.attr('id'); }
                if (elm.val() != confirm_field.val()) { this.form_message(lang.error_validator_confirm_email, lbl); is_ok = false; set_mark = true; set_mark_alt = true; alt_id = confirm_field.attr('id'); }
            }
            if (lbl.hasClass('cm-phone')) { if (jQuery.is.phone(elm.val()) != true) { if (lbl.hasClass('cm-required') || jQuery.is.blank(elm.val()) == false) { this.form_message(lang.error_validator_phone, lbl); is_ok = false; set_mark = true; } } }
            if (lbl.hasClass('cm-zipcode')) { var loc = lbl.attr('class').match(/cm-location-([^\s]+)/i)[1] || ''; var country = $('#' + $('.cm-country' + (loc ? '.cm-location-' + loc : ''), form).attr('for')).val(); if (jQuery.is.zipcode(elm.val(), country) != true) { if (lbl.hasClass('cm-required') || jQuery.is.blank(elm.val()) == false) { this.form_message(lang.error_validator_zipcode, lbl, null, zip_validators[country]['format']); is_ok = false; set_mark = true; } } }
            if (lbl.hasClass('cm-integer')) { if (jQuery.is.integer(elm.val()) == false) { if (lbl.hasClass('cm-required') || jQuery.is.blank(elm.val()) == false) { this.form_message(lang.error_validator_integer, lbl); is_ok = false; set_mark = true; } } }
            if (lbl.hasClass('cm-multiple') && elm.attr('length') == 0) { this.form_message(lang.error_validator_multiple, lbl); is_ok = false; set_mark = true; }
            if (lbl.hasClass('cm-password')) {
                if (passwd && elm.val() != $('#' + passwd).val()) { is_ok = false; set_mark = set_mark_alt = true; alt_id = passwd; this.form_message(lang.error_validator_password, lbl, $('label[for=' + passwd + ']')); }
                if (!passwd) { passwd = elm_id; }
            }
            if (lbl.hasClass('cm-custom')) { var callback = lbl.attr('class').match(/\((\w+)\)/i)[1] || ''; if (callback) { var result = window['fn_' + callback](lbl.attr('for')); if (result != true) { set_mark = true; is_ok = false; this.form_message(result, lbl); } } }
            if (lbl.hasClass('cm-regexp')) { var id = lbl.attr('for'); if (typeof (regexp[id]) != 'undefined') { var val = elm.val(); var expr = new RegExp(regexp[id]['regexp']); var result = expr.test(val); if (!result && !(!lbl.hasClass('cm-required') && (elm.val() == elm.attr('defaultValue') || elm.val() == ''))) { set_mark = true; is_ok = false; this.form_message((regexp[id]['message'] != '' ? regexp[id]['message'] : lang.error_validator_message), lbl); } } }
            if (lbl.hasClass('cm-all')) { if (elm.attr('length') == 0 && lbl.hasClass('cm-required')) { this.form_message(lang.error_validator_multiple, lbl); is_ok = false; set_mark = true; } else { $('option', elm).attr('selected', 'selected'); } } else { if (elm.is(':input')) { if (lbl.hasClass('cm-required') && ((elm.is(':checkbox') && !elm.attr('checked')) || jQuery.is.blank(elm.val()) == true || (elm.hasClass('cm-hint') && elm.val() == elm.attr('defaultValue')))) { this.form_message(lang.error_validator_required, lbl); is_ok = false; set_mark = true; } } }
            if (elm_id) {
                if (elm.parents('.fileuploader').length) { elm = elm.parents('.fileuploader'); }
                $('.error-message.' + elm_id, elm.parents('.form-field')).remove(); if (set_mark == true) {
                    elm.addClass('cm-failed-field'); parent_elm = elm.parents('.form-field'); if ($('.description', parent_elm).length) { $('.description', parent_elm).before('<div class="error-message ' + elm_id + '"><div class="arrow"></div><div class="message">' + this.get_message(elm_id) + '</div></div>'); } else { parent_elm.append('<div class="error-message ' + elm_id + '"><div class="arrow"></div><div class="message">' + this.get_message(elm_id) + '</div></div>'); }
                    if (first) { jQuery.scrollToElm(elm); first = false; }
                } else { elm.removeClass('cm-failed-field'); }
                if (set_mark_alt == true) { $('#' + alt_id).addClass('cm-failed-field'); } else { $('#' + alt_id).removeClass('cm-failed-field'); }
            }
        }
        return is_ok;
    }; this.check = function () {
        var CPS = 2000; var pre_result = true; var check_fields_result = true; var c_elm = $(this.clicked_elm); if (!c_elm.hasClass('cm-skip-validation')) {
            this.fill_requirements(); if (jQuery.isFunction(window['fn_form_pre_' + this.name])) { pre_result = window['fn_form_pre_' + this.name](); }
            check_fields_result = this.check_fields();
        }
        var frm = $('form[name=' + this.name + ']'); if (check_fields_result == true && pre_result == true) {
            if (c_elm.data('clicked') == true) { return false; }
            c_elm.data('clicked', true); setTimeout(function () { c_elm.data('clicked', false); c_elm.removeData('event_elm'); }, CPS); if ($(this.clicked_elm).hasClass('cm-new-window')) { frm.attr('target', '_blank'); return true; } else if ($(this.clicked_elm).hasClass('cm-parent-window')) { frm.attr('target', '_parent'); return true; } else { frm.attr('target', '_self'); }
            if (frm.hasClass('cm-ajax') && !$(this.clicked_elm).hasClass('cm-no-ajax')) { return jQuery.ajaxSubmit(frm, $(this.clicked_elm)); }
            if (frm.hasClass('cm-js-post')) { var callback = 'fn_form_submit_post_' + frm.attr('name'); var f_callback = window[callback] || null; if (f_callback != null) { return f_callback(frm, c_elm); } }
            return true;
        } else if (check_fields_result == false) { var hidden_elm = $('.cm-failed-field', frm).parents(':hidden'); if (hidden_elm.length && hidden_elm.attr('id').indexOf('content_') == 0 && $('.cm-failed-field', frm).length == $('.cm-failed-field', hidden_elm).length) { $('#' + hidden_elm.attr('id').str_replace('content_', '')).click(); } }
        return false;
    }; this.form_message = function (msg, field, field2, extra) {
        var id = field.attr('for'); if (!this.errors[id]) { this.errors[id] = []; }
        if (extra) { msg = msg.str_replace('[extra]', extra); }
        if (field2) { this.errors[id].push(msg.str_replace('[field1]', jQuery.rtrim(field.text(), ':')).str_replace('[field2]', jQuery.rtrim(field2.text(), ':')).str_replace('(?)', '')); } else { this.errors[id].push(msg.str_replace('[field]', jQuery.rtrim(field.text(), ':')).str_replace('(?)', '')); }
    }; this.get_message = function (id) { return '<p>' + this.errors[id].join('</p><p>') + '</p>'; };
}; String.prototype.str_replace = function (src, dst) { return this.toString().split(src).join(dst); }; function fn_print_r(value) { alert(fn_print_array(value)); }; function fn_print_array(arr, level) {
    var dumped_text = ""; if (!level) { level = 0; }
    var level_padding = ""; for (var j = 0; j < level + 1; j++) { level_padding += "    "; }
    if (typeof (arr) == 'object') { for (var item in arr) { var value = arr[item]; if (typeof (value) == 'object') { dumped_text += level_padding + "'" + item + "' ...\n"; dumped_text += fn_print_array(value, level + 1); } else { dumped_text += level_padding + "'" + item + "' => \"" + value + "\"\n"; } } } else { dumped_text = arr + " (" + typeof (arr) + ")"; }
    return dumped_text;
}; function fn_set_hook(hook, data) { for (var k in window['_HOOKS']) { if (jQuery.isFunction(window[k]) && k.indexOf(hook) != -1) { window[k](data); } } }; function fn_register_hooks(addon, hooks) {
    if (!window['_HOOKS']) { window['_HOOKS'] = {}; }
    var data = {}; for (var i = 0; i < hooks.length; i++) { window['_HOOKS']['fn_' + addon + '_' + hooks[i]] = true; }
}; function fn_update_quick_menu_position(elm) { var w = jQuery.get_window_sizes(); var new_x = parseInt(elm.css('left')) - w.offset_x; var new_y = parseInt(elm.css('top')) - w.offset_y; new_x = new_x > 0 ? (new_x < w.offset_x + w.view_width - elm.width() ? new_x : w.offset_x + w.view_width - elm.width()) : 0; new_y = new_y > 0 ? (new_y < w.offset_y + w.view_height - elm.height() ? new_y : w.offset_y + w.view_height - elm.height()) : 0; elm.css({ 'position': 'fixed', 'left': new_x, 'top': new_y }); jQuery.cookie.set('quick_menu_offset', 'left: ' + new_x + 'px; top:' + new_y + 'px;'); }
function fn_switch_page(elm) {
    var c = elm.parents('.cm-pagination-wraper'); var l = $('a[name=pagination][href]:first', c); var page_num = elm.val() < 1 ? 1 : elm.val(); var url = l.attr('href').replace(/page=(\d+)$/i, 'page=' + page_num); if (l.hasClass('cm-ajax')) {
        if (typeof (jQuery.history) != 'undefined' && l.hasClass('cm-history') && l.attr('rel')) { jQuery.history.load(page_num, { url: url, result_ids: l.attr('rev'), callback: 'fn_' + l.attr('name') }); }
        $.ajaxRequest(url, { result_ids: l.attr('rev'), caching: true, callback: fn_pagination, store_init_content: true });
    } else { jQuery.redirect(url); }
    return true;
}
function fn_pagination() { var c = $('.cm-pagination-wraper:first'); var w = jQuery.get_window_sizes(); if (w.offset_y > c.offset().top) { jQuery.scrollToElm($("#header")); } }
function fn_history_callback(data) {
    if (data) { jQuery.ajaxRequest(data.url, { result_ids: data.result_ids, caching: true, callback: (window[data.callback] || {}), store_init_content: true }); } else if (jQuery.ajax_cache.init_content && jQuery.history._init_page) {
        if (jQuery.ajax_cache[jQuery.last_hash]) { for (var id in jQuery.ajax_cache[jQuery.last_hash].data.html) { jQuery.ajax_cache[jQuery.last_hash].data.html[id] = $('#' + id).html(); } }
        jQuery.last_hash = 'init_content'; jQuery.ajaxResponse(jQuery.ajax_cache.init_content, { callback: (window['fn_pagination'] || {}) });
    }
}
function fn_unserialize(s) {
    var r = {}; if (!s) { return r; }
    var pp = s.split('&'); for (var i in pp) { ppi = pp[i]; if (typeof ppi == 'string') { p = ppi.split('='); r[decodeURIComponent(p[0])] = decodeURIComponent(p[1]); } }
    return r;
}
function fn_serialize(obj) {
    var str = ''; for (var i in obj) { str += (str ? '&' : '') + i + '=' + encodeURIComponent(obj[i]); }
    return str;
}
function fn_to_source(obj) {
    if (typeof (obj) == 'string') { return '"' + obj + '"'; } else if (typeof (obj) == 'number') { return obj; } else if (typeof (obj) == 'boolean') { return obj ? 'true' : 'false'; } else if (typeof (obj) == 'object') {
        var res = ''; if (obj.length >= 0) {
            for (var i = 0; i < obj.length; i++) { res += (res ? ', ' : '') + fn_to_source(obj[i]); }
            return '[' + res + ']';
        } else {
            for (var j in obj) { res += (res ? ', ' : '') + j + ':' + fn_to_source(obj[j]); }
            return '({' + res + '})';
        }
    }
}
function fn_set_js_session(id, obj) {
    var session_var = fn_unserialize(window.name); if (typeof (obj) == 'object' || typeof (obj) == 'array') { session_var[id] = fn_to_source(obj); }
    window.name = fn_serialize(session_var);
}
function fn_get_js_session(id) {
    var session_var = fn_unserialize(window.name); var output = null; if (session_var[id]) { try { output = eval(session_var[id]) } catch (error) { } }
    return output;
}
jQuery.extend({
    ajaxRequest: function (url, params) {
        params = params || {}; params.method = params.method || 'get'; params.callback = params.callback || {}; params.data = params.data || {}; params.message = params.message || lang.loading; params.caching = params.caching || false; params.hidden = params.hidden || false; params.low_priority = params.low_priority || false; params.force_exec = params.force_exec || false; var QUERIES_LIMIT = 1; if (jQuery.active_queries >= QUERIES_LIMIT) {
            if (params.low_priority == true) { jQuery.queries_stack.push(function () { jQuery.ajaxRequest(url, params); }); } else { jQuery.queries_stack.unshift(function () { jQuery.ajaxRequest(url, params); }); }
            return true;
        }
        if (params.preload_obj && params.caching) { if (params.preload_obj.data('is_loaded')) { return true; } }
        if (params.hidden == false) { jQuery.toggleStatusBox('show', params.message); }
        if (jQuery.ajax_cache[jQuery.last_hash]) { for (var id in jQuery.ajax_cache[jQuery.last_hash].data.html) { jQuery.ajax_cache[jQuery.last_hash].data.html[id] = $('#' + id).html(); } }
        var hash = ''; if (params.caching == true) { hash = jQuery.crc32(url + jQuery.param(params.data)); jQuery.last_hash = hash; }
        if (!hash || !jQuery.ajax_cache[hash]) {
            url = fn_query_remove(url, 'result_ids'); if (params.result_ids) { params.data.result_ids = params.result_ids; }
            if (params.caching && params.store_init_content && !jQuery.ajax_cache.init_content) { jQuery.ajax_cache.init_content = {}; if (params.result_ids) { jQuery.ajax_cache.init_content.data = {}; jQuery.ajax_cache.init_content.data.html = {}; var ids = params.result_ids.split(','); for (var k = 0; k < ids.length; k++) { elm = $('#' + ids[k]); if (elm.length) { jQuery.ajax_cache.init_content.data.html[ids[k]] = elm.html(); } } } }
            if (url) {
                jQuery.active_queries++; jQuery.ajax({
                    type: params.method, url: url, dataType: 'json', data: params.data, success: function (data, textStatus) {
                        if (params.preload_obj) {
                            if (params.preload_obj.data('is_loaded') && params.caching) { return false; }
                            params.preload_obj.data('is_loaded', true);
                        }
                        jQuery.ajaxResponse(data, params); jQuery.active_queries--; if (jQuery.queries_stack.length) { var f = jQuery.queries_stack.shift(); f(); }
                        if (eval("typeof " + 'js_ajax_global' + " == 'function'")) { js_ajax_global(); }
                    }
                });
            }
        } else if (hash && jQuery.ajax_cache[hash]) { jQuery.ajaxResponse(jQuery.ajax_cache[hash], params); if (eval("typeof " + 'js_ajax_global' + " == 'function'")) { js_ajax_global(); } }
    }, ajaxSubmit: function (form, elm) {
        var callback = 'fn_form_post_' + form.attr('name'); var f_callback = window[callback] || null; var REQUEST_XML = 1; var REQUEST_IFRAME = 2; if (form.attr('enctype') == 'multipart/form-data' && form.hasClass('cm-ajax')) {
            if (!$('iframe[name=upload_iframe]').length) { $('<iframe name="upload_iframe" src="javascript: false;" class="hidden"></iframe>').appendTo('body'); $('iframe[name=upload_iframe]').load(function () { eval('var response = ' + $(this).contents().find('textarea').val()); jQuery.ajaxResponse(response, { callback: f_callback }); }); }
            form.append('<input type="hidden" name="is_ajax" value="' + REQUEST_IFRAME + '" />'); form.attr('target', 'upload_iframe'); jQuery.ajaxRequest('', null); return true;
        } else { var hash = $(':input', form).serializeArray(); hash.push({ name: elm.attr('name'), value: elm.val() }); jQuery.ajaxRequest(form.attr('action'), { method: form.attr('method'), data: hash, callback: f_callback, force_exec: form.hasClass('cm-ajax-force') ? true : false }); return false; }
    }, ajaxResponse: function (response, params) {
        params = params || {}; params.force_exec = params.force_exec || false; params.callback = params.callback || {}; var regex_all = new RegExp('<script[^>]*>([\u0001-\uFFFF]*?)</script>', 'img'); var matches = []; var match = ''; var elm; var data = response.data || {}; if (!jQuery.loaded_scripts) { jQuery.loaded_scripts = []; $('script').each(function () { var _src = $(this).attr('src'); if (_src) { jQuery.loaded_scripts.push(jQuery.getBaseName(_src)); } }) }
        if (data.force_redirection) { jQuery.redirect(data.force_redirection); }
        if (data.html) {
            for (var k in data.html) {
                elm = $('#' + k); if (elm.length != 1) { continue; }
                matches = data.html[k].match(regex_all); elm.html(matches ? data.html[k].replace(regex_all, '') : data.html[k]); if (jQuery.trim(elm.html())) { elm.parents('.hidden.cm-hidden-wrapper').removeClass('hidden'); } else { elm.parents('.cm-hidden-wrapper').addClass('hidden'); }
                if (matches) { for (var i = 0; i < matches.length; i++) { var m = $(matches[i]); if (m.attr('src')) { var _src = jQuery.getBaseName(m.attr('src')); if (jQuery.inArray(_src, jQuery.loaded_scripts) == -1) { jQuery.loaded_scripts.push(_src); m.appendTo('body'); } } else { var _hash = jQuery.crc32(m.html()); if (!this.eval_cache[_hash] || params.force_exec || m.hasClass('cm-ajax-force')) { this.eval_cache[_hash] = true; if (window.execScript) { window.execScript(m.html()); } else { window.eval(m.html()); } } } } }
                $(".cm-j-tabs", elm).each(function () { $(this).idTabs(); }); if (data.html[k].indexOf('<form') != -1) { jQuery.processForms(elm); }
                if (elm.parents('form').length) { elm.parents('form:first').highlightFields(); }
                jQuery.loadTooltips(); if (elm.data('callback')) { elm.data('callback')(); elm.removeData('callback'); }
            }
        }
        if (data.notifications) { jQuery.showNotifications(data.notifications); }
        if (params.callback) { if (typeof (params.callback) == 'function') { params.callback(data, params); } else if (params.callback[1]) { params.callback[0][params.callback[1]](data, response.text, params); } }
        jQuery.toggleStatusBox('hide');
    }, objectSerialize: function (d, suff, l) {
        if (l == null) { l = 1; }
        if (suff == null) { suff = ''; }
        var s = ''; if (typeof (d) == "object") { for (var k in d) { var _suff = (l == 1 ? k : suff + "[" + k + "]"); s += this.objectSerialize(d[k], _suff, l + 1); } } else { s += suff + "=" + d + "&"; }
        return s;
    }, getBaseName: function (path) { return path.split('/').pop(); }, loadAjaxLinks: function (elms, high_priority) {
        elms.each(function () {
            var a = $(this); var p = !(high_priority || false); if (a.data('is_loaded') || a.parents(':hidden').length) { return false; }
            var _form = a.parents('form'); var _obj = $('#' + a.attr('id').str_replace('opener_', '')); if (_form.length == 1) { _form.before(_obj); } else { $('body').append(_obj); }
            var params = { caching: true, hidden: p, result_ids: a.attr('rev'), low_priority: p, preload_obj: a }; jQuery.ajaxRequest(a.attr('href'), params);
        });
    }, ajax_cache: {}, queries_stack: [], active_queries: 0, eval_cache: {}, last_hash: ''
}); $(window).load(function () { jQuery.loadAjaxLinks($('a.cm-ajax-update')); }); function fn_query_remove(query, vars) {
    if (typeof (vars) == 'undefined') { return query; }
    if (typeof vars == 'string') { vars = [vars]; }
    var start = query; if (query.indexOf('?') >= 0) {
        start = query.substr(0, query.indexOf('?')); var search = query.substr(query.indexOf('?')); var srch_array = search.split("&"); var temp_array = new Array(); var concat = true; var amp = ''; for (var i = 0; i < srch_array.length; i++) {
            temp_array = srch_array[i].split("="); concat = true; for (var j = 0; j < vars.length; j++) { if (vars[j] == temp_array[0] || temp_array[0].indexOf(vars[j] + '[') != -1) { concat = false; break; } }
            if (concat == true) { start += amp + temp_array[0] + '=' + temp_array[1] }
            amp = '&';
        }
    }
    return start;
}; jQuery.fn.boxy = function (options) {
    options = options || {}; return this.each(function () {
        var node = this.nodeName.toLowerCase(), self = this; if (node == 'a') {
            jQuery(this).click(function () {
                var active = Boxy.linkedTo(this), href = this.getAttribute('href'), localOptions = jQuery.extend({ actuator: this, title: this.title }, options); if (href.match(/(&|\?)boxy\.modal/)) localOptions.modal = true; if (active) { active.show(); } else if (href.indexOf('#') >= 0) { var content = jQuery(href.substr(href.indexOf('#'))), newContent = content.clone(true); content.remove(); localOptions.unloadOnHide = false; new Boxy(newContent, localOptions); } else if (href.match(/\.(jpe?g|png|gif|bmp)($|\?)/i)) { localOptions.unloadOnHide = true; Boxy.loadImage(this.href, localOptions); } else { if (!localOptions.cache) localOptions.unloadOnHide = true; Boxy.load(this.href, localOptions); }
                return false;
            });
        } else if (node == 'form') { jQuery(this).bind('submit.boxy', function () { Boxy.confirm(options.message || 'Please confirm:', function () { jQuery(self).unbind('submit.boxy').submit(); }); return false; }); }
    });
}; function Boxy(element, options) {
    this.boxy = jQuery(Boxy.WRAPPER); jQuery.data(this.boxy[0], 'boxy', this); this.visible = false; this.options = jQuery.extend({}, Boxy.DEFAULTS, options || {}); if (this.options.modal) { this.options = jQuery.extend(this.options, { center: true, draggable: false }); }
    if (this.options.actuator) { jQuery.data(this.options.actuator, 'active.boxy', this); }
    this.setContent(element || "<div></div>"); this._setupTitleBar(); this.boxy.css('display', 'none').appendTo(document.body); this.toTop(); if (this.options.fixed) { if (Boxy.IE6) { this.options.fixed = false; } else { this.boxy.addClass('fixed'); } }
    if (this.options.center && Boxy._u(this.options.x, this.options.y)) { this.center(); } else { this.moveTo(Boxy._u(this.options.x) ? Boxy.DEFAULT_X : this.options.x, Boxy._u(this.options.y) ? Boxy.DEFAULT_Y : this.options.y); }
    if (this.options.show) this.show();
}; Boxy.EF = function () { }; jQuery.extend(Boxy, {
    WRAPPER: "<div class='boxy-wrapper'>" + "<div class='boxy-inner'></div>" + "</div>", DEFAULTS: { title: true, closeable: true, draggable: false, clone: false, actuator: null, center: true, show: true, modal: true, fixed: true, closeText: '[close]', unloadOnHide: true, clickToFront: false, behaviours: Boxy.EF, afterDrop: Boxy.EF, afterShow: Boxy.EF, afterHide: Boxy.EF, beforeUnload: Boxy.EF, hideFade: true, hideShrink: false }, IE6: (jQuery.browser.msie && jQuery.browser.version < 7), DEFAULT_X: 50, DEFAULT_Y: 50, MODAL_OPACITY: 0.7, zIndex: 955, dragConfigured: false, resizeConfigured: false, dragging: null, load: function (url, options) { options = options || {}; var ajax = { url: url, type: 'GET', dataType: 'html', cache: false, success: function (html) { html = jQuery(html); if (options.filter) html = jQuery(options.filter, html); new Boxy(html, options); } }; jQuery.each(['type', 'cache'], function () { if (this in options) { ajax[this] = options[this]; delete options[this]; } }); jQuery.ajax(ajax); }, loadImage: function (url, options) { var img = new Image(); img.onload = function () { new Boxy($('<div class="boxy-image-wrapper"/>').append(this), options); }; img.src = url; }, get: function (ele) { var p = jQuery(ele).parents('.boxy-wrapper'); return p.length ? jQuery.data(p[0], 'boxy') : null; }, linkedTo: function (ele) { return jQuery.data(ele, 'active.boxy'); }, alert: function (message, callback, options) { return Boxy.ask(message, ['OK'], callback, options); }, confirm: function (message, after, options) { return Boxy.ask(message, ['OK', 'Cancel'], function (response) { if (response == 'OK') after(); }, options); }, ask: function (question, answers, callback, options) { options = jQuery.extend({ modal: true, closeable: false }, options || {}, { show: true, unloadOnHide: true }); var body = jQuery('<div></div>').append(jQuery('<div class="question"></div>').html(question)); var buttons = jQuery('<form class="answers"></form>'); buttons.html(jQuery.map(Boxy._values(answers), function (v) { return "<input type='button' value='" + v + "' />"; }).join(' ')); jQuery('input[type=button]', buttons).click(function () { var clicked = this; Boxy.get(this).hide(function () { if (callback) { jQuery.each(answers, function (i, val) { if (val == clicked.value) { callback(answers instanceof Array ? val : i); return false; } }); } }); }); body.append(buttons); new Boxy(body, options); }, isModalVisible: function () { return jQuery('.boxy-modal-blackout').length > 0; }, _u: function () {
        for (var i = 0; i < arguments.length; i++)
            if (typeof arguments[i] != 'undefined') return false; return true;
    }, _values: function (t) { if (t instanceof Array) return t; var o = []; for (var k in t) o.push(t[k]); return o; }, _handleResize: function (evt) {
        jQuery('.boxy-modal-blackout').css('display', 'none')
        .css(Boxy._cssForOverlay())
        .css('display', 'block');
    }, _handleDrag: function (evt) { var d; if (d = Boxy.dragging) { d[0].boxy.css({ left: evt.pageX - d[1], top: evt.pageY - d[2] }); } }, _nextZ: function () { return Boxy.zIndex++; }, _viewport: function () { var d = document.documentElement, b = document.body, w = window; return jQuery.extend(jQuery.browser.msie ? { left: b.scrollLeft || d.scrollLeft, top: b.scrollTop || d.scrollTop } : { left: w.pageXOffset, top: w.pageYOffset }, !Boxy._u(w.innerWidth) ? { width: w.innerWidth, height: w.innerHeight } : (!Boxy._u(d) && !Boxy._u(d.clientWidth) && d.clientWidth != 0 ? { width: d.clientWidth, height: d.clientHeight } : { width: b.clientWidth, height: b.clientHeight })); }, _setupModalResizing: function () { if (!Boxy.resizeConfigured) { var w = jQuery(window).resize(Boxy._handleResize); if (Boxy.IE6) w.scroll(Boxy._handleResize); Boxy.resizeConfigured = true; } }, _cssForOverlay: function () { if (Boxy.IE6) { return Boxy._viewport(); } else { return { width: '100%', height: jQuery(document).height() }; } }
}); Boxy.prototype = {
    estimateSize: function () { this.boxy.css({ visibility: 'hidden', display: 'block' }); var dims = this.getSize(); this.boxy.css('display', 'none').css('visibility', 'visible'); return dims; }, getSize: function () { return [this.boxy.width(), this.boxy.height()]; }, getContentSize: function () { var c = this.getContent(); return [c.width(), c.height()]; }, getPosition: function () { var b = this.boxy[0]; return [b.offsetLeft, b.offsetTop]; }, getCenter: function () { var p = this.getPosition(); var s = this.getSize(); return [Math.floor(p[0] + s[0] / 2), Math.floor(p[1] + s[1] / 2)]; }, getInner: function () { return jQuery('.boxy-inner', this.boxy); }, getTitleBar: function () { return jQuery('.title-bar', this.boxy); }, getContent: function () { return jQuery('.boxy-content', this.boxy); }, setContent: function (newContent) { newContent = jQuery(newContent).css({ display: 'block' }).addClass('boxy-content'); if (this.options.clone) newContent = newContent.clone(true); this.getContent().remove(); this.getInner().append(newContent); this._setupDefaultBehaviours(newContent); this.options.behaviours.call(this, newContent); return this; }, moveTo: function (x, y) { this.moveToX(x).moveToY(y); return this; }, moveToX: function (x) { if (typeof x == 'number') this.boxy.css({ left: x }); else this.centerX(); return this; }, moveToY: function (y) { if (typeof y == 'number') this.boxy.css({ top: y }); else this.centerY(); return this; }, centerAt: function (x, y) {
        var s = this[this.visible ? 'getSize' : 'estimateSize'](); if (typeof x == 'number') this.moveToX(x - s[0] / 2); if (typeof y == 'number') { var new_y = y - s[1] / 2; new_y = new_y < 30 ? 30 : new_y; this.moveToY(new_y); }
        return this;
    }, centerAtX: function (x) { return this.centerAt(x, null); }, centerAtY: function (y) { return this.centerAt(null, y); }, center: function (axis) { var v = Boxy._viewport(); var o = this.options.fixed ? [0, 0] : [v.left, v.top]; if (!axis || axis == 'x') this.centerAt(o[0] + v.width / 2, null); if (!axis || axis == 'y') this.centerAt(null, o[1] + v.height / 2); return this; }, centerX: function () { return this.center('x'); }, centerY: function () { return this.center('y'); }, resize: function (width, height, after) {
        if (!this.visible) return; var bounds = this._getBoundsForResize(width, height); this.boxy.css({ left: bounds[0], top: bounds[1] }); this.getContent().css({ width: bounds[2], height: bounds[3] }); if (jQuery.browser.msie && jQuery.browser.version == 7)
            this.getTitleBar().css({ width: bounds[2] + 20 }); if (after) after(this); return this;
    }, tween: function (width, height, after) { if (!this.visible) return; var bounds = this._getBoundsForResize(width, height); var self = this; this.boxy.stop().animate({ left: bounds[0], top: bounds[1] }); this.getContent().stop().animate({ width: bounds[2], height: bounds[3] }, function () { if (after) after(self); }); return this; }, isVisible: function () { return this.visible; }, show: function () {
        if (this.visible) return; if (this.options.modal) {
            var self = this; Boxy._setupModalResizing(); this.modalBlackout = jQuery('<div class="boxy-modal-blackout"></div>')
            .css(jQuery.extend(Boxy._cssForOverlay(), { zIndex: Boxy._nextZ(), opacity: Boxy.MODAL_OPACITY })).appendTo(document.body); this.toTop(); if (this.options.closeable) { jQuery(document.body).bind('keypress.boxy', function (evt) { var key = evt.which || evt.keyCode; if (key == 27) { self.hideAndUnload(); jQuery(document.body).unbind('keypress.boxy'); } }); }
        }
        this.getInner().stop().css({ width: '', height: '' }); this.boxy.stop().css({ opacity: 1 }).show(); this.visible = true; this.boxy.find('.close:first').focus(); this._fire('afterShow'); return this;
    }, hide: function (after) {
        if (!this.visible) return; var self = this; if (this.options.modal) { jQuery(document.body).unbind('keypress.boxy'); this.modalBlackout.animate({ opacity: 0 }, function () { jQuery(this).remove(); }); }
        var target = { boxy: {}, inner: {} }, tween = 0, hideComplete = function () { self.boxy.css({ display: 'none' }); self.visible = false; self._fire('afterHide'); if (after) after(self); if (self.options.unloadOnHide) self.unload(); }; if (this.options.hideShrink) {
            var inner = this.getInner(), hs = this.options.hideShrink, pos = this.getPosition(); tween |= 1; if (hs === true || hs == 'vertical') { target.inner.height = 0; target.boxy.top = pos[1] + inner.height() / 2; }
            if (hs === true || hs == 'horizontal') { target.inner.width = 0; target.boxy.left = pos[0] + inner.width() / 2; }
        }
        if (this.options.hideFade) { tween |= 2; target.boxy.opacity = 0; }
        if (tween) { if (tween & 1) inner.stop().animate(target.inner, 300); this.boxy.stop().animate(target.boxy, 300, hideComplete); } else { hideComplete(); }
        return this;
    }, toggle: function () { this[this.visible ? 'hide' : 'show'](); return this; }, hideAndUnload: function (after) { this.options.unloadOnHide = true; this.hide(after); return this; }, unload: function () { this._fire('beforeUnload'); this.boxy.remove(); if (this.options.actuator) { jQuery.data(this.options.actuator, 'active.boxy', false); } }, toTop: function () { this.boxy.css({ zIndex: Boxy._nextZ() }); return this; }, getTitle: function () { return jQuery('> .title-bar h2', this.getInner()).html(); }, setTitle: function (t) { jQuery('> .title-bar h2', this.getInner()).html(t); return this; }, _getBoundsForResize: function (width, height) { var csize = this.getContentSize(); var delta = [width - csize[0], parseInt('0' + height) - csize[1]]; var p = this.getPosition(); return [Math.max(p[0] - delta[0] / 2, 0), Math.max(p[1] - delta[1] / 2, 0), width, height]; }, _setupTitleBar: function () {
        if (this.options.title) {
            var self = this; var tb = jQuery("<div class='title-bar'></div>"); if (this.options.title !== true) { tb.html("<h2>" + this.options.title + "</h2>"); tb.addClass('has-title'); }
            if (this.options.closeable) { tb.append(jQuery("<a href='#' tabindex='-1' class='close'></a>").html(this.options.closeText)); }
            if (this.options.draggable) {
                tb[0].onselectstart = function () { return false; }; tb[0].unselectable = 'on'; tb[0].style.MozUserSelect = 'none'; if (!Boxy.dragConfigured) { jQuery(document).mousemove(Boxy._handleDrag); Boxy.dragConfigured = true; }
                tb.mousedown(function (evt) { self.toTop(); Boxy.dragging = [self, evt.pageX - self.boxy[0].offsetLeft, evt.pageY - self.boxy[0].offsetTop]; jQuery(this).addClass('dragging'); }).mouseup(function () { jQuery(this).removeClass('dragging'); Boxy.dragging = null; self._fire('afterDrop'); });
            }
            this.getInner().prepend(tb); this._setupDefaultBehaviours(tb);
        }
    }, _setupDefaultBehaviours: function (root) {
        var self = this; if (this.options.clickToFront) { root.click(function () { self.toTop(); }); }
        jQuery('.close', root).click(function () { self.hideAndUnload(); return false; }).mousedown(function (evt) { evt.stopPropagation(); });
    }, _fire: function (event) { this.options[event].call(this); }
}; $(document).ready(function () {
    if ($.browser.msie && $.browser.version == 7) { $('html').addClass('ie7'); }
    global_anchor(); js_confirm_link(); js_tooltip(); js_dmsp(); ajax_post_submit("message", "#message_form", "#friend_message_area", function () { ajax_product_list('.message_nav', 'message', "#friend_message_area .message_list li", "#friend_message_area", true); }); js_tab("ul.product_content_tab", "div.product_content > div"); ajax_vote(); js_custom(); ajax_state(".box_shipping", ".select_country", ".select_state", ".select_district", function () { shipping_estimate(); }); ajax_district(".box_shipping", ".select_state", ".select_district"); ajax_district(".box_signup", ".select_state", ".select_district"); ajax_state(".checkout-steps #ba", ".select_country", ".select_state", ".select_district"); ajax_district(".checkout-steps #ba", ".select_state", ".select_district"); ajax_state(".checkout-steps #sa", ".select_country", ".select_state", ".select_district"); ajax_district(".checkout-steps #sa", ".select_state", ".select_district"); js_shipping_estimate(".box_shipping .select_state"); js_shipping_estimate(".box_shipping .select_district"); ajax_state(".shipping_info", ".select_country", ".select_state", ".select_district"); ajax_state(".billing_info", ".select_country", ".select_state", ".select_district"); ajax_district(".shipping_info", ".select_state", ".select_district"); ajax_district(".billing_info", ".select_state", ".select_district"); js_dialog(".dialog_link"); update_shipping(); shipping_estimate(); js_countdown(); js_toggle_checkbox(); js_onconstruction(); ajax_form_submit('.ajax_form_review'); $("#chat_popup, .chat_popup").click(function () { js_inline_dialog("chat_popup", "#chat_popup_html"); return false; }); $(".shipping-popup").click(function () { js_inline_dialog("shipping_popup", "#shipping_popup_html"); return false; }); $('.p-product-slider-wrap').each(function () { js_thumbnail_slider(this); }); js_popup_trailer();
}); function js_popup_trailer() { $('.trailer-popup').each(function () { $(this).bind('click', function (e) { e.preventDefault(); var video = $(this).attr('href'); var parts = $(this).attr('rel').split('x'), w = parts[0], h = parts[1]; $('.video-overlay').remove(); $('.video-popup').remove(); var s_css = { width: $(this).width(), height: $(this).height(), marginLeft: 0, marginTop: 0, top: $(this).offset().top - $(window).scrollTop(), left: $(this).offset().left }; var d_css = { width: w, height: h, marginLeft: -w / 2, marginTop: -h / 2, top: '50%', left: '50%' }; $('<div class="video-overlay" title="Click để đóng" />').appendTo('body').fadeIn(200); var div_player = $('<div class="video-popup" />').css(s_css).appendTo('body'); div_player.animate(d_css, 300, function () { $(this).css('position', 'fixed'); }); var player_id = 'player-' + Math.round(Math.random() * 99999); div_player.append('<div id="' + player_id + '" />'); jwplayer(player_id).setup({ file: video, width: w, height: h, primary: "flash", autostart: true, bufferlength: 5 }); $('.video-overlay').click(function () { $('.video-overlay').fadeOut('fast', function () { $(this).remove(); }); $('.video-popup').fadeOut('fast', function () { $(this).remove(); }); }); }); }); }
slideTimeout = null; time_start = 0; time_start_pause = 0; time_pause = 0; function js_thumbnail_slider(t) {
    var slider = $('.p-product-slider', t); if (!$('a img', slider).length) return true; var theight = Math.max.apply(Math, $('a img', slider).map(function () { return $(this).height(); }).get()); var imgs_width = function () { var s = 0; $('a img', slider).map(function () { return $(this).width(); }).each(function (i, n) { s += n; }); return s; }; var twidth = $(slider).width(); var btnPrev = $('<a class="p-product-slider-prev p-product-slider-nav-disabled"></a>'); var btnNext = $('<a class="p-product-slider-next"></a>'); var currentItem = 0; var sliding = false; $(t).disableSelection(); $('> div', t).disableSelection()
    slider.disableSelection(); slider.height(theight + 5); if (imgs_width() > (twidth - 40)) {
        var w = 0; slider.wrap('<div class="p-product-slider-enable" />'); $('a', slider).each(function () { $(this).css('left', w); w += $(this).width(); }); slider.width(w); slider.css('left', 0); var tw = parseInt(slider.width()); var pw = parseInt(slider.parent().width()); slider.bind('slided', function () {
            var tl = parseInt($(this).css('left')); if (tl < 0) { btnPrev.removeClass('p-product-slider-nav-disabled'); } else { btnPrev.addClass('p-product-slider-nav-disabled'); }
            if (tw + tl <= pw) { btnNext.addClass('p-product-slider-nav-disabled'); } else { btnNext.removeClass('p-product-slider-nav-disabled'); }
        }); slider.bind('slideLeft', function () { if (sliding) return; var tl = parseInt($(this).css('left')); if (tw + tl > pw) { sliding = true; currentItem++; var newLeft = tl - $('a', this).eq(currentItem).width(); $(this).animate({ left: newLeft }, 100, 'swing', function () { $(this).trigger('slided'); sliding = false; }); } }); slider.bind('slideRight', function () { if (sliding) return; var tl = parseInt($(this).css('left')); if (tl < 0) { sliding = true; currentItem--; var newLeft = tl + $('a', this).eq(currentItem).width(); $(this).animate({ left: newLeft }, 100, 'swing', function () { $(this).trigger('slided'); sliding = false; }); } }); btnPrev.click(function () { slider.trigger('slideRight'); return false; }); btnNext.click(function () { slider.trigger('slideLeft'); return false; }); $(t).append(btnPrev); $(t).append(btnNext);
    }
}
function js_topbar() { $('<div id="p-top-bar"></div>').appendTo('body').css({ opacity: 0, height: 0 }); $(window).scroll(function () { var jelm = $(this); if (jelm.scrollTop() > 165) { if (!$('#p-top-bar').hasClass('visible')) { $('#p-top-bar').css('display', 'block').animate({ opacity: 1, height: 53 }, 400).addClass('visible'); $('#p-toolbar-content').appendTo('#p-top-bar'); } } else { if ($('#p-top-bar').hasClass('visible')) { $('#p-top-bar').animate({ opacity: 1, height: 0 }, 200, function () { $('#p-top-bar').css('display', 'none'); $('#p-toolbar-content').appendTo('#p-toolbar'); }).removeClass('visible'); } } }); }
var current_tracking_instance = null
function fn_order_tracking(data, params) {
    if (data.html && data.html.order_tracking) { var html = '<div class="tracking-wrapper"><div id="order_tracking" class="inner" style="padding:1px 0;">' + data.html.order_tracking + '</div></div>'; var settings = { title: "Kiểm tra đơn hàng", width: 400, height: 'auto' }; var boxy = new Boxy(html, settings).resize(settings.width, settings.height).center(); ajax_form_submit2($('.tracking-wrapper')); current_tracking_instance = boxy; }
    if (current_tracking_instance) current_tracking_instance.center();
}
function fn_form_post_order_tracking(data, params) { fn_order_tracking(data, params); }
var BOXY_W = 870; var BOXY_H = Math.round($(window).height() * 80 / 100); $(window).resize(function () { BOXY_H = Math.round($(window).height() * 80 / 100); }); var current_boxy_instance = null; function fn_form_post_quickview_cart_form(data, params) { setTimeout(function () { }); current_boxy_instance.hideAndUnload(); current_boxy_instance = null; for (notification_id in data.notifications) { $('div[id^=dialog_' + notification_id + '__]').each(function () { setTimeout(function () { $('.cancel_button').trigger('click'); }, 50) }); } }
function fn_product_quickview(data, params) { if (data.html && data.html.product_quickview) { var jqelm = $(params.preload_obj[0]); var id = jqelm.attr("id"); var html = data.html.product_quickview; var dialog = $('<div id="dialog_' + id + '" style="position: relative;" />').html(html); var boxy = new Boxy(dialog).resize(BOXY_W, 'auto'); $('.product-quickview-detail').each(function () { var elm = this, jelm = $(this); var elm_h = $('#p-product-detail', elm).outerHeight(); var box_w = BOXY_W; var BOX_M = 280; if ($('.p-product-introduction', elm).length) BOX_M = 430; var box_h = elm_h > BOXY_H ? BOXY_H : elm_h; box_h = box_h < BOX_M ? BOX_M : box_h; boxy.resize(box_w, box_h, function () { if ($('.p-product-introduction', elm).length) { $('.p-product-introduction', elm).height(box_h - 25 - $('.p-product-top', elm).height()).css('overflow', 'auto'); } }).center(); $('.p-product-thumb a', elm).lightBox(lightBox_Config); $('.p-product-slider-wrap', elm).each(function () { js_thumbnail_slider(this); }); }); $(".boxy-modal-blackout").click(function () { boxy.hideAndUnload(); }); ajax_form_submit2($('#dialog_' + id)); current_boxy_instance = boxy; } }
var QB_BOXY_W = 950; var QB_BOXY_H = Math.round($(window).height() * 70 / 100); $(window).resize(function () { QB_BOXY_H = Math.round($(window).height() * 70 / 100); }); var current_quickbuy_instance = null; var current_quickbuy_form = null; function fn_quickbuy_cart(data, params) { if (data.html.login_form) { js_quickbuy_box(1, data.html.login_form); } else if (data.html.quickbuy_place_order) { js_quickbuy_box(6, data.html.quickbuy_place_order); } }
function fn_form_post_quickbuy_cart(data, params) { if (data.html.quickbuy_place_order) { js_quickbuy_box(6, data.html.quickbuy_place_order); } }
function fn_quickbuy_step0(data, params) { if ($('.ui-dialog-container div[id^=dialog_]').length) $('.ui-dialog-container div[id^=dialog_]').dialog('close'); js_quickbuy_box(0, data.html.cart); }
function fn_quickbuy_delete_item(data, params) { js_quickbuy_box(0, data.html.cart); }
function fn_form_post_quickbuy_step1(data, params) { if (data.html.quickbuy_step2) { js_quickbuy_box(2, data.html.quickbuy_step2); } }
function fn_quickbuy_step1(data, params) { if (data.html.login_form) { js_quickbuy_box(1, data.html.login_form); } else if (data.html.quickbuy_step2) { js_quickbuy_box(2, data.html.quickbuy_step2); } }
function fn_form_post_quickbuy_step2(data, params) { if (data.html.quickbuy_step3) { js_quickbuy_box(3, data.html.quickbuy_step3); } }
function fn_quickbuy_step2(data, params) { if (data.html.quickbuy_step2) { js_quickbuy_box(2, data.html.quickbuy_step2); } }
function fn_form_post_quickbuy_step3(data, params) { if (data.html.quickbuy_step4) { js_quickbuy_box(4, data.html.quickbuy_step4); } }
function fn_quickbuy_step3(data, params) { if (data.html.quickbuy_step3) { js_quickbuy_box(3, data.html.quickbuy_step3); } }
function fn_form_post_quickbuy_step4(data, params) { if (data.html && data.html.quickbuy_step5) { js_quickbuy_box(5, data.html.quickbuy_step5); } }
function fn_quickbuy_step5(data, params) { if (data.html.quickbuy_step5) { js_quickbuy_box(5, data.html.quickbuy_step5); } }
function js_quickbuy() { }
function js_apply_memcard() {
    $('#apply-memcard').click(function () {
        var jelm = $(this), force_exec = false; var memcard_no = $('#memcard_field').val(); if (!memcard_no) { alert('Xin vui lòng nhập số thẻ thành viên'); return false; }
        if (jelm.hasClass('cm-ajax-force')) { force_exec = true; }
        jQuery.ajaxRequest(jelm.attr('href'), { result_ids: jelm.attr('rev'), data: { memcard_code: memcard_no, apply_memcard: 1 }, force_exec: force_exec, preload_obj: jelm, caching: false, store_init_content: false, callback: (window['fn_' + jelm.attr('name')] || {}) }); return false;
    });
}
function fn_apply_memcard(data, params) { }
function js_quickbuy_update_quantity(elm) {
    var jelm = $(elm); if (elm.timeoutHandle) { clearTimeout(elm.timeoutHandle); }
    if (jelm.val() == '') return; elm.timeoutHandle = setTimeout(function () { js_quickbuy_submit_quantity_form($('form[name=quickbuy_checkout_form]')); clearTimeout(elm.timeoutHandle); }, 800); return true;
}
function js_quickbuy_submit_quantity_form(jform) { jform.submit(); }
function js_quickbuy_box(step, html, callback, extra) {
    var defaults = { width: QB_BOXY_W, height: 'auto' }; var options = {}; if (typeof (extra) == 'undefined') extra = {}; switch (step) {
        case 0: options.title = 'Giỏ hàng của bạn'; callback = function () { js_apply_memcard(); }
            break; case 1: options.title = 'Đăng nhập'; options.height = 220; options.no_resize = true; break; case 6: options.title = true; options.center = false; options.fixed = false; options.no_resize = true; options.custom_class = 'quickbuy-wrapper-fixed'; break; case 2: options.title = 'Thông tin Khách hàng &amp; Địa chỉ giao hàng'; options.height = 380; break; case 3: options.title = 'Thông tin Thanh toán & Vận chuyển'; options.height = 370; break; case 4: options.title = 'Đặt hàng'; break; case 5: options.title = 'Hoàn tất đặt hàng'; options.width = 550; options.height = 84; options.no_resize = true; options.resize_callback = function () { $('.quickbuy-buttons').width(options.width); }
    }
    html = '<div class="quickbuy-wrapper ' + options.custom_class + '"><div class="inner" style="padding:1px 0;">' + html + '</div></div>'; var settings = $.extend({}, defaults, options, extra); if (current_quickbuy_instance) current_quickbuy_instance.hideAndUnload(); var boxy = new Boxy(html, settings).resize(settings.width, settings.height).center(); if ($('.quickbuy-wrapper .quickbuy-buttons').length) { $('.quickbuy-wrapper').addClass('quickbuy-has-buttons'); } else { $('.quickbuy-wrapper').removeClass('quickbuy-has-buttons'); }
    setTimeout(function () {
        var elm_h = $('.quickbuy-wrapper > .inner').height(); if (elm_h > QB_BOXY_H && !options.no_resize) {
            var box_h = QB_BOXY_H; if (settings.resize_callback) { boxy.resize(settings.width, box_h, settings.resize_callback).center(); } else { boxy.resize(settings.width, box_h).center(); }
            current_quickbuy_instance.center();
        } else { if (settings.resize_callback) settings.resize_callback(); }
    }, 20); $('.quickbuy-wrapper').resize(function () { console.log('trigger resized'); })
    $('.quickbuy-wrapper .boxy-close').click(function (e) { e.preventDefault(); boxy.hideAndUnload(); }); current_quickbuy_instance = boxy; ajax_form_submit2($('.quickbuy-wrapper')); ajax_district(".quickbuy-wrapper #billing_info", ".select_state", ".select_district"); ajax_district(".quickbuy-wrapper #shipping_info", ".select_state", ".select_district"); js_input_hint('.quickbuy-wrapper .quickbuy-form'); js_wrap_cart('.quickbuy-wrapper #quickbuy_cart .compact-cart-table')
    js_linking_form('.quickbuy-wrapper', true); if (typeof (callback) == 'function') { callback(boxy); }
}
function js_trigger_form_submit(form) { var f = form.get(); f.f = new form_handler(form.attr('name')); f.f.set_clicked(form.find(':submit').get()); js_clean_form(form); form.submit(); }
function js_quickbuy_form_focus(jform) {
    if (current_quickbuy_form && current_quickbuy_form.attr('name') != jform.attr('name')) { js_trigger_form_submit(current_quickbuy_form); if (js_check_form_errors(current_quickbuy_form)) { return false; } }
    if (current_quickbuy_form) current_quickbuy_form.removeClass('form-link-active'); current_quickbuy_form = jform; current_quickbuy_form.addClass('form-link-active'); return true;
}
function js_linking_form(el, first) {
    $(el).find('form.form-link').each(function (i) {
        var jform = $(this); jform.bind('click', function () { return js_quickbuy_form_focus(jform); })
        jform.find('#shipping_to_billing').unbind('click').click(function () { if (jform.hasClass('form-link-active') && !$(this).is(':checked')) { $('#shipping_form').switchAvailability(false); } else { $('#shipping_form').switchAvailability(true); js_trigger_form_submit(jform); } }); jform.find('.select_district').unbind('change').bind('change', function () { if ($(this).val()) { js_trigger_form_submit(jform); } }); jform.find('input[name="shipping_ids[]"]').unbind('change').change(function () {
            if (this.checked)
                js_trigger_form_submit(jform);
        }); if (first) { jform.find('input[name="payment_id"]').change(function () { js_trigger_form_submit(jform); }); }
        jform.find('.quickbuy-form-summary .inline-edit').unbind('click').click(function () {
            var jwrap = $(this).parents('.quickbuy-form-summary'); var jtarget = $('#' + jwrap.attr('rel')); if (js_quickbuy_form_focus(jtarget.parents('form.form-link'))) { jwrap.toggleClass('hidden'); jtarget.toggleClass('hidden'); if (jtarget.find('#shipping_to_billing').length) { if (jtarget.find('#shipping_to_billing').is(':checked')) { jtarget.find('#shipping_to_billing').removeAttr('checked'); jtarget.find('#shipping_form').switchAvailability(false); } } }
            return false;
        }); if (i == 0 && first) { if (!jform.find('.input-focus').parents('#billing_form').hasClass('hidden')) { jform.find('.input-focus').trigger('focus'); jform.trigger('click'); } }
    });
}
function js_check_form_errors(form) {
    if (form.find('.cm-failed-field').length) { return true; }
    return false;
}
function js_clean_form(form) { form.find('.input-hint').each(function () { if (this.value == this.hintValue) { this.value = ''; } }); }
function js_wrap_cart(el) { var max_items = 5; if ($(el).find('tr').length > max_items && !$(el).parent().hasClass('cart-table-wrap')) { var cart_wrap = $('<div class="cart-table-wrap" />'); var h = 0; $(el).find('tr:lt(' + max_items + ')').each(function () { h += $(this).height(); }); cart_wrap.css({ height: h, overflowY: 'scroll' }); $(el).wrap(cart_wrap); } }
function js_input_hint(el) {
    $(el).find('.input-hint').each(function () {
        if (this.hintValue) return true; var jelm = $(this); var hint = jelm.attr('title'); jelm.attr('title', ''); this.hintValue = hint; if (this.value == '' || this.value == this.hintValue) { this.value = this.hintValue; $(this).addClass('input-hint-empty'); }
        $(this).focus(function () { $(this).addClass('input-hint-focused'); if (this.value == this.hintValue) { this.value = ''; $(this).removeClass('input-hint-empty'); } }).bind('blur change', function () { $(this).removeClass('input-hint-focused'); if (this.value == '') { this.value = this.hintValue; $(this).addClass('input-hint-empty'); } else { $(this).removeClass('input-hint-empty'); } });
    });
}
function js_confirm_link() {
    $(".confirm_link").click(function () {
        question = $(this).attr("title"); if (!question) { question = "Bạn có chắc chắc muốn thực hiện điều này?"; }
        answer = confirm(question); if (answer) { return true; } else { return false; }
    })
}
function slide_anime() { $("#p-product-scroller").animate({ left: -(slide_width - wrapper_width) }, 3200 * num_item - (new Date().getTime() - time_start) + time_pause, "linear", function () { setTimeout(function () { $("#p-product-scroller").animate({ left: 0 }, 2000, "linear", function () { setTimeout(function () { time_pause = 0; time_start = new Date().getTime(); slide_anime(); }, 1500); }); }, 1500); }); }
function js_ajax_global() {
    js_toggle_checkbox(); ajax_form_submit('.ajax_form_review'); ajax_state(".checkout-steps #ba", ".select_country", ".select_state", ".select_district"); ajax_district(".checkout-steps #ba", ".select_state", ".select_district"); ajax_state(".checkout-steps #sa", ".select_country", ".select_state", ".select_district"); ajax_district(".checkout-steps #sa", ".select_state", ".select_district"); js_check_limit(250, $("#gift_description"), $("#gift_description_counter")); $("#gift_description").unbind('keypress'); $("#gift_description").keyup(function () { return js_check_limit(250, $(this), $("#gift_description_counter")); }); $("#shipping_map").click(function () { loadGoogleAPI(); return false; }); if (eval("typeof " + 'highlight_search' + " == 'function'")) { highlight_search(); }
    ajax_district(".quickbuy-wrapper #billing_info", ".select_state", ".select_district"); ajax_district(".quickbuy-wrapper #shipping_info", ".select_state", ".select_district"); js_input_hint('.quickbuy-wrapper .quickbuy-form'); js_wrap_cart('.quickbuy-wrapper #quickbuy_cart .compact-cart-table')
    js_linking_form('.quickbuy-wrapper'); ajax_district("#quickbuy_place_order #billing_info", ".select_state", ".select_district"); ajax_district("#quickbuy_place_order #shipping_info", ".select_state", ".select_district"); js_input_hint('#quickbuy_place_order .quickbuy-form'); js_wrap_cart('#quickbuy_place_order #quickbuy_cart .compact-cart-table')
    js_linking_form('#quickbuy_place_order'); js_apply_memcard();
}
function js_onconstruction() { $(".link_onconstruction").click(function () { alert('Chức năng này đang được xây dựng!'); return false; }); return true; }
function js_toggle_checkbox() { $(".toggle_check").each(function () { js_toggle_checkbox_rp($(this)); $(this).unbind('click'); $(this).click(function () { js_toggle_checkbox_rp($(this)); }); }); }
function js_toggle_checkbox_rp(element) { div = $("." + element.attr("title")); if (element.attr("checked")) { div.find('*').removeAttr("disabled"); div.show(); } else { div.find('*').attr("disabled", ""); div.hide(); } }
function js_countdown() { $(".countdown").each(function () { time = $(this).attr("title"); $(this).countdown({ until: +time, format: 'DHMS', layout: '<div id="t7_timer">' + '<div id="t7_vals">' + '<div id="t7_d" class="t7_numbs">{dnn}</div>' + '<div id="t7_h" class="t7_numbs">{hnn}</div>' + '<div id="t7_m" class="t7_numbs">{mnn}</div>' + '<div id="t7_s" class="t7_numbs">{snn}</div>' + '</div>' + '<div id="t7_labels">' + '<div id="t7_dl" class="t7_labs">ngày</div>' + '<div id="t7_hl" class="t7_labs">giờ</div>' + '<div id="t7_ml" class="t7_labs">phút</div>' + '<div id="t7_sl" class="t7_labs">giây</div>' + '</div>' + '<div id="t7_timer_over"></div>' + '</div>' }); }); }
function js_tooltip() { $(".tooltip").tooltip({ delay: 0, track: true, showURL: false }); }
function update_shipping() {
    $("#update_shipping").click(function () {
        if ($("#shipping_estimate_form input[name=shipping_ids]:checked").length) { shipping_input = $("#shipping_estimate_form input[name=shipping_ids]:checked"); $.ajax({ url: "index.php?dispatch=pnc_ajax.update_shipping_method", type: 'post', data: { shipping_id: shipping_input.val() }, dataType: 'json', beforeSend: function () { }, success: function (data, text) { }, error: function () { } }); } else { }
        return false;
    });
}
function js_shipping_estimate(select) { $(select).change(function () { shipping_estimate(); }); }
function shipping_estimate() {
    if ($("#shipping_estimate_form").length) { $.ajax({ url: "index.php?dispatch=pnc_ajax.get_shipping_method", type: 'post', data: $("#shipping_estimate_form").serialize(), dataType: 'json', beforeSend: function () { }, success: function (data, text) { $("#shipping_method").html(data.text); $(".shipping_method_radio").click(function () { id = $(this).attr("id"); message_id = id + "_message"; $("#shipping_method p.result").html($("#" + message_id).html()); }); }, error: function () { } }); return false; }
    return true;
}
function ajax_district(area, select_state, select_district, callback) { $(area).find(select_state).unbind('change'); $(area).find(select_state).change(function () { $.ajax({ url: "index.php?dispatch=pnc_ajax.get_district", type: 'post', data: { state_code: $(this).val() }, dataType: 'json', beforeSend: function () { $(area).find(select_district).html('<option>...</option>'); }, success: function (data, text) { if (data.text != '') { $(area).find(select_district).attr("disabled", ""); $(area).find(select_district).html(data.text); } else { $(area).find(select_district).attr("disabled", "disabled"); } }, error: function () { } }); }); }
function ajax_state(area, select_country, select_state, select_district, callback) {
    $(area).find(select_country).unbind('change'); $(area).find(select_country).change(function () {
        $.ajax({
            url: "index.php?dispatch=pnc_ajax.get_state", type: 'post', data: { country_code: $(this).val() }, dataType: 'json', beforeSend: function () { $(area).find(select_state).html('<option>...</option>'); }, success: function (data, text) {
                if (data.text != '') { $(area).find(select_state).attr("disabled", ""); $(area).find(select_state).html(data.text); } else { $(area).find(select_state).attr("disabled", "disabled"); }
                $(area).find(select_state).each(function () { $.ajax({ url: "index.php?dispatch=pnc_ajax.get_district", type: 'post', data: { state_code: $(this).val() }, dataType: 'json', beforeSend: function () { $(area).find(select_district).html('<option>...</option>'); }, success: function (data, text) { if (data.text != '') { $(area).find(select_district).attr("disabled", ""); $(area).find(select_district).html(data.text); } else { $(area).find(select_district).attr("disabled", ""); } }, error: function () { } }); }); if (typeof (callback) == 'function') { callback(); }
            }, error: function () { }
        });
    });
}
function check_form(form) { valid = true; $(form).find("label").each(function () { if ($(this).hasClass("cm-required")) { input = $(this).attr("for"); if ($("#" + input).length > 0) { if ($("#" + input).val() == '') { valid = false; } } } }); return valid; }
function ajax_post_submit(id, form, area, callback) {
    $(form).submit(function () {
        if (check_form(form)) { action = $(this).attr("action") + "&" + id + "_ajax=1" + "&is_ajax=1"; input = $(this).find("input[type=submit]"); input.attr("disabled", ""); input_text = input.val(); input.val('...'); $.ajax({ url: action, type: 'post', data: $(this).serialize(), dataType: 'json', beforeSend: function () { $(area).html("<div class='ajax_loading' style='height: 80px'></div>"); }, success: function (data, text) { input.val(input_text); input.attr("disabled", ""); $(area).html(data.text); if (typeof (callback) == 'function') { callback(); } }, error: function () { input.val(input_text); input.attr("disabled", ""); if (typeof (callback) == 'function') { callback(); } } }); }
        return false;
    });
}
function js_custom() {
    $(".bestseller_select").change(function () {
        if ($(this).attr("name") != "category_id") { val = $(this).val(); $(this).parents('.select').find('.bestseller_select').each(function () { if ($(this).attr("name") != "category_id") { $(this).val(''); } }); $(this).val(val); }
        $("#bestseller_form").submit();
    });
}
var current_page = 0; function js_change_page(pages, page) {
    if (!pages[page] || $("#preview_page").attr("src") == pages[page]) { return false; }
    $("#preview_loading").removeClass("hidden"); $("#preview_page").attr("src", pages[page]); $("#preview_page").css("visibility", "hidden"); $(".preview_next .page_next img").css("display", "none"); $(".preview_prev .page_prev img").css("display", "none"); $('#preview_page').load(function () {
        $("#preview_loading").addClass("hidden"); $("#preview_page").css("visibility", "visible"); $(".preview_prev .page_prev, .preview_next .page_next, .preview_next .page_next img, .preview_prev .page_prev img").css("display", "block"); $(".preview_prev .page_prev, .preview_next .page_next").css("width", "17px"); if (page == pages.length - 1) { $(".preview_next .page_next img").css("display", "none"); }
        if (page == 0) { $(".preview_prev .page_prev img").css("display", "none"); }
    }); current_page = page; return true;
}
function js_preview_page(pages) {
    $(".preview_prev .page_prev, .preview_next .page_next, .preview_next .page_next img, .preview_prev .page_prev img").css("display", "block"); $(".preview_prev .page_prev, .preview_next .page_next").css("width", "17px"); if (current_page == pages.length - 1) { $(".preview_next .page_next img").css("display", "none"); }
    if (current_page == 0) { $(".preview_prev .page_prev img").css("display", "none"); }
    $(".page_index").click(function (event) { id = $(this).attr("id"); page = parseInt(id.replace("page_", "")); js_change_page(pages, page); return false; }); $(".page_next").click(function (event) {
        if (current_page < pages.length) { next_page = current_page + 1; js_change_page(pages, next_page); } else { current_page = pages.length - 1; }
        return false;
    }); $(".page_prev").click(function (event) {
        if (current_page > 0) { prev_page = current_page - 1; js_change_page(pages, prev_page); } else { current_page = 1; }
        return false;
    }); hash = get_hash(); if (hash.length > 0) { page = parseInt(hash.replace('page_', '')); if (page >= 1 && page <= pages.length) { js_change_page(pages, page); } }
}
function get_hash() { var hash = window.location.hash; return hash.substring(1); }
function ajax_vote() {
    if ($(".ajax_vote").length > 0) {
        window.setInterval(function () {
            $(".ajax_vote").click(function (event) {
                href = $(this).attr("href"); href = href.replace(/#.*/, ""); id = $(this).attr("id"); post_id = id.replace("vote_yes_", ""); post_id = post_id.replace("vote_no_", ""); if (href != "" && href != "#") { ajax_url = href + "&" + "vote_ajax=1" + "&is_ajax=1"; $.ajax({ url: ajax_url, type: "get", cache: true, dataType: 'json', beforeSend: function () { $("#comment_vote_" + post_id).html("<div class='ajax_loading' style='height: 12px'></div>"); }, success: function (data, text) { $("#comment_vote_" + post_id).html("Cám ơn bạn đã đóng góp ý kiến"); }, error: function () { } }); }
                return false;
            });
        }, 100);
    }
}
function global_anchor() { $("a").each(function () { $(this).click(function (event) { if ($(this).attr("href") == "#") { event.preventDefault(); } }); }); $(".previous_page_link").click(function () { href = $(this).attr("href"); if (href == "" || href == "#") { history.go(-1); } }); $("#product_amount").change(function () { if ($("#button_cart").length > 0) { my_button_cart = $("#button_cart"); href = my_button_cart.attr("href"); if (href.search("amount") != -1) { my_button_cart.attr("href", href.replace(/amount=\d+/i, "amount=" + $(this).val())); } } }); }
function close_box(button, id, content, callback) { $(button).unbind('click'); $(button).click(function (event) { box = $(this).parents(".box"); box_content = box.find(".box_content " + content); box_content.html(''); $.cookie.set('recent_view', "0"); $(this).html('Mở cửa sổ này'); open_box(button, id, content, callback); return false; }); }
function open_box(button, id, content, callback) {
    $(button).unbind('click'); $(button).click(function () {
        href = $(this).attr("href"); href = href.replace(/#.*/, ""); if (href != "" && href != "#" && ajax_url_array.indexOf(href) == -1) {
            ajax_url = href + "&" + id + "_ajax=1" + "&is_ajax=1"; $.ajax({
                url: ajax_url, type: "get", cache: true, dataType: 'json', beforeSend: function () { $(content).html("<div class='ajax_loading' style='height: " + 100 + "px'></div>"); }, success: function (data, text) {
                    $(content).html(data.text); if (typeof (callback) == 'function') { callback(); }
                    $(button).html('Thu nhỏ cửa sổ này'); $.cookie.set('recent_view', "1"); close_box(button, id, content, callback);
                }, error: function () { }
            });
        }
        return false;
    });
}
function js_related_products(relate_product_original_price, relate_products_price) {
    $(".related_products").click(function () {
        all = $(".related_products"); checked = $(".related_products:checked"); if (checked.length == 0) { alert('Bạn phải chọn ít nhất 2 sản phẩm'); return false; }
        relate_products_count = parseInt($(".relate_products_count").html()); $(".relate_products_count").html(checked.length + 1); total_value = parseInt(relate_product_original_price); check_product_ids = new Array(); all.each(function (index) { if ($(this).attr("checked")) { id = $(this).attr("id"); product_id = id.replace("check_", ""); check_product_ids.push(product_id); total_value += parseInt(relate_products_price[index]); } }); $(".relate_products_value").html(add_digit(total_value)); relate_add_cart_href = $("#related_product_add_cart").attr("href"); $("#related_product_add_cart").attr("href", relate_add_cart_href.replace(/related_products=[\d\.]+/i, "related_products=" + check_product_ids.join("."))); return true;
    });
}
function add_digit(num) {
    num += ''; x = num.split('.'); x1 = x[0]; x2 = x.length > 1 ? ',' + x[1] : ''; var rgx = /(\d+)(\d{3})/; while (rgx.test(x1)) { x1 = x1.replace(rgx, '$1' + ',' + '$2'); }
    return x1 + x2;
}
function js_tab(tab, content) {
    tabs = $(tab); contents = $(content); if (contents.length > 0) {
        contents.css("display", "none"); contents.eq(0).css("display", "block"); tabs.find("li a").each(function (index) {
            $(this).click(function (event) {
                tabs_li = tabs.find("li"); tabs_li.removeClass("active"); tabs_li.eq(index).addClass("active"); content = contents.eq(index); if (content.length > 0) { contents.css("display", "none"); content.css("display", "block"); }
                return false;
            });
        });
    }
}
var ajax_url_array = new Array(); function ajax_product_list(nav, id, items, area, equal_height) {
    $(nav).click(function (event) {
        href = $(this).attr("href"); href = href.replace(/#.*/, ""); if (href != "" && href != "#" && ajax_url_array.indexOf(href) == -1 && href.search("\\?") != -1) { ajax_url = href + "&" + id + "_ajax=1" + "&is_ajax=1"; $.ajax({ url: ajax_url, type: "get", cache: true, dataType: 'json', beforeSend: function () { ajax_url_array.push(href); if (equal_height) { max_height = 0; $(items).each(function () { height = $(this).height(); if (max_height < height) { max_height = height; } }); $(items).html("<div class='ajax_loading' style='height: " + max_height + "px'></div>"); } else { $(items).each(function () { $(this).html("<div class='ajax_loading' style='height: " + $(this).height() + "px'></div>"); }); } }, success: function (data, text) { ajax_url_array.splice(ajax_url_array.indexOf(href), 1); $(area).html(data.text); ajax_product_list(nav, id, items, area, equal_height); }, error: function (XMLHttpRequest, textStatus, errorThrown) { ajax_url_array.splice(ajax_url_array.indexOf(href), 1); } }); }
        return false;
    });
}
function check_ie(check_version) {
    if ($.browser.msie) { if (check_version) { version = parseInt(jQuery.browser.version); if (check_version == version) { return true; } } else { return true; } }
    return false;
}
function product_ajust_list_item() { var width_ajust = 1200; width = $(window).width(); if (width >= width_ajust) { $(".book_auto_ajust").each(function () { max_height = 0; $("li:not(.li_fix)", this).each(function (index) { max_height = max_height < $(this).height() ? $(this).height() : max_height; }); if (check_ie(6) || check_ie(7)) { $("li:not(.li_fix)", this).css("height", max_height); } }); } else { $(".book_auto_ajust").each(function () { max_height = 0; $("li:not(.li_fix)", this).each(function (index) { max_height = max_height < $(this).height() ? $(this).height() : max_height; }); if (check_ie(6) || check_ie(7)) { $("li:not(.li_fix)", this).css("height", max_height); } }); } }
function slide_show() { $('.slideshow').cycle({ fx: 'fade', speed: 'fast', timeout: 0, pager: '#slideshow_nav' }); }
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt) {
        var len = this.length >>> 0; var from = Number(arguments[1]) || 0; from = (from < 0) ? Math.ceil(from) : Math.floor(from); if (from < 0) from += len; for (; from < len; from++) { if (from in this && this[from] === elt) return from; }
        return -1;
    };
}
function js_dialog(link) {
    $(link).click(function () {
        id = $(this).attr("id"); title = $(this).attr("title"); href = $(this).attr("href"); href = href.replace(/#.*/, ""); if (href != "" && href != "#" && ajax_url_array.indexOf(href) == -1) { ajax_url = href + "&is_ajax=1"; $.ajax({ url: ajax_url, type: "get", cache: true, dataType: 'json', beforeSend: function () { ajax_url_array.push(href); }, success: function (data, text) { ajax_url_array.splice(ajax_url_array.indexOf(href), 1); html = data.text; dialog = $('<div id="dialog_' + id + '" style="position: relative" />'); dialog.html(html); dialog.attr("title", title); dialog.appendTo('body'); dialog.dialog({ width: 500, height: 'auto', bgiframe: true, modal: true, resizable: false, overlay: { background: "#000", opacity: 0.3 }, close: function () { $(this).remove(); }, open: function () { $(".ui-dialog-overlay").click(function () { dialog.remove(); }); $(".add_receive_email").click(function () { clone_field = $("#dialog_" + id + " .dialog_form .clone-field").clone(); clone_field.removeClass('hidden'); clone_field.removeClass('clone-field'); $(this).before(clone_field); $("#dialog_" + id).css("height", $("#dialog_" + id).height() + clone_field.height() + 10); return false; }); ajax_state("#dialog_" + id + " .dialog_form", ".select_country", ".select_state"); $("#dialog_" + id + " .dialog_form").submit(function () { action = $(this).attr("action"); $.ajax({ url: action, type: 'post', data: $(this).serialize(), dataType: 'json', beforeSend: function () { $("#dialog_" + id + " .dialog_form").hide(); $("#dialog_" + id + " .dialog_message").removeClass('hidden'); $("#dialog_" + id + " .dialog_message").css({ height: '94%', width: '100%' }); $("#dialog_" + id + " .dialog_message").html("<div class='ajax_loading' style='height: 100%; background-position: 50% 50%;'></div>"); }, success: function (response) { response = response.text; response = eval("(" + response + ")"); if (response.code == 'error') { $("#dialog_" + id + " .dialog_message").html(response.message); $("#dialog_" + id + " .dialog_message").css({ height: 'auto', width: 'auto', Color: '#F00', padding: '5px 10px 0' }); $("#dialog_" + id).css("height", $("#dialog_" + id + " .dialog_message").height() + $("#dialog_" + id + " .dialog_form").height() + 15); $("#dialog_" + id + " .dialog_form").show(); } else { if (response.action) { switch (response.action) { case "changelocation": location.reload(true); break; default: break; } } else { $("#dialog_" + id + " .success_message").show(); $("#dialog_" + id + " .success_message").css({ top: '50%', left: '50%', marginTop: -parseInt($("#dialog_" + id + " .success_message").height()) / 2, marginLeft: -parseInt($("#dialog_" + id + " .success_message").width()) / 2 }); $("#dialog_" + id + " .dialog_message").hide(); } } }, error: function () { $("#dialog_" + id + " .dialog_message").html('Error Occured!'); $("#dialog_" + id + " .dialog_message").css({ height: 'auto', width: 'auto', Color: '#F00', padding: '5px 10px 0' }); $("#dialog_" + id).css("height", $("#dialog_" + id + " .dialog_message").height() + $("#dialog_" + id + " .dialog_form").height() + 15); $("#dialog_" + id + " .dialog_form").show(); } }); return false; }); $("#dialog_" + id + " .cancel_button").click(function () { dialog.remove(); }); } }); }, error: function () { ajax_url_array.splice(ajax_url_array.indexOf(href), 1); } }); }
        return false;
    })
}
function js_inline_dialog(id, selector, title) {
    var dialog = $('<div id="dialog_' + id + '" style="position: relative" />'); dialog_html = $(selector).clone(true); dialog_html.css("display", "block"); dialog.append(dialog_html); if (title) { dialog.attr("title", title); }
    dialog.appendTo('body'); dialog.dialog({ width: 500, height: 'auto', bgiframe: true, modal: true, resizable: false, overlay: { background: "#000", opacity: 0.3 }, close: function () { $(this).remove(); }, open: function () { $(".ui-dialog-overlay").click(function () { dialog.remove(); }); $("#dialog_" + id + " .cancel_button").click(function () { dialog.remove(); }); } });
}
var time_dmsp; var bool_dmsp; function js_dmsp() {
    $("a.dmsp").hover(function () {
        bool_dmsp = true; clearTimeout(time_dmsp); list = $("ul.list_dmsp"); list.css({ display: "block", top: parseInt($(this).css("height")) + 5, left: 0 }); if (list.width() < 220) { list.css("width", parseInt($(this).css("width")) + 20); }
        list.hover(function () { bool_dmsp = true; clearTimeout(time_dmsp); }, function () { bool_dmsp = false; js_close_dmsp(); })
    }, function () { bool_dmsp = false; js_close_dmsp(); });
}
function js_check_limit(limit_char, field, counter) {
    if (field.length > 0) { value = field.val(); if (value.length > limit_char) { field.val(value.substring(0, limit_char)); } else { counter.html(parseInt(limit_char - value.length) + 0); } }
    return true;
}
function ajax_form_submit(form) { $(form).unbind('submit'); $(form).submit(function () { form_action = $(this).attr("action"); $.ajax({ url: form_action, type: "post", data: $(this).serialize(), dataType: 'json', beforeSend: function () { }, success: function (response) { response = response.text; response = eval("(" + response + ")"); if (response.code == 'error') { alert('Error!'); } else { alert('Success!'); } }, error: function () { alert('Error!'); } }); return false; }); }
function ajax_form_submit2(dialog) { jQuery.processForms(dialog); }
function js_close_dmsp() { time_dmsp = setTimeout(function () { if (bool_dmsp == false) { $("ul.list_dmsp").css("display", "none"); } }, 150); }
Array.prototype.in_array = function (p_val) {
    for (var i = 0, l = this.length; i < l; i++) { if (this[i] == p_val) { return true; } }
    return false;
}
function loadScript(url, callback) {
    var script = document.createElement("script")
    script.type = "text/javascript"; if (script.readyState) { script.onreadystatechange = function () { if (script.readyState == "loaded" || script.readyState == "complete") { script.onreadystatechange = null; callback(); } }; } else { script.onload = function () { callback(); }; }
    script.src = url; document.getElementsByTagName("head")[0].appendChild(script);
}
function loadGoogleAPI() { var script = document.createElement("script"); script.src = "http://www.google.com/jsapi?key=ABQIAAAAQ1KpLUV0oGQ8_oAGVeGRexSegnRAs8I9Vs_1V6KCxKYuMNbyURRLx9c6OlgNpX7RQXZixvbEMHe9rg&callback=loadMaps"; script.type = "text/javascript"; document.getElementsByTagName("head")[0].appendChild(script); createLMap(); }
function loadMaps() { google.load("maps", "2", { "callback": mapLoaded }); }
var gmap = null; var geocoder = null; var marker_edit = null; function mapLoaded() {
    if (GBrowserIsCompatible()) {
        map_select = $("#l_map_select"); if (!map_select) { return false; }
        map_select.html($("<div id='s_description'></div><div id='s_map_select'></div><div id='s_button'></div>")); $("#s_description").html('Bấm chuột & di chuyển <img src="/skins/pnc/customer/pnc/images/poly_icon.jpg" alt="" /> tới gần hoặc chính xác địa chỉ giao hàng của bạn'); $("#s_button").html('<div class="p-button-red">' + '<input type="button" class="p-button-start submit_button" id="s_close_button" value="Đóng cửa sổ">' + '<div class="p-button-end"></div>' + '</div>' + '<div class="p-button-red">' + '<input type="button" class="p-button-start submit_button" id="s_select_map" value="Xác định vị trí">' + '<div class="p-button-end"></div>' + '</div>' + '<div style="float: right; line-height: 28px;">Sau khi định vị xong, bấm nút "Xác định vị trí" để hoàn tất</div>'); $("#s_close_button").click(function () { hide_l_box_content(); }); $("#s_select_map").click(function () { $("#shipping_coordinate_x").val($("#shipping_coordinate_x_temp").val()); $("#shipping_coordinate_y").val($("#shipping_coordinate_y_temp").val()); hide_l_box_content(); }); gmap = new GMap2(document.getElementById("s_map_select")); geocoder = new GClientGeocoder()
        default_coordinate = { 'x': '16.058483', 'y': '107.219237', 'zoom': '6' }; address = $("#elm_19").val() + ($("#elm_36").val() != "" ? " " + $("#elm_36 option:selected").text() : "") + ($("#elm_25").val() != "" ? " " + $("#elm_25 option:selected").text() : "") + ($("#elm_27").val() != "" ? " " + $("#elm_27 option:selected").text() : ""); if (!$("#shipping_coordinate_x").val() || !$("#shipping_coordinate_y").val()) { geocoder.getLatLng(address, function (point) { if (!point) { gmap.setCenter(new GLatLng(default_coordinate.x, default_coordinate.y), parseInt(default_coordinate.zoom)); } else { gmap.setCenter(point, 15); marker_edit = new GMarker(point, { draggable: true, autoPan: false }); gmap.addOverlay(marker_edit); $("#shipping_coordinate_x_temp").val(point.lat()); $("#shipping_coordinate_y_temp").val(point.lng()); GEvent.addListener(marker_edit, "dragend", function () { latlng = marker_edit.getLatLng(); $("#shipping_coordinate_x_temp").val(latlng.lat()); $("#shipping_coordinate_y_temp").val(latlng.lng()); }); } }); } else { current_x = $("#shipping_coordinate_x").val(); current_y = $("#shipping_coordinate_y").val(); gmap.setCenter(new GLatLng(current_x, current_y), 15); marker_edit = new GMarker(new GLatLng(current_x, current_y), { draggable: true, autoPan: false }); gmap.addOverlay(marker_edit); GEvent.addListener(marker_edit, "dragend", function () { latlng = marker_edit.getLatLng(); $("#shipping_coordinate_x_temp").val(latlng.lat()); $("#shipping_coordinate_y_temp").val(latlng.lng()); }); }
        gmap.setUIToDefault(); GEvent.addListener(gmap, 'click', function (overlay, latlng) {
            if (marker_edit) { marker_edit.setLatLng(latlng); } else { marker_edit = new GMarker(point, { draggable: true, autoPan: false }); gmap.addOverlay(marker_edit); GEvent.addListener(marker_edit, "dragend", function () { latlng = marker_edit.getLatLng(); $("#shipping_coordinate_x_temp").val(latlng.lat()); $("#shipping_coordinate_y_temp").val(latlng.lng()); }); }
            $("#shipping_coordinate_x_temp").val(latlng.lat()); $("#shipping_coordinate_y_temp").val(latlng.lng());
        });
    }
    return true;
}
function createLMap() {
    overlay = $("<div id='l_box_overlay'></div>"); if (check_ie(6)) { screen_height = getDocHeight(); screen_width = parseInt($(window).width()); overlay.css({ position: 'absolute', width: screen_width, height: screen_height }); }
    $("body").append(overlay.click(function () { hide_l_box_content(); })); overlay.css("opacity", 0.5); overlay.fadeIn(150); $(document).keydown(function (e) { if (e.keyCode == 27) { hide_l_box_content(); } }); map_select = $("<div id='l_map_select'></div>"); map_select.css({ 'top': (parseInt($(window).height()) - 500) / 2, 'left': (parseInt($(window).width()) - 700) / 2 }); $("body").append(map_select); map_loading = $("<div class='ajax_loading'></div>"); map_loading.css({ 'height': map_select.height() }); map_select.html(map_loading);
}
function hide_l_box_content() {
    if ($("#l_box_overlay")) { $("#l_box_overlay").fadeOut(150, function () { $(this).remove(); }); $("#l_map_select").remove(); }
    $(".l_box_content").fadeOut(150);
}
function l_box_handle_escape() { hide_l_box_content(); }
function getDocHeight() { var D = document; return Math.max(Math.max(D.body.scrollHeight, D.documentElement.scrollHeight), Math.max(D.body.offsetHeight, D.documentElement.offsetHeight), Math.max(D.body.clientHeight, D.documentElement.clientHeight)); }
function js_image_popup(img_src, title, width, height, addition_options) {
    var opts = { autoDimensions: false, width: width, height: height, scrolling: 'no', titleShow: false }
    jQ.extend(opts, addition_options); jQ.fancybox('<img src="' + img_src + '" title="' + title + '" alt="' + title + '" />', opts);
}
$(function () { js_user_panel(); js_left_menu(); js_back_to_top_button(); }); jQuery.extend(jQuery.easing, { customEasing: function (x, t, b, c, d) { return c * Math.sqrt(1 - (t = t / d - 1) * t) + b; }, customEasing2: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b; }, customEasing3: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * (t /= d) * t * ((s + 1) * t - s) + b; } }); var user_panel_hovering = false; var user_panel_timeout_handle = false; function js_user_panel() {
    $(document).click(function (e) {
        var jelm = $(e.target); var elm = e.target; var s; if (e.type == 'click' && e.which == 1) { if (s = elm.className.match(/up-combinations([_\w]+)?/gi)) { var class_group = s[0].replace(/up-combinations/, ''); var id_group = jelm.attr('id').replace(/sw_on_|sw_off_/, ''); $('#on_' + id_group).switchAvailability(); $('#off_' + id_group).switchAvailability(); $('#sw_on_' + id_group).toggleClass('hidden'); $('#sw_off_' + id_group).toggleClass('hidden'); var height = $('.up-col-2').height(); $('#user-panel-inner').animate({ height: height }, 400, 'customEasing2'); return true; } }
        return true;
    }); $('#user-panel').each(function () { var t = this; var upc = $('#user-panel-inner', t); $('.up-login-register-button', t).click(function (e) { var ta = this; e.preventDefault(); var height = $('.up-col-2', t).height(); if ($(t).hasClass('user-panel-collapsed')) { upc.animate({ height: height, marginTop: 10, marginBottom: 5 }, 800, 'customEasing2', function () { $(t).removeClass('user-panel-collapsed'); $('<div id="user-panel-overlay" />').appendTo('body').css({ opacity: 0 }).animate({ opacity: .8 }, 1000).click(function () { $(ta).trigger('click'); }); }); } else { upc.animate({ height: 0, marginTop: 0, marginBottom: 0 }, 300, 'customEasing3', function () { $(t).addClass('user-panel-collapsed'); $('#user-panel-overlay').fadeOut(200, function () { $(this).remove(); }); }); } }); $('#on_have_account').switchAvailability(false); $('#off_have_account').switchAvailability(true); }); $('#user-panel-dropdown').bind('mouseover', function () { user_panel_hovering = true; clearTimeout(user_panel_timeout_handle); }).bind('mouseout', function () { user_panel_hovering = false; user_panel_timeout_handle = setTimeout(js_check_user_panel_dropdown, 150); })
    $('#profile-button').bind('mouseenter', function () { $('#user-panel-dropdown').stop(true, true).fadeIn(50); user_panel_hovering = true; clearTimeout(user_panel_timeout_handle); }).bind('mouseleave', function () { user_panel_hovering = false; user_panel_timeout_handle = setTimeout(js_check_user_panel_dropdown, 150); });
}
function js_check_user_panel_dropdown() { if (!user_panel_hovering) { $('#user-panel-dropdown').stop(true, true).fadeOut(50); } }
function getCenteredCoords(width, height) {
    var xPos = null; var yPos = null; if (window.ActiveXObject) { xPos = window.event.screenX - (width / 2) + 100; yPos = window.event.screenY - (height / 2) - 100; } else { var parentSize = [window.outerWidth, window.outerHeight]; var parentPos = [window.screenX, window.screenY]; xPos = parentPos[0] + Math.max(0, Math.floor((parentSize[0] - width) / 2)); yPos = parentPos[1] + Math.max(0, Math.floor((parentSize[1] - (height * 1.25)) / 2)); }
    return [xPos, yPos];
}
function openPopupWindow(el) { var w = window.open(el.href, '_blank', 'width=450,height=500,location=1,status=1,resizable=yes'); var coords = getCenteredCoords(450, 500); w.moveTo(coords[0], coords[1]); }
function js_product_sidebar_accordion() { $('.product-sidebar').each(function () { var t = this; var _currentOn = 0; $('.p-product-introduction > h3', t).each(function (i) { $(this).click(function () { var _accordion = $('.p-list', t); _accordion.slideUp(200); _accordion.eq(i).slideDown(200); _currentOn = i; }).css('cursor', 'pointer'); }); $('.p-product-introduction > h3', t).eq(0).trigger('click'); }); }
function js_back_to_top_button(options) {
    var defaults = { text: 'Back To Top', min: 100, inDelay: 300, outDelay: 300, containerID: 'back-to-top', containerHoverID: 'back-to-top', scrollSpeed: 300, easingType: 'linear' }; var settings = $.extend(defaults, options); var containerIDhash = '#' + settings.containerID; $('body').append('<a href="#" id="' + settings.containerID + '">' + settings.text + '</a>'); $(containerIDhash).hide().click(function () { $('html, body').animate({ scrollTop: 0 }, settings.scrollSpeed, settings.easingType); return false; })
    $(window).scroll(function () {
        var sd = $(window).scrollTop(); if (typeof document.body.style.maxHeight === "undefined") { $(containerIDhash).css({ 'position': 'absolute', 'top': $(window).scrollTop() + $(window).height() - 50 }); }
        if (sd > settings.min)
            $(containerIDhash).fadeIn(settings.inDelay); else
            $(containerIDhash).fadeOut(settings.Outdelay);
    });
}
function js_left_menu() { $('.p-category-menu ul.p-subcategories').each(function () { var t = this; var p = $(this).parent(); var pa = $('> a', p); p.addClass('js-menu-parent'); p.bind('mouseenter', function (e) { $(t).show().stop().animate({ opacity: 1 }, 200); }).bind('mouseleave', function (e) { $(t).stop().animate({ opacity: 0 }, 100, 'swing', function () { $(this).hide(); }); }); pa.bind('mouseenter', function () { p.trigger('mouseenter'); }) }); }
function tet_layout() {
    if ($('.p-logo img').length) { $('.p-logo img').attr('src', $('.p-logo img').attr('src').replace('logo_pn.png', 'tet/logo_pn.png')); }
    $('img[src$=help_button.png]').each(function () { $(this).attr('src', $(this).attr('src').replace('help_button.png', 'tet/help_button.png')); }); $('img[src$=icon_1900.jpg]').each(function () { $(this).attr('src', $(this).attr('src').replace('icon_1900.jpg', 'tet/icon_1900.png')); }); $('body').attr('id', 'tet_layout'); if ($('#p-main').length) {
        $('#p-main').append('<div class="cctx cctx-l" />').append('<div class="cctx cctx-r" />'); var mainTop = $('#p-main').offset().top, maxTop = $('#p-quick-links').offset().top, negTop = mainTop - $('#header').height() - 8, iHeight = $('.cctx').height(); $('.cctx').css('top', -negTop); $(window).scroll(function () {
            var sTop = $(this).scrollTop(); if (sTop > maxTop - mainTop + negTop - iHeight) { sTop = maxTop - mainTop + negTop - iHeight; }
            $('.cctx').stop().animate({ marginTop: sTop }, { duration: 500, easing: 'customEasing' });
        });
    }
    if ($('#p-partner').length) { $('#p-partner').parent().addClass('p-footer'); }
}
var __caches = []; var CustomFunction = {
    fnOverlay: function (action) { if (action == 'show') { var yOffset = CustomFunction.getYOffset(); $('html').addClass('ov').css('top', -yOffset); $('#lb_popup').removeClass('hidden').animate({ opacity: 1 }, 500); } else if (action == 'hide') { $('html').removeClass('ov').css('top', 0); $('#lb_popup').html('').addClass('hidden').css({ opacity: 0 }); } }, getXOffset: function () {
        var pageX; if (typeof (window.pageXOffset) == 'number') { pageX = window.pageXOffset; }
        else { pageX = document.documentElement.scrollLeft; }
        return pageX;
    }, getYOffset: function () {
        var pageY; if (typeof (window.pageYOffset) == 'number') { pageY = window.pageYOffset; }
        else { pageY = document.documentElement.scrollTop; }
        return pageY;
    }
}; function fn_superbox(data, params) { js_superbox(data.html.hoisach_sukien); }
function fn_quickview_superbox(data, params) { var html = '<div onclick="$(\'#lb_close_btn\').trigger(\'click\');" class="hiddenLbClose"></div>'; html += '<div class="lb_content clearfix">'; html += data.html.product_quickview; html += '<div class="lb_bt_cl_fs"><a class="group_icon btn_close2" id="lb_close_btn"></a></div>'; html += '</div>'; js_superbox(html); }
function js_superbox(html) {
    var scrollY = CustomFunction.getYOffset(); if ($('#lb_popup').length == 0) { $('<div id="lb_popup"></div>').appendTo('body'); }
    CustomFunction.fnOverlay('show'); $('#lb_popup').html(html); $('#lb_close_btn').bind('click', function (e) { CustomFunction.fnOverlay('hide'); window.scroll(0, scrollY); $(this).unbind(e); });
}
function getQueryParams(qs) {
    qs = qs.split("+").join(" "); var params = {}, tokens, re = /[?&]?([^=]+)=([^&]*)/g; while (tokens = re.exec(qs)) { params[decodeURIComponent(tokens[1])] = decodeURIComponent(tokens[2]); }
    return params;
}
function css_browser_selector(u) { var ua = u.toLowerCase(), is = function (t) { return ua.indexOf(t) > -1 }, g = 'gecko', w = 'webkit', s = 'safari', o = 'opera', m = 'mobile', h = document.documentElement, b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + RegExp.$1) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3.6') ? g + ' ff3 ff3_6' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.$1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.$2 : '')) : is('konqueror') ? 'konqueror' : is('blackberry') ? m + ' blackberry' : is('android') ? m + ' android' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.$1 : '') : is('mozilla/') ? g : '', is('j2me') ? m + ' j2me' : is('iphone') ? m + ' iphone' : is('ipod') ? m + ' ipod' : is('ipad') ? m + ' ipad' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' + (is('windows nt 6.0') ? ' vista' : '') : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js']; c = b.join(' '); h.className += ' ' + c; return c; }; css_browser_selector(navigator.userAgent);