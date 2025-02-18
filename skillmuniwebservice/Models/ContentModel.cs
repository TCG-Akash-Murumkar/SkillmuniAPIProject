using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace m2ostnextservice.Models
{
    public class ContentModel
    {
        private MySqlConnection connection = null;
        private db_m2ostEntities db = new db_m2ostEntities();

        public ContentInfo CheckContentAccess(int conId, int userid, int orgid)
        {
            bool flag = false;
            ContentInfo info = new ContentInfo();
            info.id_user = userid;
            tbl_user user = db.tbl_user.Where(t => t.ID_USER == userid).FirstOrDefault();
            string rolSql = "select * from tbl_csst_role where id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + userid + ")";
            List<tbl_csst_role> role = db.tbl_csst_role.SqlQuery(rolSql).ToList();
            string uStr = "";
            foreach (tbl_csst_role item in role)
            {
                uStr += item.id_csst_role + ",";
            }
            uStr = uStr.TrimEnd(',');
            string additional = "";
            if (uStr == "")
            {
                additional = " AND id_user=" + userid + "";
            }
            else
            {
                additional = " AND (id_role in (" + uStr + ") or id_user=" + userid + ")";
            }

            string role_sql = "select * from tbl_content_role_mapping where id_content=" + conId + " and id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + userid + ")";
            tbl_content_role_mapping roleValidation = db.tbl_content_role_mapping.SqlQuery(role_sql).FirstOrDefault();
            if (roleValidation != null)
            {
                flag = true;
                string content_sql = "select * from tbl_content_organization_mapping  where id_content =" + conId + " AND id_organization=" + orgid; //id_category in (select id_category from tbl_content_organization_mapping ) " + additional;
                tbl_content_organization_mapping contentMap = db.tbl_content_organization_mapping.SqlQuery(content_sql).FirstOrDefault();
                if (contentMap != null)
                {
                    info.id_content = contentMap.id_content;
                    info.id_category = contentMap.id_category;
                    info.id_organization = contentMap.id_organization;
                    info.status = "A";

                }
            }

            string program_sql = "select * from tbl_content_program_mapping where id_user=" + userid + " and id_organization=" + orgid + " and id_category in (select id_category from tbl_content_organization_mapping where id_content =" + conId + " ) ";
            tbl_content_program_mapping programMap = db.tbl_content_program_mapping.SqlQuery(program_sql).FirstOrDefault();
            if (programMap != null)
            {
                flag = true;
                info.id_content = conId;
                info.id_category = Convert.ToInt32(programMap.id_category);
                info.id_organization = Convert.ToInt32(programMap.id_organization);
                info.status = "A";
                return info;
            }

            string userbased_sql = "select * from tbl_content_user_assisgnment where id_content =" + conId + " and id_organization=" + orgid + " and  id_user=" + userid + "";
            tbl_content_user_assisgnment userbasedProgram = db.tbl_content_user_assisgnment.SqlQuery(userbased_sql).FirstOrDefault();

            if (userbasedProgram != null)
            {
                flag = true;
                info.id_content = conId;
                info.id_category = Convert.ToInt32(userbasedProgram.id_category);
                info.id_organization = Convert.ToInt32(userbasedProgram.id_organization);
                info.status = "A";
                return info;
            }

            string sqlpg = "";
            sqlpg += " select * from tbl_content_organization_mapping where id_content=" + conId + " and id_category in ( SELECT distinct e.id_category FROM tbl_game_creation a LEFT JOIN ";
            sqlpg += " tbl_game_group_association b LEFT JOIN tbl_game_group c ON b.id_game_group = c.id_game_group ON a.id_game = b.id_game ";
            sqlpg += " AND b.association_type = 2 LEFT JOIN tbl_game_group_participatant d ON c.id_game_group = d.id_game_group ";
            sqlpg += " left join tbl_game_process_path e on a.id_game=e.id_game WHERE ";
            sqlpg += " a.status = 'A' AND d.id_user = " + userid + " AND a.id_organisation = " + orgid + " and e.element_type=1 )";

            tbl_content_organization_mapping game_program = db.tbl_content_organization_mapping.SqlQuery(sqlpg).FirstOrDefault();
            if (game_program != null)
            {
                flag = true;
                info.id_content = conId;
                info.id_category = Convert.ToInt32(game_program.id_category);
                info.id_organization = Convert.ToInt32(game_program.id_organization);
                info.status = "A";
                return info;
            }


            string sqlpu = "";
            sqlpu += " select * from tbl_content_organization_mapping where id_content=" + conId + " and id_category in (  SELECT  DISTINCT e.id_category FROM tbl_game_creation a ";
            sqlpu += " LEFT JOIN tbl_game_group_association b ON a.id_game = b.id_game AND b.association_type = 1 LEFT JOIN tbl_game_process_path e ON a.id_game = e.id_game ";
            sqlpu += " WHERE b.id_user = " + userid + " AND a.id_organisation = " + orgid + " AND a.status = 'A' AND e.element_type = 1 ) ";

            tbl_content_organization_mapping game_program_1 = db.tbl_content_organization_mapping.SqlQuery(sqlpu).FirstOrDefault();
            if (game_program_1 != null)
            {
                flag = true;
                info.id_content = conId;
                info.id_category = Convert.ToInt32(game_program_1.id_category);
                info.id_organization = Convert.ToInt32(game_program_1.id_organization);
                info.status = "A";
                return info;
            }

            if (!flag)
            {
                info.id_content = conId;
                info.id_category = 0;
                info.id_organization = 0;
                info.status = "F";
            }




            return info;
        }

        public List<tbl_content> getContentListFromCategory(int cid, int oid, int uid)
        {
            List<tbl_content> contentList = new List<tbl_content>();
            List<string> contentids = new List<string>();
            tbl_category category = db.tbl_category.Where(t => t.ID_CATEGORY == cid && t.STATUS == "A").FirstOrDefault();
            if (category != null)
            {

                string userbased_sql = "SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + category.ID_CATEGORY + " AND id_user=" + uid + " AND id_organization=" + oid + ")";

                List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
                foreach (tbl_content item in userbasedContent)
                {
                    contentList.Add(item);
                }
                string query = "SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + category.ID_CATEGORY + " AND id_organization=" + oid + " and STATUS='A') ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                foreach (tbl_content item in categoryContent)
                {
                    contentList.Add(item);
                }
                contentList = contentList.Distinct().ToList();
            }
            return contentList;

        }

        public int getContentLinkCount(int cid, int oid, int uid)
        {
            List<string> contentList = null;
            string qList = "";
            tbl_category category = db.tbl_category.Where(t => t.ID_CATEGORY == cid && t.STATUS == "A").FirstOrDefault();
            if (category != null)
            {

                string userbased_sql = "";// "SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_user_assisgnment where id_category=" + category.ID_CATEGORY + " AND id_user=" + uid + " AND id_organization=" + oid + ")";
                userbased_sql = "SELECT * FROM tbl_content a left join tbl_content_user_assisgnment b on a.id_content=b.id_content WHERE a.STATUS = 'A' and b.id_category = " + category.ID_CATEGORY + " AND b.id_user = " + uid + " AND b.id_organization = " + oid + "";
                List<tbl_content> userbasedContent = db.tbl_content.SqlQuery(userbased_sql).ToList();
                if (userbasedContent != null)
                {
                    contentList = new List<string>();
                    foreach (tbl_content item in userbasedContent)
                    {
                        contentList.Add(item.ID_CONTENT.ToString());
                    }
                }

                string query = "SELECT * FROM tbl_content WHERE STATUS='A'  AND id_content IN (select id_content from tbl_content_organization_mapping where id_category=" + category.ID_CATEGORY + " AND id_organization=" + oid + " and STATUS='A') ORDER BY CONTENT_QUESTION ";//order by CONTENT_COUNTER desc,CONTENT_QUESTION limit 3
                List<tbl_content> categoryContent = db.tbl_content.SqlQuery(query).ToList();
                if (categoryContent != null)
                {
                    if (contentList == null)
                    {
                        contentList = new List<string>();
                    }
                    foreach (tbl_content item in categoryContent)
                    {
                        contentList.Add(item.ID_CONTENT.ToString());
                    }
                }
                if (contentList != null)
                {
                    contentList = contentList.Distinct().ToList();
                    qList = string.Join(",", contentList);
                }

            }
            int tlinks = 0;
            if (qList != "")
            {
                string lSql = "select * from tbl_content_type_link where ID_CONTENT_ANSWER in (select ID_CONTENT_ANSWER from tbl_content_answer where id_content in(" + qList + "))";
                tlinks = db.tbl_content_type_link.SqlQuery(lSql).Count();
            }

            return tlinks;
        }

    }
    public class ContentInfo
    {
        public int id_content { get; set; }
        public int id_category { get; set; }
        public int id_organization { get; set; }
        public int id_user { get; set; }
        public string status { get; set; }
    }

    public class EventUser
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public string DNO { get; set; }
        public string MNO { get; set; }
        public string YNO { get; set; }
    }

    public class EventData
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public int EID { get; set; }
    }
    public class EventSubscription
    {
        public int OID { get; set; }
        public int UID { get; set; }
        public int EID { get; set; }
        public int OPT { get; set; }
        public string COMMENT { get; set; }
    }
}