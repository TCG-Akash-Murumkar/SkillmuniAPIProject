using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostSeminarRegFeedbackController : ApiController
    {
        public HttpResponseMessage Post([FromBody]SemFeedback Sem)
        {
            SemResponse result = new SemResponse();
            //int id_register = 0;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                  db.Database.ExecuteSqlCommand("update  tbl_sul_seminar_user_registration set ratings={0} ,  feedback={1} where id_register={2}", Sem.ratings, Sem.feedback, Sem.id_register);

                  

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
