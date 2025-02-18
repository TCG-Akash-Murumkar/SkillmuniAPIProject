using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Mail;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
{
    public class checkUserStatusController : ApiController
    {

        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(string UID, string OID)
        {
            APIRESPONSE responce = new APIRESPONSE();
            try
            {
                string str = " select * from tbl_user where id_user='" + UID + "'  limit 1";
                tbl_user dbuser = db.tbl_user.SqlQuery(str).FirstOrDefault();
                if (dbuser != null)
                {
                    //tbl_csst_role roles = db.tbl_csst_role.Find(dbuser.ID_ROLE);
                    tbl_organization ORGTBL = db.tbl_organization.Find(dbuser.ID_ORGANIZATION);
                    if (dbuser.STATUS == "A")
                    {

                        responce.KEY = "SUCCESS";
                        responce.MESSAGE = "";
                    }
                    else
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "";
                    }

                }
            }
            catch (Exception e)
            {
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }
}
