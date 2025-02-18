using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class B2BAuthenticationPostController : ApiController
    {
        public db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string UID, string password)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LoginResponseAuth response = new LoginResponseAuth();
            try
            {
                //UID = new Utility().mysqlTrim(UID);
                //password = new Utility().mysqlTrim(password);
                password = HttpUtility.UrlDecode(password);
                //string PASSWORD = new AESAlgorithm().getDecryptedString(password);
                tbl_user dbuser = new tbl_user();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                     dbuser = db.Database.SqlQuery<tbl_user>("select * from tbl_user where USERID={0} and PASSWORD={1}",UID,password).FirstOrDefault();

                }
                if (dbuser != null)
                {
                    
                        //tbl_csst_role roles = db.tbl_csst_role.Find(dbuser.ID_ROLE);
                        //tbl_organization ORGTBL = db.tbl_organization.Find(dbuser.ID_ORGANIZATION);
                        if (dbuser.STATUS == "A")
                        {
                            response.ResponseCode = "SUCCESS";
                            response.ResponseAction = 0;
                            response.ResponseMessage = "User successfully registered";
                            response.UserID = Convert.ToInt32(dbuser.ID_USER);
                            response.UserName = dbuser.USERID;
                            response.ROLEID = "1";
                            response.ORGID =Convert.ToString(dbuser.ID_ORGANIZATION); 
                            response.LogoPath = "";
                            response.BannerPath ="";
                            response.ORGEMAIL = "";
                            response.log_flag = 0;
                        tbl_profile pro = new tbl_profile();
                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                        {
                             pro = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}",dbuser.ID_USER).FirstOrDefault();


                        }

                        if (pro != null)
                            {
                                response.fullname = pro.FIRSTNAME + " " + pro.LASTNAME;
                                response.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + pro.PROFILE_IMAGE;
                            }
                            else
                            {
                                response.fullname = "NA";
                                response.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString() + "default.png";

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
                   

                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "User credentials re wrong.";
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

    }
}
