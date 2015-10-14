﻿using acct.common.POCO;
using acct.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace acct.webapi.Controllers
{
    public class DashboardController : ApiController
    {
        public IHttpActionResult Get()
        {
            InvoiceSvc svc = new InvoiceSvc();

            List<Invoice> OpenInvoicesLastYear = svc.GetByFilter(new List<Order.StatusOptions>{ Order.StatusOptions.Paid }, null, common.Helper.DateRange.DateRangeFilter.Last365Days).ToList();
            decimal OpenInvOutstandingAmount = OpenInvoicesLastYear == null ? 0 : OpenInvoicesLastYear.Sum(i => i.AmountOutstanding);
            int TotalNumOfOpenInv = OpenInvoicesLastYear == null ? 0 : OpenInvoicesLastYear.Count();

            List<Order.StatusOptions> Closed = new List<Order.StatusOptions> {
                Order.StatusOptions.Unpaid,
                Order.StatusOptions.Partial,
                Order.StatusOptions.Overdue,
            };
            decimal ReceivedAmount = svc.GetByFilter(Closed, null, common.Helper.DateRange.DateRangeFilter.Last3Month)
                .Select(c => c.AmountPaid)
                .DefaultIfEmpty()
                .Sum();
            int TotalNumOfPaidInv = svc.GetByFilter(Closed, null, common.Helper.DateRange.DateRangeFilter.Last3Month)
                .Count();


            var dashModel = new
            {
                OpenInvOutstandingAmount = OpenInvOutstandingAmount,
                TotalNumOfOpenInv = TotalNumOfOpenInv,
                ReceivedAmount = ReceivedAmount,
                TotalNumOfPaidInv = TotalNumOfPaidInv
            };
            return Ok(dashModel);
        }
    }
}
