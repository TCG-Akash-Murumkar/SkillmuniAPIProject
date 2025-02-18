using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using Newtonsoft.Json;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getMyReportDetailsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int LID, int SID,int UID,int OID)
        {
            AssessmentSheet sheet = new AssessmentSheet();
            AssessmentResponce ResposeBack = new AssessmentResponce();
            tbl_assessmnt_log log = db.tbl_assessmnt_log.Where(t => t.id_assessmnt_log == LID && SID==t.id_assessment_sheet).FirstOrDefault();
            if (log != null)
            {
                 ResposeBack = JsonConvert.DeserializeObject<AssessmentResponce>(log.json_response);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ResposeBack);

        }
    }
}
