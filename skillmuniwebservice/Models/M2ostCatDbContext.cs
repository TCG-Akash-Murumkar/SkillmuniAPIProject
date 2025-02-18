using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class M2ostCatDbContext: DbContext
    {
        static M2ostCatDbContext()
        {
            Database.SetInitializer<M2ostCatDbContext>(null);
        }
        public M2ostCatDbContext() : base("name=m2ostcat") { }
    }
}