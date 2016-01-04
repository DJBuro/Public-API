(function (b)
{
    b.reject = function (f)
    {
        var a = b.extend(!0, {
            reject: { all: !1, msie5: !0, msie6: !0 }, display: [], browserShow: !0, browserInfo: {
                firefox: { text: "Firefox", url: "http://www.mozilla.com/firefox/" }, safari: { text: "Safari", url: "http://www.apple.com/safari/download/" }, opera: { text: "Opera", url: "http://www.opera.com/download/" }, chrome: { text: "Google Chrome", url: "http://www.google.com/chrome/" }, msie: { text: "Internet Explorer", url: "http://www.microsoft.com/windows/Internet-explorer/" }, gcf: {
                    text: "Google Chrome Frame",
                    url: "http://code.google.com/chrome/chromeframe/", allow: { all: !1, msie: !0 }
                }
            }, header: "Did you know that your Internet Browser is out of date?", paragraph1: "Your browser is out of date, and may not be compatible with our website. A list of the most popular web browsers can be found below.", paragraph2: "Just click on the icons to get to the download page", close: !0, closeMessage: "By closing this window you acknowledge that your experience on this website may be degraded", closeLink: "Close This Window", closeURL: "#",
            closeESC: !0, closeCookie: !1, cookieSettings: { path: "/", expires: 0 }, imagePath: "./images/", overlayBgColor: "#000", overlayOpacity: 0.8, fadeInTime: "fast", fadeOutTime: "fast", analytics: !1
        }, f); 1 > a.display.length && (a.display = "firefox chrome msie safari opera gcf".split(" ")); b.isFunction(a.beforeReject) && a.beforeReject(); a.close || (a.closeESC = !1); f = function (a) { return (a.all ? !0 : !1) || (a[b.os.name] ? !0 : !1) || (a[b.layout.name] ? !0 : !1) || (a[b.browser.name] ? !0 : !1) || (a[b.browser.className] ? !0 : !1) }; if (!f(a.reject))
        {
            if (b.isFunction(a.onFail)) a.onFail();
            return !1
        } if (a.close && a.closeCookie)
        {
            var e = "jreject-close", c = function (c, d)
            {
                if ("undefined" != typeof d) { var e = ""; 0 !== a.cookieSettings.expires && (e = new Date, e.setTime(e.getTime() + 1E3 * a.cookieSettings.expires), e = "; expires=" + e.toGMTString()); var f = a.cookieSettings.path || "/"; document.cookie = c + "=" + encodeURIComponent(!d ? "" : d) + e + "; path=" + f } else {
                    f = null; if (document.cookie && "" !== document.cookie) for (var g = document.cookie.split(";"), h = g.length, i = 0; i < h; ++i) if (e = b.trim(g[i]), e.substring(0, c.length + 1) == c + "=")
                    {
                        f = decodeURIComponent(e.substring(c.length +
                        1)); break
                    } return f
                }
            }; if (c(e)) return !1
        } var d = '<div id="jr_overlay"></div><div id="jr_wrap"><div id="jr_inner"><h1 id="jr_header">' + a.header + "</h1>" + ("" === a.paragraph1 ? "" : "<p>" + a.paragraph1 + "</p>") + ("" === a.paragraph2 ? "" : "<p>" + a.paragraph2 + "</p>"); if (a.browserShow)
        {
            var d = d + "<ul>", i = 0, j; for (j in a.display)
            {
                var m = a.display[j], k = a.browserInfo[m] || !1; if (k && (void 0 == k.allow || f(k.allow))) d += '<li id="jr_' + m + '"><div class="jr_icon"></div><div><a href="' + (k.url || "#") + '">' + (k.text || "Unknown") + "</a></div></li>",
                ++i
            } d += "</ul>"
        } var d = d + ('<div id="jr_close">' + (a.close ? '<a href="' + a.closeURL + '">' + a.closeLink + "</a><p>" + a.closeMessage + "</p>" : "") + "</div></div></div>"), g = b("<div>" + d + "</div>"); j = h(); f = l(); g.bind("closejr", function ()
        {
            if (!a.close) return !1; b.isFunction(a.beforeClose) && a.beforeClose(); b(this).unbind("closejr"); b("#jr_overlay,#jr_wrap").fadeOut(a.fadeOutTime, function () { b(this).remove(); b.isFunction(a.afterClose) && a.afterClose() }); b("embed.jr_hidden, object.jr_hidden, select.jr_hidden, applet.jr_hidden").show().removeClass("jr_hidden");
            a.closeCookie && c(e, "true"); return !0
        }); var n = function (b) { if (a.analytics) { var c = b.split(/\/+/g)[1]; try { _gaq.push(["_trackEvent", "External Links", c, b]) } catch (e) { try { pageTracker._trackEvent("External Links", c, b) } catch (d) { } } } window.open(b, "jr_" + Math.round(11 * Math.random())); return !1 }; g.find("#jr_overlay").css({ width: j[0], height: j[1], background: a.overlayBgColor, opacity: a.overlayOpacity }); g.find("#jr_wrap").css({ top: f[1] + j[3] / 4, left: f[0] }); g.find("#jr_inner").css({
            minWidth: 100 * i, maxWidth: 140 * i, width: "trident" ==
            b.layout.name ? 155 * i : "auto"
        }); g.find("#jr_inner li").css({ background: 'transparent url("' + a.imagePath + 'background_browser.gif")no-repeat scroll left top' }); g.find("#jr_inner li .jr_icon").each(function () { var c = b(this); c.css("background", "transparent url(" + a.imagePath + "browser_" + c.parent("li").attr("id").replace(/jr_/, "") + ".gif) no-repeat scroll left top"); c.click(function () { var a = b(this).next("div").children("a").attr("href"); n(a) }) }); g.find("#jr_inner li a").click(function ()
        {
            n(b(this).attr("href"));
            return !1
        }); g.find("#jr_close a").click(function () { b(this).trigger("closejr"); if ("#" === a.closeURL) return !1 }); b("#jr_overlay").focus(); b("embed, object, select, applet").each(function () { b(this).is(":visible") && b(this).hide().addClass("jr_hidden") }); b("body").append(g.hide().fadeIn(a.fadeInTime)); b(window).bind("resize scroll", function () { var a = h(); b("#jr_overlay").css({ width: a[0], height: a[1] }); var c = l(); b("#jr_wrap").css({ top: c[1] + a[3] / 4, left: c[0] }) }); a.closeESC && b(document).bind("keydown", function (a)
        {
            27 ==
            a.keyCode && g.trigger("closejr")
        }); b.isFunction(a.afterReject) && a.afterReject(); return !0
    }; var h = function ()
    {
        var b = window.innerWidth && window.scrollMaxX ? window.innerWidth + window.scrollMaxX : document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth, a = window.innerHeight && window.scrollMaxY ? window.innerHeight + window.scrollMaxY : document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight, e = window.innerWidth ? window.innerWidth :
        document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth, c = window.innerHeight ? window.innerHeight : document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight; return [b < e ? b : e, a < c ? c : a, e, c]
    }, l = function ()
    {
        return [window.pageXOffset ? window.pageXOffset : document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollLeft : document.body.scrollLeft,
        window.pageYOffset ? window.pageYOffset : document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop]
    }
})(jQuery);
(function (b)
{
    b.browserTest = function (h, l)
    {
        var f = function (a, b) { for (var d = 0; d < b.length; d += 1) a = a.replace(b[d][0], b[d][1]); return a }, a = function (a, c, d, h)
        {
            c = { name: f((c.exec(a) || ["unknown", "unknown"])[1], d) }; c[c.name] = !0; c.version = c.opera ? window.opera.version() : (h.exec(a) || ["X", "X", "X", "X"])[3]; /safari/.test(c.name) && 400 < c.version ? c.version = "2.0" : "presto" === c.name && (c.version = 9.27 < b.browser.version ? "futhark" : "linear_b"); c.versionNumber = parseFloat(c.version, 10) || 0; a = 1; 100 > c.versionNumber && 9 < c.versionNumber &&
            (a = 2); c.versionX = "X" !== c.version ? c.version.substr(0, a) : "X"; c.className = c.name + c.versionX; return c
        }, h = (/Opera|Navigator|Minefield|KHTML|Chrome/.test(h) ? f(h, [[/(Firefox|MSIE|KHTML,\slike\sGecko|Konqueror)/, ""], ["Chrome Safari", "Chrome"], ["KHTML", "Konqueror"], ["Minefield", "Firefox"], ["Navigator", "Netscape"]]) : h).toLowerCase(); b.browser = b.extend(!l ? b.browser : {}, a(h, /(camino|chrome|firefox|netscape|konqueror|lynx|msie|opera|safari)/, [], /(camino|chrome|firefox|netscape|netscape6|opera|version|konqueror|lynx|msie|safari)(\/|\s)([a-z0-9\.\+]*?)(\;|dev|rel|\s|$)/));
        b.layout = a(h, /(gecko|konqueror|msie|opera|webkit)/, [["konqueror", "khtml"], ["msie", "trident"], ["opera", "presto"]], /(applewebkit|rv|konqueror|msie)(\:|\/|\s)([a-z0-9\.]*?)(\;|\)|\s)/); b.os = { name: (/(win|mac|linux|sunos|solaris|iphone|ipad)/.exec(navigator.platform.toLowerCase()) || ["unknown"])[0].replace("sunos", "solaris") }; l || b("html").addClass([b.os.name, b.browser.name, b.browser.className, b.layout.name, b.layout.className].join(" "))
    }; b.browserTest(navigator.userAgent)
})(jQuery);