using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateProfileController : ApiController
    {

        public HttpResponseMessage Post([FromBody]SignupModel obj)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SignupModel response = new SignupModel();
            try
            {

                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    if (obj.ref_code != null)
                    {
                        int refparent = db.Database.SqlQuery<int>("select ID_USER from tbl_user where ref_id={0} ", obj.ref_code).FirstOrDefault();
                        if (refparent > 0)
                        {
                            int refpoints = db.Database.SqlQuery<int>("select ref_points from tbl_referral_code_points_config where ref_type={0}", 1).FirstOrDefault();
                            db.Database.ExecuteSqlCommand("Insert into tbl_referral_code_user_mapping (id_user,referral_code,status,updated_date_time,referral_points) values({0},{1},{2},{3},{4}) ", obj.ID_USER, obj.ref_code, "A", DateTime.Now, refpoints);
                        }
                        else
                        {
                            db.Database.ExecuteSqlCommand("Insert into tbl_referral_code_user_mapping (id_user,referral_code,status,updated_date_time) values({0},{1},{2},{3}) ", obj.ID_USER, obj.ref_code, "A", DateTime.Now);

                        }

                    }
                    int val = db.Database.ExecuteSqlCommand("update tbl_profile  set FIRSTNAME={0} , LASTNAME={1} , STATE={2} , CITY={3},  GENDER={4} , DATE_OF_BIRTH={5} , COLLEGE={6} , GRADUATIONYEAR={7} , id_degree={8} , id_stream={9} , COUNTRY={11},STUDENT={12},NOTSTUDENT={13},id_foundation={14} ,clg_city = {15},clg_state = {16},clg_country = {17},MOBILE={18} where ID_USER={10}", obj.FIRSTNAME, obj.LASTNAME, obj.State, obj.City, obj.Gender, obj.DOB, obj.College, obj.GraduationYear, obj.id_degree, obj.id_stream, obj.ID_USER, obj.COUNTRY, obj.STUDENT, obj.NOTSTUDENT, obj.id_foundation, obj.clg_city, obj.clg_state, obj.clg_country, obj.MOBILENO);
                    if (obj.id_stream == 0)
                    {
                        db.Database.ExecuteSqlCommand("update   tbl_profile  set OTHERSTREAM={0} where ID_USER={1}", obj.OTHERSTREAM, obj.ID_USER);

                    }
                    response.response_status = "SUCCESS";
                    response.response_message = "USER SUCCESSFULLY UPDATED";

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
