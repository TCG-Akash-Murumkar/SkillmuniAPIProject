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
    [AllowAnonymous]
    public class LeaderDashboardDataController : ApiController
    {

        public HttpResponseMessage Get(int id_user, int org_id, int page_no)
        {
            List<LeaderBoardData> Leader = new List<LeaderBoardData>();
            List<LeaderBoardData> Returnresp = new List<LeaderBoardData>();



            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_user> user = new List<tbl_user>();
                    user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where STATUS='A' and ID_ORGANIZATION={0}", org_id).ToList();

                    foreach (var obj in user)
                    {
                        LeaderBoardData led = new LeaderBoardData();
                        List<tbl_user_level_log> ledlog = new List<tbl_user_level_log>();
                        ledlog = db.Database.SqlQuery<tbl_user_level_log>("SELECT *  FROM tbl_user_level_log WHERE id_user = {0} and is_qualified=1 and status='A';", obj.ID_USER).ToList();

                        if (ledlog.Count > 0)
                        {
                            led.score = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score),0) total FROM tbl_user_level_log WHERE id_user = {0} and is_qualified=1 and status='A';", obj.ID_USER).FirstOrDefault();
                            led.bonus = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(bonus),0) total FROM tbl_user_level_log WHERE id_user = {0} and is_qualified=1 and status='A';", obj.ID_USER).FirstOrDefault();
                            led.total_score = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(total_score),0) total FROM tbl_user_level_log WHERE id_user = {0} and is_qualified=1 and status='A';", obj.ID_USER).FirstOrDefault();
                            led.id_user = obj.ID_USER;
                            Leader.Add(led);
                        }

                    }
                    List<LeaderBoardData> requser1 = new List<LeaderBoardData>();
                    requser1 = Leader.OrderByDescending(o => o.total_score).Take(Convert.ToInt32(ConfigurationManager.AppSettings["LeaderBoardListLimit"])).ToList();
                    foreach (var itm in requser1)
                    {
                        tbl_profile inst = new tbl_profile();
                        inst = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();
                        if (inst.ID_USER > 0)
                        {
                            itm.username = inst.FIRSTNAME;
                            itm.location = inst.CITY;
                            itm.id_user = inst.ID_USER;
                            itm.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + inst.PROFILE_IMAGE;
                        }
                        else
                        {
                            itm.username = Convert.ToString(itm.id_user);
                            itm.location = " ";
                            itm.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + "default.png";
                        }

                    }
                    Returnresp = requser1;

                }

            }
            catch (Exception e)
            {
                throw e;

            }
            List<LeaderBoardData> finalres = new List<LeaderBoardData>();

            if (page_no == 1)
            {

                if (Returnresp.Count > 5)
                {
                    finalres = Returnresp.Take(5).ToList();

                }

                else
                {
                    finalres = Returnresp;
                }



               


            }
            else if (page_no == 2)
            {

                if (Returnresp.Count > 5)
                {
                    int count = Returnresp.Count - 5;
                    finalres = Returnresp.Skip(5).Take(count).ToList();
                }


            }
            return Request.CreateResponse(HttpStatusCode.OK, finalres);
        }

    }
}
