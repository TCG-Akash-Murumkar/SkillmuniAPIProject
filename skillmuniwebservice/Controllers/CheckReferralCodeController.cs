using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class CheckReferralCodeController : ApiController
    {

        public HttpResponseMessage Get(string code)
        {

            ReferralResponse res = new ReferralResponse();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

               string refname= db.Database.SqlQuery<string>("select referral_name from tbl_referral_code_master where referral_code={0} ", code).FirstOrDefault();
                if (refname != null)
                {
                    res.is_exist = 1;
                    res.referral_name = refname;
                }
                else
                {
                    int iduser = db.Database.SqlQuery<int>("select ID_USER from tbl_user where ref_id={0} ", code).FirstOrDefault();
                    if (iduser > 0)
                    {
                        res.is_exist = 1;
                        res.referral_name = db.Database.SqlQuery<string>("select FIRSTNAME from tbl_profile where ID_USER={0} ", iduser).FirstOrDefault(); ;

                    }
                    else
                    {
                        res.is_exist = 0;


                    }


                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}
