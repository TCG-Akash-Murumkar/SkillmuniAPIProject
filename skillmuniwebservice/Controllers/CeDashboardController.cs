using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class CeDashboardController : ApiController
    {



        public HttpResponseMessage Get(int UID, int OID, string trf)
        {
            CEDashboard dashboard = new CEDashboard();
            List<CEAssessment> ceList = new List<CEAssessment>();
            int currentindex = 0;
            int lastindex = 0;
            bool cFlag = false;
            bool pFlag = false;

            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                string cesql01 = "SELECT * FROM tbl_ce_evaluation_tile where id_organization=" + OID + " and ce_evaluation_code='" + trf + "'";
                tbl_ce_evaluation_tile tile = db.Database.SqlQuery<tbl_ce_evaluation_tile>(cesql01).FirstOrDefault();
                dashboard.tile = tile;
                string cesql03 = "select * from tbl_ce_career_evaluation_master where id_ce_evaluation_tile=" + tile.id_ce_evaluation_tile + " AND status='A' order by ordering_sequence_number ";
                List<tbl_ce_career_evaluation_master> cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql03).ToList();

                string csql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization =  " + OID + " ORDER BY attempt_no DESC LIMIT 1";
                tbl_ce_evaluation_index index = db.Database.SqlQuery<tbl_ce_evaluation_index>(csql01).FirstOrDefault();
                if (index != null)
                {
                    currentindex = index.attempt_no;
                    if (currentindex > 0)
                    {
                        lastindex = currentindex - 1;
                    }
                }
                double totalEVCount = cmaster.Count;
                string evsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master  IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + currentindex + ")";
                List<tbl_ce_career_evaluation_master> evDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql01).ToList();
                double doneEVCount = evDone.Count;
                string evsql02 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master NOT IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + currentindex + ")";
                List<tbl_ce_career_evaluation_master> evPending = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql02).ToList();
                double pendingEVCount = evPending.Count;
                if (doneEVCount > 0)
                {
                    double cval = (doneEVCount / totalEVCount) * 100;
                    dashboard.ceCurrentPercentage = Math.Round(cval, 2);
                    if (totalEVCount == doneEVCount)
                    {
                        cFlag = true;
                    }
                }
                if (cFlag)
                {
                    dashboard.ceCurrentStatus = "Completed";
                }
                else
                {
                    dashboard.ceCurrentStatus = "Incomplete";
                }
                if (lastindex > 0)
                {
                    double ptotalEVCount = cmaster.Count;
                    string pevsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master  IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + OID + " AND id_organization = " + OID + " AND attempt_no = " + lastindex + ")";
                    List<tbl_ce_career_evaluation_master> pevDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(pevsql01).ToList();
                    double pdoneEVCount = pevDone.Count;
                    string pevsql02 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master NOT IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + OID + " AND id_organization = " + OID + " AND attempt_no = " + lastindex + ")";
                    List<tbl_ce_career_evaluation_master> pevPending = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(pevsql02).ToList();
                    double ppendingEVCount = pevPending.Count;
                    if (pdoneEVCount > 0)
                    {
                        double cval = (pdoneEVCount / ptotalEVCount) * 100;
                        dashboard.ceCurrentPercentage = Math.Round(cval, 2);
                        if (ptotalEVCount == pdoneEVCount)
                        {
                            pFlag = true;
                        }
                        else
                        {
                            pFlag = false;
                        }
                    }
                }
                if (pFlag)
                {
                    dashboard.cePreviousStatus = "Completed";
                }
                else
                {
                    dashboard.cePreviousStatus = "Incomplete";
                }
                /*  ----------------------------------------------------------------------- */

                string cesql02 = "SELECT b.akcode, b.answer_key, SUM(a.job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_evalution_answer_key b WHERE a.attempt_no = " + currentindex + " AND a.id_ce_evalution_answer_key = b.id_ce_evalution_answer_key AND a.id_user = " + UID + " AND a.id_organization = " + OID + " GROUP BY b.key_code";
                List<CEAnswerKey> cDriver = db.Database.SqlQuery<CEAnswerKey>(cesql02).ToList();
                dashboard.CareerDriver = cDriver;

                List<CECategory> catList = new List<CECategory>();
                foreach (tbl_ce_career_evaluation_master row in cmaster)
                {
                    CEAssessment crow = new CEAssessment();
                    crow.career_evaluation_title = row.career_evaluation_title;
                    crow.career_evaluation_code = row.career_evaluation_code;
                    crow.ce_assessment_type = Convert.ToInt32(row.ce_assessment_type);
                    if (row.ce_assessment_type == 1) crow.cea_type = "SUL-MCA";
                    if (row.ce_assessment_type == 2) crow.cea_type = "Psychometric Assessment";
                    crow.job_points_for_ra = Convert.ToInt32(row.job_points_for_ra);
                    // string cesql04 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + " limit 1";
                    // JobPoint jPoint = db.Database.SqlQuery<JobPoint>(cesql04).FirstOrDefault();

                    string cesql05 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + "  GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 3";
                    List<JobPoint> jPointList = db.Database.SqlQuery<JobPoint>(cesql05).ToList();
                    List<int> tList = new List<int>();
                    for (int i = 0; i < 3; i++)
                    {
                        int val = 0;
                        if (jPointList.ElementAtOrDefault(i) != null)
                        {
                            val = jPointList[i].job_point;
                        }
                        tList.Add(val);
                    }
                    //crow.CEAssessList = tList;
                    ceList.Add(crow);
                }

                dashboard.ceEvaluation = ceList;
                dashboard.last_attempt_no = lastindex;
                dashboard.latest_attempt_no = currentindex;
                string cesql06 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " GROUP BY attempt_no order by attempt_no desc LIMIT 2";
                List<JobPoint> jTotalList = db.Database.SqlQuery<JobPoint>(cesql06).ToList();
                if (jTotalList.ElementAtOrDefault(0) != null)
                {
                    dashboard.ceCurrentScore = jTotalList[0].job_point;
                }
                if (jTotalList.ElementAtOrDefault(1) != null)
                {
                    dashboard.cePreviousScore = jTotalList[1].job_point;
                }

            }


                return Request.CreateResponse(HttpStatusCode.NoContent, dashboard);
        }


    }
}
