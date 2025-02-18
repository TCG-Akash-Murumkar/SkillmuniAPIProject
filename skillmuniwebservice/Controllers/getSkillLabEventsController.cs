using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getSkillLabEventsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string UID)
        {
            List<skill_lab_event> Events = new List<skill_lab_event>();
            tbl_user user = db.tbl_user.Where(t => t.USERID == UID).FirstOrDefault();
            tbl_profile prof = db.tbl_profile.Where(t => t.ID_USER == user.ID_USER).FirstOrDefault();
            prof.LOCATION = prof.LOCATION.ToUpper();
            if (user != null)
            {

                //List<tbl_scheduled_event> EventInst = db.tbl_scheduled_event.SqlQuery("select * from tbl_scheduled_event where id_organization=" + user.ID_ORGANIZATION + " and status='A'" + " and location=" + prof.LOCATION).ToList();
                //List<tbl_scheduled_event> EventInstAll = db.tbl_scheduled_event.Where(t => t.id_organization == user.ID_ORGANIZATION && t.status == "A" && t.location == "All").ToList();

                //List<tbl_scheduled_event> EventInst = db.tbl_scheduled_event.Where(t => t.id_organization == user.ID_ORGANIZATION && t.status == "A").ToList();
                //List<tbl_scheduled_event> EventInstAll = db.tbl_scheduled_event.Where(t => t.id_organization == user.ID_ORGANIZATION && t.status == "A" && t.location == "All").ToList();
                //string loc = new EventLogic().getLoc(user.ID_USER);
                List<tbl_scheduled_event> EventInst = new EventLogic().getEventList(user,prof.LOCATION);

                foreach (var item in EventInst)
                {
                    skill_lab_event obj = new skill_lab_event();
                    obj.event_additional_info = item.event_additional_info;
                    obj.event_comment = item.event_comment;
                    obj.event_description = item.event_description;
                    obj.event_start_datetime = item.registration_start_date;
                    obj.event_title = item.event_title;
                    obj.facilitator_name = item.facilitator_name;
                    obj.id_scheduled_event = item.id_scheduled_event;
                    obj.participant_level = item.participant_level;
                    obj.program_image = item.program_image;
                    obj.program_location = item.program_location;
                    obj.program_venue = item.program_venue;
                    obj.id_organization = item.id_organization;
                    List<EventBatch> batch = new List<EventBatch>();
                    batch = new EventLogic().getBatchList(obj.id_scheduled_event);

                    Events.Add(obj);
                }
                //foreach (var item in EventInstAll)
                //{
                //    skill_lab_event obj = new skill_lab_event();
                //    obj.event_additional_info = item.event_additional_info;
                //    obj.event_comment = item.event_comment;
                //    obj.event_description = item.event_description;
                //    obj.event_start_datetime = item.registration_start_date;
                //    obj.event_title = item.event_title;
                //    obj.facilitator_name = item.facilitator_name;
                //    obj.id_scheduled_event = item.id_scheduled_event;
                //    obj.participant_level = item.participant_level;
                //    obj.program_image = item.program_image;
                //    obj.program_location = item.program_location;
                //    obj.program_venue = item.program_venue;
                //    obj.id_organization = item.id_organization;
                //    List<EventBatch> batch = new List<EventBatch>();
                //    batch = new EventLogic().getBatchList(obj.id_scheduled_event);

                //    Events.Add(obj);
                //}

            }
            else
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, Events);
        }
    }
}
