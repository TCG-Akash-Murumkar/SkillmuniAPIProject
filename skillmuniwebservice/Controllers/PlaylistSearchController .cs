using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using m2ostnextservice;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PlaylistSearchController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody] PlaylistSearch search)
        {
            List<tbl_content> contents = new List<tbl_content>();
            List<tbl_content> contentsSql = new List<tbl_content>();
            search.Category = "0";
            List<SearchResponce> responces = new List<SearchResponce>();
            //List<tbl_content_metadata> metadata = db.tbl_content_metadata.Where(t => t.CONTENT_METADATA.ToLower().Contains(search.patternString.ToLower())).ToList();
            search.patternString = search.patternString.Trim();
            var metasearch=db.tbl_content_metadata.SqlQuery("select * from tbl_content_metadata where LOWER(CONTENT_METADATA) like LOWER('%"+search.patternString+"%') ");
            List<tbl_content_metadata> metadata = (List<tbl_content_metadata>)metasearch.ToList();
            List<string> ids = new List<string>();
            int userId = Convert.ToInt32(search.UserId);
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
                    catAdd = " AND ID_CATEGORY=" + search.Category + "";
                }
                else
                {
                    catAdd = " AND ID_CATEGORY IN (select ID_CATEGORY from tbl_category where ID_ORGANIZATION=" + search.OrganizationId + " and STATUS='A') ";
                }
              
                var content = db.tbl_content.SqlQuery("SELECT * FROM tbl_content WHERE ID_CONTENT NOT IN(select playlist_content from tbl_myplaylist_content where id_user="+userId+" ) AND STATUS='A' " + catAdd + "  AND ID_CONTENT IN(select ID_CONTENT from tbl_content_answer where id_content_answer IN (" + mids + ")) ORDER BY ID_CONTENT_LEVEL,CONTENT_QUESTION ");
               
                contents = (List<tbl_content>)content.ToList();
              
            }
                //responces = responces.OrderBy(t => t.ID_CONTENT_LEVEL).OrderBy(t => t.CONTENT_QUESTION).ToList();
            string sql1 = "SELECT * from tbl_content WHERE UPPER(CONTENT_QUESTION) like('%" + search.patternString + "%') AND  ID_CONTENT NOT IN (select playlist_content from tbl_myplaylist_content where id_user=" + userId + " )  AND ID_CATEGORY IN (select ID_CATEGORY from tbl_category where ID_ORGANIZATION=" + search.OrganizationId + " and STATUS='A')";
            var contSearch = db.tbl_content.SqlQuery(sql1);
            contentsSql = (List<tbl_content>)contSearch.ToList();
            foreach (tbl_content item in contentsSql)
            {
                contents.Add(item);
            }
            contents = contents.Distinct().ToList();

            foreach (tbl_content item in contents)
            {
                SearchResponce ser = new SearchResponce();
                ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                ser.ID_CONTENT = item.ID_CONTENT;               
                ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                responces.Add(ser);
            }
            responces = responces.OrderBy(t => t.ID_CONTENT_LEVEL).ThenBy(t => t.CONTENT_QUESTION).ToList();
            
            return Request.CreateResponse(HttpStatusCode.OK, responces);
        }
    }
}