using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getEpisodesStatusController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID)
        {
            List<tbl_episode_log> result = new List<tbl_episode_log>();


            List<tbl_brief_master> mas = new List<tbl_brief_master>();


            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                mas = db.Database.SqlQuery<tbl_brief_master>("select * from tbl_brief_master where id_organization={0}", OID).ToList();
                mas = mas.OrderBy(x => x.episode_sequence).ToList();
                foreach (var itm in mas)
                {
                    tbl_episode_log obj = new tbl_episode_log();
                    if (itm.episode_sequence == 1)
                    {
                        obj.id_brief_master = itm.id_brief_master;
                        obj.id_user = UID;
                        obj.oid = OID;
                        obj.status = "U";
                        obj.updated_date_time = DateTime.Now;

                    }
                    else
                    {

                        obj= db.Database.SqlQuery<tbl_episode_log>("select * from tbl_episode_log where id_user={0} and id_brief_master={1}", UID,itm.id_brief_master).FirstOrDefault();
                        if (obj == null)
                        {
                            tbl_episode_log tem = new tbl_episode_log();

                            tem.id_brief_master = itm.id_brief_master;
                            tem.id_user = UID;
                            tem.oid = OID;
                            tem.status = "L";
                            tem.updated_date_time = DateTime.Now;
                            obj = tem;

                        }
                    }
                    result.Add(obj);
                }

            }


            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

    }
}

//public HttpResponseMessage Get(int UID, int OID)
//{
//    List<EpisodeStatusResponse> result = new List<EpisodeStatusResponse>();


//    List<tbl_brief_master> mas = new List<tbl_brief_master>();


//    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
//    {
//        mas = db.Database.SqlQuery<tbl_brief_master>("select * from tbl_brief_master where id_organization={0}", OID).ToList();
//        mas = mas.OrderBy(x => x.episode_sequence).ToList();
//        string befst = "";
//        foreach (var itm in mas)
//        {
//            EpisodeStatusResponse obj = new EpisodeStatusResponse();
//            obj.id_brief_master = itm.id_brief_master;
//            obj.episode_sequence = itm.episode_sequence;
//            obj.status = "L";
//            List<tbl_brief_question> questmaster = new List<tbl_brief_question>();
//            questmaster = db.Database.SqlQuery<tbl_brief_question>("select * from tbl_brief_question where id_brief_master={0}", itm.id_brief_master).ToList();
//            List<tbl_user_quiz_log> log = new List<tbl_user_quiz_log>();
//            log = db.Database.SqlQuery<tbl_user_quiz_log>("select * from tbl_user_quiz_log where id_brief={0} and is_correct=1 and id_user={1}", itm.id_brief_master, UID).ToList();
//            if (obj.episode_sequence == 1)
//            {
//                if (questmaster.Count == log.Count)
//                {
//                    obj.status = "U";
//                    befst = "C";
//                }
//                else
//                {
//                    obj.status = "U";
//                    befst = "I";

//                }


//            }
//            else
//            {
//                if (questmaster.Count == log.Count)
//                {
//                    obj.status = "U";
//                    befst = "C";
//                }
//                else if (befst == "C")
//                {
//                    obj.status = "U";
//                    befst = "I";

//                }
//                else
//                {
//                    obj.status = "L";
//                    befst = "I";

//                }
//            }
//            result.Add(obj);
//        }

//    }


//    return Request.CreateResponse(HttpStatusCode.OK, result);

//}
