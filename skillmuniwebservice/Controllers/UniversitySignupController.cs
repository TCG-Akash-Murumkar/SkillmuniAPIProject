using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using System.Configuration;
using Newtonsoft.Json;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UniversitySignupController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Post([FromBody]SignupModel obj)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SignupModel response = new SignupModel();
            try
            {
                
                using (db_m2ostEntities db = new db_m2ostEntities())
                {

                    int col = db.Database.SqlQuery<int>("SELECT id_college FROM tbl_college_list where college_name={0}",obj.College).FirstOrDefault();
                    if(col==0)
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_college_list(college_name,status,id_user,updated_datetime) values({0},{1},{2},{3})", obj.College,"A",obj.ID_USER,DateTime.Now);


                    }

                    db.Database.ExecuteSqlCommand("insert into  tbl_profile  (FIRSTNAME,LASTNAME,MOBILE,EMAIL,STATE,CITY,GENDER,DATE_OF_BIRTH,COLLEGE,GRADUATIONYEAR,ID_USER,PROFILE_IMAGE,id_degree,id_stream,COUNTRY,STUDENT,NOTSTUDENT,id_foundation) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})", obj.FIRSTNAME,obj.LASTNAME,obj.MOBILENO,obj.MAILID,obj.State,obj.City,obj.Gender,obj.DOB,obj.College,obj.GraduationYear,obj.ID_USER, obj.PROFILEIMAGE,obj.id_degree,obj.id_stream,obj.COUNTRY,obj.STUDENT,obj.NOTSTUDENT,obj.id_foundation);


                    if(obj.id_stream==0)
                    {
                        db.Database.ExecuteSqlCommand("update   tbl_profile  set OTHERSTREAM={0} where ID_USER={1}",obj.OTHERSTREAM,obj.ID_USER);

                    }
                    response.response_status = "SUCCESS";
                    response.response_message = "USER SUCCESSFULLY REGISTERED";

                    //if (obj.ref_code != null)
                    //{
                    //    //string refcode = db.Database.SqlQuery<string>("select referral_name from tbl_referral_code_master where referral_code={0}",obj.ref_code).FirstOrDefault();

                    //    int refparent = db.Database.SqlQuery<int>("select ID_USER from tbl_user where ref_id={0} ",obj.ref_code).FirstOrDefault();
                    //    if (refparent > 0)
                    //    {
                    //        int refpoints = db.Database.SqlQuery<int>("select ref_points from tbl_referral_code_points_config where ref_type={0}", 1).FirstOrDefault();
                    //        db.Database.ExecuteSqlCommand("Insert into tbl_referral_code_user_mapping (id_user,referral_code,status,updated_date_time,referral_points) values({0},{1},{2},{3},{4}) ", obj.ID_USER, obj.ref_code, "A", DateTime.Now, refpoints);
                    //        //db.Database.ExecuteSqlCommand("Insert into tbl_user_currency_log (id_user,id_game,currency_value,status,updated_date_time) values({0},{1},{2},{3},{4}) ", obj.ID_USER, 0, refpoints, "A", DateTime.Now);

                            
                    //    }
                    //    else
                    //    {
                    //        db.Database.ExecuteSqlCommand("Insert into tbl_referral_code_user_mapping (id_user,referral_code,status,updated_date_time) values({0},{1},{2},{3}) ", obj.ID_USER, obj.ref_code, "A", DateTime.Now);

                    //    }

                    //}
                    tbl_user_log_master lgmas = new tbl_user_log_master();

                    lgmas = db.Database.SqlQuery<tbl_user_log_master>("select * from tbl_user_log_master where id_user={0}", obj.ID_USER).FirstOrDefault();

                    if (lgmas == null)
                    {
                        db.Database.ExecuteSqlCommand("insert into tbl_user_log_master (id_user,is_registered,academic_tiles,study_abroad,job,entrepreneurship,updated_date_time) values ({0},{1},{2},{3},{4},{5},{6})", obj.ID_USER, "YES", 0, 0, 0, 0, DateTime.Now);

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("update  tbl_user_log_master set is_registered={0} where id_user={1}","YES",obj.ID_USER);


                    }
                    tbl_profile pro = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}",obj.ID_USER).FirstOrDefault();
                    if (pro != null)
                    {
                        response.FIRSTNAME = pro.FIRSTNAME;
                        response.MAILID = pro.EMAIL;
                        response.State = pro.STATE;
                        response.City = pro.CITY;
                        response.College = pro.COLLEGE;
                        response.clg_city = pro.clg_city;
                        response.clg_country = pro.clg_state;

                    }
                   
                }
   

            }
            catch (Exception e)
            {
                new Utility().eventLog(controllerName + " : " + e.Message);
                new Utility().eventLog("Inner Exeption" + " : " + e.InnerException.ToString());
                new Utility().eventLog("Additional Details" + " : " + e.Message);
            }
            finally
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
