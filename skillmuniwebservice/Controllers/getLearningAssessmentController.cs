using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;
using System.Configuration;
namespace m2ostnextservice.Controllers
{
    public class getLearningAssessmentController : ApiController
    {

        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int CID, int UID, int OID)
        {
            DateTime current = System.DateTime.Now;
            List<AssessmentList> list = new List<AssessmentList>();

            List<AssessmentList> uList = new List<AssessmentList>();
            List<AssessmentList> aList = new List<AssessmentList>();

            tbl_user user = db.tbl_user.Where(t => t.ID_USER == UID).FirstOrDefault();
            string assessSql = "select * from tbl_assessment_sheet where status='A' and id_organization=" + OID + " and id_assessment_sheet in (select distinct id_assessment_sheet from tbl_assessment_categoty_mapping where id_category in (select id_category  from tbl_category where id_organization=" + OID + "))";
            List<tbl_assessment_sheet> tSheet = db.tbl_assessment_sheet.SqlQuery(assessSql).ToList();

            string assSql = "select distinct * from tbl_assessment_user_assignment where id_organization=" + OID + " AND id_user=" + UID + "  and expire_date >=DATE_FORMAT(NOW(),'%Y-%m-%d %H:%i')";
            List<tbl_assessment_user_assignment> assSheet = db.tbl_assessment_user_assignment.SqlQuery(assSql).ToList();

            if (assSheet.Count > 0)
            {
                assSheet = assSheet.OrderBy(t => t.expire_date).ToList();
            }

            foreach (tbl_assessment_user_assignment item in assSheet)
            {

                string sqls = "select * from tbl_assessment_sheet where status='A' and id_organization=" + OID + " and id_assesment =" + item.id_assessment + "";
                tbl_assessment_sheet iSheet = db.tbl_assessment_sheet.SqlQuery(sqls).FirstOrDefault();
                if (iSheet != null)
                {

                AssessmentList tmp = new AssessmentList();

                tbl_assessment assessment = db.tbl_assessment.Where(t => t.status == "A" && t.id_assessment == iSheet.id_assesment).FirstOrDefault();
                if (assessment != null)
                {
                    if (DateTime.Compare(item.expire_date.Value.AddDays(1), current) > 0)
                    {
                        if (DateTime.Compare(assessment.assess_ended.Value.AddDays(1), current) > 0)
                        {
                            if (assessment.assessment_type == 1)
                            {
                                tmp.id_assessment_sheet = iSheet.id_assessment_sheet;
                                tmp.id_assessment = assessment.id_assessment;
                                tmp.assessment_name = assessment.assessment_title;
                                tmp.assessment_description = assessment.assesment_description;
                                tmp.expiry_date = item.expire_date.Value.ToString("dd-MMM-yyyy");
                                uList.Add(tmp);

                            }
                        }
                    }
                } }
            }

            foreach (tbl_assessment_sheet local in tSheet)
            {
                AssessmentList tmp = new AssessmentList();
                tbl_assessment assessment = db.tbl_assessment.Where(t => t.status == "A" && t.id_assessment == local.id_assesment).FirstOrDefault();
                if (assessment != null)
                {
                    if (DateTime.Compare(assessment.assess_ended.Value.AddDays(1), current) > 0)
                    {
                        if (assessment.assessment_type == 1)
                        {
                            tmp.id_assessment_sheet = local.id_assessment_sheet;
                            tmp.id_assessment = assessment.id_assessment;
                            tmp.assessment_name = assessment.assessment_title;
                            tmp.assessment_description = assessment.assesment_description;
                            tmp.expiry_date = assessment.assess_ended.Value.ToString("dd-MMM-yyyy");
                            aList.Add(tmp);
                        }
                    }
                }
            }
            if (aList.Count > 0)
            {
                aList = aList.OrderBy(t => DateTime.Parse(t.expiry_date)).ThenBy(t => t.assessment_name).ToList();
            }

            foreach (AssessmentList item in uList)
            {
                list.Add(item);
            }
            foreach (AssessmentList item in aList)
            {
                list.Add(item);
            }
            return Request.CreateResponse(HttpStatusCode.OK, list);

        }
    }
}
