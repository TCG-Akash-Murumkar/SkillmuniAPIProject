using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;
using m2ostnextservice.Models;
using System.Threading;

namespace m2ostnextservice.Controllers
{
    public class GameScoringController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET: GameScoring
        public HttpResponseMessage GET(int OID)
        {
            string sqlq = "select * from tbl_assessmnt_log where id_organization =" + OID + " ";
            List<tbl_assessmnt_log> summary = db.tbl_assessmnt_log.SqlQuery(sqlq).ToList();
            foreach (tbl_assessmnt_log item in summary)
            {
                string sqla = "select * from tbl_assessment where id_assessment in (select id_assesment from tbl_assessment_sheet where id_assessment_sheet=" + item.id_assessment_sheet + " )";
                tbl_assessment assessment = db.tbl_assessment.SqlQuery(sqla).FirstOrDefault();
                if (assessment != null)
                {
                    string sqlc = "select * from tbl_category where id_organization=" + OID + " and id_category in (select distinct id_category from tbl_assessment_categoty_mapping where id_assessment=" + assessment.id_assessment + " )";
                    List<tbl_category> category = db.tbl_category.SqlQuery(sqlc).ToList();

                    foreach (tbl_category cat in category)
                    {
                        string check = new ProgramScoringModel().checkProgramComplition(cat.ID_CATEGORY, item.id_user, OID);

                        if (check == "0")
                        {
                            sc_program_content_summary cSummary = new sc_program_content_summary();
                            cSummary.id_category = cat.ID_CATEGORY;
                            cSummary.id_organization = OID;
                            cSummary.id_user = item.id_user;

                            string cmapsql = "select count(*) count from tbl_content_organization_mapping where id_category=" + cat.ID_CATEGORY + "";
                            int content_count = new ContentReportModel().getRecordCount(cmapsql);
                            int click_count = 0;
                            double percent = 0.0;
                            if (content_count > 0)
                            {
                                string sqlcat = "select count(*) count from tbl_content_organization_mapping where id_category=" + cat.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + item.id_user + " and  updated_date_time<='" + item.updated_date_time.Value.ToString("yyyy-MM-dd HH:mm:00") + "')";
                                click_count = new ContentReportModel().getRecordCount(sqlcat);
                                click_count = content_count - click_count;
                            }

                            cSummary.totoal_count = content_count;
                            cSummary.completed_count = click_count;
                            double per = (double)click_count / content_count * 100;
                            cSummary.percentage = Math.Round(per, 2);
                            cSummary.content_weightage = new ProgramScoringModel().getContentWeightage(cat.ID_CATEGORY, cSummary.percentage);
                            cSummary.log_datetime = System.DateTime.Now;
                            cSummary.status = "A";
                            cSummary.updated_date_time = System.DateTime.Now;
                            db.sc_program_content_summary.Add(cSummary);
                            db.SaveChanges();
                        }

                        if (assessment.assess_group == 1)
                        {
                            string sqlr = "select * from tbl_rs_type_qna where id_assessment=" + assessment.id_assessment + " and id_user=" + item.id_user + " and id_organization=" + OID + " and attempt_number=" + item.attempt_no + " ";
                            tbl_rs_type_qna aresult = db.tbl_rs_type_qna.SqlQuery(sqlr).FirstOrDefault();

                            if (aresult != null)
                            {
                                sc_program_assessment_scoring aSummary = new sc_program_assessment_scoring();
                                aSummary.id_assessment = assessment.id_assessment;
                                aSummary.id_user = item.id_user;
                                aSummary.id_category = cat.ID_CATEGORY;
                                aSummary.id_organization = OID;
                                aSummary.assessment_score = aresult.result_in_percentage;
                                aSummary.assessment_wieghtage = new ProgramScoringModel().getAssessmentWeightage(assessment.id_assessment, cat.ID_CATEGORY, aresult.result_in_percentage);
                                aSummary.attempt_number = item.attempt_no;
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

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }




    }
}