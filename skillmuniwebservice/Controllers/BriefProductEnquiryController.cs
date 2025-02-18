using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class BriefProductEnquiryController : ApiController
    {
        public HttpResponseMessage Post(tbl_brief_enquiry enquiry)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Result = "";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO tbl_brief_enquiry ( name, mail, phone, brief_title, enquiry, status, update_date_time) VALUES ( {0}, {1}, {2}, {3}, {4}, {5}, {6})", enquiry.name, enquiry.mail, enquiry.phone, enquiry.brief_title, enquiry.enquiry, "A", DateTime.Now);
                }




            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
                Result = "Failed";
            }
            finally
            {
                Result = "Success";
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }




    }
}
