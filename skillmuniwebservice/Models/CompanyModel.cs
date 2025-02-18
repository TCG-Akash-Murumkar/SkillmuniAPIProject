using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CompanyModel
    {
    }

    public class tbl_ce_industry_role
    {
        public int id_ce_industry_role { get; set; }
        public int id_organization { get; set; }
        public string ce_industry_role { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public class tbl_ce_industry
    {
        public int id_ce_industry { get; set; }
        public int id_organization { get; set; }
        public string ce_industry { get; set; }
        public int id_ce_industry_role { get; set; }
        public int role_job_point { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }

    public class tbl_ce_company_details
    {
        public int id_ce_company_details { get; set; }
        public int id_organization { get; set; }
        public int id_ce_industry { get; set; }
        public int id_ce_industry_role { get; set; }
        public string ce_company_name { get; set; }
        public string job_title { get; set; }
        public int job_point { get; set; }
        public string status { get; set; }
        public DateTime updated_date_time { get; set; }
    }
}