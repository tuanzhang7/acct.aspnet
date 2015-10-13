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
    public class SalesmanController : Controller
    {
        //
        // GET: /Salesman/
        SalesmanSvc svc = new SalesmanSvc();

        public ActionResult Index()
        {
            return View(svc.GetAll());
        }

        public ActionResult Create()
        {
            return View(new Salesman());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Salesman Salesman)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    // TODO: Add insert logic here
                    this.UpdateModel(Salesman);
                    svc.Save(Salesman);
                    return RedirectToAction("Index");
                    
                }
                catch
                {
                    return View(Salesman);
                }
            }
            return View(Salesman);
        }

        public ActionResult Edit(int id)
        {
            Salesman _entity = svc.GetById(id);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Salesman _entity = svc.GetById(id);
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
            Salesman _entity = svc.GetById(id);
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
