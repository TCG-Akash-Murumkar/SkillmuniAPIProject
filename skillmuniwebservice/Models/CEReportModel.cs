using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CEReportModel
    {
        private m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public CEReportModel()
        {
        }

        public void getData(string ce_evaluation_token)
        {
            string cesql01 = "SELECT * FROM tbl_ce_evaluation_index where lower(ce_evaluation_token)=lower('" + ce_evaluation_token + "') limit 1";
            tbl_ce_evaluation_index cindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(cesql01).FirstOrDefault();
            if (cindex != null)
            {
            }
        }

        public CEReturnResponse getCareerEvaluation(tbl_ce_evaluation_index cid)
        {
            string cesql01 = "SELECT a.id_ce_career_evaluation_master, a.id_ce_evaluation_tile, career_evaluation_title, career_evaluation_code, ce_evaluation_tile, ce_description, validation_period, ordering_sequence_number, no_of_question, is_time_enforced, time_enforced, CASE WHEN ce_assessment_type = 1 THEN 'SUL - MCA' WHEN ce_assessment_type = 2 THEN 'SUL psychometric ' END ce_assessment_type FROM tbl_ce_career_evaluation_master a, tbl_ce_evaluation_tile b WHERE a.id_ce_evaluation_tile = b.id_ce_evaluation_tile AND a.id_ce_career_evaluation_master = " + cid.id_ce_career_evaluation_master + " LIMIT 1";
            CEMaster cmaster = db.Database.SqlQuery<CEMaster>(cesql01).FirstOrDefault();
            if (cmaster != null)
            {
            }
            return null;
        }
    }

    public class CEReturnResponse
    {
        public List<CEUserInput> ceReturn { get; set; }
        public string returnStat { get; set; }
        public int rightCount { get; set; }
        public int totalCount { get; set; }
        public int attemptno { get; set; }
        public double percentage { get; set; }
        public List<ComplexityResult> complexity { get; set; }
        public List<AnswerKeyBlock> answerKeyBlock { get; set; }
        public string CETime { get; set; }

    }

    public class CEUserInput
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string WANS { get; set; }
        public int srno { get; set; }
        public int jpscore { get; set; }
        public int is_right { get; set; }
        public int question_complexity { get; set; }
        public string question_complexity_label { get; set; }
        public List<CEAnswerBody> answerBody { get; set; }
        public tbl_brief_question questionBody { get; set; }
    }

    public class CEMaster
    {
        public int id_ce_career_evaluation_master { get; set; }
        public int id_ce_evaluation_tile { get; set; }
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public string ce_evaluation_tile { get; set; }
        public string ce_description { get; set; }
        public int validation_period { get; set; }
        public int ordering_sequence_number { get; set; }
        public int no_of_question { get; set; }
        public int is_time_enforced { get; set; }
        public int time_enforced { get; set; }
        public int ce_assessment_type { get; set; }
    }
    public class CEAnswerBody
    {
        public int id_brief_answer { get; set; }
        public Nullable<int> id_organization { get; set; }
        public Nullable<int> id_brief_question { get; set; }
        public string brief_answer { get; set; }
        public Nullable<int> is_correct_answer { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
        public string choice_image { get; set; }
        public Nullable<int> choice_type { get; set; }
        public string answer_role { get; set; }

    }
}