using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class getGameContentAssessmentController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_brief_master)
        {
            BriefResource briefData = new BriefResource();
            try
            {
                string brfcode = "";
                using (m2ostnextserviceDbContext dbb = new m2ostnextserviceDbContext())
                {
                    brfcode = dbb.Database.SqlQuery<string>("select brief_code from tbl_brief_master where id_brief_master ={0}",id_brief_master).ToString();
                }


                int _orgid = OID; int _userid = UID;
                string brf = brfcode;
                UserScoreResponse scoreres = new UserScoreResponse();

                //string str = APIString.API + "getBriefResourceDetailWithQuestionTheme?brf=" + brf + "&UID=" + _userid + "&OID=" + _orgid;
                //string sampleJson4 = new UniversityScoringlogic().getApiResponseString(str);
                //BriefResource briefData = JsonConvert.DeserializeObject<BriefResource>(sampleJson4);

                briefData = new BriefModel().getBriefData(brf, _userid, _orgid);


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
               





            }
            catch (Exception e)
            {
                //throw e;
                throw e;
            }


            return Request.CreateResponse(HttpStatusCode.OK, briefData);
        }

    }
}
