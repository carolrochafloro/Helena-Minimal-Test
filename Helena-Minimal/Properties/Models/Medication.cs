﻿namespace Helena_Minimal.Properties.Models;

public class Medication
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lab { get; set; }
    public string Type { get; set; } // comprimido, gotas etc
    public string Dosage { get; set; } // 1cp, 50 gotas, 1 injeção
    public string Notes { get; set; }
    public string Img { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }   // calculado: data de início + duração
    public FrequencyType FrequencyType { get; set; }
    public int Recurrency { get; set; } // qts vezes por dia/mês/ano

   
}
public enum FrequencyType
{

    Daily,
    Weekly,
    Monthly,
    Yearly
}