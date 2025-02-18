using m2ostnextservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class getNotificationListController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int userid, int orgid)
        {
            APIRESPONSE responce = new APIRESPONSE();
            NotificationList alertList = new NotificationList();
            DateTime current = System.DateTime.Now;
            tbl_user user = db.tbl_user.Where(t => t.ID_USER == userid).FirstOrDefault();
            if (user != null)
            {
                List<tbl_notification_config> config = new List<tbl_notification_config>();
                config = db.tbl_notification_config.Where(t => t.id_user == user.ID_USER && t.status == "A").ToList();
                if (config != null)
                {
                    config = config.OrderByDescending(c => c.updated_date_time.Value.Date).ThenBy(c => c.updated_date_time.Value.TimeOfDay).ToList();

                    List<Notification> unRead = new List<Notification>();
                    foreach (tbl_notification_config item in config)
                    {
                        tbl_notification notify = db.tbl_notification.Where(t => t.id_notification == item.id_notification).FirstOrDefault();
                        if (notify != null)
                        {
                            Notification temp = new Notification();
                            temp.NOTIFICATION_ID = notify.id_notification;
                            if (notify.notification_type == 1)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification";
                            }
                            else if (notify.notification_type == 2)
                            {
                                temp.NOTIFICATION_TYPE = "Event Based Motification";
                            }
                            else if (notify.notification_type == 3)
                            {
                                temp.NOTIFICATION_TYPE = "Content Specific Notification";
                            }
                            else if (notify.notification_type == 4)
                            {
                                temp.NOTIFICATION_TYPE = "Reminder Notification";
                            }
                            else if (notify.notification_type == 5)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification with Content";
                            }
                            else if (notify.notification_type == 7)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification with Assessment";
                            }
                            if (item.notification_action_type == "CON")
                            {
                                tbl_content_user_assisgnment userAss = db.tbl_content_user_assisgnment.Where(t => t.id_content == item.id_content && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == userAss.id_content).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CONTENT_QUESTION;
                                }
                            }
                            else if (item.notification_action_type == "PRO")
                            {
                                tbl_content_program_mapping userAss = db.tbl_content_program_mapping.Where(t => t.id_category == item.id_category && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_category content = db.tbl_category.Where(t => t.ID_CATEGORY == userAss.id_category).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CATEGORYNAME;
                                }
                            }
                            else if (item.notification_action_type == "ASS")
                            {
                                tbl_assessment_user_assignment userAss = db.tbl_assessment_user_assignment.Where(t => t.id_assessment == item.id_assessment && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expire_date.Value.ToString("dd-MM-yyyy");
                                    tbl_assessment content = db.tbl_assessment.Where(t => t.id_assessment == userAss.id_assessment).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.assessment_title;
                                }
                            }
                            else if (item.notification_action_type == "GENCON")
                            {
                                tbl_content_user_assisgnment userAss = db.tbl_content_user_assisgnment.Where(t => t.id_content == item.id_content && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == userAss.id_content).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CONTENT_QUESTION;
                                }
                            }
                            else if (item.notification_action_type == "GENASS")
                            {
                                tbl_assessment_user_assignment userAss = db.tbl_assessment_user_assignment.Where(t => t.id_assessment == item.id_assessment && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expire_date.Value.ToString("dd-MM-yyyy");
                                    tbl_assessment content = db.tbl_assessment.Where(t => t.id_assessment == userAss.id_assessment).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.assessment_title;
                                }
                            }
                            else
                            {
                                temp.EXPIRYDATE = "";
                                temp.NOTIFICATION_MESSAGE = notify.notification_message;
                            }
                            temp.SENTDATE = item.updated_date_time.Value.ToString("dd-MM-yyyy");
                            temp.NOTIFICATION_CONFIG_ID = item.id_notification_config;
                            temp.NOTIFICATION_KEY = notify.notification_key;
                            temp.NOTIFICATION_TITLE = notify.notification_name;
                            temp.NOTIFICATION_DESCRIPTION = notify.notification_description;
                            temp.START_DATE = notify.start_date.Value.ToShortDateString();

                            temp.END_DATE = notify.end_date.Value.ToShortDateString();
                            unRead.Add(temp);
                        }
                    }
//                    unRead = unRead.OrderByDescending(t => t.SENTDATE).ToList();
                    alertList.UNREAD = unRead;
                }


                List<tbl_notification_config> configRead = new List<tbl_notification_config>();
                configRead = db.tbl_notification_config.Where(t => t.id_user == user.ID_USER && t.status == "R").ToList();
                if (configRead != null)
                {
                    configRead = configRead.OrderByDescending(c => c.updated_date_time.Value.Date).ThenBy(c => c.updated_date_time.Value.TimeOfDay).ToList();

                    List<Notification> read = new List<Notification>();
                    foreach (tbl_notification_config item in configRead)
                    {
                        tbl_notification notify = db.tbl_notification.Where(t => t.id_notification == item.id_notification).FirstOrDefault();
                        if (notify != null)
                        {

                            Notification temp = new Notification();
                            temp.NOTIFICATION_ID = notify.id_notification;
                           
                            if (notify.notification_type == 1)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification";
                            }
                            else if (notify.notification_type == 2)
                            {
                                temp.NOTIFICATION_TYPE = "Event Based Motification";
                            }
                            else if (notify.notification_type == 3)
                            {
                                temp.NOTIFICATION_TYPE = "Content Specific Notification";
                            }
                            else if (notify.notification_type == 4)
                            {
                                temp.NOTIFICATION_TYPE = "Reminder Notification";
                            }
                            else if (notify.notification_type == 5)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification with Content";
                            }
                            else if (notify.notification_type == 7)
                            {
                                temp.NOTIFICATION_TYPE = "Generic Notification with Assessment";
                            }
                            if (item.notification_action_type == "CON")
                            {
                                tbl_content_user_assisgnment userAss = db.tbl_content_user_assisgnment.Where(t => t.id_content == item.id_content && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == userAss.id_content).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CONTENT_QUESTION;
                                }

                            }
                            else if (item.notification_action_type == "PRO")
                            {
                                tbl_content_program_mapping userAss = db.tbl_content_program_mapping.Where(t => t.id_category == item.id_category && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_category content = db.tbl_category.Where(t => t.ID_CATEGORY == userAss.id_category).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CATEGORYNAME;
                                }
                            }
                            else if (item.notification_action_type == "ASS")
                            {
                                tbl_assessment_user_assignment userAss = db.tbl_assessment_user_assignment.Where(t => t.id_assessment == item.id_assessment && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expire_date.Value.ToString("dd-MM-yyyy");
                                    tbl_assessment content = db.tbl_assessment.Where(t => t.id_assessment == userAss.id_assessment).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.assessment_title;
                                }
                            }
                            else if (item.notification_action_type == "GENCON")
                            {
                                tbl_content_user_assisgnment userAss = db.tbl_content_user_assisgnment.Where(t => t.id_content == item.id_content && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expiry_date.Value.ToString("dd-MM-yyyy");
                                    tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == userAss.id_content).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.CONTENT_QUESTION;
                                }
                            }
                            else if (item.notification_action_type == "GENASS")
                            {
                                tbl_assessment_user_assignment userAss = db.tbl_assessment_user_assignment.Where(t => t.id_assessment == item.id_assessment && t.id_user == user.ID_USER && t.id_organization == orgid).FirstOrDefault();
                                if (userAss != null)
                                {
                                    temp.EXPIRYDATE = userAss.expire_date.Value.ToString("dd-MM-yyyy");
                                    tbl_assessment content = db.tbl_assessment.Where(t => t.id_assessment == userAss.id_assessment).FirstOrDefault();
                                    temp.NOTIFICATION_MESSAGE = notify.notification_message + " - " + content.assessment_title;
                                }
                            }
                            else
                            {
                                temp.EXPIRYDATE = "";
                                temp.NOTIFICATION_MESSAGE = notify.notification_message;
                            }
                            temp.SENTDATE = item.updated_date_time.Value.ToString("dd-MM-yyyy");
                            temp.NOTIFICATION_CONFIG_ID = item.id_notification_config;
                            temp.NOTIFICATION_KEY = notify.notification_key;
                            temp.NOTIFICATION_TITLE = notify.notification_name;
                            temp.NOTIFICATION_DESCRIPTION = notify.notification_description;
                            temp.START_DATE = notify.start_date.Value.ToShortDateString();

                            temp.END_DATE = notify.end_date.Value.ToShortDateString();
                            read.Add(temp);
                        }
                    }
                   // read = read.OrderByDescending(t => t.SENTDATE).ToList();
                    alertList.READ = read;
                }

                responce.KEY = "SUCCESS";
                string resJson = JsonConvert.SerializeObject(alertList);
                responce.MESSAGE = resJson;

            }
            else
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Invalid User";
            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }

    }
}
