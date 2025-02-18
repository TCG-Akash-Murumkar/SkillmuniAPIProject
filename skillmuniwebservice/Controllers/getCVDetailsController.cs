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
    public class getCVDetailsController : ApiController
    {
        public HttpResponseMessage Get(int UID,int OID)
        {
            List<tbl_cv_master> cv_mas = new List<tbl_cv_master>();
            CreateResumeDetails result = new CreateResumeDetails();


            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {

                 cv_mas = db.Database.SqlQuery<tbl_cv_master>("select * from tbl_cv_master where id_user={0} and cv_type={1}", UID,2).ToList();


            }

            if (cv_mas.Count == 0)
            {
                result.data_flag = 1;

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {

                    tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    result.ProfilePicture = prof.PROFILE_IMAGE;
                    result.FirstName = prof.FIRSTNAME;
                    result.LastName = prof.LASTNAME;
                    result.Phone = prof.MOBILE;
                    result.Email = prof.EMAIL;
                    result.Country = prof.COUNTRY;
                    result.City = prof.CITY;
                    result.DOB = Convert.ToString(prof.DATE_OF_BIRTH);
                    List<CVCollegeDetails> collis = new List<CVCollegeDetails>();
                    CVCollegeDetails col = new CVCollegeDetails();
                    col.College = prof.COLLEGE;
                    col.Degree = db.Database.SqlQuery<string>("select degree from tbl_degree_master where id_degree={0}", prof.id_degree).FirstOrDefault();
                    collis.Add(col);
                    result.College = collis;
                    //result.College[0].College = prof.COLLEGE;
                    //result.College[0].Degree= db.Database.SqlQuery<string>("select degree from tbl_degree_master where id_degree={0}", prof.id_degree).FirstOrDefault();
                    List<tbl_user_job_preferences_skill> skill = db.Database.SqlQuery<tbl_user_job_preferences_skill>("select * from tbl_user_job_preferences_skill where id_user={0}", UID).ToList();
                    int i = 1;
                    foreach (var itm in skill)
                    {
                        result.Skills = result.Skills + itm.skill;
                        if (i < skill.Count())
                        {
                            result.Skills = result.Skills + ",";
                        }
                        i++;

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            else
            {
                tbl_cv_master cv_temp = new tbl_cv_master();

                result.data_flag = 2;
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    tbl_profile prof = db.Database.SqlQuery<tbl_profile>("select * from tbl_profile where ID_USER={0}", UID).FirstOrDefault();
                    result.ProfilePicture = prof.PROFILE_IMAGE;
                    cv_temp = db.Database.SqlQuery<tbl_cv_master>("select * from tbl_cv_master where id_user={0} and cv_type={1}", UID,2).FirstOrDefault();
                    result.personel = db.Database.SqlQuery<tbl_cv_personel_info>("select * from tbl_cv_personel_info where id_cv={0}", cv_temp.id_cv).FirstOrDefault();
                    result.education = db.Database.SqlQuery<tbl_cv_education>("select * from tbl_cv_education where id_cv={0}", cv_temp.id_cv).ToList();
                    result.project_list = db.Database.SqlQuery<tbl_cv_project>("select * from tbl_cv_project where id_cv={0}", cv_temp.id_cv).ToList();
                    result.additional_info = db.Database.SqlQuery<tbl_cv_additional_info>("select * from tbl_cv_additional_info where id_cv={0}", cv_temp.id_cv).FirstOrDefault();


                }



                return Request.CreateResponse(HttpStatusCode.OK, result);


            }





        }


    }
}
