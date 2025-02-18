using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getActiveBriefListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            List<APIBrief> list = new List<APIBrief>();
            string sqlb = "SELECT a.id_organization, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp LIMIT 10 ";
            list = new BriefModel().getAPIBriefList(sqlb);
            foreach (var itm in list)
            {
                List<APIBrief> brflist = new List<APIBrief>();
                BriefBody bBody = null;
                string brfcode = itm.brief_code.ToLower().Trim();

                string sqlb1 = "SELECT a.id_organization, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
                sqlb1 += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE   LOWER(brief_code) = '" + brfcode + "' AND a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) LIMIT 10 ";
                brflist = new BriefModel().getAPIBriefList(sqlb1);
                //if (brflist != null)
                //{
                //    bBody = new BriefBody();
                //    APIBrief brief = brflist[0];

                //    bBody.BRIEF = brief;
                //    string bsql = "SELECT * FROM tbl_brief_question where id_brief_question in (select id_brief_question from  tbl_brief_question_mapping where id_brief_master=" + brief.id_brief_master + ")";
                //    List<tbl_brief_question> qList = db.tbl_brief_question.SqlQuery(bsql).ToList();
                //    List<QuestionList> qtnList = new List<QuestionList>();
                //    foreach (tbl_brief_question item in qList)
                //    {
                //        QuestionList temp = new QuestionList();
                //        temp.question = item;
                //        string sqlans = "select * from tbl_brief_answer where id_organization=" + OID + " and id_brief_question=" + item.id_brief_question + " and status='A'";
                //        List<tbl_brief_answer> answer = db.tbl_brief_answer.SqlQuery(sqlans).ToList();
                //        temp.answers = answer;
                //        qtnList.Add(temp);
                //    }
                //    bBody.QTNLIST = qtnList;
                //}
                //itm.brf_body = bBody;
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

        //public HttpResponseMessage Get(int UID, int OID)
        //{
        //    List<APIBrief> list = new List<APIBrief>();
        //    string sqlb = "SELECT a.id_organization, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
        //    sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp LIMIT 10 ";
        //    list = new BriefModel().getAPIBriefList(sqlb);

        //    if (list != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, list);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, list);
        //    }
        //}
    }
}