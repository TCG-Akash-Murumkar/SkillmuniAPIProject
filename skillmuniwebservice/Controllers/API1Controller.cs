using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class API1Controller : ApiController
    {
        public HttpResponseMessage Post([FromBody]API1Input inp)
        {
            API1Response Result = new API1Response();
            try
            {

               
                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    Result.SurveryDetails=  db.Database.SqlQuery<SurveryDetails>("select ClaimNumber as claimNo  from  SurveyFeedback where EmployeeId={0} and FeedbackStatus={1} and FeedbackExpiresOn<{2}", inp.employeeID,"Open",DateTime.Now).ToList();
                    foreach (var itm in Result.SurveryDetails)
                    {
                        itm.caseTypeId = "1";
                        itm.caseType = "Offshore Cash";
                        itm.expenseType = "Conveyance";
                        itm.claimApprovedAmount = "1200.00";
                        itm.claimAmountPaidOn = "12 March, 2020";
                        itm.feedbackReasonOptions = db.Database.SqlQuery<feedbackreasonmaster>("select ReasonCode as code,ReasonDescription as description from SurveyFeedbackReasonMaster where CaseTypeId={0} ORDER BY SeqNo ASC", 1).ToList();
                      
                    }
                    Result.ret_code = "200";
                    Result.ret_message = "SUCCESS";
                }
            }
            catch (Exception e)
            {
                Result.ret_code = "500";
                Result.ret_message = "Sorry we could not update the feedback status in CMS. Please try again after sometime.";

                Result.SurveryDetails = null;

                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }
    }
}
