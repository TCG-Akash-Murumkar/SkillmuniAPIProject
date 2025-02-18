using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getGamesListForOrganizationController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID)
        {
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            List<tbl_org_game_master> games = new List<tbl_org_game_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                games = db.Database.SqlQuery<tbl_org_game_master>("select * from tbl_org_game_master where id_org={0} and  status='A'", OID).ToList();

            }

               
            return Request.CreateResponse(HttpStatusCode.OK, games);
        }

    }
}
