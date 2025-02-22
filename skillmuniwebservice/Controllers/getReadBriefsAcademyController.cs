﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getReadBriefsAcademyController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int UID, int OID, int AcadamyTileId)
        {
            string uids = new Utility().mysqlTrim(UID.ToString());
            string oids = new Utility().mysqlTrim(OID.ToString());
            string ENCS = " ";// new Utility().mysqlTrim(ENC);
            List<tbl_brief_tile_academic_mapping> acadmap = new List<tbl_brief_tile_academic_mapping>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                acadmap = db.Database.SqlQuery<tbl_brief_tile_academic_mapping>("Select * from  tbl_brief_tile_academic_mapping  where id_academic_tile={0}", AcadamyTileId).ToList();
            }
            foreach (var itm in acadmap)
            {
                itm.BriefTileCode = db.Database.SqlQuery<string>("Select tile_code from  tbl_brief_category_tile  where id_brief_category_tile={0}", itm.id_journey_tile).FirstOrDefault();
            }

            List<BriefAPIResource> list = new List<BriefAPIResource>();
            foreach (var item in acadmap)
            {
                ENCS = item.BriefTileCode;
                tbl_brief_category_tile tile = db.tbl_brief_category_tile.Where(t => t.tile_code.ToLower() == ENCS.ToLower()).FirstOrDefault();
                if (tile != null)
                {
                    // string addition = "SELECT id_brief_category FROM tbl_brief_tile_category_mapping where id_organization=" + OID + " and id_brief_category_tile=" + tile.id_brief_category_tile + " ";
                    // string sqls = "select * from tbl_brief_user_assignment where id_user='" + uids + "' and assignment_status='S'  and id_brief_master in (SELECT id_brief_master FROM tbl_brief_master where status='A' and id_organization=" + OID + " and id_brief_category in (" + addition + "))";
                    string sqls = "select * from tbl_brief_user_assignment where id_user='" + uids + "' and assignment_status='S'  and id_brief_master in (SELECT id_brief_master FROM tbl_brief_master where status='A' and id_organization=" + OID + ")";

                    List<tbl_brief_user_assignment> check1 = db.tbl_brief_user_assignment.SqlQuery(sqls).ToList();

                    string sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
                    sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE a.status='A' and a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = '" + uids + "' AND a.id_organization = '" + oids + "' AND (published_datetime < NOW() OR scheduled_datetime < NOW())  AND a.id_brief_category IN (SELECT id_brief_category  FROM tbl_brief_tile_category_mapping WHERE id_organization = " + OID + " AND id_brief_category_tile = " + tile.id_brief_category_tile + ") ORDER BY datetimestamp DESC ";//LIMIT 50

                    list = new BriefModel().getBriefAPIResourceList(sqlb);
                    int srno = 1;
                    foreach (var itm in list)
                    {
                        itm.SRNO = srno;
                        tbl_brief_user_feedback_master feed = db.Database.SqlQuery<tbl_brief_user_feedback_master>("select * from tbl_brief_user_feedback_master where UID={0} and  id_brief_master= {1} and updated_date_time= (SELECT MAX(updated_date_time) FROM tbl_brief_user_feedback_master WHERE UID={2} and  id_brief_master ={3} );", UID, itm.id_brief_master, UID, itm.id_brief_master).FirstOrDefault();
                        if (feed != null)
                        {
                            itm.liked = feed.liked;
                            itm.disliked = feed.disliked;


                        }
                        else
                        {
                            itm.liked = 0;
                            itm.disliked = 0;
                        }
                        srno++;
                        tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == itm.id_brief_master && t.id_user == UID).FirstOrDefault();
                        if (log != null)
                        {
                            itm.RESULTSTATUS = 1;
                            itm.RESULTSCORE = Convert.ToDouble(log.brief_result);
                            tbl_brief_master master = db.tbl_brief_master.Where(t => t.id_brief_master == itm.id_brief_master).FirstOrDefault();
                            tbl_brief_master_template mTemplate = db.tbl_brief_master_template.Where(t => t.id_brief_master == itm.id_brief_master).FirstOrDefault();
                            if (mTemplate != null)
                            {
                                itm.brief_template = mTemplate.brief_template;
                            }
                            else
                            {
                                itm.brief_template = "0";
                            }
                            List<tbl_brief_master_body> mbody = db.tbl_brief_master_body.Where(t => t.id_brief_master == itm.id_brief_master).OrderBy(t => t.srno).ToList();
                            List<BriefRow> bList = new List<BriefRow>();
                            foreach (tbl_brief_master_body row in mbody)
                            {
                                BriefRow irow = new BriefRow();
                                irow.media_type = Convert.ToInt32(row.media_type);
                                irow.resouce_code = row.resouce_code;
                                irow.resource_order = mTemplate.resource_order;
                                irow.brief_destination = row.brief_destination;
                                irow.resource_number = row.resource_number;
                                irow.srno = Convert.ToInt32(row.srno);
                                irow.resource_type = Convert.ToInt32(row.resource_type);
                                irow.resouce_data = row.resouce_data;
                                irow.resouce_code = row.resouce_code;
                                irow.media_type = Convert.ToInt32(row.media_type);
                                irow.resource_mime = row.resource_mime;
                                irow.file_extension = row.file_extension;
                                irow.file_type = row.file_type;
                                bList.Add(irow);
                            }

                            itm.briefResource = bList;
                        }

                        /*--------------------------------------------------updated------------------------------------------------*/


                        /*--------------------------------------------------updated------------------------------------------------*/

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
                }
                else
                {
                }
            }
            if (list != null)
            {
                list= list.Where<BriefAPIResource>(t => t.read_status == 1).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, list);
            }
        }
    }
}
