using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class getUserBadgesController : ApiController
    {

        public HttpResponseMessage Get(int UID, int OID)
        {

            int GameId = 0;
            int beg_year = 0;
            int current_month = DateTime.Now.Month;
            int current_year= DateTime.Now.Year;
            List<UserBadgeObj> badge = new List<UserBadgeObj>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0}", 9).FirstOrDefault();
                DateTime gamestart= db.Database.SqlQuery<DateTime>("select updated_date_time from tbl_game_master where id_game={0}", GameId).FirstOrDefault();
                beg_year = gamestart.Year;
                List<tbl_badge_master> badgmas = new List<tbl_badge_master>();
                badgmas= db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
                string jsonres = new UniversityScoringlogic().getApiResponseString(str);
                UserScoreResponse scoreres = JsonConvert.DeserializeObject<UserScoreResponse>(jsonres);


                foreach (var itm in badgmas)
                {
                    itm.eligiblescore= db.Database.SqlQuery<int>("select required_score from tbl_badge_data where id_game={0} and id_badge={1}", GameId,itm.id_badge).FirstOrDefault();
                    itm.badge_logo = ConfigurationManager.AppSettings["BadgeBase"].ToString() + itm.badge_logo;
                }
                int i = current_year - beg_year;
                for (int j = 0; j <= i; j++)
                {
                   
                    for (int mon = 1; mon <= 12; mon++)
                    {
                        if (beg_year == current_year && mon > current_month)
                        {

                        }
                        else
                        {
                            foreach (var bad in badgmas)
                            {
                                UserBadgeObj obj = new UserBadgeObj();
                                int score = db.Database.SqlQuery<int>("SELECT sum(score)  FROM tbl_user_game_score_log WHERE YEAR(updated_date_time) = {0} AND MONTH(updated_date_time) = {1} and id_user={2}", beg_year, mon, UID).FirstOrDefault();
                                if (bad.eligiblescore <= score)
                                {
                                    obj.badge_won = 1;
                                    obj.id_badge = bad.id_badge;
                                    obj.id_game = GameId;
                                    obj.id_user = UID;
                                    obj.month = mon;
                                    obj.year = beg_year;
                                    badge.Add(obj);

                                }



                            }

                        }
                           

                       
                    }



                    beg_year++;
                   

                }



            }

           



            return Request.CreateResponse(HttpStatusCode.OK, badge);
        }




    }
}
