/*
 * GridViewScroll with jQuery v0.9.6.8
 * http://gridviewscroll.aspcity.idv.tw/

 * Copyright (c) 2012 Likol Lee
 * Released under the MIT license

 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 

 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */
(function (n) { jQuery.fn.extend({ gridviewScroll: function (t) { function dt(n, t) { hr(n, t), cr(n, t), lr(n, t) } function hr(t, i) { t.find("input").each(function () { var r = n(this)[0].type, u; if (r == "checkbox" || r == "radio" || r == "text") { var t = n(this), f = t[0].id.replace("_Copy", ""), e = t[0].name.replace("_Copy", ""); t[0].name = e + "_" + i, t[0].id = f + "_" + i, u = n("#" + f); switch (r) { case "checkbox": case "radio": t.off("change"), t.change(function () { var t = n(this).is(":checked"); u.attr("checked", t) }); break; case "text": t.change(function () { var t = n(this).val(); u.val(t) }) } } }) } function cr(t, i) { t.find("select").each(function () { var t = n(this), u = t[0].id.replace("_Copy", ""), f = t[0].name.replace("_Copy", ""), r; t[0].name = f + "_" + i, t[0].id = u + "_" + i, r = n("#" + u), t.off("change"), t.prop("selectedIndex", r[0].selectedIndex), t.change(function () { var n = this.selectedIndex; r.prop("selectedIndex", n) }) }) } function lr(t, i) { t.find("textarea").each(function () { var t = n(this), r = t[0].id.replace("_Copy", ""), f = t[0].name.replace("_Copy", ""), u; t[0].name = f + "_" + i, t[0].id = r + "_" + i, u = n("#" + r), t.off("change"), t.change(function () { var t = n(this).val(); u.val(t) }) }) } function ar() { var g = a.attr("id") + "Freeze", ot, o, st, nt, w, b, ht, d, ct, s, at, e, h, ft, c, v, t; for (vr(), document.getElementById(g) ? et = n("#" + g) : (et = n(a[0].cloneNode(!1)), et.attr("id", g), et.css({ position: "absolute", width: "", height: "100%", top: 0, left: 0, zIndex: ur }), o = n(ut[0].cloneNode(!1)), ot = o.attr("id") + "Freeze", o.attr("id", ot), o.css({ width: "", height: "100%" }), et[0].appendChild(o[0]), rt[0].appendChild(et[0])), o = et.children().eq(0) ; o[0].hasChildNodes() ;) o[0].removeChild(o[0].lastChild); for (st = document.createElement("TBODY"), o[0].appendChild(st), nt = ut.children().eq(0), w = k[0].length, e = 0; e < i.headerrowcount; e++) { for (h = nt[0].rows[e].cloneNode(!1), t = 0; t < w; t++) (b = k[e][t], b != "RS" && b != "CS") && (ht = parseInt(b.split(":")[1]), t < lt && (v = nt[0].rows[e].cells[ht].cloneNode(!0), h.appendChild(v))); dt(n(h), "freezeheader"), o[0].childNodes[0].appendChild(h) } for (et[0].style.display = "", d = r.attr("id") + "Freeze", document.getElementById(d) ? (p = n("#" + d), p[0].style.height = r[0].style.height, p.scrollTop(0)) : (p = n(r[0].cloneNode(!1)), p.attr("id", d), p.css({ position: "absolute", width: "", top: 0, left: 0, zIndex: ur, display: "none" }), s = n(u[0].cloneNode(!1)), ct = s.attr("id") + "Freeze", s.attr("id", ct), s[0].style.width = "", p[0].appendChild(s[0]), f[0].appendChild(p[0])), s = p.children().eq(0) ; s[0].hasChildNodes() ;) s[0].removeChild(s[0].lastChild); at = document.createElement("TBODY"), s[0].appendChild(at); var vt = l[0].rows.length, tt = lt, it = tt, y = []; for (e = 0; e < vt; e++) { if (h = l[0].rows[e].cloneNode(!1), e < i.headerrowcount) h.style.display = "none"; else { for (t = 0; t < w; t++) ft = l[0].rows[e].cells[t], t < tt && (c = ft.rowSpan, c = c ? parseInt(c) : 1, c != 1 && (it--, y[t] = c), v = ft.cloneNode(!0), h.appendChild(v)); for (t = 0; t < w; t++) y[t] && y[t] > 0 && (y[t]--, y[t] == 0 && it++); tt = it, dt(n(h), "freezeitem") } s[0].childNodes[0].appendChild(h) } p[0].style.display = "" } function vr() { var r = 0, n, t; for (lt = 0, n = 0; n < k[0].length; n++) { if (t = k[0][n], t == "RS" || t == "CS") { lt++; continue } if (i.freezesize == r) return lt; r++, lt++ } } function yr() { var e, r, t, u, o, f, n; for (ni = [], e = l.children().length, n = i.headerrowcount; n < e; n++) r = l[0].rows[n], t = r.cells[0], t.style.height = "", u = t.childNodes[0], u && u.className == "GridCellDiv" || (u = nr(t)); for (o = 1, n = i.headerrowcount; n < e; n++) { var r = l[0].rows[n], t = r.cells[0], u = t.childNodes[0], s = 0; o == 1 ? (f = t.rowSpan, f = f ? parseInt(f) : 1, o = f, s = gi(r)) : o--, s = gi(r), ni[n] = s } for (n = i.headerrowcount; n < e; n++) { var r = l[0].rows[n], t = r.cells[0], u = t.childNodes[0]; ni[n] != 0 && (t.style.height = ni[n] + "px") } } function gi(t) { var u = 0, i = t.cells[0], r = n(i), f = parseInt(r.css("padding-top")), e = parseInt(r.css("padding-bottom")); return (u = i.offsetHeight - f - e) + 0 } function pr() { var rt = u.attr("id") + "VerticalRail", r, ut, l, a, t, v, p, k, tt; document.getElementById(rt) ? c = n("#" + rt) : (c = n(it).css({ background: i.railcolor, width: i.railsize + "px", position: "absolute", zIndex: ot }), c.attr("id", rt), f.append(c), t = { right: 0 }, c.css(t), c.mousedown(function (t) { clearInterval(h); var i = n(this).offset(), r = t.clientY - i.top + n(document).scrollTop(), u = s.offset().top - i.top, f = s.height() + u; r < u && y(-1, !1, !0), r > f && y(1, !1, !0), h = window.setInterval(function () { var r = t.clientY - i.top + n(document).scrollTop(), u = s.offset().top - i.top, f = s.height() + u; r < u && y(-1, !1, !0), r > f && y(1, !1, !0) }, 200) }), c.mouseup(function () { clearInterval(h) }), c.mouseout(function () { clearInterval(h) })), r = u.attr("id") + "VerticalBar", document.getElementById(r) ? s = n("#" + r) : (s = n(it).css({ background: i.barcolor, width: i.barsize + "px", position: "absolute", zIndex: ot }), s.attr("id", r), ut = { right: (i.railsize - i.barsize) / 2 }, s.css(ut), f.append(s), s.draggable({ axis: "y", containment: c, start: function () { n(this).css({ backgroundColor: i.barhovercolor }) }, stop: function () { n(this).css({ backgroundColor: i.barcolor }) }, drag: function () { y(0, !1) } })), l = u.attr("id") + "Vertical_TIMG", document.getElementById(l) ? w = n("#" + l) : (w = n(gt).css({ height: i.arrowsize, position: "absolute", zIndex: ot, top: 0 }), w.attr("id", l), w.attr("src", i.varrowtopimg), f.append(w), t = { right: 0 }, w.css(t), w.mousedown(function () { clearInterval(h), y(-1, !1, !0), h = window.setInterval(function () { y(-1, !1, !0) }, 200) }), w.mouseup(function () { clearInterval(h) }), w.mouseout(function () { clearInterval(h) })), a = u.attr("id") + "Vertical_BIMG", document.getElementById(a) ? b = n("#" + a) : (b = n(gt).css({ height: i.arrowsize, position: "absolute", zIndex: ot }), b.attr("id", a), b.attr("src", i.varrowbottomimg), f.append(b), t = { right: 0 }, b.css(t), b.mousedown(function () { clearInterval(h), y(1, !1, !0), h = window.setInterval(function () { y(1, !1, !0) }, 200) }), b.mouseup(function () { clearInterval(h) }), b.mouseout(function () { clearInterval(h) })), v = u.attr("id") + "HorizontalRail", document.getElementById(v) ? e = n("#" + v) : (e = n(it).css({ background: i.railcolor, height: i.railsize + "px", position: "absolute", zIndex: ot }), e.attr("id", v), f.append(e), e.mousedown(function (t) { clearInterval(h); var i = n(this).offset(), r = t.clientX - i.left + n(document).scrollLeft(), u = o.offset().left - i.left, f = o.width() + u; r < u && d(-1, !0), r > f && d(1, !0), h = window.setInterval(function () { var r = t.clientX - i.left + n(document).scrollLeft(), u = o.offset().left - i.left, f = o.width() + u; r < u && d(-1, !0), r > f && d(1, !0) }, 200) }), e.mouseup(function () { clearInterval(h) }), e.mouseout(function () { clearInterval(h) })), p = u.attr("id") + "HorizontalBar", document.getElementById(p) ? o = n("#" + p) : (o = n(it).css({ background: i.barcolor, height: i.barsize + "px", position: "absolute", zIndex: ot }), o.attr("id", p), f.append(o), o.draggable({ axis: "x", containment: e, start: function () { n(this).css({ backgroundColor: i.barhovercolor }) }, stop: function () { n(this).css({ backgroundColor: i.barcolor }) }, drag: function () { d() } })), k = u.attr("id") + "Horizontal_LIMG", document.getElementById(k) ? g = n("#" + k) : (g = n(gt).css({ width: i.arrowsize, position: "absolute", zIndex: ot, top: 0 }), g.attr("id", k), g.attr("src", i.harrowleftimg), f.append(g), g.mousedown(function () { clearInterval(h), d(-1, !0), h = window.setInterval(function () { d(-1, !0) }, 200) }), g.mouseup(function () { clearInterval(h) }), g.mouseout(function () { clearInterval(h) })), tt = u.attr("id") + "Horizontal_RIMG", document.getElementById(tt) ? nt = n("#" + tt) : (nt = n(gt).css({ width: i.arrowsize, position: "absolute", zIndex: ot }), nt.attr("id", tt), nt.attr("src", i.harrowrightimg), f.append(nt), nt.mousedown(function () { clearInterval(h), d(1, !0), h = window.setInterval(function () { d(1, !0) }, 200) }), nt.mouseup(function () { clearInterval(h) }), nt.mouseout(function () { clearInterval(h) })) } function wr() { var n, t; c.css({ height: f.outerHeight() - i.railsize - i.arrowsize * 2, top: i.arrowsize }), s.css({ top: i.arrowsize }), e.css({ width: f.outerWidth() - i.railsize - i.arrowsize * 2, top: f.outerHeight() - i.railsize, left: i.arrowsize }), o.css({ top: f.outerHeight() - (i.railsize + i.barsize) / 2, left: i.arrowsize }), n = Math.max(r.outerHeight() / r[0].scrollHeight * c.outerHeight(), i.minscrollbarsize), s.css({ height: n + "px" }), t = Math.max(r.outerWidth() / r[0].scrollWidth * e.outerWidth(), i.minscrollbarsize), o.css({ width: t + "px" }), n + i.arrowsize * 2 >= r.outerHeight() || i.verticalbar == "hidden" ? (c.hide(), s.hide(), w.hide(), b.hide(), li = !0) : (c.show(), s.show(), w.show(), b.show()), t + i.arrowsize * 2 >= r.outerWidth() || i.horizontalbar == "hidden" ? (e.hide(), o.hide(), g.hide(), nt.hide()) : (e.show(), o.show(), g.show(), nt.show()), c.is(":hidden") && (rt.css({ width: v }), a.css({ width: v }), f.css({ width: v }), r.css({ width: v }), e.css({ width: v - i.arrowsize * 2 }), t = Math.max(r.outerWidth() / r[0].scrollWidth * e.outerWidth(), i.minscrollbarsize), o.css({ width: t + "px" }), e.css({ top: f.height() - i.railsize }), o.css({ top: f.height() - (i.railsize + i.barsize) / 2 })), e.is(":hidden") ? (c.is(":hidden") ? (f.css({ height: u.height() }), r.css({ height: u.height() }), ft[0].style.height = "") : (f.css({ height: ht }), r.css({ height: ht })), c.css({ height: ht - i.arrowsize * 2 }), n = Math.max(r.outerHeight() / r[0].scrollHeight * c.outerHeight(), i.minscrollbarsize), s.css({ height: n + "px" })) : c.is(":hidden") && (i.height == -1 || at > u.height() + vi - i.railsize) && (f.css({ height: u.height() + i.railsize }), r.css({ height: u.height() }), ft[0].style.height = "", e.css({ top: f.height() - i.railsize }), o.css({ top: f.height() - (i.railsize + i.barsize) / 2 })), w.css({ top: 0 }), b.css({ top: c.outerHeight() + i.arrowsize }), g.css({ top: r.outerHeight() }), nt.css({ top: r.outerHeight(), left: e.outerWidth() + i.arrowsize }), i.arrowsize == 0 && (w.hide(), b.hide(), g.hide(), nt.hide()) } function y(n, t, u, f) { var o = n, l = r.outerHeight() - s.outerHeight() - i.arrowsize, h, a; if (t || u ? (h = 0, h = t ? n * parseInt(i.wheelstep) / 100 : n * .8, o = parseInt(s.css("top")) + h * r.outerHeight() / r[0].scrollHeight * c.outerHeight(), o = Math.min(Math.max(o, i.arrowsize), l), s.css({ top: o + "px" })) : f && (o = Math.min(Math.max(o, i.arrowsize), l), s.css({ top: o + "px" })), typeof i.onScrollVertical == "function") i.onScrollVertical(parseInt(s.css("top")) - i.arrowsize); a = (parseInt(s.css("top")) - i.arrowsize) / (c.outerHeight() - s.outerHeight()), o = a * (r[0].scrollHeight - r.outerHeight()), r.scrollTop(o), i.freezesize != 0 && e[0].style.display != "none" && (o + r.outerHeight() > r[0].scrollHeight && (o = r[0].scrollHeight - r.outerHeight()), p.scrollTop(o)) } function d(n, t, u) { var f = n, h, s, c; if (t ? (h = n * .8, f = parseInt(o.css("left")) + h * r.outerWidth() / r[0].scrollWidth * e.outerWidth(), s = r.outerWidth() - o.outerWidth() - i.arrowsize, f = Math.min(Math.max(f, i.arrowsize), s), o.css({ left: f + "px" })) : u && (s = r.outerWidth() - o.outerWidth() - i.arrowsize, f = Math.min(Math.max(f, i.arrowsize), s), o.css({ left: f + "px" })), typeof i.onScrollHorizontal == "function") i.onScrollHorizontal(parseInt(o.css("left")) - i.arrowsize); c = (parseInt(o.css("left")) - i.arrowsize) / (e.outerWidth() - o.outerWidth()), f = c * (r[0].scrollWidth - r.outerWidth()), f + a.outerWidth() > a[0].scrollWidth && (f = a[0].scrollWidth - a.outerWidth()), r.scrollLeft(f), a.scrollLeft(f) } function br() { var c = st[0].cells.length, a, v, y, o, s, r, e, h, w, p, t; if (ct.show(), i.headerrowcount > 1) for (t = 1; t < i.headerrowcount; t++) l.children().eq(t).show(); for (t = 0; t < c; t++) st[0].cells[t].childNodes[0].style.width = ""; for (a = 1, v = u[0].offsetWidth, v < f[0].offsetWidth && (a = 0), y = ut.children().eq(0), kr(), ai = [], pt = [], t = 0; t < c; t++) pt[t] = !1, o = st[0].cells[t], r = o.childNodes[0].offsetWidth + a, o.style.width && o.style.width != "auto" && (s = o.style.width, s.indexOf("%") == -1 ? r = parseInt(o.style.width) : (s = s.replace("%", ""), r = parseInt(v * (s / 100)), pt[t] = !0)), a == 0 && t == c - 1 && r--, ai[t] = r; for (t = 0; t < c; t++) { if (r = ai[t], st[0].cells[t].childNodes[0].style.width = r + "px", pt[t]) for (n(st[0].cells[t]).css("width", r + "px"), e = i.headerrowcount; e < u[0].rows.length; e++) n(u[0].rows[e].cells[t]).css("width", r + "px"); for (e = 0; e < i.headerrowcount; e++) (h = k[e][t], h != "RS" && h != "CS") && (w = h.split(":")[0], w != "N") && (p = h.split(":")[1], pt[t] && n(y[0].rows[e].cells[p]).css("width", r + "px"), y[0].rows[e].cells[p].childNodes[0].style.width = r + "px") } if (ct.hide(), i.headerrowcount > 1) for (t = 1; t < i.headerrowcount; t++) l.children().eq(t).hide() } function kr() { for (var s = st[0].cells.length, e = [], t, o, f, u, r, n = 0; n < i.headerrowcount; n++) k[n] = [], e[n] = 0; for (t = 0; t < s; t++) for (n = 0; n < i.headerrowcount; n++) if (k[n][t] != "RS" && k[n][t] != "CS") { if (o = l.children().eq(n).children().eq(e[n]), e[n]++, f = o.attr("rowspan"), u = o.attr("colspan"), f = f ? parseInt(f) : 1, u = u ? parseInt(u) : 1, f != 1) for (r = n; r < f; r++) k[r][t] = "RS"; if (u != 1) { for (r = 1; r < u; r++) k[n][r + t] = "CS"; k[n][t] = "N:" + (e[n] - 1) } u == 1 && (k[n][t] = "Y:" + (e[n] - 1)) } return k } function dr() { var r, t; for (u[0].style.display = "none", r = i.headerrowcount + 1, t = 0; t < r; t++) l.children().eq(t).find("td, th").each(function () { var t = n(this)[0]; t.childNodes[0] && (t.childNodes[0].tagName == "DIV" || t.childNodes[0].className == "GridCellDiv") || nr(t) }); u[0].style.display = "" } function nr(n) { var t = document.createElement("DIV"); for (t.className = "GridCellDiv"; n.hasChildNodes() ;) t.appendChild(n.firstChild); return n.appendChild(t), t } function gr() { var p = u.attr("id") + "Wrapper", r = n("#" + p), a, h, f, c, t, v, i, y, e, o, s; if (r[0] && (a = u[0].id + "Copy", h = document.getElementById(a), h)) { for (r[0].parentNode.insertBefore(u[0], r[0]), f = h.rows.length, t = 0; t < f; t++) u[0].rows[t].style.display = ""; for (l = u.children().eq(0), nu(f), c = u[0].rows.length, t = f; t < c; t++) v = l[0].rows[t], i = v.cells[0], i.style.height = "", i.childNodes[0] && i.childNodes[0].tagName == "DIV" && i.childNodes[0].className == "GridCellDiv" && tr(i); y = u[0].id + "PagerBottom", e = document.getElementById(y), e && (o = document.createElement("TD"), o.colSpan = u[0].rows[c - 2].cells.length, o.appendChild(e.childNodes[0]), s = document.createElement("TR"), s.className = e.className, s.appendChild(o), n(s).appendTo(u.children().eq(0))), r.remove() } } function nu(t) { for (var r = t + 1, i = 0; i < r; i++) l.children().eq(i).find("td, th").each(function () { var t = n(this)[0]; t.childNodes[0] && t.childNodes[0].tagName == "DIV" && t.childNodes[0].className == "GridCellDiv" && tr(t) }) } function tr(t) { for (var i = t.childNodes[0]; i.hasChildNodes() ;) t.appendChild(i.firstChild); n(i).remove() } function uu(n) { var t = r[0].scrollLeft, u = r.outerWidth(), f = n.position().left, s = n.outerWidth(); t = f + s - u + t + 5, t < 0 && (t = 0); var h = e.outerWidth() - o.outerWidth(), c = r[0].scrollWidth - r.outerWidth(), l = t / (c / h) + i.arrowsize; o.css({ left: l + "px" }), r.scrollLeft(t), a.scrollLeft(t) } function fu(n) { var u = n.position().top, f = n.outerHeight(), t = r[0].scrollTop, o = r.outerHeight(); t = u + f - o + t + 5, t < 0 && (t = 0); var h = c.outerHeight() - s.outerHeight(), l = r[0].scrollHeight - r.outerHeight(), a = t / (l / h) + i.arrowsize; s.css({ top: a + "px" }), r.scrollTop(t), i.freezesize != 0 && e[0].style.display != "none" && p.scrollTop(t) } var i = n.extend({ width: 500, height: 300, railcolor: "#F0F0F0", barcolor: "#CDCDCD", barhovercolor: "#606060", bgcolor: "#F0F0F0", freezesize: 0, arrowsize: 0, varrowtopimg: "", varrowbottomimg: "", harrowleftimg: "", harrowrightimg: "", headerrowcount: 1, railsize: 15, barsize: 15, wheelstep: 20, minscrollbarsize: 10, startVertical: 0, startHorizontal: 0, onScrollVertical: null, onScrollHorizontal: null, enabled: !0, scrollAssociate: null, verticalbar: "auto", horizontalbar: "auto" }, t), h = null, ir = !0, l = null, ct = null, st = null, vt = !1, li = !1, it = "<div><\/div>", gt = "<img />", rt = null, f = null, a = null, r = null, c = null, s = null, w = null, b = null, e = null, o = null, g = null, nt = null, et = null, p = null, ut = null, yt = null, u = null, ft = null, tt = null, k = [], ai = null, pt = null, ni = null, rr = 0, ur = 0, ot = 0, vi = 0, ht = 0, lt = -1, u = n(this), v, at, ti, ii, ri, ui, fi, ei, oi, si, yi, wt, iu, ru, bt, hi, fr, kt, ci, sr; if (u[0]) { if (!i.enabled) { gr(); return } if (l = u.children().eq(0), !(l.children().length < 2)) { if (v = i.width, at = i.height, v == "100%" && (v = n(window).width()), at == "100%" && (at = n(window).height()), ti = u.attr("id") + "Wrapper", document.getElementById(ti) ? ft = n("#" + ti) : (ft = n(it), ft.attr("id", ti), ft.css({ width: v, height: at }), u.before(ft)), ii = u.attr("id") + "PanelHeader", document.getElementById(ii) ? rt = n("#" + ii) : (rt = n(it), rt.attr("id", ii), rt.appendTo(ft)), rt.css({ background: i.bgcolor }), ri = u.attr("id") + "PanelItem", document.getElementById(ri) ? f = n("#" + ri) : (f = n(it), f.attr("id", ri), f.appendTo(ft)), f.css({ background: i.bgcolor }), ui = u.attr("id") + "PanelHeaderContent", document.getElementById(ui) ? (a = n("#" + ui), a.scrollLeft(0), a.scrollTop(0)) : (a = n(it).css({ background: "#FFFFFF" }), a.attr("id", ui), a.appendTo(rt)), fi = u.attr("id") + "PanelItemContent", document.getElementById(fi) ? (r = n("#" + fi), r.scrollLeft(0), r.scrollTop(0)) : (r = n(it).css({ background: "#FFFFFF" }), r.attr("id", fi), r.appendTo(f), u.appendTo(r)), ct = l.children().eq(0), ct.attr("id", u.attr("id") + "Header"), st = l.children().eq(i.headerrowcount), ei = u.attr("id") + "Copy", document.getElementById(ei) ? ut = n("#" + ei) : (ut = n(u[0].cloneNode(!1)), ut.attr("id", ei), ut.appendTo(a), ir = !1), dr(), oi = ct.attr("id") + "Copy", document.getElementById(oi)) yt = n("#" + oi); else if (yt = ct.clone(!1), yt.attr("id", oi), dt(yt, "Copy"), yt.appendTo(ut), i.headerrowcount > 1) for (si = 1; si < i.headerrowcount; si++) yi = l.children().eq(si).clone(!1), dt(yi, "Copy"), yi.appendTo(ut); if (r[0].style.display = "none", vi = rt[0].offsetHeight, ht = at - vi, r[0].style.display = "", wt = u.attr("id") + "PagerBottom", document.getElementById(wt)) tt = n("#" + wt), tt[0] && tt.width(v); else { var pi = l.children().eq(l.children().length - 1), tu = pi.children().eq(0), wi = tu.children().eq(0); wi[0] != null && wi[0].tagName == "TABLE" && (document.getElementById(wt) || (tt = n(it), tt.attr("id", wt), tt.addClass(pi[0].className), f.after(tt), wi.appendTo(tt), tt.width(v)), pi.remove()) } if (tt && tt[0] && (ht -= tt.height()), f.css({ position: "relative", overflow: "hidden", width: v, height: ht }), r.css({ overflow: "hidden", width: v - i.railsize, height: ht - i.railsize, zIndex: rr }), rt.css({ position: "relative", overflow: "hidden", width: v }), a.css({ overflow: "hidden", width: v - i.railsize, zIndex: rr }), br(), i.freezesize != 0 && (iu = l.children().length - 1, ru = i.headerrowcount + 1, yr()), pr(), wr(), bt = a.attr("id") + "Freeze", hi = r.attr("id") + "Freeze", i.freezesize != 0 && e[0].style.display != "none" ? ar() : (bt = a.attr("id") + "Freeze", hi = r.attr("id") + "Freeze", document.getElementById(bt) && (n("#" + bt).hide(), n("#" + hi).hide())), i.startVertical > 0 ? (kt = parseInt(i.startVertical) + i.arrowsize, y(kt, !1, !1, !0)) : i.startVertical == -1 && (fr = r.outerHeight() - s.outerHeight() - i.arrowsize, y(fr, !1, !1, !0)), i.startHorizontal > 0 && (kt = parseInt(i.startHorizontal) + i.arrowsize, d(kt, !1, !0)), i.scrollAssociate) { var bi = i.scrollAssociate.mode, ki = i.scrollAssociate.target, di = n("#" + ki + "PanelItemContent"), er = n("#" + ki + "VerticalBar"), or = n("#" + ki + "HorizontalBar"); bi == "both" ? di.bind("scroll", function () { var n = parseInt(er.css("top")), t = parseInt(or.css("left")); y(n, !1, !1, !0), d(t, !1, !0) }) : bi == "vertical" ? di.bind("scroll", function () { var n = parseInt(er.css("top")); y(n, !1, !1, !0) }) : bi == "horizontal" && di.bind("scroll", function () { var n = parseInt(or.css("left")); d(n, !1, !0) }) } if (r.bind("keyup", function (t) { var i, u, r; if (t.keyCode == 9) { if (i = n(t.target), !i[0]) return; uu(i), fu(i), u = i[0].id + "_freezeitem", r = n("#" + u), r[0] && r.focus() } }), i.freezesize != 0 && e[0].style.display != "none" && p.bind("keydown", function (t) { var i, u, r; if (t.keyCode == 9) { if (i = n(t.target), !i[0]) return; u = i[0].id.replace("_freezeitem", ""), r = n("#" + u), r[0] && r.focus() } }), !ir) return r.hover(function () { vt = !0 }, function () { vt = !1 }), i.freezesize != 0 && e[0].style.display != "none" && p.hover(function () { vt = !0 }, function () { vt = !1 }), ci = function (n) { if (vt && !c.is(":hidden")) { var n = n || window.event, t = 0; n.wheelDelta && (t = -n.wheelDelta / 120), n.detail && (t = n.detail / 3), y(t, !0), n.preventDefault && !li && n.preventDefault(), li || (n.returnValue = !1) } }, sr = function () { window.addEventListener ? (this.addEventListener("DOMMouseScroll", ci, !1), this.addEventListener("mousewheel", ci, !1)) : document.attachEvent("onmousewheel", ci) }, sr(), this } } } }), jQuery.fn.extend({ gridviewScroll: jQuery.fn.gridviewScroll }) })(jQuery)


function showLastRowLine() {
    var gridid = $('.Gridtablestyle').attr('id');
    var gridid_orig = gridid.substr(0, gridid.lastIndexOf('Copy'));
    $('#' + gridid_orig + 'PanelItem').height("+=15");
    $('#' + gridid_orig + 'PanelItemContent').height("+=15");
}
function showLastRowLine1() {
    var gridid1 = $('.Gridtablestyle1').attr('id');
    var gridid_orig1 = gridid1.substr(0, gridid1.lastIndexOf('Copy'));
    $('#' + gridid_orig1 + 'PanelItem').height("+=15");
    $('#' + gridid_orig1 + 'PanelItemContent').height("+=15");
}
function showLastRowLine2() {
    var gridid2 = $('.Gridtablestyle2').attr('id');
    var gridid_orig2 = gridid2.substr(0, gridid2.lastIndexOf('Copy'));
    $('#' + gridid_orig2 + 'PanelItem').height("+=15");
    $('#' + gridid_orig2 + 'PanelItemContent').height("+=15");
}
function showLastRowLine3() {
    var gridid3 = $('.Gridtablestyle3').attr('id');
    var gridid_orig3 = gridid3.substr(0, gridid3.lastIndexOf('Copy'));
    $('#' + gridid_orig3 + 'PanelItem').height("+=15");
    $('#' + gridid_orig3 + 'PanelItemContent').height("+=15");
}
function showLastRowLine4() {
    var gridid4 = $('.Gridtablestyle4').attr('id');
    var gridid_orig4 = gridid4.substr(0, gridid4.lastIndexOf('Copy'));
    $('#' + gridid_orig4 + 'PanelItem').height("+=15");
    $('#' + gridid_orig4 + 'PanelItemContent').height("+=15");
}
function showLastRowLine5() {
    var gridid5 = $('.Gridtablestyle5').attr('id');
    var gridid_orig5 = gridid5.substr(0, gridid5.lastIndexOf('Copy'));
    $('#' + gridid_orig5 + 'PanelItem').height("+=15");
    $('#' + gridid_orig5 + 'PanelItemContent').height("+=15");
}
function showLastRowLine6() {
    var gridid6 = $('.Gridtablestyle6').attr('id');
    var gridid_orig6 = gridid6.substr(0, gridid6.lastIndexOf('Copy'));
    $('#' + gridid_orig6 + 'PanelItem').height("+=15");
    $('#' + gridid_orig6 + 'PanelItemContent').height("+=15");
}
function showLastRowLine7() {
    var gridid7 = $('.Gridtablestyle7').attr('id');
    var gridid_orig7 = gridid7.substr(0, gridid7.lastIndexOf('Copy'));
    $('#' + gridid_orig7 + 'PanelItem').height("+=15");
    $('#' + gridid_orig7 + 'PanelItemContent').height("+=15");
}
function showLastRowLine8() {
    var gridid8 = $('.Gridtablestyle8').attr('id');
    var gridid_orig8 = gridid8.substr(0, gridid8.lastIndexOf('Copy'));
    $('#' + gridid_orig8 + 'PanelItem').height("+=15");
    $('#' + gridid_orig8 + 'PanelItemContent').height("+=15");
}
function showLastRowLine9() {
    var gridid9 = $('.Gridtablestyle9').attr('id');
    var gridid_orig9 = gridid9.substr(0, gridid9.lastIndexOf('Copy'));
    $('#' + gridid_orig9 + 'PanelItem').height("+=15");
    $('#' + gridid_orig9 + 'PanelItemContent').height("+=15");
}

function LightSearch() {


  
    var Gridclasses = [],
    j = 0;
    $('table[class*="Gridtablestyle"]').not('table[class="PromptGridtablestyle"]').each(function () {

        $(this).find("tr").each(function () {

            $(this).addClass('gridRow');
        });


        gclass = $(this).attr('class');
        Gridclasses.push(gclass);

        x = gclass.replace('Gridtablestyle', '');
        if ($('.searchBox' + x).length == 0) {     
            $(this).parents(':eq(2)').prepend($('<input>', { type: 'text', class: 'searchBox' + x, onkeyup: 'LightSearch();', style: 'border-radius:10px;', placeholder: 'Search...' }));
  
            //V2037 uncomment again
               //// Session  V2034Remove start
                var val = sessionStorage.getItem(x);
                if (val != "" && val != null) {
                    $(".searchBox" + x).val(val + $(".searchBox" + x).val());
                }
            //V2034Remove end
            //V2037 uncomment again
        }
        j++;
    });

    Gridclasses = $.unique(Gridclasses);
    Gridclasses.forEach(function (item) {

        var arrOfTable1 = [],
        i = 0;
        j = 0;
        $('.' + item + ':last .gridRow:eq(1)').children().find("div").each(function () {

            Wid = $(this).width();
            arrOfTable1.push(Wid);
            j++;
        });
      

        $('.' + item + ':last .gridRow:not(:eq(0))').children().each(function () {
            $(this).css("width", arrOfTable1[i] + "px");
            i++;
            if(i == j)
            {
                i = 0;
            }
        });

            //if ($('.' + item + ' .GridviewScrollSelected').length != 0) {

            //    $('.' + item + ' .GridviewScrollSelected').children().each(function () {
            //        $(this).css("width", arrOfTable1[i] + "px");
            //        i++;
            //    });
            //}

     
        

        
        k = item.replace('Gridtablestyle', '');

        $("." + item + " tr:has(td)").hide();

        var iCounter = 0;
        var SearchTerm = $('.searchBox' + k).val();

        ////Session
        sessionStorage.setItem(k, SearchTerm);
        var i = sessionStorage.length;
        if (i != 0 && i != "") {
            while (i--) {
                var key = sessionStorage.key(i);
                if(key != k)
                {
                    sessionStorage.removeItem(key);
                }
            }
        }
        

        if (SearchTerm.length == 0) {

            $("." + item + " tr:has(td)").show();
            return false;
        }


        $("." + item + " tr:has(td)").children().each(function () {
            var cellText = $(this).text().toLowerCase();
            if (cellText.indexOf(SearchTerm.toLowerCase()) >= 0) {

                $(this).parent().show();
                iCounter++;
                return true;
            }


        });
      
        if (iCounter == 0) {

        }


    });

}
