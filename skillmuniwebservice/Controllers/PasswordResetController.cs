using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PasswordResetController : ApiController
    {
        public HttpResponseMessage Post([FromBody]PasswordChangeData PostData)
        {
            Response Result = new Response();

            try
            {
                tbl_sul_fest_otp otp = new tbl_sul_fest_otp();


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_user userdata = db.Database.SqlQuery<tbl_user>("select * from tbl_user where ID_USER={0}", PostData.UID).FirstOrDefault();
                    //string oldpass = new AESAlgorithm().getEncryptedString(PostData.CurrentPassword);

                    if (PostData.CurrentPassword == userdata.PASSWORD)
                    {
                        //string newpass = new AESAlgorithm().getEncryptedString(PostData.NewPassword);


                        db.Database.ExecuteSqlCommand("update  tbl_user set PASSWORD={0} where ID_USER={1}", PostData.NewPassword, PostData.UID);
                        db.Database.ExecuteSqlCommand("update  tbl_user set is_first_time_login=0 where ID_USER={0} ", PostData.UID);



                        Result.ResponseMessage = "Password updated successfully.";
                        Result.ResponseCode = "SUCCESS";




                    }
                    else
                    {


                        Result.ResponseMessage = "Entered old password is wrong.";
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
