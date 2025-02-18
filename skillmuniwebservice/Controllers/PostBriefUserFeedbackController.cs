using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostBriefUserFeedbackController : ApiController
    {
        
            public HttpResponseMessage Post([FromBody]tbl_brief_user_feedback_master Feedback)
            {
                APIPostRes result = new APIPostRes();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                try
                {


                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {

                    Feedback.id_feedback= db.Database.ExecuteSqlCommand("insert into tbl_brief_user_feedback_master(UID,OID,liked,disliked,reason,status,updated_date_time,feedback_type,id_brief_master) values({0},{1},{2},{3},{4},{5},{6},{7},{8});select max(id_feedback) from tbl_brief_user_feedback_master;", Feedback.UID,Feedback.OID,Feedback.liked,Feedback.disliked,Feedback.reason,"A",DateTime.Now,Feedback.feedback_type,Feedback.id_brief_master);
                    }


                int i = 1;
                if (Feedback.MediaFlag == 1)
                {
                    foreach (var itm in Feedback.Media)
                    {

                        byte[] ImageBase = Convert.FromBase64String(itm.media);

                        File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\BriefFeedback\" + Feedback.id_feedback + "_" + i + "." + itm.extension, ImageBase);
                        itm.media = Feedback.id_feedback + "_" + i + "." + itm.extension;
                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            db.Database.ExecuteSqlCommand("Insert into  tbl_brief_feedback_media (id_feedback,media,extension,updated_time) values({0},{1},{2},{3})", Feedback.id_feedback, itm.media, itm.extension, DateTime.Now);
                        }
                        i++;
                    }


                }
                result.Message = "Thanks for your feedback.";
                    result.Status = "SUCCESS";

                }
                catch (Exception e)
                {

                    result.Status = "FAILED";
                    result.Message = "Something went wrong please try after some time.Or else please contact admin.";


                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }

                // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            private static Random random = new Random();
            public static string RandomString(int length)
            {
                const string chars = "0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public void SendOTP(string Semail, string OTP, string Name)
            {
                try
                {
                /*  Email ID changed on requst on 08-01-2020
                string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                const string senderPassword = "Pls@210312"; // sender password here…
                */

                string senderID = "skillmuni@thegamificationcompany.com";// use sender’s email id here..
                const string senderPassword = "03012019@Skillmuni"; // sender password here…


                string recmail = Semail;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
                    string body = string.Empty;


                    //string cat_img = get_cat_image(Convert.ToInt32(con.id_category));

                    //string mailpage = ConfigurationManager.AppSettings["mail_content"];
                    //string notelink = ConfigurationManager.AppSettings["content_link"];

                    //string mailpage = "";
                    //using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                    //{
                    //    body = reader.ReadToEnd();
                    //}

                    //string category = get_cat_name(Convert.ToInt32(con.id_category));
                    //body = body.Replace("{TILE}", category);
                    //body = body.Replace("{MESSAGE}", note.notification_message);
                    //body = body.Replace("{NAME}", profile.FIRSTNAME);
                    //body = body.Replace("{SDATE}", Convert.ToString(note.start_date));
                    //body = body.Replace("{EDATE}", Convert.ToString(note.end_date));
                    //body = body.Replace("{IMAGE}", ConfigurationManager.AppSettings["content_image"] + "/" + oid + "/" + cat_img);
                    //body = body.Replace("{NOTELINK}", ConfigurationManager.AppSettings["content_link"] + con.id_content);

                    string sub = "OTP- SUL Event Registration";
                    string msg = "Thanks for your intrest in Skillmuni University Event registration. Please use below OTP to verify your email ID and complete the process. <br /> " + "OTP is <b>" + OTP + "</b>";
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                        Timeout = 30000,
                    };
                    MailMessage message = new MailMessage(senderID, recmail, sub, msg);//body replaced by msg
                    message.IsBodyHtml = true;
                    smtp.Send(message);

















                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        

    }
}
