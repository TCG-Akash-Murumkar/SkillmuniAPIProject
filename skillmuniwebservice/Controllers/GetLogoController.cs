using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;


namespace m2ostnextservice.Controllers
{
    public class GetLogoController : ApiController
    {
        public HttpResponseMessage Get(int orgID)
        {
            string logo = "";
            logo = new RegistrationModel().getOrgLogo(orgID);          
            return Request.CreateResponse(HttpStatusCode.OK, logo);
           
        }
    }
}
