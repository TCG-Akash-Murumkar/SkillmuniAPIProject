using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class OrgAssociationController : ApiController
    {
        public HttpResponseMessage Get(int organizationID)
        {
           Response response = new Response();
           List<OfflineOrgAssociation> orgAssociationList = new OfflineAccess().GetOrgAssociation(organizationID);

            if (orgAssociationList != null)
            {
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "Organization Association Retrieved.";
               
            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 1;
                response.ResponseMessage = "No links available.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, orgAssociationList);
        }
    }
}
