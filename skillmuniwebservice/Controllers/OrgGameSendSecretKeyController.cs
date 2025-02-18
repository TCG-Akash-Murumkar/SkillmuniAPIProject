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
    public class OrgGameSendSecretKeyController : ApiController
    {
       
            public HttpResponseMessage Get(string userId)
            {
            SendKeyResult result = new SendKeyResult();
            try
            {
                using (M2ostCatDbContext db = new M2ostCatDbContext())
                {
                    string OTP = RandomString(4);
                    tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_user as a inner join tbl_profile as b on a.ID_USER=b.ID_USER where a.USERID={0} and a.STATUS='A'", userId).FirstOrDefault();
                    tbl_email_verification_key_log ver_key = db.Database.SqlQuery<tbl_email_verification_key_log>("select * from tbl_email_verification_key_log where id_user={0} and status='P' ", prof.ID_USER).FirstOrDefault();
                    if (ver_key == null)
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_email_verification_key_log (id_user,secret_key,updated_date_time,status) values({0},{1},{2},{3})", prof.ID_USER, OTP, DateTime.Now, "P");


                    }
                    else
                    {
                        OTP = ver_key.secret_key;
                    }
                    SendOTP(prof.EMAIL, prof.FIRSTNAME + " " + prof.LASTNAME, OTP);
                    result.STATUS = "SUCCESS";
                    result.MESSAGE = "Secret Key has been sent to your registered email ID.";


                }

            }
            catch (Exception e)
            {
                result.STATUS = "FAILED";
                result.MESSAGE = "Something went wrong.";
            }

               


                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

        

        public void SendOTP(string Semail, string Name, string OTP)
        {
            try
            {
                /*  Email ID changed on requst on 08-01-2020
                string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                const string senderPassword = "Pls@210312"; // sender password here…
                */

                string senderID = "playtolearn@thegamificationcompany.com";// use sender’s email id here..
                const string senderPassword = "TGC@03012020"; // sender password here…


                string recmail = Semail;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
                string body = string.Empty;



                string sub = "Email Verification - TGC";
                string msg = "Dear " + Name + ",<br/><br/> Please use the secret key " + OTP + " to verify your email id. <br/><br/> Thanks and Regards,<br/>" + "The Gamification Company";


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


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
