using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBriefVersionController : ApiController
    {
        public HttpResponseMessage Get(string vid)
        {
            db_m2ostEntities db = new db_m2ostEntities();
            tbl_brief_version_control versions= new tbl_brief_version_control() ;// = db.tbl_brief_version_control.Where(t => t.id_brief_version_control > 0).FirstOrDefault();
            using (m2ostnextserviceDbContext dbc=new m2ostnextserviceDbContext())
            {
                versions = dbc.Database.SqlQuery<tbl_brief_version_control>("select * from tbl_brief_version_control where id_brief_version_control > 0").FirstOrDefault();

            }


            if (versions.version_number.ToString().Equals(vid))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "1|Success");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "0|There is an update available.");
            }
        }
    }
}