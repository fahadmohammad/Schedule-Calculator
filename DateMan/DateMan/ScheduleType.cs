using System;
using System.Collections.Generic;
using System.Text;

namespace DateMan
{
    public enum ScheduleTypeEnum
    {
        NONE = 0,
        YEAR_WEEKLY = 1,
        YEAR_MONTHLY = 2,
        MONTH_WEEKLY = 3,
        WEEKDAY = 4,
        SPECIFIC_DAYS = 5,
        DAILY = 6,
        MONTH_SPECIFIC = 7,
        YEAR_SPECIFIC = 8
    }
}
