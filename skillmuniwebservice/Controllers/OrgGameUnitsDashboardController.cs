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
    public class OrgGameUnitsDashboardController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_org_game, int id_org_game_unit, string UserFunction)
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
                        List<tbl_org_game_unit_master> unitmas = db.Database.SqlQuery<tbl_org_game_unit_master>("select * from tbl_org_game_unit_master where id_org={0} and status={1} and unit_type={2} ", OID, "A",1).ToList();

                        List<UnitRankLog> overalcen = new List<UnitRankLog>();
                        int total_user_count = 0;
                        foreach (var itm in unitmas)
                        {
                            UnitRankLog ins = new UnitRankLog();
                            GameUserLog ob = new GameUserLog();
                            //ob.total_score_gained = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 1 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3} ", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.total_score_detected = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(score), 0) total FROM tbl_org_game_user_log inner join tbl_profile on tbl_org_game_user_log.id_user=tbl_profile.ID_USER inner join tbl_user on tbl_user.ID_USER=tbl_org_game_user_log.id_user WHERE tbl_org_game_user_log.id_org_game ={0} AND tbl_org_game_user_log.score_type = 2 and tbl_profile.id_org_game_unit={1} and tbl_user.ID_ORGANIZATION={2} and tbl_org_game_user_log.id_level={3}", id_org_game, itm.id_org_game_unit, OID, 5).FirstOrDefault();
                            //ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                            ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 1 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL",1).FirstOrDefault();
                            ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 0 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL",1).FirstOrDefault();
                            ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                            if (ob.final_assessmnet_total_count > 0)
                            {
                                ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                            }
                            total_user_count = db.Database.SqlQuery<int>("SELECT COUNT( tbl_user.ID_USER) AS usercnt FROM tbl_user JOIN tbl_profile ON tbl_user.ID_USER = tbl_profile.ID_USER WHERE tbl_user.ID_ORGANIZATION = {0} AND tbl_user.user_function = {1} AND tbl_profile.id_org_game_unit = {2} and tbl_user.STATUS='A'",  OID, "CENTRAL",itm.id_org_game_unit).FirstOrDefault();
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

                            ins.RankPercentage = (ins.CompletionPercentage + ins.AverageScore) / 2;

                            ins.id_org_game = id_org_game;
                            ins.Unit = itm.unit;
                            ins.IdUnit = itm.id_org_game_unit;
                            overalcen.Add(ins);

                        }
                        overalcen = overalcen.OrderByDescending(x => x.RankPercentage).ToList();
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
                            ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 1 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function!={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL",1).FirstOrDefault();
                            ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("SELECT COUNT(is_correct) total FROM tbl_org_game_user_assessment_log INNER JOIN tbl_profile ON tbl_org_game_user_assessment_log.id_user = tbl_profile.ID_USER INNER JOIN tbl_user ON tbl_user.ID_USER = tbl_org_game_user_assessment_log.id_user WHERE tbl_org_game_user_assessment_log.id_org_game = {0} AND tbl_org_game_user_assessment_log.is_correct = 0 AND tbl_profile.id_org_game_unit = {1} AND tbl_user.ID_ORGANIZATION = {2} and tbl_user.user_function!={3} and tbl_org_game_user_assessment_log.attempt_no={4} ", id_org_game, itm.id_org_game_unit, OID, "CENTRAL",1).FirstOrDefault();
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

                            ins.RankPercentage = (ins.CompletionPercentage + ins.AverageScore) / 2;

                            ins.id_org_game = id_org_game;
                            ins.Unit = itm.unit;
                            ins.IdUnit = itm.id_org_game_unit;
                            overalNoncen.Add(ins);

                        }
                        overalNoncen = overalNoncen.OrderByDescending(x => x.RankPercentage).ToList();
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



            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

    }
}
