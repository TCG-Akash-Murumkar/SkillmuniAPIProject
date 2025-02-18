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
    public class getEvalutionResultController : ApiController
    {
        private m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public HttpResponseMessage Get(string crf, int atm, int UID, int OID,int id_job=0)
        {
            ResultResponseBody rBody = new ResultResponseBody();
            CEReturnResponse returnResponse = new CEReturnResponse();
            string cesql01 = "select * from tbl_ce_career_evaluation_master where lower(career_evaluation_code)=lower('" + crf + "') and id_organization=" + OID + " and status='A' limit 1";
            tbl_ce_career_evaluation_master cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql01).FirstOrDefault();

            string cesql02 = "SELECT * FROM tbl_ce_evaluation_log WHERE id_organization = " + OID + " AND id_user = " + UID + " AND updated_date_time in (SELECT MAX( updated_date_time ) FROM tbl_ce_evaluation_log WHERE id_job ="+id_job +" and id_user ="+ UID + " and  id_ce_career_evaluation_master = "+ cmaster.id_ce_career_evaluation_master + " )   AND id_ce_career_evaluation_master =  " + cmaster.id_ce_career_evaluation_master + " ";
            if (id_job > 0)
            {
                cesql02 = cesql02 + " and id_job=" + id_job;
            }
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
                    if (cmaster.ce_assessment_type == 2)
                    {
                        item.Description= db.Database.SqlQuery<string>("SELECT description FROM tbl_ce_evalution_answer_key where key_code={0} and id_organization={1}", item.key_code, OID).FirstOrDefault();


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
