using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CEDataClass
    {
    }
    public  class tbl_ce_evaluation_tile
    {
        public int id_ce_evaluation_tile { get; set; }
        public Nullable<int> id_organization { get; set; }
        public string ce_evaluation_tile { get; set; }
        public string ce_evaluation_code { get; set; }
        public string description { get; set; }
        public Nullable<int> sequence_order { get; set; }
        public Nullable<int> validation_period { get; set; }
        public string image_path { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
        public int cooling_period { get; set; }

        
    }
    public  class tbl_ce_career_evaluation_master
    {
        public int id_ce_career_evaluation_master { get; set; }
        public int id_organization { get; set; }
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int id_ce_evaluation_tile { get; set; }
        public string ce_description { get; set; }
        public int validation_period { get; set; }
        public Nullable<int> ordering_sequence_number { get; set; }
        public Nullable<int> no_of_question { get; set; }
        public Nullable<int> is_time_enforced { get; set; }
        public Nullable<int> time_enforced { get; set; }
        public Nullable<int> ce_assessment_type { get; set; }
        public Nullable<int> job_points_for_ra { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
        public string ce_image { get; set; }

        
    }

    public partial class tbl_ce_evaluation_progdist_mapping
    {
        public int id_ce_evaluation_progdist_mapping { get; set; }
        public string ce_evaluation_token { get; set; }
        public Nullable<int> id_ce_career_evaluation_master { get; set; }
        public Nullable<int> id_brief_question { get; set; }
        public Nullable<int> id_user { get; set; }
        public Nullable<int> question_link_type { get; set; }
        public Nullable<System.DateTime> date_time_stamp { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }

    public partial class tbl_ce_evaluation_log
    {
        public int id_ce_evaluation_log { get; set; }
        public int id_user { get; set; }
        public Nullable<int> id_organization { get; set; }
        public int id_ce_career_evaluation_master { get; set; }
        public string json_response { get; set; }
        public int attempt_no { get; set; }
        public Nullable<double> ce_evaluation_result { get; set; }

        public string cetimespan { get; set; }
        
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
        public int id_job { get; set; }

    }
    public class APIRESULT
    {
        public string STATUS { get; set; }
        public List<CETile> DETAIL { get; set; }
    }
    public class APIRESULTBanner
    {
        public string STATUS { get; set; }
        public List<Banner> Banner { get; set; }
    }
    public class APIRESULTCatTile
    {
        public string STATUS { get; set; }
        public List<JOBCATTILE> Tile { get; set; }
    }
    public class APIRESULTCat
    {
        public string STATUS { get; set; }
        public List<JOBCATEGORYLIST> Tile { get; set; }
    }

    public class tbl_job_category_header
    {
        public int id_header { get; set; }
        public string header { get; set; }
        public string status { get; set; }
        public object tbl_job_category_headercol { get; set; }
        public DateTime updated_date_time { get; set; }
        public List<tbl_job_category> category { get; set; }
    }
    public class tbl_job_category
    {
        public int id_job_category { get; set; }
        public string job_category { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_header { get; set; }
        public string tile_image { get; set; }
        public int tile_position { get; set; }

        
    }
    public class JOBCATTILE
    {
        public int id_job_category { get; set; }
        public string job_category { get; set; }
        public string tile_image { get; set; }
        public int tile_position { get; set; }
        public string buttontext { get; set; }



    }
    public class JOBCATEGORYLIST
    {
        public int id_job_category { get; set; }
        public string job_category { get; set; }
        public string tile_image { get; set; }
        public int tile_position { get; set; }



    }



    public class tbl_brief_enquiry
    {
        public int id_enquiry { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string brief_title { get; set; }
        public string enquiry { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }    
    }
 


    public partial class tbl_ce_evaluation_index
    {
        public int id_ce_evaluation_index { get; set; }
        public int id_ce_career_evaluation_master { get; set; }
        public int id_brief_master { get; set; }
        public int id_organization { get; set; }
        public int id_user { get; set; }
        public int attempt_no { get; set; }
        public string ce_evaluation_token { get; set; }
        public System.DateTime dated_time_stamp { get; set; }
        public string status { get; set; }
        public System.DateTime updated_date_time { get; set; }
    }

    public partial class tbl_ce_evalution_psychometric_setup
    {
        public int id_organization { get; set; }
        public int id_ce_career_evaluation_master { get; set; }
        public int id_brief_question { get; set; }
        public int id_brief_answer { get; set; }
        public int id_ce_evalution_answer_key { get; set; }
        public string key_code { get; set; }
        public string answer_key { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public partial class tbl_ce_evaluation_audit
    {
        public int id_ce_evaluation_audit { get; set; }
        public Nullable<int> id_ce_career_evaluation_master { get; set; }
        public Nullable<int> id_organization { get; set; }
        public Nullable<int> id_user { get; set; }
        public Nullable<int> id_brief_question { get; set; }
        public Nullable<int> question_complexity { get; set; }
        public Nullable<int> id_brief_answer { get; set; }
        public Nullable<int> value_sent { get; set; }
        public Nullable<int> attempt_no { get; set; }
        public Nullable<System.DateTime> recorded_timestamp { get; set; }
        public Nullable<int> audit_result { get; set; }
        public Nullable<int> job_point { get; set; }
        public Nullable<int> id_ce_evalution_answer_key { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }
}