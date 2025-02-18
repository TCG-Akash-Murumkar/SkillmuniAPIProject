using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class ForcePasswordChangeBriefController : ApiController
    {
        public HttpResponseMessage Get(int uid, int oid, string password)
        {
            string result = "";
            password = new AESAlgorithm().getEncryptedString(password);
            result = new ChangePasswordLogic().ChangepasswordBrief(uid, oid, password);
            if (result == "You have successfully reset your password")
            {
                new ChangePasswordLogic().UpdateuserLog(uid);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}