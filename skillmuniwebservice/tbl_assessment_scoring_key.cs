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
    
    public partial class tbl_assessment_scoring_key
    {
        public tbl_assessment_scoring_key()
        {
            this.tbl_assessment_header = new HashSet<tbl_assessment_header>();
        }
    
        public int id_assessment_scoring_key { get; set; }
        public string header_name { get; set; }
        public Nullable<int> id_assessment { get; set; }
        public Nullable<int> id_assessment_theme { get; set; }
        public Nullable<int> position { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    
        public virtual ICollection<tbl_assessment_header> tbl_assessment_header { get; set; }
    }
}
