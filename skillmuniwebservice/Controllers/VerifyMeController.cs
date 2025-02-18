using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class VerifyMeController : ApiController
    {
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] VerifyMe me)
        {
            Response response = new Response();
            int userID = new RegistrationModel().GetPendingUserID(me.UserName, me.RoleID);
            if (userID != 0)
            {
                int authCodeID = new RegistrationModel().GetAuthcodeIDOfUser(userID);
                string authCode = new RegistrationModel().GetAuthcode(authCodeID);
                if (me.VerificationCode.Equals(authCode))
                {
                    Authcode code = new Authcode();
                    code.AuthCodeID = authCodeID;
                    code.Code = me.VerificationCode;
                    code.Status = "U";
                    int authStatusResult = new RegistrationModel().UpdateAuthcodeStatus(code);
                    if (authStatusResult != 0)
                    {
                        int userUpdateResult = new RegistrationModel().UpdateUserStatus(userID, me.RoleID, "A");
                        if (userUpdateResult != 0)
                        {
                            response.ResponseCode = "SUCCESS";
                            response.ResponseAction = 1;
                            response.ResponseMessage = "User account activated.";
                        }
                        else
                        {
                            response.ResponseCode = "Failure";
                            response.ResponseAction = 0;
                            response.ResponseMessage = "Verification Process Failed. Please try again.";
                        }
                    }
                    else
                    {
                        response.ResponseCode = "Failure";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "Verification Process Failed. Please try again.";
                    }
                }
                else
                {
                    response.ResponseCode = "Failure";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "Invalid authcode. Please try again.";
                }
            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 0;
                response.ResponseMessage = "Could not find user. Please register again.";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}