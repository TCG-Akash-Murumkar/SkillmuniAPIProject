using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class m2ostnextserviceDbContext: DbContext
    {
        static m2ostnextserviceDbContext()
        {
            Database.SetInitializer<m2ostnextserviceDbContext>(null);
        }
        public m2ostnextserviceDbContext() : base("name=dbconnectionstring") { }
    }
}