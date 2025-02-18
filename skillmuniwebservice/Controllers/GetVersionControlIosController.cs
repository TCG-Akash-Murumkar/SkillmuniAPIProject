using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class GetVersionControlIosController : ApiController
    {
        public HttpResponseMessage Get(string vid)
        {
            db_m2ostEntities db = new db_m2ostEntities();
           
            string response = new RegistrationModel().get_version(vid);
            APIRESPONSE result = new APIRESPONSE();

            //tbl_version_control versions = db.tbl_version_control_ios.Where(t => t.id_version_control > 0).FirstOrDefault();
            if (response== vid)
            {
                result.KEY = "Success";
                result.MESSAGE = "";
                //return Request.CreateResponse(HttpStatusCode.OK, "1|Success");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                result.KEY = "Failure";
                result.MESSAGE = "There is an update available";
                //return Request.CreateResponse(HttpStatusCode.OK, "0|There is an update available.");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

        }
    }
}
