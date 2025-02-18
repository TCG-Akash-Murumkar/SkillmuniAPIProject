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
    public class getScoreLogForUserController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int episodeID)
        {

            List<tbl_question_episode_mapping> map = new List<tbl_question_episode_mapping>();
            List<QuestionResponse> res = new List<QuestionResponse>();
            MydashboardResponse Result = new MydashboardResponse();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                // map = db.Database.SqlQuery<tbl_question_episode_mapping>("select * from tbl_question_episode_mapping where id_brief={0}",episodeID).ToList();
                //foreach (var itm in map)
                //{

                res = db.Database.SqlQuery<QuestionResponse>("select id_brief_question,brief_question,id_organization,id_brief_master from tbl_brief_question where id_brief_master={0}", episodeID).ToList();

                foreach (var itm1 in res)
                {
                    itm1.is_question_active = 1;
                    itm1.attempt_log = db.Database.SqlQuery<tbl_user_quiz_log>("select * from tbl_user_quiz_log where id_user={0} and id_question={1}", UID, itm1.id_brief_question).ToList();
                    itm1.answer = db.Database.SqlQuery<tbl_brief_answer>("select * from tbl_brief_answer where id_brief_question={0}", itm1.id_brief_question).ToList();

                    if (itm1.answer.Count == 2)
                    {

                        if (itm1.attempt_log.Count >= 1)
                        {
                            itm1.is_question_active = 0;
                            itm1.max_score = 0;
                        }
                        else
                        {
                            itm1.max_score = 10;
                        }

                    }
                    else if (itm1.answer.Count == 3)
                    {
                        //itm1.max_score = 20;
                        if (itm1.attempt_log.Count >= 2)
                        {
                            itm1.is_question_active = 0;
                            itm1.max_score = 0;
                        }
                        else if (itm1.attempt_log.Count == 1)
                        {
                            itm1.max_score = 10;

                        }
                        else
                        {
                            itm1.max_score = 20;

                        }
                    }
                    else if (itm1.answer.Count == 4)
                    {

                        if (itm1.attempt_log.Count >= 3)
                        {
                            itm1.is_question_active = 0;
                        }
                        else if (itm1.attempt_log.Count == 2)
                        {
                            itm1.max_score = 10;
                        }
                        else if (itm1.attempt_log.Count == 1)
                        {
                            itm1.max_score = 20;

                        }
                        else
                        {
                            itm1.max_score = 30;

                        }

                    }
                    foreach (var itm in itm1.attempt_log)
                    {
                        if (itm.is_correct == 1)
                        {
                            itm1.is_question_active = 0;
                            break;
                        }
                    }

                }

                Result.TotalScore = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_user_quiz_log where id_user={0} and is_correct=1", UID).FirstOrDefault(); ;
                tbl_profile inst = new tbl_profile();
                inst = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                //}
                if (inst != null)
                {
                    Result.Name = inst.FIRSTNAME;

                    Result.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + inst.PROFILE_IMAGE;
                }
                else
                {
                    Result.Name = Convert.ToString(UID);

                    Result.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + "default.png";
                }

                Result.Question = res;
                Result.UID = UID;
                Result.OID = OID;

            }



            
           
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }
    }
}
