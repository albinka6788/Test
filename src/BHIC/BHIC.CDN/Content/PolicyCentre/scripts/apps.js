$(document).ready(function () {

    /*
# =============================================================================
#   Equalize Column
# =============================================================================
*/

    (function (b) { b.fn.equalize = function (a) { var d = !1, g = !1, c, e; b.isPlainObject(a) ? (c = a.equalize || "height", d = a.children || !1, g = a.reset || !1) : c = a || "height"; if (!b.isFunction(b.fn[c])) return !1; e = 0 < c.indexOf("eight") ? "height" : "width"; return this.each(function () { var a = d ? b(this).find(d) : b(this).children(), f = 0; a.each(function () { var a = b(this); g && a.css(e, ""); a = a[c](); a > f && (f = a) }); a.css(e, f + "px") }) } })(jQuery);

    $(function () {
        $('.equalize').equalize({ children: '.content-box' });
        $(window).resize(function () {
            $('.equalize').equalize({ reset: true, children: '.content-box' });
        });
    });



    /*
  # =============================================================================
   * HTML5 Placeholder jQuery Plugin - v2.3.0 by Mathias Bynens
   # =============================================================================
  */
    !function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : a("object" == typeof module && module.exports ? require("jquery") : jQuery) }(function (a) { function b(b) { var c = {}, d = /^jQuery\d+$/; return a.each(b.attributes, function (a, b) { b.specified && !d.test(b.name) && (c[b.name] = b.value) }), c } function c(b, c) { var d = this, f = a(this); if (d.value === f.attr(h ? "placeholder-x" : "placeholder") && f.hasClass(n.customClass)) if (d.value = "", f.removeClass(n.customClass), f.data("placeholder-password")) { if (f = f.hide().nextAll('input[type="password"]:first').show().attr("id", f.removeAttr("id").data("placeholder-id")), b === !0) return f[0].value = c, c; f.focus() } else d == e() && d.select() } function d(d) { var e, f = this, g = a(this), i = f.id; if (!d || "blur" !== d.type || !g.hasClass(n.customClass)) if ("" === f.value) { if ("password" === f.type) { if (!g.data("placeholder-textinput")) { try { e = g.clone().prop({ type: "text" }) } catch (j) { e = a("<input>").attr(a.extend(b(this), { type: "text" })) } e.removeAttr("name").data({ "placeholder-enabled": !0, "placeholder-password": g, "placeholder-id": i }).bind("focus.placeholder", c), g.data({ "placeholder-textinput": e, "placeholder-id": i }).before(e) } f.value = "", g = g.removeAttr("id").hide().prevAll('input[type="text"]:first').attr("id", g.data("placeholder-id")).show() } else { var k = g.data("placeholder-password"); k && (k[0].value = "", g.attr("id", g.data("placeholder-id")).show().nextAll('input[type="password"]:last').hide().removeAttr("id")) } g.addClass(n.customClass), g[0].value = g.attr(h ? "placeholder-x" : "placeholder") } else g.removeClass(n.customClass) } function e() { try { return document.activeElement } catch (a) { } } var f, g, h = !1, i = "[object OperaMini]" === Object.prototype.toString.call(window.operamini), j = "placeholder" in document.createElement("input") && !i && !h, k = "placeholder" in document.createElement("textarea") && !i && !h, l = a.valHooks, m = a.propHooks, n = {}; j && k ? (g = a.fn.placeholder = function () { return this }, g.input = !0, g.textarea = !0) : (g = a.fn.placeholder = function (b) { var e = { customClass: "placeholder" }; return n = a.extend({}, e, b), this.filter((j ? "textarea" : ":input") + "[" + (h ? "placeholder-x" : "placeholder") + "]").not("." + n.customClass).not(":radio, :checkbox, :hidden").bind({ "focus.placeholder": c, "blur.placeholder": d }).data("placeholder-enabled", !0).trigger("blur.placeholder") }, g.input = j, g.textarea = k, f = { get: function (b) { var c = a(b), d = c.data("placeholder-password"); return d ? d[0].value : c.data("placeholder-enabled") && c.hasClass(n.customClass) ? "" : b.value }, set: function (b, f) { var g, h, i = a(b); return "" !== f && (g = i.data("placeholder-textinput"), h = i.data("placeholder-password"), g ? (c.call(g[0], !0, f) || (b.value = f), g[0].value = f) : h && (c.call(b, !0, f) || (h[0].value = f), b.value = f)), i.data("placeholder-enabled") ? ("" === f ? (b.value = f, b != e() && d.call(b)) : (i.hasClass(n.customClass) && c.call(b), b.value = f), i) : (b.value = f, i) } }, j || (l.input = f, m.value = f), k || (l.textarea = f, m.value = f), a(function () { a(document).delegate("form", "submit.placeholder", function () { var b = a("." + n.customClass, this).each(function () { c.call(this, !0, "") }); setTimeout(function () { b.each(d) }, 10) }) }), a(window).bind("beforeunload.placeholder", function () { var b = !0; try { "javascript:void(0)" === document.activeElement.toString() && (b = !1) } catch (c) { } b && a("." + n.customClass).each(function () { this.value = "" }) })) });

    $('input, textarea').placeholder();

    /*
    # =============================================================================
     * What Input - by Jeremy Fields
     # =============================================================================
    */
    !function (e, t) { "function" == typeof define && define.amd ? define([], function () { return t() }) : "object" == typeof exports ? module.exports = t() : e.whatInput = t() }(this, function () { "use strict"; function e(e) { clearTimeout(c), n(e), f = !0, c = setTimeout(function () { f = !1 }, 1e3) } function t(e) { f || n(e) } function n(e) { var t = o(e), n = r(e), d = w[e.type]; "pointer" === d && (d = i(e)), p !== d && (!y && p && "keyboard" === d && "tab" !== v[t] && m.indexOf(n.nodeName.toLowerCase()) >= 0 || (p = d, a.setAttribute("data-whatinput", p), -1 === h.indexOf(p) && h.push(p))), "keyboard" === d && u(t) } function o(e) { return e.keyCode ? e.keyCode : e.which } function r(e) { return e.target || e.srcElement } function i(e) { return "number" == typeof e.pointerType ? b[e.pointerType] : e.pointerType } function u(e) { -1 === s.indexOf(v[e]) && v[e] && s.push(v[e]) } function d(e) { var t = o(e), n = s.indexOf(v[t]); -1 !== n && s.splice(n, 1) } var c, s = [], a = document.body, f = !1, p = null, m = ["input", "select", "textarea"], y = a.hasAttribute("data-whatinput-formtyping"), w = { keydown: "keyboard", mousedown: "mouse", mouseenter: "mouse", touchstart: "touch", pointerdown: "pointer", MSPointerDown: "pointer" }, h = [], v = { 9: "tab", 13: "enter", 16: "shift", 27: "esc", 32: "space", 37: "left", 38: "up", 39: "right", 40: "down" }, b = { 2: "touch", 3: "touch", 4: "mouse" }; return function () { var n = "mousedown"; window.PointerEvent ? n = "pointerdown" : window.MSPointerEvent && (n = "MSPointerDown"), a.addEventListener(n, t), a.addEventListener("mouseenter", t), "ontouchstart" in document.documentElement && a.addEventListener("touchstart", e), a.addEventListener("keydown", t), document.addEventListener("keyup", d) }(), { ask: function () { return p }, keys: function () { return s }, types: function () { return h }, set: n } });

    /*
    # =============================================================================
     * Sidebar-left Function
     # =============================================================================
    */
    $('.sidebar-left ul.sidebar-menu li a').click(function () {
        "use strict";
        $('.sidebar-left li').removeClass('active');
        $(this).closest('li').addClass('active');
        var checkElement = $(this).next();
        if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            $(this).closest('li').removeClass('active');
            checkElement.slideUp('fast');
        }
        if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {

            checkElement.slideDown('fast');
        }
        if ($(this).closest('li').find('ul').children().length == 0) {
            return true;
        } else {
            return false;
        }
    });

    if ($(window).width() < 1025) {
        $(".sidebar-left").removeClass("sidebar-nicescroller");
        $(".sidebar-right").removeClass("sidebar-nicescroller");
        $(".nav-dropdown-content").removeClass("scroll-nav-dropdown");
    }

    /*
    # =============================================================================
     * Toggle Buttons
     # =============================================================================
    */
    $(".btn-collapse-sidebar-left").click(function () {
        "use strict";
        $(".top-navbar").toggleClass("toggle");
        $(".sidebar-left").toggleClass("toggle");
        $(".page-content").toggleClass("toggle");
        $(this).toggleClass("toggle");
        $(".overlay").toggleClass("on");
        $("body").toggleClass("toggle");
    });
    $(".btn-collapse-nav .icon-plus").click(function () {
        "use strict";
        $(".icon-plus").toggleClass("rotate-45");
        $(".topNavigation").toggle();
    });

    /*
    # =============================================================================
     * Dropdown Menu
     # =============================================================================
    */

    $('.dropdown-triger').click(function (e) {
        $(this).siblings('.dropdown-box').toggle();
        // Close one dropdown when selecting another
        $('.dropdown-box').not($(this).siblings()).hide();
        e.stopPropagation();
    });
    // Clicking away from dropdown will remove the dropdown class
    $('html').click(function () {
        $('.dropdown-box').hide();
    });


    /*
    # =============================================================================
     * IE cursor index
     # =============================================================================
    */
    if (document.documentMode && document.documentMode < 12) {
        $(document).on("focus", ":input", function (event) {
            $(window).one("scroll", function () {
                event.target.blur();
            });

            if ($('#divLoader').is(':visible')) {
                event.target.blur();
            };
        });
    }

    /*
    # =============================================================================
    #   Tooltip by Osvaldas Valutis
    # =============================================================================
    */
    $.fn.tooltip = function () {
        var targets = $('.tooltip'),
          target = false,
          tooltip = false,
          title = false;

        targets.bind('mouseenter', function () {
            target = $(this);
            tip = target.attr('title');
            tooltip = $('<div class="tooltip-content"></div>');

            if (!tip || tip == '')
                return false;

            target.removeAttr('title');
            tooltip.css('opacity', 0)
                 .html(tip)
                 .appendTo('body');

            var init_tooltip = function () {
                if ($(window).width() < tooltip.outerWidth() * 1.5)
                    tooltip.css('max-width', $(window).width() / 1.5);
                else
                    tooltip.css('max-width', 340);

                var pos_left = target.offset().left + (target.outerWidth() / 2) - (tooltip.outerWidth() / 2),
                  pos_top = target.offset().top - tooltip.outerHeight() - 25;

                if (pos_left < 0) {
                    pos_left = target.offset().left + target.outerWidth() / 2 - 20;
                    tooltip.addClass('left');
                }
                else
                    tooltip.removeClass('left');

                if (pos_left + tooltip.outerWidth() > $(window).width()) {
                    pos_left = target.offset().left - tooltip.outerWidth() + target.outerWidth() / 2 + 20;
                    tooltip.addClass('right');
                }
                else
                    tooltip.removeClass('right');

                if (pos_top < 0) {
                    var pos_top = target.offset().top + target.outerHeight() + 20;
                    tooltip.addClass('top');
                    console.log(pos_top);
                }
                else
                    tooltip.removeClass('top');

                tooltip.css({ left: pos_left, top: pos_top })
                     .animate({ top: '+=10', opacity: 1 }, 50);
            };

            init_tooltip();
            $(window).resize(init_tooltip);

            var remove_tooltip = function () {
                tooltip.animate({ top: '-=10', opacity: 0 }, 50, function () {
                    $(this).remove();
                });

                target.attr('title', tip);
            };

            target.bind('mouseleave', remove_tooltip);
            tooltip.bind('click', remove_tooltip);
        });
    };

    /*
# =============================================================================
#   Datatable
# =============================================================================
*/
    //$(function () {
    //    $('#example').dataTable({
    //        "bPaginate": true,
    //        "bFilter": false,
    //        "bInfo": false,
    //        "lengthMenu": [[5, 10, 20, -1], [5, 10, 20, "All"]]
    //    });
    //});

    /*
# =============================================================================
#   Miscellaneous
# =============================================================================
*/

    // Handle custom triggering of clicks for browse button in IE
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        $(".file-upload").mousedown(function () {
            $(this).trigger('click');
        });
    }

    $('.tooltip').tooltip();


});