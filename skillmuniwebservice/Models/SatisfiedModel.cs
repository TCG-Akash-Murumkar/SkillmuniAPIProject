using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class SatisfiedModel
    {

        private MySqlConnection connection = null;

        public SatisfiedModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public List<SatisfiedResult> NewGetSupportingData(string answerID)
        {
            List<SatisfiedResult> satisfiedResult = null;
            try
            {

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_type_link WHERE ID_CONTENT_ANSWER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", answerID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    satisfiedResult = new List<SatisfiedResult>();
                    while (reader.Read())
                    {
                        SatisfiedResult result = new SatisfiedResult();
                        result.PATH = Convert.ToString(reader["LINK_VALUE"]);
                        result.TYPE = reader["ID_CONTENT_TYPE"].ToString();
                        result.TITLE = reader["DESCRIPTION"].ToString();
                        satisfiedResult.Add(result);
                    }
                    reader.Close();
                }

                return satisfiedResult;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }

        }

        public List<SatisfiedResult> GetSupportingData(string answerID)
        {
            List<SatisfiedResult> satisfiedResult = null;
            try
            {

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_data WHERE ID_CONTENT_ANSWER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", answerID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    satisfiedResult = new List<SatisfiedResult>();
                    while (reader.Read())
                    {
                        SatisfiedResult result = new SatisfiedResult();
                        result.PATH = Convert.ToString(reader["PATH"]);
                        result.TYPE = reader["ID_CONTENT_TYPE"].ToString();
                        result.TITLE = reader["DESCRIPTION"].ToString();
                        satisfiedResult.Add(result);
                    }
                    reader.Close();
                }

                return satisfiedResult;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }
    }

    public class SatisfiedResult
    {
        public string PATH { get; set; }
        public string TYPE { get; set; }
        public string TITLE { get; set; }
    }
}