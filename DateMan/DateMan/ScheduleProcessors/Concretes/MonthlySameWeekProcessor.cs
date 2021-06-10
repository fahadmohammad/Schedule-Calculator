using System;
using System.Collections.Generic;
using System.Linq;
using DateMan.ScheduleProcessors.Contracts;
using DateMan.ScheduleProcessors.Helpers;

namespace DateMan.ScheduleProcessors.Concretes
{

    public class MonthlySameWeekProcessor : IDateTimeProcessor
    {
        public List<SevenDaySchedule> SevenDaySchedules { get; private set; }

        public List<SevenDaySchedule> Process(List<SevenDaySchedule> sevenDaySchedules, FoodSchedule foodSchedule)
        {
            SevenDaySchedules = sevenDaySchedules;

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
