using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CEDashboardModel
    {
    }

    public class TGCStandard
    {
        public List<IndustryStandard> MyScore { get; set; }
        public List<IndustryStandard> BenchmarkScore { get; set; }
    }

    public class IndustryStandard
    {
        public int id_ce_evaluation_jobindustry { get; set; }
        public string ce_job_industry { get; set; }
        public string ce_industry_code { get; set; }
        public int job_point { get; set; }
    }

    public class tbl_ce_evaluation_jobindustry
    {
        public int id_ce_evaluation_jobindustry { get; set; }
        public int id_organization { get; set; }
        public string ce_job_industry { get; set; }
        public string ce_industry_code { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_header { get; set; }
        public string tile_image { get; set; }
        public int tile_position { get; set; }
        public string buttontext { get; set; }

        
    }
    public class tbl_ce_evaluation_jobrole
    {
        public int id_ce_evaluation_jobrole { get; set; }
        public int id_organization { get; set; }
        public string ce_industry_role { get; set; }
        public string ce_role_code { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }
    public class tbl_ce_evaluation_jobrole_user_mapping
    {
        public int id_ce_evaluation_jobrole_user_mapping { get; set; }
        public int id_ce_evaluation_jobrole { get; set; }
        public int id_user { get; set; }
        public DateTime dated_time_stamp { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }
    public class tbl_ce_evaluation_jobindustry_user_mapping
    {
        public int id_ce_evaluation_jobindustry_user_mapping { get; set; }
        public int id_ce_evaluation_jobindustry { get; set; }
        public int id_user { get; set; }
        public DateTime dated_time_stamp { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public class CompanyMaster
    {
        public int JOB_ID_ORGANIZATION { get; set; }
        public string COMPANY_NAME { get; set; }
    }

    public class CompanyDetail
    {
        public int id_ce_evaluation_jobrole { get; set; }
        public string ce_industry_role { get; set; }
        public string ce_role_code { get; set; }
        public int organization_benchmark_jobpoint { get; set; }
    }

    public class CESuggestedCompany
    {
        public int JOB_ID_ORGANIZATION { get; set; }
        public string COMPANY_NAME { get; set; }
        public List<CompanyJobPoint> CESRoleList { get; set; }
    }

    public class CompanyJobPoint
    {
        public RoleClass roleData { get; set; }
        public int MyJobScore { get; set; }
        public int OtherJobScore { get; set; }
        public int BenchmarkJobScore { get; set; }
        public int HighestScore { get; set; }
        public List<MyCEJobPoints> ceJobPoints { get; set; }
    }

    public class CompanyCEJobSetup
    {
        public int id_ce_career_evaluation_master { get; set; }
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int no_of_question { get; set; }
        public int job_points_for_ra { get; set; }
        public int ce_benchmark_jobpoint { get; set; }
        public int highest_score { get; set; }
    }

    public class MyCEJobPoints
    {
        public int id_ce_career_evaluation_master { get; set; }
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int no_of_question { get; set; }
        public int ce_benchmark_jobpoint { get; set; }
        public int highest_score { get; set; }
        public int my_score { get; set; }
        public int other_score { get; set; }
    }

    public class cpIndustryRole
    {
        public RoleClass cpRole { get; set; }
        public List<cpCompany> cpCompany { get; set; }
    }

    public class cpCompany
    {
        public CompanyRoles rowCompany { get; set; }
        public List<IndustyrRole> rowIndustry { get; set; }
    }

    public class CompanyDetail1
    {
        public RoleClass roleList { get; set; }
        public List<IndustyrRole> RowHeaders { get; set; }
    }

    public class IndustyrRole
    {
        public int id_ce_industry { get; set; }
        public string ce_industry { get; set; }
        public int role_job_point { get; set; }
        public string company_job_point { get; set; }
    }

    public class CompanyRoles
    {
        public string ce_company_name { get; set; }
        public int job_point { get; set; }
        public int orderno { get; set; }
    }

    public class RoleClass
    {
        public int id_ce_evaluation_jobrole { get; set; }
        public string ce_industry_role { get; set; }
        public int counter { get; set; }
    }

    public class CEDashboard
    {
        public tbl_ce_evaluation_tile tile { get; set; }
        public int latest_attempt_no { get; set; }
        public int last_attempt_no { get; set; }
        public List<CEAnswerKey> CareerDriver { get; set; }
        public List<RoleClass> ceRoles { get; set; }
        public List<CEAssessment> ceEvaluation { get; set; }
        public int ceCurrentScore { get; set; }
        public int cePreviousScore { get; set; }
        public string ceCurrentStatus { get; set; }
        public string cePreviousStatus { get; set; }
        public double ceCurrentPercentage { get; set; }
        public List<CEJobRoles> jobRoles { get; set; }
        public List<CESuggestedCompany> suggestedCompany { get; set; }
        public TGCStandard tgcStandard { get; set; }
        public List<RoleClass> preferedRole { get; set; }
        public List<RoleClass> suggestedRole { get; set; }
        public string psyCrf { get; set; }
        public int psyIndex { get; set; }



    }

    public class CEAssessment
    {
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int ce_assessment_type { get; set; }
        public string cea_type { get; set; }
        public int job_points_for_ra { get; set; }
        
        public List<CEData> CEAssessList { get; set; }
    }

    public class CEAnswerKey
    {
        public int id_ce_evalution_answer_key { get; set; }
        public string answer_key { get; set; }
        public string key_code { get; set; }
        public int job_point { get; set; }
    }

    public class JobPoint
    {
        public int job_point { get; set; }
        public int attempt_no { get; set; }
      
    }
    public class JobPointDated
    {
        public int job_point { get; set; }
        public int attempt_no { get; set; }
        public string cetimestamp { get; set; }
    }
    public class CEData
    {
        public int job_point { get; set; }
        public int attempt_no { get; set; }
        public string cetimestamp { get; set; }

    }

    public class CEJobRoles
    {
        public int id_ce_industry_role { get; set; }
        public string ce_industry_role { get; set; }
        public string description { get; set; }
        public List<CEJobIndustry> Industry { get; set; }
    }

    public class CEJobIndustry
    {
        public int id_ce_industry { get; set; }
        public int id_organization { get; set; }
        public string ce_industry { get; set; }
        public int id_ce_industry_role { get; set; }
        public int role_job_point { get; set; }
    }


    public class tbl_ce_evaluation_completion_report
    {
        public int id_ce_evaluation_completion_report { get; set; }
        public int id_ce_evaluation_tile { get; set; }
        public string ce_evaluation_completion_token { get; set; }
        public int id_user { get; set; }
        public int id_organization { get; set; }
        public double completion_result { get; set; }
        public int attempt_no { get; set; }
        public DateTime dated_time_stamp { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }


}