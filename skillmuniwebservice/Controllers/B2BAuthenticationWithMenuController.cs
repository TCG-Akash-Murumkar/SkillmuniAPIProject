using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Http;

using System.Net.Mail;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class B2BAuthenticationWithMenuController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody]B2BUser user)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //new Utility().eventLog(controllerName + " : " + JsonConvert.SerializeObject(user));
            LoginResponseAuth response = new LoginResponseAuth();
            try
            {

                user.PASSWORD = PasswordEncryption.ToMD5Hash(user.PASSWORD);
                //            string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' AND ID_ROLE IN (select ID_ROLE from tbl_role where ID_ORGANIZATION=" + user.ORG_ID + ") AND ID_ROLE=" + user.ROLE_ID + " and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + user.IMEI + "')";
                string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' limit 1";
                tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();
                if (dbuser != null)
                {
                    //tbl_csst_role roles = db.tbl_csst_role.Find(dbuser.ID_ROLE);
                    tbl_organization ORGTBL = db.tbl_organization.Find(dbuser.ID_ORGANIZATION);
                    if (dbuser.STATUS == "A")
                    {
                        response.ResponseCode = "SUCCESS";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "User successfully registered";
                        response.UserID = Convert.ToInt32(dbuser.ID_USER);
                        response.UserName = dbuser.USERID;
                        int oid = ORGTBL.ID_ORGANIZATION;
                        response.ROLEID = "1";
                        response.ORGID = oid.ToString();
                        response.LogoPath = new RegistrationModel().getOrgLogo(oid);
                        response.BannerPath = new RegistrationModel().getOrgBanner(oid);
                        //response.menu_response = new RegistrationModel().get_menu(oid);
                        response.ORGEMAIL = ORGTBL.DEFAULT_EMAIL;

                        if (String.IsNullOrEmpty(user.IMEI))
                        {
                            tbl_report_login_log log = new tbl_report_login_log();
                            log.id_user = dbuser.ID_USER;
                            log.id_organization = dbuser.ID_ORGANIZATION;
                            log.IMEI = "WEBSITE";
                            log.LOG_DATETIME = System.DateTime.Now;
                            db.tbl_report_login_log.Add(log);
                            db.SaveChanges();
                        }
                        else
                        {
                            tbl_report_login_log log = new tbl_report_login_log();
                            log.id_user = dbuser.ID_USER;
                            log.id_organization = dbuser.ID_ORGANIZATION;
                            log.IMEI = user.IMEI;
                            log.LOG_DATETIME = System.DateTime.Now;
                            db.tbl_report_login_log.Add(log);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        string Error = "User account is deactivated. Please contact your administrator.";
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
                        response.ORGEMAIL = "";
                    }

                }
                else
                {
                    string Error = "";
                    string strq = "select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' ";
                    tbl_user dbusercheck = db.tbl_user.SqlQuery(strq).FirstOrDefault();
                    if (dbusercheck != null)
                    {
                        Error = "Device not Registered with M2OST.Please contact Administrator..";
                    }
                    else
                    {
                        Error = "Invalid Username and Password...";
                    }

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
                    response.ORGEMAIL = "";


                }
            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        public HttpResponseMessage Get(string IMEI, string USERID, string PASSWORD, string OS, string Network, string OSVersion, string Details)
        {
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //new Utility().eventLog(controllerName + " : " + JsonConvert.SerializeObject(user));
            PASSWORD = PasswordEncryption.ToMD5Hash(PASSWORD);
            //            string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' AND ID_ROLE IN (select ID_ROLE from tbl_role where ID_ORGANIZATION=" + user.ORG_ID + ") AND ID_ROLE=" + user.ROLE_ID + " and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + user.IMEI + "')";
            string str = " select * from tbl_user where USERID='" + USERID + "' and PASSWORD='" + PASSWORD + "' and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + IMEI + "')";

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
                response.menu_response = new RegistrationModel().get_menu(oid);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                string Error = "";
                string strq = "select * from tbl_user where USERID='" + USERID + "' and PASSWORD='" + PASSWORD + "' ";
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
