using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;
using acct.web.Helper;
using acct.common.Repository;
using System.Configuration;
using PagedList;
using acct.service.Helper;
using AutoMapper;
using acct.common.Helper;
namespace acct.web.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        //
        // GET: /Invoice/
        InvoiceSvc svc = new InvoiceSvc();
        GSTSvc gstSvc = new GSTSvc();
        OptionsSvc optionsSvc = new OptionsSvc();
        CustomerSvc customerSvc;
        SalesmanSvc salesmanSvc;
        OrderSvc orderSvc = new OrderSvc();
        ControllerHelper cHelper;
        public InvoiceController(ICustomerRepo iCustomerRepo, ISalesmanRepo iSalesmanRepo)
        {
            cHelper = new ControllerHelper(this, iCustomerRepo);
            customerSvc = new CustomerSvc(iCustomerRepo);
            salesmanSvc = new SalesmanSvc(iSalesmanRepo);
        }
        public ActionResult Index(int? page, int? dateRange, List<string> status)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;
            DateRange.DateRangeFilter drFilter = dateRange == null ? DateRange.DateRangeFilter.AnyTime : (DateRange.DateRangeFilter)dateRange;

            List<Order.StatusOptions> statusList = new List<Order.StatusOptions>();
            if (status != null)
            {
                foreach (var item in status)
                {
                    Order.StatusOptions _status =
                    (Order.StatusOptions)Enum.Parse(typeof(Order.StatusOptions), item);
                    statusList.Add(_status);
                }
            }
            else
            {
                statusList.Add(Order.StatusOptions.Unpaid);
            }
            IPagedList<Invoice> onePageOfProducts = svc.GetByFilter(statusList, null, drFilter)
                .ToPagedList(_page, pageSize);

            return View(onePageOfProducts);
        }
        public ActionResult Search(string q, int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            if (string.IsNullOrEmpty(q)) { return RedirectToAction("Index"); }
            IPagedList<Invoice> list = svc.GetAll().Where
                (o => o.OrderNumber.Contains(q))
                .ToList()
                .ToPagedList(_page, pageSize);

            ViewBag.q = q;

            return View("Index", list);
        }
        [Route("Invoice/Customer/{id}")]
        public ActionResult GetByCustomer(int id, int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            Customer customer = customerSvc.GetById(id);
            if (customer != null)
            {
                IPagedList<Invoice> list = svc.GetByCustomer(id)
                .ToPagedList(_page, pageSize);
                return View("Index", list);
            }
            return View("Index");
            
        }

        //home/about
        public ActionResult GetTopInvoices(int id, int count)
        {

            Salesman salesman = salesmanSvc.GetById(id);
            if (salesman != null)
            {
                IList<Invoice> list = svc.GetAll()
                    .Where(i => i.SalesmanId == id)
                    .OrderByDescending(i => i.OrderDate)
                    .Take(count)
                    .ToList();
                return View("Index", list);
            }
            return View("Index");

        }

        public ActionResult GetMonthlyTotal(DateTime year)
        {
            //var data=svc.GetMonthlyTotal(year);
            //IList<Invoice> list=svc.GetAll().Where(i => i.Order.OrderDate > year).ToList();

            //Error: LINQ to Entities does not recognize the method 'System.String Format(System.String, System.Object, System.Object)' method, and this method cannot be translated into a store expression.
            //var summary = from s in svc.GetAll().Where(i => i.Order.OrderDate > year)
            //              group s by new { month = s.Order.OrderDate.Month, year = s.Order.OrderDate.Year } into g
            //              orderby g.Key.year,g.Key.month
            //              select new
            //              {
            //                  month = string.Format("{0}/{1}", g.Key.month, g.Key.year),
            //                  total = g.Sum(x => x.Order.TotalAmount)
            //              };

            //Perfomance Issue
            //var summary = from s in list
            //              let k = new
            //              {
            //                  month = s.Order.OrderDate.ToString("yy/MM")
            //              }
            //              group s by k into g
            //              orderby g.Key.month
            //              select new
            //              {
            //                  month = g.Key.month,
            //                  total = g.Sum(x => x.Order.TotalAmount)
            //              };

            //var query = myList.GroupBy(i => i.DateTimeField.HasValue ? i.DateTimeField.Value.Month : 0)
            //      .Select(g => new
            //      {
            //          Month = g.Key,
            //          Total = g.Sum(i => i.AmountField)
            //      });

            var summary = from s in svc.GetAll().Where(i => i.OrderDate > year)
                          group s by new { month = s.OrderDate.Month, year = s.OrderDate.Year } into g
                          orderby g.Key.year, g.Key.month
                          select new
                          {
                              year = g.Key.year,
                              month = g.Key.month ,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json=from s in summary.ToList()
                     select new {
                         month = new DateTime(s.year, s.month, 1).ToString("MMM-yy", System.Globalization.CultureInfo.InvariantCulture),
                         total=s.total
                     };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYearlyTopCustomer(DateTime year)
        {
            var summary = from s in svc.GetAll().Where(i => i.OrderDate > year)
                          group s by new { customer = s.Customer.Name } into g
                          orderby g.Key.customer
                          select new
                          {
                              customer = g.Key.customer,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json = from s in summary.OrderByDescending(s=>s.total).Take(10).ToList()
                       select new
                       {
                           customer = s.customer.TruncateLongString(20),
                           total = Decimal.Round(s.total,0)
                       };

            decimal others_total = 0;
            if (summary.OrderByDescending(s => s.total).Skip(10).Any())
            {
                others_total = summary.OrderByDescending(s => s.total).Skip(10).Sum(s => s.total);
            }

            var others = new[]
            {
                new 
                {
                    customer = "Others",
                    total = Decimal.Round(others_total,0)
                }
            };

            json=json.Concat(others);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYearlyTopSalesman(DateTime year)
        {
            var summary = from s in svc.GetAll().Where(i => i.OrderDate > year)
                          group s by new { salesman = s.Salesman.Name } into g
                          orderby g.Key.salesman
                          select new
                          {
                              salesman = g.Key.salesman,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json = from s in summary.OrderByDescending(s => s.total).Take(10).ToList()
                       select new
                       {
                           salesman = s.salesman.TruncateLongString(20),
                           total = Decimal.Round(s.total, 0)
                       };

            decimal others_total = 0;
            if(summary.OrderByDescending(s => s.total).Skip(10).Any()){
                others_total=summary.OrderByDescending(s => s.total).Skip(10).Sum(s => s.total);
            }
            


            var others = new[]
            {
                new 
                {
                    salesman = "Others",
                    total = Decimal.Round(others_total,0)
                }
            };

            json = json.Concat(others);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public int GetISOWeek(DateTime day){
            return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(day, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        public ActionResult Create(int? id)
        {
            Invoice _item = new Invoice();

            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code");
            ViewBag.CustomerId = cHelper.GetCustomerDropDown(id);
            ViewBag.NewDetail = new OrderDetail();

            _item.OrderNumber = optionsSvc.GetNextInvoiceNumber();
            _item.OrderDate = DateTime.Today;

            return View(_item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Invoice entity, FormCollection collection)
        {
            
            if (ModelState.IsValid)
            {
                entity.OrderDetail = entity.OrderDetail.
                                        Where(s => !string.IsNullOrWhiteSpace(s.Description)).ToList();
                if (orderSvc.IsExist(entity.OrderNumber))
                {
                    ModelState.AddModelError("OrderNumber", "Duplicate Number");
                    ViewBag.CustomerId = cHelper.GetCustomerDropDown();
                    return View(entity);
                }

                svc.Save(entity);
                optionsSvc.SetNextInvoiceNumber(entity.OrderNumber);
                return RedirectToAction("Details", new { id = entity.Id });
            }

            ViewBag.CustomerId = cHelper.GetCustomerDropDown();
            return View(entity);
        }

        public ActionResult Details(int id)
        {
            Invoice _entity = svc.GetById(id);
            return View(_entity);
        }

        public ActionResult Print(int id)
        {
            Invoice _entity = svc.GetById(id);

            acct.report.DTO.Helper.Mapping();
            Mapper.AssertConfigurationIsValid();

            Invoice dto = Mapper.Map<acct.common.POCO.Invoice, Invoice>(_entity);

            Byte[] bytes = cHelper.GetOrderReport((Order)dto, "Invocie");
            string fileName = "Invocie_"+_entity.OrderNumber + ".pdf";
            return File(bytes, "application/pdf", fileName);
        }
        public ActionResult PrintDO(int id)
        {
            Invoice _entity = svc.GetById(id);

            acct.report.DTO.Helper.Mapping();
            Mapper.AssertConfigurationIsValid();

            Invoice dto = Mapper.Map<acct.common.POCO.Invoice, Invoice>(_entity);

            Byte[] bytes = cHelper.GetOrderReport(dto, "Delivery Order", "~/Content/reports/order/DeliveryOrder.rdlc");
            string fileName = "DO_" + _entity.OrderNumber + ".pdf";
            return File(bytes, "application/pdf", fileName);
        }
        
        public ActionResult Edit(int id)
        {
            Invoice _entity = svc.GetById(id);
            ViewBag.CustomerIds = cHelper.GetCustomerDropDown(_entity.CustomerId);
            return View(_entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Invoice entity, FormCollection collection)
        {
            Invoice _invoice = svc.GetById(id);

            if (ModelState.IsValid)
            {
                //update Order Header
                //Disable below as no compile time error when change fields name
                //UpdateModel(_entity, new[] { "OrderDate", "OrderNumber", "Remark", "CustomerId" });
                _invoice.OrderDate = entity.OrderDate;
                _invoice.OrderNumber = entity.OrderNumber;
                _invoice.Remark = entity.Remark;
                _invoice.GSTRate = entity.GSTRate;
                _invoice.CustomerId = entity.CustomerId;
                
                //update Order detail
                foreach (var detail in _invoice.OrderDetail)
                {
                    UpdateModel(detail, "OrderDetails[" + detail.Id + "]");
                }
                //new details
                entity.OrderDetail = entity.OrderDetail.
                                        Where(s => !string.IsNullOrWhiteSpace(s.Description)).ToList();
                if (entity.OrderDetail != null && entity.OrderDetail.Count > 0)
                {
                    foreach (var item in entity.OrderDetail)
                    {
                        _invoice.OrderDetail.Add(item);
                    }
                }
                
                try
                {
                    svc.Update(_invoice);
                }
                catch (Exception e)
                {

                }
                return RedirectToAction("Details", new { id = _invoice.Id });
            }
            ViewBag.CustomerId = cHelper.GetCustomerDropDown();
            return View(_invoice);
        }
    }
}
