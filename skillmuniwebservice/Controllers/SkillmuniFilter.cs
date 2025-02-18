using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Diagnostics;
using System.Web.Routing;
using System.Web.Http.Filters;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class SkillmuniFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
            Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");

        }
    }
}
