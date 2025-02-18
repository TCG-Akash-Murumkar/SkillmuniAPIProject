using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;
using System.Diagnostics;
using System.Configuration;

namespace m2ostnextservice.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EvaluateCEvaluationController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]CERequest data)
        {
            data.CRF = new Utility().mysqlTrim(data.CRF.ToString());

            List<CEUserInput> responseCube = new List<CEUserInput>();
            CEReturnResponse returnResponse = new CEReturnResponse();
            List<ComplexityResult> comList = new List<ComplexityResult>();
            List<tbl_brief_question_complexity> qclist = db.tbl_brief_question_complexity.Where(t => t.status == "A").ToList();
            string jsonresponse = null;
            int qCount = 0;
            int rightCount = 0;
            int ce_evaluation_result = 0;
            string cesql01 = "select * from tbl_ce_career_evaluation_master where lower(career_evaluation_code)=lower('" + data.CRF + "') and id_organization=" + data.OID + " and status='A' limit 1";
            tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql01).FirstOrDefault();
            int attemptno = 0;
            int checkattemptno = 0;
            bool c1Flag = false;
            string cetime = "00:00";
            // tbl_brief_master brief = db.tbl_brief_master.Where(t => t.id_brief_master == BID).FirstOrDefault();
            if (cmaster != null)
            {
                int qtnCount = data.ASRQ.Count;
                if (qtnCount > 0)
                {
                    string cesql02 = "SELECT * FROM tbl_ce_evaluation_index where id_user=" + data.UID + " and id_ce_career_evaluation_master=" + cmaster.id_ce_career_evaluation_master + " order by id_ce_evaluation_index desc limit 1";
                    tbl_ce_evaluation_index cindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(cesql02).FirstOrDefault();

                    if (cindex != null)
                    {
                        attemptno = cindex.attempt_no;
                        checkattemptno = attemptno;
                    }
                    c1Flag = checkAttemptCompilation(cmaster.id_ce_evaluation_tile, data.UID, data.OID, checkattemptno);
                    int currentindex = 0;
                    string cqsql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + data.UID + " AND id_organization =  " + data.OID + " ORDER BY attempt_no DESC LIMIT 1";
                    tbl_ce_evaluation_index ceindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(cqsql01).FirstOrDefault();
                    if (ceindex != null)
                    {
                        currentindex = ceindex.attempt_no;
                    }
                    if (currentindex == attemptno)
                    {
                        attemptno++;
                    }
                    if (currentindex > attemptno)
                    {
                        attemptno = currentindex;
                    }
                    if (data.CETime != null)
                    {
                        cetime = data.CETime;
                    }
                    //attemptno++;
                    db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_index (ce_evaluation_token,id_ce_career_evaluation_master, id_user, id_organization, attempt_no, dated_time_stamp,cetimespan, status, updated_date_time) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", data.TOKEN, cmaster.id_ce_career_evaluation_master, data.UID, data.OID, attemptno, DateTime.Now, cetime, 'A', DateTime.Now);

                    foreach (CEASRQ row in data.ASRQ)
                    {
                        string cesql03 = "select * from tbl_brief_question where id_brief_question=" + row.QID + " and status='A' ";
                        tbl_brief_question question = db.Database.SqlQuery<tbl_brief_question>(cesql03).FirstOrDefault();
                        bool flag = false;

                        if (cmaster.ce_assessment_type == 1)
                        {
                            string cesql04 = "select * from tbl_brief_answer where id_brief_answer=" + row.ANS + " and status='A' ";
                            tbl_brief_answer ans = db.Database.SqlQuery<tbl_brief_answer>(cesql04).FirstOrDefault();
                            int audit_result = 0;
                            int job_points = 0;

                            if (ans != null)
                            {
                                if (ans.is_correct_answer == 1)
                                {
                                    flag = true;
                                    audit_result = 1;
                                    job_points = Convert.ToInt32(cmaster.job_points_for_ra);
                                }
                                else
                                {
                                    audit_result = 0;
                                }
                            }
                            else
                            {
                                audit_result = 0;
                            }
                            db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_audit (id_ce_career_evaluation_master,ce_evaluation_token, id_organization, id_user, id_brief_question, question_complexity, id_brief_answer, value_sent, attempt_no, recorded_timestamp, audit_result,job_point,id_ce_evalution_answer_key, status, updated_date_time) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", cmaster.id_ce_career_evaluation_master, data.TOKEN, data.OID, data.UID, question.id_brief_question, question.question_complexity, row.ANS, row.VAL, attemptno, DateTime.Now, audit_result, job_points, 0, 'A', DateTime.Now);
                            int id_job_log = db.Database.SqlQuery<int>("select id_job_log from tbl_job_user_log where id_user={0} and id_job={1}", data.UID, data.ID_JOB).FirstOrDefault();
                            if (id_job_log > 0)
                            {
                                db.Database.ExecuteSqlCommand("update  tbl_job_user_log set  status ='A' where id_user={0} and id_job={1}", data.UID, data.ID_JOB);

                            }
                            else
                            {
                                db.Database.ExecuteSqlCommand("INSERT INTO tbl_job_user_log (id_user,id_job, status, updated_date_time, id_org) VALUES ({0},{1},{2},{3},{4})", data.UID, data.ID_JOB, "A", DateTime.Now, data.OID);

                            }

                        }
                        if (cmaster.ce_assessment_type == 2)
                        {
                            string cesql05 = "select * from tbl_brief_answer where id_brief_question=" + row.QID + " and status='A' ";
                            List<tbl_brief_answer> ansList = db.Database.SqlQuery<tbl_brief_answer>(cesql05).ToList();
                            List<TEMPANS> tans = new List<TEMPANS>();
                            foreach (tbl_brief_answer arow in ansList)
                            {
                                TEMPANS temp = new TEMPANS();
                                temp.AID = arow.id_brief_answer;
                                if (row.ANS == arow.id_brief_answer)
                                {
                                    temp.SVAL = Convert.ToInt32(row.KVAL);
                                }
                                string cesql06 = "SELECT * FROM tbl_ce_evalution_psychometric_setup where id_brief_question= " + arow.id_brief_question + " and id_brief_answer=" + arow.id_brief_answer + " and id_ce_career_evaluation_master=" + cmaster.id_ce_career_evaluation_master + " ";
                                tbl_ce_evalution_psychometric_setup setup = db.Database.SqlQuery<tbl_ce_evalution_psychometric_setup>(cesql06).FirstOrDefault();
                                if (setup != null)
                                {
                                    temp.AKVAL = setup.id_ce_evalution_answer_key;
                                }
                                tans.Add(temp);
                            }

                            tans = tans.OrderByDescending(t => t.SVAL).ToList();
                            int aklimit = 3;// Convert.ToInt32(cmaster.job_points_for_ra);
                            int ii = 1; int tmpval = 0;
                            foreach (TEMPANS irow in tans)
                            {
                                if (ii == 1)
                                {
                                    ii++;
                                    tmpval = irow.SVAL;
                                    aklimit = aklimit - irow.SVAL;
                                }
                                else
                                {
                                    tmpval = aklimit;
                                }
                                //  Debug.WriteLine("Q " + ii + " : " + row.QID + " -- " + irow.AID + " ==> " + tmpval);
                                db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_audit (id_ce_career_evaluation_master,ce_evaluation_token, id_organization, id_user, id_brief_question, question_complexity, id_brief_answer, value_sent, attempt_no, recorded_timestamp, audit_result,job_point,id_ce_evalution_answer_key, status, updated_date_time,id_job) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", cmaster.id_ce_career_evaluation_master, data.TOKEN, data.OID, data.UID, question.id_brief_question, question.question_complexity, irow.AID, row.VAL, attemptno, DateTime.Now, tmpval, tmpval, irow.AKVAL, 'A', DateTime.Now,data.ID_JOB);
                            }
                            // Debug.WriteLine("---------------------------------------");
                        }
                    }
                    string briefUrl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "briefQuesion/" + data.OID + "/question/";
                    string choiceurl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "briefQuesion/" + data.OID + "/choice/";
                    int srno = 1;
                    foreach (CEASRQ row in data.ASRQ)
                    {
                        CEUserInput urow = new CEUserInput();
                        tbl_brief_question qtn = db.tbl_brief_question.Where(t => t.id_brief_question == row.QID).FirstOrDefault();
                        if (qtn != null)
                        {

                            if (!string.IsNullOrEmpty(qtn.question_image))
                            {
                                //C:\SULCmsProduction\Content\SKILLMUNI_DATA\briefQuesion\130\question
                                qtn.question_image = briefUrl + qtn.question_image;
                            }
                            else
                            {
                                qtn.question_image = "";
                            }

                            tbl_brief_answer wans = new tbl_brief_answer();
                            tbl_brief_answer ans = db.tbl_brief_answer.Where(t => t.id_brief_answer == row.ANS).FirstOrDefault();
                            List<CEAnswerBody> answerBody = new List<CEAnswerBody>();

                            if (cmaster.ce_assessment_type == 1)
                            {
                                int rightans = 0;

                                if (ans.is_correct_answer == 1)
                                {
                                    rightCount++;
                                    urow.is_right = 1;
                                    urow.WANS = ans.brief_answer;
                                }
                                else
                                {
                                    wans = db.tbl_brief_answer.Where(t => t.id_brief_question == row.QID && t.is_correct_answer == 1).FirstOrDefault();
                                    urow.is_right = 0;
                                    urow.WANS = wans.brief_answer;
                                }

                                string wansql01 = "SELECT CASE WHEN (a.choice_image IS NOT NULL) THEN CONCAT('" + choiceurl + "', a.choice_image) ELSE NULL END choice_image, CASE  WHEN (a.id_brief_answer = " + ans.id_brief_answer + ") THEN 'UC' WHEN (a.is_correct_answer = 1) THEN 'RC' ELSE 'NA' END answer_role, a.* FROM tbl_brief_answer a WHERE id_organization = " + data.OID+" AND id_brief_question = " + qtn.id_brief_question + " AND status = 'A'";
                                answerBody = db.Database.SqlQuery<CEAnswerBody>(wansql01).ToList();
                                urow.answerBody = answerBody;
                            }
                            if (cmaster.ce_assessment_type == 2)
                            {
                                urow.WANS = "NA";
                                urow.is_right = 0;
                                string cesql04 = "SELECT * FROM tbl_ce_evaluation_audit WHERE id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + " AND id_brief_question = " + qtn.id_brief_question + " AND id_brief_answer = " + ans.id_brief_answer + " "; ;
                                tbl_ce_evaluation_audit asetup = db.Database.SqlQuery<tbl_ce_evaluation_audit>(cesql04).FirstOrDefault();
                                if (asetup != null)
                                {
                                    urow.jpscore = Convert.ToInt32(asetup.job_point);
                                }
                            }

                            urow.questionBody = qtn;
                            urow.Question = "Q. " + qtn.brief_question;
                            urow.Answer = ans.brief_answer;

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
                        string qcsql = "SELECT CASE WHEN COUNT(*) > 0 THEN COUNT(*) ELSE 0 END totalcount, CASE WHEN COUNT(audit_result) > 0 THEN COUNT(audit_result) ELSE 0 END rightcount, CASE WHEN sum(job_point) > 0 THEN sum(job_point) ELSE 0 END jobpoint FROM tbl_ce_evaluation_audit " +
                            " WHERE id_user = " + data.UID + " AND id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + " AND attempt_no = " + attemptno + " AND id_brief_question IN (SELECT id_brief_question FROM tbl_brief_question WHERE question_complexity = " + row.question_complexity + ")";
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
                    if (cmaster.ce_assessment_type == 1)
                    {
                        returnResponse.returnStat = "You have answered " + rightCount + " out of " + qtnCount + " question right ";
                        returnResponse.rightCount = rightCount;
                        string csql01 = "SELECT  attempt_no, SUM(job_point) job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + data.UID + " AND id_organization = " + data.OID + " AND id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + " AND attempt_no = " + attemptno + " GROUP BY attempt_no limit 1";
                        JobPoint jPoint = db.Database.SqlQuery<JobPoint>(csql01).FirstOrDefault();
                        if (jPoint != null) ce_evaluation_result = jPoint.job_point;
                    }
                    else
                    {
                        returnResponse.returnStat = "NA";
                        returnResponse.rightCount = 0;
                    }
                    List<AnswerKeyBlock> ansBlock = new List<AnswerKeyBlock>();
                    if (cmaster.ce_assessment_type == 2)
                    {
                        string akUrl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "ANSWERKEY/";
                        string cesql05 = "SELECT b.id_ce_evalution_answer_key, key_code, concat('" + akUrl + "',key_code,'.png') aklogo, answer_key, SUM(b.job_point) job_point FROM tbl_ce_evalution_answer_key a, tbl_ce_evaluation_audit b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND a.id_ce_evalution_answer_key = b.id_ce_evalution_answer_key AND b.id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + " AND b.id_user = " + data.UID + " AND b.attempt_no = " + attemptno + " GROUP BY b.id_ce_evalution_answer_key ORDER BY job_point DESC";
                        ansBlock = db.Database.SqlQuery<AnswerKeyBlock>(cesql05).ToList();
                    }
                    returnResponse.answerKeyBlock = ansBlock;
                    returnResponse.complexity = comList;
                    returnResponse.ceReturn = responseCube;
                    returnResponse.totalCount = qtnCount;
                    returnResponse.attemptno = attemptno;
                    jsonresponse = JsonConvert.SerializeObject(returnResponse);
                    db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_log(id_user,id_organization,id_ce_career_evaluation_master,json_response, attempt_no, ce_evaluation_result,cetimespan, status, updated_date_time,id_job) VALUES ({0},{1}, {2}, {3}, {4}, {5}, {6}, {7}, {8},{9})", data.UID, data.OID, cmaster.id_ce_career_evaluation_master, jsonresponse, attemptno, ce_evaluation_result, cetime, 'A', DateTime.Now,data.ID_JOB);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, returnResponse);
            }
            if (!c1Flag)
            {
                bool c2Flag = checkAttemptCompilation(cmaster.id_ce_evaluation_tile, data.UID, data.OID, checkattemptno);
                if (c2Flag)
                {
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, returnResponse);
        }

        private bool checkAttemptCompilation(int ctid, int UID, int OID, int cindex)
        {
            bool cFlag = false;
            string evsql02 = "SELECT * FROM tbl_ce_career_evaluation_master  WHERE  ce_assessment_type=1 AND  id_organization = " + OID + " AND id_ce_career_evaluation_master NOT IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + cindex + ")";
            List<tbl_ce_career_evaluation_master> evPending = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql02).ToList();
            double pendingEVCount = evPending.Count;
            if (pendingEVCount > 0)
            {
                cFlag = false;
            }
            else
            {
                cFlag = true;
            }
            return cFlag;
        }

        private void generateCompletionReport(int ctid, int UID, int OID, int cindex)
        {
        }
    }
}