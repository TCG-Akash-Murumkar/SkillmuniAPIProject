using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class PostAssessmentLogController : ApiController
    {

        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post(tbl_user_level_log level)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string Result = "";
            try
            {
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    db.Database.ExecuteSqlCommand("Insert into tbl_user_level_log (id_user,level,attempt_no,score,bonus,total_score,updated_date_time,is_qualified,status,userid) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",level.id_user,level.level,level.attempt_no,level.score,level.bonus,level.total_score,DateTime.Now,level.is_qualified,"A",level.userid);

                    foreach (var itm in level.assessment)
                    {
                        db.Database.ExecuteSqlCommand("Insert into tbl_user_assessment_log (id_user,level,attempt_no,id_question,id_answer,id_user_answer,status,updated_date_time,is_right) values({0},{1},{2},{3},{4},{5},{6},{7},{8})", level.id_user, level.level, level.attempt_no, itm.id_question, itm.id_answer, itm.id_user_answer, "A", DateTime.Now, itm.is_right);

                    }
                }


                
                
            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
                Result = "Failed";
            }
            finally
            {
                Result = "Success";
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

    }
}
