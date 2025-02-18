using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getGraduationYearListController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<tbl_graduation_year> grad = new List<tbl_graduation_year> ();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                grad = db.Database.SqlQuery<tbl_graduation_year>("select * from tbl_graduation_year where status='A' ORDER BY graduation_year ASC").ToList();
            }
            //url.Url = ConfigurationManager.AppSettings["JobConsoleURL"].ToString();
            return Request.CreateResponse(HttpStatusCode.OK, grad);
        }

    }
}
