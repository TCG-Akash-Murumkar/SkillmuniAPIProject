using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class OrgGameChangePasswordController : ApiController
    {
        public HttpResponseMessage Post([FromBody]PasswordData PostData)
        {
            Response Result = new Response();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {


                    tbl_user userdata = db.Database.SqlQuery<tbl_user>("select * from tbl_user where USERID={0}", PostData.UserID).FirstOrDefault();
                    //string oldpass = new AESAlgorithm().getEncryptedString(PostData.CurrentPassword);
                    if (userdata != null)
                    {
                        //tbl_email_verification_key_log otpmas = db.Database.SqlQuery<tbl_email_verification_key_log>("select * from tbl_email_verification_key_log where id_user={0} and status='P'", userdata.ID_USER).FirstOrDefault();
                        //if (PostData.SecretKey == otpmas.secret_key)
                        //{
                            //string newpass = new AESAlgorithm().getEncryptedString(PostData.NewPassword);


                            db.Database.ExecuteSqlCommand("update  tbl_user set PASSWORD={0} where ID_USER={1}", PostData.NewPassword, userdata.ID_USER);
                            db.Database.ExecuteSqlCommand("update  tbl_user set is_first_time_login=0 where ID_USER={0} ", userdata.ID_USER);



                            Result.ResponseMessage = "Password updated successfully.";
                            Result.ResponseCode = "SUCCESS";




                        //}
                       

                    }

                    else
                    {
                        Result.ResponseMessage = "Entered User Id is wrong.";
                        Result.ResponseCode = "FAILED";

                    }
                   



                }



            }
            catch (Exception e)
            {

                Result.ResponseCode = "FAILED";

                Result.ResponseMessage = "Something went wrong please try after some time.Or else please contact admin.";


                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }

            // return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

    }
}
