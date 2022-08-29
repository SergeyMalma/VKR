using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;


namespace MyWebApp.Controllers
{
    public class DataController : Controller
    {
        private ApplicationContext _context;
        readonly IGeneratePdf _generatePdf;
        public DataController(ApplicationContext context, IGeneratePdf generatePdf)
        {
            _context = context;
            _generatePdf = generatePdf;

        }
        // GET: DataController

        [Authorize(Roles = "admin, user")]
        public ActionResult Order()
        {
            return View(_context.Orders.ToList());
        }
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {

            return View();
        }



        [HttpGet]
        public ActionResult Index1(IEnumerable<Order> orders)
        {
            return View(_context.Orders.ToList());


        }

        public ActionResult TI(bool id, bool name, bool orderid)
        {
            ViewBag.IdC = id;
            ViewBag.NameC = name;
            ViewBag.OrderIDC = orderid;
            return View(_context.Orders.ToList());



        }



        //Генерация отчета
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetOrderInfo(bool id, bool name, bool orderid)
        {
            ViewBag.IdC = id;
            ViewBag.NameC = name;
            ViewBag.OrderIDC = orderid;
            var obj = _context.Orders.ToList();

            return await _generatePdf.GetPdf("Views/Data/GetOrderInfo.cshtml", obj);
        }
        [HttpPost]
        public async Task<IActionResult> GetOrderInfo2(bool id, bool name, bool orderid)
        {


            //var obj = from x in _context.Orders select x.Id;
            var obj = _context.Orders.ToList();

            var r = new List<Order>();

            {
                foreach (var item in obj)
                {
                    var n = new Order
                    {

                    };
                    if (id == true) n.Id = item.Id;
                    if (name == true) n.Name = item.Name;
                    if (orderid == true) n.OrderID = item.OrderID;
                    r.Add(n);
                }
            }


            return await _generatePdf.GetPdf("Views/Data/GetOrderInfo2.cshtml", r);
        }
        public ActionResult UserFile(string name)
        {

            List<Files> files = _context.Files.ToList();

            var result = files.Where(a => a.UserEmail == name);

            return View(result);
        }
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile files, string name)
        {
            string name1 = User.Identity.Name;
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var objfiles = new Files()
                    {
                        DocumentId = 0,
                        Name = newFileName,
                        FileType = fileExtension,
                        CreatedOn = DateTime.Now,
                        FileName = name,
                        UserEmail = name1
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        objfiles.DataFiles = target.ToArray();
                    }

                    _context.Files.Add(objfiles);
                    _context.SaveChanges();

                }

            }
            return View("UserFile", _context.Files.ToList());
        }
        public FileResult DownloadFile(int id)
        {

            var fileDto = (from FC in _context.Files where FC.DocumentId.Equals(id) select new { FC.FileName, FC.DataFiles }).ToList().FirstOrDefault();

            return File(fileDto.DataFiles, "application/octet-stream", fileDto.FileName);
        }

        public ActionResult Earning()
        {
            return View(_context.Earnings.ToList());
        }
        public async Task<IActionResult> GetEarningInfo(bool OrderId, bool UserName, bool Payment, bool PaymentDate)
        {
            var obj = _context.Earnings.ToList();

            var r = new List<Earning>();
            {
                foreach (var item in obj)
                {
                    var n = new Earning
                    {

                    };
                    if (OrderId == true) n.OrderId = item.OrderId;
                    if (UserName == true) n.UserName = item.UserName;
                    if (Payment == true) n.Payment = item.Payment;
                    if (PaymentDate == true) n.PaymentDate = item.PaymentDate;
                    r.Add(n);
                }
            }
            return await _generatePdf.GetPdf("Views/Data/GetEarningInfo.cshtml", r);
        }
        public ActionResult OnGetChartData()
        {
            var obj = _context.Earnings.ToList();
            var r = new List<Earning>();
            {
                foreach (var item in obj)
                {
                    var n = new Earning
                    {

                    };
                    n.OrderId = item.OrderId;
                    n.UserName = item.UserName;
                    n.Payment = item.Payment;
                    n.PaymentDate = item.PaymentDate;
                    r.Add(n);
                }
            }

            //var pizza = new[]
            // {

            //    new {Name = "Подписка 1", Count = 5000},
            //    new {Name = "Подписка 2", Count = 3333},
            //    new {Name = "Подписка 3", Count = 2000},
            //    new {Name = "Подписка 4", Count = 3124},
            //    new {Name = "Подписка 5", Count = 123123}
            //};

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Topping"), x => x.UserName)
                .NewColumn(new Column(ColumnType.Number, "Slices"), x => x.Payment)
                .Build()
                .GetJson();

            return Content(json);


        }
        //public ActionResult EarningChart()    
        //{
        //    //return View();
        //    var sa = new Newtonsoft.Json.JsonSerializerSettings();
        //    var obj = _context.Earnings.ToList();
        //    var r = new List<Earning>();
        //    {
        //        foreach (var item in obj)
        //        {
        //            var n = new Earning
        //            {

        //            };
        //            n.OrderId = item.OrderId;
        //            n.UserName = item.UserName;
        //            n.Payment = item.Payment;
        //            n.PaymentDate = item.PaymentDate;
        //            r.Add(n);
        //        }
        //        TempData["DataC"] = r;
        //        return View(r);
        //        //return Json(r);
        //    }
        //}
        //////////////////////////////////////Это правильный код для диаграммм/////////
        /* public ActionResult OnGetChartData()
         {
             var pizza = new[]
              {
                 new {Name = "Mushrooms", Count = 3},
                 new {Name = "Onions", Count = 1},
                 new {Name = "Olives", Count = 1},
                 new {Name = "Zucchini", Count = 1},
                 new {Name = "Pepperoni", Count = 2}
             };

             var json = pizza.ToGoogleDataTable()
                     .NewColumn(new Column(ColumnType.String, "Topping"), x => x.Name)
                     .NewColumn(new Column(ColumnType.Number, "Slices"), x => x.Count)
                     .Build()
                     .GetJson();

             return Content(json);


         }*/
        public ActionResult EarningChart()
        {
            return View();
        }
        public ActionResult MonthlyIncome()
        {
            return View(_context.MonthlyIncomes.ToList());
        }
        public async Task<IActionResult> GetMonthlyIncomeInfo(bool Month, bool Earnings, bool SubscribedUsers, bool UnsubscribedUsers, bool SoldLicenses)
        {
            var obj = _context.MonthlyIncomes.ToList();

            var r = new List<MonthlyIncome>();
            {
                foreach (var item in obj)
                {
                    var n = new MonthlyIncome
                    {

                    };
                    if (Month == true) n.Month = item.Month;
                    if (Earnings == true) n.Earnings = item.Earnings;
                    if (SubscribedUsers == true) n.SubscribedUsers = item.SubscribedUsers;
                    if (UnsubscribedUsers == true) n.UnsubscribedUsers = item.UnsubscribedUsers;
                    if (SoldLicenses == true) n.SoldLicenses = item.SoldLicenses;
                    r.Add(n);
                }
            }
            return await _generatePdf.GetPdf("Views/Data/GetMonthlyIncomeInfo.cshtml", r);
        }
        public ActionResult License()
        {
            return View(_context.Licenses.ToList());
        }
        public async Task<IActionResult> GetLicenseInfo(bool UserEmail, bool LicenseName, bool Month, bool Age)
        {
            var obj = _context.Licenses.ToList();

            var r = new List<License>();
            {
                foreach (var item in obj)
                {
                    var n = new License
                    {

                    };
                    if (UserEmail == true) n.UserEmail = item.UserEmail;
                    if (LicenseName == true) n.LicenseName = item.LicenseName;
                    if (Month == true) n.Month = item.Month;
                    if (Age == true) n.Age = item.Age;
                    r.Add(n);
                }
            }
            return await _generatePdf.GetPdf("Views/Data/GetLicenseInfo.cshtml", r);
        }
        public ActionResult OnGetChartDataMonthlyIncome1()
        {
            var obj = _context.MonthlyIncomes.ToList();
            var r = new List<MonthlyIncome>();
            {
                foreach (var item in obj)
                {
                    var n = new MonthlyIncome
                    {

                    };
                    n.Month = item.Month;
                    n.Earnings = item.Earnings;
                    n.SubscribedUsers = item.SubscribedUsers;
                    n.UnsubscribedUsers = item.UnsubscribedUsers;
                    n.SoldLicenses = item.SoldLicenses;
                    r.Add(n);
                }
            }

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Месяц"), x => x.Month)
                .NewColumn(new Column(ColumnType.Number, "Прибыль"), x => x.Earnings)
                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataMonthlyIncome2()
        {
            var obj = _context.MonthlyIncomes.ToList();
            var r = new List<MonthlyIncome>();
            {
                foreach (var item in obj)
                {
                    var n = new MonthlyIncome
                    {

                    };
                    n.Month = item.Month;
                    n.Earnings = item.Earnings;
                    n.SubscribedUsers = item.SubscribedUsers;
                    n.UnsubscribedUsers = item.UnsubscribedUsers;
                    n.SoldLicenses = item.SoldLicenses;
                    r.Add(n);
                }
            }

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Месяц"), x => x.Month)
                .NewColumn(new Column(ColumnType.Number, "Количество новых пользователей"), x => x.SubscribedUsers)
                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataMonthlyIncome3()
        {
            var obj = _context.MonthlyIncomes.ToList();
            var r = new List<MonthlyIncome>();
            {
                foreach (var item in obj)
                {
                    var n = new MonthlyIncome
                    {

                    };
                    n.Month = item.Month;
                    n.Earnings = item.Earnings;
                    n.SubscribedUsers = item.SubscribedUsers;
                    n.UnsubscribedUsers = item.UnsubscribedUsers;
                    n.SoldLicenses = item.SoldLicenses;
                    r.Add(n);
                }
            }

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Месяц"), x => x.Month)
                .NewColumn(new Column(ColumnType.Number, "Количество ушелших пользователей"), x => x.UnsubscribedUsers)
                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataMonthlyIncome4()
        {
            var obj = _context.MonthlyIncomes.ToList();
            var r = new List<MonthlyIncome>();
            {
                foreach (var item in obj)
                {
                    var n = new MonthlyIncome
                    {

                    };
                    n.Month = item.Month;
                    n.Earnings = item.Earnings;
                    n.SubscribedUsers = item.SubscribedUsers;
                    n.UnsubscribedUsers = item.UnsubscribedUsers;
                    n.SoldLicenses = item.SoldLicenses;
                    r.Add(n);
                }
            }

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Месяц"), x => x.Month)
                .NewColumn(new Column(ColumnType.Number, "Количество проданных лицензий"), x => x.SoldLicenses)
                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataLicense()
        {
            var obj = _context.Licenses.ToList();

            var r = new List<License>();
            {
                foreach (var item in obj)
                {
                    var n = new License
                    {

                    };
                    n.UserEmail = item.UserEmail;
                    n.LicenseName = item.LicenseName;
                    n.Month = item.Month;
                    n.Age = item.Age;
                    r.Add(n);
                }
            }

            var json = obj.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "LicenseName"), x => x.LicenseName)
                .NewColumn(new Column(ColumnType.Number, "Age"), x => x.Age)
                .Build()
                .GetJson();

            return Content(json);


        }
        public async Task<IActionResult> GetEnrolleeInfo()
        {
            var obj = _context.E1Tables.ToList();
            var r = new List<E1table>();
            {
                foreach (var item in obj)
                {
                    var n = new E1table
                    {

                    };
                    n.DateOfApplication = item.DateOfApplication;
                    n.d1 = item.d1;
                    n.d2 = item.d2;
                    n.d3 = item.d3;
                    n.d4 = item.d4;
                    n.d5 = item.d5;
                    n.d6 = item.d6;
                    n.d7 = item.d7;
                    n.d8 = item.d8;
                    n.d9 = item.d9;
                    n.d10 = item.d10;
                    r.Add(n);
                }
            }

            return await _generatePdf.GetPdf("Views/Data/GetEnrolleeInfo.cshtml", r);
        }

        public ActionResult Enrollee()
        {
            var obj = _context.Enrollees.ToList();

            obj.Sort((x, y) => y.InformaticsPoint.CompareTo(x.InformaticsPoint));
            //var a = obj.Select(x =>x.InformaticsPoint, x => x.EnrolleeId == 2);
            var r = new List<Enrollee>();
            int i = 0;
            {
                foreach (var item in obj)
                {
                    i++;
                    var n = new Enrollee
                    {

                    };
                    n.EnrolleeId = i;
                    n.TotalPoint = item.TotalPoint;
                    r.Add(n);
                }

            }
            ViewBag.a = r.Where(x => x.EnrolleeId == 3).Select(x => x.TotalPoint);

            return View(obj);
        }

        public ActionResult OnGetChartDataEnrollee1()
        {
            var obj = _context.E1Tables.ToList();



            var json = obj.ToGoogleDataTable()
                // .NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Datetime, "Дата подачи"), x => x.DateOfApplication)

                //.NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Number, "09.03.02"), x => x.d1)
                .NewColumn(new Column(ColumnType.Number, "10.03.01"), x => x.d2)
                .NewColumn(new Column(ColumnType.Number, "09.03.01"), x => x.d3)
                .NewColumn(new Column(ColumnType.Number, "09.03.03"), x => x.d4)
                .NewColumn(new Column(ColumnType.Number, "09.03.04"), x => x.d5)
                .NewColumn(new Column(ColumnType.Number, "10.05.01"), x => x.d6)
                .NewColumn(new Column(ColumnType.Number, "38.03.01"), x => x.d7)
                .NewColumn(new Column(ColumnType.Number, "38.05.01"), x => x.d8)
                .NewColumn(new Column(ColumnType.Number, "01.03.05"), x => x.d9)
                .NewColumn(new Column(ColumnType.Number, "03.03.01"), x => x.d10)



                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataEnrollee2()
        {
            var obj = _context.E2Tables.ToList();



            var json = obj.ToGoogleDataTable()
                // .NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Datetime, "Дата"), x => x.DateOfApplication)

                //.NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Number, "09.03.02"), x => x.d1)
                .NewColumn(new Column(ColumnType.Number, "10.03.01"), x => x.d2)
                .NewColumn(new Column(ColumnType.Number, "09.03.01"), x => x.d3)
                .NewColumn(new Column(ColumnType.Number, "09.03.03"), x => x.d4)
                .NewColumn(new Column(ColumnType.Number, "09.03.04"), x => x.d5)
                .NewColumn(new Column(ColumnType.Number, "10.05.01"), x => x.d6)
                .NewColumn(new Column(ColumnType.Number, "38.03.01"), x => x.d7)
                .NewColumn(new Column(ColumnType.Number, "38.05.01"), x => x.d8)
                .NewColumn(new Column(ColumnType.Number, "01.03.05"), x => x.d9)
                .NewColumn(new Column(ColumnType.Number, "03.03.01"), x => x.d10)



                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult OnGetChartDataEnrollee3()
        {
            var obj = _context.E2Tables.ToList();



            var json = obj.ToGoogleDataTable()
                // .NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Datetime, "Дата"), x => x.DateOfApplication)

                //.NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Number, "09.03.02"), x => x.s1)
                .NewColumn(new Column(ColumnType.Number, "10.03.01"), x => x.s2)
                .NewColumn(new Column(ColumnType.Number, "09.03.01"), x => x.s3)
                .NewColumn(new Column(ColumnType.Number, "09.03.03"), x => x.s4)
                .NewColumn(new Column(ColumnType.Number, "09.03.04"), x => x.s5)
                .NewColumn(new Column(ColumnType.Number, "10.05.01"), x => x.s6)
                .NewColumn(new Column(ColumnType.Number, "38.03.01"), x => x.s7)
                .NewColumn(new Column(ColumnType.Number, "38.05.01"), x => x.s8)
                .NewColumn(new Column(ColumnType.Number, "01.03.05"), x => x.s9)
                .NewColumn(new Column(ColumnType.Number, "03.03.01"), x => x.s10)



                .Build()
                .GetJson();

            return Content(json);


        }
        public ActionResult EnrolleData()
        {
           // var obj = _context.E3Tables.ToList();
            return View();
        }
        public ActionResult OnGetChartDataEnrolleeData1()
        {
            var obj = _context.E3Tables.ToList();



            var json = obj.ToGoogleDataTable()
                // .NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.String, "Направление"), x =>x.SN)

                //.NewColumn(new Column(ColumnType.String, "Направление"), x => x.Direction)
                .NewColumn(new Column(ColumnType.Number, "Код физ. лица"), x => x.abitur_id)
                .NewColumn(new Column(ColumnType.Number, "Инф."), x => x.scores_1)
                .NewColumn(new Column(ColumnType.Number, "Мат."), x => x.scores_2)
                .NewColumn(new Column(ColumnType.Number, "Рус."), x => x.scores_3)
                .NewColumn(new Column(ColumnType.Number, "Всего баллов"), x => x.total_scores)
               // .NewColumn(new Column(ColumnType.Number, "10.05.01"), x => x.ex_type)




                .Build()
                .GetJson();

            return Content(json);
        }
    }

}

