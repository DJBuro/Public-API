(function($) {
    $.fn.marquee = function(_2) {
        var _3 = [], _4 = this.length;
        function getReset(_5, _6, _7) {
            var _8 = _7.behavior, _9 = _7.width, _a = _7.dir;
            var r = 0;
            if (_8 == "alternate") {
                r = _5 == 1 ? _6[_7.widthAxis] - (_9 * 2) : _9;
            } else {
                if (_8 == "slide") {
                    if (_5 == -1) {
                        r = _a == -1 ? _6[_7.widthAxis] : _9;
                    } else {
                        r = _a == -1 ? _6[_7.widthAxis] - (_9 * 2) : 0;
                    }
                } else {
                    r = _5 == -1 ? _6[_7.widthAxis] : 0;
                }
            }
            return r;
        }
        function animateMarquee() {
            var i = _3.length, _d = null, _e = null, _f = {}, _10 = [], _11 = false;
            while (i--) {
                _d = _3[i];
                _e = $(_d);
                _f = _e.data("marqueeState");
                if (_e.data("paused") !== true) {
                    _d[_f.axis] += (_f.scrollamount * _f.dir);
                    _11 = _f.dir == -1 ? _d[_f.axis] <= getReset(_f.dir * -1, _d, _f) : _d[_f.axis] >= getReset(_f.dir * -1, _d, _f);
                    if ((_f.behavior == "scroll" && _f.last == _d[_f.axis]) || (_f.behavior == "alternate" && _11 && _f.last != -1) || (_f.behavior == "slide" && _11 && _f.last != -1)) {
                        if (_f.behavior == "alternate") {
                            _f.dir *= -1;
                        }
                        _f.last = -1;
                        _e.trigger("stop");
                        _f.loops--;
                        if (_f.loops === 0) {
                            if (_f.behavior != "slide") {
                                _d[_f.axis] = getReset(_f.dir, _d, _f);
                            } else {
                                _d[_f.axis] = getReset(_f.dir * -1, _d, _f);
                            }
                            _e.trigger("end");
                        } else {
                            _10.push(_d);
                            _e.trigger("start");
                            _d[_f.axis] = getReset(_f.dir, _d, _f);
                        }
                    } else {
                        _10.push(_d);
                    }
                    _f.last = _d[_f.axis];
                    _e.data("marqueeState", _f);
                } else {
                    _10.push(_d);
                }
            }
            _3 = _10;
            if (_3.length) {
                setTimeout(animateMarquee, 25);
            }
        }
        this.each(function(i) {
            var _13 = $(this), _14 = _13.attr("width") || _13.width(), _15 = _13.attr("height") || _13.height(), _16 = _13.after("<div " + (_2 ? "class=\"" + _2 + "\" " : "") + "style=\"display: block-inline; width: " + _14 + "px; overflow: hidden;\"><div style=\"float: left; white-space: nowrap;\">" + _13.html() + "</div></div>").next(), _17 = _16.get(0), _18 = 0, _19 = (_13.attr("direction") || "left").toLowerCase(), _1a = { dir: /down|right/.test(_19) ? -1 : 1, axis: /left|right/.test(_19) ? "scrollLeft" : "scrollTop", widthAxis: /left|right/.test(_19) ? "scrollWidth" : "scrollHeight", last: -1, loops: _13.attr("loop") || -1, scrollamount: _13.attr("scrollamount") || this.scrollAmount || 2, behavior: (_13.attr("behavior") || "scroll").toLowerCase(), width: /left|right/.test(_19) ? _14 : _15 };
            if (_13.attr("loop") == -1 && _1a.behavior == "slide") {
                _1a.loops = 1;
            }
            _13.remove();
            if (/left|right/.test(_19)) {
                _16.find("> div").css("padding", "0 " + _14 + "px");
            } else {
                _16.find("> div").css("padding", _15 + "px 0");
            }
            _16.bind("stop", function() {
                _16.data("paused", true);
            }).bind("pause", function() {
                _16.data("paused", true);
            }).bind("start", function() {
                _16.data("paused", false);
            }).bind("unpause", function() {
                _16.data("paused", false);
            }).data("marqueeState", _1a);
            _3.push(_17);
            _17[_1a.axis] = getReset(_1a.dir, _17, _1a);
            _16.trigger("start");
            if (i + 1 == _4) {
                animateMarquee();
            }
        });
        return $(_3);
    };
} (jQuery));

