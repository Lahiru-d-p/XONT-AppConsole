
var nav = $('#sideNav');
var navLi = nav.find('li#hover');
navLi.hover(
    function () {
        $(this).addClass('hover-li');
    },
    function () {
        $(this).removeClass('hover-li');
    }
    );



/* Loading Tabs and related function */
function LoadTask(url, des, taskid, tasktype, username) {
    $('#container-home').hide();
    $('#toggle').hide();
    $('.container').show();
    $('#home-toggle').show();
    $('#sticky_note').show();
    $('#compare').show();
    $('#fav').show();
    $('#mail').show();
    $('#print').show();
    $('#help').show();
    $('#tools').show();
    if (tasktype != 'S' && username == 'administrator') {
        alert("User does not have permission.");
        return;
    }

    var tab_count = $('#tabs >li').size();
    if (tab_count < 4) {
        addTab(url, des, taskid, tasktype, username);
        PageMethods.AddTaskDetails(taskid);
    }
    else {
        alert("You Have Exceeded the Maximum Number of Tabs.Close One to Open Another");
    }
}

function addTab(url, des, taskid, tasktype, username) {

    // If tab already exist in the list, return
    if ($("#" + taskid).length != 0)
        return;

    // hide other tabs
    $("#tabs li").removeClass("current");
    $("#tab_content iframe").hide();

    // add new tab and related content
    $("#tabs").append("<li class='current' style='width: 150px;overflow: hidden;text-overflow: ellipsis;'><a style='overflow:hidden;width: 84%;float:left;' title='" + des + "' class='tab' id='" + taskid + "' href='#'>" + des + "</a><a href='#' style='width:16%;margin: 0;float: right;' class='remove fa fa-close'></a></li>");
    $("#tab_content").append("<div class='intrinsic-container'><iframe name='printf_" + taskid + "' class='iframe_print'  src=' " + url + " ' id='" + taskid + "_content'></iframe></div>");
    $("#" + taskid + "_content").show();
    var task_path = $("#tabs > li.current >a").attr('title');
    TaskText(task_path);
}


$(document).on('click', '#tabs a.tab', function () {

    // Get the tab name
    var contentname = $(this).attr("id") + "_content";
    // hide all other tabs
    $("#tab_content iframe").hide();
    $("#tabs li").removeClass("current");
    // show current tab
    $("#" + contentname).show();
    $(this).parent().addClass("current");
    var task_path = $("#tabs > li.current >a").attr('title');
    TaskText(task_path);
});

$(document).on('click', '#tabs a.remove', function (e) {

    if (confirm("Are you sure you want to close this window")) {
        // Get the tab name
        var tabid = $(this).parent().find(".tab").attr("id");
        // remove tab and related content
        var contentname = tabid + "_content";
        $("#" + contentname).remove();
        $(this).parent().remove();
        var task_path = $("#tabs > li.current >a").attr('title');
        TaskText(task_path);
        PageMethods.ClearSessionTask(tabid);
    }
    else {
        e.preventDefault();
    }

    if ($("#tabs li.current").length == 0 && $("#tabs li").length > 0) {
        // find the first tab
        var firsttab = $("#tabs li:first-child");
        firsttab.addClass("current");
        // get its link name and show related content
        var firsttabid = $(firsttab).find("a.tab").attr("id");
        $("#" + firsttabid + "_content").show();
        var task_path = $("#tabs > li.current >a").attr('title');
        TaskText(task_path);

    }
    if ($("#tabs li.current").length == 0 && $("#tabs li").length == 0) {
        // find the first tab

        TaskText_Home();
        $('#container-home').toggle();
        $('.container').toggle();
        $('#sticky_note').toggle();
        $('#compare').toggle();
        $('#fav').toggle();
        $('#mail').toggle();
        $('#print').toggle();
        $('#help').toggle();
        $('#tools').toggle();
        $('#home-toggle').hide();
        if ($("#viewAlt").is(":visible")) {
            if ($('#toggle').hasClass('toggleOpen')) {
                $('#toggle').toggle();
            }
        }
    }

});

/* setting text path */
function TaskText(x) {

    $('#crumb').empty().append('<li><i class="im-home6"></i><span>' + x + '</span></li>');
}

function TaskText_Home() {

    $('#crumb').empty().append('<li><i class="im-home6"></i><a href="index.html">Home</a></li>');
}

/* Print Function */
$('#print').click(function () {
    var active_tab = $('li.current > a').attr('id');
    window.frames["printf_" + active_tab].focus();
    window.frames["printf_" + active_tab].print();
});

/* Logout Function */
$('#logout').click(function () {

    var count = $('#tabs li').size();
    if (count > 0) {
        alert("Please close all tabs before exiting");
        return false;
    }
    else {
        alert("success");
        PageMethods.ClearSessionTask();
        return true;
    }
});

/* Help Function based on component */

function HelpTab() {
    $("#help_Modal").show();
    var active_tab_id = $('li.current > a').attr('id');
    var iframe = document.createElement('iframe');
    iframe.src = 'HelpFiles/' + active_tab_id + '/' + active_tab_id + '.html'
    iframe.id = 'help_iframe'; // styles associated with this id is written on the main.aspx page //
    document.getElementById("help_Modal_iframe").appendChild(iframe);
}

/* Setting Favorite function */
var $fav_c = $('#fav_c');
if (localStorage.getItem("#fav_c")) {
    $fav_c.html(localStorage.getItem("#fav_c"));
}

$("#fav").click(function () {
    $("#fav_Modal").show();
    var active_tab_id = $('li.current > a').attr('id');
    var active_iframe_src = $('#' + active_tab_id + '_content').attr('src');
    $("#fav_value").val(active_iframe_src);
    $("#fav_value").attr('title', active_iframe_src);
});

$("#fav_save").click(function () {
    var fav_text = document.getElementById("fav_name").value;
    var fav_link = document.getElementById("fav_value").value;
    if (fav_text.length == 0) {
        $("#validation").show();
    }
    else {
        var a = document.createElement('a');
        var linkText = document.createTextNode(fav_text);
        a.appendChild(linkText);
        a.href = fav_link;
        document.getElementById("fav_c").appendChild(a);
        localStorage.setItem("#fav_c", $fav_c.html());
        $("#fav_Modal").fadeOut('slow', function (c) {
            document.getElementById('fav_name').value = '';
        });
        $("#validation").hide();
    }
});

$("#fav_cancel").on('click', function (c) {
    $("#fav_Modal").fadeOut('slow', function (c) {
        document.getElementById('fav_name').value = '';
    });
});
$("#fav_close").on('click', function (c) {
    $("#fav_Modal").fadeOut('slow', function (c) {
        document.getElementById('fav_name').value = '';
    });
});

/* Date Function */
function myFunction() {
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";


    var current_date = new Date();
    //        month_value = current_date.getMonth();
    date_value = current_date.getDate();
    //        year_value = current_date.getFullYear();
    day_value = current_date.getDay();
    document.getElementById("li5_date").innerHTML = "<span>" + date_value + "</span>";
    document.getElementById("li5_day").innerHTML = "<span>" + weekday[day_value] + "</span>";
}

$('#edit').click(function () {
    $(".d").show();
    $('.cb').show();
    $("#save").show();
    $("#edit").hide();
    $('#sideNav').sortable({

    });
    $('#sideNav').sortable('enable');
    $('#sideNav input.txt').prop('disabled', true);
    $('div.toggle-sidebar a').hide();
    $('#alt').show();

});

$('#save').click(function () {
    $("#edit").show();
    $("#save").hide();
    $(".cb").hide();
    $("input:checkbox:not(:checked)").each(function () {
        $(this).parent().hide();
    });
    $('#sideNav').sortable('disable');
    $('#sideNav input.txt').prop('disabled', false);
    $('div.toggle-sidebar a').show();
    $('#alt').hide();
});
$('#target').iconpicker({
    align: 'center', // Only in div tag
    arrowClass: 'btn-danger',
    arrowPrevIconClass: 'fa fa-chevron-circle-left',
    arrowNextIconClass: 'fa fa-chevron-circle-right',
    cols: 10,
    footer: true,
    header: true,
    icon: 'fa-file',
    iconset: 'fontawesome',
    labelHeader: '{0} of {1} pages',
    labelFooter: '{0} - {1} of {2} icons',
    placement: 'bottom', // Only in button tag
    rows: 5,
    search: false,
    searchText: 'Search',
    selectedClass: 'btn-success',
    unselectedClass: ''
});

/* Tools Modal Show/Hide */
$("#tools").click(function () {
    if ($("#tools_Modal").is(":hidden")) {
        $("#tools_Modal").slideDown("slow");
    } else {
        $("#tools_Modal").hide();
    }
});
$("#tools_container").mouseleave(function () {
    $('#tools_Modal').slideUp("slow");
});


$('.popup_open').click(function () {
    $('.popup_col').toggle();
    $('#toggle_div1').toggle();


});
$('#popup_col_close').click(function () {
    $('.popup_col').toggle();
    $('#toggle_div1').toggle();


});

$('#home-toggle').click(function () {

    $('#container-home').toggle();
    $('.container').toggle();
    $('#sticky_note').toggle();
    $('#compare').toggle();
    $('#fav').toggle();
    $('#mail').toggle();
    $('#print').toggle();
    $('#help').toggle();
    $('#tools').toggle();
    if ($("#tools").is(":hidden")) {
        TaskText_Home();
    } else {
        var task_path = $("#tabs > li.current >a").attr('title');
        TaskText(task_path);
    }
    if ($("#viewAlt").is(":visible")) {
        if ($('#toggle').hasClass('toggleOpen')) {
            $('#toggle').toggle();
        }
    }
});

$('#sticky_note').click(function () {
    $('#note_Modal').show();
    $('#note_body').append("<iframe src='./UserNote.aspx' id='note_content' style='width:100%;height:274px;background-color:#fff;'></iframe>");
});

$(".im-arrow7").click(function () {
    $("#toggle_div1").toggleClass("col_left");
    $("#toggle_div2").toggleClass("col_right");
    
});

/* Multimedia modal close */

$('#lightgallery img').on('click', function () {
    $(this).addClass('widget-selected');
    if ($('img.img-responsive.widget-selected').length == $('img.img-responsive').length) {
        $("#multimedia_close").fadeIn('slow');
    }
});

$("#multimedia_close").on('click', function (c) {
    $("#multimedia_Modal").fadeOut('slow', function (c) {
    });
});
$('.d_sub').hide();
$('#arrow2').hide();

$('#third_div').click(function () {
    $('.d_sub').toggle();
    $('#arrow1').toggle();
    $('#arrow2').toggle();

});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#profile_image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#imgInp").change(function () {
    if ($('#imgInp').val() != "") {
        $('#profile_image').show('slow');
    }
    else { $('#profile_image').hide('slow'); }
    readURL(this);
});


$(function () {

    $("#li5_date").mouseover(function () {
        $(this).children(".description").show();
    }).mouseout(function () {
        $(this).children(".description").hide();
    });


});