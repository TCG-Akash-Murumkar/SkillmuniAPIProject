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
    public class getBriefNativeNotificatonController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            List<APIBrief> list = new List<APIBrief>();
            string sqlb = "";

            sqlb = "SELECT a.id_organization, question_count, brief_title, brief_code, 'NA' brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, 'NA' brief_category, 'NA' brief_subcategory, '0' id_brief_category, '0' id_brief_subcategory " +
                "FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c WHERE a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND b.id_user = " + UID + "  AND read_status = 0 AND action_status = 0 AND a.id_organization = " + OID + "  AND a.status = 'A' AND c.status = 'A' AND (b.scheduled_status = 'S' OR b.published_status = 'S') AND (b.published_datetime < NOW() OR b.scheduled_datetime < NOW()) ORDER BY a.brief_title ";

            list = new BriefModel().getAPIBriefList(sqlb);
            int srno = 1;
            foreach (var itm in list)
            {
                itm.SRNO = srno;
                srno++;
                itm.RESULTSTATUS = 0;
                itm.RESULTSCORE = 0;
            }
            if (list != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, list);
            }
        }
    }
}