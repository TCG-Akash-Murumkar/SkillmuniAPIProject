using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class SignupAttributesValidationController : ApiController
    {
        public HttpResponseMessage Get(int OID)
        {
            List<tbl_signup_config> sign = new List<tbl_signup_config>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                sign = db.Database.SqlQuery<tbl_signup_config>("select * from tbl_signup_config").ToList();

            }
               
             return Request.CreateResponse(HttpStatusCode.OK, sign);
                
        }




    }
}
