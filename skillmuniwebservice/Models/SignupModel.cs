using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class SignupModel
    {

        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string MOBILENO { get; set; }
        public string MAILID { get; set; }
        public string PROFILEIMAGE { get; set; }
        public string ID_USER { get; set; }
        public string response_status { get; set; }
        public string response_message { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string College { get; set; }
        public string GraduationYear { get; set; }
        public string State { get; set; }
        public int id_degree { get; set; }
        public int id_stream { get; set; }
        public string ref_code { get; set; }
        public string COUNTRY { get; set; }
        public int STUDENT { get; set; }
        public int NOTSTUDENT { get; set; }
        public string OTHERSTREAM { get; set; }
        public int id_foundation { get; set; }
        public string clg_country { get; set; }
        public string clg_state { get; set; }
        public string clg_city { get; set; }

    }
    public class NotificationData
    {
        public string Image { get; set; }

    }
    public class JobUrl
    {
        public string Url { get; set; }

    }
    public class Banner
    {
        public string banner_name { get; set; }
        public string banner_image { get; set; }
    }
    public class tbl_graduation_year
    {
        public int id_graduation_year { get; set; }
        public string graduation_year { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }



    public class tbl_user_extra_curricular_certificates
    {
        public int id_certificate { get; set; }
        public int id_user { get; set; }
        public string certificate_file { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class tbl_user_job_preferences
    {
        public int id_job_preference { get; set; }
        public int id_location { get; set; }
        public string job_type { get; set; }
        public int experience_years { get; set; }
        public int experience_months { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_user { get; set; }

    }

    public class tbl_user_job_preferences_skill
    {
        public int id_skill { get; set; }
        public int id_user { get; set; }

        public string skill { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }
    public class tbl_user_job_preferences_category
    {
        public int id_pref_category { get; set; }
        public int id_user { get; set; }

        public int id_category { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }

    public class tbl_user_job_preferences_location
    {
        public int id_pref_location { get; set; }
        public int id_user { get; set; }

        public int id_location { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }

    public class tbl_user_job_preferences_job_type
    {
        public int id_pref_job { get; set; }
        public int id_user { get; set; }

        public string job_type { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }





    public class videoresponse
    {
        public string filename { get; set; }
        public string baseurl { get; set; }



    }

    public class EpisodeResponse
    {
        public int id_brief_master { get; set; }
        public string brief_title { get; set; }
        public int episode_sequence { get; set; }
        public List<EpisodeContent> content { get; set; }



    }
    public class EpisodeContent
    {
        public string resouce_data { get; set; }
        public string brief_destination { get; set; }

    }

    public class QuestionEvaluationResponse
    {
        public int score { get; set; }
        public int attempt_no { get; set; }
        public int is_correct { get; set; }
        public int id_selected_answer { get; set; }
        public int id_correct_answer { get; set; }






    }

    public class EpisodewiseDataResponse
    {
        public int overall_score { get; set; }
        public int overall_rank { get; set; }
        public List<EpisodeData> Epi { get; set; }

    }
    public class EpisodewiseLeaderBoardResponse
    {
        public int EpisodeId { get; set; }
        public List<EpisodewiseLeaderRankList> RankList { get; set; }

    }
    public class EpisodewiseLeaderRankList
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string profile_image { get; set; }
        public int total_score { get; set; }
        public int rank { get; set; }

    }


    public class EpisodeStatusResponse
    {
        public int episode_sequence { get; set; }
        public int id_brief_master { get; set; }
        public string status { get; set; }


    }
    public class EpisodeData
    {
        public int Episod_score { get; set; }
        public int Episode_rank { get; set; }
        public int episode_sequence { get; set; }
        public int id_brief_master { get; set; }

    }



    public class tbl_episode_log
    {
        public int id_episode_log { get; set; }
        public int id_user { get; set; }
        public int oid { get; set; }
        public int id_brief_master { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }
    public class MydashboardResponse
    {
        public List<QuestionResponse> Question { get; set; }
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public int UID { get; set; }
        public int OID { get; set; }
        public string profile_image { get; set; }





    }
    public class QuestionResponse
    {
        public int id_brief_question { get; set; }
        public string brief_question { get; set; }
        //public int id_organization { get; set; }
        //public int id_brief_master { get; set; }
        public List<tbl_brief_answer> answer { get; set; }
        public List<tbl_user_quiz_log> attempt_log { get; set; }
        public int max_score { get; set; }
        public int is_question_active { get; set; }
        public int no_of_attempts { get; set; }
        public int earned_marks { get; set; }


    }
    public class tbl_user_quiz_log
    {
        public int id_log { get; set; }
        public int id_user { get; set; }
        public int id_brief { get; set; }
        public int id_question { get; set; }
        public int id_correct_answer { get; set; }
        public int id_selected_answer { get; set; }
        public string status { get; set; }
        public int is_correct { get; set; }
        public DateTime updated_date_time { get; set; }
        public int attempt_no { get; set; }
        public int score { get; set; }
        public int total { get; set; }




    }
    public class tbl_question_episode_mapping
    {
        public int id_mapping { get; set; }
        public int id_brief { get; set; }
        public int id_question { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }





    }
    public class preferencemodel
    {
        public string id_location { get; set; }
        public int id_user { get; set; }
        public int experience_years { get; set; }
        public string job_type { get; set; }
        public int experience_months { get; set; }
        public string skill { get; set; }
        public string category { get; set; }
        public string resumepath { get; set; }
        public string certificatepath { get; set; }
        public List<tbl_ce_evaluation_jobindustry_user_mapping> industry { get; set; }
        public List<tbl_ce_evaluation_jobrole_user_mapping> role { get; set; }
        public string industry_str { get; set; }
        public string role_str { get; set; }
        public int isVideoCvPresent {get;set;}
        public string VideoCVStatus { get; set; }
        public string VideoCVLink { get; set; }
        public int isClassicCVPresent { get; set; }
        public string ClassicCvLink { get; set; }


        //public List<tbl_user_job_preferences_skill> SKILLLIST { get; set; }
        // public List<tbl_user_job_preferences_category> CATLIST { get; set; }
        // public List<tbl_user_job_preferences_location> LOCLIST { get; set; }
        // public List<tbl_user_job_preferences_job_type> JOBTYPELIST { get; set; }




    }
    public class ReferralHistory
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public string profile_pic { get; set; }
        public int credit_points { get; set; }
        public DateTime date { get; set; }


    }

    public class tbl_referral_code_user_mapping
    {
        public int id_mapping { get; set; }
        public int id_user { get; set; }
        public string referral_code { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int referral_points { get; set; }


    }


    public class tbl_user_log_master
    {
        public int id_log { get; set; }
        public int id_user { get; set; }
        public string is_registered { get; set; }
        public int academic_tiles { get; set; }
        public int study_abroad { get; set; }
        public int job { get; set; }
        public int entrepreneurship { get; set; }
        public DateTime updated_date_time { get; set; }


    }
 

    public class categoryid
    {
        public string id_category { get; set; }
    }
    public class conac1
    {
        public string location { get; set; }
        public string percentage { get; set; }

    }


}