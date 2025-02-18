using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;

namespace m2ostnextservice.Controllers
{
    public class getUniversityGameListController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();
      
        public HttpResponseMessage GET(int OID)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<tbl_game_master> response = new List<tbl_game_master>();
            try
            {
                using (db_m2ostEntities db = new db_m2ostEntities())
                {
               
                   //string sqlq = "SELECT * FROM tbl_game_master where id_org="+OID;
                   string sqlq= "SELECT * FROM tbl_game_master  INNER JOIN tbl_theme_master ON tbl_game_master.id_theme = tbl_theme_master.id_theme where tbl_game_master.id_org="+OID+" and tbl_game_master.status='A'";

                   response = db.Database.SqlQuery<tbl_game_master>(sqlq).ToList();     

               }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK,response);
                
        }
       
    }
}