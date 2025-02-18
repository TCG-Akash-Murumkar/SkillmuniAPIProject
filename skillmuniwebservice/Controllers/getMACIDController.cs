using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class getMACIDController : ApiController
    {
        public HttpResponseMessage Get()
        {

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string Name = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).Identity.Name;

            string Name1 = System.Environment.UserName;


            string Name2 = Environment.GetEnvironmentVariable("USERNAME");
      

            string Name3 = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            //PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            //UserPrincipal user = UserPrincipal.Current;
            //string displayName = user.DisplayName;

            return Request.CreateResponse(HttpStatusCode.OK, Name1);
            

        }

    }
}
