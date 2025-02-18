using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class TriggerSulFestMailInvitaionController : ApiController
    {
        public HttpResponseMessage Get(int id_event) 
        {
            string Result = "";

            EventsHeader Res = new EventsHeader();



            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_profile> prof = new List<tbl_profile>();
                    tbl_sul_fest_master fest = new tbl_sul_fest_master();
                    fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master where id_event={0}", id_event).FirstOrDefault();

                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile inner join tbl_user on tbl_user.ID_USER= tbl_profile.ID_USER where tbl_user.ID_ORGANIZATION=130 and tbl_user.STATUS='A'").ToList();
                    SendEventMailNotification(prof, fest);


               

                    Result = "SUCCESS";
                   




                }






            }
            catch (Exception e)
            {
                Result = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }


            return Request.CreateResponse(HttpStatusCode.OK, Result);


        }

        public void SendEventMailNotification(List<tbl_profile> prof, tbl_sul_fest_master fes)
        {
            try
            {
                /*  Email ID changed on requst on 08-01-2020
                string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                const string senderPassword = "Pls@210312"; // sender password here…
                */

                string senderID = "skillmuni@thegamificationcompany.com";// use sender’s email id here..
                const string senderPassword = "03012019@Skillmuni"; // sender password here…

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                {
                    string college = db.Database.SqlQuery<string>("select college_name from tbl_college_list where id_college={0}", fes.id_college).FirstOrDefault();


                    if (fes.is_college_restricted == 1)
                    {


                        foreach (var itm in prof)

                        {
                            if (college == itm.COLLEGE)
                            {


                                if (itm.EMAIL != null && itm.EMAIL != "")
                                {
                                    string recmail = itm.EMAIL;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
                                    string body = string.Empty;


                                    //string cat_img = get_cat_image(Convert.ToInt32(con.id_category));

                                    string mailpage = ConfigurationManager.AppSettings["mail_sul_event_opublish"];
                                    //string notelink = ConfigurationManager.AppSettings["content_link"];

                                    //string mailpage = "C:\SulCMSBetaV2\Content\SKILLMUNI_DATA";
                                    using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                                    {
                                        body = reader.ReadToEnd();
                                    }

                                    //string category = get_cat_name(Convert.ToInt32(con.id_category));
                                    body = body.Replace("{REG_START}", Convert.ToString(fes.registration_start_date.Date));
                                    body = body.Replace("{REG_END}", Convert.ToString(fes.registration_end_date.Date));
                                    body = body.Replace("{FESt_MONTH}", Convert.ToString(fes.event_start_date.ToString("MMMM")));
                                    body = body.Replace("{FEST_DATE}", Convert.ToString(fes.event_start_date.Day));
                                    body = body.Replace("{COLLEGE_NAME}", Convert.ToString(college));
                                    body = body.Replace("{COLLEGE_ADDRESS}", Convert.ToString(fes.address));
                                    body = body.Replace("{START_TIME}", Convert.ToString(fes.event_start_date.ToString("h:mm tt")));
                                    body = body.Replace("{END_TIME}", Convert.ToString(fes.event_start_date.ToString("h:mm tt")));
                                    body = body.Replace("{CONTACT_NAME}", Convert.ToString(fes.contact_name));
                                    body = body.Replace("{CONTACT_NUMBER}", Convert.ToString(fes.contact_number));

                                    string sub = "New Event Available - " + fes.event_title;
                                    string msg = fes.event_objective;
                                    SmtpClient smtp = new SmtpClient
                                    {
                                        Host = "smtp.gmail.com",
                                        Port = 587,
                                        EnableSsl = true,
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                                        Timeout = 30000,
                                    };
                                    MailMessage message = new MailMessage(senderID, recmail, sub, body);//body replaced by msg
                                    message.IsBodyHtml = true;
                                    smtp.Send(message);
                                }

                            }


                        }



                    }
                    else
                    {
                        foreach (var itm in prof)

                        {
                            if (itm.EMAIL != null && itm.EMAIL != "")
                            {
                                string recmail = itm.EMAIL;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
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

                                string mailpage = ConfigurationManager.AppSettings["mail_sul_event_opublish"];
                                //string notelink = ConfigurationManager.AppSettings["content_link"];

                                //string mailpage = "C:\SulCMSBetaV2\Content\SKILLMUNI_DATA";
                                using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                                {
                                    body = reader.ReadToEnd();
                                }

                                //string category = get_cat_name(Convert.ToInt32(con.id_category));
                                body = body.Replace("{REG_START}", Convert.ToString(fes.registration_start_date.Date));
                                body = body.Replace("{REG_END}", Convert.ToString(fes.registration_end_date.Date));
                                body = body.Replace("{FESt_MONTH}", Convert.ToString(fes.event_start_date.ToString("MMMM")));
                                body = body.Replace("{FEST_DATE}", Convert.ToString(fes.event_start_date.Day));
                                body = body.Replace("{COLLEGE_NAME}", Convert.ToString(college));
                                body = body.Replace("{COLLEGE_ADDRESS}", Convert.ToString(fes.address));
                                body = body.Replace("{START_TIME}", Convert.ToString(fes.event_start_date.ToString("h:mm tt")));
                                body = body.Replace("{END_TIME}", Convert.ToString(fes.event_start_date.ToString("h:mm tt")));
                                body = body.Replace("{CONTACT_NAME}", Convert.ToString(fes.contact_name));
                                body = body.Replace("{CONTACT_NUMBER}", Convert.ToString(fes.contact_number));

                                string sub = "New Event Available - " + fes.event_title;
                                string msg = fes.event_objective;
                                SmtpClient smtp = new SmtpClient
                                {
                                    Host = "smtp.gmail.com",
                                    Port = 587,
                                    EnableSsl = true,
                                    DeliveryMethod = SmtpDeliveryMethod.Network,
                                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                                    Timeout = 30000,
                                };
                                MailMessage message = new MailMessage(senderID, recmail, sub, body);//body replaced by msg
                                message.IsBodyHtml = true;
                                smtp.Send(message);
                            }




                        }

                    }




                }






            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
