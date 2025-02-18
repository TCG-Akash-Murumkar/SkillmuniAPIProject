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
    public class getBannerContentController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int LOCATION)
        {
            
            tbl_banner_config_master result = new tbl_banner_config_master();
            //new RoadMapLogic().GameTileLog(UID, OID, AcademicTileId);
           // string baseurl = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BRIEF/";

           
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    //result = db.Database.SqlQuery<tbl_banner_config_master>("SELECT * FROM tbl_banner_config_master where banner_location={0} and banner_type={1}  and status='A'", LOCATION, 1).FirstOrDefault();
                    result = db.Database.SqlQuery<tbl_banner_config_master>("SELECT * FROM tbl_banner_config_master where banner_location={0} and banner_type={1}", LOCATION, 1).FirstOrDefault();
                if (result != null)
                {
                    result.bannerbody = db.Database.SqlQuery<tbl_banner_body>("SELECT * FROM tbl_banner_body where status='A' and id_banner_config={0} ", result.id_banner_config).ToList();
                    foreach (var itm in result.bannerbody)
                    {
                        itm.banner_image = ConfigurationManager.AppSettings["BANNERBODYIMAGE"].ToString() + itm.banner_image;
                    }
                }
                }

               



            //else if (BTYPE == 2)
            //{
            //    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            //    {

            //        result = db.Database.SqlQuery<tbl_banner_config_master>("SELECT * FROM tbl_banner_config_master where banner_location={0} and banner_type={1}  and status='A'", LOCATION,2).FirstOrDefault();

            //        result.banner_ad = db.Database.SqlQuery<tbl_banner_ad_config>("SELECT * FROM tbl_banner_ad_config where status='A' and id_banner_config={0} ", result.id_banner_config).FirstOrDefault();
            //        result.bannerbody=db.Database.SqlQuery<tbl_banner_body>("SELECT * FROM tbl_banner_body where status='A' and id_banner_config={0} ", result.id_banner_config).ToList();
            //        foreach (var itm in result.bannerbody)
            //        {
            //            itm.banner_image = ConfigurationManager.AppSettings["BANNERBODYIMAGE"].ToString() + itm.banner_image;
            //        }
            //    }

            //}

            return Request.CreateResponse(HttpStatusCode.OK, result);
            
        }
    }
}
