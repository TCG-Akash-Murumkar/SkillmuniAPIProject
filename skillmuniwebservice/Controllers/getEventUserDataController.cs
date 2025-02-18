using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getEventUserDataController : ApiController
    {
        public HttpResponseMessage Get(int UID,int OID)
        {
            preferencemodel result = new preferencemodel();

            try
            {
                using (JobDbContext db = new JobDbContext())
                {
                    tbl_user_job_preferences pref = new tbl_user_job_preferences();
                    pref = db.Database.SqlQuery<tbl_user_job_preferences>("select * from  tbl_user_job_preferences where id_user={0} ", UID).FirstOrDefault();
                 
                }




            }
            catch (Exception e)
            {
                throw e;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
