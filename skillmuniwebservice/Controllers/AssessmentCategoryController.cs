using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace m2ostnextservice.Controllers
{
    public class AssessmentCategoryController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int orgID, int cid, int uid)
        {
            List<DisplayCategory> response = new List<DisplayCategory>();

            string sqlhead = "select * from  tbl_category_heading a left join tbl_category_associantion b on a.id_category_heading=b.id_category_heading where b.id_category_tile = "+cid+" and b.status = 'A'";
            List<tbl_category_heading> categoryHead = db.tbl_category_heading.SqlQuery(sqlhead).ToList();


            tbl_user user = db.tbl_user.Where(t => t.ID_USER == uid).FirstOrDefault();
            string rolSql = "select * from tbl_csst_role where id_csst_role in (select id_csst_role from tbl_role_user_mapping where id_user=" + uid + ")";
            List<tbl_csst_role> role = db.tbl_csst_role.SqlQuery(rolSql).ToList();
            string uStr = "";
            foreach (tbl_csst_role item in role)
            {
                uStr += item.id_csst_role + ",";
            }
            uStr = uStr.TrimEnd(',');
            string additional = "";
            uStr = "";
            if (uStr == "")
            {
                additional = "(id_user=" + uid + ")";
            }
            else
            {
                additional = "(id_role in (" + uStr + ") or id_user=" + uid + ")";
            }

            string sqlUhead = "select * from tbl_category_heading where id_category_heading in (select distinct id_category_heading from tbl_content_program_mapping where id_category_tile=" + cid + " and " + additional + ")";
            List<tbl_category_heading> programHead = db.tbl_category_heading.SqlQuery(sqlUhead).ToList();
            foreach (tbl_category_heading item in programHead)
            {
                categoryHead.Add(item);
            }
            categoryHead = categoryHead.Distinct().ToList();

            if (categoryHead.Count > 0)
            {
                foreach (var item in categoryHead)
                {
                    string sql = "select distinct * from tbl_category_associantion where id_category_tile=" + cid + " and id_category_heading=" + item.id_category_heading + " and status='A' ";
                    List<tbl_category_associantion> associationList = db.tbl_category_associantion.SqlQuery(sql).ToList();
                    DisplayCategory temp = new DisplayCategory();
                    temp.Heading = item.Heading_title;

                    temp.Order = item.heading_order.ToString();
                    List<Category> categoryList = new List<Category>();
                    foreach (var pro in associationList)
                    {
                        Category catTemp = new CategoryModel().GetCategoryValue(Convert.ToInt32(pro.id_category));
                        catTemp.ORDERID = Convert.ToInt32(pro.category_order);
                        catTemp.CategoryHeader = catTemp.CategoryName;
                        categoryList.Add(catTemp);
                    }

                    string catProg = "select distinct * from tbl_category where id_category in (select distinct id_category from tbl_content_program_mapping where id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + ")";
                    List<tbl_category> program = db.tbl_category.SqlQuery(catProg).ToList();
                    int i = 1;
                    foreach (tbl_category cItem in program)
                    {
                        Category catTemp = new CategoryModel().GetCategoryValue(Convert.ToInt32(cItem.ID_CATEGORY));
                        catTemp.ORDERID = i;
                        catTemp.CategoryHeader = catTemp.CategoryName;
                        categoryList.Add(catTemp);
                        i++;
                    }

                    //string assessmentsql = "select distinct * from tbl_category_associantion where id_category_tile=" + cid + " and id_category_heading=" + item.id_category_heading + " and status='A' ";
                    //List<tbl_category_associantion> AssessmentCategory = db.tbl_category_associantion.SqlQuery(assessmentsql).ToList();



                    temp.Categories = categoryList;
                    response.Add(temp);

                }
            }

            if (response != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, response);
            }
        }
   
    }
}
