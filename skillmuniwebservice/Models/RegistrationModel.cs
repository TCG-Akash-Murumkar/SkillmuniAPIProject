using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;


namespace m2ostnextservice.Models
{

    public  class RegistrationModel
    {
       
        private  MySqlConnection connection = null;


        public RegistrationModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
        public  string getOrgLogo(int oid)
        {
            try
            {
                string code = "";
              
                MySqlCommand command = null;
                string query = "SELECT LOGO FROM tbl_organization WHERE id_organization = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", oid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    code = reader["LOGO"].ToString();
                }
                reader.Close();
                if (code == "")
                {
                    code = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "default.png";
                }
                else
                {
                    code = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "ORGLOGO" + "/" + code;
                }

                return code;
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

        public  Random random = new Random();
        public  string RandomString(int length)
        {
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public  string getOrgBanner(int oid)
        {
            try
            {
                string code = "";
               
                MySqlCommand command = null;
                string query = "SELECT Banner_path FROM tbl_organisation_banner WHERE id_organisation = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", oid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    code = reader["Banner_path"].ToString();
                }
                reader.Close();
                if (code == "")
                {
                    //code = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "default.png";
                }
                else
                {
                    code = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BANNERIMG" + "/" + code;
                }
                //code = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "banner.png";
                return code;
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


        public  int getOrgBannerID(int oid)
        {
            try
            {
                int code = 0;
              
                MySqlCommand command = null;
                string query = "SELECT id_organisation_banner FROM tbl_organisation_banner WHERE id_organization = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", oid);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    code = Convert.ToInt32(reader["id_organisation_banner"].ToString());
                }
                reader.Close();

                return code;
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


        public  int UpdateUserDevice(int userID, int deviceType, string deviceID)
        {
            try
            {
                
                MySqlCommand command = null;
                string query = "INSERT INTO tbl_user_device_link (ID_USER, ID_DEVICE_TYPE, DEVICE_ID,STATUS,UPDATED_DATE_TIME) VALUES (@value1, @value2, @value3, @value4,@value5)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                command.Parameters.AddWithValue("value2", deviceType);
                command.Parameters.AddWithValue("value3", deviceID);
                command.Parameters.AddWithValue("value4", "A");
                command.Parameters.AddWithValue("value5", System.DateTime.Now);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }


        }

        public  bool CheckDeviceExist(string deviceName, int roleId)
        {
            try
            {
             
                MySqlCommand command = null;
                string query = "select * from tbl_user_device_link a, tbl_user b where DEVICE_ID = @value1 and b.ID_ROLE = @value2 and a.ID_USER = b.ID_USER;";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", deviceName);
                command.Parameters.AddWithValue("value2", roleId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  int NewCheckUserExist(string userName, int roleID)
        {
            string addition = "";
            if (roleID > 0)
            {
                addition = " and ID_ROLE = " + roleID + "";
            }
            try
            {
                int userID = 0;
              
                MySqlCommand command = null;
                string query = "select ID_USER from tbl_user where USERID = @value1 "+addition+" and STATUS = @value3";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userName);
                command.Parameters.AddWithValue("value2", roleID);
                command.Parameters.AddWithValue("value3", "A");
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userID = Convert.ToInt32(reader["ID_USER"]);
                }
                reader.Close();
                return userID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  int GetPendingUserID(string username, int roleID)
        {
            try
            {
                int userID = 0;
                string status = "P";
               
                MySqlCommand command = null;
                string query = "SELECT ID_USER FROM tbl_user WHERE USERID = @value1 and ID_ROLE = @value2 and STATUS = @value3";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", username);
                command.Parameters.AddWithValue("value2", roleID);
                command.Parameters.AddWithValue("value3", status);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userID = Convert.ToInt32(reader["ID_USER"]);
                }
                reader.Close();
                return userID;
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
        public  int GetActiveUserID(string username, int roleID)
        {
            try
            {
                int userID = 0;
                string status = "A";
               
                MySqlCommand command = null;
                string query = "SELECT ID_USER FROM tbl_user WHERE USERID = @value1 and STATUS = @value3";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", username);                
                command.Parameters.AddWithValue("value3", status);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userID = Convert.ToInt32(reader["ID_USER"]);
                }
                reader.Close();
                return userID;
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
        public  bool CheckUserExist(string userName, int roleID)
        {
            string addition = "";
            if (roleID > 0)
            {
                addition = " and ID_ROLE = " + roleID + "";
            }
            try
            {
               
                MySqlCommand command = null;

                string query = "select * from tbl_user where USERID = @value1 " + addition;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userName);                
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  bool CheckProfileExist(string userName )
        {
          
            try
            {
              
                MySqlCommand command = null;

                string query = "select * from tbl_profile where USERID = @value1 ";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userName);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  bool CheckDeviceStatus(Login login)
        {
            try
            {
              
                MySqlCommand command = null;
                string query = "SELECT A.*,B.* from tbl_user_data A,tbl_user B where a.id_user=b.id_user AND b.userid=@value1 AND A.DEVICE_ID=@value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", login.UserName);
                command.Parameters.AddWithValue("value2", login.DeviceID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  int UpdateAuthcodeStatus(Authcode code)
        {
            try
            {

             
                MySqlCommand command = null;
                string query = "UPDATE tbl_authcode SET STATUS = @value1 WHERE ID_CODE = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", code.Status);
                command.Parameters.AddWithValue("value2", code.AuthCodeID);
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

        public  string GetAuthcode(int authID)
        {
            try
            {
                string code = "";
           
                MySqlCommand command = null;
                string query = "SELECT CODE FROM tbl_authcode WHERE ID_CODE = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", authID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    code = reader["CODE"].ToString();
                }
                reader.Close();
                return code;
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
        public  int GetAuthcodeIDOfUser(int userID)
        {
            try
            {
                int result = 0;
               
                MySqlCommand command = null;
                string query = "SELECT ID_CODE FROM tbl_user WHERE ID_USER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader["ID_CODE"]);
                }
                reader.Close();
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
        public  Authcode GetActiveAuthcode()
        {
            try
            {
                Authcode code = null;
               
                MySqlCommand command = null;
                string query = "SELECT * FROM tbl_authcode  order by ID_CODE LIMIT 1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    code = new Authcode();
                    while (reader.Read())
                    {
                        code.AuthCodeID = Convert.ToInt32(reader["ID_CODE"]);
                        code.Code = reader["CODE"].ToString();
                        code.Status = reader["STATUS"].ToString();
                    }
                    reader.Close();
                }
                return code;
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
        public  int GetRole(string roleName, int organizatioID)
        {
            try
            {
                int roleID = 0;
                
                MySqlCommand command = null;
                string query = "select * from tbl_roles where ROLENAME = @value1 and ORGANIZATIONID = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", roleName);
                command.Parameters.AddWithValue("value2", organizatioID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    roleID = Convert.ToInt32(reader["ID_ROLE"]);
                }
                reader.Close();
                return roleID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }
        public  int CreateUser(Registration user, Authcode code, string status)
        {
            try
            {
               
                MySqlCommand command = null;
                int result = 0;
                string query = "INSERT INTO tbl_user (ID_CODE, ID_ROLE, USERID, PASSWORD, FBSOCIALID, GPSOCIALID, STATUS,UPDATEDTIME,ID_ORGANIZATION) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7,@value8,@value9)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", code.AuthCodeID);
                command.Parameters.AddWithValue("value2", user.Role);
                command.Parameters.AddWithValue("value3", user.UserName);
                command.Parameters.AddWithValue("value4", user.Password);
                command.Parameters.AddWithValue("value5", user.FBSocialID);
                command.Parameters.AddWithValue("value6", user.GPSocialID);
                command.Parameters.AddWithValue("value7", status);
                command.Parameters.AddWithValue("value8", System.DateTime.Now);
                command.Parameters.AddWithValue("value9", user.OrganizationID);
                result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    return Convert.ToInt32(command.LastInsertedId);
                }
                else
                    return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }

        public  int CreateProfile(int userID, Registration user)
        {
            try
            {
                int age = 0;
                if (!string.IsNullOrEmpty(user.Age))
                {
                    age = Convert.ToInt32(user.Age);
                }

              
                MySqlCommand command = null;
                string query = "INSERT INTO tbl_profile (ID_USER, FIRSTNAME, LASTNAME, AGE, LOCATION, EMAIL, MOBILE) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                command.Parameters.AddWithValue("value2", user.FirstName);
                command.Parameters.AddWithValue("value3", user.LastName);
                command.Parameters.AddWithValue("value4", age);
                command.Parameters.AddWithValue("value5", user.Location);
                command.Parameters.AddWithValue("value6", user.Email);
                command.Parameters.AddWithValue("value7", user.Mobile);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }

        public  Profile GetActiveUserProfile(int userID)
        {
            try
            {
                Profile profile = null;
               
                MySqlCommand command = null;
                string query = "select FIRSTNAME,LASTNAME,AGE,LOCATION,MOBILE,EMAIL from tbl_profile where ID_USER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    profile = new Profile();
                    while (reader.Read())
                    {
                        profile.FirstName = reader["FIRSTNAME"].ToString();
                        profile.LastName = reader["LASTNAME"].ToString();
                        profile.Age = Convert.ToInt32(reader["AGE"]);
                        profile.Location = reader["LOCATION"].ToString();
                        profile.Mobile = reader["MOBILE"].ToString();
                        profile.Email = reader["EMAIL"].ToString();
                    }
                    reader.Close();
                }
                return profile;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }

        }
        public  int UpdateUserProfile(Registration user, int userID)
        {
            //
            try
            {
               
                MySqlCommand command = null;
                string query = "UPDATE tbl_profile SET FIRSTNAME = @value2, LASTNAME= @value3, AGE= @value4, LOCATION = @value5, EMAIL = @value6, MOBILE = @value7  WHERE ID_USER = @value0";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value2", user.FirstName);
                command.Parameters.AddWithValue("value3", user.LastName);
                command.Parameters.AddWithValue("value4", user.Age);
                command.Parameters.AddWithValue("value5", user.Location);
                command.Parameters.AddWithValue("value6", user.Email);
                command.Parameters.AddWithValue("value7", user.Mobile);
                command.Parameters.AddWithValue("value0", userID);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }

        public  int GetActionID(string action)
        {
            try
            {
                int actionID = 0;
                string status = "A";
              
                MySqlCommand command = null;
                string query = "select ID_ACTION from tbl_action where ACTION_NAME = @value1 and STATUS = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", action);
                command.Parameters.AddWithValue("value2", status);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    actionID = Convert.ToInt32(reader["ID_ACTION"]);
                }
                reader.Close();
                return actionID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }


        public  int DeleteUserRollback(int userID)
        {
            try
            {
             
                MySqlCommand command = null;
                string query = "DELETE FROM tbl_user WHERE ID_USER = @value1";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }

        }
        public  int UpdateUserLog(int userID, int deviceType, string deviceID, int action)
        {
            try
            {
              
                MySqlCommand command = null;
                string query = "INSERT INTO tbl_user_data (ID_USER, ID_DEVICE_TYPE, DEVICE_ID, ID_ACTION,UPDATEDDATETIME) VALUES (@value1, @value2, @value3, @value4, @value5)";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", userID);
                command.Parameters.AddWithValue("value2", deviceType);
                command.Parameters.AddWithValue("value3", deviceID);
                command.Parameters.AddWithValue("value4", action);
                command.Parameters.AddWithValue("value5", System.DateTime.Now);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }


        }
        public  int GetDeviceTypeID(string deviceType)
        {
            try
            {
                string status = "A";
                int deviceTypeID = 0;
               
                MySqlCommand command = null;
                string query = "select * from tbl_device_type where DEVICENAME = @value1 and STATUS= @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", deviceType);
                command.Parameters.AddWithValue("value2", status);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    deviceTypeID = Convert.ToInt32(reader["ID_DEVICE_TYPE"]);
                }
                reader.Close();
                return deviceTypeID;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }

        public  bool LoginUser(string username, string password, int roleID)
        {
            try
            {
                string status = "A";
             
                MySqlCommand command = null;
                string query = "select USERID,PASSWORD from tbl_user where USERID = @value1 and PASSWORD = @value2 and ID_ROLE = @value3 and STATUS =@value4";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", username);
                command.Parameters.AddWithValue("value2", password);
                command.Parameters.AddWithValue("value3", roleID);
                command.Parameters.AddWithValue("value4", status);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return true;
                else
                    return false;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
        }
        public  string CheckUserStatus(string username, int roleID)
        {
            try
            {
                string status = "";
               
                MySqlCommand command = null;
                string query = "select STATUS from tbl_usres where usrename = @value1 and ID_ROLE = @value2";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", username);
                command.Parameters.AddWithValue("value2", roleID);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    status = (reader["STATUS"]).ToString();
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


        public  int UpdateUserStatus(int userID, int roleID, string status)
        {
            try
            {
               
                MySqlCommand command = null;
                string query = "UPDATE tbl_user SET STATUS = @value1, UPDATEDTIME = @value4 WHERE ID_USER= @value2 and ID_ROLE= @value3";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", status);
                command.Parameters.AddWithValue("value2", userID);
                command.Parameters.AddWithValue("value3", roleID);
                command.Parameters.AddWithValue("value4", System.DateTime.Now);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }
        public  int UpdateUserSocial(Login user, int userID, int roleID)
        {
            try
            {
               
                MySqlCommand command = null;
                string query = "UPDATE tbl_user SET FBSOCIALID = @value1, GPSOCIALID = @value2,UPDATEDTIME = @value5 WHERE ID_USER= @value3 and ID_ROLE= @value4";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("value1", user.FBSocialID);
                command.Parameters.AddWithValue("value2", user.GPSocialID);
                command.Parameters.AddWithValue("value3", userID);
                command.Parameters.AddWithValue("value4", roleID);
                command.Parameters.AddWithValue("value5", System.DateTime.Now);
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally { connection.Close(); }
        }
        public List<Menu> get_menu(int oid)
        {
            List<Menu> result = new List<Menu>();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_menu where id_org=@value1;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Menu inst = new Menu();
                inst.menu_name = reader["menu_name"].ToString();
                inst.id_menu = Convert.ToInt32(reader["id_menu"].ToString());
                inst.menu_url = reader["menu_url"].ToString();
                inst.menu_icon = ConfigurationManager.AppSettings["menuicon"].ToString()+ reader["menu_icon"].ToString();
                result.Add(inst);
            }
            reader.Close();
            connection.Close();
           

            //if (result.Count==0)
            //{
               
            //    command = null;
            //    string query1 = "SELECT * FROM tbl_menu_default";
            //    connection.Open();
            //    command = connection.CreateCommand();
            //    command.CommandText = query1;
               

            //    MySqlDataReader reader1 = command.ExecuteReader();
            //    while (reader1.Read())
            //    {
            //        Menu inst = new Menu();
            //        inst.menu_name = reader1["menu_name"].ToString();
            //        inst.id_menu = Convert.ToInt32(reader1["id_menu"].ToString());
            //        inst.menu_url = reader1["menu_url"].ToString();
            //        inst.menu_icon = reader1["menu_icon"].ToString();
            //        result.Add(inst);
            //    }
            //    reader.Close();
            //    connection.Close();
            //}
            return result;
        }
        public string get_version(string ver)
        {
            string result = "";
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_version_control_ios where version_number=@value1;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", ver);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                result = reader["version_number"].ToString();
               
            }
            reader.Close();
            return result;
        }
    }
    
    public static class PasswordEncryption
    {
        public static string ToMD5Hash(this byte[] bytes)
        {
            StringBuilder hash = new StringBuilder();
            MD5 md5 = MD5.Create();
            md5.ComputeHash(bytes)
                  .ToList()
                  .ForEach(b => hash.AppendFormat("{0:x2}", b));
            return hash.ToString();
        }
        public static string ToMD5Hash(this string inputString)
        {
            return Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
        }

    }

    public  class PasswordGeneration
    {
        private  int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private  string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public  string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

    }
    public class Authcode
    {
        public int AuthCodeID { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
    }

    public class VerifyMe
    {
        public int OrganizationID { get; set; }
        public int RoleID { get; set; }
        public string VerificationCode { get; set; }
        public string UserName { get; set; }

    }
    public class Registration
    {
        public int OrganizationID { get; set; }
        public int Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }
        public string DeviceID { get; set; }
        public string FBSocialID { get; set; }
        public string GPSocialID { get; set; }
        public string INSTSocialID { get; set; }
        public string Age { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Provider { get; set; }
    }

    public class Login
    {
        public int OrganizationID { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }
        public string DeviceID { get; set; }
        public string FBSocialID { get; set; }
        public string GPSocialID { get; set; }
        public string Provider { get; set; }
    }

    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    public class GetProfile
    {
        public string USERID { get; set; }
        public int RoleID { get; set; }
        public int ORGID { get; set; }
    }
}