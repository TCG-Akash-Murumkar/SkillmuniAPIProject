using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Net.Mail;
using System.Configuration;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    //For Social Id Integration 
    public class LoginController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody] Login login)
        {
            LoginResponse response = new LoginResponse();
            int newUserID = new RegistrationModel().NewCheckUserExist(login.UserName, login.Role);
            if (newUserID > 0)
            {
                //if (login.DeviceType.Equals("MOBILE"))
                if (string.Equals(login.DeviceType, "MOBILE", StringComparison.OrdinalIgnoreCase))
                             {
                    string deviceimei = login.DeviceID;
                    bool deviceStatus = new RegistrationModel().CheckDeviceExist(deviceimei, login.Role);
                    if (!deviceStatus)
                    {
                        int deviceTypeID = new RegistrationModel().GetDeviceTypeID(login.DeviceType);
                        int updateDevice = new RegistrationModel().UpdateUserDevice(newUserID, deviceTypeID, deviceimei);
                    }
                    tbl_organization ORGTBL = db.tbl_organization.Find(login.OrganizationID);
                   
                    response.ResponseCode = "SUCCESS";
                    response.ResponseAction = 5;
                    response.ResponseMessage = "User successfully registered";
                    response.UserID = newUserID;
                    response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                    response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);
                    response.ORGEMAIL = ORGTBL.DEFAULT_EMAIL;

                    tbl_report_login_log log = new tbl_report_login_log();
                    log.id_user = newUserID;
                    log.id_organization = login.OrganizationID;
                    log.IMEI = login.DeviceID;
                    log.LOG_DATETIME = System.DateTime.Now;
                    db.tbl_report_login_log.Add(log);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, response);

                }
            }
            else
            {
                bool deviceStatus = false;//new RegistrationModel().CheckDeviceExist(login.DeviceID, login.Role);
                if (deviceStatus)
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 1;
                    response.ResponseMessage = "Your device is already registered with a different profile. Please use the other profile to login.";
                    response.UserID = 0;
                    response.LogoPath = "";
                    response.BannerPath = "";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                //generating password for facebook / google users start.
                string password = new PasswordGeneration().GetPassword();
                login.Password = PasswordEncryption.ToMD5Hash(password);
                //password generation code ends

                Authcode authCode = new Authcode();
                authCode = new RegistrationModel().GetActiveAuthcode();
                bool userid = new RegistrationModel().CheckUserExist(login.UserName, 0);
                if (userid)
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 1;
                    response.ResponseMessage = "User already present with this UserId....";
                    response.UserID = 0;
                    response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                    response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }




                if (authCode != null)
                {
                    Registration registration = new Registration();
                    registration.OrganizationID = login.OrganizationID;
                    registration.Role = 11;
                    registration.FirstName = "m2ost ";
                    registration.LastName = " user";
                    registration.UserName = login.UserName;
                    registration.Password = login.Password;
                    registration.DeviceType = login.DeviceType;
                    registration.DeviceID = login.DeviceID;
                    registration.FBSocialID = login.FBSocialID;
                    registration.GPSocialID = login.GPSocialID;
                    registration.Provider = login.Provider;
                    registration.Email = login.UserName;
                    registration.Age = "0";
                    registration.Mobile = "";

                    int userResult = new RegistrationModel().CreateUser(registration, authCode, "A");
                    if (userResult > 0)
                    {
                        tbl_role_user_mapping rolemap = new tbl_role_user_mapping();
                        rolemap.id_csst_role = 11;
                        rolemap.id_user = userResult;
                        rolemap.status = "A";
                        rolemap.updated_date_time = System.DateTime.Now;
                        db.tbl_role_user_mapping.Add(rolemap);
                        db.SaveChanges();


                        List<tbl_assignment_role_program> rProgram = db.tbl_assignment_role_program.Where(t => t.id_role == 11 && t.id_organization == login.OrganizationID).ToList();
                        foreach (tbl_assignment_role_program rpItem in rProgram)
                        {
                            tbl_content_program_mapping mapping = db.tbl_content_program_mapping.Where(t => t.id_category == rpItem.id_program && t.id_organization == login.OrganizationID && t.id_user == userResult).FirstOrDefault();
                            if (mapping == null)
                            {
                                tbl_content_program_mapping info = new tbl_content_program_mapping();
                                info.map_type = 1;
                                info.id_role = rpItem.id_role;
                                info.id_user = userResult;
                                info.id_organization = login.OrganizationID;
                                info.id_category = rpItem.id_program;
                                info.status = "A";
                                info.option_type = 0;
                                info.start_date = rpItem.start_datetime;
                                info.expiry_date = rpItem.end_datetime;
                                info.id_category_tile = rpItem.id_category_tile;
                                info.id_category_heading = rpItem.id_category_heading;
                                info.id_assessment_sheet = 0;
                                info.updated_date_time = System.DateTime.Now;
                                db.tbl_content_program_mapping.Add(info);
                                db.SaveChanges();
                            }
                        }

                        authCode.Status = "U";
                        int authResult = 1;// new RegistrationModel().UpdateAuthcodeStatus(authCode);
                        if (authResult == 1)
                        {

                            int profileResult = new RegistrationModel().CreateProfile(userResult, registration);

                            if (profileResult == 1)
                            {
                                int deviceTypeID = new RegistrationModel().GetDeviceTypeID(registration.DeviceType);
                                if (deviceTypeID != 0)
                                {
                                    int updateDevice = new RegistrationModel().UpdateUserDevice(userResult, deviceTypeID, registration.DeviceID);
                                    if (updateDevice != 0)
                                    {
                                        response.ResponseCode = "SUCCESS";
                                        response.ResponseAction = 1;
                                        response.ResponseMessage = "User successfully registered";
                                        response.UserID = userResult;
                                        response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                                        response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);
                                       
                                        tbl_report_login_log log = new tbl_report_login_log();
                                        log.id_user = userResult;
                                        log.id_organization = login.OrganizationID;
                                        log.IMEI = login.DeviceID;
                                        log.LOG_DATETIME = System.DateTime.Now;
                                        db.tbl_report_login_log.Add(log);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        response.ResponseCode = "SUCCESS";
                                        response.ResponseAction = 2;
                                        response.ResponseMessage = "User successfully registered , but could not log device details.";
                                        response.UserID = userResult;
                                        response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                                        response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);
                                    }


                                }
                                else
                                {
                                    response.ResponseCode = "SUCCESS";
                                    response.ResponseAction = 2;
                                    response.ResponseMessage = "User successfully registered, but could not capture device deails. Please update device details.";
                                    response.UserID = userResult;
                                    response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                                    response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);
                                }
                            }
                            else
                            {
                                response.ResponseCode = "SUCCESS";
                                response.ResponseAction = 2;
                                response.ResponseMessage = "User successfully registered, but profile could not be updated. Please update profile.";
                                response.UserID = userResult;
                                response.LogoPath = new RegistrationModel().getOrgLogo(login.OrganizationID);
                                response.BannerPath = new RegistrationModel().getOrgBanner(login.OrganizationID);
                            }
                        }
                        else
                        {
                            int deleteResult = new RegistrationModel().DeleteUserRollback(userResult);
                            if (deleteResult == 1)
                            {
                                response.ResponseCode = "FAILURE";
                                response.ResponseAction = 0;
                                response.ResponseMessage = "Registration Failed. Could not generate auth code. Please try again later.";
                                response.UserID = 0;
                            }
                            else
                            {
                                response.ResponseCode = "FAILURE";
                                response.ResponseAction = 0;
                                response.ResponseMessage = "Registration Failed. Could not delete user while roll back. Please contact skillmuni support.";
                                response.UserID = 0;
                            }
                        }
                    }
                    else
                    {
                        response.ResponseCode = "FAILURE";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "Registration Failed. Could not create user. Please try again later.";
                        response.UserID = 0;
                    }
                }
                else
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "Registration Failed. Could not generate auth code. Please try again later.";
                    response.UserID = 0;
                }
            }
            // }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}