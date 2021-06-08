using System;
using System.Collections.Generic;
using System.Text;

namespace DateMan
{
    public class SevenDaySchedule
    {
        public DateTime Day { get; set; }
        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public bool IsScheduledFromCms { get; set; }
    }
}
