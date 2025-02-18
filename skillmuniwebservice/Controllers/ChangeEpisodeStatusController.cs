using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class ChangeEpisodeStatusController : ApiController
    {

        public HttpResponseMessage Get(int UID, int OID,int id_brief)
        {
            tbl_episode_log log = new tbl_episode_log();
            string result = "FAILURE";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    log.id_brief_master = id_brief;
                    log.id_user = UID;
                    log.oid = OID;
                    log.status = "U";
                    log.updated_date_time = DateTime.Now;
                    tbl_episode_log te = new tbl_episode_log();
                    te = db.Database.SqlQuery<tbl_episode_log>("select * from tbl_episode_log where id_user={0} and id_brief_master={1}",UID,id_brief).FirstOrDefault();
                    if (te == null)
                    {
                        db.Database.ExecuteSqlCommand("Insert into tbl_episode_log (id_brief_master,id_user,oid,status,updated_date_time) values({0},{1},{2},{3},{4})", log.id_brief_master, log.id_user, log.oid, log.status, log.updated_date_time);


                    }

                    result = "SUCCESS";

                }


            }
            catch (Exception e)
            {
                result = "FAILURE";
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }


            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

    }
}
