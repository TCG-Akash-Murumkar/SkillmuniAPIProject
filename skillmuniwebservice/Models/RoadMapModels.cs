using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class RoadMapModels
    {

        public class tbl_academic_tiles
        {
            public int id_academic_tile { get; set; }
            public string tile_name { get; set; }
            public string tile_image { get; set; }
            public int tile_position { get; set; }
            public string status { get; set; }
            public DateTime updated_date_time { get; set; }
            public int id_org { get; set; }
            public string tile_description { get; set; }
            public int theme_id { get; set; }
            public string url { get; set; }



        }
        public class tbl_brief_tile_academic_mapping
        {
            public int id_tile_mapping { get; set; }
            public int id_academic_tile { get; set; }
            public int id_journey_tile { get; set; }
            public DateTime updated_date_time { get; set; }
            public string status { get; set; }
            public int id_org { get; set; }

        }

      


        //public class tbl_game_tiles
        //{
        //    public int id_game_tiles { get; set; }
        //    public string tile_name { get; set; }
        //    public string tile_image { get; set; }
        //    public int tile_position { get; set; }
        //    public string status { get; set; }
        //    public DateTime updated_date_time { get; set; }
        //    public int id_org { get; set; }
        //    public string tile_description { get; set; }
        //}


        //public class tbl_journey_tile_mapping
        //{
        //    public int id_tile_mapping { get; set; }
        //    public int id_game_tile { get; set; }
        //    public int id_journey_tile { get; set; }
        //    public DateTime updated_date_time { get; set; }
        //    public string status { get; set; }
        //    public int id_org { get; set; }

        //}

        public class tbl_university_kpi_grid
        {
            public int id_kpi_grid { get; set; }
            public int id_kpi_master { get; set; }
            public double start_range { get; set; }
            public double end_range { get; set; }
            public int kpi_value { get; set; }
            public string status { get; set; }
            public int updated_date_time { get; set; }
        }
        public class tbl_university_kpi_master
        {
            public int id_kpi_master { get; set; }
            public int id_organization { get; set; }
            public int kpi_name { get; set; }
            public int kpi_description { get; set; }
            public int kpi_type { get; set; }
            public string KPIID { get; set; }
            public int id_academic_tile { get; set; }
            public int id_brief_category { get; set; }
            public int id_creator { get; set; }
            public int status { get; set; }
            public DateTime updated_date_time { get; set; }
            public DateTime start_date { get; set; }
            public DateTime expiry_date { get; set; }
        }




    }
}