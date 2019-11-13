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
    public class TypeDisksController : Controller
    {
        ThueDiaDB db = new ThueDiaDB();

        // GET: TypeDisks
        public ActionResult IndexTypeDisk()
        {
            return View();
        }
        public JsonResult GetTypeList()
        {
            List<ModelTypeDisk> value = db.Types.Select(x => new ModelTypeDisk
            {
                Id_Type = x.Id_TypeDisk,
                TypeName = x.NameType,
                RentPrice=x.RentPrice,
                MaxDate=x.MaxDate
            }).ToList();
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTypeByTitle(int Id_Title)
        {
            TypeDisk vc = db.Titles.Where(x => x.Id_Title == Id_Title).First().TypeDisk;
           
            return Json(vc.NameType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypeById(int Id_Type)
        {
            TypeDisk vc = db.Types.Where(x => x.Id_TypeDisk == Id_Type).FirstOrDefault();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(vc, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTypeInDatabase(int Id_Type, double RentPrice, int MaxDate)
        {
            TypeDisk d = db.Types.Where(x => x.Id_TypeDisk == Id_Type).FirstOrDefault();
            if(d!=null)
            {
                d.RentPrice = RentPrice;
                d.MaxDate = MaxDate;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}