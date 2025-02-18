using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class SearchController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(string category, string organization, string UID)
        {
            SearchGetResponce getResponce = new SearchGetResponce();
            int userid = Convert.ToInt32(UID);
            int OID = Convert.ToInt32(organization);
            int cid = Convert.ToInt32(category);
            List<tbl_content> contentList = new List<tbl_content>();

            if (category.Equals("0"))
            {
                return Request.CreateResponse(HttpStatusCode.OK, getResponce);
                //query = "SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_organization=" + organization + " and STATUS='A') ORDER BY CONTENT_QUESTION LIMIT 30";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
            }
            else
            {

                tbl_category catData = db.tbl_category.Where(t => t.ID_CATEGORY == cid && t.STATUS == "A").FirstOrDefault();
                if (catData != null)
                {

                    int limit = Convert.ToInt32(catData.SEARCH_MAX_COUNT);
                    if (limit == 0)
                    {
                        limit = 30;
                    }
                    string userbased_sql = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + category + " AND id_user=" + userid + " AND id_organization=" + organization + ")";

                    userbased_sql = " SELECT a.* FROM     tbl_content a        LEFT JOIN   tbl_content_user_assisgnment b ON a.id_content = b.id_content WHERE    a.STATUS = 'A' AND b.id_category = " + category + " and b.id_user=" + userid + "   AND b.id_organization = " + organization + " ";

                    List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
                    foreach (tbl_content item in userbasedContent)
                    {
                        contentList.Add(item);
                    }
                    string query = "";//"SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + category + " AND id_organization=" + organization + " and STATUS='A') ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                    query = "SELECT  a.* FROM     tbl_content a    LEFT JOIN   tbl_content_organization_mapping b ON a.id_content = b.id_content WHERE a.STATUS = 'A' AND b.id_category =" + category + " AND b.id_organization = " + organization + " ORDER BY CONTENT_QUESTION  limit "+limit;
                    
                    List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                    //foreach (tbl_content item in categoryContent)
                    //{
                    //    contentList.Add(item);
                    //}

                    contentList.AddRange(categoryContent);
                    contentList = contentList.Distinct().OrderBy(t => t.ID_CONTENT_LEVEL).ThenBy(t => t.CONTENT_QUESTION).ToList();

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
                    assessList = new AssessmentModel().getAssesmentList(cid, userid, OID);                 
                    
                    getResponce.searchResponce = responces;
                    getResponce.assessmentResponce = assessList;

                    return Request.CreateResponse(HttpStatusCode.OK, getResponce);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getResponce);
                }
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] searchString search)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //new Utility().eventLog(controllerName + " : " + JsonConvert.SerializeObject(search));

            List<tbl_content> contentList = new List<tbl_content>();

            List<SearchResponce> responces = new List<SearchResponce>();
            //List<tbl_content_metadata> metadata = db.tbl_content_metadata.Where(t => t.CONTENT_METADATA.ToLower().Contains(search.patternString.ToLower())).ToList();
            search.patternString = search.patternString.Trim();
            var metasearch = db.tbl_content_metadata.SqlQuery("select * from tbl_content_metadata where LOWER(CONTENT_METADATA) like LOWER('%" + search.patternString + "%') ");
            List<tbl_content_metadata> metadata = (List<tbl_content_metadata>)metasearch.ToList();
            List<string> ids = new List<string>();
            if (metadata.Count > 0)
            {
                foreach (tbl_content_metadata meta in metadata)
                {
                    ids.Add(meta.ID_CONTENT_ANSWER.ToString());
                }

                string mids = String.Join(",", ids);
                string catAdd = "";

                if (!(search.Category == "0"))
                {
                    catAdd = " AND id_content IN (select id_content from tbl_content_organization_mapping where id_organization=" + search.OrganizationId + " and STATUS='A') ";
                }
                else
                {
                    catAdd = " AND id_content IN (select id_content from tbl_content_organization_mapping where id_organization=" + search.OrganizationId + " and STATUS='A') ";
                }

                string cSql = "SELECT * FROM tbl_content WHERE STATUS='A' " + catAdd + "  AND ( LOWER(CONTENT_QUESTION) like LOWER('%" + search.patternString + "%')  OR  ID_CONTENT IN(select ID_CONTENT from tbl_content_answer where id_content_answer IN (" + mids + "))  )";
                var content = db.tbl_content.SqlQuery(cSql);
                contentList = (List<tbl_content>)content.ToList();
            }
            string catSqlStr = "";
            if (search.Category != "0")
            {
                catSqlStr = " and id_category=" + search.Category + " ";
            }


            string program_sql = "SELECT * FROM tbl_content WHERE STATUS='A' AND  LOWER(CONTENT_QUESTION) like LOWER('%" + search.patternString + "%')  AND id_content IN (id_content IN (select id_content from tbl_content_organization_mapping where id_category in (select id_category from tbl_content_program_mapping where id_user=" + search.UserId + " AND id_organization=" + search.OrganizationId + "))) ";
            List<tbl_content> programContent = db.tbl_content.SqlQuery(program_sql).ToList();
            foreach (tbl_content item in programContent)
            {
                contentList.Add(item);
            }

            string userbased_sql = "SELECT * FROM tbl_content WHERE STATUS='A' AND LOWER(CONTENT_QUESTION) like LOWER('%" + search.patternString + "%') AND id_content IN (select id_content from tbl_content_user_assisgnment where id_user=" + search.UserId + " AND id_organization=" + search.OrganizationId + ")";
            List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
            foreach (tbl_content item in userbasedContent)
            {
                contentList.Add(item);
            }

            contentList = contentList.Distinct().ToList();
            foreach (tbl_content item in contentList)
            {
                SearchResponce ser = new SearchResponce();
                ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                ser.ID_CONTENT = item.ID_CONTENT;
                ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                responces.Add(ser);
            }
            responces = responces.OrderBy(t => t.CONTENT_QUESTION).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, responces);
        }

    }
}