using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getLevelsForGameController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID,int id_org_game)
        {
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            level_reponseResult level = new level_reponseResult();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                level.level = db.Database.SqlQuery<level_reponse>("SELECT * FROM tbl_org_game_level_mapping inner join tbl_org_game_level on tbl_org_game_level_mapping.id_level=tbl_org_game_level.id_level where tbl_org_game_level.id_org={0} and id_org_game={1} ORDER BY tbl_org_game_level.level_sequence ASC", OID,id_org_game).ToList();
                tbl_org_game_master game_master = db.Database.SqlQuery<tbl_org_game_master>("select * from tbl_org_game_master where id_org_game={0} ",id_org_game).FirstOrDefault();
                if (game_master.game_start_date_time <= DateTime.Now && game_master.game_end_date_time > DateTime.Now)
                {
                    level.is_live_game = 1;

                }
                
            }


            return Request.CreateResponse(HttpStatusCode.OK, level);
        }

    }
}
