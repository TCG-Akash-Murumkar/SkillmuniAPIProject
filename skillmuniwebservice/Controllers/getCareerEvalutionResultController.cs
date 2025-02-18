using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using Newtonsoft.Json;

namespace m2ostnextservice.Controllers
{
    public class getCareerEvalutionResultController : ApiController
    {
        private m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public HttpResponseMessage Get(string crf, int atm, int UID, int OID)
        {
            ResultResponseBody rBody = new ResultResponseBody();
            CEReturnResponse returnResponse = new CEReturnResponse();
            string cesql01 = "select * from tbl_ce_career_evaluation_master where lower(career_evaluation_code)=lower('" + crf + "') and id_organization=" + OID + " and status='A' limit 1";
            tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql01).FirstOrDefault();

            string cesql02 = "SELECT * FROM tbl_ce_evaluation_log WHERE id_organization = " + OID + " AND id_user = " + UID + " AND attempt_no = " + atm + " AND id_ce_career_evaluation_master =  " + cmaster.id_ce_career_evaluation_master + " ";
            tbl_ce_evaluation_log log = db.Database.SqlQuery<tbl_ce_evaluation_log>(cesql02).FirstOrDefault();
            if (log != null)
            {
                rBody.status = "success";
                returnResponse = JsonConvert.DeserializeObject<CEReturnResponse>(log.json_response);
                foreach (AnswerKeyBlock item in returnResponse.answerKeyBlock)
                {
                    string akUrl = ConfigurationManager.AppSettings["BRIEFIMAGE"].ToString() + "ANSWERKEY/";

                    if (item.aklogo is null)
                    {
                        item.aklogo = akUrl + item.key_code + ".png";
                    }
                }
                returnResponse.CETime = log.cetimespan;
                rBody.data = returnResponse;
                if (returnResponse.answerKeyBlock.Count > 0)
                {
                    returnResponse.answerKeyBlock = returnResponse.answerKeyBlock.OrderByDescending(x => x.job_point).ToList();
                }
            }
            else
            {
                rBody.status = "failure";
                rBody.data = null;
            }


            return Request.CreateResponse(HttpStatusCode.OK, returnResponse);
        }

    }
}
