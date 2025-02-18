using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class CrmDbContext:DbContext
    {
        static CrmDbContext()
        {
            Database.SetInitializer<m2ostnextserviceDbContext>(null);
        }
        public CrmDbContext() : base("name=dbconnectioncrm") { }
    }
}