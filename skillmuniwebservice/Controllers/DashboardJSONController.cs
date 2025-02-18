using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

using m2ostnextservice.Models;
using Newtonsoft.Json;
namespace m2ostnextservice.Controllers
{
    public class DashboardJSONController : ApiController
    {
        public HttpResponseMessage GET(int UID, int OID)
        {
            FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
            LeaderBoardResponse Leader = new LeaderBoardResponse();
            List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
            List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
            List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
            tbl_profile prof = new tbl_profile();
            int totalcurrency = 0;
            int GameId = 0;

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();
                    header.currency = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                    header.specialmetric = db.Database.SqlQuery<string>("select name from tbl_special_metric_master where id_special_metric={0}", splmetid).FirstOrDefault();
                    header.theme_metric = db.Database.SqlQuery<string>("select metric_value from tbl_theme_metric where id_theme={0}", 9).FirstOrDefault();
                    header.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"] + db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();

                    // User image update
                    if (prof.social_dp_flag == 0)
                    {
                        Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    }
                    else if (prof.PROFILE_IMAGE == "null")
                    {
                        Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
                    }
                    else
                    {
                        Leader.UserProfileImage = db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    }
                    // end of the validation
                    //Leader.UserProfileImage = db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();

                    Leader.UserName = db.Database.SqlQuery<string>(" select  concat (FIRSTNAME ,' ', LASTNAME) as Name from   tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);

                }
                int league_cnt = leag.Count;
                int j = 0;
                int z = 1;
                //foreach (var itm in leag)
                //{
                //    if (Leader.userscore > itm.minscore)
                //    {
                //        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //        {
                //            if (z < league_cnt)
                //            {
                //                Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                //            }
                //        }
                //    }
                //    j++;
                //    z++;
                //}
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Leader.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0}", UID).FirstOrDefault();
                }

                if (Leader.userleague == null)
                {
                    foreach (var itm in leag)
                    {
                        if (Leader.userscore > itm.minscore)
                        {
                            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                            {
                                if (z < league_cnt)
                                {
                                    Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                                }
                            }
                        }
                        j++;
                        z++;
                    }

                }
                if (Leader.userleague == null)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {

                        Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                    }


                }

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    //if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }

                    bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();
                }


                //foreach (var bad in badgemaster)
                //{
                //    bad.WonFlag = 0;
                //    foreach (var ob in Leader.Badge)
                //    {

                //        if (ob.id_badge == bad.id_badge)
                //        {
                //            bad.WonFlag = 1;
                //            bad.eligiblescore = Convert.ToInt32(Leader.userscore / ob.eligible_score);
                //            if (bad.eligiblescore > 3)
                //            {
                //                bad.eligiblescore = 3;
                //            }
                //            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //            {
                //                bad.money_value = bad.eligiblescore * db.Database.SqlQuery<int>("select money_value from tbl_currency_data where id_badge={0} and id_game={1}", bad.id_badge, GameId).FirstOrDefault();
                //                totalcurrency = totalcurrency + bad.money_value;
                //            }
                //        }


                //    }
                //}

                foreach (var bad in badgemaster)
                {
                    bad.WonFlag = 0;
                    foreach (var ob in bad_log)
                    {

                        if (ob.id_badge == bad.id_badge)
                        {
                            bad.WonFlag = 1;
                            bad.eligiblescore += 1;
                            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                            {
                                bad.money_value = db.Database.SqlQuery<int>("select money_value from tbl_currency_data where id_badge={0} and id_game={1}", bad.id_badge, GameId).FirstOrDefault();
                                totalcurrency = totalcurrency + bad.money_value;
                            }
                        }


                    }
                }


            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e);
                throw e;

            }

            //if (Leader.UserProfileImage == "null" || Leader.UserProfileImage =="noframe.png")
            //{
            //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

            //}
            int used_money = 0;
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                used_money = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(usedmoney),0) AS amnt FROM tbl_couponsredeemed where id_user={0} and id_game={1} and status={2}", UID, GameId, "A").FirstOrDefault();

            }
            totalcurrency = totalcurrency - used_money;

            MyDashboardJSONResponce resp = new MyDashboardJSONResponce();
            resp.Leader = Leader;
            resp.header = header;
            resp.badgemaster = badgemaster;
            resp.totalcurrency = totalcurrency;

            return Request.CreateResponse(HttpStatusCode.OK, resp);

        }
    }
}
