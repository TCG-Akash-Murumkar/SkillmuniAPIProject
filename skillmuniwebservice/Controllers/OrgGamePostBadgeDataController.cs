using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class OrgGamePostBadgeDataController : ApiController
    {
        public HttpResponseMessage Post([FromBody]PostBadgeLog Badge)
        {
            ScoreLOgicResponse Result = new ScoreLOgicResponse();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    tbl_org_game_badge_user_log log = new tbl_org_game_badge_user_log();

                    //ScoreData.log.id_score_unit = db.Database.SqlQuery<int>("select ").FirstOrDefault();
                    if (Badge.id_content == 0)
                    {
                        log.id_badge = db.Database.SqlQuery<int>(" SELECT id_badge FROM tbl_org_game_badge_level_mapping where id_level ={0} and id_org_game = {1}", Badge.id_level, Badge.id_game).FirstOrDefault();
                        log.id_content = 0;

                    }
                    else
                    {
                        log.id_badge = db.Database.SqlQuery<int>(" SELECT id_badge FROM tbl_org_game_content_badge_mapping where id_content ={0} and id_game = {1}", Badge.id_content, Badge.id_game).FirstOrDefault();

                        log.id_content = Badge.id_content;
                    }


                    log.id_game = Badge.id_game;
                    log.id_level = Badge.id_level;
                    log.id_user = Badge.UID;
                    log.status = "A";
                    log.updated_date_time = DateTime.Now;

                    db.Database.ExecuteSqlCommand("Insert into tbl_org_game_badge_user_log (id_badge,id_game,id_level,id_content,id_user,status,updated_date_time) values ({0},{1},{2},{3},{4},{5},{6})", log.id_badge, log.id_game, log.id_level, log.id_content, log.id_user, log.status, log.updated_date_time);


                    
                    Result.STATUS = "SUCCESS";

                    Result.MESSAGE = "Successfully posted.";


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
