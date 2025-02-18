using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Content
{
    public class getColorConfigController : ApiController
    {
        // GET: getColorConfig
        public HttpResponseMessage Get(int orgID)
        {
            List<ColorConfig> response = new List<ColorConfig>();
            WelcomeGif gif = new WelcomeGif();
            response = new ColorConfigLogic().get_color_config(orgID);
            gif = new ColorConfigLogic().get_welcome_gif(orgID);
            return Request.CreateResponse(HttpStatusCode.OK, new{ response= response,gif=gif});
        }
    }
}