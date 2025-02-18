using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class OrgGamePostAssessmentLogController : ApiController
    {
        public HttpResponseMessage Post([FromBody]assessJson inpdata)
        {
            List<tbl_org_game_user_assessment_log> log = JsonConvert.DeserializeObject<List<tbl_org_game_user_assessment_log>>(inpdata.log_string);

            ScoreLOgicResponse Result = new ScoreLOgicResponse();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    //ScoreData.log.id_score_unit = db.Database.SqlQuery<int>("select ").FirstOrDefault();
                    List<tbl_org_game_user_assessment_log> lg = db.Database.SqlQuery<tbl_org_game_user_assessment_log>("select * from tbl_org_game_user_assessment_log where id_org_game={0} and id_org_game_content={1} and attempt_no={2} and id_user={3} ",log[0].id_org_game,log[0].id_org_game_content,log[0].attempt_no,log[0].id_user).ToList();
                    if (lg.Count == 0)
                    {
                        foreach (var itm in log)
                        {
                            db.Database.ExecuteSqlCommand("Insert into tbl_org_game_user_assessment_log (id_org_game,id_org_game_content,attempt_no,id_org_game_level,id_question,id_answer_selected,is_correct,status,updated_date_time,id_user) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", itm.id_org_game, itm.id_org_game_content, itm.attempt_no, itm.id_org_game_level, itm.id_question, itm.id_answer_selected, itm.is_correct, "A", DateTime.Now, itm.id_user);


                        }
                        Result.STATUS = "SUCCESS";

                        Result.MESSAGE = "Successfully posted.";

                    }
                    else
                    {
                        Result.STATUS = "FAILED";
                        Result.MESSAGE = "Duplicate Entries.";
                    }
                    


                }

            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";
                Result.MESSAGE = "Something went wrong.";

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

    }
}
