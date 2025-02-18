using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class MyAssignmentController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(int orgID, int cid, int uid)
        {

            List<AssignmentCategory> aResponse = new List<AssignmentCategory>();
            List<DateTime> monthList = new List<DateTime>();
            DateTime current = System.DateTime.Now;
            monthList.Add(current);
            current=current.AddMonths(1);
            monthList.Add(current);
            current=current.AddMonths(1);
            monthList.Add(current);
            current = System.DateTime.Now;
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
            additional = "  id_user=" + uid + " ";
            int j = 1;
            foreach (DateTime month in monthList)
            {
                AssignmentCategory aItem = new AssignmentCategory();
                aItem.MONTH = month.ToString("MMMM-yyyy");
                List<DisplayCategory> response = new List<DisplayCategory>();
                string sqlhead = "select * from tbl_category_heading where id_category_heading in (select distinct id_category_heading from tbl_category_associantion where id_category_tile=" + cid + " and status='A')";
                List<tbl_category_heading> categoryHead = db.tbl_category_heading.SqlQuery(sqlhead).ToList();

                string sqlUhead = "select * from tbl_category_heading where  status='A' and  id_category_heading in (select distinct id_category_heading from tbl_content_program_mapping where MONTH(start_date)=" + month.Month + " and id_category_tile=" + cid + " and " + additional + ")";
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
                        List<Category> categoryList = new List<Category>();
                        List<Category> programList = new List<Category>();
                        DisplayCategory temp = new DisplayCategory();
                        temp.Heading = item.Heading_title;
                        temp.Order = item.heading_order.ToString();

                        string caProg = "select distinct * from tbl_content_program_mapping where  MONTH(start_date)=" + month.Month + " and id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + "";
                        List<tbl_content_program_mapping> cProgram = db.tbl_content_program_mapping.SqlQuery(caProg).ToList();
                        int i = 1;
                        foreach (tbl_content_program_mapping pItem in cProgram)
                        {
                            DateTime tdate = pItem.expiry_date.Value.AddDays(1);
                            if (DateTime.Compare(tdate, current) > 0)
                            {  
                                tbl_category cItem = db.tbl_category.Where(t => t.ID_CATEGORY == pItem.id_category && t.CATEGORY_TYPE == 0).FirstOrDefault();
                                if (cItem != null)
                                {
                                    bool cFlag = true;

                                    if (cItem.CATEGORY_TYPE == 0 || cItem.CATEGORY_TYPE == 1 || cItem.CATEGORY_TYPE == 2)
                                    {
                                        cFlag = new Utility().checkCategoryContentCount(cItem.ID_CATEGORY, orgID, uid);
                                    }

                                    Category catTemp = new CategoryModel().GetCategoryValue(Convert.ToInt32(cItem.ID_CATEGORY));
                                    catTemp.ORDERID = i;
                                    catTemp.CategoryHeader = catTemp.CategoryName;
                                    catTemp.CategoryType = Convert.ToInt32(cItem.CATEGORY_TYPE);
                                    catTemp.IS_COUNT_REQUIRED = Convert.ToInt32(cItem.COUNT_REQUIRED);
                                    if (cItem.CATEGORY_TYPE == 3)
                                    {
                                        catTemp.NEXTURL = cItem.IMAGE_URL;
                                    }
                                    else
                                    {
                                        catTemp.NEXTURL = "";
                                    }
                                    if (cFlag)
                                    {
                                        catTemp.ContentCount = 1;
                                    }
                                    else
                                    {
                                        catTemp.ContentCount = 0;
                                    }
                                    catTemp.ExpiryDate = pItem.expiry_date.Value.ToString("dd-MMM-yyyy");
                                    catTemp.LINKCOUNT = new ContentModel().getContentLinkCount(Convert.ToInt32(cItem.ID_CATEGORY), orgID, uid);
                                    programList.Add(catTemp);
                                }
                            }
                        }

                        programList = programList.OrderBy(t => t.CategoryName).ToList();
                        foreach (Category pro in programList)
                        {
                            pro.ORDERID = i;
                            i++;
                            categoryList.Add(pro);
                        }

                        temp.Categories = categoryList;
                        response.Add(temp);
                    }
                }

                response = response.OrderBy(t => t.Order).ToList();
                aItem.assigment = response;
                aItem.ORDER = j;
                aResponse.Add(aItem);
                j++;
            }


            return Request.CreateResponse(HttpStatusCode.OK, aResponse);
        }
    }
}
