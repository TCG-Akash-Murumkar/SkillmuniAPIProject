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
    public class getEpisodewiseDataController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID)
        {
            EpisodewiseDataResponse result = new EpisodewiseDataResponse();
            List<MasterLeaderBoardData> Returnresp = new List<MasterLeaderBoardData>();
            List<EpisodeData> temp = new List<EpisodeData>();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                result.overall_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1", UID).FirstOrDefault();
                List<tbl_user_quiz_log> masuser = new List<tbl_user_quiz_log>();
                masuser = db.Database.SqlQuery<tbl_user_quiz_log>("SELECT * FROM tbl_user_quiz_log  where id_org={0} group by id_user", OID).ToList();


                //log = db.Database.SqlQuery<tbl_user_quiz_log>("select * ,COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_org={0} and is_correct=1 ", OID).ToList();
                foreach (var itm in masuser)
                {
                    MasterLeaderBoardData obj = new MasterLeaderBoardData();
                    tbl_profile inst = new tbl_profile();

                    obj.id_user = itm.id_user;
                    obj.total_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1", itm.id_user).FirstOrDefault(); 

                    if (obj.total_score > 0)
                    {
                        Returnresp.Add(obj);
                    }
                }

               

                List<tbl_brief_master> mas = new List<tbl_brief_master>();


                mas = db.Database.SqlQuery<tbl_brief_master>("select * from tbl_brief_master where id_organization={0}", OID).ToList();
                foreach (var brf in mas)
                {
                    List<MasterLeaderBoardData> respinst = new List<MasterLeaderBoardData>();

                    foreach (var itm in masuser)
                    {
                        MasterLeaderBoardData objinst = new MasterLeaderBoardData();
                        objinst.id_brief_master = brf.id_brief_master;
                        objinst.id_user = itm.id_user;
                        objinst.total_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1 and id_brief={1}", itm.id_user,brf.id_brief_master).FirstOrDefault(); 
                        if (objinst.total_score > 0)
                        {
                            respinst.Add(objinst);
                        }
                    }
                    int indexepi = 0;
                    if (respinst != null)
                    {
                        respinst = respinst.OrderByDescending(x => x.total_score).ToList();
                        int i = 1;
                        foreach (var itr in respinst)
                        {
                            if (itr.id_user == UID)
                            {
                                EpisodeData instantce = new EpisodeData();
                                instantce.Episode_rank = i;
                                instantce.Episod_score = itr.total_score;
                                instantce.id_brief_master = itr.id_brief_master;
                                instantce.episode_sequence = brf.episode_sequence;
                                temp.Add(instantce);
                                //result.Epi.Add(instantce);
                            }
                            i++;

                        }
                    }
                    //else
                    //{
                    //    EpisodeData instantce = new EpisodeData();
                    //    instantce.Episode_rank = 1;
                    //    instantce.Episod_score = itr.total_score;
                    //    instantce.id_brief_master = brf.id_brief_master;
                    //    result.Epi.Add(instantce);
                    //   // result.overall_rank = 1;
                    //}
                    

                }

                result.Epi = temp;



            }
            if (Returnresp != null)
            {
                Returnresp = Returnresp.OrderByDescending(x => x.total_score).ToList();
                int i = 1;
                foreach (var itr in Returnresp)
                {
                    if (itr.id_user == UID)
                    {
                        result.overall_rank = i;
                    }
                    i++;

                }
            }
            //else
            //{
            //    result.overall_rank = 1;
            //}

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
