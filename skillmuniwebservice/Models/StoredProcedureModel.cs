using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class StoredProcedureModel
    {
        private MySqlConnection connection = null;

        public StoredProcedureModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public int strp_get_other_ce_score(int oid, int uid, int ceid)
        {
            MySqlCommand command = null;
            string SQL1 = "CALL strp_get_other_ce_score(@value0,@value1,@value2);";

            int job_point = 0;
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = SQL1;
            command.Parameters.AddWithValue("value0", oid);
            command.Parameters.AddWithValue("value1", uid);
            command.Parameters.AddWithValue("value2", ceid);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                job_point = Convert.ToInt32(reader["job_point"]);
            }
            connection.Close();
            return job_point;
        }

        public int strp_get_other_score(int oid, int uid)
        {
            MySqlCommand command = null;
            string SQL1 = "CALL strp_get_other_score(@value0,@value1);";

            int job_point = 0;
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = SQL1;
                command.Parameters.AddWithValue("value0", oid);
                command.Parameters.AddWithValue("value1", uid);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    job_point = Convert.ToInt32(reader["job_point"]);
                }
            }
            catch (Exception e) { }
            finally
            {
                connection.Close();
            }


            return job_point;
        }
    }
}