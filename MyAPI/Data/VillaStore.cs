using MyAPI.Models;
using MyAPI.Models.Dto;

namespace MyAPI.Data
{
    public class VillaStore
    {
        public static List<VillaDto> villalist= new List<VillaDto>()
        {
                new VillaDto {Id=1,Name="My Villa 1" , Description="White"},
                new VillaDto { Id = 2, Name = "My villa 2", Description = "Black" }
          
        };
    }
}
