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
    public class getBriefResourceDetailController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string brf, int UID, int OID)
        {
            List<APIBrief> list = new List<APIBrief>();
            BriefResource bBody = new BriefResource();

            brf = brf.ToLower().Trim();
            int totalQTN = 0;
            int remQtn = 0;
            string sqlb = "SELECT a.id_organization,a.question_count,a.id_brief_category,a.id_brief_sub_category, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE   LOWER(brief_code) = '" + brf + "' AND a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) LIMIT 10 ";

            sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE  LOWER(brief_code) = '" + brf + "' AND a.status='A' AND  a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp DESC LIMIT 50";

            list = new BriefModel().getAPIBriefList(sqlb);
            if (list.Count > 0)
            {
                bBody = new BriefResource();
                APIBrief brief = list[0];
                tbl_brief_master master = db.tbl_brief_master.Where(t => t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                tbl_brief_master_template mTemplate = db.tbl_brief_master_template.Where(t => t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (mTemplate != null)
                {
                    bBody.brief_template = mTemplate.brief_template;
                }
                else
                {
                    bBody.brief_template = "0";
                }
                totalQTN = brief.question_count;
                bBody.BRIEF = brief;
                List<tbl_brief_master_body> mbody = db.tbl_brief_master_body.Where(t => t.id_brief_master == brief.id_brief_master).OrderBy(t => t.srno).ToList();
                List<BriefRow> bList = new List<BriefRow>();
                foreach (tbl_brief_master_body row in mbody)
                {
                    BriefRow irow = new BriefRow();
                    irow.media_type = Convert.ToInt32(row.media_type);
                    irow.resouce_code = row.resouce_code;
                    irow.resource_order = mTemplate.resource_order;
                    irow.brief_destination = row.brief_destination;
                    irow.resource_number = row.resource_number;
                    irow.srno = Convert.ToInt32(row.srno);
                    irow.resource_type = Convert.ToInt32(row.resource_type);
                    irow.resouce_data = row.resouce_data;
                    irow.resouce_code = row.resouce_code;
                    irow.media_type = Convert.ToInt32(row.media_type);
                    irow.resource_mime = row.resource_mime;
                    irow.file_extension = row.file_extension;
                    irow.file_type = row.file_type;
                    bList.Add(irow);
                }
                bBody.briefResource = bList;
                List<QuestionList> qtnList = new List<QuestionList>();
                List<tbl_brief_question> qList = new List<tbl_brief_question>();

                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == brief.id_brief_master && t.id_user == UID).FirstOrDefault();
                if (log != null)
                {
                    bBody.RESULTSTATUS = 1;
                    bBody.RESULTSCORE = Convert.ToDouble(log.brief_result);
                    BriefReturnResponse response = null;
                    response = JsonConvert.DeserializeObject<BriefReturnResponse>(log.json_response);
                    bBody.RESULT = response;
                    bBody.QTNLIST = null;
                }
                else
                {
                    bBody.RESULTSTATUS = 0;
                    bBody.RESULTSCORE = 0;
                    bBody.RESULT = null;
                    List<int> checkList = new List<int>();

                    string bsql = "SELECT * FROM tbl_brief_question where id_organization=" + OID + " and id_brief_question in (select id_brief_question from  tbl_brief_question_mapping where id_brief_master=" + brief.id_brief_master + " and status='A') and status='A'";
                    qList = db.tbl_brief_question.SqlQuery(bsql).ToList();
                    foreach (tbl_brief_question item in qList)
                    {
                        QuestionList temp = new QuestionList();
                        tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == item.question_complexity).FirstOrDefault();
                        if (comp != null)
                        {
                            temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                            temp.question_complexity_label = comp.question_complexity_label;
                        }
                        temp.question = item;
                        string sqlans = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                        List<tbl_brief_answer> answer = db.tbl_brief_answer.SqlQuery(sqlans).ToList();
                        temp.answers = answer;
                        qtnList.Add(temp);
                        checkList.Add(item.id_brief_question);
                    }
                    remQtn = totalQTN - qList.Count();

                    if (remQtn > 0)
                    {
                        /*category distribution process*/
                        int cattype = Convert.ToInt32(master.brief_type);
                        List<tbl_brief_category> catList = new List<tbl_brief_category>();
                        if (cattype == 0)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category = " + master.id_brief_category + " ";
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }
                        if (cattype == 2)
                        {
                            List<tbl_brief_category_mapping> map = new List<tbl_brief_category_mapping>();
                            map = db.tbl_brief_category_mapping.Where(t => t.id_brief_master == master.id_brief_master).ToList();
                            if (map.Count > 0)
                            {
                                string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_category_mapping where  id_organization=" + OID + " and id_brief_master=" + master.id_brief_master + ") limit " + remQtn;
                                catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                            }
                        }
                        if (cattype == 3)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_question where id_organization=" + OID + ") limit " + remQtn;
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }
                        if (cattype == 1)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_question where id_organization=" + OID + ") limit " + remQtn;
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }

                        int flag = remQtn;
                        List<tbl_brief_question> tempList = new List<tbl_brief_question>();

                        int k = catList.Count();
                        int kl = k * 20;
                        int j = 0;
                        // for (int j = 0; j < kl; j++)
                        do
                        {
                            //  if (flag == 0) { break; }
                            int index = j % k;
                            tbl_brief_category item = catList[index];
                            tbl_brief_question temp = getProgressiveDistributionQuestion(UID, item.id_brief_category, OID);
                            if (temp != null)
                            {
                                if (checkList.Contains(temp.id_brief_question))
                                {
                                }
                                else
                                {
                                    tbl_brief_question tq = tempList.Where(t => t.id_brief_question == temp.id_brief_question).FirstOrDefault();
                                    if (tq == null)
                                    {
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
                            tbl_brief_progdist_mapping mapping = new tbl_brief_progdist_mapping();
                            mapping.id_brief_master = brief.id_brief_master;
                            mapping.id_brief_question = item.id_brief_question;
                            mapping.id_user = UID;
                            mapping.date_time_stamp = System.DateTime.Now;
                            mapping.question_link_type = 1;
                            mapping.status = "A";
                            mapping.updated_date_time = DateTime.Now;
                            db.tbl_brief_progdist_mapping.Add(mapping);
                            db.SaveChanges();

                            QuestionList temp = new QuestionList();
                            tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == item.question_complexity).FirstOrDefault();
                            if (comp != null)
                            {
                                temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                                temp.question_complexity_label = comp.question_complexity_label;
                            }
                            temp.question = item;
                            string sqlans = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                            List<tbl_brief_answer> answer = db.tbl_brief_answer.SqlQuery(sqlans).ToList();
                            temp.answers = answer;
                            qtnList.Add(temp);
                        }
                    }
                    bBody.QTNLIST = qtnList;
                }

                //tbl_brief_read_status rstatus = db.tbl_brief_read_status.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                //if (rstatus != null)
                //{
                //    if (rstatus.read_status == 1)
                //    {
                //        rstatus.read_status = 1;
                //        rstatus.read_datetime = DateTime.Now;
                //        rstatus.updated_date_time = DateTime.Now;
                //        db.SaveChanges();
                //    }
                //}
                //else
                //{
                //    rstatus = new tbl_brief_read_status();
                //    rstatus.id_user = UID;
                //    rstatus.id_brief_master = brief.id_brief_master;
                //    rstatus.read_status = 1;
                //    rstatus.status = "A";
                //    rstatus.action_dateime = null;
                //    rstatus.action_status = 0;
                //    rstatus.read_datetime = DateTime.Now;
                //    rstatus.updated_date_time = DateTime.Now;
                //    db.SaveChanges();
                //}

                tbl_brief_user_assignment urst = db.tbl_brief_user_assignment.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (urst != null)
                {
                    if (urst.scheduled_status == "NA" && urst.published_status == "S")
                    {
                        urst.published_status = "R";
                        urst.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }

                    if (urst.published_status == "NA" && urst.scheduled_status == "S")
                    {
                        urst.scheduled_status = "R";
                        urst.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                else
                {
                }
            }

            if (bBody != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bBody);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, bBody);
            }
        }

        public tbl_brief_question getProgressiveDistributionQuestion(int UID, int CID, int OID)
        {
            tbl_brief_audit lstQtn = new tbl_brief_audit();
            string qsql = "SELECT * FROM tbl_brief_audit WHERE  id_user = " + UID + " AND id_brief_question IN (SELECT id_brief_question FROM tbl_brief_question WHERE  id_organization=" + OID + " and id_brief_category = " + CID + ") ORDER BY id_brief_audit DESC LIMIT 1";
            lstQtn = db.tbl_brief_audit.SqlQuery(qsql).FirstOrDefault();
            bool nextDir = false;

            if (lstQtn != null)
            {
                tbl_brief_question qtn = db.tbl_brief_question.Where(t => t.id_brief_question == lstQtn.id_brief_question).FirstOrDefault();
                if (lstQtn.audit_result == 1)
                {
                    nextDir = true;
                }
                int complexity = getComplecityLevel(CID, nextDir, qtn.question_complexity);
                string newQtnSql = "select * from tbl_brief_question where  id_organization=" + OID + " and  id_brief_question not in (SELECT distinct id_brief_question FROM tbl_brief_audit where id_user =" + UID + " ) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY  RAND() LIMIT 1";
                tbl_brief_question nextQtn = db.tbl_brief_question.SqlQuery(newQtnSql).FirstOrDefault();
                if (nextQtn != null)
                {
                    return nextQtn;
                }
                else
                {
                    string subQtnSql = "select * from tbl_brief_question where  id_organization=" + OID + " and id_brief_question in (SELECT distinct id_brief_question FROM tbl_brief_audit where id_user =" + UID + " AND audit_result=0) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY RAND() LIMIT 1";
                    tbl_brief_question subQtn = db.tbl_brief_question.SqlQuery(subQtnSql).FirstOrDefault();
                    if (subQtn != null)
                    {
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
                string firQtnSql = "SELECT * FROM tbl_brief_question WHERE id_organization=" + OID + " and  id_brief_category =" + CID + " AND status = 'A' AND expiry_date > NOW() ORDER BY question_complexity,RAND()  LIMIT 1";
                tbl_brief_question firQtn = db.tbl_brief_question.SqlQuery(firQtnSql).FirstOrDefault();
                if (firQtn != null)
                {
                    return firQtn;
                }
                else
                {
                    return null;
                }
            }
        }

        public int getComplecityLevel(int CID, bool status, int? level)
        {
            string additional = "";
            if (status)
            {
                additional = "  AND question_complexity > " + level + " order by question_complexity  LIMIT 1 ";
            }
            else
            {
                additional = "  AND question_complexity < " + level + " order by question_complexity desc LIMIT 1 ";
            }
            string sql = "SELECT * FROM tbl_brief_question_complexity WHERE question_complexity IN (SELECT DISTINCT question_complexity FROM tbl_brief_question WHERE id_brief_category = " + CID + ") " + additional;
            tbl_brief_question_complexity levels = db.tbl_brief_question_complexity.SqlQuery(sql).FirstOrDefault();
            if (levels != null)
            {
                return Convert.ToInt32(levels.question_complexity);
            }
            else
            {
                return Convert.ToInt32(level);
            }
        }
    }
}

//image 1
//video 2
//ms word 3
//excel 4
//power point 5
//pdf file 6
//audio 7