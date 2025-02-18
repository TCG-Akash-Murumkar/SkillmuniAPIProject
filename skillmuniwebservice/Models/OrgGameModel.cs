using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class OrgGameModel
    {
    }
    public class tbl_org_game_master
    {
        public int id_org_game { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime game_start_date_time { get; set; }
        public DateTime game_end_date_time { get; set; }
        public GameUserLog ScoreLog { get; set; }



    }
    public class ScoreDataPost
    {
       // public int UID { get; set; }
       // public int OID { get; set; }
        public tbl_org_game_user_log log { get; set; }
        //public int 
    }
    public class AvatarData
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public int avatar_type { get; set; }
     
    }

    public class ScoreLOgicResponse
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }

        //public int 
    }

    public class tbl_org_game_level
    {
        public int id_level { get; set; }
        public int level_sequence { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_org { get; set; }

    }
    public class tbl_org_game_level_mapping
    {
        public int id_mapping { get; set; }
        public int id_org_game { get; set; }
        public int id_level { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
       
    }
    public class level_reponseResult
    {
        public int is_live_game { get; set; }

        public List<level_reponse> level { get; set; }


    }

    public class level_reponse
    {
        public int id_level { get; set; }
        public int level_sequence { get; set; }
        public string title { get; set; }
        public string description { get; set; }


    }
    public class tbl_org_game_content
    {
        public int id_game_content { get; set; }
        public int content_type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int id_brief_master { get; set; }
        public int id_level { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int content_sequence { get; set; }
        public List<tbl_org_game_user_log> user_log { get; set; }
        public tbl_org_game_badge_master badge_log { get; set; }


    }
    public class tbl_org_game_user_log
    {
        public int id_log { get; set; }
        public int id_user { get; set; }
        public int id_game_content { get; set; }
        public int score { get; set; }
        public int id_score_unit { get; set; }
        public int score_type { get; set; }
        public string score_unit { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_level { get; set; }
        public int id_org_game { get; set; }
        public int attempt_no { get; set; }

        
        public string timetaken_to_complete { get; set; }
        public int is_completed { get; set; }
        public int UID { get; set; }
        public int OID { get;set; }

    }
    public class tbl_org_game_badge_master
    {
        public int id_badge { get; set; }
        public string badge_name { get; set; }
        public string badge_description { get; set; }
        public int id_org { get; set; }
        public string badge_image { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int is_achieved { get; set; }
        public int badge_count { get; set; }


    }
    public class tbl_org_game_badge_level_mapping
    {
        public int id_mapping { get; set; }
        public int id_badge { get; set; }
        public int id_level { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_org_game { get; set; }

    }

    public class tbl_org_game_content_badge_mapping
    {
        public int id_mapping { get; set; }
        public int id_game { get; set; }
        public int id_content { get; set; }
        public int id_level { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
      
    }
    public class tbl_org_game_badge_user_log
    {
        public int id_log { get; set; }
        public int id_badge { get; set; }
        public int id_game { get; set; }
        public int id_level { get; set; }
        public int id_content { get; set; }
        public int id_user { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }

    }

    public class LevelUserLogResponse
    {
        public int UID { get; set; }
        public int OID { get; set; }
        public int id_game { get; set; }
        public int id_level { get; set; }
        public int is_level_completed { get; set; }

        public List<tbl_org_game_content> content  {get;set;}
        public tbl_org_game_badge_master level_badge_log { get; set; }



    }

    public class OrgGameUserDashboardResult
    {

        public List<LevelUserLogResponse> LevelUserLog { get; set; }
        public int total_score { get; set; }
        public int detucted_score { get; set; }
        public int current_score { get; set; }
        public int OverAllRank { get; set; }
        public int OverAllRankTotal { get; set; }

        public int UnitRank { get; set; }
        public int UnitRankTotal { get; set; }




    }
    public class SendKeyResult
    {
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }       
    }

    public class assessJson
    {
        public string log_string { get; set; }
    }
    public class tbl_org_game_user_assessment_log
    {
        public int id_log { get; set; }
        public int id_org_game { get; set; }
        public int id_org_game_content { get; set; }
        public int attempt_no { get; set; }
        public int id_org_game_level { get; set; }
        public int id_question { get; set; }
        public int id_answer_selected { get; set; }
        public int is_correct { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public int id_user { get; set; }
        
    }

    public class PostBadgeLog
    {
        public int id_content { get; set; }
        public int id_game { get; set; }
        public int id_level { get; set; }
        public int UID { get; set; }
        public int OID { get; set; }
      
    }
    // Model fort he User Tagged Image Lists`
    public class getPhotoListAPI
    {
        public string STATUS { get; set; }
        public List<TaggedPhotoList> usertagphotolist { get; set; }
    }
    public class TaggedPhotoList
    {
        public int id_org { get; set; }
        public int id_user { get; set; }
        public int id_game_content { get; set; }
        public int id_level { get; set; }
        public string photo_filename { get; set; }
        public string status { get; set; }

    }
   
}