using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class IPhoneCategoryController : ApiController
    {
        public HttpResponseMessage Get(string catid,int oid,int uid)
        {
            CategoryResponce categoryList = new CategoryModel().GetCategory(catid);
            if (categoryList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, categoryList);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, categoryList);
            }
        }
    }
}
