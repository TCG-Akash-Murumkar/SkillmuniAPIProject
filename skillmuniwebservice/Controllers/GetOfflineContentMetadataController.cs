using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class GetOfflineContentMetadataController : ApiController
    {
        public HttpResponseMessage Get(int organizationID)
        {
            Response response = new Response();
            List<OfflineContentMetadata> offlineContetMetadataList = new OfflineAccess().GetContentMetadata(organizationID);

            if (offlineContetMetadataList != null)
            {
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "Content Metadata Retrieved.";

            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 1;
                response.ResponseMessage = "No links available.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, offlineContetMetadataList);
        }

    }
}
