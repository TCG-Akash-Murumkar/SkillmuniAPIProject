using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostUserEnquiryController : ApiController
    {
        public HttpResponseMessage Post([FromBody]tbl_user_enquiry_data obj)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO tbl_user_enquiry_data(id_user,name,mail,phone,enquiry_reason,message,status,updated_date_time) VALUES({0},{1},{2},{3},{4},{5},{6},{7})",obj.id_user,obj.name,obj.mail, obj.phone,obj.enquiry_reason,obj.message,"A",DateTime.Now);
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
