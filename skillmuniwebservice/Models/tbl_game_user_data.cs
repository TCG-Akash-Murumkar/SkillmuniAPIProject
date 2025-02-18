using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class tbl_game_user_data
    {

        public int id_user_data { get; set; }
        public int id_org { get; set; }
        public int id_game { get; set; }
        public int id_user { get; set; }
        public double kpi_value { get; set; }
        public string status { get; set; }
        public DateTime updated_datetime { get; set; }


    }
}

