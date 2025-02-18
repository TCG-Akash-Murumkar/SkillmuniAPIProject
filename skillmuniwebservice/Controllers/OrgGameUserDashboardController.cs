using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class OrgGameUserDashboardController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_org_game,string UserFunction,int id_org_game_unit)
        {
            List<LevelUserLogResponse> Result = new List<LevelUserLogResponse>();

            OrgGameUserDashboardResult res = new OrgGameUserDashboardResult();

            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    res.total_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=1", UID, id_org_game).FirstOrDefault();
                    res.detucted_score = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=2", UID, id_org_game).FirstOrDefault();
                    res.current_score = res.total_score - res.detucted_score;
                    tbl_org_game_master master_game = new tbl_org_game_master();
                    master_game = db.Database.SqlQuery<tbl_org_game_master>("select * from tbl_org_game_master where id_org_game={0} and  status='A'", id_org_game).FirstOrDefault();
                    List<tbl_org_game_level_mapping> levelmap = new List<tbl_org_game_level_mapping>();
                    levelmap = db.Database.SqlQuery<tbl_org_game_level_mapping>("select * from tbl_org_game_level_mapping where id_org_game={0} and status='A'", id_org_game).ToList();
                    foreach (var lvl in levelmap)
                    {


                        LevelUserLogResponse obj = new LevelUserLogResponse();


                        obj.content = db.Database.SqlQuery<tbl_org_game_content>("select * from tbl_org_game_content inner join tbl_org_game_level_mapping on tbl_org_game_content.id_level=tbl_org_game_level_mapping.id_level  where tbl_org_game_content.id_level={0} and tbl_org_game_level_mapping.id_org_game={1}", lvl.id_level, id_org_game).ToList();
                        obj.UID = UID;
                        obj.OID = OID;
                        obj.id_game = id_org_game;
                        obj.id_level = lvl.id_level;
                        int cmp_lvl_count = 0;
                        foreach (var itm in obj.content)
                        {
                            itm.user_log = db.Database.SqlQuery<tbl_org_game_user_log>("select * from tbl_org_game_user_log where id_game_content={0} and id_user={1} and id_org_game={2} and id_level={3}", itm.id_game_content, UID, id_org_game, lvl.id_level).ToList();
                            foreach (var ulg in itm.user_log)
                            {
                                if (ulg.is_completed == 1)
                                {
                                    cmp_lvl_count++;
                                }
                            }
                            if (itm.user_log != null)
                            {
                                itm.badge_log = db.Database.SqlQuery<tbl_org_game_badge_master>("select * from tbl_badge_master inner join tbl_org_game_content_badge_mapping on tbl_badge_master.id_badge=tbl_org_game_content_badge_mapping.id_badge where tbl_org_game_content_badge_mapping.id_content={0} and tbl_org_game_content_badge_mapping.id_game={1} and id_level={2}", itm.id_game_content, id_org_game, lvl.id_level).FirstOrDefault();
                                if (itm.badge_log != null)
                                {
                                    itm.badge_log.badge_count = db.Database.SqlQuery<int>(" select COUNT(id_log) AS total from tbl_org_game_badge_user_log where id_user = {0} and id_content = {1} and id_game = {2} and id_level={3}", UID, itm.id_game_content, id_org_game, lvl.id_level).FirstOrDefault();
                                    itm.badge_log.is_achieved = 0;

                                    if (itm.badge_log.badge_count > 0)
                                    {

                                        itm.badge_log.is_achieved = 1;

                                    }
                                }

                            }

                        }

                        obj.level_badge_log = db.Database.SqlQuery<tbl_org_game_badge_master>("select * from tbl_badge_master inner join tbl_org_game_badge_level_mapping on tbl_badge_master.id_badge=tbl_org_game_badge_level_mapping.id_badge where tbl_org_game_badge_level_mapping.id_level={0} and tbl_org_game_badge_level_mapping.id_org_game={1} ", lvl.id_level, id_org_game).FirstOrDefault();
                        if (obj.level_badge_log != null)
                        {
                            obj.level_badge_log.badge_count = db.Database.SqlQuery<int>(" select COUNT(id_log) AS total from tbl_org_game_badge_user_log where id_user = {0} and id_content = {1} and id_game = {2} and id_level={3}", UID, 0, id_org_game, lvl.id_level).FirstOrDefault();
                            obj.level_badge_log.is_achieved = 0;

                            if (obj.level_badge_log.badge_count > 0)
                            {

                                obj.level_badge_log.is_achieved = 1;

                            }

                        }
                        if (obj.content.Count == cmp_lvl_count)
                        {
                            obj.is_level_completed = 1;
                        }
                        
                        Result.Add(obj);
                    }
                    //----------------overall ranking --------------------------------------------
                    List<tbl_user> user = new List<tbl_user>();
                    //user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_ORGANIZATION={0} and STATUS={1} and user_function={2} ", OID, "A", UserFunction).ToList();
                    user = db.Database.SqlQuery<tbl_user>("select * from tbl_user as a inner join tbl_org_game_user_log as b on a.ID_USER=b.id_user where a.ID_ORGANIZATION={0} and a.STATUS='A' and a.user_function={1} and b.attempt_no=1 and b.id_level=5",OID,UserFunction).ToList();
                    List<GameUserLog> loginst = new List<GameUserLog>();

                    foreach (var itm in user)
                    {
                        GameUserLog ob = new GameUserLog();
                        //ob.total_score_gained = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=1 and tbl_org_game_user_log.id_level={2}", itm.ID_USER, id_org_game, 5).FirstOrDefault();
                        //ob.total_score_detected = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=2 and tbl_org_game_user_log.id_level={2}", itm.ID_USER, id_org_game, 5).FirstOrDefault();
                        ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("select COALESCE(SUM(is_correct),0) total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=1 ", itm.ID_USER, id_org_game).FirstOrDefault();
                        ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("select count( is_correct) as total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=0 ", itm.ID_USER, id_org_game).FirstOrDefault();
                        ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                        if (ob.final_assessmnet_total_count > 0)
                        {
                            ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                        }
                        ob.id_user = itm.ID_USER;
                        ob.id_org_game = master_game.id_org_game;
                        ob.game_title = master_game.title;
                        ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                        tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.ID_USER).FirstOrDefault();
                        if (prof != null)
                        {
                            ob.Name = prof.FIRSTNAME + " " + prof.LASTNAME;
                            ob.PROFILE_IMAGE = ConfigurationManager.AppSettings["profileimage_base"].ToString() + prof.PROFILE_IMAGE;

                        }


                        loginst.Add(ob);


                    }
                    // Result.Score = loginst;

                    loginst = loginst.OrderByDescending(x => x.assessment_score).ToList();
                    int i = 1;
                    foreach (var rn in loginst)
                    {
                        rn.rank = i;
                        i++;
                    }

                    GameUserLog SingleUser = loginst.Find(p => p.id_user == UID);
                    if (SingleUser != null)
                    {
                        res.OverAllRank = SingleUser.rank;
                        user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_ORGANIZATION={0} and STATUS={1} and user_function={2} ", OID, "A", UserFunction).ToList();
                        res.OverAllRankTotal = user.Count();

                    }
                    else
                    {
                        user = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_ORGANIZATION={0} and STATUS={1} and user_function={2} ", OID, "A", UserFunction).ToList();
                        res.OverAllRankTotal = user.Count();

                    }
                    

                    //----------------Unit ranking --------------------------------------------
                   
                    user = db.Database.SqlQuery<tbl_user>("select * from tbl_user inner join tbl_profile on tbl_user.ID_USER=tbl_profile.ID_USER inner join tbl_org_game_user_log  on tbl_user.ID_USER=tbl_org_game_user_log.id_user  where tbl_user.ID_ORGANIZATION={0} and tbl_user.STATUS={1} and tbl_user.user_function={2} and tbl_profile.id_org_game_unit={3} and  tbl_org_game_user_log.attempt_no=1 and tbl_org_game_user_log.id_level=5 ", OID, "A", UserFunction, id_org_game_unit).ToList();
                    List<GameUserLog> loginstunit = new List<GameUserLog>();

                    foreach (var itm in user)
                    {
                        GameUserLog ob = new GameUserLog();
                        //ob.total_score_gained = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=1 and tbl_org_game_user_log.id_level={2}", itm.ID_USER, id_org_game, 5).FirstOrDefault();
                        //ob.total_score_detected = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=2 and tbl_org_game_user_log.id_level={2}", itm.ID_USER, id_org_game, 5).FirstOrDefault();
                        ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("select COALESCE(SUM(is_correct),0) total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=1 ", itm.ID_USER, id_org_game).FirstOrDefault();
                        ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("select count( is_correct) as total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=0 ", itm.ID_USER, id_org_game).FirstOrDefault();
                        ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                        if (ob.final_assessmnet_total_count > 0)
                        {
                            ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                        }
                        ob.id_user = itm.ID_USER;
                        ob.id_org_game = master_game.id_org_game;
                        ob.game_title = master_game.title;
                        ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                        tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", itm.ID_USER).FirstOrDefault();
                        if (prof != null)
                        {
                            ob.Name = prof.FIRSTNAME + " " + prof.LASTNAME;
                            ob.PROFILE_IMAGE = ConfigurationManager.AppSettings["profileimage_base"].ToString() + prof.PROFILE_IMAGE;

                        }


                        loginstunit.Add(ob);


                    }
                    // Result.Score = loginst;

                    loginst = loginst.OrderByDescending(x => x.assessment_score).ToList();
                     i = 1;
                    foreach (var rn in loginstunit)
                    {
                        rn.rank = i;
                        i++;
                    }

                    GameUserLog SingleUseruni = loginstunit.Find(p => p.id_user == UID);
                    if (SingleUseruni != null)
                    {
                        res.UnitRank = SingleUser.rank;
                        user = db.Database.SqlQuery<tbl_user>("select * from tbl_user inner join tbl_profile on tbl_user.ID_USER=tbl_profile.ID_USER   where tbl_user.ID_ORGANIZATION={0} and tbl_user.STATUS={1} and tbl_user.user_function={2} and tbl_profile.id_org_game_unit={3} ", OID, "A", UserFunction, id_org_game_unit).ToList();
                        res.UnitRankTotal = user.Count();

                    }
                    else
                    {
                        user = db.Database.SqlQuery<tbl_user>("select * from tbl_user inner join tbl_profile on tbl_user.ID_USER=tbl_profile.ID_USER   where tbl_user.ID_ORGANIZATION={0} and tbl_user.STATUS={1} and tbl_user.user_function={2} and tbl_profile.id_org_game_unit={3} ", OID, "A", UserFunction, id_org_game_unit).ToList();
                        res.UnitRankTotal = user.Count();


                    }





                }



            }
            catch (Exception e)
            {
                throw e;
            }
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();

            res.LevelUserLog = Result;
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

    }
}
