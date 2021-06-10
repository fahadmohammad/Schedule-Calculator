using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DateMan.ScheduleProcessors;
using DateMan.ScheduleProcessors.Concretes;
using DateMan.ScheduleProcessors.Factories;

namespace DateMan
{
    class Program
    {
        static void Main(string[] args)
        {
            //var lastDayOfTheMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            try
            {
                var datasets = CreateDataSet();
                var sevenDaySchedules = createSevenDaySchedules(datasets);

                var concept = LoadJson();

                foreach (var schedule in concept.ConceptSchedules.Where(x => !x.IsMarkedForDelete))
                {
                    var scheduleType = (ScheduleTypeEnum)schedule.ScheduleType;

                    switch (scheduleType)
                    {
                        case ScheduleTypeEnum.DAILY:
                        {
                            var dailyProcessor = DateTimeFactory<DailyProcessor>.CreateDateTimeProcessor();
                            sevenDaySchedules = dailyProcessor.Process(sevenDaySchedules, schedule);
                            break;
                        }
                        case ScheduleTypeEnum.WEEKDAY:
                        {
                            var weeklyProcessor = DateTimeFactory<WeeklyProcessor>.CreateDateTimeProcessor();
                            sevenDaySchedules = weeklyProcessor.Process(sevenDaySchedules, schedule);
                            break;
                        }
                        case ScheduleTypeEnum.YEAR_MONTHLY:
                        {
                            var monthlyProcessor = DateTimeFactory<MonthlyProcessor>.CreateDateTimeProcessor();
                            sevenDaySchedules = monthlyProcessor.Process(sevenDaySchedules, schedule);
                            break;
                        }
                        case ScheduleTypeEnum.YEAR_SPECIFIC:
                        {
                            var yearlyProcessor = DateTimeFactory<YearlyProcessor>.CreateDateTimeProcessor();
                                sevenDaySchedules = yearlyProcessor.Process(sevenDaySchedules, schedule);
                            break;
                        }
                        case ScheduleTypeEnum.MONTH_SPECIFIC:
                        {
                            var monDailyProcessor = DateTimeFactory<MonthlySameDateProcessor>.CreateDateTimeProcessor();
                                sevenDaySchedules = monDailyProcessor.Process(sevenDaySchedules ,schedule);
                            break;
                        }
                        case ScheduleTypeEnum.MONTH_WEEKLY:
                        {
                            var monWeeklyProcessor = DateTimeFactory<MonthlySameWeekProcessor>.CreateDateTimeProcessor();
                                sevenDaySchedules = monWeeklyProcessor.Process(sevenDaySchedules, schedule);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static IDictionary<DateTime, IList<TimeSlot>> PopulateDataSet(IList<DateTime> dates, IList<TimeSlot> timeSlots,
            IDictionary<DateTime, IList<TimeSlot>> dataSets)
        {
            var updatedDataSets = dataSets;

            foreach (var date in dates)
            {
                if (updatedDataSets.ContainsKey(date))
                {
                    updatedDataSets[date] = timeSlots;
                }
            }

            return updatedDataSets;
        }

       

        private static List<DateTime> GetDateOfDaysByWeek(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            var returnDates = new List<DateTime>();

            var limitIndex = endDate == default ? new DateTime(startDate.Year, 12, 31).Day : endDate.Day;

            var refDate = startDate;
            var refIndex = refDate.Day;

            while (refIndex <= limitIndex)
            {
                if (daysOfWeek.Contains(refDate.DayOfWeek))
                {
                    returnDates.Add(refDate);
                }

                refDate = refDate.AddDays(1);
                refIndex++;
            }

            return returnDates;
        }

        private static Dictionary<DateTime, List<TimeSlot>> CreateDataSet()
        {
            var dataSet = new Dictionary<DateTime, List<TimeSlot>>();
            var index = 1;
            var date = DateTime.UtcNow;

            while (index <= 7)
            {
                dataSet.Add(date, null);
                date = date.AddDays(1);
                index++;
            }

            return dataSet;
        }

        private static List<SevenDaySchedule> createSevenDaySchedules(IDictionary<DateTime, List<TimeSlot>> dataSets)
        {
            return dataSets.Select(item => new SevenDaySchedule {Day = item.Key, TimeSlots = item.Value ?? new List<TimeSlot>() }).ToList();
        }

        private static List<SevenDaySchedule> constructSevenDaySchedules(IList<DateTime> dates, IList<TimeSlot> timeSlots, List<SevenDaySchedule> sevenDaySchedules)
        {
            var updatedSevenDaySchedules = sevenDaySchedules;

            foreach (var date in dates)
            {
                var sevenDaySchedule = updatedSevenDaySchedules.FirstOrDefault(x => x.Day == date);

                if (sevenDaySchedule != null)
                {
                    if (sevenDaySchedule.TimeSlots == null)
                    {
                        sevenDaySchedule.TimeSlots = new List<TimeSlot>();
                    }

                    sevenDaySchedule.TimeSlots.AddRange(timeSlots);
                    sevenDaySchedule.IsScheduledFromCms = true;
                }
            }

            return sevenDaySchedules;
        }

        private static Concept LoadJson()
        {
            Concept concept;

            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "schedules.json");

            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                concept = JsonConvert.DeserializeObject<Concept>(json);
            }

            return concept;
        }
    }
}
