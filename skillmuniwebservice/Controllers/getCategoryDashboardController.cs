using m2ostnextservice.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class getCategoryDashboardController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int catid, int userid, int orgid)
        {
            DateTime current = System.DateTime.Now;
            APIRESPONSE responce = new APIRESPONSE();
            CategroyDashboard dashboard = new CategroyDashboard();
            DisplayCategory catResponce = new DisplayCategory();
            tbl_category category = db.tbl_category.Where(t => t.ID_CATEGORY == catid && t.STATUS == "A").FirstOrDefault();
            if (category != null)
            {


                Category catTemp = new CategoryModel().GetCategoryValue(Convert.ToInt32(category.ID_CATEGORY));
                catTemp.ORDERID = orgid;
                catTemp.CategoryHeader = catTemp.CategoryName;
                dashboard.CATEGORY = catTemp;
                List<tbl_content> contentList = new List<tbl_content>();
                int limit = Convert.ToInt32(category.SEARCH_MAX_COUNT);
                if (limit == 0)
                {
                    limit = 30;
                }
                string userbased_sql = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + category + " AND id_user=" + userid + " AND id_organization=" + organization + ")";

                userbased_sql = " SELECT a.* FROM     tbl_content a  LEFT JOIN   tbl_content_user_assisgnment b ON a.id_content = b.id_content WHERE    a.STATUS = 'A' AND b.id_category = " + catid + " and b.id_user=" + userid + "   AND b.id_organization = " + orgid + " ";

                List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
                foreach (tbl_content item in userbasedContent)
                {
                    contentList.Add(item);
                }
                string query = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + category + " AND id_organization=" + organization + " and STATUS='A') ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                query = "SELECT  a.* FROM     tbl_content a    LEFT JOIN   tbl_content_organization_mapping b ON a.id_content = b.id_content WHERE a.STATUS = 'A' AND b.id_category =" + catid + " AND b.id_organization = " + orgid + " ORDER BY CONTENT_QUESTION  limit " + limit;

                List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                contentList.AddRange(categoryContent);
                contentList = contentList.Distinct().ToList();

                List<SearchResponce> responces = new List<SearchResponce>();
                foreach (tbl_content item in contentList)
                {
                    SearchResponce ser = new SearchResponce();
                    ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                    ser.ID_CONTENT = item.ID_CONTENT;
                    ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                    ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                    responces.Add(ser);
                }

                List<AssessmentList> assessList = new List<AssessmentList>();
                assessList = new AssessmentModel().getAssesmentList(catid, userid, orgid);

                dashboard.CONTENTLIST = responces;
                dashboard.ASSESSMENTLIST = assessList;

                responce.KEY = "SUCCESS";
                string resJson = JsonConvert.SerializeObject(dashboard);
                responce.MESSAGE = resJson;
            }
            else
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Program Does not Exist/Program Expired";
            }
            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }

    public class getCategoryFromNotificationController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int catid, int userid, int orgid)
        {
            DateTime current = System.DateTime.Now;
            APIRESPONSE responce = new APIRESPONSE();
            CategroyDashboard dashboard = new CategroyDashboard();
            DisplayCategory catResponce = new DisplayCategory();
            tbl_category category = db.tbl_category.Where(t => t.ID_CATEGORY == catid && t.STATUS == "A").FirstOrDefault();
            if (category != null)
            {

                tbl_content_program_mapping config = new tbl_content_program_mapping();
                config = db.tbl_content_program_mapping.Where(t => t.id_user == userid && t.id_category == catid).FirstOrDefault();
                if (config != null)
                {


                    if (DateTime.Compare(config.expiry_date.Value.AddDays(1), current) > 0)
                    {


                        Category catTemp = new CategoryModel().GetCategoryValue(Convert.ToInt32(category.ID_CATEGORY));
                        catTemp.ORDERID = orgid;
                        catTemp.CategoryHeader = catTemp.CategoryName;
                        dashboard.CATEGORY = catTemp;
                        List<tbl_content> contentList = new List<tbl_content>();
                        int limit = Convert.ToInt32(category.SEARCH_MAX_COUNT);
                        if (limit == 0)
                        {
                            limit = 30;
                        }
                        string userbased_sql = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + category + " AND id_user=" + userid + " AND id_organization=" + organization + ")";

                        userbased_sql = " SELECT a.* FROM     tbl_content a  LEFT JOIN   tbl_content_user_assisgnment b ON a.id_content = b.id_content WHERE    a.STATUS = 'A' AND b.id_category = " + catid + " and b.id_user=" + userid + "   AND b.id_organization = " + orgid + " ";

                        List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
                        foreach (tbl_content item in userbasedContent)
                        {
                            contentList.Add(item);
                        }
                        string query = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + category + " AND id_organization=" + organization + " and STATUS='A') ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                        query = "SELECT  a.* FROM     tbl_content a    LEFT JOIN   tbl_content_organization_mapping b ON a.id_content = b.id_content WHERE a.STATUS = 'A' AND b.id_category =" + catid + " AND b.id_organization = " + orgid + " ORDER BY CONTENT_QUESTION  limit " + limit;

                        List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                        contentList.AddRange(categoryContent);
                        contentList = contentList.Distinct().ToList();

                        List<SearchResponce> responces = new List<SearchResponce>();
                        foreach (tbl_content item in contentList)
                        {
                            SearchResponce ser = new SearchResponce();
                            ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                            ser.ID_CONTENT = item.ID_CONTENT;
                            ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                            ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                            responces.Add(ser);
                        }

                        List<AssessmentList> assessList = new List<AssessmentList>();
                        assessList = new AssessmentModel().getAssesmentList(catid, userid, orgid);

                        dashboard.CONTENTLIST = responces;
                        dashboard.ASSESSMENTLIST = assessList;

                        responce.KEY = "SUCCESS";
                        string resJson = JsonConvert.SerializeObject(dashboard);
                        responce.MESSAGE = resJson;

                    }
                    else
                    {
                        responce.KEY = "FAILURE";
                        responce.MESSAGE = "Program has expired on " + config.expiry_date.Value.ToShortDateString();
                    }
                }
                else
                {
                    responce.KEY = "FAILURE";
                    responce.MESSAGE = "Program Does not Exist/Program Expired";
                }
            }
            else
            {
                responce.KEY = "FAILURE";
                responce.MESSAGE = "Program Does not Exist/Program Expired";
            }
            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }



}
