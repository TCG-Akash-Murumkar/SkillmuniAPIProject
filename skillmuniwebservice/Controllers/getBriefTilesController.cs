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
    public class getBriefTilesController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID,int AcademicTileId)
        {
            new RoadMapLogic().GameTileLog(UID, OID, AcademicTileId);
            string baseurl = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BRIEF/";
            List<RoadMapModels.tbl_brief_tile_academic_mapping> tile_mapping = new RoadMapLogic().getTilesMapping(AcademicTileId);
            List<tbl_brief_category_tile> mapped_tiles = new List<tbl_brief_category_tile>();
            foreach (var itm in tile_mapping)
            {
                tbl_brief_category_tile obj = new RoadMapLogic().getJourneytile(itm.id_journey_tile);
                mapped_tiles.Add(obj);

            }

            //List<tbl_brief_category_tile> tiles = db.tbl_brief_category_tile.Where(t => t.status == "A" && t.id_organization == OID).ToList();
            foreach (tbl_brief_category_tile item in mapped_tiles)
            {
                item.tile_image = baseurl + item.id_organization + "/TILE/" + item.tile_image;
            }
            mapped_tiles = mapped_tiles.OrderBy(o => o.tile_position).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, mapped_tiles);
        }
    }
}