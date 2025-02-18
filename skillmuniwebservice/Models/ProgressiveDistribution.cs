using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ProgressiveDistribution
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public void getProgressiveDistributionList(int uid, int limit, int cid, int scid)
        {
            List<int> qtnList = new List<int>();
        }
    }
}