using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBriefResultStatusController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            BriefScore status = new BriefScore();
            status.UID = UID;
            status.OID = OID;

            string usql = "SELECT * FROM tbl_brief_user_assignment WHERE id_user = " + UID + " AND id_brief_master IN (SELECT id_brief_master FROM tbl_brief_master WHERE id_organization = " + OID + " AND status = 'A')";
            List<tbl_brief_user_assignment> uList = db.tbl_brief_user_assignment.SqlQuery(usql).ToList();

            if (uList.Count > 0)
            {
                status.TOTALCOUNT = uList.Count();
                List<tbl_brief_log> bLog = db.tbl_brief_log.Where(t => t.id_organization == OID && t.attempt_no == 1 && t.id_user == UID).ToList();
                int lCount = 0; double? bAvg = 0;
                if (bLog.Count() > 0)
                {
                    lCount = bLog.Count();
                    bAvg = bLog.Average(t => t.brief_result);
                    if (bAvg != null)
                    {
                    }
                }
                status.BRIEFTAKEN = lCount;
                status.BRIEFSCORE = Convert.ToInt32(bAvg);
            }
            else
            {
                status.TOTALCOUNT = 0;
                status.BRIEFSCORE = 0;
                status.BRIEFTAKEN = 0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, status);
        }
    }
}