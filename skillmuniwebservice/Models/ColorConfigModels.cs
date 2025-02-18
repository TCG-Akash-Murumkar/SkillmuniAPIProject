using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ColorConfigModels
    {

    }
    public class ColorConfig
    {
        public int id_color_config { get; set; }
        public int id_organisation { get; set; }
        public int config_type { get; set; }
        public string grid1_bk_color { get; set; }
        public string grid1_text_color { get; set; }
        public string grid2_bk_color { get; set; }
        public string grid2_text_color { get; set; }
        public string status { get; set; }
        public DateTime created_date_time { get; set; }
        public DateTime updated_date_time { get; set; }            

    }
    public class WelcomeGif
    {
        public int id_welcome_gif { get; set; }
        public int id_org { get; set; }
        public string gif { get; set; }
        public string status { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
      


    }
}