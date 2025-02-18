using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class getBriefReadController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int BID, int UID, int OID)
        {
            tbl_brief_master brief = db.tbl_brief_master.Where(t => t.id_brief_master == BID && t.status == "A").FirstOrDefault();
            if (brief == null)
            {
            }
            else
            {
                tbl_brief_read_status rstatus = db.tbl_brief_read_status.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (rstatus != null)
                {
                    if (rstatus.read_status == 0)
                    {
                        rstatus.id_organization = OID;
                        rstatus.read_status = 1;
                        rstatus.read_datetime = DateTime.Now;
                        rstatus.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                else
                {
                    rstatus = new tbl_brief_read_status();
                    rstatus.id_user = UID;
                    rstatus.id_organization = OID;
                    rstatus.id_brief_master = brief.id_brief_master;
                    rstatus.read_status = 1;
                    rstatus.status = "A";
                    rstatus.action_dateime = null;
                    rstatus.action_status = 0;
                    rstatus.read_datetime = DateTime.Now;
                    rstatus.updated_date_time = DateTime.Now;
                    db.tbl_brief_read_status.Add(rstatus);
                    db.SaveChanges();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, OID);
        }
    }
}