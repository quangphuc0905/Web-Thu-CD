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
    public class ReportController : Controller
    {
        ThueDiaDB db = new ThueDiaDB();
        // GET: Report
        public ActionResult IndexReportCustomer()
        {
            return View();
        }
        public ActionResult IndexReportTitle()
        {
            return View();
        }

        [HttpGet]

        //Load dữ liệu của tất cả khách hàng
        public JsonResult GetAllCustomerList()
        {
            List<ModelCustomer> CusList = db.Customers.ToList().Select(x =>
                 new ModelCustomer
                 {

                     Id_Customer = x.Id_Customer,
                     Address = x.Address,
                     Fine = x.Fine,
                     FullName = x.FullName,
                     Phone = x.Phone,
                     CountCDBorrow = db.OrderDetails.Count((t=>t.Orders.Id_Customer == x.Id_Customer && t.Status == "Chưa Trả")),
                 }
                ).ToList();
            return Json(CusList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        //Load dữ liệu của khách hàng
        public JsonResult GetCustomerList()
        {
            List<ModelCustomer> CusList = db.Customers.Where(x => x.orderList.Count > 0).Select(x =>
                 new ModelCustomer
                 {
                     Id_Customer = x.Id_Customer,
                     Address = x.Address,
                     Fine = x.Fine,
                     FullName = x.FullName,
                     Phone = x.Phone,
                     CountCDBorrow = db.OrderDetails.Count((t => t.Orders.Id_Customer == x.Id_Customer && t.Status == "Chưa Trả")),
                     TotalRent = x.orderList.Sum(y => y.TotalRent)
                 }
                ).ToList();
            return Json(CusList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        //Load danh sach dia dang qua han cua customer
        public JsonResult GetListDiskLate(int Id_Customer)
        {
           
            List<ModelDiskReturn> CusList = db.OrderDetails.Where(k => k.Orders.Id_Customer == Id_Customer && k.Status == "Chưa Trả" && k.DateDue < DateTime.Now).Select(x =>
                 new ModelDiskReturn
                 {
                    ID = x.Id_Item,
                    Title_Name =x.Items.Titles.Name,
                    DateRent = x.Orders.DateRent,
                    DateDue = x.DateDue,
                 }
                ).ToList();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(CusList, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        //Load dữ liệu của tiêu đề
        public JsonResult GetTitleList()
        {
            var TitleList = db.Titles.Select(x => new
            {
                Id_Title = x.Id_Title,
                Name = x.Name,
                NameType = x.TypeDisk.NameType,
                //CountCDBorrow = x.orderList.Sum(y=>y.orderDetailList.Where(z=>z.Status == "CHƯA TRẢ").Count())
                CountOfItemOnShelf = x.ItemList.Where(y => y.Status == "On-Shelf").Count(),
                CountOfItemOnHold = x.ItemList.Where(y => y.Status == "On-Hold").Count(),
                CountOfItemRented = x.ItemList.Where(y => y.Status == "Rented").Count(),

            }).ToList();

            return Json(TitleList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //Load danh sach cac CD theo Title
        public JsonResult GetCDTitleById(int Id_Title)
        {

            var value = db.Items.Where(x => x.Titles.Id_Title == Id_Title).Select(x => new
            {
                Name = x.Titles.Name,
                TypeDisk = x.Titles.TypeDisk.NameType,
                RentPrice = x.Titles.TypeDisk.RentPrice,
                MaxDate = x.Titles.TypeDisk.MaxDate,
                Status = x.Status.Trim()
            }).ToList();

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //Load danh sach cac CD
        public JsonResult GetCustomerById(int Id_Customer)
        {
            bool kq = false;
            try
            {
                var value = db.OrderDetails.Where(x => x.Orders.Id_Customer == Id_Customer).Select(x => new
                {
                    Name = x.Items.Titles.Name,
                    DateRent = x.Orders.DateRent,
                    DateDue = x.DateDue,
                    //DateReturn = x.DateReturn,
                    RentFee = x.RentFee,
                    Late_Fee = x.lateFeeList.Sum(y => y.Late_Fee),
                    Status = x.Status
                }).ToList();
                kq = true;
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }
           

            return Json(kq, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //hien thi chi tiet khach hang
        public JsonResult GetInfoCustomer(int Id_Customer)
        {
            Customer a = db.Customers.Where(x => x.Id_Customer == Id_Customer).FirstOrDefault();
            ModelCustomer c = new ModelCustomer
            {
                Id_Customer = a.Id_Customer,
                FullName = a.FullName,
                CountCDBorrow = db.OrderDetails.Count((t => t.Orders.Id_Customer == Id_Customer && t.Status == "Chưa Trả")),
                NumDiskLate = db.OrderDetails.Count(k => k.Orders.Id_Customer ==Id_Customer && k.Status =="Chưa Trả" && k.DateDue < DateTime.Now),
                Fine = a.Fine
            };
            string val = string.Empty;

            val = JsonConvert.SerializeObject(c, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(val, JsonRequestBehavior.AllowGet);
        }
        //Tim khách hàng
        [HttpGet]
        public JsonResult GetSearchingData(string SearchBy)
        {
            List<ModelCustomer> CusList = new List<ModelCustomer>();
            if (SearchBy == "0")
            {
                try
                {
                    CusList = db.Customers.Where(x => x.Id_Customer>0).Select(x =>
                 new ModelCustomer
                 {
                     Id_Customer = x.Id_Customer,
                     Address = x.Address,
                     Fine = x.Fine,
                     FullName = x.FullName,
                     Phone = x.Phone,
                     CountCDBorrow = db.OrderDetails.Count((t => t.Orders.Id_Customer == x.Id_Customer && t.Status == "Chưa Trả")),
                     
                 }
                ).ToList();
                    return Json(CusList, JsonRequestBehavior.AllowGet);

                    //int Id = Convert.ToInt32(SearchBy);
                    //StuList = db.Customers.Where(x => x.Id_Customer == Id).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} Is Not A ID ", SearchBy);
                }
                return Json(CusList, JsonRequestBehavior.AllowGet);
            }
            else if (SearchBy == "1")
            {
                CusList = db.Customers.Where(x => x.orderList.Sum(y => y.orderDetailList.Where(z => z.DateDue < DateTime.Now && z.Status == "Chưa Trả").Count()) > 0).Select(x =>
                     new ModelCustomer
                     {
                         Id_Customer = x.Id_Customer,
                         Address = x.Address,
                         Fine = x.Fine,
                         FullName = x.FullName,
                         Phone = x.Phone,
                         CountCDBorrow = db.OrderDetails.Count((t => t.Orders.Id_Customer == x.Id_Customer && t.Status == "Chưa Trả")),
                         
                     }
                    ).ToList();
                return Json(CusList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                CusList = db.Customers.Where(x => x.Fine > 0).Select(x =>
                     new ModelCustomer
                     {
                         Id_Customer = x.Id_Customer,
                         Address = x.Address,
                         Fine = x.Fine,
                         FullName = x.FullName,
                         Phone = x.Phone,
                         CountCDBorrow = db.OrderDetails.Count((t => t.Orders.Id_Customer == x.Id_Customer && t.Status == "Chưa Trả")),
                         
                     }
                    ).ToList();
                return Json(CusList, JsonRequestBehavior.AllowGet);
                //CusList = db.Customers.Where(x => x.FullName.StartsWith(SearchBy)).ToList();
                //return Json(CusList, JsonRequestBehavior.AllowGet);
            }

        }
    }
}