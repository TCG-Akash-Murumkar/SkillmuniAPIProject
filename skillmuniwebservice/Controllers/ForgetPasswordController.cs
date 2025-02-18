using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class ForgetPasswordController : ApiController
    {
        public HttpResponseMessage Get(string userid)
        {
            db_m2ostEntities db = new db_m2ostEntities();

            tbl_user user = new ForgetPasswordLogic().getUSER(userid);
            if (user!=null)
            {
                tbl_profile profile = new ForgetPasswordLogic().getProfile(user.ID_USER);
                if (profile.EMAIL != null)
                {

                    string result= new ForgetPasswordLogic().TriggerMail(profile,user);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Mail Id is not updated with your profile. Please contact Admin Team.");
                }
                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "UserId is wrong.Try again with correct USERID or  Contact Admin Team");
            }

        }

    }
}
