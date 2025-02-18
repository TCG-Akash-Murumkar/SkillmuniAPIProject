using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace m2ostnextservice.Controllers
{
    public class getAssessmentCategoryController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int CID,int OID)
        {
            DateTime current = System.DateTime.Now;
            List<tbl_assessment_categoty_mapping> mapping = db.tbl_assessment_categoty_mapping.Where(t => t.id_category == CID).ToList();
            
            List<AssessmentList> list = new List<AssessmentList>();

            foreach (tbl_assessment_categoty_mapping item in mapping)
            {
                AssessmentList tmp = new AssessmentList();
                tbl_assessment_sheet local = db.tbl_assessment_sheet.Find(item.id_assessment_sheet);
                tbl_assessment assessment = db.tbl_assessment.Find(local.id_assesment);
                
                if (DateTime.Compare(assessment.assess_ended.Value.AddDays(1), current) > 0)
                {
                    tmp.id_assessment_sheet = local.id_assessment_sheet;
                    tmp.id_assessment = assessment.id_assessment;
                    tmp.assessment_name = assessment.assessment_title;
                    tmp.assessment_description = assessment.assesment_description;
                    tmp.expiry_date = assessment.assess_ended.Value.ToString("dd-MMM-yyyy");
                    list.Add(tmp);
                }
            }
            

            return Request.CreateResponse(HttpStatusCode.OK, list);

        }
    
    }
}
