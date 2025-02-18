using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace m2ostnextservice.Controllers
{
    public class getUnreadBriefListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string uids)
        {
            //   check();
            int unread = 0;
            uids = new AESAlgorithm().getDecryptedString(uids);
            string str = " select * from tbl_user where USERID='" + uids + "' and status='A'";
            tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();

            if (dbuser != null)
            {
                List<APIBrief> list = new List<APIBrief>();

                string sqls = "select * from tbl_brief_user_assignment where id_user='" + dbuser.ID_USER + "' and assignment_status='S'";
                sqls = "SELECT * FROM tbl_brief_read_status where id_user='" + dbuser.ID_USER + "' and read_status=0 and action_status=0 and status='A'";
                List<tbl_brief_read_status> check1 = db.tbl_brief_read_status.SqlQuery(sqls).ToList();
                if (check1.Count > 0)
                {
                    unread = check1.Count;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, unread);
        }
    }
}