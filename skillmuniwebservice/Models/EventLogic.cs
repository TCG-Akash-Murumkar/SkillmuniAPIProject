using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using m2ostnextservice.Models;

namespace m2ostnextservice.Models
{
    public class EventLogic
    {
        public MySqlConnection conn = null;

        public EventLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }

        public List<EventBatch> getBatchList(int IdEvent)
        {
            List<EventBatch> batch = new List<EventBatch>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_batch where id_event =@value1";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", IdEvent);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EventBatch obj = new EventBatch();
                        obj.batch_time = reader["batch_time"].ToString();
                        obj.id_event = Convert.ToInt32(reader["id_event"]);
                        obj.id_event_batch = Convert.ToInt32(reader["id_event_batch"]);
                        obj.id_org = Convert.ToInt32(reader["id_org"].ToString());
                        obj.participants = Convert.ToInt32(reader["participants"].ToString());
                        //obj.available_seats = obj.participants - getAvailableSeats(obj.id_event_batch);
                        batch.Add(obj);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return batch;
        }

        public tbl_web_version_control getVersionControl()
        {
            tbl_web_version_control version = new tbl_web_version_control();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_web_version_control";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        version.id_version_control = Convert.ToInt32(reader["id_version_control"].ToString());
                        version.version_number = reader["version_number"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return version;
        }

        public int getAvailableSeats(int id_batch)
        {
            int result = 0;
            List<tbl_user_event_mapping> map = new List<tbl_user_event_mapping>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_user_event_mapping where id_batch =@value1";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", id_batch);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tbl_user_event_mapping obj = new tbl_user_event_mapping();

                        obj.id_batch = Convert.ToInt32(reader["id_batch"].ToString());
                        map.Add(obj);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            if (map != null)
            {
                result = map.Count;
            }

            return result;
        }

        public List<EventBatch> getAvailable(List<EventBatch> eventbatch)
        {
            foreach (var itm in eventbatch)
            {
                itm.available_seats = itm.participants - getAvailableSeats(itm.id_event_batch);
            }
            return eventbatch;
        }

        public List<tbl_user_event_mapping> getMappedEvents(int iduser)
        {
            List<tbl_user_event_mapping> map = new List<tbl_user_event_mapping>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_user_event_mapping where id_user =@value1";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", iduser);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tbl_user_event_mapping obj = new tbl_user_event_mapping();
                        obj.id_event = Convert.ToInt32(reader["id_event"].ToString());
                        obj.id_org = Convert.ToInt32(reader["id_org"]);
                        obj.id_user = Convert.ToInt32(reader["id_user"]);
                        obj.id_user_mapping = Convert.ToInt32(reader["id_user_mapping"].ToString());
                        obj.status = reader["status"].ToString();
                        obj.updated_date_time = Convert.ToDateTime(reader["updated_date_time"].ToString());
                        obj.id_batch = Convert.ToInt32(reader["id_batch"].ToString());
                        map.Add(obj);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return map;
        }

        public tbl_user_event_mapping getMappedEvent(int iduser, int id_event)
        {
            tbl_user_event_mapping map = new tbl_user_event_mapping();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_user_event_mapping where id_user =@value1 and id_event=@value2";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", iduser);
                command.Parameters.AddWithValue("value2", id_event);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        map.id_event = Convert.ToInt32(reader["id_event"].ToString());
                        map.id_org = Convert.ToInt32(reader["id_org"]);
                        map.id_user = Convert.ToInt32(reader["id_user"]);
                        map.id_user_mapping = Convert.ToInt32(reader["id_user_mapping"].ToString());
                        map.status = reader["status"].ToString();
                        map.updated_date_time = Convert.ToDateTime(reader["updated_date_time"].ToString());
                        map.id_batch = Convert.ToInt32(reader["id_batch"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return map;
        }

        public string getBatch(int id_batch)
        {
            string batch = "";
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_batch where id_event_batch =@value1";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", id_batch);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        batch = reader["batch_time"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return batch;
        }

        public string SubscribeToEvent(int uid, int event_id, int batchid, int orgid)
        {
            string result = "";
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "insert into tbl_user_event_mapping (id_user,id_org,status,updated_date_time,id_event,id_batch)value(@value1,@value2,@value3,@value4,@value5,@value6)";
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", uid);
                cmd.Parameters.AddWithValue("value2", orgid);
                cmd.Parameters.AddWithValue("value3", "A");
                cmd.Parameters.AddWithValue("value4", DateTime.Now);
                cmd.Parameters.AddWithValue("value5", event_id);
                cmd.Parameters.AddWithValue("value6", batchid);
                conn.Open();
                cmd.ExecuteNonQuery();
                result = "successfully subscribed";
                conn.Close();
            }
            catch (Exception e)
            {
                result = "Subscription Failured";
                return result;
            }
            finally
            {
            }
            return result;
        }

        public string UnSubscribeToEvent(int uid, int event_id)
        {
            string result = "";
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "Delete from tbl_user_event_mapping where id_user=@value1 and id_event=@value2";
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", uid);
                cmd.Parameters.AddWithValue("value2", event_id);

                conn.Open();
                cmd.ExecuteNonQuery();
                result = "Unsubscribed from the event";
                conn.Close();
            }
            catch (Exception e)
            {
                result = "UnSubscription Failured";
                return result;
            }
            finally
            {
            }
            return result;
        }

        public bool SendMail(string mail, string Event, string batch, string date, int orgid, string des)
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


                    string recmail = mail; //mailids[i]
                    string body = string.Empty;
                    string logo = new RegistrationModel().getOrgLogo(orgid);
                    DateTime date1 = Convert.ToDateTime(date);

                    string mailpage = ConfigurationManager.AppSettings["mail"];
                    //string mailpage = "";
                    using (StreamReader reader = new StreamReader(@"" + mailpage + ""))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{ORGLOGO}", logo);
                    body = body.Replace("{TITLE}", Event);
                    body = body.Replace("{DATE}", date1.ToString("dd-MM-yyyy"));
                    body = body.Replace("{BATCH}", batch + " Hrs");
                    body = body.Replace("{DES}", des);

                    string sub = "Event Subscription";
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

        public string getApiResponseString(string api)
        {
            byte[] response = null;

            var wc123 = new WebClient();
            using (var wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                response = wc.DownloadData(api);
            }
            string result = System.Text.Encoding.UTF8.GetString(response);
            return result;
        }

        public int getCurrentAttendersCount(int id_event, int id_batch)
        {
            int current_count = 0;
            List<tbl_user_event_mapping> map = new List<tbl_user_event_mapping>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_user_event_mapping where id_event =@value1 and id_batch=@value2";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", id_event);
                command.Parameters.AddWithValue("value2", id_batch);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tbl_user_event_mapping obj = new tbl_user_event_mapping();
                        obj.id_event = Convert.ToInt32(reader["id_event"].ToString());

                        map.Add(obj);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            current_count = map.Count;
            return current_count;
        }

        public List<tbl_scheduled_event> getEventList(tbl_user user, string location)
        {
            List<tbl_scheduled_event> even = new List<tbl_scheduled_event>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_scheduled_event where id_organization =@value1 and status=@value2 and (location=@value3 or location='All') and participant_level like '%" + user.user_designation.ToLower() + "%'";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", user.ID_ORGANIZATION);
                command.Parameters.AddWithValue("value2", "A");
                command.Parameters.AddWithValue("value3", location);
                command.Parameters.AddWithValue("value4", user.user_designation.ToLower());
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tbl_scheduled_event obj = new tbl_scheduled_event();

                        obj.event_additional_info = reader["event_additional_info"].ToString();
                        obj.event_comment = reader["event_comment"].ToString();
                        obj.event_description = reader["event_description"].ToString();
                        obj.registration_start_date = Convert.ToDateTime(reader["registration_start_date"].ToString());
                        obj.event_title = reader["event_title"].ToString();
                        obj.facilitator_name = reader["facilitator_name"].ToString();
                        obj.id_scheduled_event = Convert.ToInt32(reader["id_scheduled_event"].ToString());
                        obj.participant_level = reader["participant_level"].ToString();
                        obj.program_image = reader["program_image"].ToString();
                        obj.program_location = reader["program_location"].ToString();
                        obj.program_venue = reader["program_venue"].ToString();
                        obj.id_organization = Convert.ToInt32(reader["id_organization"].ToString());

                        even.Add(obj);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return even;
        }
        public int getDemoNotificationcount()
        {
            int count = 0;
            List<int> notlist = new List<int>();
            try
            {
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_notification_demo where status =@value1";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int id= Convert.ToInt32(reader["id_notification"].ToString());

                        notlist.Add(id);
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            count = notlist.Count;
            return count;
        }

        //public tbl_brief_m2ost_category_mapping getCatMappdata()
        //{
        //    tbl_brief_m2ost_category_mapping ctmap = new tbl_brief_m2ost_category_mapping();

        //    using (m2ostnextserviceDbContext dbcatmapm = new m2ostnextserviceDbContext())
        //    {
        //        ctmap = dbcatmapm.Database.SqlQuery<tbl_brief_m2ost_category_mapping>("select * from tbl_brief_m2ost_category_mapping where id_brief={0}", itm.id_brief_master).FirstOrDefault();


        //    }
        //    if (ctmap != null)
        //    {
        //        if (ctmap.type == 2)
        //        {
        //            using (M2ostCatDbContext dbcat = new M2ostCatDbContext())
        //            {
        //                ctmap.CATEGORYNAME = dbcat.Database.SqlQuery<string>("select CATEGORYNAME from tbl_category where ID_CATEGORY={0} ", ctmap.id_category).FirstOrDefault();
        //                ctmap.Heading_title = dbcat.Database.SqlQuery<string>("select Heading_title from tbl_category_heading where id_category_heading={0} ", ctmap.id_category_heading).FirstOrDefault();
        //                ctmap.CategoryImage = ConfigurationManager.AppSettings["CATIm"].ToString() + dbcat.Database.SqlQuery<string>("select IMAGE_PATH from tbl_category where ID_CATEGORY={0} ", inst.cat_mapping.id_category).FirstOrDefault();

        //            }
        //        }
        //        else if (ctmap.type == 1)
        //        {
        //            ctmap.CATEGORYNAME = "this is type 1";
        //        }
               
        //    }
        //    return ctmap;
          

        //}


    }
}