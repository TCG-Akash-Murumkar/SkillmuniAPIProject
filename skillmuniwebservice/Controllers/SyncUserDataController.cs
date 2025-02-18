using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class SyncUserDataController : ApiController
    {
        public HttpResponseMessage Get(int organizationID, int roleID, string userName, string expiryDate)
        {
            Response response = new Response();
            string result = "";

            bool validity = new SyncModel().CheckSubscription(expiryDate);
            string status = new SyncModel().GetUserStatus(userName, roleID);

            if (validity && status.Equals("A"))
            {
                result = "SUCCESS";
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "User Active";
            }
            else
            {
                result = "FAILURE";
                response.ResponseCode = "FAILURE";
                response.ResponseAction = 1;
                response.ResponseMessage = "User Not Active";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
