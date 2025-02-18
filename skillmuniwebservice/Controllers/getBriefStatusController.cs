using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBriefStatusController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            BriefStatus status = new BriefStatus();
            status.UID = UID;
            status.OID = OID;

            string sqlq = "SELECT * FROM tbl_brief_read_status WHERE id_user =  " + UID + "  AND id_organization =  " + OID + "  AND status = 'A' AND id_brief_master IN (SELECT id_brief_master FROM tbl_brief_user_assignment WHERE id_user = " + UID + " AND status = 'A')";

            List<tbl_brief_read_status> restatus = db.tbl_brief_read_status.SqlQuery(sqlq).ToList();
            if (restatus.Count() > 0)
            {
                status.TOTALCOUNT = restatus.Count();
                status.READCOUNT = restatus.Where(t => t.read_status == 1).Count();
                status.UNREADCOUNT = restatus.Where(t => t.read_status == 0).Count();
            }
            else
            {
                status.TOTALCOUNT = 0;
                status.READCOUNT = 0;
                status.UNREADCOUNT = 0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }
    }
}