using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public  class SyncModel
    {
       
        private  MySqlConnection connection = null;

        public SyncModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public  bool CheckSubscription(string expiryDate)
        {
           // DateTime expiry = Convert.ToDateTime(expiryDate);
            DateTime expiry = DateTime.ParseExact(expiryDate, "yyyy-MM-dd",null);
            int result = DateTime.Compare(DateTime.Now, expiry);
            if (result < 0 || result == 0)
                return true;
            else
                return false;
        }

        public  string GetUserStatus(string userName, int roleID)
        {
            try
            {
                string status = "";
            
                MySqlCommand command = null;
                string query = "select STATUS from tbl_user where USERID = @value1 and ID_ROLE = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userName);
                command.Parameters.AddWithValue("value2", roleID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    status = Convert.ToString(reader["STATUS"]);
                }
                reader.Close();
                return status;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }
    }
}