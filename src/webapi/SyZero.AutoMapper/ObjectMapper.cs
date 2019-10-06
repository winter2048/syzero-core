using System;
using System.Collections.Generic;
using System.Text;
using SyZero.ObjectMapper;
using AutoMapper;

namespace SyZero.AutoMapper
{
    public class ObjectMapper : SyZero.ObjectMapper.IObjectMapper
    {
        private readonly IMapper _mapper;
        public ObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        TDestination SyZero.ObjectMapper.IObjectMapper.Map<TDestination>(object source)
        {
        
            return _mapper.Map<TDestination>(source);
        }

        TDestination SyZero.ObjectMapper.IObjectMapper.Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
