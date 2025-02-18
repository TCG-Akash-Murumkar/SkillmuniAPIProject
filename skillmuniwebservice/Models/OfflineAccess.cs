using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public  class OfflineAccess
    {
      
        private  MySqlConnection connection = null;


        public OfflineAccess()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public  DateTime GetExpiryDate(string user)
        {
            try
            {
                string expiry = null;
                DateTime expirydate = new DateTime();
              
                MySqlCommand command = null;
                string query = "SELECT EXPIRY FROM tbl_sethu_expiry WHERE ID_USER in (SELECT id_user FROM tbl_user WHERE USERID = @value1)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", user);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //expiry = reader["EXPIRY"].ToString();
                        expirydate = reader.GetDateTime(reader.GetOrdinal("EXPIRY"));
                    }
                    //expirydate = DateTime.ParseExact(expiry, "dd-MM-yyyy hh:mm:ss", null);
                }

                reader.Close();
                return expirydate;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }
        public  List<OfflineOrgAssociation> GetOrgAssociation(int organizationID)
        {
            try
            {
                List<OfflineOrgAssociation> orgAssociationList = new List<OfflineOrgAssociation>();
              
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineOrgAssociation association = new OfflineOrgAssociation();
                    association.ID_ASSOCIATION = reader.GetInt32(reader.GetOrdinal("ID_ASSOCIATION")).ToString();
                    association.ID_ORGANIZATION = reader.GetInt32(reader.GetOrdinal("ID_ORGANIZATION")).ToString();
                    association.ID_CATEGORY = reader.GetInt32(reader.GetOrdinal("ID_CATEGORY")).ToString();
                    association.ID_CONTENT = reader.GetInt32(reader.GetOrdinal("ID_CONTENT")).ToString();
                    association.CONTENT_COUNTER_CLICK = reader.GetInt32(reader.GetOrdinal("CONTENT_COUNTER_CLICK")).ToString();
                    association.CONTENT_COUNTER_LIKE = reader.GetInt32(reader.GetOrdinal("CONTENT_COUNTER_LIKE")).ToString();
                    association.ID_CONTENT_LEVEL = reader.GetInt32(reader.GetOrdinal("ID_CONTENT_LEVEL")).ToString();
                    orgAssociationList.Add(association);
                }
                reader.Close();
                return orgAssociationList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  List<OfflineCategory> GetCategory(int organizationID)
        {
            try
            {
                List<OfflineCategory> categoryList = new List<OfflineCategory>();
               
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category WHERE STATUS = @value1 AND ID_CATEGORY IN (SELECT DISTINCT ID_CATEGORY FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value2)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                command.Parameters.AddWithValue("value2", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineCategory category = new OfflineCategory();
                    category.ID_CATEGORY = reader.GetInt32(reader.GetOrdinal("ID_CATEGORY")).ToString();
                    category.CATEGORYNAME = reader["CATEGORYNAME"].ToString();
                    category.DESCRIPTION = reader["DESCRIPTION"].ToString();
                    category.IMAGE_PATH = reader["IMAGE_PATH"].ToString();
                    categoryList.Add(category);
                }
                reader.Close();
                return categoryList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  List<OfflineContent> GetContent(int organizationID)
        {
            try
            {
                List<OfflineContent> contentList = new List<OfflineContent>();
               
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content WHERE STATUS = @value1 AND ID_CONTENT IN (SELECT DISTINCT ID_content FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value2)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                command.Parameters.AddWithValue("value2", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineContent content = new OfflineContent();
                    content.ID_CONTENT = reader.GetInt32(reader.GetOrdinal("ID_CONTENT")).ToString();
                    content.CONTENT_QUESTION = reader["CONTENT_QUESTION"].ToString();
                    content.CONTENT_COUNTER = reader.GetInt32(reader.GetOrdinal("CONTENT_COUNTER")).ToString();
                    contentList.Add(content);
                }
                reader.Close();
                return contentList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  List<OfflineContentAnswer> GetContentAnswer(int organizationID)
        {
            try
            {
                List<OfflineContentAnswer> contentAnswerList = new List<OfflineContentAnswer>();
               
                MySqlCommand command = null;
                string query = "SELECT * FROM db_skillmuni.tbl_content_answer where STATUS = @value1 AND ID_CONTENT IN (SELECT DISTINCT ID_CONTENT FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value2)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                command.Parameters.AddWithValue("value2", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineContentAnswer contentAnswer = new OfflineContentAnswer();
                    contentAnswer.ID_CONTENT_ANSWER = reader.GetInt32(reader.GetOrdinal("ID_CONTENT_ANSWER")).ToString();
                    contentAnswer.ID_CONTENT = reader.GetInt32(reader.GetOrdinal("ID_CONTENT")).ToString();
                    contentAnswer.CONTENT_ANSWER = reader["CONTENT_ANSWER"].ToString();
                    contentAnswer.CONTENT_ANSWER_COUNTER = reader.GetInt32(reader.GetOrdinal("CONTENT_ANSWER_COUNTER")).ToString();
                    contentAnswerList.Add(contentAnswer);
                }
                reader.Close();
                return contentAnswerList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  List<OfflineContentMetadata> GetContentMetadata(int organizationID)
        {
            try
            {
                List<OfflineContentMetadata> contentMetadataList = new List<OfflineContentMetadata>();
               
                MySqlCommand command = null;
                string query = "SELECT * FROM db_skillmuni.tbl_content_metadata WHERE STATUS = @value1 AND ID_CONTENT_ANSWER IN (SELECT ID_CONTENT_ANSWER FROM db_skillmuni.tbl_content_answer where STATUS = @value2 AND ID_CONTENT IN (SELECT DISTINCT ID_CONTENT FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value3))";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                command.Parameters.AddWithValue("value2", "A");
                command.Parameters.AddWithValue("value3", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineContentMetadata contentMetadata = new OfflineContentMetadata();
                    contentMetadata.ID_CONTENT_METADATA = reader.GetInt32(reader.GetOrdinal("ID_CONTENT_METADATA")).ToString();
                    contentMetadata.ID_CONTENT_ANSWER = reader.GetInt32(reader.GetOrdinal("ID_CONTENT_ANSWER")).ToString();
                    contentMetadata.CONTENT_METADATA = reader["CONTENT_METADATA"].ToString();
                    contentMetadata.CONTENT_METADATA_COUNTER = reader.GetInt32(reader.GetOrdinal("CONTENT_METADATA_COUNTER")).ToString();
                    contentMetadataList.Add(contentMetadata);
                }
                reader.Close();
                return contentMetadataList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }


        public  List<OfflineAnswerSteps> GetAnswerSteps(int organizationID)
        {
            try
            {
                List<OfflineAnswerSteps> answerStepsList = new List<OfflineAnswerSteps>();
              
                MySqlCommand command = null;
                string query = "SELECT * FROM db_skillmuni.tbl_answer_steps WHERE STATUS = @value1 AND ID_CONTENT_ANSWER IN (SELECT ID_CONTENT_ANSWER FROM db_skillmuni.tbl_content_answer where STATUS = @value2 AND ID_CONTENT IN (SELECT DISTINCT ID_CONTENT FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value3))";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", "A");
                command.Parameters.AddWithValue("value2", "A");
                command.Parameters.AddWithValue("value3", organizationID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    OfflineAnswerSteps answerSteps = new OfflineAnswerSteps();
                    answerSteps.ID_ANSWER_STEP = reader.GetInt32(reader.GetOrdinal("ID_ANSWER_STEP")).ToString();
                    answerSteps.ID_CONTENT_ANSWER = reader.GetInt32(reader.GetOrdinal("ID_CONTENT_ANSWER")).ToString();
                    answerSteps.STEPNO = reader.GetInt32(reader.GetOrdinal("STEPNO")).ToString();
                    answerSteps.ANSWER_STEPS = reader["ANSWER_STEPS"].ToString();
                    answerStepsList.Add(answerSteps);
                }
                reader.Close();
                return answerStepsList;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

    }


    public class OfflineContentMetadata
    {
        public string ID_CONTENT_METADATA { get; set; }

        public string CONTENT_METADATA { get; set; }

        public string CONTENT_METADATA_COUNTER { get; set; }

        public string ID_CONTENT_ANSWER { get; set; }

    }


    public class OfflineContentAnswer
    {
        public string ID_CONTENT_ANSWER { get; set; }

        public string ID_CONTENT { get; set; }

        public string CONTENT_ANSWER { get; set; }

        public string CONTENT_ANSWER_COUNTER { get; set; }

    }

    public class OfflineContent
    {
        public string ID_CONTENT { get; set; }

        public string CONTENT_QUESTION { get; set; }

        public string CONTENT_COUNTER { get; set; }

    }


    public class OfflineCategory
    {
        public string ID_CATEGORY { get; set; }

        public string CATEGORYNAME { get; set; }

        public string DESCRIPTION { get; set; }

        public string IMAGE_PATH { get; set; }

    }


    public class OfflineAnswerSteps
    {

        public string ID_ANSWER_STEP { get; set; }

        public string ID_CONTENT_ANSWER{ get; set; }

        public string STEPNO { get; set; }

        public string ANSWER_STEPS { get; set; }

       

    }

    public class OfflineOrgAssociation
    {
        public string ID_ASSOCIATION { get; set;}

        public string ID_ORGANIZATION { get; set; }

        public string ID_CATEGORY { get; set; }

        public string ID_CONTENT { get; set; }

        public string CONTENT_COUNTER_CLICK { get; set; }

        public string CONTENT_COUNTER_LIKE { get; set; }

        public string ID_CONTENT_LEVEL { get; set; }

    }
}