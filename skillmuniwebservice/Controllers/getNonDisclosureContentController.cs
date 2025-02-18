using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getNonDisclosureContentController : ApiController
    {
        public HttpResponseMessage Get(int oid)
        {
            List<tbl_non_disclousure_clause_content> result = new List<tbl_non_disclousure_clause_content>();

            result = new NonDisclosureLogic().getContent(oid);
            result = new NonDisclosureLogic().getDefaultContent(16,result);
         
            return Request.CreateResponse(HttpStatusCode.OK, result);


        }
    }
}
