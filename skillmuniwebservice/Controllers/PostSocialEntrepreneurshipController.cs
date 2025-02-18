using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostSocialEntrepreneurshipController : ApiController
    {
        public HttpResponseMessage Post([FromBody]tbl_social_entrepreneurship ent)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            entrepreneurship_response Result = new entrepreneurship_response();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                   int id_social_entrepreneurship= db.Database.SqlQuery<int>("INSERT INTO tbl_social_entrepreneurship ( name, phone, email, message, website, updated_date_time) VALUES ( {0}, {1}, {2}, {3}, {4}, {5});select max(id_social_entrepreneurship) from tbl_social_entrepreneurship", ent.name, ent.phone, ent.email, ent.message, ent.website, DateTime.Now).FirstOrDefault();
                    string[] values = ent.map.Split(',');
                    foreach (var itm in values)
                    {

                        db.Database.ExecuteSqlCommand("INSERT INTO tbl_social_entrepreneurship_product_mapping ( id_social_entrepreneurship, id_product, updated_date_time) VALUES ( {0}, {1}, {2});", id_social_entrepreneurship,itm, DateTime.Now);
                    }
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
