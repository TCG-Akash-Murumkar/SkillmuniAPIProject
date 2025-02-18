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
    public class CreateCVController : ApiController
    {
        public HttpResponseMessage Post([FromBody]CVBuilderCreation CVMaster)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CVBuilderResponse Result = new CVBuilderResponse();
            try
            {
                if (CVMaster.data_flag == 1)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                    {
                        CVMaster.id_cv= db.Database.SqlQuery<int>(" insert into  tbl_cv_master (id_user,oid,created_date,modified_date,status,cv_type) values({0},{1},{2},{3},{4},{5});select max(id_cv) from tbl_cv_master", CVMaster.UID,CVMaster.OID,DateTime.Now,DateTime.Now,"A",2).FirstOrDefault();

                        db.Database.ExecuteSqlCommand(" insert into  tbl_cv_personel_info (id_cv,id_user,first_name,last_name,mobile,email,country,city,street,day,month,year,about) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", CVMaster.id_cv,CVMaster.UID,CVMaster.personel.first_name,CVMaster.personel.last_name,CVMaster.personel.mobile,CVMaster.personel.email,CVMaster.personel.country,CVMaster.personel.city,CVMaster.personel.street,CVMaster.personel.day,CVMaster.personel.month,CVMaster.personel.year,CVMaster.personel.about);
                        db.Database.ExecuteSqlCommand("insert into tbl_cv_additional_info (id_cv,id_user,skills,languages,intrests,linkedin,facebook,twitter,blog,others,refrences,awards) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", CVMaster.id_cv, CVMaster.UID,CVMaster.additional_info.skills,CVMaster.additional_info.languages,CVMaster.additional_info.intrests,CVMaster.additional_info.linkedin,CVMaster.additional_info.facebook,CVMaster.additional_info.twitter,CVMaster.additional_info.blog, CVMaster.additional_info.others,CVMaster.additional_info.refrences,CVMaster.additional_info.awards);
                        foreach (var itm in CVMaster.education)
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_cv_education (id_cv,id_user,college,degree,start_date,end_date,summary) values({0},{1},{2},{3},{4},{5},{6})", CVMaster.id_cv,CVMaster.UID,itm.college,itm.degree,itm.start_date,itm.end_date,itm.summary);
                        }
                        foreach (var itm in CVMaster.project_list)
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_cv_project (id_cv,id_user,college,project_title,start_date,end_date,summary) values({0},{1},{2},{3},{4},{5},{6})", CVMaster.id_cv, CVMaster.UID, itm.college, itm.project_title, itm.start_date, itm.end_date, itm.summary);
                        }


                    }

                }
                else if (CVMaster.data_flag == 2)
                {
                    using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())

                    {
                        CVMaster.id_cv = CVMaster.personel.id_cv;

                        db.Database.ExecuteSqlCommand("update tbl_cv_master set modified_date={0} where id_cv={1}",  DateTime.Now, CVMaster.id_cv);
                        db.Database.ExecuteSqlCommand("delete from tbl_cv_personel_info where id_cv={0}", CVMaster.id_cv);
                        db.Database.ExecuteSqlCommand("insert into  tbl_cv_personel_info (id_cv,id_user,first_name,last_name,mobile,email,country,city,street,day,month,year,about) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", CVMaster.id_cv, CVMaster.UID, CVMaster.personel.first_name, CVMaster.personel.last_name, CVMaster.personel.mobile, CVMaster.personel.email, CVMaster.personel.country, CVMaster.personel.city, CVMaster.personel.street, CVMaster.personel.day, CVMaster.personel.month, CVMaster.personel.year, CVMaster.personel.about);
                        db.Database.ExecuteSqlCommand("delete from tbl_cv_additional_info where id_cv={0}", CVMaster.id_cv);
                        db.Database.ExecuteSqlCommand("insert into tbl_cv_additional_info (id_cv,id_user,skills,languages,intrests,linkedin,facebook,twitter,blog,others,refrences,awards) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", CVMaster.id_cv, CVMaster.UID, CVMaster.additional_info.skills, CVMaster.additional_info.languages, CVMaster.additional_info.intrests, CVMaster.additional_info.linkedin, CVMaster.additional_info.facebook, CVMaster.additional_info.twitter, CVMaster.additional_info.blog, CVMaster.additional_info.others, CVMaster.additional_info.refrences, CVMaster.additional_info.awards);
                        db.Database.ExecuteSqlCommand("delete from tbl_cv_education where id_cv={0}", CVMaster.id_cv);
                        db.Database.ExecuteSqlCommand("delete from tbl_cv_project where id_cv={0}", CVMaster.id_cv);

                        foreach (var itm in CVMaster.education)
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_cv_education (id_cv,id_user,college,degree,start_date,end_date,summary) values({0},{1},{2},{3},{4},{5},{6})", CVMaster.id_cv, CVMaster.UID, itm.college, itm.degree, itm.start_date, itm.end_date, itm.summary);
                        }
                        foreach (var itm in CVMaster.project_list)
                        {
                            db.Database.ExecuteSqlCommand("insert into tbl_cv_project (id_cv,id_user,college,project_title,start_date,end_date,summary) values({0},{1},{2},{3},{4},{5},{6})", CVMaster.id_cv, CVMaster.UID, itm.college, itm.project_title, itm.start_date, itm.end_date, itm.summary);
                        }


                    }

                }           
            }
            catch (Exception e)
            {
                Result.STATUS = "FAILED";
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            finally
            {
                Result.STATUS = "SUCCESS";
                Result.RESUMELINK = ConfigurationManager.AppSettings["CVControl"].ToString() + CVMaster.id_cv;

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
    }
}
