using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace m2ostnextservice.Controllers
{
    public class EvaluateBriefController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int OID, int UID, int BID, string BRF, string ASRQ,int AcademicTileId)
        {
            //  setRightAnswer();

            BRF = new Utility().mysqlTrim(BRF.ToString());
            ASRQ = new Utility().mysqlTrim(ASRQ.ToString());

            List<BriefDataCube> dataCube = new List<BriefDataCube>();
            List<BriefUserInput> responseCube = new List<BriefUserInput>();
            BriefReturnResponse returnResponse = new BriefReturnResponse();
            List<ComplexityResult> comList = new List<ComplexityResult>();
            List<tbl_brief_question_complexity> qclist = db.tbl_brief_question_complexity.Where(t => t.status == "A").ToList();
            string jsonresponse = null;
            int qCount = 0;
            int rightCount = 0;
            tbl_brief_master brief = db.tbl_brief_master.Where(t => t.id_brief_master == BID).FirstOrDefault();
            if (brief != null)
            {
                List<string> qList = ASRQ.Split(';').ToList();
                int qtnCount = qList.Count();
                if (qtnCount > 0)
                {
                    string sqlat = "SELECT count(*) subcount FROM tbl_brief_index where id_user=" + UID + " and id_brief_master=" + brief.id_brief_master + " ";
                    int attemptno = new BriefModel().getAttamptNo(sqlat);
                    attemptno++;

                    tbl_brief_index index = new tbl_brief_index();
                    index.id_brief_master = brief.id_brief_master;
                    index.id_user = UID;
                    index.status = "A";
                    index.updated_date_time = DateTime.Now;
                    db.tbl_brief_index.Add(index);
                    db.SaveChanges();

                    foreach (string item in qList)
                    {
                        string[] content = item.Split('|');//QID|ANS|val
                        var qitn = content[0];
                        if (qitn != null)
                        {
                            BriefDataCube temp = new BriefDataCube();
                            temp.QID = Convert.ToInt32(qitn);
                            temp.AID = Convert.ToInt32(content[1]);
                            temp.VAL = "NA";
                            dataCube.Add(temp);
                            tbl_brief_question question = db.tbl_brief_question.Where(t => t.id_brief_question == temp.QID).FirstOrDefault();
                            bool flag = false;
                            tbl_brief_audit row = new tbl_brief_audit();
                            row.attempt_no = attemptno;
                            row.id_brief_master = brief.id_brief_master;
                            row.id_brief_question = temp.QID;
                            row.id_brief_answer = temp.AID;
                            row.id_user = UID;
                            row.recorded_timestamp = System.DateTime.Now;
                            row.status = "A";
                            row.question_complexity = question.question_complexity;
                            row.updated_date_time = DateTime.Now;
                            row.value_sent = 0;
                            tbl_brief_answer ans = db.tbl_brief_answer.Where(t => t.id_brief_answer == temp.AID).FirstOrDefault();
                            if (ans != null)
                            {
                                if (ans.is_correct_answer == 1)
                                {
                                    flag = true;
                                    row.audit_result = 1;
                                }
                                else
                                {
                                    row.audit_result = 0;
                                }
                            }
                            else
                            {
                                row.audit_result = 0;
                            }
                            row.id_organization = OID;
                            db.tbl_brief_audit.Add(row);
                            db.SaveChanges();
                            if (flag)
                            {
                                if (attemptno == 1)
                                {
                                    tbl_brief_b2c_right_audit right = new tbl_brief_b2c_right_audit();
                                    right.data_index = 0;
                                    right.datetime_stamp = System.DateTime.Now;
                                    right.id_brief_question = temp.QID;
                                    right.id_organization = OID;
                                    right.question_complexity = question.question_complexity;
                                    right.value_count = 1;
                                    right.status = "A";
                                    right.updated_date_time = System.DateTime.Now;
                                    right.id_user = UID;
                                    db.tbl_brief_b2c_right_audit.Add(right);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    int srno = 1;
                    foreach (BriefDataCube item in dataCube)
                    {
                        BriefUserInput urow = new BriefUserInput();
                        tbl_brief_question qtn = db.tbl_brief_question.Where(t => t.id_brief_question == item.QID).FirstOrDefault();
                        if (qtn != null)
                        {
                            tbl_brief_answer ans = db.tbl_brief_answer.Where(t => t.id_brief_answer == item.AID).FirstOrDefault();
                            tbl_brief_answer wans = db.tbl_brief_answer.Where(t => t.id_brief_question == item.QID && t.is_correct_answer == 1).FirstOrDefault();

                            if (ans.is_correct_answer == 1)
                            {
                                rightCount++;
                                urow.is_right = 1;
                            }
                            else
                            {
                                urow.is_right = 0;
                            }
                            urow.Question = "Q. " + qtn.brief_question;
                            urow.Answer = "A. " + ans.brief_answer;
                            urow.id_answer = ans.id_brief_answer;
                            urow.id_question = ans.id_brief_question;
                            urow.WANS = wans.brief_answer;
                            urow.id_wans = wans.id_brief_answer;
                            tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == qtn.question_complexity).FirstOrDefault();
                            if (comp != null)
                            {
                                urow.question_complexity = Convert.ToInt32(comp.question_complexity);
                                urow.question_complexity_label = comp.question_complexity_label;
                            }

                            urow.srno = srno++;
                            responseCube.Add(urow);
                        }
                    }

                    foreach (tbl_brief_question_complexity row in qclist)
                    {
                        string qcsql = "SELECT CASE WHEN COUNT(*) > 0 THEN COUNT(*) ELSE 0 END totalcount, CASE WHEN SUM(audit_result) > 0 THEN SUM(audit_result) ELSE 0 END rightcount FROM tbl_brief_audit " +
                            " WHERE id_user = " + UID + " AND id_brief_master = " + brief.id_brief_master + " AND attempt_no = 1 AND id_brief_question IN (SELECT id_brief_question FROM tbl_brief_question WHERE question_complexity = " + row.question_complexity + ")";
                        ComplexityResult trow = new BriefModel().getComplexityResult(qcsql);
                        if (trow.TOTALCOUNT > 0)
                        {
                            trow.question_complexity = Convert.ToInt32(row.question_complexity);
                            trow.question_complexity_label = row.question_complexity_label;
                            double tresult = 0.0;
                            tresult = (trow.RIGHTCOUNT * 100) / (trow.TOTALCOUNT);
                            tresult = Math.Round(tresult, 2);
                            trow.RESULT = tresult;
                            comList.Add(trow);
                        }
                    }
                    returnResponse.complexity = comList;
                    returnResponse.briefReturn = responseCube;
                    returnResponse.returnStat = "You have answered " + rightCount + " out of " + qtnCount + " question right ";
                    returnResponse.rightCount = rightCount;
                    returnResponse.totalCount = qtnCount;
                    returnResponse.attemptno = attemptno;

                    double result = 0.0;
                    if (rightCount > 0 && qtnCount > 0)
                    {
                        result = (rightCount * 100) / (qtnCount);
                        result = Math.Round(result, 2);
                    }
                    returnResponse.percentage = result;
                    if (attemptno == 1)
                    {
                        tbl_brief_b2c_score_audit score = new tbl_brief_b2c_score_audit();
                        score.data_index = 0;
                        score.datetime_stamp = System.DateTime.Now;
                        score.id_brief_master = brief.id_brief_master;
                        score.id_organization = OID;
                        score.value_count = Convert.ToInt32(result);
                        score.status = "A";
                        score.updated_date_time = System.DateTime.Now;
                        score.id_user = UID;
                        db.tbl_brief_b2c_score_audit.Add(score);
                        db.SaveChanges();
                    }
                    tbl_brief_log log = new tbl_brief_log();
                    log.attempt_no = attemptno;
                    log.id_brief_master = brief.id_brief_master;
                    log.id_organization = OID;
                    log.id_user = UID;
                    log.brief_result = result;
                    log.status = "A";
                    log.updated_date_time = DateTime.Now;
                    jsonresponse = JsonConvert.SerializeObject(returnResponse);
                    log.json_response = jsonresponse;
                    db.tbl_brief_log.Add(log);
                    db.SaveChanges();
                }

                tbl_brief_read_status rstatus = db.tbl_brief_read_status.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (rstatus != null)
                {
                    if (rstatus.action_status == 0)
                    {
                        rstatus.read_status = 1;
                        rstatus.read_datetime = DateTime.Now;
                        rstatus.action_status = 1;
                        rstatus.action_dateime = DateTime.Now;
                        rstatus.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                else
                {
                    rstatus = new tbl_brief_read_status();
                    rstatus.id_user = UID;
                    rstatus.id_organization = OID;
                    rstatus.id_brief_master = brief.id_brief_master;
                    rstatus.read_status = 1;
                    rstatus.status = "A";
                    rstatus.action_dateime = DateTime.Now; ;
                    rstatus.action_status = 1;
                    rstatus.read_datetime = DateTime.Now;
                    rstatus.updated_date_time = DateTime.Now;
                    db.tbl_brief_read_status.Add(rstatus);
                    db.SaveChanges();
                }

               
            }
           
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, returnResponse);
            }
            //scoring logic---University

            //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            //{
            //    tbl_game_master gm= db.Database.SqlQuery<tbl_game_master>("Select * from tbl_game_master where id_theme={0} AND status={1}", 9, "A").FirstOrDefault();
            //    if (gm != null && DateTime.Now >= gm.start_date && DateTime.Now <= gm.end_date)
            //    {
            //        tbl_game_academic_mapping game_map = db.Database.SqlQuery<tbl_game_academic_mapping>("Select * from tbl_game_academic_mapping where id_academic_tile={0} AND status={1}", AcademicTileId, "A").FirstOrDefault();

            //        if (game_map != null)
            //        {
            //            List<AssessmentScoreResponse> assessscore = new UniversityScoringlogic().ScoreCal(AcademicTileId, BID, UID, returnResponse.rightCount);
            //            List<AssessmentScoreResponse> assesssplscore = new UniversityScoringlogic().ScoreSplCal(AcademicTileId, BID, UID, returnResponse.rightCount);
            //            foreach (var itm in assessscore)
            //            {
            //                db.Database.ExecuteSqlCommand("Insert into tbl_user_game_score_log (id_user,id_game,id_brief,id_org,score,status,id_academic_tile,updated_date_time,id_metric,metric_value) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", UID, itm.id_game, itm.id_brief, OID, itm.metric_score, "A", AcademicTileId, DateTime.Now, itm.id_metric, itm.metric_name);

            //            }
            //            foreach (var item in assesssplscore)
            //            {
            //                db.Database.ExecuteSqlCommand("Insert into tbl_user_game_special_metric_score_log (id_user,id_game,id_brief,id_org,score,status,id_academic_tile,updated_date_time,id_special_metric,special_metric_value) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", UID, item.id_game, item.id_brief, OID, item.metric_score, "A", AcademicTileId, DateTime.Now, item.id_metric, item.metric_name);

            //            }



            //            //--------------------Badge Validation----------------------------
            //            //string str = APIString.API + "getUserScore?UID=" + UID + "&OID=" + OID;
            //            //string jsonres = new UniversityScoringlogic().getApiResponseString(str);
            //            //UserScoreResponse scoreres = JsonConvert.DeserializeObject<UserScoreResponse>(jsonres);

            //            List<tbl_user_game_score_log> scorelog = new List<tbl_user_game_score_log>();
            //            scorelog = db.Database.SqlQuery<tbl_user_game_score_log>("select * from tbl_user_game_score_log where id_game={0} and id_user={1} and  YEAR(updated_date_time) = {2} AND MONTH(updated_date_time) = {3} ", gm.id_game, UID,DateTime.Now.Year,DateTime.Now.Month).ToList();
            //            double score = 0;
            //            foreach (var item in scorelog)
            //            {
            //                score += item.score;
            //            }
            //            int userscore =Convert.ToInt32(score);



            //            int GameId = db.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} AND status={1}", 9,"A").FirstOrDefault();
            //            List<tbl_badge_master> badgmas = db.Database.SqlQuery<tbl_badge_master>("select * from tbl_badge_master where id_theme={0}", 9).ToList();
            //            foreach (var bad in badgmas)
            //            {
            //                bad.eligiblescore = db.Database.SqlQuery<int>("select required_score from tbl_badge_data where id_game={0} and id_badge={1}", GameId, bad.id_badge).FirstOrDefault();
            //                if (userscore >= bad.eligiblescore)
            //                {
            //                    tbl_user_badge_log badgelog = db.Database.SqlQuery<tbl_user_badge_log>("SELECT * FROM tbl_user_badge_log WHERE YEAR(updated_date_time) = {0} AND MONTH(updated_date_time) = {1} and id_user = {2}  and id_badge = {3} and id_game={4}", DateTime.Now.Year, DateTime.Now.Month, UID, bad.id_badge,GameId).FirstOrDefault();
            //                    if (badgelog == null)
            //                    {
            //                        db.Database.ExecuteSqlCommand("Insert into tbl_user_badge_log (id_user,id_org,id_badge,updated_date_time,id_game) values({0},{1},{2},{3},{4})", UID, OID, bad.id_badge, DateTime.Now, GameId);
            //                    }
            //                }

            //            }





            //            returnResponse.AssessmetGameScore = assessscore;
            //        }


            //    }
                  

            //}







            //--------------------------------------------------

            return Request.CreateResponse(HttpStatusCode.OK, returnResponse);
        }

        public void setRightAnswer()
        {
            List<tbl_brief_audit> audit = db.tbl_brief_audit.Where(t => t.attempt_no == 1 && t.audit_result == 1).ToList();
            foreach (tbl_brief_audit row in audit)
            {
                tbl_brief_b2c_right_audit temp = new tbl_brief_b2c_right_audit();
                temp.data_index = 0;
                temp.datetime_stamp = row.recorded_timestamp;
                temp.id_brief_question = row.id_brief_question;
                temp.id_organization = row.id_organization;
                temp.id_user = row.id_user;
                temp.question_complexity = row.question_complexity;
                temp.status = "A";
                temp.updated_date_time = row.updated_date_time;
                temp.value_count = 1;
                temp.data_index = 0;
                db.tbl_brief_b2c_right_audit.Add(temp);
                db.SaveChanges();
            }

            List<tbl_brief_log> scores = db.tbl_brief_log.Where(t => t.attempt_no == 1).ToList();
            foreach (tbl_brief_log row in scores)
            {
                tbl_brief_b2c_score_audit temp = new tbl_brief_b2c_score_audit();
                temp.data_index = 0;
                temp.datetime_stamp = row.updated_date_time;
                temp.value_count = Convert.ToInt32(row.brief_result);
                temp.id_brief_master = row.id_brief_master;
                temp.id_organization = row.id_organization;
                temp.id_user = row.id_user;
                temp.status = "A";
                temp.updated_date_time = DateTime.Now;
                db.tbl_brief_b2c_score_audit.Add(temp);
                db.SaveChanges();

                tbl_brief_read_status rst = new tbl_brief_read_status();
                rst.id_brief_master = row.id_brief_master;
                rst.id_organization = row.id_organization;
                rst.id_user = row.id_user;
                rst.read_status = 1;
                rst.read_datetime = row.updated_date_time;
                rst.action_status = 1;
                rst.action_dateime = row.updated_date_time;
                rst.status = "A";
                rst.updated_date_time = DateTime.Now;
                db.tbl_brief_read_status.Add(rst);
                db.SaveChanges();
            }
        }

        //public HttpResponseMessage Post([FromBody] BriefRequest request)
        //{
        //    List<BriefDataCube> dataCube = new List<BriefDataCube>();
        //    List<BriefUserInput> responseCube = new List<BriefUserInput>();
        //    BriefReturnResponse returnResponse = new BriefReturnResponse();
        //    string jsonresponse = null;
        //    int qCount = 0;
        //    int rightCount = 0;
        //    tbl_brief_master brief = db.tbl_brief_master.Where(t => t.id_brief_master == request.BID).FirstOrDefault();
        //    if (brief != null)
        //    {
        //        List<string> qList = request.ASRQ.Split(';').ToList();
        //        int qtnCount = qList.Count();
        //        if (qtnCount > 0)
        //        {
        //            string sqlat = "SELECT count(*) subcount FROM tbl_brief_index where id_user=" + request.UID + " and id_brief_master=" + brief.id_brief_master + " ";
        //            int attemptno = new BriefModel().getAttamptNo(sqlat);
        //            attemptno++;

        //            tbl_brief_index index = new tbl_brief_index();
        //            index.id_brief_master = brief.id_brief_master;
        //            index.id_user = request.UID;
        //            index.status = "A";
        //            index.updated_date_time = DateTime.Now;
        //            db.tbl_brief_index.Add(index);
        //            db.SaveChanges();

        //            foreach (string item in qList)
        //            {
        //                string[] content = item.Split('|');//QID|ANS|val
        //                var qitn = content[0];
        //                if (qitn != null)
        //                {
        //                    BriefDataCube temp = new BriefDataCube();
        //                    temp.QID = Convert.ToInt32(qitn);
        //                    temp.AID = Convert.ToInt32(content[1]);
        //                    temp.VAL = "NA";
        //                    dataCube.Add(temp);

        //                    tbl_brief_audit row = new tbl_brief_audit();
        //                    row.attempt_no = attemptno;
        //                    row.id_brief_master = brief.id_brief_master;
        //                    row.id_brief_question = temp.QID;
        //                    row.id_brief_answer = temp.AID;
        //                    row.id_user = request.UID;
        //                    row.recorded_timestamp = System.DateTime.Now;
        //                    row.status = "A";
        //                    row.updated_date_time = DateTime.Now;
        //                    row.value_sent = 0;
        //                    tbl_brief_answer ans = db.tbl_brief_answer.Where(t => t.id_brief_answer == temp.AID).FirstOrDefault();
        //                    if (ans != null)
        //                    {
        //                        if (ans.is_correct_answer == 1)
        //                        {
        //                            row.audit_result = 1;
        //                        }
        //                        else
        //                        {
        //                            row.audit_result = 0;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        row.audit_result = 0;
        //                    }
        //                    row.id_organization = OID;
        //                    db.tbl_brief_audit.Add(row);
        //                    db.SaveChanges();
        //                }
        //            }
        //            int srno = 1;
        //            foreach (BriefDataCube item in dataCube)
        //            {
        //                BriefUserInput urow = new BriefUserInput();
        //                tbl_brief_question qtn = db.tbl_brief_question.Where(t => t.id_brief_question == item.QID).FirstOrDefault();
        //                if (qtn != null)
        //                {
        //                    tbl_brief_answer ans = db.tbl_brief_answer.Where(t => t.id_brief_answer == item.AID).FirstOrDefault();
        //                    tbl_brief_answer wans = db.tbl_brief_answer.Where(t => t.id_brief_question == item.QID && t.is_correct_answer == 1).FirstOrDefault();

        //                    if (ans.is_correct_answer == 1)
        //                    {
        //                        rightCount++;
        //                        urow.is_right = 1;
        //                    }
        //                    else
        //                    {
        //                        urow.is_right = 0;
        //                    }
        //                    urow.Question = "Q. " + qtn.brief_question;
        //                    urow.Answer = "A. " + ans.brief_answer;
        //                    urow.WANS = wans.brief_answer;
        //                    tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == qtn.question_complexity).FirstOrDefault();
        //                    if (comp != null)
        //                    {
        //                        urow.question_complexity = Convert.ToInt32(comp.question_complexity);
        //                        urow.question_complexity_label = comp.question_complexity_label;
        //                    }
        //                    urow.srno = srno++;
        //                    responseCube.Add(urow);
        //                }
        //            }

        //            returnResponse.briefReturn = responseCube;
        //            returnResponse.returnStat = "You have answered " + rightCount + " out of " + qtnCount + " question right ";
        //            returnResponse.rightCount = rightCount;
        //            returnResponse.totalCount = qtnCount;
        //            returnResponse.attemptno = attemptno;

        //            double result = 0.0;
        //            if (rightCount > 0 && qtnCount > 0)
        //            {
        //                result = (rightCount * 100) / (qtnCount);
        //                result = Math.Round(result, 2);
        //            }
        //            returnResponse.percentage = result;

        //            tbl_brief_log log = new tbl_brief_log();
        //            log.attempt_no = attemptno;
        //            log.id_brief_master = brief.id_brief_master;
        //            log.id_organization = request.OID;
        //            log.id_user = request.UID;
        //            log.brief_result = result;
        //            log.status = "A";
        //            log.updated_date_time = DateTime.Now;
        //            jsonresponse = JsonConvert.SerializeObject(returnResponse);
        //            log.json_response = jsonresponse;
        //            db.tbl_brief_log.Add(log);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NoContent, returnResponse);
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, returnResponse);
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, returnResponse);
        //}
    }
}