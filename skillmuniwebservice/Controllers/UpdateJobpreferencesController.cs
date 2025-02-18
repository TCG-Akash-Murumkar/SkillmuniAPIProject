using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateJobpreferencesController : ApiController
    {


        public HttpResponseMessage Post([FromBody]preferencemodel obj)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string response ="";
            try
            {

                using (JobDbContext db = new JobDbContext())
                {
                    tbl_user_job_preferences pref = new tbl_user_job_preferences();
                    pref = db.Database.SqlQuery<tbl_user_job_preferences>("select * from  tbl_user_job_preferences where id_user={0} ", obj.id_user).FirstOrDefault();

                    if (pref == null)
                    {


                        db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences (experience_years,experience_months,status,updated_date_time,id_user) values({0},{1},{2},{3},{4})",obj.experience_years,obj.experience_months,"A",DateTime.Now,obj.id_user);
                        string[] skill= obj.skill.Split(',');
                        string[] category= obj.category.Split(',');
                        string[] location = obj.id_location.Split(',');
                        string[] jobtype = obj.job_type.Split(',');
                        string[] jobind = obj.industry_str.Split(',');
                        string[] jobrole = obj.role_str.Split(',');
                        foreach (var itm in skill)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_skill (id_user,skill,status,updated_date_time) values({0},{1},{2},{3})", obj.id_user, itm, "A", DateTime.Now);

                        }
                        foreach (var itm in category)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_category (id_category,id_user,status,updated_date_time) values({0},{1},{2},{3})",  itm, obj.id_user, "A", DateTime.Now);

                        }
                        foreach (var itm in location)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_location (id_location,id_user,status,updated_date_time) values({0},{1},{2},{3})", itm, obj.id_user, "A", DateTime.Now);

                        }
                        foreach (var itm in jobtype)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_job_type (id_user,job_type,status,updated_date_time) values({0},{1},{2},{3})", obj.id_user, itm ,"A", DateTime.Now);

                        }
                        foreach (var itm in jobind)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_ce_evaluation_jobindustry_user_mapping (id_ce_evaluation_jobindustry,id_user,date_time_stamp,status,updated_date_time) values({0},{1},{2},{3},{4})", itm, obj.id_user, DateTime.Now, "A", DateTime.Now);

                        }
                        foreach (var itm in jobrole)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_ce_evaluation_jobrole_user_mapping (id_ce_evaluation_jobrole,id_user,dated_time_stamp,status,updated_date_time) values({0},{1},{2},{3},{4})", itm, obj.id_user, DateTime.Now, "A", DateTime.Now);

                        }




                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("update  tbl_user_job_preferences set experience_years={0},experience_months={1},status={2},updated_date_time={3} where id_user={4}",  obj.experience_years, obj.experience_months, "A", DateTime.Now, obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_user_job_preferences_skill where id_user={0}", obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_user_job_preferences_category where id_user={0}", obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_user_job_preferences_location where id_user={0}", obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_user_job_preferences_job_type where id_user={0}", obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_ce_evaluation_jobindustry_user_mapping where id_user={0}", obj.id_user);
                        db.Database.ExecuteSqlCommand("delete from  tbl_ce_evaluation_jobrole_user_mapping where id_user={0}", obj.id_user);


                        string[] skill = obj.skill.Split(',');
                        string[] category = obj.category.Split(',');
                        string[] location = obj.id_location.Split(',');
                        string[] jobtype = obj.job_type.Split(',');
                        string[] jobind = obj.industry_str.Split(',');
                        string[] jobrole = obj.role_str.Split(',');

                        foreach (var itm in skill)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_skill (id_user,skill,status,updated_date_time) values({0},{1},{2},{3})", obj.id_user, itm, "A", DateTime.Now);

                        }
                        foreach (var itm in category)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_category (id_category,id_user,status,updated_date_time) values({0},{1},{2},{3})", itm, obj.id_user, "A", DateTime.Now);

                        }
                        foreach (var itm in location)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_location (id_location,id_user,status,updated_date_time) values({0},{1},{2},{3})", itm, obj.id_user, "A", DateTime.Now);

                        }
                        foreach (var itm in jobtype)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_user_job_preferences_job_type (id_user,job_type,status,updated_date_time) values({0},{1},{2},{3})", obj.id_user, itm, "A", DateTime.Now);

                        }
                        foreach (var itm in jobind)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_ce_evaluation_jobindustry_user_mapping (id_ce_evaluation_jobindustry,id_user,date_time_stamp,status,updated_date_time) values({0},{1},{2},{3},{4})", itm, obj.id_user, DateTime.Now, "A", DateTime.Now);

                        }
                        foreach (var itm in jobrole)
                        {

                            db.Database.ExecuteSqlCommand("insert into tbl_ce_evaluation_jobrole_user_mapping (id_ce_evaluation_jobrole,id_user,dated_time_stamp,status,updated_date_time) values({0},{1},{2},{3},{4})", itm, obj.id_user, DateTime.Now, "A", DateTime.Now);

                        }

                    }



                   


                }

                response = "SUCCESS";
            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
                response = "FAILURE";
            }
            finally
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
