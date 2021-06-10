using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DateMan.ScheduleProcessors.Helpers
{
    public static class WeekHelper
    {
        private static readonly GregorianCalendar Gc = new GregorianCalendar();
        public static List<System.DayOfWeek> CreateDayOfWeeks(this List<int> numericSlots)
        {
            return numericSlots.Select(numericSlot => (System.DayOfWeek)numericSlot).ToList();
        }

        public static int GetWeekOfMonth(this DateTime dateTime)
        {
            var first = new DateTime(dateTime.Year, dateTime.Month, 1);
            return dateTime.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        private static int GetWeekOfYear(this DateTime time)
        {
            return Gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

    }
}
