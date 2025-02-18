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
    public class EvaluateQuestionController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int episodeID,int id_brief_question,int id_brief_answer,int is_correct_answer, int attempt_no)
        {

            QuestionEvaluationResponse res = new QuestionEvaluationResponse();
            int id_correct_answer = 0;
            int score = 0;
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
               
                if (is_correct_answer == 0)
                {
                    id_correct_answer = db.Database.SqlQuery<int>("select id_brief_answer from tbl_brief_answer where id_brief_question={0} and is_correct_answer={1}",id_brief_question,1).FirstOrDefault();


                }
                else

                {

                    List<tbl_brief_answer> ans = db.Database.SqlQuery<tbl_brief_answer>("Select * from tbl_brief_answer where  id_brief_question={0}", id_brief_question).ToList();
                    if (attempt_no == 1)
                    {
                        if (ans.Count == 2)
                        {
                            score = 10;

                        }
                        else if (ans.Count == 3)
                        {
                            score = 20;

                        }
                        else if (ans.Count == 4)
                        {
                            score = 30;

                        }
                        //score=Convert.ToInt32(ConfigurationManager.AppSettings["attempt_1_score"].ToString());
                    }
                    else if (attempt_no == 2)
                    {
                        if (ans.Count == 3)
                        {
                            score = 10;

                        }
                        else if (ans.Count == 4)
                        {
                            score = 20;

                        }
                        //score = Convert.ToInt32(ConfigurationManager.AppSettings["attempt_2_score"].ToString());


                    }
                    else if (attempt_no == 3)
                    {
                        if (ans.Count == 4)
                        {
                            score = 10;

                        }
                        // score = Convert.ToInt32(ConfigurationManager.AppSettings["attempt_3_score"].ToString());

                    }

                    id_correct_answer = id_brief_answer;
                }

                if (attempt_no <= 3)
                {

                    db.Database.ExecuteSqlCommand("Insert into tbl_user_quiz_log (id_user,id_brief,id_question,id_correct_answer,id_selected_answer,status,is_correct,updated_date_time,attempt_no,score,id_org) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) ", UID, episodeID, id_brief_question, id_correct_answer, id_brief_answer, "A", is_correct_answer, DateTime.Now, attempt_no, score,OID);

                }
                

            }

            res.attempt_no = attempt_no;
            res.id_correct_answer = id_correct_answer;
            res.id_selected_answer = id_brief_answer;
            res.is_correct = is_correct_answer;
            res.score = score;


            return Request.CreateResponse(HttpStatusCode.OK, res);

        }

    }
}
