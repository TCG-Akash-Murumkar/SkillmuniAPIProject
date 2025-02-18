
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using m2ostnextservice.Models;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class GetOfflineAnswerStepsController : ApiController
    {

        public HttpResponseMessage Get(int organizationID)
        {
            Response response = new Response();
            List<OfflineAnswerSteps> offlineAnswerStepList = new OfflineAccess().GetAnswerSteps(organizationID);

            if (offlineAnswerStepList != null)
            {
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "AnswerSteps Retrieved.";

            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 1;
                response.ResponseMessage = "No AnswerSteps available.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, offlineAnswerStepList);
        }
   
    }
}
