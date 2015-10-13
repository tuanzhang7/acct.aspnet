using acct.common.POCO;
using acct.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace acct.web.Controllers
{
    [Authorize]
    public class ExpenseCategoryController : Controller
    {
        //
        // GET: /ExpenseCategory/
        ExpenseCategorySvc svc = new ExpenseCategorySvc();

        public ActionResult Index()
        {
            return View(svc.GetAll());
        }

        public ActionResult Create()
        {
            return View(new ExpenseCategory());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(ExpenseCategory ExpenseCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // TODO: Add insert logic here
                    this.UpdateModel(ExpenseCategory);
                    svc.Save(ExpenseCategory);
                    return RedirectToAction("Index");

                }
                catch
                {
                    return View(ExpenseCategory);
                }
            }
            return View(ExpenseCategory);
        }

        public ActionResult Edit(int id)
        {
            ExpenseCategory _entity = svc.GetById(id);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ExpenseCategory _entity = svc.GetById(id);
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    this.UpdateModel(_entity);
                    svc.Update(_entity);
                    return RedirectToAction("Index");

                }
                catch
                {
                    return View(_entity);
                }
            }
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            ExpenseCategory _entity = svc.GetById(id);
            //int schedules = _venue.Courseschedules.Count;
            //if (schedules > 0)
            //{
            //    string inuse = "Could not delete,Venue in Used";
            //    return Json(new { status = false, message = inuse });
            //}
            //else
            //{
            try
            {
                string succ = "1 record Deleted";
                svc.Delete(id);
                return Json(new { status = true, message = succ });
            }
            catch (Exception e)
            {
                return View(_entity);
            }
            //}

        }
    }
}
