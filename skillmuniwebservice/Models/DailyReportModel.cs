using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m2ostnextservice.Models
{
    public class AssessmentObjectJson
    {
        public int draw { get; set; }
        public List<List<string>> data { get; set; }
    }
    public class LoginLog
    {
        public string ORGANIZATION_NAME { get; set; }
        public string USER { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string IMEI { get; set; }
        public string TIMESTAMP { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }
    }


    public class Assessmentdata
    {
        public string ASSESSMENT_TITLE { get; set; }
        public string ID_ASSESSMENT_SHEET { get; set; }
        public string ID_ASSESSMENT { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string USER { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string EMPLOYEEID { get; set; }
        public string ID_USER { get; set; }
        public DateTime Assigned_Date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }
    }

    public class ContenLikeClass
    {
        public string ORGANIZATION_NAME { get; set; }
        public string ID_USER { get; set; }
        public string USERID { get; set; }
        public string UNAME { get; set; }
        public string ID_CONTENT { get; set; }
        public string CONTENT_QUESTION { get; set; }
        public string LikeCount { get; set; }
        public string DisLikeCount { get; set; }
        public string LASTACCESS { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }

    }

    public class ContentAccessClass
    {
        public string ID_USER { get; set; }
        public string USERID { get; set; }
        public string UNAME { get; set; }
        public string ID_ORGANIZATION { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string CATEGORYNAME { get; set; }
        public string ID_CONTENT { get; set; }
        public string CONTENT_QUESTION { get; set; }
        public string LASTACCESS { get; set; }
        public string COUNTER { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }
    }

    public class PROGRAMCOMPLETE
    {
        public string ID_USER { get; set; }
        public int ID_CATEGORY { get; set; }
        public string USERID { get; set; }
        public string UNAME { get; set; }
        public string EMPLOYEEID { get; set; }
        public string ID_ORGANIZATION { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string CATEGORYNAME { get; set; }
        public int TOTALCOUNT { get; set; }
        public int CHECKCOUNT { get; set; }
        public double PERCENTAGE { get; set; }
        public DateTime assigned_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }
    }

    public class LoginExceptionClass
    {
        public string COUNTER { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string USERID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string LASTACCESS { get; set; }
        public string EMPLOYEEID { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }
        public string USTATUS { get; set; }

    }

    public class ProgramStatus
    {
        public string program_name { get; set; }
        public string program_id { get; set; }
        public string status { get; set; }
        public int content_count { get; set; }
        public int assessment_count { get; set; }
        public DateTime assigned_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

    }

    public class AssessmentAuditClass
    {
        public int id_assessment_audit { get; set; }
        public int id_assessment { get; set; }
        public int id_user { get; set; }
        public int id_assessment_question { get; set; }
        public int id_assessment_answer { get; set; }
        public int value_sent { get; set; }
        public int attempt_no { get; set; }
        public string recorded_timestamp { get; set; }
        public string USERID { get; set; }
        public string EMPLOYEEID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string USTATUS { get; set; }
        public string LOCATION { get; set; }
        public string DESIGNATION { get; set; }
        public string RMUSER { get; set; }

    }


    public class LastAssessment
    {
        public int id_assessment;
        public int total_users;
        public string assess_created;
        public string assess_start;
        public string assess_ended;
        public string assessment_title;

        public LastAssessment(MySqlDataReader reader)
        {
            this.assess_created = Convert.ToString(reader["assess_created"]);
            this.assess_start = Convert.ToString(reader["assess_start"]);
            this.assess_ended = Convert.ToString(reader["assess_ended"]);
            this.assessment_title = Convert.ToString(reader["assessment_title"]);
            this.id_assessment = Convert.ToInt32(reader["id_assessment"]);
            this.total_users = Convert.ToInt32(reader["total_users"]);

        }
    }

    public class AssessmentSlab
    {
        public int id_assessment;
        public string assessment_title;
        public string assess_created;
        public string assess_start;
        public string assess_ended;
        public int total_final;
        public int total_users;
        public int total_incomplete;
        public int slab1;
        public int slab2;
        public int slab3;
        public int slab4;
        public int slab5;
        public AssessmentSlab(MySqlDataReader reader)
        {
            this.assessment_title = Convert.ToString(reader["assessment_title"]);
            this.assess_created = Convert.ToString(reader["assess_created"]);
            this.assess_start = Convert.ToString(reader["assess_start"]);
            this.assess_ended = Convert.ToString(reader["assess_ended"]);
            this.id_assessment = Convert.ToInt32(reader["id_assessment"]);
            this.total_users = Convert.ToInt32(reader["total_users"]);
            this.slab1 = Convert.ToInt32(reader["slab1"]);
            this.slab2 = Convert.ToInt32(reader["slab2"]);
            this.slab3 = Convert.ToInt32(reader["slab3"]);
            this.slab4 = Convert.ToInt32(reader["slab4"]);
            this.slab5 = Convert.ToInt32(reader["slab5"]);
            this.total_final = 0;
            this.total_incomplete = 0;
        }

    }
    public class AssessmentSlabGraph
    {
        public int id_assessment;
        public string assessment_title;
        public string CZONE;
        public string trainer_name;
        public int total_final;
        public int total_users;
        public int total_incomplete;
        public int slab1;
        public int slab2;
        public int slab3;
        public int slab4;
        public int slab5;
        public AssessmentSlabGraph(MySqlDataReader reader)
        {
            this.assessment_title = Convert.ToString(reader["assessment_title"]);
            this.CZONE = Convert.ToString(reader["CZONE"]);
            this.trainer_name = Convert.ToString(reader["trainer_name"]);
            this.id_assessment = Convert.ToInt32(reader["id_assessment"]);
            this.total_users = Convert.ToInt32(reader["total_users"]);
            this.slab1 = Convert.ToInt32(reader["slab1"]);
            this.slab2 = Convert.ToInt32(reader["slab2"]);
            this.slab3 = Convert.ToInt32(reader["slab3"]);
            this.slab4 = Convert.ToInt32(reader["slab4"]);
            this.slab5 = Convert.ToInt32(reader["slab5"]);
            this.total_final = 0;
            this.total_incomplete = 0;
        }

    }


    public class TopContent
    {
        public int id_content;
        public string content_question;
        public int counter;
        public int id_organization;

        public TopContent(MySqlDataReader reader)
        {
            this.content_question = Convert.ToString(reader["content_question"]);
            this.id_content = Convert.ToInt32(reader["id_content"]);
            this.counter = Convert.ToInt32(reader["counter"]);
            this.id_organization = Convert.ToInt32(reader["id_organization"]);
        }
    }

    public class TopContentUser
    {

        public string uname;
        public string USERID;
        public int counter;


        public TopContentUser(MySqlDataReader reader)
        {
            this.uname = Convert.ToString(reader["uname"]);
            this.USERID = Convert.ToString(reader["USERID"]);
            this.counter = Convert.ToInt32(reader["counter"]);
        }
    }

    public class ActiveUser
    {

        public string uname;
        public string USERID;
        public string user_department;
        public string user_designation;
        public string user_function;
        public string location;

        public ActiveUser(MySqlDataReader reader)
        {
            this.uname = Convert.ToString(reader["uname"]);
            this.USERID = Convert.ToString(reader["USERID"]);
            this.user_department = Convert.ToString(reader["user_department"]);
            this.user_designation = Convert.ToString(reader["user_designation"]);
            this.user_function = Convert.ToString(reader["user_function"]);
            this.location = Convert.ToString(reader["LOCATION"]);
        }
    }

    public class UserStat
    {

        public string USERID;
        public string EMPLOYEEID;
        public string user_department;
        public string user_designation;
        public string user_function;
        public string uStatus;
        public string FIRSTNAME;
        public string LASTNAME;
        public string LOCATION;
        public string UPDATEDTIME;

        public UserStat(MySqlDataReader reader)
        {

            this.USERID = Convert.ToString(reader["USERID"]);
            this.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
            this.user_department = Convert.ToString(reader["user_department"]);
            this.user_designation = Convert.ToString(reader["user_designation"]);
            this.user_function = Convert.ToString(reader["user_function"]);
            this.uStatus = Convert.ToString(reader["uStatus"]);
            this.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
            this.LASTNAME = Convert.ToString(reader["LASTNAME"]);
            this.LOCATION = Convert.ToString(reader["LOCATION"]);
            this.UPDATEDTIME = Convert.ToString(reader["UPDATEDTIME"]);
        }
    }

    public class UserCount
    {

        public int id_organization;
        public int total_users;
        public int active_users;
        public int deactive_users;

        public UserCount(MySqlDataReader reader)
        {

            this.id_organization = Convert.ToInt32(reader["id_organization"]);
            this.total_users = Convert.ToInt32(reader["total_users"]);
            this.active_users = Convert.ToInt32(reader["active_users"]);
            this.deactive_users = Convert.ToInt32(reader["deactive_users"]);
        }
    }

    public class ZoneCounter
    {
        public string assessment_title;
        public int nscore;
        public int sscore;
        public int escore;
        public int wscore;
        public ZoneCounter()
        {
            this.assessment_title = "";
            this.nscore = 0;
            this.sscore = 0;
            this.wscore = 0;
            this.escore = 0;
        }

    }


    public class ContentCounter
    {

        public int id_organization;
        public int id_content;
        public string CONTENT_QUESTION;
        public int counter;

        public ContentCounter(MySqlDataReader reader)
        {
            this.id_organization = Convert.ToInt32(reader["id_organization"]);
            this.id_content = Convert.ToInt32(reader["id_content"]);
            this.counter = Convert.ToInt32(reader["counter"]);
            this.CONTENT_QUESTION = Convert.ToString(reader["CONTENT_QUESTION"]);
        }
    }


    //class AssessmentReportModel
    //{
    //    private db_m2ostEntities db = new db_m2ostEntities();


    //    public string EveluateAssessment(int ASID, int UID, int ATM, trep_assessment_result_1 tres)
    //    {
    //        tbl_assessment_sheet sheet = db.tbl_assessment_sheet.Where(t => t.id_assesment == ASID).FirstOrDefault();
    //        string responce = "";
    //        if (sheet.id_assessment_theme == 1)
    //        {
    //            List<tbl_assessment_audit> audit = db.tbl_assessment_audit.Where(t => t.id_assessment == sheet.id_assesment && t.attempt_no == ATM && t.id_user == UID).ToList();
    //            int successCount = 0;
    //            int totalCount = 0;
    //            double sRate = 0.0;
    //            foreach (tbl_assessment_audit item in audit)
    //            {
    //                tbl_assessment_question qtn = db.tbl_assessment_question.Find(item.id_assessment_question);
    //                tbl_assessment_answer ans = db.tbl_assessment_answer.Find(item.id_assessment_answer);
    //                if (qtn.aq_answer == ans.id_assessment_answer.ToString())
    //                {
    //                    successCount++;
    //                }
    //            }
    //            if (successCount == 0)
    //            {

    //            }
    //            else
    //            {
    //                double per = (double)successCount / audit.Count * 100;
    //                sRate = Math.Round(per, 2);
    //            }
    //            tres.success_count = sRate;
    //            tres.assessment_result = successCount;
    //            db.trep_assessment_result_1.Add(tres);
    //            db.SaveChanges();
    //            responce = "Number of correct answers   : " + successCount.ToString();
    //        }
    //        else if (sheet.id_assessment_theme == 2)
    //        {
    //            List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
    //            List<string> responceStr = new List<string>();
    //            int icount = 0;
    //            foreach (tbl_assessment_scoring_key item in sKeys)
    //            {
    //                icount++;
    //                // List<tbl_assessment_header> header = db.tbl_assessment_header.Where(t => t.id_assessment_scoring_key == item.id_assessment_scoring_key).ToList();

    //                string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
    //                sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
    //                sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

    //                List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
    //                responceStr.Add(" " + icount + " . " + item.header_name + " : " + audit.Count);
    //            }

    //            responce = String.Join("|", responceStr);


    //        }
    //        else if (sheet.id_assessment_theme == 3)
    //        {
    //            List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
    //            List<string> responceStr = new List<string>();
    //            int icount = 0;
    //            foreach (tbl_assessment_scoring_key item in sKeys)
    //            {
    //                icount++;
    //                string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + "  and attempt_no=" + ATM + " AND id_assessment_question in ";
    //                sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " AND id_assessment_scoring_key=" + item.id_assessment_scoring_key + " )  ";

    //                //string sqlStr1 = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
    //                //sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
    //                //sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

    //                List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
    //                int count = Convert.ToInt32(audit.Sum(t => t.value_sent));
    //                responceStr.Add(" " + icount + " . " + item.header_name + " : " + count);
    //            }
    //            responce = String.Join("|", responceStr);

    //        }

    //        else if (sheet.id_assessment_theme == 4)
    //        {
    //            List<tbl_assessment_scoring_key> sKeys = db.tbl_assessment_scoring_key.Where(t => t.id_assessment == sheet.id_assesment).ToList();
    //            List<string> responceStr = new List<string>();
    //            int icount = 0;
    //            foreach (tbl_assessment_scoring_key item in sKeys)
    //            {
    //                icount++;
    //                string sqlStr = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + "  and attempt_no=" + ATM + " AND id_assessment_answer in ";
    //                sqlStr += " (select id_assessment_answer from tbl_assessment_answer where id_assessment =" + sheet.id_assesment + " AND id_assessment_scoring_key=" + item.id_assessment_scoring_key + " )  ";

    //                //string sqlStr1 = "SELECT *  FROM tbl_assessment_audit  where id_user=" + UID + " AND id_assessment_question in ";
    //                //sqlStr += " (select id_assessment_question from tbl_assessment_question where id_assessment =" + sheet.id_assesment + " ) ";
    //                //sqlStr += "and id_assessment_answer in (select id_assessment_answer from tbl_assessment_answer where id_assessment=" + sheet.id_assesment + " and position in (" + item.position + "))  and attempt_no=" + ATM + "";

    //                List<tbl_assessment_audit> audit = db.tbl_assessment_audit.SqlQuery(sqlStr).ToList();
    //                int count = Convert.ToInt32(audit.Sum(t => t.value_sent));
    //                responceStr.Add(" " + icount + " . " + item.header_name + " : " + count);
    //            }
    //            responce = String.Join("|", responceStr);

    //        }


    //        return responce;
    //    }


    //}

    public class GraphBody
    {
        public string type;

        public string name;

        public string legendText;

        public string showInLegend;

        List<DataPoints> dataPoints;

    }
    public class DataPoints
    {
        public string label;
        public string y;
    }


    class DailyReportModel
    {


        MySqlConnection conn = null;

        public DailyReportModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.conn = new MySqlConnection(con);
        }


        public List<LoginLog> getLoginReport(string STR)
        {
            List<LoginLog> log = new List<LoginLog>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LoginLog temp = new LoginLog();
                    temp.ORGANIZATION_NAME = Convert.ToString(reader["ORGANIZATION_NAME"]);
                    temp.USER = Convert.ToString(reader["USERID"]);
                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    temp.LASTNAME = Convert.ToString(reader["LASTNAME"]);
                    DateTime tDate = Convert.ToDateTime(reader["LOG_DATETIME"]);
                    temp.TIMESTAMP = tDate.ToString("dd-MM-yyyy HH:mm");
                    temp.IMEI = Convert.ToString(reader["IMEI"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }

        public List<ContenLikeClass> getContentLikeReport(string STR)
        {
            List<ContenLikeClass> log = new List<ContenLikeClass>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ContenLikeClass temp = new ContenLikeClass();
                    temp.ORGANIZATION_NAME = Convert.ToString(reader["ORGANIZATION_NAME"]);
                    temp.ID_USER = Convert.ToString(reader["id_user"]);
                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.UNAME = Convert.ToString(reader["UNAME"]);
                    temp.ID_CONTENT = Convert.ToString(reader["id_content"]);
                    temp.CONTENT_QUESTION = Convert.ToString(reader["CONTENT_QUESTION"]);
                    temp.LikeCount = Convert.ToString(reader["LikeCount"]);
                    temp.DisLikeCount = Convert.ToString(reader["DisLikeCount"]);
                    DateTime tDate = Convert.ToDateTime(reader["LASTACCESS"]);
                    temp.LASTACCESS = tDate.ToString("dd-MM-yyyy HH:mm");
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<ContentAccessClass> getContentAccessReport(string STR)
        {
            List<ContentAccessClass> log = new List<ContentAccessClass>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ContentAccessClass temp = new ContentAccessClass();
                    temp.ORGANIZATION_NAME = Convert.ToString(reader["EMPLOYEEID"]);
                    temp.ID_USER = Convert.ToString(reader["id_user"]);
                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.UNAME = Convert.ToString(reader["UNAME"]);
                    temp.ID_ORGANIZATION = Convert.ToString(reader["id_organization"]);
                    temp.CATEGORYNAME = Convert.ToString(reader["categoryname"]);
                    temp.ID_CONTENT = Convert.ToString(reader["id_content"]);
                    temp.CONTENT_QUESTION = Convert.ToString(reader["CONTENT_QUESTION"]);
                    temp.USTATUS = Convert.ToString(reader["USTATUS"]);
                    DateTime tDate = Convert.ToDateTime(reader["LASTACCESS"]);
                    temp.LASTACCESS = tDate.ToString("dd-MM-yyyy HH:mm");
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
               throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<LoginExceptionClass> getLoginExceptionReport(string STR)
        {
            List<LoginExceptionClass> log = new List<LoginExceptionClass>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LoginExceptionClass temp = new LoginExceptionClass();
                    temp.ORGANIZATION_NAME = Convert.ToString(reader["ORGANIZATION_NAME"]);
                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
                    temp.COUNTER = Convert.ToString(reader["COUNTER"]);
                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    temp.LASTNAME = Convert.ToString(reader["LASTNAME"]);
                    temp.USTATUS = Convert.ToString(reader["USTATUS"]);
                    string ddate = Convert.ToString(reader["LASTACCESS"]);
                    if (!string.IsNullOrEmpty(ddate))
                    {
                        DateTime tDate = Convert.ToDateTime(reader["LASTACCESS"]);
                        temp.LASTACCESS = tDate.ToString("dd-MM-yyyy HH:mm");
                    }
                    else
                    {
                        temp.LASTACCESS = "NA";
                    }
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<Assessmentdata> getAssessmentExceptionData(string STR)
        {
            List<Assessmentdata> log = new List<Assessmentdata>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Assessmentdata temp = new Assessmentdata();
                    temp.ASSESSMENT_TITLE = Convert.ToString(reader["ASSESSMENT_TITLE"]);
                    temp.USER = Convert.ToString(reader["USERID"]);
                    temp.ID_USER = Convert.ToString(reader["ID_USER"]);
                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    temp.LASTNAME = Convert.ToString(reader["LASTNAME"]);
                    temp.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
                    DateTime tDate = Convert.ToDateTime(reader["updated_date_time"]);
                    temp.Assigned_Date = tDate;
                    DateTime sDate = Convert.ToDateTime(reader["start_date"]);
                    temp.start_date = sDate;
                    DateTime eDate = Convert.ToDateTime(reader["expire_date"]);
                    temp.end_date = eDate;
                    temp.ID_ASSESSMENT = Convert.ToString(reader["id_assessment"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.USTATUS = Convert.ToString(reader["USTATUS"]);
                    log.Add(temp);

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<string> isAssessmentDone(string STR)
        {
            List<string> value = new List<string>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime timer = Convert.ToDateTime(reader["updated_date_time"]);
                        value.Add(timer.ToString("dd-MM-yyyy HH:mm"));
                    }

                }


            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return value;
        }
        public List<PROGRAMCOMPLETE> getProgramData(string STR)
        {
            List<PROGRAMCOMPLETE> log = new List<PROGRAMCOMPLETE>();
            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PROGRAMCOMPLETE temp = new PROGRAMCOMPLETE();
                    // temp.ORGANIZATION_NAME = Convert.ToString(reader["ORGANIZATION_NAME"]);
                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.ID_USER = Convert.ToString(reader["id_user"]);
                    temp.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
                    temp.UNAME = Convert.ToString(reader["UNAME"]);
                    temp.CATEGORYNAME = Convert.ToString(reader["CATEGORYNAME"]);
                    temp.ID_CATEGORY = Convert.ToInt32(reader["ID_CATEGORY"]);
                    temp.start_date = Convert.ToDateTime(reader["start_date"]);
                    temp.end_date = Convert.ToDateTime(reader["expiry_date"]);
                    temp.assigned_date = Convert.ToDateTime(reader["assigned_date"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    temp.USTATUS = Convert.ToString(reader["USTATUS"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<AssessmentAuditClass> getAssessmentReport(string STR)
        {
            List<AssessmentAuditClass> log = new List<AssessmentAuditClass>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AssessmentAuditClass temp = new AssessmentAuditClass();

                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    temp.LASTNAME = Convert.ToString(reader["LASTNAME"]);
                    DateTime tDate = Convert.ToDateTime(reader["recorded_timestamp"]);
                    temp.recorded_timestamp = tDate.ToString("dd-MM-yyyy HH:mm");
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.LOCATION = Convert.ToString(reader["LOCATION"]);
                    temp.DESIGNATION = Convert.ToString(reader["user_designation"]);
                    temp.RMUSER = Convert.ToString(reader["RMUSER"]);
                    temp.id_assessment_audit = Convert.ToInt32(reader["id_assessment_audit"]);
                    temp.id_assessment = Convert.ToInt32(reader["id_assessment"]);
                    temp.id_user = Convert.ToInt32(reader["id_user"]);
                    temp.id_assessment_question = Convert.ToInt32(reader["id_assessment_question"]);
                    temp.id_assessment_answer = Convert.ToInt32(reader["id_assessment_answer"]);
                    temp.value_sent = Convert.ToInt32(reader["value_sent"]);
                    temp.attempt_no = Convert.ToInt32(reader["attempt_no"]);
                    temp.USTATUS = Convert.ToString(reader["USTATUS"]);
                    log.Add(temp);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }


        public List<LastAssessment> getAssessmentStatsSql(string STR)
        {
            List<LastAssessment> log = new List<LastAssessment>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LastAssessment temp = new LastAssessment(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }


        public List<AssessmentSlab> getAssessmentSlabSql(string STR)
        {
            List<AssessmentSlab> log = new List<AssessmentSlab>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AssessmentSlab temp = new AssessmentSlab(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }

        public List<AssessmentSlabGraph> getAssessmentSlabGraphSql(string STR)
        {
            List<AssessmentSlabGraph> log = new List<AssessmentSlabGraph>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AssessmentSlabGraph temp = new AssessmentSlabGraph(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }



        public List<TopContent> getTopContentSql(string STR)
        {
            List<TopContent> log = new List<TopContent>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TopContent temp = new TopContent(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }
        public List<TopContentUser> getTopContentUserSql(string STR)
        {
            List<TopContentUser> log = new List<TopContentUser>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TopContentUser temp = new TopContentUser(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }

        public List<ActiveUser> getActiveUserList(string STR)
        {
            List<ActiveUser> log = new List<ActiveUser>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ActiveUser temp = new ActiveUser(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }

        public List<UserStat> getUserStat(string STR)
        {
            List<UserStat> log = new List<UserStat>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserStat temp = new UserStat(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }


        public List<UserCount> getUserCountStat(string STR)
        {
            List<UserCount> log = new List<UserCount>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserCount temp = new UserCount(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }


        public List<ContentCounter> getContentCounter(string STR)
        {
            List<ContentCounter> log = new List<ContentCounter>();

            try
            {
                MySqlCommand command = null;
                string query = STR;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ContentCounter temp = new ContentCounter(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }
            return log;
        }



        public int getAssessmentTotalCount(string sqls)
        {
            int count = 0;

            try
            {
                MySqlCommand command = null;
                string query = sqls;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int counter = Convert.ToInt32(reader["counter"]);
                    count = counter;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { conn.Close(); }

            return count;
        }
    }
}
