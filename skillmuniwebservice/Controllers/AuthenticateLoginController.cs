using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class AuthenticateLoginController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody] B2BUser user)
        {
            user.PASSWORD = PasswordEncryption.ToMD5Hash(user.PASSWORD);
            //            string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' AND ID_ROLE IN (select ID_ROLE from tbl_role where ID_ORGANIZATION=" + user.ORG_ID + ") AND ID_ROLE=" + user.ROLE_ID + " and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + user.IMEI + "')";
            string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + user.IMEI + "')";
            tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();
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
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                string Error = "";
                string strq = "select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' ";
                tbl_user dbusercheck = db.tbl_user.SqlQuery(strq).FirstOrDefault();
                if (dbusercheck != null)
                {
                    Error = "Device not Registered with Cafe Style Culture Club...";
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
