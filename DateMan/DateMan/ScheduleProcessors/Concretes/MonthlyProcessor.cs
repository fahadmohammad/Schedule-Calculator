using System;
using System.Collections.Generic;
using System.Linq;
using DateMan.ScheduleProcessors.Contracts;

namespace DateMan.ScheduleProcessors.Concretes
{
    public class MonthlyProcessor : IDateTimeProcessor
    {
        public List<SevenDaySchedule> SevenDaySchedules { get; private set; }

        public List<SevenDaySchedule> Process(List<SevenDaySchedule> sevenDaySchedules, FoodSchedule foodSchedule)
        {
            SevenDaySchedules = sevenDaySchedules;

            var timeSlots = foodSchedule.DateRanges;

            var startDay = timeSlots.Select(x => x.StartTime).FirstOrDefault();
            var endDay = timeSlots.Select(x => x.EndTime).FirstOrDefault();

            var scheduledMonths = foodSchedule.NumericSlots;

            foreach (var sevenDaySchedule in SevenDaySchedules)
            {
                var month = sevenDaySchedule.Day.Month;

                if (!scheduledMonths.Contains(month)) continue;

                if ((sevenDaySchedule.Day >= startDay && endDay != default && sevenDaySchedule.Day <= endDay)
                    || (sevenDaySchedule.Day >= startDay && endDay == default))
                {
                    sevenDaySchedule.TimeSlots.AddRange(foodSchedule.TimeSlots);
                    sevenDaySchedule.IsScheduledFromCms = true;
                }
            }

            return SevenDaySchedules;
        }

        //private List<int> GetRefMonths()
        //{
        //    return DataSets.Keys.First().Month != DataSets.Keys.Last().Month ? new List<int> { DataSets.Keys.First().Month, DataSets.Keys.Last().Month } 
        //        : new List<int> { DataSets.Keys.First().Month};
        //}
    }
}
