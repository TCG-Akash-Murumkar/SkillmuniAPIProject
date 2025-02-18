using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBuisinessStagesListController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<tbl_buisiness_stages_master> mas = new List<tbl_buisiness_stages_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                mas = db.Database.SqlQuery<tbl_buisiness_stages_master>("select * from tbl_buisiness_stages_master where status='A' ").ToList();

            }

                return Request.CreateResponse(HttpStatusCode.OK, mas);

        }

    }
}
