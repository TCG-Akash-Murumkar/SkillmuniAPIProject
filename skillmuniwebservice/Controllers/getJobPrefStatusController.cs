using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;

namespace m2ostnextservice.Controllers
{
    public class getJobPrefStatusController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int uid)
        {
            jobPreferranceStatus jps = new jobPreferranceStatus();
            using (m2ostnextserviceDbContext db =new m2ostnextserviceDbContext())
            {
                jps.status = db.Database.SqlQuery<string>("call iSJobPreferenceStatus({0})",uid).FirstOrDefault();
            }

            return Request.CreateResponse(HttpStatusCode.OK,jps);
        }
    }
}
