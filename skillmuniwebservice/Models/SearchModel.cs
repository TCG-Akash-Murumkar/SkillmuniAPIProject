using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public  class SearchModel
    {       
        private  MySqlConnection connection = null;


        public SearchModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public  Content GetContentDetail(string contentID)
        {
            try
            {
                Content result = null;
              
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content WHERE ID_CONTENT=@value1 ORDER BY CONTENT_QUESTION";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", contentID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    result = new Content();
                    while (reader.Read())
                    {
                        result.ID_CONTENT = Convert.ToInt32(reader["ID_CONTENT"]);
                        result.CONTENT_QUESTION = reader["CONTENT_QUESTION"].ToString();
                        result.ID_USER = Convert.ToInt32(reader["ID_USER"]);
                    }
                    reader.Close();
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

        public  List<string> GetContentAnswer(string contentID)
        {
            try
            {
                List<string> content = null;
               
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_answer WHERE ID_CONTENT=@value1  ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", contentID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    content = new List<string>();
                    while (reader.Read())
                    {
                        string result = reader["ID_CONTENT_ANSWER"].ToString();
                        content.Add(result);
                    }
                    reader.Close();
                }
                return content;
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

        public  List<ContentAnswer> GetContentAnswerDetail(string answerID)
        {
            try
            {
                List<ContentAnswer> answerList = null;
               
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_answer WHERE ID_CONTENT_ANSWER IN(" + answerID + ") ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    answerList = new List<ContentAnswer>();
                    while (reader.Read())
                    {
                        ContentAnswer result = new ContentAnswer();
                        result.ID_CONTENT_ANSWER = Convert.ToInt32(reader["ID_CONTENT_ANSWER"]);
                        result.CONTENT_ANSWER = reader["CONTENT_ANSWER1"].ToString();
                        answerList.Add(result);
                    }
                    reader.Close();
                }
                return answerList;
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
        public  string GetCategoryLabel(string categoryID)
        {
            try
            {
                string CategoryName = null;
              
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category WHERE ID_CATEGORY= @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", categoryID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CategoryName = reader["CATEGORYNAME"].ToString();
                }
                reader.Close();

                return CategoryName;
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

        public  List<string> GetContentId(string answerID)
        {
            try
            {
                List<string> content = null;
              
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_content_answer WHERE ID_CONTENT_ANSWER IN(" + answerID + ") ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    content = new List<string>();
                    while (reader.Read())
                    {
                        string result = reader["ID_CONTENT"].ToString();
                        content.Add(result);
                    }
                    reader.Close();
                }
                return content;
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

        public  List<ContentAssociation> GetApprovedContentId(string contentID, string categoryID, string OrgId)
        {
            try
            {
                List<ContentAssociation> contentAssociation = null;
               
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                if (categoryID.Equals("0"))
                {
                    string query = "SELECT * FROM tbl_content WHERE ID_CONTENT IN(" + contentID + ") AND ID_ORGANIZATION=@value1 ";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("value1", OrgId);
                }
                else
                {
                    string query = "SELECT * FROM tbl_content WHERE ID_CONTENT IN(" + contentID + ") AND ID_ORGANIZATION=@value1 AND ID_CATEGORY=@value2 ";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("value1", OrgId);
                    command.Parameters.AddWithValue("value2", categoryID);
                }
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    contentAssociation = new List<ContentAssociation>();
                    while (reader.Read())
                    {
                        ContentAssociation res = new ContentAssociation();
                        res.ID_CONTENT = Convert.ToInt32(reader["ID_CONTENT"]);
                        res.ID_CATEGORY = Convert.ToInt32(reader["ID_CATEGORY"]);
                        res.ID_ORGANIZATION = Convert.ToInt32(reader["ID_ORGANIZATION"]);                     
                        res.ID_CONTENT_LEVEL = Convert.ToInt32(reader["ID_CONTENT_LEVEL"]);
                        contentAssociation.Add(res);
                    }
                    reader.Close();
                }
                return contentAssociation;
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
        public  List<string> GetsearchPattern(List<string> metadata)
        {
            List<string> metadataList = null;
            try
            {
                foreach (string ptrn in metadata)
                {
                    if (String.IsNullOrWhiteSpace(ptrn) || String.IsNullOrEmpty(ptrn))
                    {

                    }
                    else
                    {
                      
                        MySqlCommand command = null;
                        string query = "SELECT * FROM tbl_content_metadata WHERE LOWER(CONTENT_METADATA) LIKE '%" + ptrn + "%' ";
                        connection.Open();
                        command = connection.CreateCommand();
                        command.CommandText = query;
                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            metadataList = new List<string>();
                            while (reader.Read())
                            {
                                string result = reader["ID_CONTENT_ANSWER"].ToString();
                                metadataList.Add(result);
                            }
                            reader.Close();
                        }

                        connection.Close();
                    }
                }

                return metadataList;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {

            }
        }

        public  List<AnswerSteps> GetStepsForAnswer(int answerID)
        {
            try
            {
                List<AnswerSteps> stepsList = null;
                string status = "A";
              
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_answer_steps WHERE ID_CONTENT_ANSWER = @value1 and STATUS = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", answerID);
                command.Parameters.AddWithValue("value2", status);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    stepsList = new List<AnswerSteps>();
                    while (reader.Read())
                    {
                        AnswerSteps steps = new AnswerSteps();
                        steps.StepsID = Convert.ToInt32(reader["ID_ANSWER_STEP"]);
                        steps.StepNO = Convert.ToInt32(reader["STEPNO"]);
                        steps.StepAnswer = reader["ANSWER_STEPS"].ToString();
                        stepsList.Add(steps);
                    }
                    reader.Close();
                }
                return stepsList;
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
        public  List<NewAnswerSteps> NewGetStepsForAnswer(int answerID, int orgId)
        {
            db_m2ostEntities db = new db_m2ostEntities();
            try
            {
                List<NewAnswerSteps> stepsList = null;
                string status = "A";
              
                MySqlCommand command = null;
                string query = "SELECT a.*,b.ID_CONTENT FROM tbl_content_answer_steps a,tbl_content_answer b WHERE a.ID_CONTENT_ANSWER = @value1 AND a.ID_CONTENT_ANSWER=b.ID_CONTENT_ANSWER and a.STATUS = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", answerID);
                command.Parameters.AddWithValue("value2", status);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    stepsList = new List<NewAnswerSteps>();
                    while (reader.Read())
                    {
                        int content_id = Convert.ToInt32(reader["ID_CONTENT"].ToString());
                        tbl_content content = db.tbl_content.Find(content_id);
                        NewAnswerSteps steps = new NewAnswerSteps();
                        steps.ID_ANSWER_STEP = Convert.ToInt32(reader["ID_ANSWER_STEP"]);
                        steps.STEPNO = Convert.ToInt32(reader["STEPNO"]);
                        int theme = content.ID_THEME;
                        steps.ID_THEME = Convert.ToInt32(reader["ID_THEME"]);                   
                        steps.ANSWER_STEPS_PART1 = reader["ANSWER_STEPS_PART1"].ToString();                     
                        steps.ANSWER_STEPS_PART2 = reader["ANSWER_STEPS_PART2"].ToString();
                        steps.ANSWER_STEPS_PART3 = reader["ANSWER_STEPS_PART3"].ToString();
                        steps.ANSWER_STEPS_PART4 = reader["ANSWER_STEPS_PART4"].ToString();
                        steps.ANSWER_STEPS_PART5 = reader["ANSWER_STEPS_PART5"].ToString();
                        steps.ANSWER_STEPS_PART6 = reader["ANSWER_STEPS_PART6"].ToString();
                        steps.ANSWER_STEPS_PART7 = reader["ANSWER_STEPS_PART7"].ToString();
                        steps.ANSWER_STEPS_PART8 = reader["ANSWER_STEPS_PART8"].ToString();
                        steps.ANSWER_STEPS_PART9 = reader["ANSWER_STEPS_PART9"].ToString();
                        steps.ANSWER_STEPS_PART10 = reader["ANSWER_STEPS_PART10"].ToString();
                        
                        steps.ANSWER_STEPS_IMG1 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG1"].ToString();
                        steps.ANSWER_STEPS_IMG2 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG2"].ToString();
                        steps.ANSWER_STEPS_IMG3 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG3"].ToString();
                        steps.ANSWER_STEPS_IMG4 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG4"].ToString();
                        steps.ANSWER_STEPS_IMG5 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG5"].ToString();
                        steps.ANSWER_STEPS_IMG6 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG6"].ToString();
                        steps.ANSWER_STEPS_IMG7 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG7"].ToString();
                        steps.ANSWER_STEPS_IMG8 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG8"].ToString();
                        steps.ANSWER_STEPS_IMG9 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG9"].ToString();
                        steps.ANSWER_STEPS_IMG10 = ConfigurationManager.AppSettings["ANSIMAGE"].ToString() + orgId + "/" + reader["ID_CONTENT"].ToString() + "/" + reader["ANSWER_STEPS_IMG10"].ToString();


                        steps.ANSWER_STEPS_BANNER = "";
                        steps.REDIRECTION_URL = "";

                        stepsList.Add(steps);
                    }
                    reader.Close();
                }
                return stepsList;
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
        public  List<ContentAssociation> GetDefaultApprovedContentId(string categoryID, string orgID)
        {

            try
            {
                List<ContentAssociation> contentAssociation = null;
               
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                if (categoryID.Equals("0"))
                {
                    string query = "SELECT * FROM tbl_content WHERE STATUS='A' and IS_PRIMARY=1 LIMIT 5";
                    // string query = "SELECT * FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value2   LIMIT 5";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("value2", orgID);
                }
                else
                {
                    string query = "SELECT * FROM tbl_content_org_association WHERE ID_ORGANIZATION = @value2 AND ID_CATEGORY= @value1 AND ID_CONTENT IN(SELECT ID_CONTENT FROM tbl_content WHERE STATUS='A') LIMIT 5";
                    //string query = "SELECT * FROM tbl_content_org_association WHERE  ID_ORGANIZATION  =@value2 AND ID_CATEGORY= @value1 LIMIT 5";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("value1", categoryID);
                    command.Parameters.AddWithValue("value2", orgID);

                }
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    contentAssociation = new List<ContentAssociation>();
                    while (reader.Read())
                    {
                        ContentAssociation result = new ContentAssociation();
                        result.ID_CONTENT = Convert.ToInt32(reader["ID_CONTENT"]);
                        result.ID_CATEGORY = Convert.ToInt32(reader["ID_CATEGORY"]);
                        //  res.CONTENT_COUNTER = Convert.ToInt32(reader["CONTENT_COUNTER"]);
                        result.ID_ORGANIZATION = 1;                    
                        result.ID_CONTENT_LEVEL = Convert.ToInt32(reader["ID_CONTENT_LEVEL"]);
                        contentAssociation.Add(result);
                    }
                    reader.Close();
                }
                return contentAssociation;
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


    public class SearchResult
    {
        public Content QUESTION { get; set; }
        public List<ContentAnswer> ANSWERS { get; set; }
        public string CATEGORY_ID { get; set; }
        public string CATEGORY_LABEL { get; set; }

    }

    public class Content
    {
        public int ID_CONTENT { get; set; }
        public int ID_USER { get; set; }
        public string CONTENT_QUESTION { get; set; }

    }

    public class ContentAssociation
    {
       
        public int ID_ORGANIZATION { get; set; }
        public int ID_CATEGORY { get; set; }
        public int ID_CONTENT { get; set; }
        public int ID_CONTENT_LEVEL { get; set; }
        public int CONTENT_COUNTER { get; set; }

    }

    public partial class ContentAnswer
    {
        public int ID_CONTENT_ANSWER { get; set; }
        public int ID_CONTENT { get; set; }
        public string CONTENT_ANSWER { get; set; }

        //public int CONTENT_ANSWER_COUNTER { get; set; }        
    }

    public class searchString
    {
        public string patternString { get; set; }
        public string Category { get; set; }
        public string OrganizationId { get; set; }
        public string UserId { get; set; }
        public string AccessRole { get; set; }
    }

    public class PlaylistSearch
    {
        public string patternString { get; set; }
        public string Category { get; set; }
        public string OrganizationId { get; set; }
        public string UserId { get; set; }
    }

    public class searchStringNew
    {
        public string patternString { get; set; }
        public string Category { get; set; }

    }
    public class AnswerSteps
    {
        public int StepsID { get; set; }

        public int StepNO { get; set; }

        public string StepAnswer { get; set; }
    }

    public class NewAnswerSteps
    {
        public int ID_ANSWER_STEP { get; set; }
        public int STEPNO { get; set; }
        public int ID_THEME { get; set; }
        public string ANSWER_STEPS_PART1 { get; set; }
        public string ANSWER_STEPS_PART2 { get; set; }
        public string ANSWER_STEPS_PART3 { get; set; }
        public string ANSWER_STEPS_PART4 { get; set; }
        public string ANSWER_STEPS_PART5 { get; set; }
        public string ANSWER_STEPS_PART6 { get; set; }
        public string ANSWER_STEPS_PART7 { get; set; }
        public string ANSWER_STEPS_PART8 { get; set; }
        public string ANSWER_STEPS_PART9 { get; set; }
        public string ANSWER_STEPS_PART10 { get; set; }
        public string ANSWER_STEPS_IMG1 { get; set; }
        public string ANSWER_STEPS_IMG2 { get; set; }
        public string ANSWER_STEPS_IMG3 { get; set; }
        public string ANSWER_STEPS_IMG4 { get; set; }
        public string ANSWER_STEPS_IMG5 { get; set; }
        public string ANSWER_STEPS_IMG6 { get; set; }
        public string ANSWER_STEPS_IMG7 { get; set; }
        public string ANSWER_STEPS_IMG8 { get; set; }
        public string ANSWER_STEPS_IMG9 { get; set; }
        public string ANSWER_STEPS_IMG10 { get; set; }
        
        public string ANSWER_STEPS_BANNER { get; set; }
        public string REDIRECTION_URL { get; set; }

    }

    public class B2BUser
    {
        public string IMEI { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string OS { get; set; }
        public string Network { get; set; }
        public string OSVersion { get; set; }
        public string Details { get; set; }
        public string REURL { get; set; }
    }

    public class MonoLogin
    {
        public string IMEI { get; set; }
        public string UID { get; set; }
        public string OID { get; set; }
    }
    public class B2BIphone
    {
        public int ORG_ID { get; set; }
        public int ROLE_ID { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
    }
    public class B2BIphoneAuth
    {
        public int ORG_ID { get; set; }
        public int ROLE_ID { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string DEVID { get; set; }
    }
    public class ResponceValue
    {
        public string status { get; set; }
    }

    public class GCMBODY
    {
        public string GCM { get; set; }
        public string UID { get; set; }
        public string OID { get; set; }
    }
}