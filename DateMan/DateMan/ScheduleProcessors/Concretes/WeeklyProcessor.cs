using System;
using System.Collections.Generic;
using System.Linq;
using DateMan.ScheduleProcessors.Contracts;
using DateMan.ScheduleProcessors.Helpers;

namespace DateMan.ScheduleProcessors.Concretes
{
    public class WeeklyProcessor : IDateTimeProcessor
    {
        public List<SevenDaySchedule> SevenDaySchedules { get; private set; }

        public List<SevenDaySchedule> Process(List<SevenDaySchedule> sevenDaySchedules, FoodSchedule foodSchedule)
        {
            SevenDaySchedules = sevenDaySchedules;

            var daysOfWeek = foodSchedule.NumericSlots.CreateDayOfWeeks();

            var timeSlots = foodSchedule.DateRanges;

            var startDay = timeSlots.Select(x => x.StartTime).FirstOrDefault();
            var endDay = timeSlots.Select(x => x.EndTime).FirstOrDefault();

            foreach (var sevenDaySchedule in SevenDaySchedules)
            {
                if (!daysOfWeek.Contains(sevenDaySchedule.Day.DayOfWeek)) continue;

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
