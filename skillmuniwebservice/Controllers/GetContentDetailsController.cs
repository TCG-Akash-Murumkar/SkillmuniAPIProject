using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class GetContentDetailsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int conId, int userid, int orgid)
        {
            tbl_user user = db.tbl_user.Where(t => t.ID_USER == userid).FirstOrDefault();
            AnswerResponse result = new AnswerResponse();

            ContentInfo contentInfo = new ContentModel().CheckContentAccess(conId, userid, orgid);

            if (contentInfo.status=="A")
            {
                tbl_content_counters counter = new tbl_content_counters();
                counter.id_content = conId;
                counter.id_user = userid;
                counter.flag = 1;
                counter.updated_date_time = System.DateTime.Now;

                db.tbl_content_counters.Add(counter);
                db.SaveChanges();
                tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == conId).FirstOrDefault();
                int count = content.CONTENT_COUNTER;
                count++;
                content.CONTENT_COUNTER = count;
                db.SaveChanges();
                int id_category = 0;

                var answer = db.tbl_content_answer.Where(t => t.ID_CONTENT == conId).ToList();
                tbl_content_answer tbl_answer = (tbl_content_answer)answer[0];
                var relatedQtn = db.tbl_content_link.Where(t => t.ID_CONTENT_PARENT == conId).ToList();
                List<tbl_content_link> linkedQtn = new List<tbl_content_link>();
                List<tbl_content> related = new List<tbl_content>();
                List<tbl_content_metadata> metalist = new List<tbl_content_metadata>();
                List<SearchResponce> relatedResponce = new List<SearchResponce>();
                List<SearchResponce> linkedResponce = new List<SearchResponce>();

                if (relatedQtn.Count > 0)
                {
                    linkedQtn = (List<tbl_content_link>)relatedQtn;
                    foreach (tbl_content_link con in linkedQtn)
                    {
                        SearchResponce res = new SearchResponce();
                        tbl_content cnt = db.tbl_content.Find(con.ID_CONTENT_CHILD);
                        tbl_content_organization_mapping childMap = db.tbl_content_organization_mapping.Where(t => t.id_content == cnt.ID_CONTENT && t.id_organization == orgid).FirstOrDefault();

                        if (childMap != null)
                        {
                            if (cnt.STATUS == "A" && childMap.id_organization.Equals(contentInfo.id_organization))
                            {
                                res.ID_CONTENT = cnt.ID_CONTENT;
                                res.CONTENT_QUESTION = cnt.CONTENT_QUESTION;
                                if(cnt.CONTENT_OWNER==orgid)
                                {
                                    res.ID_CONTENT_LEVEL = cnt.ID_CONTENT_LEVEL;
                                }
                                else
                                {
                                    res.ID_CONTENT_LEVEL = 1;
                                }
                               
                                res.EXPIRYDATE = cnt.EXPIRY_DATE.Value.ToString("dd-mm-yyyy");

                                linkedResponce.Add(res);
                            }
                        }
                    }
                }


                var metadatas = db.tbl_content_metadata.Where(t => t.ID_CONTENT_ANSWER == tbl_answer.ID_CONTENT_ANSWER).ToList();
                if (metadatas.Count > 0)
                {
                    metalist = (List<tbl_content_metadata>)metadatas;
                    List<string> mlist = new List<string>();
                    List<string> mvlist = new List<string>();
                    foreach (tbl_content_metadata meta in metalist)
                    {
                        mlist.Add(meta.ID_CONTENT_ANSWER.ToString().Trim());
                        mvlist.Add(meta.CONTENT_METADATA.Trim());
                    }
                    string value = String.Join(",", mlist);
                    string metavalue = String.Join("|", mvlist);
                    metavalue = metavalue.ToLower();
                    metavalue = metavalue.Replace("'", "\\'");
                    //new Utility().eventLog(metavalue);
                    string sqlQ = "select Distinct * from tbl_content where status='A' AND id_content not in(" + content.ID_CONTENT + ") AND id_content in (select id_content from tbl_content_answer where id_content_answer in (select id_content_answer from tbl_content_metadata where LOWER(content_metadata) REGEXP '" + metavalue + "'))";
                  //  new Utility().eventLog(sqlQ);
                    related = db.tbl_content.SqlQuery(sqlQ).ToList();

                    relatedResponce = new List<SearchResponce>();
                    foreach (tbl_content con in related)
                    {
                        SearchResponce res = new SearchResponce();
                        res.ID_CONTENT = con.ID_CONTENT;
                        res.CONTENT_QUESTION = con.CONTENT_QUESTION;
                        if (con.CONTENT_OWNER == orgid)
                        {
                            res.ID_CONTENT_LEVEL = con.ID_CONTENT_LEVEL;
                        }
                        else
                        {
                            res.ID_CONTENT_LEVEL = 1;
                        }
                        

                        res.EXPIRYDATE = con.EXPIRY_DATE.Value.ToString("dd-mm-yyyy");
                        tbl_content_organization_mapping relMap = db.tbl_content_organization_mapping.Where(t => t.id_content == con.ID_CONTENT && t.id_organization == orgid).FirstOrDefault();
                        if (relMap != null)
                        {


                            if (relMap.id_organization.Equals(contentInfo.id_organization))
                            {
                                relatedResponce.Add(res);
                            }
                        }
                    }
                }


                var feedback = db.tbl_feedback_bank.SqlQuery("select * from tbl_feedback_bank where id_feedback_bank in (select id_feedback_bank from tbl_feedback_bank_link where id_content_answer=" + tbl_answer.ID_CONTENT_ANSWER + ")").FirstOrDefault();
                tbl_feedback_bank feedback_bank = (tbl_feedback_bank)feedback;

                linkedResponce = linkedResponce.OrderBy(t => t.CONTENT_QUESTION).ToList();
                relatedResponce = relatedResponce.OrderBy(t => t.CONTENT_QUESTION).ToList();

                result.ID_CATEGORY = contentInfo.id_category;
                result.ID_CONTENT = content.ID_CONTENT;
                result.ID_THEME = content.ID_THEME;
                result.EXPIRYDATE = content.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                result.ID_CONTENT_ANSWER = tbl_answer.ID_CONTENT_ANSWER;

                result.CONTENT_QUESTION = content.CONTENT_QUESTION;
                result.CONTENT_TITLE = content.CONTENT_HEADER;


                result.CONTENT_ANSWER_TITLE = "";
                result.CONTENT_ANSWER_HEADER = "";
                result.CONTENT_ANSWER1 = tbl_answer.CONTENT_ANSWER1;
                result.CONTENT_ANSWER2 = tbl_answer.CONTENT_ANSWER2;
                result.CONTENT_ANSWER3 = tbl_answer.CONTENT_ANSWER3;
                result.CONTENT_ANSWER4 = tbl_answer.CONTENT_ANSWER4;
                result.CONTENT_ANSWER5 = tbl_answer.CONTENT_ANSWER5;
                result.CONTENT_ANSWER6 = tbl_answer.CONTENT_ANSWER6;
                result.CONTENT_ANSWER7 = tbl_answer.CONTENT_ANSWER7;
                result.CONTENT_ANSWER8 = tbl_answer.CONTENT_ANSWER8;
                result.CONTENT_ANSWER9 = tbl_answer.CONTENT_ANSWER9;
                result.CONTENT_ANSWER10 = tbl_answer.CONTENT_ANSWER10;


                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG1))
                {
                    result.CONTENT_ANSWER_IMG1 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG1;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG1 = "";
                }

                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG2))
                {
                    result.CONTENT_ANSWER_IMG2 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG2;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG2 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG3))
                {
                    result.CONTENT_ANSWER_IMG3 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG3;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG3 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG4))
                {
                    result.CONTENT_ANSWER_IMG4 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG4;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG4 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG5))
                {
                    result.CONTENT_ANSWER_IMG5 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG5;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG5 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG6))
                {
                    result.CONTENT_ANSWER_IMG6 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG6;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG6 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG7))
                {
                    result.CONTENT_ANSWER_IMG7 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG7;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG7 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG8))
                {
                    result.CONTENT_ANSWER_IMG8 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG8;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG8 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG9))
                {
                    result.CONTENT_ANSWER_IMG9 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG9;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG9 = "";
                }
                if (!String.IsNullOrEmpty(tbl_answer.CONTENT_ANSWER_IMG10))
                {
                    result.CONTENT_ANSWER_IMG10 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + tbl_answer.CONTENT_ANSWER_IMG10;
                }
                else
                {
                    result.CONTENT_ANSWER_IMG10 = "";
                }
                              
                result.CONTENT_ANSWER_BANNER = "";
                tbl_content_banner cbanner = db.tbl_content_banner.Where(t => t.id_content == content.ID_CONTENT && t.id_organization == orgid).FirstOrDefault();
                if (cbanner == null)
                {
                    result.CONTENT_BANNER = "";
                    result.CONTENT_BANNER_URL = "";

                }
                else
                {
                    tbl_banner banner = db.tbl_banner.Where(t => t.id_banner == cbanner.id_banner && t.status == "A").FirstOrDefault();
                    if (banner == null)
                    {
                        result.CONTENT_BANNER = "";
                        result.CONTENT_BANNER_URL = "";
                    }
                    else
                    {
                        result.CONTENT_BANNER = banner.banner_name;
                        result.CONTENT_BANNER_URL = banner.banner_action_url;
                        string urls = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Banner/";
                        result.CONTENT_BANNER_IMG = urls + banner.banner_image;
                    }
                }
               
                result.CONTENT_ANSWER_COUNTER = tbl_answer.CONTENT_ANSWER_COUNTER.ToString();
                result.LinkedQuestion = linkedResponce;
                result.RelatedQuestion = relatedResponce;
                if (feedback_bank != null && feedback_bank.ID_FEEDBACK_BANK > 0)
                {
                    result.has_feedback = true;
                    result.ID_FEEDBACK_BANK = feedback_bank.ID_FEEDBACK_BANK;
                    result.FEEDBACK_NAME = feedback_bank.FEEDBACK_NAME;
                    result.FEEDBACK_QUESTION = feedback_bank.FEEDBACK_QUESTION;
                    result.FEEDBACK_CHOICES = feedback_bank.FEEDBACK_CHOICES;
                    result.FEEDBACK_IMAGE = ConfigurationManager.AppSettings["FEEDIMAGE"].ToString() + feedback_bank.ID_FEEDBACK_BANK.ToString() + "/" + feedback_bank.FEEDBACK_IMAGE;
                }
                else
                {
                    result.has_feedback = false;
                    result.ID_FEEDBACK_BANK = 0;
                    result.FEEDBACK_NAME = "_";
                    result.FEEDBACK_QUESTION = "_";
                    result.FEEDBACK_CHOICES = "_";
                    result.FEEDBACK_IMAGE = "_";
                }
                var tbl_step = db.tbl_content_answer_steps.Where(t => t.ID_CONTENT_ANSWER == tbl_answer.ID_CONTENT_ANSWER).ToList();
                if (tbl_step.Count > 0)
                {
                    result.HAS_ANSWER_STEP = true;
                }
                else
                {
                    result.HAS_ANSWER_STEP = false;
                }

                result.ASSESSMENT_FLAG = new AssessmentModel().getAssessmentCheck(content.ID_CONTENT,orgid);
                result.STATUS = "1";
                result.MESSAGE = "Authorization Success.";
            }
            else
            {
                result.STATUS = "0";
                result.MESSAGE = "You are not Authorized to Acces this Content/Activity.";

            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
