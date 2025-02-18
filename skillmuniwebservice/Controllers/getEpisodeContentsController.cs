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
    public class getEpisodeContentsController : ApiController
    {



        public HttpResponseMessage Get(int UID, int OID)
        {
            List<EpisodeResponse> res = new List<EpisodeResponse>();

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                res = db.Database.SqlQuery<EpisodeResponse>("select id_brief_master,brief_title,episode_sequence from tbl_brief_master where id_organization={0} and status='A'",OID).ToList();
                foreach (var itm in res)
                {
                    itm.content = db.Database.SqlQuery<EpisodeContent>("select resouce_data,brief_destination from tbl_brief_master_body where id_brief_master={0}", itm.id_brief_master).ToList();
                    foreach (var obj in itm.content)
                    {
                        obj.brief_destination = ConfigurationManager.AppSettings["BriefBaseURL"].ToString() + obj.brief_destination;

                    }
                    //itm.content = ConfigurationManager.AppSettings["BriefBaseURL"].ToString()+ db.Database.SqlQuery<string>("select brief_destination from tbl_brief_master_body where id_brief_master={0}", itm.id_brief_master).FirstOrDefault(); 

                }


            }



            return Request.CreateResponse(HttpStatusCode.OK, res);

        }

    }
}
