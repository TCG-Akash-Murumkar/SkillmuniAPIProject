using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class ChangeProfileImageController : ApiController
    {
        public HttpResponseMessage Post([FromBody]ProfileImageData Prof)
        {
            Response Result = new Response();
            try
            {

                byte[] ProfDecoded = Convert.FromBase64String(Prof.ImageBase);
                File.WriteAllBytes(ConfigurationManager.AppSettings["DpFilePath"].ToString() + Prof.UID + "." + Prof.ImageExtn, ProfDecoded);
                Result.ResponseMessage = "Profile image updated successfully.";
                Result.ResponseCode = "SUCCESS";
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    db.Database.ExecuteSqlCommand("update tbl_profile set PROFILE_IMAGE={0} where ID_USER={1}", Prof.UID + "." + Prof.ImageExtn,Prof.UID);
                }
            }
            catch (Exception e)
            {
                Result.ResponseCode = "FAILED";

                Result.ResponseMessage = "Something went wrong please try after some time.Or else please contact admin.";


                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

    }
}
