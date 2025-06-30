using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using XONT.Common.Message;
using XONT.VENTURA.V2MNT21;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class MultiMedia : Page
    {
        private MessageSet _msg;
        private User _user;

        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (User) Session["Main_LoginUser"];
            //_user = new User { UserName = "nadeeka", PowerUser = "0", BusinessUnit = "XONT" };
            //Session["Main_LoginUser"] = _user;

            LoadFiles();
        }


        private void LoadFiles()
        {
            var multimedias = Session["Main_Multimedia"] as List<MultimediaDetails>;

            List<MultimediaDetails> multimediaImages =
                multimedias.Where(
                    p =>
                    ((p.Extension.Trim() == ".jpg") || (p.Extension.Trim() == ".bmp") || (p.Extension.Trim() == ".png") ||
                     (p.Extension.Trim() == ".jpeg"))).ToList();
            List<MultimediaDetails> multimediaFlash =
                multimedias.Where(p => ((p.Extension.Trim() == ".mp4") || (p.Extension.Trim() == ".swf"))).ToList();
            List<MultimediaDetails> multimediaText =
                multimedias.Where(p => ((p.Type == 'T'))).ToList();
            // var table = new HtmlTable();
            // var tableRow = new HtmlTableRow();
            //HtmlGenericControl li = new HtmlGenericControl("li");
            HtmlGenericControl div = new HtmlGenericControl("div");
            int i = 0;
            foreach (MultimediaDetails file in multimediaFlash)
            {

                i++;
                HtmlGenericControl list1 = new HtmlGenericControl("li");
                list1.Attributes["class"] = "col-xs-6 col-sm-4 col-md-3";
                list1.Attributes["data-html"] = "#video"+i.ToString();
                list1.Attributes["data-poster"] = "V2MNT21/data/img" + file.FileName.Trim() + ".png";

                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", "");
                anchor.Attributes.Add("title", i.ToString());
                anchor.Attributes.Add("onclick", "javascript:loadToHead(this.id)");
                anchor.ID = file.FileName.Trim() + file.Extension.Trim();

                var image = new HtmlImage();
                image.Src = "V2MNT21/data/img" + file.FileName.Trim() + ".png";
                image.Attributes["class"] = "img-responsive";
                anchor.Controls.Add(image);
                list1.Controls.Add(anchor);

   
                div.ID = "video" + i.ToString();
                div.Attributes["style"] = "display:none";
                HtmlGenericControl video = new HtmlGenericControl("video");
                video.Attributes["class"] = "lg-video-object lg-html5";
                HtmlGenericControl source = new HtmlGenericControl("source");
                source.Attributes["src"] = "V2MNT21/data/" + file.FileName.Trim() + file.Extension.Trim();
                source.Attributes["type"] = "video/mp4";
                video.Controls.Add(source);
                div.Controls.Add(video);

                multimedia_videolist.Controls.Add(list1);
                multimedia_video.Controls.Add(div);
                //    i++;

                //    var tableCell = new HtmlTableCell();
                //    var anchor = new HtmlAnchor();
                //    anchor.Attributes.Add("rel", "#voverlay");
                //    anchor.HRef = "engine/swf/player.swf?url=../../V2MNT21/data/" + file.FileName.Trim() +
                //                  file.Extension.Trim();
                //    anchor.Title = i.ToString();
                //    anchor.ID = file.FileName.Trim() +
                //                file.Extension.Trim();
                //    anchor.Attributes.Add("onclick",
                //                          "javascript:loadToHead(this.id)");

                //    var image = new HtmlImage();
                //    image.Alt = i.ToString();
                //    image.Src = "V2MNT21/data/img" + file.FileName.Trim() + ".png";
                //    image.Style.Add("vertical-align", "middle");
                //    anchor.Controls.Add(image);
                //    var span = new HtmlGenericControl("SPAN");
                //    anchor.Controls.Add(span);
                //    tableCell.Controls.Add(anchor);
                //    tableRow.Cells.Add(tableCell);
                //    //    plhFiles.Controls.Add(anchor);
            }
         
            foreach (MultimediaDetails file in multimediaImages)
            {

                i++;
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes["class"] = "col-xs-6 col-sm-4 col-md-3";
                li.Attributes["data-responsive"] = "img/1-375.jpg 375, img/1-480.jpg 480, img/1.jpg 800";
                li.Attributes["data-src"] = "V2MNT21/data/" + file.FileName.Trim() + file.Extension.Trim();
                li.Attributes["data-sub-html"] = "<h4>Description</h4><p> " + file.Description.ToString().Trim() +"</p>";
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", "");
                anchor.Attributes.Add("title", i.ToString());
                anchor.Attributes.Add("onclick", "javascript:loadToHead(this.id)");
                anchor.ID = file.FileName.Trim() + file.Extension.Trim();
                var image = new HtmlImage();
                image.Src = "V2MNT21/data/img" + file.FileName.Trim() + ".png";
                image.Attributes["class"] = "img-responsive";
                anchor.Controls.Add(image);
                li.Controls.Add(anchor);

                  multimedia_trial.Controls.Add(li);
                //    //var tableCell = new HtmlTableCell();
                //    //var anchor = new HtmlAnchor();
                //    //anchor.Attributes.Add("rel", "#voverlay");
                //    //anchor.HRef = "V2MNT21/data/" + file.FileName.Trim() + file.Extension.Trim();
                //    //anchor.Title = i.ToString();
                //    //anchor.ID = file.FileName.Trim() +
                //    //            file.Extension.Trim();
                //    //anchor.Attributes.Add("onclick",
                //    //                      "javascript:loadToHead(this.id)");
                //    //var image = new HtmlImage();
                //    //image.Alt = i.ToString();
                //    //image.Src = "V2MNT21/data/img" + file.FileName.Trim() + ".png";

                //    //image.Style.Add("vertical-align", "middle");
                //    //anchor.Controls.Add(image);
                //    //tableCell.Controls.Add(anchor);
                //    //tableRow.Cells.Add(tableCell);
                //    ////plhFiles.Controls.Add(anchor);
            }
            foreach (MultimediaDetails text in multimediaText)
            {

                i++;
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes["class"] = "col-xs-6 col-sm-4 col-md-3";
                li.Attributes["style"] = "text-decoration:none;cursor: default;pointer-events: none; ; ";
                li.Attributes["data-sub-html"] = "<h4>Description</h4><p> " + text.Description.ToString().Trim() + "</p>";
                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("title", i.ToString());
                anchor.Attributes.Add("style", "text-decoration:none;cursor: default;pointer-events: none; ; ");
                anchor.Attributes.Add("class", "test");
                //  anchor.ID = text.FileName.Trim() + text.Extension.Trim();
                var image = new HtmlInputText();
                image.Value = text.Text.ToString().Trim();

               // image.Src = "V2MNT21/data/img" + file.FileName.Trim() + ".png";
               // image.Attributes["class"] = "img-responsive";
                anchor.Controls.Add(image);
                li.Controls.Add(anchor);

                multimedia_trial.Controls.Add(li);
            
            }
            //table.Rows.Add(tableRow);
            //Uncomment this 
            //plhFiles.Controls.Add(table);






            //if (!IsPostBack)
            //{
            //    if (multimediaFlash.Count > 0)
            //    {
            //        ClientScript.RegisterStartupScript(GetType(), "script",
            //                                           "<script language='javascript'> document.getElementById('" +
            //                                           multimediaFlash[0].FileName.Trim() +
            //                                           multimediaFlash[0].Extension.Trim() + "').onclick() </script>");
            //    }
            //    else if (multimediaImages.Count > 0)
            //    {
            //        ClientScript.RegisterStartupScript(GetType(), "script",
            //                                           "<script language='javascript'> document.getElementById('" +
            //                                           multimediaImages[0].FileName.Trim() +
            //                                           multimediaImages[0].Extension.Trim() + "').onclick() </script>");
            //    }
            //}
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string[] words = playVedioList.Value.Trim().Split(',');

            string[] distinctWords = (from p in words select p).Distinct().ToArray();
            var multimedias = Session["Main_MultimediaMandotory"] as List<MultimediaDetails>;
            bool isExists = true;
            foreach (MultimediaDetails s in multimedias)
            {
                string fileName = s.FileName.Trim() + s.Extension.Trim();


                string[] isExistsWords =
                    (from p in distinctWords where p == fileName select p).ToArray();
                if (isExistsWords.Length <= 0)
                {
                    isExists = false;
                    break;
                }
            }
            if (isExists)
            {
                //var multimediaAll = Session["Main_Multimedia"] as List<Multimedia>;
                //List<int> multimediaRecID = multimediaAll.Select(p => p.MultimediaRecID).ToList();
                //IUserDAO userDao = new UserManager();
                //userDao.UpdateMultimediaDate(_user.BusinessUnit, _user.UserName, multimediaRecID, ref _msg);
                //if (_msg == null)
                //{
                ClientScript.RegisterStartupScript(GetType(), "Main_Multimeadia",
                                                                "javascript:window.parent.HideMultimediaPopup();",
                                                                true);
                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(GetType(), "script",
                //                                   "<script language='javascript'>alert('Error Update');</script>");
                //    return;
                //}
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "script",
                                                   "<script language='javascript'>alert('View All Advertisements ');</script>");
            }
        }
    }
}