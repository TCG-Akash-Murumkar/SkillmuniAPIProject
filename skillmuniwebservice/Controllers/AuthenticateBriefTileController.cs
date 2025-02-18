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
    public class AuthenticateBriefTileController : ApiController
    {

        public HttpResponseMessage Get(int UID, int OID, int id_academy)
        {
            AuthenticateBrief Auth=new AuthenticateBrief();
            Auth.AuthFlag = "0";
            string res = "0";
            tbl_academy_level_brief_restriction acres = new tbl_academy_level_brief_restriction();
            List<tbl_restriction_user_log> lg = new List<tbl_restriction_user_log>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                acres = db.Database.SqlQuery<tbl_academy_level_brief_restriction>("select * from tbl_academy_level_brief_restriction where id_academy = {0}", id_academy).FirstOrDefault();
                if (acres != null)
                {
                    if (acres.time == 1)
                    {
                        DateTime tdy = DateTime.Now.Date;
                        lg = db.Database.SqlQuery<tbl_restriction_user_log>("select * from tbl_restriction_user_log where UID = {0} and id_academy={1} and date(updated_date_time)={2}", UID, id_academy,tdy).ToList();
                        if (lg.Count < acres.brief_count)
                        {
                            res = "1";
                            Auth.AuthFlag = "1";
                        }
                        else
                        {
                            Auth.Message = "Please comeback tommorrow to read more.";
                           
                            
                        }
                       

                    }
                    else if(acres.time==2)
                    {
                        int tdy = DateTime.Now.Hour;
                        DateTime dt = DateTime.Now.Date;
                        lg = db.Database.SqlQuery<tbl_restriction_user_log>("SELECT * FROM tbl_restriction_user_log where UID = {0} and id_academy={1} and EXTRACT(HOUR  FROM updated_date_time)={2} and  date(updated_date_time)={3}", UID, id_academy, tdy,dt).ToList();
                        if (lg.Count < acres.brief_count)
                        {
                            res = "1";
                            Auth.AuthFlag = "1";
                        }
                        else
                        {
                            int hrs = DateTime.Now.Hour;
                            hrs += 1;
                            if (hrs >= 12)
                            {
                                int time = hrs - 12;
                                if (hrs == 24)
                                {
                                    Auth.Message = "Please comeback after 1 hour to read more.";
                                }
                                else
                                {
                                    Auth.Message = "Please comeback after  1 hour to read more.";

                                }
                               
                            }
                            else
                            {
                                Auth.Message = "Please comeback after 1 hour to read more.";
                            }

                        }

                    }

                }
                else
                {
                    Auth.AuthFlag = "1";
                    res = "1";
                }


            }

            return Request.CreateResponse(HttpStatusCode.OK,Auth);//1 for authenticate // 0 for decline
        }
    }
}
