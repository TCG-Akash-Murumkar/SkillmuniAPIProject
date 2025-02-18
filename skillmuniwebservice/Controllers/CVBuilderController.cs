using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using m2ostnextservice.Models;
using Rotativa;

namespace m2ostnextservice.Controllers
{
    public class CVBuilderController : Controller
    {
        // GET: CVBuilder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateResume(int id_cv)
        {
            //int id_cv = 1;
            CreateResumeDetails result = new CreateResumeDetails();


            result.data_flag = 2;
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                result.personel = db.Database.SqlQuery<tbl_cv_personel_info>("select * from tbl_cv_personel_info where id_cv={0}", id_cv).FirstOrDefault();
                result.education = db.Database.SqlQuery<tbl_cv_education>("select * from tbl_cv_education where id_cv={0}", id_cv).ToList();
                result.project_list = db.Database.SqlQuery<tbl_cv_project>("select * from tbl_cv_project where id_cv={0}", id_cv).ToList();
                result.additional_info = db.Database.SqlQuery<tbl_cv_additional_info>("select * from tbl_cv_additional_info where id_cv={0}", id_cv).FirstOrDefault();


            }
            ViewData["CVMaster"] = result;
            ViewData["Name"] = "Prasanth";

            return View();
        }
        public ActionResult GetCVDetails(int id_cv)
        {
            
            return new ActionAsPdf("GenerateResume",new { id_cv = id_cv });
        }
    }
}