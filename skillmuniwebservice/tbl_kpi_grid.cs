//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace m2ostnextservice
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_kpi_grid
    {
        public int id_kpi_grid { get; set; }
        public Nullable<int> id_kpi_master { get; set; }
        public Nullable<double> start_range { get; set; }
        public Nullable<double> end_range { get; set; }
        public Nullable<int> kpi_value { get; set; }
        public string kpi_text { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }
}
