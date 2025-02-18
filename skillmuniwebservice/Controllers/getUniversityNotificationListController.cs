using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getUniversityNotificationListController : ApiController
    {
        public HttpResponseMessage Get(int id_user)
        {
            UniversityNotification res = new UniversityNotification();


            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    res.content_notification = db.Database.SqlQuery<tbl_content_notification_master>("SELECT * FROM tbl_content_notification_master INNER JOIN tbl_brief_category_tile ON tbl_content_notification_master.id_brief_category_tile = tbl_brief_category_tile.id_brief_category_tile where tbl_content_notification_master.status={0} group by tbl_content_notification_master.updated_datetime desc", "A").ToList();
                    res.general_notification = db.Database.SqlQuery<tbl_url_notification_master>("select   * from tbl_url_notification_master where status={0} group by updated_datetime desc", "A").ToList();
                    foreach (var itm in res.content_notification)
                    {
                        string academictilename = db.Database.SqlQuery<string>("select tile_name from tbl_academic_tiles where id_academic_tile={0}",itm.id_academic_tile).FirstOrDefault();
                        string notificat = itm.notification_message + " You can find the brief in " + academictilename + " under " + itm.category_tile;
                        itm.message = notificat;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;

            }

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}
