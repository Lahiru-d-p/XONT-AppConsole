(function(){
var KasperskyLab = (function (context) {
    "use strict";
    /**
     * @return {string}
     */
    function GetClass(obj) {
        if (typeof obj === "undefined")
            return "undefined";
        if (obj === null)
            return "null";
        return Object.prototype.toString.call(obj)
            .match(/^\[object\s(.*)\]$/)[1];
    }
    var exports = {}, undef;
    /**
     * @return {string}
     */
    function ObjectToJson(object) {
        if (object === null || object == Infinity || object == -Infinity || object === undef)
            return "null";
        var className = GetClass(object);
        if (className == "Boolean") {
            return "" + object;
        } else if (className == "Number") {
            return window.isNaN(object) ? "null" : "" + object;
        } else if (className == "String") {
            return '"' + object + '"';
        }
        if (typeof object == "object") {
            if (!ObjectToJson.check) ObjectToJson.check = [];
            for (var i=0, chkLen=ObjectToJson.check.length ; i<chkLen ; ++i) {
                if (ObjectToJson.check[i] === object) {
                    throw new TypeError();
                }
            }
            ObjectToJson.check.push(object);
            var str = '';
            if (className == "Array") {
                for (var index = 0, length = object.length; index < length; ++index) {
                    str += ObjectToJson(object[index]) + ',';
                }
                ObjectToJson.check.pop();
                return "["+str.slice(0,-1)+"]";
            } else {
                for (var property in object) {
                    if (object.hasOwnProperty(property)) {
                        str += '"' + property + '":' + ObjectToJson(object[property]) + ',';
                    }
                }
                ObjectToJson.check.pop();
                return "{"+str.slice(0,-1)+"}";
            }
        }
        return undef;
    }
    exports.stringify = function (source) {
        return ObjectToJson(source);
    };
    var parser = {
        source : null,
        grammar : /^[\x20\t\n\r]*(?:([,:\[\]{}]|true|false|null)|(-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)|"((?:[^\r\n\t\\\"]|\\(?:["\\\/trnfb]|u[0-9a-fA-F]{4}))*)")/,
        ThrowError : function() {
            throw new SyntaxError('JSON syntax error');
        },
        NextToken : function(token) {
            this.source = token.input.slice(token[0].length);
            return this.grammar.exec(this.source);
        },
        ParseArray : function(){
            var token = this.grammar.exec(this.source),
                parseItem = token && token[1] != ']',
                result = [];
            for(;;token = this.NextToken(token)) {
                if (!token)
                    this.ThrowError();
                if (parseItem) {
                    result.push(this.ParseValue(token));
                    token = this.grammar.exec(this.source);
                } else {
                    if (token[1]) {
                        if (token[1] == ']') {
                            break;
                        } else if (token[1] != ',') {
                            this.ThrowError();
                        }
                    } else {
                        this.ThrowError();
                    }
                }
                parseItem = !parseItem;
            }
            return result;
        },
        ParseObject : function(){
            var propertyName, parseProperty = true, result = {};
            for(var token = this.grammar.exec(this.source);;token = this.NextToken(token)) {
                if (!token)
                    this.ThrowError();
                if (parseProperty) {
                    if (token[1] && token[1] == '}') {
                        //this.source = this.source.slice(token[0].length);
                        break;
                    } else if (token[1] || token[2] || !token[3]) {
                        this.ThrowError();
                    }
                    propertyName = token[3];
                    token = this.NextToken(token);
                    if (!token || !token[1] || token[1] != ':')
                        this.ThrowError();
                    parseProperty = false;
                } else {
                    if (!propertyName)
                        this.ThrowError();
                    result[ propertyName ] = this.ParseValue(token);
                    token = this.NextToken(this.grammar.exec(this.source));
                    if (token[1]) {
                        if (token[1] == '}') {
                            break;
                        } else if (token[1] != ',') {
                            this.ThrowError();
                        }
                    } else {
                        this.ThrowError();
                    }
                    propertyName = undef;
                    parseProperty = true;
                }
            }
            return result;
        },
        ParseValue : function(token){
            if (token[1]) {
                switch (token[1]){
                    case '[' :
                        this.source = this.source.slice(token[0].length);
                        return this.ParseArray();
                    case '{' :
                        this.source = this.source.slice(token[0].length);
                        return this.ParseObject();
                    case 'true' :
                        return true;
                    case 'false' :
                        return false;
                    case 'null' :
                        return null;
                    default:
                        this.ThrowError();
                }
            } else if (token[2]) {
                return  +token[2];
            }
            return token[3].replace(/\\(?:u(.{4})|(["\\\/\']))/g, function(substr, utfCode, esc){
                return utfCode ? String.fromCharCode(parseInt(utfCode, 16)) : esc;
            });
        },
        Parse : function(str) {
            if ('String' != GetClass(str))
                throw new TypeError();
            this.source = str;
            var token = this.grammar.exec(this.source);
            if (!token)
                this.ThrowError();
            return this.ParseValue(token);
        }
    };
    exports.parse = function (source) {
        return parser.Parse(source);
    };
    context['JSONStringify'] = exports.stringify;
    context['JSONParse'] = exports.parse;
    return context;
}).call(this, KasperskyLab || {});
/** @suppress {duplicate} */ var KasperskyLab = (function (/** !Object */ ns) {
	/** 
	 *  @export
	 *  @const
	 *  @type {!number}
	 */
    ns.MaxRequestDelay = 2000;
    /**
     *  @export
     *  @nosideeffects
     */
	ns.Log = function()
    {
    };
	/**
	 *  @export
	 *  @return {!string}
	 */
    ns.GetResourceSrc = function (/** !string */ resourceName) {
		var prefix = ns.PREFIX || "http://gc.kis.scr.kaspersky-labs.com/";
		var resSignature = ns.RES_SIGNATURE || "CE0A2501E9C1-154B-39B4-22A2-98DB47B1";
        return prefix + resSignature + resourceName;
    };
	/**
	 *  @export
	 */
    ns.AddEventListener = function (/** !Element */ element, /** !string */ name, /** function(Object=) */ func, useCapture) {
        if ("addEventListener" in element)
            element.addEventListener(name, function (e) { func(e || window.event); }, useCapture);
        else
            element.attachEvent('on' + name, function (e) { func.call(element, e || window.event); });
    };
    
	/**
	 *  @export
	 */
    ns.AddRemovableEventListener = function (/** !Element */ element, /** !string */ name, /** function(Object=) */ func) {
        if (element.addEventListener)
            element.addEventListener(name, func, false);
        else
            element.attachEvent('on' + name, func);
    };
	
	/**
	 *  @export
	 */
	ns.RemoveEventListener = function (/** !Element */ element, /** !string */ name, /** function(Object=) */func) {
        if (element.removeEventListener)
            element.removeEventListener(name, func, false);
        else
            element.detachEvent('on' + name, func);
    };
	/**
	 *  @private
	 */
    function InsertStyleRule(/** !Element */ style, /** !string */ rule) {
        if (style.styleSheet)
            style.styleSheet.cssText += rule + '\n';
        else
            style.appendChild(document.createTextNode(rule));
    }
	/**
	 *  @export
	 */
    ns.AddStyles = function (/** Array<!string>= */rules) {
        if (typeof rules !== 'object' || rules.constructor !== Array) {
            return;
        }
        /** @struct */var style = document.createElement('style');
        style.type = 'text/css';
        style.setAttribute('nonce', ns.ContentSecurityPolicyNonceAttribute);
        for (var i = 0, /** @const */len = rules.length; i < len; ++i)
            InsertStyleRule(style, rules[i]);
        if (document.head)
            document.head.appendChild(style);
        else
            document.getElementsByTagName('head')[0].appendChild(style);
    };
	/**
	 *  @export
	 *  @return {!number}
	 */
    ns.GetCurrentTime = function () {
        return new Date().getTime();
    };
	
	/**
	 *  @export
	 *  @return {{left: !number, top: !number}}
	 */
	ns.GetPageScroll = function()
	{
		return {
				left: (document.documentElement && document.documentElement.scrollLeft) || document.body.scrollLeft,
				top: (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop
			};
	};
	/**
	 *  @export
	 *  @return {!number}
	 */
	ns.GetPageHeight = function()
	{
		return document.documentElement.clientHeight || document.body.clientHeight;
	};
	/**
	 *  @export
	 *  @return {!number}
	 */
	ns.GetPageWidth = function()
	{
		return document.documentElement.clientWidth || document.body.clientWidth;
	};
	
	
	
	// Search link filters
	// ---------------------------
	function IsGoogleSearch(linkElement)
	{
		if (linkElement.parentNode.tagName.toLowerCase() === "h3" &&
			linkElement.parentNode.className.toLowerCase() === "r")
			return true;
		return false;
	}
	function IsYandexSearch(linkElement)
	{
		if (linkElement.parentNode.tagName.toLowerCase() === "h2" && (
				linkElement.className.toLowerCase().indexOf("serp-item__title-link") !== -1 ||
				linkElement.className.toLowerCase().indexOf("b-serp-item__title-link") !== -1))
			return true;
		return false;
	}
	function IsYahooSearch(linkElement)
	{
		// chrome: fc-14th ac-1st
		// ff & ie: ac-algo ac-21th
		if (linkElement.className.toLowerCase().indexOf("ac-1st") !== -1 ||
			linkElement.className.toLowerCase().indexOf("ac-21th") !== -1)
			return true;
		return false;
	}
	function IsYahooCoSearch(linkElement)
	{
		if (linkElement.parentNode.tagName.toLowerCase() === "h3" &&
			linkElement.parentNode.parentNode &&
			linkElement.parentNode.parentNode.className.toLowerCase() === "hd")
			return true;
		return false;
	}
	function IsBingSearch(linkElement)
	{
		if (linkElement.parentNode.parentNode && (
				linkElement.parentNode.parentNode.className.toLowerCase().indexOf("sb_tlst") !== -1 ||
				linkElement.parentNode.parentNode.className.toLowerCase().indexOf("b_algo") !== -1))
			return true;
		if (linkElement.parentNode.tagName.toLowerCase() === "h2" &&
			linkElement.parentNode.parentNode &&
			linkElement.parentNode.parentNode.parentNode &&
			linkElement.parentNode.parentNode.className.toLowerCase().indexOf("b_title") !== -1 &&
			linkElement.parentNode.parentNode.parentNode.className.toLowerCase().indexOf("b_algo") !== -1)
			return true;
		return false;
	}
	function IsMailRuSearch(linkElement)
	{
		if (linkElement.target.toLowerCase() === "_blank" && (
			linkElement.parentNode.className.toLowerCase() === "res-head" ||
			linkElement.parentNode.className.toLowerCase() === "result__title"))
			return true;
		return false;
	}
	function IsNigmaRuSearch(linkElement)
	{
		if (linkElement.parentNode.className.toLowerCase() === "snippet_title")
			return true;
		return false;
	}
	function IsRamblerRuSearch(linkElement)
	{
		if (linkElement.className.toLowerCase() === "b-serp-item__link")
			return true;
		return false;
	}
	function IsBaiduComSearch(linkElement)
	{
		if (linkElement.parentNode.className.toLowerCase() === "t")
			return true;
		return false;
	}
	function IsBaiduJpSearch(linkElement)
	{
		if (linkElement.parentNode.tagName.toLowerCase() === "h3" &&
			linkElement.parentNode.parentNode &&
			linkElement.parentNode.parentNode.parentNode &&
			linkElement.parentNode.parentNode.parentNode.className.toLowerCase() === "web")
			return true;
		return false;
	}
	function IsAskComSearch(linkElement)
	{
		if (linkElement.className.toLowerCase() === "web-result-title-link")
			return true;
		return false;
	}
	function NotSearchSite()
	{
		return false;
	}
	
	try
	{
		var currentPageUrl = document.location.href;
		var schemeEndPos = currentPageUrl.indexOf("://");
		var linkFilterFunction;
		if (schemeEndPos !== -1)
		{
			var host = currentPageUrl.substr(schemeEndPos + 3).toLowerCase();
			if (host.indexOf("www.google.") === 0)
				linkFilterFunction = IsGoogleSearch;
			else if (host.indexOf("yandex.")  === 0)
				linkFilterFunction = IsYandexSearch;
			else if (host.indexOf("search.yahoo.com") === 0)
				linkFilterFunction = IsYahooSearch;
			else if (host.indexOf("search.yahoo.co.") === 0)
				linkFilterFunction = IsYahooCoSearch;
			else if (host.indexOf("www.bing.com") === 0)
				linkFilterFunction = IsBingSearch;
			else if (host.indexOf("go.mail.ru") === 0)
				linkFilterFunction = IsMailRuSearch;
			else if (host.indexOf("nigma.ru") === 0)
				linkFilterFunction = IsNigmaRuSearch;
			else if (host.indexOf("www.nigma.ru") === 0)
				linkFilterFunction = IsNigmaRuSearch;
			else if (host.indexOf("nova.rambler.ru") === 0)
				linkFilterFunction = IsRamblerRuSearch;
			else if (host.indexOf("www.baidu.com") === 0)
				linkFilterFunction = IsBaiduComSearch;
			else if (host.indexOf("www.baidu.jp") === 0)
				linkFilterFunction = IsBaiduJpSearch;
			else if (host.indexOf("www.ask.com") === 0)
				linkFilterFunction = IsAskComSearch;
			else
				linkFilterFunction = NotSearchSite;
		}
		
		ns.IsLinkSearchResult = linkFilterFunction;
	}
	catch(e)
	{
		ns.IsLinkSearchResult = NotSearchSite;
	}
	
	function MutationChangeObserver(observeTag)
	{
		var m_observer;
		var m_callback;
		function ProcessNodeList(nodeList)
		{
			for (var i = 0; i < nodeList.length; ++i)
			{
				if (nodeList[i].nodeType === 1 && nodeList[i].getElementsByTagName(observeTag).length > 0)
					return true;
			}
			return false;
		}
		
		
		function ProcessDomChange(records)
		{
			if (!m_callback)
				return;
			for (var i = 0; i < records.length; ++i)
			{
				var record = records[i];
				if ((record.addedNodes.length && ProcessNodeList(record.addedNodes)) ||
					(record.removedNodes.lenght && ProcessNodeList(record.removedNodes)))
				{
					m_callback();
					return;
				}
			}
		}
		
		this.Start = function(callback)
		{
			m_callback = callback;
			m_observer = new MutationObserver(ProcessDomChange);
			m_observer.observe(document, { childList: true, subtree: true });
		};
		this.Stop = function()
		{
			m_observer.disconnect();
			m_callback = null;
		};
	}
	
	function DomEventsChangeObserver(observeTag)
	{
		var m_callback;
	
		function ProcessEvent(event)
		{
			var node = event.target;
			if (!m_callback || node.nodeType !== 1)
				return;
			if (node.nodeName.toLowerCase() === observeTag || node.getElementsByTagName(observeTag).length > 0)
				m_callback();
		}
		this.Start = function(callback)
		{
			window.addEventListener("DOMNodeInserted", ProcessEvent);
			window.addEventListener("DOMNodeRemoved", ProcessEvent);
			m_callback = callback;
		}
		this.Stop = function()
		{
			window.removeEventListener("DOMNodeInserted", ProcessEvent);
			window.removeEventListener("DOMNodeRemoved", ProcessEvent);
			m_callback = null;
		}
	}
	
	function TimeoutChangeObserver(observeTag)
	{
		var m_interval;
		var m_callback;		
		var m_tagCount;
		var m_attribute = 'klot_' + ns.GetCurrentTime();
		function IsChangesOccure(nodeList)
		{
			for (var i = 0; i < nodeList.length; ++i)
				if (!nodeList[i][m_attribute])
					return true;
			return false;
		}
		function FillTagInfo(nodeList)
		{
			m_tagCount = nodeList.length;
			for (var i = 0; i < m_tagCount; ++i)
				nodeList[i][m_attribute] = true;
		}
		
		function TimeoutProcess()
		{
			if (!m_callback)
				return;
			var nodeList = document.getElementsByTagName(observeTag);
			if (nodeList.length !== m_tagCount || IsChangesOccure(nodeList))
			{
				FillTagInfo(nodeList);
				m_callback();
			}
		}
		
		this.Start = function(callback)
		{
			m_callback = callback;
			FillTagInfo(document.getElementsByTagName(observeTag));
			m_interval = setInterval(TimeoutProcess, 10 * 1000);
		}
		this.Stop = function()
		{
			clearInterval(m_interval);
			m_callback = null;
		}
	}
	
	ns.GetDomChangeObserver = function(observeTag)
	{
		var observeTagLowerCase = observeTag.toLowerCase();
		if (window.MutationObserver)
			return new MutationChangeObserver(observeTagLowerCase);
			
		if (window.addEventListener)
			return new DomEventsChangeObserver(observeTagLowerCase);
		
		return new TimeoutChangeObserver(observeTagLowerCase);
	}
    
    return ns;
}) (KasperskyLab || {});
/** @suppress {duplicate} */ var KasperskyLab = (function (/** !Object */ns) {
	/**
	 *  @export
	 *  @constructor
	 *  @final
	 *  @struct
	 *  @param {!string} balloonName
	 *  @param {!number} position
	 *  @param {!function(Element)} createCallback
	 *  @param {function(Object=)=} clickCallback
	 */
    ns.Balloon = function (balloonName, position, createCallback, clickCallback) {
		/** @const @struct*/var self = this;
		/** @const @type{!number} */var IECompatMode = 5; // checked IE render mode 6 && 7 - it is ok. Only 5 is broken
        /** @struct */var m_balloon = null;
        /** @type {?number} */var m_timeout = null;
        /** @const @type{!string} */var m_balloonDivName = 'balloon_parent_div_' + balloonName;
		/**
		 *  @private
		 */
        function AddStyles() {
			ns.AddStyles([
				'.kisb * { position: relative; display:block; overflow-x:hidden; width: auto; margin:0; padding:0; font-family: Verdana; line-height: 150%; text-indent:0; border:0; text-align:left; box-sizing:content-box; letter-spacing: normal;}',
				'.kisb { z-index:2147483647; width: 280px; cursor:default; display:block;}',
				'.kisb a { text-decoration: underline; display:inline-block; }',
				'.kisb a:hover { text-decoration: none; }',
				'.kisb a, .kisb a:link, .kisb a:hover, .kisb a:visited { color: #008ccc;}']);
        }
		/**
		 *  @private
		 */
        function ClearTimeoutInternal() {
            if (m_timeout)
                clearTimeout(m_timeout);
            m_timeout = 0;
        }
		/**
		 *  @private
		 */
        function MouseIsOver() {
            ClearTimeoutInternal.call(self);
        }
		
		/**
		 *  @private
		 */
        function MouseLeaveBalloon() {
            if (!m_timeout) {
                ClearTimeoutInternal();
                m_timeout = setTimeout(function () { HideBalloon(); }, 200);
            }
        }
		/**
		 *  @private
		 *  @param {Object} mouseArgs
		 */
        function MouseClick(mouseArgs) {
            if (clickCallback && clickCallback(mouseArgs || window.event)) {
                HideBalloon.call(self);
            }
        }
		/**
		 *  @private
		 *  @return {!Element}
		 */
        function CreateBalloon() {
            AddStyles();
            /** @struct */var balloonDiv = document.createElement('div');
            balloonDiv.className = 'kisb';
            balloonDiv.id = m_balloonDivName;
			ns.AddEventListener(balloonDiv, 'mouseout', MouseLeaveBalloon);
			ns.AddEventListener(balloonDiv, 'mouseover', MouseIsOver);
			ns.AddEventListener(balloonDiv, 'click', MouseClick);
            createCallback(balloonDiv);
            balloonDiv.style.visibility = 'hidden';
            document.body.appendChild(balloonDiv);
            return balloonDiv;
        }
		/**
		 *  @private
		 *  @param {!Element} element
		 *  @return {{width:!number,height:!number}}
		 */
        function GetElementSize(element) {
			var fixIECompatMode = document.attachEvent && document.documentMode && document.documentMode == IECompatMode;
            var rect = fixIECompatMode ? {width:element.clientWidth || element.scrollWidth, height:element.clientHeight || element.scrollHeight} : element.getBoundingClientRect();
            var width = rect.width ? rect.width : rect.right - rect.left;
            var height = rect.height ? rect.height : rect.bottom - rect.top;
            return { width: width, height: height };
        }
		/**
		 *  @private
		 */
        function HideBalloon() {
            if (!!m_balloon)
                m_balloon.style.visibility = 'hidden';
        }
		/**
		 *  @private
		 *  @param {!number} clientX
		 *  @param {!number} clientY
		 *  @param {number=} showDuring
		 */
        function ShowBalloonInternal(clientX, clientY, showDuring) {
			m_balloon = document.getElementById(m_balloonDivName);
            if (!m_balloon)
                m_balloon = CreateBalloon();
			if (m_balloon.style.visibility == 'visible')
				return;
				
            /** @type {!number} */var x = 0;
            /** @type {!number} */var y = 0;
            /** @type {{width:!number,height:!number}} */var balloonSize = GetElementSize(m_balloon);
            if (position == 1) {
                x = clientX;
                y = clientY - (balloonSize.height ? balloonSize.height : 20);
            }
            else if (position == 2) {
				/** @const @type {!number}*/var clientWidth = ns.GetPageWidth();
                /** @const @type {!number}*/var halfWidth = balloonSize.width / 2;
                if (halfWidth > clientX)
                    x = 0;
                else if (halfWidth + clientX > clientWidth)
                    x = clientWidth - balloonSize.width;
                else
                    x = clientX - halfWidth;
                y = (clientY + balloonSize.height > ns.GetPageHeight()) ? clientY - balloonSize.height : clientY;
            }
            else {
                x = clientX;
                y = clientY;
            }
			if (y < 0)
				y = 0;
				
			/** @struct @const */var scroll = ns.GetPageScroll();
			y += scroll.top;
			x += scroll.left;
            m_balloon.style.position = 'absolute';
            m_balloon.style.left = Math.round(x).toString() + 'px';
            m_balloon.style.top = Math.round(y).toString() + 'px';
            m_balloon.style.visibility = 'visible';
            ClearTimeoutInternal();
            m_timeout = setTimeout(function () { HideBalloon(); }, showDuring);
        }
		/**
		 *  @export
		 *  @param {!function():{x:number,y:number}} getCoord
		 *  @param {!number} showAfter
		 *  @param {number=} showDuring
		 */
        this.ShowBalloon = function (getCoord, showAfter, showDuring) {
            ClearTimeoutInternal();
            if (!showAfter) {
                /** @const */var coord = getCoord();
                ShowBalloonInternal(coord.x, coord.y, showDuring);
            }
            else {
                m_timeout = setTimeout(function () {
                    /** @const */var coord = getCoord();
                    if (coord.isNeed) {
                        ShowBalloonInternal(coord.x, coord.y, showDuring);
                    } else {
                        HideBalloon();
                    }
                }, showAfter);
            }
        }
    };
    return ns;
}) (KasperskyLab || {});
(function(){
// need wrap anonymous function (head + hider + session + plugins + tail)
/** @suppress {duplicate} */var kaspersyLabSessionInstance = null;
(function (/** !Object */ ns) {
    var prefix = ns.PREFIX || "http://gc.kis.scr.kaspersky-labs.com/";
    var signature = ns.SIGNATURE || "1B74BD89-2A22-4B93-B451-1C9E1052A0EC";
    if (window[signature])
    {
        ns.AddRunner = function(){};
        return;
    }
    
    (function (/** !string */ signature) {
        /**
         * @const
         * @type {string}
         */
        var pattern = signature.toLowerCase();
        for (var i = 0, scriptsCount = document.scripts.length; i < scriptsCount; ++i) {
            /** @type {!Element} */ var tag = document.scripts[i];
            if (typeof tag.src === 'string' && tag.src.length > 76 &&
                tag.src.toLowerCase().indexOf(pattern) > 0 &&
                /\/[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}\/main.js/.test(tag.src)) {
                window[signature] = true;
                tag.parentElement.removeChild(tag);
                return; // we MUST inject script tag BEFORE all others scripts
            }
        }
    })(signature);
    /**
     * @type {{GetAsyncRequest : function():!Object, GetSyncRequest : function():!Object}}
     * @private
     */
    var ajaxRequest = (function () {
        /** @const @type {!Function}*/ var oldOpen = window.XMLHttpRequest && window.XMLHttpRequest.prototype.open;
        /** @const @type {!Function}*/ var oldSend = window.XMLHttpRequest && window.XMLHttpRequest.prototype.send;
		/** @const @type {!Function}*/ var oldXHR = window.XMLHttpRequest;
		/** @const @type {!Function}*/ var oldXDR = window.XDomainRequest;
        return {
            /** return {!Object} */
            GetAsyncRequest: function () {
                /** @struct */var xmlhttp = oldXDR ? new oldXDR() : new oldXHR();
                if (!oldXDR) {
                    xmlhttp.open = oldOpen;
                    xmlhttp.send = oldSend;
                }
				xmlhttp.onprogress = function () {};
                return xmlhttp;
            },
            /** return {!Object} */
            GetSyncRequest: function () {
                /** @struct */var xmlhttp = new oldXHR();
                xmlhttp.open = oldOpen;
                xmlhttp.send = oldSend;
				xmlhttp.onprogress = function () {};
                return xmlhttp;
            }
        };
    })();
	function NoCacheParameter() 
	{
		return "&nocache=" + Math.floor((1 + Math.random()) * 0x10000).toString(16);
	}	
	
    /**
     *  @private
     *  @param {!string} address
     */
    function SendAsync(address) {
        try {
			/** @struct */var request = ajaxRequest.GetAsyncRequest();
			if (request) {
				request.open("GET", address);
				request.send();
			} else {
				ns.Log("Ajax is not supported. Cannot send to product request: " + address);
			}
        }
        catch (e) {
            ns.Log("Sending error: " + (e.message || e) + " for: " + address);
        }
    }
    /**
     *  @private
     *  @param {!string} address
	 *  @return {!boolean}
     */
    function SendSync(address) {
        try {
			/** @struct */var request = ajaxRequest.GetSyncRequest();
			if (request) {
				request.open("GET", address, false);
				request.send();
				return request.status === 200;
			}
        }
        catch (e) {
            ns.Log("Sending error: " + (e.message || e) + " for: " + address);
        }
		return false;
    }
    /**
     *  @private
     *  @param {!string} address
     *  @param {string} msg
     */
    function SendLog(address, msg) {
		try {
			ns.Log(msg);
			setTimeout(function () {
				SendAsync(address + '/log?' + encodeURIComponent(msg));
			}, 0);
		}
		catch (e) {
			ns.Log('Sending log error: ' + (e.message || e));
		}
	}
	
    /**
     *  @private
     *  @final
     *  @constructor
     *  @struct
     *  @param {!string} ajaxPath
     */
    var CallReceiver = function (ajaxPath) {
        /** @const @type {!string} */ var m_path = ajaxPath + '/from';
        /** @type{Object<string,Object>} */  var m_plugins = {};
        /** @type{!number} */  var m_requestDelay = ns.MaxRequestDelay;
        /** @type{?number} */ var m_requestTimer = null;
        /** @type{Object} */ var m_request = null;
		/** @type{!boolean} */ var m_isProductConnected = false;
        /**
         *  @private
         *  @param {!string} methodName
         *  @param {!Function} callback
         *  @throws {!string}
         */
        this.RegisterMethod = function (methodName, callback) {
            /** @const @type{?string} */var pluginId = GetPluginIdFromMethodName(methodName);
            if (pluginId) {
                /** @type {Object<string,Function>} */var methods = GetPluginMethods(pluginId);
                if (methods) {
                    if (methods[methodName]) {
                        throw 'Already registered method ' + methodName;
                    }
                    methods[methodName] = callback;
                }
                else {
                    throw 'Cannot registered ' + methodName;
                }
            }
        };
        /**
         *  @private
         *  @param {!string} pluginId
         *  @param {null|undefined|function(number):number} callbackPing
         *  @param {null|undefined|function((number|string|null))} callbackError
         *  @throws {!string}
         */
        this.RegisterPlugin = function (pluginId, callbackPing, callbackError) {
            if (m_plugins[pluginId]) {
                throw 'Already started plugin ' + pluginId;
            }
            /** @struct */
            var plugin = {
                onError: callbackError,
                onPing: callbackPing,
                methods: {}
            };
            m_plugins[pluginId] = plugin;
            if (m_requestTimer === null) {
                UpdateDelay();
                if (IsPluginListEmpty())
                    return;
                m_requestTimer = setTimeout(function ping() {
                    try {
                        UpdateDelay();
                        if (IsPluginListEmpty())
                            return;
                        m_request = m_request || ajaxRequest.GetAsyncRequest();
                        SendRequest(m_request, m_path);
                        m_requestTimer = setTimeout(ping, m_requestDelay);
                    }
                    catch (e) {
                        ReportError('Send ping request: ' + (e.message || e));
                    }
                }, m_requestDelay);
            }
        };
        /**
         *  @private
         *  @param {!string} pluginId
         */
        this.UnregisterPlugin = function (pluginId) {
            delete m_plugins[pluginId];
            if (IsPluginListEmpty())
                ClearTimer();
        };
        /**
         *  @private
         */
        this.UnregisterAll = function () {
            if (m_plugins.length === 0)
                return;
            ClearTimer();
            m_plugins = {};
        };
        /**
         *  @private
         */
        function ClearTimer() {
            try {
                if (m_requestTimer) clearTimeout(m_requestTimer);
            } catch (e) {
            }
            try {
                if (m_request) {
                    m_request.onload = function () {
                    };
                    m_request.onerror = function () {
                    };
                    m_request.abort();
                }
            }
            catch (e) {
            }
            m_requestTimer = null;
            m_request = null;
        }
        /**
         *  @package
         *  @return {!boolean}
         */
        this.IsEmpty = IsPluginListEmpty;
        /**
         *  @private
         *  @return {!boolean}
         */
        function IsPluginListEmpty() {
            for (/** @type{!string} @const*/var key in m_plugins) {
                if (m_plugins.hasOwnProperty(key))
                    return false;
            }
            return true;
        }
		
		/**
		 *	@return {!boolean}
		 */
		this.IsProductConnected = function()
		{
			return m_isProductConnected;
		}
        /**
         *  @private
         */
        function UpdateDelay() {
            /** @type {!number} */var newDelay = ns.MaxRequestDelay;
            /** @const @type {!number} */var currentTime = ns.GetCurrentTime();
            for (/** @type{!string} @const*/var pluginId in m_plugins) {
                try {
                    /**@type {function(number):number}*/var onPing = m_plugins[pluginId].onPing;
                    if (onPing) {
                        /**@const @type{!number}*/var delay = onPing(currentTime);
                        if (delay < newDelay && delay > 0 && delay < ns.MaxRequestDelay) {
                            newDelay = delay;
                        }
                    }
                }
                catch (e) {
                    ReportPluginError(pluginId, 'UpdateDelay: ' + (e.message || e));
                }
            }
            m_requestDelay = newDelay;
        }
        /**
         *  @private
         *  @param {!string} pluginId
         *  @param {(string|number|null)} status
         */
        function ReportPluginError(pluginId, status) {
            /** @const @type{function((string|number|null))}*/var onError = m_plugins[pluginId].onError;
            if (onError)
                onError(status);
        }
        /**
         *  @private
         *  @param {(string|number|null)} status
         */
        function ReportError(status) {
            for (var pluginId in m_plugins)
                ReportPluginError(pluginId, status);
        }
        /**
         *  @private
         *  @param {?string} methodName
         *  @return {(string|null)}
         */
        function GetPluginIdFromMethodName(methodName) {
            if (methodName) {
                /** @const @type{Array<!string>}*/var names = methodName.split('.', 2);
                if (names.length === 2) {
                    return names[0];
                }
            }
            return null;
        }
        /**
         *  @private
         *  @param {!string} pluginId
         *  @return {Object<string,Function>}
         */
        function GetPluginMethods(pluginId) {
            /** @const @struct*/var plugin = m_plugins[pluginId];
            return plugin ? plugin.methods : null;
        }
        /**
         *  @private
         *  @param {!string} pluginId
         *  @param {!string} methodName
         *  @param {Array<string>} args
         *  @return {!boolean}
         */
        function CallPluginMethod(pluginId, methodName, args) {
            /** @const @dict*/var methods = GetPluginMethods(pluginId);
            if (methods) {
                /** @const @type{Function}*/var callback = methods[methodName];
                if (callback) {
                    try {
                        callback(args);
                        SendLog(ajaxPath, methodName + ' executed.');
                        return true;
                    }
                    catch (e) {
                        SendLog(ajaxPath, 'Call ' + methodName + ' in plugin ' + pluginId + ' error: ' + (e.message || e));
                    }
                }
            }
            SendLog(ajaxPath, 'Cannot call ' + methodName + ' for plugin ' + pluginId);
            return false;
        }
        /**
         *  @private
         *  @param {!Object} request
         *  @param {!string} path
         */
        function SendRequest(request, path) {
            try {
                request.open('GET', path);
                request.onerror = function () {
					m_isProductConnected = false;
					PostponeInit();
                    ReportError('AJAX ping network error!');
                };
                request.onload = function () {
					try
					{
						m_isProductConnected = true;
						/**
						 *  @const
						 *  @struct
						 *  @type {null|undefined|{result:number,method:string,parameters:Array<string>}}
						 */
						var command = request.responseText && ns.JSONParse(request.responseText);
						if (!command || typeof command.parameters === "undefined" || typeof command.method === "undefined") {
							ReportError('AJAX pong is not received. Product is deactivated');
							return;
						}
						if (command.method.length) {
							setTimeout(function () { m_request && SendRequest(m_request, m_path); }, 0);
							ns.Log('Try to find js callback ' + command.method);
							/** @const @type{?string}*/var pluginId = GetPluginIdFromMethodName(command.method);
							if (pluginId) {
								CallPluginMethod(pluginId, command.method, command.parameters);
							}
						}
					}
					catch (e)
					{
						ReportError("onload error " + (e.message || e));
					}
                };
                request.send(null);
            }
            catch (e) {
                ReportError('Ajax send ping exception: ' + (e.message || e));
            }
        }
    };
    /**
     *  @private
     *  @final
     *  @constructor
     *  @struct
     *  @param {!string} ajaxId
     *  @param {!string} sessionId
     *  @param {?string} prefix
     */
    var KasperskyLabSessionClass = function (ajaxId, sessionId, prefix) {
        /** @const @struct*/var self = this;
        /** @const @type{!string}*/var m_path = (prefix || '/') + ajaxId + '/' + sessionId;
        /** @const @type{!CallReceiver}*/var m_callReceiver = new CallReceiver(m_path);
        /**
         *  @private
         *  @param {!string} methodName
         *  @param {Array<string>} arrayOfArgs
         *  @param {?function(number=, Array<string>=)} callbackResult
         *  @param {?function(string=)} callbackError
         */
        function Call(methodName, arrayOfArgs, callbackResult, callbackError) {
            try {
                /** @struct */var request = ajaxRequest.GetAsyncRequest();
                if (!request) {
                    callbackError && callbackError('Cannot create AJAX request!');
                }
                /** @const @type{boolean} */ var noParameters = !arrayOfArgs || arrayOfArgs.length === 0;
                if (noParameters) {
                    request.open('GET', m_path + '/to/' + methodName + '/?get' + NoCacheParameter());
                } else {
                    request.open('POST', m_path + '/to/' + methodName);
                }
                /** type{?number} */
                var timeout = setTimeout(function () {
                    request.abort();
                    callbackError && callbackError('Cannot send AJAX request for calling ' + methodName);
                }, 120000);
                request.onerror = function () {
                    request.onerror = function () {
                    };
                    request.onload = function () {
                    };
                    callbackError && callbackError('AJAX request error for calling ' + methodName);
                };
                request.onload = function () {
                    clearTimeout(timeout);
                    timeout = null;
                    request.onload = function () {
                    };
                    request.onerror = function () {
                    };
                    if (request.responseText) {
                        if (callbackResult) {
                            /**
                             *  @const
                             *  @struct
                             *  @type {{result:!number,method:!string,parameters:Array<!string>}}
                             */
                            var command = ns.JSONParse(request.responseText);
                            callbackResult(command.result, command.parameters);
                        }
                        request = null;
                        return;
                    }
                    request = null;
                    if (callbackError) {
                        callbackError('AJAX request with unsupported url type!'); // when unsupported url type (file://) status == 0
                    }
                };
                request.send(noParameters ? null : ns.JSONStringify({
                    result: 0,
                    method: methodName,
                    parameters: arrayOfArgs
                }));
                ns.Log('Call native function ' + methodName);
            }
            catch (e) {
                if (callbackError) {
                    callbackError('AJAX request ' + methodName + ' exception: ' + (e.message || e));
                }
            }
        }
        /**
         *  @private
         *  @param {!string} methodName
         *  @param {Array<string>} arrayOfArgs
         *  @param {?function(number=, Array<string>=)=} callbackResult
         *  @param {?function(string=)=} callbackError
         *  @return {!boolean}
         */
        function SyncCall(methodName, arrayOfArgs, callbackResult, callbackError) {
            try {
                /** @struct */var request = ajaxRequest.GetSyncRequest();
                if (!request) {
                    callbackError && callbackError('Cannot create AJAX request!');
                    return false;
                }
                /** @const @type{boolean} */ var noParameters = !arrayOfArgs || arrayOfArgs.length === 0;
                if (noParameters) {
                    request.open('GET', m_path + '/to/' + methodName + '/?get' + NoCacheParameter(), false);
                } else {
                    request.open('POST', m_path + '/to/' + methodName, false);
                }
                ns.Log('SyncCall native function ' + methodName);
                request.send(noParameters ? null : ns.JSONStringify({
                    result: 0,
                    method: methodName,
                    parameters: arrayOfArgs
                }));
                if (request.status === 200 && request.responseText) {
                    if (callbackResult) {
                        /**
                         *  @const
                         *  @struct
                         *  @type {{result:!number,method:!string,parameters:Array<!string>}}
                         */
                        var command = ns.JSONParse(request.responseText);
                        callbackResult(command.result, command.parameters);
                    }
                    request = null;
                    return true;
                }
            }
            catch (e) {
                if (callbackError)
                    callbackError('AJAX request ' + methodName + ' exception: ' + (e.message || e));
            }
            return false;
        }
        /**
         *  @private
         */
        function Stop() {
            try {
                m_callReceiver.UnregisterAll();
                ns.Log("session stopped");
			    
				if (m_callReceiver.IsProductConnected())
				{
					var shutdownCommandPath = m_path + "/shutdown?get" + NoCacheParameter();
					if (!SendSync(shutdownCommandPath))
						SendAsync(shutdownCommandPath);
				}
            }
            catch (e) {
            }
        }
        /**
         *  @private
         *  @param {!string} pluginId
         */
        function DeactivatePlugin(pluginId) {
            ns.Log('DeactivatePlugin ' + pluginId);
            m_callReceiver.UnregisterPlugin(pluginId);
            if (m_callReceiver.IsEmpty()) {
                Stop();
            }
        }
        /**
         *  @private
         *  @param {!string} pluginId
         *  @param {null|undefined|function(number):number} callbackPing
         *  @param {null|undefined|function((number|string|null))} callbackError
         */
        function ActivatePlugin(pluginId, callbackPing, callbackError) {
            ns.Log('ActivatePlugin ' + pluginId);
            m_callReceiver.RegisterPlugin(pluginId, callbackPing, function (e) {
                callbackError && callbackError(e);
                m_callReceiver.UnregisterPlugin(pluginId);
                if (m_callReceiver.IsEmpty()) {
                    Stop();
                }
            });
        }
        /**
         *  @private
         *  @param {!string} methodName
         *  @param {!Function} callback
         */
        function RegisterMethod(methodName, callback) {
            ns.Log('RegisterMethod ' + methodName);
            m_callReceiver.RegisterMethod(methodName, callback);
        }
        /**
         *  @export
         *  @param {!string} msg
         */
        this.Log = function (msg) {
            msg && SendLog(m_path, msg);
        };
        /**
         *  @export
         *  @param {!function(Function=, Function=, Function=, Function=, Function=)} init
         */
        this.InitializePlugin = function (init) {
            init(
                function () {
                    ActivatePlugin.apply(self, arguments);
                },
                function () {
                    RegisterMethod.apply(self, arguments);
                },
                function () {
                    Call.apply(self, arguments);
                },
                function () {
                    DeactivatePlugin.apply(self, arguments);
                },
                function () {
                    return SyncCall.apply(self, arguments);
                }
            );
        };
		
		ns.AddEventListener(window, "beforeunload", function() 
			{
				if (!m_callReceiver.IsEmpty())
					Stop();
			});
    };
	var runners = {};
	var pluginsInitData = [];
	ns.AddRunner = function(pluginName, runnerFunc, initParameters)
	{
		runners[pluginName] = runnerFunc;
		if (initParameters)
		{
			pluginsInitData.push({plugin: pluginName, parameters: initParameters});
		}
	};
	
	ns.ContentSecurityPolicyNonceAttribute = signature;
	function GetSpecialPlugins(predefined) 
	{
		return (predefined) ? "&plugins=" + encodeURIComponent(predefined) : "";    				
	}
	function IsDefined(variable)
	{
	    return "undefined" !== typeof(variable);
	}
	function Init()
	{
		var specialPlugins = IsDefined(ns.PLUGINS_LIST) ? GetSpecialPlugins(ns.PLUGINS_LIST) : GetSpecialPlugins("");
		var connectRequest = ajaxRequest.GetAsyncRequest();
		var serializedInitData = (pluginsInitData.length) ? "&data=" + encodeURIComponent(ns.JSONStringify({data : pluginsInitData})) : "";
		var initTimeout = setTimeout(function ()
			{
				connectRequest.abort();
				PostponeInit();
			}, 120000);
		
		connectRequest.open("GET", prefix + signature + "/init?url=" + encodeURIComponent(document.location.href) + specialPlugins + serializedInitData + NoCacheParameter());
		connectRequest.onload = function()
			{
				clearTimeout(initTimeout);
				/**
				*  @const
				*  @struct
				*  @type {{ajaxId:string,localization:string,plugins:Array<{name:string,localization:string,settings:Object}>}}
				*/
				var initSettings = connectRequest.responseText && ns.JSONParse(connectRequest.responseText);
				kaspersyLabSessionInstance = new KasperskyLabSessionClass(initSettings.ajaxId, initSettings.sessionId, prefix);
				var plugins = initSettings.plugins;
				for (var i = 0, pluginsCount = plugins.length; i < pluginsCount; ++i)
				{
					var plugin = plugins[i];
					var pluginRunnerFunction = runners[plugin.name];
					if (pluginRunnerFunction)
						pluginRunnerFunction(KasperskyLab, kaspersyLabSessionInstance, plugin.settings, plugin.localization);
				}
				
				connectRequest.onload = function(){};
				connectRequest.onerror = function(){};
			};
		connectRequest.onerror = function()
			{
				clearTimeout(initTimeout);
				connectRequest.onload = function(){};
				connectRequest.onerror = function(){};
				PostponeInit();
			};
		connectRequest.send(null);
	}
	
	var postponedInitTimeout = null;
	function PostponeInit()
	{
		clearTimeout(postponedInitTimeout)
		postponedInitTimeout = setTimeout(function () { Init(); }, 60 * 1000);
	}
	
	setTimeout(function () { Init(); }, 0);
})(KasperskyLab);
(function (ns) 
{
ns.waitForApiInjection = function(isApiInjected, eventName, callback)
{
    if (isApiInjected())
    {
        callback();
        return;
    }
    var subscription = createSubscription(eventName, onApiInjected)
    function onApiInjected()
    {
        if (isApiInjected())
        {
            subscription.unsubscribe();
            callback();
        }
    }
}
function createSubscription(eventName, callback)
{
    var windowEventsSupported = document.createEvent || window.addEventListener;
    return new (windowEventsSupported ? ModernSubscription : IeLegacySubscription)(eventName, callback);
}
function ModernSubscription(eventName, callback)
{
    ns.AddRemovableEventListener(window, eventName, callback);
    this.unsubscribe = function()
    {
        ns.RemoveEventListener(window, eventName, callback);
    }
}
function IeLegacySubscription(eventName, callback)
{
    ns.AddRemovableEventListener(document.documentElement, 'propertychange', onPropertyChange);
    this.unsubscribe = function()
    {
        ns.RemoveEventListener(document.documentElement, 'propertychange', onPropertyChange);
    }
    function onPropertyChange(event)
    {
        if (event.propertyName == eventName)
            callback();
    }
}
})(KasperskyLab || {});
var tabIdPropertyName = KasperskyLab.LIGHT_PLUGIN_API_KEY || 'klTabId_kis';
var scriptPluginId = Math.floor((1 + Math.random()) * 0x10000).toString(16);
function isApiInjected()
{
    return !!window[tabIdPropertyName];
}
function makeTopLevelArgument()
{
    return window == window.top ? 'true' : 'false';
}
function removeTabIdProperty()
{
	try {
		delete window[tabIdPropertyName];
	} catch (e) {
		window[tabIdPropertyName] = undefined;
	}	
}
var documentInitParameters = isApiInjected() ? [String(window[tabIdPropertyName]), makeTopLevelArgument(), scriptPluginId] : null;
KasperskyLab.AddRunner("light_doc", function (ns, session)
{
session.InitializePlugin(function(activatePlugin, _, callFunction)
{
	activatePlugin('light_doc');
	
	if (documentInitParameters)
	{
		removeTabIdProperty();
		return;
	}
	
	ns.waitForApiInjection(isApiInjected, tabIdPropertyName, function()
	{
		var tabId = String(window[tabIdPropertyName]);
		removeTabIdProperty();
		callFunction("light_doc.registerDocument", [
			tabId, 
			document.URL,
			makeTopLevelArgument(),
            scriptPluginId
			]);
	});	
});
}, documentInitParameters);
(function (ns)
{
ns.IsPositionEqual = function(prevPos, currentPos)
{
	return prevPos && currentPos && prevPos.top === currentPos.top && prevPos.left === currentPos.left;
};
	
ns.GetAbsoluteElementPosition = function(element)
{
	var box = element.getBoundingClientRect();
	var scroll = ns.GetPageScroll();
	return {
			left: box.left + scroll.left,
			top: box.top + scroll.top,
			right: box.right + scroll.left,
			bottom: box.bottom + scroll.top
		};
};
})(KasperskyLab || {});
(function (ns) 
{
ns.ProtectableElementDetector = function(protectMode)
{
	var m_typesForbidden = ['hidden', 'submit', 'radio', 'checkbox', 'button', 'image', 'number', 'email'];
	var m_protectMode = protectMode;
	/**
	 * @return {boolean}
	 */
	this.Test = function(element)
	{
		if (m_protectMode < 2 || m_protectMode > 3)
			return false;
			
		var elementType = element.getAttribute('type');
		elementType = elementType && elementType.toLowerCase();
		if (m_protectMode === 2)
		{
			if (elementType != 'password')
				return false;
		}
		else if (Includes(m_typesForbidden, elementType))
		{
			return false;
		}
		if (GetComputedStyle(element, 'display') === 'none')
			return false;
		var maxLength = parseInt(element.getAttribute('maxlength'), 10);
		return typeof maxLength === 'number' && maxLength <= 3 ? false : !element.readOnly;
	};
	/**
	 * @return {boolean}
	 */
	function Includes(list, text)
	{
		var i = 0, count = list.length;
		for (; i < count; ++i)
		    if (list[i] === text)
				return true;
		return false;
	}
	/**
	 * @return {string}
	 */
	function GetComputedStyle(element, property)
	{
		var value;
		if (element.currentStyle)
		{
			value = element.currentStyle[property];
		}
		else
		{
			var styles = window.getComputedStyle(element, '');
			if (styles)
				value = styles.getPropertyValue(property);
		}
		return typeof value !== 'string' ? '' : value.toLowerCase();
	}
}
})(KasperskyLab || {});
(function (ns)
{
ns.SecureInputTooltip = function(locales)
{
	var m_tooltip,
		// position helpers
		m_curPosition,
		m_top,
		m_bottom,
		m_pointer,
		m_needRestoreFocus,
		that = this;
	function CreateDivWithStyle(cssText)
	{
		var div = document.createElement('div');
		div.style.cssText = cssText;
		return div;
	}
	
	function CreateTooltip()
	{
		var balloonDiv = CreateDivWithStyle("position:absolute;display:block;width:auto;height:auto;padding:0;margin:0;text-align:left;border:none;border-radius:0;background:#transparent;z-index:2147483646;font-size:0px");
		var iconDiv = CreateDivWithStyle("position:relative;display:block;height:auto;width:auto;padding:35px 10px 11px;margin:0;text-align:left;border:1px solid #B2B2B2;border-radius:5px;background:#fff url(" + KasperskyLab.GetResourceSrc('/vk/VkTooltipBanner.png') + ") 10px 9px no-repeat;z-index:2147483646;");
		var close = CreateDivWithStyle("position:absolute;right:10px;top:6px;display:inline-block;height:16px;width:8px;padding:0;margin:0;text-align:left;background:url(" + KasperskyLab.GetResourceSrc('/vk/close_icon.png') + ") right center no-repeat;cursor:pointer;border:none;border-radius:0;white-space:nowrap;z-index:2147483646;");
		var contentText = CreateDivWithStyle("position:static;display:block;width:auto;height:auto;padding:0;margin:0;font:11px/1 Arial,Helvetica,sans-serif;text-align:left;color:#000;border:none;border-radius:0;background:#transparent;white-space:nowrap;z-index:2147483646;");
		m_top = CreateDivWithStyle("position:relative;display:block;width:32px;height:12px;padding:0;margin:0 0 -1px;text-align:left;border:none;border-radius:0;background:#transparent;z-index:2147483647;");
		m_bottom = CreateDivWithStyle("position:relative;display:block;width:32px;height:12px;padding:0;margin:-1px 0 0;text-align:left;border:none;border-radius:0;background:#transparent;z-index:2147483647;");
		m_pointer = CreateDivWithStyle("position:static;display:block;padding:0;margin:0;height:12px;width:100%;z-index:2147483647;");
		
		balloonDiv.appendChild(m_top);
		balloonDiv.appendChild(iconDiv);
		balloonDiv.appendChild(m_bottom);
		
		iconDiv.appendChild(close);
		iconDiv.appendChild(contentText);
		
		contentText.appendChild(document.createTextNode(locales["VkTooltipText"]));
		
		close.onmouseover = function(){m_needRestoreFocus = true;};
		close.onmouseout = function(){m_needRestoreFocus = false;};
		
		m_tooltip = balloonDiv;
	}
    function PositionTooltipPointer(position)
	{
		if (m_pointer.parentNode)
			m_pointer.parentNode.removeChild(m_pointer);
		
		var destinationDiv, pointerBackgroundImage;
		if (position === "top")
		{
			pointerBackgroundImage = "url(" + ns.GetResourceSrc('/vk/pointer_top.png') + ") no-repeat right bottom";
			destinationDiv = m_top;
        } 
		else if (position === "bottom") 
		{
			pointerBackgroundImage = "url(" + ns.GetResourceSrc('/vk/pointer_bottom.png') + ") no-repeat right top";
            destinationDiv = m_bottom;
        }
		m_pointer.style.background = pointerBackgroundImage;
		destinationDiv.appendChild(m_pointer);
    }
    function PositionTooltip(element)
	{
        var inputPosition = ns.GetAbsoluteElementPosition(element);
        if (!ns.IsPositionEqual(m_curPosition, inputPosition))
		{
            m_tooltip.style.left = inputPosition.left + "px";
			
			var tooltipHeight = m_tooltip.offsetHeight;
			var inputTopRelative = inputPosition.top - ns.GetPageScroll().top;
			var clientHeightUnderInput = ns.GetPageHeight() - inputTopRelative - element.offsetHeight;
            if ((clientHeightUnderInput > tooltipHeight - 1) || 
				(inputPosition.top - tooltipHeight + 1 < 0))
			{
                m_tooltip.style.top = (inputPosition.top + element.offsetHeight - 1) + "px";
                PositionTooltipPointer("top");
            }
			else
			{
                m_tooltip.style.top = (inputPosition.top - tooltipHeight + 1) + "px";
                PositionTooltipPointer("bottom");
            }
            m_curPosition = inputPosition;
        }
	}
	this.Show = function(element)
	{
		document.body.appendChild(m_tooltip);
		PositionTooltip(element);
		var timer = setInterval(function() { PositionTooltip(element); }, 100);
		this.Hide = function()
		{
			clearInterval(timer);
			document.body.removeChild(m_tooltip);
			if (m_needRestoreFocus)
				setTimeout(function(){element.focus();}, 0);
			m_tooltip = null;
			this.Show = function(){};
			this.Hide = function(){};
		};
		setTimeout(function()
		{
			that.Hide();
		}, 3000);
	};
	this.Hide = function(){};
	CreateTooltip();
};
})(KasperskyLab || {});
(function (ns)
{
ns.VirtualKeyboardInputIcon = function(clickCallback)
{
	var m_curPosition,
		m_iconDiv,
		m_element;
	
	function CreateDivWithStyle(cssText)
	{
		var div = document.createElement('div');
		div.style.cssText = cssText;
		return div;
	}
	function PositionIcon()
	{
		var iconStyle = m_iconDiv.style,
			inputPosition = ns.GetAbsoluteElementPosition(m_element);
		if (!ns.IsPositionEqual(m_curPosition, inputPosition))
		{
			iconStyle.left = inputPosition.left + m_element.offsetWidth - 20 + 'px';
			iconStyle.top = inputPosition.top + (m_element.offsetHeight - 16) / 2 + 'px';
			m_curPosition = inputPosition;
		}
	}
	
	function CreateIcon()
	{
		m_iconDiv = CreateDivWithStyle('position:absolute;display:block;width:16px;height:16px;background:url(' + ns.GetResourceSrc('/vk/VkInputFieldIcon.png') + ') no-repeat center center;cursor:pointer;z-index:2147483647;font-size:0px');
		ns.AddEventListener(m_iconDiv, 'mouseover', function ()
		{
			m_iconDiv.style.filter = 'alpha(opacity=60)';	// IE8
			m_iconDiv.style.opacity = 0.6;
		});
		ns.AddEventListener(m_iconDiv, 'mouseout', function()
		{
			m_iconDiv.style.filter = 'alpha(opacity=100)';	// IE8
			m_iconDiv.style.opacity = 1;
		});
		ns.AddEventListener(m_iconDiv, 'click', clickCallback);
	}
	
	CreateIcon();
	
	// showing icon
	this.Show = function(element)
	{
		this.Hide();
		m_element = element;
		document.body.appendChild(m_iconDiv);
		PositionIcon();
		var timer = setInterval(PositionIcon, 100);
		this.Hide = function()
		{
			clearInterval(timer);
			document.body.removeChild(m_iconDiv);
			this.Hide = function(){};
		};
	};
	this.Hide = function(){};
};
})(KasperskyLab || {});
KasperskyLab.AddRunner("vk", function (ns, session, settings, locales)
{
var VirtualKeyboard = function()
{
	var m_callFunction, m_syncCallFunction;
	var m_virtualKeyboardIconShowMode = 0;
	var m_secureInputProtectMode = 0;
	var m_activeElement = null;
	var m_lastFocusedElement = null;
	
	// 0 - not protected
	// 1 - starting protect
	// 2 - protected
	// 3 - stopping protect
	var m_protectedState = 0;
	var m_enabledSecureInput = false;
	var m_protectChangeTimeout;
	
	var m_protectableVirtualKeyboardChecker = new ns.ProtectableElementDetector(settings.vkProtectMode);
	var m_protectableSecureInputChecker = null;
	var m_protectableVirtualKeyboardIconChecker = null;
	
	var m_attributeName = "vk_" + Math.floor((1 + Math.random()) * 0x10000).toString(16);
	
	function ShowVirtualKeyboard()
	{
		if (m_lastFocusedElement)
			m_lastFocusedElement.focus();
		m_callFunction("vk.showKeyboard");
	}
	var m_tooltip = new ns.SecureInputTooltip(locales);
	var m_icon = new ns.VirtualKeyboardInputIcon(ShowVirtualKeyboard);
	var m_iconHideTimer;
	var m_postponeStart;
	var m_shutdown = false;
	session.InitializePlugin(function(activatePlugin, registerMethod, callFunction, deactivatePlugin, syncCallFunction){
		m_callFunction = callFunction;
		m_syncCallFunction = syncCallFunction;
		activatePlugin('vk', OnPing);
		registerMethod('vk.settings', SetSettings);
	});
	/**
	 * @return {number}
	 */
	function OnPing()
	{
		return ns.MaxRequestDelay;
	}
	
	function SetSettings(argumentList)
	{
		var newVirtualKeyboardIconShowMode = parseInt(argumentList[0], 10);
		var newSecureInputProtectMode = parseInt(argumentList[1], 10);
		SetSettingsImpl(newVirtualKeyboardIconShowMode, newSecureInputProtectMode);
	}
	
	function SetSettingsImpl(newVirtualKeyboardIconShowMode, newSecureInputProtectMode)
	{
		if (newSecureInputProtectMode != m_secureInputProtectMode)
			m_protectableSecureInputChecker = new ns.ProtectableElementDetector(newSecureInputProtectMode);
		if (newVirtualKeyboardIconShowMode != m_virtualKeyboardIconShowMode)
			m_protectableVirtualKeyboardIconChecker = new ns.ProtectableElementDetector(newVirtualKeyboardIconShowMode);
		
		if (newSecureInputProtectMode > m_secureInputProtectMode ||
			newVirtualKeyboardIconShowMode > m_virtualKeyboardIconShowMode)
		{
			SetEventListenersToElements(document.getElementsByTagName('input'),
				function(event) { OnElementFocus(event.target || event.srcElement); },
				function() { OnElementBlur(); });
		}
		// TODO: may be need send blur if current active element don't need protect
		
		m_secureInputProtectMode = newSecureInputProtectMode;
		m_virtualKeyboardIconShowMode = newVirtualKeyboardIconShowMode;
	}
	
	function NeedProtectElement(element)
	{
		return m_protectableSecureInputChecker.Test(element) || m_protectableVirtualKeyboardChecker.Test(element);
	}
	
	function SetEventListenersToElement(element, focusHandler, blurHandler, currentFocusedElement)
	{
		if (!NeedProtectElement(element))
			return;
		
		var secureInputValue = element.getAttribute(m_attributeName);
		if (secureInputValue && secureInputValue === "subscribed")
			return;
		element.setAttribute(m_attributeName, "subscribed");
		ns.AddEventListener(element, "focus", focusHandler, true);
		ns.AddEventListener(element, "blur", blurHandler, true);
		if (element === currentFocusedElement)
			OnElementFocus(element);
	}
	
	function SetEventListenersToElements(elements, focusHandler, blurHandler)
	{
		var currentFocusedElement = GetFocusedElement();
		var i = 0, count = elements.length;
		for (; i < count; ++i)
			SetEventListenersToElement(elements[i], focusHandler, blurHandler, currentFocusedElement);
	}
	function GetFocusedElement()
	{
		var element = document.activeElement;
		if (document.hasFocus() &&
			element &&
			element.tagName.toLowerCase() === 'input')
		{
			return element;
		}
		return null;
	}
	
	function HandleStartProtectCallback(result, args, needSecureInputCall)
	{
		if (m_protectedState === 3)	// 3 - stopping protect
		{
			if (result === 0)
				StopProtect();
			else
				m_protectedState = 0;	// 0 - not protected
			return;
		}
		if (result === 0)
		{
			if (!args || args.length < 1)
			{
				session.Log("ERR VK - unexpected arguments");
				return;
			}
			
			m_enabledSecureInput = args[0] === "true";
			m_protectedState = 2;	// 2 - protected
			
			var needSecureInput = m_protectableSecureInputChecker.Test(m_activeElement);
			if (needSecureInput === needSecureInputCall)
				ShowBalloons();
			else
				CheckProtectModeAndShowBalloons();
			return;
		}
		else if (result === 1)
		{
			m_postponeStart = setTimeout(function() {OnElementFocus(m_activeElement);}, 100);
		}
		m_protectedState = 0;	// 0 - not protected
	}
	function StartProtect()
	{
		var needSecureInput = m_protectableSecureInputChecker.Test(m_activeElement);
		m_protectedState = 1;	// 1 - starting protect
		m_callFunction("vk.startProtect", [needSecureInput.toString()], function(result, args) { HandleStartProtectCallback(result, args, needSecureInput);});
	}
	
	function ChangeMode()
	{
		var needSecureInput = m_protectableSecureInputChecker.Test(m_activeElement);
		m_protectedState = 1;	// 1 - starting protect
		m_callFunction("vk.changeMode", [needSecureInput.toString()], function(result, args) { HandleStartProtectCallback(result, args, needSecureInput);});
	}
	function StopProtect()
	{
  	    m_protectedState = 3;	// 3 - stopping protect
	    m_callFunction("vk.stopProtect", [], function (result)
			{
				if (m_protectedState === 1 && result === 0)	// 1 - starting protect
				{
					StartProtect();
					return;
				}
				m_protectedState = 0;		// 0 - not protected
				m_activeElement = null;
			});
	}
	
	function ShowBalloons()
	{
		if (m_enabledSecureInput)
			m_tooltip.Show(m_activeElement);
		if (m_protectableVirtualKeyboardIconChecker.Test(m_activeElement))
			m_icon.Show(m_activeElement, ShowVirtualKeyboard);
	}
	
	function CheckProtectModeAndShowBalloons()
	{
		var needSecureInput = m_protectableSecureInputChecker.Test(m_activeElement);
		if (needSecureInput != m_enabledSecureInput)
			ChangeMode();
		else
			ShowBalloons();
	}
	function OnElementFocus(element)
	{
		if (m_shutdown)
			return;
		if (m_iconHideTimer)
		{
			clearTimeout(m_iconHideTimer);
			m_iconHideTimer = null;
			m_icon.Hide();
		}
		if (!NeedProtectElement(element))
		{
			m_activeElement = null;
			return;
		}
		m_activeElement = element;
		m_lastFocusedElement = element;
		clearTimeout(m_postponeStart);
		clearTimeout(m_protectChangeTimeout);
		m_protectChangeTimeout = setTimeout(function(){ProcessFocus();}, 0);
	}
	function OnElementBlur()
	{
		if (m_shutdown)
			return;
		clearTimeout(m_postponeStart);
		m_iconHideTimer = setTimeout(function() {m_icon.Hide();}, 500);
		m_tooltip.Hide();
		clearTimeout(m_protectChangeTimeout);
		m_protectChangeTimeout = setTimeout(function(){ProcessBlur();}, 0);
	}
	function ProcessFocus()
	{
		if (m_protectedState === 0)			// 0 - not protected
			StartProtect();
		else if (m_protectedState === 2) 	// 2 - protected
			CheckProtectModeAndShowBalloons();
		else if (m_protectedState === 3)	// 3 - stopping protect
			m_protectedState = 1;			// 1 - starting protect
	}
	function ProcessBlur()
	{
		if (m_protectedState === 2)	// 2 - protected
			StopProtect();
		else if (m_protectedState === 1)	// 1 - starting protect
			m_protectedState = 3;	// 3 - stopping protect
	}
	SetSettingsImpl(settings.vkMode, settings.skMode);
	ns.AddEventListener(window, 'beforeunload', function()
		{
			clearTimeout(m_protectChangeTimeout);
			clearTimeout(m_postponeStart);
			m_shutdown = true;
	});
};
var instance = null;
if (document.readyState === "complete")
{
	instance = new VirtualKeyboard();
}
else
{
	ns.AddEventListener(window, "load", function(){
		if (!instance)
			instance = new VirtualKeyboard();
	});
}
});
KasperskyLab.AddRunner("ob", function (ns, session)
{
/**
 *  @constructor
 *  @struct
 *  @private
 */
var OnlineBanking = function()
{
    /** type{!number} */ var m_idleStartTime = ns.GetCurrentTime();
	/** type{!boolean} */var m_isInFrame = true;
		
    try {
        m_isInFrame = window != window.top;
    } catch (e) {
        session.Log('Online banking cross domain frame! ' + (e.message || e));
    }
    session.InitializePlugin(function(activatePlugin, registerMethod, callFunction, deactivatePlugin){
        if (m_isInFrame) {
            callFunction('ob.disconnect', []);
        } else {
            activatePlugin('ob', OnPing, OnError);
            registerMethod('ob.navigate', NavigateTo);
            registerMethod('ob.shutdown',
                function(){
                    deactivatePlugin('ob');
                }
            );
            callFunction('ob.connect', [],
                function(result){
                    // check eka::sOk
                    if (result != 0) {
                        deactivatePlugin('ob');
                    }
                },
                OnError);
        }
    });
	/**
	 *  @private
	 *  @param {!Object} e
	 */
    function OnError(e) {
        session.Log('ERR OB - ' + (e.message || e));
    }
    /**
     *  @private
     *  @param  {!number} currentTime
     *  @return {!number}
     */
    function OnPing(currentTime) {
        /** @const @type {!number} */var idleTime = (currentTime >= m_idleStartTime) ? currentTime - m_idleStartTime : 0;
        // first 10s pin delay == 500ms
        return idleTime <= 10000 ? 500 : ns.MaxRequestDelay;
    }
	
	/**
	 *  @private
	 *  @param {Array<string>} args
	 */
	function NavigateTo(args) {
		if (args.length < 1)
		{
			session.Log("Wrong arguments for navigate" + args.length);
			return;
		}
		document.location.href = args[0];
   }
};
try
{
    /** @const @struct */ var instance = new OnlineBanking();
}
catch(e)
{
    session.Log('Online banking exception ' + (e.message || e));
}
});
KasperskyLab.AddRunner("cb", function (ns, session, settings, locales)
{
/**
 *  @constructor
 *  @struct
 *  @private
 */
var ContentBlocker = function()
{
    /** 
     * @type {!number}
     */
	var m_idleStartTime = ns.GetCurrentTime();
	var m_signRequest = [];
	var m_callFunction;
	 
	/**
	 *  @const
	 *  @type {function(!string)}
	 */
	var m_deactivateFunction;
    session.InitializePlugin(function(activatePlugin, registerMethod, callFunction, deactivatePlugin){
        m_deactivateFunction = deactivatePlugin;
        m_callFunction = callFunction;
        activatePlugin('cb', OnPing, OnError);
        registerMethod('cb.reloadUrl', ReloadUrl);
        registerMethod('cb.signReady', SignReady);
        registerMethod('cb.shutdown',
                function(){
                    deactivatePlugin('cb');
                }
        );
        callFunction('cb.connect', [],
            function(result){
                // check eka::sOk
                if (result != 0) {
                    deactivatePlugin('cb');
                }
            },
            function(e){
                OnError(e);
                deactivatePlugin('cb');
            });
    });
	/**
	 *  @private
	 *  @param {Object} e
	 */
    function OnError(e) {
        session.Log('ERR CB - ' + (e.message || e));
    }
    /**
     * @private
	 * @param  {!number} currentTime
     * @return {!number}
     */
    function OnPing(currentTime) {
        /** @const @type{!number} */var idleTime = (currentTime >= m_idleStartTime) ? currentTime - m_idleStartTime : 0;
        // first 10s pin delay == 500ms
        return idleTime <= 10000 ? 500 : ns.MaxRequestDelay;
    }
	/**
	 *  @private
	 */
    function ReloadUrl() {
		session.Log("Start reload url " + document.readyState);
        m_idleStartTime = ns.GetCurrentTime();
        document.location.href = document.location.href;
    }
    /**
     *  @private
     *  @param {Array<!string>} args
     */
    function SignReady(args) {
        m_idleStartTime = ns.GetCurrentTime();
    
        if (args.length !== 2) {
            session.Log('ContentBlocker error: invalid parameter count ' + args.length);
            return;
        }
        if (!m_signRequest[args[0]]){
            session.Log('ContentBlocker error: invalid challenge');
            return;            
        }
        m_signRequest[args[0]](args[1]);
    }
	
	if (settings.isProductInfoRequired)
	{
		window.KasperskyLab = (function (object)
		{
			var thisProductId = locales["ProductName"] + " " + locales["ProductVersion"];
			var thisProduct = {
				'productName': locales["ProductName"],
				'productVersion': locales["ProductVersion"],
				'productPPCSID': locales["ProductPPCSID"],
				'deviceId': locales["DeviceId"],
				'features': {
					'isSafeMoneyInstalled': locales["IsSafeMoneyInstalled"],
					'isSafeMoneyProtectionEnabled': settings.isSafeMoneyProtectionEnabled
				}
			};
			object._products = object._products || {};
			object._products[thisProductId] = thisProduct;
			object.getInstalledProducts = function() {
				var products = [];
				for (var productId in object._products) {
					products.push(object._products[productId]);
				}
				return products;
			};
			object.initProductInfoProvider = function (signedProductInfoProvider) {
				delete object.initProductInfoProvider;
				object._products2 = object._products2 || {};
				object._products2[thisProductId] = signedProductInfoProvider;
				object.getInstalledProducts2 = function (challenge, timeoutMs, callback) {
					for (var productId in object._products2) {
						object._products2[productId](challenge, timeoutMs, callback);
					}
				};
			};
			return object;
		})(window.KasperskyLab || {});
		window.KasperskyLab.initProductInfoProvider(function(challenge, timeoutMs, callback){
			m_signRequest[challenge] = callback;
			m_callFunction('cb.signedProductInfo', [challenge, timeoutMs],
				function (result) {
					// check eka::sOk
					if (result != 0) {
						m_signRequest[challenge] = null;
					}
				},
				function (e) {
					OnError(e);
					m_deactivateFunction('cb');
				});                                   
		});
	}
};
try
{
    new ContentBlocker();
    if (window.dispatchEvent) {
        window.dispatchEvent(new CustomEvent("cb.ready"));
    }
}
catch(e)
{
    session.Log('ContentBlocker exception ' + (e.message || e));
}
});
(function (ns) {
/**
 *  @private
 *  @final
 *  @constructor
 *  @struct
 */
ns.UrlAdvisorBalloon = function (locales)
{
	/** 
	 *  @const
	 *  @type {Array<{name:string,bit:number}>}
	 */
	var threatTypes = [
		{name:'Unknown', bit:-1},
		{name:locales["PhishingName"], bit:62},
		{name:locales["MalwareName"], bit:63}
	];
	
	/** 
	 *  @const
	 *  @type {Array<{headerClass:string,headerNode:string,textNode:string,marker:string}>}
	 */
	var ratingIds = [
		{headerClass:'kl_header green', headerNode:locales["UrlAdvisorBalloonHeaderGood"], textNode : locales["UrlAdvisorSetLocalContentOnlineGood"], marker:'kl_marker allow'},
		{headerClass:'kl_header grey', headerNode:locales["UrlAdvisorBalloonHeaderSuspicious"], textNode:locales["UrlAdvisorSetLocalContentOnlineSuspicious"], marker:'kl_marker unknown'},
		{headerClass:'kl_header red', headerNode:locales["UrlAdvisorBalloonHeaderDanger"], textNode:locales["UrlAdvisorSetLocalContentOnlineDanger"], marker:'kl_marker disallow'}
	];
	/**
	 *  @private
	 *  @param {string} imageName
	 */
	function GetImageSrc(imageName)
	{
		return ns.GetResourceSrc('/ua/' + imageName);
	}
	/**
	 *  @private
	 */
	function AddStyles()
	{
		ns.AddStyles([
			'.kisb * { position: relative; display:block; overflow-x:hidden; width: auto; margin:0; padding:0; font-family: Verdana; line-height: 150%; text-indent:0; border:0; box-sizing:content-box; color:#000000; letter-spacing: normal; ' + locales["RtlStyle"] + '}',
			'.kisb { z-index:2147483647; width: 280px; cursor:default; display:block;}',
			'.kisb .kl_header { padding:27px 26px 10px 26px;  }',
			'.kisb .kl_header.red { background-image:url(' + GetImageSrc('header_red.png') + ');}',
			'.kisb .kl_header.grey { background-image:url(' + GetImageSrc('header_grey.png') + ');}',
			'.kisb .kl_header.green { background-image:url(' + GetImageSrc('header_green.png') + ');}',
			'.kisb .kl_content { background: url(' + GetImageSrc('content.png') + ') repeat-y top left; padding:10px 26px 10px 26px;}',
			'.kisb .kl_content .block { width: 228px!important; }',
			'.kisb .kl_marker { margin-top: 5px; margin-bottom:10px; padding-top:2px; padding-left:25px; background-position-x: 0; background-position-y: 0; background-repeat: no-repeat; word-wrap: break-word;}',
			'.kisb .kl_marker.allow { background-image: url( ' + GetImageSrc('allow.png') + ')}',
			'.kisb .kl_marker.disallow { background-image: url(' + GetImageSrc('disallow.png') + ')}',
			'.kisb .kl_marker.unknown { background-image: url(' + GetImageSrc('unknown.png') + ')}',
			'.kisb .kl_tag { display:inline-block; background: left top url(' + GetImageSrc('tag.png') + ') no-repeat; padding-left:12px; padding-right:5px; margin-right:5px; margin-bottom:5px; font-size: 7.5pt;}',
			'.kisb .kl_external { margin-top: 15px; padding-top:2px; padding-left:20px; background-position-x: left; background-position-y: top; background-repeat: no-repeat;}',
			'.kisb .kl_external { background-image: url(' + GetImageSrc('external.png') + ')}',
			'.kisb .kl_footer { background: url(' + GetImageSrc('footer.png') + ') no-repeat bottom left; padding:6px 26px 22px 26px;}',
			'.kisb .kl_content * { font-size:8pt; }',
			'.kisb a { text-decoration: underline; display:inline-block; }',
			'.kisb a:hover { text-decoration: none; }',
			'.kisb a, .kisb a:link, .kisb a:hover, .kisb a:visited { color: #008ccc;}',
			'.kisb .kl_header { font-size:12pt; color: #ffffff;  }',
			'.kisb .kl_footer a { font-size:8pt; color: #4d4d4d; text-decoration:underline; }'
		]);
	}
	
	/**
	 *  @private
	 *  @param {{high:number,low:number}} bigInt
	 */
	function FixBigInt(bigInt)
	{
		if (bigInt.low >= 10000000000)
		{
			var addedHigh = Math.floor(bigInt.low / 10000000000);
			bigInt.low = bigInt.low % 10000000000;
			bigInt.high += addedHigh;
		}
	}
	
	/**
	 *  @private
	 *  @param {{high:number,low:number}} bigInt
	 *  @return {string}
	 */
	function BigIntToString(bigInt)
	{
		/** type {string} */var result = '';
		if (bigInt.high)
		{
			result += bigInt.high;
			for (var /** type {number} */i = bigInt.low.toString().length; i < 10; ++i)
				result += '0';
		}
		result += bigInt.low;
		return result;
	}
	
	/**
	 *  @private
	 *  @param {number} bit
	 *  @return {{high:number,low:number}}
	 */
	function GetMask(bit)
	{
		if (bit < 0 || bit >= 64)
			return {high: 0, low: 0};
		/**
		 *  @type {{high:number,low:number}}
		 */
		var mask = {
			high: 0,
			low: Math.pow(2, Math.min(50, bit))
		};
		FixBigInt(mask);
		
		if (bit > 50)
		{
			mask.low *= Math.pow(2, bit - 50);
			mask.high *= Math.pow(2, bit - 50);
			FixBigInt(mask);
		}
		return mask;
	}
	
	/**
	 *  @private
	 *  @param {Array<number>} threats
	 *  @param {Array<number>} categories
	 *  @return {string}
	 */
	function GetCategoriesMask(threats, categories)
	{
		/** @type {{high:number,low:number}}*/var mask = {high: 0, low: 0};
		/** @type Array<number> */var bits = [];
		var i, count;
		if (threats)
			for (i = 0, count = threats.length; i < count; ++i)
				bits.push(threatTypes[threats[i]].bit);
		if (categories)
			for (i = 0, count = categories.length; i < count; ++i)
				bits.push(categories[i] - 1);
		for (i = 0, count = bits.length; i < count; ++i)
		{
			/** 
			 *  @const
			 *  @type {{high:number,low:number}}
			 */
			var addingMask = GetMask(bits[i]);
			mask.high += addingMask.high;
			mask.low += addingMask.low;
			FixBigInt(mask);
		}
		return BigIntToString(mask);
	}
	
	/**
	 *  @private
	 *  @param {number} threat
	 *  @return {string}
	 */
	function ConvertThreat(threat)
	{
		return threatTypes[threat].name;
	}
	/**
	 *  @private
	 *  @param {number} category
	 *  @return {string}
	 */
	function ConvertCategory(category)
	{
		return locales["CAT_" + category];
	}
    /**
     *  @private
     *  @param {Node} parentElement
     *  @param {Array<number>} list
     *  @param {function(number):string} converter
     */
	function AddTagsFromList(parentElement, list, converter)
	{
		if (!list)
			return;
		for (var i = 0, count = list.length; i < count; ++i)
		{
			/**
			 *  @const
			 *  @type {Element}
			 */
			var spanElement = document.createElement('span');
			spanElement.className = 'kl_tag';
			spanElement.appendChild(document.createTextNode(converter(list[i])));
			parentElement.appendChild(spanElement);
		}
	}
	
	/**
	 *  @private
	 *  @param {Element} parentElement
	 *  @param {{categories:Array<number>,threats:Array<number>}} verdict
	 */
	function AddVerdictTags(parentElement, verdict)
	{
		if ((!verdict.categories || verdict.categories.length == 0) && (!verdict.threats || verdict.threats.length == 0))
			return;
		parentElement.appendChild(document.createElement('br'));
		parentElement.appendChild(document.createElement('br'));
		parentElement.appendChild(document.createTextNode(locales["UrlAdvisorDescribeCategories"] + ": "));
		
		/**
		 *  @const
		 *  @type {Node}
		 */
		var divElement = parentElement.appendChild(document.createElement('div'));
		AddTagsFromList(divElement, verdict.categories, ConvertCategory);
		AddTagsFromList(divElement, verdict.threats, ConvertThreat);
	}
	
	/**
	 *  @private
	 *  @param {Element} element
	 */
	function RemoveAllChilds(element)
	{
		while (element.childNodes.length > 0)
			element.removeChild(element.childNodes[0]);
	}
	
	/**
	 *  @private
	 *  @param {{url:string, rating:number, threats:Array<number>, categories:Array<number>}} verdict
	 *  @param {string} additionalInfoLink
	 *  @param {string} termsLink
	 */
	function UpdateBalloon(verdict, additionalInfoLink, termsLink)
	{
		/** @type {Element}*/var headerElement = document.getElementById('balloon_header');
		/** @type {Element}*/var contentElement = document.getElementById('balloon_content');
		/** @type {Element}*/var markerElement = document.getElementById('balloon_marker');
		/** @type {Element}*/var tagElement = document.getElementById('balloon_tags');
		/** @type {Element}*/var externalElement = document.getElementById('balloon_external');
		/** @type {Element}*/var termsElement = document.getElementById('balloon_terms');
		RemoveAllChilds(headerElement);
		RemoveAllChilds(contentElement);
		RemoveAllChilds(markerElement);
		RemoveAllChilds(tagElement);
		headerElement.className = ratingIds[verdict.rating-1].headerClass;
		headerElement.appendChild(document.createTextNode(ratingIds[verdict.rating-1].headerNode));
		contentElement
			.appendChild(document.createElement('b'))
			.appendChild(document.createTextNode(ratingIds[verdict.rating-1].textNode));
		markerElement.className = ratingIds[verdict.rating-1].marker;
		markerElement.appendChild(document.createTextNode(verdict.url));
		AddVerdictTags(tagElement, verdict);
		externalElement.href = additionalInfoLink + '&act-exturl=' + encodeURIComponent(verdict.url) + '&act-threat-cat=' + (verdict.rating - 1) + '&act-content-cat=' + GetCategoriesMask(verdict.threats, verdict.categories) + '&rpe=1';
		termsElement.href = termsLink;
	}
	
	/**
	 *  @private
	 */
	function CheckMouseOverBalloon(mouseArgs)
	{
		if (m_balloon)
		{
			/**@const @struct*/var args = mouseArgs || window.event;
			/**@const @struct*/var rect = m_balloon.getBoundingClientRect();
			if (args.clientX > rect.left &&
				args.clientX < rect.right &&
				args.clientY > rect.top &&
				args.clientY < rect.bottom)
			{
				return;
			}
			HideBalloon();
		}
	}
	/**
	 *  @private
	 *  @param {Element} balloonDiv
	 */
	function CreateHeaderDiv(balloonDiv)
	{
		/** @const @struct*/var headerDiv = document.createElement('div');
		headerDiv.id = 'balloon_header';
		
		balloonDiv.appendChild(headerDiv);
	}
	
	/**
	 *  @private
	 *  @param {Element} balloonDiv
	 */
	function CreateContentDiv(balloonDiv)
	{
		/** @const @type {Element}*/var contentDiv = document.createElement('div');
		contentDiv.className = 'kl_content';
		/** @const @type {Element}*/var blockDiv = document.createElement('div');
		blockDiv.className = 'block';
		contentDiv.appendChild(blockDiv);
		/** @const @type {Element}*/var contentTypeDiv = document.createElement('div');
		contentTypeDiv.className = 'block';
		contentTypeDiv.id = 'balloon_content';
		blockDiv.appendChild(contentTypeDiv);
		/** @const @type {Element}*/var markerDiv = document.createElement('div');
		markerDiv.className = 'kl_marker allow';
		markerDiv.id = 'balloon_marker';
		blockDiv.appendChild(markerDiv);
		/** @const @type {Element}*/var tagsDiv = document.createElement('div');
		tagsDiv.id = 'balloon_tags';
		blockDiv.appendChild(tagsDiv);
		/** @const @type {Element}*/var infoLink = document.createElement('a');
		infoLink.href = '#';
		infoLink.id = 'balloon_external';
		infoLink.className = 'kl_external';
		infoLink.target = '_blank';
		infoLink.appendChild(document.createTextNode(locales["UrlAdvisorSetLocalContentGotoForInfo"]));
		blockDiv.appendChild(infoLink);
		
		balloonDiv.appendChild(contentDiv);
	}
	
	/**
	 *  @private
	 *  @param {Element} balloonDiv
	 */
	function CreateFooterDiv(balloonDiv)
	{
		/** @const @type {Element}*/var footerDiv = document.createElement('div');
		footerDiv.className = 'kl_footer';
		footerDiv.id = 'balloon_footer';
		/** @const @type {Element}*/var termsLink = document.createElement('a');
		termsLink.id = 'balloon_terms';
		termsLink.target = '_blank';
		termsLink.appendChild(document.createTextNode(locales["UrlAdvisorSetLocalContentTermsOfUsage"]));
		// TODO: add terms onclick like in previous version
		footerDiv.appendChild(termsLink);
		
		balloonDiv.appendChild(footerDiv);
	}
	
	/**
	 *  @private
	 *  @return {Element}
	 */
	function CreateBalloon()
	{
		AddStyles();
	
		/** @const @type {Element}*/var balloonDiv = document.createElement('div');
		balloonDiv.className = 'kisb';
		balloonDiv.id = 'balloon_parent_div';
		balloonDiv.onmouseout = CheckMouseOverBalloon;
		
		CreateHeaderDiv(balloonDiv);
		CreateContentDiv(balloonDiv);
		CreateFooterDiv(balloonDiv);
		
		balloonDiv.style.visibility = 'hidden';
		document.body.appendChild(balloonDiv);
		return balloonDiv;
	}
	/**@struct*/var m_balloon = null;
	/**
	 *  @private
	 *  @param {Element} element
	 *  @return {{width: number, height: number}}
	 */
	function GetElementSize(element)
	{
		/**@const @struct*/var rect = element.getBoundingClientRect();
		/**@const @type{number}*/var width = rect.width ? rect.width : rect.right - rect.left;
		/**@const @type{number}*/var height = rect.height ? rect.height : rect.bottom - rect.top;
			
		return {width: width, height: height};
	}
	
	/**
	 *  @param {number} clientX
	 *  @param {number} clientY
	 *  @param {{url:string, rating:number, threats:Array<number>, categories:Array<number>}} verdict
	 *  @param {string} additionalInfoLink
	 *  @param {string} termsLink
	 */
	this.ShowBalloon = function(clientX, clientY, verdict, additionalInfoLink, termsLink)
	{
		m_balloon = document.getElementById('balloon_parent_div');
		if (!m_balloon)
			m_balloon = CreateBalloon();
		UpdateBalloon(verdict, additionalInfoLink, termsLink);
		
		/** @const @type{number}*/var clientWidth = ns.GetPageWidth();
		/** @const @type{{width: number, height: number}}*/var balloonSize = GetElementSize(m_balloon);
		/** @const @type{number}*/var halfWidth = balloonSize.width / 2;
		/** @type{number}*/var x;
		if (halfWidth > clientX)
			x = 0;
		else if (halfWidth + clientX > clientWidth)
			x = clientWidth - balloonSize.width;
		else
			x = clientX - halfWidth;
		
		/** @const @type{number}*/var clientHeight = ns.GetPageHeight();
		/** @type{number}*/var y = (clientY + balloonSize.height > clientHeight) ? clientY - balloonSize.height : clientY;
		if (y < 0)
			y = 0;
		/** @const @type{{top:number,left:number}}*/var scroll = ns.GetPageScroll();
		y += scroll.top;
		x += scroll.left;
		m_balloon.style.position = 'absolute';
		m_balloon.style.top = Math.round(y).toString() + 'px';
		m_balloon.style.left = Math.round(x).toString() + 'px';
		m_balloon.style.visibility = 'visible';
	}
	
	/**
	 *  @private
	 */
	function HideBalloon()
	{
		if (m_balloon)
			m_balloon.style.visibility = 'hidden';
	}
};
}) (KasperskyLab || {});
KasperskyLab.AddRunner("ua", function (ns, session, settings, locales) {
/**
 *  @private
 *  @final
 *  @constructor
 *  @struct
 */
var UrlAdvisor = function()
{
	/** @struct @const */var m_urlAdvisorBalloon = new ns.UrlAdvisorBalloon(locales);
	/** @type {boolean}*/var m_settingsEnabled = settings.enable;
	/** @type {boolean}*/var m_enabled = m_settingsEnabled && (settings.state === 'Enabled');
	/** @type {boolean}*/var m_checkOnlySearchResults = settings.mode;
	/** @type {string}*/var m_additionalInfoUrl = settings.infoUrl;
	/** @type {string}*/var m_termsUrl = settings.termsUrl;
	/** @type {boolean}*/var m_postponeCategorizeStarted = false;
	/** @type {number}*/var m_urlCategorizeRequestTime = 0;
	/** @const @type {string}*/var m_checkedAtributeName = 'kl_' + ns.GetCurrentTime();
	/** @const @type {string}*/var m_iconName = 'kl_' + ns.GetCurrentTime();
	var m_observer;
    
	
	var m_state = settings.state,
		m_callFunction;
	session.InitializePlugin(function(activatePlugin, registerMethod, callFunction){
		m_callFunction = callFunction;
		activatePlugin('ua', OnPing, OnError);
		registerMethod('ua.verdict', SetVerdict);
		registerMethod('ua.settings', SetSettings);
		registerMethod('ua.state', SetState);
	});
	Run();
	
	/**
	 * @private
	 * @param {number} currentTime
	 * @return {number}
	*/
	function OnPing(currentTime)
	{
		/**
		 *  @const
		 *  @type {number}
		 */
		var timeFormRequest = (currentTime >= m_urlCategorizeRequestTime) ? currentTime - m_urlCategorizeRequestTime : 0;
		return timeFormRequest <= 10000 ? 500 : ns.MaxRequestDelay;
	}
	/**
	 *  @private
	 *  @param {Object} e
	 */
	function OnError(e) {
		session.Log('ERR UA - ' + (e.message || e));
	}
	/**
	 *  @private
	 *  @return {string}
	 */
	function GetHref(link)
	{
		try	{ return link.href;	} catch(e){}
		try	{ return link.getAttribute('href');	} catch(e){}
		return '';
	}
	
	/**
	 *  @private
	 *  @param {Node} linkElement
	 *  @return {!boolean}
	 */
	function IsLinkHighlighted(linkElement)
	{
		/** @const @type {Node} */var nextElement = linkElement.nextSibling;
		return nextElement !== null && nextElement.name == m_iconName;
	}
	
	/**
	 *  @private
	 *  @param {Element} linkElement
	 *  @return {Node}
	 */
	function GetLinkIcon(linkElement)
	{
		if (!IsLinkHighlighted(linkElement))
		{
			/** @type {Element} */var icon = document.createElement('img');
			icon.name = m_iconName;
			icon.width = 12;
			icon.height = 12;
			linkElement.parentNode.insertBefore(icon, linkElement.nextSibling);
		}
		return linkElement.nextSibling;
	}
	
	/**
	 *  @private
	 *  @param {Node} icon
	 *  @param {{rating:number, url:string}} verdict
	 */
	function UpdateIconImage(icon, verdict)
	{
		if (verdict.rating === 1)
		{
			icon.src = ns.GetResourceSrc('/ua/UrlAdvisorGoodImage.png');
			icon['kis_status'] = 16;
		}
		else if (verdict.rating === 2)
		{
			icon.src = ns.GetResourceSrc('/ua/UrlAdvisorSuspiciousImage.png');
			icon['kis_status'] = 8;
		}
		else if (verdict.rating === 3)
		{
			icon.src = ns.GetResourceSrc('/ua/UrlAdvisorDangerImage.png');
			icon['kis_status'] = 4;
		}
	}
	
	/**
	 *  @private
	 *  @param {Node} icon
	 *  @param {{rating:number, url:string}} verdict
	 */
	function SubscribeIconOnMouseEvents(icon, verdict)
	{
		/** type {number} */var balloonTimerId = 0;
		icon.onmouseout = function()
		{
			if (balloonTimerId)
			{
				window.clearTimeout(balloonTimerId);
				balloonTimerId = 0;
			}
		};
		icon.onmouseover = function(mouseArgs)
		{
			if (!balloonTimerId)
			{
				/** @const @struct */var args = mouseArgs || window.event;
				balloonTimerId = window.setTimeout(function(/**number*/clientX, /**number*/clientY)
					{
						return function()
						{
							m_urlAdvisorBalloon.ShowBalloon(clientX, clientY, verdict, m_additionalInfoUrl, m_termsUrl);
						}
					}(args.clientX, args.clientY),
					300);
			}
		};
	}
	
	/**
	 *  @private
	 */
	function SetVerdict(argumentsList)
	{
		for (/**type {number}*/var currentVerdict = 0; currentVerdict < argumentsList.length; currentVerdict++)
		{
			/**
			 *  @const
			 *  @type {{rating:number, url:string}}
			 */
			var verdict = ns.JSONParse(argumentsList[currentVerdict]);
			for (/**type {number}*/var currentLinkIndex = 0; currentLinkIndex < document.links.length; currentLinkIndex++)
			{
				/**
				 *  @const
				 *  @type {!Element}
				 */
				var linkElement = document.links[currentLinkIndex];
				if (verdict.url === GetHref(linkElement) && (!m_checkOnlySearchResults || ns.IsLinkSearchResult(linkElement)))
				{
					/**
					 *  @const
					 *  @type {Node}
					 */
					var icon = GetLinkIcon(linkElement);
					if (!!icon)
					{
						UpdateIconImage(icon, verdict);
						SubscribeIconOnMouseEvents(icon, verdict);
					}
				}
			}
		}
	}
	/**
	 *  @private
	 */
	function SetSettingsImpl(argumentList)
	{
		if (argumentList.length > 0)
		{
			m_settingsEnabled = (argumentList[0] != '0');
			m_enabled = m_settingsEnabled && (m_state == 'Enabled');
	 	}
		if (!m_enabled)
			return;
		m_checkOnlySearchResults = !!(argumentList.length > 1 && argumentList[1] == 1);
		
		if (argumentList.length > 2)
			m_termsUrl = argumentList[2];
			
		if (argumentList.length > 3)
			m_additionalInfoUrl = argumentList[3];
	}
	/**
	 *  @private
	 */
	function ClearImages()
	{
		/**
		 *  @const
		 *  @type {NodeList}
		 */
		var images = document.getElementsByName(m_iconName);
		while (images.length > 0)
			images[0].parentNode.removeChild(images[0]);
	}	
	
	/**
	 *  @private
	 */
	function SetSettings(argumentList)
	{
		ClearImages();
		SetSettingsImpl(argumentList);
		CategorizeUrl();
	}
	/**
	 *  @private
	 *  @param {Array<string>} argumentList
	 */
	function SetState(argumentList)
	{
		if (argumentList.length > 0)
		{
			ClearImages();	
			m_state = argumentList[0];
			m_enabled = m_settingsEnabled && (m_state == 'Enabled');
			CategorizeUrl();
		}
	}
	/**
	 * @private
	 * @param {Element} linkElement
	 * @return {!boolean}
	 */
	function IsNeedCategorizeLink(linkElement)
	{
		try
		{
			return !linkElement.isContentEditable && !!linkElement.parentNode && !IsLinkHighlighted(linkElement) &&
				(!m_checkOnlySearchResults || ns.IsLinkSearchResult(linkElement)) &&
				linkElement.id !== 'balloon_external' &&
				linkElement.id !== 'balloon_terms';
		}
		catch(e)
		{
			session.Log('check link exception: ' + (e.message || e));
			return false;
		}
	}
	/**
	 *  @private
	 */
	function ProcessDomChange()
	{
		if (!m_postponeCategorizeStarted)
		{
			setTimeout(CategorizeUrl, 500);
			m_postponeCategorizeStarted = true;
		}
		
		var images = document.getElementsByName(m_iconName);
		for (var i = 0; i < images.length; ++i)
		{
			var linkNode = images[i].previousSibling;
			if (!linkNode || !linkNode.nodeName || linkNode.nodeName.toLowerCase() !== "a")
			{
				var imageNode = images[i];
				imageNode.parentNode.removeChild(imageNode);
			}
		}
	}
	
	/**
	 *  @private
	 */
	function CategorizeUrl()
	{
		if (!m_enabled)
		{
			session.Log('skip categorize links because UA disabled');
			return;
		}
			
		m_postponeCategorizeStarted = false;
		var linksForCategorize = [];
		
		try
		{
			for (var i = 0; i < document.links.length; i++)
			{
				/** @const @type {Element} */var link = document.links[i];
				if (IsNeedCategorizeLink(link))
				{
					link[m_checkedAtributeName] = true; 
					/** @const @type {string} */var href = GetHref(link);
					if (!!href) {
						linksForCategorize.push(href); 
					} else {
						ns.Log('access to href blocked by browser'); 
					}
				}
			}
		}
		catch(e)
		{
			session.Log('find links exception: ' + (e.message || e));
		}
		
		if (linksForCategorize.length)
		{
			m_callFunction("ua.categorize", linksForCategorize, null, OnError);
			m_urlCategorizeRequestTime = ns.GetCurrentTime();
		}
		else
		{
			session.Log('UA not found links for categorization');
		}
	}
	/**
	 *  @private
	 */
	function Run()
	{
		CategorizeUrl();
		m_observer = ns.GetDomChangeObserver("a");
		m_observer.Start(ProcessDomChange);
		ns.AddEventListener(window, "unload", 
			function()
			{
				if (m_observer)
					m_observer.Stop();
			});
	};
};
/** type {UrlAdvisor} */var instance = null;
function RunUrlAdvisor() {
	try
	{
		if (!instance) {
			instance = new UrlAdvisor();
		}
	}
	catch(e)
	{
		session.Log('UrlAdvisor exception ' + (e.message || e));
	}
}
if (document.readyState === "loading")
{
	if (document.addEventListener)
	{
		document.addEventListener('DOMContentLoaded', RunUrlAdvisor, true);
	}
	else if (document.all)
	{
		document.write("<script type=\"text/javascript\" id=\"DeferLoadTagName\" defer=\"defer\" src=\"javascript:void(0)\"><\/script>");
		/** @const @struct */var contentloadtag = document.getElementById("DeferLoadTagName");
		contentloadtag.onreadystatechange = function(){
			if (this.readyState=="complete")
					RunUrlAdvisor();
			};
	}
}
else
{
	RunUrlAdvisor();
}
});

 })();

 })();
