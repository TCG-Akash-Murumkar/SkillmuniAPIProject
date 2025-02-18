
using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{

    public class RegistrationServiceController : ApiController
    {

        // POST api/<controller>

        public HttpResponseMessage Post([FromBody]Registration registration)
        {
            Response response = new Response();
            string verificationCode = "";
            registration.Password = PasswordEncryption.ToMD5Hash(registration.Password);
            bool result = new RegistrationModel().CheckUserExist(registration.UserName, registration.Role);
            if (result)
            {
                response.ResponseCode = "FAILURE";
                response.ResponseAction = 0;
                response.ResponseMessage = "Given user name already present in the system. Please try with another user name.";
            }
            else
            {
                Authcode authCode = new Authcode();
                authCode = new RegistrationModel().GetActiveAuthcode();
                if (authCode != null)
                {
                    verificationCode = authCode.Code.ToString();
                    int userResult = new RegistrationModel().CreateUser(registration, authCode, "P");
                    if (userResult > 0)
                    {
                        authCode.Status = "R";
                        int authResult = new RegistrationModel().UpdateAuthcodeStatus(authCode);
                        if (authResult == 1)
                        {
                            int profileResult = new RegistrationModel().CreateProfile(userResult, registration);
                            if (profileResult == 1)
                            {
                                int deviceTypeID = new RegistrationModel().GetDeviceTypeID(registration.DeviceType);
                                if (deviceTypeID != 0)
                                {
                                    int actioID = new RegistrationModel().GetActionID("Registration");
                                    if (actioID != 0)
                                    {
                                        int updateLogResult = new RegistrationModel().UpdateUserLog(userResult, deviceTypeID, registration.DeviceID, actioID);
                                        if (updateLogResult != 0)
                                        {

                                            response.ResponseCode = "SUCCESS";
                                            response.ResponseAction = 1;
                                            response.ResponseMessage = "User successfully registered with pending status. Please activate the account.";
                                        }
                                        else
                                        {
                                            response.ResponseCode = "SUCCESS";
                                            response.ResponseAction = 2;
                                            response.ResponseMessage = "User successfully registered with pending status, but could not log details. Please activate the account.";
                                        }
                                    }
                                    else
                                    {
                                        response.ResponseCode = "SUCCESS";
                                        response.ResponseAction = 2;
                                        response.ResponseMessage = "User successfully registered with pending status, but could not log details. Please activate the account.";
                                    }
                                }
                                else
                                {
                                    response.ResponseCode = "SUCCESS";
                                    response.ResponseAction = 2;
                                    response.ResponseMessage = "User successfully registered with pending status, but could not capture device deails. Please activate the account and update device details.";
                                }
                            }
                            else
                            {
                                response.ResponseCode = "SUCCESS";
                                response.ResponseAction = 2;
                                response.ResponseMessage = "User successfully registered with pending status, but profile could not be updated. Please activate the account and update profile.";
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
                            }
                            else
                            {
                                response.ResponseCode = "FAILURE";
                                response.ResponseAction = 0;
                                response.ResponseMessage = "Registration Failed. Could not delete user while roll back. Please contact administrator.";
                            }
                        }
                    }
                    else
                    {
                        response.ResponseCode = "FAILURE";
                        response.ResponseAction = 0;
                        response.ResponseMessage = "Registration Failed. Could not create user. Please try again later.";
                    }
                }
                else
                {
                    response.ResponseCode = "FAILURE";
                    response.ResponseAction = 0;
                    response.ResponseMessage = "Registration Failed. Could not generate auth code. Please try again later.";
                }
            }
            if (response.ResponseCode.Equals("SUCCESS"))
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(registration.UserName);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                mail.Subject = ConfigurationManager.AppSettings["MailSendAuthCodeSubject"].ToString();
                mail.Body = "<p>Your Verification Code for Skillmuni access is:<h5> UserName: " + verificationCode + "</h5></p>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["MailSMTP"].ToString(); ;
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MailUserName"].ToString(), ConfigurationManager.AppSettings["MailPassword"].ToString());// Enter seders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}