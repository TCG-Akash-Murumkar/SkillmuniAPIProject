using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class GetEmployeeIDController : ApiController
    {
        public HttpResponseMessage Get(int uid)
        {
            db_m2ostEntities db = new db_m2ostEntities();

            tbl_user user = db.tbl_user.Where(t => t.ID_USER == uid).FirstOrDefault();
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user.EMPLOYEEID);
        }
    }
}