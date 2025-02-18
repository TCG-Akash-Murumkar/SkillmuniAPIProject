using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostResumeController : ApiController
    {
        public HttpResponseMessage Post([FromBody]ResumePost Resume)
        {
            PostResumeResponse result = new PostResumeResponse();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string resumename = "";
            try
            {
                byte[] sPDFDecoded = Convert.FromBase64String(Resume.resumeBase);

                //System.IO.File.Create(@"D:\SkillmuniJobFrontEnd\Workspace\SkillmuniJobPortal\SkillmuniJobPortal\Content\Resume\" + Resume.UID + ".pdf");
                if (Resume.type == "pdf")
                {
                   //File.WriteAllBytes(@"C:\SULAPIProduction\Content\Resume\" + Resume.UID + ".pdf", sPDFDecoded);
                    // string pth = ConfigurationManager.AppSettings["Bannerim"].ToString() Resume.UID + ".pdf";
                   File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\Resume\" + Resume.UID + ".pdf", sPDFDecoded);

                    resumename = Resume.UID + "." + Resume.type;
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        db.Database.ExecuteSqlCommand("update tbl_profile set ResumeLocation={0}, ResumeFlag=1 where ID_USER={1} ", resumename = Resume.UID + "." + Resume.type, Resume.UID);
                    }
                }
                else if (Resume.type == "docx")
                {
                   //File.WriteAllBytes(@"C:\SULAPIProduction\Content\Resume\" + Resume.UID + ".docx", sPDFDecoded);
                   File.WriteAllBytes(@"C:\SulAPIBetaV2\Content\Resume\" + Resume.UID + ".docx", sPDFDecoded);


                    resumename = Resume.UID + "." + Resume.type;
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        db.Database.ExecuteSqlCommand("update tbl_profile set ResumeLocation={0},ResumeFlag=1 where ID_USER={1} ", resumename = Resume.UID + "." + Resume.type, Resume.UID);
                    }
                }


            }
            catch (Exception e)
            {
                //new Utility().eventLog(controllerName + " : " + e.Message);
                //new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                //new Utility().eventLog("Additional Details" + " : " + e.Message);
                result.STATUS = "FAILED";
                //return Request.CreateResponse(HttpStatusCode.OK, "FAILED");

                return Request.CreateResponse(HttpStatusCode.OK, result);
                //throw e;
            }
            finally
            {
                result.RESUMELINK = ConfigurationManager.AppSettings["ResumePath"].ToString() + resumename;
                result.STATUS = "SUCCESS";

            }
           // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


    }
}
