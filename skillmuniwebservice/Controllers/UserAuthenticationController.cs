using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UserAuthenticationController : ApiController
    {
        public HttpResponseMessage Post([FromBody]UserCredentials user)
        {
            AuthResponse Result = new AuthResponse();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    tbl_user userdet = db.Database.SqlQuery<tbl_user>("select * from tbl_user where USERID={0} and PASSWORD={1}", user.USERID, user.PASSWORD).FirstOrDefault();
                    if (userdet != null)
                    {
                        tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", userdet.ID_USER).FirstOrDefault();
                        Result.AuthStatus = "SUCCESS";
                        Result.AuthMessage = "User authenticated successfully.";
                        Result.FIRST_NAME = prof.FIRSTNAME;
                        Result.IDUSER = prof.ID_USER;
                        Result.LAST_NAME = prof.LASTNAME;
                        Result.PROFILE_IMAGE = ConfigurationManager.AppSettings["profileimage_base"].ToString() + prof.PROFILE_IMAGE;
                        Result.USERID = userdet.USERID;
                        Result.OID = Convert.ToInt32(userdet.ID_ORGANIZATION);
                        Result.id_org_game_unit = prof.id_org_game_unit;
                        Result.UserFunction = userdet.user_function;
                        Result.unit = db.Database.SqlQuery<string>("select unit from tbl_org_game_unit_master where id_org_game_unit={0}", Result.id_org_game_unit).FirstOrDefault();
                        //List<tbl_org_game_master> master_game = new List<tbl_org_game_master>();
                        //master_game = db.Database.SqlQuery<tbl_org_game_master>("select * from tbl_org_game_master where id_org={0} and  status='A'", Result.OID).ToList();
                        //List<GameUserLog> loginst = new List<GameUserLog>();
                        //foreach (var itm in master_game)
                        //{
                        //    GameUserLog ob = new GameUserLog();
                        //    ob.total_score_gained = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=1", Result.IDUSER, itm.id_org_game).FirstOrDefault();
                        //    ob.total_score_detected = db.Database.SqlQuery<int>("select COALESCE(SUM(score),0) total from tbl_org_game_user_log where id_user={0} and id_org_game={1} and score_type=2", Result.IDUSER, itm.id_org_game).FirstOrDefault();
                        //    ob.id_org_game = itm.id_org_game;
                        //    ob.game_title = itm.title;
                        //    ob.current_overallscore = ob.total_score_gained - ob.total_score_detected;
                        //    loginst.Add(ob);


                        //}
                        //Result.Score = loginst;

                        string sql01 = "select avatar_type from tbl_org_game_user_avatar where id_user=" + Result.IDUSER + " and status='A'";
                        int avatar_type = 0;
                        var avtType= db.Database.SqlQuery<int>(sql01).FirstOrDefault();
                        if (avtType != null)
                        {
                            avatar_type =Convert.ToInt32(avtType);
                        }
                        using (m2ostnextserviceDbContext dbb = new m2ostnextserviceDbContext())
                        {
                            Result.is_first_time_login = dbb.Database.SqlQuery<int>("select is_first_time_login from tbl_user where ID_USER={0} ", Result.IDUSER).FirstOrDefault();
                            prof = dbb.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", Result.IDUSER).FirstOrDefault();

                            if (Result.is_first_time_login == 1)
                            {
                                string OTP = RandomString(4);
                                //string mail = new AESAlgorithm().getDecryptedString(prof.EMAIL);
                                string mail = prof.EMAIL;
                                tbl_email_verification_key_log ver_key = dbb.Database.SqlQuery<tbl_email_verification_key_log>("select * from tbl_email_verification_key_log where id_user={0} and status='P' ", Result.IDUSER).FirstOrDefault();
                                if (ver_key == null)
                                {
                                    dbb.Database.ExecuteSqlCommand("insert into tbl_email_verification_key_log (id_user,secret_key,updated_date_time,status) values({0},{1},{2},{3})", Result.IDUSER, OTP, DateTime.Now, "P");


                                }
                                else
                                {
                                    OTP = ver_key.secret_key;
                                }
                                //SendOTP(mail, prof.FIRSTNAME + " " + prof.LASTNAME, OTP);

                            }
                        }




                    }
                    else
                    {
                        Result.AuthStatus = "FAILED";
                        Result.AuthMessage = "Entered userid and password is wrong.";

                    }


                }

            }
            catch (Exception e)
            {
                Result.AuthStatus = "FAILED";
                Result.AuthMessage = "Something went wrong.";

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }
        public void SendOTP(string Semail, string Name, string OTP)
        {
            try
            {
                /*  Email ID changed on requst on 08-01-2020
                string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                const string senderPassword = "Pls@210312"; // sender password here…
                */

                string senderID = "playtolearn@thegamificationcompany.com";// use sender’s email id here..
                const string senderPassword = "TGC@03012020"; // sender password here…


                string recmail = Semail;//new AESAlgorithm().getDecryptedString(profile.EMAIL); //mailids[i]
                string body = string.Empty;



                string sub = "Email Verification - TGC";
                string msg = "Dear " + Name + ",<br/><br/> Please use the secret key " + OTP + " to verify your email id. <br/><br/> Thanks and Regards,<br/>" + "The Gamification Company";


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
