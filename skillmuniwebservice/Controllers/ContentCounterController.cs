using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
{
    public class ContentCounterController : ApiController
    {
            db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
            public HttpResponseMessage Get(int CID,int UID,int FLAG)
            {
                tbl_content_counters counter = new tbl_content_counters();
                counter.id_content = CID;
                counter.id_user = UID;
                counter.flag = FLAG;
                counter.updated_date_time = System.DateTime.Now;
                db.tbl_content_counters.Add(counter);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "1");
            }
    }
}
