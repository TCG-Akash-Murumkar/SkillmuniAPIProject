using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
    
{
    public class UserDashboardDataController : ApiController
    {
        public HttpResponseMessage Get(int id_user)
        {
           List<tbl_user_level_log> user = new List<tbl_user_level_log>();


            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    user = db.Database.SqlQuery<tbl_user_level_log>("select * from tbl_user_level_log where id_user={0} and status='A'",id_user).ToList();
                    foreach (var itm in user)
                    {
                        itm.assessment = db.Database.SqlQuery<tbl_user_assessment_log>("select * from tbl_user_assessment_log where id_user={0} and level={1} and attempt_no={2} and status='A'",itm.id_user,itm.level,itm.attempt_no).ToList();
                    }

                }

            }
            catch(Exception e)
            {
                throw e;

            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }



    }
}
