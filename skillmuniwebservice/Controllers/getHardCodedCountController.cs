using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getHardCodedCountController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int AcadamyTileId)//string ENC
        {
            BriefCountResponse Count = new BriefCountResponse();
            Count.ReadCount = 1;
            Count.UnReadCount = 2;
            Count.TOTALCOUNT = 3;

            
                return Request.CreateResponse(HttpStatusCode.OK, Count);
           
        }

    }
}
