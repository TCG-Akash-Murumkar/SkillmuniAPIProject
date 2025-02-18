using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class getBriefReadStatusController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string brf, int UID, int OID)
        {
       
            BriefReadStatus read = new BriefReadStatus();
            read.Assessment = 0;
            read.BookMark = 0;
            read.BrfCode = brf;
            List<APIBrief> list = new List<APIBrief>();
            BriefResource bBody = new BriefResource();

            brf = brf.ToLower().Trim();
      
            string sqlb = "SELECT a.id_organization,a.question_count,a.id_brief_category,a.id_brief_sub_category, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE   LOWER(brief_code) = '" + brf + "' AND a.id_brief_master = b.id_brief_master AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) LIMIT 10 ";

            sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE  LOWER(brief_code) = '" + brf + "' AND a.status='A' AND  a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = " + UID + " AND a.id_organization = " + OID + " AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp DESC LIMIT 50";

            //list = new BriefModel().getAPIBriefList(sqlb);
            tbl_brief_master master = db.tbl_brief_master.Where(t => t.brief_code == brf).FirstOrDefault();

            if (master.id_brief_master > 0)
            {
                bBody = new BriefResource();
                //APIBrief brief = list[0];
                //tbl_brief_master master = db.tbl_brief_master.Where(t => t.id_brief_master == brief.id_brief_master).FirstOrDefault();
               

                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == master.id_brief_master && t.id_user == UID).FirstOrDefault();

                if (log != null)
                {
                  
                    read.Assessment = 1;
                }

                tbl_brief_read_status rstatus = db.tbl_brief_read_status.Where(t => t.id_user == UID && t.id_brief_master == master.id_brief_master).FirstOrDefault();

                if (rstatus != null)
                {
                    if (rstatus.read_status == 0)
                    {

                        read.BookMark = 0;
                    }
                    else
                    {
                        read.BookMark = 1;

                    }
                   
                }
                else
                {
                    rstatus = new tbl_brief_read_status();
                    rstatus.id_user = UID;
                    rstatus.id_organization = OID;
                    rstatus.id_brief_master = master.id_brief_master;
                    rstatus.read_status = 0;
                    rstatus.status = "A";
                    rstatus.action_dateime = null;
                    rstatus.action_status = 0;
                    rstatus.read_datetime = DateTime.Now;
                    rstatus.updated_date_time = DateTime.Now;
                    db.tbl_brief_read_status.Add(rstatus);
                    db.SaveChanges();
                    read.BookMark = 0;
                }




            }


            return Request.CreateResponse(HttpStatusCode.OK, read);
           
        }

     

    }
}
