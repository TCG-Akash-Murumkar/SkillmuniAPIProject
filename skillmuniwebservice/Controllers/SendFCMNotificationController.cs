using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class SendFCMNotificationController : ApiController
    {

        public HttpResponseMessage Get(string GCMID, string message,string type,string image,string league="0")
        {
            new UniversityScoringlogic().SendNotification(GCMID, message, type, image,league);

            return Request.CreateResponse(HttpStatusCode.OK, "S");
        }

    }
}
