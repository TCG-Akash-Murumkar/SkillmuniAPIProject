using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace m2ostnextservice.Models
{
    public class AssessmentModel
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        private MySqlConnection connection = null;

        public AssessmentModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
        public int getAttamptNo(int ASID, int UID)
        {
            int ret = 0;
            try
            {


                MySqlCommand command = null;
                string query = "SELECT count(*) subcount FROM tbl_assessment_index where id_user = " + UID + " and  id_assessment_sheet = " + ASID + "";
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
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


        public string getAssessmentCheck(int CID, int OID)
        {
            string ret = "0";
            try
            {


                MySqlCommand command = null;
                string query = "SELECT *  FROM tbl_assessment_mapping where id_content = " + CID + " AND id_organization=" + OID + ""; ;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = "1";
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


        public string EveluateAssessment(int ASID, int UID, int ATM)
        {
            tbl_assessment_sheet sheet = db.tbl_assessment_sheet.Where(t => t.id_assesment == ASID).FirstOrDefault();
            string responce = "";
            if (sheet.id_assessment_theme == 1)
            {
                List<tbl_assessment_audit> audit = db.tbl_assessment_audit.Where(t => t.id_assessment == sheet.id_assesment && t.attempt_no == ATM && t.id_user == UID).ToList();
                int successCount = 0;
                int totalCount = 0;
                double sRate = 0.0;
                foreach (tbl_assessment_audit item in audit)
                {
                    tbl_assessment_question qtn = db.tbl_assessment_question.Find(item.id_assessment_question);
                    tbl_assessment_answer ans = db.tbl_assessment_answer.Find(item.id_assessment_answer);
                    if (qtn.aq_answer == ans.id_assessment_answer.ToString())
                    {
                        successCount++;
                    }
                }
                if (successCount == 0)
                {

                }
                else
                {
                    double per = (double)successCount / audit.Count * 100;
                    sRate = Math.Round(per, 2);
                }
                //tres.success_count = sRate;
                //tres.assessment_result = successCount;
                //db.trep_assessment_result_1.Add(tres);
                //db.SaveChanges();
                responce = "Number of correct answers   : " + successCount.ToString();
            }
            else if (sheet.id_assessment_theme == 2)
            {
                List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
                List<string> responceStr = new List<string>();
                int icount = 0;
                foreach (tbl_assessment_scoring_key item in sKeys)
                {
                    icount++;
                    // List<tbl_assessment_header> header = db.tbl_assessment_header.Where(t => t.id_assessment_scoring_key == item.id_assessment_scoring_key).ToList();

                    string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
                    sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
                    sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

                    List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
                    responceStr.Add(" " + icount + " . " + item.header_name + " : " + audit.Count);
                }

                responce = String.Join("|", responceStr);


            }
            else if (sheet.id_assessment_theme == 3)
            {
                List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
                List<string> responceStr = new List<string>();
                int icount = 0;
                foreach (tbl_assessment_scoring_key item in sKeys)
                {
                    icount++;
                    string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + "  and attempt_no=" + ATM + " AND id_assessment_question in ";
                    sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " AND id_assessment_scoring_key=" + item.id_assessment_scoring_key + " )  ";

                    //string sqlStr1 = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
                    //sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
                    //sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

                    List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
                    int count = Convert.ToInt32(audit.Sum(t => t.value_sent));
                    responceStr.Add(" " + icount + " . " + item.header_name + " : " + count);
                }
                responce = String.Join("|", responceStr);

            }

            else if (sheet.id_assessment_theme == 4)
            {
                List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
                List<string> responceStr = new List<string>();
                int icount = 0;
                foreach (tbl_assessment_scoring_key item in sKeys)
                {
                    icount++;
                    string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + "  and attempt_no=" + ATM + " AND id_assessment_answer in ";
                    sqlStr += " (select id_assessment_answer from tbl_assessment_answer where id_assessment =" + sheet.id_assesment + " AND id_assessment_scoring_key=" + item.id_assessment_scoring_key + " )  ";

                    //string sqlStr1 = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
                    //sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
                    //sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

                    List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
                    int count = Convert.ToInt32(audit.Sum(t => t.value_sent));
                    responceStr.Add(" " + icount + " . " + item.header_name + " : " + count);
                }
                responce = String.Join("|", responceStr);

            }


            return responce;
        }

        public List<AssessmentList> getAssesmentList(int CID, int UID, int OID)
        {
            DateTime current = System.DateTime.Now;
            List<AssessmentList> list = new List<AssessmentList>();
            tbl_user user = db.tbl_user.Where(t => t.ID_USER == UID).FirstOrDefault();
            string assessSql = "";// "select * from tbl_assessment_sheet where status='A' and id_organization=" + OID + " and id_assessment_sheet in (select distinct id_assessment_sheet from tbl_assessment_categoty_mapping where id_category =" + CID + ")";
            assessSql = "select  a.* from   tbl_assessment_sheet a left join tbl_assessment_categoty_mapping b  on a.id_assessment_sheet=b.id_assessment_sheet  where a.status = 'A' and a.id_organization = " + OID + " and b.id_category = " + CID + "";

            List<tbl_assessment_sheet> tSheet = db.tbl_assessment_sheet.SqlQuery(assessSql).ToList();

            string assSql = "select distinct * from tbl_assessment_user_assignment where id_organization=" + OID + " AND id_user=" + UID + " AND id_category=" + CID + "";
            List<tbl_assessment_user_assignment> assSheet = db.tbl_assessment_user_assignment.SqlQuery(assSql).ToList();

            foreach (tbl_assessment_user_assignment item in assSheet)
            {
                tbl_assessment_sheet iSheet = db.tbl_assessment_sheet.SqlQuery("select * from tbl_assessment_sheet where status='A' and id_organization=" + OID + " and id_assesment =" + item.id_assessment).FirstOrDefault();

                AssessmentList tmp = new AssessmentList();
                tbl_assessment assessment = db.tbl_assessment.Where(t => t.status == "A" && t.id_assessment == iSheet.id_assesment).FirstOrDefault();
                if (assessment != null)
                {
                    if (DateTime.Compare(item.expire_date.Value.AddDays(1), current) > 0)
                    {
                        if (DateTime.Compare(assessment.assess_ended.Value.AddDays(1), current) > 0)
                        {
                            tmp.id_assessment_sheet = iSheet.id_assessment_sheet;
                            tmp.id_assessment = assessment.id_assessment;
                            tmp.assessment_name = assessment.assessment_title;
                            tmp.assessment_description = assessment.assesment_description;
                            tmp.expiry_date = item.expire_date.Value.ToString("dd-MMM-yyyy");
                            list.Add(tmp);
                        }
                    }
                }
            }

            foreach (tbl_assessment_sheet local in tSheet)
            {
                AssessmentList tmp = new AssessmentList();
                tbl_assessment assessment = db.tbl_assessment.Where(t => t.status == "A" && t.id_assessment == local.id_assesment).FirstOrDefault();
                if (assessment != null)
                {
                    if (DateTime.Compare(assessment.assess_ended.Value.AddDays(1), current) > 0)
                    {
                        tmp.id_assessment_sheet = local.id_assessment_sheet;
                        tmp.id_assessment = assessment.id_assessment;
                        tmp.assessment_name = assessment.assessment_title;
                        tmp.assessment_description = assessment.assesment_description;
                        tmp.expiry_date = assessment.assess_ended.Value.ToString("dd-MMM-yyyy");
                        list.Add(tmp);
                    }
                }
            }

            if (tSheet.Count > 0)
            {
                tSheet = tSheet.Distinct().ToList();
            }
            if (list.Count > 0)
            {
                list = list.OrderBy(t => t.assessment_name).ToList();
            }

            return list;
        }


        public List<CategorySummary> getCategorySummaryForScoring(int OID)
        {
            List<CategorySummary> list = new List<CategorySummary>();
            DateTime current = System.DateTime.Now;

            string gsql = "select * from tbl_game_creation where id_organization="+OID+" and status='A'";
            List<tbl_game_creation> game = db.tbl_game_creation.SqlQuery(gsql).ToList();

            foreach(tbl_game_creation item in game)
            {

            }


            string csql = "select * from tbl_category where id_organization=" + OID + " and status='A' and CATEGORY_TYPE in (0,1)";
            List<tbl_category> category = db.tbl_category.SqlQuery(csql).ToList();
            foreach (tbl_category item in category)
            {
                string assessSql = "";
                assessSql += " SELECT p.* FROM tbl_assessment p left join  tbl_assessment_sheet a LEFT JOIN tbl_assessment_categoty_mapping b ON a.id_assessment_sheet = b.id_assessment_sheet ";
                assessSql += " on p.id_assessment=a.id_assesment WHERE a.status = 'A' and a.id_organization=" + OID + " AND b.id_category = " + item.ID_CATEGORY + " ";
                List<tbl_assessment> tSheet = db.tbl_assessment.SqlQuery(assessSql).ToList();
                string contentSql = "select * from tbl_content a left join tbl_content_organization_mapping b on a.ID_CONTENT=b.id_content where b.id_category = " + item.ID_CATEGORY + " and b.id_organization = " + OID + " and a.status='A'";
                List<tbl_content> tContent = db.tbl_content.SqlQuery(contentSql).ToList();

                CategorySummary summary = new CategorySummary(tContent, tSheet);
                list.Add(summary);
            }
            return list;
        }


    }
    public class CategorySummary
    {
        public List<tbl_content> ContentList { get; set; }
        public List<tbl_assessment> AssessmentList { get; set; }
        public CategorySummary(List<tbl_content> c, List<tbl_assessment> a)
        {
            this.ContentList = c;
            this.AssessmentList = a;
        }
    }


    public class DataCube
    {
        public string QID { get; set; }
        public string AID { get; set; }
        public string VAL { get; set; }
    }

    public class Assessment
    {
        public int id_assessment { get; set; }
        public string assessment_title { get; set; }
        public string assesment_description { get; set; }
        public int id_organization { get; set; }
        public string assess_type { get; set; }
        public string low_value { get; set; }
        public string low_title { get; set; }
        public string high_value { get; set; }
        public string high_title { get; set; }
    }
    public class AssessmentQuestion
    {
        public int id_assessment_question { get; set; }
        public int id_organization { get; set; }
        public string assessment_question { get; set; }
        public string question_image { get; set; }
        public string aq_answer { get; set; }
    }

    public class AssessmentOption
    {
        public int id_assessment_answer { get; set; }
        public int id_assessment_question { get; set; }
        public string answer_description { get; set; }
    }

    public class AssessmentSheet
    {
        public List<Assessment> Assessment { get; set; }
        public List<QuestionAnswer> QuestionAnswer { get; set; }
        public int THEME { get; set; }
    }

    public class QuestionAnswer
    {
        public List<AssessmentQuestion> AssessmenQuestion { get; set; }
        public List<AssessmentOption> AssessmentOption { get; set; }
    }

    public class AssessmentRequest
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public int ASID { get; set; }
        public List<string> ASRQ { get; set; }
    }

    public class AssessmentResponce
    {
        public List<Assessment> Assessment { get; set; }
        public string Message { get; set; }
        public List<UserInput> QuestionAnswer { get; set; }
        public string Attempt { get; set; }
    }

    public class UserInput
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string WANS { get; set; }
    }

    public class AssessmentList
    {
        public int id_assessment_sheet { get; set; }
        public int id_assessment { get; set; }
        public string assessment_name { get; set; }
        public string assessment_description { get; set; }
        public string expiry_date { get; set; }
    }

    public class TeamReport
    {
        public string USERNAME { get; set; }
        public string EMPLOYEEID { get; set; }
        public List<MyReport> REPORTS { get; set; }
    }

    public class TeamMember
    {
        public string USERNAME { get; set; }
        public string EMPLOYEEID { get; set; }
        public string USERID { get; set; }
        public string ID_USER { get; set; }
        public string DESIGNATION { get; set; }
        public string FUNCTION { get; set; }
        public string GRADE { get; set; }
        public string DEPARTMENT { get; set; }
    }

    public class MyReport
    {
        public List<AssessmentReport> LEARNING { get; set; }
        public List<AssessmentReport> PSYCHOMETRIC { get; set; }
    }

    public class AssessmentReport
    {
        public int id_assessment_log { get; set; }
        public int id_assessment_sheet { get; set; }
        public int id_assessment { get; set; }
        public string assessment_name { get; set; }
        public string assessment_description { get; set; }
        public string attempt { get; set; }
        public string LogDate { get; set; }
    }

}