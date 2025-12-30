using AutoMapper;
using SyZero.Example2.Core.Entities;
using SyZero.Example2.IApplication.Examples.Dto;

namespace SyZero.Example2.Application.MapProfile
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<Example, CreateExample2Dto>();
            CreateMap<CreateExample2Dto, Example>();
            CreateMap<Example, Example2Dto>();
            CreateMap<Example2Dto, Example>();
        }
    }
}
