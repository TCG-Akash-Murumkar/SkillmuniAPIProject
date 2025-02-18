using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class NonDisclosureLogic
    {
        private MySqlConnection connection = null;


        public NonDisclosureLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
        public string CheckNonDisclosureLog(int uid,int oid)
        {
            string result = "";
            tbl_non_disclousure_clause_log obj = new tbl_non_disclousure_clause_log();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_non_disclousure_clause_log where id_user=@value1 and id_org=@value2;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", uid);
            command.Parameters.AddWithValue("value2", oid);

            MySqlDataReader reader = command.ExecuteReader();
            obj.id_clause_log = 0;
            while (reader.Read())
            {
                
                obj.id_clause_log = Convert.ToInt32(reader["id_clause_log"].ToString());              
            }
            if (obj.id_clause_log == 0)
            {
                result = "FAILURE";
            }
            else
            {
                result = "SUCCESS";
            }
            reader.Close();
            connection.Close();
            return result;
        }


        public List<tbl_non_disclousure_clause_content> getContent(int oid)
        {
            List<tbl_non_disclousure_clause_content> result = new List<tbl_non_disclousure_clause_content>();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_non_disclousure_clause_content where id_org=@value1 and status='A';";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                tbl_non_disclousure_clause_content obj = new tbl_non_disclousure_clause_content();
                obj.id_clause_content = Convert.ToInt32(reader["id_clause_content"].ToString());
                obj.content = reader["content"].ToString();
                obj.content_title =  reader["content_title"].ToString();
                obj.id_creator = Convert.ToInt32(reader["id_creator"].ToString());
                obj.id_org = Convert.ToInt32(reader["id_org"].ToString());
                obj.updated_date_time = Convert.ToDateTime(reader["updated_date_time"].ToString());
                result.Add(obj);
            }
          
            reader.Close();
            connection.Close();
            return result;
        }

        public List<tbl_non_disclousure_clause_content> getDefaultContent(int oid,List<tbl_non_disclousure_clause_content> result1)
        {
            tbl_non_disclousure_clause_content result = new tbl_non_disclousure_clause_content();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_non_disclousure_clause_content where id_org=@value1;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.id_clause_content = Convert.ToInt32(reader["id_clause_content"].ToString());
                result.content = reader["content"].ToString();
                result.content_title = reader["content_title"].ToString();
                result.id_creator = Convert.ToInt32(reader["id_creator"].ToString());
                result.id_org = Convert.ToInt32(reader["id_org"].ToString());
                result.updated_date_time = Convert.ToDateTime(reader["updated_date_time"].ToString());
                result1.Add(result);
            }

            reader.Close();
            connection.Close();
            return result1;
        }

        public string postLog(string response,tbl_non_disclousure_clause_log log)
        {
          
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                String str = "INSERT INTO tbl_non_disclousure_clause_log(id_org, id_user, log_status, updated_date_time) VALUES (@value2,@value3,@value4,@value5)";
                cmd.CommandText = str;
           
                cmd.Parameters.AddWithValue("value2", log.id_org);
                cmd.Parameters.AddWithValue("value3", log.id_user);
                cmd.Parameters.AddWithValue("value4", "A");
                cmd.Parameters.AddWithValue("value5", System.DateTime.Now);
               
                connection.Open();
                int val = cmd.ExecuteNonQuery();
                if (val > 0)
                {
                    response = "SUCCESS";
                }
               
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }


            return response;

        }

        

    }
}