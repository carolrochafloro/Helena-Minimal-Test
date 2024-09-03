namespace Helena_Minimal.Models;

public class Times
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public Guid MedicationId { get; set; }
    public bool IsTaken {  get; set; }

}
