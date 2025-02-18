using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class LogNonClosureClauseController : ApiController
    {
        public HttpResponseMessage Post([FromBody]tbl_non_disclousure_clause_log log)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //new Utility().eventLog(controllerName + " : " + JsonConvert.SerializeObject(user));
            string response = "FAILURE";
            try
            {
                response = new NonDisclosureLogic().postLog(response,log);
               
            }
            catch (Exception e)
            {
                
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


    }
}
