using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class GetProfileBodyController : ApiController
    {
        public HttpResponseMessage Post([FromBody]GetProfile pro)
        {
            try
            {
                Response response = new Response();
                int userID = new RegistrationModel().GetActiveUserID(pro.USERID, pro.RoleID);
                if (userID != 0)
                {
                    Profile profile =new RegistrationModel().GetActiveUserProfile(userID);
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "Could not find active user.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception e)
            {
                throw (e);               
            }
        }
    }
}
