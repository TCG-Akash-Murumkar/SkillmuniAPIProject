using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class API2Controller : ApiController
    {
        public HttpResponseMessage Post([FromBody]API2Input inp)
        {
            API2Response Result = new API2Response();
            try
            {


                using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
                {
                    db.Database.ExecuteSqlCommand("insert into SurveyFeedbackSubmitJson (JsonString,SubmittedOn,SubmittedStatus) values({0},{1},{2})",inp,DateTime.Now,"SUCCESS");
                    db.Database.ExecuteSqlCommand("update SurveyFeedback set FeedbackStatus={0},Ratings={1},FeedbackCapturedOn={2} where employeeID={3} and claimNo={4}","CLOSED",inp.rating,DateTime.Now,inp.employeeID,inp.claimNo);
                    foreach (var itm in inp.feedbackReasonSelected)
                    {
                        int SurveyFeedbackID = db.Database.SqlQuery<int>("select ID from SurveyFeedback where ClaimNumber={0} and EmployeeId={1}",inp.claimNo,inp.employeeID).FirstOrDefault();
                        db.Database.ExecuteSqlCommand("insert into SurveyFeedbackReasonOptions (SurveyFeedbackID,ReasonCode) values({0},{1})", SurveyFeedbackID,itm.code);


                    }

                    Result.ret_code = "200";
                    Result.ret_message = "SUCCESS";
                }
            }
            catch (Exception e)
            {
                Result.ret_code = "500";
                Result.ret_message = "Sorry we could not update the feedback status in CMS. Please try again after sometime.";


                return Request.CreateResponse(HttpStatusCode.OK, Result);

            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);

        }

    }
}
