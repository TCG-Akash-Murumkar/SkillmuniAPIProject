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
    public class getCategoryTileListForNonLearningController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID, int tile_type)
        {
            //new RoadMapLogic().GameTileLog(UID, OID, AcademicTileId);
            string baseurl = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BRIEF/";
            //List<RoadMapModels.tbl_brief_tile_academic_mapping> tile_mapping = new RoadMapLogic().getTilesMapping(AcademicTileId);
            List<tbl_brief_category_tile> mapped_tiles = new List<tbl_brief_category_tile>();
            //foreach (var itm in tile_mapping)
            //{
            //    tbl_brief_category_tile obj = new RoadMapLogic().getJourneytile(itm.id_journey_tile);
            //    mapped_tiles.Add(obj);

            //}
            mapped_tiles = db.Database.SqlQuery<tbl_brief_category_tile>("select * from tbl_brief_category_tile where tile_type={0} and status='A' ORDER BY tile_position ASC", tile_type).ToList();

            //List<tbl_brief_category_tile> tiles = db.tbl_brief_category_tile.Where(t => t.status == "A" && t.id_organization == OID).ToList();
            foreach (tbl_brief_category_tile item in mapped_tiles)
            {
                item.buttontext = db.Database.SqlQuery<string>("select buttontext from tbl_brief_category_tile where id_brief_category_tile={0} ", item.id_brief_category_tile).FirstOrDefault();
                item.tile_image = baseurl + item.id_organization + "/TILE/" + item.tile_image;
            }
            mapped_tiles = mapped_tiles.OrderBy(o => o.updated_date_time).ToList();
            BriefTilResponse res = new BriefTilResponse();
            if (mapped_tiles.Count > 0)
            {
                res.Status = "SUCCESS";
                mapped_tiles = mapped_tiles.OrderBy(x => x.tile_position).ToList();
                res.Tile = mapped_tiles;



            }
            else
            {

                res.Status = "FAILED";
            }
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}
