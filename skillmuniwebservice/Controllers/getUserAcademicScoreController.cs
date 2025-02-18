using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;

namespace m2ostnextservice.Controllers
{
    public class getUserAcademicScoreController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID,int AcademicTileId)
        {

            UserScoreResponse Score = new UserScoreResponse();
            int GameId = 0;



            List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();

            }

            Score.id_game = GameId;
            Score.id_user = UID;


            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                List<tbl_user_game_score_log> scorelog = new List<tbl_user_game_score_log>();
                scorelog = db.Database.SqlQuery<tbl_user_game_score_log>("select * from tbl_user_game_score_log where id_game={0} and id_user={1} and id_academic_tile={2} ", GameId, UID, AcademicTileId).ToList();
                double score = 0;
                foreach (var item in scorelog)
                {
                    score += item.score;
                }
                Score.userscore = score;
                List<tbl_user_game_special_metric_score_log> splscorelog = new List<tbl_user_game_special_metric_score_log>();
                splscorelog = db.Database.SqlQuery<tbl_user_game_special_metric_score_log>("select * from tbl_user_game_special_metric_score_log where id_game={0} and id_user={1} and id_academic_tile={2} ", GameId, UID,AcademicTileId).ToList();
                double splscore = 0;
                foreach (var item in splscorelog)
                {
                    splscore += item.score;
                }
                Score.specialmetricscore = splscore;
                List<tbl_badge_master> userbadge = new List<tbl_badge_master>();
                List<tbl_badge_master> instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                List<UserBadge> Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                foreach (var bad in instbadgemaster)
                {
                    bad.WonFlag = 0;
                    foreach (var ob in Badge)
                    {

                        if (ob.id_badge == bad.id_badge)
                        {
                            bad.WonFlag = 1;
                            bad.eligiblescore = Convert.ToInt32(Score.userscore / ob.eligible_score);
                        }


                    }

                    userbadge.Add(bad);
                }

                int id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                string cur_image = db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_currency={0}", id_currency).FirstOrDefault();


                int objcurval = 0;
                foreach (var bad in userbadge)
                {

                    if (bad.WonFlag == 1)
                    {
                        bad.currency_value = bad.eligiblescore * db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                        bad.currency_name = cur_name;
                        objcurval = objcurval + bad.currency_value;

                    }
                }
                Score.currency_value = objcurval;
                Score.currency_name = cur_name;
                Score.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"].ToString() + cur_image;




            }
            return Request.CreateResponse(HttpStatusCode.OK, Score);
        }


    }
}
