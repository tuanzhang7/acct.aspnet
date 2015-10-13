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
    public class GSTController : Controller
    {
        //
        // GET: /GST/
        GSTSvc svc = new GSTSvc();

        public ActionResult Index()
        {
            return View(svc.GetAll());
        }

        public ActionResult Create()
        {
            return View(new GST());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(GST GST)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    // TODO: Add insert logic here
                    this.UpdateModel(GST);
                    svc.Save(GST);
                    return RedirectToAction("Index");
                    
                }
                catch
                {
                    return View(GST);
                }
            }
            return View(GST);
        }

        public ActionResult Edit(int id)
        {
            GST _entity = svc.GetById(id);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            GST _entity = svc.GetById(id);
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
            GST _entity = svc.GetById(id);
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
