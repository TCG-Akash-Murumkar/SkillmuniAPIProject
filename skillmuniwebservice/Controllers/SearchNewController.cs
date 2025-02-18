using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class SearchNewController : ApiController
    {
        public HttpResponseMessage Get(string category, string organization)
        {
            List<SearchResult> searchResult = new List<SearchResult>();
            List<ContentAssociation> content = new SearchModel().GetDefaultApprovedContentId(category, organization);
            if (content != null)
            {
                foreach (ContentAssociation association in content)
                {
                    string id = association.ID_CONTENT.ToString(); ;
                    SearchResult result = new SearchResult();
                    result.QUESTION =new SearchModel().GetContentDetail(id);
                    List<string> sanswer =new SearchModel().GetContentAnswer(id);
                    string listanswers = "'" + string.Join("','", sanswer) + "'";
                    result.ANSWERS =new SearchModel().GetContentAnswerDetail(listanswers);
                    result.CATEGORY_ID = association.ID_CATEGORY.ToString();
                    result.CATEGORY_LABEL =new SearchModel().GetCategoryLabel(association.ID_CATEGORY.ToString());
                    searchResult.Add(result);
                }
                searchResult = searchResult.OrderBy(x => x.QUESTION.CONTENT_QUESTION).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, searchResult);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, searchResult);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] searchString search)
        {
            string value = "";
            List<SearchResult> searchResult = new List<SearchResult>();

            value = search.patternString.ToLower();
            List<string> pattern = value.Split(' ').ToList();

            List<string> metadata =new SearchModel().GetsearchPattern(pattern);
            string searchanswers = "'" + string.Join("','", metadata) + "'";
            List<string> contentraw = new SearchModel().GetContentId(searchanswers);
            string searchraw = "'" + string.Join("','", contentraw) + "'";
            List<ContentAssociation> content =new SearchModel().GetApprovedContentId(searchraw, search.Category, search.OrganizationId);
            if (content != null)
            {

                if (content.Count > 0)
                {
                    foreach (ContentAssociation association in content)
                    {
                        string id = association.ID_CONTENT.ToString(); ;
                        SearchResult sresult = new SearchResult();
                        sresult.QUESTION = new SearchModel().GetContentDetail(id);
                        List<string> sanswer = new SearchModel().GetContentAnswer(id);
                        string listanswers = "'" + string.Join("','", sanswer) + "'";
                        sresult.ANSWERS =new SearchModel().GetContentAnswerDetail(listanswers);
                        sresult.CATEGORY_ID = association.ID_CATEGORY.ToString();
                        sresult.CATEGORY_LABEL =new SearchModel().GetCategoryLabel(association.ID_CATEGORY.ToString());
                        searchResult.Add(sresult);
                    }
                    searchResult = searchResult.OrderBy(x => x.QUESTION.CONTENT_QUESTION).ToList();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, searchResult);
        }
    }
}
