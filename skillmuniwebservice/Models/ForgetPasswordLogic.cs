using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ForgetPasswordLogic
    {
        public MySqlConnection conn = null;
        public ForgetPasswordLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }
        public string TriggerMail(tbl_profile profile,tbl_user user)
        {
            Random rnd = new Random();
            int rand = rnd.Next(100, 1000);

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
          
            string passworde = Convert.ToString(rand) +"!"+new string(Enumerable.Repeat(chars, 2).Select(s => s[rnd.Next(s.Length)]).ToArray());
            string result = "";
            try
            {
                string password = PasswordEncryption.ToMD5Hash(passworde);

                MySqlCommand cmd = conn.CreateCommand();
                string str = "Update tbl_user set PASSWORD='" + password + "' where ID_USER='" + user.ID_USER + "'";
                cmd.CommandText = str;
                conn.Open();
                cmd.ExecuteNonQuery();
                SendMail(profile.EMAIL,passworde);
                result = "Password have been sent to your Mail ID . Please Check Your Mail";


            }
            catch (Exception e)
            {
                result = "SOMETHING WENT WRONG.Try again after sometime.";
            }
            finally
            {
                conn.Close();
            }


            return result;
        }

        public tbl_user getUSER(string userid)
        {
            tbl_user result = new tbl_user();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_user where USERID=@value1 and STATUS='A';";
            conn.Open();
            command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", userid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                result.PASSWORD = reader["PASSWORD"].ToString();
                result.ID_USER = Convert.ToInt32(reader["ID_USER"].ToString());
               




            }
            reader.Close();
            conn.Close();
            return result;
        }

        public tbl_profile getProfile(int userid)
        {
            tbl_profile result = new tbl_profile();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_profile where ID_USER=@value1;";
            conn.Open();
            command = conn.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", userid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                result.EMAIL = reader["EMAIL"].ToString();
            
            }
            reader.Close();
            conn.Close();
            return result;
        }
        public bool SendMail(string mail,string password)
        {

            try
            {
                if (true)//File.Exists(path)
                {

                    /*  Email ID changed on requst on 08-01-2020
                    string senderID = "paathshala-learningtech@paathshala.biz";// use sender’s email id here..
                    const string senderPassword = "Pls@210312"; // sender password here…
                    */

                    string senderID = "skillmuni@thegamificationcompany.com";// use sender’s email id here..
                    const string senderPassword = "03012019@Skillmuni"; // sender password here…


                    string recmail =mail; //mailids[i]
                    string body = string.Empty;



                    string mailpage = ConfigurationManager.AppSettings["mail"];
                    //string mailpage = "";
                    using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                    {
                        body = reader.ReadToEnd();
                    }

                  
                 
                    body = body.Replace("{OTP}","Your new password is "+ password);

                    string sub = "Password Reset Request";
                    string msg = "";


                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                        Timeout = 30000,
                    };
                    MailMessage message = new MailMessage(senderID, recmail, sub, body);
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message : " + e.Message);
                Console.WriteLine("Trace : " + e.StackTrace);
            }
            return true;
        }
    }
}