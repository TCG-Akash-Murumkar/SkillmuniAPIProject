using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBriefCompletionListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            string sqll = "SELECT b.brief_code,a.id_user,a.id_brief_master, b.brief_title, CASE WHEN a.brief_result IS NULL THEN 0 ELSE a.brief_result END brief_result,a.attempt_no, c.FIRSTNAME FROM tbl_brief_log a, tbl_brief_master b, tbl_profile c WHERE a.id_brief_master = b.id_brief_master AND a.id_user = c.ID_USER AND a.id_organization=" + OID + " AND a.id_user=" + UID + " and b.status='A' order by id_brief_log desc limit 20";
            List<BriefCollection> list = new BriefModel().getUserTestResult(sqll);

            if (list != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, list);
            }
        }
    }
}