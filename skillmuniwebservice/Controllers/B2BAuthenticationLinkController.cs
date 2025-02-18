using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MySql.Data;
using System.Web.Http;

using System.Net.Mail;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class B2BAuthenticationLinkController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]B2BUser user)
        {
            string reURL = new Utility().mysqlTrim(user.REURL);
            user.USERID = new Utility().mysqlTrim(user.USERID);

            tbl_user dbuser = db.tbl_user.Where(t => t.USERID == user.USERID && t.STATUS == "A").FirstOrDefault();
            if (dbuser != null)
            {
                tbl_csst_role roles = db.tbl_csst_role.Find(dbuser.ID_ROLE);
                tbl_organization ORGTBL = db.tbl_organization.Find(roles.id_organization);

                LoginResponseAuth response = new LoginResponseAuth();
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 0;
                response.ResponseMessage = "User successfully registered";
                response.UserID = Convert.ToInt32(dbuser.ID_USER);
                response.UserName = dbuser.USERID;
                int oid = ORGTBL.ID_ORGANIZATION;
                response.ROLEID = roles.id_csst_role.ToString();
                response.ORGID = oid.ToString();
                response.LogoPath = new RegistrationModel().getOrgLogo(oid);
                response.BannerPath = new RegistrationModel().getOrgBanner(oid);
                response.REURL = reURL;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                string Error = "Invalid Username and Password...";

                LoginResponseAuth response = new LoginResponseAuth();
                response.ResponseCode = "FAILURE";
                response.ResponseAction = 0;
                response.ResponseMessage = Error;
                response.UserID = 0;
                response.UserName = "";
                int oid = 0;
                response.ROLEID = "";
                response.ORGID = oid.ToString();
                response.LogoPath = "";
                response.BannerPath = "";

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }
}