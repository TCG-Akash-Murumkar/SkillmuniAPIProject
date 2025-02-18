using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class OrgGameUserAvatarUpdateController : ApiController
    {
        public HttpResponseMessage Post([FromBody]AvatarData Avatar)
        {
            ScoreLOgicResponse Result = new ScoreLOgicResponse();
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                   int id_log = db.Database.SqlQuery<int>("select id_log from tbl_org_game_user_avatar where id_user={0} and status='A'",Avatar.UID).FirstOrDefault();
                    
                    if (id_log ==0)
                    {
                       
                        db.Database.ExecuteSqlCommand("Insert into tbl_org_game_user_avatar (id_user,avatar_type,id_org,status,updated_date_time) values ({0},{1},{2},{3},{4})", Avatar.UID, Avatar.avatar_type,Avatar.OID,"A",DateTime.Now);
                        Result.STATUS = "SUCCESS";
                        Result.OID = Avatar.OID;
                        Result.MESSAGE = "Successfully Updated.";
                       
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("Update tbl_org_game_user_avatar set avatar_type={0} , updated_date_time={1}  where id_user={2}", Avatar.avatar_type,  DateTime.Now, Avatar.UID);

                        Result.STATUS = "SUCCESS";
                        Result.MESSAGE = "Successfully Updated.";

                    }



                }

            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";
                Result.MESSAGE = "Something went wrong.";

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

    }
}
