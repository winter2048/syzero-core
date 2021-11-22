using System;
using System.Collections.Generic;
using System.Text;

namespace SyZero.Feign
{
    public interface IFeignProxyBuilder
    {
        Object Build(Type interfaceType, params object[] constructor);
        T Build<T>(params object[] constructor);
    }
}
