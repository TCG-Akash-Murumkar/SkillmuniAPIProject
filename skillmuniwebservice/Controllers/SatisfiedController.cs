using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class SatisfiedController : ApiController
    {

        public HttpResponseMessage Get(string answerID,int oid,int uid)
        {
            Response response = new Response();
            List<SatisfiedResult> satisfiedResult = new SatisfiedModel().NewGetSupportingData(answerID);

            if (satisfiedResult != null)
            {
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "Links successfully retrieved.";               
            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 1;
                response.ResponseMessage = "No links available.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, satisfiedResult);
        }
    }
}