using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;
using m2ostnextservice.Models;


namespace m2ostnextservice.Controllers
{
    public class EvaluateAssessmentController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody] AssessmentRequest request)
        {
            tbl_assessment_sheet sheet = db.tbl_assessment_sheet.Where(t => t.id_assesment == request.ASID && t.status == "A").FirstOrDefault();
            AssessmentResponce response = new AssessmentResponce();
            tbl_assessment assessment = db.tbl_assessment.Find(sheet.id_assesment);
            Assessment assess = new Assessment();
            assess.assessment_title = assessment.assessment_title;
            assess.assesment_description = assessment.assesment_description;
            assess.id_assessment = assessment.id_assessment;
            assess.id_organization = Convert.ToInt32(assessment.id_organization);
            assess.assess_type = assessment.assess_type;

            List<Assessment> asList = new List<Assessment>();
            asList.Add(assess);

            tbl_assessment_index index = new tbl_assessment_index();
            index.id_assessment_sheet = sheet.id_assessment_sheet;
            index.id_user = request.UID;
            index.status = "A";
            index.updated_date_time = System.DateTime.Now;
            db.tbl_assessment_index.Add(index);
            db.SaveChanges();
            List<string> qList = request.ASRQ;
            int attampt = new AssessmentModel().getAttamptNo(sheet.id_assessment_sheet, request.UID); //
            List<UserInput> userInput = new List<UserInput>();
            int qCount = 0;
            foreach (string item in qList)
            {
                qCount++;
                UserInput uin = new UserInput();
                string[] content = item.Split('|');//QID|ANS|val                       
                tbl_assessment_question qtn = db.tbl_assessment_question.Find(Convert.ToInt32(content[0]));
                tbl_assessment_answer ans = db.tbl_assessment_answer.Find(Convert.ToInt32(content[1]));
                uin.Question = qtn.assessment_question;
                uin.Answer = ans.answer_description;
                userInput.Add(uin);
                tbl_assessment_audit audit = new tbl_assessment_audit();
                audit.id_assessment = sheet.id_assesment;
                audit.id_assessment_question = Convert.ToInt32(content[0]);
                audit.id_assessment_answer = Convert.ToInt32(content[1]);
                audit.recorded_timestamp = System.DateTime.Now;
                audit.updated_date_time = System.DateTime.Now;
                audit.attempt_no = attampt;
                audit.status = "A";
                audit.id_user = request.UID;
                db.tbl_assessment_audit.Add(audit);
                db.SaveChanges();
            }

            string message = "";// new AssessmentModel().EveluateAssessment(request.ASID, request.UID, attampt);

            response.QuestionAnswer = userInput;
            response.Message = " No Of Questions : " + qCount + "|" + message;
            response.Assessment = asList;
            response.Attempt = attampt.ToString();
            string resJson = JsonConvert.SerializeObject(response);
            //AssessmentResponce ResposeBack = JsonConvert.DeserializeObject<AssessmentResponce>(resJson);

            tbl_assessmnt_log log = new tbl_assessmnt_log();
            log.attempt_no = attampt;
            log.id_assessment_sheet = sheet.id_assessment_sheet;
            log.id_user = request.UID;
            log.json_response = resJson;
            log.status = "A";
            log.updated_date_time = System.DateTime.Now;
            db.tbl_assessmnt_log.Add(log);
            db.SaveChanges();

            scoreAssessment(log);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GET(int UID, int ASID, string ASRQ)
        {

            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // new Utility().eventLog(controllerName + " : ----------------------------------------");
            // new Utility().eventLog("UID : " + UID + " ASID : " + ASID + " : " + ASRQ);

            tbl_assessment_sheet sheet = db.tbl_assessment_sheet.Where(t => t.id_assesment == ASID).FirstOrDefault();
            AssessmentResponce response = new AssessmentResponce();
            tbl_assessment assessment = db.tbl_assessment.Find(sheet.id_assesment);
            bool flag = false;
            // Check the Assessment Group is "1" & Set the Changes Done : 06-10-2016
            if (assessment.assess_group == 1)
            {
                if (assessment.ans_requiered == 1)
                {
                    flag = true;
                }
            }
            // End of Changes 06-10-2016
            Assessment assess = new Assessment();
            assess.assessment_title = assessment.assessment_title;
            assess.assesment_description = assessment.answer_description;
            assess.id_assessment = assessment.id_assessment;
            assess.id_organization = Convert.ToInt32(assessment.id_organization);
            assess.assess_type = assessment.assess_type;

            List<Assessment> asList = new List<Assessment>();
            asList.Add(assess);
            List<tbl_assessment_question> qtnList = db.tbl_assessment_question.Where(t => t.status == "A" && t.id_assessment == assess.id_assessment).ToList();
            tbl_assessment_index index = new tbl_assessment_index();
            index.id_assessment_sheet = sheet.id_assessment_sheet;
            index.id_user = UID;
            index.status = "A";
            index.updated_date_time = System.DateTime.Now;
            db.tbl_assessment_index.Add(index);
            db.SaveChanges();

            string[] qList = ASRQ.Split(';');
            int attampt = new AssessmentModel().getAttamptNo(sheet.id_assessment_sheet, UID); //
            List<UserInput> userInput = new List<UserInput>();
            int qCount = 0;
            List<DataCube> dataCube = new List<DataCube>();

            if (assessment.assess_group == 4)
            {
                foreach (string item in qList)
                {
                    DataCube cube = new DataCube();
                    string[] content = item.Split('|');//QID|ANS|val       

                    cube.QID = content[0];
                    cube.AID = content[1];
                    cube.VAL = content[2];
                    dataCube.Add(cube);

                    tbl_assessment_audit audit = new tbl_assessment_audit();
                    audit.id_assessment = sheet.id_assesment;
                    audit.id_assessment_question = Convert.ToInt32(content[0]);
                    audit.id_assessment_answer = Convert.ToInt32(content[1]);
                    audit.value_sent = Convert.ToInt32(content[2]);
                    audit.recorded_timestamp = System.DateTime.Now;

                    audit.updated_date_time = System.DateTime.Now;
                    audit.attempt_no = attampt;
                    audit.status = "A";
                    audit.id_user = UID;
                    db.tbl_assessment_audit.Add(audit);
                    db.SaveChanges();
                }

                List<List<DataCube>> dataCubeList = new List<List<DataCube>>();
                foreach (tbl_assessment_question item in qtnList)
                {
                    List<DataCube> tList = new List<DataCube>();
                    tList = dataCube.Where(t => t.QID == item.id_assessment_question.ToString()).ToList();
                    dataCubeList.Add(tList);
                }

                foreach (List<DataCube> item in dataCubeList)
                {
                    qCount++;
                    UserInput uin = new UserInput();
                    uin.WANS = "";
                    tbl_assessment_question qtn = db.tbl_assessment_question.Find(Convert.ToInt32(item[0].QID));
                    uin.Question = "Q . " + qtn.assessment_question;
                    tbl_assessment_answer ans = new tbl_assessment_answer();
                    List<string> aList = new List<string>();
                    foreach (DataCube dit in item)
                    {
                        ans = db.tbl_assessment_answer.Find(Convert.ToInt32(dit.AID));
                        aList.Add(ans.answer_description + " [ " + dit.VAL + " ]");
                    }
                    uin.Answer = String.Join("\n", aList);
                    userInput.Add(uin);
                }
            }
            else
            {
                foreach (string item in qList)
                {
                    qCount++;
                    UserInput uin = new UserInput();
                    uin.WANS = "";
                    string[] content = item.Split('|');//QID|ANS|val                       
                    tbl_assessment_question qtn = db.tbl_assessment_question.Find(Convert.ToInt32(content[0]));
                    uin.Question = "Q . " + qtn.assessment_question;
                    tbl_assessment_answer ans = new tbl_assessment_answer();
                    //Validating 
                    int aqno = Convert.ToInt32(qtn.aq_answer);
                    if (flag)
                    {
                        if (!String.IsNullOrEmpty(qtn.aq_answer))
                        {
                            if (aqno > 0)
                            {
                                //display answer
                                tbl_assessment_answer qans = db.tbl_assessment_answer.Find(aqno);
                                uin.WANS = qans.answer_description;
                            }
                        }
                    }

                    if (content[1] == "0")
                    {
                        uin.Answer = " Response Value : " + content[2];
                    }
                    else
                    {
                        ans = db.tbl_assessment_answer.Find(Convert.ToInt32(content[1]));
                        uin.Answer = "-" + ans.answer_description;
                        if (assessment.assess_group == 4)
                        {
                            uin.Answer = "-" + ans.answer_description + " [ " + content[2] + " ]";
                        }
                    }


                    userInput.Add(uin);
                    tbl_assessment_audit audit = new tbl_assessment_audit();
                    audit.id_assessment = sheet.id_assesment;
                    audit.id_assessment_question = Convert.ToInt32(content[0]);
                    audit.id_assessment_answer = Convert.ToInt32(content[1]);
                    audit.value_sent = Convert.ToInt32(content[2]);
                    audit.recorded_timestamp = System.DateTime.Now;
                    audit.updated_date_time = System.DateTime.Now;
                    audit.attempt_no = attampt;
                    audit.status = "A";
                    audit.id_user = UID;
                    db.tbl_assessment_audit.Add(audit);
                    db.SaveChanges();

                    //uncomment       //trep_assessment_eval_temp_1 eval = new trep_assessment_eval_temp_1();
                    //eval.id_organization=assessment.id_organization;
                    //eval.id_user = UID;
                    //eval.id_assessment = assessment.id_assessment;
                    //eval.id_assessnemt_sheet = sheet.id_assessment_sheet;
                    //eval.id_assessment_question = Convert.ToInt32(content[0]);
                    //eval.id_assessment_answer = Convert.ToInt32(content[1]);
                    //if (aqno == Convert.ToInt32(content[1]))
                    //{
                    //    eval.assessment_result = 1;
                    //}
                    //else
                    //{
                    //    eval.assessment_result = 0;
                    //}
                    //eval.result_timestamp = System.DateTime.Now;
                    //eval.attempt_no = attampt;
                    //db.trep_assessment_eval_temp_1.Add(eval);
                    //db.SaveChanges();

                }
            }

            string message = new AssessmentModel().EveluateAssessment(ASID, UID, attampt);

            response.QuestionAnswer = userInput;
            response.Message = "Assessment Summary |Number Of questions         : " + qCount + "|" + message + "|Assessment Details";
            response.Assessment = asList;
            response.Attempt = attampt.ToString();

            string resJson = JsonConvert.SerializeObject(response);
            //AssessmentResponce ResposeBack = JsonConvert.DeserializeObject<AssessmentResponce>(resJson);

            tbl_assessmnt_log log = new tbl_assessmnt_log();
            log.attempt_no = attampt;
            log.id_organization = assessment.id_organization;
            log.id_assessment_sheet = sheet.id_assessment_sheet;
            log.id_user = UID;
            log.json_response = resJson;
            log.status = "A";
            log.updated_date_time = System.DateTime.Now;
            db.tbl_assessmnt_log.Add(log);
            db.SaveChanges();
            scoreAssessment(log);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private void scoreAssessment(tbl_assessmnt_log log)
        {


            int i = 0;

            tbl_rs_type_qna trs = db.tbl_rs_type_qna.Where(t => t.id_assessment_log == log.id_assessmnt_log).FirstOrDefault();
            if (trs == null)
            {

                tbl_assessment_sheet sheet = db.tbl_assessment_sheet.Where(t => t.id_assessment_sheet == log.id_assessment_sheet).FirstOrDefault();
                tbl_assessment assessment = db.tbl_assessment.Find(sheet.id_assesment);
                string responce = "";
                if (sheet.id_assessment_theme == 1)
                {
                    i++;
                    tbl_rs_type_qna qna = new tbl_rs_type_qna();
                    qna.id_user = log.id_user;
                    qna.id_assessment = assessment.id_assessment;
                    qna.id_assessment_log = log.id_assessmnt_log;
                    qna.id_assessment_sheet = sheet.id_assessment_sheet;
                    qna.id_organization = assessment.id_organization;
                    qna.attempt_number = log.attempt_no;
                    qna.status = "A";
                    qna.updated_date_time = System.DateTime.Now;



                    List<tbl_assessment_audit> audit = db.tbl_assessment_audit.Where(t => t.id_assessment == sheet.id_assesment && t.attempt_no == log.attempt_no && t.id_user == log.id_user).ToList();
                    int successCount = 0;
                    int totalCount = 0;
                    double sRate = 0.0;
                    foreach (tbl_assessment_audit item in audit)
                    {

                        tbl_assessment_question qtn = db.tbl_assessment_question.Find(item.id_assessment_question);
                        tbl_assessment_answer ans = db.tbl_assessment_answer.Find(item.id_assessment_answer);
                        if (qtn.aq_answer == ans.id_assessment_answer.ToString())
                        {
                            successCount++;
                        }
                        totalCount++;
                    }
                    if (successCount == 0)
                    {

                    }
                    else
                    {
                        double per = (double)successCount / audit.Count * 100;
                        sRate = Math.Round(per, 2);
                    }
                   
                    qna.total_question = totalCount;
                    qna.right_answer_count = successCount;
                    qna.wrong_answer_count = totalCount - successCount;
                    qna.result_in_percentage = sRate;
                    db.tbl_rs_type_qna.Add(qna);
                    db.SaveChanges();



                }
                else if (sheet.id_assessment_theme == 2)
                {
                   

                }
                else if (sheet.id_assessment_theme == 3)
                {
                    

                }

                else if (sheet.id_assessment_theme == 4)
                {
                    

                }
            }


        }

        private void program_scoring(int aid, int attempt, int uid, int oid)
        {

            sc_game_element_weightage eLog = new sc_game_element_weightage();

            string sqlc = "select * from tbl_category where id_organization=" + oid + " and id_category in (select distinct id_category from tbl_assessment_categoty_mapping where id_assessment=" + aid + " )";
            List<tbl_category> category = db.tbl_category.SqlQuery(sqlc).ToList();

            foreach (tbl_category item in category)
            {
                string check = new ProgramScoringModel().checkProgramComplition(item.ID_CATEGORY, uid, oid);

                if (check == "0")
                {
                    sc_program_content_summary cSummary = new sc_program_content_summary();
                    cSummary.id_category = item.ID_CATEGORY;
                    cSummary.id_organization = oid;
                    cSummary.id_user = uid;

                    string cmapsql = "select count(*) count from tbl_content_organization_mapping where id_category=" + item.ID_CATEGORY + "";
                    int content_count = new ContentReportModel().getRecordCount(cmapsql);
                    int click_count = 0;
                    double percent = 0.0;
                    if (content_count > 0)
                    {
                        string sqlcat = "select count(*) count from tbl_content_organization_mapping where id_category=" + item.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + uid + ")";
                        click_count = new ContentReportModel().getRecordCount(sqlcat);
                        click_count = content_count - click_count;
                    }

                    cSummary.totoal_count = content_count;
                    cSummary.completed_count = click_count;
                    double per = (double)click_count / content_count * 100;
                    cSummary.percentage = Math.Round(per, 2);
                    cSummary.content_weightage = new ProgramScoringModel().getContentWeightage(item.ID_CATEGORY, cSummary.percentage);
                    cSummary.log_datetime = System.DateTime.Now;
                    cSummary.status = "A";
                    cSummary.updated_date_time = System.DateTime.Now;
                    db.sc_program_content_summary.Add(cSummary);
                    db.SaveChanges();

                    tbl_assessment assessment = db.tbl_assessment.Find(aid);
                    if (assessment.assess_group == 1)
                    {
                        string sqla = "select * from tbl_rs_type_qna where id_assessment=" + aid + " and id_user=" + uid + " and id_organization=" + oid + " and attempt_number=" + attempt + " ";
                        tbl_rs_type_qna aresult = db.tbl_rs_type_qna.SqlQuery(sqla).FirstOrDefault();

                        if (aresult != null)
                        {
                            sc_program_assessment_scoring aSummary = new sc_program_assessment_scoring();
                            aSummary.id_assessment = aid;
                            aSummary.id_user = uid;
                            aSummary.id_category = item.ID_CATEGORY;
                            aSummary.id_organization = oid;
                            aSummary.assessment_score = aresult.result_in_percentage;
                            aSummary.assessment_wieghtage = new ProgramScoringModel().getAssessmentWeightage(aid, item.ID_CATEGORY, aresult.result_in_percentage);
                            aSummary.attempt_number = attempt;
                            aSummary.log_datetime = System.DateTime.Now;
                            aSummary.status = "A";
                            aSummary.updated_date_time = System.DateTime.Now;
                            db.sc_program_assessment_scoring.Add(aSummary);
                            db.SaveChanges();
                        }

                    }


                }


            }


        }


    }
}
