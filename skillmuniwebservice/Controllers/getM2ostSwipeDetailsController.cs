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
    public class getM2ostSwipeDetailsController : ApiController
    {

        public HttpResponseMessage Get(int id_brief_master)
        {
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            List<tbl_third_party_right_swipe_m2ost> ctmap = new List<tbl_third_party_right_swipe_m2ost>();

            using (m2ostnextserviceDbContext dbcatmapm = new m2ostnextserviceDbContext())
            {
                ctmap = dbcatmapm.Database.SqlQuery<tbl_third_party_right_swipe_m2ost>("select * from tbl_third_party_right_swipe_m2ost where id_brief={0}", id_brief_master).ToList();


            }


            foreach (var itm in ctmap)
            {

                if (itm.type == 2)
                {
                    using (M2ostCatDbContext dbcat = new M2ostCatDbContext())
                    {
                        itm.CATEGORYNAME = dbcat.Database.SqlQuery<string>("select CATEGORYNAME from tbl_category where ID_CATEGORY={0} ", itm.id_category).FirstOrDefault();
                        itm.Heading_title = dbcat.Database.SqlQuery<string>("select Heading_title from tbl_category_heading where id_category_heading={0} ", itm.id_category_heading).FirstOrDefault();
                        itm.CategoryImage = ConfigurationManager.AppSettings["CATIm"].ToString() + dbcat.Database.SqlQuery<string>("select IMAGE_PATH from tbl_category where ID_CATEGORY={0} ", itm.id_category).FirstOrDefault();

                    }
                }






            }




            return Request.CreateResponse(HttpStatusCode.OK, ctmap);
        }



    }
}
