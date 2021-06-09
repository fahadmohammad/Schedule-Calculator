using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DateMan.ScheduleProcessors
{
    public class YearProcessor
    {
        public Dictionary<DateTime, List<TimeSlot>> DataSets { get; }
        public List<SevenDaySchedule> SevenDaySchedules { get; }

        public YearProcessor(Dictionary<DateTime, List<TimeSlot>> dataSets, List<SevenDaySchedule> sevenDaySchedules)
        {
            DataSets = dataSets;
            SevenDaySchedules = sevenDaySchedules;
        }

        public List<SevenDaySchedule> Process(FoodSchedule foodSchedule)
        {
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
