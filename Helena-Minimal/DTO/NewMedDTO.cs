namespace Helena_Minimal.DTO;

public class NewMedDTO
{
    public string Name { get; set; }
    public string Lab { get; set; }
    public string Type { get; set; }
    public string Dosage { get; set; }
    public string Notes { get; set; }
    public string Img { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public int FrequencyType { get; set; }
    public int Recurrency { get; set; }
    public Guid DoctorId { get; set; }
    public string IndicatedFor { get; set; }
    public List<NewTimeDTO> Times { get; set; }
}
