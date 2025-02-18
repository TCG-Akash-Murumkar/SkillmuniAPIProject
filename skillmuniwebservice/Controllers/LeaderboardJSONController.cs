using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
{
    public class LeaderboardJSONController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID)
        {
            LeaderboardJSONResponce responce = new LeaderboardJSONResponce();
            try
            {
                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                string gamename = "";

                tbl_profile prof = new tbl_profile();
                List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
                List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
                List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    gamename = db.Database.SqlQuery<string>("select name from tbl_game_master where id_game={0}", GameId).FirstOrDefault();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();
                    badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                    bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();


                }
                leag = leag.OrderBy(o => o.minscore).ToList();
                int userleagueid = 0;

                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                //Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID);
                //Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
                List<LeaderBoardUserList> requserlist = new List<LeaderBoardUserList>();
                LeaderBoardUserList cur_user = new LeaderBoardUserList();
                //cur_user = Leader.UserList.Where<LeaderBoardUserList>(t => t.id_user == UID).FirstOrDefault();
                Leader.userscore = cur_user.metric_score;
                int league_cnt = leag.Count;
                int j = 0;
                int z = 1;

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Leader.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault();
                }

                //if (Leader.userleague == null)
                //{
                //    foreach (var itm in leag)
                //    {
                //        if (Leader.userscore > itm.minscore)
                //        {
                //            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //            {
                //                if (z < league_cnt)
                //                {
                //                    Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                //                }
                //            }
                //        }
                //        j++;
                //        z++;
                //    }

                //}
                if (Leader.userleague == null)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {

                        Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                    }


                }

                int i = 1;
                int rnk = 1;
                //foreach (var itm in Leader.UserList)
                //{
                //    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //    {
                //        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                //        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                //        {


                //            if (profobj.PROFILE_IMAGE == "null")
                //            {
                //                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

                //            }
                //            else
                //            {
                //                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                //            }


                //            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                //            itm.city = profobj.CITY;
                //            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                //            instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                //            itm.userbadge = new List<tbl_badge_master>();
                //            itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);
                //            foreach (var bad in instbadgemaster)
                //            {
                //                bad.WonFlag = 0;
                //                foreach (var ob in itm.Badge)
                //                {

                //                    if (ob.id_badge == bad.id_badge)
                //                    {
                //                        bad.WonFlag = 1;
                //                        bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                //                    }


                //                }

                //                itm.userbadge.Add(bad);
                //            }
                //            int a = 0;
                //            int b = 1;
                //            //foreach (var league_itm in leag)
                //            //{
                //            //    if (itm.metric_score > league_itm.minscore)
                //            //    {

                //            //        if (b < league_cnt)
                //            //        {
                //            //            itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[a + 1].id_league).FirstOrDefault();

                //            //        }

                //            //    }
                //            //    a++;
                //            //    b++;
                //            //}

                //            //if (itm.userleague == null)
                //            //{

                //            //    itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();



                //            //}


                //            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user,GameId).FirstOrDefault();


                //            //if (itm.userleague == null)
                //            //{
                //            //    foreach (var item in leag)
                //            //    {
                //            //        if (itm.metric_score > item.minscore)
                //            //        {

                //            //            if (z < league_cnt)
                //            //            {
                //            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                //            //            }

                //            //        }
                //            //        a++;
                //            //        b++;
                //            //    }

                //            //}
                //            if (itm.userleague == null)
                //            {

                //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                //            }


                //            if (Leader.userleague == itm.userleague && prof.id_stream == profobj.id_stream)
                //            {
                //                itm.Rank = rnk;
                //                requserlist.Add(itm);
                //                rnk++;
                //                i++;
                //            }

                //        }



                //    }
                //}
                //Leader.UserList = requserlist;
                int id_currency = 0;
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                    //foreach (var itm in Leader.UserList)
                    //{
                    //    List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                    //    user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                    //    int objcurval = 0;
                    //    foreach (var bad in itm.userbadge)
                    //    {
                    //        foreach (var ob in user_bad_log)
                    //        {
                    //            if (ob.id_badge == bad.id_badge)
                    //            {
                    //                bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                    //                bad.currency_name = cur_name;
                    //                objcurval = objcurval + bad.currency_value;

                    //            }
                    //        }
                    //    }
                    //    itm.currencyvalue = objcurval;

                    //}


                }



                FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    // Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    // Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
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



                    header.currency = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                    header.specialmetric = db.Database.SqlQuery<string>("select name from tbl_special_metric_master where id_special_metric={0}", splmetid).FirstOrDefault();
                    header.theme_metric = db.Database.SqlQuery<string>("select metric_value from tbl_theme_metric where id_theme={0}", 9).FirstOrDefault();
                    header.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"] + db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                }




                foreach (var bad in badgemaster)
                {
                    bad.WonFlag = 0;
                    foreach (var ob in bad_log)
                    {

                        if (ob.id_badge == bad.id_badge)
                        {
                            bad.WonFlag = 1;
                            bad.eligiblescore += 1;
                            //if (bad.eligiblescore > 3)
                            //{
                            //    bad.eligiblescore = 3;
                            //}
                        }


                    }
                }
                responce.Leader =Leader;
                responce.header = header;
                responce.badgemaster = badgemaster;
                responce.gamename = gamename;
                //ViewData["Leader"] = Leader;
                //ViewData["Header"] = header;
                //ViewData["UID"] = UID;
                //ViewData["OID"] = OID;
                //ViewData["badgemaster"] = badgemaster;
                //ViewData["gamename"] = gamename;
            }
            catch (Exception e)
            {

                throw e;
            }
            return Request.CreateResponse(HttpStatusCode.OK, responce);


        }
    }
}
