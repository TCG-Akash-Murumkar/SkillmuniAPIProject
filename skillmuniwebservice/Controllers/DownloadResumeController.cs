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
    public class DownloadResumeController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID)//string ENC
        {
            tbl_profile prof = new tbl_profile();
            ResumeResponse Res = new ResumeResponse();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}",UID).FirstOrDefault();
                Res.ResumeFlag = prof.ResumeFlag;
            }
            if (prof.ResumeFlag == 1)
            {

                Res.ResumePath = ConfigurationManager.AppSettings["ResumePath"] + prof.ResumeLocation;
            }

            return Request.CreateResponse(HttpStatusCode.OK,Res);
        }
    }
}
