namespace Helena_Minimal.DTO;

public class NewTimeDTO
{
    public List<int> WeekDay { get; set; }
    public List<DateOnly> Dates { get; set; }
    public List<string> Time { get; set; }
}
