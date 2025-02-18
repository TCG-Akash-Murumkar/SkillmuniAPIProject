using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CEModel
    {

        private MySqlConnection connection = null;

        public CEModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }
    }

    public class ResultResponseBody
    {
        public string status { get; set; }
        public CEReturnResponse data { get; set; }
        public string CETime { get; set; }
        public string Description { get; set; }

    }




    public class CETile
    {
        public int id_organization { get; set; }
        public string ce_evaluation_tile { get; set; }
        public string ce_evaluation_code { get; set; }
        public string description { get; set; }
        public int sequence_order { get; set; }
        public string image_path { get; set; }

        public List<CECategory> CECategory { get; set; }
        public bool reattempt { get; set; }
        public bool cooling_period { get; set; }

        public string cooling_period_expiry { get; set; }


    }

    public class CECategory
    {
        public int id_ce_career_evaluation_master { get; set; }
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int id_ce_evaluation_tile { get; set; }
        public string ce_description { get; set; }
        public int validation_period { get; set; }
        public int ordering_sequence_number { get; set; }
        public int no_of_question { get; set; }
        public int is_time_enforced { get; set; }
        public int time_enforced { get; set; }
        public int ce_assessment_type { get; set; }
        public int job_points_for_ra { get; set; }
        public string CEToken { get; set; }
        public string ce_image { get; set; }
        public bool cFlag { get; set; }
        public int last_attempt_number { get; set; }

        




    }

    public class CEBriefBody
    {
        public CECategory CEBody { get; set; }
        public List<CEQuestionList> QTNLIST { get; set; }
        public BriefReturnResponse RESULT { get; set; }
        public int RESULTSTATUS { get; set; }
        public double RESULTSCORE { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public tbl_ce_evaluation_tile tile { get; set; }
    }

    public class CEQuestionList
    {
        public int question_complexity { get; set; }
        public string question_complexity_label { get; set; }
        public string CEToken { get; set; }
        public int qtnnum { get; set; }

        public tbl_brief_question question { get; set; }
        public List<tbl_brief_answer> answers { get; set; }
    }
    public class CEQuestionSorted
    {
        public tbl_brief_question question { get; set; }
        public int ordering_sequence { get; set; }
    }

    public class CERequest
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public int BID { get; set; }
        public string CRF { get; set; }
        public string TOKEN { get; set; }
        public List<CEASRQ> ASRQ { get; set; }
        public string CETime { get; set; }
        public int ID_JOB { get; set; }

    }

    public class CEASRQ
    {
        public int QID { get; set; }
        public int ANS { get; set; }
        public string VAL { get; set; }
        public string KVAL { get; set; }
    }

    public class TEMPANS
    {
        public int AID { get; set; }
        public int AKVAL { get; set; }
        public int SVAL { get; set; }
    }

    public class AnswerKeyBlock
    {
        public int id_ce_evalution_answer_key { get; set; }
        public string key_code { get; set; }
        public string answer_key { get; set; }
        public int job_point { get; set; }
        public string aklogo { get; set; }
        public string Description { get; set; }
    }


}