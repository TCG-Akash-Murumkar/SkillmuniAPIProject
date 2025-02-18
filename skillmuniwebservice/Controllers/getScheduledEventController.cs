using m2ostnextservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Net.Mail;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;


namespace m2ostnextservice.Controllers
{
    public class getScheduledEventListController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]EventUser USER)
        {
            DateTime current = System.DateTime.Now;
            APIRESPONSE responce = new APIRESPONSE();
            List<EventThumbnail> Read = new List<EventThumbnail>();
            List<EventThumbnail> Unread = new List<EventThumbnail>();
            EventResponse responsebody = new EventResponse();

            string dateset = "";
            string additional = "";
            // eDstr = " and a.UPDATED_DATE_TIME <= STR_TO_DATE('" + endate + "', '%d-%m-%Y')";
            if (USER.DNO != "0")
            {
                additional = " and event_start_datetime LIKE '" + USER.YNO + "-" + USER.MNO + "-" + USER.DNO + "%'";
            }
            else if (USER.MNO != "0")
            {
                additional = " and event_start_datetime LIKE '" + USER.YNO + "-" + USER.MNO + "%'";
            }

            string eSql = "select * from tbl_scheduled_event_subscription_log where id_user=" + USER.UID + " and id_organization=" + USER.OID + " and id_scheduled_event in (select id_scheduled_event from tbl_scheduled_event where status in ('A','X') " + additional + ")";
            List<tbl_scheduled_event_subscription_log> log = db.tbl_scheduled_event_subscription_log.SqlQuery(eSql).ToList();
            foreach (tbl_scheduled_event_subscription_log item in log)
            {
                tbl_scheduled_event iEvent = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == item.id_scheduled_event).FirstOrDefault();
                if (iEvent != null)
                {
                    EventThumbnail temp = new EventThumbnail();
                    temp.id_scheduled_event = iEvent.id_scheduled_event;
                    temp.event_description = iEvent.event_description;
                    // temp.event_duration = iEvent.event_duration;
                    temp.event_start_datetime = iEvent.event_start_datetime.Value.ToString("dd-MM-yyyy HH:mm");
                    temp.event_title = iEvent.event_title;
                    // temp.program_description = iEvent.program_description;
                    // temp.program_location = iEvent.program_location;
                    temp.program_name = iEvent.program_name;
                    //  temp.facilitator_name = iEvent.facilitator_name;
                    //  temp.facilitator_organization = iEvent.facilitator_organization;
                    temp.program_objective = iEvent.program_objective;
                    temp.registration_start_date = iEvent.registration_start_date.Value.ToString("dd-MM-yyyy HH:mm");
                    temp.registration_end_date = iEvent.registration_end_date.Value.ToString("dd-MM-yyyy HH:mm");
                    //  temp.no_of_participants = iEvent.no_of_participants.ToString();
                    if (iEvent.event_type == 1)
                    {
                        temp.event_type = "Open";
                    }
                    else if (iEvent.event_type == 2)
                    {
                        temp.event_type = "Closed";
                    }
                    temp.attachment_info = "";
                    if (iEvent.event_group_type == 1)
                    {
                        temp.event_group_type = "Face to Face";
                    }
                    else if (iEvent.event_group_type == 2)
                    {
                        temp.event_group_type = "Online";
                    }
                    else if (iEvent.event_group_type == 3)
                    {
                        temp.event_group_type = "M2OST";
                        if (iEvent.attachment_type == 1)
                        {
                            temp.attachment_info = "Program is attached";
                        }
                        else if (iEvent.attachment_type == 2)
                        {
                            temp.attachment_info = "Attachment is attached";
                        }
                    }


                    if (iEvent.status == "X")
                    {
                        temp.STATUS = "X";
                        temp.MESSAGE = "Event has been cancelled.";
                        temp.COMMENT = iEvent.event_comment;
                    }
                    else if (item.subscription_status == "A")
                    {
                        temp.STATUS = "A";
                        temp.MESSAGE = "You have subscribed to the event.";
                        temp.COMMENT = "";
                    }
                    else if (item.subscription_status == "R")
                    {
                        temp.STATUS = "R";
                        temp.MESSAGE = "You have declined the invitation to the event.";
                        temp.COMMENT = item.event_user_comment;
                    }
                    else if (item.subscription_status == "C")
                    {
                        temp.STATUS = "R";
                        temp.MESSAGE = "Your manager has rejected your request.";
                        temp.COMMENT = item.event_user_comment;
                    }
                    else if (item.subscription_status == "P")
                    {
                        if (current > iEvent.event_start_datetime)
                        {
                            temp.STATUS = "W";
                            temp.MESSAGE = "This event has been completed. You are put on waiting List .";
                            temp.COMMENT = "";
                        }
                        else
                        {
                            temp.STATUS = "P";
                            temp.MESSAGE = "Your subscription request is pending for approval.";
                            temp.COMMENT = "";
                        }

                    }
                    else if (item.subscription_status == "O")
                    {
                        if (current > iEvent.event_start_datetime)
                        {
                            temp.STATUS = "E";
                            temp.MESSAGE = "This event has been completed.";
                            temp.COMMENT = "";
                        }
                        else if (current > iEvent.registration_end_date)
                        {
                            temp.STATUS = "T";
                            temp.MESSAGE = "Registration to the event is closed.";
                            temp.COMMENT = "";
                        }
                        else
                        {
                            temp.STATUS = "O";
                            temp.MESSAGE = "You have not yet subscribed to the event.";
                            temp.COMMENT = "";
                        }
                    }
                    else if (item.subscription_status == "L")
                    {
                        temp.STATUS = "L";
                        temp.MESSAGE = "Participant limit of " + iEvent.no_of_participants + " is already full for the event.";
                        temp.COMMENT = "";
                    }

                    if (item.status == "S")
                    {
                        Unread.Add(temp);
                    }
                    else if (item.status == "R")
                    {
                        Read.Add(temp);
                    }


                    /*loop*/
                }

            }

            responsebody.READ = Read;
            responsebody.UNREAD = Unread;

            responce.KEY = "SUCCESS";
            string resJson = JsonConvert.SerializeObject(responsebody);
            responce.MESSAGE = resJson;


            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }

    public class getScheduledEventController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]EventData USER)
        {
            DateTime current = System.DateTime.Now;
            APIRESPONSE responce = new APIRESPONSE();
            ScheduledEvent temp = new ScheduledEvent();

            tbl_scheduled_event_subscription_log item = db.tbl_scheduled_event_subscription_log.Where(t => t.id_scheduled_event == USER.EID && t.id_user == USER.UID).FirstOrDefault();
            if (item != null)
            {
                tbl_scheduled_event iEvent = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == item.id_scheduled_event && t.status != "P").FirstOrDefault();
                if (iEvent != null)
                {
                    item.status = "R";
                    db.SaveChanges();

                    temp.id_scheduled_event = iEvent.id_scheduled_event;
                    temp.event_description = iEvent.event_description;
                    temp.event_duration = iEvent.event_duration;
                    temp.event_start_datetime = iEvent.event_start_datetime.Value.ToString("dd-MM-yyyy HH:mm");
                    temp.event_title = iEvent.event_title;
                    if (iEvent.event_group_type == 1)
                    {
                        temp.program_description = iEvent.event_additional_info;
                    }
                    else if (iEvent.event_group_type == 2)
                    {
                        temp.REDIRECTION_URL = iEvent.event_online_url;
                    }
                    else if (iEvent.event_group_type == 3)
                    {
                        string attachment = "";
                        if (iEvent.attachment_type == 1)
                        {
                            tbl_category program = db.tbl_category.Where(t => t.ID_CATEGORY == iEvent.id_program && t.STATUS == "A" && t.CATEGORY_TYPE == 0).FirstOrDefault();
                            tbl_category_tiles tProgram = db.tbl_category_tiles.Where(t => t.id_category_tiles == iEvent.id_category_tile && t.status == "A").FirstOrDefault();
                            tbl_category_heading hProgram = db.tbl_category_heading.Where(t => t.id_category_heading == iEvent.id_category_heading && t.status == "A").FirstOrDefault();

                            if (program != null)
                            {
                                attachment = program.CATEGORYNAME + " [Tile : " + tProgram.tile_heading + " , Heading : " + hProgram.Heading_title + " ]";
                            }
                        }
                        else if (iEvent.attachment_type == 2)
                        {
                            tbl_assessment assess = db.tbl_assessment.Where(t => t.id_assessment == iEvent.id_assessment && t.status == "A").FirstOrDefault();
                            tbl_category program = db.tbl_category.Where(t => t.ID_CATEGORY == iEvent.id_category && t.STATUS == "A").FirstOrDefault();
                            if (assess != null)
                            {
                                attachment = assess.assessment_title + " [ " + program.CATEGORYNAME + " ]";
                            }
                        }
                        temp.program_description = attachment;
                    }


                    temp.program_location = iEvent.program_venue + " , " + iEvent.program_location;
                    temp.program_name = iEvent.program_name;
                    temp.facilitator_name = iEvent.facilitator_name;
                    temp.facilitator_organization = iEvent.facilitator_organization;
                    temp.program_objective = iEvent.program_objective;

                    temp.is_approval = iEvent.is_approval;
                   temp.is_response = iEvent.is_response;

                   // temp.is_approval = iEvent.is_response;
                   // temp.is_response = iEvent.is_approval;

                    temp.is_unsubscribe = iEvent.is_unsubscribe;

                    if (iEvent.program_duration_type == 1)
                    {
                        temp.program_duration_type = "OPEN";
                        temp.program_duration = "";
                    }
                    else if (iEvent.program_duration_type == 2)
                    {
                        temp.program_duration_type = "CLOSED";

                        if (iEvent.program_duration_unit == "H")
                        {
                            temp.program_duration = iEvent.program_end_date + " ( " + iEvent.program_duration + " Hour )";
                        }
                        else if (iEvent.program_duration_unit == "D")
                        {
                            temp.program_duration = iEvent.program_end_date + " ( " + iEvent.program_duration + " Day )";
                        }
                        else if (iEvent.program_duration_unit == "W")
                        {
                            temp.program_duration = iEvent.program_end_date + " ( " + iEvent.program_duration + " Week )";
                        }
                        else if (iEvent.program_duration_unit == "M")
                        {
                            temp.program_duration = iEvent.program_end_date + " ( " + iEvent.program_duration + " Month )";
                        }
                    }

                    temp.registration_start_date = iEvent.registration_start_date.Value.ToString("dd-MM-yyyy HH:mm");
                    temp.registration_end_date = iEvent.registration_end_date.Value.ToString("dd-MM-yyyy HH:mm");
                    temp.no_of_participants = iEvent.no_of_participants.ToString();
                    if (iEvent.event_type == 1)
                    {
                        temp.event_type = "Open";
                    }
                    else if (iEvent.event_type == 2)
                    {
                        temp.event_type = "Closed";
                    }
                    temp.attachment_info = "";
                    temp.attachment_type = "0";
                    if (iEvent.event_group_type == 1)
                    {
                        temp.event_group_type = "Face to Face";
                    }
                    else if (iEvent.event_group_type == 2)
                    {
                        temp.event_group_type = "Online";
                    }
                    else if (iEvent.event_group_type == 3)
                    {
                        temp.attachment_type = "1";
                        temp.event_group_type = "M2OST";
                        if (iEvent.attachment_type == 1)
                        {
                            temp.REDIRECTION_URL = "api/getCategoryDashboard?catid=" + iEvent.id_program + "&userid=" + USER.UID + "&orgid=" + USER.OID;
                            string attachment_info = "Program is attached : ";
                            tbl_category program = db.tbl_category.Where(t => t.ID_CATEGORY == iEvent.id_program).FirstOrDefault();
                            if (program != null)
                            {
                                temp.attachment_info = attachment_info + program.CATEGORYNAME;
                            }
                        }
                        else if (iEvent.attachment_type == 2)
                        {
                            temp.REDIRECTION_URL = "api/Assessmentsheet?ASID=" + iEvent.id_assessment + "&UID=" + USER.UID + "&OID=" + USER.OID;
                            temp.attachment_info = "Attachment is attached ";
                        }
                    }

                    if (iEvent.status == "X")
                    {
                        temp.STATUS = "X";
                        temp.MESSAGE = "Event has been canceled.";
                        temp.COMMENT = iEvent.event_comment;
                    }
                    else if (item.subscription_status == "A")
                    {
                        if (current > iEvent.event_start_datetime)
                        {
                            temp.STATUS = "E";
                            temp.MESSAGE = "This event has been completed.";
                            temp.COMMENT = "";
                        }
                        else
                        {
                            temp.STATUS = "A";
                            temp.MESSAGE = "You have subscribed to the event.";
                            temp.COMMENT = "";
                        }
                    }
                    else if (item.subscription_status == "R")
                    {
                        temp.STATUS = "R";
                        temp.MESSAGE = "You have declined the invitation to the event.";
                        temp.COMMENT = item.event_user_comment;
                    }
                    else if (item.subscription_status == "C")
                    {
                        temp.STATUS = "C";
                        temp.MESSAGE = "Your manager has rejected your request.";
                        temp.COMMENT = item.event_user_comment;
                    }
                    else if (item.subscription_status == "P")
                    {
                        temp.STATUS = "P";
                        temp.MESSAGE = "Your subscription request is still pending.";
                        temp.COMMENT = "";
                    }
                    else if (item.subscription_status == "O")
                    {
                        if (current > iEvent.event_start_datetime)
                        {
                            temp.STATUS = "E";
                            temp.MESSAGE = "This event has been completed.";
                            temp.COMMENT = "";
                        }
                        else if (current > iEvent.registration_end_date)
                        {
                            temp.STATUS = "T";
                            temp.MESSAGE = "Registration to the event is closed.";
                            temp.COMMENT = "";
                        }
                        else
                        {
                            temp.STATUS = "O";
                            temp.MESSAGE = "You have not yet subscribed to the event.";
                            temp.COMMENT = "";
                        }

                    }
                    else if (item.subscription_status == "L")
                    {
                        temp.STATUS = "L";
                        temp.MESSAGE = "Participant limit of " + iEvent.no_of_participants + " is already full.";
                        temp.COMMENT = "";
                    }

                    /*loop*/
                }
                else
                {
                    responce.KEY = "FAILURE";
                    responce.MESSAGE = "Could not find the Event in the System. Event might be deleted .Please contact your Repoting Manager. ";
                }
                responce.KEY = "SUCCESS";
                string resJson = JsonConvert.SerializeObject(temp);
                responce.MESSAGE = resJson;
            }
            else
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Could not find the Event in the System. Event might be deleted .Please contact your Repoting Manager.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }


    public class setScheduledEventSubscrioptionController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody]EventSubscription USER)
        {
            APIRESPONSE responce = new APIRESPONSE();
            tbl_scheduled_event_subscription_log item = db.tbl_scheduled_event_subscription_log.Where(t => t.id_scheduled_event == USER.EID && t.id_user == USER.UID).FirstOrDefault();
            tbl_user uData = db.tbl_user.Where(t => t.ID_USER == USER.UID && t.STATUS == "A").FirstOrDefault();
            if (uData == null)
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Could not Find the User.";
                return Request.CreateResponse(HttpStatusCode.OK, responce);
            }
            else
            {
                if (item != null)
                {
                    tbl_scheduled_event iEvent = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == item.id_scheduled_event && t.status == "A").FirstOrDefault();
                    int pCount = db.tbl_scheduled_event_subscription_log.Where(t => t.id_scheduled_event == USER.EID).Count(t => t.subscription_status == "A");

                    if (iEvent != null)
                    {
                        item.event_user_response = USER.OPT;
                        item.event_user_response_timestamp = System.DateTime.Now;

                        if (iEvent.event_type == 1)
                        {
                            string particiapnts = iEvent.participant_level;
                            string[] pList = particiapnts.Split(',');
                            for (int i = 0; i < pList.Length; i++)
                            {
                                pList[i] = "'" + pList[i].ToUpper().Trim() + "'";
                            }
                            string pStr = string.Join(",", pList);
                            string strSql = "select * from tbl_user where id_organization=" + USER.OID + " AND  id_user=" + USER.UID + " AND status='A' AND upper(user_designation) in (" + pStr + ")";
                            tbl_user uCheck = db.tbl_user.SqlQuery(strSql).FirstOrDefault();
                            if (uCheck == null)
                            {
                                responce.KEY = "FAILURE";
                                responce.MESSAGE = "Your Designation does not match with Participant Level...";
                                return Request.CreateResponse(HttpStatusCode.OK, responce);
                            }
                            else
                            {

                                if (USER.OPT == 1)
                                {
                                    if (iEvent.no_of_participants <= pCount)
                                    {
                                        item.subscription_status = "P";
                                        db.SaveChanges();
                                        responce.KEY = "FAILURE";
                                        responce.MESSAGE = "Participant limit " + iEvent.no_of_participants + " has already reached.Cannot Subscribe to the content.";
                                        return Request.CreateResponse(HttpStatusCode.OK, responce);
                                    }
                                    if (iEvent.is_approval == "1")
                                    {
                                        item.subscription_status = "P";
                                        item.event_user_comment = USER.COMMENT;
                                        new SendApprovalMail().sendApporovalmail(uData, iEvent, item);
                                    }
                                    else
                                    {
                                        item.subscription_status = "A";
                                        item.event_user_comment = USER.COMMENT;
                                    }
                                 
                                }
                                else
                                {
                                    item.subscription_status = "R";
                                    item.event_user_comment = USER.COMMENT;
                                }
                                db.SaveChanges();
                            }
                        }

                        if (iEvent.event_type == 2)
                        {
                            if (USER.OPT == 1)
                            {
                                item.subscription_status = "A";
                                item.event_user_comment = USER.COMMENT;
                            }
                            else
                            {
                                item.subscription_status = "R";
                                item.event_user_comment = USER.COMMENT;
                            }
                            db.SaveChanges();
                        }
                    }
                }
                responce.KEY = "SUCCESS";
                responce.MESSAGE = "SUCCESS";
            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }

        //Don't forget the using System.Security.Cryptography; statement wher you add this class

    }

}
