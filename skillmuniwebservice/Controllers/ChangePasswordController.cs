using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using m2ostnextservice.Models;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
namespace m2ostnextservice.Controllers
{
    public class ChangePasswordController : ApiController
    {
        public HttpResponseMessage Post([FromBody]Password pswd)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string response = " ";
            try
            {

                //pswd.PASSWORD = PasswordEncryption.ToMD5Hash(pswd.PASSWORD);
                response= new ChangePasswordLogic().changepassword(pswd);
              
            }
            catch (Exception e)
            {
                response = "SOMETHING WENT WRONG. PASSWORD IS  NOT UPDATED.";
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

    }
}
