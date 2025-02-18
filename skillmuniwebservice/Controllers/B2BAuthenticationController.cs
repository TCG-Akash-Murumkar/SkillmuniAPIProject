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
using System.Web.Http.Cors;
using System.Web;

namespace m2ostnextservice.Controllers
{
    //[EnableCors(origins: "http://www.thegamificationcompany.com", headers: "*", methods: "*")]
    public class B2BAuthenticationController : ApiController
    {
        
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]B2BUser user)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LoginResponseAuth response = new LoginResponseAuth();

            try
            {
                user.USERID = new Utility().mysqlTrim(user.USERID);

                user.PASSWORD = HttpUtility.UrlDecode(user.PASSWORD);
                tbl_user dbuser = db.tbl_user.Where(t => t.USERID == user.USERID && t.STATUS == "A").FirstOrDefault();
                if (dbuser != null)
                {
                    //string PASSWORD = new AESAlgorithm().getDecryptedString(dbuser.PASSWORD);
                    if (dbuser.PASSWORD == user.PASSWORD)
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
                            response.ORGEMAIL = ORGTBL.DEFAULT_EMAIL;
                            response.log_flag = new ChangePasswordLogic().CheckFirstLogin(response.UserID);
                            tbl_profile prof = new tbl_profile();
                            using (m2ostnextserviceDbContext dbb = new m2ostnextserviceDbContext())
                            {
                                response.is_first_time_login = dbb.Database.SqlQuery<int>("select is_first_time_login from tbl_user where ID_USER={0} ", dbuser.ID_USER).FirstOrDefault();
                                prof = dbb.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", dbuser.ID_USER).FirstOrDefault();

                                if (response.is_first_time_login == 1)
                                {
                                    string OTP = RandomString(4);
                                    string mail = new AESAlgorithm().getDecryptedString(prof.EMAIL);
                                    tbl_email_verification_key_log ver_key = dbb.Database.SqlQuery<tbl_email_verification_key_log>("select * from tbl_email_verification_key_log where id_user={0} and status='P' ", dbuser.ID_USER).FirstOrDefault();
                                    if (ver_key == null)
                                    {
                                        dbb.Database.ExecuteSqlCommand("insert into tbl_email_verification_key_log (id_user,key,updated_date_time,status) values({0},{1},{2},{3})", dbuser.ID_USER, OTP, DateTime.Now, "P");


                                    }
                                    else
                                    {
                                        OTP = ver_key.secret_key;
                                    }
                                    SendOTP(mail, prof.FIRSTNAME + " " + prof.LASTNAME, OTP);

                                }
                            }
                            tbl_profile pro = db.tbl_profile.Where(t => t.ID_USER == dbuser.ID_USER).FirstOrDefault();
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

        public HttpResponseMessage Get(string USERID="", string PASSWORD="", string IMEI = "", string OS ="", string Network="", string OSVersion="", string Details="")
        {
            
            LoginResponseAuth response = new LoginResponseAuth();

            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //new Utility().eventLog(controllerName + " : " + JsonConvert.SerializeObject(user));
            //PASSWORD = PasswordEncryption.ToMD5Hash(PASSWORD);
            //PASSWORD = new AESAlgorithm().getDecryptedString(PASSWORD);
            //            string str = " select * from tbl_user where USERID='" + user.USERID + "' and PASSWORD='" + user.PASSWORD + "' AND ID_ROLE IN (select ID_ROLE from tbl_role where ID_ORGANIZATION=" + user.ORG_ID + ") AND ID_ROLE=" + user.ROLE_ID + " and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + user.IMEI + "')";
            //string str = " select * from tbl_user where USERID='" + USERID + "' and PASSWORD='" + PASSWORD + "' and ID_USER in (select ID_USER from tbl_user_device_link where DEVICE_ID='" + IMEI + "')";
            USERID = new Utility().mysqlTrim(USERID);
            //PASSWORD = new Utility().mysqlTrim(PASSWORD);----29/04/19
            PASSWORD = new AESAlgorithm().getEncryptedString(PASSWORD);
            tbl_user dbuser = db.tbl_user.Where(t => t.USERID == USERID && t.STATUS == "A" && t.PASSWORD==PASSWORD).FirstOrDefault();

            //tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();
            if (dbuser != null)
            {
                //string PASSWORDAES = new AESAlgorithm().getDecryptedString(dbuser.PASSWORD);

                //tbl_csst_role roles = db.tbl_csst_role.Find(dbuser.ID_ROLE);
                tbl_organization ORGTBL = db.tbl_organization.Find(dbuser.ID_ORGANIZATION);
                if (dbuser.STATUS == "A")
                {
                    response.ResponseCode = "SUCCESS";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "User successfully authenticated";
                    response.UserID = Convert.ToInt32(dbuser.ID_USER);
                    response.UserName = dbuser.USERID;
                    int oid = ORGTBL.ID_ORGANIZATION;
                    response.ROLEID = "1";
                    response.ORGID = oid.ToString();
                    response.LogoPath = new RegistrationModel().getOrgLogo(oid);
                    response.BannerPath = new RegistrationModel().getOrgBanner(oid);
                    response.ORGEMAIL = ORGTBL.DEFAULT_EMAIL;
                    response.log_flag = new ChangePasswordLogic().CheckFirstLogin(response.UserID);

                    tbl_profile pro = db.tbl_profile.Where(t => t.ID_USER == dbuser.ID_USER).FirstOrDefault();
                    if (pro != null)
                    {
                        response.fullname = pro.FIRSTNAME + " " + pro.LASTNAME;
                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            response.profile_image = ConfigurationManager.AppSettings["profileimage_base"].ToString()+ db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}", response.UserID).FirstOrDefault();
                         }
                    }
                    else
                    {
                        response.fullname = "NA";
                    }
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        response.total_score = db.Database.SqlQuery<int>("SELECT COALESCE(SUM(total_score),0) total FROM tbl_user_level_log WHERE id_user = {0} and is_qualified=1 and status='A';", response.UserID).FirstOrDefault();
                        response.last_successive_level = db.Database.SqlQuery<int>("SELECT COALESCE( max(level),0) last_level FROM db_sul_beta.tbl_user_level_log where id_user={0} and is_qualified=1 and status='A';", response.UserID).FirstOrDefault();

                    }

                    if (String.IsNullOrEmpty(IMEI))
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
                            log.IMEI = IMEI;
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
                string Error = "Invalid User Name/Password";
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
            
           
                
               
            
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        public void SendOTP(string Semail, string Name, string OTP)
        {
            try
            {
                /*  Email ID changed on requst on 08-01-2020
                string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                const string senderPassword = "Pls@210312"; // sender password here…
                */

                string senderID = "skillmuni@thegamificationcompany.com";// use sender’s email id here..
                const string senderPassword = "03012019@Skillmuni"; // sender password here…


                string recmail = Semail;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
                string body = string.Empty;
               


                string sub = "Email Verification - TGC";
                string msg = "Dear " + Name + ",<br/><br/> Please use the secret key "+ OTP + " to verify your email id. <br/><br/> Thanks and Regards,<br/>" + "The Gamification Company";


                //string notelink = ConfigurationManager.AppSettings["content_link"];



                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, recmail, sub, msg);//body replaced by msg
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}