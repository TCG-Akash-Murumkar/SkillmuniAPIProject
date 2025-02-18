using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getStreamListController : ApiController
    {
        public HttpResponseMessage Get(int id_degree)
        {
            List<tbl_stream_master> stream = new List<tbl_stream_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                stream = db.Database.SqlQuery<tbl_stream_master>("select * from tbl_stream_master where id_degree={0} ",id_degree).ToList();

            }

            return Request.CreateResponse(HttpStatusCode.OK, stream);
        }

    }
}
