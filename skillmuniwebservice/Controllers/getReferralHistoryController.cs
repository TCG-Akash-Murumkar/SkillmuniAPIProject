using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getReferralHistoryController : ApiController
    {
        public HttpResponseMessage Get(int UID)
        {
            List<ReferralHistory> result = new List<ReferralHistory>();
          

            try
            {
                using (JobDbContext db = new JobDbContext())
                {
                    string refcode = db.Database.SqlQuery<string>("select ref_id from tbl_user where ID_USER={0}", UID).FirstOrDefault();

                    List<tbl_referral_code_user_mapping> ref_map = db.Database.SqlQuery<tbl_referral_code_user_mapping>("select * from tbl_referral_code_user_mapping where referral_code={0}", refcode).ToList();
                    foreach (var itm in ref_map)
                    {
                        ReferralHistory obj = new ReferralHistory();
                        obj.credit_points = itm.referral_points;
                        obj.date = itm.updated_date_time;

                        tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}",itm.id_user).FirstOrDefault();
                        obj.mobile = prof.MOBILE;
                        obj.name = prof.FIRSTNAME + " " + prof.LASTNAME;
                        if (prof.PROFILE_IMAGE != "null" && prof.PROFILE_IMAGE != null)
                        {
                            obj.profile_pic = prof.PROFILE_IMAGE;

                        }
                        else
                        {
                            obj.profile_pic = ConfigurationManager.AppSettings["ProfileDefaultBase"].ToString() ; 
                        }
                        result.Add(obj);

                    }
                   
                }




            }
            catch (Exception e)
            {
                throw e;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
