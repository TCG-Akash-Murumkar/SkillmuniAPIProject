using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class getSearchController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // POST api/<controller>
        public HttpResponseMessage get(string patternString, string Category, string OrganizationId, string UserId, string AccessRole)
        {
            List<tbl_content> contents = new List<tbl_content>();
            Category = "0";
            List<SearchResponce> responces = new List<SearchResponce>();
            //List<tbl_content_metadata> metadata = db.tbl_content_metadata.Where(t => t.CONTENT_METADATA.ToLower().Contains(search.patternString.ToLower())).ToList();
            patternString = patternString.Trim();
            var metasearch = db.tbl_content_metadata.SqlQuery("select * from tbl_content_metadata where LOWER(CONTENT_METADATA) like LOWER('%" + patternString + "%') ");
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

                if (!(Category == "0"))
                {
                    catAdd = " AND ID_CATEGORY=" + Category + "";
                }
                else
                {
                    catAdd = " AND ID_CATEGORY IN (select ID_CATEGORY from tbl_category where ID_ORGANIZATION=" + OrganizationId + " and STATUS='A') ";
                }
                var content = db.tbl_content.SqlQuery("SELECT * FROM tbl_content WHERE STATUS='A' " + catAdd + "  AND ID_CONTENT IN(select ID_CONTENT from tbl_content_answer where id_content_answer IN (" + mids + "))  ");
                contents = (List<tbl_content>)content.ToList();

                foreach (tbl_content item in contents)
                {
                    SearchResponce ser = new SearchResponce();
                    ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                    ser.ID_CONTENT = item.ID_CONTENT;
                    ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                    ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                    responces.Add(ser);
                }
                responces = responces.OrderBy(t => t.CONTENT_QUESTION).ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, responces);
        }
    
    }
}
