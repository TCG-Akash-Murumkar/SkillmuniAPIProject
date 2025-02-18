using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class SubscriptionModel
    {

        private MySqlConnection connection = null;

        public SubscriptionModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public  bool UpdateSubscription(int userId, int contentId, DateTime expiryDate)
        {
            try
            {

                MySqlCommand command = null;
                int result = 0;
                string query = "INSERT INTO tbl_subscriptions (ID_USER,ID_CONTENT,STATUS,UPDATEDTIME,EXPIRY_DATE) VALUES (@value1, @value2, @value3, @value4, @value5)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userId);
                command.Parameters.AddWithValue("value2", contentId);
                command.Parameters.AddWithValue("value3", "A");
                command.Parameters.AddWithValue("value4", System.DateTime.Now);
                command.Parameters.AddWithValue("value5", expiryDate);
                result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }



        public  int SubscriptionCheck(int CID, int UID)
        {
            int flag =new SubscriptionModel().checkSubscription(CID, UID);


            if (flag > 0)
            {
                bool flagc =new SubscriptionModel().checkExpiry(CID, UID);
                if (flagc)
                {
                    return flag;
                }
                else
                {
                    return -1;
                }

            }
            else
            {
                return 0;
            }
        }

        public  int checkSubscription(int cid, int uid)
        {
            int flag = 0;
            try
            {

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_subscriptions where STATUS = 'A' and  ID_USER = @value1 and id_content=@value2 limit 1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", uid);
                command.Parameters.AddWithValue("value2", cid);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        flag = Convert.ToInt32(reader["id_subscription_log"]);
                    }

                    reader.Close();
                }

                return flag;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  bool checkExpiry(int cid, int uid)
        {
            bool flag = true;
            try
            {

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_subscriptions where STATUS = 'A' and  ID_USER = @value1 and id_content=@value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", uid);
                command.Parameters.AddWithValue("value2", cid);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime expire = Convert.ToDateTime(reader["EXPIRY_DATE"].ToString());
                        if (System.DateTime.Now > expire)
                        {
                            flag = false;
                        }
                    }
                }

                return flag;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  List<Subscription> GetSubscriptionDetails(int userId)
        {
            try
            {
                List<Subscription> subscriptionList = null;

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_subscriptions where STATUS = 'A' and  ID_USER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    subscriptionList = new List<Subscription>();

                    while (reader.Read())
                    {
                        Subscription subscription = new Subscription();
                        subscription.UserId = Convert.ToInt32(reader["ID_USER"]);
                        subscription.ContentId = Convert.ToInt32(reader["ID_CONTENT"]);
                        subscription.ExpiryDate = reader.GetDateTime(reader.GetOrdinal("EXPIRY_DATE")).ToString();
                        subscriptionList.Add(subscription);
                    }
                    reader.Close();
                }

                return subscriptionList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }

        public  int GetBanner(string oid, string uid)
        {
            bool flag = true;
            int code = 0;
            try
            {

                MySqlCommand command = null;
                string query = "SELECT id_organisation_banner_links FROM tbl_organisation_banner_links WHERE id_organisation_banner in (select id_organisation_banner from tbl_organisation_banner where id_organisation = @value1)and id_user=@value2 ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", oid);
                command.Parameters.AddWithValue("value2", uid);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        code = Convert.ToInt32(reader["id_organisation_banner_links"].ToString());
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                connection.Close();
            }

            return code;
        }


        public  int SetBannerInsert(string bid, string uid, string opt)
        {
            try
            {

                MySqlCommand command = null;
                string query = "INSERT INTO tbl_organisation_banner_links (id_organisation_banner, id_user, user_option, updated_date_time) VALUES (@value1,@value2,@value3,@value4)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", bid);
                command.Parameters.AddWithValue("value2", uid);
                command.Parameters.AddWithValue("value3", opt);
                command.Parameters.AddWithValue("value4", System.DateTime.Now);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    result = Convert.ToInt32(command.LastInsertedId);
                }
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                connection.Close();
            }

        }


        public  int SetBannerUpdate(string bid, string opt)
        {
            try
            {

                MySqlCommand command = null;
                string query = "UPDATE tbl_organisation_banner_links SET user_option=@value1 WHERE id_organisation_banner_links=@value";

                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", opt);
                command.Parameters.AddWithValue("value", bid);

                int result = command.ExecuteNonQuery();
                return result;

            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                connection.Close();
            }


        }





    }
    public class Subscription
    {
        public int UserId { get; set; }

        public int ContentId { get; set; }

        public string ExpiryDate { get; set; }
    }

    public class PackSubscription
    {
        public int PackId { get; set; }

        public string PackSKU { get; set; }

        public string UserSKU { get; set; }

        public int UserId { get; set; }

        public string ContentId { get; set; }

        public string ExpiryDate { get; set; }
    }



}