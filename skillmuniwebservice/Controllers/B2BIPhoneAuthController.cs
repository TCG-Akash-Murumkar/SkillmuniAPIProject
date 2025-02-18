using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class B2BIPhoneAuthController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        string responceString = "";
        public HttpResponseMessage Post([FromBody] B2BIphoneAuth user)
        {
            user.PASSWORD = PasswordEncryption.ToMD5Hash(user.PASSWORD);
            string str = " select * from tbl_user where lower(USERID) like lower(\"" + user.USERID + "\") AND PASSWORD like \"" + user.PASSWORD + "\" AND ID_ORGANIZATION=" + user.ORG_ID + "  AND ID_ROLE=" + user.ROLE_ID + " ";
            tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();

            if (dbuser != null)
            {
                tbl_user_device_link link = db.tbl_user_device_link.Where(t => t.ID_USER == dbuser.ID_USER).FirstOrDefault();
                if (link != null)
                {
                    tbl_user userC = db.tbl_user.Find(link.ID_USER);
                    if (userC.EXPIRY_DATE < System.DateTime.Now)
                    {
                        responceString = "expired";
                    }
                    else
                    {
                        link.UPDATED_DATE_TIME = System.DateTime.Now;
                        if (!string.IsNullOrEmpty(user.DEVID))
                        {
                            link.DEVICE_ID = user.DEVID;
                        }
                        db.SaveChanges();
                        responceString = user.USERID; ;
                    }
                }

            }
            else
            {
                responceString = "false";
            }
            // return Request.CreateResponse(HttpStatusCode.OK, "false");
            return Request.CreateResponse(HttpStatusCode.OK, responceString);
        }
    }
}
