using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{

    public class RoadMapLogic
    {
        public MySqlConnection conn = null;

        public RoadMapLogic()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }
        public List<RoadMapModels.tbl_academic_tiles> getGameTiles(int oid)
        {
            List<RoadMapModels.tbl_academic_tiles> tiles = new List<RoadMapModels.tbl_academic_tiles>();
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "select * from tbl_academic_tiles where id_org=@value1 ORDER BY tile_position ASC";
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", oid);

               
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RoadMapModels.tbl_academic_tiles obj = new RoadMapModels.tbl_academic_tiles();

                    obj.id_academic_tile = Convert.ToInt32(reader["id_academic_tile"].ToString());
                    obj.id_org = Convert.ToInt32(reader["id_org"].ToString());
                    obj.status = reader["status"].ToString();
                    obj.tile_description = reader["tile_description"].ToString();
                    obj.tile_image = reader["tile_image"].ToString();
                    obj.tile_name = reader["tile_name"].ToString();
                    obj.tile_position = Convert.ToInt32(reader["tile_position"].ToString());
                    obj.theme_id = Convert.ToInt32(reader["theme_id"].ToString());
                    obj.url = reader["url"].ToString();


                    tiles.Add(obj);
                }
               
            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }

            return tiles;
        }


        public List<RoadMapModels.tbl_brief_tile_academic_mapping> getTilesMapping(int gametile_id)
        {
            List<RoadMapModels.tbl_brief_tile_academic_mapping> tiles = new List<RoadMapModels.tbl_brief_tile_academic_mapping>();
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "select * from tbl_brief_tile_academic_mapping where id_academic_tile=@value1";
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", gametile_id);


                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RoadMapModels.tbl_brief_tile_academic_mapping obj = new RoadMapModels.tbl_brief_tile_academic_mapping();

                    obj.id_tile_mapping = Convert.ToInt32(reader["id_tile_mapping"].ToString());
                    obj.id_org = Convert.ToInt32(reader["id_org"].ToString());
                    obj.status = reader["status"].ToString();
                    obj.id_journey_tile =Convert.ToInt32(reader["id_journey_tile"].ToString());
                    obj.id_academic_tile =Convert.ToInt32( reader["id_academic_tile"].ToString());
                   
                    tiles.Add(obj);
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }

            return tiles;
        }
        public tbl_brief_category_tile getJourneytile(int journeytileid)
          {
            tbl_brief_category_tile obj = new tbl_brief_category_tile();
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                string str = "select * from tbl_brief_category_tile where id_brief_category_tile=@value1";
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = str;
                cmd.Parameters.AddWithValue("value1", journeytileid);


                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   

                    obj.assessment_complation = Convert.ToInt32(reader["assessment_complation"].ToString());
                    obj.attempt_limit = Convert.ToInt32(reader["attempt_limit"].ToString());
                    obj.category_tile = reader["category_tile"].ToString();
                    obj.category_tile_type = Convert.ToInt32(reader["category_tile_type"].ToString());
                    obj.id_brief_category_tile = Convert.ToInt32(reader["id_brief_category_tile"].ToString());
                    obj.id_organization = Convert.ToInt32(reader["id_organization"].ToString());
                    obj.status = reader["status"].ToString();
                    obj.tile_code = reader["tile_code"].ToString();
                    obj.tile_description = reader["tile_description"].ToString();
                    obj.tile_image = reader["tile_image"].ToString();
                    obj.tile_position =Convert.ToInt32( reader["tile_position"].ToString());
                    obj.updated_date_time =Convert.ToDateTime( reader["updated_date_time"].ToString());

                  
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }

            return obj;
        }
        public void GameTileLog(int uid,int oid,int id_gametile)
        {

            //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            //{
            //    List<tbl_user> use = db.Database.SqlQuery<tbl_user>("select * from tbl_user where STATUS='A'").ToList();
            //}

            try
                {


                    MySqlCommand cmd = conn.CreateCommand();
                    string str = "Insert into tbl_academic_tile_log(id_user,oid,updated_date_time,id_academic_tile)values(@value1,@value2,@value3,@value4)";
                    cmd.CommandText = str;
                    conn.Open();
                    cmd.Parameters.AddWithValue("value1", uid);
                    cmd.Parameters.AddWithValue("value2", oid);
                    cmd.Parameters.AddWithValue("value3", DateTime.Now);
                    cmd.Parameters.AddWithValue("value4", id_gametile);
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

        public string LoginValidate(string uid,string pswd)
        {
            string result = "FAILURE";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int iduser = db.Database.SqlQuery<int>("select ID_USER from tbl_cms_users where USERNAME={0} and PASSWORD={1}",uid,pswd).FirstOrDefault();
                    if (iduser > 0)
                    {
                        result = "SUCCESS";
                    }

                }
            }

            catch (Exception e)
            {

            }


            return result;
        }




    }
}