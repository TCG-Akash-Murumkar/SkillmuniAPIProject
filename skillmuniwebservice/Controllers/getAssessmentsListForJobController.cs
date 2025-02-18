using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getAssessmentsListForJobController : ApiController
    {
        public HttpResponseMessage Get() 
        {
            List<JobAssessments> Result = new List<JobAssessments>();




            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    
                    Result= db.Database.SqlQuery<JobAssessments>("select * from tbl_ce_career_evaluation_master where status={0}", "A").ToList();

                    foreach (var itm in Result)
                    {
                        itm.Cat= db.Database.SqlQuery<AssessmentCategory>("SELECT tbl_ce_category_mapping.id_ce_category_mapping, tbl_brief_category.brief_category, tbl_brief_category.id_brief_category FROM tbl_ce_category_mapping INNER JOIN tbl_brief_category ON tbl_ce_category_mapping.id_brief_category = tbl_brief_category.id_brief_category where tbl_ce_category_mapping.id_ce_career_evaluation_master={0} and tbl_ce_category_mapping.status={1}",itm.id_ce_career_evaluation_master, "A").ToList();
                    }

                }






            }
            catch (Exception e)
            {

            }


            return Request.CreateResponse(HttpStatusCode.OK, Result);


        }

    }
}
