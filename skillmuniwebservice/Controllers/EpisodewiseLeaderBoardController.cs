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
    public class EpisodewiseLeaderBoardController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_brief)
        {

            EpisodewiseLeaderBoardResponse result = new EpisodewiseLeaderBoardResponse();
            List<EpisodewiseLeaderRankList> temp = new List<EpisodewiseLeaderRankList>();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                List<tbl_user_quiz_log> masuser = new List<tbl_user_quiz_log>();

                List<tbl_brief_master> mas = new List<tbl_brief_master>();

                result.EpisodeId = id_brief;
                // mas = db.Database.SqlQuery<tbl_brief_master>("select * from tbl_brief_master where id_organization={0}", OID).ToList();

                List<MasterLeaderBoardData> respinst = new List<MasterLeaderBoardData>();
                masuser = db.Database.SqlQuery<tbl_user_quiz_log>("SELECT * FROM tbl_user_quiz_log  where id_org={0} and id_brief={1} group by id_user", OID, id_brief).ToList();

                foreach (var itm in masuser)
                {
                    EpisodewiseLeaderRankList objinst = new EpisodewiseLeaderRankList();
                    objinst.id_user = itm.id_user; ;
                    tbl_profile inst = new tbl_profile();
                    inst = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();
                    
                    if (inst != null)
                    {
                        objinst.username = inst.FIRSTNAME;

                        objinst.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + inst.PROFILE_IMAGE;
                    }
                    else
                    {
                        objinst.username = Convert.ToString(itm.id_user);

                        objinst.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + "default.png";
                    }                 
                    objinst.total_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1 and id_brief={1}", itm.id_user, id_brief).FirstOrDefault();
                    if (objinst.total_score > 0)
                    {
                        temp.Add(objinst);
                    }
                }
                result.RankList = temp;
                result.RankList = result.RankList.OrderByDescending(x => x.total_score).ToList();
                int i = 1;
                foreach (var itm in result.RankList)
                {
                    itm.rank = i;
                    i++;
                }

            }


            return Request.CreateResponse(HttpStatusCode.OK, result);





        }


    }
}
