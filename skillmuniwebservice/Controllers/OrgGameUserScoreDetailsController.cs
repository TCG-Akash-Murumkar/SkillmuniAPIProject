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
    public class OrgGameUserScoreDetailsController : ApiController
    {
        public HttpResponseMessage Get(int UID, int OID,int id_org_game)
        {
            //string baseurl = ConfigurationManager.AppSettings["GameTile"].ToString();
            GameUserLog ob = new GameUserLog();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                ob.final_assessmnet_right_count = db.Database.SqlQuery<int>("select COALESCE(SUM(is_correct),0) total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=1 and attempt_no=1", UID, id_org_game).FirstOrDefault();
                ob.final_assessmnet_wrong_count = db.Database.SqlQuery<int>("select count( is_correct) as total from tbl_org_game_user_assessment_log where id_user={0} and id_org_game={1} and is_correct=0 and attempt_no=1 ", UID, id_org_game).FirstOrDefault();
                ob.final_assessmnet_total_count = ob.final_assessmnet_right_count + ob.final_assessmnet_wrong_count;
                if (ob.final_assessmnet_total_count > 0)
                {
                    ob.assessment_score = (Convert.ToDouble(ob.final_assessmnet_right_count) / Convert.ToDouble(ob.final_assessmnet_total_count)) * 100;
                }            
                tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                if (prof != null)
                {
                    ob.Name = prof.FIRSTNAME + " " + prof.LASTNAME;
                    ob.PROFILE_IMAGE = ConfigurationManager.AppSettings["profileimage_base"].ToString() + prof.PROFILE_IMAGE;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, ob);
        }
    }
}
