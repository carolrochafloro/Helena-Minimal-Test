using Helena_Minimal.DTO;
using Helena_Minimal.Models;

namespace Helena_Minimal.Services;

public class DateTimeCreation
{

    public List<Times> CreateDailyTimes(Guid medId, DateOnly start, DateOnly end, List<NewTimeDTO> times)
    {

        List<Times> newTimeList = new List<Times>();
        int timeDifference = end.DayNumber - start.DayNumber + 1;

       for (int i = 0; i < timeDifference; i++)
        {
            var currentDay = start.AddDays(i);

            foreach (var item in times)
            {
                foreach (var time in item.Time)
                {
                    TimeOnly convertedNewTime = TimeOnly.Parse(time);
                    DateTime correctDateTime = currentDay.ToDateTime(convertedNewTime);

                    correctDateTime = DateTime.SpecifyKind(correctDateTime, DateTimeKind.Utc);
                    Times newTime = new Times()
                    {

                        Id = Guid.NewGuid(),
                        MedicationId = medId,
                        DateTime = correctDateTime,
                        IsTaken = false,

                    };

                    newTimeList.Add(newTime);

                }
            }
        }

        return newTimeList;

    }

    public List<Times> CreateWeeklyTimes(Guid medId, DateOnly start, DateOnly end, List<NewTimeDTO> times)
    {
        List<Times> newTimes = new List<Times>();
        int timeDifference = end.DayNumber - start.DayNumber + 1;
       

        for (int i = 0; i < timeDifference; i++)
        {
            var currentDay = start.AddDays(i);

            foreach (var item in times)
            {
              
              
                if (item.WeekDay.Contains((int)currentDay.DayOfWeek)) {

                    foreach (var item1 in item.Time)
                    {
                        TimeOnly convertedNewTime = TimeOnly.Parse(item1);
                        DateTime correctDateTime = currentDay.ToDateTime(convertedNewTime);

                        correctDateTime = DateTime.SpecifyKind(correctDateTime, DateTimeKind.Utc);

                        var timeToAdd = new Times()
                        {
                            Id = Guid.NewGuid(),
                            MedicationId = medId,
                            IsTaken = false,
                            DateTime = correctDateTime,

                        };

                        newTimes.Add(timeToAdd);
                    }

                 

                }


        }

        }

        return newTimes;


    }
}

