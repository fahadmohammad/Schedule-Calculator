using System;
using System.Collections.Generic;
using System.Text;
using DateMan.ScheduleProcessors.Concretes;
using DateMan.ScheduleProcessors.Contracts;

namespace DateMan.ScheduleProcessors.Factories
{
    public static class DateTimeFactory<T> where T: IDateTimeProcessor, new()
    {
        public static IDateTimeProcessor CreateDateTimeProcessor()
        {
            return new T();
        }
    }
}
