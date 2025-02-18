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
    public class getBannerAdContentController : ApiController
    {
        public HttpResponseMessage Get(int id_academy, int id_cat_tile)
        {
            //tbl_banner_config_master result = new tbl_banner_config_master();
            bannerApi apires = new bannerApi();
            List<tbl_banner_config_master> result = new List<tbl_banner_config_master>();
            //new RoadMapLogic().GameTileLog(UID, OID, AcademicTileId);
            // string baseurl = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BRIEF/"

             using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
             {

                //result = db.Database.SqlQuery<tbl_banner_config_master>("select * from tbl_banner_config_master inner join tbl_banner_ad_config on tbl_banner_config_master.id_banner_config= tbl_banner_ad_config.id_banner_config where tbl_banner_ad_config.id_academic_tile={0} and tbl_banner_ad_config.id_brief_category_tile={1}", id_academy, id_cat_tile).FirstOrDefault();
                result = db.Database.SqlQuery<tbl_banner_config_master>("select * from tbl_banner_config_master inner join tbl_banner_ad_config on tbl_banner_config_master.id_banner_config= tbl_banner_ad_config.id_banner_config where tbl_banner_ad_config.id_academic_tile={0} and tbl_banner_ad_config.id_brief_category_tile={1} and tbl_banner_config_master.status='A'", id_academy, id_cat_tile).ToList();
                foreach(var itm in result)
                {
                    if (result != null)
                    {
                        itm.banner_ad = db.Database.SqlQuery<tbl_banner_ad_config>("SELECT * FROM tbl_banner_ad_config where status='A' and id_banner_config={0} ", itm.id_banner_config).ToList();
                        itm.bannerbody = db.Database.SqlQuery<tbl_banner_body>("SELECT * FROM tbl_banner_body where status='A' and id_banner_config={0} ", itm.id_banner_config).ToList();
                        foreach (var item in itm.bannerbody)
                        {
                            item.banner_image = ConfigurationManager.AppSettings["BANNERBODYIMAGE"].ToString() + item.banner_image;
                        }
                    }
                }
                if(result.Count > 0)
                {
                    apires.status = "SUCCESS";
                    apires.ad_master_banner = result;
                }
                else
                {
                    apires.status = "FAILURE";
                }

                //if (result != null)
                //{
                //    result.banner_ad = db.Database.SqlQuery<tbl_banner_ad_config>("SELECT * FROM tbl_banner_ad_config where status='A' and id_banner_config={0} ", result.id_banner_config).ToList();
                //    result.bannerbody = db.Database.SqlQuery<tbl_banner_body>("SELECT * FROM tbl_banner_body where status='A' and id_banner_config={0} ", result.id_banner_config).ToList();
                //    foreach (var itm in result.bannerbody)
                //    {
                //        itm.banner_image = ConfigurationManager.AppSettings["BANNERBODYIMAGE"].ToString() + itm.banner_image;
                //    }
                //}
            }

            
            //return Request.CreateResponse(HttpStatusCode.OK, result);
            return Request.CreateResponse(HttpStatusCode.OK, apires);
        }

    }
}
