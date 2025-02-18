using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getNotificationCountController : ApiController
    {
        public HttpResponseMessage Get()
        {
            int count = new EventLogic().getDemoNotificationcount();

            //tbl_version_control versions = db.tbl_version_control.Where(t => t.id_version_control > 0).FirstOrDefault();
           
           
                return Request.CreateResponse(HttpStatusCode.OK, count);
           
        }
    }
}
