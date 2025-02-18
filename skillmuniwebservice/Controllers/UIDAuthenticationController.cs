using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class UIDAuthenticationController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string USERID)
        {
            string str = " select * from tbl_user where USERID='" + USERID + "' and status='A'";
            tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();

            if (dbuser != null)
            {
                tbl_profile prof = db.tbl_profile.Where(t => t.ID_USER == dbuser.ID_USER).FirstOrDefault();
                tbl_csst_role roles = db.tbl_csst_role.Where(t => t.id_csst_role == dbuser.ID_ROLE).FirstOrDefault();
                tbl_organization ORGTBL = db.tbl_organization.Where(t => t.ID_ORGANIZATION == dbuser.ID_ORGANIZATION).FirstOrDefault();

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
                response.fullname = prof.FIRSTNAME + " " + prof.LASTNAME;
                response.log_flag = new ChangePasswordLogic().CheckFirstLogin(response.UserID);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                string Error = "";
                string strq = "select * from tbl_user where USERID='" + USERID + "'";
                tbl_user dbusercheck = db.tbl_user.SqlQuery(strq).FirstOrDefault();
                if (dbusercheck != null)
                {
                    Error = "Device not Registered with M2OST.Please contact Administrator..";
                }
                else
                {
                    Error = "Invalid Username and Password...";
                }
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