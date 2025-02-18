using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getIndustryListController : ApiController
    {

        public HttpResponseMessage Get()
        {
            List<tbl_ce_evaluation_jobindustry> mas = new List<tbl_ce_evaluation_jobindustry>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                mas = db.Database.SqlQuery<tbl_ce_evaluation_jobindustry>("select * from tbl_ce_evaluation_jobindustry where status='A' ").ToList();
            }

            return Request.CreateResponse(HttpStatusCode.OK, mas);

        }
    }
}
