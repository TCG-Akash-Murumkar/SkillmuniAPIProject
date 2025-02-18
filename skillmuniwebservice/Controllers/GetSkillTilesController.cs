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
    public class GetSkillTilesController : ApiController
    {

        public HttpResponseMessage Get(int UID, int OID)
        {
            string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            List<RoadMapModels.tbl_academic_tiles> tiles = new RoadMapLogic().getGameTiles(OID).Where(o => o.tile_position > 10000).ToList();
            foreach (var item in tiles)
            {
                item.tile_image = baseurl + item.tile_image;
            }

            return Request.CreateResponse(HttpStatusCode.OK, tiles);
        }
    }
}
