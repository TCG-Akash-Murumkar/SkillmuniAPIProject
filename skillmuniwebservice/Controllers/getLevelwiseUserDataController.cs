using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getLevelwiseUserDataController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_org_game, int id_level)
        {
            LevelUserLogResponse Result = new LevelUserLogResponse();

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Result.content = db.Database.SqlQuery<tbl_org_game_content>("select * from tbl_org_game_content inner join tbl_org_game_level_mapping on tbl_org_game_content.id_level=tbl_org_game_level_mapping.id_level  where tbl_org_game_content.id_level={0} and tbl_org_game_level_mapping.id_org_game={1}", id_level, id_org_game).ToList();
                    Result.UID = UID;
                    Result.OID = OID;
                    Result.id_game = id_org_game;
                    Result.id_level = id_level;
                    int cmp_lvl_count = 0;
                    foreach (var itm in Result.content)
                    {
                        itm.user_log = db.Database.SqlQuery<tbl_org_game_user_log>("select * from tbl_org_game_user_log where id_game_content={0} and id_user={1} and id_org_game={2} and id_level={3}", itm.id_game_content, UID, id_org_game, id_level).ToList();
                        foreach (var ulg in itm.user_log)
                        {
                            if (ulg.is_completed == 1)
                            {
                                cmp_lvl_count++;
                            }
                        }
                        if (itm.user_log != null)
                        {
                            itm.badge_log = db.Database.SqlQuery<tbl_org_game_badge_master>("select * from tbl_org_game_badge_master inner join tbl_org_game_content_badge_mapping on tbl_org_game_badge_master.id_badge=tbl_org_game_content_badge_mapping.id_badge where tbl_org_game_content_badge_mapping.id_content={0} and tbl_org_game_content_badge_mapping.id_game={1} and id_level={2}", itm.id_game_content, id_org_game, id_level).FirstOrDefault();
                            if (itm.badge_log != null)
                            {
                                itm.badge_log.badge_count = db.Database.SqlQuery<int>(" select COUNT(id_log) AS total from tbl_org_game_badge_user_log where id_user = {0} and id_content = {1} and id_game = {2} and id_level={3}", UID, itm.id_game_content, id_org_game, id_level).FirstOrDefault();
                                itm.badge_log.is_achieved = 0;

                                if (itm.badge_log.badge_count > 0)
                                {

                                    itm.badge_log.is_achieved = 1;

                                }
                            }

                        }

                        //itm.user_log = db.Database.SqlQuery<tbl_org_game_user_log>("select * from tbl_org_game_user_log where id_game_content={0} and id_user={1} and id_org_game={2} and id_level={3} and updated_date_time=(SELECT MAX(updated_date_time)  FROM tbl_org_game_user_log where id_game_content={3} and id_user={4} and id_org_game={5} and id_level={6} )", itm.id_game_content, UID, id_org_game, id_level, itm.id_game_content, UID, id_org_game, id_level).ToList();


                    }
                    if (Result.content.Count == cmp_lvl_count)
                    {
                        Result.is_level_completed = 1;
                    }

                    Result.level_badge_log = db.Database.SqlQuery<tbl_org_game_badge_master>("select * from tbl_badge_master inner join tbl_org_game_badge_level_mapping on tbl_badge_master.id_badge=tbl_org_game_badge_level_mapping.id_badge where tbl_org_game_badge_level_mapping.id_level={0} and tbl_org_game_badge_level_mapping.id_org_game={1} ", id_level, id_org_game).FirstOrDefault();
                    if (Result.level_badge_log != null)
                    {
                        Result.level_badge_log.badge_count = db.Database.SqlQuery<int>(" select COUNT(id_log) AS total from tbl_org_game_badge_user_log where id_user = {0} and id_content = {1} and id_game = {2} and id_level={3}", UID, 0, id_org_game, id_level).FirstOrDefault();
                        Result.level_badge_log.is_achieved = 0;

                        if (Result.level_badge_log.badge_count > 0)
                        {

                            Result.level_badge_log.is_achieved = 1;

                        }

                    }

                }



            }
            catch (Exception e)
            {
                throw e;
            }
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
           

            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

    }
}
