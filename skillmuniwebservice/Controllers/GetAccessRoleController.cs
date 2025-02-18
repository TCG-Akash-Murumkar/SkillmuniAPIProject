using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using m2ostnextservice;

namespace m2ostnextservice.Controllers
{
    public class GetAccessRoleController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(int OID)
        {
            List<tbl_csst_role> roles = db.tbl_csst_role.Where(t => t.id_organization == OID).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, roles);
        }
    }
}
