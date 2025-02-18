using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getEventsInDateController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string IdString ,string id_user)
        {
            tbl_user user = db.tbl_user.Where(t => t.USERID == id_user).FirstOrDefault();
            List<tbl_scheduled_event> EventInst = new List<tbl_scheduled_event>();
            List<int> Ids = new List<int>();
            string[] splited = IdString.Split('|');
          
            foreach (var itm in splited)
            {
                Ids.Add(Convert.ToInt32(itm));
            }
            List<skill_lab_event> Events = new List<skill_lab_event>();

            foreach (var itm in Ids)
            {


                tbl_scheduled_event item = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == itm).FirstOrDefault();



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
                obj.BatchList  = new EventLogic().getBatchList(obj.id_scheduled_event);
                obj.BatchList = new EventLogic().getAvailable(obj.BatchList);

                tbl_user_event_mapping map = new tbl_user_event_mapping();
                map = new EventLogic().getMappedEvent(user.ID_USER,itm);
                if (map != null)
                {
                    obj.id_batch = map.id_batch;
                    obj.batch = new EventLogic().getBatch(map.id_batch);
                }
               



                Events.Add(obj);

            }
           

            return Request.CreateResponse(HttpStatusCode.OK, Events);
        }
    }
}
