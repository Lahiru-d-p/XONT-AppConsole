
var select, stylesheet;

function changeCSS() {
    "use strict";
    select = document.getElementById("style");
    stylesheet = document.getElementById("stylesheet");

    if (select.value === "green") {
        PageMethods.saveTheme("green");
    } else if (select.value === "blue") {
        PageMethods.saveTheme("blue");
    }
    else if (select.value === "gray") {
       PageMethods.saveTheme("gray");
    }
    else if (select.value === "gray") {
       PageMethods.saveTheme("gray");
    }
    else if (select.value === "purple") {
        PageMethods.saveTheme("purple");
    }
    
}

function style() {
    alert('hit');
    //if (color == 'blue') {

    //    document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:dodgerblue;font-size:30px;"><span> Blue </span> </div>';

    //    $('#divAddColor').css('background', 'rgba(30, 144, 255, 0.44) none repeat scroll 0% 0%');
    //    $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(6, 64, 93)');
    //}
    //else if (color == 'green') {

    //    document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:lightseagreen;font-size:30px;"><span> Green </span> </div>';
    //    $('#divAddColor').css('background', 'rgba(32, 178, 170, 0.34) none repeat scroll 0% 0%');
    //    $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(19, 131, 124)');
    //}
    //else if (color == 'gray') {

    //    document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:gray;font-size:30px;"><span> Gray </span> </div>';
    //    $('#divAddColor').css('background', 'rgba(32, 178, 170, 0.34) none repeat scroll 0% 0%');
    //    $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(19, 131, 124)');
    //}
    //else if (color == 'purple') {

    //    document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:purple;font-size:30px;"><span> Purple </span> </div>';
    //    $('#divAddColor').css('background', 'rgba(128, 0, 137, 0.27) none repeat scroll 0% 0%');
    //    $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(60, 2, 60)');
    //}
    //document.getElementById('txtTheme').value = color;
}


function expandNotes() {
    var currentWidth = $('#note_content').width();
    var currentHeight = $('#note_content').height();
    var parentDivWidth = $('#note_parent_div').width();
    
    console.log(currentHeight);

    if (currentHeight <= 500) {
        $('#note_content').width(currentWidth + 80).height(currentHeight + 60);
        $('#note_parent_div').width(parentDivWidth + 80);
        $('#icoContractNote').show();
        $('#icoExpandNote').hide();
    }
}
function contractNotes() {

    var currentWidth = $('#note_content').width();
    var currentHeight = $('#note_content').height();
    var parentDivWidth = $('#note_parent_div').width();

    console.log(currentHeight);
        $('#note_content').width(currentWidth - 80).height(currentHeight - 60);
        $('#note_parent_div').width(parentDivWidth - 80);
        $('#icoExpandNote').show();
        $('#icoContractNote').hide();
    
}


function trial1()
{
    $('#note_Modal').show();
    $('#note_body').append("<iframe src='./UserNote.aspx' id='note_content' style='border-style:none;width: 435px; height: 440px;background-color:#fff;'></iframe>");
   
}

function trial2()
{
    $("#note_Modal").fadeOut('slow', function(c){
    });
    $("#note_content").remove();
}

function testbu()
{

    var bu_session =document.getElementById('txtBusinessUnit').value;
    $('.role_bu').each(function () {
      // $(this).parent().parent().css("background-color", "transparent");
       $(this).parent().parent().removeClass("bu_highlight");
        var c = $(this).val();
        if(c == bu_session)
        {
            //$(this).parent().parent().css("background-color", "#2c3e50");
            $(this).parent().parent().addClass("bu_highlight");
        }
    });
}


function showExpiredReminders()
{
    $("#uplExpiredReminders").show();
}
function hideExpiredReminders() {
    $("#uplExpiredReminders").hide();
}

function HideReminderPopup() {
    $("#uplReminderTriggered").hide();
}

 function ShowReminderPopup()
{
     $("#uplReminderTriggered").show();
     $("#reminder_ok").click(function()
     {
         $("#uplReminderTriggered").hide();
     });
 }

//V2007

 



 //function search_reset()
 //{
 //    $(".role_checkbox > input:checked").each(function () {
 //       // $(this).prop('checked', false);
 //        //$(this).prop('checked', true);
 //        $(this).prop("checked", true).change();
 //       // $(this).trigger('click');
 //        //$(this).prop("checked", true).change();
 //    });
   
 //   // $(".current_role").click();
 //}
 //function setActiveRole()
 //{
 //    //$('.role_bookmark').first().addClass('current_role');
 //    $(".role_checkbox > input:checked").each(function () {
 //        $(this).parent().parent().find('.role_bookmark').addClass('current_role');
 //    });
 //}

function updatePanelScript()
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

    function t()
    {
        alert('hi');
    }



    //var role_Elements = new Array;
    //role_Elements = [];
    //var role_div = document.getElementById('fav_c'),
    //role_divChildren = role_div.getElementsByTagName('a');

    //for (var i = 0; i < role_divChildren.length; i++) {
    //    var extra = $(role_divChildren[i]).attr("id");
    //    $('#' + extra + '').parent().hide();
    //    var role_href = $(role_divChildren[i]).attr("href");
    //    var role_id = role_href.substring(0, role_href.indexOf('/'));
    //    role_Elements.push(role_id);
    //}
 
    //var role_name = $.trim($('#lblRoleName').html().split("-").pop());
    
    //for (var j = 0; j < role_Elements.length; j++) {
    //    if ($('#' + role_name + '-' + role_Elements[j] + '_task').length) {
    //        $('a[href="' + role_Elements[j] + '/'+ role_Elements[j]+'.aspx_'+role_name+'"]').parent().show();
    //    }
    //}

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

});

$(function() {

    $('.note-container').draggable({
        handle: 'div.s-header',
        containment: 'body'  
    });
   

    $("#sub").sortable({

    });
    $("#sub").disableSelection();


});


$(function() {
    //<!-- Settings Modal Popup -->
    $("#main_help").click(function () { $("#help_Modal").show() });;
    $("#help_close").on('click', function (c) {
        $("#help_Modal").fadeOut('slow', function (c) {
        });
    });

    $("#help_close1").on('click', function (c) {
        $("#help_Modal1").fadeOut('slow', function (c) {
        });
    });
    
    //<!-- CRM Modal Popup -->
        $("#crm").click(function() { $("#crm_Modal").show() });; 
    $("#crm_close").on('click', function(c){
        $("#crm_Modal").fadeOut('slow', function(c){
        });
    });  
    $("#setting_close").click(function()
    {
    
        $("#settings_Modal").fadeOut('slow', function () {
        });
    });
      
      
   
    
    //<!-- Contacts Popup -->
        $("#contacts").click(function() { 
            $("#contacts_Modal").show(); 
            $("#contacts_body").append("<iframe src='./ContactCard.aspx' id='contacts_content' style='border-style:none;width:100%;height:385px;background-color:#fff;'></iframe>");
            $('#toggle_div1').hide();
        });
    $("#contacts_close").on('click', function(c){
        $("#contacts_Modal").hide();
        $("#toggle_div1").show();
        $("#contacts_content").remove();
    });

    //<!-- About Modal Popup -->
        $("#about").click(function() { $("#about_Modal").show() });
    $("#about_close").on('click', function(c){
        $("#about_Modal").fadeOut('slow', function(c){
        });
    }); 
	
   //delete bookmarks 
       
    $("#delete_fav").on('click', function(c){
        $("#save_fav").show();
        $("#delete_fav").hide();
        $(".cbx_css").show();
        
    }); 
    var fav_Array = new Array;
    $("#save_fav").on('click', function(c){
        $("#delete_fav").show();
        $("#save_fav").hide();
        $(".cbx_css").hide();
       
        fav_Array=[];

        if ($('#fav_c :checkbox:checked').length > 0)
        {
            $('#fav_c input:checkbox:checked').each(function(){
                var tabId = $(this).attr("id");
                fav_Array.push(tabId);
                $('#'+tabId+'_div').remove();
            });
            PageMethods.deleteBookmarks(fav_Array);
        }
           

    }); 

	
   // <!-- Compare Popup -->
        $("#compare").click(function() { $("#compare_Modal").toggle() });; 
	
	
    //<!-- sticky notes -->

        $("#note_close").on('click', function(c){
            $("#note_Modal").fadeOut('slow', function(c){
            });
            $("#note_content").remove();
        }); 
});