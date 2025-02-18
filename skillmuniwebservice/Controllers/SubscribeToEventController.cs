using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using m2ostnextservice.Models;

using System.IO;
using System.Threading;

using System.Runtime.Serialization;
using System.Security.Permissions;

namespace m2ostnextservice.Controllers
{
    public class SubscribeToEventController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string userid, int id_event, int id_batch, int orgid)
        {
            string result = "";
            try
            {
                string ClientID = "447409942028-atdo5vigt87n0sgn6n0ddpd85vcb53m4.apps.googleusercontent.com";
                string ClientSecret = "VxbmkqnDgavJTnd-n02C0BVT";
                string ApplicationName = "Skill LAb Event";

                List<EventBatch> chk_batch = new List<EventBatch>();
                chk_batch = new EventLogic().getBatchList(id_event);
                int total_attendrs = 0;

                foreach (var itm in chk_batch)
                {
                    if (id_batch == itm.id_event_batch)
                    {
                        total_attendrs = itm.participants;
                    }
                }
                int current_attenders = new EventLogic().getCurrentAttendersCount(id_event, id_batch);
                if (current_attenders < total_attendrs)
                {
                    tbl_user user = db.tbl_user.Where(t => t.USERID == userid).FirstOrDefault();

                    result = new EventLogic().SubscribeToEvent(user.ID_USER, id_event, id_batch, orgid);

                    tbl_scheduled_event item = db.tbl_scheduled_event.Where(t => t.id_scheduled_event == id_event).FirstOrDefault();

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
                    //--------------------------------------------------------------------------------

                    List<skill_lab_event> Events = new List<skill_lab_event>();
                    obj.id_batch = id_batch;
                    obj.batch = new EventLogic().getBatch(id_batch);

                    tbl_profile profile = db.tbl_profile.Where(t => t.ID_USER == user.ID_USER).FirstOrDefault();
                    //string str1 = "https://www.skillmuni.in/SkillmuniBriefApi/api/DecryptAES?pass=" + profile.EMAIL;
                    //string email = new EventLogic().getApiResponseString(str1);
                    string email = profile.EMAIL;
                    new EventLogic().SendMail(email, obj.event_title, obj.batch, Convert.ToString(obj.event_start_datetime), orgid, obj.event_description);
                    //------------------------------------------------------------------------------------

                    //    var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    //             new ClientSecrets
                    //             {
                    //                 ClientId = "7063792525-7elg557ghh5r52ininf26cjjoteocdes.apps.googleusercontent.com",
                    //                 ClientSecret = "92sy6oHQ2Ug65FM1eFHi5tZ5",

                    //             },
                    //             new[] { CalendarService.Scope.Calendar },
                    //             "user",

                    //             CancellationToken.None).Result;

                    //    // Create the service.
                    //    var service = new CalendarService(new BaseClientService.Initializer
                    //    {
                    //        HttpClientInitializer = credential,
                    //        ApplicationName = "SkillEvent",
                    //    });
                    //    var myEvent = new Event
                    //    {
                    //        Summary = "Google Calendar Api Sample Code by Mukesh Salaria",
                    //        Location = "Gurdaspur, Punjab, India",
                    //        Start = new EventDateTime
                    //        {
                    //            DateTime = DateTime.Parse("2018-10-15T09:00:00-07:00"),
                    //            TimeZone = "America/Los_Angeles",
                    //        },
                    //        End = new EventDateTime
                    //        {
                    //            DateTime = DateTime.Parse("2018-10-15T17:00:00-07:00"),
                    //            TimeZone = "America/Los_Angeles",
                    //        },
                    //        Recurrence = new String[] { "RRULE:FREQ=WEEKLY;BYDAY=MO" },
                    //        Attendees = new List<EventAttendee>
                    //{
                    //new EventAttendee { Email = "prasanthrajsalem02@gmail.com"}
                    //},
                    //    };

                    //    var recurringEvent = service.Events.Insert(myEvent, "primary");
                    //    recurringEvent.SendNotifications = true;
                    //    recurringEvent.Execute();
                }
                else
                {
                    result = "Seats are filled. Please try with other batch";
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}