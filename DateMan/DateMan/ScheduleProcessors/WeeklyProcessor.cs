using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DateMan.ScheduleProcessors.Helpers;

namespace DateMan.ScheduleProcessors
{
    public class WeeklyProcessor
    {
        public Dictionary<DateTime, List<TimeSlot>> DataSets { get; }
        public List<SevenDaySchedule> SevenDaySchedules { get; }

        public WeeklyProcessor(Dictionary<DateTime, List<TimeSlot>> dataSets, List<SevenDaySchedule> sevenDaySchedules)
        {
            DataSets = dataSets;
            SevenDaySchedules = sevenDaySchedules;
        }

        public List<SevenDaySchedule> Process(FoodSchedule foodSchedule)
        {
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
