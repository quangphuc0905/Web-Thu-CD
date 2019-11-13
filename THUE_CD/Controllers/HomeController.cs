using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUE_CD.Models;
using THUE_CD.ViewModel;

namespace THUE_CD.Controllers
{
    public class HomeController : Controller
    {
        ThueDiaDB db = new ThueDiaDB();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Total = "";
            return View();
        }
        public JsonResult GetCustomerById(int Id_Customer)
        {
            Customer model = db.Customers.Where(x => x.Id_Customer == Id_Customer).FirstOrDefault();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemById(int Id_Item)
        {
            Item model = db.Items.Where(x => x.Id_Item == Id_Item).SingleOrDefault();

            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemByIdOnShelf(int Id_Item)
        {
            Item model = db.Items.Where(x => x.Id_Item == Id_Item && x.Status == "On-Shelf").SingleOrDefault();

            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}