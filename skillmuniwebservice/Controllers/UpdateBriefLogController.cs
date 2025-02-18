using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateBriefLogController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int id_academy,int id_brief_master,int id_brief_tile)
        {
            tbl_restriction_user_log lg = new tbl_restriction_user_log();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                lg = db.Database.SqlQuery<tbl_restriction_user_log>("select * from tbl_restriction_user_log where UID={0} and id_brief_master={1}",UID,id_brief_master).FirstOrDefault();
                if(lg==null)
                {
                    db.Database.ExecuteSqlCommand("Insert into  tbl_restriction_user_log (UID,OID,id_brief_master,id_academy,updated_date_time,status,id_brief_tile) values({0},{1},{2},{3},{4},{5},{6})", UID,OID,id_brief_master,id_academy,DateTime.Now,"A",id_brief_tile);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

    }
}
