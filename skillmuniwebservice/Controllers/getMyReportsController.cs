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
    public class getMyReportsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int UID, string FLAG,int OID,int MID)
        {

            MyReport sheet = new MyReport();
            List<AssessmentReport> learnList = new List<AssessmentReport>();
            List<AssessmentReport> psyList = new List<AssessmentReport>();


                string sql = "select a.* from  tbl_assessment_sheet a,tbl_assessment b where a.id_assessment_sheet in (select distinct id_assessment_sheet from  tbl_assessmnt_log where id_user=" + UID + ") and a.id_assesment=b.id_assessment order by b.assessment_title ";
                List<tbl_assessment_sheet> logAssessment = db.tbl_assessment_sheet.SqlQuery(sql).ToList();

                foreach (tbl_assessment_sheet lItem in logAssessment)
                {
                    List<tbl_assessmnt_log> logs = db.tbl_assessmnt_log.Where(t => t.id_user == UID && t.id_assessment_sheet == lItem.id_assessment_sheet).OrderByDescending(t => t.updated_date_time).ToList();

                    foreach (tbl_assessmnt_log item in logs)
                    {
                        AssessmentReport tmp = new AssessmentReport();
                        tbl_assessment_sheet local = db.tbl_assessment_sheet.Find(item.id_assessment_sheet);
                        tbl_assessment assessment = db.tbl_assessment.Find(local.id_assesment);
                        tmp.id_assessment_log = item.id_assessmnt_log;
                        tmp.id_assessment_sheet = local.id_assessment_sheet;
                        tmp.id_assessment = assessment.id_assessment;
                        tmp.assessment_name = assessment.assessment_title;
                        tmp.assessment_description = assessment.assesment_description;
                        tmp.attempt = item.attempt_no.ToString();
                        tmp.LogDate = item.updated_date_time.Value.ToString("dd-MMM-yyyy HH:mm");
                        if (assessment.assessment_type == 1)
                        {
                            learnList.Add(tmp);
                        }
                        if (assessment.assessment_type == 2)
                        {
                            psyList.Add(tmp);
                        }
                        
                    }
                }
                sheet.LEARNING = learnList;
                sheet.PSYCHOMETRIC = psyList;

            
            return Request.CreateResponse(HttpStatusCode.OK, sheet);
        }
    }
}
