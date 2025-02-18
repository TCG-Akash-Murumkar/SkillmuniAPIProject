using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class B2BIPhoneController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody] B2BIphone user)
        {
            user.PASSWORD = PasswordEncryption.ToMD5Hash(user.PASSWORD);
            string str = " select * from tbl_user where USERID like \""+user.USERID+"\" AND PASSWORD like \""+user.PASSWORD+"\" AND  id_organization=" + user.ORG_ID + " AND ID_ROLE=" + user.ROLE_ID + " ";
            tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();
            if (dbuser != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dbuser.USERID);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "false");
            }

        }
    }
}
