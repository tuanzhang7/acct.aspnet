using acct.common.POCO;
using acct.service;
using acct.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace acct.web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            InvoiceSvc svc = new InvoiceSvc();

            List<Invoice> OpenInvoicesLastYear= svc.GetOpenInvoicesLastYear();
            decimal OpenInvOutstandingAmount = OpenInvoicesLastYear == null ? 0 : OpenInvoicesLastYear.Sum(i => i.AmountOutstanding);
            int TotalNumOfOpenInv = OpenInvoicesLastYear == null ? 0 : OpenInvoicesLastYear.Count();


            decimal ReceivedAmount = svc.GetByFilter("closed",null, common.Helper.DateRange.DateRangeFilter.Last3Month)
                .Select(c => c.AmountPaid)
                .DefaultIfEmpty()
                .Sum();
            int TotalNumOfPaidInv = svc.GetByFilter("closed", null, common.Helper.DateRange.DateRangeFilter.Last3Month)
                .Count();

            DashboardModel dashModel = new DashboardModel();
            dashModel.OpenInvOutstandingAmount = OpenInvOutstandingAmount;
            dashModel.TotalNumOfOpenInv = TotalNumOfOpenInv;

            dashModel.ReceivedAmount = ReceivedAmount;
            dashModel.TotalNumOfPaidInv = TotalNumOfPaidInv;

            return View(dashModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
