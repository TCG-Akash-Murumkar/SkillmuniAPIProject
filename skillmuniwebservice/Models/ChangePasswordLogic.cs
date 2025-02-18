using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ChangePasswordLogic
    {
        public MySqlConnection conn = null;

        public ChangePasswordLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }

        public string changepassword(Password pswd)
        {
            string result = "";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int iduser = db.Database.SqlQuery<int>("select ID_USER from tbl_user where PASSWORD={0}",pswd.OLDPASSWORD).FirstOrDefault();
                    if (iduser == pswd.ID_USER)
                    {
                        db.Database.ExecuteSqlCommand("update tbl_user set PASSWORD={0} , UPDATEDTIME={1} where ID_USER={2}",DateTime.Now, pswd.ID_USER);
                        result = "Password changed Successfully.";
                    }
                    else
                    {
                        result = "Old password entered is wrong.";

                    }

                }
                   
            }
            catch (Exception e)
            {
                result = "Something went wrong. password is  not updated.";
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int CheckFirstLogin(int uid)
        {
            int log_flag = 0;
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "select * from tbl_user_login_log where uid=@value1";
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", uid);

                int usid = 0;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usid = Convert.ToInt32(reader["uid"].ToString());
                }
                if (usid != 0)
                {
                    log_flag = 1;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }

            return log_flag;
        }

        public string ChangepasswordBrief(int uid, int oid, string pswd)
        {
            string result = "";
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "Update tbl_user set PASSWORD='" + pswd + "' where ID_USER='" + uid + "' and ID_ORGANIZATION=" + oid + ";";
                cmd.CommandText = str;
                conn.Open();
                cmd.ExecuteNonQuery();
                result = "You have successfully reset your password";
            }
            catch (Exception e)
            {
                result = "Something went wrong. password is  not updated.";
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public void UpdateuserLog(int uid)
        {
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "insert into tbl_user_login_log (uid)value(" + uid + ")";
                cmd.CommandText = str;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }
        }
    }
}