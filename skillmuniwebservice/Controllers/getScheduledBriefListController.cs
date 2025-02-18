﻿using System;
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
    public class getScheduledBriefListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID)
        {
            //   check();

            string uids = new Utility().mysqlTrim(UID.ToString());
            string oids = new Utility().mysqlTrim(OID.ToString());

            List<APIBrief> list = new List<APIBrief>();
            //    string sqlb1 = "SELECT a.id_organization, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user ";
            //    sqlb1 += " FROM tbl_brief_master a, tbl_brief_user_assignment b WHERE a.id_brief_master = b.id_brief_master AND b.id_user = " + uids + " AND a.id_organization = " + oids + " AND (published_datetime > NOW() OR scheduled_datetime > NOW()) ORDER BY datetimestamp DESC LIMIT 15 ";

            string sqls = "select * from tbl_brief_user_assignment where id_user='" + uids + "'  and assignment_status='S' and  id_brief_master in (SELECT id_brief_master FROM tbl_brief_master where status='A' and id_organization=" + OID + ")";
            List<tbl_brief_user_assignment> check1 = db.tbl_brief_user_assignment.SqlQuery(sqls).ToList();
            foreach (tbl_brief_user_assignment item in check1)
            {
                tbl_brief_read_status read = db.tbl_brief_read_status.Where(t => t.id_brief_master == item.id_brief_master && t.id_user == item.id_user).FirstOrDefault();
                if (read == null)
                {
                    tbl_brief_read_status rst = new tbl_brief_read_status();
                    rst.id_brief_master = item.id_brief_master;
                    rst.id_user = item.id_user;
                    rst.id_organization = OID;
                    rst.read_status = 0;
                    rst.action_status = 0;
                    rst.id_organization = OID;
                    rst.status = "A";
                    rst.updated_date_time = DateTime.Now;
                    db.tbl_brief_read_status.Add(rst);
                    db.SaveChanges();
                }
            }

            string sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
            sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE a.status='A' and  a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = '" + uids + "' AND a.id_organization = '" + oids + "' AND (published_datetime < NOW() OR scheduled_datetime < NOW()) ORDER BY datetimestamp DESC LIMIT 50";

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

                /*  tbl_brief_user_assignment uassign = db.tbl_brief_user_assignment.Where(t => t.id_brief_master == itm.id_brief_master && t.id_user == UID).FirstOrDefault();
                  if (uassign != null)
                  {
                      if (uassign.assignment_status == "S")
                      {
                          uassign.assignment_status = "R";
                          db.SaveChanges();
                      }
                  }*/
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

        public void check()
        {
            string sqls = "select * from tbl_brief_user_assignment where  assignment_status='S'";

            List<tbl_brief_user_assignment> check1 = db.tbl_brief_user_assignment.SqlQuery(sqls).ToList();
            foreach (tbl_brief_user_assignment item in check1)
            {
                tbl_brief_read_status read = db.tbl_brief_read_status.Where(t => t.id_brief_master == item.id_brief_master && t.id_user == item.id_user).FirstOrDefault();
                if (read == null)
                {
                    tbl_brief_read_status rst = new tbl_brief_read_status();
                    rst.id_brief_master = item.id_brief_master;
                    rst.id_user = item.id_user;

                    rst.read_status = 0;
                    rst.action_status = 0;
                    rst.status = "A";
                    rst.updated_date_time = DateTime.Now;
                    //  db.tbl_brief_read_status.Add(rst);
                    //  db.SaveChanges();
                }
            }
        }
    }
}