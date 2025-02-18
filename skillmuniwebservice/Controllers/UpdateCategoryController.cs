using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class UpdateCategoryController : ApiController
    {
        public HttpResponseMessage Get(string ctList,int orgID,int uid)
        {
            List<CategoryResponce> categoryList = new List<CategoryResponce>();

            if(!string.IsNullOrEmpty(ctList))
            {
            string[] catList = ctList.Split('|');
            string newCat = string.Join(",", catList);

            foreach (string cat in catList)
            {
                if (!string.IsNullOrEmpty(cat))
                {
                    CategoryResponce category = new CategoryModel().GetCategory(cat);


                    if (category != null)
                    {
                        category.Status = "safe";
                    }
                    else
                    {
                        category = new CategoryResponce();
                        category.CategoryID = Convert.ToInt32(cat);
                        category.Status = "false";
                    }
                    categoryList.Add(category);
                }

                
            }
            List<CategoryResponce> newList = new List<CategoryResponce>();
            newList = new CategoryModel().GetNewCategory(newCat, orgID.ToString());

                if(newList.Count>0)
                {
                    foreach(CategoryResponce cata in newList)
                    {
                        cata.Status = "true";
                        categoryList.Add(cata);
                    }
                }
            return Request.CreateResponse(HttpStatusCode.OK, categoryList);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, categoryList);
            }
        }
    }
}
