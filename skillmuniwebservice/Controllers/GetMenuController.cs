using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class GetMenuController : ApiController
    {
        public HttpResponseMessage Get(int orgID)
        {
            List<Menu> response = new List<Menu>();
            response = new RegistrationModel().get_menu(orgID);
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }
    }
}
