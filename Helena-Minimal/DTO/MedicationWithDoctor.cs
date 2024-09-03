using Helena_Minimal.Models;

namespace Helena_Minimal.DTO;

public class MedicationWithDoctor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lab { get; set; }
    public string Type { get; set; } // comprimido, gotas etc
    public string Dosage { get; set; } // 1cp, 50 gotas, 1 injeção
    public string Notes { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }   // calculado: data de início + duração
    public FrequencyType FrequencyType { get; set; }
    public int Recurrency { get; set; } // qts vezes por dia/mês/ano
    public string DoctorName { get; set; }
    public string DoctorSpecialty { get; set; }
    public string? IndicatedFor { get; set; }

    public List<TimeDTO> Times { get; set; }
}
