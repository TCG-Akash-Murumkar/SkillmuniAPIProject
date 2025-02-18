using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class VerifyEmailSecretKeyController : ApiController
    {

        public HttpResponseMessage Post([FromBody]VerifyEmailKey PostData)
        {
            VerifyOTPResponse result = new VerifyOTPResponse();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            tbl_sul_fest_master fest = new tbl_sul_fest_master();
            tbl_sul_fest_event_registration reg = new tbl_sul_fest_event_registration();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_email_verification_key_log otpmas = db.Database.SqlQuery<tbl_email_verification_key_log>("select * from tbl_email_verification_key_log where id_user={0} and status='P'", PostData.UID).FirstOrDefault();
                    if (otpmas.secret_key == PostData.SecretKey )
                    {

                        db.Database.ExecuteSqlCommand("update  tbl_email_verification_key_log set status='A' where id_user={0} and status='P' ", PostData.UID);



                        result.Message = "Email verified successfully.";
                        result.Status = "SUCCESS";
                       
                      


                    }
                    else
                    {
                        
                            result.Message = "Entered Key is wrong.";
                            result.Status = "FAILED";

                      

                    }



                }



            }
            catch (Exception e)
            {

                result.Status = "FAILED";
                result.Message = "Something went wrong please try after some time.Or else please contact admin.";


                return Request.CreateResponse(HttpStatusCode.OK, result);

            }

            // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }


       
    }
}
