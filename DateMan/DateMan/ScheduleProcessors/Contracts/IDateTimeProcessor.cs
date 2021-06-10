using System;
using System.Collections.Generic;
using System.Text;

namespace DateMan.ScheduleProcessors.Contracts
{
    public interface IDateTimeProcessor
    {
        //Dictionary<DateTime, List<TimeSlot>> DataSets { get; }
        List<SevenDaySchedule> SevenDaySchedules { get; }
        List<SevenDaySchedule> Process(List<SevenDaySchedule> sevenDaySchedules, FoodSchedule foodSchedule);
    }
}
