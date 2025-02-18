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
    public class getJobCategoryListController : ApiController
    {


        //public HttpResponseMessage Get(int OID)
        public HttpResponseMessage Get()
        {
            APIRESULTCatTile fin = new APIRESULTCatTile();
            List<tbl_ce_evaluation_jobindustry> cat = new List<tbl_ce_evaluation_jobindustry>();
            List<JOBCATTILE> res = new List<JOBCATTILE>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                cat = db.Database.SqlQuery<tbl_ce_evaluation_jobindustry>("select * from tbl_ce_evaluation_jobindustry where status='A'").ToList();
                foreach (var itm in cat)
                {
                    JOBCATTILE inst = new JOBCATTILE();
                    inst.id_job_category = itm.id_ce_evaluation_jobindustry;
                    inst.job_category = itm.ce_job_industry;
                    inst.tile_image = ConfigurationManager.AppSettings["jobcatimg"].ToString() + itm.tile_image;
                    inst.tile_position = itm.tile_position;
                    inst.buttontext = itm.buttontext;
                    res.Add(inst);
                }
            }
            if (res.Count > 0)
            {
                fin.Tile = res;
                fin.Tile = fin.Tile.OrderBy(x => x.tile_position).ToList();
                fin.STATUS = "SUCCESS";

            }
            else
            {
                fin.STATUS = "FAILURE";

            }

            return Request.CreateResponse(HttpStatusCode.OK, fin);
        }

    }
}
