using AutoMapper;
using SyZero.Example2.Core.Entities;
using SyZero.Example2.IApplication.Examples.Dto;

namespace SyZero.Example2.Application.MapProfile
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<Example, CreateExampleDto>();
            CreateMap<CreateExampleDto, Example>();
            CreateMap<Example, ExampleDto>();
            CreateMap<ExampleDto, Example>();
        }
    }
}
