using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostFeedbackController : ApiController
    {
        public HttpResponseMessage Post([FromBody]FeedbackPost Feed)
        {
            FeedbackResponse res = new FeedbackResponse();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Feed.id_feedback = db.Database.SqlQuery<int>("insert into tbl_feedback_master(Issues,Suggestions,Content,UI,Description,MediaFlag,updated_date_time,Contact,UID,OID) values({0}, {1},{2},{3},{4},{5},{6},{7},{8},{9});SELECT LAST_INSERT_ID();", Feed.Issues, Feed.Suggestions, Feed.Content, Feed.UI, Feed.Description, Feed.MediaFlag, DateTime.Now, Feed.Contact, Feed.UID, Feed.OID).FirstOrDefault();

                }
                int i = 1;
                if (Feed.MediaFlag == 1)
                {
                    foreach (var itm in Feed.Media)
                    {

                        byte[] ImageBase = Convert.FromBase64String(itm.media);

                        File.WriteAllBytes(@"C:\SULAPIProduction\Content\Feedback\" + Feed.id_feedback + "_" + i + "." + itm.extension, ImageBase);
                        itm.media = Feed.id_feedback +"_"+ i + "." + itm.extension;
                        using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                        {
                            db.Database.ExecuteSqlCommand("Insert into  tbl_feedback_media (id_feedback,media,extension,updated_time) values({0},{1},{2},{3})", Feed.id_feedback, itm.media, itm.extension, DateTime.Now);
                        }
                        i++;
                    }


                }

            }
            catch (Exception e)
            {
                //new Utility().eventLog(controllerName + " : " + e.Message);
                //new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                //new Utility().eventLog("Additional Details" + " : " + e.Message);
                res.Result = "Failed";
                return Request.CreateResponse(HttpStatusCode.OK, res);
                //throw e;
            }
            finally
            {
                res.Result = "Success";
            }
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }


    }
}
