using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class ForcePasswordChangeController : ApiController
    {
        public HttpResponseMessage Get(int uid,int oid)
        {
            string result = "";

            string b_type = new ForcePasswordChangeLogic().checkOrgType(oid);
            if (b_type == "Y")
            {
                result = new ForcePasswordChangeLogic().getGCMStatus(uid);
            }
            else
            {
                result = "SUCCESS";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
            

        }

    }
}
