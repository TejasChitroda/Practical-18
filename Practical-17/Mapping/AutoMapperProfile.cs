using AutoMapper;
using Practical_17.Models;

namespace Practical_17.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, StudentViewModel>().ReverseMap();
            // Add more mappings as needed
        }
    }
   
}
