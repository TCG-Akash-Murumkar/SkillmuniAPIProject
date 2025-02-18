using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ForcePasswordChangeLogic
    {
        private MySqlConnection connection = null;


        public ForcePasswordChangeLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
        public string getGCMStatus(int uid)
        {
            string response="";
            List<tbl_user_gcm_log> result = new List<tbl_user_gcm_log>();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_user_gcm_log where  id_user="+uid+" and status='A';";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
       

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                tbl_user_gcm_log inst = new tbl_user_gcm_log();
                inst.id_user_gcm_log = Convert.ToInt32(reader["id_user_gcm_log"].ToString());
              

                result.Add(inst);
            }
            if (result.Count == 0)
            {

                response = "FAILURE";

            }
            else
            {
                response = "SUCCESS";

            }
            reader.Close();
            connection.Close();

            return response;
        }

        public string checkOrgType(int oid)
        {
            string response = "";
            tbl_organization result = new tbl_organization();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_organization where ID_ORGANIZATION="+oid+" and STATUS='A';";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.ID_BUSINESS_TYPE =Convert.ToInt32(reader["ID_BUSINESS_TYPE"].ToString());      
            }
            if (result.ID_BUSINESS_TYPE == 2)
            {
                response = "Y";
            }
            else
            {
                response = "N";
            }
            reader.Close();
            connection.Close();
            return response;
        }
    }
}