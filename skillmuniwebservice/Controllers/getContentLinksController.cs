using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
{
    public class getContentLinksController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(int cid, int oid, int uid)
        {
            List<tbl_content_type_link> links = new List<tbl_content_type_link>();
            List<SatisfiedResult> satisfiedResult = new List<SatisfiedResult>();

            List<tbl_content> contentList = new ContentModel().getContentListFromCategory(cid, oid, uid);
            foreach (tbl_content content in contentList)
            {
                if (content != null)
                {
                    tbl_content_answer answer = db.tbl_content_answer.Where(t => t.ID_CONTENT == content.ID_CONTENT).FirstOrDefault();
                    if (answer != null)
                    {
                        links = db.tbl_content_type_link.Where(t => t.ID_CONTENT_ANSWER == answer.ID_CONTENT_ANSWER).ToList();
                        foreach (tbl_content_type_link item in links)
                        {
                            SatisfiedResult result = new SatisfiedResult();
                            result.PATH = item.LINK_VALUE; //Convert.ToString(reader["LINK_VALUE"]);
                            result.TYPE = item.ID_CONTENT_TYPE.ToString();// reader["ID_CONTENT_TYPE"].ToString();
                            result.TITLE = item.DESCRIPTION;// reader["DESCRIPTION"].ToString();
                            satisfiedResult.Add(result);
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, satisfiedResult);
        }

    }
}
