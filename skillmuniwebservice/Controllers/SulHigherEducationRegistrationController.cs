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
    public class SulHigherEducationRegistrationController : ApiController
    {
        public HttpResponseMessage Post([FromBody]HigherEduReg High)
        {
            HigherEduResponse result = new HigherEduResponse();
            //int id_register = 0;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    result.id_register = db.Database.SqlQuery<int>("insert into tbl_sul_higher_education_user_registration(id_higher_education,id_user,status,update_date_time,slot) values({0},{1},{2},{3},{4});select max(id_register) from tbl_sul_higher_education_user_registration", High.id_higher_education, High.id_user, "A", DateTime.Now, High.slot).FirstOrDefault();

                    tbl_sul_fest_event_registration reg = db.Database.SqlQuery<tbl_sul_fest_event_registration>("select * from tbl_sul_fest_event_registration where id_event={0} and UID={1}",High.id_event,High.id_user).FirstOrDefault();
                    if (reg == null)
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_sul_fest_event_registration(UID,id_college,id_state,id_city,id_event,status,updated_date_time) values({0},{1},{2},{3},{4},{5},{6}) ", High.id_user, 0, 0, 0, High.id_event, "A", DateTime.Now);


                    }

                    tbl_sul_higher_education_user_registration usereg = db.Database.SqlQuery<tbl_sul_higher_education_user_registration>("select * from tbl_sul_higher_education_user_registration where id_higher_education={0} and id_user={1}", High.id_higher_education, High.id_user).FirstOrDefault();

                    tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", High.id_user).FirstOrDefault();
                    tbl_sul_higher_education_master high = db.Database.SqlQuery<tbl_sul_higher_education_master>("select * from  tbl_sul_higher_education_master where id_higher_education={0} ", High.id_higher_education).FirstOrDefault();
                    tbl_sul_fest_master mas = db.Database.SqlQuery<tbl_sul_fest_master>("select * from  tbl_sul_fest_master where id_event={0} ", High.id_event).FirstOrDefault();

                    SendOTP(prof.EMAIL, prof.FIRSTNAME, high, usereg, mas);



                    result.Message = "Registered successfully.";
                    result.Status = "SUCCESS";




                }



            }
            catch (Exception e)
            {

                result.Message = "Something went wrong.";
                result.Status = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, result);




            }

            // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        public void SendOTP(string Semail, string Name, tbl_sul_higher_education_master sem, tbl_sul_higher_education_user_registration reg, tbl_sul_fest_master mas)
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



                string sub = "Your slot has been successfully booked!";
                string msg = "Dear " + Name + ",<br/><br/> Your slot has been booked. Below are the details of your booked slot;<br/><br/>" + "Registration ID: " + reg.id_register + "<br/><br/>" + "What:" + "HIGHER EDUCATION COUNSELING" + "<br/><br/>" + "Where: " + sem.location + "<br/><br/>" + "When:" + mas.event_start_date + "|Slot- " + reg.slot + "<br/><br/>" + "Thanks and Regards,<br/>" + "Skillmuni Team";


                //string notelink = ConfigurationManager.AppSettings["content_link"];



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
