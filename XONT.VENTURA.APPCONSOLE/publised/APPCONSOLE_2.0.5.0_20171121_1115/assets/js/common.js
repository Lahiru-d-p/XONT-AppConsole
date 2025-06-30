
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

/* Tools Modal Show/Hide */
$("#tools").click(function () {
    if ($("#tools_Modal").is(":hidden")) {
        $("#tools_Modal").slideDown("slow");
    } else {
        $("#tools_Modal").hide();
    }
});

function LoadTask2(taskCode)
{
    $("a[name='divUserTask']").each(function () {
        var t = $(this).attr('id');
        if (t.trim() == taskCode.trim())
        {
             $(this).children('input').click();
           
        }
    });
   
}
var populated = [];
/* Loading Tabs and related function */
function LoadTask(url, des, taskid, tasktype, username) {

    $('#container-home').hide();
    $('#toggle').hide();
    $('.container').show();
    $('#home-toggle').show();
    $('#fullScreentabs').show();
    $('#sticky_note').show();
    $('#compare').show();
    $('#fav').show();
    $('#mail').show();
    $('#print').show();
    $('#help').show();
    //$('#compare_icon').show();
    $('#tools').show();



    if (tasktype != 'S' && username == 'administrator') {
        alert("User does not have permission.");
        return;
    }

    var tab_count = $('#tabs >li').size();
    if (tab_count > 0) {
        $('#compare_icon').show();
    }
    if (tab_count < 4) {

        var tabToPopulate = 0; //no switch case for 0 that is why

        for (var current = 1; current < 5; current++) {
            available = $.inArray(current, populated);

            if (available < 0) {
                populated.push(current);
                tabToPopulate = current;

                break;
            }
        }

        // Add tab function
        switch (tabToPopulate) {
            case 1:
                addTab(url, des, taskid, tasktype, username, 1);
                break;
            case 2:
                addTab(url, des, taskid, tasktype, username, 2);
                break;

            case 3:
                addTab(url, des, taskid, tasktype, username, 3);
                break;

            case 4:
                addTab(url, des, taskid, tasktype, username, 4);
                break;
            default: break;
        }
        // j++;
        PageMethods.AddTaskDetails(taskid);
    }
    else {
        if ($("#" + taskid).length != 0) {
            for (var l = 1; l < 5; l++) {
                $("#tab_content" + l).hide();
            }
            var trial2 = $("#" + taskid + "_content").parent().parent().attr("id");
            $("#" + trial2).show();
            $("#" + trial2).css('visibility', 'visible');
            $("#tabs li").removeClass("current");
            $("#" + taskid).parent().addClass("current");
            return;

        }
        else {
            $("#max_tab").show();
            $("#max_ok").click(function () {
                $("#max_tab").fadeOut('slow', function (c) {
                });
            });
        }
    }
}
//Function calld when link in nav. panel clicked
function addTab(url, des, taskid, tasktype, username, i) {

    $("#tools_container").hide();
    // If tab already exist in the list, return
    if ($("#" + taskid).length != 0) {
        var index = populated.indexOf(i);
        populated.splice(index, 1);
        for (var k = 1; k < 5; k++) {
            $("#tab_content" + k).hide();
        }
        var trial1 = $("#" + taskid + "_content").parent().parent().attr("id");
        $("#" + trial1).show();
        $("#" + trial1).css('visibility', 'visible');
        $("#tabs li").removeClass("current");
        $("#" + taskid).parent().addClass("current");
        $("#tools_container").show();

        var task_path = $("#tabs > li.current >a").attr('title');
        TaskText(task_path);
        return;

    }

    // hide other tabs
    $("#tabs li").removeClass("current");
    //$("#tab_content iframe").hide();

    for (var k = 1; k < 5; k++) {
        $("#tab_content" + k).hide();
    }

    // add new tab and related content
    $("#tabs").append("<li class='current' style='width: 150px;overflow: hidden;text-overflow: ellipsis;'><a style='overflow:hidden;width: 70%;float:left;' title='" + des + "' class='tab' id='" + taskid + "' href='#'>" + des + "</a><a id='" + taskid + "_close' href='#' style='width:16%;margin: 0;float: right;' class='remove im-close'></a><input type='checkbox' id='" + taskid + "_min'  class='cb_cmp' style='display:none;margin-top:4px;float: right;'/></li>");
    $('#tab_content' + i).append("<div id='loader" + taskid + "' class='loader'><img src='./images/task_loading.gif' width='50' height='50' style='margin-left:45%; margin-top:25%'/></div><div id='intrinsic" + taskid + "' class='intrinsic-container'><iframe name='printf_" + taskid + "' class='iframe_print'  src=' " + url + " ' id='" + taskid + "_content' ></iframe></div>");
    var trial = $("#" + taskid + "_content").parent().parent().attr("id");
    $("#" + trial).show();
    $("#" + trial).css('visibility', 'visible');

    var task_path = $("#tabs > li.current >a").attr('title');
    document.getElementById('txtCurrentTaskCode').value = taskid;
    TaskText(task_path);

    if ($('#note_Modal').css('display') == 'block') {
        $("#note_Modal").hide();
    }

    $("#" + taskid + "_content").on('load', function () {



       

        $('#intrinsic' + taskid + '  iframe').css("background-color", "#fff");
        $('#loader' + taskid).remove();
        $("#tools_container").show();

        PageMethods.checkUserNotificationTask(taskid,notification_result);
    })

  
}

function notification_result(ResultString) {
    var task = new Array();
    task = ResultString;
    if (task[1] == "true")
    {
        var iframe = $("#" + task[0] + "_content").contents();

            iframe.find("#btnOK").click(function () {
                PageMethods.UpdateUserNotification(task[0]);
            });
    }
}

//Function called when clicking various tabs to set as active
$(document).on('click', '#tabs a.tab', function () {

    if ($('#note_Modal').css('display') == 'block') {
        $("#note_alert").show();
        $("#note_ok").click(function () {
            $("#note_alert").fadeOut('slow', function (c) {
            });

        });
        return;
    }
    else {

        // added on 5/18/2016 with respect to comparison feature

        var contentname = $(this).attr("id") + "_content";

        var t_id = $(this).attr("id");
        if ($('#intrinsic' + t_id).hasClass('current_min')) {
            $('.iframe_print').css('border', 'none');
            $('#' + contentname).css('border', '1px solid rgb(202, 201, 199)');
        }

        // hide all other tabs
        for (var k = 1; k < 5; k++) {
            $("#tab_content" + k).hide();
        }

        $("#tabs li").removeClass("current");
        var trial = $("#" + contentname).parent().parent().attr('id');
        $("#" + trial).show();
        $(this).parent().addClass("current");
        var task_path = $("#tabs > li.current >a").attr('title');
        var current_tabid = $(this).attr("id");
        document.getElementById('txtCurrentTaskCode').value = current_tabid;
        TaskText(task_path);
    }

});

//Close function

$(document).on('click', '#tabs a.remove', function (e) {

    var tabid = $(this).parent().find(".tab").attr("id");
    $("#" + tabid).addClass("current_close");

    //pdf report close issue...V2013
    var t = $('#' + tabid + '_content').attr("id");
    var pdf = $('#' + t).contents().find('#pdffile')
    if (pdf) {
        $('#' + t).contents().find('#pdffile').css('display', 'none');
    }
    //...

    $("#close_Modal").show();
    $("#close_cancel").click(function () {
        var t = $(".current_close").attr("id");
        $("#" + t).removeClass("current_close");

        //pdf report close issue...V2013
        var t = $('#' + tabid + '_content').attr("id");
        var pdf = $('#' + t).contents().find('#pdffile')
        if (pdf) {
            $('#' + t).contents().find('#pdffile').css('display', 'block');
        }
        //...

        $("#close_Modal").fadeOut('slow', function (c) {
        });
    });
    $("#close_ok").click(function () {
        if ($('#note_Modal').css('display') == 'block') {
            $("#note_Modal").hide();
        }
        var t = $(".current_close").attr("id");

        var section1 = $('.intrinsic-container');
        if ($('#intrinsic' + t).hasClass("current_min")) {

            var section = $('#sidebar');
            var width = section.width();
            if (width == 65) {

                $(".current_min").parent().removeClass('d_block');
                $('.current_min').removeClass("current_min_second");
                $('.current_min').removeClass("med1");
                $('.intrinsic-container').removeClass("med1_fullscreen");
                $(".intrinsic-container").removeClass("current_min_fullscreen");
                $('.intrinsic-container').removeClass("current_min");
                $('.intrinsic-container').addClass("med");
                $('.iframe_print').css('border', 'none');
            }
            else if (width == 255) {

                $(".current_min").parent().removeClass('d_block');
                $('.current_min').removeClass("current_min_second");
                $('.intrinsic-container').removeClass("current_min");
                $(".intrinsic-container").removeClass("current_min_fullscreen");

                $('.iframe_print').css('border', 'none');
            }
        }

        $('#loader' + t).remove();
        var contentname = t + "_content";
        var id_tab = $('#' + contentname).parent().parent().attr("id");
        var tabnumber = parseInt(id_tab.substr(id_tab.length - 1));
        var index = populated.indexOf(tabnumber);
        populated.splice(index, 1);
        $("#" + contentname).remove();
        $('#intrinsic' + t).remove();
        $("#" + t).parent().remove();
        var task_path = $("#tabs > li.current >a").attr('title');
        TaskText(task_path);

        // added on 5/18/2016 with respect to comparison feature

        var tab_count = $('#tabs >li').size();
        if (tab_count < 2) {
            $('#compare_icon').hide();
            $('.cb_cmp').attr('checked', false);
            $('.cb_cmp').hide();

        }


        PageMethods.ClearSessionTask(tabid);
        cleanLocalStorage(tabid);

        if ($("#tabs li.current").length == 0 && $("#tabs li").length > 0) {
            // find the first tab
            var firsttab = $("#tabs li:first-child");
            firsttab.addClass("current");
            // get its link name and show related content
            var firsttabid = $(firsttab).find("a.tab").attr("id");
            var firsttab_content = $("#" + firsttabid + "_content").parent().parent().attr("id");
            $("#" + firsttab_content).show();
            var task_path = $("#tabs > li.current >a").attr('title');
            document.getElementById('txtCurrentTaskCode').value = firsttabid;
            TaskText(task_path);
        }
        if ($("#tabs li.current").length == 0 && $("#tabs li").length == 0) {
            TaskText_Home();
            $('#container-home').toggle();
            $('.container').toggle();
            $('#sticky_note').toggle();
            $('#compare').toggle();
            $('#fav').toggle();
            $('#mail').toggle();
            $('#print').toggle();
            $('#help').toggle();
            //$('#compare_icon').toggle();
            $('#tools').toggle();
            $('#home-toggle').hide();
            $('#fullScreentabs').toggle();
            if ($("#viewAlt").is(":visible")) {
                if ($('#toggle').hasClass('toggleOpen')) {
                    $('#toggle').toggle();
                }
            }
            if ($("#wrapper").hasClass("fullScreenParent")) {
                $("#wrapper").removeClass("fullScreenParent");
                $(".intrinsic-container").removeClass("fullScreenChild");
                $("#minimizeTabs").hide();
            }

        }
        $("#" + t).removeClass("current_close");
        $("#close_Modal").fadeOut('slow', function (c) {
        });
    });

});

$('#compare').click(function () {
    $('.cb_cmp').show();
});

//minimize 

$(document).on('change', '#tabs .cb_cmp', function (e) {
    var len = $(".cb_cmp:checked").length;
    var nav_element = $('#sidebar');
    var nav_width = nav_element.width();
    if (len == 2) {
        // Hide checkboxes not checked //
        $(".cb_cmp:not(:checked)").each(function () {
            $(this).hide();
        });




        // Get id of tab & add border to iframe//
        var tabid = $(this).parent().find(".tab").attr("id");
        $('#intrinsic' + tabid + ' iframe').css('border', '1px solid rgb(202, 201, 199)');

        // Add class to iframe parent of tabs with checked checkboxes //
        $('.cb_cmp:checked').each(function () {
            var tabid1 = $(this).parent().find(".tab").attr("id");
            $('#intrinsic' + tabid1).addClass("current_min");
            if ($('#intrinsic' + tabid1).hasClass("fullScreenPartialChild")) {
                $('#intrinsic' + tabid1).addClass("current_min_fullscreen");
            }


        });


        if (nav_width == 65) {
            $('.intrinsic-container').each(function () {
                if ($(this).hasClass('current_min')) {
                    if ($(this).hasClass("fullScreenPartialChild")) {
                        $(this).addClass("current_min_fullscreen");
                        $(this).addClass("med1_fullscreen");
                    }
                    $(this).addClass('med1');
                }
            });
        }


        // Align one of the comparison iframe to the right by adding the following class //
        $('.current_min').eq(1).addClass("current_min_second");
        $(".current_min").parent().addClass('d_block');

        // Toggle active tab by clicking on iframe //
        $('.tab').each(function () {
            var currenttab = $(this).attr("id");
            var oFrame = document.getElementById(currenttab + "_content");
            oFrame.contentWindow.document.onclick = function () {
                $('.iframe_print').css('border', 'none');
                $("#tabs li").removeClass("current");
                $('#' + currenttab).parent().addClass("current");
                var task_path = $("#tabs > li.current >a").attr('title');
                TaskText(task_path);
                document.getElementById('txtCurrentTaskCode').value = currenttab;
                $('#' + currenttab + '_content').css('border', '1px solid rgb(202, 201, 199)');
            };
        });

        // Set z-indeex of iframes not associated with checkboxes to a higher value //
        $('.intrinsic-container').each(function () {
            if (!$(this).hasClass('current_min')) {
                $(this).css('z-index', '2000');
            }
            else {
                $(this).css('z-index', '');
            }
        });
    }
    else if (len == 1) {

        // In case of only one checkbox //
        $('.cb_cmp').show();
        //$(".current_min").parent().removeClass('d_block');
        //$('.current_min').removeClass("current_min_second");
        //    $('.intrinsic-container').removeClass("current_min");

        //    $('.iframe_print').css('border', 'none');
        if (nav_width == 65) {
            $(".current_min").parent().removeClass('d_block');
            $('.current_min').removeClass("current_min_second");
            $('.current_min').removeClass("med1");
            $('.intrinsic-container').removeClass("current_min");
            $('.intrinsic-container').removeClass("med1_fullscreen");
            $(".intrinsic-container").removeClass("current_min_fullscreen");
            $('.intrinsic-container').addClass("med");
            $('.iframe_print').css('border', 'none');
        }
        else if (nav_width == 255) {
            $(".current_min").parent().removeClass('d_block');
            $('.current_min').removeClass("current_min_second");
            $('.intrinsic-container').removeClass("current_min");
            $(".intrinsic-container").removeClass("current_min_fullscreen");

            $('.iframe_print').css('border', 'none');


        }


        $('.intrinsic-container').each(function () {
            if (!$(this).hasClass('current_min')) {
                $(this).css('z-index', '2000');
            }
        });
    }
    else if (len == 0) {

        // When all checkboxes are unchecked //
        $('.cb_cmp').hide();

        if (width == 65) {

            $(".current_min").parent().removeClass('d_block');
            $('.current_min').removeClass("current_min_second");
            $('.current_min').removeClass("med1");
            $('.intrinsic-container').removeClass("current_min");
            $('.intrinsic-container').removeClass("med1_fullscreen");
            $(".intrinsic-container").removeClass("current_min_fullscreen");
            $('.intrinsic-container').addClass("med");
            $('.iframe_print').css('border', 'none');
        }
        else if (width == 255) {

            $(".current_min").parent().removeClass('d_block');
            $('.current_min').removeClass("current_min_second");
            $('.intrinsic-container').removeClass("current_min");
            $(".intrinsic-container").removeClass("current_min_fullscreen");
            $('.iframe_print').css('border', 'none');
        }

        $('.intrinsic-container').each(function () {
            if (!$(this).hasClass('current_min')) {
                $(this).css('z-index', '');
            }
        });
    }
    else {
        // When number of checkboxes checked exceeds 2 , alert //
        $('.iframe_print').css('border', 'none');
        $(this).attr('checked', false);
        $("#cmp_tab").show();
        $("#cmp_ok").click(function () {
            $("#cmp_tab").fadeOut('slow', function (c) {
            });
        });

    }

});

//right sidebar - minimum one checkbox must be checked at all times
$(document).on('change', '.role_checkbox > input', function (e) {
    var c_len = $(".role_checkbox > input:checked").length;
    if (c_len == 1) {
        $('.role_checkbox > input:checked').prop('disabled', true);
    }
    else {
        $('.role_checkbox > input').prop('disabled', false);
    }

});

// Toggle sidebar - added med1 for comparison feature

$(".toggle-sidebar").click(function () {

    var section1 = $('.intrinsic-container');

    var section = $('#sidebar');
    var width = section.width();
    if (width == 65) {
        $('.intrinsic-container').each(function () {
            if ($(this).hasClass('current_min')) {
                $(this).addClass('med1');
            }
            else {
                $(this).addClass('med');
            }

        });
    }
    else if (width == 255) {
        $('.intrinsic-container').each(function () {
            if ($(this).hasClass('current_min')) {
                $(this).removeClass('med1');
            }
            else {
                $(this).removeClass('med');
            }

        });

    }
});


/*link bookmarks with nav click */
$(document).on('click', '#fav_c a.fav_css', function () {

    var element_fav = $(this).attr("href");
    var nav_element = element_fav.substring(0, element_fav.indexOf('/'));
    var role_name_original = $.trim(element_fav.split("_").pop());
    var role_name = $.trim(role_name_original.replace(/\+/g, '_'));
    //var role_id = $.trim($("input[value=" + role_name + "]").attr("id"));
    var role_id = $("[title=" + role_name + "]").attr("id");
    var role_chkbox_id = $('#' + role_id).parent().parent().parent().find('.role_checkbox > input ').attr('id');
    if ($('#' + role_chkbox_id).is(':checked')) {

        $("#" + role_name + "-" + nav_element + "_task").click();
    }
    else {
        $('#' + role_chkbox_id).click();
        setTimeout(function () {
            $("#" + role_name + "-" + nav_element + "_task").click();
        }, 500);
    }
    return false;
});






/* setting text path */
function TaskText(x) {

    $('#crumb').empty().append('<li><i class="im-home6"></i><span>' + x + '</span></li>');
}

function TaskText_Home() {

    $('#crumb').empty().append('<li><i class="im-home6"></i><a href="Main.aspx">Home</a></li>');
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
        // alert("Please close all tabs before exiting");
        $("#logout_modal").show();
        $("#logout_ok").click(function () {
            $("#logout_modal").fadeOut('slow', function (c) {
            });
        });
        return false;
    }
    else {
        PageMethods.ClearSessionTask();
        return true;
    }
});

/* Settings Function */

$("#settings_Modal").hide();
$(".settings_open").click(function () {
    var count = $('#tabs li').size();
    if (count > 0) {
        $("#settings_postback").show();
        $("#postback_ok").click(function () {
            $("#settings_postback").fadeOut('slow', function (c) {
            });
        });
        return false;
    }
    else {
        $("#settings_Modal").show();
        $("#ok").on('click', function (c) {
            $("#settings_Modal").fadeOut('slow', function (c) {
            });
        });
    }

});

/* Help Function based on component */

function HelpTab() {
    $("#help_Modal1").show();
    var active_tab_id = $('li.current > a').attr('id');
    var iframe = document.createElement('iframe');
    iframe.src = 'HelpFiles/' + active_tab_id + '/' + active_tab_id + '.html'
    iframe.id = 'help_iframe'; // styles associated with this id is written on the main.aspx page //
    document.getElementById("help_Modal_iframe").appendChild(iframe);
}

/* Setting Favorite function */

$("#fav").click(function () {
    $("#fav_Modal").show();
    $("#fav_name").focus();
    var active_tab_id = $('li.current > a').attr('id');
    var active_iframe_src = $('#' + active_tab_id + '_content').attr('src');
    $("#fav_value").val(active_iframe_src);
    $("#fav_value").attr('title', active_iframe_src);
});

var bookmark_Array = new Array;

$("#fav_save").click(function () {
    var fav_text_original = $.trim(document.getElementById("fav_name").value);
    var fav_text = fav_text_original.replace(/ /g, "_");
    var fav_displayName = document.getElementById("fav_name").value;
    var fav_link_original1 = document.getElementById("fav_value").value;

    var fav_taskcode = $.trim(fav_link_original1.substr(0, fav_link_original1.indexOf('/')));
    var fav_role_name = $("span:contains(" + fav_taskcode + ")").parent().parent().parent().parent().prev().children().html();
    var role_name_original = $.trim(fav_role_name.split("-").pop());
    var role_name1 = role_name_original.replace(/ /g, "+");
    fav_link_original = $.trim(fav_link_original1);
    var fav_link = fav_link_original += '_' + role_name1;

    bookmark_Array = [];
    var flag = 0;
    var bookmark_div = document.getElementById('fav_c'),
     divChildren = bookmark_div.getElementsByTagName('input');

    for (var i = 0; i < divChildren.length; i++) {
        var bookmark_id = $(divChildren[i]).attr("id");
        bookmark_Array.push(bookmark_id);
    }

    for (var j = 0; j < bookmark_Array.length; j++) {
        if (fav_text == bookmark_Array[j]) {
            $("#duplicate_name").show();
            flag = 1;
            return;
        }
    }

    if (fav_text.length == 0) {
        $("#validation").show();
        return;
    }
    else if ($('.error-keyup-2').length) {
        return;
    }
    else {

        var a_length = $("#fav_c > div").length;
        if (a_length > 7) {
            $("#max_fav").show();
            $("#max_favok").click(function () {
                $("#max_fav").fadeOut('slow', function (c) {
                });
                $("#fav_Modal").fadeOut('slow', function (c) {
                    document.getElementById('fav_name').value = '';
                });
            });
        }
        else {

            $('#fav_c').append("<div id='" + fav_text + "_div'><span class='cbx_css'><input id='" + fav_text + "' type='checkbox' name='" + fav_text + "_cbx' /></span><a  class='fav_css' href='" + fav_link + " ' id='" + fav_text + "_link' >" + fav_displayName + "</a></div>");

            $("#fav_Modal").fadeOut('slow', function (c) {
                document.getElementById('fav_name').value = '';
            });
            $("#validation").hide();
            $('span.error-keyup-2').remove();
            $('span#duplicate_name').hide();
            PageMethods.saveBookmarks(fav_text, fav_link, fav_displayName);

        }
    }
});

$("#fav_cancel").on('click', function (c) {
    $("#fav_Modal").fadeOut('slow', function (c) {
        document.getElementById('fav_name').value = '';
        $('span.error-keyup-2').remove();
    });
});
$("#fav_close").on('click', function (c) {
    $("#fav_Modal").fadeOut('slow', function (c) {
        document.getElementById('fav_name').value = '';
        $('span.error-keyup-2').remove();
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
    date_value = current_date.getDate();
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
    $("#sidebar input:checkbox:not(:checked)").each(function () {
        $(this).parent().hide();
    });
    $('#sideNav').sortable('disable');
    $('#sideNav input.txt').prop('disabled', false);
    $('div.toggle-sidebar a').show();
    $('#alt').hide();
});

//$('#target').iconpicker({
//    align: 'center', // Only in div tag
//    arrowClass: 'btn-danger',
//    arrowPrevIconClass: 'fa fa-chevron-circle-left',
//    arrowNextIconClass: 'fa fa-chevron-circle-right',
//    cols: 10,
//    footer: true,
//    header: true,
//    icon: 'fa-file',
//    iconset: 'fontawesome',
//    labelHeader: '{0} of {1} pages',
//    labelFooter: '{0} - {1} of {2} icons',
//    placement: 'bottom', // Only in button tag
//    rows: 5,
//    search: false,
//    searchText: 'Search',
//    selectedClass: 'btn-success',
//    unselectedClass: ''
//});



$(document).mouseup(function (e) {
    var container = $("#tools_Modal");

    if (!container.is(e.target) // if the target of the click isn't the container...
        && container.has(e.target).length === 0) // ... nor a descendant of the container
    {
        $('#tools_Modal').slideUp("slow");
    }
});


$('.popup_open').click(function () {
    $('.popup_col').toggle();
    $('#toggle_div1').toggle();


});
$('#popup_col_close').click(function () {
    $('.popup_col').toggle();
    $('#toggle_div1').toggle();


});

$('#expired_popup_col_close').click(function () {
    $("#uplExpiredReminders").hide();
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
    $('#compare_icon').toggle();
    $('#fullScreentabs').toggle();
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

$('#fullScreentabs').click(function () {
    //$("#fullscreen").click();
    $('.intrinsic-container').each(function () {

        if ($(".intrinsic-container").hasClass("current_min")) {
            if ($(".intrinsic-container").hasClass("med1")) {
                $(".current_min").addClass("med1_fullscreen");
            }
            $(".current_min").addClass("current_min_fullscreen");
            $(".intrinsic-container").addClass("fullScreenPartialChild");
        }
        else {
            $(".intrinsic-container").addClass("fullScreenChild");
        }
    });
    $("#wrapper").addClass("fullScreenParent");
    $("#minimizeTabs").show();
    $(".cb_cmp").attr("disabled", true);
});

$('#minimizeTabs').click(function () {
    $("#wrapper").removeClass("fullScreenParent");
    $('.intrinsic-container').each(function () {
        if ($(".intrinsic-container").hasClass("med1_fullscreen")) {
            $(".intrinsic-container").removeClass("med1_fullscreen");
        }
        if ($(".intrinsic-container").hasClass("fullScreenPartialChild")) {
            $(".current_min").removeClass("current_min_fullscreen");
            $(".intrinsic-container").removeClass("fullScreenPartialChild");
        }
        else {
            $(".intrinsic-container").removeClass("fullScreenChild");
        }
    });
    $(".cb_cmp").removeAttr("disabled");
    $(this).hide();
});

$(".im-arrow7").click(function () {

    $("#toggle_div1").toggleClass("col_left");
    $("#toggle_div2").toggleClass("col_right");

    $("li.d").toggleClass("hasSub");
    var i = 0;
    $('#sideNav a#expand ').each(function () {
        i++;
        if ($(this).hasClass("expand")) {
            $(".sideNav-arrow").removeClass("rotateM180");
            $(this).removeClass('expand');
            $('ul.sub').removeClass('show');
            $('ul.sub').hide();
        }
        else {
            $(this).toggleClass('notExpand');
        }
    });
});


$('#right-sidebar ul li a').click(function () {
    if ($("#sidebar").hasClass("collapse-sidebar")) {
        $('.im-arrow7').click();
    }
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

$("#mail").click(function () {
    ShowNav();
    setTimeout(function () {
        PopUpURL('UserMail.aspx');
    }, 500);
});

$('.d_sub').hide();
$('#arrow2').hide();

$('#third_div').click(function () {
    $('.d_sub').toggle();
    $('#arrow1').toggle();
    $('#arrow2').toggle();

});
function HideMail() {
    document.getElementById("Iframe1").src = "";
    $("#mail_Modal").hide();
}
function HideErrorPopup() {
    document.getElementById("Iframe1").src = "";
    $("#mail_Modal").hide();
}
function HideUserMessage() {
    document.getElementById("Iframe1").src = "";
    $("#mail_Modal").hide();
}

function HideAdminAlert() {
    document.getElementById("Iframe1").src = "";
    $("#mail_Modal").hide();
}
function PopUpURL(url) {
    ShowPopup(url)
}
function ShowPopup(url) {

    $("#mail_Modal").show();
    document.getElementById("Iframe1").src = url;
}
function ShowNav() {
    if ($("#sidebar").hasClass("collapse-sidebar")) {
        $('.im-arrow7').click();
        setTimeout(function () {
            PageMethods.ImgMailClick();
        }, 500);
    }
    else {
        setTimeout(function () {
            PageMethods.ImgMailClick();
        }, 500);
    }
}

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

// Extra Multimedia related scripts 
function PopMultimediaURL(url) {
    ShowMultimedia(url)
}

function ShowMultimedia(url) {
    $("#multimedia_Modal").show();
    document.getElementById("Iframe1Multimeadia").src = url;

}
function HideMultimediaPopup() {
    $("#multimedia_Modal").hide();
    document.getElementById("Iframe1Multimeadia").src = "";
}

// clean local storage when exit from V3 component.
function cleanLocalStorage(taskCode) {
    for (var key in localStorage) {
        var keySplitArray = key.split('_');
        if (keySplitArray.length>0) {
            if (keySplitArray[0]==taskCode) {
                localStorage.removeItem(key);
            }
        }
    }
}