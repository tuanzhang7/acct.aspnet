using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;
using acct.common.Repository;
using acct.web.Helper;
using AutoMapper;
using System.Configuration;
using PagedList;

namespace acct.web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        
        CustomerSvc svc;
        GSTSvc gstSvc = new GSTSvc();
        InvoiceSvc invoiceSvc = new InvoiceSvc();
        ControllerHelper cHelper;
        public CustomerController(ICustomerRepo iCustomerRepo)
        {
            svc = new CustomerSvc(iCustomerRepo);
            cHelper = new ControllerHelper(this, iCustomerRepo);
        }
        public ActionResult ngIndex()
        {
            return View();

        }
        public ActionResult ngDetails(int id)
        {
            return View();

        }
        public ActionResult Index(int? page,string q)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            var list = svc.GetAll();

            if (!string.IsNullOrEmpty(q)) {
                list = svc.Search(q);
                ViewBag.q = q;
            }

            IPagedList<Customer> pagedList = list.OrderBy(x => x.Name)
                .ToPagedList(_page, pageSize);

            return View(pagedList);

        }
        public ActionResult Lookup(string q, int limit=5)
        {
            IList<Customer> entity = null;
            if (!string.IsNullOrEmpty(q))// && q.Length >= 2)
            {
                entity = svc.GetAll().Where
                    (o => o.Name.Contains(q))
                .ToList();

                if (entity != null)
                {
                    var returnValue = from c in entity
                                    .Take(limit)
                                      orderby c.Name
                                      select new
                                      {
                                          id = c.Id.ToString(),
                                          value = c.Name
                                      };
                    return Json(returnValue, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code");

            Customer _customer = new Customer();
            return View(_customer);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Customer customer, FormCollection collection)
        {
            if (ModelState.IsValid)
            {

                this.UpdateModel(customer);


                svc.Save(customer);

                return RedirectToAction("Details", new { id = customer.Id });
            }
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code");
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            Customer _entity = svc.GetById(id);
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code", _entity.idmas_GST);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id,Customer customer, FormCollection collection)
        {
            Customer _entity = svc.GetById(id);
            if (ModelState.IsValid)
            {
                _entity.Name = collection["Name"];
                _entity.Address = collection["Address"];
                _entity.Phone = collection["Phone"];
                _entity.Fax = collection["Fax"];
                _entity.Email = string.IsNullOrEmpty(collection["Email"])?null:collection["Email"];
                _entity.ContactName = collection["ContactName"];

                _entity.idmas_GST =int.Parse(collection["idmas_GST"]);
                try
                {
                    svc.Update(_entity);
                }
                catch (Exception e)
                {
                    
                }
                

                return RedirectToAction("Details", new { id = _entity.Id });
            }
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code", _entity.idmas_GST);
            return View(_entity);
        }

        public ActionResult Details(int id)
        {
            Customer _entity = svc.GetById(id);
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code", _entity.idmas_GST);

            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int page = 1;
            int _page = page == null ? 1 : (int)page;

            IPagedList<Invoice> list = invoiceSvc.GetByCustomer(id)
                .ToPagedList(_page, pageSize);
            ViewBag.RecentInvoice = list;

            decimal balance=svc.GetBalance(id);
            ViewBag.balance = balance.ToString("C");

            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            try
            {
                string succ = "1 record Deleted";
                svc.Delete(id);
                return Json(new { status = true, message = succ });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "error when delete" });
            }
            //}

        }
        public ActionResult Print()
        {
            IList <Customer> list = svc.GetAll().Take(10).ToList();

            Mapper.AssertConfigurationIsValid();
            List<Customer> reportList = new List<Customer>();
            foreach (var item in list)
            {
                Customer cust =
                    Mapper.Map<Customer, Customer>(item);
                reportList.Add(cust);
            }

            string ReportFile = "~/Content/reports/order/Customers.rdlc";
            string DataSourceName = "DataSet_Customers";


            Byte[] bytes = cHelper.GetReport(ReportFile, DataSourceName, reportList);

            string fileName = "Customers.pdf";
            return File(bytes, "application/pdf", fileName);
        }

    }
}
