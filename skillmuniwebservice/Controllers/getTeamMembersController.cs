using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class getTeamMembersController : ApiController
    {
           db_m2ostEntities db = new db_m2ostEntities();

           public HttpResponseMessage Get(int UID, string FLAG, int OID)
           {
               List<TeamMember> teamMember = new List<TeamMember>();
               tbl_user user = db.tbl_user.Where(t => t.ID_USER == UID).FirstOrDefault();
               if (user != null)
               {
                   List<tbl_user> team = db.tbl_user.Where(t => t.reporting_manager == user.ID_USER).ToList();
                   foreach (tbl_user item in team)
                   {
                       tbl_profile profile = db.tbl_profile.Where(t => t.ID_USER == item.ID_USER).FirstOrDefault();
                       if (profile != null)
                       {
                           TeamMember temp = new TeamMember();
                           temp.USERID = item.USERID;
                           temp.ID_USER = item.ID_USER.ToString();
                           temp.USERNAME = profile.FIRSTNAME + " " + profile.LASTNAME;
                           temp.DEPARTMENT = item.user_department;
                           temp.DESIGNATION = item.user_designation;
                           temp.GRADE = item.user_grade;
                           temp.FUNCTION = item.user_function;
                           temp.EMPLOYEEID = item.EMPLOYEEID;
                           teamMember.Add(temp);
                       }
                   }

               }
               else
               {

               }

               return Request.CreateResponse(HttpStatusCode.OK, teamMember);
           }
    }
}
