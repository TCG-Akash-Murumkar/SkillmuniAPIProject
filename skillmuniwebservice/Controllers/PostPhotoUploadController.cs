using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;



using System.Web.Mvc;
using m2ostnextservice.Models;

// Tagged Photo Upload User based with there respective Location / Zone
namespace m2ostnextservice.Controllers
{
    public class PostPhotoUploadController : ApiController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/PostPhotoUpload/TagPhotoUpload")]
        public async Task<HttpResponseMessage> TagPhotoUpload()
        {
            TagPhotoUploadResponse Result = new TagPhotoUploadResponse();
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
                string tempDocUrl = WebConfigurationManager.AppSettings["TagImage"];

                DateTime currdate = Convert.ToDateTime(DateTime.Now);
                //----------------------Database-Update---------------------------
                string ImgExtn = formData["EXTN"];
                int UID = Convert.ToInt32(formData["UID"]);
                int OID = Convert.ToInt32(formData["OID"]);
                int GCI = Convert.ToInt32(formData["GCI"]);
                int Level = Convert.ToInt32(formData["Level"]);
                string id_lati = formData["LATI"];
                string id_long = formData["LONGI"];
                string thisFileName = UID + currdate.ToString("ddMMyyyyHHmm") + "." + ImgExtn;

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO tbl_tag_photo_upload(id_org,id_user, id_game_content, id_level, photo_filename, id_lati,id_long,updated_time, status) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                    OID, UID, GCI, Level, thisFileName, id_lati, id_long, DateTime.Now, "A");
                }

                //----------------------------------------------------------------
                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "Content", "TagedImage");
                filename = System.IO.Path.Combine(directoryName, thisFileName);
                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                string DocsPath = tempDocUrl + "/" + "/";
                URL = DocsPath + thisFileName;
                //Directory.CreateDirectory(@directoryName);  
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                    Result.STATUS = "SUCCESS";
                    Result.Taglink = URL;
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

