using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateUserLogController : ApiController
    {
        public HttpResponseMessage Get(int UID,int pageId)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Result = "";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {


                    if (pageId == 1)//academic_tiles
                    {

                        db.Database.ExecuteSqlCommand("update  tbl_user_log_master set academic_tiles=academic_tiles+1 where id_user={0}", UID);



                    }
                    else if (pageId == 2)//study_abroad
                    {
                        db.Database.ExecuteSqlCommand("update  tbl_user_log_master set study_abroad=study_abroad+1 where id_user={0}", UID);


                    }
                    else if (pageId == 3)//job
                    {
                        db.Database.ExecuteSqlCommand("update  tbl_user_log_master set job=job+1 where id_user={0}", UID);


                    }
                    else if (pageId == 4)//entrepreneurship
                    {
                        db.Database.ExecuteSqlCommand("update  tbl_user_log_master set entrepreneurship=entrepreneurship+1 where id_user={0}", UID);


                    }
                }


            }
            catch (Exception e)
            {
                Result = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            finally
            {
                Result = "SUCCESS";
                //Result.RESUMELINK = ConfigurationManager.AppSettings["CVControl"].ToString() + CVMaster.id_cv;

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

    }
}
