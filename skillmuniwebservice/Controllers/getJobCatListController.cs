using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getJobCatListController : ApiController
    {
        public HttpResponseMessage Get(int OID)
        {
            tbl_job_category_header fin = new tbl_job_category_header();
            fin.header = "Select Category";
            fin.id_header = 1;
            fin.status = "A";
            fin.updated_date_time = DateTime.Now;
            List<tbl_job_category> cat = new List<tbl_job_category>();
            List<JOBCATEGORYLIST> res = new List<JOBCATEGORYLIST>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                cat = db.Database.SqlQuery<tbl_job_category>("select * from tbl_job_category where status='A'").ToList();

                foreach (var itm in cat)
                {
                    JOBCATEGORYLIST inst = new JOBCATEGORYLIST();
                    inst.id_job_category = itm.id_job_category;
                    inst.job_category = itm.job_category;
                    inst.tile_image = ConfigurationManager.AppSettings["jobcatimg"].ToString() + itm.tile_image;
                    inst.tile_position = itm.tile_position;
                    res.Add(inst);
                }
            }
            if (cat.Count > 0)
            {
                fin.category = cat;
                //fin.Tile = res;
                ////fin.Tile = fin.Tile.OrderBy(x => x.tile_position).ToList();
                //fin.STATUS = "SUCCESS";

            }
            else
            {
                //fin.STATUS = "FAILURE";

            }

            return Request.CreateResponse(HttpStatusCode.OK, fin);
        }

    }
}
