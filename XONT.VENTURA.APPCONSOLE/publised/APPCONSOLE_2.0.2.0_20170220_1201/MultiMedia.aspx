<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiMedia.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.MultiMedia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

   <%-- <script type="text/javascript" src="js/swfobject.js"></script>

    <!-- Start VideoLightBox.com HEAD section -->
    <link rel="stylesheet" href="engine/css/videolightbox.css" type="text/css" />
    <style type="text/css">
        #videogallery a#videolb
        {
            display: none;
        }
        
        #clsmod {
	position:absolute;
	width:34px;
	height:34px;
	z-index:9999;
	cursor:pointer;
}

		.clsmodbtn
		{
			width:30px;
			height:30px; 
			background-color: transparent;
			background: url("images/close.png") no-repeat;
			cursor:pointer;
		}

    </style>
    
    <link rel="stylesheet" type="text/css" href="engine/css/overlay-minimal.css" />

    <script src="engine/js/jquery.tools.min.js" type="text/javascript"></script>

    <script src="engine/js/swfobject.js" type="text/javascript"></script>

    <!-- make all links with the 'rel' attribute open overlays -->

    <script src="engine/js/videolightbox.js" type="text/javascript"></script>

    <!-- End VideoLightBox.com HEAD section -->
    <link href="Transport_files/default.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="Transport_files/print.css" type="text/css" rel="stylesheet" media="print" />
    <%--<script type="text/javascript" src="Transport_files/jquery.js"></script>--%>

 <%--   <script type="text/javascript" src="Transport_files/jcarousellite_1.js"></script>

    <script type="text/javascript" src="Transport_files/global.js"></script>

    <script type="text/javascript" language="javascript"></script>--%>

    <!-- Additionally added on 3/11/2016 -->

        <link href="assets/cdn/font_googleapi.css" rel="stylesheet" type="text/css" />
		<link rel="stylesheet" type="text/css" href="assets/css/common.css"/>
    <link href="assets/css/flexslider.css" rel="stylesheet" />

		<link href="assets/css/bootstrap.css" rel="stylesheet" />

		<link rel="stylesheet" href="icon-fonts/font-awesome-4.6.3/css/font-awesome.min.css"/>
        <link rel="stylesheet" href="bootstrap-iconpicker/css/bootstrap-iconpicker.min.css"/>
				
        <link href="assets/css/icons.css" rel="stylesheet" />
		 <link href="assets/cdn/font_awesome.css" rel="stylesheet" />

		  <link href="./lightGallery-master/dist/css/lightgallery.css" rel="stylesheet" />
		
	    <link href="index.css" rel="stylesheet" type="text/css" />
        <link href="css/newinlinedata.css" rel="stylesheet" type="text/css" />

         <script type="text/javascript" src="assets/cdn/jquery.js"></script>
        <script type="text/javascript"  src="assets/cdn/jquery_ui.js"></script>
      

		<link rel="stylesheet" type="text/css" href="assets/css/component.css" />

       <script type="text/javascript" language="javascript">
           $(document).ready(function () {
               $('#lightgallery').lightGallery({
                   enableDrag: false,
                   thumbnail: false,
                   controls: false,
                   autoplayControls: false
               });
           });
          </script>

    <script type="text/javascript">



        function loadToHead(id) {
            document.getElementById("playVedioList").value = document.getElementById("playVedioList").value + "," + id;

        }
    
    </script>

</head>
<body style="background-color:Transparent; overflow:hidden;" >
    <form id="form1" runat="server">
         <asp:PlaceHolder runat="server" ID="multimedia_video"    />
     <div id="multimedia_Modal" class="modalDialog2 fade in" style="display:block;position:relative;height:100%;z-index:1000;">
    <div>	
		 <div class="demo-gallery" style="margin:10%;padding-bottom:5%;background-color:#fff;">
		 <div class="s-header">
             <asp:LinkButton ID="Button1" style="float:right;padding:10px;cursor:pointer;color:#fff;"  OnClick="Button1_Click" BorderStyle="None" 
                BorderWidth="0px" CssClass="im-close" Height="30px" Width="30px" runat="server"></asp:LinkButton>
			<h4 class="s-title" style="font-weight:bold;">Multimedia Gallery </h4>
		</div>
        <%--     <div id="videogallery">
                                <asp:PlaceHolder runat="server" ID="plhFiles"    />
                            </div>--%>
              
            <ul id="lightgallery" class="list-unstyled row" style="padding-top:5%;padding-left:10%;">
                 <asp:PlaceHolder runat="server" ID="multimedia_trial"    />
               <%--              
                <li class="col-xs-6 col-sm-4 col-md-3" data-responsive="img/13-375.jpg 375, img/13-480.jpg 480, img/13.jpg 800" data-src="./lightGallery-master/demo/img/13-1600.jpg" data-sub-html="<h4>Bowness Bay</h4><p>A beautiful Sunrise this morning taken En-route to Keswick not one as planned but I'm extremely happy I was passing the right place at the right time....</p>">
                    <a href="">
                        <img class="img-responsive" src="./lightGallery-master/demo/img/thumb-13.jpg">
                    </a>
                </li>  --%>   
                 <asp:PlaceHolder runat="server" ID="multimedia_videolist"    /> 
             <%--   <li data-poster="V2MNT21/data/img1.png" data-sub-html="video caption1" data-html="#video2" >
      <img src="V2MNT21/data/img1.png" />
  </li>         --%>
            </ul>
		</div>
    </div>
        
</div>    
    <input id="playVedio" type="hidden" runat="server" value="0"></input>
    <input id="playVedioList" type="hidden" runat="server" value="0"></input>
    </form>

   
		
		<script type="text/javascript" src="./assets/js/bootstrap/bootstrap.js"></script>
        <script type="text/javascript" src="./assets/js/libs/modernizr.custom.js"></script>
        <script type="text/javascript" src="./assets/js/jRespond.min.js"></script>
		<script type="text/javascript" src="./assets/plugins/core/slimscroll/jquery.slimscroll.min.js"></script>
		<script type="text/javascript" src="./assets/plugins/core/slimscroll/jquery.slimscroll.horizontal.min.js"></script>
		<script type="text/javascript" src="./assets/plugins/misc/ion-sound/ion.sound.js"></script>
		<script type="text/javascript" src="./assets/plugins/misc/highlight/highlight.pack.js"></script>

        <script type="text/javascript" src="./assets/js/jquery.appStart.js"></script>
        <script type="text/javascript" src="./assets/js/app.js"></script>
        
		<script type="text/javascript" src="./bootstrap-iconpicker/js/iconset/iconset-fontawesome-4.2.0.min.js"></script>
        <script type="text/javascript" src="./bootstrap-iconpicker/js/iconset/iconset-all.min.js"></script>
        <script type="text/javascript" src="./bootstrap-iconpicker/js/bootstrap-iconpicker.js"></script>
        
		<script type="text/javascript" src="./lightGallery-master/dist/js/lightgallery.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-fullscreen.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-thumbnail.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-video.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-autoplay.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-zoom.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-hash.js"></script>
        <script type="text/javascript" src="./lightGallery-master/dist/js/lg-pager.js"></script>
        <script type="text/javascript" src="./lightGallery-master/lib/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="./assets/js/common.js"></script>
    <script type="text/javascript" defer src="assets/js/jquery.flexslider.js"></script>
    <script type="text/javascript">

    $(window).load(function(){
      $('.flexslider').flexslider({
        animation: "slide",
        animationLoop: false,
        itemWidth: 210,
        itemMargin: 5,
        pausePlay: true,
       
      });
    });
  </script>

    <script type="text/javascript" src="./assets/js/bootbox.js"></script>
</body>
</html>
