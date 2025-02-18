using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class Response
    {
        public string ResponseCode { get; set; }
        public int ResponseAction { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class UserCredentials
    {
        
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
     
    }
    public class ProfileImageData
    {

        public int UID { get; set; }
        public string ImageBase { get; set; }
        public string ImageExtn { get; set; }

    }
    public class AuthResponse
    {
        public int IDUSER { get; set; }
        public string USERID { get; set; }
        public int OID { get; set; }

        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PROFILE_IMAGE { get; set; }
        public string AuthStatus { get; set; }
        public string AuthMessage { get; set; }
        public int id_org_game_unit { get; set; }
        public string UserFunction { get; set; }
        public int is_first_time_login { get; set; }
        public string unit { get; set; }
        public int avatar_type { get; set; }





        //public List<GameUserLog> Score { get; set; }



    }

    public class OrgGameLeaderBoardResponse
    {
        public List<GameUserLog> OverAll { get; set; }
        //public List<GameUserLog> Unit { get; set; }
        //public List<UnitRankLog> UnitStanding { get; set; }
        public List<UnitRankLog> CENTRALUnits { get; set; }
        public List<UnitRankLog> NONCENTRALUnits { get; set; }
        public List<UnitRankLog> OVERALLUnits { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }



    }
    public class tbl_org_game_unit_master
    {
        public int id_org_game_unit { get; set; }
        public string unit { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_org { get; set; }
        public int unit_type { get; set; }

        


    }

    public class UnitCompletionReport
    {
        public DateTime LastCompletedDate { get; set; }
        public string UnitName { get; set; }


    }

    public class UnitCompletionAverageReport
    {
        public TimeSpan TotalTime { get; set; }
        public int averageSecs { get; set; }
        public string UnitName { get; set; }


    }
    public class UnituserScoreWiseData
    {

        public string UnitName { get; set; }
        public List<OrguserScoreData> UserList {get;set;}


    }
    public class OrguserScoreData
    {

        public string Name { get; set; }
        public double AssessmnetScore { get; set; }
        public int Rank { get; set; }
        public TimeSpan TimeTakenToComplete { get; set; }


    }


    public class GameUserLog
    {
        public int id_org_game { get; set; }
        public int total_score_gained { get; set; }
        public int total_score_detected { get; set; }
        public int current_overallscore { get; set; }
        public string game_title { get; set; }
        public int rank { get; set; }
        public double assessment_score { get; set; }
        public int id_user { get; set; }

        public string Name { get; set; }
        public string PROFILE_IMAGE { get; set; }
        public int final_assessmnet_right_count { get; set; }
        public int final_assessmnet_wrong_count { get; set; }
        public int final_assessmnet_total_count { get; set; }


    }
    public class UnitRankLog
    {
        public int id_org_game { get; set; }
        public double AverageScore { get; set; }        
        public int Rank { get; set; }
        public string Unit { get; set; }
        public int IdUnit { get; set; }
        public double CompletionPercentage { get; set; }
        public double RankPercentage { get; set; }


    }


    public class LogsResponce
    {
        public List<SearchResponce> CONTENTS { get; set; }
        public List<SubscriptionPack> PACK { get; set; }
        public string USER_SKU { get; set; }
        public int COUNTER { get; set; }
    }

    public class CartRequest
    {
        public int UID { get; set; }
        public int CID { get; set; }
    }

    public class CartCheckOut
    {
        public int UID { get; set; }
        public string CID { get; set; }
    }

    public class CartResponse
    {
        public int id_subscription_cart { get; set; }
        public List<SearchResponce> content { get; set; }
        public string id_user { get; set; }
    }

    public class SubscriptionPack
    {
        public int id_subscription_pack { get; set; }
        public string pack_name { get; set; }
        public int pack_size { get; set; }
        public string pack_price { get; set; }
        public string pack_sku { get; set; }
    }

    public class SearchGetResponce
    {
        public List<SearchResponce> searchResponce { get; set; }
        public List<AssessmentList> assessmentResponce { get; set; }
    }

    public class SearchResponce
    {
        public int ID_CONTENT { get; set; }
        public string CONTENT_QUESTION { get; set; }
        public int ID_CONTENT_LEVEL { get; set; }
        public string EXPIRYDATE { get; set; }
    }

    public class LoginResponse
    {
        public string ResponseCode { get; set; }
        public int ResponseAction { get; set; }
        public string ResponseMessage { get; set; }
        public int UserID { get; set; }
        public string LogoPath { get; set; }
        public string BannerPath { get; set; }
        public string ORGEMAIL { get; set; }
    }

    public class LoginResponseAuth
    {
        public string ResponseCode { get; set; }
        public int ResponseAction { get; set; }
        public string ResponseMessage { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LogoPath { get; set; }
        public string BannerPath { get; set; }
        public string ROLEID { get; set; }
        public string ORGID { get; set; }
        public string ORGEMAIL { get; set; }
        public string REURL { get; set; }
        public List<Menu> menu_response { get; set; }
        public string fullname { get; set; }
        public int log_flag { get; set; }
        public int profile_data { get; set; }
        public string profile_image { get; set; }
        public int total_score { get; set; }
        public int last_successive_level { get; set; }
        public string ref_id { get; set; }
        public string UserEmail { get; set; }
        public string college { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string college_state { get; set; }
        public string college_city { get; set; }
        public int id_college { get; set; }
        public int is_first_time_login { get; set; }

        



    }

    public class tbl_email_verification_key_log
    {
        public int id_log { get; set; }
        public int id_user { get; set; }
        public string secret_key { get; set; }
        public DateTime updated_date_time { get; set; }
        public string status { get; set; }

    }
    public class VerifyEmailKey
    {
        public int UID { get; set; }
        public string SecretKey { get; set; }


    }
    public class PasswordChangeData
    {
        public int UID { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }


    }
    public class PasswordData
    {
        public string UserID { get; set; }
        public string NewPassword { get; set; }
        public string SecretKey { get; set; }


    }




    public class tbl_user_level_log
    {
        public int id_level_log { get; set; }
        public int id_user { get; set; }
        public int level { get; set; }
        public int attempt_no { get; set; }
        public int score { get; set; }
        public int bonus { get; set; }
        public int total_score { get; set; }
        public DateTime updated_date_time { get; set; }
        public int is_qualified { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
        public List<tbl_user_assessment_log> assessment { get; set; }

    }

    public class tbl_entrepreneurship_master
    {
        public int id_entrepreneurship { get; set; }
        public string company_name { get; set; }
        public string founders { get; set; }
        public string foundation_date { get; set; }
        public string reason { get; set; }
        public int id_buisiness_stage { get; set; }
        public string revenue { get; set; }
        public string far_from_launch { get; set; }
        public string company_structure { get; set; }
        public string buisiness_stage_others { get; set; }
        public DateTime updated_date_time { get; set; }
        public string product_code { get; set; }
        public string website { get; set; }
        public int id_user { get; set; }


    }
    public class tbl_social_entrepreneurship
    {
        public int id_social_entrepreneurship { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string product { get; set; }
        public string message { get; set; }
        public string website { get; set; }
        public DateTime updated_date_time { get; set; }
       //public  List<tbl_social_entrepreneurship_product_mapping> map { get; set; }
       public string map { get; set; }

    }
    public class tbl_social_entrepreneurship_product_mapping
    {
        public int id_mapping { get; set; }
        public int id_social_entrepreneurship { get; set; }
        public int id_product { get; set; }
        public DateTime updated_date_time { get; set; }
      

    }



    public class tbl_entrepreneurship_files
    {
        public int id_file { get; set; }
        public string file { get; set; }
        public string extension { get; set; }
        public int id_entrepreneurship { get; set; }
        public DateTime updated_date_time { get; set; }
    }
    public class entrepreneurship_response
    {
        public int id_entrepreneurship { get; set; }
        public string status { get; set; }
      
    }
    public class tbl_user_enquiry_data
    {
        public int id_enquiry { get; set; }
        public int id_user { get; set; }
        public int enquiry_type { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public int enquiry_reason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }


    public class tbl_social_entrepreneurship_product_master
    {
        public int id_product { get; set; }
        public string product { get; set; }

        public string status { get; set; }

        public int web_flag { get; set; }

        public DateTime updated_date_time { get; set; }


    }

  
    public class EntPost
    {
        public int id_entrepreneurship { get; set; }
        public int files_count { get; set; }
        public List<EntFiles> files { get; set; }

    }
    public class EntFiles
    {        
        public string Base { get; set; }
        public string type { get; set; }

    }





    public class AttemptResponse
    {
        public int id_level { get; set; }
        public int last_attempt { get; set; }

    }

    public class UniversityNotification
    {
        public List<tbl_url_notification_master> general_notification { get; set; }
        public List<tbl_content_notification_master> content_notification { get; set; }


    }
    public class tbl_url_notification_master
    {
        public int id_content_notifcation { get; set; }
        public string notifcation_title { get; set; }
        public string notification_message { get; set; }
        public string notification_url { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }


    }
    public class tbl_content_notification_master
    {
        public int id_content_notifcation { get; set; }
        public int id_academic_tile { get; set; }
        public int id_brief_category_tile { get; set; }
        public string notifcation_title { get; set; }
        public string notification_message { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }
        public string tile_code { get; set; }
        public string category_tile { get; set; }
        public string message { get; set; }



    }



    public class tbl_user_assessment_log
    {
        public int id_user_assessment_log { get; set; }
        public int id_user { get; set; }
        public int level { get; set; }
        public int attempt_no { get; set; }
        public int id_question { get; set; }
        public int id_answer { get; set; }
        public int id_user_answer { get; set; }
        public DateTime updated_date_time { get; set; }
        public string status { get; set; }
        public int is_right { get; set; }

    }
    public class LeaderBoardData
    {
        public int id_user { get; set; }
        public string username { get; set; }

        public string profile_image { get; set; }
        public string location { get; set; }
        public int score { get; set; }
        public int bonus { get; set; }
        public int total_score { get; set; }


    }
    public class MasterLeaderBoardData
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string profile_image { get; set; }
        public int total_score { get; set; }
        public int id_brief_master { get; set; }
        public int Rank { get; set; }


    }
    public class MydashboardDataResponse
    {
        public int id_user { get; set; }
        public int overall_score { get; set; }
        public int overall_rank { get; set; }
        public List<MydashoardEpisodeData> Epi { get; set; }


    }
    public class MydashoardEpisodeData
    {
        public int id_brief_master { get; set; }
        public int episode_sequence { get; set; }
        public int Episode_rank { get; set; }
        public int Episod_score { get; set; }
        public List<MydashoardQuestionLog> question { get; set; }
        //public List<tbl_user_quiz_log> log { get; set; }
    }
    public class MydashoardQuestionLog
    {
        public int id_question { get; set; }
        public int attempts_count { get; set; }
        public int question_score { get; set; }
     
    }


    public class tbl_foundation_master
    {
        public int id_foundation { get; set; }
        public string foundation { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }








    public class AnswerResponse
    {
        public int ID_CONTENT { get; set; }
        public int ID_CATEGORY { get; set; }
        public int ID_THEME { get; set; }
        public string CONTENT_TITLE { get; set; }
        public string CONTENT_QUESTION { get; set; }
        public string EXPIRYDATE { get; set; }

        public int ID_CONTENT_ANSWER { get; set; }
        public string CONTENT_ANSWER_TITLE { get; set; }
        public string CONTENT_ANSWER_HEADER { get; set; }
        public string CONTENT_ANSWER1 { get; set; }
        public string CONTENT_ANSWER2 { get; set; }
        public string CONTENT_ANSWER3 { get; set; }
        public string CONTENT_ANSWER4 { get; set; }
        public string CONTENT_ANSWER5 { get; set; }
        public string CONTENT_ANSWER6 { get; set; }
        public string CONTENT_ANSWER7 { get; set; }
        public string CONTENT_ANSWER8 { get; set; }
        public string CONTENT_ANSWER9 { get; set; }
        public string CONTENT_ANSWER10 { get; set; }

        public string CONTENT_ANSWER_IMG1 { get; set; }
        public string CONTENT_ANSWER_IMG2 { get; set; }
        public string CONTENT_ANSWER_IMG3 { get; set; }
        public string CONTENT_ANSWER_IMG4 { get; set; }
        public string CONTENT_ANSWER_IMG5 { get; set; }
        public string CONTENT_ANSWER_IMG6 { get; set; }
        public string CONTENT_ANSWER_IMG7 { get; set; }
        public string CONTENT_ANSWER_IMG8 { get; set; }
        public string CONTENT_ANSWER_IMG9 { get; set; }
        public string CONTENT_ANSWER_IMG10 { get; set; }
        public string CONTENT_ANSWER_BANNER { get; set; }
        public string BANNER_REDIRECTION_URL { get; set; }

        public string CONTENT_ANSWER_COUNTER { get; set; }
        public Boolean HAS_ANSWER_STEP { get; set; }
        public List<SearchResponce> LinkedQuestion { get; set; }
        public List<SearchResponce> RelatedQuestion { get; set; }
        public Boolean has_feedback { get; set; }

        public int ID_FEEDBACK_BANK { get; set; }
        public string FEEDBACK_NAME { get; set; }
        public string FEEDBACK_QUESTION { get; set; }
        public string FEEDBACK_CHOICES { get; set; }
        public string FEEDBACK_IMAGE { get; set; }

        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public string ASSESSMENT_FLAG { get; set; }

        public string CONTENT_BANNER { get; set; }
        public string CONTENT_BANNER_URL { get; set; }
        public string CONTENT_BANNER_IMG { get; set; }
    }

    public class Notification
    {
        public int NOTIFICATION_CONFIG_ID { get; set; }
        public int NOTIFICATION_ID { get; set; }
        public string NOTIFICATION_TITLE { get; set; }
        public string NOTIFICATION_MESSAGE { get; set; }
        public string NOTIFICATION_KEY { get; set; }
        public string NOTIFICATION_DESCRIPTION { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public string EXPIRYDATE { get; set; }
        public string SENTDATE { get; set; }
        public string NOTIFICATION_TYPE { get; set; }
    }

    public class NotificationList
    {
        public List<Notification> READ { get; set; }
        public List<Notification> UNREAD { get; set; }
    }

    public class NotificationAlert
    {
        public int NOTIFICATION_ID { get; set; }
        public string NOTIFICATION_TITLE { get; set; }
        public string NOTIFICATION_MESSAGE { get; set; }
        public string NOTIFICATION_KEY { get; set; }
        public string NOTIFICATION_DESCRIPTION { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public string NOTIFICATION_TYPE { get; set; }
        public string ACTION_TYPE { get; set; }
        public string REDIRECTION_URL { get; set; }
        public int ID_USER { get; set; }
    }

    public class APIRESPONSE
    {
        public string KEY { get; set; }
        public string MESSAGE { get; set; }
    }

    public class tbl_api_authenticate
    {
        public string id_auth { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string updated_date_time { get; set; }
    }

    public class CategroyDashboard
    {
        public Category CATEGORY { get; set; }
        public List<SearchResponce> CONTENTLIST { get; set; }
        public List<AssessmentList> ASSESSMENTLIST { get; set; }
    }

    public class EventResponse
    {
        public List<EventThumbnail> READ { get; set; }
        public List<EventThumbnail> UNREAD { get; set; }
    }

    public class ScheduledEvent
    {
        public int id_scheduled_event { get; set; }
        public string event_title { get; set; }
        public string event_description { get; set; }
        public string registration_start_date { get; set; }
        public string registration_end_date { get; set; }
        public string event_start_datetime { get; set; }
        public string event_duration { get; set; }
        public string event_type { get; set; }
        public string event_group_type { get; set; }
        public string program_name { get; set; }
        public string program_description { get; set; }
        public string program_objective { get; set; }
        public string facilitator_name { get; set; }
        public string facilitator_organization { get; set; }
        public string program_image { get; set; }
        public string no_of_participants { get; set; }
        public string program_location { get; set; }
        public string attachment_type { get; set; }
        public string attachment_id { get; set; }
        public string attachment_title { get; set; }
        public string program_start_date { get; set; }
        public string program_duration_type { get; set; }
        public string program_duration { get; set; }
        public string program_duration_unit { get; set; }
        public string program_end_date { get; set; }
        public string attachment_info { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public string COMMENT { get; set; }
        public string REDIRECTION_URL { get; set; }
        public string is_approval { get; set; }
        public string is_response { get; set; }
        public string is_unsubscribe { get; set; }
    }

    public class EventThumbnail
    {
        public int id_scheduled_event { get; set; }
        public string event_title { get; set; }
        public string event_description { get; set; }
        public string registration_start_date { get; set; }
        public string registration_end_date { get; set; }
        public string event_start_datetime { get; set; }
        public string event_duration { get; set; }
        public string event_type { get; set; }
        public string event_group_type { get; set; }
        public string program_name { get; set; }
        public string program_description { get; set; }
        public string program_objective { get; set; }
        public string facilitator_name { get; set; }
        public string facilitator_organization { get; set; }
        public string no_of_participants { get; set; }
        public string program_location { get; set; }
        public string attachment_info { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public string COMMENT { get; set; }
    }

    public class Menu
    {
        public int id_menu { get; set; }
        public int id_org { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
    }

    public class version
    {
        public int id_version_control { get; set; }
        public string version_number { get; set; }
        public DateTime updated_date_time { get; set; }
    }
}