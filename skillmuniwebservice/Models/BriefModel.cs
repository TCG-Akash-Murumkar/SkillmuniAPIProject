using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using Newtonsoft.Json;
using m2ostnextservice.Models;
using static iTextSharp.text.pdf.AcroFields;
using System.Security.Cryptography;

namespace m2ostnextservice.Models
{
    public class B2COrg
    {
        public int OID { get; set; }
        public string ORG { get; set; }
    }

    public class B2COMPLEX
    {
        public int CID { get; set; }
        public string COMPLEX { get; set; }
    }

    public class TRANSUSER
    {
        public string FIRSTNAME { get; set; }
        public string USERTYPE { get; set; }
        public int ID_ROLE { get; set; }
        public int ID_USER { get; set; }
        public string USERID { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string EMPLOYEEID { get; set; }
        public string user_department { get; set; }
        public string user_designation { get; set; }
        public string user_function { get; set; }
        public string user_grade { get; set; }
        public string reporting_manager { get; set; }
        public int process_status { get; set; }
    }

    public class B2CResponse
    {
        public int IDS { get; set; }
        public int UID { get; set; }
        public string EMPID { get; set; }
        public int OID { get; set; }
        public int BID { get; set; }
        public double VALUE { get; set; }
        public string timestamp { get; set; }
        public int CLEVEL { get; set; }
    }

    public class ResumeResponse
    {
        public int ResumeFlag { get; set; }
        public string ResumePath { get; set; }
    }

    public class B2CSocial
    {
        public string SCTYPE { get; set; }
        public string SCID { get; set; }
        public string SCNAME { get; set; }
        public string SCEM { get; set; }
        public string SCPH { get; set; }
        public string IMEI { get; set; }
        public string SCSTATUS { get; set; }
    }

    public class tbl_user_game_score_log
    {
        public int id_game_score_log { get; set; }
        public int id_user { get; set; }
        public int id_game { get; set; }
        public int id_brief { get; set; }
        public int id_org { get; set; }
        public double score { get; set; }
        public string status { get; set; }
        public int id_academic_tile { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_metric { get; set; }
        public string metric_value { get; set; }

    }
    public class tbl_user_game_special_metric_score_log
    {
        public int id_game_score_log { get; set; }
        public int id_user { get; set; }
        public int id_game { get; set; }
        public int id_brief { get; set; }
        public int id_org { get; set; }
        public double score { get; set; }
        public string status { get; set; }
        public int id_academic_tile { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_special_metric { get; set; }
        public string special_metric_value { get; set; }

    }

    public class tbl_badge_data
    {
        public int id_badge_data { get; set; }
        public int id_game { get; set; }
        public int id_badge { get; set; }
        public int id_org { get; set; }
        public int required_score { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }
        public int id_metric { get; set; }

    }


    public class AssessmentScoreResponse
    {
        public int id_game { get; set; }
        public int id_brief { get; set; }
        public int id_metric { get; set; }
        public string metric_name { get; set; }
        public double metric_score { get; set; }

    }

    public class LeaderBoardResponse
    {
        public int id_game { get; set; }
        public int id_user { get; set; }
        public double userscore { get; set; }
        public string userleague { get; set; }
        public List<UserBadge> Badge { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }

        public string UserProfileImage { get; set; }
        public List<LeaderBoardUserList> UserList { get; set; }
        public string MailId { get; set; }
        public int social_dp_flag { get; set; }



    }
    public class UserScoreResponse
    {
        public int id_game { get; set; }
        public int id_user { get; set; }
        public double userscore { get; set; }
        public double specialmetricscore { get; set; }
        public int currency_value { get; set; }
        public string currency_name { get; set; }
        public string currency_image { get; set; }

    }
    public class tbl_leagues_data
    {
        public int id_league_data { get; set; }
        public int id_league { get; set; }
        public int id_theme { get; set; }
        public int id_game { get; set; }
        public double minscore { get; set; }
        public int evaluation_type { get; set; }
        public int expression_type { get; set; }
        public int movement_number { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int level { get; set; }
        public int id_metric { get; set; }
    }


    public class FootballThemeLeaderBoardHeader
    {
        public string specialmetric { get; set; }
        public string currency { get; set; }
        public string theme_metric { get; set; }
        public string currency_image { get; set; }

    }
    public class UserBadge
    {
        public int id_badge { get; set; }
        public string badge_name { get; set; }
        public string badge_image { get; set; }
        public int eligible_score { get; set; }


    }
    public class BriefTilResponse
    {
        public string Status { get; set; }
        public List<tbl_brief_category_tile> Tile { get; set; }


    }

    //sridhar create model for banner API on 18-09-19
    public class bannerApi
    {
        public string status { get; set; }
        public List<tbl_banner_config_master> ad_master_banner { get; set; }
    }
    // end
    public class tbl_banner_config_master
    {
        public int id_banner_config { get; set; }
        public string banner_name { get; set; }
        public int banner_location { get; set; }
        public int banner_position { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int banner_type { get; set; }
        public List<tbl_banner_body> bannerbody { get; set; }
        public List<tbl_banner_ad_config> banner_ad { get; set; }

    }

    public class tbl_banner_body
    {
        public int id_banner_body { get; set; }
        public string banner_url { get; set; }
        public string banner_image { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_banner_config { get; set; }
    }

    public class tbl_banner_ad_config
    {
        public int id_banner_ad_config { get; set; }
        public int id_banner_config { get; set; }
        public int id_academic_tile { get; set; }
        public int id_brief_category_tile { get; set; }
        public int brief_number { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }



    public class LeaderBoardUserList
    {
        public int id_game { get; set; }
        public int id_user { get; set; }
        public int Rank { get; set; }
        public string city { get; set; }
        public string UserProfileImage { get; set; }
        public string Username { get; set; }
        public double special_metric_score { get; set; }
        public int currencyvalue { get; set; }
        public double metric_score { get; set; }
        public string metric_image { get; set; }
        public List<UserBadge> Badge { get; set; }
        public string userleague { get; set; }

        public List<tbl_badge_master> userbadge { get; set; }

    }

    public class tbl_brief_tile_academic_mapping
    {
        public int id_tile_mapping { get; set; }
        public int id_academic_tile { get; set; }
        public int id_journey_tile { get; set; }
        public DateTime updated_date_time { get; set; }
        public string status { get; set; }
        public int id_org { get; set; }
        public string BriefTileCode { get; set; }

    }




    public class tbl_game_academic_mapping
    {
        public int id_mapping { get; set; }
        public int id_org { get; set; }
        public int id_game { get; set; }
        public int id_academic_tile { get; set; }
        public string user_assign_flag { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string game_name { get; set; }
        public string academic_tile_name { get; set; }


    }
    public class tbl_university_kpi_grid
    {
        public int id_kpi_grid { get; set; }
        public int id_kpi_master { get; set; }
        public double start_range { get; set; }
        public double end_range { get; set; }
        public string kpi_text { get; set; }
        public int kpi_value { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_game { get; set; }
        public int id_metric { get; set; }

    }
    public class tbl_university_special_point_grid
    {
        public int id_special_point_grid { get; set; }
        public int id_special_points { get; set; }
        public double start_range { get; set; }
        public int end_range { get; set; }
        public int special_value { get; set; }
        public string special_text { get; set; }
        public int special_metric { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_metric { get; set; }
        public int id_game { get; set; }
    }
    public class tbl_badge_master
    {
        public int id_badge { get; set; }
        public int id_theme { get; set; }
        public string badge_name { get; set; }
        public string badge_logo { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }
        public int WonFlag { get; set; }
        public int eligiblescore { get; set; }

        public string currency_name { get; set; }
        public int currency_value { get; set; }
        public int badge_count { get; set; }
        public int money_value { get; set; }
    }

    public class tbl_user_badge_log
    {
        public int id_badge_log { get; set; }
        public int id_user { get; set; }
        public int id_org { get; set; }
        public int id_badge { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_game { get; set; }

    }

    public class tbl_user_league_log
    {
        public int id_league_log { get; set; }
        public int id_user { get; set; }
        public int id_org { get; set; }
        public int id_game { get; set; }
        public int id_league { get; set; }
        public string league { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }

    public class tbl_badge_won_message
    {
        public int id_message { get; set; }
        public int id_game { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_badge { get; set; }
    }
    public class tbl_league_message
    {
        public int id_message { get; set; }
        public int id_game { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_league { get; set; }
    }


    public class tbl_theme_leagues
    {
        public int id_league { get; set; }
        public int id_theme { get; set; }
        public string league_name { get; set; }
        public string league_logo { get; set; }
        public string status { get; set; }
        public int id_org { get; set; }
        public DateTime updated_date_time { get; set; }
        public int level { get; set; }
    }

    public class LeagueInstance
    {
        public int id_league { get; set; }
        public int id_user { get; set; }
        public int league { get; set; }
        public string league_logo { get; set; }

    }




    public class tbl_social_platform_active_directory
    {
        public int id_social_platform_active_directory { get; set; }
        public int id_organization { get; set; }
        public int id_social_platform_master { get; set; }
        public string social_platform_code { get; set; }
        public int id_user { get; set; }
        public string social_code { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public class tbl_social_platform_master
    {
        public int id_social_platform_master { get; set; }
        public string social_platform_code { get; set; }
        public string social_platform_title { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class tbl_signup_config
    {
        public int id_signup_config { get; set; }
        public string reg_name { get; set; }
        public int mandatory { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }
    }


    public class APIBrief
    {
        public int id_organization { get; set; }
        public string brief_title { get; set; }
        public string brief_code { get; set; }
        public string brief_description { get; set; }
        public DateTime? datetimestamp { get; set; }
        public string scheduled_type { get; set; }
        public int override_dnd { get; set; }
        public int id_brief_master { get; set; }
        public int is_question_attached { get; set; }
        public int read_status { get; set; }
        public int action_status { get; set; }
        public int id_brief_category { get; set; }
        public int id_brief_sub_category { get; set; }
        public int id_user { get; set; }
        public int question_count { get; set; }
        public int RESULTSTATUS { get; set; }
        public double RESULTSCORE { get; set; }
        public string brief_subcategory { get; set; }
        public string brief_category { get; set; }
        public int SRNO { get; set; }
    }

    public class BriefReadStatus
    {
        public int BookMark { get; set; }
        public int Assessment { get; set; }
        public string BrfCode { get; set; }

    }

    public class tbl_buisiness_stages_master
    {
        public int id_buisiness_stage { get; set; }
        public string stage { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }



    public class BriefStatus
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public int TOTALCOUNT { get; set; }
        public int READCOUNT { get; set; }
        public int UNREADCOUNT { get; set; }
    }

    public class BriefScore
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public int TOTALCOUNT { get; set; }
        public int BRIEFTAKEN { get; set; }
        public int BRIEFSCORE { get; set; }
    }



    public class BriefAPIResource
    {
        public int id_organization { get; set; }
        public string brief_title { get; set; }
        public string brief_code { get; set; }
        public string brief_description { get; set; }
        public DateTime? datetimestamp { get; set; }
        public string scheduled_type { get; set; }
        public int override_dnd { get; set; }
        public int id_brief_master { get; set; }
        public int is_question_attached { get; set; }
        public int read_status { get; set; }
        public int action_status { get; set; }
        public Nullable<int> id_brief_category { get; set; }
        public int id_brief_sub_category { get; set; }
        public int id_user { get; set; }
        public int question_count { get; set; }
        public int RESULTSTATUS { get; set; }
        public double RESULTSCORE { get; set; }
        public string brief_subcategory { get; set; }
        public string brief_category { get; set; }
        public int SRNO { get; set; }
        public string brief_template { get; set; }
        public List<BriefRow> briefResource { get; set; }
        public string RestrictionMessage { get; set; }
        public int RestrictionCode { get; set; }
        public int SlideCheckCount { get; set; }
        public DateTime? BrfDate { get; set; }
        public Cat_Mapping_Data ctmap { get; set; }
        public tbl_brief_m2ost_category_mapping cat_mapping { get; set; }
        public int brief_attachment_flag { get; set; }
        public string brief_attachement_url { get; set; }
        public int liked { get; set; }
        public int disliked { get; set; }




    }

    public class BriefResponse
    {
        public List<BriefAPIResource> BriefList { get; set; }
        public int ValidationNumber { get; set; }
    }


    public class tbl_brief_m2ost_category_mapping
    {
        public int id_mapping { get; set; }
        public int id_brief { get; set; }
        public int id_category { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public int type { get; set; }
        public string URL { get; set; }
        public DateTime updated_date_time { get; set; }
        public string CATEGORYNAME { get; set; }
        public int id_category_heading { get; set; }
        public string Heading_title { get; set; }
        public string CategoryImage { get; set; }
        public List<tbl_third_party_app_right_swipe_mapping> third_party_app { get; set; }



    }

    public class tbl_third_party_app_right_swipe_mapping
    {
        public int id_third_party { get; set; }
        public int id_mapping { get; set; }
        public int third_party_type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string logo { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }


    }

    public class tbl_third_party_right_swipe_m2ost
    {
        public int id_swipe { get; set; }
        public int id_brief { get; set; }
        public int id_category { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public int type { get; set; }
        public string URL { get; set; }
        public DateTime updated_date_time { get; set; }
        public string CATEGORYNAME { get; set; }
        public int id_category_heading { get; set; }
        public string Heading_title { get; set; }
        public string CategoryImage { get; set; }



    }



    public class Cat_Mapping_Data
    {
        public int id_cat { get; set; }
        public int id_cat_heading { get; set; }
        public string URL { get; set; }
    }



    public class BriefCountResponse
    {
        public int ReadCount { get; set; }
        public int UnReadCount { get; set; }
        public int TOTALCOUNT { get; set; }

    }
    public class collegelistdetails
    {
        public int id_college { get; set; }
        public string college_name { get; set; }

    }

    public class tbl_brief_tile_level_brief_restriction
    {
        public int id_restriction { get; set; }
        public int id_brief_tile { get; set; }
        public int OID { get; set; }
        public int brief_count { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int time { get; set; }
        public int id_academy { get; set; }

    }

    public class tbl_degree_master
    {
        public int id_degree { get; set; }
        public string degree { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class tbl_stream_master
    {
        public int id_stream { get; set; }
        public string stream { get; set; }
        public int id_master { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class ReferralResponse
    {
        public int is_exist { get; set; }
        public string referral_name { get; set; }

    }

    public class tbl_academy_level_brief_restriction
    {
        public int id_restriction { get; set; }
        public int id_academy { get; set; }
        public int OID { get; set; }
        public int brief_count { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int time { get; set; }

    }


    public class AuthenticateBrief
    {
        public string AuthFlag { get; set; }
        public string Message { get; set; }
    }

    public class tbl_restriction_user_log
    {
        public int id_log { get; set; }
        public int UID { get; set; }
        public int OID { get; set; }
        public int id_brief_master { get; set; }
        public int id_academy { get; set; }
        public DateTime updated_date_time { get; set; }
        public string status { get; set; }
        public int id_brief_tile { get; set; }

    }




    public class BriefBody
    {
        public APIBrief BRIEF { get; set; }
        public List<QuestionList> QTNLIST { get; set; }
        public BriefReturnResponse RESULT { get; set; }
        public int RESULTSTATUS { get; set; }
        public double RESULTSCORE { get; set; }
    }

    public class BriefChart
    {
        public string Label { get; set; }
        public double value { get; set; }
        public int complexity { get; set; }
    }

    public class ResumePost
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public string resumeBase { get; set; }
        public string type { get; set; }

    }

    public class tbl_cv_master
    {
        public int id_cv { get; set; }
        public int id_user { get; set; }
        public int oid { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }

        public string status { get; set; }

        public int cv_type { get; set; }


    }

    public class tbl_video_cv
    {
        public int id_video { get; set; }
        public int id_user { get; set; }
        public string videoname { get; set; }
        public string extn { get; set; }
        public string status { get; set; }



    }


    public class tbl_cv_personel_info
    {
        public int id_cv_personel_info { get; set; }
        public int id_cv { get; set; }
        public int id_user { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public string country { get; set; }

        public string city { get; set; }

        public string street { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public string about { get; set; }
        public string year { get; set; }


    }


    public class tbl_cv_additional_info
    {
        public int id_cv_additional { get; set; }
        public int id_cv { get; set; }
        public int id_user { get; set; }
        public string skills { get; set; }
        public string languages { get; set; }
        public string intrests { get; set; }
        public string linkedin { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string blog { get; set; }
        public string others { get; set; }
        public string refrences { get; set; }
        public string awards { get; set; }

    }

    public class tbl_cv_education
    {
        public int id_cv_additional { get; set; }
        public int id_cv { get; set; }
        public int id_user { get; set; }
        public string college { get; set; }
        public string degree { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string summary { get; set; }

    }

    public class tbl_cv_project
    {
        public int id_cv_project { get; set; }
        public int id_cv { get; set; }
        public int id_user { get; set; }
        public string college { get; set; }
        public string project_title { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string summary { get; set; }

    }

    public class CVBuilderCreation
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public int data_flag { get; set; }
        public int id_cv { get; set; }
        public tbl_cv_personel_info personel { get; set; }
        public List<tbl_cv_education> education { get; set; }
        public tbl_cv_additional_info additional_info { get; set; }
        public List<tbl_cv_project> project_list { get; set; }

    }

    public class FileTest
    {
        public HttpPostedFile file { get; set; }

    }
    public class APITEST
    {
        public string file { get; set; }

    }

    public class VideoCVBuilder
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public string VideoBase { get; set; }
        public string EXTN { get; set; }
        public byte[] VideoString { get; set; }



    }


    public class PostResumeResponse
    {
        public string STATUS { get; set; }
        public string RESUMELINK { get; set; }



    }



    public class CVBuilderResponse
    {
        public string STATUS { get; set; }
        public string RESUMELINK { get; set; }
    }

    public class DPUpdateResponse
    {
        public string STATUS { get; set; }
        public string DPLink { get; set; }
    }

    public class TagPhotoUploadResponse
    {
        public string STATUS { get; set; }
        public string Taglink { get; set; }
    }

    public class CreateResumeDetails
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePictureEXTN { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string DOB_Date { get; set; }
        public string DOB { get; set; }

        public string DOB_Month { get; set; }
        public string DOB_Year { get; set; }
        public string AboutYourself { get; set; }
        public List<CVCollegeDetails> College { get; set; }
        public List<CVProjectDetails> Project { get; set; }
        public string Skills { get; set; }
        public string Languages { get; set; }
        public string Interest { get; set; }
        public string LinkedIn { get; set; }
        public string FaceBook { get; set; }
        public string Twitter { get; set; }
        public string PersonalBlog { get; set; }
        public string Other { get; set; }
        public string References { get; set; }
        public string Awards { get; set; }
        public int cv_type { get; set; }
        public tbl_cv_personel_info personel { get; set; }
        public List<tbl_cv_education> education { get; set; }
        public tbl_cv_additional_info additional_info { get; set; }
        public List<tbl_cv_project> project_list { get; set; }
        public int data_flag { get; set; }



    }
    public class CVCollegeDetails
    {
        public string College { get; set; }
        public string Degree { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Summary { get; set; }

    }
    public class CVProjectDetails
    {
        public string College { get; set; }
        public string ProjectTitle { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Summary { get; set; }

    }
    public class ExtraCertificatePost
    {
        public int UID { get; set; }
        public string CertificateBase { get; set; }
        public string type { get; set; }

    }


    public class FeedbackPost
    {

        public int id_feedback { get; set; }
        public int Issues { get; set; }
        public int Suggestions { get; set; }
        public int Content { get; set; }
        public int UI { get; set; }
        public string Description { get; set; }
        public int MediaFlag { get; set; }
        public DateTime updated_date_time { get; set; }
        public string Contact { get; set; }
        public int UID { get; set; }
        public int OID { get; set; }
        public List<FeedbackMedia> Media { get; set; }
    }


    public class FeedbackResponse

    {
        public string Result { get; set; }
    }

    public class FeedbackMedia
    {
        public int id_media { get; set; }
        public int id_feedback { get; set; }
        public string media { get; set; }
        public string extension { get; set; }
        public DateTime updated_time { get; set; }
    }



    public static class APIString
    {
        public static string API = ConfigurationManager.AppSettings["api_full"].ToString();
        public static string RAW = ConfigurationManager.AppSettings["api_raw"].ToString();

    }


    public class BriefResource
    {
        public APIBrief BRIEF { get; set; }
        public List<QuestionList> QTNLIST { get; set; }
        public BriefReturnResponse RESULT { get; set; }
        public int RESULTSTATUS { get; set; }
        public double RESULTSCORE { get; set; }
        public string brief_template { get; set; }
        public List<BriefRow> briefResource { get; set; }
        public double GameScore { get; set; }
        public double SplScore { get; set; }

    }

    public class BriefRequest
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public int BID { get; set; }
        public string BRF { get; set; }
        public string ASRQ { get; set; }
    }

    public class BriefRow
    {
        public string resource_order { get; set; }
        public string brief_destination { get; set; }
        public string resource_number { get; set; }
        public int srno { get; set; }
        public int id_brief_master { get; set; }
        public int resource_type { get; set; }
        public string resouce_data { get; set; }
        public string resouce_code { get; set; }
        public int media_type { get; set; }
        public string resource_mime { get; set; }
        public string file_extension { get; set; }
        public string file_type { get; set; }
    }

    public class tbl_sul_fest_master
    {
        public int id_event { get; set; }
        public string event_title { get; set; }
        public string event_objective { get; set; }
        public string event_logo { get; set; }
        public int is_registration_needed { get; set; }
        public DateTime registration_start_date { get; set; }
        public DateTime registration_end_date { get; set; }
        public DateTime event_start_date { get; set; }
        public DateTime event_end_date { get; set; }
        public string event_duration { get; set; }
        public string location_text { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public int is_event_closed { get; set; }
        public int user_count { get; set; }
        public int is_college_restricted { get; set; }
        public int id_college { get; set; }
        public int is_paid_event { get; set; }
        public string amount { get; set; }
        public int is_org_specified { get; set; }
        public int id_org { get; set; }
        public int is_sponsor_available { get; set; }
        public int id_sponsor { get; set; }
        public string sponsor_logo { get; set; }
        public string event_status { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public List<tbl_event_type_mapping> event_type { get; set; }
        public List<tbl_sub_event_type_mapping> sub_event_type { get; set; }
        public string sponsor { get; set; }
        public int register_status { get; set; }
        public string college_name { get; set; }
        public string state_name { get; set; }
        public string city_name { get; set; }
        public int registration_date_status { get; set; }
        public int registration_count_exceed_status { get; set; }
        public int registration_user_status { get; set; }
        public string contact_name { get; set; }
        public string contact_number { get; set; }
        public List<tbl_sul_seminar_master> seminar { get; set; }
        public List<tbl_sul_higher_education_master> highereducation { get; set; }
    }

    public class tbl_event_type_master
    {
        public int id_event_type { get; set; }
        public string event_type { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }

    }

    public class JobAssessments
    {
        public int id_ce_career_evaluation_master { get; set; }
        public string career_evaluation_code { get; set; }
        public string career_evaluation_title { get; set; }
        public List<AssessmentCategory> Cat { get; set; }
        public string ce_description { get; set; }








    }
    public class AssessmentCategory
    {
        public string brief_category { get; set; }
        public int id_brief_category { get; set; }
    }


    public class tbl_event_type_mapping
    {
        public int id_type_mapping { get; set; }
        public int id_event_type { get; set; }
        public int id_event { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string event_type { get; set; }

    }

    public class tbl_sub_event_type_master
    {
        public int id_sub_event_type { get; set; }
        public string sub_event_type { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }

    }

    public class tbl_sub_event_type_mapping
    {
        public int id_type_mapping { get; set; }
        public int id_sub_event_type { get; set; }
        public int id_event { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string sub_event_type { get; set; }

    }

    public class tbl_event_organisation_master
    {
        public int id_org { get; set; }
        public string organisation { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class tbl_event_sponsor_master
    {
        public int id_sponsor { get; set; }
        public string sponsor { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }


    public class BriefModel
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        private MySqlConnection connection = null;

        public BriefModel()
        {
            string con = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
            this.connection = new MySqlConnection(con);
        }

        public List<APIBrief> getAPIBriefList(string sql)
        {
            List<APIBrief> log = new List<APIBrief>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    APIBrief temp = new APIBrief();
                    temp.id_user = Convert.ToInt32(reader["id_user"]);
                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.datetimestamp = Convert.ToDateTime(reader["datetimestamp"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.is_question_attached = Convert.ToInt32(reader["is_question_attached"]);
                    temp.action_status = Convert.ToInt32(reader["action_status"]);
                    temp.read_status = Convert.ToInt32(reader["read_status"]);
                    temp.brief_category = Convert.ToString(reader["brief_category"]);
                    temp.brief_subcategory = Convert.ToString(reader["brief_subcategory"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }
        public List<APIBrief> getAPIBriefForAssessmnet(string sql, int uid)
        {
            List<APIBrief> log = new List<APIBrief>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    APIBrief temp = new APIBrief();
                    temp.id_user = uid;// Convert.ToInt32(reader["id_user"]);
                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.datetimestamp = DateTime.Now; //Convert.ToDateTime(reader["datetimestamp"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.is_question_attached = Convert.ToInt32(reader["is_question_attached"]);
                    temp.action_status = 0;// Convert.ToInt32(reader["action_status"]);
                    temp.read_status = 0; //Convert.ToInt32(reader["read_status"]);
                    temp.brief_category = Convert.ToString(reader["brief_category"]);
                    temp.brief_subcategory = Convert.ToString(reader["brief_subcategory"]);
                    //temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    //temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }
        public List<BriefAPIResource> getBriefAPIResourceListCus(string sql, List<BriefAPIResource> list)
        {
            List<BriefAPIResource> log = new List<BriefAPIResource>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefAPIResource temp = new BriefAPIResource();
                    temp.id_user = Convert.ToInt32(reader["id_user"]);
                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.datetimestamp = Convert.ToDateTime(reader["datetimestamp"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.is_question_attached = Convert.ToInt32(reader["is_question_attached"]);
                    temp.action_status = Convert.ToInt32(reader["action_status"]);
                    temp.read_status = Convert.ToInt32(reader["read_status"]);
                    temp.brief_category = Convert.ToString(reader["brief_category"]);
                    temp.brief_subcategory = Convert.ToString(reader["brief_subcategory"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.brief_template = "0";

                    list.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return list;
        }

        public List<BriefAPIResource> getBriefAPIResourceList(string sql)
        {
            List<BriefAPIResource> log = new List<BriefAPIResource>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefAPIResource temp = new BriefAPIResource();
                    temp.id_user = Convert.ToInt32(reader["id_user"]);
                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.datetimestamp = Convert.ToDateTime(reader["datetimestamp"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.is_question_attached = Convert.ToInt32(reader["is_question_attached"]);
                    temp.action_status = Convert.ToInt32(reader["action_status"]);
                    temp.read_status = Convert.ToInt32(reader["read_status"]);
                    temp.brief_category = Convert.ToString(reader["brief_category"]);
                    temp.brief_subcategory = Convert.ToString(reader["brief_subcategory"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.brief_template = "0";
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        int id_brief_status = db.Database.SqlQuery<int>("select id_brief_status from tbl_brief_status where id_brief_master={0} and brief_status={1} ", temp.id_brief_master, "Published").FirstOrDefault();
                        if (id_brief_status > 0)
                        {
                            log.Add(temp);


                        }

                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<tbl_brief_master> getBriefList(string sql, int uid)
        {
            List<tbl_brief_master> log = new List<tbl_brief_master>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tbl_brief_master temp = new tbl_brief_master();
                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    //temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.updated_date_time = Convert.ToDateTime(reader["updated_date_time"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.brief_attachment_flag = Convert.ToInt32(reader["brief_attachment_flag"]);
                    //temp.id_brief_sub_category = Convert.ToInt32(reader["id_brief_subcategory"]);
                    if (temp.brief_attachment_flag == 4)
                    {
                        temp.brief_attachement_url = reader["brief_attachement_url"].ToString();

                    }
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        int id_brief_status = db.Database.SqlQuery<int>("select id_brief_status from tbl_brief_status where id_brief_master={0} and brief_status={1} ", temp.id_brief_master, "Published").FirstOrDefault();
                        if (id_brief_status > 0)
                        {
                            log.Add(temp);


                        }

                    }


                    //log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<BriefAPIResource> getBriefListWithAcademy(string sql, int UID)
        {
            List<BriefAPIResource> log = new List<BriefAPIResource>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefAPIResource temp = new BriefAPIResource();

                    temp.question_count = Convert.ToInt32(reader["question_count"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.id_organization = Convert.ToInt32(reader["id_organization"]);
                    temp.override_dnd = Convert.ToInt32(reader["override_dnd"]);
                    temp.id_user = UID;
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_description = Convert.ToString(reader["brief_description"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.BrfDate = Convert.ToDateTime(reader["updated_date_time"]);
                    temp.id_brief_category = Convert.ToInt32(reader["id_brief_category"]);
                    temp.brief_attachment_flag = Convert.ToInt32(reader["brief_attachment_flag"]);
                    if (temp.brief_attachment_flag == 4)
                    {
                        temp.brief_attachement_url = reader["brief_attachement_url"].ToString();
                    }
                    double? briefLogResult = Convert.ToDouble(string.IsNullOrEmpty(reader["briefLogResult"]?.ToString()) ? 0 : reader["briefLogResult"]);
                    if (briefLogResult.HasValue && briefLogResult.Value != 0)
                    {
                        temp.RESULTSTATUS = 1;
                        temp.RESULTSCORE = Convert.ToDouble(briefLogResult.Value);
                    }
                    else
                    {
                        temp.RESULTSTATUS = 0;
                        temp.RESULTSCORE = 0;
                    }
                    temp.liked = Convert.ToInt32(string.IsNullOrEmpty(reader["liked"]?.ToString()) ? 0 : reader["liked"]);
                    temp.disliked = Convert.ToInt32(string.IsNullOrEmpty(reader["disliked"]?.ToString()) ? 0 : reader["disliked"]);
                    temp.brief_template = string.IsNullOrEmpty(reader["brief_template"]?.ToString()) ? "0" : reader["brief_template"].ToString();
                    
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }
        public List<TRANSUSER> getAPITUserList(string sql, int type)
        {
            List<TRANSUSER> log = new List<TRANSUSER>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                string rmtype = "NA";
                if (type == 4)
                {
                    rmtype = "RM";
                }
                if (type == 6)
                {
                    rmtype = "EM";
                }

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TRANSUSER temp = new TRANSUSER();
                    temp.USERTYPE = rmtype;
                    temp.ID_ROLE = type;

                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    temp.ID_USER = Convert.ToInt32(reader["ID_USER"]);
                    temp.USERID = Convert.ToString(reader["USERID"]);
                    temp.EMAIL = Convert.ToString(reader["EMAIL"]);
                    temp.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
                    temp.PASSWORD = Convert.ToString(reader["PASSWORD"]);
                    temp.EMPLOYEEID = Convert.ToString(reader["EMPLOYEEID"]);
                    temp.user_department = Convert.ToString(reader["user_department"]);
                    temp.user_designation = Convert.ToString(reader["user_designation"]);
                    temp.user_function = Convert.ToString(reader["user_function"]);
                    temp.user_grade = Convert.ToString(reader["user_grade"]);
                    temp.reporting_manager = Convert.ToString(reader["reporting_manager"]);
                    temp.process_status = 0;
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        /*  ---------------------------------------------- */

        public List<briefView> getBriefView(string sql)
        {
            List<briefView> log = new List<briefView>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    briefView temp = new briefView(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<BriefUser> getBriefUserList(string sql)
        {
            List<BriefUser> log = new List<BriefUser>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefUser temp = new BriefUser(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public int getAttamptNo(string sql)
        {
            int ret = 0;
            try
            {
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
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

        public double getQuestionCounter(string sql)
        {
            double ret = 0;
            try
            {
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = Convert.ToDouble(reader["counter"]);
                        ret = Math.Round(ret, 2);
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

        public List<TestBrief> getTestBriefUserList(string sql)
        {
            List<TestBrief> log = new List<TestBrief>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TestBrief temp = new TestBrief(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<B2COrg> getOrganizationList(string sql)
        {
            List<B2COrg> log = new List<B2COrg>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    B2COrg temp = new B2COrg();
                    temp.OID = Convert.ToInt32(reader["ID_ORGANIZATION"]);
                    temp.ORG = reader["ORGANIZATION_NAME"].ToString();
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<BriefCollection> getUserTestResult(string sql)
        {
            List<BriefCollection> log = new List<BriefCollection>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefCollection temp = new BriefCollection();
                    temp.attempt_no = Convert.ToInt32(reader["attempt_no"]);
                    temp.id_user = Convert.ToInt32(reader["id_user"]);
                    temp.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
                    temp.brief_result = Convert.ToDouble(reader["brief_result"]);
                    temp.brief_code = Convert.ToString(reader["brief_code"]);
                    temp.brief_title = Convert.ToString(reader["brief_title"]);
                    temp.FIRSTNAME = Convert.ToString(reader["FIRSTNAME"]);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public List<BriefResultSummery> getBriefResultSummery(string sql)
        {
            List<BriefResultSummery> log = new List<BriefResultSummery>();

            try
            {
                MySqlCommand command = null;
                string query = sql;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BriefResultSummery temp = new BriefResultSummery(reader);
                    log.Add(temp);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { connection.Close(); }
            return log;
        }

        public ComplexityResult getComplexityResult(string sql)
        {
            ComplexityResult ret = new ComplexityResult();
            try
            {
                MySqlCommand command = null;
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = sql;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret.RIGHTCOUNT = Convert.ToInt32(reader["rightcount"]);
                        ret.TOTALCOUNT = Convert.ToInt32(reader["totalcount"]);
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

        public BriefResource getBriefData(string brf, int UID, int OID)
        {
            List<APIBrief> list = new List<APIBrief>();
            BriefResource bBody = new BriefResource();

            brf = brf.ToLower().Trim();
            int totalQTN = 0;
            int remQtn = 0;
            //string sqlb = "SELECT a.id_organization,a.question_count,a.id_brief_category,a.id_brief_sub_category, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
            //sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE   LOWER(brief_code) = '" + brf + "' AND a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) LIMIT 10 ";

            //sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
            //sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE  LOWER(brief_code) = '" + brf + "' AND a.status='A' AND  a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp DESC LIMIT 50";
            string sqlb = "SELECT a.id_organization, question_count, brief_title, brief_code, brief_description, a.override_dnd, a.id_brief_master, a.is_add_question is_question_attached, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory FROM tbl_brief_master a, tbl_brief_category d, tbl_brief_subcategory e WHERE LOWER(brief_code) = '" + brf + "' AND a.status = 'A' AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_organization = " + OID + " ORDER BY a.updated_date_time DESC LIMIT 50";
            list = new BriefModel().getAPIBriefForAssessmnet(sqlb, UID);

            if (list.Count > 0)
            {
                bBody = new BriefResource();
                APIBrief brief = list[0];
                tbl_brief_master master = db.tbl_brief_master.Where(t => t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                tbl_brief_master_template mTemplate = db.tbl_brief_master_template.Where(t => t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (mTemplate != null)
                {
                    bBody.brief_template = mTemplate.brief_template;
                }
                else
                {
                    bBody.brief_template = "0";
                }
                totalQTN = brief.question_count;
                bBody.BRIEF = brief;
                List<tbl_brief_master_body> mbody = db.tbl_brief_master_body.Where(t => t.id_brief_master == brief.id_brief_master).OrderBy(t => t.srno).ToList();
                List<BriefRow> bList = new List<BriefRow>();
                foreach (tbl_brief_master_body row in mbody)
                {
                    BriefRow irow = new BriefRow();
                    irow.media_type = Convert.ToInt32(row.media_type);
                    irow.resouce_code = row.resouce_code;
                    irow.resource_order = mTemplate.resource_order;
                    irow.brief_destination = row.brief_destination;
                    irow.resource_number = row.resource_number;
                    irow.srno = Convert.ToInt32(row.srno);
                    irow.resource_type = Convert.ToInt32(row.resource_type);
                    irow.resouce_data = row.resouce_data;
                    irow.resouce_code = row.resouce_code;
                    irow.media_type = Convert.ToInt32(row.media_type);
                    irow.resource_mime = row.resource_mime;
                    irow.file_extension = row.file_extension;
                    irow.file_type = row.file_type;
                    bList.Add(irow);
                }
                bBody.briefResource = bList;
                List<QuestionList> qtnList = new List<QuestionList>();
                List<tbl_brief_question> qList = new List<tbl_brief_question>();

                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == brief.id_brief_master && t.id_user == UID).FirstOrDefault();
                if (log != null)
                {
                    bBody.RESULTSTATUS = 1;
                    bBody.RESULTSCORE = Convert.ToDouble(log.brief_result);
                    BriefReturnResponse response = null;
                    response = JsonConvert.DeserializeObject<BriefReturnResponse>(log.json_response);
                    bBody.RESULT = response;
                    bBody.QTNLIST = null;
                    using (m2ostnextserviceDbContext dbc = new m2ostnextserviceDbContext())
                    {
                        int gameid = 0;
                        bBody.GameScore = 0;
                        gameid = dbc.Database.SqlQuery<int>("select id_game from tbl_game_master where id_theme={0} and status={1}", 9, "A").FirstOrDefault();
                        if (gameid != 0)
                        {
                            tbl_user_game_score_log scorelg = dbc.Database.SqlQuery<tbl_user_game_score_log>("select * from tbl_user_game_score_log where id_user={0} and id_game={1} and id_brief={2} and status={3}", UID, gameid, brief.id_brief_master, "A").FirstOrDefault();
                            if (scorelg != null)
                            {
                                bBody.GameScore = scorelg.score;
                            }

                        }

                    }


                }
                else
                {

                    //------------------------Question theme Logic-------24-01-19-------------------------------
                    bBody.RESULTSTATUS = 0;
                    bBody.RESULTSCORE = 0;
                    bBody.RESULT = null;
                    List<int> checkList = new List<int>();

                    string bsql = "SELECT * FROM tbl_brief_question where id_organization=" + OID + " and id_brief_question in (select id_brief_question from  tbl_brief_question_mapping where id_brief_master=" + brief.id_brief_master + " and status='A') and status='A'";
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        qList = db.Database.SqlQuery<tbl_brief_question>(bsql).ToList();

                    }

                    foreach (tbl_brief_question item in qList)
                    {
                        QuestionList temp = new QuestionList();
                        tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == item.question_complexity).FirstOrDefault();
                        if (comp != null)
                        {
                            temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                            temp.question_complexity_label = comp.question_complexity_label;
                        }
                        temp.question = item;
                        string sqlans = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                        List<tbl_brief_answer> answer = new List<tbl_brief_answer>();

                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            answer = db.Database.SqlQuery<tbl_brief_answer>(sqlans).ToList();
                        }
                        temp.answers = answer;
                        qtnList.Add(temp);
                        checkList.Add(item.id_brief_question);
                    }
                    remQtn = totalQTN - qList.Count();

                    if (remQtn > 0)
                    {
                        /*category distribution process*/
                        int cattype = Convert.ToInt32(master.brief_type);
                        List<tbl_brief_category> catList = new List<tbl_brief_category>();
                        if (cattype == 0)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category = " + master.id_brief_category + " ";
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }
                        if (cattype == 2)
                        {
                            List<tbl_brief_category_mapping> map = new List<tbl_brief_category_mapping>();
                            map = db.tbl_brief_category_mapping.Where(t => t.id_brief_master == master.id_brief_master).ToList();
                            if (map.Count > 0)
                            {
                                string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_category_mapping where  id_organization=" + OID + " and id_brief_master=" + master.id_brief_master + ") limit " + remQtn;
                                catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                            }
                        }
                        if (cattype == 3)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_question where id_organization=" + OID + ") limit " + remQtn;
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }
                        if (cattype == 1)
                        {
                            string qsql = "select * from tbl_brief_category where status='A' and id_organization=" + OID + "  and id_brief_category in (SELECT distinct id_brief_category FROM tbl_brief_question where id_organization=" + OID + ") limit " + remQtn;
                            catList = db.tbl_brief_category.SqlQuery(qsql).ToList();
                        }

                        int flag = remQtn;
                        List<tbl_brief_question> tempList = new List<tbl_brief_question>();

                        int k = catList.Count();
                        int kl = k * 20;
                        int j = 0;
                        // for (int j = 0; j < kl; j++)
                        do
                        {
                            //  if (flag == 0) { break; }
                            int index = j % k;
                            tbl_brief_category item = catList[index];
                            tbl_brief_question temp = getProgressiveDistributionQuestion(UID, item.id_brief_category, OID);
                            if (temp != null)
                            {
                                if (checkList.Contains(temp.id_brief_question))
                                {
                                }
                                else
                                {
                                    tbl_brief_question tq = tempList.Where(t => t.id_brief_question == temp.id_brief_question).FirstOrDefault();
                                    if (tq == null)
                                    {
                                        tempList.Add(temp);
                                        flag = flag - 1;
                                    }
                                }
                            }
                            if (j > 150)
                            {
                                break;
                            }
                            j++;
                        } while (tempList.Count != remQtn);
                        foreach (tbl_brief_question item in tempList)
                        {
                            tbl_brief_progdist_mapping mapping = new tbl_brief_progdist_mapping();
                            mapping.id_brief_master = brief.id_brief_master;
                            mapping.id_brief_question = item.id_brief_question;
                            mapping.id_user = UID;
                            mapping.date_time_stamp = System.DateTime.Now;
                            mapping.question_link_type = 1;
                            mapping.status = "A";
                            mapping.updated_date_time = DateTime.Now;
                            db.tbl_brief_progdist_mapping.Add(mapping);
                            db.SaveChanges();

                            QuestionList temp = new QuestionList();
                            tbl_brief_question_complexity comp = db.tbl_brief_question_complexity.Where(t => t.question_complexity == item.question_complexity).FirstOrDefault();
                            if (comp != null)
                            {
                                temp.question_complexity = Convert.ToInt32(comp.question_complexity);
                                temp.question_complexity_label = comp.question_complexity_label;
                            }
                            temp.question = item;
                            string sqlans = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                            //List<tbl_brief_answer> answer = db.tbl_brief_answer.SqlQuery(sqlans).ToList();
                            List<tbl_brief_answer> answer = new List<tbl_brief_answer>();

                            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                            {
                                answer = db.Database.SqlQuery<tbl_brief_answer>(sqlans).ToList();
                            }


                            temp.answers = answer;
                            qtnList.Add(temp);
                        }
                    }
                    bBody.QTNLIST = qtnList;
                }

                //tbl_brief_read_status rstatus = db.tbl_brief_read_status.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                //if (rstatus != null)
                //{
                //    if (rstatus.read_status == 1)
                //    {
                //        rstatus.read_status = 1;
                //        rstatus.read_datetime = DateTime.Now;
                //        rstatus.updated_date_time = DateTime.Now;
                //        db.SaveChanges();
                //    }
                //}
                //else
                //{
                //    rstatus = new tbl_brief_read_status();
                //    rstatus.id_user = UID;
                //    rstatus.id_brief_master = brief.id_brief_master;
                //    rstatus.read_status = 1;
                //    rstatus.status = "A";
                //    rstatus.action_dateime = null;
                //    rstatus.action_status = 0;
                //    rstatus.read_datetime = DateTime.Now;
                //    rstatus.updated_date_time = DateTime.Now;
                //    db.SaveChanges();
                //}

                tbl_brief_user_assignment urst = db.tbl_brief_user_assignment.Where(t => t.id_user == UID && t.id_brief_master == brief.id_brief_master).FirstOrDefault();
                if (urst != null)
                {
                    if (urst.scheduled_status == "NA" && urst.published_status == "S")
                    {
                        urst.published_status = "R";
                        urst.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }

                    if (urst.published_status == "NA" && urst.scheduled_status == "S")
                    {
                        urst.scheduled_status = "R";
                        urst.updated_date_time = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                else
                {
                }
            }

            //if (bBody != null)
            //{
            return bBody;
            //return Request.CreateResponse(HttpStatusCode.OK, bBody);
            //}
            //else
            //{

            //    //return Request.CreateResponse(HttpStatusCode.NoContent, bBody);
            //}
        }

        public tbl_brief_question getProgressiveDistributionQuestion(int UID, int CID, int OID)
        {
            tbl_brief_audit lstQtn = new tbl_brief_audit();
            string qsql = "SELECT * FROM tbl_brief_audit WHERE  id_user = " + UID + " AND id_brief_question IN (SELECT id_brief_question FROM tbl_brief_question WHERE  id_organization=" + OID + " and id_brief_category = " + CID + ") ORDER BY id_brief_audit DESC LIMIT 1";
            lstQtn = db.tbl_brief_audit.SqlQuery(qsql).FirstOrDefault();
            bool nextDir = false;

            if (lstQtn != null)
            {

                //tbl_brief_question qtn = db.tbl_brief_question.Where(t => t.id_brief_question == lstQtn.id_brief_question).FirstOrDefault();
                tbl_brief_question qtn = new tbl_brief_question();

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    qtn = db.Database.SqlQuery<tbl_brief_question>("select * from tbl_brief_question where id_brief_question={0} ", lstQtn.id_brief_question).FirstOrDefault();

                }
                if (lstQtn.audit_result == 1)
                {
                    nextDir = true;
                }
                int complexity = getComplecityLevel(CID, nextDir, qtn.question_complexity);
                string newQtnSql = "select * from tbl_brief_question where  id_organization=" + OID + " and  id_brief_question not in (SELECT distinct id_brief_question FROM tbl_brief_audit where id_user =" + UID + " ) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY  RAND() LIMIT 1";
                tbl_brief_question nextQtn = new tbl_brief_question();

                //tbl_brief_question nextQtn = db.tbl_brief_question.SqlQuery(newQtnSql).FirstOrDefault();
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    nextQtn = db.Database.SqlQuery<tbl_brief_question>(newQtnSql).FirstOrDefault();

                }

                if (nextQtn != null)
                {
                    return nextQtn;
                }
                else
                {
                    string subQtnSql = "select * from tbl_brief_question where  id_organization=" + OID + " and id_brief_question in (SELECT distinct id_brief_question FROM tbl_brief_audit where id_user =" + UID + " AND audit_result=0) and question_complexity=" + complexity + " and status='A' and expiry_date>now() ORDER BY RAND() LIMIT 1";
                    //tbl_brief_question subQtn = db.tbl_brief_question.SqlQuery(subQtnSql).FirstOrDefault();
                    tbl_brief_question subQtn = new tbl_brief_question();

                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                    {
                        subQtn = db.Database.SqlQuery<tbl_brief_question>(subQtnSql).FirstOrDefault();

                    }

                    if (subQtn != null)
                    {
                        return subQtn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                string firQtnSql = "SELECT * FROM tbl_brief_question WHERE id_organization=" + OID + " and  id_brief_category =" + CID + " AND status = 'A' AND expiry_date > NOW() ORDER BY question_complexity,RAND()  LIMIT 1";
                //tbl_brief_question firQtn = db.tbl_brief_question.SqlQuery(firQtnSql).FirstOrDefault();
                tbl_brief_question firQtn = new tbl_brief_question();

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    firQtn = db.Database.SqlQuery<tbl_brief_question>(firQtnSql).FirstOrDefault();

                }


                if (firQtn != null)
                {
                    return firQtn;
                }
                else
                {
                    return null;
                }
            }
        }

        public int getComplecityLevel(int CID, bool status, int? level)
        {
            string additional = "";
            if (status)
            {
                additional = "  AND question_complexity > " + level + " order by question_complexity  LIMIT 1 ";
            }
            else
            {
                additional = "  AND question_complexity < " + level + " order by question_complexity desc LIMIT 1 ";
            }
            string sql = "SELECT * FROM tbl_brief_question_complexity WHERE question_complexity IN (SELECT DISTINCT question_complexity FROM tbl_brief_question WHERE id_brief_category = " + CID + ") " + additional;
            tbl_brief_question_complexity levels = db.tbl_brief_question_complexity.SqlQuery(sql).FirstOrDefault();
            if (levels != null)
            {
                return Convert.ToInt32(levels.question_complexity);
            }
            else
            {
                return Convert.ToInt32(level);
            }
        }

    }

    public class briefView
    {
        public int id_brief_master { get; set; }
        public string brief_title { get; set; }
        public int brief_type { get; set; }
        public string brief_code { get; set; }
        public int question_count { get; set; }
        public DateTime scheduled_timestamp { get; set; }
        public string brief_category { get; set; }
        public string brief_subcategory { get; set; }
        public string brief_status { get; set; }
        public int status_code { get; set; }

        public briefView(MySqlDataReader reader)
        {
            this.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
            this.brief_type = Convert.ToInt32(reader["brief_type"]);
            this.question_count = Convert.ToInt32(reader["question_count"]);
            this.status_code = Convert.ToInt32(reader["status_code"]);
            this.brief_title = Convert.ToString(reader["brief_title"]);
            this.brief_code = Convert.ToString(reader["brief_code"]);
            this.brief_category = Convert.ToString(reader["brief_category"]);
            this.brief_subcategory = Convert.ToString(reader["brief_subcategory"]);
            this.brief_status = Convert.ToString(reader["brief_status"]);
            this.scheduled_timestamp = Convert.ToDateTime(reader["scheduled_timestamp"]);
        }
    }

    public class BriefUser
    {
        public int PRUSER { get; set; }
        public string PRUSERID { get; set; }
        public string PRNAME { get; set; }
        public string PRFUNCTION { get; set; }
        public string PRCITY { get; set; }
        public string PRLOCATION { get; set; }
        public string RMUSER { get; set; }
        public string RMUSERID { get; set; }
        public string RMNAME { get; set; }
        public int id_brief_user_assignment { get; set; }
        public int id_brief_master { get; set; }

        public BriefUser(MySqlDataReader reader)
        {
            this.PRUSER = Convert.ToInt32(reader["PRUSER"]);
            this.PRUSERID = Convert.ToString(reader["PRUSERID"]);
            this.PRNAME = Convert.ToString(reader["PRNAME"]);
            this.PRFUNCTION = Convert.ToString(reader["PRFUNCTION"]);
            this.PRCITY = Convert.ToString(reader["PRCITY"]);
            this.PRLOCATION = Convert.ToString(reader["PRLOCATION"]);
            this.RMUSER = Convert.ToString(reader["RMUSER"]);
            this.RMUSERID = Convert.ToString(reader["RMUSERID"]);
            this.RMNAME = Convert.ToString(reader["RMNAME"]);
            this.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
            this.id_brief_user_assignment = Convert.ToInt32(reader["id_brief_user_assignment"]);
        }
    }

    public class TestBrief
    {
        public string brief_title { get; set; }
        public string brief_code { get; set; }
        public int id_brief_master { get; set; }
        public int id_user { get; set; }
        public string firstname { get; set; }

        public TestBrief(MySqlDataReader reader)
        {
            this.id_brief_master = Convert.ToInt32(reader["id_brief_master"]);
            this.id_user = Convert.ToInt32(reader["id_user"]);
            this.brief_title = Convert.ToString(reader["brief_title"]);
            this.firstname = Convert.ToString(reader["firstname"]);
            this.brief_code = Convert.ToString(reader["brief_code"]);
        }
    }

    public class QuestionList
    {
        public int question_complexity { get; set; }
        public string question_complexity_label { get; set; }
        public tbl_brief_question question { get; set; }
        public List<tbl_brief_answer> answers { get; set; }
    }

    public class BriefDataCube
    {
        public int QID { get; set; }
        public int AID { get; set; }
        public string VAL { get; set; }
    }

    public class BriefUserInput
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string WANS { get; set; }
        public int srno { get; set; }
        public int is_right { get; set; }
        public int question_complexity { get; set; }
        public string question_complexity_label { get; set; }
        public Nullable<int> id_question { get; set; }
        public int id_answer { get; set; }
        public int id_wans { get; set; }
        public int questiontheme { get; set; }
        public int questionchoicetype { get; set; }
        public string questionimg { get; set; }
        public int answertheme { get; set; }
        public int answerchoicetype { get; set; }
        public string answerimg { get; set; }
        public int wanstheme { get; set; }
        public int wanschoicetype { get; set; }
        public string wansimg { get; set; }



    }

    public class ComplexityResult
    {
        public int question_complexity { get; set; }
        public string question_complexity_label { get; set; }
        public double RESULT { get; set; }
        public int RIGHTCOUNT { get; set; }
        public int TOTALCOUNT { get; set; }
    }

    public class BriefReturnResponse
    {
        public List<BriefUserInput> briefReturn { get; set; }
        public string returnStat { get; set; }
        public int rightCount { get; set; }
        public int totalCount { get; set; }
        public int attemptno { get; set; }
        public double percentage { get; set; }
        public List<ComplexityResult> complexity { get; set; }
        public List<AssessmentScoreResponse> AssessmetGameScore { get; set; }

    }

    public class BriefCollection
    {
        public string brief_code { get; set; }
        public string brief_title { get; set; }
        public int attempt_no { get; set; }
        public int id_user { get; set; }
        public int id_brief_master { get; set; }
        public string FIRSTNAME { get; set; }
        public double brief_result { get; set; }
    }

    public class BriefResultSummery
    {
        public double brief_result { get; set; }
        public string prname { get; set; }
        public string rmname { get; set; }
        public DateTime completedtime { get; set; }
        public int attempt_no { get; set; }
        public int id_user { get; set; }

        public BriefResultSummery(MySqlDataReader reader)
        {
            this.id_user = Convert.ToInt32(reader["id_user"]);
            this.attempt_no = Convert.ToInt32(reader["attempt_no"]);
            this.brief_result = Convert.ToDouble(reader["brief_result"]);
            this.prname = Convert.ToString(reader["prname"]);
            this.rmname = Convert.ToString(reader["rmname"]);
            this.completedtime = Convert.ToDateTime(reader["completedtime"].ToString());
        }
    }

    public partial class skill_lab_event
    {
        public int id_scheduled_event { get; set; }
        public Nullable<int> id_organization { get; set; }
        public Nullable<int> id_event_creator { get; set; }
        public string event_title { get; set; }
        public string event_description { get; set; }

        public Nullable<System.DateTime> event_start_datetime { get; set; }

        public string facilitator_name { get; set; }

        public string program_image { get; set; }

        public string program_venue { get; set; }
        public string program_location { get; set; }

        public string event_additional_info { get; set; }
        public string event_comment { get; set; }
        public string participant_level { get; set; }

        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
        public List<EventBatch> BatchList { get; set; }
        public string batch { get; set; }
        public int id_batch { get; set; }
    }

    public class EventBatch
    {
        public int id_event_batch { get; set; }
        public int id_event { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime update_date { get; set; }
        public string batch_time { get; set; }
        public int participants { get; set; }
        public int available_seats { get; set; }
    }

    public class tbl_user_event_mapping
    {
        public int id_user_mapping { get; set; }
        public int id_user { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_event { get; set; }
        public int id_batch { get; set; }
    }

    public partial class tbl_web_version_control
    {
        public int id_version_control { get; set; }
        public string version_number { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }

    public class UserBadgeObj
    {
        public int id_user { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int id_badge { get; set; }
        public int badge_won { get; set; }
        public int id_game { get; set; }

    }

    //sridhar wrote a model for job preferrance status
    public class jobPreferranceStatus
    {
        public string status { get; set; }
    }

    public class tbl_brief_user_feedback_master
    {
        public int id_feedback { get; set; }
        public int UID { get; set; }
        public int OID { get; set; }
        public int liked { get; set; }
        public int disliked { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int feedback_type { get; set; }
        public int id_brief_master { get; set; }
        public int MediaFlag { get; set; }
        public List<FeedbackMedia> Media { get; set; }
    }


    public class SULResponse
    {
        public string status { get; set; }
        public string response { get; set; }

    }
    public class EventsHeader
    {
        public int paid { get; set; }
        public int free { get; set; }
    }

}