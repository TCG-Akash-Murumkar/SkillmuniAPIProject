using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Mail;

namespace m2ostnextservice.Models
{
    public class Utility
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public void eventLog(string str)
        {
            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Content/Log/"));
            string filename = System.DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            // bool exists = System.IO.Directory.Exists(@directory);
            DateTime timer = System.DateTime.Now;
            if (!exists)
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/Log/"));
            string fPath = HttpContext.Current.Server.MapPath("~/Content/Log/") + filename;
            if (!File.Exists(fPath))
            {
                File.Create(fPath);
            }


            using (StreamWriter file = File.AppendText(fPath))
            {
                string[] lines = { "timestamp : " + timer.ToString("dd-MM-yyyy HH:mm:ss") + " : " + str };
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

        public bool checkCategoryContentCount(int cid, int oid, int uid)
        {
            bool flag = false;
            try
            {
                string userbased_sql = "SELECT count(*) COUNT FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + cid + " AND id_user=" + uid + " AND id_organization=" + oid + " )";
                //int userbasedContent = db.tbl_content.SqlQuery(userbased_sql).Count();

                int userbasedContent = new CategoryModel().getContentInCategoryCount(userbased_sql);

                if (userbasedContent > 0)
                {
                    return true;
                }
                string query = "SELECT count(*) COUNT  FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + cid + " AND id_organization=" + oid + " and STATUS='A'  ) ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                                                                                                                                                                                                                                                                                 // List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                int category_count = new CategoryModel().getContentInCategoryCount(query);
                if (category_count > 0)
                {
                    return true;
                }
                string assSql = "select count(*) COUNT  from tbl_assessment_sheet where id_assesment in (select id_assessment from tbl_assessment_user_assignment where id_category=" + cid + " AND id_user=" + uid + ") OR id_assesment in (select id_assessment from tbl_assessment_categoty_mapping where id_category=" + cid + ") and status='A'";
                //List<tbl_assessment_sheet> tSheet = db.tbl_assessment_sheet.SqlQuery(assSql).ToList();
                int assessment_count = new CategoryModel().getContentInCategoryCount(assSql);

                if (assessment_count > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                new Utility().eventLog("ex m :" + e.Message);
                new Utility().eventLog("ex s :" + e.StackTrace);
                if (e.InnerException != null)
                {
                    new Utility().eventLog("ex i :" + e.InnerException.ToString());
                }
            }
            return flag;
        }

        public string SendMail(string TO, string FROM, string body, string SUBJECT)
        {
            string response = "Mail sent to : ";
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(TO);
                mail.From = new MailAddress(FROM);
                mail.Subject = SUBJECT;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("info@paathshala.biz", "3203kalokhesadan");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                /**************************************************************************/
            }
            catch (SmtpFailedRecipientException ex)
            {
                response = "";
                SmtpStatusCode statusCode = ex.StatusCode;

                if (statusCode == SmtpStatusCode.MailboxUnavailable)
                {
                    response = "Email address is unavailable ";
                }
                else if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.TransactionFailed)
                {
                    response = " Mail box is busy|Mailbox is unavailable|Transaction is failed ";
                }
                else
                {
                    response = "Exception : " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        response += " [" + ex.InnerException.Message + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                response = "Exception : " + ex.Message;
                if (ex.InnerException != null)
                {
                    response += " [" + ex.InnerException.Message + "]";
                }
            }

            return response;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string SendMailError(string TO, string FROM, string body, string SUBJECT)
        {
            string response = "Mail sent to : ";
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(TO);
                mail.From = new MailAddress(FROM);
                mail.Subject = SUBJECT;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential();
                smtp.EnableSsl = true;
                smtp.Send(mail);
                /**************************************************************************/
            }
            catch (SmtpFailedRecipientException ex)
            {
                response = "";
                SmtpStatusCode statusCode = ex.StatusCode;

                if (statusCode == SmtpStatusCode.MailboxUnavailable)
                {
                    response = "Email address is unavailable ";
                }
                else if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.TransactionFailed)
                {
                    response = " Mail box is busy|Mailbox is unavailable|Transaction is failed ";
                }
                else
                {
                    response = "Exception : " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        response += " [" + ex.InnerException.Message + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                response = "Exception : " + ex.Message;
                if (ex.InnerException != null)
                {
                    response += " [" + ex.InnerException.Message + "]";
                }
            }

            return response;
        }

        public string mysqlTrim(string str)
        {
            char[] MyChar = { '\'', '%', '=' };
            string[] sarray = { "LIKE", "SELECT", "INSERT", "--" };
            str = str.Trim(MyChar);
            str = str.Replace("LIKE", "");
            str = str.Replace("--", "");
            return Regex.Replace(str, @"[\x00'""\b\n\r\t\cZ\\%]",
                delegate (Match match)
                {
                    string v = match.Value;
                    switch (v)
                    {
                        case "\x00":            // ASCII NUL (0x00) character
                            return "\\0";

                        case "\b":              // BACKSPACE character
                            return "\\b";

                        case "\n":              // NEWLINE (linefeed) character
                            return "\\n";

                        case "\r":              // CARRIAGE RETURN character
                            return "\\r";

                        case "\t":              // TAB
                            return "\\t";

                        case "\u001A":          // Ctrl-Z
                            return "\\Z";

                        default:
                            return "\\" + v;
                    }
                });
        }

        public string uniqueIDS(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKL01234MNOPQRSTUVWXYZ56789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class SendApprovalMail
    {
        public db_m2ostEntities db = new db_m2ostEntities();

        public string sendApporovalmail(tbl_user uData, tbl_scheduled_event iEvent, tbl_scheduled_event_subscription_log item)
        {
            string eUrl = ConfigurationManager.AppSettings["SERVERURL"].ToString();
            string rmname = "";
            string eBody = "";
            string eEvent = "";
            string toName = "";
            string toEvent = "";
            string eSubject = "";
            string enString = new Encrypt().EncryptString("qwerty123456qw", "m2ost");
            eUrl += "ev/eventApproval?e=" + iEvent.id_scheduled_event + "&o=" + uData.ID_ORGANIZATION + "&u=" + uData.ID_USER + "&a=" + enString;
            tbl_profile uProfile = db.tbl_profile.Where(t => t.ID_USER == uData.ID_USER).FirstOrDefault();
            tbl_user rm = db.tbl_user.Where(t => t.ID_USER == uData.reporting_manager).FirstOrDefault();
            string rmEmail = "";

            if (rm != null)
            {
                tbl_profile profile = db.tbl_profile.Where(t => t.ID_USER == rm.ID_USER).FirstOrDefault();
                rmname = profile.FIRSTNAME + " " + profile.LASTNAME + "[" + rm.USERID + "]";
                rmEmail = profile.EMAIL;
            }

            if (iEvent != null)
            {
                toName = uProfile.FIRSTNAME + " " + uProfile.LASTNAME + " - " + uData.USERID;
                toEvent = iEvent.event_title;
                eBody = "User <strong>" + toName + "</strong> has sent a request to subscribe to the Event : <strong>" + iEvent.event_title + "</strong> ";
                eEvent = "<h4>Event Description </h4> ";
                eEvent += "<br>Title :<strong>" + iEvent.event_title + "</strong> ";
                eEvent += "<br>Objective :<strong>" + iEvent.program_objective + "</strong> ";
                eEvent += "<br>Schedule :<strong>" + iEvent.event_start_datetime.Value.ToString("dd-MM-yyyy HH:MM") + "</strong> ";
                eEvent += "<br>Facilitator :<strong>" + iEvent.facilitator_name + " [" + iEvent.facilitator_organization + "]</strong> ";

                eSubject = toEvent + " : Approval Request form " + toName;
            }
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Content/eBody.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{name}", rmname);
            body = body.Replace("{body}", eBody);
            body = body.Replace("{event}", eEvent);
            body = body.Replace("{url}", eUrl);

            bool flag = new Utility().IsValidEmail(rmEmail);
            if (flag)
            {
                new Utility().SendMail(rmEmail, "info@paathshala.biz", body, eSubject);
            }
            else
            {
            }
            return "";
        }
    }

    public class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";

        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;

        //Encrypt
        public string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //Decrypt
        public string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}