using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class logGCMController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody] GCMBODY body)
        {
            APIRESPONSE responce = new APIRESPONSE();
            int uids = Convert.ToInt32(body.UID);
            tbl_user_gcm_log log = db.tbl_user_gcm_log.Where(t => t.GCMID == body.GCM && t.id_user == uids).FirstOrDefault();
            if (log == null)
            {
                tbl_user_gcm_log temp = new tbl_user_gcm_log();
                temp.id_user = uids;
                temp.GCMID = body.GCM.Trim();
                temp.status = "A";
                temp.updated_date_time = System.DateTime.Now;

                db.tbl_user_gcm_log.Add(temp);
                db.SaveChanges();
            }
            responce.KEY = "SUCCESS";
            responce.MESSAGE = "Success";
            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }
}
