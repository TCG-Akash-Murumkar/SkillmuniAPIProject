using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class OrgGameScoreDataPostController : ApiController
    {
        public HttpResponseMessage Post([FromBody]tbl_org_game_user_log ScoreData)
        {
            ScoreLOgicResponse Result = new ScoreLOgicResponse();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_org_game_user_log> lg = db.Database.SqlQuery<tbl_org_game_user_log>("select * from tbl_org_game_user_log where id_game_content={0} and id_level={1} and id_org_game={2} and  id_user={3} and attempt_no={4}", ScoreData.id_game_content,ScoreData.id_level,ScoreData.id_org_game,ScoreData.UID,ScoreData.attempt_no).ToList();
                    if (lg.Count==0)
                    {
                        ScoreData.id_score_unit = db.Database.SqlQuery<int>("select id_score_unit from  tbl_org_game_score_unit_mapping where id_org_game={0} ", ScoreData.id_org_game).FirstOrDefault();
                        ScoreData.score_unit = db.Database.SqlQuery<string>("select unit from  tbl_org_game_score_unit_master where id_sore_unit={0} ", ScoreData.id_score_unit).FirstOrDefault();

                        db.Database.ExecuteSqlCommand("Insert into tbl_org_game_user_log (id_user,id_game_content,score,id_score_unit,score_unit,score_type,status,updated_date_time,id_level,id_org_game,attempt_no,timetaken_to_complete,is_completed) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", ScoreData.UID, ScoreData.id_game_content, ScoreData.score, ScoreData.id_score_unit, ScoreData.score_unit, ScoreData.score_type, "A", DateTime.Now, ScoreData.id_level, ScoreData.id_org_game, ScoreData.attempt_no, ScoreData.timetaken_to_complete, ScoreData.is_completed);
                        Result.STATUS = "SUCCESS";
                        Result.OID = ScoreData.OID;
                        Result.MESSAGE = "Successfully posted.";
                        if (ScoreData.is_completed == 1)
                        {

                        }
                    }
                    else
                    {
                        Result.STATUS = "FAILED";
                        Result.MESSAGE = "Duplicate entries.";

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
