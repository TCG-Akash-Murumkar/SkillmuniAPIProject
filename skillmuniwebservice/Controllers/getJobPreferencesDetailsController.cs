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
    public class getJobPreferencesDetailsController : ApiController
    {
        public HttpResponseMessage Get(int UID)
        {
            preferencemodel result = new preferencemodel();

            try
            {
                using (JobDbContext db = new JobDbContext())
                {
                    tbl_user_job_preferences pref = new tbl_user_job_preferences();
                    pref = db.Database.SqlQuery<tbl_user_job_preferences>("select * from  tbl_user_job_preferences where id_user={0} ", UID).FirstOrDefault();
                    if (pref != null)
                    {
                        result.experience_months = pref.experience_months;
                        result.experience_years = pref.experience_years;
                    }
                    List< tbl_user_job_preferences_skill > SKILLLIST = db.Database.SqlQuery<tbl_user_job_preferences_skill>("select * from  tbl_user_job_preferences_skill where id_user={0}", UID).ToList();
                    int i = 1;
                    int sklcnt = SKILLLIST.Count;
                    foreach (var itm in SKILLLIST)
                    {

                        //result.jobskill.add(itm);

                        result.skill = result.skill + itm.skill;
                        if (i < sklcnt)
                        {
                            result.skill = result.skill + ",";
                        }
                        i++;
                    }
                    List<tbl_user_job_preferences_category> CATLIST = db.Database.SqlQuery<tbl_user_job_preferences_category>("select * from  tbl_user_job_preferences_category where id_user={0} and status = 'A'", UID).ToList();
                    int j = 1;
                    int catnt = CATLIST.Count;
                    if (catnt > 0)
                    {
                        foreach (var itm in CATLIST)
                        {
                            // result.CATID.Add(itm);

                            result.category = result.category + itm.id_category;
                            if (j < catnt)
                            {
                                result.category = result.category + ",";
                            }
                            j++;
                        }
                    }
                    else
                    {
                       // result.category ='"'+ "0" +'"';

                    }
                    List<tbl_user_job_preferences_location> LOCLIST = db.Database.SqlQuery<tbl_user_job_preferences_location>("select * from  tbl_user_job_preferences_location where id_user={0}", UID).ToList();
                    int k = 1;
                    int locnt = LOCLIST.Count;
                    if (locnt > 0)
                    {
                        foreach (var itm in LOCLIST)
                        {
                            //result.LOCID.Add(itm);
                            result.id_location = result.id_location + itm.id_location;
                            if (k < locnt)
                            {
                                result.id_location = result.id_location + ",";
                            }
                            k++;
                        }
                    }
                    else
                    {
                       // result.id_location = '"' + "0" + '"';

                    }

                    List<tbl_user_job_preferences_job_type> JOBTYPELIST = db.Database.SqlQuery<tbl_user_job_preferences_job_type>("select * from  tbl_user_job_preferences_job_type where id_user={0}", UID).ToList();
                    int l = 1;
                    int typcnt = JOBTYPELIST.Count;
                    foreach (var itm in JOBTYPELIST)
                    {
                        //result.JOBTYPE.Add(itm);
                        result.job_type = result.job_type + itm.job_type;
                        if (l < typcnt)
                        {
                            result.job_type = result.job_type + ",";
                        }
                        l++;
                    }
                    result.certificatepath = ConfigurationManager.AppSettings["CertificatePath"].ToString()+ db.Database.SqlQuery<string>("select certificate_file from  tbl_user_extra_curricular_certificates  where id_user={0} ", UID).FirstOrDefault();

                }

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    int resumeflag=  db.Database.SqlQuery<int>("select ResumeFlag from  tbl_profile  where ID_USER={0} ",UID).FirstOrDefault();
                    if (resumeflag == 1)
                    {
                        result.resumepath= ConfigurationManager.AppSettings["ResumePath"].ToString()+ db.Database.SqlQuery<string>("select ResumeLocation from  tbl_profile  where ID_USER={0} ", UID).FirstOrDefault();
                    }

                    result.role= db.Database.SqlQuery<tbl_ce_evaluation_jobrole_user_mapping>("select * from  tbl_ce_evaluation_jobrole_user_mapping  where id_user={0} ", UID).ToList();
                    result.industry = db.Database.SqlQuery<tbl_ce_evaluation_jobindustry_user_mapping>("select * from  tbl_ce_evaluation_jobindustry_user_mapping  where id_user={0} and status='A'", UID).ToList();
                    int rolind = 1;
                    int rlcnt = result.role.Count;
                    if (rlcnt > 0)
                    {
                        foreach (var itm in result.role)
                        {
                            result.role_str = result.role_str + itm.id_ce_evaluation_jobrole;
                            if (rolind < rlcnt)
                            {
                                result.role_str = result.role_str + ",";
                            }
                            rolind++;

                        }


                    }
                    else
                    {
                        //result.role_str = '"' + "0" + '"';

                    }

                    int indusind = 1;
                    int induscnt = result.industry.Count;
                    if (induscnt > 0)
                    {
                        foreach (var itm in result.industry)
                        {
                            result.industry_str = result.industry_str + itm.id_ce_evaluation_jobindustry;
                            if (indusind < induscnt)
                            {
                                result.industry_str = result.industry_str + ",";
                            }
                            indusind++;

                        }

                    }
                    else
                    {
                        //result.industry_str = '"' + "0" + '"';

                    }

                    tbl_cv_master mas = db.Database.SqlQuery<tbl_cv_master>(" select * from tbl_cv_master where id_user ={0} and cv_type={1}", UID, 1).FirstOrDefault();
                    if (mas != null)
                    {
                        result.isVideoCvPresent = 1;
                        tbl_video_cv vidcv= db.Database.SqlQuery<tbl_video_cv>(" select * from tbl_video_cv where id_cv ={0}", mas.id_cv).FirstOrDefault();
                        result.VideoCVStatus = vidcv.status;
                        result.VideoCVLink= ConfigurationManager.AppSettings["vidcv"].ToString()+ UID + "." + vidcv.extn;

                    }
                    else
                    {
                        result.isVideoCvPresent = 0;
                    }
                    tbl_cv_master mascv = db.Database.SqlQuery<tbl_cv_master>(" select * from tbl_cv_master where id_user ={0} and cv_type={1}", UID, 2).FirstOrDefault();
                    if (mascv != null)
                    {
                        result.isClassicCVPresent = 1;
                       
                        result.ClassicCvLink = ConfigurationManager.AppSettings["CVControl"].ToString() + mascv.id_cv;

                    }
                    else
                    {
                        result.isClassicCVPresent = 0;
                    }


                }



            }
            catch (Exception e)
            {
                throw e;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
