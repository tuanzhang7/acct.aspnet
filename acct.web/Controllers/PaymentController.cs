using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;
using acct.web.Helper;

namespace acct.web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/
        PaymentSvc svc = new PaymentSvc();
        //PaymentDetailSvc pdSvc = new PaymentDetailSvc();
        GSTSvc gstSvc = new GSTSvc();
        public ActionResult Index()
        {
            IList<Payment> list=svc.GetAll().Take(10).ToList();
            return View(list);
        }
        public ActionResult Create()
        {
            ViewBag.idmas_GST = new SelectList(gstSvc.GetAll(), "Id", "Code");

            Payment _payment = new Payment();
            return View(_payment);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Payment payment, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                int invoiceId_payment = int.Parse(collection["invoiceId_payment"]);
                int customerId_payment = int.Parse(collection["customerId_payment"]);
                payment.CustomerId = invoiceId_payment;
                svc.Save(payment);
                IList<PaymentDetail> list = null;
                        //svc.GetAll().Where(p=>p.PaymentDetail==payment.InvoiceId)
                        //.ToList() ;

                return Json(new
                {
                    success = true,
                    html = this.RenderViewToString("~/Views/Payment/_ListItem.cshtml", list)
                });
            }
            return Json(new
            {
                success = false,
                errors = ModelState.Errors(),
                html = ModelState.Errors()
            });
        }

        public ActionResult Edit(int id)
        {
            Payment _entity = svc.GetById(id);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id,Payment payment, FormCollection collection)
        {
            Payment _entity = svc.GetById(id);
            if (ModelState.IsValid)
            {
                _entity.PaymentMethod = collection["PaymentMethod"];
                _entity.ReferenceNumber = collection["ReferenceNumber"];
                _entity.Remarks = collection["Remarks"];

                _entity.Amount = decimal.Parse(collection["Amount"]);
                _entity.CustomerId = int.Parse(collection["CustomerId"]);
                try
                {
                    svc.Update(_entity);
                }
                catch (Exception e)
                {
                    
                }
                

                return RedirectToAction("Details", new { id = _entity.Id });
            }
            return View(_entity);
        }

        public ActionResult Details(int id)
        {
            Payment _entity = svc.GetById(id);
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

    }
}
