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
    public class UploadExtraCertificateController : ApiController
    {

        public HttpResponseMessage Post([FromBody]ExtraCertificatePost Certificate)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string certname = "";
            try
            {
                byte[] sPDFDecoded = Convert.FromBase64String(Certificate.CertificateBase);

                //System.IO.File.Create(@"D:\SkillmuniJobFrontEnd\Workspace\SkillmuniJobPortal\SkillmuniJobPortal\Content\Resume\" + Resume.UID + ".pdf");
                if (Certificate.type == "pdf")
                {
                    File.WriteAllBytes(@"C:\SULAPIProduction\Content\Certificate\" + Certificate.UID + ".pdf", sPDFDecoded);
                    certname = Certificate.UID + "." + Certificate.type;
                    using (JobDbContext db = new JobDbContext())
                    {
                        int id_certificate = db.Database.SqlQuery<int>("select id_certificate from   tbl_user_extra_curricular_certificates where id_user={0}", Certificate.UID).FirstOrDefault();
                        if (id_certificate > 0)
                        {

                            db.Database.ExecuteSqlCommand("update   tbl_user_extra_curricular_certificates set  certificate_file={0} , status={1} , updated_date_time={2} where id_user={3} ",  certname, "A", DateTime.Now,Certificate.UID);

                        }
                        else

                        {
                            db.Database.ExecuteSqlCommand("insert into  tbl_user_extra_curricular_certificates (id_user,certificate_file,status,updated_date_time) values({0},{1},{2},{3}) ", Certificate.UID, certname, "A", DateTime.Now);

                        }


                    }
                }
                else if (Certificate.type == "docx")
                {
                    File.WriteAllBytes(@"C:\SULAPIProduction\Content\Certificate\" + Certificate.UID + ".docx", sPDFDecoded);
                    certname = Certificate.UID + "." + Certificate.type;
                    using (JobDbContext db = new JobDbContext())
                    {
                        int id_certificate = db.Database.SqlQuery<int>("select id_certificate from   tbl_user_extra_curricular_certificates where id_user={0}", Certificate.UID).FirstOrDefault();
                        if (id_certificate > 0)
                        {

                            db.Database.ExecuteSqlCommand("update   tbl_user_extra_curricular_certificates set  certificate_file={0} , status={1} , updated_date_time={2} where id_user={3} ", certname, "A", DateTime.Now, Certificate.UID);

                        }
                        else

                        {
                            db.Database.ExecuteSqlCommand("insert into  tbl_user_extra_curricular_certificates (id_user,certificate_file,status,updated_date_time) values({0},{1},{2},{3}) ", Certificate.UID, certname, "A", DateTime.Now);

                        }
                    }
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
