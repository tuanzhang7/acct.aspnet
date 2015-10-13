using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.common.Helper
{
    public class DateRange
    {
        public enum DateRangeFilter
        {
            [Display(Name = "Any Time")]
            AnyTime = 0,
            [Display(Name = "Current Year")]
            ThisYear = 1,
            [Display(Name = "Current Month")]
            ThisMonth = 2,
            [Display(Name = "Past 3 Month")]
            Last3Month = 3,
            [Display(Name = "Past 7 Days")]
            Last7Days = 4,
            [Display(Name = "Past 365 Days")]
            Last365Days = 5
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime CurrentDate,DateRangeFilter filter )
        {
            if (filter == DateRangeFilter.ThisMonth)
            {
                StartDate = GetFirstDayOfMonth(CurrentDate);
                EndDate = GetLastDayOfMonth(CurrentDate);
            }
            else if (filter == DateRangeFilter.ThisYear)
            {
                StartDate = GetFirstDayOfYear(CurrentDate);
                EndDate = GetLastDayOfYear(CurrentDate);
            }
            else if (filter == DateRangeFilter.Last7Days)
            {
                EndDate = CurrentDate;
                StartDate = EndDate.AddDays(-7);
            }
            else if (filter == DateRangeFilter.Last3Month)
            {
                EndDate = CurrentDate;
                StartDate = EndDate.AddMonths(-3);
            }
            else if (filter == DateRangeFilter.Last365Days)
            {
                EndDate = CurrentDate;
                StartDate = EndDate.AddYears(-1);
            }
            else if (filter == DateRangeFilter.AnyTime)
            {
                StartDate = DateTime.MinValue;
                EndDate = GetLastDayOfYear(CurrentDate);
            }
        }

        public static DateRange GetDateRange(DateTime CurrentDate, DateRangeFilter Filter)
        {
            return new DateRange(CurrentDate, Filter);
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            DateTime d;
            d = new DateTime(date.Year, date.Month, 1);
            return d;
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            DateTime firstDay = GetFirstDayOfMonth(date);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            return lastDay;
        }

        public static DateTime GetFirstDayOfYear(DateTime date)
        {
            DateTime d;
            d = new DateTime(date.Year, 1, 1);
            return d;
        }

        public static DateTime GetLastDayOfYear(DateTime date)
        {
            DateTime firstDay = GetFirstDayOfYear(date);
            DateTime lastDay = firstDay.AddYears(1).AddDays(-1);
            return lastDay;
        }
    }
}
