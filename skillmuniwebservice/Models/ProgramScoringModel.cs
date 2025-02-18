using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ProgramScoringModel
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        private MySqlConnection connection = null;

        public ProgramScoringModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public string checkProgramComplition(int cid, int uid, int oid)
        {

            string sqls = "select * from sc_game_element_weightage where element_type=1 and id_category=" + cid + " and id_user=" + uid + " ";
            sc_game_element_weightage item = db.sc_game_element_weightage.SqlQuery(sqls).FirstOrDefault();
            if (item == null)
            {
                return "0";
            }
            return "1";
        }


        public double getWeightage(string sqlq)
        {
            try
            {
                double count = 0;
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sqlq;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = Convert.ToDouble(reader.GetDouble(0));                        
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

            return 0.0;
        }


        public double getContentWeightage(int cid, double? value)
        {
            double weightage = 0.0;
            double vData = Convert.ToDouble(value);
            if (vData > 0)
            {
            }
            else
            {
                return 0;
            }

            string sqlq = "";
            sqlq += " SELECT TRUNCATE((" + value + " * b.kpi_value/100),2) weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c ";
            sqlq += " WHERE   a.id_kpi_master = b.id_kpi_master AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master ";
            sqlq += " and " + value + " > b.start_range and " + value + " <=end_range and c.id_category=" + cid + " and c.kpi_type=1 ";

            weightage = getWeightage(sqlq);

            return weightage;
        }
        public double getAssessmentWeightage(int aid, int cid, double? value)
        {
            double weightage = 0.0;
            double vData = Convert.ToDouble(value);
            if (vData > 0)
            {
            }
            else
            {
                return 0;
            }

            string sqlq = "";
            sqlq += " SELECT TRUNCATE((" + value + " * b.kpi_value/100),2) weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c ";
            sqlq += " WHERE   a.id_kpi_master = b.id_kpi_master     AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master ";
            sqlq += " and " + value + " > b.start_range and " + value + " <=end_range and c.id_assessment=" + aid + " and c.kpi_type=2 ";

            weightage = getWeightage(sqlq);

            return weightage;
        }

        public double getKPIWeightage(int aid, int cid, double? value)
        {
            double weightage = 0.0;
            double vData = Convert.ToDouble(value);
            if (vData > 0)
            {
            }
            else
            {
                return 0;
            }

            string sqlq = "";
            sqlq += " SELECT TRUNCATE((" + value + " * b.kpi_value/100),2) weightage FROM tbl_kpi_master a, tbl_kpi_grid b, tbl_kpi_program_scoring c ";
            sqlq += " WHERE   a.id_kpi_master = b.id_kpi_master AND a.id_kpi_master = c.id_kpi_master AND b.id_kpi_master = c.id_kpi_master ";
            sqlq += " and " + value + " > b.start_range and " + value + " <=end_range and c.id_category=" + cid + " and c.kpi_type=1 ";

            weightage = getWeightage(sqlq);

            return weightage;
        }


    }



}