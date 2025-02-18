using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class CalltoActionController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage GetSteps(int answerID, int orgID, int uid)
        {

            List<NewAnswerSteps> answerSteps = new List<NewAnswerSteps>(); //new SearchModel().NewGetStepsForAnswer(answerID, orgID);

            tbl_content_answer answer = db.tbl_content_answer.Where(t => t.ID_CONTENT_ANSWER == answerID).FirstOrDefault();
            if (answer != null)
            {
                tbl_content content = db.tbl_content.Where(t => t.ID_CONTENT == answer.ID_CONTENT).FirstOrDefault();
                List<tbl_content_answer_steps> steps = new List<tbl_content_answer_steps>();
                steps = db.tbl_content_answer_steps.Where(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER && t.STATUS == "A").ToList();
                
                foreach (tbl_content_answer_steps item in steps)
                {
                    NewAnswerSteps temp = new NewAnswerSteps();
                    temp.ID_ANSWER_STEP = item.ID_ANSWER_STEP;
                    temp.ID_THEME = Convert.ToInt32(item.ID_THEME);
                    temp.STEPNO = item.STEPNO;
                    temp.ANSWER_STEPS_PART1 = item.ANSWER_STEPS_PART1;
                    temp.ANSWER_STEPS_PART2 = item.ANSWER_STEPS_PART2;
                    temp.ANSWER_STEPS_PART3 = item.ANSWER_STEPS_PART3;
                    temp.ANSWER_STEPS_PART4 = item.ANSWER_STEPS_PART4;
                    temp.ANSWER_STEPS_PART5 = item.ANSWER_STEPS_PART5;
                    temp.ANSWER_STEPS_PART6 = item.ANSWER_STEPS_PART6;
                    temp.ANSWER_STEPS_PART7 = item.ANSWER_STEPS_PART7;
                    temp.ANSWER_STEPS_PART8 = item.ANSWER_STEPS_PART8;
                    temp.ANSWER_STEPS_PART9 = item.ANSWER_STEPS_PART9;
                    temp.ANSWER_STEPS_PART10 = item.ANSWER_STEPS_PART10;
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG1))
                    {
                        temp.ANSWER_STEPS_IMG1 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG1;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG1 = "";
                    }

                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG2))
                    {
                        temp.ANSWER_STEPS_IMG2 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG2;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG2 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG3))
                    {
                        temp.ANSWER_STEPS_IMG3 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG3;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG3 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG4))
                    {
                        temp.ANSWER_STEPS_IMG4 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG4;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG4 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG5))
                    {
                        temp.ANSWER_STEPS_IMG5 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG5;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG5 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG6))
                    {
                        temp.ANSWER_STEPS_IMG6 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG6;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG6 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG7))
                    {
                        temp.ANSWER_STEPS_IMG7 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG7;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG7 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG8))
                    {
                        temp.ANSWER_STEPS_IMG8 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG8;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG8 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG9))
                    {
                        temp.ANSWER_STEPS_IMG9 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG9;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG9 = "";
                    }
                    if (!String.IsNullOrEmpty(item.ANSWER_STEPS_IMG10))
                    {
                        temp.ANSWER_STEPS_IMG10 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + "" + content.CONTENT_OWNER + "/" + content.ID_CONTENT.ToString() + "/" + item.ANSWER_STEPS_IMG10;
                    }
                    else
                    {
                        temp.ANSWER_STEPS_IMG10 = "";
                    }
                    temp.ANSWER_STEPS_BANNER = "";
                    temp.REDIRECTION_URL = "";
                    answerSteps.Add(temp);
                }
            }

            if (answerSteps != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, answerSteps);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, answerSteps);
            }
        }
    }
}