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
// Partial Profile Update API Update , Name, Mobile, Email,  Student / Non Student
// Created on 07-4-2020
namespace m2ostnextservice.Controllers
{
    public class PostProfilePartialUpdateController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Post([FromBody]PartialProfileUpdate obj)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            PartialProfileUpdate response = new PartialProfileUpdate();
            try
            {
                using (db_m2ostEntities db = new db_m2ostEntities())
                {
                    db.Database.ExecuteSqlCommand("update tbl_profile set FIRSTNAME={0}, EMAIL={1},MOBILE={2},STUDENT={3} where ID_USER={4}", obj.FIRSTNAME, obj.MAILID, obj.MOBILENO, obj.STUDENT, obj.ID_USER);
                }
                            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
            }
            finally { }
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }
}