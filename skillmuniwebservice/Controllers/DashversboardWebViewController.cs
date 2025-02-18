using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m2ostnextservice.Controllers
{
    public class DashversboardWebViewController : Controller
    {
        // GET: DashversboardWebView
        public ActionResult AssessmentSheet(string brfcode, int UID, int OID, int ACID, int BriefTileID = 0)
        {
            return RedirectToAction("AssessmentSheet", "DashboardWebView", new { brfcode = brfcode, UID = UID, OID = OID, ACID = ACID, BriefTileID = BriefTileID });
        }
    }
}