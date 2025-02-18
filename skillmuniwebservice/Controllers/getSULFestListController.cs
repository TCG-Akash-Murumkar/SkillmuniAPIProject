using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getSULFestListController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int type) //1-live:::2-Upcoming:::3-Completed
        {
            List<tbl_sul_fest_master> Result = new List<tbl_sul_fest_master>();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_sul_fest_master> fest = new List<tbl_sul_fest_master>();
                    tbl_profile prof = new tbl_profile();


                    fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master where event_status={0} and status='A'", "P").ToList();
                    //fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master ").ToList();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    foreach (var itm in fest)
                    {
                        try
                        {
                            if (itm.id_event == 113)
                            {
                                int x = 0;
                            }
                            List<tbl_sul_fest_event_mapping> eventmapsem = new List<tbl_sul_fest_event_mapping>();
                            eventmapsem = db.Database.SqlQuery<tbl_sul_fest_event_mapping>("select * from tbl_sul_fest_event_mapping where id_event={0} and type=1", itm.id_event).ToList();
                            List<tbl_sul_fest_event_mapping> eventmaphigher = new List<tbl_sul_fest_event_mapping>();
                            eventmaphigher = db.Database.SqlQuery<tbl_sul_fest_event_mapping>("select * from tbl_sul_fest_event_mapping where id_event={0} and type=2", itm.id_event).ToList();
                            List<tbl_sul_seminar_master> seminartem = new List<tbl_sul_seminar_master>();
                            List<tbl_sul_higher_education_master> highertem = new List<tbl_sul_higher_education_master>();
                            foreach (var sem in eventmapsem)
                            {
                                tbl_sul_seminar_master semin = db.Database.SqlQuery<tbl_sul_seminar_master>("select * from tbl_sul_seminar_master where id_seminar={0}", sem.id_seminar).FirstOrDefault();
                                //tbl_sul_seminar_timeslot semslot = db.Database.SqlQuery<tbl_sul_seminar_timeslot>("select * from tbl_sul_seminar_timeslot where id_seminar={0}", sem.id_seminar).FirstOrDefault();
                                List<tbl_sul_seminar_timeslot_new > semslot = db.Database.SqlQuery<tbl_sul_seminar_timeslot_new>("select * from tbl_sul_seminar_timeslot_new where id_seminar={0}", sem.id_seminar).ToList();
                                foreach (var slt in semslot)
                                {
                                    slt.slot_start_time = slt.slot_start_time_hour + ":" + slt.slot_start_time_minute + " " + slt.session_start;
                                    slt.slot_end_time = slt.slot_end_time_hour + ":" + slt.slot_end_time_minute + " " + slt.session_end;
                                    List<tbl_sul_seminar_user_registration> semusercnt = db.Database.SqlQuery<tbl_sul_seminar_user_registration>("select * from tbl_sul_seminar_user_registration where id_seminar={0} and slot_id={1} ", sem.id_seminar, slt.id_slot).ToList();

                                    slt.count_restriction = slt.count_restriction - semusercnt.Count;

                                }
                                //int starthr;
                                //int endhr;
                                //if (semslot.session_start_time == "AM") { starthr = (semslot.time_slot_start_time_hour * 60) + semslot.time_slot_start_time_minute; } else { starthr = ((semslot.time_slot_start_time_hour + 12) * 60) + (semslot.time_slot_start_time_minute); };
                                //if (semslot.session_end_time == "AM") { endhr = (semslot.time_slot_end_time_hour * 60) + semslot.time_slot_end_time_minute; } else { endhr = ((semslot.time_slot_end_time_hour + 12) * 60) + semslot.time_slot_end_time_minute; };
                                //int timegap = Convert.ToInt16(semin.time_interval);
                                //List<seminar_time_slots> tems = new List<seminar_time_slots>();
                                //int j = 0;


                                //if (timegap > 0)
                                //{

                                //    for (int i = 1; i != 0;)
                                //    {
                                //        seminar_time_slots slots = new seminar_time_slots();
                                //        if (j != 0)
                                //        {
                                //            starthr = starthr + timegap;
                                //        }
                                //        slots.slot = Convert.ToString(Convert.ToDouble(starthr) / 60) + " Hr";
                                //        string[] spltt = slots.slot.Split('.');
                                //        if (spltt.Length > 1)
                                //        {
                                //            string[] finmin = spltt[1].Split(' ');
                                //            int minss = Convert.ToInt32(Convert.ToDouble("0." + finmin[0]) * 60);
                                //            slots.slot = spltt[0] + ":" + minss + " Hr";
                                //        }
                                //        else
                                //        {
                                //            string[] finmin = spltt[0].Split(' ');
                                //            slots.slot = finmin[0] + ":" + "00" + " Hr";
                                //        }
                                //        j++;
                                //        //semin.slots[0].slot = Convert.ToString(starthr) + "Hr";
                                //        tems.Add(slots);
                                //        //semin.slots.Add(slots);
                                //        if (starthr >= endhr)
                                //        {
                                //            i = 0;
                                //        }
                                //    }
                                //}
                                //List<tbl_sul_slot_seminar> tems = db.Database.SqlQuery<tbl_sul_slot_seminar>("select * from tbl_sul_slot_seminar where id_seminar={0}",semin.id_seminar).ToList();

                                semin.slots = semslot;
                                List<tbl_sul_seminar_user_registration> semuser = db.Database.SqlQuery<tbl_sul_seminar_user_registration>("select * from tbl_sul_seminar_user_registration where id_seminar={0} and id_user={1} ", sem.id_seminar, UID).ToList();

                                if (semuser != null)
                                {
                                    if (semuser.Count == semslot.Count)
                                    {
                                        semin.is_registered = 1;
                                    }
                                    else
                                    {
                                        semin.is_registered = 0;
                                    }
                                    //semin.is_registered = 1;
                                    //semin.slot_registered = semuser;
                                    semin.semslots = semuser;
                                }
                                else
                                {
                                    semin.is_registered = 0;
                                }
                                seminartem.Add(semin);
                                //itm.seminar.Add(semin);
                            }
                            if (seminartem != null)
                            {
                                itm.seminar = seminartem;
                            }
                            foreach (var hig in eventmaphigher)
                            {
                                tbl_sul_higher_education_master higher = db.Database.SqlQuery<tbl_sul_higher_education_master>("select * from tbl_sul_higher_education_master where id_higher_education={0}", hig.id_higher_education).FirstOrDefault();
                                tbl_sul_higher_education_timeslot highslot = db.Database.SqlQuery<tbl_sul_higher_education_timeslot>("select * from tbl_sul_higher_education_timeslot where id_higher_education={0}", hig.id_higher_education).FirstOrDefault();
                                int starthr;
                                int endhr;
                                if (highslot.session_start_time == "AM") { starthr = (highslot.time_slot_start_time_hour * 60) + highslot.time_slot_start_time_minute; } else { starthr = ((highslot.time_slot_start_time_hour + 12) * 60) + highslot.time_slot_start_time_minute; };
                                if (highslot.session_end_time == "AM") { endhr = (highslot.time_slot_end_time_hour * 60) + highslot.time_slot_end_time_minute; ; } else { endhr = ((highslot.time_slot_end_time_hour + 12) * 60) + highslot.time_slot_end_time_minute; };
                                int timegap = higher.time_interval;
                                List<higher_education_time_slots> temh = new List<higher_education_time_slots>();
                                int j = 0;

                                for (int i = 1; i != 0;)
                                {
                                    if (j != 0)
                                    {
                                        starthr = starthr + timegap;
                                    }

                                    higher_education_time_slots slots = new higher_education_time_slots();
                                    slots.slot = Convert.ToString(Convert.ToDouble(starthr) / 60) + " Hr";
                                    string[] spltt = slots.slot.Split('.');
                                    if (spltt.Length > 1)
                                    {
                                        string[] finmin = spltt[1].Split(' ');
                                        int minss = Convert.ToInt32(Convert.ToDouble("0." + finmin[0]) * 60);
                                        slots.slot = spltt[0] + ":" + minss + " Hr";
                                    }
                                    else
                                    {
                                        string[] finmin = spltt[0].Split(' ');
                                        slots.slot = finmin[0] + ":" + "00" + " Hr";

                                    }
                                    temh.Add(slots);
                                    //higher.slots.Add(slots);
                                    if (starthr >= endhr)
                                    {
                                        i = 0;
                                    }
                                    j++;
                                }
                                higher.slots = temh;
                                tbl_sul_higher_education_user_registration highuser = db.Database.SqlQuery<tbl_sul_higher_education_user_registration>("select * from tbl_sul_higher_education_user_registration where id_higher_education={0} and id_user={1}", hig.id_higher_education, UID).FirstOrDefault();

                                if (highuser != null)
                                {
                                    higher.is_registered = 1;
                                    higher.slot_registered = highuser.slot;
                                }
                                else
                                {
                                    higher.is_registered = 0;
                                }
                                highertem.Add(higher);
                            }
                            if (highertem != null)
                            {
                                itm.highereducation = highertem;

                            }
                            if (itm.is_registration_needed == 1)
                            {
                                int regcount = db.Database.SqlQuery<int>("select count(id_register) from tbl_sul_fest_event_registration where id_event={0} and status={1}", itm.id_event, "A").FirstOrDefault();
                                if (itm.registration_start_date <= DateTime.Now && itm.registration_end_date > DateTime.Now)
                                {
                                    itm.registration_date_status = 1;

                                    if (regcount < itm.user_count)
                                    {
                                        itm.registration_count_exceed_status = 0;
                                        string idapp = db.Database.SqlQuery<string>("select status from tbl_sul_fest_event_registration where id_event={0}  and UID={1}", itm.id_event, UID).FirstOrDefault();
                                        if (idapp == "A")
                                        {
                                            itm.registration_user_status = 1;

                                        }
                                        else
                                        {
                                            itm.registration_user_status = 0;

                                        }
                                    }
                                    else
                                    {
                                        string idapp = db.Database.SqlQuery<string>("select status from tbl_sul_fest_event_registration where id_event={0}  and UID={1}", itm.id_event, UID).FirstOrDefault();
                                        if (idapp == "A")
                                        {
                                            itm.registration_user_status = 1;

                                        }
                                        else
                                        {
                                            itm.registration_user_status = 0;

                                        }
                                        itm.registration_count_exceed_status = 1;
                                    }
                                }
                                else
                                {
                                    itm.registration_date_status = 0;
                                }
                            }
                            using (CrmDbContext crm = new CrmDbContext())
                            {
                                itm.state_name = crm.Database.SqlQuery<string>("select name from states where id={0}", itm.state).FirstOrDefault();
                                itm.city_name = crm.Database.SqlQuery<string>("select name from cities where id={0}", itm.city).FirstOrDefault();
                            }

                            itm.event_type = db.Database.SqlQuery<tbl_event_type_mapping>("select * from tbl_event_type_mapping inner join tbl_event_type_master on tbl_event_type_master.id_event_type=tbl_event_type_mapping.id_event_type  where tbl_event_type_mapping.id_event={0}", itm.id_event).ToList();
                            itm.sub_event_type = db.Database.SqlQuery<tbl_sub_event_type_mapping>("select * from tbl_sub_event_type_mapping inner join tbl_sub_event_type_master on tbl_sub_event_type_master.id_sub_event_type=tbl_sub_event_type_mapping.id_sub_event_type  where tbl_sub_event_type_mapping.id_event={0}", itm.id_event).ToList();
                            itm.event_logo = ConfigurationManager.AppSettings["FestEventLogo"].ToString() + itm.event_logo;
                            if (itm.is_sponsor_available == 1)
                            {
                                itm.sponsor = db.Database.SqlQuery<string>("select sponsor from tbl_event_sponsor_master where id_sponsor={0}", itm.id_sponsor).FirstOrDefault();
                                itm.sponsor_logo = ConfigurationManager.AppSettings["SponSorLogo"].ToString() + itm.sponsor_logo;
                            }
                            if (itm.is_college_restricted == 1)
                            {
                                string college = db.Database.SqlQuery<string>("select college_name from tbl_college_list where id_college={0}", itm.id_college).FirstOrDefault();
                                if (college == prof.COLLEGE)
                                {
                                    itm.college_name = college;

                                    tbl_sul_fest_event_registration reg = db.Database.SqlQuery<tbl_sul_fest_event_registration>("select * from tbl_sul_fest_event_registration where UID={0} and id_event={1}", UID, itm.id_event).FirstOrDefault();
                                    if (reg != null)
                                    {
                                        if (reg.status == "A")
                                        {
                                            itm.register_status = 1;


                                        }
                                        else
                                        {
                                            itm.register_status = 2;

                                        }
                                    }
                                    else
                                    {
                                        itm.register_status = 0;
                                    }

                                    if (type == 1)
                                    {
                                        if (itm.event_start_date <= DateTime.Now && itm.event_end_date >= DateTime.Now)
                                        {
                                            Result.Add(itm);

                                        }
                                    }
                                    else if (type == 2)
                                    {
                                        if (itm.event_start_date > DateTime.Now)
                                        {
                                            Result.Add(itm);

                                        }

                                    }
                                    else if (type == 3)
                                    {
                                        if (itm.event_end_date < DateTime.Now)
                                        {
                                            Result.Add(itm);

                                        }

                                    }


                                }

                            }
                            else
                            {
                                tbl_sul_fest_event_registration reg = db.Database.SqlQuery<tbl_sul_fest_event_registration>("select * from tbl_sul_fest_event_registration where UID={0} and id_event={1}", UID, itm.id_event).FirstOrDefault();
                                if (reg != null)
                                {
                                    if (reg.status == "A")
                                    {
                                        itm.register_status = 1;


                                    }
                                    else
                                    {
                                        itm.register_status = 2;

                                    }
                                }
                                else
                                {
                                    itm.register_status = 0;
                                }
                                if (type == 1)
                                {
                                    if (itm.event_start_date <= DateTime.Now && itm.event_end_date >= DateTime.Now)
                                    {
                                        Result.Add(itm);

                                    }
                                }
                                else if (type == 2)
                                {
                                    if (itm.event_start_date > DateTime.Now)
                                    {
                                        Result.Add(itm);

                                    }

                                }
                                else if (type == 3)
                                {
                                    if (itm.event_end_date < DateTime.Now)
                                    {
                                        Result.Add(itm);

                                    }

                                }

                            }

                        }
                        catch (Exception e) {
                            throw e;
                            //return Request.CreateResponse(HttpStatusCode.OK, e);
                        }
                    }
                }






            }
            catch (Exception e)
            {
                //return Request.CreateResponse(HttpStatusCode.OK, e);
                throw e;
            }

            Result = Result.OrderBy(o => o.event_start_date).ToList();


            return Request.CreateResponse(HttpStatusCode.OK, Result);


        }

    }
}
