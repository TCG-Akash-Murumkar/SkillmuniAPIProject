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
    public class AssessmentListController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int CID, int UID, int OID)
        {
            DateTime current = System.DateTime.Now;
            AssessmentSheet sheet = new AssessmentSheet();
            List<AssessmentList> list = new List<AssessmentList>();
            List<tbl_assessment_mapping> mapping = db.tbl_assessment_mapping.Where(t => t.id_content == CID && t.id_organization == OID).ToList();

            foreach (tbl_assessment_mapping item in mapping)
            {
                AssessmentList tmp = new AssessmentList();
                tbl_assessment_sheet local = db.tbl_assessment_sheet.Where(t => t.status == "A" && t.id_organization == OID && t.id_assessment_sheet == item.id_assessment_sheet).FirstOrDefault();
                if (local != null)
                {
                    tbl_assessment assessment = db.tbl_assessment.Where(t => t.status == "A" && t.id_organization == OID && t.id_assessment == local.id_assesment).FirstOrDefault();
                    if (assessment != null)
                    {
                        DateTime tdate = assessment.assess_ended.Value.AddDays(1);
                        if (DateTime.Compare(tdate, current) > 0)
                        {
                            if (assessment.status == "A")
                            {
                                tmp.id_assessment_sheet = local.id_assessment_sheet;
                                tmp.id_assessment = assessment.id_assessment;
                                tmp.assessment_name = assessment.assessment_title;
                                tmp.assessment_description = assessment.assesment_description;
                                tmp.expiry_date = assessment.assess_ended.Value.ToString("dd-MMM-yyyy");
                                list.Add(tmp);
                            }
                        }
                    }
                }
                //tbl_assessment_question question = db.tbl_assessment_question.Find(local.id_assessment_question);                
                //List<tbl_assessment_answer> answers = db.tbl_assessment_answer.Where(t => t.id_assessment_question == local.id_assessment_question).ToList();

            }



            return Request.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}
