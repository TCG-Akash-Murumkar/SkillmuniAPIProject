using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace m2ostnextservice.Models
{
    public class UniversityScoringlogic
    {
        public List<AssessmentScoreResponse> ScoreCal(int id_academy, int id_brief, int id_user, int right_count)
        {

            List<AssessmentScoreResponse> res = new List<AssessmentScoreResponse>();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_game_academic_mapping game_map = db.Database.SqlQuery<tbl_game_academic_mapping>("Select * from tbl_game_academic_mapping where id_academic_tile={0} and status={1}", id_academy, "A").FirstOrDefault();
                    string asssinment_flag = db.Database.SqlQuery<string>("Select assignment_flag from tbl_game_master where id_game={0} ", game_map.id_game).FirstOrDefault();
                    if (asssinment_flag == "A")
                    {
                        res = scoringlogic(game_map, id_brief, right_count);

                    }
                    else
                    {
                        int iduse = db.Database.SqlQuery<int>("Select id_user from tbl_user_game_assignment where id_game={0} and id_user={1}", game_map.id_game, id_user).FirstOrDefault();
                        if (iduse > 0) { res = scoringlogic(game_map, id_brief, right_count); }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return res;
        }
        public List<AssessmentScoreResponse> ScoreSplCal(int id_academy, int id_brief, int id_user, int right_count)
        {

            List<AssessmentScoreResponse> res = new List<AssessmentScoreResponse>();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_game_academic_mapping game_map = db.Database.SqlQuery<tbl_game_academic_mapping>("Select * from tbl_game_academic_mapping where id_academic_tile={0} and status={1}", id_academy, "A").FirstOrDefault();
                    string asssinment_flag = db.Database.SqlQuery<string>("Select assignment_flag from tbl_game_master where id_game={0} ", game_map.id_game).FirstOrDefault();
                    if (asssinment_flag == "A")
                    {
                        res = SPLscoringlogic(game_map, id_brief, right_count);

                    }
                    else
                    {
                        int iduse = db.Database.SqlQuery<int>("Select id_user from tbl_user_game_assignment where id_game={0} and id_user={1}", game_map.id_game, id_user).FirstOrDefault();
                        if (iduse > 0) { res = SPLscoringlogic(game_map, id_brief, right_count); }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return res;
        }


        public List<AssessmentScoreResponse> scoringlogic(tbl_game_academic_mapping map, int id_brief, int rightcount)
        {
            List<AssessmentScoreResponse> ass_score = new List<AssessmentScoreResponse>();

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    List<int> metricids = db.Database.SqlQuery<int>("SELECT distinct id_metric FROM tbl_university_kpi_grid where id_game={0}", map.id_game).ToList();
                    //string asssinment_flag = db.Database.SqlQuery<string>("Select assignment_flag from tbl_game_master where id_game={0} ", game_map.id_game).FirstOrDefault();
                    foreach (var itm in metricids)
                    {
                        AssessmentScoreResponse obj = new AssessmentScoreResponse();
                        obj.id_brief = id_brief;
                        obj.id_game = map.id_game;
                        obj.id_metric = itm;
                        obj.metric_name = db.Database.SqlQuery<string>("SELECT  metric_value FROM tbl_theme_metric where id_metric={0} ", itm).FirstOrDefault();

                        int idkpimaster = db.Database.SqlQuery<int>("SELECT distinct id_kpi_master FROM tbl_university_kpi_grid where id_game={0} and id_metric={1}", map.id_game, itm).FirstOrDefault();
                        int kpitype = db.Database.SqlQuery<int>("SELECT kpi_type FROM tbl_university_kpi_master where id_kpi_master={0}", idkpimaster).FirstOrDefault();
                        if (kpitype == 1)
                        {
                            tbl_university_kpi_grid kpi = db.Database.SqlQuery<tbl_university_kpi_grid>("SELECT* FROM tbl_university_kpi_grid where id_game = {0} and id_kpi_master = {1}", map.id_game, idkpimaster).FirstOrDefault();
                            double kpival = (kpi.start_range * rightcount);
                            obj.metric_score = kpival;
                        }
                        else if (kpitype == 2)
                        {
                            List<tbl_university_kpi_grid> kpi = db.Database.SqlQuery<tbl_university_kpi_grid>("SELECT* FROM tbl_university_kpi_grid where id_game = {0} and id_kpi_master = {1}", map.id_game, idkpimaster).ToList();
                            foreach (var it in kpi)
                            {
                                if ((rightcount >= it.start_range) && (rightcount <= it.end_range))
                                {
                                    double kpival = (it.kpi_value * rightcount);
                                    obj.metric_score = kpival;
                                }
                            }


                        }
                        ass_score.Add(obj);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return ass_score;
        }

        public List<AssessmentScoreResponse> SPLscoringlogic(tbl_game_academic_mapping map, int id_brief, int rightcount)
        {
            List<AssessmentScoreResponse> ass_score = new List<AssessmentScoreResponse>();

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    List<int> metricids = db.Database.SqlQuery<int>("SELECT distinct id_metric FROM tbl_university_special_point_grid where id_game={0}", map.id_game).ToList();
                    //string asssinment_flag = db.Database.SqlQuery<string>("Select assignment_flag from tbl_game_master where id_game={0} ", game_map.id_game).FirstOrDefault();
                    foreach (var itm in metricids)
                    {
                        AssessmentScoreResponse obj = new AssessmentScoreResponse();
                        obj.id_brief = id_brief;
                        obj.id_game = map.id_game;
                        obj.id_metric = itm;
                        obj.metric_name = db.Database.SqlQuery<string>("SELECT  name FROM tbl_special_metric_master where id_special_metric={0} ", itm).FirstOrDefault();

                        int idkpimaster = db.Database.SqlQuery<int>("SELECT distinct id_special_points FROM tbl_university_special_point_grid where id_game={0} and id_metric={1}", map.id_game, itm).FirstOrDefault();
                        int kpitype = db.Database.SqlQuery<int>("SELECT special_value_type FROM tbl_university_special_points_master where id_special_points={0}", idkpimaster).FirstOrDefault();

                        //-----Change From Here------22/01/19
                        if (kpitype == 1)
                        {
                            tbl_university_special_point_grid kpi = db.Database.SqlQuery<tbl_university_special_point_grid>("SELECT* FROM tbl_university_special_point_grid where id_game = {0} and id_special_points = {1}", map.id_game, idkpimaster).FirstOrDefault();
                            double kpival = (kpi.start_range * rightcount);
                            obj.metric_score = kpival;
                        }
                        else if (kpitype == 2)
                        {
                            List<tbl_university_special_point_grid> kpi = db.Database.SqlQuery<tbl_university_special_point_grid>("SELECT* FROM tbl_university_special_point_grid where id_game = {0} and id_special_points = {1}", map.id_game, idkpimaster).ToList();
                            foreach (var it in kpi)
                            {
                                if ((rightcount >= it.start_range) && (rightcount <= it.end_range))
                                {
                                    double kpival = (it.special_value * rightcount);
                                    obj.metric_score = kpival;
                                }
                            }


                        }
                        //--------------------------------------
                        ass_score.Add(obj);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return ass_score;
        }

        public List<UserBadge> getBadgeList(int id_user, int id_game)
        {
            List<UserBadge> badge = new List<UserBadge>();
            List<tbl_user_game_score_log> scorelog = new List<tbl_user_game_score_log>();

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    double? score = 0;
                    string ussql01 = "select  CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END score from tbl_user_game_score_log where id_game=" + id_game + " and id_user= " + id_user + " ";
                    score = db.Database.SqlQuery<double>(ussql01).FirstOrDefault();

                    //scorelog = db.Database.SqlQuery<tbl_user_game_score_log>("select * from tbl_user_game_score_log where id_game={0} and id_user={1}", id_game, id_user).ToList();
                    //double score = 0;
                    //foreach (var itm in scorelog)
                    //{
                    //    score += itm.score;
                    //}

                    int themeid = db.Database.SqlQuery<int>("select id_theme from tbl_game_master where id_game={0}", id_game).FirstOrDefault();
                    List<tbl_badge_data> badgedata = db.Database.SqlQuery<tbl_badge_data>("select * from tbl_badge_data where id_game={0}", id_game).ToList();
                    foreach (var itm in badgedata)
                    {
                        UserBadge obj = new UserBadge();
                        int i = 1;
                        if (score >= itm.required_score)
                        {
                            if (i <= 3)
                            {
                                tbl_badge_master mas = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_badge={0}", itm.id_badge).FirstOrDefault();
                                obj.id_badge = itm.id_badge;
                                obj.badge_name = mas.badge_name;
                                obj.eligible_score = itm.required_score;
                                obj.badge_image = ConfigurationManager.AppSettings["BadgeBase"] + mas.badge_logo;
                                badge.Add(obj);
                            }

                            i++;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return badge;
        }


        public List<LeaderBoardUserList> getUserListLeaderBoard(int id_game, int oid,int id_league)
        {
            List<LeaderBoardUserList> UserList = new List<LeaderBoardUserList>();


            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {



                    string sql01 = "SELECT a.id_user, (SELECT CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END FROM tbl_user_game_score_log WHERE a.id_user = id_user AND id_game = " + id_game + ") metric_score, (SELECT CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END FROM tbl_user_game_special_metric_score_log WHERE a.id_user = id_user AND id_game =  " + id_game + ") special_metric_score FROM tbl_user a  inner join tbl_user_league_log b on a.id_user=b.id_user WHERE a.ID_ORGANIZATION =  " + oid + " AND a.status = 'A' and  a.id_user!=2503   AND b.id_league= "+id_league;

                    UserList = db.Database.SqlQuery<LeaderBoardUserList>(sql01).ToList();










                    /* updated by pavan*/

                    //List<tbl_user> user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_ORGANIZATION={0} and STATUS={1} and  ID_USER!=2503", oid, "A").ToList();
                    //foreach (var itm in user)
                    //{
                    //    //tbl_profile prof= db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0} ",itm.ID_USER).FirstOrDefault();
                    //    LeaderBoardUserList obj1 = new LeaderBoardUserList();
                    //    obj.id_user = itm.ID_USER;

                    //    double score = 0;
                    //    string ussql01 = "select  CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END score from tbl_user_game_score_log where id_game=" + id_game + " and id_user= " + itm.ID_USER + " ";
                    //    score = db.Database.SqlQuery<double>(ussql01).FirstOrDefault();

                    //    //List<tbl_user_game_score_log> scorelog = new List<tbl_user_game_score_log>();
                    //    //scorelog = db.Database.SqlQuery<tbl_user_game_score_log>("select * from tbl_user_game_score_log where id_game={0} and id_user={1} ", id_game, itm.ID_USER).ToList();

                    //    //foreach (var item in scorelog)
                    //    //{
                    //    //    score += item.score;
                    //    //}
                    //    obj.metric_score = score;
                    //    double splscore = 0;
                    //    string ussql02 = "select CASE WHEN (SUM(score) > 0) THEN SUM(score) ELSE 0 END score from tbl_user_game_special_metric_score_log where  id_game=" + id_game + " and id_user= " + itm.ID_USER + " ";
                    //    splscore = db.Database.SqlQuery<double>(ussql02).FirstOrDefault();
                    //    obj.special_metric_score = splscore;

                    //    //List<tbl_user_game_special_metric_score_log> splscorelog = new List<tbl_user_game_special_metric_score_log>();
                    //    //splscorelog = db.Database.SqlQuery<tbl_user_game_special_metric_score_log>("select * from tbl_user_game_special_metric_score_log where id_game={0} and id_user={1}", id_game, itm.ID_USER).ToList();

                    //    //foreach (var item in splscorelog)
                    //    //{
                    //    //    splscore += item.score;
                    //    //}

                    //    UserList.Add(obj);
                    //}
                    
                    /* updated by pavan*/

                }
            }
            catch (Exception e)
            {
                
            }

            return UserList;
        }

        public string getApiPost(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                response = client.UploadValues(uri, pairs);
            }
            string result = System.Text.Encoding.UTF8.GetString(response);
            return result;
        }

        public string getApiResponseString(string api)
        {
            byte[] response = null;

            var wc123 = new WebClient();
            using (var wc = new WebClient())
            {
               
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                response = wc.DownloadData(api);
            }
            string result = System.Text.Encoding.UTF8.GetString(response);
            return result;
        }


        public string SendNotification(string deviceRegIds, string message, string type, string image, string eligiblescore = "0", int currency = 0, string tag = "Badge Name", string title = "")
        {
            string responseLine = "";
            try
            {
                string GoogleAppID = "AAAAGrnsAbc:APA91bH3oHyM5R0KrFxEexkW-DmnOr5HD1oyKmsmP_nlUjNEdlmAUu1ZF7YuD3y8NGmMx_760dgynH1hYw603TN7CAnpgD4yp59dUFOMi198H42RweTvKHYEwfVzdusHMMZuKnRvjXUW";
                var SENDER_ID = "114788401591";


                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentType = "application/json";
                NotificationData dat = new NotificationData();
                dat.Image = image;
                var payload = new
                {
                    to = deviceRegIds,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = message,
                        title = title,
                        badge = 1,
                        icon = image,
                        color = eligiblescore,
                        sound = currency,
                        tag = tag


                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return responseLine;
        }




    }
}