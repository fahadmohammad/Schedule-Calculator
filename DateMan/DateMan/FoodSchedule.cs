using System;
using System.Collections.Generic;
using System.Text;

namespace DateMan
{
    public class FoodSchedule
    {
        public FoodSchedule()
        {
            NumericSlots = new List<int>();
            SpecificDateSlots = new List<DateTime>();
            TimeSlots = new List<TimeSlot>();
        }
        public string ItemId { get; set; }
        public string Title { get; set; }
        public bool IsMarkedForDelete { get; set; }
        public bool FillEverySlot { get; set; }
        public int ScheduleType { get; set; }
        public List<int> NumericSlots { get; set; }
        public List<DateTime> SpecificDateSlots { get; set; }
        public List<TimeSlot> DateRanges { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
        public FoodSchedule ChildFoodSchedule { get; set; } 
        public DateTime LastModifiedDate { get; set; }
    }
}
