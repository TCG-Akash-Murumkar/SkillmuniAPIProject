using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class GetUserAttemptsController : ApiController
    {
        public HttpResponseMessage Get(int id_user)
        {
            List<AttemptResponse> attempt = new List<AttemptResponse>();
            List<tbl_user_level_log> level = new List<tbl_user_level_log>();
            


            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    level = db.Database.SqlQuery<tbl_user_level_log>("select DISTINCT  * from tbl_user_level_log where id_user={0} and status='A'", id_user).ToList();
                    foreach (var itm in level)
                    {
                        AttemptResponse obj = new AttemptResponse();
                        obj.last_attempt = db.Database.SqlQuery<int>("select MAX(attempt_no) AS maxlevel from tbl_user_level_log where id_user={0} and level={1} and status='A'", id_user, itm.level).FirstOrDefault();
                        obj.id_level = itm.level;
                        attempt.Add(obj);

                    }

                }

            }
            catch (Exception e)
            {
                throw e;

            }

            return Request.CreateResponse(HttpStatusCode.OK, attempt);
        }

    }
}
