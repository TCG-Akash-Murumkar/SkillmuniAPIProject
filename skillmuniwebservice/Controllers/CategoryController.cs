using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using m2ostnextservice.Models;
using System.Web;
using Newtonsoft.Json;
namespace m2ostnextservice.Controllers
{
    public class CategoryController : ApiController
    {
        db_m2ostEntities db = new db_m2ostEntities();
        // GET api/<controller>/5
        public HttpResponseMessage Get(int orgID, int uid)
        {

            APIRESPONSE responce = new APIRESPONSE();
            downtime_log log = db.downtime_log.Where(t => t.status == "A").FirstOrDefault();
            if (log != null)
            {
                
                responce.KEY = "FAILURE";
                responce.MESSAGE=log.message_text;
            }
            else
            {
                
                
            List<tbl_category_tiles> tiles = db.tbl_category_tiles.Where(t => t.id_organization == orgID && t.status == "A").ToList();
            List<CategoryTile> categoryList = new List<CategoryTile>();
            foreach (tbl_category_tiles item in tiles)
            {
                CategoryTile categoryDetails = new CategoryTile();
                categoryDetails.CategoryID = item.id_category_tiles;
                categoryDetails.CategoryName = item.tile_heading;
                categoryDetails.CategoryDescription = "";
                categoryDetails.OrganisationId = orgID;
                //categoryDetails.menu_response = new RegistrationModel().get_menu(oid);
                string imgEnc = HttpUtility.UrlEncode(item.tile_image);
                categoryDetails.CategoryImagePath = ConfigurationManager.AppSettings["CATIMAGE"].ToString() + "Tiles" + "/" + imgEnc;
                categoryDetails.Is_Primary = 1;
                categoryDetails.SubCount = new CategoryModel().getSubCount(item.id_category_tiles.ToString());
                categoryDetails.Template = item.category_theme.ToString();
                int ORDID = Convert.ToInt32(item.category_order);
                categoryDetails.ORDERID = ORDID;
                if (item.category_theme == 1)//industry-S-Dev,Trainnig
                {
                    categoryDetails.NEXTAPI = "api/DisplayCategory?orgID=" + orgID + "&cid=" + item.id_category_tiles + "&uid=" + uid;
                }
                else if (item.category_theme == 2)//
                {
                    categoryDetails.NEXTAPI = "api/MyAssignment?orgID=" + orgID + "&cid=" + item.id_category_tiles + "&uid=" + uid;
                }
                else if (item.category_theme == 5)//Learning Assessment
                {
                    categoryDetails.NEXTAPI = "api/getLearningAssessment?CID=" + item.id_category_tiles + "&uid=" + uid + "&oid=" + orgID;
                }
                else if (item.category_theme == 4)
                {
                    categoryDetails.NEXTAPI = "api/DisplayCategory?orgID=" + orgID + "&cid=" + item.id_category_tiles + "&uid=" + uid;
                }
                else if (item.category_theme == 6)//psychometric Assessment
                {
                    categoryDetails.NEXTAPI = "api/DisplayCategory?orgID=" + orgID + "&cid=" + item.id_category_tiles + "&uid=" + uid;
                }
                else if (item.category_theme == 7)//URL Link
                {
                    categoryDetails.NEXTAPI = item.image_url;
                }

                categoryList.Add(categoryDetails);
            }

            categoryList = categoryList.OrderBy(t => t.ORDERID).ToList();

            responce.KEY = "SUCCESS";
            string resJson = JsonConvert.SerializeObject(categoryList);
            responce.MESSAGE = resJson;

            }

            return Request.CreateResponse(HttpStatusCode.OK, responce);
        }
    }
}