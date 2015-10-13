using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acct.common.Helper
{
    public class Utility
    {
        

        public static List<DateTime> GetDatesByWeekDay(string weekdayName,
            DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom.CompareTo(dateTo) > 0)
            {
                throw new System.ArgumentException("dateFrom cannot be later then dateTo");
            }
            List<DateTime> DateList = new List<DateTime>();
            try
            {
                DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), weekdayName);

                for (DateTime d = dateFrom; d <= dateTo; d = d.AddDays(1))
                {
                    if (dayOfWeek == d.DayOfWeek)
                    {
                        DateList.Add(d);
                        d = d.AddDays(6);
                    }
                }
            }
            catch (Exception e)
            {
                throw new System.ArgumentException("WeekdayName is Invalid");
            }


            return DateList;
        }
        public static DateTime FormatToTime(DateTime datetime)
        {
            string myDateTimeYearString = "1/1/1800 ";
            string myTimeFromString = datetime.ToString("HH:mm");

            return DateTime.Parse(myDateTimeYearString + myTimeFromString);
        }
        public static DateTime JoinDateTime(DateTime date, TimeSpan time)
        {
            DateTime result = date + time;
            return result;
        }
        public static long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(
     new DateTime(1970, 1, 1, 0, 0, 0));

            return (long)Math.Truncate(tspan.TotalSeconds);
        }
        public static DateTime FromUnixTimespan(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        
        public static List<KeyValuePair<DateTime, DateTime>> SplitToWeek(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom.CompareTo(dateTo) > 0)
            {
                throw new System.ArgumentException("dateFrom cannot be later then dateTo");
            }

            List<KeyValuePair<DateTime, DateTime>> _List = new List<KeyValuePair<DateTime, DateTime>>();
            
            try
            {
                DateTime start=dateFrom;
                DateTime end;
                for (DateTime d = dateFrom; d <= dateTo; d = d.AddDays(1))
                {
                    if (d.DayOfWeek==DayOfWeek.Sunday||d==dateTo)
                    {
                        end = d;
                        //d = d.AddDays(6);
                        _List.Add(new KeyValuePair<DateTime, DateTime>(start, end));
                        start = d.AddDays(1);
                    }
                }
            }
            catch (Exception e)
            {
                throw new System.ArgumentException("WeekdayName is Invalid");
            }


            return _List;
        }
        //public static bool IsChineeze(char text)
        //{
        //    return ChineseChar.IsValidChar(text);
        //}
        //public static string ToPinYin(string str)
        //{

        //    string pinyin = string.Empty;

        //    foreach (var item in str.ToCharArray())
        //    {

        //        if (ChineseChar.IsValidChar(item))//是汉字
        //        {

        //            ChineseChar chars = new ChineseChar(item);

        //            pinyin += string.Format("{0} ", chars.Pinyins[0]);

        //            //chars.IsPolyphone属性标识是不是多音字，

        //            //chars.PinyinCount//拼音的个数

        //        }

        //    }
        //    pinyin = System.Text.RegularExpressions.Regex.Replace(pinyin, "\\d", string.Empty);
        //    return pinyin.Trim();

        //}
        
        public static TimeSpan ParseStopWatchTime(string StopWatchTime)
        {
            //01:12.6 1 min 12 sec

            TimeSpan duration;
            if (string.IsNullOrEmpty(StopWatchTime))
            {
                duration = TimeSpan.Zero;
            }
            else
            {
                StopWatchTime="00:" + StopWatchTime + "00";
                if (TimeSpan.TryParse(StopWatchTime ,out duration))
                {
                }
            }
            return duration;
        }
    }
}
