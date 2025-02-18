using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;


namespace m2ostnextservice.Controllers
{
    public class getEntrepreneurshipProductListController : ApiController
    {
        public HttpResponseMessage Get(int web_flag)
        {
            List<tbl_social_entrepreneurship_product_master> mas = new List<tbl_social_entrepreneurship_product_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                mas = db.Database.SqlQuery<tbl_social_entrepreneurship_product_master>("select * from tbl_social_entrepreneurship_product_master where status='A' and web_flag={0} ",web_flag).ToList();




            }

            return Request.CreateResponse(HttpStatusCode.OK, mas);

        }

    }
}
