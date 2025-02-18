using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getSubscribedEventsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string userid)
        {
            List<tbl_user_event_mapping> map = new List<tbl_user_event_mapping>();
            tbl_user user = db.tbl_user.Where(t => t.USERID == userid).FirstOrDefault();
            map = new EventLogic().getMappedEvents(user.ID_USER);

          
            List<skill_lab_event> Events = new List<skill_lab_event>();

            foreach (var itm in map)
            {


                tbl_scheduled_event item = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == itm.id_event).FirstOrDefault();



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
                obj.BatchList = new EventLogic().getBatchList(obj.id_scheduled_event);
                obj.id_batch = itm.id_batch;
                obj.batch = new EventLogic().getBatch(itm.id_batch);



                Events.Add(obj);

            }


            return Request.CreateResponse(HttpStatusCode.OK, Events);
        }
    }
}
