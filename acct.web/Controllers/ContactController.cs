using acct.common.POCO;
using acct.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace acct.web.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /GST/
        UnitMeasureSvc svc = new UnitMeasureSvc();

        public ActionResult Index()
        {
            return View(svc.GetAll());
        }

        public ActionResult Create()
        {
            return View(new UnitMeasure());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(UnitMeasure entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    // TODO: Add insert logic here
                    this.UpdateModel(entity);
                    svc.Save(entity);
                    return RedirectToAction("Index");
                    
                }
                catch
                {
                    return View(entity);
                }
            }
            return View(entity);
        }

        public ActionResult Edit(int id)
        {
            UnitMeasure _entity = svc.GetById(id);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            UnitMeasure _entity = svc.GetById(id);
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
            UnitMeasure _entity = svc.GetById(id);
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
