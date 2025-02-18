using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class AssessmentSheetController : ApiController
    {
        APIRESPONSE responce = new APIRESPONSE();
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int ASID, int UID, int OID)
        {
            DateTime current = System.DateTime.Now;
            tbl_assessment_sheet sheets = db.tbl_assessment_sheet.Where(t => t.id_assesment == ASID && t.status == "A").FirstOrDefault();
            if (sheets != null)
            {
                tbl_assessment assessment = db.tbl_assessment.Where(t => t.id_assessment == sheets.id_assesment && t.status == "A").FirstOrDefault(); ;
                if (assessment != null)
                {
                    bool flag = true;
                    DateTime aeDate = (DateTime)assessment.assess_ended;

                    string atimer = aeDate.ToString("HH:mm"); ;
                    if (atimer == "00:00")
                    {
                        aeDate = aeDate.AddDays(1);
                    }

                    int dcv = DateTime.Compare(aeDate, current);
                    if ( dcv > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!flag)
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "The assessment has expired ...";
                        return Request.CreateResponse(HttpStatusCode.OK, responce);
                    }
                 

                    string assSql = "select distinct * from tbl_assessment_user_assignment where id_organization=" + OID + " AND id_user=" + UID + " AND id_assessment=" + assessment.id_assessment + "";
                    tbl_assessment_user_assignment assSheet = db.tbl_assessment_user_assignment.SqlQuery(assSql).FirstOrDefault();
                    if (assSheet != null)
                    {
                        DateTime eDate = (DateTime)assSheet.expire_date;

                        string timer= eDate.ToString("HH:mm"); ;
                        if (timer == "00:00")
                        {
                            eDate = eDate.AddDays(1);
                        }
                        if (DateTime.Compare(eDate, current) > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }

                       
                    }
                    /****************************************************************
                    * Developer : Prashanth                                        *
                    * Change For : To Check the No Attempt taken for an Assessment *
                    * Dated on 06-10-2016                                          *
                    * **************************************************************/
                    if (flag)
                    {


                        int attampt = new AssessmentModel().getAttamptNo(sheets.id_assessment_sheet, UID);

                        if (attampt < assessment.total_attempt)
                        {

                            List<QuestionAnswer> questionAnswer = new List<QuestionAnswer>();

                            List<tbl_assessment_question> question = db.tbl_assessment_question.Where(t => t.id_assessment == sheets.id_assesment && t.status == "A").ToList();
                            foreach (tbl_assessment_question item in question)
                            {
                                QuestionAnswer tmp = new QuestionAnswer();
                                tmp.AssessmenQuestion = new List<AssessmentQuestion>();
                                tmp.AssessmentOption = new List<AssessmentOption>();

                                AssessmentQuestion tQuestion = new AssessmentQuestion();
                                tQuestion.id_assessment_question = item.id_assessment_question;
                                tQuestion.id_organization = Convert.ToInt32(item.id_organization);
                                tQuestion.assessment_question = item.assessment_question;
                                if (!string.IsNullOrEmpty(item.question_image))
                                {
                                    tQuestion.question_image = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "Assessment/" + item.question_image;
                                }
                                else
                                {
                                    tQuestion.question_image = "";
                                }
                                tQuestion.aq_answer = "";
                                tmp.AssessmenQuestion.Add(tQuestion);

                                List<tbl_assessment_answer> answers = db.tbl_assessment_answer.Where(t => t.id_assessment_question == item.id_assessment_question && t.status == "A").ToList();
                                List<AssessmentOption> optionList = new List<AssessmentOption>();
                                foreach (tbl_assessment_answer aitem in answers)
                                {
                                    AssessmentOption option = new AssessmentOption();
                                    option.id_assessment_answer = aitem.id_assessment_answer;
                                    option.id_assessment_question = item.id_assessment_question;
                                    option.answer_description = aitem.answer_description;
                                    optionList.Add(option);
                                }
                                tmp.AssessmentOption = optionList;
                                questionAnswer.Add(tmp);
                            }

                            Assessment assess = new Assessment();
                            assess.assessment_title = assessment.assessment_title;
                            assess.assesment_description = assessment.assesment_description;
                            assess.id_assessment = assessment.id_assessment;
                            assess.id_organization = Convert.ToInt32(assessment.id_organization);
                            assess.assess_type = assessment.assess_group.ToString();
                            if (assessment.assess_group == 3)
                            {
                                assess.low_title = assessment.lower_title;
                                assess.low_value = assessment.lower_value;
                                assess.high_title = assessment.high_title;
                                assess.high_value = assessment.high_value;
                            }
                            else
                            {
                                assess.low_title = "";
                                assess.low_value = "";
                                assess.high_title = "";
                                assess.high_value = "";
                            }


                            List<Assessment> asList = new List<Assessment>();
                            asList.Add(assess);

                            AssessmentSheet sheet = new AssessmentSheet();
                            sheet.Assessment = asList;
                            sheet.QuestionAnswer = questionAnswer;
                            sheet.THEME = Convert.ToInt32(sheets.id_assessment_theme);


                            responce.KEY = "SUCCESS";
                            string resJson = JsonConvert.SerializeObject(sheet);
                            responce.MESSAGE = resJson;

                        }
                        else
                        {
                            responce.KEY = "FAILURE";
                            responce.MESSAGE = "No of attempt exceed";
                        }

                    }
                    else
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "The assessment has expired ";
                    }


                    //------------------------------------------End of code change--------------------------------------------------




                }
                else
                {
                    responce.KEY = "FAILURE";
                    responce.MESSAGE = "Assessment is not found.please contact admin...";
                }
            }
            else
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Invalid Assessmebnt sheet..";
            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);


        }

    }
}
