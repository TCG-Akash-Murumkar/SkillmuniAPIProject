using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class collegelistController : ApiController
    {
        public HttpResponseMessage Get(int id_city)
        {
            ResponseBody rb = new ResponseBody();
            List<collegelistdetails> tbl = new List<collegelistdetails>();
            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {


                    tbl = db.Database.SqlQuery<collegelistdetails>("SELECT * FROM tbl_college_list where status='A' and id_city={0} ORDER BY college_name ASC",id_city).ToList();

                }
                //rb.data = tbl;
                //rb.counter = tbl.Count;
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, e);
            }




            return Request.CreateResponse(HttpStatusCode.OK, tbl);
        }
    }


    public class ResponseBody
    {
        public int counter { get; set; }
        public List<collegelistdetails> data { get; set; }
    }
}
