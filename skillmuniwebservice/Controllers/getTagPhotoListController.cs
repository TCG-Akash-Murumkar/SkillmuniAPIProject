using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using m2ostnextservice.Models;
//Tagged Image list based on the user list with respect to zone
namespace m2ostnextservice.Controllers
{
    public class getTagPhotoListController : ApiController
    {
        public HttpResponseMessage Get(int UID,int OID,int Level)
        {
            getPhotoListAPI tagphotolist = new getPhotoListAPI();
            List<TaggedPhotoList> taglist = new List<TaggedPhotoList>();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                taglist = db.Database.SqlQuery<TaggedPhotoList>("select * from tbl_tag_photo_upload where ID_USER={0} and id_org={1} and id_level={2} ", UID, OID, Level).ToList();
              
                foreach (var itm in taglist)
                {
                    //TaggedPhotoList image_url = new TaggedPhotoList();
                    itm.photo_filename = WebConfigurationManager.AppSettings["TagImage"].ToString() + "/"+itm.photo_filename;
                    //taglist.Add(image_url);
                }

                if (taglist.Count > 0)
                {
                    tagphotolist.STATUS = "SUCCESS";
                    tagphotolist.usertagphotolist = taglist;
                }
                else
                {
                    tagphotolist.STATUS = "FAILURE";
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, tagphotolist);
        }
    }
}
