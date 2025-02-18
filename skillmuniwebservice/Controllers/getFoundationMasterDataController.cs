using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getFoundationMasterDataController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<tbl_foundation_master> res = new List<tbl_foundation_master>();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                res = db.Database.SqlQuery<tbl_foundation_master>("select * from tbl_foundation_master where status='A'").ToList();
               


            }



            return Request.CreateResponse(HttpStatusCode.OK, res);

        }

    }
}
