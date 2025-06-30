
var select, stylesheet;

function changeCSS() {
    "use strict";
    select = document.getElementById("style");
    stylesheet = document.getElementById("stylesheet");

    if (select.value === "standard") {
        stylesheet.href = "assets/css/green.css";
        localStorage.setItem('save', select.value);
    } else if (select.value === "trial") {
        stylesheet.href = "assets/css/anushka.css";
        localStorage.setItem('save', select.value);
    }
    else if (select.value === "alternative") {
        stylesheet.href = "assets/css/blue.css";
        localStorage.setItem('save', select.value);
    }
    
}

function trial()
{
    var nav = $('#sideNav');
    var navLi = nav.find('li');
    var navLink = nav.find('a');
    var navSub = nav.find('li>ul.sub');
    navSub.closest('li').addClass('hasSub');
    if(!navSub.prev('a').hasClass('notExpand')) {
        navSub.prev('a').addClass('notExpand');
    }
    if(!$('#sideNav').hasClass('show-arrows')) {
        $('#sideNav').addClass('show-arrows');
    }
    if(!navSub.prev('a').find('i.sideNav-arrow').length) {
        navSub.prev('a').prepend('<i class="im-arrow-down2 sideNav-arrow"></i>');
    }

    navLink.on("click", function(e){
        var _this = $(this);
        if(_this.hasClass('notExpand')) {
            e.preventDefault();
            navexpand = nav.find('li.hasSub .expand');
            navexpand.next('ul').removeClass('show');
            navexpand.next('ul').slideUp("fast"); 
            navexpand.addClass('notExpand').removeClass('expand');
            navexpand.find('.sideNav-arrow').removeClass('rotateM180').addClass('rotate0');
            _this.next('ul').slideDown("fast");
            _this.next('ul').addClass('show');
            _this.addClass('expand').removeClass('notExpand');
            navLi.removeClass('highlight-menu');
            _this.closest('li.hasSub').addClass('highlight-menu');
            _this.find('.sideNav-arrow').removeClass('rotate0').addClass('rotateM180');                  
        } else if (_this.hasClass('expand')) {
            e.preventDefault();            
            _this.next('ul').removeClass('show');
            _this.next('ul').slideUp("fast");
            _this.addClass('notExpand').removeClass('expand');
            _this.find('.sideNav-arrow').removeClass('rotateM180').addClass('rotate0');
            navLi.removeClass('highlight-menu');
        }
    });
}



function autoCSS() {
    "use strict";
    select = document.getElementById("style");
    stylesheet = document.getElementById("stylesheet");

    if (localStorage.getItem('save')) {
        select.options[localStorage.getItem('save')].selected = true;
        changeCSS();
    }
}

$(function() {
   
    $("#sub").sortable({

    });
    $("#sub").disableSelection();

    var tabs = $("#wrapper").tabs();
    tabs.find(".tab_sort").sortable({
        axis: "x",
    });
});

$(function() {

    $(".note-container").draggable({
        containment : "parent"
    });

   

    $("#sub").sortable({

    });
    $("#sub").disableSelection();

    var tabs = $(".tabs").tabs();
    tabs.find(".ui-tabs-nav").sortable({
        axis: "x",
        stop: function() {
            tabs.tabs("refresh");
        }
    });

});


$(function() {
    <!-- Settings Modal Popup -->
        $("#settings_Modal").hide();
    $(".settings_open").click(function() { $("#settings_Modal").show(); });
    $("#ok").on('click', function(c){
        $("#settings_Modal").fadeOut('slow', function(c){
        });
    });

    //<!-- Help Modal Popup -->
    //	$(".im-question").click(function() { $("#help_Modal").show() });; 
    $("#help_close").on('click', function(c){
        $("#help_Modal").fadeOut('slow', function(c){
        });
    });  
    
    <!-- CRM Modal Popup -->
        $("#crm").click(function() { $("#crm_Modal").show() });; 
    $("#crm_close").on('click', function(c){
        $("#crm_Modal").fadeOut('slow', function(c){
        });
    });  

    <!-- Contacts Popup -->
        $("#contacts").click(function() { 
            $("#contacts_Modal").show(); 
            $("#contacts_body").append("<iframe src='./ContactCard.aspx' id='contacts_content' style='width:100%;height:374px;background-color:#fff;'></iframe>");
            $('#toggle_div1').hide();
        });
    $("#contacts_close").on('click', function(c){
        $("#contacts_Modal").hide();
        $("#toggle_div1").show();
        $("#contacts_content").remove();
    });

    <!-- About Modal Popup -->
        $("#about").click(function() { $("#about_Modal").show() });
    $("#about_close").on('click', function(c){
        $("#about_Modal").fadeOut('slow', function(c){
        });
    }); 
	
    <!-- clear localstorage -->
        $("#clear_local").on('click', function(c){

            localStorage.clear();
            $('#fav_c').empty();
        }); 
	
    <!-- Compare Popup -->
        $("#compare").click(function() { $("#compare_Modal").toggle() });; 
	
	
    <!-- sticky notes -->

        $("#note_close").on('click', function(c){
            $("#note_Modal").fadeOut('slow', function(c){
            });
            $("#note_content").remove();
        }); 
});