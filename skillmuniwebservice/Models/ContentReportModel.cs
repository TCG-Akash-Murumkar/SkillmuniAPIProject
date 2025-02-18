using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
   

    public class ContentReportModel2
    {
        private MySqlConnection conn = null;

        public ContentReportModel2()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }

        public List<ContentLike> getContentLikes(string str)
        {

            List<ContentLike> result = new List<ContentLike>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLike pd = new ContentLike();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.LIKES = Convert.ToInt32(reader["LikeCount"].ToString());
                    pd.DISLIKES = Convert.ToInt32(reader["DisLikeCount"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.ENDDATE = reader["EXPIRY_DATE"].ToString();
                    pd.LASTACCESS = reader["LASTACCESS"].ToString();
                    pd.CONTENT = reader["CONTENT_QUESTION"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<string> getLocationList(int oid, string lAdd)
        {

            List<string> result = new List<string>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = "select Distinct LOCATION from tbl_profile where id_user in (select id_user from tbl_role_user_mapping where id_organization=" + oid + lAdd + ")";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string loc = reader["LOCATION"].ToString();
                    if (!string.IsNullOrEmpty(loc))
                    {
                        result.Add(loc);
                    }

                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLike> getContentAccess(string str)
        {

            List<ContentLike> result = new List<ContentLike>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLike pd = new ContentLike();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.LIKES = 0;
                    pd.DISLIKES = 0;
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.ENDDATE = reader["EXPIRY_DATE"].ToString();
                    pd.LASTACCESS = reader["LASTACCESS"].ToString();
                    pd.CONTENT = reader["CONTENT_QUESTION"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public MonthData getContentCount(string str)
        {
            MonthData count = new MonthData();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count.LIKES = Convert.ToInt32(reader["LIKES"].ToString());
                    count.DISLIKES = Convert.ToInt32(reader["DISLIKES"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }


            return count;
        }

        public List<ContentLocationWise> getLocationWiseContentAccess(string str)
        {

            List<ContentLocationWise> result = new List<ContentLocationWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationWise pd = new ContentLocationWise();
                    pd.ID_USER = Convert.ToInt32(reader["id_user"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.USERID = reader["USERID"].ToString();
                    pd.FIRSTNAME = reader["FIRSTNAME"].ToString();
                    pd.LASTNAME = reader["LASTNAME"].ToString();
                    pd.LOCATION = reader["LOCATION"].ToString().ToUpper();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

    }

    public class ContentReportModel1
    {
        private MySqlConnection conn = null;

        public ContentReportModel1()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }

        public List<tbl_user> get_user_organization(string org_id)
        {
            List<tbl_user> result = new List<tbl_user>();
            try
            {
                MySqlCommand command = null;
                string query = "select b.USERID,b.ID_USER from tbl_organization a,tbl_user b,tbl_role c ";
                query += " where a.ID_ORGANIZATION = @value1  AND b.ID_ROLE = c.ID_ROLE and c.ID_ORGANIZATION = a.ID_ORGANIZATION and b.STATUS ='A' ";
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@value1", org_id);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tbl_user pd = new tbl_user();
                    pd.ID_USER = reader.GetInt32(reader.GetOrdinal("ID_USER"));
                    pd.USERID = reader["USERID"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentReport> getContentReportfilterlist(string query)
        {
            List<ContentReport> result = new List<ContentReport>();
            try
            {
                conn.Open();
                string SQL = query;

                MySqlCommand command = new MySqlCommand(SQL, conn);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    ContentReport add = new ContentReport();
                    add.ID_USER = dr.GetInt32(dr.GetOrdinal("ID_USER"));
                    add.USERID = dr["USERID"].ToString();
                    add.content_name = dr["CONTENT_QUESTION"].ToString();
                    add.orgnization_name = dr["ORGANIZATION_NAME"].ToString();
                    add.created_dated = Convert.ToDateTime(dr["UPDATED_DATE_TIME"].ToString());
                    add.expity_date = Convert.ToDateTime(dr["EXPIRY_DATE"].ToString());
                    add.count_accessed = dr.GetInt32(dr.GetOrdinal("CONTENT_COUNTER"));
                    result.Add(add);
                }
            }
            catch (Exception e)
            {
                throw (e);
                //Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentReport> getContentOptionfilterlist(string query)
        {
            List<ContentReport> result = new List<ContentReport>();
            try
            {
                conn.Open();
                string SQL = query;

                MySqlCommand command = new MySqlCommand(SQL, conn);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    ContentReport add = new ContentReport();
                    add.ID_USER = dr.GetInt32(dr.GetOrdinal("ID_USER"));
                    add.USERID = dr["USERID"].ToString();
                    add.content_name = dr["CONTENT_QUESTION"].ToString();
                    add.created_dated = Convert.ToDateTime(dr["UPDATED_DATE_TIME"].ToString());
                    add.expity_date = Convert.ToDateTime(dr["EXPIRY_DATE"].ToString());
                    add.countflag = Convert.ToInt32(dr["count"].ToString());
                    result.Add(add);
                }
            }
            catch (Exception e)
            {
                throw (e);
                //Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentReport> getContentLoc(string query)
        {
            List<ContentReport> result = new List<ContentReport>();
            try
            {
                conn.Open();
                string SQL = query;

                MySqlCommand command = new MySqlCommand(SQL, conn);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    ContentReport add = new ContentReport();
                    add.location = dr["LOCATION"].ToString();
                    add.username = dr["FIRSTNAME"].ToString();
                    add.ID_USER = dr.GetInt32(dr.GetOrdinal("ID_USER"));
                    result.Add(add);
                }
            }
            catch (Exception e)
            {
                throw (e);
                //Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<usersdetails> getContentTopUser(string query)
        {
            List<usersdetails> result = new List<usersdetails>();
            try
            {
                conn.Open();
                string SQL = query;

                MySqlCommand command = new MySqlCommand(SQL, conn);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    usersdetails add = new usersdetails();
                    add.ID_USER = dr.GetInt32(dr.GetOrdinal("ID_USER"));
                    add.count = dr.GetInt32(dr.GetOrdinal("Lcount"));
                    //add.listuserdetails = new ContentReportModel().get(Convert.ToString(add.countflag));

                    result.Add(add);
                }
            }
            catch (Exception e)
            {
                throw (e);
                //Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }
    }

    public class ContentReport
    {
        public int ID_USER { get; set; }
        public string USERID { get; set; }
        public string content_name { get; set; }
        public string orgnization_name { get; set; }
        public DateTime created_dated { get; set; }
        public DateTime expity_date { get; set; }
        public DateTime lastaccess_date { get; set; }
        public int count_accessed { get; set; }
        public string flag { get; set; }
        public int countflag { get; set; }
        public string location { get; set; }
        public string username { get; set; }
        public List<usersdetails> listuserdetails = new List<usersdetails>();
    }

    public class usersdetails
    {
        public int ID_USER { get; set; }
        public string username { get; set; }
        public string location { get; set; }
        public int count { get; set; }
    }

    public class locationbyuser
    {
        public string location { get; set; }
        public List<usercount> counter { get; set; }
    }

    public class usercount
    {
        public tbl_user user { get; set; }
        public tbl_profile profile { get; set; }
        public int count { get; set; }
    }

    public class ContentLike
    {
        public string CONTENT { get; set; }
        public int LIKES { get; set; }
        public int ID_CONTENT { get; set; }
        public int DISLIKES { get; set; }
        public int CONTENTACCESS { get; set; }
        public string CREATEDDATE { get; set; }
        public string ENDDATE { get; set; }
        public string LASTACCESS { get; set; }

    }

    public class ContentLocationWise
    {
        public string USERID { get; set; }
        public int ID_USER { get; set; }
        public int CONTENTACCESS { get; set; }
        public string LOCATION { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public int ID_CONTENT { get; set; }
    }

    public class ContentLocationGenderWise
    {
        public string USERID { get; set; }
        public int ID_USER { get; set; }
        public int CONTENTACCESS { get; set; }
        public int ID_CONTENT { get; set; }
        public string CONTENT_QUESTION { get; set; }
        public string LOCATION { get; set; }
        public int MALE { get; set; }
        public int FEMALE { get; set; }
        public string DESIGNATION { get; set; }
    }

    public class MonthData
    {
        public int LIKES { get; set; }
        public int DISLIKES { get; set; }
    }

    public class ContentReportModel
    {
        private MySqlConnection conn = null;

        public ContentReportModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }

        public List<ContentLike> getContentLikes(string str)
        {

            List<ContentLike> result = new List<ContentLike>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLike pd = new ContentLike();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.LIKES = Convert.ToInt32(reader["LikeCount"].ToString());
                    pd.DISLIKES = Convert.ToInt32(reader["DisLikeCount"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.ENDDATE = reader["EXPIRY_DATE"].ToString();
                    pd.LASTACCESS = reader["LASTACCESS"].ToString();
                    pd.CONTENT = reader["CONTENT_QUESTION"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLocationWise> getContentLocation(string str)
        {

            List<ContentLocationWise> result = new List<ContentLocationWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationWise pd = new ContentLocationWise();
                    pd.LOCATION = reader["LOCATION"].ToString();
                    pd.FIRSTNAME = reader["FIRSTNAME"].ToString();
                    pd.LASTNAME = reader["LASTNAME"].ToString();
                    pd.USERID = reader["USERID"].ToString();
                    pd.ID_USER = Convert.ToInt32(reader["ID_USER"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLocationGenderWise> getContentLocationGender(string str)
        {

            List<ContentLocationGenderWise> result = new List<ContentLocationGenderWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationGenderWise pd = new ContentLocationGenderWise();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.CONTENT_QUESTION = reader["CONTENT_QUESTION"].ToString();
                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    pd.MALE = Convert.ToInt32(reader["MALE"].ToString());
                    pd.FEMALE = Convert.ToInt32(reader["FEMALE"].ToString());

                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLocationGenderWise> getLocationGenderAccess(string str)
        {

            List<ContentLocationGenderWise> result = new List<ContentLocationGenderWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationGenderWise pd = new ContentLocationGenderWise();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.CONTENT_QUESTION = reader["CONTENT_QUESTION"].ToString();
                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    pd.MALE = Convert.ToInt32(reader["MALE"].ToString());
                    pd.FEMALE = Convert.ToInt32(reader["FEMALE"].ToString());
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLocationGenderWise> getDesignationAccess(string str)
        {

            List<ContentLocationGenderWise> result = new List<ContentLocationGenderWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationGenderWise pd = new ContentLocationGenderWise();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.CONTENT_QUESTION = reader["CONTENT_QUESTION"].ToString();
                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    pd.DESIGNATION = reader["DESIGNATION"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<string> getLocationList(int oid, string lAdd)
        {

            List<string> result = new List<string>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = "select Distinct LOCATION from tbl_profile where id_user in (select id_user from tbl_role_user_mapping where id_organization=" + oid + lAdd + ")";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string loc = reader["LOCATION"].ToString();
                    if (!string.IsNullOrEmpty(loc))
                    {
                        result.Add(loc);
                    }

                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLike> getContentAccess(string str)
        {

            List<ContentLike> result = new List<ContentLike>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLike pd = new ContentLike();
                    pd.ID_CONTENT = Convert.ToInt32(reader["id_content"].ToString());
                    pd.LIKES = 0;
                    pd.DISLIKES = 0;
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.ENDDATE = reader["EXPIRY_DATE"].ToString();
                    pd.LASTACCESS = reader["LASTACCESS"].ToString();
                    pd.CONTENT = reader["CONTENT_QUESTION"].ToString();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public List<ContentLocationWise> getLocationAccess(string str)
        {

            List<ContentLocationWise> result = new List<ContentLocationWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationWise pd = new ContentLocationWise();
                    pd.LOCATION = reader["LOCATION"].ToString();
                    pd.FIRSTNAME = reader["FIRSTNAME"].ToString();
                    pd.LASTNAME = reader["LASTNAME"].ToString();
                    pd.USERID = reader["USERID"].ToString();
                    pd.ID_USER = Convert.ToInt32(reader["id_user"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["Contentaccess"].ToString());
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        public MonthData getLocationCount(string str)
        {
            MonthData count = new MonthData();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count.LIKES = Convert.ToInt32(reader["ContentAccess"].ToString());

                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }


            return count;
        }

        public MonthData getLocationGenderCount(string str)
        {
            MonthData count = new MonthData();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count.LIKES = Convert.ToInt32(reader["MALE"].ToString());
                    count.DISLIKES = Convert.ToInt32(reader["FEMALE"].ToString());

                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }


            return count;
        }

        public MonthData getContentCount(string str)
        {
            MonthData count = new MonthData();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count.LIKES = Convert.ToInt32(reader["LIKES"].ToString());
                    count.DISLIKES = Convert.ToInt32(reader["DISLIKES"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }


            return count;
        }

        public List<ContentLocationWise> getLocationWiseContentAccess(string str)
        {

            List<ContentLocationWise> result = new List<ContentLocationWise>();
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ContentLocationWise pd = new ContentLocationWise();
                    pd.ID_USER = Convert.ToInt32(reader["id_user"].ToString());
                    pd.CONTENTACCESS = Convert.ToInt32(reader["COUNTER"].ToString());
                    pd.USERID = reader["USERID"].ToString();
                    pd.FIRSTNAME = reader["FIRSTNAME"].ToString();
                    pd.LASTNAME = reader["LASTNAME"].ToString();
                    pd.LOCATION = reader["LOCATION"].ToString().ToUpper();
                    result.Add(pd);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }

        /*-----------------------------------------------------------------------------------------*/
        public int getRecordCount(string str)
        {

            int result = 0;
            try
            {
                MySqlCommand command = null;

                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["count"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                conn.Close();
                conn = null;
            }
            return result;
        }



    }

}