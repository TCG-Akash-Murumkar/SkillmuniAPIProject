using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class getCareerEvaluationDataController : ApiController
    {
        // private db_m2ostEntities db = new db_m2ostEntities();
        private m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public HttpResponseMessage Get(int UID, int OID)
        {
            APIRESULT res = new APIRESULT();
            List<CETile> tiles = new List<CETile>();
            //  string baseurl = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BRIEF/";

            string cesql01 = "select * from tbl_ce_evaluation_tile where status='A' and id_organization=" + OID + " ";
            List<tbl_ce_evaluation_tile> ceTiles = db.Database.SqlQuery<tbl_ce_evaluation_tile>(cesql01).ToList();

            foreach (tbl_ce_evaluation_tile item in ceTiles)
            {
                CETile temp = new CETile();
                temp.ce_evaluation_code = item.ce_evaluation_code;
                temp.ce_evaluation_tile = item.ce_evaluation_tile;
                temp.description = item.description;
                temp.id_organization = Convert.ToInt32(item.id_organization);
                temp.image_path = ConfigurationManager.AppSettings["CETileBImageBase"].ToString() + item.image_path;
                temp.sequence_order = Convert.ToInt32(item.sequence_order);
                // temp.black_image = ConfigurationManager.AppSettings["CETileBImageBase"].ToString() + item.black_image;

                int currentindex = 0;
                currentindex = checkCurrentIndex(item.id_ce_evaluation_tile, UID, OID);
                temp.reattempt = false;
                if (currentindex > 0)
                {
                    bool cFlag = checkAttemptCompilation(item.id_ce_evaluation_tile, UID, OID, currentindex);
                    if (cFlag)
                    {
                        temp.reattempt = true;
                        if (item.cooling_period > 0)
                        {
                            DateTime expirydate = DateTime.Now;
                            bool cpflag = checkCoolingPeriodExpiry(item.id_ce_evaluation_tile, UID, OID, item.cooling_period, out expirydate);
                            if (cpflag)
                            {
                                temp.cooling_period = true;
                            }
                            else
                            {
                                temp.cooling_period = false;
                            }
                            temp.cooling_period_expiry = expirydate.ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            temp.cooling_period = false;
                            temp.cooling_period_expiry = DateTime.Now.ToString("dd-MM-yyyy");
                        }
                    }
                    else
                    {
                        temp.cooling_period = false;
                        temp.cooling_period_expiry = DateTime.Now.ToString("dd-MM-yyyy");
                    }
                }
                else
                {
                    temp.reattempt = false;
                    temp.cooling_period = false;
                    temp.cooling_period_expiry = DateTime.Now.ToString("dd-MM-yyyy");
                }

                string cesql02 = "select * from tbl_ce_career_evaluation_master where id_ce_evaluation_tile=" + item.id_ce_evaluation_tile + "  AND id_organization=" + OID + " AND status='A' order by ordering_sequence_number ";
                List<tbl_ce_career_evaluation_master> cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql02).ToList();
                List<CECategory> catList = new List<CECategory>();
                foreach (tbl_ce_career_evaluation_master row in cmaster)
                {
                    CECategory crow = new CECategory();
                    crow.id_ce_career_evaluation_master = row.id_ce_career_evaluation_master;
                    crow.career_evaluation_title = row.career_evaluation_title;
                    crow.career_evaluation_code = row.career_evaluation_code;
                    crow.id_ce_evaluation_tile = Convert.ToInt32(row.id_ce_evaluation_tile);
                    crow.ce_description = row.ce_description;
                    crow.validation_period = Convert.ToInt32(row.validation_period);
                    crow.ordering_sequence_number = Convert.ToInt32(row.ordering_sequence_number);
                    crow.no_of_question = Convert.ToInt32(row.no_of_question);
                    crow.is_time_enforced = Convert.ToInt32(row.is_time_enforced);
                    crow.time_enforced = Convert.ToInt32(row.time_enforced);
                    crow.ce_assessment_type = Convert.ToInt32(row.ce_assessment_type);
                    crow.job_points_for_ra = Convert.ToInt32(row.job_points_for_ra);
                    crow.ce_image = ConfigurationManager.AppSettings["CeImageBase"].ToString() + OID + "/" + row.ce_image;

                    bool doneFlag = checkCurrentEvaluationStatus(row.id_ce_career_evaluation_master, UID, OID, currentindex);
                    crow.cFlag = doneFlag;
                    crow.last_attempt_number = db.Database.SqlQuery<int>("select COALESCE (max(attempt_no),0) from tbl_ce_evaluation_log where id_user={0} and id_ce_career_evaluation_master={1} ", UID, row.id_ce_career_evaluation_master).FirstOrDefault();

                    catList.Add(crow);
                }
                temp.CECategory = catList;
                tiles.Add(temp);
            }

            tiles = tiles.OrderBy(o => o.sequence_order).ToList();
            if (tiles != null)
            {
                res.STATUS = "SUCCESS";
                res.DETAIL = tiles;
            }
            else
            {
                res.STATUS = "FAILURE";
            }

            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        private int checkCurrentIndex(int ctid, int UID, int OID)
        {
            int cindex = 0; int lindex = 0;
            string csql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user =  " + UID + "  AND id_organization =  " + OID + "  AND id_ce_career_evaluation_master IN (SELECT id_ce_career_evaluation_master FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_evaluation_tile =  " + ctid + " ) ORDER BY attempt_no DESC LIMIT 1";
            tbl_ce_evaluation_index ceindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(csql01).FirstOrDefault();
            if (ceindex != null)
            {
                cindex = ceindex.attempt_no;
            }
            return cindex;
        }

        private bool checkAttemptCompilation(int ctid, int UID, int OID, int cindex)
        {
            bool cFlag = false;
            string dsql03 = "select * from tbl_ce_career_evaluation_master where id_ce_evaluation_tile=" + ctid + " AND status='A' order by ordering_sequence_number ";
            List<tbl_ce_career_evaluation_master> cmasterList = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(dsql03).ToList();
            double totalEVCount = cmasterList.Count;
            string evsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master  IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + cindex + ")";
            List<tbl_ce_career_evaluation_master> evDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql01).ToList();
            double doneEVCount = evDone.Count;
            string evsql02 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master NOT IN (SELECT DISTINCT id_ce_career_evaluation_master FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND attempt_no = " + cindex + ")";
            List<tbl_ce_career_evaluation_master> evPending = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(evsql02).ToList();
            double pendingEVCount = evPending.Count;
            if (doneEVCount > 0)
            {
                if (evPending.Count == 1)
                {
                    if (evPending[0].ce_assessment_type == 2)
                    {
                        string csql01 = "SELECT * FROM tbl_ce_evaluation_log WHERE id_user =  " + UID + "  AND id_organization =  " + OID + "  AND id_ce_career_evaluation_master =" + evPending[0].id_ce_career_evaluation_master + " ORDER BY attempt_no DESC , updated_date_time DESC LIMIT 1";
                        tbl_ce_evaluation_log ceindex = db.Database.SqlQuery<tbl_ce_evaluation_log>(csql01).FirstOrDefault();
                        if (ceindex != null)
                        {
                            if (evPending[0].validation_period > 0)
                            {
                                DateTime expirydate = ceindex.updated_date_time.Value.AddMonths(evPending[0].validation_period);
                                if (expirydate > DateTime.Now)
                                {
                                    doneEVCount = doneEVCount + 1;
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                doneEVCount = doneEVCount + 1;
                            }
                        }
                    }
                }
                if (totalEVCount == doneEVCount)
                {
                    cFlag = true;
                }
            }

            return cFlag;
        }

        private bool checkCurrentEvaluationStatus(int ceid, int UID, int OID, int cindex)
        {
            bool aFlag = false;
            string chsql01 = "SELECT * FROM tbl_ce_career_evaluation_master WHERE id_organization = " + OID + " AND id_ce_career_evaluation_master IN (SELECT id_ce_career_evaluation_master FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master=" + ceid + " AND attempt_no = " + cindex + ") limit 1";
            tbl_ce_career_evaluation_master chDone = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(chsql01).FirstOrDefault();
            if (chDone == null)
            {
                aFlag = true;
            }
            return aFlag;
        }

        private bool checkCoolingPeriodExpiry(int ctid, int UID, int OID, int cpdays, out DateTime expiry)
        {
            bool cpflag = false;
            expiry = DateTime.Now;
            string csql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user =  " + UID + "  AND id_organization =  " + OID + "  AND id_ce_career_evaluation_master IN (SELECT id_ce_career_evaluation_master FROM tbl_ce_career_evaluation_master WHERE ce_assessment_type=1 AND id_organization = " + OID + " AND id_ce_evaluation_tile =  " + ctid + " ) ORDER BY attempt_no DESC , dated_time_stamp DESC LIMIT 1";
            tbl_ce_evaluation_index ceindex = db.Database.SqlQuery<tbl_ce_evaluation_index>(csql01).FirstOrDefault();
            if (ceindex != null)
            {
                if (cpdays > 0)
                {
                    expiry = ceindex.dated_time_stamp.AddDays(cpdays);
                    if (expiry > DateTime.Now)
                    {
                        cpflag = false;
                    }
                    else
                    {
                        cpflag = true;
                    }
                }
            }
            return cpflag;
        }

    }
}