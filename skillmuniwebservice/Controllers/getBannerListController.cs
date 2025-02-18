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
    public class getBannerListController : ApiController
    {
        //db_m2ostEntities db = new db_m2ostEntities();



        public HttpResponseMessage Get(int OID)
        {
            APIRESULTBanner fin = new APIRESULTBanner();
            List<tbl_banner> Ban = new List<tbl_banner>();
            List<Banner> res = new List<Banner>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
               
                Ban = db.Database.SqlQuery<tbl_banner>("select * from tbl_banner where id_organization={0} and status='A'", OID).ToList();
                foreach (var itm in Ban)
                {
                    Banner inst = new Banner();
                    inst.banner_name = itm.banner_name;
                    inst.banner_image= ConfigurationManager.AppSettings["Bannerim"].ToString() + itm.banner_image;
                    res.Add(inst);
                }
            }
            if (res.Count > 0)
            {
                fin.Banner = res;
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
