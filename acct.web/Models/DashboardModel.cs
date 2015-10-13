using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace acct.web.Models
{
    public class DashboardModel
    {
        public decimal OpenInvOutstandingAmount { get; set; }
        public int TotalNumOfOpenInv { get; set; }
        public decimal ReceivedAmount { get; set; }
        public int TotalNumOfPaidInv { get; set; }
    }
}