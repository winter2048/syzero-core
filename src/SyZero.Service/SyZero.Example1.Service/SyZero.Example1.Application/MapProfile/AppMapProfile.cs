using AutoMapper;
using SyZero.Example1.Core.Entities;
using SyZero.Example1.IApplication.Examples.Dto;

namespace SyZero.Example1.Application.MapProfile
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<Example, CreateExample1Dto>();
            CreateMap<CreateExample1Dto, Example>();
            CreateMap<Example, Example1Dto>();
            CreateMap<Example1Dto, Example>();
        }
    }
}
