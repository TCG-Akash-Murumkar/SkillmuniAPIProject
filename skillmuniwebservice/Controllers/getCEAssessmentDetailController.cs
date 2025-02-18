using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace m2ostnextservice.Controllers
{
    public class getCEAssessmentDetailController : ApiController
    {
        private m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public HttpResponseMessage Get(string crf, int UID, int OID)
        {
            CEBriefBody bBody = new CEBriefBody();
            List<APIBrief> list = new List<APIBrief>();
            crf = crf.ToLower().Trim();
            int currentindex = 0;
            int lastindex = 0;
            string cesql01 = "select * from tbl_ce_career_evaluation_master where lower(career_evaluation_code)=lower('" + crf + "') and id_organization=" + OID + " and status='A' limit 1";
            tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql01).FirstOrDefault();
            bool cFlag = false; bool aFlag = false;
            if (cmaster != null)
            {
                int psyIndex = 0; int psyFlag = 0;
                int cattype = Convert.ToInt32(cmaster.ce_assessment_type);
                int nextattempt = 0;
                int ctid = Convert.ToInt32(cmaster.id_ce_evaluation_tile);
                currentindex = getCurrentIndex(crf, UID, OID);
                nextattempt = currentindex;
                string ctsql = "SELECT * FROM tbl_ce_evaluation_tile where id_organization=" + OID + " and id_ce_evaluation_tile=" + cmaster.id_ce_evaluation_tile;
                bBody.tile = db.Database.SqlQuery<tbl_ce_evaluation_tile>(ctsql).FirstOrDefault();

                cFlag = checkAttemptCompilation(ctid, UID, OID, currentindex);

                if (cFlag)
                {
                    nextattempt++;
                }

                if (cattype == 1)
                {
                    aFlag = checkCurrentEvaluationStatus(cmaster.id_ce_career_evaluation_master, UID, OID, nextattempt);
                }

                if (cattype == 2)
                {
                    psyFlag = checkPsychometricEvaluationStatus(cmaster.id_ce_career_evaluation_master, UID, OID, out int retIndex);
                    if (psyFlag > 0)
                    {
                        aFlag = false;
                    }
                    else
                    {
                        aFlag = true;
                    }

                    psyIndex = retIndex;
                }

                if (!aFlag && cattype == 2)
                {
                    if (psyFlag == 2)
                    {
                        bBody.status = "failed";
                        bBody.message = "You have already completed this Psychometric Assessment. You can re-attempt after some time.";
                        return Request.CreateResponse(HttpStatusCode.OK, bBody);
                    }
                    else
                    {
                        aFlag = true;
                        //bBody.status = "failed";
                        //bBody.message = "You have already completed this Psychometric Assessment .You can re-attempt after some time ";
                    }
                }

                if (aFlag)
                {
                    string cetoken = "CET" + new Utility().uniqueIDS(7);
                    int totalQTN = 0;
                    int remQtn = 0;
                    totalQTN = Convert.ToInt32(cmaster.no_of_question);
                    tbl_ce_career_evaluation_master row = cmaster;
                    CECategory crow = new CECategory();
                    crow.id_ce_career_evaluation_master = row.id_ce_career_evaluation_master;
                    crow.career_evaluation_title = row.career_evaluation_title;
                    crow.career_evaluation_code = row.career_evaluation_code;
                    crow.id_ce_evaluation_tile = Convert.ToInt32(row.id_ce_evaluation_tile);
                    crow.ce_description = row.ce_description;
                    crow.validation_period = Convert.ToInt32(row.validation_period);
                    crow.ordering_sequence_number = Convert.ToInt32(row.ordering_sequence_number);
                    crow.no_of_question = Convert.ToInt32(row.no_of_question);
                    crow.is_time_enforced = Convert.ToInt32(row.is_time_enforced);
                    crow.time_enforced = Convert.ToInt32(row.time_enforced);
                    crow.ce_assessment_type = Convert.ToInt32(row.ce_assessment_type);
                    crow.job_points_for_ra = Convert.ToInt32(row.job_points_for_ra);
                    crow.CEToken = cetoken;
                    bBody.CEBody = crow;
                    List<CEQuestionList> qtnList = new List<CEQuestionList>();
                    List<tbl_brief_question> qList = new List<tbl_brief_question>();

                    bBody.RESULTSTATUS = 0;
                    bBody.RESULTSCORE = 0;
                    bBody.RESULT = null;
                    List<int> checkList = new List<int>();

                    remQtn = totalQTN;

                    /*category distribution process*/

                    List<tbl_brief_category> catList = new List<tbl_brief_category>();
                    string briefUrl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "briefQuesion/" + OID + "/question/";
                    string choiceurl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "briefQuesion/" + OID + "/choice/";
                    if (cattype == 1)
                    {
                        string cesql03 = "SELECT * FROM tbl_brief_category WHERE status='A' and  id_brief_category IN (SELECT DISTINCT id_brief_category FROM tbl_ce_category_mapping WHERE  status='A' and  id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + ")";
                        catList = db.Database.SqlQuery<tbl_brief_category>(cesql03).ToList();

                        int flag = remQtn;
                        List<tbl_brief_question> tempList = new List<tbl_brief_question>();

                        int k = catList.Count();
                        int kl = k * 20;
                        int j = 0;
                        int totalcount = 0;
                        do
                        {
                            int index = j % k;
                            tbl_brief_category item = catList[index];
                            tbl_brief_question temp = getCEProgressiveDistributionQuestion(UID, item.id_brief_category, OID, totalcount);

                            if (temp != null)
                            {
                                totalcount++;
                                if (checkList.Contains(temp.id_brief_question))
                                {
                                }
                                else
                                {
                                    tbl_brief_question tq = tempList.Where(t => t.id_brief_question == temp.id_brief_question).FirstOrDefault();
                                    if (tq == null)
                                    {
                                        if (!string.IsNullOrEmpty(temp.question_image))
                                        {
                                            //C:\SULCmsProduction\Content\SKILLMUNI_DATA\briefQuesion\130\question
                                            temp.question_image = briefUrl + temp.question_image;
                                        }
                                        else
                                        {
                                            temp.question_image = "";
                                        }
                                        tempList.Add(temp);
                                        flag = flag - 1;
                                    }
                                }
                            }
                            if (j > 150)
                            {
                                break;
                            }
                            j++;
                        } while (tempList.Count != remQtn);

                        foreach (tbl_brief_question item in tempList)
                        {
                            tbl_ce_evaluation_progdist_mapping mapping = new tbl_ce_evaluation_progdist_mapping();
                            mapping.id_ce_career_evaluation_master = cmaster.id_ce_career_evaluation_master;
                            mapping.ce_evaluation_token = cetoken;
                            mapping.id_brief_question = item.id_brief_question;
                            mapping.id_user = UID;
                            mapping.date_time_stamp = System.DateTime.Now;
                            mapping.question_link_type = 0;
                            mapping.status = "A";
                            mapping.updated_date_time = DateTime.Now;
                            db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_progdist_mapping (ce_evaluation_token, id_ce_career_evaluation_master, id_brief_question, id_user, question_link_type, date_time_stamp, status, updated_date_time) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", cetoken, cmaster.id_ce_career_evaluation_master, item.id_brief_question, UID, 0, DateTime.Now, 'A', DateTime.Now);

                            CEQuestionList temp = new CEQuestionList();
                            string cesql04 = "select * from tbl_brief_question_complexity where question_complexity=" + item.question_complexity + " and status='A'";
                            tbl_brief_question_complexity comp = db.Database.SqlQuery<tbl_brief_question_complexity>(cesql04).FirstOrDefault();
                            if (comp != null)
                            {
                                temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                                temp.question_complexity_label = comp.question_complexity_label;
                            }
                            temp.CEToken = cetoken;
                            temp.question = item;
                            string cesql05 = "SELECT CASE WHEN (a.choice_image IS NOT NULL) THEN CONCAT('" + choiceurl + "', a.choice_image) ELSE NULL END choice_image, a.* FROM tbl_brief_answer a WHERE id_organization = " + OID + " AND id_brief_question = " + item.id_brief_question + " AND status = 'A'";
                            List<tbl_brief_answer> answer = db.Database.SqlQuery<tbl_brief_answer>(cesql05).ToList();
                            temp.answers = answer;
                            qtnList.Add(temp);
                        }
                    }

                    if (cattype == 2)
                    {
                        List<tbl_brief_question> tempList = new List<tbl_brief_question>();

                        string cesql06 = "SELECT * FROM tbl_brief_category where  status='A' and   id_brief_category in (SELECT id_brief_category FROM tbl_ce_category_mapping where status='A' and id_ce_career_evaluation_master=" + cmaster.id_ce_career_evaluation_master + ") and status='A' and id_organization=" + OID + " ";
                        tbl_brief_category cmcategory = db.Database.SqlQuery<tbl_brief_category>(cesql06).FirstOrDefault();
                        if (cmcategory != null)
                        {
                            //string cesql07 = "SELECT DISTINCT a.* FROM tbl_brief_question a, tbl_ce_evalution_psychometric_setup b WHERE a.id_brief_question = b.id_brief_question AND b.id_ce_career_evaluation_master = " + cmaster.id_ce_career_evaluation_master + " AND b.id_organization = " + OID + " AND a.id_brief_category = " + cmcategory.id_brief_category + " AND a.status = 'A' AND expiry_date > NOW() ORDER BY b.ordering_sequence";
                            string cesql07 = "SELECT * FROM tbl_brief_question WHERE id_organization=" + OID + " and  id_brief_category =" + cmcategory.id_brief_category + " AND status = 'A' AND expiry_date > NOW() ORDER BY question_complexity ";
                            tempList = db.Database.SqlQuery<tbl_brief_question>(cesql07).ToList();
                            List<CEQuestionSorted> sortList = new List<CEQuestionSorted>();
                            int i = 1;
                            foreach (tbl_brief_question item in tempList)
                            {
                                CEQuestionSorted sorted = new CEQuestionSorted();
                                sorted.question = item;
                                string cesql071 = "SELECT ordering_sequence FROM tbl_ce_evalution_psychometric_setup where id_ce_career_evaluation_master=" + cmaster.id_ce_career_evaluation_master + " and id_organization= " + OID + "   and id_brief_question=" + item.id_brief_question + " ";
                                int ids = db.Database.SqlQuery<int>(cesql071).FirstOrDefault();
                                sorted.ordering_sequence = ids;
                                sortList.Add(sorted);
                            }
                            sortList = sortList.OrderBy(t => t.ordering_sequence).ToList();

                            foreach (CEQuestionSorted item in sortList)
                            {
                                tbl_ce_evaluation_progdist_mapping mapping = new tbl_ce_evaluation_progdist_mapping();
                                mapping.id_ce_career_evaluation_master = cmaster.id_ce_career_evaluation_master;
                                mapping.ce_evaluation_token = cetoken;
                                mapping.id_brief_question = item.question.id_brief_question;
                                mapping.id_user = UID;
                                mapping.date_time_stamp = System.DateTime.Now;
                                mapping.question_link_type = 1;
                                mapping.status = "A";
                                mapping.updated_date_time = DateTime.Now;
                                db.Database.ExecuteSqlCommand("INSERT INTO tbl_ce_evaluation_progdist_mapping (ce_evaluation_token, id_ce_career_evaluation_master, id_brief_question, id_user, question_link_type, date_time_stamp, status, updated_date_time) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", cetoken, cmaster.id_ce_career_evaluation_master, item.question.id_brief_question, UID, 1, DateTime.Now, 'A', DateTime.Now);
                                //   db.tbl_ce_evaluation_progdist_mapping.Add(mapping);
                                //   db.SaveChanges();

                                CEQuestionList temp = new CEQuestionList();
                                string cesql08 = "select * from tbl_brief_question_complexity where question_complexity=" + item.question.question_complexity + " and status='A'";
                                tbl_brief_question_complexity comp = db.Database.SqlQuery<tbl_brief_question_complexity>(cesql08).FirstOrDefault();
                                if (comp != null)
                                {
                                    temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                                    temp.question_complexity_label = comp.question_complexity_label;
                                }

                                if (!string.IsNullOrEmpty(item.question.question_image))
                                {
                                    //C:\SULCmsProduction\Content\SKILLMUNI_DATA\briefQuesion\130\question
                                    item.question.question_image = briefUrl + item.question.question_image;
                                }
                                else
                                {
                                    item.question.question_image = "";
                                }
                                temp.qtnnum = i;
                                temp.CEToken = cetoken;
                                item.question.qtnnum = i;
                                temp.question = item.question;
                                // string cesql09 = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                                string cesql09 = "SELECT CASE WHEN (a.choice_image IS NOT NULL) THEN CONCAT('" + choiceurl + "', a.choice_image) ELSE NULL END choice_image, a.* FROM tbl_brief_answer a WHERE id_organization = " + OID + " AND id_brief_question = " + item.question.id_brief_question + " AND status = 'A'";
                                List<tbl_brief_answer> answer = db.Database.SqlQuery<tbl_brief_answer>(cesql09).ToList();
                                temp.answers = answer;
                                qtnList.Add(temp);
                                i++;
                            }
                        }
                    }

                    bBody.QTNLIST = qtnList;
                    bBody.status = "success";
                    bBody.message = "";
                }
                else
                {
                    bBody.status = "failed";
                    bBody.message = "You need to complete all the other assessments to re-attempt this one.";
                    return Request.CreateResponse(HttpStatusCode.OK, bBody);
                }
            }
            else
            {
                bBody.status = "failed";
                bBody.message = "Invalid Assessment Request.";
                return Request.CreateResponse(HttpStatusCode.OK, bBody);
            }

            return Request.CreateResponse(HttpStatusCode.OK, bBody);
        }

        private tbl_brief_question getCEProgressiveDistributionQuestion(int UID, int CID, int OID, int totalcount)
        {
            string sql015 = "select count(*) scount from tbl_brief_question where  id_organization=" + OID + "  and  id_brief_category =" + CID + " and status='A'";
            int tcount = db.Database.SqlQuery<int>(sql015).FirstOrDefault();
            bool repeateFlag = false;
            bool redistibutionFlag = false;
            if (tcount <= totalcount)
            {
                repeateFlag = true;
            }
            if (tcount > 0)
            {
                if ((tcount * 2) <= totalcount)
                    redistibutionFlag = true;
            }
            tbl_ce_evaluation_audit lstQtn = new tbl_ce_evaluation_audit();
            string cesql10 = "SELECT * FROM tbl_ce_evaluation_audit WHERE  id_user = " + UID + " AND id_brief_question IN (SELECT id_brief_question FROM tbl_brief_question WHERE  id_organization=" + OID + " and id_brief_category = " + CID + ") ORDER BY id_ce_evaluation_audit DESC LIMIT 1";
            lstQtn = db.Database.SqlQuery<tbl_ce_evaluation_audit>(cesql10).FirstOrDefault();
            bool nextDir = false;

            if (lstQtn != null)
            {
                string cesql11 = "select * from tbl_brief_question where id_brief_question= " + lstQtn.id_brief_question + " ";
                tbl_brief_question qtn = db.Database.SqlQuery<tbl_brief_question>(cesql11).FirstOrDefault();
                if (lstQtn.audit_result == 1)
                {
                    nextDir = true;
                }
                else
                {
                }
                int complexity = getComplecityLevel(CID, nextDir, qtn.question_complexity);
                if (repeateFlag)
                {
                    string cesql121 = "select * from tbl_brief_question where  id_organization=" + OID + "  and  id_brief_category =" + CID + " and  id_brief_question in (SELECT distinct id_brief_question FROM tbl_ce_evaluation_audit where id_user =" + UID + " AND audit_result=0) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY  RAND() LIMIT 1";
                    tbl_brief_question nextQtn1 = db.Database.SqlQuery<tbl_brief_question>(cesql121).FirstOrDefault();
                    if (nextQtn1 != null)
                    {
                        //   Debug.WriteLine("QTN R : " + CID + " - " + nextQtn1.question_complexity + "-" + nextQtn1.brief_question);
                        return nextQtn1;
                    }
                }
                if (redistibutionFlag)
                {
                    string cesql122 = "select * from tbl_brief_question where  id_organization=" + OID + "  and  id_brief_category =" + CID + " and status='A' and expiry_date>now() ORDER BY question_complexity, RAND() LIMIT 1";
                    tbl_brief_question nextQtn2 = db.Database.SqlQuery<tbl_brief_question>(cesql122).FirstOrDefault();
                    if (nextQtn2 != null)
                    {
                        return nextQtn2;
                    }
                }
                string cesql12 = "select * from tbl_brief_question where  id_organization=" + OID + "  and  id_brief_category =" + CID + " and  id_brief_question not in (SELECT distinct id_brief_question FROM tbl_ce_evaluation_audit where id_user =" + UID + " ) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY  RAND() LIMIT 1";
                tbl_brief_question nextQtn = db.Database.SqlQuery<tbl_brief_question>(cesql12).FirstOrDefault();
                if (nextQtn != null)
                {
                    // Debug.WriteLine("QTN N : " + CID + " - " + nextQtn.question_complexity + "-" + nextQtn.brief_question);
                    return nextQtn;
                }
                else
                {
                    string cesql13 = "select * from tbl_brief_question where   id_organization=" + OID + "  and  id_brief_category =" + CID + " and id_brief_question in (SELECT distinct id_brief_question FROM tbl_ce_evaluation_audit where id_user =" + UID + " AND audit_result=0) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY RAND() LIMIT 1";
                    tbl_brief_question subQtn = db.Database.SqlQuery<tbl_brief_question>(cesql13).FirstOrDefault();
                    if (subQtn != null)
                    {
                        //  Debug.WriteLine("QTN S : " + CID + " - " + subQtn.question_complexity + "-" + subQtn.brief_question);
                        return subQtn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                string cesql14 = "SELECT * FROM tbl_brief_question WHERE id_organization=" + OID + " and  id_brief_category =" + CID + " AND status = 'A' AND expiry_date > NOW() ORDER BY question_complexity,RAND()  LIMIT 1";
                tbl_brief_question firQtn = db.Database.SqlQuery<tbl_brief_question>(cesql14).FirstOrDefault();
                if (firQtn != null)
                {
                    // Debug.WriteLine("QTN F : " + CID + " - " + firQtn.question_complexity + "-" + firQtn.brief_question);
                    return firQtn;
                }
                else
                {
                    return null;
                }
            }
        }

        private int getComplecityLevel(int CID, bool status, int? level)
        {
            string cesql150 = "select count(*) scount from tbl_brief_question where  id_brief_category = " + CID + " AND question_complexity=" + level + " ";
            int lcount = db.Database.SqlQuery<int>(cesql150).FirstOrDefault();
            if (lcount != 0)
            {
            }

            string additional = "";
            if (status)
            {
                additional = "  AND question_complexity > " + level + " order by question_complexity  LIMIT 1 ";
            }
            else
            {
                additional = "  AND question_complexity < " + level + " order by question_complexity desc LIMIT 1 ";
            }
            string cesql15 = "SELECT * FROM tbl_brief_question_complexity WHERE question_complexity IN (SELECT DISTINCT question_complexity FROM tbl_brief_question WHERE id_brief_category = " + CID + ") " + additional;
            tbl_brief_question_complexity levels = db.Database.SqlQuery<tbl_brief_question_complexity>(cesql15).FirstOrDefault();
            if (levels != null)
            {
                return Convert.ToInt32(levels.question_complexity);
            }
            else
            {
                return Convert.ToInt32(level);
            }
        }

        private int getCurrentIndex(string crf, int UID, int OID)
        {
            int cindex = 1; int lindex = 0;
            string csql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization =  " + OID + " ORDER BY attempt_no DESC LIMIT 1";
            tbl_ce_evaluation_index ceindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(csql01).FirstOrDefault();
            if (ceindex != null)
            {
                cindex = ceindex.attempt_no;
            }
            return cindex;
        }

        private bool checkAttemptCompilation(int ctid, int UID, int OID, int cindex)
        {
            bool cFlag = false;
            string dsql03 = "select * from tbl_ce_career_evaluation_master WHERE ce_assessment_type=1 AND id_ce_evaluation_tile=" + ctid + " AND status='A' order by ordering_sequence_number ";
            List<tbl_ce_career_evaluation_master> cmasterList = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(dsql03).ToList();
            double totalEVCount = cmasterList.Count;
            string evsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE  ce_assessment_type=1 AND  id_organization = " + OID + " AND id_ce_career_evaluation_master  IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + cindex + ")";
            List<tbl_ce_career_evaluation_master> evDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql01).ToList();
            double doneEVCount = evDone.Count;
            string evsql02 = "SELECT * FROM tbl_ce_career_evaluation_master  WHERE  ce_assessment_type=1 AND  id_organization = " + OID + " AND id_ce_career_evaluation_master NOT IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + cindex + ")";
            List<tbl_ce_career_evaluation_master> evPending = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql02).ToList();
            double pendingEVCount = evPending.Count;
            if (doneEVCount > 0)
            {
                if (totalEVCount == doneEVCount)
                {
                    cFlag = true;
                }
            }
            else
            {
                cFlag = true;
            }
            return cFlag;
        }

        private bool checkCurrentEvaluationStatus(int ceid, int UID, int OID, int cindex)
        {
            bool aFlag = false;
            string chsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master IN (SELECT id_ce_career_evaluation_master FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master=" + ceid + " AND attempt_no = " + cindex + ") limit 1";
            tbl_ce_career_evaluation_master chDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(chsql01).FirstOrDefault();
            if (chDone == null)
            {
                aFlag = true;
            }
            // return aFlag;
            return true;
        }

        //private int checkPsychometricEvaluationStatus(int ceid, int UID, int OID)
        //{
        //    bool aFlag = false;
        //    int ilog = 0;
        //    string chsql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + ceid + " ORDER BY attempt_no DESC LIMIT 1";
        //    tbl_ce_evaluation_index index = db.Database.SqlQuery<tbl_ce_evaluation_index>(chsql01).FirstOrDefault();
        //    if (index != null)
        //    {
        //        ilog = index.attempt_no;
        //        string csql = "select * from tbl_ce_career_evaluation_master where id_ce_career_evaluation_master=" + ceid + " ";
        //        tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(csql).FirstOrDefault();
        //        if (cmaster != null)
        //        {
        //            if (cmaster.validation_period > 0)
        //            {
        //                DateTime lastattempt = index.dated_time_stamp;
        //                lastattempt = lastattempt.AddMonths(Convert.ToInt32(cmaster.validation_period));
        //                if (DateTime.Now > lastattempt)
        //                {
        //                    ilog = 1;
        //                }
        //                else
        //                {
        //                    ilog = 2;
        //                }
        //            }
        //        }
        //    }
        //    return ilog;
        //}

        private int checkPsychometricEvaluationStatus(int ceid, int UID, int OID, out int retIndex)
        {
            bool aFlag = false;
            int ilog = 0;
            retIndex = 0;
            string chsql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + ceid + " ORDER BY attempt_no DESC LIMIT 1";
            tbl_ce_evaluation_index index = db.Database.SqlQuery<tbl_ce_evaluation_index>(chsql01).FirstOrDefault();
            if (index != null)
            {
                ilog = index.attempt_no;
                string csql = "select * from tbl_ce_career_evaluation_master where id_ce_career_evaluation_master=" + ceid + " ";
                tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(csql).FirstOrDefault();
                if (cmaster != null)
                {
                    retIndex = index.attempt_no;
                    if (cmaster.validation_period > 0)
                    {
                        DateTime lastattempt = index.dated_time_stamp;
                        lastattempt = lastattempt.AddMonths(Convert.ToInt32(cmaster.validation_period));
                        if (DateTime.Now > lastattempt)
                        {
                            ilog = 1;
                        }
                        else
                        {
                            ilog = 2;
                        }
                    }
                }
            }
            return ilog;
        }
    }

    public class psyCheck
    {
        public bool cFlag { get; set; }
        public int attemptno { get; set; }
        public DateTime nextDate { get; set; }
    }
}