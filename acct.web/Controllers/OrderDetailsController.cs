using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;

namespace acct.web.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        //
        // GET: /Order/
        OrderDetailSvc svc = new OrderDetailSvc();
        
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
        }
    }
}
