using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class SULFestEventRegistrationController : ApiController
    {
        public HttpResponseMessage Post([FromBody]FestRegistration Fest)
        {
            FestRegResponse result = new FestRegResponse();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();
                tbl_profile pro = new tbl_profile();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    pro = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}",Fest.UID).FirstOrDefault();
                    if (Fest.is_new_college == 1)
                    {
                        Fest.id_college = db.Database.SqlQuery<int>("insert into tbl_college_list(college_name,clg_state,status,id_city,id_organization,id_user,updated_datetime,clg_city) values({0},{1},{2},{3},{4},{5},{6},{7}) ;select max(id_college) from tbl_college_list", Fest.college_name, Fest.state, "A", Fest.id_city, 130, Fest.UID, DateTime.Now, Fest.city).FirstOrDefault();


                    }
                    if (Fest.register_status == 2)
                    {
                        db.Database.ExecuteSqlCommand("delete from  tbl_sul_fest_event_registration where UID={0} and id_event={1} ", Fest.UID, Fest.id_event);

                        db.Database.ExecuteSqlCommand("insert into tbl_sul_fest_event_registration(UID,id_college,id_state,id_city,id_event,status,updated_date_time) values({0},{1},{2},{3},{4},{5},{6}) ", Fest.UID, Fest.id_college, Fest.id_state, Fest.id_city, Fest.id_event, "P", DateTime.Now);


                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_sul_fest_event_registration(UID,id_college,id_state,id_city,id_event,status,updated_date_time) values({0},{1},{2},{3},{4},{5},{6}) ", Fest.UID, Fest.id_college, Fest.id_state, Fest.id_city, Fest.id_event, "P", DateTime.Now);

                    }
                    otp.OTP = RandomString(4);
                    int otpid = db.Database.SqlQuery<int>("select id_otp from tbl_sul_fest_otp where id_event={0} and UID={1} ", Fest.id_event, Fest.UID).FirstOrDefault();
                    if (otpid > 0)
                    {
                        db.Database.ExecuteSqlCommand("delete from  tbl_sul_fest_otp where id_otp={0} ", otpid);
                        db.Database.ExecuteSqlCommand("insert into tbl_sul_fest_otp(UID,id_event,OTP,status,updated_date_time) values({0},{1},{2},{3},{4}) ", Fest.UID, Fest.id_event, otp.OTP, "A", DateTime.Now);


                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_sul_fest_otp(UID,id_event,OTP,status,updated_date_time) values({0},{1},{2},{3},{4}) ", Fest.UID, Fest.id_event, otp.OTP, "A", DateTime.Now);
                    }
                    if (pro.EMAIL != Fest.Email)
                    {
                        SendOTP(Fest.Email, otp.OTP, Fest.user_name);
                        result.Message = "OTP sent to your mail ID.";
                        db.Database.ExecuteSqlCommand("update  tbl_profile set EMAIL={0}, FIRSTNAME={1} where ID_USER={2}", Fest.Email, Fest.user_name, Fest.UID);
                        result.OTP_Status = "SENT";

                    }
                    else
                    {
                        result.Message = "Mail ID already present.";
                        result.OTP_Status = "DECLINED";
                        db.Database.ExecuteSqlCommand("update  tbl_sul_fest_event_registration set status={0} where UID={1} and id_event={2}", "A", Fest.UID, Fest.id_event);

                    }




                }

               
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
                string msg = "Hi "+ Name + ", <br/><br/>Here is your OTP for the email change request.<br/><br/>" + OTP+ "<br/><br/>Please do not share this OTP with anyone.<br/><br/>Thanks and Regards,<br/>Skillmuni Team";
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
