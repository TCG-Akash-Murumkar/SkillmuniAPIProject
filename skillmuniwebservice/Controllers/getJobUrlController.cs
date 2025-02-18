using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getJobUrlController : ApiController
    {
        public HttpResponseMessage Get()
        {
            JobUrl url = new JobUrl();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                url.Url = db.Database.SqlQuery<string>("select url from tbl_joburl_master where status='A'").FirstOrDefault();
            }
                //url.Url = ConfigurationManager.AppSettings["JobConsoleURL"].ToString();
            return Request.CreateResponse(HttpStatusCode.OK, url);
        }

    }
}
