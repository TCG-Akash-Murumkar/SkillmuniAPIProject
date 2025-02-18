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
    public class B2BMonoLoginController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody]MonoLogin user)
        {
            APIRESPONSE responce = new APIRESPONSE();
            try
            {

                string str1 = " select * from tbl_user where id_user='" + user.UID + "' and id_organization='" + user.OID + "'   limit 1";
                tbl_user dbuser = db.tbl_user.SqlQuery(str1).FirstOrDefault();
                if (dbuser != null)
                {
                    if (dbuser.STATUS == "A")
                    {
                        string str = " select * from tbl_report_login_log where id_user='" + user.UID + "' and id_organization='" + user.OID + "' and IMEI not like 'WEBSITE' order by id_report_login_log desc limit 1";
                        tbl_report_login_log log = db.tbl_report_login_log.SqlQuery(str).FirstOrDefault();

                        if (log != null)
                        {
                            if (log.IMEI == user.IMEI)
                            {
                                responce.KEY = "SUCCESS";
                                responce.MESSAGE = "SUCCESS";
                            }
                            else
                            {
                                responce.KEY = "FAILURE";
                                responce.MESSAGE = "You are Logged in with some other device , please login again to use in this device.";
                            }
                        }
                        else
                        {
                            responce.KEY = "SUCCESS";
                            responce.MESSAGE = "SUCCESS";
                        }
                    }
                    else
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "User account is deactivated. Please contact your administrator";
                   
                    }
                }
                else
                {
                    responce.KEY = "FAILURE";
                    responce.MESSAGE = "'User account is invalid. Please contact your administrator";
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
