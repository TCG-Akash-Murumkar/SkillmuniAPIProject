using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class DecryptAESController : ApiController
    {

        public HttpResponseMessage Get(string pass)
        {

            string password = "3sc3RLrpd17";
            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));

            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            //  string encrypted = new AESAlgorithm().EncryptString(message, key, iv);
            string en_password = new AESAlgorithm().DecryptString(pass, key, iv);





            return Request.CreateResponse(HttpStatusCode.OK, en_password);
        }
    }
}
