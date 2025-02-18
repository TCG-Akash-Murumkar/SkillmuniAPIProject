using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class CreateResumeController : ApiController
    {

        public HttpResponseMessage Post([FromBody]CreateResumeDetails CV)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string resumename = "";
            try
            {
                if (CV.ProfilePicture != null)
                {
                    byte[] ProfDecoded = Convert.FromBase64String(CV.ProfilePicture);
                    File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\CVProfile\" + CV.UID + "."+CV.ProfilePictureEXTN, ProfDecoded);

                }





            }
            catch (Exception e)
            {
                //new Utility().eventLog(controllerName + " : " + e.Message);
                //new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                //new Utility().eventLog("Additional Details" + " : " + e.Message);
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
                //throw e;
            }
            finally
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

    }
}
