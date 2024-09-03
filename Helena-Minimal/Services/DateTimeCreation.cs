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
}

