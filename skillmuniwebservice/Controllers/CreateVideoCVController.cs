using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using m2ostnextservice.Models;


namespace m2ostnextservice.Controllers
{
    public class CreateVideoCVController : ApiController
    {

        public HttpResponseMessage Post([FromBody]VideoCVBuilder CVMaster)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CVBuilderResponse Result = new CVBuilderResponse();
            try
            {



                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                    {
                        tbl_cv_master mas= db.Database.SqlQuery<tbl_cv_master>(" select * from tbl_cv_master where id_user ={0} and cv_type={1}", CVMaster.UID,1).FirstOrDefault();
                        if (mas == null)
                        {
                            int id_cv = db.Database.SqlQuery<int>(" insert into  tbl_cv_master (id_user,oid,created_date,modified_date,status,cv_type) values({0},{1},{2},{3},{4},{5});select max(id_cv) from tbl_cv_master", CVMaster.UID, CVMaster.OID, DateTime.Now, DateTime.Now, "A", 1).FirstOrDefault();

                           Byte[] bytes = Convert.FromBase64String(CVMaster.VideoBase);
                          File.WriteAllBytes(@"D:\SkillmuniUniversityService\CVTest\" + CVMaster.UID + "."+CVMaster.EXTN, bytes);

                        //File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\VideoCV\" + CVMaster.UID + "."+CVMaster.EXTN, bytes);
                        db.Database.ExecuteSqlCommand("insert into tbl_video_cv (id_cv,videoname,extn,status) values({0},{1},{2},{3})", id_cv,CVMaster.UID,CVMaster.EXTN,"P");


                    }
                    else
                        {
                        Byte[] bytes = Convert.FromBase64String(CVMaster.VideoBase);

                        //var uri = new System.Uri("c:\\foo");
                        //var converted = uri.AbsoluteUri;
                        //Upload(CVMaster.VideoString). ContinueWith(t => Console.WriteLine(t.Exception),TaskContinuationOptions.OnlyOnFaulted);

                        // File.WriteAllBytes(@"D:\SkillmuniUniversityService\CVTest\" + CVMaster.UID + "." + CVMaster.EXTN, bytes);

                        File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\VideoCV\" + CVMaster.UID + "." + CVMaster.EXTN, bytes);
                        db.Database.ExecuteSqlCommand("update tbl_cv_master set modified_date={0} where id_cv={1}", DateTime.Now, mas.id_cv);
                        db.Database.ExecuteSqlCommand("update tbl_video_cv set extn={0} where id_cv={1}",CVMaster.EXTN, mas.id_cv);


                    }

                }

               
            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            finally
            {
                Result.STATUS = "SUCCESS";
                Result.RESUMELINK = ConfigurationManager.AppSettings["vidcv"].ToString() + CVMaster.UID+"."+CVMaster.EXTN;

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        public static async Task<string> Upload(byte[] image)
        {
            using (var client = new HttpClient())
            {
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new MemoryStream(image)), "bilddatei", "upload.jpg");

                    using (
                       var message =
                           await client.PostAsync("http://180.149.241.40/sulapibetav2/Content/VideoCV", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;
                    }
                }
            }
        }



                     


    }
}
