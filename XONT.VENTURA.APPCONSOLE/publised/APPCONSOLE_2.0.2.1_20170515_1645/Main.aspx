<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="XONT.Ventura.AppConsole.Main" EnableEventValidation="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc2" %>

<!DOCTYPE html>
<html lang="en" class="no-js">
	<head id="Head1" runat ="server">
		<meta charset="UTF-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge">
		<meta name="viewport" content="width=device-width, initial-scale=1"> 
		<title>X-ONT VENTURA CRM</title>
		<meta name="description" content="VENTURA CRM" />
		<meta name="keywords" content="VENTURA CRM" />

        <script src="js/Main_TabPanel.js" type="text/javascript"></script>
        <link href="assets/cdn/font_googleapi.css" rel="stylesheet" type="text/css" />
		<link rel="stylesheet" type="text/css" href="assets/css/common.css"/>
        <%--<link rel="stylesheet" type="text/css" href="assets/css/green.css"; />--%>

        <link href="TabPanel.css" rel="stylesheet" type="text/css" />
		<link href="assets/css/bootstrap.css" rel="stylesheet" />
      

		<link rel="stylesheet" href="icon-fonts/font-awesome-4.6.3/css/font-awesome.min.css"/>
        <link rel="stylesheet" href="bootstrap-iconpicker/css/bootstrap-iconpicker.min.css"/>
		
         <link href="assets/css/calendar.css" rel="stylesheet" />		
        <link href="assets/css/icons.css" rel="stylesheet" />
		 <link href="assets/cdn/font_awesome.css" rel="stylesheet" />

		<link rel="stylesheet" type="text/css" href="css/normalize.css" />
        <link href="css/slick.css" rel="stylesheet" />
        <link href="css/slick-theme.css" rel="stylesheet" />
		<link rel="stylesheet" type="text/css" href="css/tabs.css" />
		<link rel="stylesheet" type="text/css" href="css/tabstyles.css" />
		  <link href="./lightGallery-master/dist/css/lightgallery.css" rel="stylesheet">
		
		<link rel="apple-touch-icon-precomposed" sizes="144x144" href="assets/img/ico/apple-touch-icon-144-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="114x114" href="assets/img/ico/apple-touch-icon-114-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="72x72" href="assets/img/ico/apple-touch-icon-72-precomposed.png">
        <link rel="apple-touch-icon-precomposed" href="assets/img/ico/apple-touch-icon-57-precomposed.png">

        <meta name="msapplication-TileColor" content="#3399cc" />
	    <link href="index.css" rel="stylesheet" type="text/css" />
	    <link rel="stylesheet" type="text/css" href="jquery.gridster.css">
        <link rel="stylesheet" type="text/css" href="demo.css">
        <link href="css/newinlinedata.css" rel="stylesheet" type="text/css" />
      
        <script src="LCSK/assets/js/jquery-2.1.1.min.js"></script>
        <%--<script src="assets/cdn/jquery.js"></script>--%> 
          <%--<script src="http://code.jquery.com/jquery-1.6.4.js" type="text/javascript"></script>--%>
        <%--<script type="text/javascript" src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>--%>
       
       <script src="js/slick.js"></script>
       
	    <script src="lcsk/assets/js/jquery.signalR-2.1.1.min.js" type="text/javascript"></script>
	    <script src="signalr/hubs" type="text/javascript"></script>
        <script src="js/adminAlert.js"></script>
	    <script src="lcsk/chat.js" type="text/javascript"></script>


        <script src="assets/cdn/jquery_ui.js"></script>
        <link href="dist/css/bootstrap-colorpicker.min.css" rel="stylesheet">
        <script src="dist/js/bootstrap-colorpicker.js"></script>
		<link rel="stylesheet" type="text/css" href="assets/css/component.css" />
		
        <link rel="stylesheet" href="dist/css/owl.carousel.css">
        <link rel="stylesheet" href="dist/css/owl.theme.css">
        <script src="dist/js/owl.carousel.js"></script>

		<script src="assets/js/modernizr.custom.js"></script>
        <script src="assets/js/main.js"></script>
        <script src="assets/js/custom.js" type="text/javascript"></script>
        <script src="js/Disable_BackSpace.js"></script>
        
<script type="text/javascript">

    function HandleClose() {
        var browser = navigator.appName;
        if (browser == "Microsoft Internet Explorer") {
            if ((window.event.clientX < 0) || (window.event.clientY < 0)) {
                PageMethods.AbandonSession();
            }
        }
        else {
            PageMethods.AbandonSession();
        }
        //VR004 End

    }
             </script>
  
  <script type="text/javascript" language="javascript">
      $(document).ready(function () {

          //$('.your-class').slick({
          //   // dots: true,
          //    arrows: true,
          //    //infinite: true,
          //    //speed: 100,
          //    //initialSlide: 2,
          //    slidesToShow: 4,
          //    //centerMode: true,
          //    variableWidth: true,
          //   // draggable: true,
          //    centerPadding: '60px'
          //});

          $("#owl-example").owlCarousel({
              items: 3,
              navigation: true,
              navigationText: [
     "<span class='fa fa-chevron-circle-left'></span>",
     "<span class='fa fa-chevron-circle-right'></span>"
              ]
          });

          $('#lightgallery').lightGallery({
              enableDrag: false,
              thumbnail: false,
              controls: false,
              autoplayControls: false
          });

          //Iframe resize //
          myFunction();

          $('.role_checkbox > input').prop('checked', false);
          $('.role_checkbox > input:first').prop('checked', true);
          $('.role_checkbox > input:first').prop('disabled', true);
          $('.role_checkbox > input:first').parent().parent().find('.role_bookmark').addClass('current_role');


          $(".role_bookmark").first().parent().parent().addClass('role_highlight');
          $(".role_checkbox > input").click(function (e) {
              $('#s').val('');
              $('.role_checkbox > input').each(function () {
                  $(this).parent().parent().find('.role_bookmark').removeClass('current_role');
              });
              $(".role_checkbox > input:checked").each(function () {
                  $(this).parent().parent().find('.role_bookmark').addClass('current_role');
              });

              $('.role_checkbox > input').each(function () {
                  $(this).parent().parent().find('a').removeClass('role_highlight');
              });
              $(".role_checkbox > input:checked").each(function () {
                  $(this).parent().parent().find('a').addClass('role_highlight');
              });

              //setTimeout(function () {
              //    $('#right-sidebar').addClass('hide-sidebar');
              //}, 300);

          });

          $(document).mouseup(function (e) {
              var container = $("#right-sidebar");

              if (!container.is(e.target) // if the target of the click isn't the container...
                  && container.has(e.target).length === 0) // ... nor a descendant of the container
              {
                  $('#right-sidebar').addClass('hide-sidebar');
              }
          });


          ///role_bookmarks();


          $('.keyup-char').keyup(function () {
              $('span.error-keyup-2').remove();
              $('span#duplicate_name').hide();
              $('#validation').hide();
              var inputVal = $(this).val();
              var characterReg = /^\s*[a-zA-Z0-9,\s]+\s*$/;
              if (!characterReg.test(inputVal)) {
                  $(this).after('<span class="error error-keyup-2">No special characters allowed.</span>');
              }
              else if ($(this).val().length > 28) {
                  $(this).after('<span class="error error-keyup-2">Maximum 28 characters.</span>');
              }
              if ($(this).val().length === 0) {
                  $('span.error-keyup-2').remove();
                  $('span#duplicate_name').hide();
              }
          });



          $('#search').click(function () {
              $('#search').hide();
              $('#edit').hide();
              $('#save').hide();
              $('#fullscreen').hide();
              $('#reset').hide();
              $('#expand').hide();
              $('#role_test').hide();
              $('#view').hide();
              $('#s').show();
              $('#search_close').show();
              $('#s').focus();

          });
          $('#search_close').click(function () {
              $('#search').show();
              $('#edit').show();
              $('#save').hide();
              $('#fullscreen').show();
              $('#reset').show();
              $('#expand').show();
              $('#role_test').show();
              $('#s').hide();
              $('#view').show();
              $('#search_close').hide();
              if ($('#role_icon').hasClass('fa fa-outdent')) {
                  $('#role_icon ').toggleClass("fa fa-indent");
                  $('#role_icon ').toggleClass("fa fa-outdent");
              }

              $('#role_icon ').removeClass("hasTaskCode");

          });

          $(".changePswd").keyup(function (e) {
              if (($('#txtCurrentPassword').val()) && ($('#txtNewPassword').val()) && ($('#txtConfirmNewPassword').val())) {

                  $('#changePassword').show();
              }
              else {

                  $('#changePassword').hide();
              }
          });



          $("#s").keyup(function t(e) {
              var code = e.which; // recommended to use e.which, it's normalized across browsers

              var valThis = $(this).val().toLowerCase();
              if ($('#role_icon').hasClass('hasTaskCode')) {
                  $('.role_code ').each(function () {
                      var text = $(this).text().toLowerCase();
                      if (text.indexOf(valThis) == 0) {
                          $(this).closest('li.d').find("ul#documents").addClass('show');
                          $(this).closest('li.d').find("a#expand").removeClass('notExpand');
                          $(this).closest('li.d').find("a#expand").addClass('expand');
                          $(this).closest('li.d').find("i.im-arrow-down2").removeClass('rotate0');
                          $(this).closest('li.d').find("i.im-arrow-down2").addClass('rotateM180');
                          $(this).closest('li.d').addClass('highlight-menu');
                      }
                      else {
                          $(this).parent().parent().hide();
                      }
                      // return false;

                  });
              }
              else {
                  $('.AppCMenutask ').each(function () {
                      var text = $(this).attr('value').toLowerCase();


                      if (text.indexOf(valThis) == 0) {
                          $(this).closest('li.d').find("ul#documents").addClass('show');
                          $(this).closest('li.d').find("a#expand").removeClass('notExpand');
                          $(this).closest('li.d').find("a#expand").addClass('expand');
                          $(this).closest('li.d').find("i.im-arrow-down2").removeClass('rotate0');
                          $(this).closest('li.d').find("i.im-arrow-down2").addClass('rotateM180');
                          $(this).closest('li.d').addClass('highlight-menu');
                      }
                      else {
                          $(this).parent().parent().hide();

                      }
                      // return false;

                  });
              }


              if (code == 8) {
                  $("ul#documents").removeClass('show');
                  $('li.d').removeClass('highlight-menu');
                  $(this).closest('li.d').find("i.im-arrow-down2").addClass('rotate0');
                  $(this).closest('li.d').find("i.im-arrow-down2").removeClass('rotateM180');
                  $(this).closest('li.d').find("a#expand").addClass('notExpand');
                  $(this).closest('li.d').find("a#expand").removeClass('expand');
                  $('.AppCMenutask ').each(function () {
                      $(this).parent().parent().show();
                  });
                  var valThis = $(this).val().toLowerCase();
                  if ($('#role_icon').hasClass('hasTaskCode')) {
                      $('.role_code ').each(function () {
                          var text = $(this).text().toLowerCase();
                          if (text.indexOf(valThis) == 0) {
                              $(this).closest('li.d').find("ul#documents").addClass('show');
                              $(this).closest('li.d').find("a#expand").removeClass('notExpand');
                              $(this).closest('li.d').find("a#expand").addClass('expand');
                              $(this).closest('li.d').find("i.im-arrow-down2").removeClass('rotate0');
                              $(this).closest('li.d').find("i.im-arrow-down2").addClass('rotateM180');
                              $(this).closest('li.d').addClass('highlight-menu');
                          }
                          else {
                              $(this).parent().parent().hide();
                          }
                      });
                  }
                  else {
                      $('.AppCMenutask ').each(function () {
                          var text = $(this).attr('value').toLowerCase();


                          if (text.indexOf(valThis) == 0) {
                              $(this).closest('li.d').find("ul#documents").addClass('show');
                              $(this).closest('li.d').find("a#expand").removeClass('notExpand');
                              $(this).closest('li.d').find("a#expand").addClass('expand');
                              $(this).closest('li.d').find("i.im-arrow-down2").removeClass('rotate0');
                              $(this).closest('li.d').find("i.im-arrow-down2").addClass('rotateM180');
                              $(this).closest('li.d').addClass('highlight-menu');
                          }
                          else {
                              $(this).parent().parent().hide();
                          }
                      });
                  }

              }
              if (!$(this).val()) {
                  $("ul#documents").removeClass('show');
                  $('li.d').removeClass('highlight-menu');
                  $(".sideNav-arrow").addClass('rotate0');
                  $(".sideNav-arrow").removeClass('rotateM180');
                  $(this).closest('li.d').find("a#expand").addClass('notExpand');
                  $(this).closest('li.d').find("a#expand").removeClass('expand');
                  if ($('#role_icon').hasClass('hasTaskCode')) {
                      $('.role_code ').each(function () {
                          $(this).parent().parent().show();
                      });
                  }
                  else {
                      $('.AppCMenutask ').each(function () {
                          $(this).parent().parent().show();

                      });
                  }

              }
          });

          //V2002


          $(".im-arrow7").click(function () {
              if ($("#sidebar").hasClass('collapse-sidebar')) {
                  $(".role_name").show();
              }
              else {
                  $(".role_name").hide();
              }
          });

          function randomColor() {
              return '#' + (Math.floor(Math.random() * 16777216) & 0xFFFFFF).toString(16);
          }

          $("#view").click(function () {
              $("#view").toggle();
              $("#viewAlt").toggle();
              $("#search").hide();
              $("li.view").each(function () {

                  if ($(this).find("a#expand").hasClass('expand')) {
                      $(this).find("a#expand").click();
                  }
                  $(this).find("a#expand").removeClass('notExpand');
                  $(this).find("a#expand").addClass('viewExpand');
                  $(this).addClass('graphical');


              });
          });


          $("li.view").click(function () {

              $("#graphicalView").empty();

              if ($(this).hasClass('graphical')) {

                  $('.graphical').each(function () {
                      if ($(this).hasClass('highlight-menu')) {
                          $(this).removeClass('highlight-menu');
                      }
                  });
                  $(this).addClass('highlight-menu');
                  $(this).clone(true, true).addClass('innerView').appendTo("#graphicalView");

                  $('.innerView').find('ul.sub').show();
                  $('.innerView').find('a#expand').remove();
                  $('.innerView').find('input.cb').remove();
                  $('.innerView').css('list-style-type', 'none');
                  $('.innerView').find('li#drag').css('display', 'inline-block');
                  $('.innerView').find('li#drag').css('text-align', 'center');
                  $('.innerView').find('li#drag').css('padding', '12px');
                  $('.innerView').find('li#drag').find('i').css('font-size', '40px');
                  $('.innerView').find('li#drag').find('i').css('margin', '0');
                  $('.innerView').find('li#drag').find('input').css('text-align', 'center');
                  $('.innerView').find('li#drag').css('background', 'rgba(95, 123, 150, 0.63)');
                  $('.innerView').find('li#drag').css('width', '15%');
                  $('.innerView').find('li#drag').css('margin', '15px');
                  $('.innerView').find('li#drag').hover(function () { $(this).toggleClass('graphical_hover'); });
                  $('.innerView').find("i.fa").each(function () {
                      $(this).css('color', randomColor());
                  });
              }
          });

          $("a#expand").click(function () {
              if ($("#container-home").is(":visible")) {
                  if ($(this).hasClass('viewExpand')) {
                      if ($('#toggle').css('display') == 'none') {
                          $("#toggle").show("slide", { direction: 'left' }, 3000);
                          $("#toggle").addClass('toggleOpen');
                      }
                  }
              }
          });

          $("#toggleClose").click(function () {
              $("#toggle").hide("slide", { direction: 'left' }, 3000);
              $("#toggle").removeClass('toggleOpen');
          });





          $("#viewAlt").click(function () {
              $("#viewAlt").toggle();
              $("#view").toggle();
              $("#toggleClose").click();
              $("#search").show();
              $("li.view").each(function () {
                  if ($(this).find("a#expand").hasClass('viewExpand')) {
                      $(this).find("a#expand").removeClass('viewExpand');
                      $(this).find("a#expand").addClass('notExpand');
                  }

                  if ($(this).hasClass('highlight-menu')) {
                      $(this).removeClass('highlight-menu');
                  }
                  $(this).removeClass('graphical');
              });

          });



          //
          $("#role_test").click(function () {
              $('.role_code').toggle();
              //$('.AppCMenutask ').css('width', '60%');
              $('.AppCMenutask ').toggleClass("role_t");
              $('#role_icon ').toggleClass("fa fa-indent");
              $('#role_icon ').toggleClass("fa fa-outdent");
              $('#role_icon ').toggleClass("hasTaskCode");
          });

          var $window = $(window),
          flagWas = '';

          function checkWidth() {
              var windowsize = $window.width(),
              flagNow;
              if (windowsize <= 767) {
                  flagNow = 'low';
              }
              else if (windowsize >= 767 && windowsize <= 979) {
                  flagNow = 'med';
              }

              if (flagNow != flagWas) {
                  $('.intrinsic-container').removeClass(flagWas);
                  $('.intrinsic-container').addClass(flagNow);
                  flagWas = flagNow;
              }
          }
          // Execute on load
          checkWidth();
          // Bind event listener
          $(window).resize(checkWidth);

          //V2007




          function role_bookmarks() {
              var role_Elements = new Array;
              role_Elements = [];
              var role_div = document.getElementById('fav_c'),
              role_divChildren = role_div.getElementsByTagName('a');

              for (var i = 0; i < role_divChildren.length; i++) {
                  var extra = $(role_divChildren[i]).attr("id");
                  $('#' + extra + '').parent().hide();
                  var role_href = $(role_divChildren[i]).attr("href");
                  var role_id = role_href.substring(0, role_href.indexOf('/'));
                  role_Elements.push(role_id);
              }

              var role_name = $.trim($('#lblRoleName').html().split("-").pop());

              for (var j = 0; j < role_Elements.length; j++) {
                  if ($('#' + role_name + '-' + role_Elements[j] + '_task').length) {
                      $('a[href="' + role_Elements[j] + '/' + role_Elements[j] + '.aspx_' + role_name + '"]').parent().show();
                  }
              }
          }



          // End Iframe Resize //


      });

      function closePasswordModal() {
          $("#settings_Modal").fadeOut('slow', function () {
          });
      }

      function graphicalViewScript() {
          if ($("#sidebar").hasClass('collapse-sidebar')) {
              $(".role_name").hide();
          }
          else {
              $(".role_name").show();
          }


          if ($('#viewAlt').is(":visible")) {
              $("li.view").each(function () {

                  if ($(this).find("a#expand").hasClass('expand')) {
                      $(this).find("a#expand").click();
                  }
                  $(this).find("a#expand").removeClass('notExpand');
                  $(this).find("a#expand").addClass('viewExpand');
                  $(this).addClass('graphical');

              });
          }
          $("li.view").click(function () {

              $("#graphicalView").empty();

              if ($(this).hasClass('graphical')) {

                  $('.graphical').each(function () {
                      if ($(this).hasClass('highlight-menu')) {
                          $(this).removeClass('highlight-menu');
                      }
                  });
                  $(this).addClass('highlight-menu');
                  $(this).clone(true, true).addClass('innerView').appendTo("#graphicalView");

                  $('.innerView').find('ul.sub').show();
                  $('.innerView').find('a#expand').remove();
                  $('.innerView').find('input.cb').remove();
                  $('.innerView').css('list-style-type', 'none');
                  $('.innerView').find('li#drag').css('display', 'inline-block');
                  $('.innerView').find('li#drag').css('text-align', 'center');
                  $('.innerView').find('li#drag').css('padding', '12px');
                  $('.innerView').find('li#drag').find('i').css('font-size', '40px');
                  $('.innerView').find('li#drag').find('i').css('margin', '0');
                  $('.innerView').find('li#drag').find('input').css('text-align', 'center');
                  $('.innerView').find('li#drag').css('background', 'rgba(95, 123, 150, 0.63)');
                  $('.innerView').find('li#drag').css('width', '15%');
                  $('.innerView').find('li#drag').css('margin', '15px');
                  $('.innerView').find('li#drag').hover(function () { $(this).toggleClass('graphical_hover'); });
                  $('.innerView').find("i.fa").each(function () {
                      $(this).css('color', randomColor());
                  });


              }
          });
          $("a#expand").click(function () {
              if ($("#container-home").is(":visible")) {
                  if ($(this).hasClass('viewExpand')) {
                      if ($('#toggle').css('display') == 'none') {
                          $("#toggle").show("slide", { direction: 'left' }, 3000);
                          $("#toggle").addClass('toggleOpen');
                      }
                  }
              }
          });


      }


      function HideMail() {
          $("#mail_Modal").hide();
      }


      function setUploadButtonState() {

          var maxFileSize = 307200
          var fileUpload = $('#imgProUpload');
          if (fileUpload.val() == '') {
              return false;
          }
          else {

              // $('#defaultAvatar').attr('checked', 'false');
              if (fileUpload[0].files[0].size < maxFileSize) {
                  $('#btnProPicSet').prop('disabled', false);
                  $('#lblProPicValidation').text('')

                  return true;
              } else {
                  $('#lblProPicValidation').text('File must be less than 300kb')
                  return false;
              }
          }
      }


      function businessUnit_switch(bu) {
          var check = true;
          var count = $('#tabs li').size();
          if (count > 0) check = false;
          if (!check) {

              $("#bu_postback").show();
              $("#bu_ok").click(function () {
                  $("#bu_postback").fadeOut('slow', function (c) {
                  });
              });
              setTimeout(function () {
                  $('#right-sidebar').addClass('hide-sidebar');
              }, 300);
              return false;
          }
          else {
              document.getElementById('txtBusinessUnit').value = bu;
              setTimeout(function () {
                  $('#right-sidebar').addClass('hide-sidebar');
              }, 300);
              return true;
          }


      }


</script>
    
          <script>
              $(function () {
                  //$('#cp2').colorpicker();
                  var color = '';
                  $('#cp2').colorpicker().on('changeColor', function (e) {


                      var x = $('#spanColor').css('backgroundColor');
                      var hex = rgb2hex(x);


                      $("#fontColor").val(hex);

                  });
                  $('#selectLanguage').on('change', function () {
                      $('#languageSelect').val($(this).val());
                  });
                  $('#ddlFont').on('change', function () {
                      $('#fontName').val($(this).val());
                  });
                  $('#ddlFontSize').on('change', function () {
                      $('#fontSize').val($(this).val());
                  });
              });
              function rgb2hex(rgb) {
                  rgb = rgb.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
                  return (rgb && rgb.length === 4) ? "#" +
                   ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
                   ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
                   ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2) : '';
              }

              //V2007


</script>
         <script type="text/javascript">
             function ShowAlertPopup() {

                 $("#uplAlert").show();
                 $("#alert_ok").click(function () {
                     $("#uplAlert").hide();
                 });
             }

             function ApplyColor(color) {
                 if (color == 'blue') {

                     document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:dodgerblue;font-size:30px;"><span> Blue </span> </div>';

                     $('#divAddColor').css('background', 'rgba(30, 144, 255, 0.44) none repeat scroll 0% 0%');
                     $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(6, 64, 93)');
                 }
                 else if (color == 'green') {

                     document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:lightseagreen;font-size:30px;"><span> Green </span> </div>';
                     $('#divAddColor').css('background', 'rgba(32, 178, 170, 0.34) none repeat scroll 0% 0%');
                     $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(19, 131, 124)');
                 }
                 else if (color == 'gray') {

                     document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:gray;font-size:30px;"><span> Gray </span> </div>';
                     $('#divAddColor').css('background', 'rgba(128, 128, 128, 0.59) none repeat scroll 0% 0%');
                     $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(37, 41, 40)');
                 }
                 else if (color == 'purple') {

                     document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyAll();" class="fa fa-dot-circle-o" style="color:purple;font-size:30px;"><span> Purple </span> </div>';
                     $('#divAddColor').css('background', 'rgba(128, 0, 137, 0.27) none repeat scroll 0% 0%');
                     $('#divAddColor').css('box-shadow', '0px 0px 15px rgb(60, 2, 60)');
                 }
                 document.getElementById('txtTheme').value = color;
             }
             function ApplyAll() {
                 $('#divAddColor').css('background', 'rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%');
                 $('#divAddColor').css('box-shadow', '0px 0px 15px rgba(15, 11, 11, 0.5)');
                 document.getElementById('theme_colors').innerHTML = '<div onclick="ApplyColor(\'blue\');" class="fa fa-circle" style="color:dodgerblue;font-size:30px;"> </div>&nbsp;<div  onclick="ApplyColor(\'green\');" class="fa fa-circle" style="color:lightseagreen;font-size:30px;"> </div>&nbsp;<div  onclick="ApplyColor(\'purple\');"  class="fa fa-circle" style="color:purple;font-size:30px;"></div>&nbsp;<div  onclick="ApplyColor(\'gray\');"  class="fa fa-circle" style="color:gray;font-size:30px;"></div>';
             }


                 </script>
     

  

<style>
    .fullScreenParent {
        position: fixed;
        background: #f2f1ef;
        top: 0;
        left: 0;
        bottom: 0;
        right: 0;
        margin: auto;
        z-index: 999999;
        width: 100%;
        height: 100%;
        margin-top: 0 !important;
    }

    .fullScreenChild {
        position: fixed;
        left: 0;
        bottom: 0;
        right: 0;
        margin: auto;
        z-index: 999999;
        height: 97%;
        width: 100% !important;
    }
     .fullScreenPartialChild{
        position: fixed;
      
        bottom: 0;
      
        margin: auto;
     
        height: 97%;
        width: 100% !important;
    }


    .owl-prev, .owl-next {
        position: absolute;
        top: 50%;
        padding: 5px;
        margin: 0;
        z-index: 100;
        font-size: 3rem;
        cursor: pointer;
        color: #555;
    }

    .owl-prev {
        left: -60px;
    }

    .owl-next {
        right: -60px;
    }

    .owl-theme .owl-controls .owl-buttons div {
        color: #555;
        display: inline-block;
        zoom: 1;
        *display: inline; /*IE7 life-saver */
        font-size: 3rem;
        -webkit-border-radius: 0px;
        -moz-border-radius: 0px;
        border-radius: 0px;
        background: transparent;
        filter: Alpha(Opacity=100); /*IE7 fix*/
        opacity: 1;
        margin-top: -32px;
    }

        .owl-theme .owl-controls .owl-buttons div:hover {
            color: #2a6496;
        }

    /*//new*/
    .graphical_hover {
        background: rgba(255, 255, 255, 0.631373) !important;
    }

    .colorpicker-visible {
        z-index: 99999999999999999 !important;
    }


    #theme_colors div {
        cursor: pointer;
    }

    * {
        box-sizing: border-box;
    }

    .slider {
        width: 50%;
        margin: 100px auto;
    }

    .slick-slide {
        margin: 0px 20px;
    }

        .slick-slide img {
            width: 100%;
        }

    .slick-prev:before,
    .slick-next:before {
        color: black;
    }


    /**/
    .role_t {
        width: 60% !important;
    }

    #sidebar .sidebar-inner .option-buttons .option-buttons-inner .btn {
        margin: 0 !important;
    }

    .panel-controls a {
        text-decoration: none;
        float: left;
        width: auto;
        padding: 8px 4px 7px;
    }

    .panel-controls {
        width: auto;
        float: right;
        right: 10px;
        top: 0;
    }

    .todo-task-item {
        width: 90% !important;
        position: absolute !important;
        padding: 8px 0 !important;
        padding-left: 10px !important;
    }

    .checkbox_style input {
        margin-top: 15px;
    }
    /* reminders related style above*/
    .loader {
        position: absolute;
        z-index: 200;
        width: 100%;
        height: 80%;
    }

    .cbx_css {
        display: none;
        float: left;
    }

    .error {
        color: #d2232a;
        -webkit-border-radius: 12px;
        border-radius: 12px;
        background-color: #d8d6d3;
        padding: 5px;
        width: 100%;
    }

    .current_min {
        width: calc(50% - 128px) !important;
    }
    .current_min_fullscreen {
        width: calc(50%) !important;
        left:initial;
    }

    .current_min1 {
        width: calc(50% - 33px) !important;
    }

    .med1 {
        width: calc(50% - 33px) !important;
    }
    .med1_fullscreen {
        width: calc(50%) !important;
     
    }
    .d_block {
        display: block !important;
    }

    .current_min_second {
        right: 0 !important;
    }

    #s::-ms-clear {
        display: none;
    }

    .slimScrollDiv {
        top: 11px !important;
    }

    .profile_checkbox input[type="checkbox"] {
        margin-left: 5px;
    }

    #defaultAvatar {
        visibility: hidden;
    }
    /* test styles */

    .defaultAvatar input[type=checkbox]:checked + label:after {
        background: #00bf00;
    }

    .defaultAvatar {
        float: right;
        width: 38px;
        height: 10px;
        background: #7E7E7E;
        margin: 20px auto;
        -webkit-border-radius: 50px;
        -moz-border-radius: 50px;
        border-radius: 50px;
        position: relative;
        -webkit-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,0.2);
        -moz-box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,0.2);
        box-shadow: inset 0px 1px 1px rgba(0,0,0,0.5), 0px 1px 0px rgba(255,255,255,0.2);
    }

        .defaultAvatar label {
            display: block;
            width: 16px;
            height: 16px;
            -webkit-border-radius: 50px;
            -moz-border-radius: 50px;
            border-radius: 50px;
            -webkit-transition: all .4s ease;
            -moz-transition: all .4s ease;
            -o-transition: all .4s ease;
            -ms-transition: all .4s ease;
            transition: all .4s ease;
            cursor: pointer;
            position: absolute;
            top: -3px;
            left: -3px;
            -webkit-box-shadow: 0px 2px 5px 0px rgba(0,0,0,0.3);
            -moz-box-shadow: 0px 2px 5px 0px rgba(0,0,0,0.3);
            box-shadow: 0px 2px 5px 0px rgba(0,0,0,0.3);
            background: #fcfff4;
            background: -webkit-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
            background: -moz-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
            background: -o-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
            background: -ms-linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
            background: linear-gradient(top, #fcfff4 0%, #dfe5d7 40%, #b3bead 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fcfff4', endColorstr='#b3bead',GradientType=0 );
        }

            .defaultAvatar label:after {
                content: '';
                position: absolute;
                width: 12px;
                height: 12px;
                -webkit-border-radius: 50px;
                -moz-border-radius: 50px;
                border-radius: 50px;
                background: #DEDADA;
                left: 2px;
                top: 2px;
                -webkit-box-shadow: inset 0px 1px 1px rgba(0,0,0,1), 0px 1px 0px rgba(255,255,255,0.9);
                -moz-box-shadow: inset 0px 1px 1px rgba(0,0,0,1), 0px 1px 0px rgba(255,255,255,0.9);
                box-shadow: inset 0px 1px 1px rgba(0,0,0,1), 0px 1px 0px rgba(255,255,255,0.9);
            }

        .defaultAvatar input[type=checkbox]:checked + label {
            left: 29px;
        }
</style>
 
</head>

<body class="ovh" onbeforeunload="HandleClose();" onload="autoCSS();">
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <input type="hidden" id="txtCurrentTaskCode" runat="server" />
    <input type="hidden" id="txtBusinessUnit" runat="server" />
  <input type="hidden" id="userId" runat="server" />
      <input id="txtTheme" type="hidden" runat="server" value="blue" />
     <input type="hidden" id="fontColor" runat="server" />
     <input type="hidden" id="fontName" runat="server" />
     <input type="hidden" id="fontSize" runat="server" />
     <input type="hidden" id="languageSelect" runat="server" />
    

 <div style="display:none;" id="video1">
    <video class="lg-video-object lg-html5" controls preload="none">
        <source src="./lightGallery-master/demo/videos/infocruiser_tutorial.mp4" type="video/mp4">
         Your browser does not support HTML5 video.
    </video>
</div>
 

   
    <!-- Settings modal -->
    
    <div id="settings_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
    <div>	
		<div class="s-content" style="height:390px;width:65%"> 
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> Settings </h4><a  id="setting_close" class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px; color:#fff;margin-top:-30px;text-decoration:none;"></a></div>
            <div style="height:70%;width:90%;margin-left:5%;">
		<%--<div class="s-body" style="padding-bottom:20px;width:50%;height:100%;float:left;border-right:1px solid rgb(149, 165, 166);">--%>
				
    <section id="owl-example" class="owl-carousel" style="margin-top:10px;padding-top:10px;">
       <%-- <table>
            <tr>
                <td style="padding:10px;">--%>
        <%--background:rgba(128, 128, 128, 0.11) none repeat scroll 0% 0%;--%>
   <div style="margin-top:10px;">
		  <div id="divAddColor" style="position: relative; width: 200px; top: 4px; height: 200px; background: rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%; padding: 10px; margin: 0px 5px 5px; box-shadow: 0px 0px 15px rgba(15, 11, 11, 0.5);">
                <h4 class="s-title" style="font-weight:bold;">Themes </h4>
              <div id="theme_colors" style="padding:30px;">
           <div onclick="ApplyColor('blue');" class="fa fa-circle" style="color:dodgerblue;font-size:30px;"> </div>
           <div  onclick="ApplyColor('green');" class="fa fa-circle" style="color:lightseagreen;font-size:30px;"> </div>
           <div  onclick="ApplyColor('purple');"  class="fa fa-circle" style="color:purple;font-size:30px;"></div>
            <div  onclick="ApplyColor('gray');"  class="fa fa-circle" style="color:gray;font-size:30px;"></div>
                 </div>   </div>

	
    </div>
                  <%--  </td>
                <td style="padding:10px;">--%>
    <div style="margin-top:10px;">
         <div style="position: relative; width: 200px; top: 4px; height: 200px; background: rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%; padding: 10px; margin: 0px 5px 5px; box-shadow: 0px 0px 15px rgba(15, 11, 11, 0.5);">
     
			<h4 class="s-title" style="font-weight:bold;">Profile Picture</h4>
			     <asp:FileUpload ID="imgProUpload" ToolTip="Select a jpg or png file for your profile picture" runat="server" style="width:190px;height:30px;"/>
        
                <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage= "jpg/jpeg/png file types only!"
                        ValidationExpression ="^.+(.jpg|.JPG|.JPEG|.jpeg|.png|.PNG)$" ControlToValidate="imgProUpload"></asp:RegularExpressionValidator>

               <asp:CustomValidator ID="customValidatorUpload" runat="server" ErrorMessage="Less than 300Kb files only!" ControlToValidate="imgProUpload" ClientValidationFunction="setUploadButtonState();" />
               <asp:Label runat="server" ID="lblProPicValidation" ForeColor="Red" EnableViewState="false"></asp:Label>
         
             <div>
             <div class="defaultAvatar">
                 <asp:CheckBox ID="defaultAvatar" runat="server" />
	
	<label for="defaultAvatar"></label>
</div> <span style="float:right;padding:10px;">Default Avatar</span>

             </div>	

             </div>
		
    </div>
                   <%--  </td>
                <td style="padding:10px;">--%>
    <div id="font-block" style="margin-top:10px;">
         <div  style="position: relative; width: 200px; top: 4px; height: 200px; background: rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%; padding: 10px; margin: 0px 5px 5px; box-shadow: 0px 0px 15px rgba(15, 11, 11, 0.5);">
        	<h4 class="s-title" style="font-weight:bold;">Fonts</h4>
             <table>
                  <tr>
                      
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Size" CssClass="settingsfont"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlFontSize" runat="server" CssClass="form-control" style="height:29px;padding:2px;" Width="50px">
                    </asp:DropDownList>
                </td>
                </tr>
                   <tr>
                <td>
                    <asp:Label ID="lblFont" runat="server" Text="Font" CssClass="settingsfont"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlFont" runat="server" CssClass="form-control"  style="width:150px;height:29px;padding:2px;">
                    </asp:DropDownList>
                </td>
                </tr>
               <%--  </table>
             <table>--%>
                 <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="settingsfont" Text="Color"></asp:Label>
                </td>
                <td>
              <%--  <input type="text" id="colorBackgroundColor"  runat="server"
                        maxlength="7" style="width: 70px"/>--%>
                   <%-- <input id="colorBackgroundColor" runat="server"  class="Textboxstyle"  
                        maxlength="7" style="width: 80px" />--%>
                   <div id="cp2" class="input-group colorpicker-component">
                                       
                  <input id="colorBackgroundColor" runat="server"  type="text" value="#00AABB" class="form-control" style="height:29px;padding:2px;" />
                 <span class="input-group-addon" style="height: 29px; width: 30px;padding:0;" ><i id="spanColor" runat="server"></i></span>
                </div>

                </td>
                
            </tr>
             </table>
  </div>
    </div>
                   <%--   </td>
                <td style="padding:10px;">--%>
    <div style="margin-top:10px;">
         <div style="position: relative; width: 200px; top: 4px; height: 200px; background: rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%; padding: 10px; margin: 0px 5px 5px; box-shadow: 0px 0px 15px rgba(15, 11, 11, 0.5);">
        <h4 class="s-title" style="font-weight:bold;">Language</h4>
             <table>
                 <tr>
                       <td class="" colspan="9">
                    <select id="selectLanguage" name="D1" runat="server" class="form-control" style="height:29px;padding:2px;width: 174px;">
                        <option>English</option>
                        <option>Sinhala</option>
                        <option>Tamil</option>
                    </select>
                </td>
                 </tr>
             </table>
     </div>
        
    </div>
                    <%--  </td>
                <td style="padding:10px;">--%>
         <div style="margin-top:10px;">
         <div style="width:280px;position: relative; top: 4px; height: 200px; background: rgba(128, 128, 128, 0.27) none repeat scroll 0% 0%; padding: 10px; margin: 0px 5px 5px; box-shadow: 0px 0px 15px rgba(15, 11, 11, 0.5);">
        <h4 class="s-title" style="font-weight:bold;">Change Password</h4>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanelPassword" >
         <Triggers>
            <asp:AsyncPostBackTrigger controlid="changePassword" eventname="Click" />
        </Triggers>
         <ContentTemplate>
           <table>
                  <tr>
                      
                <td>
                    <asp:Label ID="currentPswd" runat="server" Text="Current Password" CssClass="settingsfont"></asp:Label>
                </td>
                <td>
                   
                    <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"
                    EnableViewState="true" CssClass="form-control changePswd" style="width:100px;height:29px;padding:2px;"></asp:TextBox>
                      <%--<input id="txtCurrentPassword" runat="server"  type="password" enableviewstate="true"  maxlength="10" class="changePswd" style="width:100px;height:29px;padding:2px;" />--%>

                </td>
                </tr>
                   <tr>
               <td>
                    <asp:Label ID="newPswd" runat="server" Text="New Password" CssClass="settingsfont"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"
                    EnableViewState="true" CssClass="form-control changePswd" style="width:100px;height:29px;padding:2px;"></asp:TextBox>
                     <%--<input id="txtNewPassword" runat="server"  type="password"  maxlength="10" enableviewstate="true" class=" changePswd" style="width:100px;height:29px;padding:2px;" />--%>
                </td>
                </tr>
             
                 <tr>
               <td style="padding-right:10px;">
                    <asp:Label ID="confirmNewPswd" runat="server" Text="Re-type New Password" CssClass="settingsfont"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password"
                    EnableViewState="true" CssClass="form-control changePswd" style="width:100px;height:29px;padding:2px;"></asp:TextBox>
                     <%--<input id="txtConfirmNewPassword" runat="server"  type="password"  maxlength="10" class=" changePswd" style="width:100px;height:29px;padding:2px;" />--%>
                </td>
             </tr>
             </table>
             <table>
               <tr>
                                    <td>
                                        <asp:Label ID="lblPasswordError" runat="server" Text="" CssClass="Errormessagetextstyle"></asp:Label>
                                        <asp:CompareValidator ID="comparePassword" CssClass="Errormessagetextstyle" 
                                            runat="server" ValidationGroup="valChangePassword" 
                                            ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword"
                                            ErrorMessage="Passwords do not match....." Display="Dynamic"></asp:CompareValidator>
                                        
                                       
                                    </td>
               </tr>
             </table>
             </ContentTemplate>
            </asp:UpdatePanel>
     </div>
    </div>
                    <%--</td>
                </tr>
        </table>--%>
     </section>
            
            
           
  
</div>
            <div class="s-footer">
                <asp:LinkButton class="btn btn-primary" id="changePassword" ValidationGroup="valChangePassword" Text="Change Password" OnClick="btn_changePassword" runat="server" style="display:none;"></asp:LinkButton>
                <asp:LinkButton class="btn btn-primary" id="setDefault" CausesValidation="false" Text="Set to Default" OnClick="saveSettingDefault" runat="server"></asp:LinkButton>
		<asp:LinkButton class="btn btn-primary" id="ok" CausesValidation="false" Text="OK" OnClick="saveSetting" runat="server"></asp:LinkButton>
		</div>  
                </div>
		
		</div></div>
   

<%--<div id="settings_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
    <div>	
		<div class="s-content" style="height:370px;width:50%"> 
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> Settings </h4></div>
            <div style="height:70%;">
		<div class="s-body" style="padding-bottom:20px;width:50%;height:100%;float:left;border-right:1px solid rgb(149, 165, 166);">
			<h4 class="s-title" style="font-weight:bold;">Themes </h4>
		
			<select name="style" id="style" runat="server" onChange="changeCSS();">
                <option id="select" value="green" selected="selected" style="display:none" >Select Theme</option>
                <option id="blue" value="blue">Blue</option>
				<option id="green" value="green">Green</option>
                <option id="gray" value="gray">Gray</option>
                <option id="purple" value="purple">Purple</option>
				
			</select>  
  
</div>
              
            <div class="s-body" style="padding-bottom:20px;width:50%;height:100%;float:left;border-right:1px solid rgb(149, 165, 166);overflow:auto;">
			<h4 class="s-title" style="font-weight:bold;">Profile Picture</h4>
			     <asp:FileUpload ID="imgProUpload" ToolTip="Select a jpg or png file for your profile picture" runat="server" />
        
                <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage= "jpg/jpeg/png file types only!"
                        ValidationExpression ="^.+(.jpg|.JPG|.JPEG|.jpeg|.png|.PNG)$" ControlToValidate="imgProUpload"> </asp:RegularExpressionValidator>
               <asp:CustomValidator ID="customValidatorUpload" runat="server" ErrorMessage="Less than 300Kb files only!" ControlToValidate="imgProUpload" ClientValidationFunction="setUploadButtonState();" />
               <asp:Label runat="server" ID="lblProPicValidation" ForeColor="Red" EnableViewState="false"></asp:Label>

		    </div>
                

                </div>
		<div class="s-footer">
		<asp:LinkButton class="btn btn-primary" id="ok" CausesValidation="false" Text="OK" OnClick="saveSetting" runat="server"></asp:LinkButton>
		</div>
		</div>
    </div>
</div>--%>

    <!-- Mail modal -->
<div id="mail_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
       <div>  
              
                     <iframe id="Iframe1" style="border: 0px; 
             padding: 0px; border-style: none;top:0;bottom:0;left:0;right:0;position:absolute;border-width: 0px; margin: auto; width: 450px;
             height: 380px"></iframe>
            
    </div>
</div>

     <!-- Multimedia modal -->
 <div id="multimedia_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		
			<iframe id="Iframe1Multimeadia"  style="border: 0px; 
             padding: 0px; border-style: none;top:0;bottom:0;left:0;right:0;position:absolute;border-width: 0px; margin: auto; width: 100%;
             height: 100%;"></iframe>
		
    </div>
</div>



<!-- About modal -->
 <div id="about_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:335px;">
      
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> About Ventura</h4></div>
		<div class="s-body">
	

		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="about_close">OK</a>
		</div>
		</div>
    </div>
</div>

<!-- Close confirmation Modal -->
    <div id="close_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:145px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Confirm</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Are you sure you want to close this window ?</div>
		</div>
		<div class="s-footer">
            <a class="btn btn-default" id="close_cancel" style="float:right;margin-top:10px;">Cancel</a>
		<a class="btn btn-primary" id="close_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
        </div>
		</div>
    </div>
</div>

  <!-- Maximum tab exceeded modal --> 
    <div id="max_tab" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:175px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">You Have Exceeded the Maximum Number of Tabs,Close One to Open Another !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="max_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

      <div id="cmp_tab" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:175px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Select Two Tabs for Compare mode !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="cmp_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

    <!-- Maximum favoritestab exceeded modal --> 
 <div id="max_fav" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
 <div>	
		<div class="s-content" style="height:145px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Sorry! You Have Exceeded the Maximum Number of Favorites ,Delete One to Save Another !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="max_favok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

<!-- tab switch notes alert --> 
 <div id="note_alert" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
 <div>	
		<div class="s-content" style="height:145px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Please close the component specific note before switching tabs.</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="note_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

<!-- Logout Alert modal --> 
    <div id="logout_modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:145px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Please close all tabs before exiting !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="logout_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>


<!-- Access Settings Modal -->
      <div id="settings_postback" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:145px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Please close all tabs before accessing Settings !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="postback_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

<!-- Access BusinessUnit Modal -->
      <div id="bu_postback" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:145px;width:500px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;">Alert!</h4></div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;">Please close all tabs before changing BusinessUnit !</div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="bu_ok" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
		</div>
		</div>
    </div>
</div>

<!-- CRM modal -->
 <div id="crm_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:570px;width:95%;">
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> CRM <a  id="crm_close" class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px; color:#fff;"></a></h4></div>
		<div class="s-body">
		<iframe src="http://x-ontcrm.xontworld.com" style="width:100%;height:492px;background-color:#fff;margin-top:10px">
		    </iframe>
		</div>
		
		</div>
    </div>
</div>
 <!-- Help modal -->
 <div id="help_Modal" class="modalDialog2 fade in" style="display:none;position:relative;height:100%;">
	<div>	
		<div class="s-content" style="height:570px;width:95%;">
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> Help Menu </h4>
                	<a style="font-size:20px;float:right;text-decoration:none;cursor:pointer;margin-top:-20px;" class="fa fa-close" id="help_close"></a></div>
		<div class="s-body" >
			<iframe src="HelpFiles/HelpContents.aspx" style="width:100%;height:492px;background-color:#fff;margin-top:10px">
		    </iframe>
		
		</div>
		</div>
    </div>
</div>
      <%--  <!-- Reminders Triggered Popup -->
     <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uplReminderTriggered" style="display:none;">
         <ContentTemplate>
    <div id="reminder_triggered_Modal" class="modalDialog2 fade in" style="position:relative;height:100%;">
       
	<div>	
		<div class="s-content" style="height:23%;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;"><asp:Label runat="server" ID="lblReminderName" ></asp:Label>   <asp:LinkButton runat="server" ID="reminder_ok"  OnClick="lbtnReminderDispose_Click" class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px; color:#fff;"></asp:LinkButton></h4>
         
		</div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;"> <asp:Label runat="server" ID="lblReminderTriggered"></asp:Label></div>
		</div>
		<div class="s-footer">
			<a class="btn btn-primary" id="" style="float:right;margin-top:10px;margin-right:10px;">OK</a>
            <asp:LinkButton ID="lbtbRemindLater" runat="server" OnClick="lbtnRemindLater_Click" Text="Remind me later" ToolTip="Remind me 10 minutes later"></asp:LinkButton>
		</div>
		</div>
    </div>
        </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger  ControlID="tmrReminder" EventName="Tick"/>
                
            </Triggers>

        </asp:UpdatePanel>--%>
      <!-- Reminders Triggered Popup -->
     <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uplReminderTriggered" style="display:none;">
         <ContentTemplate>
    <div id="reminder_triggered_Modal" class="modalDialog2 fade in" style="position:relative;height:100%;">
       
	<div>	
		<div class="s-content" style="height:21%;min-width:550px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;"><span class="fa fa-bell"></span>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblReminderName"></asp:Label> 
               </h4>
         
		</div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;"> <asp:Label runat="server" ID="lblReminderTriggered"></asp:Label></div>
		</div>
		<div class="s-footer">
            <asp:LinkButton ID="lbtbRemindLater" style="float:left;margin-top:10px;margin-right:10px;cursor:pointer;padding-left:5px !important;padding-right:5px !important;padding:0;" class="btn btn-primary" runat="server" OnClick="lbtnRemindLater_Click" Text="Remind me again in 10 minutes" ToolTip="Remind me again in 10 minutes"></asp:LinkButton>
          <asp:LinkButton runat="server" ID="reminder_ok"  class="btn btn-primary" OnClick="lbtnReminderDispose_Click" Text="OK" style="cursor:pointer;float:left;margin-top:10px;margin-right:10px;padding-left:5px !important;padding-right:5px !important;padding:0;"></asp:LinkButton>
             </div>
		</div>
    </div>
        </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger  ControlID="tmrReminder" EventName="Tick"/>
                
            </Triggers>

        </asp:UpdatePanel>
        


    <!-- Admin Alert Popup -->
    <audio id="myTune" controls loop style="display:none;">      
<source src="adminAlert.mp3" type="audio/mpeg">
</audio>
     <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uplAlert" style="display:none;">
         <ContentTemplate>
    <div id="admin_alert_Modal" class="modalDialog2 fade in" style="position:relative;height:100%;">
       
	<div>	
		<div class="s-content" style="height:21%;min-width:550px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;"><span class="fa fa-bell">ALERT</span>&nbsp;&nbsp;
               </h4>
         
		</div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;"> <asp:Label runat="server" ID="lblAlertMessage"></asp:Label></div>
		</div>
		<div class="s-footer">     
          <asp:LinkButton runat="server" ID="alert_ok"  class="btn btn-primary"  Text="OK" style="cursor:pointer;float:left;margin-top:10px;margin-right:10px;padding-left:5px !important;padding-right:5px !important;padding:0;"></asp:LinkButton>
             </div>
		</div>
    </div>
        </div>
            </ContentTemplate>
           

        </asp:UpdatePanel>


    <!-- End -- >


 <!-- Sticky Notes Modal-->
 <div class="note-container" id="note_Modal" style="display:none;top:35% !important;z-index:2000;">
    <asp:UpdatePanel runat="server" ID="uplNoteBody" UpdateMode="Conditional"> 
        <ContentTemplate>     
<div class="s-content note" style="cursor:move;padding:5px; border-style: solid;border-width: 1px; border-top:0px;">
		<div class="s-header" style="padding:0; text-align:center;">
			<h4 class="s-title" style="font-weight:bold;"> Notes <asp:LinkButton ID="note_close" OnClick="btnNoteCloseClick" runat="server"><i class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px; color:#fff;"></i></asp:LinkButton></h4>
		</div>
		<div class="s-body" id="note_body" style="padding:0;margin-bottom: -10px;">
		</div>
		
		</div>
             
        </ContentTemplate>
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="sticky_note" EventName="Click" />
        </Triggers>
		</asp:UpdatePanel>
     </div>
	 
<!-- Comparison -->

<%--<div id="compare_Modal"  style="display:none;position:absolute;height:72%;width:100%;margin-top:12%;">
	<div>	
	
	
		<div class="s-body">
		<iframe id="frame1" src="" style="width:50%;height:450px;float:left;border:solid 1px gray;" ALLOWTRANSPARENCY="true">
		    </iframe>
		    <iframe id="frame2" src="" style="width:50%;height:450px;float:right;border:solid 1px gray;"  ALLOWTRANSPARENCY="true">
		    </iframe>
		</div>
		
	
    </div>
</div>--%>

<!-- Main Header -->
<div id="header">
            <div class="container-fluid">
                <div class="navbar">
                    <div class="navbar-header">


                        <%--</asp:LinkButton>--%>
                        <!-- Show navigation toggle on phones -->
                        <button id="showNav" class="navbar-toggle" type="button">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <!-- Show logo for large screens and laptops -->
                        <a class="navbar-brand visible-lg visible-md"> 
                        <img src="images/logo-v2.png" alt="VENTURA CRM" style="width: 185px;">
                        </a>
                        <!-- Show logo for small screens -->
                        <a class="navbar-brand hidden-lg hidden-md hidden-xs">
                        <img src="images/logo-v2.png" alt="VENTURA CRM" style="width: 185px;">
                        </a>
                        <%--<input id="currentBU" runat="server" />--%>
                        <span style="color:white;line-height:52px;font-weight:900;font-size:17px;" class="fa fa-database">&nbsp;&nbsp;&nbsp;<span  runat="server" id="currentBU"></span>
        <%--/v2007--%>
                            <asp:Label ID="lbNames" runat="server" Text=""></asp:Label>
       </span>
                    </div>
                    <nav id="top-nav" class="navbar-no-collapse" role="navigation">
                   
                        <ul class="nav navbar-nav pull-right">
                           <%-- <li class="hidden-lg hidden-md">
                                <!-- close button for search form in small screens -->
                                <button type="button" class="close closeSearchForm" aria-hidden="true">&times;</button>
                                <!-- show search button in small screens -->
                                <a class="resSearchBtn hidden-lg hidden-md" href="#"><i class="im-search3"></i></a>
                            </li>--%>
                        
                            <li class="dropdown">
                           
                                <a href="#" data-toggle="dropdown" style="display:block !important;">
                                   <img id="profile_image" class="avatar" runat="server" src="assets/img/avatars/avatar.png" width="36" height="36">&nbsp;<span class="avatar-name" runat="server" id="spUserName"></span>
                                    <span class="caret ml5"></span>
                                </a>
                                <ul class="dropdown-menu right" role="menu">
                                    <%--<li><a href="#"><i class="im-user"></i> Profile</a>
                                    </li>--%>
                                    <li><a href="#" id="settings-popup" class="settings_open"><i class="im-cog2"></i>Settings</a>
                                    </li>
                                    <li><a id="about" style="cursor:pointer;"><i class="fa fa-copyright"></i> About</a>
                                    </li>
                                    <li><asp:LinkButton ID="logout" OnClick="btnLogout_Click" runat="server"><i class="fa fa-power-off"></i> Logout</asp:LinkButton>
                                    </li>
                                </ul>
                            </li>
                            <li style="float:right;">
                                <a id="toggle-right-sidebar" href="#" title="User Roles/Business Unit" style="display:block !important;">
                                    <i class="fa fa-cogs"></i>
                                   <span class="sr-only">Maintenance</span>
                                </a>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
</div>

<!-- left sidebar -->

		<aside id="sidebar" style="font-weight:300;font-size:13px;">
            <div class="sidebar-inner">
                <div class="toggle-sidebar">
                    <a href="#"><i class="im-arrow7"></i></a>
                     <div id="alt" class="toggle_div"><i class="im-arrow6 toggle_i"></i></div>
                </div>
                <div class="option-buttons">
                    <div class="option-buttons-inner">   
                     <a href="#" id="expand" style="padding:5px 10px;" class="expand-subs btn btn-default btn-sm tip" title="Expand all SubMenus"><i class="im-sort2"></i></a>
                        <a href="#" style="padding:5px 10px;;" id="edit" onclick="hideSelected();" class="edit-navigation btn btn-default btn-sm tip" title="Edit Navigation"><i class="im-pencil"></i></a>
						<a href="#" style="display:none;padding:5px 10px;" id="save" class="edit-navigation btn btn-default btn-sm tip" title="Save"><i class="fa fa-floppy-o" style="padding:0px 0px 0px 1.7px"></i></a>
                        <%--<a href="#" id="reset" style="padding:5px 10px;" class="reset-layout btn btn-default btn-sm tip" title="Reset Panels Position"><i class="im-spinner12"></i></a>--%>
                         <%--<asp:LinkButton ID="view" style="text-decoration:none;padding:5px 10px;font-size:18px;" runat="server" OnClick="btnView_Click" CssClass="fa fa-th-large btn btn-default btn-sm tip  " ToolTip="Graphical View"></asp:LinkButton>--%>
                         <a href="#" id="view" style="padding:5px 10px;" class="btn btn-default btn-sm tip" title="Graphical View"><i class="fa fa-th-large"></i></a>
                         <a href="#" id="viewAlt" style="padding:5px 10px;display:none;" class="btn btn-default btn-sm tip" title="Classic View"><i class="fa fa-list-ul"></i></a>
                         <a href="#" id="fullscreen" style="padding:5px 10px;" class="launch-fullscreen btn btn-default btn-sm tip" title="Launch Full Screen"><i class="im-expand"></i></a>
                           <a href="#" id="role_test" style="padding:5px 10px;" class="btn btn-default btn-sm tip" title="Show Task Code"><i id="role_icon" class="fa fa-indent"></i></a>
                        <a href="#" id="search" style="padding:5px 10px;"  class=" btn btn-default btn-sm tip" title="Search Menu"><i class="fa fa-search"></i></a>
                        <asp:LinkButton ID="search_close" style="text-decoration:none;display:none;padding:5px 10px;float:right;margin-right:10px;" runat="server" OnClick="btnSearchClose_Click" CssClass="fa fa-close" ToolTip="Close"></asp:LinkButton>
                        <input style="display:none;width:193px;height:30px;border:2px solid gray;border-radius:5px;" placeholder="Search..." id="s" type="text" autocomplete="off"/>
                      </div>
                </div>
                <div class="sidebar-scrollarea">

                           


              <asp:UpdatePanel runat="server" ID="uplMenuTasks" UpdateMode="Conditional"> 

                    <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="repUerRoles" EventName="ItemCommand" />
                     <asp:AsyncPostBackTrigger ControlID="search_close" EventName="Click" />
                   <%--<asp:AsyncPostBackTrigger ControlID="chk_box" EventName="CheckedChanged" />--%>
                    </Triggers>

                    <ContentTemplate>
                     <%--   <h5 class="sidebar-title">
                            <asp:Label ID="lblRoleName" runat="server" Text=""></asp:Label> </h5>--%>
                    <% =_navigation%>
                   </ContentTemplate>

                       
              </asp:UpdatePanel>

                     <input id="txtTaskCode" type="hidden" runat="server" value=""/>
					<%--<div id="nav">
						<div id="dropme"></div> 
					</div>--%>
                    <div class="clearfix"></div>
                </div>
            </div>
        </aside>

<!-- right sidebar -->		
		 <aside id="right-sidebar" class="hide-sidebar" style="opacity: 0.95; filter: alpha(opacity=95);">
            <div class="sidebar-inner" style="background: rgba(70,86,100,1);">
                <div class="sidebar-panel mt0">
                    <div class="sidebar-panel-content fullwidth pt0">
                        <div class="chat-user-list">
                        
                        <section>
	<div class="tabs tabs-style-topline">
			<div style="height:4px;border-bottom:1px solid rgba(40,44,42,0.1);"></div>
			<div>
				<nav>
					<ul>
						<li  id="tab1" class="t_sort"><a class="source" href="#section-topline-1"><span>User Roles</span></a></li>
						<li  id="tab2" class="t_sort"><a class="source" href="#section-topline-2"><span>Business Unit</span></a></li>
						
					</ul>	
				</nav>
			</div>
			<div style="width:1%;float:right;border-bottom:1px solid rgba(40,44,42,0.1);"></div>	
				
			<div id="print-content" class="content-wrap">
				<section id="section-topline-1">
                     <div class="chat-write" style="margin-top:10px;">
                <ul id="sideNav" class="nav nav-pills nav-stacked" style="background: rgba(70,86,100,1);">
                 <%--   <asp:UpdatePanel runat="server" ID="uplRoles">
                        <ContentTemplate>--%>
                   
                  <asp:Repeater ID="repUerRoles" runat="server" OnItemDataBound="itemBound"> 
                     <ItemTemplate>
                        <li id="drag"  class="d e drag1"> 
							<a href="#" style="background: transparent;width:91% !important;">
                                <i style="font-size:14px;" class="fa fa-home"></i> 
                                <span class="txt">                   
                                      <asp:Button CssClass="role_bookmark" style="width:90%;text-align:left; background: transparent; border:0px; color: #F2F1EF;" ID="BtnUserRole" runat="server" 
                                        CommandName='<%# DataBinder.Eval(Container.DataItem,"RoleCode") %>' UseSubmitBehavior="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Description") %>'
                        Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' Enabled="false" ToolTip='<%# DataBinder.Eval(Container.DataItem,"Description") %>' />
                                    
                                     </span>
  
							</a>
                             <asp:CheckBox ID="chk_box" runat="server"  AutoPostBack="True" Checked="false" OnCheckedChanged="Check_Clicked" CssClass="role_checkbox"/>
								</li>
                                  
			                 <asp:HiddenField ID="btn_id" runat="server" Value ='<%# DataBinder.Eval(Container.DataItem,"RoleCode") %>' />     
                                <asp:HiddenField ID="btn_desc" runat="server" Value ='<%# DataBinder.Eval(Container.DataItem,"Description") %>' />                   

                        </ItemTemplate>
                 </asp:Repeater>
                    		
                            <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                   
                </ul>
					
                        </div>
				</section>

<section id="section-topline-2">
    <div class="chat-write" style="margin-top: 10px;">				
        <ul id="sideNav" class="nav nav-pills nav-stacked">	
              <asp:Repeater runat="server" ID="bUnit">	
           <ItemTemplate>	
            <li id="drag" class="d e drag1">				
                <a href="#">				
                    <i style="font-size: 14px;" class="fa fa-home"></i>				
                     <span class="txt">                      
                         <asp:Button CssClass="role_bu" style="width:90%;text-align:left; background: transparent; border:0px; color: #F2F1EF;" ID="BtnBUnit" runat="server" 
                         UseSubmitBehavior="false" Text='<%# DataBinder.Eval(Container.DataItem,"BusinessUnit") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"BusinessUnit") %>' OnClick="btnBUnit_Click"   OnClientClick="if ( ! businessUnit_switch(this.value)) {return false;}"  />
                                </span>			                                                                                                                                                                                                                    
                </a>				
            </li>		  </ItemTemplate>		
           	       </asp:Repeater>		
        </ul>				
    </div>				
</section>               
</div>
</div>
</section>

                        </div>
                    </div>
                </div>
            </div>
        </aside>
		


  
		
<!-- Main Body -->		
		   <div id="content">
            <div class="content-wrapper">
			<!-- Header -->	
                <div id="page-heading" class="page-header">
                <h2><%--<i class="icon im-home2"></i>--%>
		        <div style="float:right;">
					   
					<div style="float:left"><a href="#" id="home-toggle" style="display:none; background-color:#F2F1EF;" class="btn btn-default btn-sm tip" title="Home"><i class="im-home"></i></a></div>
                    <div style="float:left"><a href="#" id="fullScreentabs" style="display:none; background-color:#F2F1EF;" class="btn btn-default btn-sm tip" title="Full Screen"><i class="fa fa-arrows-alt"></i></a></div>
					<div id="tools_container" style="float:right;display:inline-block; margin-left:2px;">
						<a href="#" id="tools" style="display:none;" class="btn btn-default btn-sm tip"><i class="im-tools"></i></a>
					    <div id="tools_Modal"  class="notesdisplay3" style="display:none;position:absolute;z-index:100;">
                        <div>
		                    <a href="#" id="print" style="display:none;color:#fff;" class="btn btn-sm tip "  title="Print"><i class="im-print"></i></a>
                            <a href="#" id="mail" style="display:none;color:#fff;" class="btn btn-sm tip" title="Mail"><i class="fa fa-envelope"></i></a>           
					    	<a href="#" id="fav" style="display:none;color:#fff;" class="btn btn-sm tip" title="Add to Favorite"><i class="fa fa-bookmark"></i></a> 
                            <asp:LinkButton OnClick="btnNoteClick" ID="sticky_note" style="display:none;color:#fff;" runat="server" class="btn btn-sm tip"><i class="fa fa-edit" title="Sticky Notes"></i></asp:LinkButton>
						    <%--<a href="#" id="sticky_note" style="display:none;color:#fff;" class="btn btn-sm tip" title="Sticky Notes"><i class="fa fa-edit"></i></a>--%>     
						    <a href="#" id="help" style="display:none;color:#fff;" class="btn btn-sm tip" title="Help" onclick="HelpTab()"><i class="fa fa-question"></i></a>
		                </div></div>
		            </div>
                    	<div id="compare" style="float:right;display:inline-block; margin-left:2px;">
                            <a href="#" id="compare_icon" style="display:none;" title="Select 2 tabs" class="btn btn-default btn-sm tip"><i class="fa fa-copy"></i></a>
                            </div>
				</div>
					
					</h2>
                    <ul id="crumb" class="breadcrumb"></ul>
			   </div>
           <!-- SS -->
			<div id="toggle" style="float: left;width: 100%;position: absolute;background: rgba(23, 60, 93, 0.84);z-index: 9;display:none;height:100%;" class="content-inner">
              <div class="fa fa-arrow-circle-left" id="toggleClose" style="float:right;font-size: 30px;color: gainsboro;cursor: pointer;"></div>
             <div id="graphicalView" style="text-align:center;overflow:auto;height:90%;width: 100%;margin:auto;left:0;bottom:0;position:absolute;"></div>
			</div>
			<!-- Inner Content -->	 
             <div class="content-inner" style="padding:0;"> 
			 
			<!-- Home Page -->	 
             <div id="container-home" class="tile-row">
              
              <%--   <!-- expired reminders -->      
                
<asp:UpdatePanel ID="uplExpiredReminders" runat="server" style="display:none"><ContentTemplate>
          <div id="reminder_expired_Modal" class="modalDialog2 fade in" style="position:relative;height:100%;">
    <table>
                   
                    <asp:Repeater ID = "rptExpiredReminders" runat="server" >
                        <ItemTemplate>
                            <tr>
                                <td> <asp:Label ID="lbltitle" Text='<%# Eval("ReminderID") %>' ToolTip='<%# "Expired On: </br> " + Eval("TriggerTime") %>' runat="server" ></asp:Label></td>
                                <td></td>
                                <td> <asp:Label ID="lblMessage" Text='<%# Eval("Message").ToString().Length>=20?Eval("Message").ToString().Substring(0,19):Eval("Message").ToString() %>' ToolTip='<%# Eval("Message") %>' runat="server" ></asp:Label></td>
                                <td></td>
                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                     
            </table>
         </div>
  </ContentTemplate></asp:UpdatePanel>--%>




                 <asp:UpdatePanel ID="uplExpiredReminders" runat="server" style="display:none;"><ContentTemplate>
    <div id="reminder_expired_Modal" class="modalDialog2 fade in" style="position:relative;height:100%;z-index:999;">
       
	<div style="top:0;left:0;right:0;bottom:0;">	
		<div class="s-content" style="height:250px;min-width:550px;">
		<div class="s-header" style="padding:5px !important;">
			<h4 class="s-title" style="font-weight:bold;"><span class="fa fa-bell"></span>&nbsp;&nbsp;&nbsp;<span>Expired Reminders</span><div id="expired_popup_col_close" class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px;"></div>
               </h4>
         
		</div>
		<div class="s-body">
            <div class="bootbox-body" style="color:#000;margin-top:5px;margin-bottom:5px;overflow:auto;height:140px;">
                 <table>
                   
                    <asp:Repeater ID = "rptExpiredReminders" runat="server" >
                        <ItemTemplate>
                            <tr>
                                <td> <asp:Label ID="lbltitle" Text='<%# Eval("ReminderID") %>' ToolTip='<%# "Expired On: </br> " + Eval("TriggerTime") %>' runat="server" ></asp:Label></td>
                                <td></td>
                                <td style="padding-left:2em;"> <asp:Label ID="lblMessage" Text='<%# Eval("Message").ToString().Length>=20?Eval("Message").ToString().Substring(0,19):Eval("Message").ToString() %>' ToolTip='<%# Eval("Message") %>' runat="server" ></asp:Label></td>
                                <td></td>
                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                     
            </table>
            </div>
		</div>
		<div class="s-footer">
          <%--<a class="btn btn-primary" id="expired_close" style="cursor:pointer;float:left;margin-top:10px;margin-right:10px;padding-left:5px !important;padding-right:5px !important;padding:0;">OK</a>--%>
             </div>
		</div>
    </div>
        </div>
            </ContentTemplate>
          <%--  <Triggers>
                <asp:AsyncPostBackTrigger  ControlID="tmrReminder" EventName="Tick"/>
                
            </Triggers>--%>

        </asp:UpdatePanel>
        
			<!-- Reminder popup -->	
     
<%--			<div id="reminder_Modal" class="popup_col" style="display:none">

			<div style="position:relative;height:100%;background-color:#ddd;width:94%;margin-left:10px;">
		  <header>
                   <div id="popup_col_close" class="im-close" style="float:right;padding:10px;cursor:pointer;"></div></header>
                       <asp:UpdatePanel runat="server" ID="uplReminders" UpdateMode="Conditional">	
                            <ContentTemplate>
                        <asp:MultiView ID="mlViewReminder" runat="server">
                <table>
                   <asp:View ID="vExisting" runat="server" >
                        <asp:Label runat="server" ID="lblRemindersTitle" Text="Reminders"></asp:Label>
                        <br />
                        <asp:LinkButton ID="lbtnNew" Text="new" OnClick="lbtnNew_click" ToolTip="Create New" runat="server"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnSelectMultiple" OnClick="lbtnSelect_click" Text="Select" ToolTip="Select Multiple" runat="server"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete"  Text="delete" OnClick="lbtnDelete_click" ToolTip="Delete Selected" Visible="false" runat="server"></asp:LinkButton>

                       <asp:Repeater runat="server" ID="rptReminders" OnItemCommand="rptReminders_itemSelected">
                          <HeaderTemplate>
                              <br/>
                          </HeaderTemplate>
                         
                           <ItemTemplate>
                              
                               <asp:CheckBox runat="server" ID="cbxReminderSelect" Visible="false" />
                               <asp:LinkButton runat="server" ID="lbtnReminder" Text='<%# DataBinder.Eval(Container.DataItem,"ReminderID") %>' ToolTip="View/Edit"
                                   CommandName="select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReminderID") %>'></asp:LinkButton>
                               <br />
                           </ItemTemplate>

                       </asp:Repeater>
                        <asp:Label runat="server" ID ="lblRemindersCount" ForeColor="Red" EnableViewState="false"></asp:Label>
                     </asp:View>
    


                   <asp:View ID="vEditNew" runat="server">
                       
                              
                      
                       <tr >
                            <td>
                                    <asp:Label ID="lblReminderTitle" runat="server" Text ="Reminder Title" ></asp:Label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtReminderTitle" EnableViewState="false" runat="server" Text="" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqValReminderTitle" runat="server" ControlToValidate="txtReminderTitle"
                                     CssClass="Errormessagetextstyle" ErrorMessage="*" ValidationGroup="valReminder"></asp:RequiredFieldValidator>
                            </td>
                       </tr>
                       <tr>
                            <td>
                            
                                    </br>
                                    <asp:Label runat="server" Text="Select Time and Date: "></asp:Label>
                                    </br>
                        </td>
                         </tr>
                       <tr>
                            <td>
                                    <asp:TextBox ID="txtReminderDate" EnableViewState="false" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calSelectDate" runat="server" EnableViewState="false" TargetControlID="txtReminderDate"
                                    PopupButtonID="btnSelectDate" />
                                    <asp:LinkButton ID="btnSelectDate" EnableViewState="false" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    CssClass="calender" Height="17px" Width="20px" CausesValidation="False"
                                    TabIndex="10" />
                                    <asp:RequiredFieldValidator ID="reqValReminderDate" runat="server" ControlToValidate="txtReminderDate"
                                        CssClass="Errormessagetextstyle" ErrorMessage="*" ValidationGroup="valReminder"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                       <tr>
                            <td>
                                <cc2:TimeSelector ID="tmeReminder" runat="server" EnableViewState="false" SelectedTimeFormat="Twelve"  MinuteIncrement="5"  
                                    AmPm="AM" DisplaySeconds="False">
                                </cc2:TimeSelector>
                            </td>
                        </tr>
                       <tr>
                           <td>
                               <asp:Label runat="server" Text="Message"></asp:Label>
                           </td>

                       </tr>
                       <tr>
                           <td>
                              <asp:TextBox ID="txtReminderMsg" runat="server" EnableViewState="false" TextMode="MultiLine" Columns="20" Rows="4" MaxLength="120"></asp:TextBox>
                               <asp:RequiredFieldValidator runat="server" ID="reqValTxtreminder" ValidationGroup="valReminder" ControlToValidate="txtReminderMsg" ErrorMessage="*"  CssClass="Errormessagetextstyle">

                               </asp:RequiredFieldValidator>

                           </td>

                       </tr>
                       <tr>
                           <td>
                               <asp:HiddenField ID="hdnfEditReminder" runat="server" />
                               <asp:Label  ID="lblReminderError" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                               <asp:LinkButton runat="server" ID="lbtnReminderCancel" OnClick="lbtnReminderCancel_Click" Text="Cancel" EnableViewState="false" ToolTip="Back to Reminders" ></asp:LinkButton>
                               <asp:LinkButton runat="server" ID="lbtnReminderSave" OnClick="lbtnReminderSave_Click" EnableViewState="false" Visible="false" Text="Save" ToolTip="Save" ValidationGroup="valReminder" ></asp:LinkButton>
                               <asp:LinkButton runat="server" ID="lbtnReminderEditSave" OnClick="lbtnReminderEditSave_Click" EnableViewState="false" Visible="false" Text="Save" ToolTip="Save Changes" ValidationGroup="valReminder" ></asp:LinkButton>
                               <asp:LinkButton ID="lbtnEditSelectedReminder" OnClick="ReminderEdit_click" EnableViewState="false" Visible="false" runat="server" Text="Edit" ></asp:LinkButton>
                           </td>

                       </tr>
                       </asp:View>
    
                </table>

               </asp:MultiView>
  
            
             
         
          </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="tmrReminder" EventName="Tick"/>
              <asp:AsyncPostBackTrigger ControlID="reminder_ok" EventName="Click" />
            </Triggers>
                          
         </asp:UpdatePanel>
                            </div> 

	</div>
            
    <asp:Timer id="tmrReminder" runat="server"
            OnTick="reminderTriggered" Enabled="false">
    </asp:Timer>--%>
	<div id="reminder_Modal" class="popup_col" style="display:none;">
			<div style="position:relative;height:100%;background-color:#fff;width:94%;margin-left:10px;">
		     <header>
               <div class="s-header" style="padding:0; text-align:center;">
			   <h4 class="s-title" style="font-weight:bold;">Reminders<div id="popup_col_close" class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px;"></div></h4>
		       </div>
             </header>
              <asp:UpdatePanel runat="server" ID="uplReminders" UpdateMode="Conditional">	
              <ContentTemplate>
              <asp:MultiView ID="mlViewReminder" runat="server">
                 <table>
                 <asp:View ID="vExisting" runat="server" >
                     <div class="content-wrapper" style="border-style:none;">
                     <div class="content-inner" style="border-style:none;">
                        <div class="row">
                        <div class="col-lg-4 col-md-4 sortable-layout ui-sortable" style="width:100%;padding:0;border-style:none;">
                            <div class="panel-heading" style="border-style:none;">
                              <div class="panel-controls">                                                                
                                    <asp:LinkButton ID="lbtnNew" Text="new" OnClick="lbtnNew_click" ToolTip="Create New Reminder" runat="server"><i class="fa fa-plus-square-o fa-lg"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnSelectMultiple" OnClick="lbtnSelect_click" Text="Select" ToolTip="Click to Select the reminders to delete"  runat="server" style="margin-top:-1px;"><i class="fa fa-trash-o fa-lg" style="margin-top:-1px;"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnDelete"  Text="delete" OnClick="lbtnDelete_click" ToolTip="Confirm Deletion" Visible="false" runat="server"><i class="fa fa-check" ></i></asp:LinkButton>
                              </div>
                              </div>
                             
                              <div class="panel-body" style="display: block;padding-top:0; border-bottom-color:#fff;">
                              <div class="todo-widget" style="height:323px;overflow:auto;">
                                    <div class="todo-task-text">
                                    <asp:Repeater runat="server" ID="rptReminders" OnItemCommand="rptReminders_itemSelected">
                                    <ItemTemplate>
                                        <ul class="todo-list">
                                        <asp:CheckBox runat="server" ID="cbxReminderSelect" Visible="false" CssClass="checkbox_style" />
                                        <asp:LinkButton runat="server" class="todo-task-item" ID="lbtnReminder" Text='<%# DataBinder.Eval(Container.DataItem,"ReminderID") %>' ToolTip="View/Edit"
                                         CommandName="select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ReminderID") %>'></asp:LinkButton>
                                        </ul>
                                    </ItemTemplate>
                                    </asp:Repeater>
                                    </div>
                             </div> 
                             </div>
                        </div>
                        </div>
                     </div>
                     </div>
                  <asp:Label runat="server" ID ="lblRemindersCount" ForeColor="Red" EnableViewState="false"></asp:Label>
                  </asp:View>
    


                   <asp:View ID="vEditNew" runat="server">
                       
                          
                       
                           <div id="Div1" class="collapsed-sidebar">
                  <div class="content-wrapper" style="border-style:none;">
                     <div class="content-inner" style="border-style:none;">
                        <div class="row">
                        <div class="col-lg-4 col-md-4 sortable-layout ui-sortable" style="width:100%;padding:0;border-style:none;">
                            <div class="panel-heading" style="border-style:none;">
                                        <h4 class="panel-title">
                                            <i class="fa fa-th-list"></i>Create/Edit</h4>
                                        <div class="panel-controls">
                                <asp:LinkButton runat="server" ID="lbtnReminderSave" OnClick="lbtnReminderSave_Click" EnableViewState="false" Visible="false"  ToolTip="Save" ValidationGroup="valReminder" ><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>
                               <asp:LinkButton runat="server" ID="lbtnReminderEditSave" OnClick="lbtnReminderEditSave_Click" EnableViewState="false" Visible="false" Text="Save" ToolTip="Save Changes" ValidationGroup="valReminder" ><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>        
                                   <asp:LinkButton ID="lbtnEditSelectedReminder" OnClick="ReminderEdit_click" EnableViewState="false" Visible="false" runat="server" Text="Edit" ><i class="fa fa-pencil-square-o fa-lg"></i></asp:LinkButton>
                                              <asp:LinkButton runat="server" ID="lbtnReminderCancel" OnClick="lbtnReminderCancel_Click" Text="Cancel" EnableViewState="false" ToolTip="Back to Reminders" ><i class="fa fa-times fa-lg"></i></asp:LinkButton>
                             
                         
                                        </div>
                                    </div>
                                                                
                                         <div class="panel-body" style="display: block;padding-top:0; border-bottom-color:#fff;">
                                                <div class="todo-widget">
                                                    <ul class="todo-list" id="Ul1">
                                                        <li class="todo-task-item">
                                                            <div class="todo-task-text">
                
                                        
                                        <div>
                                            
                                                    <asp:Label ID="lblReminderTitle" runat="server" CssClass="Captionstyle" 
                                                        Text=" Reminder Title: " style="display: block;">                      
</asp:Label>
                                                
                                                </div>
                                            <div>
                                                 <asp:TextBox ID="txtReminderTitle" EnableViewState="false" runat="server" Text="" CssClass="col-md-55"  ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqValReminderTitle" runat="server" ControlToValidate="txtReminderTitle"
                                     CssClass="Errormessagetextstyle" ErrorMessage="*" ValidationGroup="valReminder"></asp:RequiredFieldValidator>

                                                
                                                </div>
                                            <div>
                                                
                                                    <asp:Label  runat="server" CssClass="Captionstyle" Text=" Select Time and Date : " style="display: block;"></asp:Label>
                                               
                                                  
                                                <asp:TextBox ID="txtReminderDate" style="float:left;" CssClass="col-md-122" EnableViewState="false" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calSelectDate" runat="server" EnableViewState="false" TargetControlID="txtReminderDate"
                                    PopupButtonID="btnSelectDate" CssClass="cal_Theme1"  />
                                    <asp:LinkButton ID="btnSelectDate" EnableViewState="false" style="float:left;font-size:20px;margin-left:5px;" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    CssClass="fa fa-calendar" CausesValidation="False"
                                    TabIndex="10" />
                                    <asp:RequiredFieldValidator ID="reqValReminderDate" runat="server" ControlToValidate="txtReminderDate"
                                        CssClass="Errormessagetextstyle" ErrorMessage="*" ValidationGroup="valReminder"></asp:RequiredFieldValidator>


                                                   <cc2:TimeSelector ID="tmeReminder" runat="server" EnableViewState="false" SelectedTimeFormat="Twelve"  MinuteIncrement="5"  
                                    AmPm="AM" DisplaySeconds="False">
                                </cc2:TimeSelector>

                                                 </div>
                                            <div>

                                                    <asp:Label runat="server" CssClass="Captionstyle" Text="Message : " style="display: block;">                                           
</asp:Label>
                                                 <asp:TextBox ID="txtReminderMsg" CssClass="col-md-55"  runat="server" EnableViewState="false" TextMode="MultiLine" Columns="20" Rows="4" MaxLength="120"></asp:TextBox>
                               <asp:RequiredFieldValidator runat="server" ID="reqValTxtreminder" ValidationGroup="valReminder" ControlToValidate="txtReminderMsg" ErrorMessage="*"  CssClass="Errormessagetextstyle">

                               </asp:RequiredFieldValidator>
                                                   
                                                </div>

                                             <asp:HiddenField ID="hdnfEditReminder" runat="server" />
                                 <asp:Label  ID="lblReminderError" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                                             
                                                
                                                 </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
             
                       
                       
                       
                       
                     
                       </asp:View>
    
                </table>

               </asp:MultiView>
  
            
             
         
          </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="tmrReminder" EventName="Tick"/>
              <asp:AsyncPostBackTrigger ControlID="reminder_ok" EventName="Click" />
            </Triggers>
                          
         </asp:UpdatePanel>
                            </div> 

	</div>
            
    <asp:Timer id="tmrReminder" runat="server"
            OnTick="reminderTriggered" Enabled="false">
    </asp:Timer>

   <!-- V2007 -->
                  
  

           <!-- Contacts popup -->			
			<div class="s-contents" id="contacts_Modal" style="display:none;background:#fff;margin-left:6px;padding-right:4px;padding-left:4px; padding-top:4px;padding-bottom:4px;">
				<div style="position:relative;height:100%;background-color:#ddd;">
					
					<div class="s-header" style="padding:0; text-align:center;">
			<h4 class="s-title" style="font-weight:bold;">Contacts<i class="fa fa-close" style="float:right;cursor:pointer;margin-top: 10px;margin-right: 10px;" id="contacts_close"></i></h4>
		</div>
		<div class="s-body" id="contacts_body" style="padding:0;">
		<%--<iframe src="./ContactCard.aspx" style="width:100%;height:374px;background-color:#fff;">
		    </iframe>--%>
		</div>
				</div>
            </div>
					
			<!-- left Content [widgets] -->			
			<div class="col-left-tile" id="toggle_div1">
				<div style="position:relative;width:98%;float:right;margin-top:10px;">
					<div class="wrapper gridster ready">
					<ul  id="hello">
						<li id="li1" class="gs-w bg-blue " data-row="1" data-col="1" data-sizex="1" data-sizey="1">
							<i id="main_help" style="color:#fff;margin:auto;position:absolute;top:20%;left:0;right:0;font-size:50px;" class="im-question" title="Help"></i><span class="widget_text">Help</span></li>
						<li id="li2" class="gs-w bg-darkBlue"  data-row="2" data-col="1" data-sizex="1" data-sizey="1">
							<i style="color:#fff;margin:auto;position:absolute;top:20%;left:0;right:0;font-size:50px;" class="im-tools settings_open"></i><span class="widget_text">Settings</span></li>
						<li  id="li3" style="overflow:hidden;background-color:#2F8593;" class="gs-w container1" data-row="1" data-col="1" data-sizex="2" data-sizey="2"><header style="font-size:14px;width:100%;font-weight:normal;text-align:left;cursor:default;">Favorites</header>					
                          
                            <div id="fav_c">
                                <asp:PlaceHolder id="plhFavourite" runat="server"/>
                                                </div>  

                            <%--clear_local--%>                       
  <a id="delete_fav" title="Delete" style="font-weight:normal;float:right;margin:10px;color:#fff;"><i class="fa fa-trash"></i></a>
   <a id="save_fav" title="Done" style="display:none;font-weight:normal;float:right;margin:10px;color:#fff;"><i class="fa fa-check"></i></a>
						</li>
						<li id="li4" class="gs-w bg-lightBlue"  data-row="4" data-col="1" data-sizex="1" data-sizey="1">
							<i id="contacts" style="color:#fff;margin:auto;position:absolute;top:20%;left:0;right:0;font-size:50px;" class="im-profile"></i><span class="widget_text">Contacts</span></li>
						<li id="li5" class="gs-w bg-green"  data-row="4" data-col="2" data-sizex="1" data-sizey="1">
						 <p id="li5_date" style="margin-top:40%;color:#fff;font-size:5em;"></p><p id="li5_day" class="widget_text" style="margin-top:28%;"></p></li>
						<li id="li6" class="gs-w bg-yellow" data-row="4" data-col="3" data-sizex="1" data-sizey="1">
							<i style="color:#fff;margin:auto;position:absolute;top:20%;left:0;right:0;font-size:50px;" class="fa fa-bell popup_open"></i><span class="widget_text">Reminder</span></li>
						<li id="li7" class="gs-w" style="display:none;background-color:rgb(221, 221, 221);"   data-row="3" data-col="1" data-sizex="2" data-sizey="1">
                   
						</li>
						<li id="li8" class="gs-w bg-red"   data-row="3" data-col="3" data-sizex="1" data-sizey="1"><header></header>
						<i id="crm" style="color:#fff;margin:auto;position:absolute;top:20%;left:0;right:0;font-size:50px;" class="fa fa-user"></i> <span class="widget_text">CRM</span> </li>
					</ul>
					</div>
		

		<style>
		    .gridster li header {
		        background: none;
		        color: #fff;
		        display: block;
		        line-height: normal;
		        padding: 4px 6px 6px;
		        font-size: 9px;
		        float: right;
		        cursor: move;
		    }

		    .widget_text {
		        float: left;
		        margin-top: 82%;
		        padding-left: 5px;
		        color: #fff;
		        font-weight: normal;
		        font-size: 11px;
		    }
		</style>
                   
		<script src="jquery.gridster.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript">
    var gridster;

    $(function () {
        var preventClick = function (e) { e.stopPropagation(); e.preventDefault(); };
        gridster = $(".gridster ul").gridster({
            widget_base_dimensions: [100, 100],
            widget_margins: [5, 5],
            serialize_params: function ($w, wgd) {
                return {
                    id: $($w).attr('id'),
                    col: wgd.col,
                    row: wgd.row,
                    size_x: wgd.size_x,
                    size_y: wgd.size_y,
                    htmlContent: $($w).html()
                };
            },
            draggable: {
                start: function (event, ui) {
                    // Stop event from propagating down the tree on the capture phase
                    ui.$player[0].addEventListener('click', preventClick, true);
                },
                stop: function (event, ui) {
                    var player = ui.$player;
                    setTimeout(function () {
                        player[0].removeEventListener('click', preventClick, true);
                    });
                    var positions = JSON.stringify(this.serialize());
                    localStorage.setItem('positions', positions);

                    $.post(
    "process.php",
    { "positions": positions },
    function (data) {
        if (data == 200)
            console.log("Data successfully sent to the server");
        else
            console.log("Error: Data cannot be sent to the server")
    }
    );
                }
            }
        }).data('gridster');

        //        $(window).resize(function() {
        //            gridster.resize_widget_dimensions({
        //                widget_base_dimensions: [200, 200],
        //                widget_margins: [10, 10]
        //            });
        //        });

        var localData = JSON.parse(localStorage.getItem('positions'));

        if (localData != null) {
            $.each(localData, function (i, value) {
                console.log('found positions');
                console.log(value.col);
                console.log(value.row);
                console.log(value.size_x);
                console.log(value.size_y);
                var id_name;

                id_name = "#";
                id_name = id_name + value.id;

                $(id_name).attr({ "data-col": value.col, "data-row": value.row, "data-sizex": value.size_x, "data-sizey": value.size_y, "data-value": value.htmlContent });

            });
        }
        else {
            console.log('No data stored in the local storage. Continue with the default values');
        }

    });
		</script>
		 
		</div>
        </div>
            
            
        <!-- Right Content [slider] -->    
		<div class="col-right-tile" id="toggle_div2">
			<div id="toggle_element" class="toggle_min">
				<div id="layer" style="background-color:#ddd;">
				<div class="slideshow" id="slideshow">
				<ol class="slides">
					<li class="current">
                       
                         <div class="description" style="width:60%;">
							<h2>X-ONT Software (Pvt) Ltd</h2>
							<p style="margin-top:20px;">X-ONT VENTURA solution is an enterprise-wide application that addresses the Sales and Distribution requirements of distribution companies’ operation.
X-ONT VENTURA & VIRTUSEL solutions strength is its ability to connect the 
distributed business processes into an integrated information management system. The power of X-ONT VENTURA real-time integration supports and simplifies enterprise-wide business processes for large distribution companies who rely on hundreds of 3rd party Dealers and sales channels. Its function and scope covers all distribution & selling aspects of a company’s distribution automation including Sales   administration, Dealer   automation, 
Sales Force Automation and an integrated redistribution Management Information System (MIS). 
 </p>
						</div>
						<div class="tiltview col" style="left:30% !important;width:70% !important;top:50% !important;">
							<a><img src="img/Tab1.jpg"/></a>
						</div>

						
					</li>
					<li>
						<div class="description">
							<h2>WHY X-ONT? </h2>
							<p style="margin-top:20px;">In essence X-ONT believes there are certain criteria that potential clients will be using in their selection of a software partner. 
<br>
 Financially Stable <br>
 Experienced in the IT Industry <br>
 Proven Track Record <br>
 Thorough Knowledge of the Retail Industry <br>
 Modern Technology <br>
 Commitment to the Industry <br>
 Direct Local Presence <br>
 </p>
						</div>
						<div class="tiltview row" style="left:20% !important;width:170% !important;top:70% !important;">
							<a><img src="img/Tab2.jpg"/></a>
						
						</div>
					</li>
					<li>
						<div class="tiltview col" style="transform:translateY(-50%) rotateX(0deg) rotateZ(0deg) !important;left:0 !important;">
                            <a>

                             <video controls style="width:200%;margin-top: 40px;height: 410px;">
                                  <source src="./XONTOrnox2.mp4" type="video/mp4">
                                    Your browser does not support the video tag.
                             </video></a>
						</div>
					</li>
				</ol>
			</div>
            </div>
        </div>
	</div> 
</div>
				
<!-- Content Tabs -->				

<div class="container" style="font-size:10px !important;width:100%;display:none; padding-left:0;padding-right:0;">
<div id="wrapper" style="width:100%;">
    <div style="float:left"><a href="#" id="minimizeTabs" style="font-size:19px;margin-top:-12px;display:none" class="btn" title="Minimize"><i class="fa fa-minus-square"></i></a></div>
    <ul id="tabs" class="tab_sort">
        <!-- Tabs go here -->
    </ul>
   <%-- <asp:UpdatePanel ID="uplIframe1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>--%>
            <%--<asp:Button ID="Button1" runat="server" Text="Button" />--%>
             <div id="tab_content1">      
        <!-- Tab content goes here -->
            </div>
   <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>

   <%-- <asp:UpdatePanel ID="uplIframe2" runat="server"  UpdateMode="Conditional">
        <ContentTemplate>--%>
             <%--<asp:Button ID="Button2" runat="server" Text="Button" />--%>
             <div id="tab_content2" >
        <!-- Tab content goes here -->
            </div>
      <%--  </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="uplIframe3" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>--%>
             <div id="tab_content3">
        <!-- Tab content goes here -->
         </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="uplIframe4" runat="server"  UpdateMode="Conditional">
        <ContentTemplate>--%>
             <div id="tab_content4">
        <!-- Tab content goes here -->
            </div>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>
   --%>
    
    <!-- Help modal -->
 <div id="help_Modal1" class="modalDialog2 fade in" style="display:none;position:fixed;height:100%;width:100%;margin:auto;top:0;left:0;right:0;bottom:0;z-index:1000;">
	<div>	
		<div class="s-content" style="height:90%;width:80%;">
		<div class="s-header">
			<h4 class="s-title" style="font-weight:bold;"> Help Menu </h4>
                	<a style="font-size:20px;float:right;text-decoration:none;cursor:pointer;margin-top:-20px;" class="fa fa-close" id="help_close1"></a></div>
		<div class="s-body" id="help_Modal_iframe">
			
		
		</div>
		</div>
    </div>
</div>
        <!-- favorite modal -->
<div id="fav_Modal" class="modalDialog2 fade in" style="display:none;position:fixed;height:100%;top:0;bottom:0;left:0;right:0;margin:auto;z-index:2;">
    <div>	
		<div class="s-content" style="height:190px;width:600px;">
		<div class="s-header">
			<i class="im-close"  id="fav_close" style="float:right;padding:10px;cursor:pointer;"></i>
			<h4 class="s-title" style="font-weight:bold;"> Add to Favorites </h4>
		</div>
		<div class="s-body">
			<form autocomplete="off">
			<br>
				Name:&nbsp; <input type="text" style="width:50%;" class="keyup-char" id="fav_name" autocomplete="off" required/><span id="validation" style="margin-left:5px;display:none;color:Red;">*</span>
			<br>
				URL:&nbsp; <input type="text" style="width:80%;margin-left:11px;text-overflow: ellipsis;overflow:hidden;" id="fav_value" readonly />
			<br><br>
                <span id="duplicate_name" class="error" style="display:none;"> Name Already Exists !</span>
				<input type="button" id="fav_save" value="Save"  class="btn btn-primary" style="float:right;margin-left:20px;" />
				<input type="button" id="fav_cancel" value="Cancel" class="btn btn-default" style="float:right;" />
			</form>
		</div>
		</div>
    </div>
</div>
</div>	
</div>
</div>
</div>
</div>
<div class="clearfix"></div>

<!-- Scripts -->	
<%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>--%>
		 <script src="./assets/js/common.js"></script>
		<!-- Slider -->
 
		<script src="assets/js/classie.js"></script>
		<script src="assets/js/tiltSlider.js"></script>
		<script>
		    new TiltSlider(document.getElementById('slideshow'));
		</script>
		
		<script src="./assets/js/bootstrap/bootstrap.js"></script>
        <script src="./assets/js/libs/modernizr.custom.js"></script>
        <script src="./assets/js/jRespond.min.js"></script>
		<script src="./assets/plugins/core/slimscroll/jquery.slimscroll.min.js"></script>
		<script src="./assets/plugins/core/slimscroll/jquery.slimscroll.horizontal.min.js"></script>
		<script src="./assets/plugins/misc/ion-sound/ion.sound.js"></script>
		<script src="./assets/plugins/misc/highlight/highlight.pack.js"></script>
       	
        <script src="./assets/js/jquery.appStart.js"></script>
    
        <script src="./assets/js/app.js"></script>
    
		<script type="text/javascript" src="./bootstrap-iconpicker/js/iconset/iconset-fontawesome-4.2.0.min.js"></script>
        <script type="text/javascript" src="./bootstrap-iconpicker/js/iconset/iconset-all.min.js"></script>
        <script type="text/javascript" src="./bootstrap-iconpicker/js/bootstrap-iconpicker.js"></script>
        
		<script src="./lightGallery-master/dist/js/lightgallery.js"></script>
        <script src="./lightGallery-master/dist/js/lg-fullscreen.js"></script>
        <script src="./lightGallery-master/dist/js/lg-thumbnail.js"></script>
        <script src="./lightGallery-master/dist/js/lg-video.js"></script>
        <script src="./lightGallery-master/dist/js/lg-autoplay.js"></script>
        <script src="./lightGallery-master/dist/js/lg-zoom.js"></script>
        <script src="./lightGallery-master/dist/js/lg-hash.js"></script>
        <script src="./lightGallery-master/dist/js/lg-pager.js"></script>
        <script src="./lightGallery-master/lib/jquery.mousewheel.min.js"></script>
   
  
   
    <script src="./assets/js/bootbox.js"></script>
    <script src="js/cbpFWTabs.js"></script>
		<script>
		    (function () {

		        [].slice.call(document.querySelectorAll('.tabs')).forEach(function (el) {
		            new CBPFWTabs(el);
		        });

		    })();
		</script>
      
   
		</form>   
    
	</body>
	
</html>