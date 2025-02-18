using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class tbl_game_master
    {
        public int id_game { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int id_kpi { get; set; }
        public int game_type { get; set; }
        public int id_theme { get; set; }
        public int id_metric { get; set; }
        public int id_org { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
        public string theme_logo { get; set; }
        public int relegation_switch { get; set; }
        public int id_special_metric { get; set; }
        public string assignment_flag { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

    }
}