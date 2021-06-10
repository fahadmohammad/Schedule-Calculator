using System;
using System.Collections.Generic;
using System.Linq;
using DateMan.ScheduleProcessors.Contracts;

namespace DateMan.ScheduleProcessors.Concretes
{
    public class YearlyProcessor : IDateTimeProcessor
    {
        public List<SevenDaySchedule> SevenDaySchedules { get; private set; }

        public List<SevenDaySchedule> Process(List<SevenDaySchedule> sevenDaySchedules, FoodSchedule foodSchedule)
        {
            SevenDaySchedules = sevenDaySchedules;

            var startDay = foodSchedule.DateRanges.Select(x => x.StartTime).FirstOrDefault();
            var endDay = foodSchedule.DateRanges.Select(x => x.EndTime).FirstOrDefault();

            foreach (var sevenDaySchedule in SevenDaySchedules)
            {
                var isMonthAndDayEqual = sevenDaySchedule.Day.Month == startDay.Month &&
                                         sevenDaySchedule.Day.Day == startDay.Day;

                if ((endDay == default && isMonthAndDayEqual)
                    || (endDay != default && isMonthAndDayEqual && endDay >= sevenDaySchedule.Day))
                {
                    sevenDaySchedule.TimeSlots.AddRange(foodSchedule.TimeSlots);
                    sevenDaySchedule.IsScheduledFromCms = true;
                }

            }

            return SevenDaySchedules;
        }
    }
}
