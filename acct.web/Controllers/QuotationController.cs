using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;
using acct.common.Repository;
using System.Configuration;
using PagedList;
using acct.web.Helper;
using AutoMapper;

namespace acct.web.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        //
        // GET: /Quotation/
        QuotationSvc svc = new QuotationSvc();
        OptionsSvc optionsSvc = new OptionsSvc();
        GSTSvc gstSvc = new GSTSvc();
        CustomerSvc customerSvc;
        OrderSvc orderSvc = new OrderSvc();
        ControllerHelper cHelper;
        public QuotationController(ICustomerRepo iCustomerRepo)
        {
            cHelper = new ControllerHelper(this, iCustomerRepo);
            customerSvc = new CustomerSvc(iCustomerRepo); 
        }
        public ActionResult Index(int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            IPagedList<Quotation> onePageOfProducts = svc.GetAll()
                .OrderByDescending(x => x.OrderNumber)
                .ToPagedList(_page, pageSize);

            return View(onePageOfProducts);
        }
        public ActionResult Search(string q, int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            if (string.IsNullOrEmpty(q)) { return RedirectToAction("Index"); }
            IPagedList<Quotation> list = svc.GetAll().Where
                (o => o.OrderNumber.Contains(q))
                .ToList()
                .ToPagedList(_page, pageSize);

            ViewBag.q = q;

            return View("Index", list);
        }
        public ActionResult Create(int? id)
        {
            Quotation _item = new Quotation();

            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code");
            ViewBag.CustomerId = cHelper.GetCustomerDropDown(id);
            ViewBag.NewDetail = new OrderDetail();

            _item.OrderNumber = optionsSvc.GetNextQuotationNumber();
            _item.OrderDate = DateTime.Today;

            return View(_item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Quotation entity, FormCollection collection)
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
                optionsSvc.SetNextQuotationNumber(entity.OrderNumber);
                return RedirectToAction("Details", new { id = entity.Id });
            }

            ViewBag.CustomerId = cHelper.GetCustomerDropDown();
            return View(entity);
        }

        public ActionResult Details(int id)
        {
            Quotation _entity = svc.GetById(id);
            return View(_entity);
        }

        public ActionResult Edit(int id)
        {
            Quotation _entity = svc.GetById(id);
            ViewBag.CustomerIds = cHelper.GetCustomerDropDown(_entity.CustomerId);
            return View(_entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Quotation entity, FormCollection collection)
        {
            Quotation _entity = svc.GetById(id);

            if (ModelState.IsValid)
            {
                _entity.OrderDate = entity.OrderDate;
                _entity.OrderNumber = entity.OrderNumber;
                _entity.Remark = entity.Remark;
                _entity.GSTRate = entity.GSTRate;
                _entity.CustomerId = entity.CustomerId;

                //update Order detail
                foreach (var detail in _entity.OrderDetail)
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
                        _entity.OrderDetail.Add(item);
                    }
                }

                try
                {
                    svc.Update(_entity);
                }
                catch (Exception e)
                {

                }
                return RedirectToAction("Details", new { id = _entity.Id });
            }
            ViewBag.CustomerId = cHelper.GetCustomerDropDown();
            return View(_entity);
        }

        public ActionResult Print(int id)
        {
            Quotation _entity = svc.GetById(id);

            acct.report.DTO.Helper.Mapping();
            Mapper.AssertConfigurationIsValid();

            Quotation dto = Mapper.Map<acct.common.POCO.Quotation, Quotation>(_entity);

            Byte[] bytes = cHelper.GetOrderReport(dto, "Quotation", "~/Content/reports/order/order.rdlc");
            string fileName = "Quotation_" + _entity.OrderNumber + ".pdf";
            return File(bytes, "application/pdf", fileName);
        }
    }
}
