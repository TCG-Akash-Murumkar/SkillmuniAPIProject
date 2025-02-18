using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class B2CAPIController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int IDS, string VT, int OID)
        {
            List<B2CResponse> response = new List<B2CResponse>();
            if (VT == "TYPE1")
            {
                //right answers
                string sql = "select * from tbl_brief_b2c_right_audit where id_organization=" + OID + " AND id_brief_b2c_right_audit >" + IDS + " ";
                List<tbl_brief_b2c_right_audit> log = db.tbl_brief_b2c_right_audit.SqlQuery(sql).ToList();
                foreach (tbl_brief_b2c_right_audit row in log)
                {
                    tbl_user user = db.tbl_user.Where(t => t.ID_USER == row.id_user).FirstOrDefault();
                    if (user != null)
                    {
                        B2CResponse temp = new B2CResponse();
                        temp.IDS = row.id_brief_b2c_right_audit;
                        temp.BID = 0;
                        temp.OID = Convert.ToInt32(row.id_organization);
                        temp.timestamp = row.datetime_stamp.Value.ToString("yyyy-MM-dd HH:mm");
                        temp.EMPID = user.EMPLOYEEID;
                        temp.UID = Convert.ToInt32(row.id_user);
                        temp.VALUE = Convert.ToInt32(row.value_count);
                        temp.CLEVEL = Convert.ToInt32(row.question_complexity);
                        response.Add(temp);
                    }
                }
            }
            if (VT == "TYPE2")
            {
                //right answers with complexity
                string sql = "select * from tbl_brief_b2c_right_audit where  id_organization=" + OID + " AND id_brief_b2c_right_audit >" + IDS;
                List<tbl_brief_b2c_right_audit> log = db.tbl_brief_b2c_right_audit.SqlQuery(sql).ToList();
                foreach (tbl_brief_b2c_right_audit row in log)
                {
                    tbl_user user = db.tbl_user.Where(t => t.ID_USER == row.id_user).FirstOrDefault();
                    if (user != null)
                    {
                        B2CResponse temp = new B2CResponse();
                        temp.IDS = row.id_brief_b2c_right_audit;
                        temp.BID = 0;
                        temp.OID = Convert.ToInt32(row.id_organization);
                        temp.timestamp = row.datetime_stamp.Value.ToString("yyyy-MM-dd HH:mm");
                        temp.EMPID = user.EMPLOYEEID;
                        temp.UID = Convert.ToInt32(row.id_user);
                        temp.VALUE = Convert.ToInt32(row.value_count);
                        temp.CLEVEL = Convert.ToInt32(row.question_complexity);
                        response.Add(temp);
                    }
                }
            }
            if (VT == "TYPE3")
            {
                //Brief score
                string sql = "SELECT * FROM tbl_brief_b2c_score_audit where id_organization=" + OID + " AND  id_brief_b2c_score_audit >" + IDS;
                List<tbl_brief_b2c_score_audit> log = db.tbl_brief_b2c_score_audit.SqlQuery(sql).ToList();
                foreach (tbl_brief_b2c_score_audit row in log)
                {
                    tbl_user user = db.tbl_user.Where(t => t.ID_USER == row.id_user).FirstOrDefault();
                    if (user != null)
                    {
                        B2CResponse temp = new B2CResponse();
                        temp.IDS = row.id_brief_b2c_score_audit;
                        temp.BID = 0;
                        temp.OID = Convert.ToInt32(row.id_organization);
                        temp.timestamp = row.datetime_stamp.Value.ToString("yyyy-MM-dd HH:mm");
                        temp.EMPID = user.EMPLOYEEID;
                        temp.UID = Convert.ToInt32(row.id_user);
                        temp.VALUE = Convert.ToInt32(row.value_count);
                        temp.CLEVEL = 0;
                        response.Add(temp);
                    }
                }
            }
            if (VT == "TYPE4")
            {
                //Brief Read counter
                string sql = "SELECT * FROM tbl_brief_read_status where id_organization=" + OID + " AND  read_status=1 and id_brief_read_status > " + IDS;
                List<tbl_brief_read_status> log = db.tbl_brief_read_status.SqlQuery(sql).ToList();
                foreach (tbl_brief_read_status row in log)
                {
                    tbl_user user = db.tbl_user.Where(t => t.ID_USER == row.id_user).FirstOrDefault();
                    if (user != null)
                    {
                        B2CResponse temp = new B2CResponse();
                        temp.IDS = row.id_brief_read_status;
                        temp.BID = 0;
                        temp.OID = Convert.ToInt32(row.id_organization);
                        temp.timestamp = row.read_datetime.Value.ToString("yyyy-MM-dd HH:mm");
                        temp.EMPID = user.EMPLOYEEID;
                        temp.UID = Convert.ToInt32(row.id_user);
                        temp.VALUE = Convert.ToInt32(row.read_status);
                        temp.CLEVEL = 0;
                        response.Add(temp);
                    }
                }
            }
            if (VT == "ORLIST")
            {
                string osql = "select * from tbl_organization where status='A' ";
                List<B2COrg> orgList = new BriefModel().getOrganizationList(osql);

                return Request.CreateResponse(HttpStatusCode.OK, orgList);
            }
            if (VT == "QOMPLEX")
            {
                List<B2COMPLEX> cList = new List<B2COMPLEX>();
                string osql = "select * from tbl_brief_question_complexity where status='A' ";
                List<tbl_brief_question_complexity> orgList = db.tbl_brief_question_complexity.SqlQuery(osql).ToList();
                foreach (tbl_brief_question_complexity row in orgList)
                {
                    B2COMPLEX temp = new B2COMPLEX();
                    temp.CID = Convert.ToInt32(row.question_complexity);
                    temp.COMPLEX = row.question_complexity_label;
                    cList.Add(temp);
                }
                string strCom = JsonConvert.SerializeObject(cList);
                return Request.CreateResponse(HttpStatusCode.OK, strCom);
            }
            if (VT == "SBORG")
            {
                List<B2COMPLEX> cList = new List<B2COMPLEX>();

                string rmsql = "SELECT a.id_user, b.FIRSTNAME,b.EMAIL, a.USERID, a.PASSWORD, a.EMPLOYEEID, a.user_department, a.user_designation, a.user_function, a.user_grade, a.reporting_manager FROM tbl_user a, tbl_profile b WHERE a.ID_USER = b.ID_USER AND ID_ORGANIZATION = " + IDS + " AND a.ID_USER IN (SELECT DISTINCT reporting_manager FROM tbl_user WHERE ID_ORGANIZATION = " + IDS + ")";
                List<TRANSUSER> rmuser = new BriefModel().getAPITUserList(rmsql, 4);
                string emsql = "SELECT a.id_user,b.FIRSTNAME,b.EMAIL, a.USERID, a.PASSWORD, a.EMPLOYEEID, a.user_department, a.user_designation, a.user_function, a.user_grade, a.reporting_manager FROM tbl_user a, tbl_profile b WHERE a.ID_USER = b.ID_USER AND ID_ORGANIZATION = " + IDS + " AND a.reporting_manager > 0 AND a.ID_USER NOT IN (SELECT DISTINCT reporting_manager FROM tbl_user WHERE ID_ORGANIZATION = " + IDS + ")";
                List<TRANSUSER> emuser = new BriefModel().getAPITUserList(emsql, 6);

                rmuser.AddRange(emuser);

                return Request.CreateResponse(HttpStatusCode.OK, rmuser);
            }
            //string strResponse = JsonConvert.SerializeObject(response);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}