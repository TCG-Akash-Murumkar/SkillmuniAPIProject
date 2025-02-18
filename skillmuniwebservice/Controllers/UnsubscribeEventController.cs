using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UnsubscribeEventController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string userid, int id_event)
        {
            tbl_user user = db.tbl_user.Where(t => t.USERID == userid).FirstOrDefault();

            string result = new EventLogic().UnSubscribeToEvent(user.ID_USER, id_event);





            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
