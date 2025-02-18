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
    public class PostEntrepreneurshipFilesController : ApiController
    {
        public HttpResponseMessage Post([FromBody]EntPost Ent)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                int i = 1;
                foreach (var itm in Ent.files)
                {
                    byte[] sPDFDecoded = Convert.FromBase64String(itm.Base);

                    //System.IO.File.Create(@"D:\SkillmuniJobFrontEnd\Workspace\SkillmuniJobPortal\SkillmuniJobPortal\Content\Resume\" + Resume.UID + ".pdf");
                  
                        File.WriteAllBytes(@"C:\SULAPIProduction\Content\EntFile\Ent" + Ent.id_entrepreneurship+i + "."+itm.type, sPDFDecoded);
                       string filename ="Ent"+ Ent.id_entrepreneurship+i + "." + itm.type;
                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_entrepreneurship_files (file,extension,id_entrepreneurship,updated_date_time) values({0},{1},{2},{3})", filename,itm.type,Ent.id_entrepreneurship,DateTime.Now);
                        }

                    i++;
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
