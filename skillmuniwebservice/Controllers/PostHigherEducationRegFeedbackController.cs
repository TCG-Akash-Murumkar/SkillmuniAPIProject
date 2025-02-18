using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostHigherEducationRegFeedbackController : ApiController
    {
        public HttpResponseMessage Post([FromBody]HigherEduFeedback High)
        {
            HigherEduResponse result = new HigherEduResponse();
            //int id_register = 0;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    db.Database.ExecuteSqlCommand("update  tbl_sul_higher_education_user_registration set ratings={0} ,  feedback={1} where id_register={2}", High.ratings, High.feedback, High.id_register);



                    result.Message = "Feedback updated successfully.";
                    result.Status = "SUCCESS";




                }



            }
            catch (Exception e)
            {

                result.Message = "Something went wrong.";
                result.Status = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, result);




            }

            // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

    }
}
