using Helena_Minimal.DTO;
using Helena_Minimal.Models;

namespace Helena_Minimal.Services;

public class DateTimeCreation
{

    /* Inserir exceptions para agendamento maior do que X anos a partir da data atual e incluir blocos try/catch */

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

                if (item.WeekDay.Contains((int)currentDay.DayOfWeek))
                {

                    foreach (var item1 in item.Time)
                    {
                        TimeOnly convertedNewTime = TimeOnly.Parse(item1);
                        DateTime correctDateTime = currentDay.ToDateTime(convertedNewTime);

                        if (correctDateTime.Year > 2026)
                        {
                            break;
                        }

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

    public List<Times> CreateMonthlyTimes(Guid medId, DateOnly start, DateOnly end, List<NewTimeDTO> times)
    {
        /* Criar novo método para yearly, está adicionando meses - ou add verificação, mas precisaria receber frequency type */
        List<Times> newTimes = new List<Times>();
        int timeInterval = end.DayNumber - start.DayNumber + 1;


        foreach (var time in times)
        {

            foreach (var item in time.Dates)
            {

                foreach (var item1 in time.Time)
                {
                    TimeOnly convertedTime = TimeOnly.Parse(item1);
                    var day = item.Day;
                    var month = item.Month;
                    var year = item.Year;

                    for (int i = 0; i < timeInterval; i++)
                    {
                        DateOnly theDate = new DateOnly(year, month, day).AddMonths(i);

                        if (theDate > end)
                        {
                            break;
                        }

                        if (theDate.Year > 2026)
                        {

                            break;
                        }


                        DateTime correctDate = item.ToDateTime(convertedTime);
                        correctDate = DateTime.SpecifyKind(correctDate, DateTimeKind.Utc);

                            var timeToAdd = new Times
                            {
                                Id = Guid.NewGuid(),
                                DateTime = correctDate,
                                MedicationId = medId,
                                IsTaken = false
                            };

                            newTimes.Add(timeToAdd);
                        
                    }

                }

            }

            
        }

        return newTimes;
    }

    public List<Times> CreateYearlyTimes(Guid medId, DateOnly start, DateOnly end, List<NewTimeDTO> times)
    {
        var newTimes = new List<Times>();
        int timeInterval = end.DayNumber - start.DayNumber + 1;

        foreach (var time in times)
        {

            foreach (var item in time.Dates)
            {

                foreach (var item1 in time.Time)
                {
                    TimeOnly convertedTime = TimeOnly.Parse(item1);
                    var day = item.Day;
                    var month = item.Month;
                    var year = item.Year;

                    for (int i = 0; i < timeInterval; i++)
                    {
                        DateOnly theDate = new DateOnly(year, month, day).AddYears(i);

                        if (theDate > end)
                        {
                            break;
                        }

                        if (theDate.Year > 2026)
                        {

                            break;
                        }


                        DateTime correctDate = item.ToDateTime(convertedTime);
                        correctDate = DateTime.SpecifyKind(correctDate, DateTimeKind.Utc);

                        var timeToAdd = new Times
                        {
                            Id = Guid.NewGuid(),
                            DateTime = correctDate,
                            MedicationId = medId,
                            IsTaken = false
                        };

                        newTimes.Add(timeToAdd);

                    }

                }

            }


        }

        return newTimes;
    }

}
