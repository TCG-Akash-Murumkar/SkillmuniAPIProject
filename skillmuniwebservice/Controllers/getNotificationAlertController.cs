using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class getNotificationAlertController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int configid, int userid, int orgid)
        {

            APIRESPONSE responce = new APIRESPONSE();
            NotificationAlert alert = new NotificationAlert();
            tbl_user user = db.tbl_user.Where(t => t.ID_USER == userid).FirstOrDefault();
            if (user != null)
            {
                tbl_notification_config config = new tbl_notification_config();
                config = db.tbl_notification_config.Where(t => t.id_user == user.ID_USER && t.id_notification_config == configid ).FirstOrDefault();
                if (config != null)
                {
                    if (config.status == "A")
                    {
                        config.status = "R";
                        config.read_timestamp = System.DateTime.Now;
                        db.SaveChanges();
                    }
                    tbl_notification notify = db.tbl_notification.Where(t => t.id_notification == config.id_notification).FirstOrDefault();
                    if (notify != null)
                    {
                        NotificationAlert temp = new NotificationAlert();
                        temp.NOTIFICATION_ID = notify.id_notification;
                        temp.NOTIFICATION_KEY = notify.notification_key;
                        temp.NOTIFICATION_TITLE = notify.notification_name;
                        temp.NOTIFICATION_DESCRIPTION = notify.notification_description;
                        temp.NOTIFICATION_MESSAGE = notify.notification_message;
                        temp.START_DATE = notify.start_date.Value.ToShortDateString();
                        temp.END_DATE = notify.end_date.Value.ToShortDateString();
                        if (config.notification_action_type == "CON")
                        {
                            temp.ACTION_TYPE = "CON";
                            temp.NOTIFICATION_TYPE = "Content Specific Notification - Content";
                            temp.REDIRECTION_URL = "api/GetContentDetails?conId=" + config.id_content + "&userid=" + config.id_user + "&orgid=" + orgid;
                        }
                        else if (config.notification_action_type == "PRO")
                        {
                            temp.ACTION_TYPE = "PRO";
                            temp.NOTIFICATION_TYPE = "Content Specific Notification - Program";
                            temp.REDIRECTION_URL = "api/getCategoryFromNotification?catid=" + config.id_category + "&userid=" + config.id_user + "&orgid=" + orgid;
                        }
                        else if (config.notification_action_type == "ASS")
                        {
                            temp.ACTION_TYPE = "ASS";
                            temp.NOTIFICATION_TYPE = "Content Specific Notification - Assessment";
                            temp.REDIRECTION_URL = "api/Assessmentsheet?ASID=" + config.id_assessment + "&UID=" + config.id_user + "&OID=" + orgid;
                        }
                        else if (config.notification_action_type == "GEN")
                        {
                            temp.ACTION_TYPE = "GEN";
                            temp.NOTIFICATION_TYPE = "Generic Notification";
                            temp.REDIRECTION_URL = "NA";
                        }
                        else if (config.notification_action_type == "GENCON")
                        {
                            temp.ACTION_TYPE = "GENCON";
                            temp.NOTIFICATION_TYPE = "Generic Notification With Content";
                            temp.REDIRECTION_URL = "api/GetContentDetails?conId=" + config.id_content + "&userid=" + config.id_user + "&orgid=" + orgid;
                        }
                        else if (config.notification_action_type == "GENASS")
                        {
                            temp.ACTION_TYPE = "GENASS";
                            temp.NOTIFICATION_TYPE = "Generic Notification with Assessment";
                            temp.REDIRECTION_URL = "api/Assessmentsheet?ASID=" + config.id_assessment + "&UID=" + config.id_user + "&OID=" + orgid;
                        }

                        responce.KEY = "SUCCESS";
                        string resJson = JsonConvert.SerializeObject(temp);
                        responce.MESSAGE = resJson;
                    }
                    else
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "Invalid Notification";
                    }
                }
                else
                {
                    responce.KEY = "FAILURE";
                    responce.MESSAGE = "Invalid Notification / Notification Expired ";
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }

    }
}
