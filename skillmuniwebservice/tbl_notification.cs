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
    
    public partial class tbl_notification
    {
        public int id_notification { get; set; }
        public int notification_type { get; set; }
        public string notification_key { get; set; }
        public string notification_name { get; set; }
        public string notification_description { get; set; }
        public string notification_message { get; set; }
        public int reminder_flag { get; set; }
        public int id_organization { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public int reminder_time { get; set; }
        public int reminder_frequency { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public string notification_action_type { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> updated_date_time { get; set; }
    }
}
