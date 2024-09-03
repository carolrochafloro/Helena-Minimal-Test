namespace Helena_Minimal.DTO;

public class DayMedicationDTO
{
    public Guid MedicationId { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Dosage { get; set; }

    public List<TimeDTO> Times { get; set; }

}
