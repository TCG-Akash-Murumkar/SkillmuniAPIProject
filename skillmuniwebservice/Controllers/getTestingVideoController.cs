using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getTestingVideoController : ApiController
    {

        public HttpResponseMessage Get(string vid)
        {
            db_m2ostEntities db = new db_m2ostEntities();
            List<videoresponse> res = new List<videoresponse>();

            //tbl_version_control versions = db.tbl_version_control.Where(t => t.id_version_control > 0).FirstOrDefault();
            int j = 1;
            for (int i = 0; i < 7; i++)
            {
                videoresponse obj = new videoresponse();
                obj.baseurl= ConfigurationManager.AppSettings["VideoBase"].ToString();
                obj.filename= ConfigurationManager.AppSettings["VideoFile"+j].ToString();
                j++;
                res.Add(obj);

            }
           
            //res[0].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[0].filename = ConfigurationManager.AppSettings["VideoFile1"].ToString();

            //res[1].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[1].filename = ConfigurationManager.AppSettings["VideoFile2"].ToString();

            //res[2].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[2].filename = ConfigurationManager.AppSettings["VideoFile3"].ToString();

            //res[3].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[3].filename = ConfigurationManager.AppSettings["VideoFile4"].ToString();

            //res[4].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[4].filename = ConfigurationManager.AppSettings["VideoFile5"].ToString();

            //res[5].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[5].filename = ConfigurationManager.AppSettings["VideoFile6"].ToString();

            //res[6].baseurl = ConfigurationManager.AppSettings["VideoBase"].ToString();
            //res[6].filename = ConfigurationManager.AppSettings["VideoFile7"].ToString();

            return Request.CreateResponse(HttpStatusCode.OK, res);
           
        }
    }
}
