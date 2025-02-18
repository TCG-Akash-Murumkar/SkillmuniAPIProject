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
    public class DisplayCategoryController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        //public HttpResponseMessage Get(int orgID, int cid, int uid)
        //{
        //    List<DisplayCategory> response = new List<DisplayCategory>();

        //    try
        //    {
        //        //new Utility().eventLog("inside Display category : OID: " + orgID + " ciD: " + cid + " UID :" + uid);

        //        string sqlhead = "select a.* from tbl_category_heading a left join tbl_category_associantion b on a.id_category_heading =b.id_category_heading where id_category_tile=" + cid + " and b.status='A'";

        //        List<tbl_category_heading> categoryHead = db.tbl_category_heading.SqlQuery(sqlhead).ToList();

        //        tbl_user user = db.tbl_user.Where(t => t.ID_USER == uid).FirstOrDefault();
        //        string rolSql = "select a.* from tbl_csst_role a left join tbl_role_user_mapping b on a.id_csst_role = b.id_csst_role  where b.id_user=" + uid + " ";

        //        List<tbl_csst_role> role = db.tbl_csst_role.SqlQuery(rolSql).ToList();
        //        string uStr = "";
        //        foreach (tbl_csst_role item in role)
        //        {
        //            uStr += item.id_csst_role + ",";
        //        }
        //        uStr = uStr.TrimEnd(',');
        //        string additional = "";
        //        uStr = "";
        //        if (uStr == "")
        //        {
        //            additional = "(id_user=" + uid + ")";
        //        }
        //        else
        //        {
        //            additional = "(id_role in (" + uStr + ") or id_user=" + uid + ")";
        //        }

        //        string sqlUhead = "select * from tbl_category_heading where  status='A' and  id_category_heading in (select distinct id_category_heading from tbl_content_program_mapping where id_category_tile=" + cid + " and " + additional + ")";


        //        List<tbl_category_heading> programHead = db.tbl_category_heading.SqlQuery(sqlUhead).ToList();
        //        foreach (tbl_category_heading item in programHead)
        //        {
        //            categoryHead.Add(item);
        //        }
        //        //  new Utility().eventLog("in time : ");
        //        categoryHead = categoryHead.Distinct().ToList();

        //        if (categoryHead.Count > 0)
        //        {
        //            int k = 1;
        //            foreach (var item in categoryHead)
        //            {

        //                List<Category> categoryList = new List<Category>();
        //                List<Category> programList = new List<Category>();
        //                DisplayCategory temp = new DisplayCategory();
        //                temp.Heading = item.Heading_title;
        //                temp.HeadingID = item.id_category_heading;
        //                temp.Order = item.heading_order.ToString();

        //                //string caProg = "select distinct * from tbl_content_program_mapping where id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + "";
        //                //List<tbl_content_program_mapping> cProgram = db.tbl_content_program_mapping.SqlQuery(caProg).ToList();


        //                ////string catProg = "select distinct * from tbl_category where category_type = 0 AND status='A' and  id_category in (select distinct id_category from tbl_content_program_mapping where id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + ")";
        //                ////List<tbl_category> program = db.tbl_category.SqlQuery(catProg).ToList();
        //                //int i = 1;
        //                //int k1 = 1;
        //                //int k2 = 1;
        //                //// program = program.OrderBy(t => t.CATEGORYNAME).ToList();
        //                //foreach (tbl_content_program_mapping pItem in cProgram)
        //                //{
        //                //    tbl_category cItem = db.tbl_category.Where(t => t.ID_CATEGORY == pItem.id_category && t.CATEGORY_TYPE == 0).FirstOrDefault();
        //                //    if (cItem != null)
        //                //    {

        //                //        bool cFlag = true;
        //                //        if (cItem.CATEGORY_TYPE == 0 || cItem.CATEGORY_TYPE == 1 || cItem.CATEGORY_TYPE == 2)
        //                //        {
        //                //            cFlag = true;// new Utility().checkCategoryContentCount(cItem.ID_CATEGORY, orgID, uid);
        //                //        }


        //                //        Category categoryDetails = new Category();

        //                //        categoryDetails.CategoryID = cItem.ID_CATEGORY;
        //                //        categoryDetails.CategoryName = cItem.CATEGORYNAME;
        //                //        categoryDetails.CategoryDescription = cItem.DESCRIPTION;
        //                //        categoryDetails.OrganisationId = cItem.ID_ORGANIZATION;
        //                //        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + cItem.IMAGE_PATH.ToString().Trim();
        //                //        categoryDetails.Is_Primary = cItem.ID_CATEGORY;
        //                //        categoryDetails.Is_Program = Convert.ToInt32(cItem.CATEGORY_TYPE);
        //                //        categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(cItem.COUNT_REQUIRED);
        //                //        categoryDetails.SubCount = 0;// new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());

        //                //        categoryDetails.ORDERID = i;
        //                //        categoryDetails.CategoryHeader = categoryDetails.CategoryName;
        //                //        categoryDetails.CategoryType = Convert.ToInt32(cItem.CATEGORY_TYPE);
        //                //        categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(cItem.COUNT_REQUIRED);
        //                //        if (cItem.CATEGORY_TYPE == 3)
        //                //        {
        //                //            categoryDetails.NEXTURL = cItem.IMAGE_URL;
        //                //        }
        //                //        else
        //                //        {
        //                //            categoryDetails.NEXTURL = "";
        //                //        }
        //                //        if (cFlag)
        //                //        {
        //                //            categoryDetails.ContentCount = 1;
        //                //        }
        //                //        else
        //                //        {
        //                //            categoryDetails.ContentCount = 0;
        //                //        }
        //                //        categoryDetails.ExpiryDate = pItem.expiry_date.Value.ToString("dd-MMM-yyyy");
        //                //        categoryDetails.LINKCOUNT = 0;
        //                //        programList.Add(categoryDetails);
        //                //        //new Utility().eventLog("1.01." + k + "." + k1); k1++;
        //                //    }
        //                //}

        //                //programList = programList.OrderBy(t => t.CategoryName).ToList();
        //                //foreach (Category pro in programList)
        //                //{
        //                //    pro.ORDERID = i;
        //                //    i++;
        //                //    categoryList.Add(pro);
        //                //}
        //                //string sql = "select * from tbl_category where id_category in ( select distinct a.id_category from tbl_category_associantion a,tbl_category b where a.id_category_tile=" + cid + " and a.id_category_heading=" + item.id_category_heading + " and b.status='A' and a.id_category=b.id_category ) order by CATEGORYNAME";
        //                //List<tbl_category> associationList = db.tbl_category.SqlQuery(sql).ToList();

        //                //associationList = associationList.OrderBy(t => t.ORDERID).ToList();

        //                //foreach (var ctim in associationList)
        //                //{
        //                //    if (ctim != null)
        //                //    {

        //                //        bool cFlag = true;
        //                //        if (ctim.CATEGORY_TYPE == 0 || ctim.CATEGORY_TYPE == 1 || ctim.CATEGORY_TYPE == 2)
        //                //        {
        //                //            cFlag = true;// new Utility().checkCategoryContentCount(ctim.ID_CATEGORY, orgID, uid);
        //                //        }
        //                //        Category categoryDetails = new Category();
        //                //        categoryDetails.CategoryID = ctim.ID_CATEGORY;
        //                //        categoryDetails.CategoryName = ctim.CATEGORYNAME;
        //                //        categoryDetails.CategoryDescription = ctim.DESCRIPTION;
        //                //        categoryDetails.OrganisationId = ctim.ID_ORGANIZATION;
        //                //        categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + ctim.IMAGE_PATH.ToString().Trim();
        //                //        categoryDetails.Is_Primary = ctim.ID_CATEGORY;
        //                //        categoryDetails.Is_Program = Convert.ToInt32(ctim.CATEGORY_TYPE);
        //                //        categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(ctim.COUNT_REQUIRED);
        //                //        categoryDetails.SubCount = 0;// new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());
        //                //        categoryDetails.ORDERID = Convert.ToInt32(ctim.ORDERID) + i;
        //                //        categoryDetails.CategoryHeader = categoryDetails.CategoryName;
        //                //        categoryDetails.CategoryType = Convert.ToInt32(ctim.CATEGORY_TYPE);
        //                //        categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(ctim.COUNT_REQUIRED);
        //                //        if (ctim.CATEGORY_TYPE == 3)
        //                //        {
        //                //            categoryDetails.NEXTURL = ctim.IMAGE_URL;
        //                //        }
        //                //        else
        //                //        {
        //                //            categoryDetails.NEXTURL = "";
        //                //        }
        //                //        if (cFlag)
        //                //        {
        //                //            categoryDetails.ContentCount = 1;
        //                //        }
        //                //        else
        //                //        {
        //                //            categoryDetails.ContentCount = 0;
        //                //        }
        //                //        categoryDetails.ExpiryDate = "";
        //                //        categoryDetails.LINKCOUNT = 0;
        //                //        categoryList.Add(categoryDetails);
        //                //        //new Utility().eventLog("1.01." + k + "." + k2); k2++;
        //                //    }
        //                //}

        //                temp.Categories = categoryList;
        //                response.Add(temp);
        //                k++;
        //            }
        //        }
        //        response = response.OrderBy(t => t.Order).ToList();
        //        // new Utility().eventLog(" out time ");
        //    }
        //    catch (Exception e)
        //    {
        //        new Utility().eventLog("ex m :" + e.Message);
        //        new Utility().eventLog("ex s :" + e.StackTrace);
        //        if (e.InnerException != null)
        //        {
        //            new Utility().eventLog("ex i :" + e.InnerException.ToString());
        //        }
        //    }
        //    if (response != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, response);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, response);
        //    }
        //}




        public HttpResponseMessage Get(int orgID, int cid, int uid)
        {
            List<DisplayCategory> response = new List<DisplayCategory>();
            DateTime current = System.DateTime.Now;
            try
            {
                //new Utility().eventLog("inside Display category : OID: " + orgID + " ciD: " + cid + " UID :" + uid);

                string sqlhead = "select a.* from tbl_category_heading a left join tbl_category_associantion b on a.id_category_heading =b.id_category_heading where id_category_tile=" + cid + " and b.status='A'";

                List<tbl_category_heading> categoryHead = db.tbl_category_heading.SqlQuery(sqlhead).ToList();

                tbl_user user = db.tbl_user.Where(t => t.ID_USER == uid).FirstOrDefault();
                string rolSql = "select a.* from tbl_csst_role a left join tbl_role_user_mapping b on a.id_csst_role = b.id_csst_role  where b.id_user=" + uid + " ";

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

                string sqlUhead = "select * from tbl_category_heading where  status='A' and  id_category_heading in (select distinct id_category_heading from tbl_content_program_mapping where id_category_tile=" + cid + " and " + additional + ")";


                List<tbl_category_heading> programHead = db.tbl_category_heading.SqlQuery(sqlUhead).ToList();
                foreach (tbl_category_heading item in programHead)
                {
                    categoryHead.Add(item);
                }
                //  new Utility().eventLog("in time : ");
                categoryHead = categoryHead.Distinct().ToList();

                if (categoryHead.Count > 0)
                {
                    int k = 1;
                    foreach (var item in categoryHead)
                    {

                        List<Category> categoryList = new List<Category>();
                        List<Category> programList = new List<Category>();
                        DisplayCategory temp = new DisplayCategory();
                        temp.Heading = item.Heading_title;
                        temp.Order = item.heading_order.ToString();

                        string caProg = "select distinct * from tbl_content_program_mapping where id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + "";
                        List<tbl_content_program_mapping> cProgram = db.tbl_content_program_mapping.SqlQuery(caProg).ToList();


                        //string catProg = "select distinct * from tbl_category where category_type = 0 AND status='A' and  id_category in (select distinct id_category from tbl_content_program_mapping where id_category_tile=" + cid + " and id_category_heading =" + item.id_category_heading + " and " + additional + ")";
                        //List<tbl_category> program = db.tbl_category.SqlQuery(catProg).ToList();
                        int i = 1;
                        int k1 = 1;
                        int k2 = 1;
                        // program = program.OrderBy(t => t.CATEGORYNAME).ToList();
                        foreach (tbl_content_program_mapping pItem in cProgram)
                        {
                            if (DateTime.Compare(pItem.expiry_date.Value.AddDays(1), current) > 0)
                            {
                                tbl_category cItem = db.tbl_category.Where(t => t.ID_CATEGORY == pItem.id_category && t.CATEGORY_TYPE == 0).FirstOrDefault();
                                if (cItem != null)
                                {

                                    bool cFlag = true;
                                    if (cItem.CATEGORY_TYPE == 0 || cItem.CATEGORY_TYPE == 1 || cItem.CATEGORY_TYPE == 2)
                                    {
                                        cFlag = true;// new Utility().checkCategoryContentCount(cItem.ID_CATEGORY, orgID, uid);
                                    }


                                    Category categoryDetails = new Category();

                                    categoryDetails.CategoryID = cItem.ID_CATEGORY;
                                    categoryDetails.CategoryName = cItem.CATEGORYNAME;
                                    categoryDetails.CategoryDescription = cItem.DESCRIPTION;
                                    categoryDetails.OrganisationId = cItem.ID_ORGANIZATION;
                                    categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + cItem.IMAGE_PATH.ToString().Trim();
                                    categoryDetails.Is_Primary = cItem.ID_CATEGORY;
                                    categoryDetails.Is_Program = Convert.ToInt32(cItem.CATEGORY_TYPE);
                                    categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(cItem.COUNT_REQUIRED);
                                    categoryDetails.SubCount = 0;// new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());

                                    categoryDetails.ORDERID = i;
                                    categoryDetails.CategoryHeader = categoryDetails.CategoryName;
                                    categoryDetails.CategoryType = Convert.ToInt32(cItem.CATEGORY_TYPE);
                                    categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(cItem.COUNT_REQUIRED);
                                    if (cItem.CATEGORY_TYPE == 3)
                                    {
                                        categoryDetails.NEXTURL = cItem.IMAGE_URL;
                                    }
                                    else
                                    {
                                        categoryDetails.NEXTURL = "";
                                    }
                                    if (cFlag)
                                    {
                                        categoryDetails.ContentCount = 1;
                                    }
                                    else
                                    {
                                        categoryDetails.ContentCount = 0;
                                    }
                                    categoryDetails.ExpiryDate = pItem.expiry_date.Value.ToString("dd-MMM-yyyy");
                                    categoryDetails.LINKCOUNT = 0;
                                    programList.Add(categoryDetails);
                                    //new Utility().eventLog("1.01." + k + "." + k1); k1++;
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
                        string sql = "select * from tbl_category where id_category in ( select distinct a.id_category from tbl_category_associantion a,tbl_category b where a.id_category_tile=" + cid + " and a.id_category_heading=" + item.id_category_heading + " and b.status='A' and a.id_category=b.id_category ) order by CATEGORYNAME";
                        List<tbl_category> associationList = db.tbl_category.SqlQuery(sql).ToList();

                        associationList = associationList.OrderBy(t => t.ORDERID).ToList();

                        foreach (var ctim in associationList)
                        {
                            if (ctim != null)
                            {
                                bool cFlag = true;
                                if (ctim.CATEGORY_TYPE == 0 || ctim.CATEGORY_TYPE == 1 || ctim.CATEGORY_TYPE == 2)
                                {
                                    cFlag = true;// new Utility().checkCategoryContentCount(ctim.ID_CATEGORY, orgID, uid);
                                }
                                Category categoryDetails = new Category();
                                categoryDetails.CategoryID = ctim.ID_CATEGORY;
                                categoryDetails.CategoryName = ctim.CATEGORYNAME;
                                categoryDetails.CategoryDescription = ctim.DESCRIPTION;
                                categoryDetails.OrganisationId = ctim.ID_ORGANIZATION;
                                categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + ctim.IMAGE_PATH.ToString().Trim();
                                categoryDetails.Is_Primary = ctim.ID_CATEGORY;
                                categoryDetails.Is_Program = Convert.ToInt32(ctim.CATEGORY_TYPE);
                                categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(ctim.COUNT_REQUIRED);
                                categoryDetails.SubCount = 0;// new CategoryModel().getSubCount(reader["ID_CATEGORY"].ToString());
                                categoryDetails.ORDERID = Convert.ToInt32(ctim.ORDERID) + i;
                                categoryDetails.CategoryHeader = categoryDetails.CategoryName;
                                categoryDetails.CategoryType = Convert.ToInt32(ctim.CATEGORY_TYPE);
                                categoryDetails.IS_COUNT_REQUIRED = Convert.ToInt32(ctim.COUNT_REQUIRED);
                                if (ctim.CATEGORY_TYPE == 3)
                                {
                                    categoryDetails.NEXTURL = ctim.IMAGE_URL;
                                }
                                else
                                {
                                    categoryDetails.NEXTURL = "";
                                }
                                if (cFlag)
                                {
                                    categoryDetails.ContentCount = 1;
                                }
                                else
                                {
                                    categoryDetails.ContentCount = 0;
                                }
                                categoryDetails.ExpiryDate = "";
                                categoryDetails.LINKCOUNT = 0;
                                categoryList.Add(categoryDetails);
                                //new Utility().eventLog("1.01." + k + "." + k2); k2++;
                            }
                        }

                        temp.Categories = categoryList;
                        response.Add(temp);
                        k++;
                    }
                }
                response = response.OrderBy(t => t.Order).ToList();
                // new Utility().eventLog(" out time ");
            }
            catch (Exception e)
            {
                new Utility().eventLog("ex m :" + e.Message);
                new Utility().eventLog("ex s :" + e.StackTrace);
                if (e.InnerException != null)
                {
                    new Utility().eventLog("ex i :" + e.InnerException.ToString());
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
