
/* Uncomment for chat feature */
/* var myHub = $.connection.chatHub;

$(function () {
    LCSKChat.config();
    LCSKChat.init();
});

var LCSKChat = function () {
    var chatKey = 'lcsk-chatId';
    var requestChat = false;
    var chatId = '';
    var chatEditing = false;

    var options = [];

    function setDefaults() {
        options.position = 'fixed';
        options.placement = 'bottom-right';

        options.headerPaddings = '3px 10px 3px 10px';
        options.headerBackgroundColor = '#E4E4E4';
        options.headerTextColor = 'white';
      options.headerBorderColor = '#2C3E50';
        options.headerGradientStart = 'rgb(44, 62, 80)';
        options.headerGradientEnd = '#657480';
        options.headerFontSize = '15px';

        options.boxBorderColor = '#A3A3A3';
        options.boxBackgroundColor = '#fff';

        options.width = 250;
     
        options.offlineTitle = 'Contact us!';
        options.onlineTitle = 'Chat with us!';

        options.waitingForOperator = 'Thanks, give us 1 minute to accept the chat...';
        options.emailSent = 'Your email was sent, thanks we\'ll get back to you asap.';
        options.emailFailed = 'Doh! The email could not be sent at the moment.<br /><br />Sorry about that.';

    }

    function config(args) {
        setDefaults();

        if (args != null) {
            for (key in options) {
                if (args[key]) {
                    options[key] = args[key];
                }
            }
        }
    }

    function getPlacement() {
        if (options.placement == 'bottom-right') {
            return 'bottom:0px;right:0px;';
        }
        return '';
    }

    function init() {
        $('body').append(
            //'<div id="chat-box-header" style="display: block;position:' + options.position + ';' + getPlacement() + 'width:' + options.width + 'px;padding:' + options.headerPaddings + ';color:' + options.headerTextColor + ';font-size:' + options.headerFontSize + ';cursor:pointer;background:' + options.headerBackgroundColor + ';filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=\'' + options.headerGradientStart + '\', endColorstr=\'' + options.headerGradientEnd + '\');background: -webkit-gradient(linear, left top, left bottom, from(' + options.headerGradientStart + '), to(' + options.headerGradientEnd + '));background: -moz-linear-gradient(top,  ' + options.headerGradientStart + ',  ' + options.headerGradientEnd + ');border:1px solid ' + options.headerBorderColor + ';box-shadow:inset 0 0 7px #0354cb;-webkit-box-shadow:inset 0 0 7px #0354cb;border-radius: 5px;">' + options.offlineTitle + '</div>' +
            //'<div id="chat-box" style="display:none;position:' + options.position + ';' + getPlacement() + 'width:' + options.width + 'px;height:300px;padding: 10px 10px 10px 10px;border: 1px solid ' + options.boxBorderColor + ';background-color:' + options.boxBackgroundColor + ';font-size:small;"></div>'
            '<div id="chat-box-header" style="display: block;position:' + options.position + ';' + getPlacement() + 'width:' + options.width + 'px;padding:' + options.headerPaddings + ';color:' + options.headerTextColor + ';font-size:' + options.headerFontSize + ';cursor:pointer;background:' + options.headerBackgroundColor + ';filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=\'' + options.headerGradientStart + '\', endColorstr=\'' + options.headerGradientEnd + '\');background: -webkit-gradient(linear, left top, left bottom, from(' + options.headerGradientStart + '), to(' + options.headerGradientEnd + '));background: -moz-linear-gradient(top,  ' + options.headerGradientStart + ',  ' + options.headerGradientEnd + ');border:1px solid ' + options.headerBorderColor + ';-webkit-box-border-top-left-radius: 5px;border-top-right-radius: 5px;">' + options.offlineTitle + '</div>' +
            '<div id="chat-box" style="display:none;position:' + options.position + ';' + getPlacement() + 'width:' + options.width + 'px;height:300px;padding: 10px 10px 10px 10px;border: 1px solid ' + options.boxBorderColor + ';background-color:' + options.boxBackgroundColor + ';opacity: 0.8;font-size:14px !important;color: black !important;"></div>'
        );

        $.connection.hub.start()
            .done(function () {
                var userId = document.getElementById("userId").value;
                var existingChatId = getExistingChatId(chatKey);
                $.get("http://ipinfo.io", function (response) {
                    myHub.server.logVisit(document.location.href, document.referrer,  response.city, response.region, response.country, existingChatId, userId);
                }, "jsonp");
            })
            .fail(function () { chatRefreshState(false); });

        $('body').on({
            click: function () {
                toggleChatBox();
            }
        }, '#chat-box-header');

        $('#chat-box').on({
            keydown: function (e) {
                var msg = $(this).val();
                var chat_user = document.getElementById("userId").value;
                var user = chat_user;
                if (e.keyCode == 13 && msg != '') {
                    e.preventDefault();
                    e.stopPropagation();

                    if (chatId == null || chatId == '') {
                        myHub.server.requestChat(msg,user);
                        $('#chat-box-msg').html(options.waitingForOperator);
                    } else {
                        myHub.server.send(msg,user);
                    }

                    $('#chat-box-textinput').val('');
                }
            }
        }, '#chat-box-textinput');

        $('#chat-box').on({
            keydown: function () {
                chatEditing = true;
            }
        }, '.chat-editing');

        $('#chat-box').on({
            click: function () {
                myHub.server.sendEmail($('#chat-box-email').val(), $('#chat-box-cmt').val());

                $('#chat-box').html(options.emailSent);
                chatEditing = false;
            }
        }, '#chat-box-send');
    }

    function chatRefreshState(state) {
        if (state) {
            $('#chat-box-header').text(options.onlineTitle);
            $('#chat-box').html(
                  '<div id="chat-box-msg" style="height:250px;overflow:auto;">' +
                  '<p>Have a question? Let\'s chat!</p><p>Add your question on the field below and press ENTER.</p></div>' +
                  '<div id="chat-box-input" style="height:50px;"><textarea id="chat-box-textinput" style="width:100%;height: 32px;border:1px solid #5D6167;border-radius: 3px;" /></div>'
              );
            if (!requestChat) {
                $('#chat-box').html(
                    '<div id="chat-box-msg" style="height:250px;overflow:auto;">' +
                    '<p>Have a question? Let\'s chat!</p><p>Add your question on the field below and press ENTER.</p></div>' +
                    '<div id="chat-box-input" style="height:50px;"><textarea id="chat-box-textinput" style="width:100%;height: 32px;border:1px solid #5D6167;border-radius: 3px;" /></div>'
                );
            }
        } else {
            if (!chatEditing) {
                $('#chat-box-header').text(options.offlineTitle);
               // $('#chat-box-input').hide();
                $('#chat-box').html(
                    '<p>Your email</p><input type="text" id="chat-box-email" style="border:1px solid #0354cb;border-radius: 3px;width: 100%;" class="chat-editing" />' +
                    '<p>Your message</p><textarea id="chat-box-cmt" cols="40" rows="7" class="chat-editing" style="border:1px solid #5D6167;border-radius: 3px;height:130px;"></textarea>' +
                    '<p><input type="button" id="chat-box-send" value="Contact us" />'
                );
            }
        }
    }

    function toggleChatBox() {
        var elm = $('#chat-box-header');
        if ($('#chat-box').hasClass('chat-open')) {
            $('#chat-box').removeClass('chat-open');
            elm.css('bottom', '0px');
        } else {
            var y = 271 + elm.height();
            $('#chat-box').addClass('chat-open');
            elm.css('bottom', y);
        }
        $('#chat-box').slideToggle();
    }

    function hasStorage() {
        return typeof(Storage) !== 'undefined';
    }

    function setChatId(chatId) {
        if (hasStorage()) {
            sessionStorage.setItem(chatKey, chatId);
        }
    }

    function getExistingChatId() {
        if (hasStorage()) {
            return sessionStorage.getItem(chatKey);
        }
    }

    myHub.client.setChat = function (id, agentName, existing) {
        chatId = id;
        requestChat = true;

        setChatId(chatId);

        if (existing) {
            if (!$('#chat-box').hasClass('chat-open')) {
                toggleChatBox();
            }

            $('#chat-box-msg').append('<p>Continuing your chat with <strong>' + agentName + '</strong></p>');
        } else {
            $('#chat-box-msg').append('<p>You are now chatting with <strong>' + agentName + '</strong></p>');
        }
    };

    myHub.client.addMessage = function (from, msg) {
        if (chatId != null && chatId != '') {
            if (!requestChat) {
                if (!$('#chat-box').hasClass('chat-open')) {
                    toggleChatBox();
                }
                requestChat = true;
            }

            $('#chat-box-msg').append('<p><strong>' + from + '</strong>: ' + msg + '</p>');
            if (from == '') {
                chatId = '';
                requestChat = false;
            }

            $("#chat-box-msg").scrollTop($("#chat-box-msg")[0].scrollHeight);
        }
    }

    myHub.client.emailResult = function (state) {
        if (!state) {
            $('#chat-box').html(options.emailFailed);
        }
    };

    myHub.client.onlineStatus = function (state) {
        chatRefreshState(state);
    };

    return {
        config: config,
        init: init
    }
} ();
*/