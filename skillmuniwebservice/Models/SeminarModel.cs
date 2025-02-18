using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class SeminarModel
    {
    }
    public class tbl_sul_seminar_master
    {
        public int id_seminar { get; set; }
        public string title { get; set; }
        public string objective { get; set; }
        public string stream { get; set; }
        public DateTime seminar_start_time { get; set; }
        public DateTime seminar_end_time { get; set; }
        public string seminar_duration { get; set; }
        public string speaker_name { get; set; }
        public string speaker_organisation { get; set; }
        public string location { get; set; }
        public Nullable<int> user_count { get; set; }
        public string seminar_status { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }
        public Nullable<int> fest_type { get; set; }
        public Nullable<int> time_interval { get; set; }
        public List<tbl_sul_seminar_timeslot_new> slots { get; set; }
        public Nullable<int> is_registered { get; set; }
        public List<tbl_sul_seminar_user_registration> slot_registered { get; set; }
        public List<tbl_sul_seminar_user_registration> semslots { get; set; }





    }

    public class seminar_time_slots
    {
        public string slot { get; set; }


    }
    public class tbl_sul_slot_seminar
    {
        public int slot_id { get; set; }
        public string slot { get; set; }
        public int day { get; set; }
        public int id_seminar { get; set; }
        public string speaker_name { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }


    }

    

    public class tbl_sul_seminar_user_registration
    {
        public int id_register { get; set; }
        public int id_seminar { get; set; }
        public int id_user { get; set; }
        public string ratings { get; set; }
        public string feedback { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }
        public string slot { get; set; }
        public int slot_id { get; set; }
        public DateTime slot_date { get; set; }

        




    }
    public class tbl_sul_seminar_timeslot
    {
        public int id_slot { get; set; }
        public int time_slot_start_time_hour { get; set; }
        public int time_slot_start_time_minute { get; set; }
        public int time_slot_end_time_hour { get; set; }
        public int time_slot_end_time_minute { get; set; }
        public string session_start_time { get; set; }
        public string session_end_time { get; set; }
        public int id_seminar { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }

    }

    public class tbl_sul_seminar_timeslot_new
    {
        public int id_slot { get; set; }
        public int slot_start_time_hour { get; set; }
        public int slot_start_time_minute { get; set; }
        public string session_start { get; set; }
        public int slot_end_time_hour { get; set; }
        public int slot_end_time_minute { get; set; }
        public string session_end { get; set; }
        public int day { get; set; }
        public int serial_no { get; set; }
        public string speaker_name { get; set; }
        public string description { get; set; }
        public int count_restriction { get; set; }
        public int id_seminar { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string slot_start_time { get; set; }
        public string slot_end_time { get; set; }
        public DateTime slot_date { get; set; }

        



    }
    public class tbl_sul_fest_event_mapping
    {
        public int id_mapping { get; set; }
        public int id_event { get; set; }
        public int id_seminar { get; set; }
        public int id_higher_education { get; set; }
        public string status { get; set; }
        public int type { get; set; }
        public DateTime updated_date_time { get; set; }
    }

}
   

