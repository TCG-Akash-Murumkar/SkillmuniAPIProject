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
    public class getBriefNotificationListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            List<APIBrief> list = new List<APIBrief>();
            string sqlb = "";

            sqlb = "SELECT a.id_organization, question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory " +
                " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE a.status='A' and a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = " + UID + "  AND a.id_organization = " + OID + " AND (scheduled_status='S' or published_status='S') AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp DESC LIMIT 20";
            list = new BriefModel().getAPIBriefList(sqlb);
            int srno = 1;
            foreach (var itm in list)
            {
                itm.SRNO = srno;
                srno++;
                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == itm.id_brief_master && t.id_user == UID).FirstOrDefault();
                if (log != null)
                {
                    itm.RESULTSTATUS = 1;
                    itm.RESULTSCORE = Convert.ToDouble(log.brief_result);
                }
                else
                {
                    itm.RESULTSTATUS = 0;
                    itm.RESULTSCORE = 0;
                }
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