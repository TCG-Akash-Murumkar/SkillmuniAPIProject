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
    public class getBriefAssessmsntResultController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string BRF, int UID, int OID, int ATM)
        {
            BriefReturnResponse response = null;
            tbl_brief_master brief = db.tbl_brief_master.Where(t => t.brief_code.ToLower() == BRF.ToLower().Trim() && t.status == "A").FirstOrDefault();
            if (brief != null)
            {
                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == ATM && t.id_brief_master == brief.id_brief_master && t.id_user == UID).FirstOrDefault();
                response = JsonConvert.DeserializeObject<BriefReturnResponse>(log.json_response);
            }
            if (response != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, response);
            }
        }
    }
}