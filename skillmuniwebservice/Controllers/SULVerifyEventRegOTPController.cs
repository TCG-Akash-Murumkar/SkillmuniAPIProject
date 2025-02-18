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
    public class SULVerifyEventRegOTPController : ApiController
    {
        public HttpResponseMessage Post([FromBody]VerifyOTP OTP)
        {
            VerifyOTPResponse result = new VerifyOTPResponse();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            tbl_sul_fest_master fest = new tbl_sul_fest_master();
            tbl_sul_fest_event_registration reg = new tbl_sul_fest_event_registration();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_sul_fest_otp otpmas = db.Database.SqlQuery<tbl_sul_fest_otp>("select * from tbl_sul_fest_otp where id_event={0} and UID={1} ", OTP.id_event, OTP.UID).FirstOrDefault();
                    reg = db.Database.SqlQuery<tbl_sul_fest_event_registration>("select * from tbl_sul_fest_event_registration where UID={0} and id_event={1} ", OTP.UID, OTP.id_event).FirstOrDefault();
                    int expmins = db.Database.SqlQuery<int>("select expiry_mins from tbl_sul_fest_otp_expiry_config where status={0}", "A").FirstOrDefault();
                    DateTime tdy = DateTime.Now;
                    tdy = tdy.AddMinutes(expmins);
                    if (otpmas.OTP == OTP.OTP && otpmas.updated_date_time <= tdy)
                    {



                        //db.Database.ExecuteSqlCommand("delete from  tbl_sul_fest_otp where id_otp={0} ", otpmas);
                        db.Database.ExecuteSqlCommand("update tbl_sul_fest_event_registration set status={0} where UID={1} and id_event={2}", "A", OTP.UID, OTP.id_event);
                        result.Message = "OTP verified successfully.";
                        result.Status = "SUCCESS";
                        string tomail = db.Database.SqlQuery<string>("SELECT EMAIL FROM tbl_profile where ID_USER={0}", OTP.UID).FirstOrDefault();
                        string name = db.Database.SqlQuery<string>("SELECT FIRSTNAME FROM tbl_profile where ID_USER={0}", OTP.UID).FirstOrDefault();
                        List<tbl_user_gcm_log> gcm = new List<tbl_user_gcm_log>();
                        gcm = db.Database.SqlQuery<tbl_user_gcm_log>("select * from tbl_user_gcm_log where id_user={0} and status='A'", OTP.UID).ToList();
                        fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master where id_event={0}", OTP.id_event).FirstOrDefault();

                        foreach (var itm in gcm)
                        {
                            SendNotification(itm.GCMID, "Successfully registered for the event '" + fest.event_title + "'", fest.event_title, ConfigurationManager.AppSettings["FestEventLogo"].ToString() + fest.event_logo);
                        }
                        SendOTP(tomail, name, fest, reg);



                    }
                    else
                    {
                        if (otpmas.OTP == OTP.OTP)
                        {
                            result.Message = "OTP expired. Please click on resend and verify again.";
                            result.Status = "FAILED";

                        }
                        else
                        {
                            result.Message = "Entered OTP is wrong.";
                            result.Status = "FAILED";

                        }

                    }



                }



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


        public void SendOTP(string Semail, string Name, tbl_sul_fest_master fes, tbl_sul_fest_event_registration reg)
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

                string mailpage = ConfigurationManager.AppSettings["mail_sul_event_reg_thanks"];
                //string notelink = ConfigurationManager.AppSettings["content_link"];

                //string mailpage = "";
                using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                {
                    body = reader.ReadToEnd();
                }
                string college = "";
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                {
                     college = db.Database.SqlQuery<string>("select college_name from tbl_college_list where id_college={0}", fes.id_college).FirstOrDefault();


                }




                body = body.Replace("{ID}", Convert.ToString(reg.id_event));
                body = body.Replace("{TITLE}", fes.event_title);
                body = body.Replace("{COLLEGE}", college);
                body = body.Replace("{ADDRESS}", fes.address);
                body = body.Replace("{STRT_DATE}", Convert.ToString(fes.event_start_date));
                body = body.Replace("{END_DATE}", Convert.ToString(fes.event_end_date));

                string sub = "SUL Event registered Successfully";
                string msg = "Hi " + Name + ",<br/><br/> You have been successfully registered  for  Skillmuni University Event. ";


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
                MailMessage message = new MailMessage(senderID, recmail, sub, body);//body replaced by msg
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string SendNotification(string deviceRegIds, string message, string title, string image)
        {
            string responseLine = "";
            try
            {
                string GoogleAppID = "AAAAGrnsAbc:APA91bH3oHyM5R0KrFxEexkW-DmnOr5HD1oyKmsmP_nlUjNEdlmAUu1ZF7YuD3y8NGmMx_760dgynH1hYw603TN7CAnpgD4yp59dUFOMi198H42RweTvKHYEwfVzdusHMMZuKnRvjXUW";
                var SENDER_ID = "114788401591";


                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentType = "application/json";
                NotificationData dat = new NotificationData();

                var payload = new
                {
                    to = deviceRegIds,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        body = message,
                        title = title,

                        icon = image,


                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return responseLine;
        }


    }
}
