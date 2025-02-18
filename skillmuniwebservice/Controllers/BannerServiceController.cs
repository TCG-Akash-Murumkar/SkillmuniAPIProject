using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class BannerServiceController : ApiController
    {
        public HttpResponseMessage Get(string OID, string option, string uid)
        {
            int bannerID = new SubscriptionModel().GetBanner(OID, uid);
            if (bannerID > 0)
            {
                new SubscriptionModel().SetBannerUpdate(bannerID.ToString(), option);
            }
            else
            {
                bannerID = new SubscriptionModel().SetBannerInsert(OID, uid, option);
            }
            return Request.CreateResponse(HttpStatusCode.OK, bannerID);
        }
    }
}