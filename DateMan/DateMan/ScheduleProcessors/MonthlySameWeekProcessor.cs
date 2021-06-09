using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DateMan.ScheduleProcessors.Helpers;

namespace DateMan.ScheduleProcessors
{
    public class MonthlySameWeekProcessor
    {
        public Dictionary<DateTime, List<TimeSlot>> DataSets { get; }
        public List<SevenDaySchedule> SevenDaySchedules { get; }

        private GregorianCalendar _gc;

        public MonthlySameWeekProcessor(Dictionary<DateTime, List<TimeSlot>> dataSets, List<SevenDaySchedule> sevenDaySchedules)
        {
            DataSets = dataSets;
            SevenDaySchedules = sevenDaySchedules;

            _gc = new GregorianCalendar();
        }

        public List<SevenDaySchedule> Process(FoodSchedule foodSchedule)
        {
            var timeSlots = foodSchedule.DateRanges;

            var startDay = timeSlots.Select(x => x.StartTime).FirstOrDefault();
            var endDay = timeSlots.Select(x => x.EndTime).FirstOrDefault();

            var weekNumbers = foodSchedule.NumericSlots;
            var daysOfWeek = foodSchedule.ChildFoodSchedule.NumericSlots.CreateDayOfWeeks();

            foreach (var sevenDaySchedule in SevenDaySchedules)
            {
                var weekNumber = sevenDaySchedule.Day.GetWeekOfMonth();

                if (!weekNumbers.Contains(weekNumber) 
                    || !daysOfWeek.Contains(sevenDaySchedule.Day.DayOfWeek)) continue;

                if ((sevenDaySchedule.Day.Date >= startDay.Date && endDay != default && sevenDaySchedule.Day.Date <= endDay.Date)
                    || (sevenDaySchedule.Day.Date >= startDay && endDay == default))
                {
                    sevenDaySchedule.TimeSlots.AddRange(foodSchedule.TimeSlots);
                    sevenDaySchedule.IsScheduledFromCms = true;
                }
            }

            return SevenDaySchedules;
        }


        
    }
}
