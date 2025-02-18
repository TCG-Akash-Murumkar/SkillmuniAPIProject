using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class HigherEducationModel
    {
    }
    public class tbl_sul_higher_education_master
    {
        public int id_higher_education { get; set; }
        public string message_to_display { get; set; }
        public string redirect_url { get; set; }
        public string event_name { get; set; }
        public DateTime higher_education_start_time { get; set; }
        public DateTime higher_education_end_time { get; set; }
        public int time_interval { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }
        public List<higher_education_time_slots> slots { get; set; }
        public int is_registered { get; set; }
        public string slot_registered { get; set; }

    }
    public class tbl_sul_higher_education_timeslot
    {
        public int id_slot { get; set; }
        public int time_slot_start_time_hour { get; set; }
        public int time_slot_start_time_minute { get; set; }
        public int time_slot_end_time_hour { get; set; }
        public int time_slot_end_time_minute { get; set; }
        public string session_start_time { get; set; }
        public string session_end_time { get; set; }
        public int id_higher_education { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }
    }
    public class tbl_sul_higher_education_user_registration
    {
        public int id_register { get; set; }
        public int id_higher_education { get; set; }
        public int id_user { get; set; }
        public string ratings { get; set; }
        public string feedback { get; set; }
        public string slot { get; set; }
        public string status { get; set; }
        public DateTime update_date_time { get; set; }

    }

    public class higher_education_time_slots
    {
        public string slot { get; set; }


    }
}