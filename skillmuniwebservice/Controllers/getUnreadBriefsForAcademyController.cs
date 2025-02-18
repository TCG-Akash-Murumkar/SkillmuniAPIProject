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
    public class getUnreadBriefsForAcademyController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(int UID, int OID, int AcadamyTileId)//string ENC
        {
            string uids = new Utility().mysqlTrim(UID.ToString());
            string oids = new Utility().mysqlTrim(OID.ToString());
            //string ENCS = new Utility().mysqlTrim(ENC);
            tbl_brief_tile_level_brief_restriction rest = new tbl_brief_tile_level_brief_restriction();
            tbl_academy_level_brief_restriction acres = new tbl_academy_level_brief_restriction();
            List<tbl_brief_tile_academic_mapping> acadmap = new List<tbl_brief_tile_academic_mapping>();
            List<tbl_restriction_user_log> lg = new List<tbl_restriction_user_log>();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                acadmap = db.Database.SqlQuery<tbl_brief_tile_academic_mapping>("Select * from  tbl_brief_tile_academic_mapping  where id_academic_tile={0}", AcadamyTileId).ToList();
                acres = db.Database.SqlQuery<tbl_academy_level_brief_restriction>("select * from tbl_academy_level_brief_restriction where id_academy = {0}", AcadamyTileId).FirstOrDefault();
                rest = db.Database.SqlQuery<tbl_brief_tile_level_brief_restriction>("select * from tbl_brief_tile_level_brief_restriction where  id_academy={0}", AcadamyTileId).FirstOrDefault();

                if (acres != null)
                {
                    if (rest.time == 1)
                    {
                        DateTime tdy = DateTime.Now.Date;

                        lg = db.Database.SqlQuery<tbl_restriction_user_log>("select * from tbl_restriction_user_log where UID = {0} and  date(updated_date_time)={1} and id_academy={2}", UID, tdy, AcadamyTileId).ToList();
                    }
                    else if (rest.time == 2)
                    {
                        int tdy = DateTime.Now.Hour;
                        DateTime dt = DateTime.Now.Date;
                        lg = db.Database.SqlQuery<tbl_restriction_user_log>("select * from tbl_restriction_user_log where UID = {0}  and EXTRACT(HOUR  FROM updated_date_time)={1} and  date(updated_date_time)={2} and id_academy={3}", UID, tdy, dt, AcadamyTileId).ToList();


                    }
                }

            }

            foreach (var itm in acadmap)
            {
                itm.BriefTileCode = db.Database.SqlQuery<string>("Select tile_code from  tbl_brief_category_tile  where id_brief_category_tile={0}", itm.id_journey_tile).FirstOrDefault();
            }
            //List<BriefAPIResource> reslist = new List<BriefAPIResource>();
            List<BriefAPIResource> list = new List<BriefAPIResource>();
            // foreach (var ob in acadmap)
            // {
            // string ENCS = ob.BriefTileCode;
            //tbl_brief_category_tile tile = db.tbl_brief_category_tile.Where(t => t.tile_code.ToLower() == ENCS.ToLower()).FirstOrDefault();
            // if (tile != null)
            // {
            // string addition = "SELECT id_brief_category FROM tbl_brief_tile_category_mapping where id_organization=" + OID + " and id_brief_category_tile=" + tile.id_brief_category_tile + " ";
            // string sqls = "select * from tbl_brief_user_assignment where id_user='" + uids + "' and assignment_status='S'  and id_brief_master in (SELECT id_brief_master FROM tbl_brief_master where status='A' and id_organization=" + OID + " and id_brief_category in (" + addition + "))";
            string sqls = "select * from tbl_brief_user_assignment where id_user='" + uids + "' and assignment_status='S'  and id_brief_master in (SELECT id_brief_master FROM tbl_brief_master where status='A' and id_organization=" + OID + ")";



            //string sqlb = "SELECT a.id_organization,question_count, brief_title, brief_code, brief_description, CASE WHEN scheduled_status = 'NA' THEN published_datetime WHEN published_status = 'NA' THEN scheduled_datetime ELSE NULL END datetimestamp, CASE WHEN scheduled_status = 'NA' THEN 'P' WHEN published_status = 'NA' THEN 'S' ELSE NULL END scheduled_type, a.override_dnd, a.id_brief_master, b.id_user, a.is_add_question is_question_attached, c.action_status, c.read_status, d.brief_category, e.brief_subcategory, d.id_brief_category, e.id_brief_subcategory ";
            //sqlb += " FROM tbl_brief_master a, tbl_brief_user_assignment b, tbl_brief_read_status c, tbl_brief_category d, tbl_brief_subcategory e WHERE a.status='A' and a.id_brief_master = b.id_brief_master AND a.id_brief_master = c.id_brief_master AND b.id_user = c.id_user AND a.id_brief_category = d.id_brief_category AND a.id_brief_sub_category = e.id_brief_subcategory AND a.id_brief_sub_category = e.id_brief_subcategory AND b.id_user = '" + uids + "' AND a.id_organization = '" + oids + "' AND (published_datetime < NOW() OR scheduled_datetime < NOW())  AND a.id_brief_category IN (SELECT id_brief_category  FROM tbl_brief_tile_category_mapping WHERE id_organization = " + OID + " AND id_brief_category_tile = " + tile.id_brief_category_tile + ") ORDER BY datetimestamp DESC ";//LIMIT 50

            //new BriefModel().getBriefAPIResourceListCus(sqlb, list);
            List<tbl_brief_master> mas = new List<tbl_brief_master>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                acadmap = db.Database.SqlQuery<tbl_brief_tile_academic_mapping>("Select * from  tbl_brief_tile_academic_mapping  where id_academic_tile={0}", AcadamyTileId).ToList();

                foreach (var itm in acadmap)
                {
                    itm.BriefTileCode = db.Database.SqlQuery<string>("Select tile_code from  tbl_brief_category_tile  where id_brief_category_tile={0}", itm.id_journey_tile).FirstOrDefault();
                }
                foreach (var itm in acadmap)
                {
                    int idcattile = db.Database.SqlQuery<int>("SELECT id_brief_category_tile FROM tbl_brief_category_tile where tile_code={0}", itm.BriefTileCode).FirstOrDefault();
                    List<tbl_brief_master> idcats = db.Database.SqlQuery<tbl_brief_master>("SELECT * FROM tbl_brief_category_tile INNER JOIN tbl_brief_tile_category_mapping ON tbl_brief_category_tile.id_brief_category_tile = tbl_brief_tile_category_mapping.id_brief_category_tile where tbl_brief_category_tile.id_organization={0} and tbl_brief_tile_category_mapping.id_brief_category_tile={1} ;", OID, idcattile).ToList();
                    foreach (var idcat in idcats)
                    {
                        if (idcat.id_brief_category != 0)
                        {
                            List<tbl_brief_master> instob = new BriefModel().getBriefList("select * from tbl_brief_master where status='A' and id_brief_category=" + idcat.id_brief_category, OID);
                            mas.AddRange(instob);

                        }


                    }

                }


            }

            int srno = 1;
            foreach (var itm in mas)
            {
                BriefAPIResource test = new BriefAPIResource();
                test.SRNO = srno;
                test.brief_title = itm.brief_title;
                test.brief_description = itm.brief_description;
                test.id_brief_category = itm.id_brief_category;
                test.id_brief_master = itm.id_brief_master;
                test.id_organization = OID;
                test.id_user = UID;
                test.brief_code = itm.brief_code;
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    List<tbl_brief_question_mapping> qtnls = db.Database.SqlQuery<tbl_brief_question_mapping>("SELECT * FROM tbl_brief_question_mapping where id_brief_master={0}", test.id_brief_master).ToList();
                    if (qtnls.Count > 0)
                    {
                        test.question_count = qtnls.Count;
                        test.is_question_attached = 1;

                    }


                    test.brief_category = db.Database.SqlQuery<string>("select brief_category from tbl_brief_category where id_brief_category={0}", test.id_brief_category).FirstOrDefault();

                    tbl_brief_user_feedback_master feed = db.Database.SqlQuery<tbl_brief_user_feedback_master>("select * from tbl_brief_user_feedback_master where UID={0} and  id_brief_master= {1} and updated_date_time= (SELECT MAX(updated_date_time) FROM tbl_brief_user_feedback_master WHERE UID={2} and  id_brief_master ={3} );", UID, itm.id_brief_master, UID, itm.id_brief_master).FirstOrDefault();
                    if (feed != null)
                    {
                        test.liked = feed.liked;
                        test.disliked = feed.disliked;


                    }
                    else
                    {
                        test.liked = 0;
                        test.disliked = 0;
                    }

                }
                srno++;
                tbl_brief_log log = db.tbl_brief_log.Where(t => t.attempt_no == 1 && t.id_brief_master == itm.id_brief_master && t.id_user == UID).FirstOrDefault();
                if (log != null)//|| rd != null)
                {
                    test.read_status = 1;
                    test.RESULTSTATUS = 1;
                    test.RESULTSCORE = Convert.ToDouble(log.brief_result);
                }
                else
                {
                    test.read_status = 0;
                    test.RESULTSTATUS = 0;
                    test.RESULTSCORE = 0;
                }

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_brief_m2ost_category_mapping inst_obj = new tbl_brief_m2ost_category_mapping();
                    test.cat_mapping = db.Database.SqlQuery<tbl_brief_m2ost_category_mapping>("select * from tbl_brief_m2ost_category_mapping where id_brief={0}", itm.id_brief_master).FirstOrDefault();
                    if (test.cat_mapping != null)
                    {
                        if (test.cat_mapping.type == 2)
                        {
                            using (M2ostCatDbContext dbcat = new M2ostCatDbContext())
                            {
                                test.cat_mapping.CATEGORYNAME = dbcat.Database.SqlQuery<string>("select CATEGORYNAME from tbl_category where ID_CATEGORY={0} ", test.cat_mapping.id_category).FirstOrDefault();
                                test.cat_mapping.Heading_title = dbcat.Database.SqlQuery<string>("select Heading_title from tbl_category_heading where id_category_heading={0} ", test.cat_mapping.id_category_heading).FirstOrDefault();
                                test.cat_mapping.CategoryImage = ConfigurationManager.AppSettings["CATIm"].ToString() + dbcat.Database.SqlQuery<string>("select IMAGE_PATH from tbl_category where ID_CATEGORY={0} ", test.cat_mapping.id_category).FirstOrDefault();

                            }
                        }
                    }



                }

                /*--------------------------------------------------updated------------------------------------------------*/
                tbl_brief_master master = db.tbl_brief_master.Where(t => t.id_brief_master == itm.id_brief_master).FirstOrDefault();
                tbl_brief_master_template mTemplate = db.tbl_brief_master_template.Where(t => t.id_brief_master == itm.id_brief_master).FirstOrDefault();
                if (mTemplate != null)
                {
                    test.brief_template = mTemplate.brief_template;
                }
                else
                {
                    test.brief_template = "0";
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
                test.briefResource = bList;
                list.Add(test);
            }
            //  }

            // }


            List<BriefAPIResource> unreadlist = new List<BriefAPIResource>();
            List<BriefAPIResource> unreadlistres = new List<BriefAPIResource>();
            if (list != null)
            {
                unreadlist = list.Where<BriefAPIResource>(t => t.read_status == 0).ToList();
                //unreadlist = unreadlist.Take(5);
                if (acres != null)
                {

                    int cn = acres.brief_count - lg.Count;
                    unreadlistres = unreadlist.Take(cn).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, unreadlistres);


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, unreadlist);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, unreadlist);
            }


        }
    }
}
