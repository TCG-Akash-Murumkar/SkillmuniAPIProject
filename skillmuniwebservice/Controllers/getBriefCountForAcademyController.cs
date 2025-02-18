using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getBriefCountForAcademyController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID, int AcadamyTileId)
        {
            //// START ==== OLD CODE

            //string uids = new Utility().mysqlTrim(UID.ToString());
            //string oids = new Utility().mysqlTrim(OID.ToString());

            //List<tbl_brief_tile_academic_mapping> acadmap = new List<tbl_brief_tile_academic_mapping>();
            ////using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            ////{
            ////    acadmap = db.Database.SqlQuery<tbl_brief_tile_academic_mapping>("Select * from  tbl_brief_tile_academic_mapping  where id_academic_tile={0}", AcadamyTileId).ToList();
            ////    foreach (var itm in acadmap)
            ////    {
            ////        itm.BriefTileCode = db.Database.SqlQuery<string>("Select tile_code from  tbl_brief_category_tile  where id_brief_category_tile={0}", itm.id_journey_tile).FirstOrDefault();
            ////    }
            ////}

            //List<BriefAPIResource> list = new List<BriefAPIResource>();

            //BriefCountResponse count = new BriefCountResponse();
            //count.TOTALCOUNT = 0;
            //count.ReadCount = 0;
            //count.UnReadCount = 0;

            //List<tbl_brief_master> mas = new List<tbl_brief_master>();
            //using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            //{

            //    acadmap = db.Database.SqlQuery<tbl_brief_tile_academic_mapping>("Select * from  tbl_brief_tile_academic_mapping  where id_academic_tile={0}", AcadamyTileId).ToList();

            //    foreach (var itm in acadmap)
            //    {
            //        itm.BriefTileCode = db.Database.SqlQuery<string>("Select tile_code from  tbl_brief_category_tile  where id_brief_category_tile={0}", itm.id_journey_tile).FirstOrDefault();
            //    }
            //    foreach (var itm in acadmap)
            //    {
            //        int idcattile = db.Database.SqlQuery<int>("SELECT id_brief_category_tile FROM tbl_brief_category_tile where tile_code={0}", itm.BriefTileCode).FirstOrDefault();
            //        List<tbl_brief_master> idcats = db.Database.SqlQuery<tbl_brief_master>("SELECT * FROM tbl_brief_category_tile INNER JOIN tbl_brief_tile_category_mapping ON tbl_brief_category_tile.id_brief_category_tile = tbl_brief_tile_category_mapping.id_brief_category_tile where tbl_brief_category_tile.id_organization={0} and tbl_brief_tile_category_mapping.id_brief_category_tile={1} ;", OID, idcattile).ToList();
            //        foreach (var idcat in idcats)
            //        {
            //            if (idcat.id_brief_category != 0)
            //            {
            //                List<tbl_brief_master> instob = new BriefModel().getBriefList("select * from tbl_brief_master where status='A' and id_brief_category=" + idcat.id_brief_category, OID);
            //                mas.AddRange(instob);

            //            }


            //        }

            //    }


            //}
            //count.TOTALCOUNT = mas.Count;
            //int r = 0;
            //int u = 0;
            //foreach (var itm in mas)
            //{

            //    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            //    {
            //        tbl_brief_log log = db.Database.SqlQuery<tbl_brief_log>("select * from tbl_brief_log where id_user={0} and id_brief_master={1} ", UID, itm.id_brief_master).FirstOrDefault();
            //        if (log != null)//|| rd != null)
            //        {

            //            r++;
            //        }
            //        else
            //        {
            //            u++;
            //        }
            //    }





            //}
            //count.ReadCount = r;
            //count.UnReadCount = u;

            //return Request.CreateResponse(HttpStatusCode.OK, count);

            //// END ===== OLD CODE

            //// START ===== KISHAN - CODE

            string uids = new Utility().mysqlTrim(UID.ToString());
            string oids = new Utility().mysqlTrim(OID.ToString());

            BriefCountResponse count = new BriefCountResponse();
            count.TOTALCOUNT = 0;
            count.ReadCount = 0;
            count.UnReadCount = 0;

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                count.TOTALCOUNT = db.Database.SqlQuery<int>($"SELECT COUNT(*) FROM tbl_brief_master m INNER JOIN tbl_brief_tile_category_mapping catm ON catm.id_brief_category = m.id_brief_category INNER JOIN tbl_brief_category_tile cat ON cat.id_brief_category_tile = catm.id_brief_category_tile INNER JOIN tbl_brief_tile_academic_mapping ac ON ac.id_journey_tile = cat.id_brief_category_tile WHERE m.status='A' and cat.id_organization={OID} and ac.id_academic_tile={AcadamyTileId};").FirstOrDefault();

                count.ReadCount = db.Database.SqlQuery<int>($"SELECT COUNT(*) from tbl_brief_log log\r\nINNER JOIN tbl_brief_master m ON m.id_brief_master = log.id_brief_master\r\nINNER JOIN tbl_brief_tile_category_mapping catm ON catm.id_brief_category = m.id_brief_category\r\nINNER JOIN tbl_brief_category_tile cat ON cat.id_brief_category_tile = catm.id_brief_category_tile \r\nINNER JOIN tbl_brief_tile_academic_mapping ac ON ac.id_journey_tile = cat.id_brief_category_tile\r\nWHERE m.status='A' and cat.id_organization={OID} and ac.id_academic_tile={AcadamyTileId} and log.id_user = {UID};").FirstOrDefault();
                count.UnReadCount = count.TOTALCOUNT - count.ReadCount;
            }
            return Request.CreateResponse(HttpStatusCode.OK, count);

            //// END ===== KISHAN - CODE

        }
    }
}
