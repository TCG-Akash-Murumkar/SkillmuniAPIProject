using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getDegreeListController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<tbl_degree_master> degree = new List<tbl_degree_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                degree = db.Database.SqlQuery<tbl_degree_master>("select * from tbl_degree_master where status='A' ").ToList();

            }

            return Request.CreateResponse(HttpStatusCode.OK, degree);
        }
    }
}
