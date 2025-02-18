using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostEntrepreneurshipController : ApiController
    {
        public HttpResponseMessage Post(tbl_entrepreneurship_master ent)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            entrepreneurship_response Result = new entrepreneurship_response();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Result.id_entrepreneurship = db.Database.SqlQuery<int>("INSERT INTO tbl_entrepreneurship_master ( company_name, founders, foundation_date, reason, id_buisiness_stage, revenue, far_from_launch, company_structure, buisiness_stage_others, updated_date_time, product_code, website,id_user) VALUES ( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12});select max(id_entrepreneurship) from tbl_entrepreneurship_master",ent.company_name,ent.founders,ent.foundation_date,ent.reason,ent.id_buisiness_stage,ent.revenue,ent.far_from_launch,ent.company_structure,ent.buisiness_stage_others,DateTime.Now,ent.product_code,ent.website,ent.id_user).FirstOrDefault();

                }
               



            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
                Result.status = "Failed";
            }
            finally
            {
                Result.status = "Success";
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }


    }
}
