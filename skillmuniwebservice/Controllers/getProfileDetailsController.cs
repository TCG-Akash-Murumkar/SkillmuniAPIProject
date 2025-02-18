using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getProfileDetailsController : ApiController
    {
        public HttpResponseMessage Get(int UID)
        {
            tbl_profile profile = new tbl_profile();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                profile = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0} ", UID).FirstOrDefault();
                profile.ref_code = db.Database.SqlQuery<string>("select referral_code from tbl_referral_code_user_mapping where id_user={0}",UID).FirstOrDefault();
                if (profile.social_dp_flag == 0)
                {
                   profile.PROFILE_IMAGE= WebConfigurationManager.AppSettings["DpBase"]+ profile.PROFILE_IMAGE;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, profile);
        }
    }
}
