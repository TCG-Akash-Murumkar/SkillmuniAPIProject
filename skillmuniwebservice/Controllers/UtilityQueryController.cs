using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UtilityQueryController : Controller
    {
        // GET: UtilityQuery
        public ActionResult Index()
        {
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                List<string> loc = db.Database.SqlQuery<string>("SELECT DISTINCT  LOCATION FROM tbl_user inner join tbl_profile on tbl_user.ID_USER=tbl_profile.ID_USER  where tbl_user.ID_ORGANIZATION=48").ToList();
                foreach (var itm in loc)
                {
                    int totcnt= db.Database.SqlQuery<int>("select  count(tbl_user.ID_USER) as users  FROM tbl_user inner join tbl_profile on tbl_user.ID_USER=tbl_profile.ID_USER  where tbl_user.ID_ORGANIZATION=48 and LOCATION='"+itm+"'").FirstOrDefault();
                }

            }
                return View();
        }
    }
}