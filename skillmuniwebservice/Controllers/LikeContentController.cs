using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class LikeContentController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int OID, int CID, int UID, int FLAG)
        {
            tbl_report_content counter = new tbl_report_content();
            counter.ID_CONTENT = CID;
            counter.ID_ORGANIZATION = OID;
            counter.ID_USER = UID;
            counter.CHOICE = FLAG;
            counter.STATUS = "A";
            counter.UPDATED_DATE_TIME = System.DateTime.Now;
            db.tbl_report_content.Add(counter);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "1");
        }
    }
}
