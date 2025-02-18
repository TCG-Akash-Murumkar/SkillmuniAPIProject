using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class GetOfflineCategoryController : ApiController
    {
        public HttpResponseMessage Get(int organizationID)
        {
            Response response = new Response();
            List<OfflineCategory> offlineCategoryList = new OfflineAccess().GetCategory(organizationID);

            if (offlineCategoryList != null)
            {
                response.ResponseCode = "SUCCESS";
                response.ResponseAction = 1;
                response.ResponseMessage = "Category Retrieved.";
            }
            else
            {
                response.ResponseCode = "Failure";
                response.ResponseAction = 1;
                response.ResponseMessage = "No Category available.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, offlineCategoryList);
        }
    }
}

//using m2ostnextservice.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//using System.Security.Cryptography;
//using System.IO;
//using System.Text;

//using System;

//namespace m2ostnextservice.Controllers
//{
//    public class GetOfflineCategoryController : ApiController
//    {
//        public HttpResponseMessage Get(string ent)
//        {
//            string password = "3sc3RLrpd17";

//            // Create sha256 hash
//            SHA256 mySHA256 = SHA256Managed.Create();
//            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));

//            // Create secret IV
//            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

//            //  string encrypted = new AESAlgorithm().EncryptString(message, key, iv);
//            string decrypted = new AESAlgorithm().DecryptString(ent, key, iv);

//            return Request.CreateResponse(HttpStatusCode.OK, decrypted);
//        }
//    }
//}