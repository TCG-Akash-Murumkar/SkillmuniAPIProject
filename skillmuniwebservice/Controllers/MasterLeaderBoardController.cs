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
    public class MasterLeaderBoardController : ApiController
    {
        public HttpResponseMessage Get(int OID)
        {
            List<videoresponse> res = new List<videoresponse>();
            List<tbl_user_quiz_log> log = new List<tbl_user_quiz_log>();
            List<LeaderBoardData> Leader = new List<LeaderBoardData>();
            List<MasterLeaderBoardData> Returnresp = new List<MasterLeaderBoardData>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<tbl_user_quiz_log> masuser = new List<tbl_user_quiz_log>();
                masuser = db.Database.SqlQuery<tbl_user_quiz_log>("SELECT * FROM tbl_user_quiz_log  where id_org={0} group by id_user", OID).ToList();

                //log = db.Database.SqlQuery<tbl_user_quiz_log>("select * ,COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_org={0} and is_correct=1 ", OID).ToList();
                foreach (var itm in masuser)
                {
                    MasterLeaderBoardData obj = new MasterLeaderBoardData();
                    tbl_profile inst = new tbl_profile();
                    inst = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();
                    obj.id_user = itm.id_user;
                    obj.total_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1", itm.id_user).FirstOrDefault(); ;
                    if (inst != null)
                    {
                        obj.username = inst.FIRSTNAME;

                        obj.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + inst.PROFILE_IMAGE;
                    }
                    else
                    {
                        obj.username = Convert.ToString(itm.id_user);

                        obj.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + "default.png";
                    }
                    if (obj.total_score > 0)
                    {
                        Returnresp.Add(obj);
                    }
                }

            }

            if (Returnresp != null)
            {
                Returnresp = Returnresp.OrderByDescending(x => x.total_score).ToList();
                int i = 1;
                foreach (var itr in Returnresp)
                {
                         itr.Rank = i;
                    
                         i++;

                }
            }


            return Request.CreateResponse(HttpStatusCode.OK, Returnresp);

        }




    }
}
