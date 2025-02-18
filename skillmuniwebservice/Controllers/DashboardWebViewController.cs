using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using m2ostnextservice.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace m2ostnextservice.Controllers
{
    public class DashboardWebViewController : Controller
    {
        // GET: DashboardWebView
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FootballLeaderBoard(int UID, int OID)
        {
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
                ViewData["Leader"] = Leader;
                ViewData["Header"] = header;
                ViewData["UID"] = UID;
                ViewData["OID"] = OID;
                ViewData["badgemaster"] = badgemaster;
                ViewData["gamename"] = gamename;
            }
            catch (Exception e)
            {

                throw e;
            }
            return View();
        }
        public ActionResult ProfileHome(int UID, int OID)
        {
            tbl_profile prof = new tbl_profile();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
            }

            string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
            string sampleJson4 = new UniversityScoringlogic().getApiResponseString(str);
            UserScoreResponse briefData = JsonConvert.DeserializeObject<UserScoreResponse>(sampleJson4);
            //if (prof.PROFILE_IMAGE == "null")
            //{
            //    prof.PROFILE_IMAGE = ConfigurationManager.AppSettings["ProfileDefaultBase"];

            //}
            //else
            //{
            //    prof.PROFILE_IMAGE = ConfigurationManager.AppSettings["ProfileImageBase"] + prof.PROFILE_IMAGE;

            //}

                prof.PROFILE_IMAGE = ConfigurationManager.AppSettings["ProfileImageBase"] + prof.PROFILE_IMAGE;

            prof.FIRSTNAME = prof.FIRSTNAME + " " + prof.LASTNAME;
            ViewData["profile"] = prof;
            ViewData["UID"] = UID;
            ViewData["OID"] = OID;
            ViewData["briefData"] = briefData;
            return View();
        }

        public ActionResult AssessmentSheet(string brfcode, int UID, int OID, int ACID, int BriefTileID = 0)
        {
            try
            {


                int _orgid = OID; int _userid = UID;
                string brf = brfcode;
                UserScoreResponse scoreres = new UserScoreResponse();

                //string str = APIString.API + "getBriefResourceDetailWithQuestionTheme?brf=" + brf + "&UID=" + _userid + "&OID=" + _orgid;
                //string sampleJson4 = new UniversityScoringlogic().getApiResponseString(str);
                //BriefResource briefData = JsonConvert.DeserializeObject<BriefResource>(sampleJson4);

                BriefResource briefData = new BriefModel().getBriefData(brf, _userid, _orgid);


                //string str1 = APIString.API + "getScheduledBriefList?UID=" + _userid + "&OID=" + _orgid;
                //string sampleJson5 = new UniversityScoringlogic().getApiResponseString(str1);
                //List<APIBrief> ResposeBack = JsonConvert.DeserializeObject<List<APIBrief>>(sampleJson5);
                string[] colors = new string[5];
                List<BriefChart> right = new List<BriefChart>();
                List<BriefChart> wrong = new List<BriefChart>();

                if (briefData.RESULTSTATUS == 1)
                {
                    string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
                    string jsonres = new UniversityScoringlogic().getApiResponseString(str);
                    scoreres = JsonConvert.DeserializeObject<UserScoreResponse>(jsonres);
                    // briefData.SplScore = scoreres.specialmetricscore;

                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_user_game_special_metric_score_log splscorelog = db.Database.SqlQuery<tbl_user_game_special_metric_score_log>("select * from tbl_user_game_special_metric_score_log where id_brief={0} and id_user={1}", briefData.BRIEF.id_brief_master, UID).FirstOrDefault();
                        if (splscorelog != null)
                        {
                            briefData.SplScore = splscorelog.score;

                        }

                        foreach (var itm in briefData.RESULT.briefReturn)
                        {

                            itm.questiontheme = db.Database.SqlQuery<int>("select question_theme_type from tbl_brief_question where id_brief_question={0}", itm.id_question).FirstOrDefault();
                            //if (itm.questiontheme == 1)
                            //{


                            //}
                            if (itm.questiontheme == 2)
                            //else if (itm.questiontheme == 2)
                            {
                                itm.questionchoicetype = db.Database.SqlQuery<int>("select question_choice_type from tbl_brief_question where id_brief_question={0}", itm.id_question).FirstOrDefault();
                                itm.answerchoicetype = db.Database.SqlQuery<int>("select choice_type from tbl_brief_answer where id_brief_answer={0}", itm.id_answer).FirstOrDefault();
                                if (itm.id_wans > 0)
                                {
                                    itm.wanschoicetype = db.Database.SqlQuery<int>("select choice_type from tbl_brief_answer where id_brief_answer={0}", itm.id_wans).FirstOrDefault();
                                }
                                if (itm.questionchoicetype == 1)
                                {

                                }
                                else if (itm.questionchoicetype == 2)
                                {
                                    itm.questionimg = db.Database.SqlQuery<string>("select question_image from tbl_brief_question where id_brief_question={0}", itm.id_question).FirstOrDefault();

                                }
                                else if (itm.questionchoicetype == 3)
                                {
                                    itm.questionimg = db.Database.SqlQuery<string>("select question_image from tbl_brief_question where id_brief_question={0}", itm.id_question).FirstOrDefault();

                                }

                                if (itm.answerchoicetype == 1)
                                {

                                }
                                else if (itm.answerchoicetype == 2)
                                {
                                    itm.answerimg = db.Database.SqlQuery<string>("select choice_image from tbl_brief_answer where id_brief_answer={0}", itm.id_answer).FirstOrDefault();

                                }
                                else if (itm.answerchoicetype == 3)
                                {
                                    itm.answerimg = db.Database.SqlQuery<string>("select choice_image from tbl_brief_answer where id_brief_answer={0}", itm.id_answer).FirstOrDefault();

                                }
                                if (itm.id_wans > 0)
                                {
                                    //if (itm.wanschoicetype == 1)
                                    //{

                                    //}
                                    //else if (itm.wanschoicetype == 2)

                                    if (itm.wanschoicetype == 2)
                                    {
                                        itm.wansimg = db.Database.SqlQuery<string>("select choice_image from tbl_brief_answer where id_brief_answer={0}", itm.id_wans).FirstOrDefault();

                                    }
                                    else if (itm.wanschoicetype == 3)
                                    {
                                        itm.wansimg = db.Database.SqlQuery<string>("select choice_image from tbl_brief_answer where id_brief_answer={0}", itm.id_wans).FirstOrDefault();

                                    }
                                }

                            }



                        }


                    }
                    //foreach (var itm in briefData.RESULT.complexity)
                    //{
                    //    BriefChart rtemp = new BriefChart();
                    //    BriefChart wtemp = new BriefChart();
                    //    rtemp.Label = itm.question_complexity_label;
                    //    rtemp.complexity = itm.question_complexity;
                    //    rtemp.value = (itm.RIGHTCOUNT / briefData.RESULT.briefReturn.Count) * 100;

                    //    wtemp.Label = itm.question_complexity_label;
                    //    wtemp.complexity = itm.question_complexity;
                    //    wtemp.value = ((itm.TOTALCOUNT - itm.RIGHTCOUNT) / briefData.RESULT.briefReturn.Count) * 100;
                    //    right.Add(rtemp);
                    //    wrong.Add(wtemp);
                    //}
                }
                ViewData["right"] = right;
                ViewData["wrong"] = wrong;
                // ViewData["brfList"] = ResposeBack;
                ViewData["brief"] = briefData;
                ViewData["UID"] = UID;
                ViewData["OID"] = OID;
                ViewData["ACID"] = ACID;
                ViewData["BTileId"] = BriefTileID;
                ViewData["scoreres"] = scoreres;





            }
            catch (Exception e)
            {
                //throw e;
                throw e;
            }

            return View();
        }

        public string AssessmentResult(int UID, int OID)
        {

            UID = Convert.ToInt32(Request.Form["UID"].ToString());
            OID = Convert.ToInt32(Request.Form["OID"].ToString());
            int acid = Convert.ToInt32(Request.Form["ACID"].ToString());
            int _orgid = OID; int _userid = UID;
            string brfcd = Request.Form["brf_id"].ToString();
            int qCount = Convert.ToInt32(Request.Form["qtn_count_" + brfcd].ToString());
            string brfcode = Request.Form["brfcode"].ToString();
            int brief_tile_id = Convert.ToInt32(Request.Form["BTileId"].ToString());

            ////int asid = Convert.ToInt32(Request.Form[id]);
            string arid = "";
            List<string> ar_id = new List<string>();

            int j = 0;
            string[] ars = new string[qCount];

            for (int i = 1; i <= qCount; i++)
            {
                ar_id.Add(Request.Form["qna_" + brfcd + i].ToString());
                string srt = Request.Form["qna_" + brfcd + i].ToString();
                ars[j] = srt;
                j++;
                //arid +='"'+   Request.Form["qna_" + id + i].ToString()+'"' + ',';
            }
            string arss = "";
            for (int i = 1; i <= qCount; i++)
            {
                arid += Request.Form["qna_" + brfcd + i] + ";";
            }
            arid = arid.TrimEnd(';');
            arid = arid.Trim();

            //arid = arid.TrimEnd(',');
            //arss = arid.TrimEnd('"');
            //arss = arid.TrimEnd(',');
            //arid = arid.Trim ('"');
            //arid = arid.Trim();
            //arid= arid.Replace("\\", "");

            string tes = Convert.ToString(ar_id);
            string ansrlist = JsonConvert.SerializeObject(ar_id).ToString();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("OID", Convert.ToString(_orgid));
            collection.Add("UID", Convert.ToString(_userid));
            collection.Add("BID", Convert.ToString(brfcd));
            collection.Add("BRF", brfcode);
            collection.Add("ASRQ", arid);
            Regex RgxUrl = new Regex("[^a-z0-9]");
            bool check = RgxUrl.IsMatch(brfcode);

            if (check)
            {
                string str1 = APIString.API + "EvaluateBriefAcademy?OID=" + _orgid + "&UID=" + _userid + "&BID=" + brfcd + "&BRF=" + brfcode + "&ASRQ=" + arid + "&AcademicTileId=" + acid + "&brief_tile_id=" + brief_tile_id;
                string sampleJson5 = new UniversityScoringlogic().getApiResponseString(str1);
                BriefReturnResponse result = JsonConvert.DeserializeObject<BriefReturnResponse>(sampleJson5);

                //string api = APIString.API + "EvaluateBrief";
                //string response = new UniversityScoringlogic().getApiPost(api, collection);
                //BriefReturnResponse result = JsonConvert.DeserializeObject<BriefReturnResponse>(response);
                //assres = result;//global variable
                //string str = APIString.API + "getBriefDetails?brf=" + brf + "&UID=" + _userid + "&OID=" + _orgid;
                //string sampleJson4 = new BriefLogic().getApiResponseString(str);
                //BriefBody briefData = JsonConvert.DeserializeObject<BriefBody>(sampleJson4);

                //string str1 = APIString.API + "getActiveBriefList?UID=" + _userid + "&OID=" + _orgid;
                //string sampleJson5 = new BriefLogic().getApiResponseString(str1);
                //List<APIBrief> ResposeBack = JsonConvert.DeserializeObject<List<APIBrief>>(sampleJson5);

                //ViewData["brfList"] = ResposeBack;
            }
            else
            {
                return brfcode;
            }

            return brfcode;
        }


        public ActionResult MyDashboard(int UID, int OID)
        {
            FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
            LeaderBoardResponse Leader = new LeaderBoardResponse();
            List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
            List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
            UserScoreResponse scoreres = new UserScoreResponse();
            List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
            tbl_profile prof = new tbl_profile();
            int totalcurrency = 0;
            int GameId = 0;

            try
            {
                string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
                string jsonres = new UniversityScoringlogic().getApiResponseString(str);
                scoreres = JsonConvert.DeserializeObject<UserScoreResponse>(jsonres);
                Leader.userscore = scoreres.userscore;
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

            ViewData["Leader"] = Leader;
            ViewData["Header"] = header;
            ViewData["UID"] = UID;
            ViewData["OID"] = OID;
            ViewData["badgemaster"] = badgemaster;
            ViewData["ScoreRes"] = scoreres;
            ViewData["totalcurrency"] = totalcurrency;

            return View();

        }


        public ActionResult RewardsPage(int UID, int OID)
        {



            FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
            LeaderBoardResponse Leader = new LeaderBoardResponse();
            List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
            List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
            UserScoreResponse scoreres = new UserScoreResponse();
            List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
            int totalcurrency = 0;
            int GameId = 0;

            try
            {
                string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
                string jsonres = new UniversityScoringlogic().getApiResponseString(str);
                scoreres = JsonConvert.DeserializeObject<UserScoreResponse>(jsonres);

                Leader.userscore = scoreres.userscore;




                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0} ORDER BY id_badge ASC", 9).ToList();
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();


                    header.currency = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                    header.specialmetric = db.Database.SqlQuery<string>("select name from tbl_special_metric_master where id_special_metric={0}", splmetid).FirstOrDefault();
                    header.theme_metric = db.Database.SqlQuery<string>("select metric_value from tbl_theme_metric where id_theme={0}", 9).FirstOrDefault();
                    header.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"] + db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    Leader.UserProfileImage = db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    Leader.UserName = db.Database.SqlQuery<string>(" select  FIRSTNAME from   tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                    Leader.MailId = db.Database.SqlQuery<string>(" select  EMAIL from   tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    Leader.id_user = UID;
                    bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();


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
                            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                            {
                                //bad.currency_value = bad.eligiblescore * db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_game={1}", bad.id_badge, GameId).FirstOrDefault();
                                //int id_cur = db.Database.SqlQuery<int>("select id_currency from tbl_currency_data where id_badge={0} and id_game={1}", bad.id_badge, GameId).FirstOrDefault();
                                //bad.currency_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_currency={0}", id_cur).FirstOrDefault();
                                ////bad.badge_logo = " ";
                                ////bad.badge_logo = ConfigurationManager.AppSettings["BadgeBase"] + bad.badge_logo;

                                //bad.money_value = db.Database.SqlQuery<int>("select money_value from tbl_currency_data where id_badge={0} and id_game={1}", bad.id_badge, GameId).FirstOrDefault();
                                //totalcurrency = totalcurrency + bad.money_value;
                            }
                        }


                    }
                }




            }
            catch (Exception e)
            {
                throw e;

            }
            int used_money = 0;
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                tbl_user userdetails = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_USER={0}", UID).FirstOrDefault();

                totalcurrency = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(currency_value),0) AS amnt FROM tbl_user_currency_log where id_user={0} and status={1}", UID, "A").FirstOrDefault();//----------02-12-19
                totalcurrency = totalcurrency + db.Database.SqlQuery<int>("SELECT COALESCE(SUM(referral_points),0) AS amnt FROM tbl_referral_code_user_mapping where referral_code={0} and status={1}", userdetails.ref_id, "A").FirstOrDefault();//----------02-12-19

                used_money = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(usedmoney),0) AS amnt FROM tbl_couponsredeemed where id_user={0} and id_game={1} and status={2}", UID, GameId, "A").FirstOrDefault();

            }



            if (Leader.UserProfileImage == "null" || Leader.UserProfileImage =="noframe.png" )
            {
                Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

            }

            totalcurrency = totalcurrency - used_money;
            string base64Currency = "";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Convert.ToString(totalcurrency));
            base64Currency = System.Convert.ToBase64String(plainTextBytes);

            ViewData["Leader"] = Leader;
            ViewData["Header"] = header;
            ViewData["UID"] = UID;
            ViewData["OID"] = OID;
            ViewData["badgemaster"] = badgemaster;
            ViewData["ScoreRes"] = scoreres;
            ViewData["TotalCurrency"] = totalcurrency;
            ViewData["base64Currency"] = base64Currency;
            ViewData["used_money"] = used_money;



            return View();
        }

        public ActionResult RewardsRedirectPage(int UID)
        {
            List<CouponsRedeemed> cpn = new List<CouponsRedeemed>();

            try
            {

                int OID = Convert.ToInt32(ConfigurationManager.AppSettings["SOCIALORG"]);
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();

                    cpn = db.Database.SqlQuery<CouponsRedeemed>("select * from tbl_couponsredeemed where id_user={0} and id_game={1} and status='A'", UID, GameId).ToList();
                }



            }
            catch (Exception e)
            {
                throw e;

            }


            ViewData["cpn"] = cpn;
            return View();
        }


        public ActionResult RewardsPost(string Data, string Hash)
        {

            try
            {
                Data = Server.UrlDecode(Data);
                Hash = Server.UrlDecode(Hash);


                RootObject Rewards = JsonConvert.DeserializeObject<RootObject>(Data);
                Rewards.id_org = Convert.ToInt32(ConfigurationManager.AppSettings["SOCIALORG"]);
                Rewards.id_user = Convert.ToInt32(Rewards.AccountID);
                //System.IO.File.WriteAllText(@"F:\Backup\test.txt", Data);
                //new File.AppendAllText(@"F:\Backup\test.txt", Data);
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    Rewards.id_game = GameId;
                    db.Database.ExecuteSqlCommand("Insert into tbl_rewards_redeem_master (AccountID,EmailAddress,PartnerCode,ProviderCode,RedeemType,TotalPoints,TransactionID,UserName,id_user,id_org,id_game) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", Rewards.AccountID, Rewards.EmailAddress, Rewards.PartnerCode, Rewards.ProviderCode, Rewards.RedeemType, Rewards.TotalPoints, Rewards.TransactionID, Rewards.UserName, Rewards.id_user, Rewards.id_org, Rewards.id_game);
                    foreach (var itm in Rewards.MiscellaneousData1.CouponsRedeemed)
                    {

                        if (itm.CouponID == "931" || itm.CouponID == "1016" || itm.CouponID == "1019")
                        { }
                        else
                        {
                            itm.usedmoney = Convert.ToInt32(itm.PointsUsed);
                            db.Database.ExecuteSqlCommand("Insert into tbl_couponsredeemed (CouponID,WebsiteName,CouponCode,CouponDescription,Link,PointsUsed,Image,ExpiryDate,id_game,id_user,id_org,usedmoney) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", itm.CouponID, itm.WebsiteName, itm.CouponCode, itm.CouponDescription, itm.Link, itm.PointsUsed, itm.Image, itm.ExpiryDate, GameId, Rewards.id_user, Rewards.id_org, itm.usedmoney);
                            //db.Database.ExecuteSqlCommand("Insert into tbl_couponsredeemed (PointsUsed,CouponID,CouponCode,CouponDescription) values({0},{1},{2},{3})", itm.PointsUsed,itm.CouponID,itm.CouponCode,itm.CouponDescription);
                        }
                    }
                }








                //string newUrl;
                //while ((newUrl = Uri.UnescapeDataString(Data)) != Data)
                //    Data = newUrl;

                //Data = newUrl;


            }
            catch (Exception e)
            {
                throw e;

            }



            return View();
        }
        public ActionResult FormSub()
        {
            return View();
        }
        public ActionResult Loginaction()
        {
            try
            {
                string uid = Request.Form["uid"];
                string password = Request.Form["pswd"];
                string result = new RoadMapLogic().LoginValidate(uid, password);
                if (result == "SUCCESS")
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("FormSub");
        }
        public ActionResult FootballLeaderBoardForCollege(int UID, int OID)
        {
            try
            {
                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                string gamename = "";
                int userleagueid = 0;


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
                    userleagueid = db.Database.SqlQuery<int>("select id_league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault(); ;


                }
                leag = leag.OrderBy(o => o.minscore).ToList();
                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID, userleagueid);
                Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
                List<LeaderBoardUserList> requserlist = new List<LeaderBoardUserList>();
                LeaderBoardUserList cur_user = new LeaderBoardUserList();
                cur_user = Leader.UserList.Where<LeaderBoardUserList>(t => t.id_user == UID).FirstOrDefault();
                Leader.userscore = cur_user.metric_score;
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

                //if (Leader.userleague == null)
                //{
                //    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //    {

                //        Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                //    }
                //}
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
                foreach (var itm in Leader.UserList)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                        {


                            if (profobj.PROFILE_IMAGE == "null")
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

                            }
                            else
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                            }


                            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                            itm.city = profobj.CITY;
                            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                            instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                            itm.userbadge = new List<tbl_badge_master>();
                            itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);
                            foreach (var bad in instbadgemaster)
                            {
                                bad.WonFlag = 0;
                                foreach (var ob in itm.Badge)
                                {

                                    if (ob.id_badge == bad.id_badge)
                                    {
                                        bad.WonFlag = 1;
                                        bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                                    }


                                }

                                itm.userbadge.Add(bad);
                            }
                            int a = 0;
                            int b = 1;
                            //foreach (var league_itm in leag)
                            //{
                            //    if (itm.metric_score > league_itm.minscore)
                            //    {

                            //        if (b < league_cnt)
                            //        {
                            //            itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[a + 1].id_league).FirstOrDefault();

                            //        }

                            //    }
                            //    a++;
                            //    b++;
                            //}


                            //if (itm.userleague == null)
                            //{

                            //    itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();



                            //}


                            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user, GameId).FirstOrDefault();


                            //if (itm.userleague == null)
                            //{
                            //    foreach (var item in leag)
                            //    {
                            //        if (itm.metric_score > item.minscore)
                            //        {

                            //                if (z < league_cnt)
                            //                {
                            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                            //                }

                            //        }
                            //        a++;
                            //        b++;
                            //    }

                            //}
                            if (itm.userleague == null)
                            {


                                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();





                            }


                            if (Leader.userleague == itm.userleague && prof.COLLEGE == profobj.COLLEGE && prof.COLLEGE != null && prof.COLLEGE != "")
                            {
                                itm.Rank = rnk;
                                requserlist.Add(itm);
                                rnk++;
                                i++;
                            }

                        }



                    }
                }
                Leader.UserList = requserlist;
                int id_currency = 0;
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                    foreach (var itm in Leader.UserList)
                    {
                        List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                        user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                        int objcurval = 0;
                        foreach (var bad in itm.userbadge)
                        {
                            foreach (var ob in user_bad_log)
                            {
                                if (ob.id_badge == bad.id_badge)
                                {
                                    bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                                    bad.currency_name = cur_name;
                                    objcurval = objcurval + bad.currency_value;

                                }
                            }
                        }
                        itm.currencyvalue = objcurval;

                    }


                }



                FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    header.currency = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                    header.specialmetric = db.Database.SqlQuery<string>("select name from tbl_special_metric_master where id_special_metric={0}", splmetid).FirstOrDefault();
                    header.theme_metric = db.Database.SqlQuery<string>("select metric_value from tbl_theme_metric where id_theme={0}", 9).FirstOrDefault();
                    header.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"] + db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                }
                if (Leader.UserProfileImage == "null")
                {
                    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
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
                ViewData["Leader"] = Leader;
                ViewData["Header"] = header;
                ViewData["UID"] = UID;
                ViewData["OID"] = OID;
                ViewData["badgemaster"] = badgemaster;
                ViewData["gamename"] = gamename;
            }
            catch (Exception e)
            {

                throw e;
            }
            return View();
        }

        public ActionResult FootballLeaderBoardForCountry(int UID, int OID)
        {
            try
            {
                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                string gamename = "";
                int userleagueid = 0;

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
                    Leader.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault();
                    userleagueid = db.Database.SqlQuery<int>("select id_league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault(); ;


                }
                leag = leag.OrderBy(o => o.minscore).ToList();


                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID, userleagueid);
                Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
                List<LeaderBoardUserList> requserlist = new List<LeaderBoardUserList>();
                LeaderBoardUserList cur_user = new LeaderBoardUserList();
                cur_user = Leader.UserList.Where<LeaderBoardUserList>(t => t.id_user == UID).FirstOrDefault();
                Leader.userscore = cur_user.metric_score;
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
                if (Leader.userleague == null)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {

                        Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                    }
                }

                int i = 1;
                int rnk = 1;
                foreach (var itm in Leader.UserList)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                        {


                            if (profobj.PROFILE_IMAGE == "null")
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

                            }
                            else
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                            }


                            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                            itm.city = profobj.CITY;
                            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                            instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                            itm.userbadge = new List<tbl_badge_master>();
                            itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);
                            foreach (var bad in instbadgemaster)
                            {
                                bad.WonFlag = 0;
                                foreach (var ob in itm.Badge)
                                {

                                    if (ob.id_badge == bad.id_badge)
                                    {
                                        bad.WonFlag = 1;
                                        bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                                    }


                                }

                                itm.userbadge.Add(bad);
                            }
                            int a = 0;
                            int b = 1;
                            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user, GameId).FirstOrDefault();


                            //if (itm.userleague == null)
                            //{
                            //    foreach (var item in leag)
                            //    {
                            //        if (itm.metric_score > item.minscore)
                            //        {

                            //            if (z < league_cnt)
                            //            {
                            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                            //            }

                            //        }
                            //        a++;
                            //        b++;
                            //    }

                            //}
                            if (itm.userleague == null)
                            {


                                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();





                            }


                            if (Leader.userleague == itm.userleague && prof.COUNTRY == profobj.COUNTRY)
                            {
                                itm.Rank = rnk;
                                requserlist.Add(itm);
                                rnk++;
                                i++;
                            }

                        }



                    }
                }
                Leader.UserList = requserlist;
                int id_currency = 0;
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                    foreach (var itm in Leader.UserList)
                    {
                        List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                        user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                        int objcurval = 0;
                        foreach (var bad in itm.userbadge)
                        {
                            foreach (var ob in user_bad_log)
                            {
                                if (ob.id_badge == bad.id_badge)
                                {
                                    bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                                    bad.currency_name = cur_name;
                                    objcurval = objcurval + bad.currency_value;

                                }
                            }
                        }
                        itm.currencyvalue = objcurval;

                    }


                }



                FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    header.currency = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                    header.specialmetric = db.Database.SqlQuery<string>("select name from tbl_special_metric_master where id_special_metric={0}", splmetid).FirstOrDefault();
                    header.theme_metric = db.Database.SqlQuery<string>("select metric_value from tbl_theme_metric where id_theme={0}", 9).FirstOrDefault();
                    header.currency_image = ConfigurationManager.AppSettings["CurrencyImageBase"] + db.Database.SqlQuery<string>("select currency_logo from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                }
                if (Leader.UserProfileImage == "null")
                {
                    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
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
                ViewData["Leader"] = Leader;
                ViewData["Header"] = header;
                ViewData["UID"] = UID;
                ViewData["OID"] = OID;
                ViewData["badgemaster"] = badgemaster;
                ViewData["gamename"] = gamename;
            }
            catch (Exception e)
            {

                throw e;
            }
            return View();
        }

        public ActionResult APIREstrictionPage()
        {

            try
            {

            }
            catch (Exception e)
            {

            }
            return View();
        }
        //public void GenerateREf()
        //{
        //    List<tbl_user> user = new List<tbl_user>();

        //    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
        //    {

        //        user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_ORGANIZATION=130").ToList();
        //        foreach (var itm in user)
        //        {

        //            string refe = RandomString(5);
        //            int id = db.Database.SqlQuery<int>("select ID_USER from tbl_user where  ref_id={0}",refe).FirstOrDefault();
        //            db.Database.ExecuteSqlCommand("update  tbl_user set ref_id={0} where ID_USER={1}", refe, itm.ID_USER);


        //        }
        //    }

        //}
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult FootballLeaderboardPartialView(int UID, int OID)
        {

            try
            {

                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                int userleagueid = 0;
                //string gamename = "";

                tbl_profile prof = new tbl_profile();
                List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
                //List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
                //List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    //gamename = db.Database.SqlQuery<string>("select name from tbl_game_master where id_game={0}", GameId).FirstOrDefault();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();
                    //badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                    //bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();
                    userleagueid = db.Database.SqlQuery<int>("select id_league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault(); ;


                }
                leag = leag.OrderBy(o => o.minscore).ToList();

                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                //Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID, userleagueid);
                Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
                List<LeaderBoardUserList> requserlist = new List<LeaderBoardUserList>();
                LeaderBoardUserList cur_user = new LeaderBoardUserList();
                cur_user = Leader.UserList.Where<LeaderBoardUserList>(t => t.id_user == UID).FirstOrDefault();
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
                foreach (var itm in Leader.UserList)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                        {


                            if (profobj.social_dp_flag==0)
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                            }
                            else if (profobj.PROFILE_IMAGE=="null")
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
                            }
                            else
                            {
                                itm.UserProfileImage = profobj.PROFILE_IMAGE; 

                            } 


                            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                            itm.city = profobj.CITY;
                            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                            //instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                            itm.userbadge = new List<tbl_badge_master>();
                            //-Change on 11-11-19
                            //itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);

                            //-Change on 11-11-19--------------------------------------------

                            //foreach (var bad in instbadgemaster)
                            //{
                            //    bad.WonFlag = 0;
                            //    foreach (var ob in itm.Badge)
                            //    {

                            //        if (ob.id_badge == bad.id_badge)
                            //        {
                            //            bad.WonFlag = 1;
                            //            bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                            //        }


                            //    }

                            //    itm.userbadge.Add(bad);
                            //}
                            //---------------------------------------------------------


                            int a = 0;
                            int b = 1;
                            //foreach (var league_itm in leag)
                            //{
                            //    if (itm.metric_score > league_itm.minscore)
                            //    {

                            //        if (b < league_cnt)
                            //        {
                            //            itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[a + 1].id_league).FirstOrDefault();

                            //        }

                            //    }
                            //    a++;
                            //    b++;
                            //}

                            //if (itm.userleague == null)
                            //{

                            //    itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();



                            //}


                            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user, GameId).FirstOrDefault();


                            //if (itm.userleague == null)
                            //{
                            //    foreach (var item in leag)
                            //    {
                            //        if (itm.metric_score > item.minscore)
                            //        {

                            //            if (z < league_cnt)
                            //            {
                            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                            //            }

                            //        }
                            //        a++;
                            //        b++;
                            //    }

                            //}
                            if (itm.userleague == null)
                            {

                                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                            }
                            if (itm.id_user == 3640)
                            {
                                string s = "s";

                            }

                            if (Leader.userleague == itm.userleague) // && prof.id_stream == profobj.id_stream
                            {
                                itm.Rank = rnk;
                                requserlist.Add(itm);
                                itm.currencyvalue = db.Database.SqlQuery<int>("select COALESCE(SUM(currency_value),0) from tbl_user_currency_log where id_user={0}", itm.id_user).FirstOrDefault();
                                string curref = db.Database.SqlQuery<string>("select ref_id from tbl_user where ID_USER={0}", itm.id_user).FirstOrDefault();
                                itm.currencyvalue = itm.currencyvalue + db.Database.SqlQuery<int>("select COALESCE(SUM(referral_points),0) from tbl_referral_code_user_mapping where referral_code={0}", curref).FirstOrDefault();
                                rnk++;
                                i++;
                            }

                        }



                    }
                }
                Leader.UserList = requserlist;
                int id_currency = 0;

                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                //    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                //    foreach (var itm in Leader.UserList)
                //    {
                //        List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                //        user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                //        int objcurval = 0;
                //        foreach (var bad in itm.userbadge)
                //        {
                //            foreach (var ob in user_bad_log)
                //            {
                //                if (ob.id_badge == bad.id_badge)
                //                {
                //                    bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                //                    bad.currency_name = cur_name;
                //                    objcurval = objcurval + bad.currency_value;

                //                }
                //            }
                //        }
                //        itm.currencyvalue = objcurval;

                //    }


                //}

                //-----------------------------------------------------------------------

                //FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                //    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                //    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                //}
                //if (Leader.UserProfileImage == "null")
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
                //}
                //-----------------------------------------------------------------------

                ViewData["Leader"] = Leader;

            }
            catch (Exception e)
            {

                throw e;
            }

            return View();
        }
        public ActionResult FootBallLeaderBoardCollegePartialView(int UID, int OID)
        {

            try
            {
                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                int userleagueid = 0;
                //string gamename = "";

                tbl_profile prof = new tbl_profile();
                List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
                //List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
                //List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    //gamename = db.Database.SqlQuery<string>("select name from tbl_game_master where id_game={0}", GameId).FirstOrDefault();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();
                    //badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                    //bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();
                    userleagueid = db.Database.SqlQuery<int>("select id_league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault(); ;


                }
                leag = leag.OrderBy(o => o.minscore).ToList();

                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                //Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID, userleagueid);
                Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
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
                foreach (var itm in Leader.UserList)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                        {


                            if (profobj.PROFILE_IMAGE == "null")
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

                            }
                            else
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                            }


                            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                            itm.city = profobj.CITY;
                            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                            //instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                            itm.userbadge = new List<tbl_badge_master>();
                            //-Change on 11-11-19
                            //itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);

                            //-Change on 11-11-19--------------------------------------------

                            //foreach (var bad in instbadgemaster)
                            //{
                            //    bad.WonFlag = 0;
                            //    foreach (var ob in itm.Badge)
                            //    {

                            //        if (ob.id_badge == bad.id_badge)
                            //        {
                            //            bad.WonFlag = 1;
                            //            bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                            //        }


                            //    }

                            //    itm.userbadge.Add(bad);
                            //}
                            //---------------------------------------------------------


                            int a = 0;
                            int b = 1;
                            //foreach (var league_itm in leag)
                            //{
                            //    if (itm.metric_score > league_itm.minscore)
                            //    {

                            //        if (b < league_cnt)
                            //        {
                            //            itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[a + 1].id_league).FirstOrDefault();

                            //        }

                            //    }
                            //    a++;
                            //    b++;
                            //}

                            //if (itm.userleague == null)
                            //{

                            //    itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();



                            //}


                            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user, GameId).FirstOrDefault();


                            //if (itm.userleague == null)
                            //{
                            //    foreach (var item in leag)
                            //    {
                            //        if (itm.metric_score > item.minscore)
                            //        {

                            //            if (z < league_cnt)
                            //            {
                            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                            //            }

                            //        }
                            //        a++;
                            //        b++;
                            //    }

                            //}
                            if (itm.userleague == null)
                            {

                                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                            }


                            if (Leader.userleague == itm.userleague && prof.COLLEGE == profobj.COLLEGE && prof.COLLEGE != null)//&& prof.COLLEGE != null && prof.COLLEGE != ""
                            {
                                itm.Rank = rnk;
                                requserlist.Add(itm);
                                itm.currencyvalue = db.Database.SqlQuery<int>("select COALESCE(SUM(currency_value),0) from tbl_user_currency_log where id_user={0}", itm.id_user).FirstOrDefault();
                                string curref = db.Database.SqlQuery<string>("select ref_id from tbl_user where ID_USER={0}", itm.id_user).FirstOrDefault();
                                itm.currencyvalue = itm.currencyvalue + db.Database.SqlQuery<int>("select COALESCE(SUM(referral_points),0) from tbl_referral_code_user_mapping where referral_code={0}", curref).FirstOrDefault();
                                rnk++;
                                i++;
                            }

                        }



                    }
                }
                Leader.UserList = requserlist;
                int id_currency = 0;

                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                //    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                //    foreach (var itm in Leader.UserList)
                //    {
                //        List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                //        user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                //        int objcurval = 0;
                //        foreach (var bad in itm.userbadge)
                //        {
                //            foreach (var ob in user_bad_log)
                //            {
                //                if (ob.id_badge == bad.id_badge)
                //                {
                //                    bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                //                    bad.currency_name = cur_name;
                //                    objcurval = objcurval + bad.currency_value;

                //                }
                //            }
                //        }
                //        itm.currencyvalue = objcurval;

                //    }


                //}

                //-----------------------------------------------------------------------

                //FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                //    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                //    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                //}
                //if (Leader.UserProfileImage == "null")
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
                //}
                //-----------------------------------------------------------------------

                ViewData["Leader"] = Leader;

            }
            catch (Exception e)
            {

                throw e;
            }

            return View();

        }
        public ActionResult FootBallLeaderBoardCountryPartialView(int UID, int OID)
        {
            try
            {
                LeaderBoardResponse Leader = new LeaderBoardResponse();
                int GameId = 0;
                int userleagueid = 0;
                //string gamename = "";

                tbl_profile prof = new tbl_profile();
                List<tbl_leagues_data> leag = new List<tbl_leagues_data>();
                //List<tbl_badge_master> badgemaster = new List<tbl_badge_master>();
                //List<tbl_user_badge_log> bad_log = new List<tbl_user_badge_log>();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                    //gamename = db.Database.SqlQuery<string>("select name from tbl_game_master where id_game={0}", GameId).FirstOrDefault();
                    prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    leag = db.Database.SqlQuery<tbl_leagues_data>("select * from tbl_leagues_data where id_game={0}", GameId).ToList();
                    //badgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                    //bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", UID, GameId).ToList();
                    userleagueid = db.Database.SqlQuery<int>("select id_league from tbl_user_league_log where id_user={0} and id_game={1}", UID, GameId).FirstOrDefault(); ;


                }
                leag = leag.OrderBy(o => o.minscore).ToList();

                Leader.id_game = GameId;
                Leader.id_user = UID;
                Leader.UserName = prof.FIRSTNAME + " " + prof.LASTNAME;
                //Leader.City = prof.CITY;
                //Leader.Badge = new UniversityScoringlogic().getBadgeList(UID, GameId);
                Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId, OID, userleagueid);
                Leader.UserList = Leader.UserList.OrderByDescending(o => o.metric_score).ToList();
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
                foreach (var itm in Leader.UserList)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        tbl_profile profobj = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.id_user).FirstOrDefault();

                        if (i <= Convert.ToInt32(ConfigurationManager.AppSettings["UserListLimit"]) && profobj != null)
                        {


                            if (profobj.PROFILE_IMAGE == "null")
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];

                            }
                            else
                            {
                                itm.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + profobj.PROFILE_IMAGE;

                            }


                            itm.Username = profobj.FIRSTNAME + " " + profobj.LASTNAME;
                            itm.city = profobj.CITY;
                            List<tbl_badge_master> instbadgemaster = new List<tbl_badge_master>();
                            //instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
                            itm.userbadge = new List<tbl_badge_master>();
                            //-Change on 11-11-19
                            //itm.Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);

                            //-Change on 11-11-19--------------------------------------------

                            //foreach (var bad in instbadgemaster)
                            //{
                            //    bad.WonFlag = 0;
                            //    foreach (var ob in itm.Badge)
                            //    {

                            //        if (ob.id_badge == bad.id_badge)
                            //        {
                            //            bad.WonFlag = 1;
                            //            bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                            //        }


                            //    }

                            //    itm.userbadge.Add(bad);
                            //}
                            //---------------------------------------------------------


                            int a = 0;
                            int b = 1;
                            //foreach (var league_itm in leag)
                            //{
                            //    if (itm.metric_score > league_itm.minscore)
                            //    {

                            //        if (b < league_cnt)
                            //        {
                            //            itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[a + 1].id_league).FirstOrDefault();

                            //        }

                            //    }
                            //    a++;
                            //    b++;
                            //}

                            //if (itm.userleague == null)
                            //{

                            //    itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();



                            //}


                            itm.userleague = db.Database.SqlQuery<string>("select league from tbl_user_league_log where id_user={0} and id_game={1}", itm.id_user, GameId).FirstOrDefault();


                            //if (itm.userleague == null)
                            //{
                            //    foreach (var item in leag)
                            //    {
                            //        if (itm.metric_score > item.minscore)
                            //        {

                            //            if (z < league_cnt)
                            //            {
                            //                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[j + 1].id_league).FirstOrDefault();

                            //            }

                            //        }
                            //        a++;
                            //        b++;
                            //    }

                            //}
                            if (itm.userleague == null)
                            {

                                itm.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault();


                            }


                            if (Leader.userleague == itm.userleague && prof.COUNTRY == profobj.COUNTRY)
                            {
                                itm.Rank = rnk;
                                requserlist.Add(itm);
                                itm.currencyvalue = db.Database.SqlQuery<int>("select COALESCE(SUM(currency_value),0) from tbl_user_currency_log where id_user={0}", itm.id_user).FirstOrDefault();
                                string curref = db.Database.SqlQuery<string>("select ref_id from tbl_user where ID_USER={0}", itm.id_user).FirstOrDefault();
                                itm.currencyvalue = itm.currencyvalue + db.Database.SqlQuery<int>("select COALESCE(SUM(referral_points),0) from tbl_referral_code_user_mapping where referral_code={0}", curref).FirstOrDefault();
                                rnk++;
                                i++;
                            }

                        }



                    }
                }
                Leader.UserList = requserlist;
                int id_currency = 0;

                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                //    string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();


                //    foreach (var itm in Leader.UserList)
                //    {
                //        List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                //        user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                //        int objcurval = 0;
                //        foreach (var bad in itm.userbadge)
                //        {
                //            foreach (var ob in user_bad_log)
                //            {
                //                if (ob.id_badge == bad.id_badge)
                //                {
                //                    bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                //                    bad.currency_name = cur_name;
                //                    objcurval = objcurval + bad.currency_value;

                //                }
                //            }
                //        }
                //        itm.currencyvalue = objcurval;

                //    }


                //}

                //-----------------------------------------------------------------------

                //FootballThemeLeaderBoardHeader header = new FootballThemeLeaderBoardHeader();
                //--------Change on 11-11-19---------------------------------------------

                //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                //    int splmetid = db.Database.SqlQuery<int>("select id_metric from tbl_university_special_point_grid where id_game={0}", GameId).FirstOrDefault();
                //    if (Leader.userleague == null) { Leader.userleague = db.Database.SqlQuery<string>("select league_name from tbl_theme_leagues where id_league={0}", leag[0].id_league).FirstOrDefault(); }
                //}
                //if (Leader.UserProfileImage == "null")
                //{
                //    Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileDefaultBase"];
                //}
                //-----------------------------------------------------------------------

                ViewData["Leader"] = Leader;

            }
            catch (Exception e)
            {

                throw e;
            }

            return View();
        }
        public void currency_value_generator()
        {

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                int id_currency = db.Database.SqlQuery<int>("select id_currency from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                string cur_name = db.Database.SqlQuery<string>("select currency_value from tbl_currency_points where id_theme={0}", 9).FirstOrDefault();
                int GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                List<tbl_badge_master> instbadgemaster = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();

                //List<tbl_user> Userlist= db.Database.SqlQuery<tbl_user>("select * from tbl_user where STATUS={0}", "A").ToList();
                string sql01 = "SELECT a.id_user, (SELECT CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END FROM tbl_user_game_score_log WHERE a.id_user = id_user AND id_game = " + GameId + ") metric_score, (SELECT CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END FROM tbl_user_game_special_metric_score_log WHERE a.id_user = id_user AND id_game =  " + GameId + ") special_metric_score FROM tbl_user a WHERE a.ID_ORGANIZATION =  " + 130 + " AND a.status = 'A' and  a.id_user!=2503 ";

                List<LeaderBoardUserList> UserList = db.Database.SqlQuery<LeaderBoardUserList>(sql01).ToList();

                foreach (var itm in UserList)
                {
                    List<tbl_user_badge_log> user_bad_log = new List<tbl_user_badge_log>();
                    user_bad_log = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log where id_user={0} and id_game={1} ", itm.id_user, GameId).ToList();

                    List<tbl_badge_master> userbadge = new List<tbl_badge_master>();
                    List<UserBadge> Badge = new UniversityScoringlogic().getBadgeList(itm.id_user, GameId);



                    foreach (var bad in instbadgemaster)
                    {
                        bad.WonFlag = 0;
                        foreach (var ob in Badge)
                        {

                            if (ob.id_badge == bad.id_badge)
                            {
                                bad.WonFlag = 1;
                                bad.eligiblescore = Convert.ToInt32(itm.metric_score / ob.eligible_score);
                            }


                        }

                        userbadge.Add(bad);
                    }

                    int objcurval = 0;
                    foreach (var bad in userbadge)
                    {
                        foreach (var ob in user_bad_log)
                        {
                            if (ob.id_badge == bad.id_badge)
                            {
                                bad.currency_value = db.Database.SqlQuery<int>("select currency_value from tbl_currency_data where id_badge={0} and id_currency={1}", bad.id_badge, id_currency).FirstOrDefault();
                                bad.currency_name = cur_name;
                                objcurval = objcurval + bad.currency_value;

                            }
                        }
                    }
                    int currencyvalue = objcurval;
                    db.Database.ExecuteSqlCommand("Insert into tbl_user_currency_log (id_user,id_game,currency_value,status,updated_date_time) values({0},{1},{2},{3},{4})", itm.id_user, GameId, currencyvalue, "A", DateTime.Now);

                }


            }


        }

        public void activity_log()
        {

        }

        public void UnitAverageScoreReport(int id_org_game, string UserFunction, int OID, int UID)
        {
            OrgGameLeaderBoardResponse Result = new OrgGameLeaderBoardResponse();
            List<GameUserLog> loginst = new List<GameUserLog>();

            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            //level_reponseResult level = new level_reponseResult();
            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int tot_content_cnt = db.Database.SqlQuery<int>("SELECT count(id_game_content) FROM tbl_org_game_content inner join tbl_org_game_level_mapping on tbl_org_game_content.id_level=tbl_org_game_level_mapping.id_level where tbl_org_game_level_mapping.id_org_game={0}", id_org_game).FirstOrDefault();

                    tbl_org_game_master master_game = new tbl_org_game_master();
                    master_game = db.Database.SqlQuery<tbl_org_game_master>("select * from tbl_org_game_master where id_org_game={0} and  status='A'", id_org_game).FirstOrDefault();




                    //-----------------------------Central Units------------------------------------


                    if (UserFunction == "CENTRAL")
                    {
                        List<tbl_org_game_unit_master> unitmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where id_org={0} and status={1} and unit_type={2} ", OID, "A", 1).ToList();

                        List<UnitRankLog> overalcen = new List<UnitRankLog>();
                        int total_user_count = 0;
                        foreach (var itm in unitmas)
                        {
                            UnitRankLog ins = new UnitRankLog();
                            GameUserLog ob = new GameUserLog();
                            //ob.total_score_gained = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 1 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3} ", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.total_score_detected = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 2 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3}", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                            ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 1 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL", 1).FirstOrDefault();
                            ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 0 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL", 1).FirstOrDefault();
                            ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                            if (ob.final_assessmnet_total_count > 0)
                            {
                                ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                            }
                            total_user_count = db.Database.SqlQuery<int>("SELECT COUNT( tbl_user.ID_USER) AS usercnt FROM tbl_user JOIN tbl_profile ON tbl_user.ID_USER = tbl_profile.ID_USER WHERE tbl_user.ID_ORGANIZATION = {0} AND tbl_user.user_function = {1} AND tbl_profile.id_org_game_unit = {2} and tbl_user.STATUS='A'", OID, "CENTRAL", itm.id_org_game_unit).FirstOrDefault();
                            //if (ob.assessment_score > 0 && total_user_count > 0)
                            //{
                            //    ins.AverageScore = ob.assessment_score / total_user_count;

                            //}
                            //else
                            //{
                            //    ins.AverageScore = 0;
                            //}
                            ins.AverageScore = ob.assessment_score;
                            ins.AverageScore = Math.Round(ins.AverageScore, 2);

                            int total_completed_count = db.Database.SqlQuery<int>("SELECT COUNT(tbl_org_game_user_log.id_log) AS logcnt  FROM tbl_org_game_user_log INNER JOIN tbl_user ON tbl_org_game_user_log.id_user = tbl_user.ID_USER INNER JOIN tbl_profile ON tbl_profile.ID_USER = tbl_org_game_user_log.id_user where tbl_org_game_user_log.is_completed=1 and tbl_user.STATUS='A' and tbl_profile.id_org_game_unit={0} and tbl_org_game_user_log.id_org_game={1} and tbl_org_game_user_log.is_completed={2} and tbl_org_game_user_log.id_level={3} and  tbl_user.user_function={4}", itm.id_org_game_unit, id_org_game, 1, 5, "CENTRAL").FirstOrDefault();
                            // int tot_content_cnt = db.Database.SqlQuery<int>("SELECT count(id_game_content) FROM tbl_org_game_content inner join tbl_org_game_level_mapping on tbl_org_game_content.id_level=tbl_org_game_level_mapping.id_level where tbl_org_game_level_mapping.id_org_game={0}", id_org_game).FirstOrDefault();
                            tot_content_cnt = tot_content_cnt * total_user_count;
                            if (total_user_count > 0)
                            {
                                ins.CompletionPercentage = (Convert.ToDouble(total_completed_count) / Convert.ToDouble(total_user_count)) * 100;


                            }
                            else
                            {
                                ins.CompletionPercentage = 0;
                            }
                            ins.CompletionPercentage = Math.Round(ins.CompletionPercentage, 2);

                            //ins.RankPercentage = (ins.CompletionPercentage + ins.AverageScore) / 2;

                            ins.id_org_game = id_org_game;
                            ins.Unit = itm.unit;
                            ins.IdUnit = itm.id_org_game_unit;
                            overalcen.Add(ins);

                        }
                        overalcen = overalcen.OrderByDescending(x => x.AverageScore).ToList();
                        int i = 1;
                        foreach (var itm in overalcen)
                        {
                            itm.Rank = i;
                            i++;


                        }
                        Result.CENTRALUnits = overalcen;


                    }
                    //-------------------------------------------------------------
                    //-----------------------------Non Central Units------------------------------------
                    if (UserFunction != "CENTRAL")
                    {
                        List<tbl_org_game_unit_master> unitmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where id_org={0} and status={1} and unit_type={2} ", OID, "A", 2).ToList();

                        List<UnitRankLog> overalNoncen = new List<UnitRankLog>();
                        int total_user_count = 0;
                        foreach (var itm in unitmas)
                        {
                            UnitRankLog ins = new UnitRankLog();
                            GameUserLog ob = new GameUserLog();
                            //ob.total_score_gained = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 1 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3} ", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.total_score_detected = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 2 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3}", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                            ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 1 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function!={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL", 1).FirstOrDefault();
                            ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 0 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function!={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL", 1).FirstOrDefault();
                            ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                            if (ob.final_assessmnet_total_count > 0)
                            {
                                ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                            }
                            total_user_count = db.Database.SqlQuery<int>("SELECT COUNT( tbl_user.ID_USER) AS usercnt FROM tbl_user JOIN tbl_profile ON tbl_user.ID_USER = tbl_profile.ID_USER WHERE tbl_user.ID_ORGANIZATION = {0} AND tbl_user.user_function != {1} AND tbl_profile.id_org_game_unit = {2} and tbl_user.STATUS='A'", OID, "CENTRAL", itm.id_org_game_unit).FirstOrDefault();
                            //if (ob.assessment_score > 0 && total_user_count > 0)
                            //{
                            //    ins.AverageScore = ob.assessment_score / total_user_count;

                            //}
                            //else
                            //{
                            //    ins.AverageScore = 0;
                            //}
                            ins.AverageScore = ob.assessment_score;
                            ins.AverageScore = Math.Round(ins.AverageScore, 2);

                            int total_completed_count = db.Database.SqlQuery<int>("SELECT COUNT(tbl_org_game_user_log.id_log) AS logcnt  FROM tbl_org_game_user_log INNER JOIN tbl_user ON tbl_org_game_user_log.id_user = tbl_user.ID_USER INNER JOIN tbl_profile ON tbl_profile.ID_USER = tbl_org_game_user_log.id_user where tbl_org_game_user_log.is_completed=1 and tbl_user.STATUS='A' and tbl_profile.id_org_game_unit={0} and tbl_org_game_user_log.id_org_game={1} and tbl_org_game_user_log.is_completed={2} and tbl_org_game_user_log.id_level={3} and  tbl_user.user_function!={4}", itm.id_org_game_unit, id_org_game, 1, 5, "CENTRAL").FirstOrDefault();
                            // int tot_content_cnt = db.Database.SqlQuery<int>("SELECT count(id_game_content) FROM tbl_org_game_content inner join tbl_org_game_level_mapping on tbl_org_game_content.id_level=tbl_org_game_level_mapping.id_level where tbl_org_game_level_mapping.id_org_game={0}", id_org_game).FirstOrDefault();
                            tot_content_cnt = tot_content_cnt * total_user_count;
                            if (total_user_count > 0)
                            {
                                ins.CompletionPercentage = (Convert.ToDouble(total_completed_count) / Convert.ToDouble(total_user_count)) * 100;


                            }
                            else
                            {
                                ins.CompletionPercentage = 0;
                            }
                            ins.CompletionPercentage = Math.Round(ins.CompletionPercentage, 2);

                            //ins.RankPercentage = (ins.CompletionPercentage + ins.AverageScore) / 2;

                            ins.id_org_game = id_org_game;
                            ins.Unit = itm.unit;
                            ins.IdUnit = itm.id_org_game_unit;
                            overalNoncen.Add(ins);

                        }
                        overalNoncen = overalNoncen.OrderByDescending(x => x.AverageScore).ToList();
                        int i = 1;
                        foreach (var itm in overalNoncen)
                        {
                            itm.Rank = i;
                            i++;


                        }
                        Result.NONCENTRALUnits = overalNoncen;

                    }

                    //-------------------------------------------------------------




                }
                Result.STATUS = "SUCCESS";
                Result.MESSAGE = "Data retrived successfully.";

            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";
                Result.MESSAGE = "Something went wrong.";
                //throw e;
            }

        }

        public void earliestFinishReport()
        {
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<UnitCompletionReport> unitscomp = new List<UnitCompletionReport>();
                List<tbl_org_game_unit_master> unitsmas = new List<tbl_org_game_unit_master>();
                unitsmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where status='A' and id_org=132 and unit_type={0}", 1).ToList();
                foreach (var itm in unitsmas)
                {
                    List<tbl_org_game_user_log> user_log = db.Database.SqlQuery<tbl_org_game_user_log>("SELECT * FROM tbl_org_game_user_log AS a INNER JOIN tbl_user AS b ON a.id_user = b.ID_USER INNER JOIN tbl_profile AS c ON b.id_user = c.ID_USER WHERE b.STATUS = 'A' AND c.id_org_game_unit = {0} AND a.id_level = 5 AND a.is_completed = 1 AND b.ID_ORGANIZATION = 132", itm.id_org_game_unit).ToList();
                    List<tbl_user> userlist = db.Database.SqlQuery<tbl_user>("select * from tbl_user as a inner join tbl_profile as b on a.ID_USER=b.ID_USER where a.STATUS='A' and b.id_org_game_unit={0}", itm.id_org_game_unit).ToList();
                    if (user_log.Count == userlist.Count)
                    {
                        UnitCompletionReport ins = new UnitCompletionReport();
                        ins.UnitName = itm.unit;
                        ins.LastCompletedDate = user_log.Select(x => x.updated_date_time).Max();

                        unitscomp.Add(ins);

                    }
                }

            }
        }

        public void AverageTimeTakenunit()
        {
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<UnitCompletionAverageReport> unitscomp = new List<UnitCompletionAverageReport>();
                List<tbl_org_game_unit_master> unitsmas = new List<tbl_org_game_unit_master>();
                unitsmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where status='A' and id_org=132 and unit_type={0}", 2).ToList();
                foreach (var itm in unitsmas)
                {
                    List<tbl_org_game_user_log> user_log = db.Database.SqlQuery<tbl_org_game_user_log>("SELECT * FROM tbl_org_game_user_log AS a INNER JOIN tbl_user AS b ON a.id_user = b.ID_USER INNER JOIN tbl_profile AS c ON b.id_user = c.ID_USER WHERE b.STATUS = 'A' AND c.id_org_game_unit = {0} AND a.id_level = 5 AND a.is_completed = 1 AND b.ID_ORGANIZATION = 132", itm.id_org_game_unit).ToList();
                    List<tbl_user> userlist = db.Database.SqlQuery<tbl_user>("select * from tbl_user as a inner join tbl_profile as b on a.ID_USER=b.ID_USER where a.STATUS='A' and b.id_org_game_unit={0}", itm.id_org_game_unit).ToList();

                    if (user_log.Count == userlist.Count)
                    {
                        UnitCompletionAverageReport ins = new UnitCompletionAverageReport();

                        ins.TotalTime = TimeSpan.Parse("00:00");

                        foreach (var lgitm in user_log)
                        {
                            TimeSpan intime = TimeSpan.Parse("00:" + lgitm.timetaken_to_complete);
                            ins.TotalTime = intime + ins.TotalTime;

                        }
                        ins.averageSecs = Convert.ToInt32(ins.TotalTime.TotalSeconds) / userlist.Count;
                        ins.UnitName = itm.unit;


                        unitscomp.Add(ins);

                    }
                }

            }


        }

        public void ScoreWiseUserRank()
        {
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<UnituserScoreWiseData> Result = new List<UnituserScoreWiseData>();
                List<tbl_org_game_unit_master> unitsmas = new List<tbl_org_game_unit_master>();
                unitsmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where status='A' and id_org=132 and unit_type={0}", 2).ToList();
                foreach (var itm in unitsmas)
                {
                    List<tbl_org_game_user_log> user_log = db.Database.SqlQuery<tbl_org_game_user_log>("SELECT * FROM tbl_org_game_user_log AS a INNER JOIN tbl_user AS b ON a.id_user = b.ID_USER INNER JOIN tbl_profile AS c ON b.id_user = c.ID_USER WHERE b.STATUS = 'A' AND c.id_org_game_unit = {0} AND a.id_level = 5 AND a.attempt_no = 1 AND b.ID_ORGANIZATION = 132", itm.id_org_game_unit).ToList();
                    List<tbl_user> userlist = db.Database.SqlQuery<tbl_user>("select * from tbl_user as a inner join tbl_profile as b on a.ID_USER=b.ID_USER where a.STATUS='A' and b.id_org_game_unit={0}", itm.id_org_game_unit).ToList();


                    UnituserScoreWiseData ins = new UnituserScoreWiseData();

                    ins.UnitName = itm.unit;
                    List<OrguserScoreData> UserListobj = new List<OrguserScoreData>();
                    foreach (var lgitm in user_log)
                    {
                        OrguserScoreData ob = new OrguserScoreData();
                        ob.TimeTakenToComplete = TimeSpan.Parse("00:" + lgitm.timetaken_to_complete);
                        ob.Name = db.Database.SqlQuery<string>("select concat(a.FIRSTNAME,' ',a.LASTNAME) as name from tbl_profile as a where a.ID_USER={0}", lgitm.id_user).FirstOrDefault();
                        int rightcount = db.Database.SqlQuery<int>("select count(id_log) from tbl_org_game_user_assessment_log as a where a.id_user={0} and a.is_correct=1 and a.attempt_no=1",lgitm.id_user).FirstOrDefault();
                        int wrongcount = db.Database.SqlQuery<int>("select count(id_log) from tbl_org_game_user_assessment_log as a where a.id_user={0} and a.is_correct=0 and a.attempt_no=1", lgitm.id_user).FirstOrDefault();
                        int totalcount = rightcount + wrongcount;
                        ob.AssessmnetScore =Convert.ToDouble(( Convert.ToDouble(rightcount) / Convert.ToDouble(totalcount))*100);

                        UserListobj.Add(ob);

                    }

                    ins.UserList = UserListobj;
                    Result.Add(ins);


                }

            }


        }
        public void TimeWiseUserRank()
        {
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<UnituserScoreWiseData> Result = new List<UnituserScoreWiseData>();
                List<tbl_org_game_unit_master> unitsmas = new List<tbl_org_game_unit_master>();
                unitsmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where status='A' and id_org=132 and unit_type={0}", 2).ToList();
                foreach (var itm in unitsmas)
                {
                    List<tbl_org_game_user_log> user_log = db.Database.SqlQuery<tbl_org_game_user_log>("SELECT * FROM tbl_org_game_user_log AS a INNER JOIN tbl_user AS b ON a.id_user = b.ID_USER INNER JOIN tbl_profile AS c ON b.id_user = c.ID_USER WHERE b.STATUS = 'A' AND c.id_org_game_unit = {0} AND a.id_level = 5 AND a.is_completed = 1 AND b.ID_ORGANIZATION = 132", itm.id_org_game_unit).ToList();
                    List<tbl_user> userlist = db.Database.SqlQuery<tbl_user>("select * from tbl_user as a inner join tbl_profile as b on a.ID_USER=b.ID_USER where a.STATUS='A' and b.id_org_game_unit={0}", itm.id_org_game_unit).ToList();


                    UnituserScoreWiseData ins = new UnituserScoreWiseData();

                    ins.UnitName = itm.unit;
                    List<OrguserScoreData> UserListobj = new List<OrguserScoreData>();
                    foreach (var lgitm in user_log)
                    {
                        OrguserScoreData ob = new OrguserScoreData();
                        ob.TimeTakenToComplete = TimeSpan.Parse("00:" + lgitm.timetaken_to_complete);
                        ob.Name = db.Database.SqlQuery<string>("select concat(a.FIRSTNAME,' ',a.LASTNAME) as name from tbl_profile as a where a.ID_USER={0}", lgitm.id_user).FirstOrDefault();
                        int rightcount = db.Database.SqlQuery<int>("select count(id_log) from tbl_org_game_user_assessment_log as a where a.id_user={0} and a.is_correct=1 and a.attempt_no=1", lgitm.id_user).FirstOrDefault();
                        int wrongcount = db.Database.SqlQuery<int>("select count(id_log) from tbl_org_game_user_assessment_log as a where a.id_user={0} and a.is_correct=0 and a.attempt_no=1", lgitm.id_user).FirstOrDefault();
                        int totalcount = rightcount + wrongcount;
                        ob.AssessmnetScore = Convert.ToDouble((Convert.ToDouble(rightcount) / Convert.ToDouble(totalcount)) * 100);

                        UserListobj.Add(ob);

                    }

                    ins.UserList = UserListobj;
                    Result.Add(ins);


                }

            }


        }



    }

}