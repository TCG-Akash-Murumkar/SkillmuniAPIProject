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
    public class CategoryDetailsController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        public HttpResponseMessage Get(string category, string organization)
        {
            CateogryDetails Details = new CateogryDetails();
            List<Category> response = new List<Category>();

            int cid = Convert.ToInt32(category);

            tbl_category_associantion asso = db.tbl_category_associantion.Find(cid);


            tbl_category cat = db.tbl_category.Find(asso.id_category);
            tbl_category_heading head = db.tbl_category_heading.Find(asso.id_category_heading);
            if (cat != null)
            {
                Category categoryDetails = new Category();
                categoryDetails.CategoryID = cat.ID_CATEGORY;
                categoryDetails.CategoryName = cat.CATEGORYNAME;
                categoryDetails.CategoryDescription = cat.DESCRIPTION;
                categoryDetails.OrganisationId = cat.ID_ORGANIZATION;
                categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "" + categoryDetails.OrganisationId.ToString() + "/" + cat.IMAGE_PATH;
                categoryDetails.Is_Primary = 0;
                categoryDetails.CategoryHeader = head.Heading_title;
                categoryDetails.SubCount = 0;
                categoryDetails.ORDERID = Convert.ToInt32(asso.category_order);
                response.Add(categoryDetails);

                List<tbl_content> contents = new List<tbl_content>();
                string query = "";

                query = "SELECT * FROM tbl_content WHERE STATUS='A' AND ID_CATEGORY=" + cat.ID_CATEGORY + "  ORDER BY CONTENT_QUESTION  LIMIT 15 ";
                var content = db.tbl_content.SqlQuery(query);
                contents = (List<tbl_content>)content.ToList();

                List<SearchResponce> responces = new List<SearchResponce>();
                foreach (tbl_content item in contents)
                {
                    SearchResponce ser = new SearchResponce();
                    ser.CONTENT_QUESTION = item.CONTENT_QUESTION;
                    ser.ID_CONTENT = item.ID_CONTENT;
                    ser.ID_CONTENT_LEVEL = item.ID_CONTENT_LEVEL;
                    ser.EXPIRYDATE = item.EXPIRY_DATE.Value.ToString("dd-MM-yyyy");
                    responces.Add(ser);
                }
                responces = responces.OrderBy(t => t.ID_CONTENT_LEVEL).ThenBy(t => t.CONTENT_QUESTION).ToList();

                Details.Categories = response;
                Details.Contents = responces;
                Details.Order ="0";
                Details.Heading = "";




                return Request.CreateResponse(HttpStatusCode.OK, Details);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "");
            }
        }

    }
}
