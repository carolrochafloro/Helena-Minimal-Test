namespace Helena_Minimal.DTO;
using AutoMapper;
using Helena_Minimal.Models;

public class Automapping : Profile
{
    public Automapping()
    {
        CreateMap<Times, TimeDTO>().ReverseMap();
        CreateMap<Medication, MedicationWithDoctor>().ReverseMap();
    }
}
