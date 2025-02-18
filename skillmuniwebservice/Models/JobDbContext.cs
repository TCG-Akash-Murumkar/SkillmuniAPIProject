using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace m2ostnextservice.Models
{
    public class JobDbContext:DbContext
    {

        static JobDbContext()
        {
            Database.SetInitializer<JobDbContext>(null);
        }
        public JobDbContext() : base("name=dbJob") { }
    }
}