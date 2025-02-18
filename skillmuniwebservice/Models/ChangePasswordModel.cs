using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ChangePasswordModel
    {


    }

    public class Password
    {
        public int ID_USER { get; set; }
        public int ID_ORGANIZATION { get; set; }
        public string NEWPASSWORD { get; set; }     
        public DateTime UPDATEDTIME { get; set; }
        public string OLDPASSWORD { get; set; }

    }

}