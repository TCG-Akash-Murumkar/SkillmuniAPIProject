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
using System.Security.Cryptography;

namespace m2ostnextservice.Controllers
{
    public class B2CSocialAuthController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody] B2CSocial user)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            LoginResponseAuth response = new LoginResponseAuth();
            int soid = Convert.ToInt32(ConfigurationManager.AppSettings["SOCIALORG"].ToString());
            int iduse = 0;
            try
            {
                string sqlsc = "SELECT * FROM tbl_social_platform_active_directory where social_code='" + user.SCTYPE + "' and social_platform_code='" + user.SCID + "' and status='A' limit 1";
                tbl_social_platform_active_directory sc = db.Database.SqlQuery<tbl_social_platform_active_directory>(sqlsc).FirstOrDefault();

                if (sc != null)
                {
                    tbl_user suser = db.tbl_user.Where(t => t.ID_USER == sc.id_user).FirstOrDefault();

                    tbl_organization ORGTBL = db.tbl_organization.Find(suser.ID_ORGANIZATION);
                    if (suser.STATUS == "A")
                    {
                        response.ResponseCode = "SUCCESS";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "User successfully registered";
                        response.UserID = Convert.ToInt32(suser.ID_USER);
                        response.UserName = suser.USERID;
                        int oid = ORGTBL.ID_ORGANIZATION;
                        response.ROLEID = "1";
                        response.ORGID = oid.ToString();

                        response.LogoPath = new RegistrationModel().getOrgLogo(oid);
                        response.BannerPath = new RegistrationModel().getOrgBanner(oid);
                        response.ORGEMAIL = ORGTBL.DEFAULT_EMAIL;
                        response.log_flag = new ChangePasswordLogic().CheckFirstLogin(response.UserID);
                        tbl_profile pro = new tbl_profile();
                        using (m2ostnextserviceDbContext instdb = new m2ostnextserviceDbContext())
                        {
                            response.ref_id = instdb.Database.SqlQuery<string>("select ref_id from tbl_user where ID_USER={0}", suser.ID_USER).FirstOrDefault();
                            pro = instdb.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", suser.ID_USER).FirstOrDefault();
                        }


                        if (pro != null)
                        {
                            response.fullname = pro.FIRSTNAME + " " + pro.LASTNAME;
                            response.profile_data = 1;
                            response.UserEmail = pro.EMAIL;
                            response.state = pro.STATE;
                            response.city = pro.CITY;
                            response.college = pro.COLLEGE;
                            int college = db.Database.SqlQuery<int>("select id_college from tbl_college_list where college_name={0}", pro.COLLEGE).FirstOrDefault();
                            if (college > 0)
                            {
                                response.id_college = college;
                            }

                            response.college_city = pro.clg_city;
                            response.college_state = pro.clg_state;

                        }
                        else
                        {
                            response.fullname = "NA";
                            response.profile_data = 0;
                        }

                        if (String.IsNullOrEmpty(user.IMEI))
                        {
                            tbl_report_login_log log = new tbl_report_login_log();
                            log.id_user = suser.ID_USER;
                            log.id_organization = suser.ID_ORGANIZATION;
                            log.IMEI = "WEBSITE";
                            log.LOG_DATETIME = System.DateTime.Now;
                            db.tbl_report_login_log.Add(log);
                            db.SaveChanges();
                        }
                        else
                        {
                            tbl_report_login_log log = new tbl_report_login_log();
                            log.id_user = suser.ID_USER;
                            log.id_organization = suser.ID_ORGANIZATION;
                            log.IMEI = user.IMEI;
                            log.LOG_DATETIME = System.DateTime.Now;
                            db.tbl_report_login_log.Add(log);
                            db.SaveChanges();
                        }
                        tbl_user_log_master lgmas = new tbl_user_log_master();

                        lgmas = db.Database.SqlQuery<tbl_user_log_master>("select * from tbl_user_log_master where id_user={0}", response.UserID).FirstOrDefault();

                        if (lgmas == null)
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_user_log_master (id_user,is_registered,academic_tiles,study_abroad,job,entrepreneurship,updated_date_time) values ({0},{1},{2},{3},{4},{5},{6})", response.UserID, "NO", 0, 0, 0, 0, DateTime.Now);

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
                    response.profile_data = 0;
                    if (user.SCSTATUS == "T")
                    {
                        string unid = new Utility().uniqueIDS(8);
                        tbl_csst_role role = db.tbl_csst_role.Where(t => t.id_organization == soid).FirstOrDefault();

                        //tbl_role role = db.tbl_role.Where(t => t.ID_ORGANIZATION == soid).FirstOrDefault();
                        tbl_user tuser = new tbl_user();
                        tuser.ID_ORGANIZATION = soid;
                        tuser.ID_ROLE = role.id_csst_role;
                        tuser.EMPLOYEEID = unid;
                        tuser.EXPIRY_DATE = DateTime.Now.AddMonths(12);
                        tuser.FBSOCIALID = "";
                        tuser.GPSOCIALID = "";
                        tuser.ID_CODE = 1;
                        tuser.is_reporting = 0;
                        tuser.PASSWORD = PasswordEncryption.ToMD5Hash("PLS" + unid);
                        tuser.reporting_manager = 0;
                        tuser.STATUS = "A";
                        tuser.USERID = unid;
                        tuser.UPDATEDTIME = System.DateTime.Now;
                        tuser.reporting_manager = Convert.ToInt32(ConfigurationManager.AppSettings["id_reporting_manager"].ToString());
                        db.tbl_user.Add(tuser);
                        db.SaveChanges();
                        response.ref_id = new RegistrationModel().RandomString(5);



                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            db.Database.ExecuteSqlCommand("Update tbl_user set UPDATEDTIME={0} where ID_USER={1}", System.DateTime.Now, tuser.ID_USER);
                            db.Database.ExecuteSqlCommand("update  tbl_user set ref_id={0} where ID_USER={1}", response.ref_id, tuser.ID_USER);

                        }

                        int ids = tuser.ID_USER;
                        if (ids > 0)
                        {
                            if (String.IsNullOrEmpty(user.IMEI))
                            {
                                tbl_report_login_log log = new tbl_report_login_log();
                                log.id_user = ids;
                                log.id_organization = soid;
                                log.IMEI = "SOCIALWEBSITE";
                                log.LOG_DATETIME = System.DateTime.Now;
                                db.tbl_report_login_log.Add(log);
                                db.SaveChanges();
                            }
                            else
                            {
                                tbl_report_login_log log = new tbl_report_login_log();
                                log.id_user = ids;
                                log.id_organization = soid;
                                log.IMEI = user.IMEI;
                                log.LOG_DATETIME = System.DateTime.Now;
                                db.tbl_report_login_log.Add(log);
                                db.SaveChanges();
                            }
                            tbl_social_platform_master smaster = db.Database.SqlQuery<tbl_social_platform_master>("select * from tbl_social_platform_master where social_platform_code={0}", user.SCTYPE).FirstOrDefault();
                            if (smaster != null)
                            {
                                tbl_social_platform_active_directory temp = new tbl_social_platform_active_directory();
                                temp.id_organization = soid;
                                temp.id_user = ids;
                                temp.social_code = smaster.social_platform_code;
                                temp.social_platform_code = user.SCID;
                                temp.id_social_platform_master = smaster.id_social_platform_master;
                                temp.status = "A";
                                temp.updated_date_time = DateTime.Now;



                                db.Database.ExecuteSqlCommand("insert into tbl_social_platform_active_directory(id_organization,id_social_platform_master,social_platform_code,id_user,social_code,status,updated_date_time) values({0},{1},{2},{3},{4},{5},{6})", temp.id_organization,temp.id_social_platform_master,temp.social_platform_code,temp.id_user,temp.social_code,temp.status,temp.updated_date_time);
                                //db.tbl_social_platform_active_directory.Add(temp);
                                //db.SaveChanges();
                            }
                            response.ResponseCode = "SUCCESS";
                            response.ResponseAction = 0;
                            response.ResponseMessage = "User successfully registered";
                            response.UserID = Convert.ToInt32(tuser.ID_USER);
                            response.UserName = tuser.USERID;
                            response.ROLEID = "1";
                            response.ORGID = soid.ToString();
                            response.LogoPath = new RegistrationModel().getOrgLogo(soid);
                            response.BannerPath = new RegistrationModel().getOrgBanner(soid);
                            response.ORGEMAIL = "";
                            response.log_flag = 1;
                            response.profile_data = 0;
                        }
                        //--------------------------------Brief Auto Assignment-------------------
                        //UserSession user = (UserSession)HttpContext.Session.Contents["UserSession"];
                        int orgid = soid;
                        iduse = tuser.ID_USER;

                        List<tbl_brief_master> brief_list = new List<tbl_brief_master>();

                        brief_list = db.Database.SqlQuery<tbl_brief_master>("select * from tbl_brief_master where id_organization={0} and status={1}", soid, "A").ToList();
                        //var brief_list = this.db.tbl_brief_master.Where(o => o.id_organization == soid && o.status == "A" && this.db.tbl_brief_status.Any(t => t.id_brief_master == o.id_brief_master && t.status_code == 5 && t.status == "A")).ToList();

                        foreach (tbl_brief_master brfitm in brief_list)
                        {
                            //tbl_brief_status sts= db.Database.SqlQuery<tbl_brief_status>("select * from tbl_brief_status where status_code={0} and status={1} and id_brief_master={2}", 5, "A",brfitm.id_brief_master).Any();
                            if (db.Database.SqlQuery<tbl_brief_status>("select * from tbl_brief_status where status_code={0} and status={1} and id_brief_master={2}", 5, "A", brfitm.id_brief_master).Any())
                            {
                                var row = new tbl_brief_user_assignment
                                {
                                    id_brief_master = brfitm.id_brief_master,
                                    scheduled_datetime = null,
                                    id_user = iduse,
                                    scheduled_status = "NA",//S-Sent,R-Recieved,A-completed
                                    published_datetime = DateTime.Now,
                                    published_status = "S",
                                    status = "A",
                                    updated_date_time = DateTime.Now,
                                    assignment_datetime = DateTime.Now,
                                    assignment_status = "P",
                                };
                                this.db.tbl_brief_user_assignment.Add(row);




                                var rst = new tbl_brief_read_status
                                {
                                    id_brief_master = brfitm.id_brief_master,
                                    id_user = iduse,
                                    read_status = 0,
                                    action_status = 0,
                                    id_organization = soid,
                                    status = "A",
                                    updated_date_time = DateTime.Now,
                                };

                                this.db.tbl_brief_read_status.Add(rst);
                            }



                        }

                        this.db.SaveChanges();



                        


                    }

                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_user_log_master (id_user,is_registered,academic_tiles,study_abroad,job,entrepreneurship,updated_date_time) values ({0},{1},{2},{3},{4},{5},{6})", iduse, "NO", 0, 0, 0, 0, DateTime.Now);

                    }

                }
               
            }
            catch (Exception e)
            {
                //new Utility().eventLog(controllerName + " : " + e.Message);
                //new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                //new Utility().eventLog("Additional Details" + " : " + e.Message);
                return Request.CreateResponse(HttpStatusCode.OK, e);
                //throw e;
            }
            finally
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }

}