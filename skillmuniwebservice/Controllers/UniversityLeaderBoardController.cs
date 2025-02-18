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
    public class UniversityLeaderBoardController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID, int GameId)
        {
            LeaderBoardResponse Leader = new LeaderBoardResponse();

            Leader.id_game = GameId;
            Leader.id_user = UID;
            Leader.Badge = new UniversityScoringlogic().getBadgeList(UID,GameId);
           // Leader.UserList = new UniversityScoringlogic().getUserListLeaderBoard(GameId,OID);
            Leader.UserList= Leader.UserList.OrderBy(o => o.metric_score).ToList();
            using (m2ostnextserviceDbContext db=new m2ostnextserviceDbContext())
            {
                Leader.UserProfileImage = ConfigurationManager.AppSettings["ProfileImageBase"] + db.Database.SqlQuery<string>("select PROFILE_IMAGE from tbl_profile where ID_USER={0}",UID).FirstOrDefault();

            }


            return Request.CreateResponse(HttpStatusCode.NoContent, Leader);
           
        }
    }
}
