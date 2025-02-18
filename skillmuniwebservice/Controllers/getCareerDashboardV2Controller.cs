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
    public class getCareerDashboardV2Controller : ApiController
    {
        public m2ostnextserviceDbContext db = new m2ostnextserviceDbContext();
        public int ceIndex;

        public HttpResponseMessage Get(int UID, int OID, string trf)
        {
            CEDashboard dashboard = new CEDashboard();
            List<CEAssessment> ceList = new List<CEAssessment>();

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
            ceIndex = currentindex;
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
                    CEAssessment crow = new CEAssessment();
                    crow.career_evaluation_title = row.career_evaluation_title;
                    crow.career_evaluation_code = row.career_evaluation_code;
                    crow.ce_assessment_type = Convert.ToInt32(row.ce_assessment_type);
                    if (row.ce_assessment_type == 1) crow.cea_type = "SUL-MCA";
                    if (row.ce_assessment_type == 2) crow.cea_type = "Psychometric Assessment";
                    crow.job_points_for_ra = Convert.ToInt32(row.job_points_for_ra);
                    // string cesql04 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + " limit 1";
                    // JobPoint jPoint = db.Database.SqlQuery<JobPoint>(cesql04).FirstOrDefault();

                    string cesql05 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point,DATE_FORMAT(recorded_timestamp, '%d-%m-%Y') AS cetimestamp FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " AND id_ce_career_evaluation_master = " + row.id_ce_career_evaluation_master + "  GROUP BY attempt_no,cetimestamp ORDER BY attempt_no DESC LIMIT 3";
                    List<JobPointDated> jPointList = db.Database.SqlQuery<JobPointDated>(cesql05).ToList();
                    List<int> tList = new List<int>();
                    List<CEData> cedata = new List<CEData>();
                    for (int i = 0; i < 3; i++)
                    {
                        int val = 0;
                        CEData ced = new CEData();
                        if (jPointList.ElementAtOrDefault(i) != null)
                        {

                            ced.attempt_no = jPointList[i].attempt_no;
                            ced.cetimestamp = jPointList[i].cetimestamp;
                            ced.job_point = jPointList[i].job_point;
                        }
                        cedata.Add(ced);
                        tList.Add(val);
                    }
                    crow.CEAssessList = cedata;
                    ceList.Add(crow);
                }
            }

            dashboard.ceEvaluation = ceList;
            dashboard.last_attempt_no = lastindex;
            dashboard.latest_attempt_no = currentindex;
            //            string cesql06 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit WHERE id_user = " + UID + " AND id_organization = " + OID + " GROUP BY attempt_no order by attempt_no desc LIMIT 2";
            string cesql06 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + "  AND a.id_organization = " + OID + "  GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 2";

            List<JobPoint> jTotalList = db.Database.SqlQuery<JobPoint>(cesql06).ToList();
            if (jTotalList.ElementAtOrDefault(0) != null)
            {
                dashboard.ceCurrentScore = jTotalList[0].job_point;
            }
            if (jTotalList.ElementAtOrDefault(1) != null)
            {
                dashboard.cePreviousScore = jTotalList[1].job_point;
            }

            psyIndex = checkPsychometricEvaluationIndex(tile.id_ce_evaluation_tile, UID, OID);

            string ccesql01 = "SELECT b.id_ce_evalution_answer_key,b.key_code, b.answer_key, SUM(a.job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_evalution_answer_key b, tbl_ce_career_evaluation_master c WHERE  a.attempt_no = " + psyIndex + "  AND a.id_ce_career_evaluation_master = c.id_ce_career_evaluation_master AND a.id_ce_evalution_answer_key = b.id_ce_evalution_answer_key AND c.ce_assessment_type = 2 AND a.id_user = " + UID + " AND a.id_organization = " + OID + " GROUP BY b.key_code , b.id_ce_evalution_answer_key , b.answer_key ORDER BY job_point desc limit 3";
            List<CEAnswerKey> cDriver = db.Database.SqlQuery<CEAnswerKey>(ccesql01).ToList();
            List<RoleClass> ceRoles = new List<RoleClass>();
            if (cDriver.Count > 0)
            {
                string cdList = "";
                foreach (CEAnswerKey item in cDriver)
                {
                    cdList = cdList + "," + item.id_ce_evalution_answer_key;
                }
                cdList = cdList.TrimEnd(',');
                cdList = cdList.TrimStart(',');
                ceRoles = getEmploymentRoles(cdList, OID, UID);
                string sql01 = "SELECT * FROM tbl_ce_career_evaluation_master where status='A' and ce_assessment_type=2 and id_organization=" + OID + " and id_ce_evaluation_tile=" + tile.id_ce_evaluation_tile + " ";
                tbl_ce_career_evaluation_master cdAssessment = db.Database.SqlQuery<tbl_ce_career_evaluation_master>(sql01).FirstOrDefault();
                if (cdAssessment != null)
                {
                    dashboard.psyCrf = cdAssessment.career_evaluation_code;
                    dashboard.psyIndex = psyIndex;
                }
            }
            dashboard.ceRoles = ceRoles;
            dashboard.CareerDriver = cDriver;


            int sentpoints = 0;

            if (dashboard.ceCurrentStatus == "Completed")
            {
                sentpoints = dashboard.ceCurrentScore;
            }
            else
            {
                sentpoints = dashboard.cePreviousScore;
            }



            string ccesql02 = "SELECT a.id_ce_evaluation_jobrole, a.ce_industry_role, 0 AS counter FROM tbl_ce_evaluation_jobrole a, tbl_ce_evaluation_jobrole_user_mapping b WHERE a.id_ce_evaluation_jobrole = b.id_ce_evaluation_jobrole AND b.id_user = " + UID + " AND a.status = 'A'";
            List<RoleClass> preferedRole = new List<RoleClass>();
            preferedRole = db.Database.SqlQuery<RoleClass>(ccesql02).ToList();
            dashboard.preferedRole = preferedRole;
            dashboard.suggestedRole = ceRoles;
            List<RoleClass> allRole = new List<RoleClass>();
            allRole.AddRange(ceRoles);
            allRole.AddRange(preferedRole);



            List<CESuggestedCompany> suggestedRole = new List<CESuggestedCompany>();

            if (allRole.Count > 0)
            {
                suggestedRole = getSuggestedCompany(allRole, OID, UID, sentpoints);
            }
            dashboard.suggestedCompany = suggestedRole;
            /* ------Job roles start here---------------------------------------------------------------- */
            List<CEJobRoles> jroles = new List<CEJobRoles>();
            List<tbl_ce_industry_role> roles = new List<tbl_ce_industry_role>();
            string cwsql01 = "SELECT * FROM tbl_ce_industry_role where id_organization=" + OID + " ";
            roles = db.Database.SqlQuery<tbl_ce_industry_role>(cwsql01).ToList();

            foreach (tbl_ce_industry_role row in roles)
            {
                List<CEJobIndustry> jIndustry = new List<CEJobIndustry>();
                CEJobRoles item = new CEJobRoles();
                item.ce_industry_role = row.ce_industry_role;
                item.description = row.description;
                item.id_ce_industry_role = row.id_ce_industry_role;
                string cwsql02 = "SELECT * FROM tbl_ce_industry WHERE id_organization = " + OID + " AND id_ce_industry_role = " + row.id_ce_industry_role + " AND status = 'A'";
                List<tbl_ce_industry> industry = db.Database.SqlQuery<tbl_ce_industry>(cwsql02).ToList();

                foreach (tbl_ce_industry irow in industry)
                {
                    CEJobIndustry temp = new CEJobIndustry();
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
            TGCStandard tgcStandard = getTGCIndustry(OID, UID, sentpoints);

            dashboard.tgcStandard = tgcStandard;

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

        private List<RoleClass> getEmploymentRoles(string cdList, int OID, int UID)
        {
            string csql01 = "SELECT a.id_ce_evaluation_jobrole, a.ce_industry_role, COUNT(*) counter FROM tbl_ce_evaluation_jobrole a, tbl_ce_evaluation_role_answerkey_setup b, tbl_ce_evalution_answer_key_setup c, tbl_ce_evalution_answer_key d WHERE a.id_ce_evaluation_jobrole = b.id_ce_industry_role AND b.id_ce_evalution_answer_key_setup = c.id_ce_evalution_answer_key_setup AND LOWER(c.key_code) = LOWER(d.key_code) AND a.id_organization = b.id_organization AND b.id_organization = c.id_organization AND c.id_organization = d.id_organization AND a.id_organization = " + OID + " AND d.id_ce_evalution_answer_key IN(" + cdList + ") GROUP BY a.id_ce_evaluation_jobrole ORDER BY counter DESC LIMIT 3 ";
            List<RoleClass> roles1 = db.Database.SqlQuery<RoleClass>(csql01).ToList();
            string csql02 = "SELECT a.id_ce_evaluation_jobrole, ce_industry_role, 0 AS counter FROM tbl_ce_evaluation_jobrole a, tbl_ce_evaluation_jobrole_user_mapping b WHERE a.id_ce_evaluation_jobrole = b.id_ce_evaluation_jobrole AND id_user=" + UID + " ";
            List<RoleClass> roles2 = new List<RoleClass>();// db.Database.SqlQuery<RoleClass>(csql02).ToList();
            foreach (RoleClass item in roles2)
            {
                if (!(roles1.Any(t => t.id_ce_evaluation_jobrole == item.id_ce_evaluation_jobrole)))
                {
                    roles1.Add(item);
                }
            }
            return roles1;
        }

        //private List<cpIndustryRole> getSuggestedCompanyRole(List<RoleClass> roles, int OID, int jpoint)
        //{
        //    List<cpIndustryRole> roleList = new List<cpIndustryRole>();
        //    foreach (RoleClass row in roles)
        //    {
        //        cpIndustryRole tempRow = new cpIndustryRole();
        //        tempRow.cpRole = row;
        //        string jindustrysql = "SELECT id_ce_industry, ce_industry, role_job_point FROM tbl_ce_industry WHERE status = 'A' AND id_organization = " + OID + " AND id_ce_industry_role = " + row.id_ce_evaluation_jobrole + " AND role_job_point <= " + jpoint + " ";
        //        List<IndustyrRole> headerColumn = db.Database.SqlQuery<IndustyrRole>(jindustrysql).ToList();
        //        //string ocompanysql = "SELECT ce_company_name, job_point, @srno:=@srno + 1 orderno FROM tbl_ce_company_details, (SELECT @srno:=0) AS srno WHERE id_ce_industry IN (SELECT id_ce_industry FROM tbl_ce_industry WHERE id_organization =  " + OID + "  AND status = 'A' AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND role_job_point <=  " + jpoint + " ) AND id_organization = " + OID + " AND status = 'A'";
        //        string ocompanysql = "SELECT ce_company_name, job_point, 0 orderno FROM tbl_ce_company_details WHERE id_ce_industry IN (SELECT id_ce_industry FROM tbl_ce_industry WHERE id_organization =  " + OID + "  AND status = 'A' AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND role_job_point <=  " + jpoint + " ) AND id_organization = " + OID + " AND status = 'A'";
        //        List<CompanyRoles> headerRow = db.Database.SqlQuery<CompanyRoles>(ocompanysql).ToList();
        //        List<cpCompany> rowCompany = new List<cpCompany>();
        //        foreach (CompanyRoles crole in headerRow)
        //        {
        //            cpCompany cprow = new cpCompany();
        //            cprow.rowCompany = crole;
        //            List<IndustyrRole> rowIndustry = new List<IndustyrRole>();
        //            foreach (IndustyrRole irole in headerColumn)
        //            {
        //                string cesql = "SELECT * FROM tbl_ce_company_details WHERE status = 'A' AND id_organization = " + OID + " AND id_ce_industry_role =  " + row.id_ce_industry_role + "  AND id_ce_industry =  " + irole.id_ce_industry + "  AND LOWER(ce_company_name) = LOWER('" + crole.ce_company_name.Trim() + "')";
        //                tbl_ce_company_details company = db.Database.SqlQuery<tbl_ce_company_details>(cesql).FirstOrDefault();

        //                if (company != null)
        //                {
        //                    irole.company_job_point = company.job_point.ToString(); ;
        //                }
        //                else
        //                {
        //                    irole.company_job_point = "NA";
        //                }
        //                rowIndustry.Add(irole);
        //            }
        //            cprow.rowIndustry = rowIndustry;
        //            rowCompany.Add(cprow);
        //        }
        //        tempRow.cpCompany = rowCompany;
        //        roleList.Add(tempRow);
        //    }
        //    return roleList;
        //}

        private List<CESuggestedCompany> getSuggestedCompany(List<RoleClass> roles, int OID, int UID, int jpoint)
        {
            string cdList = "";
            foreach (RoleClass item in roles)
            {
                cdList = cdList + "," + item.id_ce_evaluation_jobrole;
            }
            cdList = cdList.TrimEnd(',');
            cdList = cdList.TrimStart(',');

            string csql01 = "SELECT a.ID_ORGANIZATION JOB_ID_ORGANIZATION, a.COMPANY_NAME FROM job_organization a, tbl_ce_evaluation_job_organization_setup b WHERE a.ID_ORGANIZATION = b.id_job_organization AND b.id_ce_evaluation_jobrole IN (" + cdList + ") GROUP BY a.ID_ORGANIZATION LIMIT 4";
            List<CompanyMaster> compMaste = db.Database.SqlQuery<CompanyMaster>(csql01).ToList();
            List<CESuggestedCompany> resCompany = new List<CESuggestedCompany>();
            foreach (CompanyMaster row in compMaste)
            {
                CESuggestedCompany company = new CESuggestedCompany();
                company.COMPANY_NAME = row.COMPANY_NAME;
                company.JOB_ID_ORGANIZATION = row.JOB_ID_ORGANIZATION;

                List<CompanyJobPoint> cpJobPoint = new List<CompanyJobPoint>();
                string csql02 = "SELECT b.id_ce_evaluation_jobrole, b.ce_industry_role, ce_role_code, a.organization_benchmark_jobpoint FROM tbl_ce_evaluation_job_organization_setup a, tbl_ce_evaluation_jobrole b WHERE a.id_ce_evaluation_jobrole = b.id_ce_evaluation_jobrole AND a.id_ce_evaluation_jobrole IN (" + cdList + ") GROUP BY a.id_ce_evaluation_jobrole,a.organization_benchmark_jobpoint ORDER BY a.organization_benchmark_jobpoint desc ";
                List<CompanyDetail> detail = db.Database.SqlQuery<CompanyDetail>(csql02).ToList();
                foreach (CompanyDetail crow in detail)
                {
                    CompanyJobPoint cjpoint = new CompanyJobPoint();
                    cjpoint = getCompanyJobPoint(row.JOB_ID_ORGANIZATION, crow, OID, UID);
                    cjpoint.roleData = new RoleClass();
                    cjpoint.roleData.id_ce_evaluation_jobrole = crow.id_ce_evaluation_jobrole;
                    cjpoint.roleData.ce_industry_role = crow.ce_industry_role;

                    List<CompanyCEJobSetup> setup = new List<CompanyCEJobSetup>();
                    string csql03 = "SELECT a.id_ce_career_evaluation_master,a.career_evaluation_title, a.career_evaluation_code, no_of_question, job_points_for_ra, ce_benchmark_jobpoint, CASE WHEN job_points_for_ra > 0 THEN (job_points_for_ra * no_of_question) ELSE no_of_question END highest_score FROM tbl_ce_career_evaluation_master a, tbl_ce_organization_ce_setup b WHERE a.ce_assessment_type = 1 AND a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master " +
                        " AND b.id_job_organization = " + row.JOB_ID_ORGANIZATION + " AND b.id_ce_evaluation_jobrole = " + crow.id_ce_evaluation_jobrole + " ";
                    setup = db.Database.SqlQuery<CompanyCEJobSetup>(csql03).ToList();
                    List<MyCEJobPoints> detailList = new List<MyCEJobPoints>();
                    string celist = "";
                    foreach (CompanyCEJobSetup irow in setup)
                    {
                        celist = celist + "," + irow.id_ce_career_evaluation_master;
                        MyCEJobPoints itemp = new MyCEJobPoints();
                        itemp.career_evaluation_title = irow.career_evaluation_title;
                        itemp.career_evaluation_code = irow.career_evaluation_code;
                        itemp.ce_benchmark_jobpoint = irow.ce_benchmark_jobpoint;
                        itemp.highest_score = irow.highest_score;
                        itemp.no_of_question = irow.no_of_question;
                        string csql011 = "SELECT attempt_no, SUM(job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + " AND a.id_organization = " + OID + "  AND attempt_no = " + ceIndex + "  AND a.id_ce_career_evaluation_master = " + irow.id_ce_career_evaluation_master + " GROUP BY attempt_no";
                        JobPoint mypoint = db.Database.SqlQuery<JobPoint>(csql011).FirstOrDefault();
                        if (mypoint != null)
                        {
                            itemp.my_score = mypoint.job_point;
                        }
                        else
                        {
                            itemp.my_score = 0;
                        }
                        //  string csql021 = "SELECT CASE WHEN (job_point > 0) THEN AVG(job_point) ELSE 0 END job_point, 0 attempt_no FROM (SELECT job_point, MAX(attempt_no), id_user FROM (SELECT a.id_user, attempt_no, SUM(job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1  AND a.id_ce_career_evaluation_master = " + irow.id_ce_career_evaluation_master + " AND a.id_organization = " + OID + " AND a.id_user NOT IN (" + UID + ") GROUP BY attempt_no , a.id_user,job_point ORDER BY attempt_no DESC) AS m1 GROUP BY m1.id_user,m1.job_point) AS m2";
                        //  JobPoint otherpoint = db.Database.SqlQuery<JobPoint>(csql021).FirstOrDefault();

                        int ojobpoint = new StoredProcedureModel().strp_get_other_ce_score(OID, UID, irow.id_ce_career_evaluation_master);
                        itemp.other_score = ojobpoint;

                        detailList.Add(itemp);
                    }

                    celist = celist.TrimEnd(',');
                    celist = celist.TrimStart(',');
                    if (celist != "")
                    {
                        List<CompanyCEJobSetup> bsetup = new List<CompanyCEJobSetup>();
                        string csql023 = "SELECT a.id_ce_career_evaluation_master, a.career_evaluation_title, a.career_evaluation_code, no_of_question, job_points_for_ra, ce_benchmark_jobpoint, CASE WHEN job_points_for_ra > 0 THEN (job_points_for_ra * no_of_question) ELSE no_of_question END highest_score FROM tbl_ce_career_evaluation_master a, tbl_ce_organization_ce_setup b WHERE a.ce_assessment_type = 1 AND a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.id_job_organization = 1 and a.id_ce_career_evaluation_master not in (" + celist + ") AND b.id_ce_evaluation_jobrole = " + crow.id_ce_evaluation_jobrole + " ";
                        bsetup = db.Database.SqlQuery<CompanyCEJobSetup>(csql023).ToList();
                        foreach (CompanyCEJobSetup irow in bsetup)
                        {
                            MyCEJobPoints itemp = new MyCEJobPoints();
                            itemp.career_evaluation_title = irow.career_evaluation_title;
                            itemp.career_evaluation_code = irow.career_evaluation_code;
                            itemp.ce_benchmark_jobpoint = irow.ce_benchmark_jobpoint;
                            itemp.highest_score = irow.highest_score;
                            itemp.no_of_question = irow.no_of_question;
                            string csql011 = "SELECT attempt_no, SUM(job_point) job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + " AND a.id_organization = " + OID + "  AND attempt_no = " + ceIndex + "  AND a.id_ce_career_evaluation_master = " + irow.id_ce_career_evaluation_master + " GROUP BY attempt_no";
                            JobPoint mypoint = db.Database.SqlQuery<JobPoint>(csql011).FirstOrDefault();
                            if (mypoint != null)
                            {
                                itemp.my_score = mypoint.job_point;
                            }
                            else
                            {
                                itemp.my_score = 0;
                            }
                            int ojobpoint = new StoredProcedureModel().strp_get_other_ce_score(OID, UID, irow.id_ce_career_evaluation_master);
                            itemp.other_score = ojobpoint;
                            detailList.Add(itemp);
                        }
                    }
                    cjpoint.ceJobPoints = detailList;
                    cpJobPoint.Add(cjpoint);
                }
                company.CESRoleList = cpJobPoint;
                resCompany.Add(company);
            }
            return resCompany;
        }

        private CompanyJobPoint getCompanyJobPoint(int CID, CompanyDetail RID, int OID, int UID)
        {
            CompanyJobPoint temp = new CompanyJobPoint();
            int cebenchmarkpoint = 0;
            int cemypoint = 0;
            int ceotherpoint = 30;
            int cehighestpoint = 0;
            string csql01 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, CASE WHEN SUM(job_point) > 0 THEN SUM(job_point) ELSE 0 END job_point FROM tbl_ce_evaluation_audit a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + " AND a.id_organization = " + OID + " GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 1";
                   csql01 = "SELECT CASE WHEN attempt_no > 0 THEN attempt_no ELSE 0 END attempt_no, ROUND(AVG(CASE WHEN ce_evaluation_result > 0 THEN (ce_evaluation_result / b.no_of_question) * 100 ELSE 0 END), 0) job_point FROM tbl_ce_evaluation_log a, tbl_ce_career_evaluation_master b WHERE a.id_ce_career_evaluation_master = b.id_ce_career_evaluation_master AND b.ce_assessment_type = 1 AND id_user = " + UID + " AND a.id_organization = " + OID + " GROUP BY attempt_no ORDER BY attempt_no DESC LIMIT 1";

            JobPoint mypoint = db.Database.SqlQuery<JobPoint>(csql01).FirstOrDefault();
            if (mypoint != null) cemypoint = mypoint.job_point;
            int ojobpoint = new StoredProcedureModel().strp_get_other_score(OID, UID);
            if (ojobpoint > 0)
            {
                ceotherpoint = ojobpoint;
            }
            string csql03 = "SELECT organization_benchmark_jobpoint job_point, 0 attempt_no FROM tbl_ce_evaluation_job_organization_setup WHERE id_job_organization = " + CID + " AND id_ce_evaluation_jobrole = " + RID.id_ce_evaluation_jobrole + "";
            JobPoint benchmarkpoint = db.Database.SqlQuery<JobPoint>(csql03).FirstOrDefault();
            if (benchmarkpoint != null) { cebenchmarkpoint = benchmarkpoint.job_point; }
            else
            {
                string csql04 = "SELECT organization_benchmark_jobpoint job_point, 0 attempt_no FROM tbl_ce_evaluation_job_organization_setup WHERE id_job_organization = 1 AND id_ce_evaluation_jobrole = " + RID.id_ce_evaluation_jobrole + "";
                JobPoint benchmarkpoint1 = db.Database.SqlQuery<JobPoint>(csql04).FirstOrDefault();
                if (benchmarkpoint1 != null) { cebenchmarkpoint = benchmarkpoint1.job_point; }
            }
            //  string csql05 = "SELECT SUM(no_of_question * job_points_for_ra) job_point, 0 attempt_no FROM tbl_ce_career_evaluation_master WHERE id_organization =  " + OID + " AND ce_assessment_type = 1 AND status = 'A'";
            // JobPoint hightestpoint = db.Database.SqlQuery<JobPoint>(csql05).FirstOrDefault();
            // if (hightestpoint != null) { cehighestpoint = hightestpoint.job_point; }
            cehighestpoint = 100;
            temp.MyJobScore = cemypoint;
            temp.OtherJobScore = ceotherpoint;
            temp.BenchmarkJobScore = cebenchmarkpoint;
            temp.HighestScore = cehighestpoint;
            return temp;
        }

        private TGCStandard getTGCIndustry(int OID, int UID, int jpoint)
        {
            TGCStandard tgcStandard = new TGCStandard();
            string csql01 = "SELECT a.id_ce_evaluation_jobindustry, a.ce_job_industry, a.ce_industry_code, b.benchmark_jobpoint job_point FROM tbl_ce_evaluation_jobindustry a, tbl_ce_evaluation_jobindustry_tgc_setup b, tbl_ce_evaluation_jobindustry_user_mapping c WHERE a.id_ce_evaluation_jobindustry = b.id_ce_evaluation_jobindustry AND a.id_ce_evaluation_jobindustry = c.id_ce_evaluation_jobindustry    AND a.id_organization = " + OID + "  AND c.id_user =" + UID + " AND benchmark_jobpoint >= " + jpoint + " ";
            List<IndustryStandard> standard = db.Database.SqlQuery<IndustryStandard>(csql01).ToList();
            int limit = 10;
            if (standard.Count > 0)
            {
                limit = 10 - standard.Count;
            }
            //string csql02 = "SELECT a.id_ce_evaluation_jobindustry, a.ce_job_industry, a.ce_industry_code, b.benchmark_jobpoint job_point FROM tbl_ce_evaluation_jobindustry a, tbl_ce_evaluation_jobindustry_tgc_setup b WHERE a.id_ce_evaluation_jobindustry = b.id_ce_evaluation_jobindustry   AND a.id_organization = " + OID + "  AND b.benchmark_jobpoint >= " + jpoint + " AND a.id_ce_evaluation_jobindustry NOT IN (SELECT id_ce_evaluation_jobindustry FROM tbl_ce_evaluation_jobindustry_user_mapping WHERE id_user=" + UID + ") LIMIT " + limit;
            //List<IndustryStandard> standard1 = db.Database.SqlQuery<IndustryStandard>(csql02).ToList();
            //standard.AddRange(standard1);
            tgcStandard.MyScore = standard;
            //string csql03 = "SELECT a.id_ce_evaluation_jobindustry, a.ce_job_industry, a.ce_industry_code, b.benchmark_jobpoint job_point FROM tbl_ce_evaluation_jobindustry a, tbl_ce_evaluation_jobindustry_tgc_setup b WHERE a.id_ce_evaluation_jobindustry = b.id_ce_evaluation_jobindustry AND b.benchmark_jobpoint >= " + jpoint + " LIMIT 10";
            string csql03 = "SELECT a.id_ce_evaluation_jobindustry, a.ce_job_industry, a.ce_industry_code, b.benchmark_jobpoint job_point FROM tbl_ce_evaluation_jobindustry a, tbl_ce_evaluation_jobindustry_tgc_setup b WHERE a.id_ce_evaluation_jobindustry = b.id_ce_evaluation_jobindustry AND b.benchmark_jobpoint >= " + jpoint + "  LIMIT 10";
            List<IndustryStandard> benchmark = db.Database.SqlQuery<IndustryStandard>(csql03).ToList();
            tgcStandard.BenchmarkScore = benchmark;
            return tgcStandard;
        }

    }
}