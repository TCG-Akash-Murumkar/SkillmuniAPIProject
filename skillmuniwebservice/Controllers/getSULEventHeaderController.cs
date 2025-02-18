using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class getSULEventHeaderController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int type=0) //1-live:::2-Upcoming:::3-Completed
        {
            List<tbl_sul_fest_master> Result = new List<tbl_sul_fest_master>();

            EventsHeader Res = new EventsHeader();



            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_sul_fest_master> fest = new List<tbl_sul_fest_master>();
                    tbl_profile prof = new tbl_profile();

                    fest = db.Database.SqlQuery<tbl_sul_fest_master>("select * from tbl_sul_fest_master where event_status={0}", "P").ToList();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    foreach (var itm in fest)
                    {

                        //using (CrmDbContext crm = new CrmDbContext())
                        //{
                        //    itm.state_name = db.Database.SqlQuery<string>("select name from states where id={0}", itm.state).FirstOrDefault();
                        //    itm.city_name = db.Database.SqlQuery<string>("select name from cities where id={0}", itm.city).FirstOrDefault();

                        //}




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

                                //if (type == 1)
                                //{
                                //    if (itm.event_start_date <= DateTime.Now && itm.event_end_date >= DateTime.Now)
                                //    {
                                //        Result.Add(itm);

                                //    }
                                //}
                                //else if (type == 2)
                                //{
                                //    if (itm.event_start_date > DateTime.Now)
                                //    {
                                //        Result.Add(itm);

                                //    }

                                //}
                                //else if (type == 3)
                                //{
                                //    if (itm.event_end_date < DateTime.Now)
                                //    {
                                //        Result.Add(itm);

                                //    }

                                //}
                                if (itm.is_paid_event == 1)
                                {
                                    Res.paid++;
                                }
                                else
                                {
                                    Res.free++;

                                }
                                Result.Add(itm);


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
                            //if (type == 1)
                            //{
                            //    if (itm.event_start_date <= DateTime.Now && itm.event_end_date >= DateTime.Now)
                            //    {
                            //        Result.Add(itm);

                            //    }
                            //}
                            //else if (type == 2)
                            //{
                            //    if (itm.event_start_date > DateTime.Now)
                            //    {
                            //        Result.Add(itm);

                            //    }

                            //}
                            //else if (type == 3)
                            //{
                            //    if (itm.event_end_date < DateTime.Now)
                            //    {
                            //        Result.Add(itm);

                            //    }

                            //}
                            if (itm.is_paid_event == 1)
                            {
                                Res.paid++;
                            }
                            else
                            {
                                Res.free++;

                            }
                            Result.Add(itm);

                        }


                    }



                }






            }
            catch (Exception e)
            {

            }


            return Request.CreateResponse(HttpStatusCode.OK, Res);


        }

    }
}
