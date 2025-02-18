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
    public class getQuestionsForEpisodeController : ApiController

    {

        public HttpResponseMessage Get(int UID,int OID,int episodeID)
        {
           
            List<tbl_question_episode_mapping> map = new List<tbl_question_episode_mapping>();
            List<QuestionResponse> res = new List<QuestionResponse>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
               // map = db.Database.SqlQuery<tbl_question_episode_mapping>("select * from tbl_question_episode_mapping where id_brief={0}",episodeID).ToList();
                //foreach (var itm in map)
                //{

                    res = db.Database.SqlQuery<QuestionResponse>("select id_brief_question,brief_question,id_organization,id_brief_master from tbl_brief_question where id_brief_master={0}", episodeID).ToList();
                    foreach (var itm1 in res)
                    {
                    itm1.is_question_active = 1;
                    List<tbl_user_quiz_log> attempt_log_inst = new List<tbl_user_quiz_log>();
                    itm1.attempt_log = db.Database.SqlQuery<tbl_user_quiz_log>("select * from tbl_user_quiz_log where id_user={0} and id_question={1}", UID, itm1.id_brief_question).ToList();
                    attempt_log_inst = db.Database.SqlQuery<tbl_user_quiz_log>("select * from tbl_user_quiz_log where id_user={0} and id_question={1}", UID, itm1.id_brief_question).ToList();
                    itm1.answer= db.Database.SqlQuery<tbl_brief_answer>("select * from tbl_brief_answer where id_brief_question={0}", itm1.id_brief_question).ToList();
                    if (itm1.answer.Count == 2)
                    {

                        if (attempt_log_inst.Count >= 1)
                        {
                            itm1.is_question_active = 0;
                            itm1.max_score = 0;

                        }
                        else
                        {
                            itm1.max_score = 10;
                        }

                        itm1.no_of_attempts = attempt_log_inst.Count;
                    }
                    else if (itm1.answer.Count == 3)
                    {
                        //itm1.max_score = 20;
                        if (attempt_log_inst.Count >= 2)
                        {
                            itm1.is_question_active = 0;
                            itm1.max_score = 0;
                        }
                        else if (attempt_log_inst.Count == 1)
                        {
                            itm1.max_score = 10;

                        }
                        else
                        {
                            itm1.max_score = 20;

                        }
                        itm1.no_of_attempts = attempt_log_inst.Count;

                    }
                    else if (itm1.answer.Count == 4)
                    {

                        if (attempt_log_inst.Count >= 3)
                        {
                            itm1.is_question_active = 0;
                        }
                        else if (attempt_log_inst.Count == 2)
                        {
                            itm1.max_score = 10;
                        }
                        else if (attempt_log_inst.Count == 1)
                        {
                            itm1.max_score = 20;

                        }
                        else 
                        {
                            itm1.max_score = 30;

                        }
                        itm1.no_of_attempts = attempt_log_inst.Count;

                    }
                    foreach (var itm in attempt_log_inst)
                    {
                        if (itm.is_correct == 1)
                        {
                            itm1.is_question_active = 0;
                            itm1.earned_marks = itm.score;
                         
                            break;
                        }
                    }

                }


                //}

            }




            return Request.CreateResponse(HttpStatusCode.OK, res);

        }

    }
}
