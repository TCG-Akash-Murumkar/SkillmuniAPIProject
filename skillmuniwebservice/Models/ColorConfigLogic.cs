using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace m2ostnextservice.Models
{
    public class ColorConfigLogic
    {
        private MySqlConnection connection = null;


        public ColorConfigLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
        public List<ColorConfig> get_color_config(int oid)
        {
            List<ColorConfig> result = new List<ColorConfig>();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_color_config where id_organisation=@value1 order by config_type;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ColorConfig inst = new ColorConfig();
                inst.id_color_config = Convert.ToInt32(reader["id_color_config"].ToString());
                inst.config_type =Convert.ToInt32( reader["config_type"].ToString());
                inst.id_organisation = Convert.ToInt32(reader["id_organisation"].ToString());
                inst.grid1_bk_color = reader["grid1_bk_color"].ToString();
                inst.grid1_text_color = reader["grid1_text_color"].ToString();
                inst.grid2_bk_color = reader["grid2_bk_color"].ToString();
                inst.grid2_text_color = reader["grid2_text_color"].ToString();
               
                result.Add(inst);
            }
            if (result.Count==0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    ColorConfig inst = new ColorConfig();

                    inst.config_type = i;
                    inst.id_organisation = oid;
                    if (i == 3)
                    { 
                    inst.grid1_bk_color = WebConfigurationManager.AppSettings["3_background_1"];
                    inst.grid1_text_color = WebConfigurationManager.AppSettings["3_text_1"];
                    inst.grid2_bk_color = WebConfigurationManager.AppSettings["3_background_2"];
                    inst.grid2_text_color = WebConfigurationManager.AppSettings["3_text_2"];
                    }
                    if (i == 1)
                    {
                        inst.grid1_bk_color = WebConfigurationManager.AppSettings["1_background_1"];
                        inst.grid1_text_color = WebConfigurationManager.AppSettings["1_text_1"];
                       
                    }
                    if (i == 2)
                    {
                        inst.grid1_bk_color = WebConfigurationManager.AppSettings["2_background_1"];
                        inst.grid1_text_color = WebConfigurationManager.AppSettings["2_text_1"];
                       
                    }
                    if (i == 4)
                    {
                        inst.grid1_bk_color = WebConfigurationManager.AppSettings["4_background_1"];
                        inst.grid1_text_color = WebConfigurationManager.AppSettings["4_text_1"];
                        inst.grid2_bk_color = WebConfigurationManager.AppSettings["4_background_2"];
                        inst.grid2_text_color = WebConfigurationManager.AppSettings["4_text_2"];
                    }

                    result.Add(inst);

                }
            }
                reader.Close();
                connection.Close();
            
            return result;
        }
        public WelcomeGif get_welcome_gif(int oid)
        {
            WelcomeGif result = new WelcomeGif();
            MySqlCommand command = null;
            string query = "SELECT * FROM tbl_welcome_gif where id_org=@value1;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("value1", oid);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                result.id_welcome_gif = Convert.ToInt32(reader["id_welcome_gif"].ToString());
                result.id_org = Convert.ToInt32(reader["id_org"].ToString());
                result.gif = WebConfigurationManager.AppSettings["welcomegifpath"] + reader["gif"].ToString();
                result.status = reader["status"].ToString();

               

               
            }
            if (result.gif == null)
            {



                result.id_org = oid;
                result.gif = WebConfigurationManager.AppSettings["welcomegifpath"] + "default.gif";
                result.status = "A";


            }
            reader.Close();
            connection.Close();
            return result;
        }
        public tbl_profile getpro(string query)
        {
            tbl_profile result = new tbl_profile();
            MySqlCommand command = null;
            //string query = "SELECT * FROM tbl_welcome_gif where id_org=@value1;";
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
          
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                result.FIRSTNAME =reader["FIRSTNAME"].ToString();
                result.LASTNAME = reader["LASTNAME"].ToString();
                




            }
        
            reader.Close();
            connection.Close();
            return result;
        }

        

    }
}