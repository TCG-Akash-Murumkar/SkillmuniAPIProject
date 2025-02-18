
using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class UpdateUserProfileController : ApiController
    {

        public HttpResponseMessage Get(string username, int roleID,int orgid,int uid)
        {
            try
            {
                Response response = new Response();
                int userID = new RegistrationModel().GetActiveUserID(username, roleID);
                if (userID != 0)
                {
                    Profile profile = new RegistrationModel().GetActiveUserProfile(userID);
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "Could not find active user.";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception e)
            {
                throw (e);
                //return Request.CreateResponse(HttpStatusCode.OK, e);
            }
        }
        public HttpResponseMessage Post([FromBody]Registration profile)
        {

            Response response = new Response();
            int userID = new RegistrationModel().GetActiveUserID(profile.UserName, profile.Role);
            if (userID != 0)
            {
                Profile tProfile = new RegistrationModel().GetActiveUserProfile(userID);
                if (tProfile.Age == 0)
                {
                    int profileResult = new RegistrationModel().UpdateUserProfile(profile, userID);
                    if (profileResult == 1)
                    {
                        response.ResponseCode = "SUCCESS";
                        response.ResponseAction = 1;
                        response.ResponseMessage = "User profile updates successfully.";
                    }
                    else
                    {
                        response.ResponseCode = "FAILURE";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "Could not update user profile. Please try again.";
                    }
                }
                else
                {
                    response.ResponseCode = "SUCCESS";
                    response.ResponseAction = 1;
                    response.ResponseMessage = "User profile updates successfully.";
                }
            }
            else
            {
                response.ResponseCode = "FAILURE";
                response.ResponseAction = 0;
                response.ResponseMessage = "Could not find Active User. Please register again.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}