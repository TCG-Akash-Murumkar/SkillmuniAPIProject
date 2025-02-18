using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateSULProfileImageController : ApiController
    {
        [HttpPost]
        [Route("api/UpdateSULProfileImage/ProfileDPUpdate")]
        public async Task<HttpResponseMessage> ProfileDPUpdate()
        {
            DPUpdateResponse Result = new DPUpdateResponse();

            try
            {
                // Check if the request contains multipart/form-data.  
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;
                //access files  
                IList<HttpContent> files = provider.Files;

                HttpContent file1 = files[0];

                string filename = String.Empty;
                Stream input = await file1.ReadAsStreamAsync();
                string directoryName = String.Empty;
                string URL = String.Empty;
                string tempDocUrl = WebConfigurationManager.AppSettings["DPUrl"];

                //----------------------Database-Update---------------------------
                string ImgExtn = formData["EXTN"];
                int UID = Convert.ToInt32(formData["UID"]);
                int OID = Convert.ToInt32(formData["OID"]);

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                {
                    db.Database.ExecuteSqlCommand("update tbl_profile set PROFILE_IMAGE={0},social_dp_flag={1} where ID_USER={2}",UID+"."+ ImgExtn, 0,UID);

                }

                var thisFileName = UID + "." + ImgExtn;

                //----------------------------------------------------------------

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "Content", "UserDP");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "UserDP" + "/";
                URL = DocsPath + thisFileName;




                //Directory.CreateDirectory(@directoryName);  
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                    Result.STATUS = "SUCCESS";
                    Result.DPLink = URL;
                }


                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";

                return Request.CreateResponse(HttpStatusCode.OK, Result);


            }

        }
    }
}
