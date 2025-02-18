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
    public class TriggerSulFestInAppInvitaionController : ApiController
    {
        public HttpResponseMessage Get(int id_event)
        {
            string Result = "";

            EventsHeader Res = new EventsHeader();



            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    
                    tbl_sul_fest_master fest = new tbl_sul_fest_master();
                    fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master where id_event={0}", id_event).FirstOrDefault();



                    List<tbl_user_gcm_log> fcmList = new List<tbl_user_gcm_log>();
                    fcmList = db.Database.SqlQuery<tbl_user_gcm_log>("select * from tbl_user_gcm_log where status='A'").ToList();
                    SendEventPushNotification(fcmList, fest);

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



        public void SendEventPushNotification(List<tbl_user_gcm_log> fcm, tbl_sul_fest_master fes)
        {
            try
            {
                string GoogleAppID = "AAAAGrnsAbc:APA91bH3oHyM5R0KrFxEexkW-DmnOr5HD1oyKmsmP_nlUjNEdlmAUu1ZF7YuD3y8NGmMx_760dgynH1hYw603TN7CAnpgD4yp59dUFOMi198H42RweTvKHYEwfVzdusHMMZuKnRvjXUW";
                var SENDER_ID = "114788401591";


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    if (fes.is_college_restricted == 1)
                    {
                        string college = db.Database.SqlQuery<string>("select college_name from tbl_college_list where id_college={0}", fes.id_college).FirstOrDefault();
                        foreach (var itm in fcm)
                        {
                            tbl_profile prof = new tbl_profile();

                            prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                            if (college == prof.COLLEGE)
                            {

                                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                                tRequest.Method = "post";
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
                                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                                tRequest.ContentType = "application/json";
                                //NotificationData dat = new NotificationData();
                                //dat.Image = image;
                                
                                var payload = new
                                {
                                    to = itm.GCMID,
                                    priority = "high",
                                    content_available = true,
                                    notification = new
                                    {
                                        body = fes.event_title+ fes.event_objective,
                                        title ="SULFest" ,
                                        badge = 1,
                                        icon = fes.city,
                                        color = ConfigurationManager.AppSettings["SULeventlogo"].ToString() + fes.event_logo
                                        //sound = currency,
                                        //tag = tag


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


                        }

                    }
                    else
                    {
                        foreach (var itm in fcm)
                        {


                            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                            tRequest.Method = "post";
                            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
                            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                            tRequest.ContentType = "application/json";
                            //NotificationData dat = new NotificationData();
                            //dat.Image = image;
                            var payload = new
                            {
                                to = itm.GCMID,
                                priority = "high",
                                content_available = true,
                                notification = new
                                {
                                    body = fes.event_objective,
                                    title = fes.event_title,
                                    badge = 1,
                                    icon = fes.city
                                    //color = eligiblescore,
                                    //sound = currency,
                                    //tag = tag


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
