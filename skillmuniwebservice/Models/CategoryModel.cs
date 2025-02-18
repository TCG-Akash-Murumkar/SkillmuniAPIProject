using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CategoryModel
    {
        private MySqlConnection connection = null;

        public CategoryModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public List<Category> GetCategoryDetails(int orgID)
        {
            try
            {
                List<Category> categoryList = null;

                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category_tiles where STATUS = 'A' and  ID_ORGANIZATION = @value1 ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", orgID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    categoryList = new List<Category>();

                    while (reader.Read())
                    {
                        Category categoryDetails = new Category();
                        categoryDetails.CategoryID = Convert.ToInt32(reader["ID_CATEGORY"]);
                        categoryDetails.CategoryName = reader["CATEGORYNAME"].ToString();
                        categoryDetails.CategoryDescription = reader["DESCRIPTION"].ToString();
                        categoryDetails.OrganisationId = Convert.ToInt32(reader["ID_ORGANIZATION"].ToString());
                        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + reader["IMAGE_PATH"].ToString();
                        categoryDetails.Is_Primary = 1;
                        categoryDetails.SubCount = new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());
                        categoryList.Add(categoryDetails);
                    }
                    reader.Close();
                }

                return categoryList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }


        public Category GetCategoryValue(int categoryId)
        {
            try
            {
                Category categoryList = null;


                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category where STATUS = 'A' and ID_CATEGORY = @value1 ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", categoryId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    categoryList = new Category();
                    while (reader.Read())
                    {
                        Category categoryDetails = new Category();
                        categoryDetails.CategoryID = Convert.ToInt32(reader["ID_CATEGORY"]);
                        categoryDetails.CategoryName = reader["CATEGORYNAME"].ToString();
                        categoryDetails.CategoryDescription = reader["DESCRIPTION"].ToString();
                        categoryDetails.OrganisationId = Convert.ToInt32(reader["ID_ORGANIZATION"].ToString());
                        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + reader["IMAGE_PATH"].ToString();
                        categoryDetails.Is_Primary = Convert.ToInt32(reader["ID_CATEGORY"].ToString());
                        categoryDetails.Is_Program = Convert.ToInt32(reader["CATEGORY_TYPE"].ToString());
                        categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(reader["COUNT_REQUIRED"].ToString());
                        categoryDetails.SubCount = new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());
                        categoryList = categoryDetails;
                    }
                    reader.Close();
                }

                return categoryList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }

        public int getContentInCategoryCount(string sql)
        {
            try
            {
                int count = 0;
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader["COUNT"]);
                    }
                    reader.Close();
                }

                return count;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }
        public List<Category> getCatgegoryFromHeading(int orgID, int cid, string sql)
        {
            try
            {
                List<Category> categoryList = null;


                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category where STATUS = 'A' and  ID_ORGANIZATION = @value1 and ID_PARENT=@value2 and upper(SUB_HEADING) LIKE upper('%" + sql + "%')  and IS_PRIMARY=0";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", orgID);
                command.Parameters.AddWithValue("value2", cid);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    categoryList = new List<Category>();

                    while (reader.Read())
                    {
                        Category categoryDetails = new Category();
                        categoryDetails.CategoryID = Convert.ToInt32(reader["ID_CATEGORY"]);
                        categoryDetails.CategoryName = reader["CATEGORYNAME"].ToString();
                        categoryDetails.CategoryDescription = reader["DESCRIPTION"].ToString();
                        categoryDetails.OrganisationId = Convert.ToInt32(reader["ID_ORGANIZATION"].ToString());
                        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + reader["IMAGE_PATH"].ToString();
                        categoryDetails.Is_Primary = 0;
                        categoryDetails.CategoryHeader = reader["SUB_HEADING"].ToString();
                        categoryDetails.SubCount = new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());
                        categoryDetails.ORDERID = Convert.ToInt32(reader["SUB_HEADING"].ToString());
                        categoryList.Add(categoryDetails);
                    }
                    reader.Close();
                }

                return categoryList;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }
        public int getSubCount(string id)
        {
            int ret = 0;
            try
            {


                MySqlCommand command = null;
                string query = "SELECT count(*) subcount FROM tbl_category where STATUS = 'A' and  ID_PARENT = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", id);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        ret = Convert.ToInt32(reader["subcount"]);

                    }
                    reader.Close();
                }


            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }


            return ret;
        }

        public CategoryResponce GetCategory(string str)
        {
            CategoryResponce categoryList = null;
            try
            {


                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category where STATUS = 'A' and  ID_CATEGORY = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", str);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    categoryList = new CategoryResponce();

                    while (reader.Read())
                    {
                        CategoryResponce categoryDetails = new CategoryResponce();
                        categoryDetails.CategoryID = Convert.ToInt32(reader["ID_CATEGORY"]);
                        categoryDetails.CategoryName = reader["CATEGORYNAME"].ToString();
                        categoryDetails.CategoryDescription = reader["DESCRIPTION"].ToString();
                        categoryDetails.OrganisationId = Convert.ToInt32(reader["ID_ORGANIZATION"].ToString());

                        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + reader["IMAGE_PATH"].ToString();
                        categoryList = categoryDetails;
                    }
                    reader.Close();
                }


            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

            return categoryList;
        }

        public List<CategoryResponce> GetNewCategory(string str, string orgID)
        {
            List<CategoryResponce> categoryList = null;
            try
            {


                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_category where STATUS = 'A' and  ID_CATEGORY not in (" + str + ") and ID_ORGANIZATION=@value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", orgID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    categoryList = new List<CategoryResponce>();

                    while (reader.Read())
                    {
                        CategoryResponce categoryDetails = new CategoryResponce();
                        categoryDetails.CategoryID = Convert.ToInt32(reader["ID_CATEGORY"]);
                        categoryDetails.CategoryName = reader["CATEGORYNAME"].ToString();
                        categoryDetails.CategoryDescription = reader["DESCRIPTION"].ToString();
                        categoryDetails.OrganisationId = Convert.ToInt32(reader["ID_ORGANIZATION"].ToString());
                        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + reader["IMAGE_PATH"].ToString();
                        categoryList.Add(categoryDetails);
                    }
                    reader.Close();
                }


            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

            return categoryList;
        }

    }

    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public string CategoryHeader { get; set; }

        public string CategoryImagePath { get; set; }

        public int OrganisationId { get; set; }

        public int Is_Primary { get; set; }

        public int SubCount { get; set; }

        public int ORDERID { get; set; }

        public int Is_Program { get; set; }

        public string ExpiryDate { get; set; }

        public int ContentCount { get; set; }

        public int CategoryType { get; set; }

        public int IS_COUNT_REQUIRED { get; set; }

        public string NEXTURL { get; set; }

        public int LINKCOUNT { get; set; }

    }



    public class CategoryTile
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public string CategoryHeader { get; set; }

        public string CategoryImagePath { get; set; }

        public int OrganisationId { get; set; }

        public int Is_Primary { get; set; }

        public int SubCount { get; set; }

        public int ORDERID { get; set; }

        public string Template { get; set; }

        public string NEXTAPI { get; set; }
    }

    public class CategoryResponce
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public string CategoryImagePath { get; set; }

        public int OrganisationId { get; set; }

        public string Status { get; set; }
    }

    public class DisplayCategory
    {
        public string Heading { get; set; }
        public List<Category> Categories { get; set; }
        public string Order { get; set; }
        public int HeadingID { get; set; }

    }
    public class AssignmentCategory
    {
        public string MONTH { get; set; }
        public List<DisplayCategory> assigment { get; set; }
        public int ORDER { get; set; }
    }
    public class CateogryDetails
    {
        public string Heading { get; set; }
        public List<Category> Categories { get; set; }
        public string Order { get; set; }
        public List<SearchResponce> Contents { get; set; }
    }
    public class DisplayMyPlaylist
    {
        public string id_heading { get; set; }
        public string Heading { get; set; }
        public List<Category> Categories { get; set; }
        public string Order { get; set; }
    }

}