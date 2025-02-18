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
    public class DocumentUploadController : ApiController
    {
        /// <summary>  
        /// Upload Document.....  
        /// </summary>        
        /// <returns></returns>  
        [HttpPost]
        [Route("api/DocumentUpload/VideoCVUpload")]
        public async Task<HttpResponseMessage> VideoCVUpload()
        {
            CVBuilderResponse Result = new CVBuilderResponse();

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
                string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                //----------------------Database-Update---------------------------
                string VideoExtension = formData["EXTN"];
                int UID = Convert.ToInt32(formData["UID"]);
                int OID = Convert.ToInt32(formData["OID"]);

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                {
                    tbl_cv_master mas = db.Database.SqlQuery<tbl_cv_master>(" select * from tbl_cv_master where id_user ={0} and cv_type={1}", UID, 1).FirstOrDefault();
                    if (mas == null)
                    {
                        int id_cv = db.Database.SqlQuery<int>(" insert into  tbl_cv_master (id_user,oid,created_date,modified_date,status,cv_type) values({0},{1},{2},{3},{4},{5});select max(id_cv) from tbl_cv_master", UID, OID, DateTime.Now, DateTime.Now, "A", 1).FirstOrDefault();


                        db.Database.ExecuteSqlCommand("insert into tbl_video_cv (id_cv,videoname,extn,status) values({0},{1},{2},{3})", id_cv, UID, VideoExtension, "P");


                    }
                    else
                    {

                        db.Database.ExecuteSqlCommand("update tbl_cv_master set modified_date={0} where id_cv={1}", DateTime.Now, mas.id_cv);
                        db.Database.ExecuteSqlCommand("update tbl_video_cv set extn={0} where id_cv={1}", VideoExtension, mas.id_cv);


                    }

                }

                var thisFileName = UID + "." + VideoExtension;

                //----------------------------------------------------------------

                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "Content", "VideoCV");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "VideoCV" + "/";
                URL = DocsPath + thisFileName;




                //Directory.CreateDirectory(@directoryName);  
                using (Stream file = File.OpenWrite(filename))
                {
                    input.CopyTo(file);
                    //close file  
                    file.Close();
                    Result.STATUS = "SUCCESS";
                    Result.RESUMELINK = URL;
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