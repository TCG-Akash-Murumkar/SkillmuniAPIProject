using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace m2ostnextservice.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class getCEMyDashboardController : ApiController
    {
        public m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();

        public HttpResponseMessage Get(int UID, int OID, string trf)
        {
            CEDashboardT dashboard = new CEDashboardT();
            List<CEAssessmentT> ceList = new List<CEAssessmentT>();

            int currentindex = 0;
            int lastindex = 0;
            int psyIndex = 0;
            bool cFlag = false;
            bool pFlag = false;
            string cesql01 = "SELECT * FROM tbl_ce_evaluation_tile where id_organization=" + OID + " and ce_evaluation_code='" + trf + "'";
            tbl_ce_evaluation_tile tile = db.Database.SqlQuery<tbl_ce_evaluation_tile>(cesql01).FirstOrDefault();
            dashboard.tile = tile;
            string cesql03 = "select * from tbl_ce_career_evaluation_master where id_ce_evaluation_tile=" + tile.id_ce_evaluation_tile + " AND status='A' order by ordering_sequence_number ";
            List<tbl_ce_career_evaluation_master> cmaster = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(cesql03).ToList();

            currentindex = checkCurrentIndex(tile.id_ce_evaluation_tile, UID, OID);

            if (currentindex > 0)
            {
                lastindex = currentindex - 1;
            }

            /* for current status ----------------------------------------------------------------------- */

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
            /*  ----------------------------------------------------------------------- */

            /* for previous status ----------------------------------------------------------------------- */
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

            List<CECategory> catList = new List<CECategory>();
            foreach (tbl_ce_career_evaluation_master row in cmaster)
            {
                if (row.ce_assessment_type == 1)
                {
                    CEAssessmentT crow = new CEAssessmentT();
                    crow.career_evaluation_title = row.career_evaluation_title;
                    crow.career_evaluation_code = row.career_evaluation_code;
                    crow.ce_assessment_type = Convert.ToInt32(row.ce_assessment_type);
                    if (row.ce_assessment_type == 1) crow.cea_type = "SUL-MCA";
                    if (row.ce_assessment_type == 2) crow.cea_type = "Psychometric Assessment";
                    crow.job_points_for_ra = Convert.ToInt32(row.job_points_for_ra);
                    // string cesql04 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + " limit 1";
                    // JobPoint jPoint = db.Database.SqlQuery<JobPoint>(cesql04).FirstOrDefault();

                    string cesql05 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + "  GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 3";
                    List<JobPointT> jPointList = db.Database.SqlQuery<JobPointT>(cesql05).ToList();
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
                    crow.CEAssessList = tList;
                    ceList.Add(crow);
                }
            }

            dashboard.ceEvaluation = ceList;
            dashboard.last_attempt_no = lastindex;
            dashboard.latest_attempt_no = currentindex;
            //            string cesql06 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " GROUP BY attempt_no order by attempt_no desc LIMIT 2";
            string cesql06 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + "  AND a.id_organization = " + OID + "  GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 2";

            List<JobPointT> jTotalList = db.Database.SqlQuery<JobPointT>(cesql06).ToList();
            if (jTotalList.ElementAtOrDefault(0) != null)
            {
                dashboard.ceCurrentScore = jTotalList[0].job_point;
            }
            if (jTotalList.ElementAtOrDefault(1) != null)
            {
                dashboard.cePreviousScore = jTotalList[1].job_point;
            }

            psyIndex = checkPsychometricEvaluationIndex(tile.id_ce_evaluation_tile, UID, OID);

            string ccesql01 = "SELECT b.id_ce_evalution_answer_key,b.key_code, b.answer_key, SUM(a.job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_evalution_answer_key b, tbl_ce_career_evaluation_master c WHERE  a.attempt_no = " + psyIndex + "  AND a.id_ce_career_evaluation_master = c.id_ce_career_evaluation_master AND a.id_ce_evalution_answer_key = b.id_ce_evalution_answer_key AND c.ce_assessment_type = 2 AND a.id_user = " + UID + " AND a.id_organization = " + OID + " GROUP BY b.key_code , b.id_ce_evalution_answer_key , b.answer_key  ORDER BY job_point desc limit 3";
            List<CEAnswerKeyT> cDriver = db.Database.SqlQuery<CEAnswerKeyT>(ccesql01).ToList();
            List<RoleClassT> ceRoles = new List<RoleClassT>();
            if (cDriver.Count > 0)
            {
                string cdList = "";
                foreach (CEAnswerKeyT item in cDriver)
                {
                    cdList = cdList + "," + item.id_ce_evalution_answer_key;
                }
                cdList = cdList.TrimEnd(',');
                cdList = cdList.TrimStart(',');
                ceRoles = getEmploymentRoles(cdList, OID);
            }
            dashboard.ceRoles = ceRoles;
            dashboard.CareerDriver = cDriver;

            List<cpIndustryRoleT> suggestedRole = new List<cpIndustryRoleT>();

            int sentpoints = 0;

            if (dashboard.ceCurrentStatus == "Completed")
            {
                sentpoints = dashboard.ceCurrentScore;
            }
            else
            {
                sentpoints = dashboard.cePreviousScore;
            }
            suggestedRole = getSuggestedCompanyRole(ceRoles, OID, sentpoints);
            dashboard.suggestedCompany = suggestedRole;
            /* ------Job roles start here---------------------------------------------------------------- */
            List<CEJobRolesT> jroles = new List<CEJobRolesT>();
            List<tbl_ce_industry_role> roles = new List<tbl_ce_industry_role>();
            string cwsql01 = "SELECT * FROM tbl_ce_industry_role where id_organization=" + OID + " ";
            roles = db.Database.SqlQuery<tbl_ce_industry_role>(cwsql01).ToList();

            foreach (tbl_ce_industry_role row in roles)
            {
                List<CEJobIndustryT> jIndustry = new List<CEJobIndustryT>();
                CEJobRolesT item = new CEJobRolesT();
                item.ce_industry_role = row.ce_industry_role;
                item.description = row.description;
                item.id_ce_industry_role = row.id_ce_industry_role;
                string cwsql02 = "SELECT * FROM tbl_ce_industry WHERE id_organization = " + OID + " AND id_ce_industry_role = " + row.id_ce_industry_role + " AND status = 'A'";
                List<tbl_ce_industry> industry = db.Database.SqlQuery<tbl_ce_industry>(cwsql02).ToList();

                foreach (tbl_ce_industry irow in industry)
                {
                    CEJobIndustryT temp = new CEJobIndustryT();
                    temp.id_ce_industry = irow.id_ce_industry;
                    temp.id_ce_industry_role = irow.id_ce_industry_role;
                    temp.role_job_point = irow.role_job_point;
                    temp.ce_industry = irow.ce_industry;
                    temp.id_organization = irow.id_organization;
                    jIndustry.Add(temp);
                }
                item.Industry = jIndustry;
                jroles.Add(item);
            }
            dashboard.jobRoles = jroles;
            /* ---------------------------------------------------------------------- */

            return Request.CreateResponse(HttpStatusCode.OK, dashboard);
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
                if (totalEVCount == doneEVCount)
                {
                    cFlag = true;
                }
            }

            return cFlag;
        }

        private int checkPsychometricEvaluationIndex(int ctid, int UID, int OID)
        {
            bool aFlag = false;
            int ilog = 0;
            string chsql01 = "SELECT * FROM tbl_ce_evaluation_index WHERE id_user = " + UID + " AND id_organization =  " + OID + "  AND id_ce_career_evaluation_master IN (SELECT id_ce_career_evaluation_master FROM tbl_ce_career_evaluation_master WHERE id_organization =  " + OID + "  AND id_ce_evaluation_tile =  " + ctid + "  AND ce_assessment_type = 2) ORDER BY attempt_no DESC LIMIT 1";
            tbl_ce_evaluation_index index = db.Database.SqlQuery<tbl_ce_evaluation_index>(chsql01).FirstOrDefault();
            if (index != null)
            {
                ilog = index.attempt_no;
            }
            return ilog;
        }

        private List<RoleClassT> getEmploymentRoles(string cdList, int OID)
        {
            string csql01 = "SELECT a.id_ce_industry_role, a.ce_industry_role, COUNT(*) counter FROM tbl_ce_industry_role a, tbl_ce_evaluation_role_answerkey_setup b, tbl_ce_evalution_answer_key_setup c, tbl_ce_evalution_answer_key d " +
                "WHERE a.id_ce_industry_role = b.id_ce_industry_role AND b.id_ce_evalution_answer_key_setup = c.id_ce_evalution_answer_key_setup AND LOWER(c.key_code) = LOWER(d.key_code) AND a.id_organization = b.id_organization AND b.id_organization = c.id_organization " +
                "AND c.id_organization = d.id_organization AND a.id_organization = " + OID + " AND d.id_ce_evalution_answer_key IN (" + cdList + ") GROUP BY a.id_ce_industry_role ORDER BY counter DESC LIMIT 4";
            List<RoleClassT> roles = db.Database.SqlQuery<RoleClassT>(csql01).ToList();
            return roles;
        }

        private List<cpIndustryRoleT> getSuggestedCompanyRole(List<RoleClassT> roles, int OID, int jpoint)
        {
            List<cpIndustryRoleT> roleList = new List<cpIndustryRoleT>();
            foreach (RoleClassT row in roles)
            {
                cpIndustryRoleT tempRow = new cpIndustryRoleT();
                tempRow.cpRole = row;
                string jindustrysql = "SELECT id_ce_industry, ce_industry, role_job_point FROM tbl_ce_industry WHERE status = 'A' AND id_organization = " + OID + " AND id_ce_industry_role = " + row.id_ce_industry_role + " AND role_job_point <= " + jpoint + " ";
                List<IndustyrRoleT> headerColumn = db.Database.SqlQuery<IndustyrRoleT>(jindustrysql).ToList();
                //string ocompanysql = "SELECT ce_company_name, job_point, @srno:=@srno + 1 orderno FROM tbl_ce_company_details, (SELECT @srno:=0) AS srno WHERE id_ce_industry IN (SELECT id_ce_industry FROM tbl_ce_industry WHERE id_organization =  " + OID + "  AND status = 'A' AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND role_job_point <=  " + jpoint + " ) AND id_organization = " + OID + " AND status = 'A'";
                string ocompanysql = "SELECT ce_company_name, job_point, 0 orderno FROM tbl_ce_company_details WHERE id_ce_industry IN (SELECT id_ce_industry FROM tbl_ce_industry WHERE id_organization =  " + OID + "  AND status = 'A' AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND role_job_point <=  " + jpoint + " ) AND id_organization = " + OID + " AND status = 'A'";
                List<CompanyRolesT> headerRow = db.Database.SqlQuery<CompanyRolesT>(ocompanysql).ToList();
                List<cpCompanyT> rowCompany = new List<cpCompanyT>();
                foreach (CompanyRolesT crole in headerRow)
                {
                    cpCompanyT cprow = new cpCompanyT();
                    cprow.rowCompany = crole;
                    List<IndustyrRoleT> rowIndustry = new List<IndustyrRoleT>();
                    foreach (IndustyrRoleT irole in headerColumn)
                    {
                        string cesql = "SELECT * FROM tbl_ce_company_details WHERE status = 'A' AND id_organization = " + OID + " AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND id_ce_industry =  " + irole.id_ce_industry + "  AND LOWER(ce_company_name) = LOWER('" + crole.ce_company_name.Trim() + "')";
                        tbl_ce_company_details company = db.Database.SqlQuery<tbl_ce_company_details>(cesql).FirstOrDefault();

                        if (company != null)
                        {
                            irole.company_job_point = company.job_point.ToString(); ;
                        }
                        else
                        {
                            irole.company_job_point = "NA";
                        }
                        rowIndustry.Add(irole);
                    }
                    cprow.rowIndustry = rowIndustry;
                    rowCompany.Add(cprow);
                }
                tempRow.cpCompany = rowCompany;
                roleList.Add(tempRow);
            }
            return roleList;
        }
    }

    public class RoleClassT
    {
        public int id_ce_industry_role { get; set; }
        public string ce_industry_role { get; set; }
        public int counter { get; set; }
    }

    public class cpIndustryRoleT
    {
        public RoleClassT cpRole { get; set; }
        public List<cpCompanyT> cpCompany { get; set; }
    }

    public class cpCompanyT
    {
        public CompanyRolesT rowCompany { get; set; }
        public List<IndustyrRoleT> rowIndustry { get; set; }
    }

    public class CompanyDetailT
    {
        public RoleClass roleList { get; set; }
        public List<IndustyrRoleT> RowHeaders { get; set; }
    }

    public class IndustyrRoleT
    {
        public int id_ce_industry { get; set; }
        public string ce_industry { get; set; }
        public int role_job_point { get; set; }
        public string company_job_point { get; set; }
    }

    public class CompanyRolesT
    {
        public string ce_company_name { get; set; }
        public int job_point { get; set; }
        public int orderno { get; set; }
    }

    public class CEDashboardT
    {
        public tbl_ce_evaluation_tile tile { get; set; }
        public int latest_attempt_no { get; set; }
        public int last_attempt_no { get; set; }
        public List<CEAnswerKeyT> CareerDriver { get; set; }
        public List<RoleClassT> ceRoles { get; set; }
        public List<CEAssessmentT> ceEvaluation { get; set; }
        public int ceCurrentScore { get; set; }
        public int cePreviousScore { get; set; }
        public string ceCurrentStatus { get; set; }
        public string cePreviousStatus { get; set; }
        public double ceCurrentPercentage { get; set; }
        public List<CEJobRolesT> jobRoles { get; set; }
        public List<cpIndustryRoleT> suggestedCompany { get; set; }
    }

    public class CEAssessmentT
    {
        public string career_evaluation_title { get; set; }
        public string career_evaluation_code { get; set; }
        public int ce_assessment_type { get; set; }
        public string cea_type { get; set; }
        public int job_points_for_ra { get; set; }
        public List<int> CEAssessList { get; set; }
    }

    public class CEAnswerKeyT
    {
        public int id_ce_evalution_answer_key { get; set; }
        public string answer_key { get; set; }
        public string key_code { get; set; }
        public int job_point { get; set; }
    }

    public class JobPointT
    {
        public int job_point { get; set; }
        public int attempt_no { get; set; }
    }

    public class CEJobRolesT
    {
        public int id_ce_industry_role { get; set; }
        public string ce_industry_role { get; set; }
        public string description { get; set; }
        public List<CEJobIndustryT> Industry { get; set; }
    }

    public class CEJobIndustryT
    {
        public int id_ce_industry { get; set; }
        public int id_organization { get; set; }
        public string ce_industry { get; set; }
        public int id_ce_industry_role { get; set; }
        public int role_job_point { get; set; }
    }
}