using AutoMapper;
using SyZero.Example1.Core.Entities;
using SyZero.Example1.IApplication.Examples.Dto;

namespace SyZero.Example1.Application.MapProfile
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
