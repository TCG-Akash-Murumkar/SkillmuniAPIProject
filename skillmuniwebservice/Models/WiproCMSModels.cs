using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class WiproCMSModels
    {
    }
    public class API1Input
    {
        public string employeeID { get; set; }
       
    }
    public class API2Input
    {
        public string employeeID { get; set; }
        public string claimNo { get; set; }
        public string rating { get; set; }
        public List<feedbackReasonSelected> feedbackReasonSelected { get; set; }
        public string additionalComments { get; set; }
    }
    public class feedbackReasonSelected
    {
        public string code { get; set; }

        
    }
    public class SurveyFeedback
    {
        public int ID { get; set; }
        public string ClaimNumber { get; set; }

        public string EmployeeId { get; set; }

        public DateTime FeedbackOpenedOn { get; set; }

        public DateTime FeedbackExpiresOn { get; set; }
        public string FeedbackStatus { get; set; }

        public int Ratings { get; set; }
        public string Remarks { get; set; }
        public DateTime FeedbackCapturedOn { get; set; }




    }
    public class API1Response
    {
        public string ret_code { get; set; }
        public string ret_message { get; set; }
        public List<SurveryDetails> SurveryDetails { get; set; }


    }
    public class API2Response
    {
        public string ret_code { get; set; }
        public string ret_message { get; set; }

    }
    public class SurveryDetails
    {
        public string claimNo { get; set; }
        public string caseTypeId { get; set; }
        public string caseType { get; set; }
        public string expenseType { get; set; }
        public string claimApprovedAmount { get; set; }
        public string claimAmountPaidOn { get; set; }
        public List<feedbackreasonmaster> feedbackReasonOptions { get; set; }


    }

    public class feedbackreasonmaster
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class SurveyFeedbackReasonMaster
    {
        public int ID { get; set; }
        public int SeqNo { get; set; }

        public string ReasonCode { get; set; }

        public string ReasonDescription { get; set; }

        public string CaseTypeId { get; set; }

        


    }


}